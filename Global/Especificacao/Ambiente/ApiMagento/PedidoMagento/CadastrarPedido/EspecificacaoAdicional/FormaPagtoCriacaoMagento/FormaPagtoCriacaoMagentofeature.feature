@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
@GerenciamentoBanco
Feature: FormaPagtoCriacaoMagentofeature
	Estamos testando com o "Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"

Background:
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "1018.48"
	When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "019"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "123"
	When Lista de itens com "1" itens
	When Lista de itens "0" informo "Fabricante" = "001"
	When Lista de itens "0" informo "Produto" = "001000"
	When Lista de itens "0" informo "Qtde" = "2"
	When Lista de itens "0" informo "Preco_Venda" = "509.24"
	When Lista de itens "0" informo "Preco_NF" = "520.00"
	When Informo "VlTotalDestePedido" = "1018.48"
	Given Reiniciar banco ao terminar cenário

Scenario: Verificar se o produto não existe na t_PRODUTO_LOJA
	Given Limpar tabela "t_PRODUTO_LOJA"
	Then Erro "regex .*Produto não cadastrado para a loja. Produto:*"

Scenario: Tipo_Parcelamento - parcelado única - t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR
	Given Limpar tabela "t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR"
	Then Erro "Coeficiente não cadastrado para o fabricante. Fabricante: 001, TipoParcela: SE"


Scenario: Tipo_Parcelamento - não existe
Given Pedido base
When Informo "Tipo_Parcelamento" = "12"
Then Erro "regex .*Coeficiente não cadastrado para o fabricante.*"
