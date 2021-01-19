@ignore
Feature: PedidoMagentoRepetido
#2) Seria necessário tratar a possibilidade de ocorrer acesso concorrente entre o cadastramento semi-automático e a integração.
#Em ambos os casos, seria importante verificar no instante final antes da efetivar o cadastramento do pedido se o número Magento e,
#caso exista, o número do pedido marketplace já estão cadastrados em algum pedido c/ st_entrega válido (diferente de cancelado).

#loja/PedidoNovoConfirma.asp
#ATENÇÃO: fazer com 2 campos separadamente; pedido_bs_x_marketplace e pedido_bs_x_ac
#se loja = NUMERO_LOJA_ECOMMERCE_AR_CLUBE
#'	VERIFICA SE HÁ PEDIDO JÁ CADASTRADO COM O MESMO Nº PEDIDO MAGENTO (POSSÍVEL CADASTRO EM DUPLICIDADE)
#	if alerta = "" then
#		if s_pedido_ac <> "" then
#			s = "SELECT" & _
#					" tP.pedido," & _
#					" tP.pedido_bs_x_ac," & _
#					" tP.data_hora," & _
#					" tP.vendedor," & _
#					" tU.nome AS nome_vendedor," & _
#					" tP.endereco_cnpj_cpf AS cnpj_cpf," & _
#					" tP.endereco_nome AS nome_cliente" & _
#				" FROM t_PEDIDO tP" & _
#					" LEFT JOIN t_USUARIO tU ON (tP.vendedor = tU.usuario)" & _
#				" WHERE" & _
#					" (tP.st_entrega <> '" & ST_ENTREGA_CANCELADO & "')" & _
#					" AND (pedido_bs_x_ac = '" & s_pedido_ac & "')" & _
#					" AND (" & _
#						"tP.pedido NOT IN (" & _
#							"SELECT DISTINCT" & _
#								" pedido" & _
#							" FROM t_PEDIDO_DEVOLUCAO tPD" & _
#							" WHERE" & _
#								" (tPD.pedido = tP.pedido)" & _
#								" AND (tPD.status IN (" & _
#									COD_ST_PEDIDO_DEVOLUCAO__FINALIZADA & "," & _
#									COD_ST_PEDIDO_DEVOLUCAO__MERCADORIA_RECEBIDA & "," & _
#									COD_ST_PEDIDO_DEVOLUCAO__EM_ANDAMENTO & "," & _
#									COD_ST_PEDIDO_DEVOLUCAO__CADASTRADA & _
#									")" & _
#								")" & _
#							")" & _
#						")" & _
#					" AND (" & _
#						"tP.pedido NOT IN (" & _
#							"SELECT DISTINCT" & _
#								" pedido" & _
#							" FROM t_PEDIDO_ITEM_DEVOLVIDO tPID" & _
#							" WHERE" & _
#								" (tPID.pedido = tP.pedido)" & _
#							")" & _
#						")"
#			set rs = cn.execute(s)
#			if Not rs.Eof then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "O nº pedido Magento " & Trim("" & rs("pedido_bs_x_ac")) & " já está cadastrado no pedido " & Trim("" & rs("pedido"))
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "Data de cadastramento do pedido: " & formata_data_hora_sem_seg(rs("data_hora"))
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "Cadastrado por: " & Trim("" & rs("vendedor"))
#				if Ucase(Trim("" & rs("vendedor"))) <> Ucase(Trim("" & rs("nome_vendedor"))) then alerta=alerta & " (" & Trim("" & rs("nome_vendedor")) & ")"
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "Cliente: " & cnpj_cpf_formata(Trim("" & rs("cnpj_cpf"))) & " - " & Trim("" & rs("nome_cliente"))
#				end if 'if Not rs.Eof
#			end if 'if s_pedido_ac <> ""
#		end if 'if alerta = ""

Scenario: Pedido_bs_x_marketplace e Marketplace_codigo_origem já existem
	When Fazer esta validação

