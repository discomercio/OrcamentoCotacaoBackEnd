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

Scenario: Loja = "201" (vamos ler do appsettings)
	Given Pedido base
	And Informo "appsettings.Loja" = "201"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "loja" = "201"

@ignore
Scenario: Loja diferente de "201"
	#Esse teste retorna a mensagem "Produto não cadastrado para a loja. Produto: 00322"
	#talvez incluir uma validação para magento
	Given Pedido base
	And Informo "appsettings.Loja" = "001"
	Then Erro "pegar erro"

Scenario: Vendedor = usuário que fez o login (ler do token)
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "vendedor" = "USUARIOAPIMAGENTO"

@GerenciamentoBanco
Scenario: Vendedor diferente do token
	Given Reiniciar banco ao terminar cenário
	Given Pedido base
	And Tabela "t_USUARIO" apagar registro com campo "usuario" = "USUARIOAPIMAGENTO"
	Then Erro "Usuário não encontrado."