@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
@GerenciamentoBanco
Feature: PedidoMagentoRepetido
#2) Seria necessário tratar a possibilidade de ocorrer acesso concorrente entre o cadastramento semi-automático e a integração.
#Em ambos os casos, seria importante verificar no instante final antes da efetivar o cadastramento do pedido se o número Magento e,
#caso exista, o número do pedido marketplace já estão cadastrados em algum pedido c/ st_entrega válido (diferente de cancelado).

#Isto está feito no ASP abaixo:
#loja/PedidoNovoConfirma.asp
#ATENÇÃO: fazer com 2 campos separadamente; pedido_bs_x_marketplace e pedido_bs_x_ac
#no asp:
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
#........................
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

Scenario: Pedido_bs_x_marketplace e Pedido_bs_x_ac já existem
	Given Reiniciar banco ao terminar cenário

	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "128"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "103456789"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "019"
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "3132.90"
	Then Sem nenhum erro

	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "138"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "103456789"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "019"
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "3132.90"
	Then Erro "regex O nº pedido Magento Pedido_Bs_X_Ac 103456789 .*"

	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "128"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "100456789"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "019"
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "3132.90"
	Then Erro "regex O nº pedido Magento Pedido_Bs_X_Marketplace 128 .*"

	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "138"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "100456789"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "019"
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "3132.90"
	Then Sem nenhum erro
