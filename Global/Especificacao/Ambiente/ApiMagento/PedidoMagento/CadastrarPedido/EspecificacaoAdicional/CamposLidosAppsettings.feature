@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: CamposLidosAppsettings

@ListaDependencias
Scenario: AdicionarDependencia
	Given AdicionarDependencia ambiente = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", especificacao = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional.CamposLidosAppsettings"

Scenario: Orcamentista = "FRETE" (vamos ler do appsettings) - precisa existir
	#Vamos colocar um indicador no appsettings que não esteja cadastrado no banco e ver se ele testa
	Given Reiniciar appsettings
	And Pedido base
	And Informo "frete" = "10"
	And Informo "appsettings.Orcamentista" = "um orçamentista que não existe"
	Then Erro "O Orçamentista não existe!"

	Given Reiniciar appsettings
	And Pedido base
	Then Sem nenhum erro

@ignore
Scenario: Orcamentista = "FRETE" (vamos ler do appsettings)
	#vamos verificar se salvou onde deveria
	#vamos testar com o KONAR
	Given Pedido base
	And Informo "frete" = "10"
	Then Sem nenhum erro
	And No pedido gravado, verificar campo "indicador" = "FRETE"
	When Fazer esta validação

@ignore
Scenario: Loja = "201" (vamos ler do appsettings)
	When Fazer esta validação

@ignore
Scenario: Vendedor = usuário que fez o login (ler do token)
	When Fazer esta validação

