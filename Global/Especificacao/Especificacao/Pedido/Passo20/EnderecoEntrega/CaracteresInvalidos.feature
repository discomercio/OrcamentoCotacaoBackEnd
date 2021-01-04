@Especificacao.Pedido.Passo20.EnderecoEntrega
Feature: EnderecoEntrega CaracteresInvalidos

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

#orcamento/OrcamentoNovoConsiste.asp
#if alerta = "" then
#	if Not isTextoValido(orcamento_endereco_nome, s_caracteres_invalidos) then
#		alerta="DADOS CADASTRAIS: O CAMPO 'NOME' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#	elseif Not isTextoValido(orcamento_endereco_logradouro, s_caracteres_invalidos) then
#		alerta="DADOS CADASTRAIS: O CAMPO 'ENDEREÇO' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#	elseif Not isTextoValido(orcamento_endereco_numero, s_caracteres_invalidos) then
#		alerta="DADOS CADASTRAIS: O CAMPO NÚMERO DO ENDEREÇO POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#	elseif Not isTextoValido(orcamento_endereco_complemento, s_caracteres_invalidos) then
#		alerta="DADOS CADASTRAIS: O CAMPO 'COMPLEMENTO' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#	elseif Not isTextoValido(orcamento_endereco_bairro, s_caracteres_invalidos) then
#		alerta="DADOS CADASTRAIS: O CAMPO 'BAIRRO' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#	elseif Not isTextoValido(orcamento_endereco_cidade, s_caracteres_invalidos) then
#		alerta="DADOS CADASTRAIS: O CAMPO 'CIDADE' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#	elseif Not isTextoValido(orcamento_endereco_contato, s_caracteres_invalidos) then
#		alerta="DADOS CADASTRAIS: O CAMPO 'CONTATO' POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " & s_caracteres_invalidos
#		end if
#	end if

Scenario Outline: CaracteresInvalidos
	Given Pedido base com endereço de entrega
	When Informo "<campo>" = "Carater inválido: €"
	Then Erro "<erro>"
	Given Pedido base com endereço de entrega
	When Informo "<campo>" = "Carater inválido: £"
	Then Erro "<erro>"
	Given Pedido base com endereço de entrega
	When Informo "<campo>" = "Texto"
	Then Sem erro "<erro>"
	Examples:
		| campo                       | erro                                             |
		| EndEtg_endereco             | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| EndEtg_endereco_numero      | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| EndEtg_endereco_complemento | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| EndEtg_bairro               | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| EndEtg_cidade               | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| EndEtg_nome                 | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |

Scenario Outline: CaracteresInvalidos fora API Magento
	#A API Magento não tem o campo Endereco_rg
	#sobre os outros, o processo dela é diferente: o endereço de entrega é copiado para o endereço cadastral, então o erro não aparece
	#por que copia? veja Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\FluxoCadastroPedidoMagento_PF.feature
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	When Informo "<campo>" = "Carater inválido: €"
	Then Erro "<erro>"
	Given Pedido base
	When Informo "<campo>" = "Carater inválido: £"
	Then Erro "<erro>"
	Given Pedido base
	When Informo "<campo>" = "Texto"
	Then Sem erro "<erro>"
	Examples:
		| campo                | erro                                             |
		| Endereco_rg          | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| Endereco_nome        | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| Endereco_logradouro  | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| Endereco_numero      | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| Endereco_complemento | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| Endereco_bairro      | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| Endereco_cidade      | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| Endereco_contato     | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| Endereco_Email       | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |
		| Endereco_EmailXml    | regex .*POSSUI UM OU MAIS CARACTERES INVÁLIDOS.* |

