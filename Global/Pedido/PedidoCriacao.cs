using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Pedido.Dados.Criacao;
using Prepedido.Dados.FormaPagto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Produto.RegrasCrtlEstoque;
using Cliente.Dados;
using Cliente;

namespace Pedido
{
    public class PedidoCriacao
    {
        private readonly PedidoBll pedidoBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly Prepedido.FormaPagto.ValidacoesFormaPagtoBll validacoesFormaPagtoBll;
        private readonly Prepedido.PrepedidoBll prepedidoBll;
        private readonly Prepedido.FormaPagto.FormaPagtoBll formaPagtoBll;
        private readonly Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll;
        private readonly EfetivaPedidoBll efetivaPedidoBll;
        private readonly ClienteBll clienteBll;

        public PedidoCriacao(PedidoBll pedidoBll, InfraBanco.ContextoBdProvider contextoProvider,
            Prepedido.FormaPagto.ValidacoesFormaPagtoBll validacoesFormaPagtoBll, Prepedido.PrepedidoBll prepedidoBll,
            Prepedido.FormaPagto.FormaPagtoBll formaPagtoBll, Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll,
            EfetivaPedidoBll efetivaPedidoBll, ClienteBll clienteBll)
        {
            this.pedidoBll = pedidoBll;
            this.contextoProvider = contextoProvider;
            this.validacoesFormaPagtoBll = validacoesFormaPagtoBll;
            this.prepedidoBll = prepedidoBll;
            this.formaPagtoBll = formaPagtoBll;
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
            this.efetivaPedidoBll = efetivaPedidoBll;
            this.clienteBll = clienteBll;
        }
        //criar uma classe: Global/Pedido/PedidoBll/PedidoCriacao com a rotina CadastrarPrepedido, 
        //quer vai retornar um PedidoCriacaoRetorno com o id do pedido, dos filhotes, 
        //as mensagens de erro e as mensagens de erro da validação dos 
        //dados cadastrais (quer dizer, duas listas de erro.) 
        //É que na loja o tratamento dos erros dos dados cadastrais vai ser diferente).
        public async Task<PedidoCriacaoRetornoDados> CadastrarPedido(PedidoCriacaoDados pedido, decimal limiteArredondamento,
            decimal maxErroArredondamento)
        {
            PedidoCriacaoRetornoDados pedidoRetorno = new PedidoCriacaoRetornoDados();
            pedidoRetorno.ListaErros = new List<string>();
            //pedidoRetorno.ListaErros.Add("Ainda não implementado");
            var db = contextoProvider.GetContextoLeitura();
            /* FLUXO DE CRIAÇÃO DE PEDIDO 1ºpasso
             * PedidoNovoConsiste.asp = uma tela antes de finalizar o pedido
             * 1- verificar se a loja esta habilitada para ECommerce
             */


            /* 2- busca os dados do cliente */
            var tcliente = db.Tclientes.Where(r => r.Cnpj_Cpf == pedido.DadosCliente.Cnpj_Cpf).FirstOrDefault();
            DadosClienteCadastroDados clienteCadastro = clienteBll.ObterDadosClienteCadastro(tcliente, pedido.LojaUsuario);

            /* 7- se tiver "vendedor_externo", busca (nome, razão social) na t_LOJA
            * 8- busca o orçamentista para saber se permite RA */
            TorcamentistaEindicador tOrcamentista = await prepedidoBll.BuscarTorcamentista(pedido.DadosCliente.Indicador_Orcamentista);
            if (tOrcamentista == null)
            {
                pedidoRetorno.ListaErros.Add("O Orçamentista não existe!");
                return pedidoRetorno;
            }

            float perc_desagio_RA = 0;
            float perc_limite_RA_sem_desagio = 0;
            decimal vl_limite_mensal = 0;
            decimal vl_limite_mensal_consumido = 0;
            decimal vl_limite_mensal_disponivel = 0;
            float percDescComissaoUtilizar = 0;
            PercentualMaxDescEComissao percentualMax = new PercentualMaxDescEComissao();

            /* 13- valida o tipo de parcelamento "AV", "CE", "SE" */
            /* 14- valida a quantidade de parcela */
            FormaPagtoDados formasPagto = await formaPagtoBll.ObterFormaPagto(tOrcamentista.Apelido, pedido.DadosCliente.Tipo);

            string c_custoFinancFornecTipoParcelamento = prepedidoBll.ObterSiglaFormaPagto(pedido.FormaPagtoCriacao);
            short c_custoFinancFornecQtdeParcelas = (Int16)prepedidoBll.ObterQtdeParcelasFormaPagto(pedido.FormaPagtoCriacao);

            if (UtilsGlobais.Util.ValidarTipoCustoFinanceiroFornecedor(pedidoRetorno.ListaErros, c_custoFinancFornecTipoParcelamento,
                c_custoFinancFornecQtdeParcelas))
            {
                if (validacoesFormaPagtoBll.ValidarFormaPagto(pedido.FormaPagtoCriacao, pedidoRetorno.ListaErros,
                limiteArredondamento, maxErroArredondamento, c_custoFinancFornecTipoParcelamento, formasPagto,
                tOrcamentista.Permite_RA_Status, pedido.Vl_total_NF, pedido.Vl_total))
                {
                    /* 9- valida endereço de entrega */
                    if (await validacoesPrepedidoBll.ValidarEnderecoEntrega(pedido.EnderecoEntrega, pedidoRetorno.ListaErros,
                        pedido.DadosCliente.Indicador_Orcamentista, pedido.DadosCliente.Tipo))
                    {
                        /* 10- valida se o pedido é com ou sem indicação
                         * 11- valida percentual máximo de comissão */
                        if (pedido.ComIndicador)
                        {
                            if (string.IsNullOrEmpty(pedido.NomeIndicador))
                            {
                                pedidoRetorno.ListaErros.Add("Informe quem é o indicador.");
                            }

                            /* 3- busca o percentual máximo de comissão*/
                            percentualMax = await pedidoBll.ObterPercentualMaxDescEComissao(pedido.LojaUsuario);

                            if (pedido.DadosCliente.Tipo == Constantes.ID_PJ)
                                percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                            else
                                percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;

                            if (!string.IsNullOrEmpty(pedido.PercRT.ToString()))
                                pedidoBll.ValidarPercentualRT((float)pedido.PercRT, percentualMax.PercMaxComissao, pedidoRetorno.ListaErros);

                            //perc_desagio_RA
                            perc_desagio_RA = await UtilsGlobais.Util.ObterPercentualDesagioRAIndicador(pedido.NomeIndicador, contextoProvider);
                            perc_limite_RA_sem_desagio = await UtilsGlobais.Util.VerificarSemDesagioRA(contextoProvider);
                            vl_limite_mensal = await UtilsGlobais.Util.ObterLimiteMensalComprasDoIndicador(pedido.NomeIndicador, contextoProvider);
                            vl_limite_mensal_consumido = await UtilsGlobais.Util.CalcularLimiteMensalConsumidoDoIndicador(pedido.NomeIndicador, DateTime.Now, contextoProvider);
                            vl_limite_mensal_disponivel = vl_limite_mensal - vl_limite_mensal_consumido;
                        }
                    }
                }
            }

            //se tiver erro vamos retornar
            if (pedidoRetorno.ListaErros.Count > 0)
            {
                return pedidoRetorno;
            }

            /* 4- busca get_registro_t_parametro(ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto) */
            Tparametro tParametro = await UtilsGlobais.Util.BuscarRegistroParametro(
                Constantes.ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto, contextoProvider);

            percDescComissaoUtilizar = pedidoBll.VerificarPagtoPreferencial(tParametro, pedido, percDescComissaoUtilizar,
                    percentualMax, pedido.Vl_total);


            /* 5- recebe o retorno da busca do item 2 => dados do cliente*/

            /* 6- instancia v_item = recebe os campos do produto (produtos, fabricante, qtde) */
            List<cl_ITEM_PEDIDO_NOVO> v_item = new List<cl_ITEM_PEDIDO_NOVO>();
            short sequencia = 0;
            pedido.ListaProdutos.ForEach(x =>
            {
                sequencia++;
                v_item.Add(new cl_ITEM_PEDIDO_NOVO
                {
                    produto = x.Produto,
                    Fabricante = x.Fabricante,
                    Qtde = (short)x.Qtde,
                    Preco_Venda = x.Preco_Venda,
                    Preco_NF = x.Preco_NF,
                    qtde_estoque_total_disponivel = 0,
                    Qtde_estoque_vendido = 0,
                    Qtde_estoque_sem_presenca = 0,
                    Sequencia = sequencia
                });
            });

            await pedidoBll.VerificarSePedidoExite(v_item, pedido, pedido.DadosCliente.Indicador_Orcamentista, pedidoRetorno.ListaErros);
            //se tiver erro vamos retornar
            if (pedidoRetorno.ListaErros.Count > 0)
            {
                return pedidoRetorno;
            }



            //busca produtos , busca percentual custo finananceiro, calcula desconto 716 ate 824
            //desc_dado_arredondado
            //estamos alterando o v_item com descontos verificados e aplicados
            List<string> vdesconto = new List<string>();
            await pedidoBll.VerificarDescontoArredondado(pedido.LojaUsuario, v_item, pedidoRetorno.ListaErros, c_custoFinancFornecTipoParcelamento,
                c_custoFinancFornecQtdeParcelas, pedido.DadosCliente.Id, percDescComissaoUtilizar, vdesconto);

            /* 15- busca o coeficiente de cada produto do item 6 */
            //faz a lógica, regras para consumo do estoque 947 ate 1297   
            //vou buscar a lista de coeficiente para calcular o valor de custoFinacFornec...
            float coeficiente = await pedidoBll.BuscarCoeficientePercentualCustoFinanFornec(pedido, c_custoFinancFornecQtdeParcelas,
                c_custoFinancFornecTipoParcelamento, pedidoRetorno.ListaErros);


            //Faz a verificação de regra de cada produto
            //RECUPERA OS PRODUTOS QUE O CLIENTE CONCORDOU EM COMPRAR MESMO SEM PRESENÇA NO ESTOQUE.
            //v_spe
            List<cl_CTRL_ESTOQUE_PEDIDO_ITEM_NOVO> v_spe = new List<cl_CTRL_ESTOQUE_PEDIDO_ITEM_NOVO>();
            //essa lista armazena a qtde de empresas que irá atender um produto
            List<int> vEmpresaAutoSplit = new List<int>();


            /* 16- verifica a regra para consumo de estoque
            * 17- valida o cd, se é manual ou se é automático
            * 18- busca a regra de estoque
            * 19- verifica se tem erro na leitura das regras de estoque
            * 20- verifica se as regras associadas aos produtos estão ok
            * 21- se o cd foi selecionado manualmente, verifica se o cd esta habilitado em todas as regras
            * 22- busca a disponibilidade de estoque
            * 23- verifica se o produto tem estoque sufuciente, somando o estoque de todas as empresas candidatas
            * 24- verifica a quantidade de cd que irá atender o pedido (aqui é feita a verificação se será splitado o pedido)
            * 25- verifica a qtde de pedidos que será gerado (split)
            * 26- Faz a contagem de pedido que será cadastrado
            * 27- verifica se tem algum produto descontinuado*/
            foreach (var produto in pedido.ListaProdutos)
            {
                //COMPARAR SE É EXATAMENTE A MESMA REGRA
                ProdutoValidadoComEstoqueDados produto_validado_item = new ProdutoValidadoComEstoqueDados();

                //vamos buscar as regras relacionadas ao produto
                produto_validado_item = await pedidoBll.VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado(produto,
                UtilsGlobais.Util.SoDigitosCpf_Cnpj(pedido.DadosCliente.Cnpj_Cpf), pedido.IdNfeSelecionadoManual);

                if (produto_validado_item.ListaErros.Count > 0)
                {
                    foreach (var erro in produto_validado_item.ListaErros)
                    {
                        if (erro.Contains("PRODUTO SEM PRESENÇA"))
                        {
                            v_spe.Add(new cl_CTRL_ESTOQUE_PEDIDO_ITEM_NOVO
                            {
                                Produto = produto_validado_item.Produto.Produto,
                                Fabricante = UtilsGlobais.Util.Normaliza_Codigo(produto.Fabricante, Constantes.TAM_MIN_FABRICANTE),
                                Qtde_solicitada = (short)produto_validado_item.Produto.QtdeSolicitada,
                                Qtde_estoque = (short)produto_validado_item.Produto.Estoque
                            });
                            pedido.OpcaoVendaSemEstoque = true;
                            produto_validado_item.Produto.Lst_empresa_selecionada.ForEach(x =>
                            {
                                if (!vEmpresaAutoSplit.Contains(x))
                                    vEmpresaAutoSplit.Add(x);
                            });
                            //produto_validado_item.Produto.Lst_empresa_selecionada);
                        }
                    }
                }
                else
                {
                    produto_validado_item.Produto.Lst_empresa_selecionada.ForEach(x =>
                    {
                        if (!vEmpresaAutoSplit.Contains(x))
                            vEmpresaAutoSplit.Add(x);
                    });
                }
            }



            /* 12- busca os dados dos produtos do item 6 */


            /* 28- verifica o percentual de RA permitido 
            * 29- verifica se tem mensagem de alerta para algum produto
            */
            //busca valor de limite para aprovação automática da analise de credito 1317 ate 1325
            string vl_aprov_auto_analise_credito = await pedidoBll.LeParametroControle(
                Constantes.ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO);

            if (!validacoesPrepedidoBll.ValidarDetalhesPrepedido(pedido.DetalhesPedido, pedidoRetorno.ListaErros))
            {
                return pedidoRetorno;
            }

            //obtenção de transportadora que atenda ao cep informado, se houver
            TtransportadoraCep transportadora = pedido.EnderecoEntrega.OutroEndereco == true &&
                !string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_cep) ?
                await pedidoBll.ObterTransportadoraPeloCep(pedido.EnderecoEntrega.EndEtg_cep) :
                await pedidoBll.ObterTransportadoraPeloCep(pedido.DadosCliente.Cep);

            //estou buscando a regra para passar para o metodo 
            //verificar se retorna o esperado
            List<RegrasBll> lstRegras = new List<RegrasBll>();
            foreach (var produto in pedido.ListaProdutos)
            {
                List<RegrasBll> lstRegrast = new List<RegrasBll>();
                lstRegrast = (await pedidoBll.VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado_Teste(
                    produto, pedido.DadosCliente.Cnpj_Cpf, pedido.IdNfeSelecionadoManual)).ToList();
                lstRegras.Add(lstRegrast[0]);
            }

            //cadastra o pedido 1734 ate 2016
            if (pedidoRetorno.ListaErros.Count == 0)
            {
                //vamos efetivar o cadastro do pedido
                //vamos abrir uma nova transaction do contexto que esta sendo utilizado para Using


                //OBS => PedBonshop = pedido Pedido_Bs_X_At
                string PedBonshop = "";
                //AFAZER: CRIAR O EFETIVAR PEDIDO
                using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
                {
                    pedidoRetorno.Id = await efetivaPedidoBll.Novo_EfetivarCadastroPedido(pedido, vEmpresaAutoSplit,
                        pedido.Usuario, c_custoFinancFornecTipoParcelamento, c_custoFinancFornecQtdeParcelas, transportadora,
                        v_item, v_spe, vdesconto, lstRegras, perc_limite_RA_sem_desagio, pedido.LojaUsuario, perc_desagio_RA,
                        tcliente, !string.IsNullOrEmpty(pedido.VendedorExterno), pedido.NomeIndicador, pedidoRetorno.ListaErros, dbgravacao);

                    bool efetivou = !string.IsNullOrWhiteSpace(pedidoRetorno.Id);
                    if (efetivou)
                    {
                        //vamos gravar o Log aqui
                        //monta o log 2691 ate 2881
                        //grava log 
                        //commit
                        await dbgravacao.SaveChangesAsync();
                        dbgravacao.transacao.Commit();
                    }
                }
            }

            /* FLUXO DE CRIAÇÃO DE PEDIDO 2º passo
             * PedidoNovoConfirma.asp
             * 1- verifica se é memorização completa de endereços
             * 2- verifica se pedido é com indicação
             * 3- valida a forma de pagto
             * 4- monta a qtde de parcela com base no tipo da forma de pagamento escolhida
             * 5- valida a qtde de parcela e tipo de parcelamento
             * 6- busca dados do cliente
             * 7- busca percentual máximo de comissão e desconto por loja
             * 8- busca a relação de meios de pagto preferenciais que fazer uso do percentual de comissão e desconto nível 2
             * 9- valida o percetual de comissão
             * 10- busca orçamentista
             * 11- verifica se orçamentista permite RA
             * 12- busca os percentuais de comissão e limites de compra do indicador
             * 13- instancia e v_item para receber os dados dos produtos (produto, fabricante, qtde, preco_venda, permiteRA, preco_nf)
             * 14- verifica se existe esse pedido para não duplicar
             * 15- valida tipo de parcelamento
             * 16- cálcula os valores totais do pedido (vl_total, vl_total_nf, vl_total_ra)
             * 17- verifica o percentual de comissão e desconto
             * 18- verifica qual percentual utilizar conforme o tipo do pagamento escolhido
             * 19- verifica se algum produto está com o preço zerado
             * 20- verifica cada produto
             * 21- recupera os produtos que o cliente concordou em comprar sem estoque
             * 22- faz a lógica para consumo de estoque
             * 23- busca as regras para consumo de estoque
             * 24- verifica se houve erro na leitura das regras de consumo de estoque
             * 25- verifica se as regras associadas estão ok
             * 26- Caso a seleção do CD foi manual, verifica se o CD selecionada está habilitado em todas as regras
             * 27- busca a disponibilidade de estoque
             * 28- verifica se tem produto com estoque insuficiente
             * 29- verifica a qtde de pedidos que serão cadastrados (split)
             * 30- verifica a qtde de pedido necessária
             * 31- faz a contagem de empresas que serão utilizadas no split, ou a qtde de pedidos que será criada
             * 32- busca o valor limite  para aprovação automática da analise de credito
             * 33- busca o percentual da comissão 
             * 34- valida se o pedido é com ou sem indicação
             * 35- valida a entrega imediata
             * 36- valida bem de uso de consumo
             * 37- valida instalador instala
             * 38- verifica se tem endereço de entrega e se o cep do endereço de entrega esta vazio
             * 39- valida a o total da forma de pagto
             * 40- busca a transportadora que atende o cep informado
             * 41- valida endereço de entrega
             * 42- Gera um número temporario de pedido
             * 43- abre a transação para fazer o cadastro do pedido
             * 44- 
             */


            return await Task.FromResult(pedidoRetorno);
        }


    }
}
