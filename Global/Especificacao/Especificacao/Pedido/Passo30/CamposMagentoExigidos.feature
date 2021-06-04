@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: CamposMagentoExigidos

Background: Configuracao
	Given Reiniciar appsettings
	Given Reiniciar banco ao terminar cenário

#loja/PedidoNovoConfirma.asp
#se loja == NUMERO_LOJA_ECOMMERCE_AR_CLUBE
#s_origem_pedido é obrigatório
#
#if alerta = "" then
#'	PARA PEDIDOS DO ARCLUBE, É PERMITIDO FICAR SEM O Nº MAGENTO SOMENTE NOS SEGUINTES CASOS:
#'		1) PEDIDO ORIGINADO PELO TELEVENDAS
#'		2) PEDIDO GERADO CONTRA A TRANSPORTADORA (EM CASOS QUE A TRANSPORTADORA SE RESPONSABILIZA PELA REPOSIÇÃO DE MERCADORIA EXTRAVIADA)
#	if (Trim(s_origem_pedido) <> "002") And (Trim(s_origem_pedido) <> "019") then
#		if s_pedido_ac = "" then
#			alerta=texto_add_br(alerta)
#			alerta=alerta & "Informe o nº Magento"
#			end if
#		end if
#
#    if s_pedido_ac <> "" then
#        if s_pedido_ac <> retorna_so_digitos(s_pedido_ac) then
#            alerta=texto_add_br(alerta)
#	        alerta=alerta & "O número Magento deve conter apenas dígitos"
#        end if
#
#        do while Len(s_pedido_ac) < 9
#            if Len(s_pedido_ac) = 8 then
#                s_pedido_ac = "1" & s_pedido_ac
#            else
#                s_pedido_ac = "0" & s_pedido_ac
#				end if
#			Loop
#
#		if Left(s_pedido_ac, 1) <> "1" then
#			alerta=texto_add_br(alerta)
#			alerta=alerta & "O número do pedido Magento inicia com dígito inválido para a loja " & loja
#			end if
#		end if 'if s_pedido_ac <> ""
#
#	s = "SELECT * FROM t_CODIGO_DESCRICAO WHERE (grupo = 'PedidoECommerce_Origem') AND (codigo = '" & s_origem_pedido & "')"
#	set rs = cn.execute(s)
#	if rs.Eof then
#		alerta=texto_add_br(alerta)
#		alerta=alerta & "Código de origem do pedido (marketplace) não cadastrado: " & s_origem_pedido
#	else
#	'	PROCESSA OS PARÂMETROS DEFINIDOS PARA A ORIGEM (GRUPO)
#		s = "SELECT * FROM T_CODIGO_DESCRICAO WHERE (grupo = 'PedidoECommerce_Origem_Grupo') AND (codigo = '" & Trim("" & rs("codigo_pai")) & "')"
#		set rs2 = cn.execute(s)
#		if Not rs2.Eof then
#		'	OBTÉM O PERCENTUAL DE COMISSÃO DO MARKETPLACE
#			perc_RT = rs2("parametro_campo_real")
#		'	DEVE COLOCAR AUTOMATICAMENTE COM 'CRÉDITO OK'?
#			if rs2("parametro_1_campo_flag") = 1 then blnPedidoECommerceCreditoOkAutomatico = True
#		'	Nº PEDIDO MARKETPLACE É OBRIGATÓRIO?
#			if rs2("parametro_2_campo_flag") = 1 then
#				if s_numero_mktplace = "" then
#					alerta=texto_add_br(alerta)
#					alerta=alerta & "Informe o nº do pedido do marketplace (" & Trim("" & rs("descricao")) & ")"
#					end if
#				end if
#			end if 'if Not rs2.Eof then
#		end if 'if rs.Eof
#	if rs.State <> 0 then rs.Close
#	end if 'if alerta = "" then
#
#    if s_numero_mktplace <> "" then
#        s = ""
#        For i = 1 To Len(s_numero_mktplace)
#            c = Mid(s_numero_mktplace, i, 1)
#            If IsNumeric(c) Or c = chr(45) Then s = s & c
#            Next
#        if s_numero_mktplace <> s then
#            alerta=texto_add_br(alerta)
#			alerta=alerta & "O número Marketplace deve conter apenas dígitos e hífen"
#			end if
#		end if

Scenario: Pedido_bs_x_marketplace - vazio
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "3132.90"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = ""
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "010"
	Then Erro "Informe o nº do pedido do marketplace (Mercado Livre)"

Scenario: Pedido_bs_x_marketplace - somente digitos
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "3132.90"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456789"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "123AA"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "010"
	Then Erro "O número Marketplace deve conter apenas dígitos e hífen"
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "3132.90"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456789"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "12-3AA"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "010"
	Then Erro "O número Marketplace deve conter apenas dígitos e hífen"

Scenario: Marketplace_codigo_origem - vazio
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "126"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = ""
	Then Erro "Informe o Marketplace_codigo_origem."

Scenario: Marketplace_codigo_origem - não existe
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "127"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456789"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "123"
	Then Erro "Código Marketplace não encontrado."

Scenario: Pedido_bs_x_ac - vazio
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = ""
	Then Erro "Favor informar o número do pedido Magento(Pedido_bs_x_ac)!"

Scenario: Pedido_bs_x_ac - formato inválido
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "12345678"
	Then Erro "Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!"
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "1234567890"
	Then Erro "Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!"

Scenario: Pedido_bs_x_ac - somente digitos
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "1234567AA"
	Then Erro "O número Magento deve conter apenas dígitos!"

@ignore
Scenario: Pedido_bs_x_ac - inicia com dígito inválido loja 201
	And verificar diigito inicial conforme a t_loja
	#hamilton vai mandar script com esses campos- campos ocmecam com MAGENTO_API_REST, apicar somente se for magento 2
	Given Pedido base
	And Informo "t_loja" de "201", campo "digito_magento" = "4"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "223456778"
	Then Erro "regex .*O número do pedido Magento inicia com dígito inválido para a loja"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "423456778"
	Then Sem erro "regex .*O número do pedido Magento inicia com dígito inválido para a loja"

@ignore
Scenario: Pedido_bs_x_ac - inicia com dígito inválido loja 202
	And verificar diigito inicial conforme a t_loja
	Given Pedido base
	And Informo "appsettings.Loja" = "202"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456778"
	Then Erro "regex .*O número do pedido Magento inicia com dígito inválido para a loja"

