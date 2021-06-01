@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: ParcelaUnica
	Estamos testando o pagamento COD_FORMA_PAGTO_PARCELA_UNICA

Background:
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "1040.00"
	When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "019"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "123"
	And Informo "frete" = "10"
	When Lista de itens com "1" itens
	When Lista de itens "0" informo "Produto" = "001000"
	When Lista de itens "0" informo "Qtde" = "2"
	When Lista de itens "0" informo "Preco_Venda" = "509.24"
	When Lista de itens "0" informo "Preco_NF" = "520.00"
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Given Reiniciar banco ao terminar cenário

Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - verifica campos na tabela t_PEDIDO
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "5"
	And Tabela "t_PEDIDO" registro criado, verificar campo "CustoFinancFornecTipoParcelamento" = "SE"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_forma_pagto" = "2"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_vencto_apos" = "30"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_valor" = "1050.00"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_valor_parcela" = "0.0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "av_forma_pagto" = "0"

Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - C_pu_valor errado
	When Recalcular totais do pedido
	When Informo "C_pu_valor" = "1"
	Then Erro "Valor total da forma de pagamento diferente do valor total!"

Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - C_pu_valor negativo
	When Informo "C_pu_valor" = "-1"
	Then Erro "Valor da parcela única é inválido."

Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - C_pu_valor zerado
	When Informo "C_pu_valor" = "0"
	Then Erro "Valor da parcela única é inválido."

Scenario: COD_FORMA_PAGTO_PARCELA_UNICA - t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR
	Given Limpar tabela "t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR"
	Then Erro "Coeficiente não cadastrado para o fabricante. Fabricante: 001, TipoParcela: SE"

Scenario: Tipo_Parcelamento - não existe
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "12"
	Then Erro "regex .*Coeficiente não cadastrado para o fabricante.*"

@ignore
Scenario: Tipo_Parcelamento conforme origem
	#/// Pedido que vier do Markeplace deve ser Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELA_UNICA = "5"
	#/// Pedido que vier do Magento deve ser Tipo_Parcelamento = COD_FORMA_PAGTO_A_VISTA = "1" ou Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELADO_CARTAO = "2"
	#Se não obedecer essas condições, dar erro
	Given Fazer este teste