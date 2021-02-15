@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: AVista
	Estamos testando o pagamento COD_FORMA_PAGTO_A_VISTA

Background:
	Given Pedido base

Scenario: COD_FORMA_PAGTO_A_VISTA - verifica campos na tabela t_PEDIDO
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "av_forma_pagto" = "6"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_valor" = "0.0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_valor_parcela" = "0.0"