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
using Cep;

#nullable enable

namespace Pedido.Criacao
{

    public class PedidoCriacao
    {
        #region construtor
        public readonly PedidoBll PedidoBll;
        public readonly InfraBanco.ContextoBdProvider ContextoProvider;
        public readonly Prepedido.FormaPagto.ValidacoesFormaPagtoBll ValidacoesFormaPagtoBll;
        public readonly Prepedido.PrepedidoBll PrepedidoBll;
        public readonly Prepedido.FormaPagto.FormaPagtoBll FormaPagtoBll;
        public readonly Prepedido.ValidacoesPrepedidoBll ValidacoesPrepedidoBll;
        public readonly EfetivaPedidoBll EfetivaPedidoBll;
        public readonly ClienteBll ClienteBll;
        public readonly CepBll CepBll;
        public readonly IBancoNFeMunicipio BancoNFeMunicipio;
        internal readonly Execucao.Execucao Execucao;
        internal readonly Execucao.Gravacao Gravacao = new Execucao.Gravacao();

        public PedidoCriacao(PedidoBll pedidoBll, InfraBanco.ContextoBdProvider contextoProvider,
            Prepedido.FormaPagto.ValidacoesFormaPagtoBll validacoesFormaPagtoBll, Prepedido.PrepedidoBll prepedidoBll,
            Prepedido.FormaPagto.FormaPagtoBll formaPagtoBll, Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll,
            EfetivaPedidoBll efetivaPedidoBll, ClienteBll clienteBll, CepBll cepBll, IBancoNFeMunicipio bancoNFeMunicipio)
        {
            this.PedidoBll = pedidoBll;
            this.ContextoProvider = contextoProvider;
            this.ValidacoesFormaPagtoBll = validacoesFormaPagtoBll;
            this.PrepedidoBll = prepedidoBll;
            this.FormaPagtoBll = formaPagtoBll;
            this.ValidacoesPrepedidoBll = validacoesPrepedidoBll;
            this.EfetivaPedidoBll = efetivaPedidoBll;
            this.ClienteBll = clienteBll;
            this.CepBll = cepBll;
            this.BancoNFeMunicipio = bancoNFeMunicipio;

            Execucao = new Execucao.Execucao(this);
        }
        #endregion


        //uma classe: Global/Pedido/PedidoBll/PedidoCriacao com a rotina CadastrarPrepedido, 
        //que retorna um PedidoCriacaoRetorno com o id do pedido, dos filhotes, 
        //as mensagens de erro e as mensagens de erro da validação dos 
        //dados cadastrais (quer dizer, duas listas de erro.) 
        //É que na loja o tratamento dos erros dos dados cadastrais vai ser diferente).
        public async Task<PedidoCriacaoRetornoDados> CadastrarPedido(PedidoCriacaoDados pedido)
        {
            PedidoCriacaoRetornoDados retorno = new PedidoCriacaoRetornoDados();

            /*
Fluxo no módulo loja:
1 - Passo10: Escolher cliente já cadastrado
	Se o cliente não existir, ele deve ser cadastrado primeiro. (arquivo CLiente/FLuxoCadastroCliente - criar esse arquivo)
2 - Passo20: Confirmar (ou editar) dados cadastrais e informar endereço de entrega
	se editar dados cadastrais, salva na t_cliente
2.5 - Passo 25: somente na API. Validar dados cadastrais. Não existe na tela porque sempre se usa o atual do cliente.
3 - Passo30: Escolher indicador e RA e Modo de Seleção do CD 
4 - Passo40: Escolher produtos, quantidades e alterar valores e forma de pagamento
5 - Passo50: Informar observações (entrega imediata, instalador instala, etc) 
6 - Passo60: Salvar o pedido
*/

            //setup dados
            await Execucao.ConfigurarExecucaoInicial(pedido);

            //todo: separar em outro passo 05, normalizacao de campos
            pedido.EnderecoCadastralCliente.Endereco_cnpj_cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(pedido.EnderecoCadastralCliente.Endereco_cnpj_cpf);
            pedido.EnderecoCadastralCliente.Endereco_cep = UtilsGlobais.Util.Cep_SoDigito(pedido.EnderecoCadastralCliente.Endereco_cep);
            pedido.EnderecoCadastralCliente.Endereco_tel_res = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoCadastralCliente.Endereco_tel_res);
            pedido.EnderecoCadastralCliente.Endereco_tel_cel = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoCadastralCliente.Endereco_tel_cel);
            pedido.EnderecoCadastralCliente.Endereco_tel_com = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoCadastralCliente.Endereco_tel_com);
            pedido.EnderecoCadastralCliente.Endereco_tel_com_2 = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoCadastralCliente.Endereco_tel_com_2);
            if (pedido.EnderecoEntrega.OutroEndereco)
            {
                pedido.EnderecoEntrega.EndEtg_cnpj_cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(pedido.EnderecoEntrega.EndEtg_cnpj_cpf);
                pedido.EnderecoEntrega.EndEtg_cep = UtilsGlobais.Util.Cep_SoDigito(pedido.EnderecoEntrega.EndEtg_cep);
                pedido.EnderecoEntrega.EndEtg_tel_res = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoEntrega.EndEtg_tel_res);
                pedido.EnderecoEntrega.EndEtg_tel_cel = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoEntrega.EndEtg_tel_cel);
                pedido.EnderecoEntrega.EndEtg_tel_com = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoEntrega.EndEtg_tel_com);
                pedido.EnderecoEntrega.EndEtg_tel_com_2 = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoEntrega.EndEtg_tel_com_2);
            }

            var passo10 = new Criacao.Passo10.Passo10(pedido, retorno, this);
            passo10.Permissoes();
            //se tiver erro de permissao retorna imediatamente
            if (retorno.AlgumErro())
                return retorno;

            //setup adicional
            await Execucao.ConfigurarExecucaoComPermissaoOk(pedido, retorno);
            if (retorno.AlgumErro()) return retorno;

            await passo10.ValidarCliente();
            if (retorno.AlgumErro()) return retorno;

            //somente valida o endereço de entrega. Os dados cadastrais são validados no passo 10 (ou 25)
            //na API magento, por hora, nunca será feito
            await new Passo20.Passo20(pedido, retorno, this).ValidarEnderecoEntregaAsync();

            //passo 25 feito em Criacao.Passo10.Passo10.ValidarCliente

            await new Passo30.Passo30(pedido, retorno, this).ExecutarAsync();
            await new Passo40.Passo40(pedido, retorno, this).ExecutarAsync();
            if (retorno.AlgumErro()) return retorno;
            new Passo50.Passo50(pedido, retorno, this).Executar();

            await new Passo60.Passo60(pedido, retorno, this).ExecutarAsync();

            //se tiver algum erro, limpa os numeros de pedidos gerados
            if (retorno.AlgumErro())
            {
                retorno.LimparPedidos();
            }
            return retorno;

            //return await CadastrarPedido_anterior(pedido, Execucao.UsuarioPermissao);
        }

        private async Task<PedidoCriacaoRetornoDados> CadastrarPedido_anterior(PedidoCriacaoDados pedido,
            UtilsGlobais.Usuario.UsuarioPermissao usuarioPermissao)
        {
            PedidoCriacaoRetornoDados pedidoRetorno = new PedidoCriacaoRetornoDados();

            var db = ContextoProvider.GetContextoLeitura();


            if (pedidoRetorno.ListaErros.Any())
                return pedidoRetorno;



            /* 2- busca os dados do cliente */
            Tcliente tcliente = db.Tclientes.Where(r => r.Cnpj_Cpf == pedido.Cliente.Cnpj_Cpf).FirstOrDefault();
            if (tcliente == null)
            {
                pedidoRetorno.ListaErros.Add($"O cliente não está cadastrado: {pedido.Cliente.Cnpj_Cpf}");
                return pedidoRetorno;
            }

            /* 5- recebe o retorno da busca do item 2 => dados do cliente*/
            DadosClienteCadastroDados clienteCadastro = ClienteBll.ObterDadosClienteCadastro(tcliente, pedido.Ambiente.Loja);

            /* 6- instancia v_item = recebe os campos do produto (produtos, fabricante, qtde) */
            List<Cl_ITEM_PEDIDO_NOVO> v_item = new List<Cl_ITEM_PEDIDO_NOVO>();
            short sequencia = 0;
            foreach (var x in pedido.ListaProdutos)
            {
                sequencia++;
                v_item.Add(new Cl_ITEM_PEDIDO_NOVO(
                    produto: x.Produto,
                    fabricante: x.Fabricante,
                    qtde: x.Qtde
                    )
                {
                    Preco_Venda = x.Preco_Venda,
                    Preco_NF = x.Preco_NF,
                    Qtde_estoque_total_disponivel = 0,
                    Qtde_estoque_vendido = 0,
                    Qtde_estoque_sem_presenca = 0,
                    Sequencia = sequencia
                });
            };

            await new Pedido.PedidoRepetidoBll(ContextoProvider).PedidoJaCadastrado(pedido, pedidoRetorno.ListaErros, tcliente.Id);

            //se tiver erro vamos retornar
            if (pedidoRetorno.ListaErros.Count > 0)
                return pedidoRetorno;

            //busca produtos , busca percentual custo finananceiro, calcula desconto 716 ate 824
            //desc_dado_arredondado
            //estamos alterando o v_item com descontos verificados e aplicados
            List<string> vdesconto = new List<string>();
            await PedidoBll.VerificarDescontoArredondado(this, pedido.Ambiente.Loja, v_item, pedidoRetorno.ListaErros, Execucao.C_custoFinancFornecTipoParcelamento,
                Execucao.C_custoFinancFornecQtdeParcelas, tcliente.Id, Execucao.PercDescComissaoUtilizar, vdesconto);

            /* 15- busca o coeficiente de cada produto do item 6 */
            //vou buscar a lista de coeficiente para calcular o valor de custoFinacFornec...
            float coeficiente = await PedidoBll.BuscarCoeficientePercentualCustoFinanFornec(this, pedido, Execucao.C_custoFinancFornecQtdeParcelas,
                Execucao.C_custoFinancFornecTipoParcelamento, pedidoRetorno.ListaErros);

            //Faz a verificação de regra de cada produto
            //RECUPERA OS PRODUTOS QUE O CLIENTE CONCORDOU EM COMPRAR MESMO SEM PRESENÇA NO ESTOQUE.
            //v_spe
            List<Cl_CTRL_ESTOQUE_PEDIDO_ITEM_NOVO> v_spe = new List<Cl_CTRL_ESTOQUE_PEDIDO_ITEM_NOVO>();
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
            //antes vamos validar o CD 

            if (pedido.Ambiente.Id_nfe_emitente_selecao_manual != 0)
                if (usuarioPermissao.Permitido(Constantes.OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD))
                    pedidoRetorno.ListaErros.Add("Usuário não tem permissão de especificar o CD!");

            //Se tiver erro retorna
            if (pedidoRetorno.ListaErros.Count != 0) return pedidoRetorno;

            foreach (var produto in pedido.ListaProdutos)
            {
                //COMPARAR SE É EXATAMENTE A MESMA REGRA
                ProdutoValidadoComEstoqueDados produto_validado_item;

                //vamos buscar as regras relacionadas ao produto
                produto_validado_item = await PedidoBll.VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado(produto,
                UtilsGlobais.Util.SoDigitosCpf_Cnpj(pedido.Cliente.Cnpj_Cpf), pedido.Ambiente.Id_nfe_emitente_selecao_manual);

                if (produto_validado_item.ListaErros.Count > 0)
                {
                    foreach (var erro in produto_validado_item.ListaErros)
                    {
                        if (erro.Contains("PRODUTO SEM PRESENÇA"))
                        {
                            v_spe.Add(new Cl_CTRL_ESTOQUE_PEDIDO_ITEM_NOVO
                            (
                                produto: produto_validado_item.Produto.Produto,
                                fabricante: UtilsGlobais.Util.Normaliza_Codigo(produto.Fabricante, Constantes.TAM_MIN_FABRICANTE),
                                qtde_solicitada: (short)(produto_validado_item.Produto.QtdeSolicitada ?? 0),
                                qtde_estoque: (short)produto_validado_item.Produto.Estoque,
                                qtde_estoque_vendido: 0,
                                qtde_estoque_sem_presenca: 0,
                                qtde_estoque_global: 0,
                                descricao: "",
                                descricao_html: ""
                            ));
                            foreach (var x in produto_validado_item.Produto.Lst_empresa_selecionada)
                                if (!vEmpresaAutoSplit.Contains(x))
                                    vEmpresaAutoSplit.Add(x);

                        }
                    }
                }
                else
                {
                    foreach (var x in produto_validado_item.Produto.Lst_empresa_selecionada)
                        if (!vEmpresaAutoSplit.Contains(x))
                            vEmpresaAutoSplit.Add(x);

                }
            }

            /* 12- busca os dados dos produtos do item 6 */
            /* 28- verifica o percentual de RA permitido 
            * 29- verifica se tem mensagem de alerta para algum produto
            */
            //busca valor de limite para aprovação automática da analise de credito 1317 ate 1325
            string vl_aprov_auto_analise_credito = await PedidoBll.LeParametroControle(
                Constantes.ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO);

            //obtenção de transportadora que atenda ao cep informado, se houver
            TtransportadoraCep? transportadora = pedido.EnderecoEntrega.OutroEndereco == true &&
                !string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_cep) ?
                await PedidoBll.ObterTransportadoraPeloCep(pedido.EnderecoEntrega.EndEtg_cep) :
                await PedidoBll.ObterTransportadoraPeloCep(pedido.EnderecoCadastralCliente.Endereco_cep);

            //estou buscando a regra para passar para o metodo 
            //verificar se retorna o esperado
            List<RegrasBll> lstRegras = new List<RegrasBll>();
            foreach (var produto in pedido.ListaProdutos)
            {
                var lstRegrast = (await PedidoBll.VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado_Teste(
                    produto, pedido.Cliente.Cnpj_Cpf, pedido.Ambiente.Id_nfe_emitente_selecao_manual));
                if (lstRegrast.regrasBlls.Count > 0)
                    lstRegras.Add(lstRegrast.regrasBlls[0]);
                if (lstRegrast.prodValidadoEstoqueListaErros.Count > 0)
                {
                    pedidoRetorno.ListaErros.Add($"Sem regra de consumo de estoque para o produto: {produto.Fabricante} {produto.Produto}");
                    pedidoRetorno.ListaErros.AddRange(lstRegrast.prodValidadoEstoqueListaErros);
                }
            }

            //cadastra o pedido 1734 ate 2016
            if (pedidoRetorno.ListaErros.Count == 0)
            {
                //vamos efetivar o cadastro do pedido
                //vamos abrir uma nova transaction do contexto que esta sendo utilizado para Using
                using var dbgravacao = ContextoProvider.GetContextoGravacaoParaUsing();
                pedidoRetorno.Id = await EfetivaPedidoBll.EfetivarCadastroPedido(pedido, vEmpresaAutoSplit,
                    pedido.Ambiente.Usuario, Execucao.C_custoFinancFornecTipoParcelamento, Execucao.C_custoFinancFornecQtdeParcelas, transportadora,
                    v_item, v_spe, vdesconto, lstRegras, Execucao.Perc_limite_RA_sem_desagio, pedido.Ambiente.Loja, Execucao.Perc_desagio_RA,
                    tcliente, pedidoRetorno.ListaErros, dbgravacao, pedido.Configuracao.SistemaResponsavelCadastro);

                if (pedidoRetorno.ListaErros.Count() > 0)
                    return pedidoRetorno;

                await dbgravacao.SaveChangesAsync();
                dbgravacao.transacao.Commit();
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
