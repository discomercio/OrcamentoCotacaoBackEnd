@ignore
Feature: COD_FORMA_PAGTO_PARCELADO_CARTAO
#pergunta: se COD_FORMA_PAGTO_PARCELADO_CARTAO temos que usar os coeficientes do fabricante?
#respsota: sim, mas precisa manter o valor do preço e o total da nota igual. Vamos colocar essa diferença no Vl Lista
#"ListaProdutos": [
#    {
#      "Fabricante": "001",
#      "Produto": "001000",
#      "Qtde": 2,
#      "Preco_Venda": 509.24,
#      "Preco_NF": 520.00
#    },
#    {
#      "Fabricante": "001",
#      "Produto": "001001",
#      "Qtde": 2,
#      "Preco_Venda": 1188.23,
#      "Preco_NF": 1200.00
#    }
#  ],
#  "VlTotalDestePedido": 3440.00,
Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "FormaPagtoCriacao.C_pc_qtde" = "1"
	When Informo "FormaPagtoCriacao.C_pc_valor" = "3440.00"
	When Informo "ListaProdutos[0].Fabricante" = "001"
	When Informo "ListaProdutos[0].Produto" = "001000"
	When Informo "ListaProdutos[0].Qtde" = "2"
	When Informo "ListaProdutos[0].Preco_Venda" = "509.24"
	When Informo "ListaProdutos[0].Preco_NF" = "520.00"
	When Informo "ListaProdutos[1].Fabricante" = "001"
	When Informo "ListaProdutos[1].Produto" = "001001"
	When Informo "ListaProdutos[1].Qtde" = "2"
	When Informo "ListaProdutos[1].Preco_Venda" = "1188.23"
	When Informo "ListaProdutos[1].Preco_NF" = "1200.00"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número do pedido}", verificar campo "tipo_parcelamento" = "2"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número do pedido}", verificar campo "pc_qtde_parcelas" = "1"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número do pedido}", verificar campo "pc_valor_parcela" = "3440.00"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número do pedido}", verificar campo "vl_total_NF" = "3440.00"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número do pedido}", verificar campo "vl_total_familia" = "3394.94"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número do pedido}", verificar campo "vl_total_RA" = "45.06"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número do pedido}", verificar campo "C_pc_qtde" = "2"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número do pedido}", verificar campo "C_pc_valor" = "3440.00"
	And Tabela "t_PEDIDO_ITEM" registro com campo "pedido" = "{número do pedido}", verificar campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro com campo "pedido" = "{número do pedido}", verificar campo "preco_venda" = "1188.23"
	And Tabela "t_PEDIDO_ITEM" registro com campo "pedido" = "{número do pedido}", verificar campo "preco_NF" = "1200.00"
	And Tabela "t_PEDIDO_ITEM" registro com campo "pedido" = "{número do pedido}", verificar campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro com campo "pedido" = "{número do pedido}", verificar campo "preco_venda" = "509.24"
	And Tabela "t_PEDIDO_ITEM" registro com campo "pedido" = "{número do pedido}", verificar campo "preco_NF" = "520.00"

