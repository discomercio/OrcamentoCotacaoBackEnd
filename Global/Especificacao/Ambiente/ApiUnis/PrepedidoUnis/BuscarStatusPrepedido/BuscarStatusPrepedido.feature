@ignore
@Ambiente.ApiUnis.PrepedidoUnis.BuscarStatusPrepedido.BuscarStatusPrepedido
Feature: ListarStatusPrepedido

Scenario: Prepedido não existe
    Given Informo "orcamento" = "nao existe"
	Then Erro status code "204"

Scenario: Cria um prepedido
    Given Prepedido base
	And Criar prepedido
	Then Resposta "St_orc_virou_pedido" = "false"

Scenario: Prepedido que virou pedido
    Given Prepedido base
	And Criar prepedido
	And Alterar prepedido criado, informo "St_Orc_Virou_Pedido" = "true"
	And Resposta "St_orc_virou_pedido" = "true"

