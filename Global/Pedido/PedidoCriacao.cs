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

        public PedidoCriacao(PedidoBll pedidoBll, InfraBanco.ContextoBdProvider contextoProvider,
            Prepedido.FormaPagto.ValidacoesFormaPagtoBll validacoesFormaPagtoBll, Prepedido.PrepedidoBll prepedidoBll,
            Prepedido.FormaPagto.FormaPagtoBll formaPagtoBll, Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll)
        {
            this.pedidoBll = pedidoBll;
            this.contextoProvider = contextoProvider;
            this.validacoesFormaPagtoBll = validacoesFormaPagtoBll;
            this.prepedidoBll = prepedidoBll;
            this.formaPagtoBll = formaPagtoBll;
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
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
            pedidoRetorno.ListaErros.Add("Ainda não implementado");

            /* FLUXO DE CRIAÇÃO DE PEDIDO 1ºpasso
             * PedidoNovoConsiste.asp = uma tela antes de finalizar o pedido
             * 1- verificar se a loja esta habilitada para ECommerce
             */


            /* 2- busca os dados do cliente */


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
                            /* 3- busca o percentual máximo de comissão*/
                            percentualMax = await pedidoBll.ObterPercentualMaxDescEComissao(pedido.LojaUsuario);
                            
                            if (pedido.DadosCliente.Tipo == Constantes.ID_PJ)
                                percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                            else
                                percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;

                            if (!string.IsNullOrEmpty(pedido.PercRT.ToString()))
                                ValidarPercentualRT((float)pedido.PercRT, percentualMax.PercMaxComissao, pedidoRetorno.ListaErros);

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

            percDescComissaoUtilizar = VerificarPagtoPreferencial(tParametro, pedido, percDescComissaoUtilizar,
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

            await VerificarSePedidoExite(v_item, pedido, pedido.DadosCliente.Indicador_Orcamentista, pedidoRetorno.ListaErros);
            //se tiver erro vamos retornar
            if (pedidoRetorno.ListaErros.Count > 0)
            {
                return pedidoRetorno;
            }



            //busca produtos , busca percentual custo finananceiro, calcula desconto 716 ate 824
            //desc_dado_arredondado
            //estamos alterando o v_item com descontos verificados e aplicados
            await VerificarDescontoArredondado(loja, v_item, lstErros, c_custoFinancFornecTipoParcelamento,
                c_custoFinancFornecQtdeParcelas, pedido.DadosCliente.Id, percDescComissaoUtilizar, vdesconto);

            /* 12- busca os dados dos produtos do item 6 */





            /* 15- busca o coeficiente de cada produto do item 6
            * 16- verifica a regra para consumo de estoque
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
            * 27- verifica se tem algum produto descontinuado
            * 28- verifica o percentual de RA permitido 
            * 29- verifica se tem mensagem de alerta para algum produto
            */

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

        public void ValidarPercentualRT(float percComissao, float percentualMax, List<string> lstErros)
        {
            if (percComissao < 0 || percComissao > 100)
            {
                lstErros.Add("Percentual de comissão inválido.");
            }
            if (percComissao > percentualMax)
            {
                lstErros.Add("O percentual de comissão excede o máximo permitido.");
            }
        }

        public async Task VerificarSePedidoExite(List<cl_ITEM_PEDIDO_NOVO> v_item, PedidoCriacaoDados pedido,
            string usuario, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();

            //verificar se o pedido existe
            string hora_atual = UtilsGlobais.Util.TransformaHora_Minutos();

            List<cl_ITEM_PEDIDO_NOVO> lstProdTask = await (from c in db.TpedidoItems
                                                           where c.Tpedido.Id_Cliente == pedido.DadosCliente.Id &&
                                                                 c.Tpedido.Data == DateTime.Now.Date &&
                                                                 c.Tpedido.Loja == pedido.DadosCliente.Loja &&
                                                                 c.Tpedido.Vendedor == usuario &&
                                                                 c.Tpedido.Data >= DateTime.Now.Date &&
                                                                 c.Tpedido.Hora.CompareTo(hora_atual) <= 0 &&
                                                                 c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO
                                                           orderby c.Pedido, c.Sequencia
                                                           select new cl_ITEM_PEDIDO_NOVO
                                                           {
                                                               Pedido = c.Pedido,
                                                               produto = c.Produto,
                                                               Fabricante = c.Fabricante,
                                                               Qtde = (short)c.Qtde,
                                                               Preco_Venda = c.Preco_Venda
                                                           }).ToListAsync();

            lstProdTask.ForEach(x =>
            {
                v_item.ForEach(y =>
                {
                    if (x.produto == y.produto &&
                        x.Fabricante == y.Fabricante &&
                        x.Qtde == y.Qtde &&
                        x.Preco_Venda == y.Preco_Venda)
                    {
                        lstErros.Add("Este pedido já foi gravado com o número " + x.Pedido);
                        return;
                    }
                });
            });
        }

        private float VerificarPagtoPreferencial(Tparametro tParametro, PedidoCriacaoDados pedido,
            float percDescComissaoUtilizar, PercentualMaxDescEComissao percentualMax, decimal vl_total)
        {
            List<string> lstOpcoesPagtoPrefericiais = new List<string>();
            if (!string.IsNullOrEmpty(tParametro.Id))
            {
                //a verificação é feita na linha 380 ate 388
                lstOpcoesPagtoPrefericiais = tParametro.Campo_texto.Split(',').ToList();
            }

            string s_pg = "";
            decimal? vlNivel1 = 0;
            decimal? vlNivel2 = 0;

            //identifica e verifica se é pagto preferencial e calcula  637 ate 712
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
                s_pg = pedido.FormaPagtoCriacao.Op_av_forma_pagto;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                s_pg = pedido.FormaPagtoCriacao.Op_pu_forma_pagto;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                s_pg = Constantes.ID_FORMA_PAGTO_CARTAO;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                s_pg = Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA;
            if (!string.IsNullOrEmpty(s_pg))
            {
                if (lstOpcoesPagtoPrefericiais.Count > 0)
                {
                    foreach (var op in lstOpcoesPagtoPrefericiais)
                    {
                        if (s_pg == op)
                        {
                            if (pedido.DadosCliente.Tipo == Constantes.ID_PJ)
                                percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                            else
                                percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;
                        }
                    }
                }
            }

            bool pgtoPreferencial = false;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                s_pg = pedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto;

                if (!string.IsNullOrEmpty(s_pg))
                {
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
                    {
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
                    }
                }
                //verificamos a entrada
                if (pgtoPreferencial)
                    vlNivel2 = pedido.FormaPagtoCriacao.C_pce_entrada_valor;
                else
                    vlNivel1 = pedido.FormaPagtoCriacao.C_pce_entrada_valor;

                //Identifica e contabiliza o valor das parcelas
                pgtoPreferencial = false;
                s_pg = pedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto;
                if (!string.IsNullOrEmpty(s_pg))
                {
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
                    {
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
                    }
                }

                if (pgtoPreferencial)
                    vlNivel2 = vlNivel2 +
                        (pedido.FormaPagtoCriacao.C_pce_prestacao_qtde * pedido.FormaPagtoCriacao.C_pce_prestacao_valor);
                else
                    vlNivel1 = vlNivel1 +
                        (pedido.FormaPagtoCriacao.C_pce_prestacao_qtde * pedido.FormaPagtoCriacao.C_pce_prestacao_valor);

                if (vlNivel2 > (vl_total / 2))
                {
                    if (pedido.DadosCliente.Tipo == Constantes.ID_PJ)
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                    else
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;
                }
            }
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                s_pg = pedido.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto;

                if (!string.IsNullOrEmpty(s_pg))
                {
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
                    {
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
                    }
                }
                //verificamos a entrada
                if (pgtoPreferencial)
                    vlNivel2 = pedido.FormaPagtoCriacao.C_pse_prim_prest_valor;
                else
                    vlNivel1 = pedido.FormaPagtoCriacao.C_pse_prim_prest_valor;

                //Identifica e contabiliza o valor das parcelas
                pgtoPreferencial = false;
                s_pg = pedido.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto;
                if (!string.IsNullOrEmpty(s_pg))
                {
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
                    {
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
                    }
                }

                if (pgtoPreferencial)
                    vlNivel2 = vlNivel2 +
                        (pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde * pedido.FormaPagtoCriacao.C_pse_demais_prest_valor);
                else
                    vlNivel1 = vlNivel1 +
                        (pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde * pedido.FormaPagtoCriacao.C_pse_demais_prest_valor);

                if (vlNivel2 > (vl_total / 2))
                {
                    if (pedido.DadosCliente.Tipo == Constantes.ID_PJ)
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                    else
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;
                }
            }
            return percDescComissaoUtilizar;
        }

        private async Task VerificarDescontoArredondado(string loja, List<cl_ITEM_PEDIDO_NOVO> v_item, 
            List<string> lstErros, string c_custoFinancFornecTipoParcelamento, short c_custoFinancFornecQtdeParcelas, 
            string id_cliente, float percDescComissaoUtilizar, List<string> vdesconto)
        {
            var db = contextoProvider.GetContextoLeitura();

            float coeficiente = 0;
            float? desc_dado_arredondado = 0;


            //aqui estão verificando o v_item e não pedido
            //vamos vericar cada produto da lista
            foreach (var item in v_item)
            {
                var produtoLojaTask = (from c in db.TprodutoLojas.Include(x => x.Tproduto).Include(x => x.Tfabricante)
                                       where c.Tproduto.Fabricante == item.Fabricante &&
                                             c.Tproduto.Produto == item.produto &&
                                             c.Loja == loja
                                       select c).FirstOrDefaultAsync();

                if (produtoLojaTask == null)
                    lstErros.Add("Produto " + item.produto + " do fabricante " + item.Fabricante + "NÃO está " +
                        "cadastrado para a loja " + loja);
                else
                {
                    TprodutoLoja produtoLoja = await produtoLojaTask;
                    item.Preco_lista = (decimal)produtoLoja.Preco_Lista;
                    item.Margem = (float)produtoLoja.Margem;
                    item.Desc_max = (float)produtoLoja.Desc_Max;
                    item.Comissao = (float)produtoLoja.Comissao;
                    item.Preco_fabricante = (decimal)produtoLoja.Tproduto.Preco_Fabricante;
                    item.Vl_custo2 = produtoLoja.Tproduto.Vl_Custo2;
                    item.Descricao = produtoLoja.Tproduto.Descricao;
                    item.Descricao_html = produtoLoja.Tproduto.Descricao_Html;
                    item.Ean = produtoLoja.Tproduto.Ean;
                    item.Grupo = produtoLoja.Tproduto.Grupo;
                    item.Peso = (float)produtoLoja.Tproduto.Peso;
                    item.Qtde_volumes = (short)produtoLoja.Tproduto.Qtde_Volumes;
                    item.Markup_fabricante = produtoLoja.Tfabricante.Markup;
                    item.cubagem = produtoLoja.Tproduto.Cubagem;
                    item.Ncm = produtoLoja.Tproduto.Ncm;
                    item.Cst = produtoLoja.Tproduto.Cst;
                    item.Descontinuado = produtoLoja.Tproduto.Descontinuado;

                    if (c_custoFinancFornecTipoParcelamento ==
                            Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
                        coeficiente = 1;
                    else
                    {
                        var coeficienteTask = (from c in db.TpercentualCustoFinanceiroFornecedors
                                               where c.Fabricante == item.Fabricante &&
                                                     c.Tipo_Parcelamento == c_custoFinancFornecTipoParcelamento &&
                                                     c.Qtde_Parcelas == c_custoFinancFornecQtdeParcelas
                                               select c).FirstOrDefaultAsync();
                        if (await coeficienteTask == null)
                            lstErros.Add("Opção de parcelamento não disponível para fornecedor " + item.Fabricante +
                                ": " + DecodificaCustoFinanFornecQtdeParcelas(c_custoFinancFornecTipoParcelamento,
                                c_custoFinancFornecQtdeParcelas) + " parcela(s)");
                        else
                        {
                            coeficiente = (await coeficienteTask).Coeficiente;
                            //voltamos a atribuir ao tpedidoItem
                            item.Preco_lista = Math.Round((decimal)coeficiente * item.Preco_lista, 2);
                        }


                    }

                    item.custoFinancFornecCoeficiente = coeficiente;

                    if (item.Preco_lista == 0)
                    {
                        item.Desc_Dado = 0;
                        desc_dado_arredondado = 0;
                    }
                    else
                    {
                        item.Desc_Dado = (float)(100 *
                            (item.Preco_lista - item.Preco_Venda) / item.Preco_lista);
                        desc_dado_arredondado = item.Desc_Dado;
                    }

                    if (desc_dado_arredondado > percDescComissaoUtilizar)
                    {
                        var tDescontoTask = from c in db.Tdescontos
                                            where c.Usado_status == 0 &&
                                                  c.Id_cliente == id_cliente &&
                                                  c.Fabricante == item.Fabricante &&
                                                  c.Produto == item.produto &&
                                                  c.Loja == loja &&
                                                  c.Data >= DateTime.Now.AddMinutes(-30)
                                            orderby c.Data descending
                                            select c;

                        Tdesconto tdesconto = await tDescontoTask.FirstOrDefaultAsync();

                        if (tdesconto == null)
                        {
                            lstErros.Add("Produto " + item.produto + " do fabricante " + item.Fabricante +
                                ": desconto de " + item.Desc_Dado + "% excede o máximo permitido.");
                        }
                        else
                        {
                            tdesconto = await tDescontoTask.FirstOrDefaultAsync();
                            if ((decimal)item.Desc_Dado >= tdesconto.Desc_max)
                                lstErros.Add("Produto " + item.produto + " do fabricante " + item.Fabricante +
                                    ": desconto de " + item.Desc_Dado + " % excede o máximo autorizado.");
                            else
                            {
                                item.Abaixo_min_status = 1;
                                item.abaixo_min_autorizacao = tdesconto.Id;
                                item.Abaixo_min_autorizador = tdesconto.Autorizador;
                                item.Abaixo_min_superv_autorizador = tdesconto.Supervisor_autorizador;

                                //essa variavel aparentemente apenas sinaliza 
                                //se existe uma senha de autorização para desconto superior
                                if (vdesconto.Count > 0)
                                {
                                    vdesconto.Add(tdesconto.Id);
                                }
                            }
                        }
                    }
                }
            }
        }

        private string DecodificaCustoFinanFornecQtdeParcelas(string tipoParcelamento, short custoFFQtdeParcelas)
        {
            string retorno = "";

            if (tipoParcelamento == Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
                retorno = "0+" + custoFFQtdeParcelas;
            else if (tipoParcelamento == Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA)
                retorno = "1+" + custoFFQtdeParcelas;

            return retorno;
        }
    }
}
