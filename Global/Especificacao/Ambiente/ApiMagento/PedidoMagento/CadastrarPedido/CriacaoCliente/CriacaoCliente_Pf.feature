@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: Magento CriacaoCliente_Pf
#Testando a criação do cliente
#Se for PF, copio o endereço de entrega para o endereço de cobrança e apago o endereço de entrega.
#	no caso de campos que só existam no endereço de cobrança (exemplo: telefone) mantemos o do endereço de cobrança e não exigimos o campo.
#Depois disso, se o cliente não existir, cadastrar com o endereço de cobrança


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
	Given AdicionarDependencia ambiente = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", especificacao = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente.CriacaoCliente_Pf"


@ignore
Scenario: Caminho feliz
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
	When Informo "EndEtg_cnpj_cpf" = "29756194804"
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "endereco" = "Rua Professor Fábio Fanucchi"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "endereco_numero" = "1"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "bairro" = "Jardim São Paulo(Zona Norte)"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "cidade" = "São Paulo"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "uf" = "SP"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "cep" = "02045080"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "nome" = "Vivian"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "tipo_pessoa" = "PF"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "cnpj_cpf" = "29756194804"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_NAO"

@ignore
Scenario: Exigimos endereço de entrega para PF
	#Se for PF, copio o endereço de entrega para o endereço de cobrança e apago o endereço de entrega.
	#NOta: poderiamos usar o dado cadastral, mas a especificação está dizendo que sempre terá um endereço de entrega.
	Given Pedido base
	When Informo "OutroEndereco" = "false"
	Then Erro "Obrigatório informar um endereço de entrega na API Magento para cliente PF."

@ignore
Scenario: se for PF, assumimos Endereco_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RURAL_NAO e Endereco_contribuinte_icms_status = NAO
	Given Pedido base
	Then Sem nenhum erro
	#COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = 1,
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "produtor_rural_status" = "1"
	#COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO = 1,
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "contribuinte_icms_status" = "1"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "ie" = ""


@ignore
Scenario: usar o flag para indicar que esse t_cliente foi criado pelo magento (o sistema_responsavel_cadastro)
	Given Pedido base
	Then Sem nenhum erro
	#COD_SISTEMA_RESPONSAVEL_CADASTRO__ERP_WEBAPI = 4   //API magento
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "sistema_responsavel_cadastro" = "4"

@ignore
Scenario: falta: sexo e data de nascimento - vao ficar em branco
	Given Pedido base
	Then Sem nenhum erro
	#data de nascimento fica com null
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "nascimento" = "null"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "sexo" = ""

@ignore
Scenario: comprovar que salva todos os campos
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	#só pra garantir que estmaos usando o CPF certo
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "29756194804"

	#os campos obrigatórios já foram verificados
	#vamos verificar se os opcionais são salvos
	When Informo "EndEtg_endereco_complemento" = "compl"
	When Informo "EndEtg_email" = "email@teste.com"
	When Informo "EndEtg_email_xml" = "emailxml@teste.com"
	When Informo "EndEtg_ddd_res" = "12"
	When Informo "EndEtg_tel_res" = "12345678"
	When Informo "EndEtg_ddd_com" = "34"
	When Informo "EndEtg_tel_com" = "34567890"
	When Informo "EndEtg_ramal_com" = "5"
	When Informo "EndEtg_ddd_cel" = "45"
	When Informo "EndEtg_tel_cel" = "56789012"
	When Informo "EndEtg_ddd_com_2" = "56"
	When Informo "EndEtg_tel_com_2" = "56789012"
	When Informo "EndEtg_ramal_com_2" = "7"

	Then Sem nenhum erro

	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "endereco_complemento" = "compl"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "email" = "email@teste.com"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "email_xml" = "emailxml@teste.com"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "ddd_res" = "12"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "tel_res" = "12345678"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "ddd_com" = "34"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "tel_com" = "34567890"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "ramal_com" = "5"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "ddd_cel" = "45"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "tel_cel" = "56789012"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "ddd_com_2" = "56"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "tel_com_2" = "56789012"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "ramal_com_2" = "7"


@ignore
Scenario: validar emails
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_email" = "email sem arroba"
	Then Erro "e-mail inválido"

	Given Pedido base
	When Informo "EndEtg_email_xml" = "email sem arroba"
	Then Erro "e-mail inválido"

@ignore
Scenario: validar telefones
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	Given fazer este cenário

@ignore
Scenario: Não atualizar dados se o cliente já existir
	Given fazer este cenário

