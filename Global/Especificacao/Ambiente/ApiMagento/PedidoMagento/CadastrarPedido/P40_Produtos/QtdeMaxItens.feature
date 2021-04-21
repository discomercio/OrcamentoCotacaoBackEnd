@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
Feature: QtdeMaxItens

Scenario: QtdeMaxItens - excede qtde de itens com os produtos já separados
	#alerta=alerta & "O número de itens que está sendo cadastrado (" & CStr(n) & ") excede o máximo permitido por pedido (" & CStr(MAX_ITENS) & ")!!"
	Given Pedido base
	When Informo "appsettings.Loja" = "201"
	When Lista de itens com "13" itens
	#item 1
	When Lista de itens "0" informo "Sku" = "001000"
	When Lista de itens "0" informo "Quantidade" = "1"
	When Lista de itens "0" informo "Subtotal" = "340.00"
	When Lista de itens "0" informo "RowTotal" = "340.00"
	#item 2
	When Lista de itens "1" informo "Sku" = "001001"
	When Lista de itens "1" informo "Quantidade" = "1"
	When Lista de itens "1" informo "Subtotal" = "792.63"
	When Lista de itens "1" informo "RowTotal" = "792.63"
	#item 3
	When Lista de itens "2" informo "Sku" = "001002"
	When Lista de itens "2" informo "Quantidade" = "1"
	When Lista de itens "2" informo "Subtotal" = "372.75"
	When Lista de itens "2" informo "RowTotal" = "372.75"
	#item 4
	When Lista de itens "3" informo "Sku" = "001003"
	When Lista de itens "3" informo "Quantidade" = "1"
	When Lista de itens "3" informo "Subtotal" = "827.22"
	When Lista de itens "3" informo "RowTotal" = "827.22"
	#item 5
	When Lista de itens "4" informo "Sku" = "001004"
	When Lista de itens "4" informo "Quantidade" = "1"
	When Lista de itens "4" informo "Subtotal" = "386.44"
	When Lista de itens "4" informo "RowTotal" = "386.44"
	#item 6
	When Lista de itens "5" informo "Sku" = "001005"
	When Lista de itens "5" informo "Quantidade" = "1"
	When Lista de itens "5" informo "Subtotal" = "899.03"
	When Lista de itens "5" informo "RowTotal" = "899.03"
	#item 7
	When Lista de itens "6" informo "Sku" = "001006"
	When Lista de itens "6" informo "Quantidade" = "1"
	When Lista de itens "6" informo "Subtotal" = "471.91"
	When Lista de itens "6" informo "RowTotal" = "471.91"
	#item 8
	When Lista de itens "7" informo "Sku" = "001007"
	When Lista de itens "7" informo "Quantidade" = "1"
	When Lista de itens "7" informo "Subtotal" = "956.05"
	When Lista de itens "7" informo "RowTotal" = "956.05"
	#item 9
	When Lista de itens "8" informo "Sku" = "001008"
	When Lista de itens "8" informo "Quantidade" = "1"
	When Lista de itens "8" informo "Subtotal" = "597.30"
	When Lista de itens "8" informo "RowTotal" = "597.30"
	#item 10
	When Lista de itens "9" informo "Sku" = "001009"
	When Lista de itens "9" informo "Quantidade" = "1"
	When Lista de itens "9" informo "Subtotal" = "1210.63"
	When Lista de itens "9" informo "RowTotal" = "1210.63"
	#item 11
	When Lista de itens "12" informo "Sku" = "001012"
	When Lista de itens "12" informo "Quantidade" = "1"
	When Lista de itens "12" informo "Subtotal" = "835.54"
	When Lista de itens "12" informo "RowTotal" = "835.54"
	#item 12
	When Lista de itens "11" informo "Sku" = "001013"
	When Lista de itens "11" informo "Quantidade" = "1"
	When Lista de itens "11" informo "Subtotal" = "1918.50"
	When Lista de itens "11" informo "RowTotal" = "1918.50"
	#item 13
	When Lista de itens "10" informo "Sku" = "002000"
	When Lista de itens "10" informo "Quantidade" = "1"
	When Lista de itens "10" informo "Subtotal" = "583.04"
	When Lista de itens "10" informo "RowTotal" = "583.04"
	#
	Then Erro "São permitidos no máximo 12 itens por pedido."

@ignore
Scenario: QtdeMaxItens - excede qtde de itens com os produtos compostos
	Given Pedido base
	When Informo "appsettings.Loja" = "201"
	When Lista de itens com "13" itens
	#item 1
	When Lista de itens "0" informo "Sku" = "001090"
	When Lista de itens "0" informo "Quantidade" = "1"
	When Lista de itens "0" informo "Subtotal" = "ajustar valores"
	When Lista de itens "0" informo "RowTotal" = "ajustar valores"
	#item 2
	When Lista de itens "1" informo "Sku" = "001091"
	When Lista de itens "1" informo "Quantidade" = "1"
	When Lista de itens "1" informo "Subtotal" = "ajustar valores"
	When Lista de itens "1" informo "RowTotal" = "ajustar valores"
	#item 3
	When Lista de itens "2" informo "Sku" = "001092"
	When Lista de itens "2" informo "Quantidade" = "1"
	When Lista de itens "2" informo "Subtotal" = "ajustar valores"
	When Lista de itens "2" informo "RowTotal" = "ajustar valores"
	#item 4
	When Lista de itens "3" informo "Sku" = "001093"
	When Lista de itens "3" informo "Quantidade" = "1"
	When Lista de itens "3" informo "Subtotal" = "ajustar valores"
	When Lista de itens "3" informo "RowTotal" = "ajustar valores"
	#item 5
	When Lista de itens "4" informo "Sku" = "001094"
	When Lista de itens "4" informo "Quantidade" = "1"
	When Lista de itens "4" informo "Subtotal" = "ajustar valores"
	When Lista de itens "4" informo "RowTotal" = "ajustar valores"
	#item 6
	When Lista de itens "5" informo "Sku" = "001096"
	When Lista de itens "5" informo "Quantidade" = "1"
	When Lista de itens "5" informo "Subtotal" = "ajustar valores"
	When Lista de itens "5" informo "RowTotal" = "ajustar valores"
	#item 7
	When Lista de itens "6" informo "Sku" = "003030"
	When Lista de itens "6" informo "Quantidade" = "1"
	When Lista de itens "6" informo "Subtotal" = "ajustar valores"
	When Lista de itens "6" informo "RowTotal" = "ajustar valores"
	#item 8
	When Lista de itens "7" informo "Sku" = "003031"
	When Lista de itens "7" informo "Quantidade" = "1"
	When Lista de itens "7" informo "Subtotal" = "ajustar valores"
	When Lista de itens "7" informo "RowTotal" = "ajustar valores"
	#
	Then Erro "São permitidos no máximo 12 itens por pedido."