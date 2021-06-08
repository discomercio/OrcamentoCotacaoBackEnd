@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: Validacao
	Todos os testes estão gravando um novo cliente.

Background: Configuracoes
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_CLIENTE"
	Given Pedido base
	And Limpar dados cadastrais e endereço de entrega
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_nome" = "Vivian"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"

Scenario: Validacao - CEP vs endereco
	a api do magento faz a validação do CEP, então testamos pelo IGBE:
	When Informo "EndEtg_cidade" = "Cidade não no IBGE"
	Then Erro "Município 'Cidade não no IBGE' não consta na relação de municípios do IBGE para a UF de 'SP'!"

Scenario: Validacao - tamanho endereço
	When Informo "EndEtg_endereco" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "ENDEREÇO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Validacao - tamanho endereço 2
	#                                          10        20        30       40         50        60
	When Informo "EndEtg_endereco" = "12345678901234567890123456789012345678901234567890123456789"
	Then Sem erro "ENDEREÇO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Validacao - tamanho número
	When Informo "EndEtg_endereco_numero" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "NÚMERO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Validacao - tamanho número 2
	#                                          10        20        30       40         50        60
	When Informo "EndEtg_endereco_numero" = "12345678901234567890123456789012345678901234567890123456789"
	Then Sem erro "NÚMERO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Validacao - tamanho complemento 2
	#                                                      10        20        30        40        50        60
	When Informo "EndEtg_endereco_complemento" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Sem erro "COMPLEMENTO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_nome" = "Vivian"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"
	#                                                      10        20        30        40        50        60
	When Informo "EndEtg_endereco_complemento" = "12345678901234567890123456789012345678901234567890123456789"
	Then Sem erro "COMPLEMENTO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Validacao - tamanho bairro
	When Informo "EndEtg_bairro" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "BAIRRO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Validacao - tamanho bairro 2
	#                                          10        20        30       40         50        60
	When Informo "EndEtg_bairro" = "12345678901234567890123456789012345678901234567890123456789"
	Then Sem erro "BAIRRO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Validacao - tamanho cidade
	When Informo "EndEtg_cidade" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "CIDADE EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Validacao - tamanho cidade 2
	#                                          10        20        30       40         50        60
	When Informo "EndEtg_cidade" = "12345678901234567890123456789012345678901234567890123456789"
	Then Sem erro "CIDADE EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"
	#vamos gavar na t_cliente
	Given Pedido base
	And Limpar dados cadastrais e endereço de entrega
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_nome" = "Vivian"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"
	Then Sem nenhum erro
	#e agora verificamos que está limitando o tamanho mesmo que o cliente já esteja cadastrado
	Given Pedido base
	And Limpar dados cadastrais e endereço de entrega
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_nome" = "Vivian"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"
	#                                        10        20        30       40         50        60
	When Informo "EndEtg_cidade" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "CIDADE EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"