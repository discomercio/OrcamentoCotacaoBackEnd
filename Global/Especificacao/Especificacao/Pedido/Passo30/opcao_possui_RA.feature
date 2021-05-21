@Especificacao.Pedido.PedidoFaltandoImplementarSteps
#@ignore
#@Especificacao.Pedido.Passo30
Feature: Validações do opcao_possui_RA
#na verdade, o campo opcao_possui_RA não é informado; ele é calculado a partir dos valores
#o campo PermiteRAStatus é informado, e consistido

#sem indicador
Scenario: RA
	Given Pedido base
	And Alterar pedido colocar RA de 1 real
	When Informo "indicador" = ""
	Then Erro "Necessário indicador para usar RA"

#com indicador
Scenario: PermiteRAStatus inconsistente
	Given Pedido base
	When Informo "PermiteRAStatus" = "0"
	When Informo "indicador" = "um indicador que pode RA"
	#neste caso, aceitamos salvar
	Then Sem nenhum erro

Scenario: PermiteRAStatus 
	Given Pedido base
	And Alterar pedido colocar RA de 1 real
	When Informo "PermiteRAStatus" = "0"
	When Informo "indicador" = "um indicador que pode RA"
	Then Erro "Pedido está usando RA mas está inconsistente com PermiteRAStatus."

Scenario: indicador sem RA
	Given Pedido base
	And Alterar pedido colocar RA de 1 real
	When Informo "PermiteRAStatus" = "1"
	When Informo "indicador" = "um indicador que nao pode RA"
	Then Erro "Indicador não tem permissão para usar RA"

Scenario: indicador sem RA 2
	Given Pedido base
	And Alterar pedido colocar RA de 1 real
	When Informo "PermiteRAStatus" = "1"
	When Informo "indicador" = "um indicador que nao pode RA"
	Then Erro "Indicador não tem permissão para usar RA"

Scenario: indicador sem RA 3
	Given Pedido base
	#o pedido base não tem RA
	When Informo "PermiteRAStatus" = "0"
	Then Sem nenhum erro
	#PedidoNovoConfirma.asp rs("opcao_possui_RA") = "-" ' Não se aplica
	Then Tabela "t_PEDIDO" registro pai criado, verificar campo "opcao_possui_RA" = "-"

