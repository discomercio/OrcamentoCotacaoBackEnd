@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: DadosPessoais

Background: Configuracoes
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_CLIENTE"
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
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	When Informo "cnpj_cpf" = "29756194804"

Scenario: DadosPessoais - diferente de PF
	When Informo "EndEtg_tipo_pessoa" = ""
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = ""
	Then Erro "A API Magento somente aceita pedidos para PF (EnderecoCadastralCliente.Endereco_tipo_pessoa)."
	When Informo "EndEtg_tipo_pessoa" = "PJ"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PJ"
	Then Erro "A API Magento somente aceita pedidos para PF (EnderecoCadastralCliente.Endereco_tipo_pessoa)."

Scenario: DadosPessoais - nome
	When Informo "EndEtg_nome" = ""
	Then Erro "PREENCHA O NOME DO CLIENTE."

Scenario: DadosPessoais - CPF
	When Informo "EndEtg_cnpj_cpf" = ""
	When Informo "cnpj_cpf" = ""
	Then Erro "CPF NÃO FORNECIDO."

Scenario: DadosPessoais - remover pontuações do CPF
	When Informo "EndEtg_cnpj_cpf" = "297.561.948-04"
	When Informo "cnpj_cpf" = "297.561.948-04"
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "cnpj_cpf" = "29756194804"

Scenario: DadosPessoais - Produtor Rural
	#Para cliente PF assumimos que Endereco_produtor_rural_status = 1 (COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "produtor_rural_status" = "1"

Scenario: DadosPessoais - Contribuinte ICMS e IE
	Para cliente PF assumimos que Endereco_contribuinte_icms_status = 0 (INICIAL)
	e IE = vazio
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "contribuinte_icms_status" = "0"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "ie" = ""

Scenario: DadosPessoais - cliente foi criado pelo magento (o sistema_responsavel_cadastro)
	Given Pedido base
	#só pra garantir que estmaos usando o CPF certo
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "cnpj_cpf" = "29756194804"
	Then Sem nenhum erro
	#COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO = 5   //API magento
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "sistema_responsavel_cadastro" = "5"

Scenario: DadosPessoais - sexo e data de nascimento em branco
	#Sexo: Retirar obrigatoriedade do preenchimento do sexo, permitindo deixá-lo vazio.
	#cadastrar o usuário pelo magento e verificar que sexo e nascimento estão em branco (sexo fica string vazia e nascimento fica NULL)
	Given Pedido base
	#só pra garantir que estmaos usando o CPF certo
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "cnpj_cpf" = "29756194804"
	Then Sem nenhum erro
	#data de nascimento fica com null
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "dt_nasc" = "null"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "sexo" = ""

Scenario: EnderecoCadastralCliente CPF diferente do principal
	Given Pedido base
	And Informo "EnderecoCadastralCliente.Endereco_cnpj_cpf" = "1"
	And Informo "pedidoMagentoDto.Cnpj_Cpf" = "2"
	Then Erro "Cnpj_Cpf está diferente de EnderecoEntrega.EndEtg_cnpj_cpf."