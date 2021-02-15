@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: ParcelaUnica
	Estamos testando o pagamento COD_FORMA_PAGTO_PARCELA_UNICA

Background:
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "019"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "123"

Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - verifica campos na tabela t_PEDIDO
	When Informo "C_pu_valor" = "3132.90"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "5"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_forma_pagto" = "2"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_vencto_apos" = "30"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_valor" = "3132.90"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_valor_parcela" = "0.0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "av_forma_pagto" = "0"

Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - C_pu_valor errado
	When Informo "C_pu_valor" = "1"
	Then Erro "Valor total da forma de pagamento diferente do valor total!"

Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - C_pu_valor negativo
	When Informo "C_pu_valor" = "-1"
	Then Erro "Valor da parcela única é inválido."

Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - C_pu_valor zerado
	When Informo "C_pu_valor" = "0"
	Then Erro "Valor da parcela única é inválido."