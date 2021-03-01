@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: CreditoAutomaticoECommerce

#tratado no código em \arclube\Global\Pedido\Criacao\Passo30\CamposMagentoExigidos.cs:ConfigurarBlnPedidoECommerceCreditoOkAutomatico()
#
#'TRATAMENTO PARA CADASTRAMENTO DE PEDIDOS DO SITE MAGENTO DO ARCLUBE
#	verifica se deve colocar crédito OK automaticamente
#loja/PedidoNOvoConfirma.asp
#	'TRATAMENTO PARA CADASTRAMENTO DE PEDIDOS DO SITE MAGENTO DO ARCLUBE
#	dim blnPedidoECommerceCreditoOkAutomatico
#	blnPedidoECommerceCreditoOkAutomatico = False
#	if loja = NUMERO_LOJA_ECOMMERCE_AR_CLUBE then
#		if alerta = "" then
#			if s_origem_pedido = "" then
#				alerta = "Informe a origem do pedido"
#				end if
#			end if
#
#		if alerta = "" then
#		'	PARA PEDIDOS DO ARCLUBE, É PERMITIDO FICAR SEM O Nº MAGENTO SOMENTE NOS SEGUINTES CASOS:
#		'		1) PEDIDO ORIGINADO PELO TELEVENDAS
#		'		2) PEDIDO GERADO CONTRA A TRANSPORTADORA (EM CASOS QUE A TRANSPORTADORA SE RESPONSABILIZA PELA REPOSIÇÃO DE MERCADORIA EXTRAVIADA)
#			if (Trim(s_origem_pedido) <> "002") And (Trim(s_origem_pedido) <> "019") then
#				if s_pedido_ac = "" then
#					alerta=texto_add_br(alerta)
#					alerta=alerta & "Informe o nº Magento"
#					end if
#				end if
#
#            if s_pedido_ac <> "" then
#                if s_pedido_ac <> retorna_so_digitos(s_pedido_ac) then
#                    alerta=texto_add_br(alerta)
#	                alerta=alerta & "O número Magento deve conter apenas dígitos"
#                end if
#
#                do while Len(s_pedido_ac) < 9
#                    if Len(s_pedido_ac) = 8 then
#                        s_pedido_ac = "1" & s_pedido_ac
#                    else
#                        s_pedido_ac = "0" & s_pedido_ac
#						end if
#					Loop
#
#				if Left(s_pedido_ac, 1) <> "1" then
#					alerta=texto_add_br(alerta)
#					alerta=alerta & "O número do pedido Magento inicia com dígito inválido para a loja " & loja
#					end if
#				end if 'if s_pedido_ac <> ""
#
#			s = "SELECT * FROM t_CODIGO_DESCRICAO WHERE (grupo = 'PedidoECommerce_Origem') AND (codigo = '" & s_origem_pedido & "')"
#			set rs = cn.execute(s)
#			if rs.Eof then
#				alerta=texto_add_br(alerta)
#				alerta=alerta & "Código de origem do pedido (marketplace) não cadastrado: " & s_origem_pedido
#			else
#			'	PROCESSA OS PARÂMETROS DEFINIDOS PARA A ORIGEM (GRUPO)
#				s = "SELECT * FROM T_CODIGO_DESCRICAO WHERE (grupo = 'PedidoECommerce_Origem_Grupo') AND (codigo = '" & Trim("" & rs("codigo_pai")) & "')"
#				set rs2 = cn.execute(s)
#				if Not rs2.Eof then
#				'	OBTÉM O PERCENTUAL DE COMISSÃO DO MARKETPLACE
#					perc_RT = rs2("parametro_campo_real")
#				'	DEVE COLOCAR AUTOMATICAMENTE COM 'CRÉDITO OK'?
#					if rs2("parametro_1_campo_flag") = 1 then blnPedidoECommerceCreditoOkAutomatico = True
#				'	Nº PEDIDO MARKETPLACE É OBRIGATÓRIO?
#					if rs2("parametro_2_campo_flag") = 1 then
#						if s_numero_mktplace = "" then
#							alerta=texto_add_br(alerta)
#							alerta=alerta & "Informe o nº do pedido do marketplace (" & Trim("" & rs("descricao")) & ")"
#							end if
#						end if
#					end if 'if Not rs2.Eof then
#				end if 'if rs.Eof
#			if rs.State <> 0 then rs.Close
#			end if 'if alerta = "" then
#
#            if s_numero_mktplace <> "" then
#                s = ""
#                For i = 1 To Len(s_numero_mktplace)
#                    c = Mid(s_numero_mktplace, i, 1)
#                    If IsNumeric(c) Or c = chr(45) Then s = s & c
#                    Next
#                if s_numero_mktplace <> s then
#                    alerta=texto_add_br(alerta)
#					alerta=alerta & "O número Marketplace deve conter apenas dígitos e hífen"
#					end if
#				end if
#e, no momento do salvamento:
#if blnPedidoECommerceCreditoOkAutomatico then
#	rs("analise_credito")=Clng(COD_AN_CREDITO_OK)
#	rs("analise_credito_data")=Now
#	rs("analise_credito_usuario")="AUTOMÁTICO"
Scenario: CreditoAutomaticoECommerce - NUMERO_LOJA_ECOMMERCE_AR_CLUBE
	#essa opção é usada se loja for NUMERO_LOJA_ECOMMERCE_AR_CLUBE ou se a origem for a API do magento
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "C_pu_valor" = "3132.90"
	When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "010"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "123"
	When Informo "appsettings.Loja" = "201"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "5"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vendedor" = "USRMAG"
	And Tabela "t_PEDIDO" registro criado, verificar campo "analise_credito" = "2"
	And Tabela "t_PEDIDO" registro criado, verificar campo "analise_credito_usuario" = "AUTOMÁTICO"
	And Tabela "t_PEDIDO" registro criado, verificar campo "analise_endereco_tratar_status" = "0"

Scenario: CreditoAutomaticoECommerce - NUMERO_LOJA_ECOMMERCE_AR_CLUBE á vista
	#teste 4 =>
	#	loja = 201 && pagamento á vista && pagamento em boleto
	#	tpedido.Analise_Credito = 8;
	#    tpedido.Analise_Credito_Usuario = AUTOMÁTICO;
	#	Analise_Credito_Pendente_Vendas_Motivo = 006
	Given Pedido base
	When Informo "appsettings.Loja" = "201"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "av_forma_pagto" = "6"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vendedor" = "USRMAG"
	And Tabela "t_PEDIDO" registro criado, verificar campo "analise_credito" = "8"
	And Tabela "t_PEDIDO" registro criado, verificar campo "analise_credito_usuario" = "AUTOMÁTICO"
	And Tabela "t_PEDIDO" registro criado, verificar campo "analise_endereco_tratar_status" = "0"

Scenario: CreditoAutomaticoECommerce - Loja diferente de NUMERO_LOJA_ECOMMERCE_AR_CLUBE
	#essa opção é usada se loja for NUMERO_LOJA_ECOMMERCE_AR_CLUBE ou se a origem for a API do magento
	#Pedido_bs_x_marketplace e Marketplace_codigo_origem
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vendedor" = "USRMAG"
	And Tabela "t_PEDIDO" registro criado, verificar campo "analise_credito" = "9"
	And Tabela "t_PEDIDO" registro criado, verificar campo "analise_credito_usuario" = "AUTOMÁTICO"
	And Tabela "t_PEDIDO" registro criado, verificar campo "analise_endereco_tratar_status" = "0"