@ignore
@Especificacao.Pedido.Passo30
Feature: Validações do opcao_possui_RA
#na verdade, o campo opcao_possui_RA não é informado; ele é calculado a partir dos valores
#o campo PermiteRAStatus é informado, e consistido

#sem indicador
Scenario: RA
	When Pedido base
	And Alterar pedido colocar RA de 1 real
	And Informo "indicador" = ""
	Then Erro "Necessário indicador para usar RA"

#com indicador
Scenario: PermiteRAStatus inconsistente
	When Pedido base
	And Informo "PermiteRAStatus" = "0"
	And Informo "indicador" = "um indicador que pode RA"
	#neste caso, aceitamos salvar
	Then Sem nenhum erro

Scenario: PermiteRAStatus 
	When Pedido base
	And Alterar pedido colocar RA de 1 real
	And Informo "PermiteRAStatus" = "0"
	And Informo "indicador" = "um indicador que pode RA"
	Then Erro "Pedido está usando RA mas está inconsistente com PermiteRAStatus."

Scenario: indicador sem RA
	When Pedido base
	And Alterar pedido colocar RA de 1 real
	And Informo "PermiteRAStatus" = "1"
	And Informo "indicador" = "um indicador que nao pode RA"
	Then Erro "Indicador não tem permissão para usar RA"

Scenario: indicador sem RA 2
	When Pedido base
	And Alterar pedido colocar RA de 1 real
	And Informo "PermiteRAStatus" = "1"
	And Informo "indicador" = "um indicador que nao pode RA"
	Then Erro "Indicador não tem permissão para usar RA"

Scenario: indicador sem RA 3
	When Pedido base
	#o pedido base não tem RA
	And Informo "PermiteRAStatus" = "0"
	Then Sem erro
	#PedidoNovoConfirma.asp rs("opcao_possui_RA") = "-" ' Não se aplica
	And No pedido gravado, campo "opcao_possui_RA" = "-"

