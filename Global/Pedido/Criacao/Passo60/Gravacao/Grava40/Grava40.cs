using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava40
{
    class Grava40 : PassoBaseGravacao
    {
        public Grava40(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao, Execucao.Gravacao gravacao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao, gravacao)
        {
        }

        public void Executar()
        {
            //	Passo40: Verifica se a disponibilidade do estoque foi alterada - Linha 1159
            //	HÁ PRODUTO C/ ESTOQUE INSUFICIENTE (SOMANDO-SE O ESTOQUE DE TODAS AS EMPRESAS CANDIDATAS) - linha 1127

            //	Porque: avisamos o usuário que existem produtos sem presença no estoque e, no momento de salvar, os produtos sem presença no estoque foram alterados.
            //	No caso da ApiMagento, não temos essa verificação

            //deixamos o código do ASP aqui porque esse trecho é um pouco complicado

            /*
            '	HÁ PRODUTO C/ ESTOQUE INSUFICIENTE (SOMANDO-SE O ESTOQUE DE TODAS AS EMPRESAS CANDIDATAS)
            erro_produto_indisponivel = False
            for iItem=Lbound(v_item) to Ubound(v_item)
                if Trim(v_item(iItem).produto) <> "" then
                    qtde_estoque_total_disponivel = 0
                    for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
                        if Trim(vProdRegra(iRegra).produto) <> "" then
                            for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
                                if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente > 0) And _
                                    ( (id_nfe_emitente_selecao_manual = 0) Or (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = id_nfe_emitente_selecao_manual) ) then
                                    'VERIFICA SE O CD ESTÁ HABILITADO
                                    'IMPORTANTE: A SELEÇÃO MANUAL DE CD PERMITE O USO DE CD DESATIVADO
                                    if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).st_inativo = 0) Or (id_nfe_emitente_selecao_manual <> 0) then
                                        if (vProdRegra(iRegra).fabricante = v_item(iItem).fabricante) And (vProdRegra(iRegra).produto = v_item(iItem).produto) then
                                            qtde_estoque_total_disponivel = qtde_estoque_total_disponivel + vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_estoque
                                            end if
                                        end if
                                    end if
                                next
                            end if
                        next

                    v_item(iItem).qtde_estoque_total_disponivel = qtde_estoque_total_disponivel

                    if v_item(iItem).qtde > qtde_estoque_total_disponivel then
                        erro_produto_indisponivel = True
                        end if
                    end if
                next
            */
            foreach (var v_item_iItem in Gravacao.ProdutoGravacaoLista)
            {
                var qtde_estoque_total_disponivel = 0;
                foreach (var vProdRegra_iRegra in Gravacao.ListaRegrasControleEstoque)
                {
                    //vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD) é twmsCdXUfXPessoaXCd
                    foreach (var twmsCdXUfXPessoaXCd in vProdRegra_iRegra.TwmsCdXUfXPessoaXCd)
                    {
                        if ((twmsCdXUfXPessoaXCd.Id_nfe_emitente > 0) &&
                            ((Pedido.Ambiente.Id_nfe_emitente_selecao_manual == 0) || (twmsCdXUfXPessoaXCd.Id_nfe_emitente == Pedido.Ambiente.Id_nfe_emitente_selecao_manual)))
                        {
                            //'VERIFICA SE O CD ESTÁ HABILITADO
                            //'IMPORTANTE: A SELEÇÃO MANUAL DE CD PERMITE O USO DE CD DESATIVADO
                            if ((twmsCdXUfXPessoaXCd.St_inativo == 0) || (Pedido.Ambiente.Id_nfe_emitente_selecao_manual != 0))
                            {
                                if ((vProdRegra_iRegra.Fabricante == v_item_iItem.Pedido.Fabricante) && (vProdRegra_iRegra.Produto == v_item_iItem.Pedido.Produto))
                                {
                                    qtde_estoque_total_disponivel = qtde_estoque_total_disponivel + (twmsCdXUfXPessoaXCd.Estoque_Qtde_Estoque ?? 0);
                                }
                            }
                        }
                    }

                }

                //eram 2 loops, fizemos em um loop só

                /*
                        if erro_produto_indisponivel then
                            for i=Lbound(v_item) to Ubound(v_item)
                                if v_item(i).qtde > v_item(i).qtde_estoque_total_disponivel then
                                    if (opcao_venda_sem_estoque="") then
                                        alerta=texto_add_br(alerta)
                                        alerta=alerta & "Produto " & v_item(i).produto & " do fabricante " & v_item(i).fabricante & ": falta(m) " & Cstr(Abs(v_item(i).qtde_estoque_total_disponivel-v_item(i).qtde)) & " unidade(s) no estoque."
                                    else
                                        qtde_spe = -1
                                        for j=Lbound(v_spe) to Ubound(v_spe)
                                            if (v_item(i).fabricante=v_spe(j).fabricante) And (v_item(i).produto=v_spe(j).produto) then
                                                qtde_spe = v_spe(j).qtde_estoque
                                                exit for
                                                end if
                                            next
                                        if qtde_spe <> v_item(i).qtde_estoque_total_disponivel then
                                            alerta=texto_add_br(alerta)
                                            alerta=alerta & "Produto " & v_item(i).produto & " do fabricante " & v_item(i).fabricante & ": disponibilidade do estoque foi alterada."
                                            end if
                                        end if
                                    end if
                                next
                            end if
                */


                //o v_item(i).qtde_estoque_total_disponivel foi calculado acima, está na variável qtde_estoque_total_disponivel
                if (v_item_iItem.Pedido.Qtde > qtde_estoque_total_disponivel)
                {
                    /*
                '	HÁ ALGUM PRODUTO DESCONTINUADO?
                    if alerta = "" then
                        for i=Lbound(v_item) to Ubound(v_item)
                            if Trim(v_item(i).produto) <> "" then
                                if Ucase(Trim(v_item(i).descontinuado)) = "S" then
                                    if v_item(i).qtde > v_item(i).qtde_estoque_total_disponivel then
                                        alerta=texto_add_br(alerta)
                                        alerta=alerta & "Produto (" & v_item(i).fabricante & ")" & v_item(i).produto & " consta como 'descontinuado' e não há mais saldo suficiente no estoque para atender à quantidade solicitada."
                                        end if
                                    end if
                                end if
                            next
                        end if
                    */
                    var produto = (from p in Execucao.TabelasBanco.TprodutoLoja_Include_Tprodtuo_Tfabricante_Validado
                                   where p.Tproduto.Descontinuado.ToUpper().Trim() == "S"
                                   && p.Produto == v_item_iItem.Pedido.Produto
                                   && p.Fabricante == v_item_iItem.Pedido.Fabricante
                                   && p.Loja == Pedido.Ambiente.Loja
                                   select p);
                    if (produto.Any())
                    {
                        Retorno.ListaErros.Add("Produto (" + v_item_iItem.Pedido.Fabricante + ")" + v_item_iItem.Pedido.Produto +
                            " consta como 'descontinuado' e não há mais saldo suficiente " +
                            "no estoque para atender à quantidade solicitada.");
                    }


                    //erro_produto_indisponivel = true;
                    if (v_item_iItem.Pedido.Qtde_spe_usuario_aceitou.HasValue)
                    {
                        //se Qtde_spe_usuario_aceitou for null, não verificamos
                        if (v_item_iItem.Pedido.Qtde_spe_usuario_aceitou == 0)
                        {
                            //mesangem quando o usuário não aceitou nenhum produto faltando
                            Retorno.ListaErros.Add($"Produto {v_item_iItem.Pedido.Produto} do fabricante {v_item_iItem.Pedido.Fabricante}: falta(m) " +
                                $"{Math.Abs(qtde_estoque_total_disponivel - v_item_iItem.Pedido.Qtde)} unidade(s) no estoque.");
                        }
                        else
                        {
                            //se houver MAIS estoque disponível, não damos erro
                            if ((v_item_iItem.Pedido.Qtde - qtde_estoque_total_disponivel) > v_item_iItem.Pedido.Qtde_spe_usuario_aceitou)
                            {
                                //mensagem quando o número está menor do que o que ele concordou
                                Retorno.ListaErros.Add($"Produto {v_item_iItem.Pedido.Produto} do fabricante {v_item_iItem.Pedido.Fabricante}: disponibilidade do estoque foi alterada.");
                            }
                        }
                    }
                }
            }
        }
    }
}
