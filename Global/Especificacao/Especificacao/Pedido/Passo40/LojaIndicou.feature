@ignore
Feature: LojaIndicou

Background: Configurar lojas para teste
	set r = cn.Execute("SELECT * FROM t_LOJA WHERE (comissao_indicacao > 0) ORDER BY CONVERT(smallint,loja)")
	Given Limpar tabela "t_LOJA"

	And Novo registro em "t_LOJA"
	And Novo registro "loja" = "100"
	And Novo registro "comissao_indicacao" = "0"
	And Gravar registro 

	And Novo registro em "t_LOJA"
	And Novo registro "loja" = "200"
	And Novo registro "comissao_indicacao" = "1"
	And Gravar registro 

	And reinicializar banco após teste

Scenario: Seomnte pode existir se for vendedor externo
	Given Pedido base
	When Usuario logado "vendedor_externo" = "0"
	And Informo "loja_indicou" = "xx"
	Then Erro "loja_indicou permitidio somente para vendedores externos"

	Given Pedido base
	When Usuario logado "vendedor_externo" = "0"
	And Informo "loja_indicou" = "200"
	Then Erro "loja_indicou permitidio somente para vendedores externos"

Scenario: Vendedor externo
	Given Pedido base
	When Usuario logado "vendedor_externo" = "1"
	And Informo "loja_indicou" = "xx"
	Then Erro "loja_indicou não existe"

	Given Pedido base
	When Usuario logado "vendedor_externo" = "1"
	And Informo "loja_indicou" = "100"
	Then Erro "loja_indicou não existe"

	Given Pedido base
	When Usuario logado "vendedor_externo" = "1"
	And Informo "loja_indicou" = "200"
	Then Sem erro

Scenario: Vendedor externo sem loja
	Given Pedido base
	When Usuario logado "vendedor_externo" = "1"
	And Informo "loja_indicou" = ""
	Then Sem erro

