@Ambiente.ApiUnis.PrepedidoUnis.BuscarStatusPrepedido.BuscarStatusPrepedido
Feature: BuscarStatusPrepedido

Scenario: Prepedido não existe
    Given Informo "orcamento" = "nao existe"
	Then Erro status code "204"

Scenario: Cria um prepedido
    Given Criar prepedido
    And Informo "orcamento" = "especial: prepedido criado"
	Then Resposta "St_orc_virou_pedido" = "false"

Scenario: Prepedido que virou pedido
    Given Criar prepedido
    And Informo "orcamento" = "especial: prepedido criado"
    #precisamos reiniciar pq editamos uns campos de maneira forçada e pode prejudicar outros testes
    And Reiniciar banco ao terminar cenário
	And Alterar prepedido criado, passar para pedido
	And Resposta "St_orc_virou_pedido" = "true"

