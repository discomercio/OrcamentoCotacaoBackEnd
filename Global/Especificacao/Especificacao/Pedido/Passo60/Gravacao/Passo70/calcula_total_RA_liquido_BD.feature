@Especificacao.Pedido.Passo60
Feature: calcula_total_RA_liquido_BD

Background:
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"

Scenario: calcula_total_RA_liquido_BD - magento
	#loja/PedidoNovoConfirma.asp
	#Para o pedido pai: chama a rotina calcula_total_RA_liquido_BD linha 2225 e atualiza vl_total_RA_liquido qtde_parcelas_desagio_RA st_tem_desagio_RA
	#calcula_total_RA_liquido_BD(id_pedido, vl_total_RA_liquido, msg_erro)
	#if indice_pedido = 1 then
	#	s = "SELECT * FROM t_PEDIDO WHERE (pedido='" & id_pedido & "')"
	#	if rs.State <> 0 then rs.Close
	#	rs.open s, cn
	#	if rs.Eof then
	#		alerta = "Falha ao consultar o registro do novo pedido (" & id_pedido & ")"
	#	else
	#		rs("vl_total_RA_liquido") = vl_total_RA_liquido
	#		rs("qtde_parcelas_desagio_RA") = 0
	#		if vl_total_RA <> 0 then
	#			rs("st_tem_desagio_RA") = 1
	#		else
	#			rs("st_tem_desagio_RA") = 0
	#			end if
	#		rs.Update
	#		end if
	#	end if
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	When Informo "Frete" = "10"
	When Lista de itens "0" informo "Preco_NF" = "704.05"
	When Lista de itens "1" informo "Preco_NF" = "1051.07"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vl_total_RA_liquido" = "377.34"

@ignore
Scenario: calcula_total_RA_liquido_BD - Loja
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	When Informo "PermiteRaStatus" = "1"
	When Lista de itens "0" informo "Preco_NF" = "704.05"
	When Lista de itens "1" informo "Preco_NF" = "1051.07"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vl_total_RA_liquido" = "377.34"