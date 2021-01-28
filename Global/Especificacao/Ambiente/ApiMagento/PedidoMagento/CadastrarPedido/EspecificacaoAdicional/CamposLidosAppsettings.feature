@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: CamposLidosAppsettings

@ListaDependencias
Scenario: AdicionarDependencia
	Given AdicionarDependencia ambiente = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", especificacao = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional.CamposLidosAppsettings"

Scenario: Indicador = "FRETE" (vamos ler do appsettings) - precisa existir
	#Vamos colocar um indicador no appsettings que não esteja cadastrado no banco e ver se ele testa
	Given Reiniciar appsettings
	And Pedido base
	And Informo "frete" = "10"
	And Informo "appsettings.Indicador" = "um Indicador que não existe"
	Then Erro "O Indicador não existe!"

	Given Reiniciar appsettings
	And Pedido base
	Then Sem nenhum erro

Scenario: Indicador = "FRETE" (vamos ler do appsettings)
	#vamos verificar se salvou onde deveria
	#vamos testar com o KONAR
	Given Pedido base
	And Informo "frete" = "10"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "indicador" = "FRETE"
	#No pedido gravado, verificar campo "indicador" = "FRETE"

@ignore
Scenario: Loja = "201" (vamos ler do appsettings)
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "loja" = "201" 
	And afazer: colocar uma loja inválida e verificar que é validado

@ignore
Scenario: Vendedor = usuário que fez o login (ler do token)
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro campo "Vendedor" = "UsuarioApiMagento" 
	And afazer: colocar um usuario inválido e verificar que é validado

