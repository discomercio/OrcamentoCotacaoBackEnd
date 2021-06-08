@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: Frete
	Caso o campo Frete seja diferente de zero ou vazio, significa que teremos indicador para ser cadastrado no pedido.
	Se houver frete, deve-se automaticamente informar que o pedido possui RA e selecionar o indicador 'FRETE'

Background: Configuracoes
	Given Pedido base

Scenario: Frete - indicador não existe (ler do appsettings)
	Vamos alterar o indicador do appsettings para um indicador que não existe
	And Informo "frete" = "10"
	And Informo "appsettings.Indicador" = "um Indicador que não existe"
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Then Erro "O Indicador não existe!"

Scenario: Frete - indicador existe (ler do appsettings)
	Verificar se salvou onde deveria
	Given Reiniciar appsettings
	And Informo "frete" = "10"
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "indicador" = "FRETE"

Scenario: Frete - com indicador
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "001"
	When Lista de itens "0" informo "Preco_venda" = "610.58"
	When Informo "Frete" = "10.00"
	When Informo "appsettings.Loja" = "201"
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "indicador" = "FRETE"
	And Tabela "t_PEDIDO" registro criado, verificar campo "permite_RA_status" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "Opcao_Possui_RA" = "S"

Scenario: Frete - sem indicador
	When Informo "Frete" = "0"
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "indicador" = ""
	And Tabela "t_PEDIDO" registro criado, verificar campo "permite_RA_status" = "0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "Opcao_Possui_RA" = "-"

Scenario: Erro se o orçamentista não puder usar o frete
	Given Reiniciar banco ao terminar cenário
	Given Tabela "t_ORCAMENTISTA_E_INDICADOR" registro apelido = "frete", alterar campo "permite_RA_status" = "0"
	When Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456789"
	When Informo "Frete" = "10"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Then Erro "Indicador não tem permissão para usar RA"

Scenario: Frete - verifica se salvou em t_PEDIDO.magento_shipping_amount
	#O valor do frete no momento não estamos tratando no sistema, mas acho que podemos salvar a informação no campo t_PEDIDO.vl_frete
	#Por enquanto, todos os registros estão c/ esse valor zerado e o campo não é usado em nenhum lugar, mas já que temos
	#a possibilidade de salvar essa informação, creio que deveríamos gravar nesse campo
	#em 210316 reuniao semanal
	#campo apimagento.frete (colocar comentário no swagger: valor liquido do frete) e gravar em t_pedido.magento_shipping_amount
	#porque o frete está todo com 0, e pode ser que afete alguma coisa no sistema. COmo esse frete do magento é somente ifnormativo, decidimos guardar nesse campo
	When Informo "Frete" = "123"
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "Frete_valor" = "0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "magento_shipping_amount" = "123"