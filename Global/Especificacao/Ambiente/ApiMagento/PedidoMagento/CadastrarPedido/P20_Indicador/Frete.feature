@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
Feature: Frete
	Caso o campo Frete seja diferente de zero ou vazio, significa que teremos indicador para ser cadastrado no pedido.
	Se houver frete, deve-se automaticamente informar que o pedido possui RA e selecionar o indicador 'FRETE'

Background: Configuracoes
	Given Pedido base

Scenario: Frete - indicador não existe (ler do appsettings)
	Vamos alterar o indicador do appsettings para um indicador que não existe
	And Informo "frete" = "10"
	And Informo "appsettings.Indicador" = "um Indicador que não existe"
	Then Erro "O Indicador não existe!"

Scenario: Frete - indicador existe (ler do appsettings)
	Verificar se salvou onde deveria
	Given Reiniciar appsettings
	And Informo "frete" = "10"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "indicador" = "FRETE"

Scenario: Frete - com indicador
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "001"
	When Lista de itens "0" informo "Preco_venda" = "610.58"
	When Informo "Frete" = "10.00"
	When Informo "appsettings.Loja" = "201"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "indicador" = "FRETE"
	And Tabela "t_PEDIDO" registro criado, verificar campo "permite_RA_status" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "Opcao_Possui_RA" = "S"

Scenario: Frete - sem indicador
	When Informo "Frete" = "0"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "indicador" = ""
	And Tabela "t_PEDIDO" registro criado, verificar campo "permite_RA_status" = "0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "Opcao_Possui_RA" = "-"

Scenario: Frete - verifica se salvou em t_PEDIDO.magento_shipping_amount
	#O valor do frete no momento não estamos tratando no sistema, mas acho que podemos salvar a informação no campo t_PEDIDO.vl_frete
	#Por enquanto, todos os registros estão c/ esse valor zerado e o campo não é usado em nenhum lugar, mas já que temos
	#a possibilidade de salvar essa informação, creio que deveríamos gravar nesse campo
	#em 210316 reuniao semanal
	#campo apimagento.frete (colocar comentário no swagger: valor liquido do frete) e gravar em t_pedido.magento_shipping_amount
	#porque o frete está todo com 0, e pode ser que afete alguma coisa no sistema. COmo esse frete do magento é somente ifnormativo, decidimos guardar nesse campo
	When Informo "Frete" = "123"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "Frete_valor" = "0"
	And Tabela "t_PEDIDO" registro criado, verificar campo "magento_shipping_amount" = "123"