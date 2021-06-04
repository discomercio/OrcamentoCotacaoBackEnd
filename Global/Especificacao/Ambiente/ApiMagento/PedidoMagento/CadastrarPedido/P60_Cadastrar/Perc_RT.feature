@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
Feature: Perc_RT
	
perc_rt: Lemos da T_CODIGO_DESCRICAO, procurar PedidoECommerce_Origem_Grupo. temos que ler conforme a origem e ler o valor percentual.
ver PedidoNovoConfirma.asp '	OBTÉM O PERCENTUAL DE COMISSÃO DO MARKETPLACE
@ignore
Scenario: validação de Perc_RT
	Given Pedido base
	When Informo "Tipo_Parcelamento" = "COD_FORMA_PAGTO_PARCELA_UNICA"
	When Informo "C_pu_valor" = "3132.90"
	When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "010"
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "555"
	And Informo "Frete" = "10.00"
	When Lista de itens "0" informo "Preco_venda" = "610.58"
	When Informo "appsettings.Loja" = "201"
	And informar na T_CODIGO_DESCRICAO o perc_rt de 78.12
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "perc_RT" = "78.12"