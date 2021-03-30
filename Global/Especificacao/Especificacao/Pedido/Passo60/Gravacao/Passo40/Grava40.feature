@ignore
@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: Grava40.feature

Background:
	Given Reiniciar banco ao terminar cenário

#precisamos testar isto:
#        if erro_produto_indisponivel then
#            for i=Lbound(v_item) to Ubound(v_item)
#                if v_item(i).qtde > v_item(i).qtde_estoque_total_disponivel then
#                    if (opcao_venda_sem_estoque="") then
#                        alerta=texto_add_br(alerta)
#                        alerta=alerta & "Produto " & v_item(i).produto & " do fabricante " & v_item(i).fabricante & ": falta(m) " & Cstr(Abs(v_item(i).qtde_estoque_total_disponivel-v_item(i).qtde)) & " unidade(s) no estoque."
#                    else
#                        qtde_spe = -1
#                        for j=Lbound(v_spe) to Ubound(v_spe)
#                            if (v_item(i).fabricante=v_spe(j).fabricante) And (v_item(i).produto=v_spe(j).produto) then
#                                qtde_spe = v_spe(j).qtde_estoque
#                                exit for
#                                end if
#                            next
#                        if qtde_spe <> v_item(i).qtde_estoque_total_disponivel then
#                            alerta=texto_add_br(alerta)
#                            alerta=alerta & "Produto " & v_item(i).produto & " do fabricante " & v_item(i).fabricante & ": disponibilidade do estoque foi alterada."
#                            end if
#                        end if
#                    end if
#                next
#            end if

#este erro nunca pode acontecer no magento porque não verificamos qtde_autorizada_sem_presenca

Scenario: Passo40: Verifica se a disponibilidade do estoque foi alterada - Linha 1159
	Given Fazer este teste
