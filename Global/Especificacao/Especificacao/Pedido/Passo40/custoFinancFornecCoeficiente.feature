@Especificacao.Pedido.PedidoFaltandoImplementarSteps
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


Scenario: custoFinancFornecCoeficiente
	Given Pedido base
	When Informo "PedidoProdutoPedidoDados.CustoFinancFornecCoeficiente" = "123"
	Then Erro "Coeficiente do fabricante (003) está incorreto!"

	Given Pedido base 
	When Informo "FormaPagtoCriacao.Tipo_parcelamento" = "1"
	When Informo "FormaPagtoCriacao.Op_av_forma_pagto" = "1"
	When Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "AV"
	When Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "0"
	When Informo "ListaProdutos[0].Preco_Venda" = "939.87"
	When Informo "ListaProdutos[0].Preco_NF" = "939.87"
	When Informo "ListaProdutos[0].Preco_Lista" = "939.87"
	When Informo "ListaProdutos[0].Preco_NF" = "939.87"
	When Informo "ListaProdutos[0].CustoFinancFornecCoeficiente" = "1"
	Then Erro "Coeficiente do fabricante (003) está incorreto!"


