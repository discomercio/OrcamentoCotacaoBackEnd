@ignore
Feature: ClientePendentes

#coisas pendentes no cliente:
#- passar o RefComercial e RefBancaria como null ou como vazio e ver se dá erro
#- nos telefones, os símbolos devem ser removidos
# verificar caracteres inválidos em 9 campos, veja loja/ClienteAtualiza.asp alerta="O CAMPO 'BAIRRO' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
Scenario: Afazer ClientePendentes
	When Fazer esta validação

# cliente PF não tem referências, vou deixar aqui para ver se precisamos testar caso o cliente PF tenha ref's
 #afazer: montar referências para testar cliente PF
Scenario: Referencias PF 
	Given Pedido base cliente PF
	When Informo "RefComercial" = ""
	Then Erro "Se cliente tipo PF, não deve constar referência comercial!"

	Given Pedido base cliente PF
	When Informo "RefBancaria" =  ""
	Then Erro "Se cliente tipo PF, não deve constar referência bancária!"

Scenario: Referencias PJ
	Given Pedido base cliente PJ
	When Informo "RefComercial" = "null"
	Then Sem nenhum erro
	Given Pedido base cliente PJ
	When Informo "RefComercial" = ""
	Then Sem nenum erro
	Given Pedido base cliente PJ
	When Informo "RefBancaria" = "null"
	Then Sem nenhum erro
	Given Pedido base cliente PJ
	When Informo "RefBancaria" =  ""
	Then Sem nenhum erro

#Perguntas para o Edu
# O "Then" deve ser "Sem nenhum erro"?
# Temos que comparar o dados depois de salvar?
Scenario: telefones com simbolos
	Given Pedido base cliente PF
	When Informo "DadosCliente.DddResidencial" = "12"
	When Informo "DadosCliente.TelefoneResidencial" = "1234-5678"
	Then Erro "Telefone residencial com símbolo"
	Given Pedido base cliente PF
	When Informo "DadosCliente.DddCelular" = "12"
	When Informo "DadosCliente.Celular" = "98160-3313"
	Then Erro "Telefone celular com símbolo"
	Given Pedido base cliente PF
	When Informo "DadosCliente.DddComercial" = "12"
	When Informo "DadosCliente.TelComercial" = "1234-678"
	Then Erro "Telefone comercial com símbolo"
	Given Pedido base cliente PJ
	When Informo "DadosCliente.DddComercial2" = "12"
	When Informo "DadosCliente.TelComercial2" = "1234-5678"
	Then Erro "Telefone comercial com símbolo"

Scenario: CaracteresInvalidos
	Given Pedido base
	When Informo "DadosCliente.Nome" = "Caracter inválido: $"
	Then Erro "O CAMPO 'NOME' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: $"

	Given Pedido base
	When Informo "DadosCliente.Endereco" = "Caracter inválido: $"
	Then Erro "O CAMPO 'ENDEREÇO' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: $"

	Given Pedido base
	When Informo "DadosCliente.Numero" = "Caracter inválido: $"
	Then Erro "O CAMPO 'NÚMERO' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: $"

	Given Pedido base
	When Informo "DadosCliente.Complemento" = "Caracter inválido: $"
	Then Erro "O CAMPO 'COMPLEMENTO' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: $"

	Given Pedido base
	When Informo "DadosCliente.Bairro" = "Caracter inválido: $"
	Then Erro "O CAMPO 'BAIRRO' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: $"

	Given Pedido base
	When Informo "DadosCliente.Cidade" = "Caracter inválido: $"
	Then Erro "O CAMPO 'CIDADE' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: $"

	Given Pedido base
	When Informo "DadosCliente.Contato" = "Caracter inválido: $"	
	Then Erro "O CAMPO 'CONTATO' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: $"

	Given Pedido base
	When Informo "DadosCliente.Observacao_Filiacao" = "Caracter inválido: $"
	Then Erro "O CAMPO 'OBSERVAÇÕES' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: $"

	
	