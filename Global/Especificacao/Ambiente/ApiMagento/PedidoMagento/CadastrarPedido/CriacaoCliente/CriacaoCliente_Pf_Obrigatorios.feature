﻿@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: Magento CriacaoCliente_Pf_Obrigatorios

#Testando os campos obrigatorios e o caminho feliz
#Campos do endereço de entrega:
#        OutroEndereco
#        [Required] string EndEtg_endereco
#        [Required] string EndEtg_endereco_numero
#        [Required] string EndEtg_bairro
#        [Required] string EndEtg_cidade
#        [Required] string EndEtg_uf
#        [Required] string EndEtg_cep
#        string? EndEtg_endereco_complemento
#        string? EndEtg_email
#        string? EndEtg_email_xml
#        string? EndEtg_ddd_res
#        string? EndEtg_tel_res
#        string? EndEtg_ddd_com
#        string? EndEtg_tel_com
#        string? EndEtg_ramal_com
#        string? EndEtg_ddd_cel
#        string? EndEtg_tel_cel
#        string? EndEtg_ddd_com_2
#        string? EndEtg_tel_com_2
#        string? EndEtg_ramal_com_2
#        [Required] string EndEtg_nome
#        [Required] string EndEtg_tipo_pessoa
#        [Required] string EndEtg_cnpj_cpf
#        string? PontoReferencia
Background: Acertar banco de dados
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_CLIENTE"

@ListaDependencias
Scenario: AdicionarDependencia
	Given AdicionarDependencia ambiente = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", especificacao = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente.CriacaoCliente_Pf_Obrigatorios"

Scenario: Caminho feliz
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
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "endereco" = "Rua Professor Fábio Fanucchi"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "endereco_numero" = "1"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "bairro" = "Jardim São Paulo(Zona Norte)"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "cidade" = "São Paulo"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "uf" = "SP"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "cep" = "02045080"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "nome" = "Vivian"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "tipo" = "PF"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "cnpj_cpf" = "29756194804"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_NAO"

Scenario: Obrigatório 1
	Given Pedido base
	And Limpar dados cadastrais e endereço de entrega
	When Informo "OutroEndereco" = "true"
	#When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
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
	Then Erro "PREENCHA O ENDEREÇO."

Scenario: Obrigatório 2
	Given Pedido base
	And Limpar dados cadastrais e endereço de entrega
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	#When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_nome" = "Vivian"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"
	Then Erro "PREENCHA O NÚMERO DO ENDEREÇO."

Scenario: Obrigatório 3
	Given Pedido base
	And Limpar dados cadastrais e endereço de entrega
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	#	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_nome" = "Vivian"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"
	Then Erro "PREENCHA O BAIRRO."

Scenario: Obrigatório 4
	Given Pedido base
	And Limpar dados cadastrais e endereço de entrega
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	#	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_nome" = "Vivian"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"
	Then Erro "PREENCHA A CIDADE."

Scenario: Obrigatório 6
	Given Pedido base
	And Limpar dados cadastrais e endereço de entrega
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = ""
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_nome" = "Vivian"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"
	Then Erro "INFORME O UF."

Scenario: Obrigatório 7
	Given Pedido base
	And Limpar dados cadastrais e endereço de entrega
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = ""
	When Informo "EndEtg_nome" = "Vivian"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"
	Then Erro "INFORME O CEP."

Scenario: Obrigatório 8
	Given Pedido base
	And Limpar endereço de entrega
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	#	When Informo "EndEtg_nome" = "Vivian"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"
	Then Erro "PREENCHA O NOME DO CLIENTE."

Scenario: Obrigatório 9
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
	When Informo "EndEtg_tipo_pessoa" = ""
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = ""
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"
	Then Erro "A API Magento somente aceita pedidos para PF (EnderecoCadastralCliente.Endereco_tipo_pessoa)."

Scenario: Obrigatório 10
	Given Pedido base
	And Limpar endereço de entrega
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
	When Informo "EndEtg_cnpj_cpf" = ""
	When Informo "cnpj_cpf" = ""
	Then Erro "CPF NÃO FORNECIDO."

Scenario: validar cep contra endereço
	Given Pedido base
	And Limpar dados cadastrais e endereço de entrega
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_cidade" = "Santos"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_nome" = "Vivian"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"
	Then Erro "Cidade não confere"
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
	When Informo "EndEtg_ddd_res" = "11"
	When Informo "EndEtg_tel_res" = "1234-5678"
	Then Sem nenhum erro

Scenario: Endereço de entrega - tamanho endereço
	Given Pedido base
	When Informo "EndEtg_endereco" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "ENDEREÇO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

Scenario: Endereço de entrega - tamanho endereço 2
	Given Pedido base
	#                                          10        20        30       40         50        60
	When Informo "EndEtg_endereco" = "12345678901234567890123456789012345678901234567890123456789"
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

Scenario: Endereço de entrega - tamanho complemento
	Given Pedido base
	When Informo "EndEtg_endereco_complemento" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "COMPLEMENTO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: 61 CARACTERES<br>TAMANHO MÁXIMO: 60 CARACTERES"

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