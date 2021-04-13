@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
Feature: ParceladoCartao
	Estamos testando o pagamento PARCELADO_CARTAO

Background:
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELADO_CARTAO"

Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO - verifica campos na tabela t_PEDIDO
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
	#When Informo "VlTotalDestePedido" = "3394.94"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "2"
	And Tabela "t_PEDIDO" registro criado, verificar campo "CustoFinancFornecTipoParcelamento" = "SE"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_qtde_parcelas" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_valor_parcela" = "3440.00"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vl_total_NF" = "3440.00"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vl_total_familia" = "3394.94"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vl_total_RA" = "45.06"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_venda" = "509.24"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_NF" = "520.00"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_venda" = "1188.23"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_NF" = "1200.00"

Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO - C_pc_valor errado
	When Informo "C_pc_qtde" = "1"
	When Informo "C_pc_valor" = "1"
	Then Erro "Valor total da forma de pagamento diferente do valor total!"

Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO - C_pc_valor negativo
	When Informo "C_pc_qtde" = "1"
	When Informo "C_pc_valor" = "-1"
	Then Erro "Valor de parcela inválido (parcelado no cartão [internet])."

Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO - C_pc_qtde zerada
	When Informo "C_pc_qtde" = "0"
	When Informo "C_pc_valor" = "3132.90"
	#When Informo "VlTotalDestePedido" = "3132.90"
	Then Erro "regex .*Coeficiente não cadastrado para o fabricante. Fabricante:*"

Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO - C_pc_qtde negativa
	When Informo "C_pc_qtde" = "-1"
	When Informo "C_pc_valor" = "3132.90"
	#When Informo "VlTotalDestePedido" = "3132.90"
	Then Erro "regex .*Coeficiente não cadastrado para o fabricante. Fabricante:*"

Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO - C_pc_qtde alta
	When Informo "C_pc_qtde" = "20"
	When Informo "C_pc_valor" = "3132.90"
	#When Informo "VlTotalDestePedido" = "3132.90"
	Then Erro "Coeficiente não cadastrado para o fabricante. Fabricante: 003, TipoParcela: SE"

#pergunta: se COD_FORMA_PAGTO_PARCELADO_CARTAO temos que usar os coeficientes do fabricante?
#respsota: sim, mas precisa manter o valor do preço e o total da nota igual. Vamos colocar essa diferença no Vl Lista
Scenario: COD_FORMA_PAGTO_PARCELADO_CARTAO - manter o mesmo valor
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
	#When Informo "VlTotalDestePedido" = "3394.94"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "2"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_qtde_parcelas" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_valor_parcela" = "3440.00"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vl_total_NF" = "3440.00"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vl_total_familia" = "3394.94"
	And Tabela "t_PEDIDO" registro criado, verificar campo "vl_total_RA" = "45.06"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_venda" = "509.24"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "1" campo "preco_NF" = "520.00"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "qtde" = "2"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_venda" = "1188.23"
	And Tabela "t_PEDIDO_ITEM" registro criado, verificar item "2" campo "preco_NF" = "1200.00"