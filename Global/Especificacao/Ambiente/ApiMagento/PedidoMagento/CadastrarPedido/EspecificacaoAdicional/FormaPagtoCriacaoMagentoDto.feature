@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
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
Scenario: Tipo_Parcelamento - á vista
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "av_forma_pagto" = "6"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_valor" = "0.0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_valor_parcela" = "0.0"

#afazer: precisa fazer alteração em todas as aplicações para passar o sistemaResponsavel
# Parcela única é validado apenas se o cliente for PJ, no caso do magento não faremos pedido para PJ mas,
# aceitamos Parcela única para PF
# isso ocorre em FormaPagoBll.ObterFormaPagto
@ignore
Scenario: Tipo_Parcelamento - parcela única
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "5"
	When Informo "C_pu_valor" = "3132.90"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "5"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_forma_pagto" = "2"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_vencto_apos" = "30"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_valor" = "3132.90"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_qtde_parcelas" = "0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_valor_parcela" = "0.0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "av_forma_pagto" = "0"

@ignore
Scenario: Tipo_Parcelamento - parcelado cartão
	#afazer - necessário cria o calculo de desconto para quando confrontar os dados o valor de preco_lista esteja correto
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "2"
	When Informo "C_pc_qtde" = "1"
	When Informo "C_pc_valor" = "3132.90"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "tipo_parcelamento" = "2"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_forma_pagto" = "0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "av_forma_pagto" = "0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pu_valor" = "0.0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_qtde_parcelas" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "pc_valor_parcela" = "3132.90"