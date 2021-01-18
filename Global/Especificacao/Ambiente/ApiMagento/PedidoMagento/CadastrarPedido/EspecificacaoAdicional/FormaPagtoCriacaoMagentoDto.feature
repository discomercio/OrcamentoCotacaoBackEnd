@ignore
Feature: FormaPagtoCriacaoMagentoDto
#validar os campos:
#        /// Tipo_Parcelamento:
#        ///     COD_FORMA_PAGTO_A_VISTA = "1",
#        ///     COD_FORMA_PAGTO_PARCELADO_CARTAO = "2",
#        ///     COD_FORMA_PAGTO_PARCELA_UNICA = "5",
#        /// <hr />
#        /// </summary>
#        [Required]
#        public short Tipo_Parcelamento { get; set; }//Tipo da forma de pagto
#
#        public decimal? C_pu_valor { get; set; }
#
#        public int? C_pc_qtde { get; set; }
#        public decimal? C_pc_valor { get; set; }


#se tiver dados em outros campos esta errado
@mytag
Scenario: Tipo_Parcelamento - á vista
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "tipo_parcelamento" = "1"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "av_forma_pagto" = "6"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pu_valor" = "0.00"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pc_qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pc_valor_parcela" = "0.00"

Scenario: Tipo_Parcelamento - parcela única
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "3132.90"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "tipo_parcelamento" = "5"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pu_forma_pagto" = "2"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pu_vencto_apos" = "30"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pu_valor" = "3132.90"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pc_qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pc_valor_parcela" = "0.00"
		And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "av_forma_pagto" = "0"


Scenario: Tipo_Parcelamento - parcelado cartão
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "2"
	When Informo "C_pc_qtde" = "1"
	When Informo "C_pc_valor" = "3132.90"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "tipo_parcelamento" = "2"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pu_forma_pagto" = "0"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "av_forma_pagto" = "0"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pu_valor" = "0.00"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pc_qtde_parcelas" = "1"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "pc_valor_parcela" = "3132.90"
