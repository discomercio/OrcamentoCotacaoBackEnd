@ignore
Feature: calcula_total_RA_liquido_BD

Scenario: calcula_total_RA_liquido_BD
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
	Given Pedido base
	When Informo "PermiteRAStatus" = "1"
	When Informo "ListaProdutos[0].Preco_NF" = "704.05"
	When Informo "ListaProdutos[1].Preco_NF" = "1051.07"
	When Informo "ValorTotalDestePedidoComRA" = "3510.24" 3470.24
	Then Tabela "t_PEDIDO" registro com campo "pedido" = "pedido gerado", verificar campo "vl_total_RA_liquido" = "40.0000"

