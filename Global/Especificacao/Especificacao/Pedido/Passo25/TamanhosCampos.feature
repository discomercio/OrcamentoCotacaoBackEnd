#@Especificacao.Pedido.Passo20.EnderecoEntrega
@Especificacao.Pedido.PedidoFaltandoImplementarSteps
@GerenciamentoBanco
Feature: TamanhosCampos

#Testando os tamanhos máximos dos dados do endereço cadastral (endereço de cobrança)
#Campos:
#public string Endereco_logradouro { get; set; }
#public string Endereco_numero { get; set; }
#public string Endereco_complemento { get; set; }
#public string Endereco_bairro { get; set; }
#public string Endereco_cidade { get; set; }
#public string Endereco_uf { get; set; }
#public string Endereco_cep { get; set; }
#public string Endereco_email { get; set; }
#public string Endereco_email_xml { get; set; }
#public string Endereco_nome { get; set; }
#public string Endereco_ddd_res { get; set; }
#public string Endereco_tel_res { get; set; }
#public string Endereco_ddd_com { get; set; }
#public string Endereco_tel_com { get; set; }
#public string Endereco_ramal_com { get; set; }
#public string Endereco_ddd_cel { get; set; }
#public string Endereco_tel_cel { get; set; }
#public string Endereco_ddd_com_2 { get; set; }
#public string Endereco_tel_com_2 { get; set; }
#public string Endereco_ramal_com_2 { get; set; }
#public string Endereco_tipo_pessoa { get; set; }
#public string Endereco_cnpj_cpf { get; set; }
#public byte Endereco_contribuinte_icms_status { get; set; }
#public byte Endereco_produtor_rural_status { get; set; }
#public string Endereco_ie { get; set; }
#public string Endereco_rg { get; set; }
#public string Endereco_contato { get; set; }

Scenario: Caminho feliz
	#Colocamos todos os campos e verificamos que são gravados no pedido
	Given Pedido base
	#informar os campos acima, e não esses do endereço de entrega, que copiamos só apra marcar o que temos que fazer
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
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito" = "2"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco" = "Rua Professor Fábio Fanucchi"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "endereco_numero" = "1"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "bairro" = "Jardim São Paulo(Zona Norte)"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "cidade" = "São Paulo"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "uf" = "SP"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "cep" = "02045080"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "nome" = "Vivian"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "tipo" = "PF"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "cnpj_cpf" = "29756194804"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_NAO"


Scenario: Endereço de entrega - tamanho endereço
	Given Pedido base
	When Informo "Endereco_logradouro" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "ENDEREÇO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Endereço de entrega - tamanho endereço 2
	Given Pedido base
	#                                          10        20        30       40         50        60
	When Informo "Endereco_logradouro" = "12345678901234567890123456789012345678901234567890123456789"
	Then Sem erro "ENDEREÇO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Endereço de entrega - tamanho número
	Given Pedido base
	When Informo "EndEtg_endereco_numero" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "NÚMERO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Endereço de entrega - tamanho número 2
	Given Pedido base
	#                                          10        20        30       40         50        60
	When Informo "EndEtg_endereco_numero" = "12345678901234567890123456789012345678901234567890123456789"
	Then Sem erro "NÚMERO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Endereço de entrega - tamanho complemento 2
	Given Pedido base
	#                                          10        20        30       40         50        60
	When Informo "EndEtg_endereco_complemento" = "12345678901234567890123456789012345678901234567890123456789"
	Then Sem erro "COMPLEMENTO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Endereço de entrega - tamanho bairro
	Given Pedido base
	When Informo "EndEtg_bairro" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "BAIRRO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Endereço de entrega - tamanho bairro 2
	Given Pedido base
	#                                          10        20        30       40         50        60
	When Informo "EndEtg_bairro" = "12345678901234567890123456789012345678901234567890123456789"
	Then Sem erro "BAIRRO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Endereço de entrega - tamanho cidade
	Given Pedido base
	When Informo "EndEtg_cidade" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "CIDADE EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Endereço de entrega - tamanho cidade 2
	Given Pedido base
	#                                          10        20        30       40         50        60
	When Informo "EndEtg_cidade" = "12345678901234567890123456789012345678901234567890123456789"
	Then Sem erro "CIDADE EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	#                                        10        20        30       40         50        60
	When Informo "EndEtg_cidade" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "CIDADE EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"
