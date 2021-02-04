using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava30
{
    class Grava30 : PassoBaseGravacao
    {
        public Grava30(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao)
        {
        }

        public async Task ExecutarAsync()
        {


            //Passo30: OBTÉM DISPONIBILIDADE DO PRODUTO NO ESTOQUE -linha 1083
            //	Para todas as regras linha 1086
            //		Se o CD deve ser usado(manual ou auto)
            //		para todos os CDs da regra linha 1088
            //			Procura esse produto na lista de produtos linha 1095
            //			estoque_verifica_disponibilidade_integral_v2 em estoque.asp, especificado em Passo30 / estoque_verifica_disponibilidade_integral_v2.feature
            //				'Calcula quantidade em estoque no CD especificado

            //	Traduzindo:
            //			Calcula o estoque de cada produto em cada CD que pode ser usado



            /*
                    'OBTÉM DISPONIBILIDADE DO PRODUTO NO ESTOQUE
                    for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
                        if Trim(vProdRegra(iRegra).produto) <> "" then
                            for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
                                if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente > 0) And _
                                    ( (id_nfe_emitente_selecao_manual = 0) Or (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = id_nfe_emitente_selecao_manual) ) then
                                    'VERIFICA SE O CD ESTÁ HABILITADO
                                    'IMPORTANTE: A SELEÇÃO MANUAL DE CD PERMITE O USO DE CD DESATIVADO
                                    if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).st_inativo = 0) Or (id_nfe_emitente_selecao_manual <> 0) then
                                        idxItem = Lbound(v_item) - 1
                                        for iItem=Lbound(v_item) to Ubound(v_item)
                                            if (vProdRegra(iRegra).fabricante = v_item(iItem).fabricante) And (vProdRegra(iRegra).produto = v_item(iItem).produto) then
                                                idxItem = iItem
                                                exit for
                                                end if
                                            next
                                        if idxItem < Lbound(v_item) then
                                            alerta=texto_add_br(alerta)
                                            alerta=alerta & "Falha ao localizar o produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " na lista de produtos a ser processada"
                                        else
                                            vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.fabricante = v_item(idxItem).fabricante
                                            vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.produto = v_item(idxItem).produto
                                            vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.descricao = v_item(idxItem).descricao
                                            vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.descricao_html = v_item(idxItem).descricao_html
                                            vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada = v_item(idxItem).qtde
                                            vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_estoque = 0
                                            if Not estoque_verifica_disponibilidade_integral_v2(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente, vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque) then
                                                alerta=texto_add_br(alerta)
                                                alerta=alerta & "Falha ao tentar consultar disponibilidade no estoque do produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto
                                                end if
                                            end if
                                        end if
                                    end if

                                if alerta <> "" then exit for
                                next
                            end if
            */
            foreach (var vProdRegra_iRegra in Execucao.Gravacao.ListaRegrasControleEstoque)
            {
                //vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD) é twmsCdXUfXPessoaXCd
                foreach (var twmsCdXUfXPessoaXCd in vProdRegra_iRegra.TwmsCdXUfXPessoaXCd)
                {
                    if (twmsCdXUfXPessoaXCd.Id_nfe_emitente > 0 &&
                        (Pedido.Ambiente.Id_nfe_emitente_selecao_manual == 0 || twmsCdXUfXPessoaXCd.Id_nfe_emitente == Pedido.Ambiente.Id_nfe_emitente_selecao_manual))
                    {
                        if ((twmsCdXUfXPessoaXCd.St_inativo == 0) || (Pedido.Ambiente.Id_nfe_emitente_selecao_manual != 0))
                        {
                            //for iItem = Lbound(v_item) to Ubound(v_item)
                            //v_item(iItem) é produtoNoPedido
                            Pedido.Dados.Criacao.PedidoCriacaoProdutoDados? v_item_idxItem = null;
                            foreach (var produtoNoPedido in Pedido.ListaProdutos)
                            {
                                //if (vProdRegra(iRegra).fabricante = v_item(iItem).fabricante) And(vProdRegra(iRegra).produto = v_item(iItem).produto) then
                                if ((vProdRegra_iRegra.Fabricante == produtoNoPedido.Fabricante) && (vProdRegra_iRegra.Produto == produtoNoPedido.Produto))
                                {
                                    v_item_idxItem = produtoNoPedido;
                                    break;
                                }
                            }

                            if (v_item_idxItem == null)
                            {
                                Retorno.ListaErros.Add("Falha ao localizar o produto (" + vProdRegra_iRegra.Fabricante + ")" + vProdRegra_iRegra.Produto + " na lista de produtos a ser processada");
                            }
                            else
                            {
                                twmsCdXUfXPessoaXCd.Estoque_Fabricante = v_item_idxItem.Fabricante;
                                twmsCdXUfXPessoaXCd.Estoque_Produto = v_item_idxItem.Produto;
                                twmsCdXUfXPessoaXCd.Estoque_Descricao = Execucao.TabelasBanco.TprodutoLoja_Include_Tprodtuo_Tfabricante.Where
                                    (r => r.Fabricante == v_item_idxItem.Fabricante && r.Produto == v_item_idxItem.Produto).Select(r => r.Tproduto.Descricao).FirstOrDefault();
                                twmsCdXUfXPessoaXCd.Estoque_DescricaoHtml = Execucao.TabelasBanco.TprodutoLoja_Include_Tprodtuo_Tfabricante.Where
                                    (r => r.Fabricante == v_item_idxItem.Fabricante && r.Produto == v_item_idxItem.Produto).Select(r => r.Tproduto.Descricao_Html).FirstOrDefault();
                                twmsCdXUfXPessoaXCd.Estoque_Qtde_Solicitado = v_item_idxItem.Qtde;
                                twmsCdXUfXPessoaXCd.Estoque_Qtde_Estoque = 0;

                                await global::Produto.Estoque.Estoque.Estoque_verifica_disponibilidade_integral_v2(ContextoBdGravacao, twmsCdXUfXPessoaXCd.Id_nfe_emitente, twmsCdXUfXPessoaXCd);
                            }


                        }

                    }
                }
            }
        }
    }
}
