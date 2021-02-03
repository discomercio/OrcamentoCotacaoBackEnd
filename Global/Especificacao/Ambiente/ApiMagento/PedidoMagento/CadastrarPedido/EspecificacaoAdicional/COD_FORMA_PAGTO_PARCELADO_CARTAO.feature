@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
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
@ignore
Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO
	#ao debugar o teste ele passa 1 vez e valida os 2 itens, depois ele segue e valida novamente e gera erro
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"
	When Informo "FormaPagtoCriacao.C_pc_qtde" = "1"
	When Informo "FormaPagtoCriacao.C_pc_valor" = "3440.00"
	When Informo "Frete" = "10"
	When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001000"
	When Lista de itens "0" informo "Qtde" = "2"
	When Lista de itens "0" informo "Preco_Venda" = "509.24" 
	When Lista de itens "0" informo "Preco_NF" = "520.00"
	When Lista de itens "1" informo "Fabricante" = "001"
	When Lista de itens "1" informo "Produto" = "001001"
	When Lista de itens "1" informo "Qtde" = "2"
	When Lista de itens "1" informo "Preco_Venda" = "1188.23"
	When Lista de itens "1" informo "Preco_NF" = "1200.00"
	When Informo "VlTotalDestePedido" = "3394.94" 
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "2"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_qtde_parcelas" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_valor_parcela" = "3440.00"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vl_total_NF" = "3440.00"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vl_total_familia" = "3394.94"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vl_total_RA" = "45.06"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar campo "preco_venda" = "509.24"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar campo "preco_NF" = "520.00"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar campo "preco_venda" = "1188.23"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar campo "preco_NF" = "1200.00"

