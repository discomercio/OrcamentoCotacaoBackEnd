@Especificacao.Pedido.PedidoFaltandoImplementarSteps
Feature: Senhas_de_autorizacao_para_desconto_superior
#loja/PedidoNovoConfirma.asp
#note que ele libera as senhas que foram usadas anteriormente para liberar o desconto superior, e somente essas
#linha 2251
#'		SENHAS DE AUTORIZAÇÃO PARA DESCONTO SUPERIOR
#		for k = Lbound(v_desconto) to Ubound(v_desconto)
#			if Trim(v_desconto(k)) <> "" then
#				s = "SELECT * FROM t_DESCONTO" & _
#					" WHERE (usado_status=0)" & _
#					" AND (cancelado_status=0)" & _
#					" AND (id='" & Trim(v_desconto(k)) & "')"
#				if rs.State <> 0 then rs.Close
#				rs.open s, cn
#				if rs.Eof then
#					alerta = "Senha de autorização para desconto superior não encontrado."
#					exit for
#				else
#					rs("usado_status") = 1
#					rs("usado_data") = Now
#					if (operacao_origem = OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO) And blnMagentoPedidoComIndicador then
#						rs("vendedor") = sIdVendedor
#					else
#						rs("vendedor") = usuario
#						end if
#					rs("usado_usuario") = usuario
#					rs.Update
#					if Err <> 0 then
#					'	~~~~~~~~~~~~~~~~
#						cn.RollbackTrans
#					'	~~~~~~~~~~~~~~~~
#						Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_BD)
#						end if
#					end if
#				end if
#			next

Scenario: Senhas_de_autorizacao_para_desconto_superior
	When Fazer esta validação

