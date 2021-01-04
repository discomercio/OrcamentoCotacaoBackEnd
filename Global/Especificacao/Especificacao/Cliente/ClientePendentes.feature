@ignore
Feature: ClientePendentes
#coisas pendentes no cliente:
#- passar o RefComercial e RefBancaria como null ou como vazio e ver se dá erro
#- nos telefones, os símbolos devem ser removidos
# verificar caracteres inválidos em 9 campos, veja loja/ClienteAtualiza.asp alerta="O CAMPO 'BAIRRO' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos

Scenario: Afazer ClientePendentes
	When Fazer esta validação

