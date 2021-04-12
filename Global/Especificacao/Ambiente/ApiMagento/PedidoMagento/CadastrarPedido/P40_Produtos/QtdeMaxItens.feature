@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: QtdeMaxItens
	

Scenario: QtdeMaxItens - excede qtde de itens com os produtos já separados
	#alerta=alerta & "O número de itens que está sendo cadastrado (" & CStr(n) & ") excede o máximo permitido por pedido (" & CStr(MAX_ITENS) & ")!!"
	Given Pedido base
	When Informo "appsettings.Loja" = "201"
	When Lista de itens com "13" itens
	#item 1
	When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001000"
	When Lista de itens "0" informo "Qtde" = "1"
	When Lista de itens "0" informo "Preco_Venda" = "338.85"
	When Lista de itens "0" informo "Preco_NF" = "340.00"
	#item 2
	When Lista de itens "1" informo "Fabricante" = "001"
	When Lista de itens "1" informo "Produto" = "001001"
	When Lista de itens "1" informo "Qtde" = "1"
	When Lista de itens "1" informo "Preco_Venda" = "790.63"
	When Lista de itens "1" informo "Preco_NF" = "792.63"
	#item 3
	When Lista de itens "2" informo "Fabricante" = "001"
	When Lista de itens "2" informo "Produto" = "001002"
	When Lista de itens "2" informo "Qtde" = "1"
	When Lista de itens "2" informo "Preco_Venda" = "370.75"
	When Lista de itens "2" informo "Preco_NF" = "372.75"
	#item 4
	When Lista de itens "3" informo "Fabricante" = "001"
	When Lista de itens "3" informo "Produto" = "001003"
	When Lista de itens "3" informo "Qtde" = "1"
	When Lista de itens "3" informo "Preco_Venda" = "825.22"
	When Lista de itens "3" informo "Preco_NF" = "827.22"
	#item 5
	When Lista de itens "4" informo "Fabricante" = "001"
	When Lista de itens "4" informo "Produto" = "001004"
	When Lista de itens "4" informo "Qtde" = "1"
	When Lista de itens "4" informo "Preco_Venda" = "384.44"
	When Lista de itens "4" informo "Preco_NF" = "386.44"
	#item 6
	When Lista de itens "5" informo "Fabricante" = "001"
	When Lista de itens "5" informo "Produto" = "001005"
	When Lista de itens "5" informo "Qtde" = "1"
	When Lista de itens "5" informo "Preco_Venda" = "897.03"
	When Lista de itens "5" informo "Preco_NF" = "899.03"
	#item 7
	When Lista de itens "6" informo "Fabricante" = "001"
	When Lista de itens "6" informo "Produto" = "001006"
	When Lista de itens "6" informo "Qtde" = "1"
	When Lista de itens "6" informo "Preco_Venda" = "469.91"
	When Lista de itens "6" informo "Preco_NF" = "471.91"
	#item 8
	When Lista de itens "7" informo "Fabricante" = "001"
	When Lista de itens "7" informo "Produto" = "001007"
	When Lista de itens "7" informo "Qtde" = "1"
	When Lista de itens "7" informo "Preco_Venda" = "954.05"
	When Lista de itens "7" informo "Preco_NF" = "956.05"
	#item 9
	When Lista de itens "8" informo "Fabricante" = "001"
	When Lista de itens "8" informo "Produto" = "001008"
	When Lista de itens "8" informo "Qtde" = "1"
	When Lista de itens "8" informo "Preco_Venda" = "595.30"
	When Lista de itens "8" informo "Preco_NF" = "597.30"
	#item 10
	When Lista de itens "9" informo "Fabricante" = "001"
	When Lista de itens "9" informo "Produto" = "001009"
	When Lista de itens "9" informo "Qtde" = "1"
	When Lista de itens "9" informo "Preco_Venda" = "1208.63"
	When Lista de itens "9" informo "Preco_NF" = "1210.63"
	#item 11
	When Lista de itens "12" informo "Fabricante" = "001"
	When Lista de itens "12" informo "Produto" = "001012"
	When Lista de itens "12" informo "Qtde" = "1"
	When Lista de itens "12" informo "Preco_Venda" = "833.54"
	When Lista de itens "12" informo "Preco_NF" = "835.54"
	#item 12
	When Lista de itens "11" informo "Fabricante" = "001"
	When Lista de itens "11" informo "Produto" = "001013"
	When Lista de itens "11" informo "Qtde" = "1"
	When Lista de itens "11" informo "Preco_Venda" = "1916.50"
	When Lista de itens "11" informo "Preco_NF" = "1918.50"
	#item 13
	When Lista de itens "10" informo "Fabricante" = "002"
	When Lista de itens "10" informo "Produto" = "002000"
	When Lista de itens "10" informo "Qtde" = "1"
	When Lista de itens "10" informo "Preco_Venda" = "581.04"
	When Lista de itens "10" informo "Preco_NF" = "583.04"
	#
	Then Erro "São permitidos no máximo 12 itens por pedido."

