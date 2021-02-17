@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: EspecificacaoMagento
Definições da ApiMagento
Paradigma de salvamento: fazer o mesmo que acontece com o processo semi-automático.
Se o semi-automático der erro, damos erro. Se aceitar, aceitamos.
Estoque: não é um problema. 

Scenario: preço: aceitamos o valor que vier do magento.
	#nao fazemos nenhuma crítica nos preços que chegam do magento
	Given Pedido base
	When Informo "Frete" = "10"
	When Informo "VlTotalDestePedido" = "3200.00"
	When Lista de itens "0" informo "Preco_Venda" = "650.00"
	When Lista de itens "0" informo "Preco_NF" = "800.00"
	When Lista de itens "1" informo "Preco_Venda" = "950.00"
	When Lista de itens "1" informo "Preco_NF" = "1100.00"
	Then Sem nenhum erro

Scenario: produtos: sempre virão divididos, nunca vai vir um produto composto.
	Given Pedido base
	When Informo "VlTotalDestePedido" = "1648.00"
	When Informo "appsettings.Loja" = "201"
	When Lista de itens com "1" itens
	When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001090"
	When Lista de itens "0" informo "Preco_Venda" = "1648.00"
	When Lista de itens "0" informo "Preco_NF" = "1648.00"
	Then Erro "Produto não cadastrado para a loja. Produto: 001090, loja: 201"

Scenario: Produtos e quantidades devem existir
	#if (b) {
	#	ha_item=true;
	#	if (trim(f.c_produto[i].value)=="") {
	#		alert("Informe o código do produto!!");
	#		f.c_produto[i].focus();
	#		return;
	#		}
	#	if (trim(f.c_qtde[i].value)=="") {
	#		alert("Informe a quantidade!!");
	#		f.c_qtde[i].focus();
	#		return;
	#		}
	#	if (parseInt(f.c_qtde[i].value)<=0) {
	#		alert("Quantidade inválida!!");
	#		f.c_qtde[i].focus();
	#		return;
	#		}
	#	}
	#}
	Given Pedido base
	When Lista de itens "0" informo "Produto" = ""
	Then Erro "regex .*Produto não cadastrado para a loja. Produto:*"
	#Given Pedido base
	#When Lista de itens "0" informo "Qtde" = ""
	#Then Erro "Informe a quantidade!!"
	Given Pedido base
	When Lista de itens "0" informo "Qtde" = "0"
	Then Erro "regex .*com Qtde menor ou igual a zero!*"
	Given Pedido base
	When Lista de itens "0" informo "Qtde" = "-1"
	Then Erro "regex .*com Qtde menor ou igual a zero!*"