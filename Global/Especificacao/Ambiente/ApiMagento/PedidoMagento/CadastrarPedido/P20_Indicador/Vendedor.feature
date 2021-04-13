@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
@GerenciamentoBanco
Feature: Vendedor

Background: Configuracoes
	Given Reiniciar appsettings

Scenario: Vendedor = usuário que fez o login (ler do token)
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "vendedor" = "USRMAG"

Scenario: Vendedor diferente do token
	Given Reiniciar banco ao terminar cenário
	Given Pedido base
	And Tabela "t_USUARIO" apagar registro com campo usuario = "USRMAG"
	Then Erro "Usuário não encontrado."