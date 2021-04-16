@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: CamposObrigatorios

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

Scenario: CamposObrigatorios - sucesso
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

Scenario: CamposObrigatorios - endereço
	When Informo "EndEtg_endereco" = ""
	Then Erro "PREENCHA O ENDEREÇO."

Scenario: CamposObrigatorios - numero
	When Informo "EndEtg_endereco_numero" = ""
	Then Erro "PREENCHA O NÚMERO DO ENDEREÇO."

Scenario: CamposObrigatorios - bairro
	When Informo "EndEtg_bairro" = ""
	Then Erro "PREENCHA O BAIRRO."

Scenario: CamposObrigatorios - cidade
	When Informo "EndEtg_cidade" = ""
	Then Erro "PREENCHA A CIDADE."

Scenario: CamposObrigatorios - UF
	When Informo "EndEtg_uf" = ""
	Then Erro "INFORME O UF."

Scenario: CamposObrigatorios - CEP
	When Informo "EndEtg_cep" = ""
	Then Erro "INFORME O CEP."