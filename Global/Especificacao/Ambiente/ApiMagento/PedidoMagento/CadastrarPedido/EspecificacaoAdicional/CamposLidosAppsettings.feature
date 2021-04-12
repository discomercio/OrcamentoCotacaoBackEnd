@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: CamposLidosAppsettings

Background: reiniciar appsettings
	Given Reiniciar appsettings

@ListaDependencias
Scenario: AdicionarDependencia
	Given AdicionarDependencia ambiente = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", especificacao = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional.CamposLidosAppsettings"
#analisar onde colocar esse testes de vendedor

Scenario: Vendedor = usuário que fez o login (ler do token)
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "vendedor" = "USRMAG"

@GerenciamentoBanco
Scenario: Vendedor diferente do token
	Given Reiniciar banco ao terminar cenário
	Given Pedido base
	And Tabela "t_USUARIO" apagar registro com campo usuario = "USRMAG"
	Then Erro "Usuário não encontrado."