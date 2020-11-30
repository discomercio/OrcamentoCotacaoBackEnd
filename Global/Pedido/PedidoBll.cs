using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using InfraBanco.Modelos;
using InfraBanco.Constantes;
using Produto.RegrasCrtlEstoque;
using UtilsGlobais;
using Produto;
using InfraBanco;

#nullable enable

namespace Pedido
{
    public class PedidoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public PedidoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        // MÉTODOS NOVOS SENDO MOVIDO
        public async Task<PercentualMaxDescEComissao> ObterPercentualMaxDescEComissao(string loja)
        {
            var db = contextoProvider.GetContextoLeitura();

            var ret = from c in db.Tlojas
                      where c.Loja == loja
                      select new PercentualMaxDescEComissao
                      {
                          PercMaxComissao = c.Perc_Max_Comissao,
                          PercMaxComissaoEDesc = c.Perc_Max_Comissao_E_Desconto,
                          PercMaxComissaoEDescNivel2 = c.Perc_Max_Comissao_E_Desconto_Nivel2,
                          PercMaxComissaoEDescPJ = c.Perc_Max_Comissao_E_Desconto_Pj,
                          PercMaxComissaoEDescNivel2PJ = c.Perc_Max_Comissao_E_Desconto_Nivel2_Pj
                      };

            return await ret.FirstOrDefaultAsync();
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

        public async Task VerificarSePedidoExite(List<Cl_ITEM_PEDIDO_NOVO> v_item, PedidoCriacaoDados pedido,
            string usuario, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();

            //verificar se o pedido existe
            string hora_atual = UtilsGlobais.Util.TransformaHora_Minutos();

            var lstProdTask = await (from c in db.TpedidoItems
                                     where c.Tpedido.Id_Cliente == pedido.DadosCliente.Id &&
                                           c.Tpedido.Data == DateTime.Now.Date &&
                                           c.Tpedido.Loja == pedido.DadosCliente.Loja &&
                                           c.Tpedido.Vendedor == usuario &&
                                           c.Tpedido.Data >= DateTime.Now.Date &&
                                           c.Tpedido.Hora.CompareTo(hora_atual) <= 0 &&
                                           c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO
                                     orderby c.Pedido, c.Sequencia
                                     select new
                                     {
                                         c.Pedido,
                                         c.Produto,
                                         c.Fabricante,
                                         Qtde = c.Qtde ?? 0,
                                         c.Preco_Venda
                                     }).ToListAsync();

            foreach (var x in lstProdTask)
            {
                foreach (var y in v_item)
                {
                    if (x.Produto == y.Produto &&
                        x.Fabricante == y.Fabricante &&
                        x.Qtde == y.Qtde &&
                        x.Preco_Venda == y.Preco_Venda)
                    {
                        lstErros.Add("Este pedido já foi gravado com o número " + x.Pedido);
                        return;
                    }
                };
            };
        }

        public float VerificarPagtoPreferencial(Tparametro tParametro, PedidoCriacaoDados pedido,
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

        public async Task VerificarDescontoArredondado(string loja, List<Cl_ITEM_PEDIDO_NOVO> v_item,
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
                                             c.Tproduto.Produto == item.Produto &&
                                             c.Loja == loja
                                       select c).FirstOrDefaultAsync();

                if (produtoLojaTask == null)
                    lstErros.Add("Produto " + item.Produto + " do fabricante " + item.Fabricante + "NÃO está " +
                        "cadastrado para a loja " + loja);
                else
                {
                    TprodutoLoja produtoLoja = await produtoLojaTask;
                    item.Preco_lista = produtoLoja.Preco_Lista ?? 0;
                    item.Margem = produtoLoja.Margem ?? 0;
                    item.Desc_max = produtoLoja.Desc_Max ?? 0;
                    item.Comissao = produtoLoja.Comissao ?? 0;
                    item.Preco_fabricante = produtoLoja.Tproduto.Preco_Fabricante ?? 0;
                    item.Vl_custo2 = produtoLoja.Tproduto.Vl_Custo2;
                    item.Descricao = produtoLoja.Tproduto.Descricao;
                    item.Descricao_html = produtoLoja.Tproduto.Descricao_Html;
                    item.Ean = produtoLoja.Tproduto.Ean;
                    item.Grupo = produtoLoja.Tproduto.Grupo;
                    item.Peso = produtoLoja.Tproduto.Peso;
                    item.Qtde_volumes = produtoLoja.Tproduto.Qtde_Volumes ?? 0;
                    item.Markup_fabricante = produtoLoja.Tfabricante.Markup;
                    item.Cubagem = produtoLoja.Tproduto.Cubagem;
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

                    item.CustoFinancFornecCoeficiente = coeficiente;

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
                                                  c.Produto == item.Produto &&
                                                  c.Loja == loja &&
                                                  c.Data >= DateTime.Now.AddMinutes(-30)
                                            orderby c.Data descending
                                            select c;

                        Tdesconto tdesconto = await tDescontoTask.FirstOrDefaultAsync();

                        if (tdesconto == null)
                        {
                            lstErros.Add("Produto " + item.Produto + " do fabricante " + item.Fabricante +
                                ": desconto de " + item.Desc_Dado + "% excede o máximo permitido.");
                        }
                        else
                        {
                            tdesconto = await tDescontoTask.FirstOrDefaultAsync();
                            if ((decimal)item.Desc_Dado >= tdesconto.Desc_max)
                                lstErros.Add("Produto " + item.Produto + " do fabricante " + item.Fabricante +
                                    ": desconto de " + item.Desc_Dado + " % excede o máximo autorizado.");
                            else
                            {
                                item.Abaixo_min_status = 1;
                                item.Abaixo_min_autorizacao = tdesconto.Id;
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

            if (tipoParcelamento == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
                retorno = "0+" + custoFFQtdeParcelas;
            else if (tipoParcelamento == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA)
                retorno = "1+" + custoFFQtdeParcelas;

            return retorno;
        }

        public async Task<float> BuscarCoeficientePercentualCustoFinanFornec(PedidoCriacaoDados pedido, short qtdeParcelas, string siglaPagto, List<string> lstErros)
        {
            float coeficiente = 0;

            var db = contextoProvider.GetContextoLeitura();

            if (siglaPagto == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
                coeficiente = 1;
            else
            {
                foreach (var i in pedido.ListaProdutos)
                {
                    var percCustoTask = from c in db.TpercentualCustoFinanceiroFornecedors
                                        where c.Fabricante == i.Fabricante &&
                                              c.Tipo_Parcelamento == siglaPagto &&
                                              c.Qtde_Parcelas == qtdeParcelas
                                        select c;

                    var percCusto = await percCustoTask.FirstOrDefaultAsync();

                    if (percCusto != null)
                    {
                        coeficiente = percCusto.Coeficiente;
                        i.Preco_Lista = (decimal)coeficiente * (i.CustoFinancFornecPrecoListaBase);
                    }
                    else
                    {
                        lstErros.Add("Opção de parcelamento não disponível para fornecedor " + i.Fabricante + ": " +
                            DecodificaCustoFinanFornecQtdeParcelas(pedido.FormaPagtoCriacao.C_forma_pagto, qtdeParcelas) + " parcela(s)");
                    }

                }
            }

            return coeficiente;
        }

        public async Task<ProdutoValidadoComEstoqueDados> VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado(PedidoProdutoPedidoDados produto,
            string cpf_cnpj, int id_nfe_emitente_selecao_manual)
        {
            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = from c in db.Tclientes
                              where c.Cnpj_Cpf == cpf_cnpj
                              select c;

            Tcliente cliente = await clienteTask.FirstOrDefaultAsync();

            var prodValidadoEstoqueListaErros = new List<string>();

            //obtém  a sigla para regra
            string cliente_regra = Produto.UtilsProduto.MultiCdRegraDeterminaPessoa(cliente.Tipo, cliente.Contribuinte_Icms_Status,
                cliente.Produtor_Rural_Status);

            //buscar o produto
            //PedidoProdutosDtoPedido produto = new PedidoProdutosDtoPedido();

            List<RegrasBll> regraCrtlEstoque = (await ObterCtrlEstoqueProdutoRegraParaUMProduto(produto, cliente,
                prodValidadoEstoqueListaErros)).ToList();

            //afazer: verificar se há necessidade de continuar com esse método, pois acima faz a mesma coisa com validação
            await UtilsProduto.ObterCtrlEstoqueProdutoRegra_Teste(prodValidadoEstoqueListaErros, regraCrtlEstoque, cliente.Uf, cliente_regra, contextoProvider);

            VerificarRegrasAssociadasParaUMProduto(regraCrtlEstoque, prodValidadoEstoqueListaErros, cliente, id_nfe_emitente_selecao_manual);

            if (id_nfe_emitente_selecao_manual != 0)
                await VerificarCDHabilitadoTodasRegras(regraCrtlEstoque, id_nfe_emitente_selecao_manual, prodValidadoEstoqueListaErros);

            await ObterDisponibilidadeEstoque(regraCrtlEstoque, produto, prodValidadoEstoqueListaErros, id_nfe_emitente_selecao_manual);

            //meto responsavel por atribuir a qtde de estoque ao produto
            //await Util.Util.VerificarEstoque(regraCrtlEstoque, produto, id_nfe_emitente_selecao_manual, contextoProvider);

            bool estoqueInsuficiente = VerificarEstoqueInsuficienteUMProduto(regraCrtlEstoque, produto,
                id_nfe_emitente_selecao_manual, prodValidadoEstoqueListaErros);

            VerificarQtdePedidosAutoSplit(regraCrtlEstoque, prodValidadoEstoqueListaErros, produto, id_nfe_emitente_selecao_manual);

            List<int> lst_empresa_selecionada = ContagemEmpresasUsadasAutoSplit(regraCrtlEstoque, id_nfe_emitente_selecao_manual);

            await ExisteProdutoDescontinuado(produto, prodValidadoEstoqueListaErros);

            var prodValidadoEstoqueProduto = new ProdutoPedidoDados();
            prodValidadoEstoqueProduto.Produto = produto.Produto;
            prodValidadoEstoqueProduto.Fabricante = produto.Fabricante;
            prodValidadoEstoqueProduto.Estoque = produto.Qtde_estoque_total_disponivel ?? 0;
            prodValidadoEstoqueProduto.QtdeSolicitada = produto.Qtde;
            prodValidadoEstoqueProduto.Preco_lista = produto.Preco_Lista;
            prodValidadoEstoqueProduto.Descricao_html = produto.Descricao;
            prodValidadoEstoqueProduto.Lst_empresa_selecionada = lst_empresa_selecionada;
            ProdutoValidadoComEstoqueDados prodValidadoEstoque = new ProdutoValidadoComEstoqueDados(prodValidadoEstoqueProduto,
                prodValidadoEstoqueListaErros);

            return prodValidadoEstoque;
        }

        //todo: afazer: tentar unificar com Prepedido.PrepedidoBll.ObterCtrlEstoqueProdutoRegra
        public async Task<IEnumerable<RegrasBll>> ObterCtrlEstoqueProdutoRegraParaUMProduto(PedidoProdutoPedidoDados produto,
            Tcliente tcliente, List<string> lstErros)
        {
            List<RegrasBll> lstRegrasCrtlEstoque = new List<RegrasBll>();

            var db = contextoProvider.GetContextoLeitura();

            //vamos verificar passando todos os produtos simples da lista de produto que irá para ser selecionado

            var regraProdutoTask = from c in db.TprodutoXwmsRegraCds
                                   where c.Fabricante == produto.Fabricante &&
                                         c.Produto == produto.Produto
                                   select c;

            var regra = await regraProdutoTask.FirstOrDefaultAsync();

            if (regra == null)
            {
                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                    Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': produto (" + produto.Fabricante + ")" +
                    produto.Produto + " não possui regra associada");
            }
            else
            {
                if (regra.Id_wms_regra_cd == 0)
                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                        Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': produto (" + produto.Fabricante + ")" +
                        produto.Produto + " não está associado a nenhuma regra");
                else
                {
                    var wmsRegraTask = from c in db.TwmsRegraCds
                                       where c.Id == regra.Id_wms_regra_cd
                                       select c;

                    var wmsRegra = await wmsRegraTask.FirstOrDefaultAsync();
                    if (wmsRegra == null)
                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                            Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': regra associada ao produto (" +
                            produto.Fabricante + ")" +
                            produto.Produto + " não foi localizada no banco de dados (Id=" + regra.Id_wms_regra_cd + ")");
                    else
                    {
                        RegrasBll itemRegra = new RegrasBll();
                        itemRegra.Fabricante = produto.Fabricante;
                        itemRegra.Produto = produto.Produto;

                        itemRegra.TwmsRegraCd = new t_WMS_REGRA_CD
                        {
                            Id = wmsRegra.Id,
                            Apelido = wmsRegra.Apelido,
                            Descricao = wmsRegra.Descricao,
                            St_inativo = wmsRegra.St_inativo
                        };

                        var wmsRegraCdXUfTask = from c in db.TwmsRegraCdXUfs
                                                where c.Id_wms_regra_cd == itemRegra.TwmsRegraCd.Id &&
                                                      c.Uf == tcliente.Uf
                                                select c;
                        var wmsRegraCdXUf = await wmsRegraCdXUfTask.FirstOrDefaultAsync();

                        if (wmsRegraCdXUf == null)
                        {
                            itemRegra.St_Regra = false;
                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                                Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': regra associada ao produto (" +
                                produto.Fabricante + ")" +
                                produto.Produto + " não está cadastrada para a UF '" + tcliente.Uf + "' (Id=" +
                                regra.Id_wms_regra_cd + ")");
                        }
                        else
                        {
                            itemRegra.TwmsRegraCdXUf = new t_WMS_REGRA_CD_X_UF
                            {
                                Id = wmsRegraCdXUf.Id,
                                Id_wms_regra_cd = wmsRegraCdXUf.Id_wms_regra_cd,
                                Uf = wmsRegraCdXUf.Uf,
                                St_inativo = wmsRegraCdXUf.St_inativo
                            };

                            //buscar a sigla tipo pessoa
                            var tipo_pessoa = UtilsProduto.MultiCdRegraDeterminaPessoa(tcliente.Tipo,
                                tcliente.Contribuinte_Icms_Status, tcliente.Produtor_Rural_Status);

                            var wmsRegraCdXUfXPessoaTask = from c in db.TwmsRegraCdXUfPessoas
                                                           where c.Id_wms_regra_cd_x_uf == itemRegra.TwmsRegraCdXUf.Id &&
                                                                 c.Tipo_pessoa == tipo_pessoa
                                                           select c;

                            var wmsRegraCdXUfXPessoa = await wmsRegraCdXUfXPessoaTask.FirstOrDefaultAsync();

                            if (wmsRegraCdXUfXPessoa == null)
                            {
                                itemRegra.St_Regra = false;
                                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                    tcliente.Uf + "' e '" +
                                    Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                    "': regra associada ao produto (" + produto.Fabricante + ")" +
                                    produto.Produto + " não está cadastrada para a UF '" + tcliente.Uf +
                                    "' (Id=" + regra.Id_wms_regra_cd + ")");
                            }
                            else
                            {
                                itemRegra.TwmsRegraCdXUfXPessoa = new t_WMS_REGRA_CD_X_UF_X_PESSOA
                                {
                                    Id = wmsRegraCdXUfXPessoa.Id,
                                    Id_wms_regra_cd_x_uf = wmsRegraCdXUfXPessoa.Id_wms_regra_cd_x_uf,
                                    Tipo_pessoa = wmsRegraCdXUfXPessoa.Tipo_pessoa,
                                    St_inativo = wmsRegraCdXUfXPessoa.St_inativo,
                                    Spe_id_nfe_emitente = wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                };

                                if (wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                                {
                                    itemRegra.St_Regra = false;
                                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                        tcliente.Uf + "' e '" +
                                        Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                        "': regra associada ao produto (" + produto.Fabricante + ")" +
                                        produto.Produto +
                                        " não especifica nenhum CD para aguardar produtos sem presença no estoque (Id="
                                        + regra.Id_wms_regra_cd + ")");
                                }
                                else
                                {
                                    var nfEmitenteTask = from c in db.TnfEmitentes
                                                         where c.Id == wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                                         select c;
                                    var nfEmitente = await nfEmitenteTask.FirstOrDefaultAsync();

                                    if (nfEmitente != null)
                                    {
                                        if (nfEmitente.St_Ativo != 1)
                                        {
                                            itemRegra.St_Regra = false;
                                            lstErros.Add("Falha na regra de consumo do estoque para a UF '" +
                                                tcliente.Uf + "' e '" +
                                                Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                                "': regra associada ao produto (" + produto.Fabricante +
                                                ")" + produto.Produto +
                                                " especifica um CD para aguardar produtos sem presença no estoque que não está habilitado " +
                                                "(Id=" + regra.Id_wms_regra_cd + ")");
                                        }
                                    }
                                    var wmsRegraCdXUfXPessoaXcdTask = from c in db.TwmsRegraCdXUfXPessoaXCds
                                                                      where c.Id_wms_regra_cd_x_uf_x_pessoa == wmsRegraCdXUfXPessoa.Id
                                                                      orderby c.Ordem_prioridade
                                                                      select c;
                                    var wmsRegraCdXUfXPessoaXcd = await wmsRegraCdXUfXPessoaXcdTask.ToListAsync();

                                    if (wmsRegraCdXUfXPessoaXcd.Count == 0)
                                    {
                                        itemRegra.St_Regra = false;
                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                            tcliente.Uf + "' e '" +
                                            Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                            "': regra associada ao produto (" + produto.Fabricante + ")" +
                                            produto.Produto +
                                            " não especifica nenhum CD para consumo do estoque (Id=" +
                                            regra.Id_wms_regra_cd + ")");
                                    }
                                    else
                                    {
                                        itemRegra.TwmsCdXUfXPessoaXCd = new List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD>();
                                        foreach (var i in wmsRegraCdXUfXPessoaXcd)
                                        {
                                            t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
                                            {
                                                Id = i.Id,
                                                Id_wms_regra_cd_x_uf_x_pessoa = i.Id_wms_regra_cd_x_uf_x_pessoa,
                                                Id_nfe_emitente = i.Id_nfe_emitente,
                                                Ordem_prioridade = i.Ordem_prioridade,
                                                St_inativo = i.St_inativo
                                            };

                                            var nfeCadastroPrincipalTask = from c in db.TnfEmitentes
                                                                           where c.Id == item_cd_uf_pess_cd.Id_nfe_emitente
                                                                           select c;

                                            var nfeCadastroPrincipal = await nfeCadastroPrincipalTask.FirstOrDefaultAsync();

                                            if (nfeCadastroPrincipal != null)
                                            {
                                                if (nfeCadastroPrincipal.St_Ativo != 1)
                                                    item_cd_uf_pess_cd.St_inativo = 1;
                                            }

                                            itemRegra.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
                                        }
                                        foreach (var i in itemRegra.TwmsCdXUfXPessoaXCd)
                                        {

                                            if (i.Id_nfe_emitente == itemRegra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                            {
                                                if (i.St_inativo == 1)
                                                    lstErros.Add(
                                                        "Falha na leitura da regra de consumo do estoque para a UF '" +
                                                        tcliente.Uf + "' e '" +
                                                        Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                                        "': regra associada ao produto (" +
                                                        produto.Fabricante + ")" + produto.Produto +
                                                        " especifica o CD '" + Util.ObterApelidoEmpresaNfeEmitentes(wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente,
                                                        contextoProvider) +
                                                        "' para alocação de produtos sem presença no estoque, sendo que este CD está desativado para " +
                                                        "processar produtos disponíveis (Id=" + regra.Id_wms_regra_cd + ")");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        lstRegrasCrtlEstoque.Add(itemRegra);
                    }
                }
            }
            return lstRegrasCrtlEstoque;
        }

        public static void VerificarRegrasAssociadasParaUMProduto(List<RegrasBll> lst, List<string> lstErros,
            Tcliente cliente, int id_nfe_emitente_selecao_manual)
        {

            /*id_nfe_emitente_selecao_manual = 0;*///esse é a seleção do checkebox 

            foreach (var i in lst)
            {
                if (!string.IsNullOrEmpty(i.Produto))
                {
                    if (i.TwmsRegraCd.Id == 0)
                    {
                        lstErros.Add("Produto (" + i.Fabricante + ")" + i.Produto +
                            " não possui regra de consumo do estoque associada");
                    }
                    else if (i.St_Regra == false)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                            "' associada ao produto (" + i.Fabricante + ")" + i.Produto + " está desativada");
                    }
                    else if (i.TwmsRegraCdXUf.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                            "' associada ao produto (" + i.Fabricante + ")" + i.Produto + " está bloqueada para a UF '" +
                            cliente.Uf + "'");
                    }
                    else if (i.TwmsRegraCdXUfXPessoa.St_inativo == 1)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            i.Fabricante + ")" + i.Produto + " está bloqueada para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                    else if (i.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                            "' associada ao produto (" + i.Fabricante + ")" + i.Produto +
                            " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                    else
                    {
                        int qtdeCdAtivo = 0;
                        foreach (var t in i.TwmsCdXUfXPessoaXCd)
                        {
                            if (t.Id_nfe_emitente > 0)
                            {
                                if (t.St_inativo == 0)
                                {
                                    qtdeCdAtivo = qtdeCdAtivo + 1;
                                }
                            }
                        }

                        if (qtdeCdAtivo == 0 && id_nfe_emitente_selecao_manual == 0)
                        {
                            lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                                "' associada ao produto (" + i.Fabricante + ")" + i.Produto +
                                " não especifica nenhum CD ativo para clientes '" + cliente.Tipo +
                                "' da UF '" + cliente.Uf + "'");


                        }
                    }
                }
            }
        }

        public async Task VerificarCDHabilitadoTodasRegras(List<RegrasBll> lstRegras,
            int id_nfe_emitente_selecao_manual, List<string> lstErros)
        {
            //id_nfe_emitente_selecao_manual = 0;//esse é a seleção do checkebox 
            bool desativado = false;
            bool achou = false;
            List<string> lstErrosAux = new List<string>();

            foreach (var i in lstRegras)
            {
                achou = false;
                desativado = false;
                if (i.Produto != "")
                {
                    foreach (var t in i.TwmsCdXUfXPessoaXCd)
                    {
                        if (t.Id_nfe_emitente == id_nfe_emitente_selecao_manual)
                        {
                            achou = true;
                            if (t.St_inativo == 1)
                            {
                                desativado = true;
                            }
                        }
                    }
                }
                if (!achou)
                {
                    lstErrosAux.Add("Produto (" + i.Fabricante + ")" + i.Produto + ": regra '"
                        + i.TwmsRegraCd.Apelido + "' (Id=" + i.TwmsRegraCd.Id + ") não permite o CD '" +
                        await UtilsGlobais.Util.ObterApelidoEmpresaNfeEmitentes(id_nfe_emitente_selecao_manual, contextoProvider));
                }
                else if (desativado)
                {
                    lstErrosAux.Add("Regra '" + i.TwmsRegraCd.Apelido + "'(Id = " + i.TwmsRegraCd.Id + ") define o CD '" +
                        await Util.ObterApelidoEmpresaNfeEmitentes(id_nfe_emitente_selecao_manual, contextoProvider) +
                        "' como 'desativado'");
                }

            }

            if (lstErrosAux.Count > 0)
            {
                //não iremos utilizar essa msg, mas deixaremos aqui caso necessite
                //string erro = "O CD selecionado manualmente não pode ser usado devido aos seguintes motivos:";
                foreach (var e in lstErrosAux)
                {
                    lstErros.Add(e);
                }

            }
        }

        public async Task ObterDisponibilidadeEstoque(List<RegrasBll> lstRegrasCrtlEstoque, PedidoProdutoPedidoDados produto,
            List<string> lstErros, int id_nfe_emitente_selecao_manual)
        {
            //int id_nfe_emitente_selecao_manual = 0;

            foreach (var r in lstRegrasCrtlEstoque)
            {
                if (!string.IsNullOrEmpty(r.Produto))
                {
                    if (r.TwmsRegraCd != null)
                    {
                        foreach (var p in r.TwmsCdXUfXPessoaXCd)
                        {
                            if (p.Id_nfe_emitente > 0 &&
                                (id_nfe_emitente_selecao_manual == 0 || p.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                            {
                                if (p.St_inativo == 0 || id_nfe_emitente_selecao_manual != 0)
                                {
                                    if (r.Fabricante == produto.Fabricante && r.Produto == produto.Produto)
                                    {
                                        p.Estoque_Fabricante = produto.Fabricante;
                                        p.Estoque_Produto = produto.Produto;
                                        p.Estoque_DescricaoHtml = produto.Descricao;
                                        p.Estoque_Qtde_Solicitado = produto.Qtde;//essa variavel não deve ser utilizada, a qtde só sera solicitada 
                                        //quando o usuario inserir a qtde 
                                        p.Estoque_Qtde = 0;
                                        if (!await EstoqueVerificaDisponibilidadeIntegralV2(p, produto))
                                        {
                                            lstErros.Add("Falha ao tentar consultar disponibilidade no estoque do produto (" +
                                                r.Fabricante + ")" + r.Produto);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task<bool> EstoqueVerificaDisponibilidadeIntegralV2(t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD regra,
            PedidoProdutoPedidoDados produto)
        {
            bool retorno = false;
            if (regra.Estoque_Qtde_Solicitado > 0 && regra.Estoque_Produto != "")
            {
                var retornaRegra = await BuscarListaQtdeEstoque(regra);
                produto.Qtde_estoque_total_disponivel = retornaRegra.Estoque_Qtde_Estoque_Global;
                retorno = true;
            }
            return retorno;
        }

        private async Task<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD> BuscarListaQtdeEstoque(t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD regra)
        {
            var db = contextoProvider.GetContextoLeitura();
            int qtde = 0;
            int qtdeUtilizada = 0;
            int saldo = 0;

            if (regra.Estoque_Qtde_Solicitado > 0 && !string.IsNullOrEmpty(regra.Estoque_Produto))
            {
                var estoqueCDTask = (from c in db.TestoqueItems.Include(r => r.Testoque)
                                     where c.Testoque.Id_nfe_emitente == regra.Id_nfe_emitente &&
                                           c.Fabricante == regra.Estoque_Fabricante &&
                                           c.Produto == regra.Estoque_Produto &&
                                           (c.Qtde - c.Qtde_utilizada) > 0
                                     select new
                                     {
                                         qtde = c.Qtde ?? 0,
                                         qtdeUtilizada = c.Qtde_utilizada ?? 0
                                     });
                if (estoqueCDTask != null)
                {
                    qtde = await estoqueCDTask.SumAsync(x => x.qtde);
                    qtdeUtilizada = await estoqueCDTask.SumAsync(x => x.qtdeUtilizada);
                    saldo = qtde - qtdeUtilizada;
                    regra.Estoque_Qtde = (short)(qtde - qtdeUtilizada);


                    var estoqueGlobalTask = (from c in db.TestoqueItems.Include(r => r.Testoque)
                                             where c.Fabricante == regra.Estoque_Fabricante &&
                                                   c.Produto == regra.Estoque_Produto &&
                                                   (c.Qtde - c.Qtde_utilizada) > 0 &&
                                                   (c.Testoque.Id_nfe_emitente == regra.Id_nfe_emitente ||
                                                    db.TnfEmitentes.Where(r => r.St_Habilitado_Ctrl_Estoque == 1 && r.St_Ativo == 1)
                                                    .Select(r => r.Id).Contains(c.Testoque.Id_nfe_emitente))
                                             select new
                                             {
                                                 qtde = c.Qtde ?? 0,
                                                 qtdeUtilizada = c.Qtde_utilizada ?? 0
                                             });
                    qtde = await estoqueGlobalTask.SumAsync(x => x.qtde);
                    qtdeUtilizada = await estoqueGlobalTask.SumAsync(x => x.qtdeUtilizada);
                    saldo = qtde - qtdeUtilizada;
                    regra.Estoque_Qtde_Estoque_Global = (short)(qtde - qtdeUtilizada);
                }


            }

            return regra;
        }

        private bool VerificarEstoqueInsuficienteUMProduto(List<RegrasBll> lstRegras, PedidoProdutoPedidoDados produto,
            int id_nfe_emitente_selecao_manual, List<string> lstErros)
        {
            bool retorno = false;
            int qtde_estoque_total_disponivel = 0;

            if (!string.IsNullOrEmpty(produto.Produto))
            {
                foreach (var regra in lstRegras)
                {
                    if (!string.IsNullOrEmpty(regra.Produto))
                    {
                        foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                        {
                            if (r.Id_nfe_emitente > 0 &&
                                (id_nfe_emitente_selecao_manual == 0 || r.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                            {
                                if (r.St_inativo == 0 || id_nfe_emitente_selecao_manual != 0)
                                {
                                    if (regra.Fabricante == produto.Fabricante && regra.Produto == produto.Produto)
                                    {
                                        qtde_estoque_total_disponivel += (int)r.Estoque_Qtde;
                                    }
                                }
                            }
                        }
                    }
                }


                if (qtde_estoque_total_disponivel == 0)
                {
                    produto.Qtde_estoque_total_disponivel = 0;
                    lstErros.Add("PRODUTO SEM PRESENÇA NO ESTOQUE");
                    retorno = true;
                }
                else
                {
                    produto.Qtde_estoque_total_disponivel = (short?)qtde_estoque_total_disponivel;
                }
            }

            return retorno;

        }

        public List<RegrasBll> VerificarQtdePedidosAutoSplit(List<RegrasBll> lstRegras, List<string> lstErros,
            PedidoProdutoPedidoDados produto, int id_nfe_emitente_selecao_manual)
        {
            int qtde_a_alocar = 0;

            List<RegrasBll> lstRegras_apoio = lstRegras;
            lstRegras = new List<RegrasBll>();

            if (!string.IsNullOrEmpty(produto.Produto))
            {
                qtde_a_alocar = (int)produto.Qtde;

                foreach (var regra in lstRegras_apoio)
                {
                    if (qtde_a_alocar == 0)
                        break;
                    if (!string.IsNullOrEmpty(regra.Produto))
                    {
                        foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                        {
                            if (qtde_a_alocar == 0)
                                break;
                            if (re.Id_nfe_emitente > 0 &&
                                (id_nfe_emitente_selecao_manual == 0 || re.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                            {
                                if (re.St_inativo == 0 || id_nfe_emitente_selecao_manual != 0)
                                {
                                    if (regra.Fabricante == produto.Fabricante && regra.Produto == produto.Produto)
                                    {
                                        if (re.Estoque_Qtde >= qtde_a_alocar)
                                        {
                                            re.Estoque_Qtde_Solicitado = (short)qtde_a_alocar;
                                            qtde_a_alocar = 0;
                                        }
                                        else if (re.Estoque_Qtde > 0)
                                        {
                                            re.Estoque_Qtde_Solicitado = re.Estoque_Qtde;
                                            qtde_a_alocar = qtde_a_alocar - re.Estoque_Qtde;
                                            lstRegras.Add(regra);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    lstRegras.Add(regra);
                }

                if (qtde_a_alocar > 0)
                {
                    foreach (var regra in lstRegras_apoio)
                    {
                        if (qtde_a_alocar == 0)
                            break;
                        if (!string.IsNullOrEmpty(regra.Produto))
                        {
                            foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (qtde_a_alocar == 0)
                                    break;
                                if (id_nfe_emitente_selecao_manual == 0)
                                {
                                    //seleção automática
                                    if (regra.Fabricante == produto.Fabricante && regra.Produto == produto.Produto &&
                                        re.Id_nfe_emitente > 0 &&
                                        re.Id_nfe_emitente == regra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                    {
                                        re.Estoque_Qtde_Solicitado ??= 0;
                                        re.Estoque_Qtde_Solicitado = (short)(re.Estoque_Qtde_Solicitado + qtde_a_alocar);
                                        qtde_a_alocar = 0;
                                    }
                                }
                                else
                                {
                                    //seleção manual
                                    if (regra.Fabricante == produto.Fabricante &&
                                       regra.Produto == produto.Produto &&
                                       re.Id_nfe_emitente > 0 &&
                                       re.Id_nfe_emitente == regra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                    {
                                        re.Estoque_Qtde_Solicitado ??= 0;
                                        re.Estoque_Qtde_Solicitado = (short)(re.Estoque_Qtde_Solicitado + qtde_a_alocar);
                                        qtde_a_alocar = 0;//verificar esse valor
                                    }
                                }
                            }
                        }

                        lstRegras.Add(regra);
                    }
                }
                if (qtde_a_alocar > 0)
                {
                    lstErros.Add("Falha ao processar a alocação de produtos no estoque: restaram " +
                        qtde_a_alocar + " unidades do produto (" +
                        produto.Fabricante + ")" + produto.Produto +
                        " que não puderam ser alocados na lista de produtos sem presença no estoque de nenhum CD");
                }
            }

            return lstRegras;
        }

        private List<int> ContagemEmpresasUsadasAutoSplit(List<RegrasBll> lstRegras, int id_nfe_emitente_selecao_manual)
        {
            int qtde_empresa_selecionada = 0;
            List<int> lista_empresa_selecionada = new List<int>();

            foreach (var regra in lstRegras)
            {
                if (!string.IsNullOrEmpty(regra.Produto))
                {
                    foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                    {
                        if (r.Id_nfe_emitente > 0 &&
                            (id_nfe_emitente_selecao_manual == 0 || r.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                        {
                            if (r.Estoque_Qtde_Solicitado > 0)
                            {
                                qtde_empresa_selecionada++;
                                lista_empresa_selecionada.Add(r.Id_nfe_emitente);
                            }
                        }
                    }
                }
            }

            return lista_empresa_selecionada;
        }

        private async Task ExisteProdutoDescontinuado(PedidoProdutoPedidoDados produto, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();


            if (!string.IsNullOrEmpty(produto.Produto))
            {
                var produtoTask = (from c in db.Tprodutos
                                   where c.Produto == produto.Produto
                                   select c.Descontinuado).FirstOrDefaultAsync();
                var p = await produtoTask;

                if (p.ToUpper() == "S")
                {
                    if (produto.Qtde > produto.Qtde_estoque_total_disponivel)
                        lstErros.Add("Produto (" + produto.Fabricante + ")" + produto.Produto +
                            " consta como 'descontinuado' e não há mais saldo suficiente " +
                            "no estoque para atender à quantidade solicitada.");
                }
            }
        }

        public class VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado_TesteRetorno
        {
            public List<RegrasBll> regrasBlls = new List<RegrasBll>();
            public List<string> prodValidadoEstoqueListaErros = new List<string>();
        }
        public async Task<VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado_TesteRetorno> VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado_Teste(PedidoProdutoPedidoDados produto,
            string cpf_cnpj, int id_nfe_emitente_selecao_manual)
        {
            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = from c in db.Tclientes
                              where c.Cnpj_Cpf == cpf_cnpj
                              select c;

            Tcliente cliente = await clienteTask.FirstOrDefaultAsync();

            var retorno = new VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado_TesteRetorno();

            //obtém  a sigla para regra
            string cliente_regra = UtilsProduto.MultiCdRegraDeterminaPessoa(cliente.Tipo, cliente.Contribuinte_Icms_Status,
                cliente.Produtor_Rural_Status);

            //buscar o produto
            //PedidoProdutosDtoPedido produto = new PedidoProdutosDtoPedido();

            List<RegrasBll> regraCrtlEstoque = (await ObterCtrlEstoqueProdutoRegraParaUMProduto(produto, cliente,
                retorno.prodValidadoEstoqueListaErros)).ToList();

            await UtilsProduto.ObterCtrlEstoqueProdutoRegra_Teste(retorno.prodValidadoEstoqueListaErros, regraCrtlEstoque, cliente.Uf,
                cliente_regra, contextoProvider);

            VerificarRegrasAssociadasParaUMProduto(regraCrtlEstoque, retorno.prodValidadoEstoqueListaErros, cliente,
                id_nfe_emitente_selecao_manual);

            if (id_nfe_emitente_selecao_manual != 0)
                await VerificarCDHabilitadoTodasRegras(regraCrtlEstoque, id_nfe_emitente_selecao_manual,
                    retorno.prodValidadoEstoqueListaErros);

            await ObterDisponibilidadeEstoque(regraCrtlEstoque, produto, retorno.prodValidadoEstoqueListaErros,
                id_nfe_emitente_selecao_manual);

            //meto responsavel por atribuir a qtde de estoque ao produto
            //await Util.Util.VerificarEstoque(regraCrtlEstoque, produto, id_nfe_emitente_selecao_manual, contextoProvider);

            bool estoqueInsuficiente = VerificarEstoqueInsuficienteUMProduto(regraCrtlEstoque, produto,
                id_nfe_emitente_selecao_manual, retorno.prodValidadoEstoqueListaErros);

            regraCrtlEstoque = VerificarQtdePedidosAutoSplit(regraCrtlEstoque, retorno.prodValidadoEstoqueListaErros, produto, id_nfe_emitente_selecao_manual);

            List<int> lst_empresa_selecionada = ContagemEmpresasUsadasAutoSplit(regraCrtlEstoque, id_nfe_emitente_selecao_manual);

            await ExisteProdutoDescontinuado(produto, retorno.prodValidadoEstoqueListaErros);

            retorno.regrasBlls = regraCrtlEstoque;

            return retorno;
        }


        public async Task<string> LeParametroControle(string id)
        {
            var db = contextoProvider.GetContextoLeitura();

            string controle = await (from c in db.Tcontroles
                                     where c.Id_Nsu == id
                                     select c.Nsu).FirstOrDefaultAsync();


            return controle;
        }

        public async Task<TtransportadoraCep> ObterTransportadoraPeloCep(string cep)
        {
            cep = cep.Replace("-", "").Trim();

            int cepteste = int.Parse(cep);
            cep = cepteste.ToString();
            var db = contextoProvider.GetContextoLeitura();



            TtransportadoraCep transportadoraCep = await (from c in db.TtransportadoraCeps
                                                          where (c.Tipo_range == 1 && c.Cep_unico == cep) ||
                                                                (
                                                                    c.Tipo_range == 2 &&
                                                                     (
                                                                         c.Cep_faixa_inicial.CompareTo(cep) <= 0 &&
                                                                         c.Cep_faixa_final.CompareTo(cep) >= 0
                                                                      )
                                                                )
                                                          select c).FirstOrDefaultAsync();

            return transportadoraCep;
        }

        public async Task<int> Fin_gera_nsu(string id_nsu, List<string> lstErros, ContextoBdGravacao dbgravacao)
        {
            int intRetorno = 0;
            //int intRecordsAffected = 0;
            //int intQtdeTentativas, intNsuUltimo, intNsuNovo;
            //bool blnSucesso = true;
            int nsu = 0;

            //conta a qtde de id
            var qtdeIdFin = from c in dbgravacao.TfinControles
                            where c.Id == id_nsu
                            select c.Id;


            if (qtdeIdFin != null)
            {
                intRetorno = await qtdeIdFin.CountAsync();
            }

            //não está cadastrado, então cadastra agora 
            if (intRetorno == 0)
            {
                //criamos um novo para salvar
                TfinControle tfinControle = new TfinControle();

                tfinControle.Id = id_nsu;
                tfinControle.Nsu = 0;
                tfinControle.Dt_hr_ult_atualizacao = DateTime.Now;

                dbgravacao.Add(tfinControle);

            }

            //laço de tentativas para gerar o nsu(devido a acesso concorrente)


            //obtém o último nsu usado
            var tfincontroleEditando = await (from c in dbgravacao.TfinControles
                                              where c.Id == id_nsu
                                              select c).FirstOrDefaultAsync();


            if (tfincontroleEditando == null)
            {
                lstErros.Add("Falha ao localizar o registro para geração de NSU (" + id_nsu + ")!");
                return nsu;
            }


            tfincontroleEditando.Id = id_nsu;
            tfincontroleEditando.Nsu++;
            tfincontroleEditando.Dt_hr_ult_atualizacao = DateTime.Now;
            //tenta atualizar o banco de dados
            dbgravacao.Update(tfincontroleEditando);

            await dbgravacao.SaveChangesAsync();

            return tfincontroleEditando.Nsu;
        }

        public async Task<bool> Grava_log_estoque_v2(string strUsuario, short id_nfe_emitente, string strFabricante,
            string strProduto, short intQtdeSolicitada, short intQtdeAtendida, string strOperacao,
            string strCodEstoqueOrigem, string strCodEstoqueDestino, string strLojaEstoqueOrigem,
            string strLojaEstoqueDestino, string strPedidoEstoqueOrigem, string strPedidoEstoqueDestino,
            string strDocumento, string strComplemento, string strIdOrdemServico, ContextoBdGravacao contexto)
        {

            TestoqueLog testoqueLog = new TestoqueLog();

            testoqueLog.data = DateTime.Now.Date;
            testoqueLog.Data_hora = DateTime.Now;
            testoqueLog.Usuario = strUsuario;
            testoqueLog.Id_nfe_emitente = id_nfe_emitente;
            testoqueLog.Fabricante = strFabricante;
            testoqueLog.Produto = strProduto;
            testoqueLog.Qtde_solicitada = intQtdeSolicitada;
            testoqueLog.Qtde_atendida = intQtdeAtendida;
            testoqueLog.Operacao = strOperacao;
            testoqueLog.Cod_estoque_origem = strCodEstoqueOrigem;
            testoqueLog.Cod_estoque_destino = strCodEstoqueDestino;
            testoqueLog.Loja_estoque_origem = strLojaEstoqueOrigem;
            testoqueLog.Loja_estoque_destino = strLojaEstoqueDestino;
            testoqueLog.Pedido_estoque_origem = strPedidoEstoqueOrigem;
            testoqueLog.Pedido_estoque_destino = strPedidoEstoqueDestino;
            testoqueLog.Documento = strDocumento;
            testoqueLog.Complemento = strComplemento.Length > 80 ? strComplemento.Substring(0, 80) : strComplemento;
            testoqueLog.Id_ordem_servico = strIdOrdemServico;

            contexto.Add(testoqueLog);
            await contexto.SaveChangesAsync();


            return true;
        }

        public void ConsisteProdutosValorZerados(List<PedidoProdutoPedidoDados> lstProdutos, List<string> lstErros,
            bool comIndicacao, short PermiteRaStatus)
        {
            foreach (var x in lstProdutos)
            {
                if (x.Preco_Venda <= 0)
                    lstErros.Add("Produto '" + x.Produto + "' está com valor de venda zerado!");
                else if (comIndicacao && PermiteRaStatus == 1 && x.Preco_NF <= 0)
                    lstErros.Add("Produto '" + x.Produto + "' está com preço zerado!");
            };
        }

        public async Task ValidarProdutosComFormaPagto(PedidoCriacaoDados pedidoCriacao, string siglaCustoFinancFornec,
            int qtdeParcCustoFinancFornec, List<string> lstErros)
        {
            if (pedidoCriacao.FormaPagtoCriacao.Rb_forma_pagto != Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                var db = contextoProvider.GetContextoLeitura();

                foreach (var prod in pedidoCriacao.ListaProdutos)
                {
                    TpercentualCustoFinanceiroFornecedor custoFinancFornec = await (from c in db.TpercentualCustoFinanceiroFornecedors
                                                                                    where c.Fabricante == prod.Fabricante &&
                                                                                        c.Tipo_Parcelamento == siglaCustoFinancFornec &&
                                                                                        c.Qtde_Parcelas == qtdeParcCustoFinancFornec
                                                                                    select c).FirstOrDefaultAsync();

                    if (custoFinancFornec == null)
                        lstErros.Add("Opção de parcelamento não disponível para fornecedor " + prod.Fabricante + ": " +
                            DecodificaCustoFinanFornecQtdeParcelas(siglaCustoFinancFornec, (short)qtdeParcCustoFinancFornec) + " parcela(s).");


                    TprodutoLoja prodLoja = await (from c in db.TprodutoLojas.Include(x => x.Tproduto)
                                                   where c.Tproduto.Produto == prod.Produto &&
                                                   c.Tproduto.Fabricante == prod.Fabricante &&
                                                   c.Loja == pedidoCriacao.LojaUsuario
                                                   select c).FirstOrDefaultAsync();

                    if (prodLoja == null)
                        lstErros.Add("Produto " + prod.Produto + " não localizado para a loja " + pedidoCriacao.LojaUsuario + ".");

                }

            }
        }
    }
}
