@Especificacao.Pedido.Passo40
Feature: custoFinancFornecCoeficiente

#validar os campos
#PedidoProdutoPedidoDados
#        public decimal CustoFinancFornecPrecoListaBase { get; set; }
#        public decimal Preco_NF { get; set; }
#        public decimal Preco_Lista { get; set; }
#        public float? Desc_Dado { get; set; }
#        public decimal Preco_Venda { get; set; }
#        public decimal TotalItem { get; set; }
#        public decimal? TotalItemRA { get; set; }
#        public float? Comissao { get; set; }
#        public short? Qtde_estoque_total_disponivel { get; set; }
#        public float CustoFinancFornecCoeficiente { get; set; }
Background:
	#No magento não enviamos o coeficiente do produto
	#Implementado em \ApiMagento\PedidoMagento\CadastrarPedido\EspecificacaoAdicional\FormaPagtoCriacaoMagento\FormaPagtoCriacaoMagentofeature.feature
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

Scenario: custoFinancFornecCoeficiente
	Given Pedido base
	When Lista de itens "0" informo "CustoFinancFornecCoeficiente" = "123"
	Then Erro "Coeficiente do fabricante (003) está incorreto!"

Scenario: custoFinancFornecCoeficiente - a vista
	Given Pedido base
	When Informo "FormaPagtoCriacao.Tipo_parcelamento" = "1"
	When Informo "FormaPagtoCriacao.Op_av_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.Rb_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "AV"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "0"
	When Lista de itens com "1" itens
	When Lista de itens "0" informo "Qtde" = "1"
	When Lista de itens "0" informo "Preco_Venda" = "626.58"
	When Lista de itens "0" informo "Preco_NF" = "626.58"
	When Lista de itens "0" informo "Preco_Lista" = "626.58"
	When Lista de itens "0" informo "Preco_NF" = "626.58"
	When Lista de itens "0" informo "CustoFinancFornecCoeficiente" = "0"
	When Informo "ValorTotalDestePedidoComRA" = "626.58"
	When Informo "VlTotalDestePedido" = "626.58"
	Then Erro "Coeficiente do fabricante (003) está incorreto!"