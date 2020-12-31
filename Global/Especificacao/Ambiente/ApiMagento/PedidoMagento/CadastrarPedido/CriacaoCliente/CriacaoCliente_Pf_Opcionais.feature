@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: Magento CriacaoCliente_Pf_Opcionais
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
	Given AdicionarDependencia ambiente = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", especificacao = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente.CriacaoCliente_Pf_Opcionais"


Scenario: comprovar que salva todos os campos
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	#só pra garantir que estmaos usando o CPF certo
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "cnpj_cpf" = "29756194804"

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

	Given Limpar tabela "t_CLIENTE"
	Given Pedido base
	When Informo "EndEtg_email_xml" = "email sem arroba"
	Then Erro "e-mail inválido"

@ignore
Scenario: validar telefones - especificação
#pergunta:
#	- exigir telefones com a lógica atual (exemplo: não permitir telefone comercial para PF)
#	atualmente, se é cliente PF, não aceitamos nenhum telefone.
#
#	lógica do endereço de entrega:
#	- se cliente PF, somente endereço e justificativa (proibimos os outros campos)
#	- se cliente PJ, tem telefones, CPF/CNPJ, IE, razão social
#	lógica do cadastro do cliente:
#	- se cliente PF, exige pelo menos um telefone
#
#resposta:
#	primeiro passar para o endereço de cobrança
#	não exigir telefones e aceitamos todos que recebermos

@ignore
Scenario: validar telefones
	Given Pedido base
	Given fazer este cenário

