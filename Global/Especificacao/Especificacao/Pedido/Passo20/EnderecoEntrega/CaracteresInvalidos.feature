@ignore
Feature: EnderecoEntrega CaracteresInvalidos

Background: pedido base
	Given pedido base com endereço de entrega
#loja/PedidoNovoConsiste.asp
#		if Not isTextoValido(EndEtg_endereco, s_caracteres_invalidos) then
#			alerta="O CAMPO 'ENDEREÇO DE ENTREGA' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#		elseif Not isTextoValido(EndEtg_endereco_numero, s_caracteres_invalidos) then
#			alerta="O CAMPO NÚMERO DO ENDEREÇO DE ENTREGA POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#		elseif Not isTextoValido(EndEtg_endereco_complemento, s_caracteres_invalidos) then
#			alerta="O CAMPO COMPLEMENTO DO ENDEREÇO DE ENTREGA POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#		elseif Not isTextoValido(EndEtg_bairro, s_caracteres_invalidos) then
#			alerta="O CAMPO BAIRRO DO ENDEREÇO DE ENTREGA POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#		elseif Not isTextoValido(EndEtg_cidade, s_caracteres_invalidos) then
#			alerta="O CAMPO CIDADE DO ENDEREÇO DE ENTREGA POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#		elseif Not isTextoValido(EndEtg_nome, s_caracteres_invalidos) then
#			alerta="O CAMPO NOME DO ENDEREÇO DE ENTREGA POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos

Scenario Outline: EndEtg
	Given pedido base com endereço de entrega
	When Informo <campo> = "Carater inválido: €"
	Then Erro regex <erro>
	Given pedido base com endereço de entrega
	When Informo <campo> = "Carater inválido: £"
	Then Erro regex <erro>
	Given pedido base com endereço de entrega
	When Informo <campo> = "Texto"
	Then Sem erro regex <erro>
	Examples:
		| campo                       | erro                                       |
		| EndEtg_endereco             | .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| EndEtg_endereco_numero      | .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| EndEtg_endereco_complemento | .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| EndEtg_bairro               | .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| EndEtg_cidade               | .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| EndEtg_nome                 | .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |

