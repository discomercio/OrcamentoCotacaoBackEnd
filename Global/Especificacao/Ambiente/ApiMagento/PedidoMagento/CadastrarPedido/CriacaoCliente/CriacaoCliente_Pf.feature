@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: Magento CriacaoCliente_Pf
#Testando a criação do cliente
#Se for PF, copio o endereço de entrega para o endereço de cobrança e apago o endereço de entrega.
#	no caso de campos que só existam no endereço de cobrança (exemplo: telefone) mantemos o do endereço de cobrança e não exigimos o campo.
#Depois disso, se o cliente não existir, cadastrar com o endereço de cobrança


Background: Acertar banco de dados
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_CLIENTE"

@ListaDependencias
Scenario: AdicionarDependencia
	Given AdicionarDependencia ambiente = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", especificacao = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente.CriacaoCliente_Pf"


Scenario: Exigimos endereço de entrega para PF
	#Se for PF, copio o endereço de entrega para o endereço de cobrança e apago o endereço de entrega.
	#201229 reuniao semanal.txt
	#- fazer o endereço de entrega para PF obrigatório?
	#sim, exigir.
	Given Pedido base
	When Informo "OutroEndereco" = "false"
	Then Erro "Obrigatório informar um endereço de entrega na API Magento para cliente PF."


Scenario: se for PF, assumimos Endereco_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RURAL_NAO e Endereco_contribuinte_icms_status = INICIAL
#Ao cadastrar o cliente:
#- se for PF, assumimos Endereco_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RURAL_NAO e Endereco_contribuinte_icms_status = INICIAL
#- se for PJ, deixar o pedido st_etg_imediata = 1 (não)
#	e colocar Endereco_contribuinte_icms_status = inicial, Endereco_ie = vazio
#Contribuinte ICMS
#Para cliente PJ, quando o cliente for cadastrado automaticamente, manter o campo contribuinte_icms_status com o status default (zero).
	Given Pedido base
	#só pra garantir que estmaos usando o CPF certo
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "cnpj_cpf" = "29756194804"
	Then Sem nenhum erro
	#COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = 1,
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "produtor_rural_status" = "1"
	#COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL = 0,
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "contribuinte_icms_status" = "0"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "ie" = ""


Scenario: usar o flag para indicar que esse t_cliente foi criado pelo magento (o sistema_responsavel_cadastro)
	Given Pedido base
	#só pra garantir que estmaos usando o CPF certo
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "cnpj_cpf" = "29756194804"
	Then Sem nenhum erro
	#COD_SISTEMA_RESPONSAVEL_CADASTRO__ERP_WEBAPI = 4   //API magento
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "29756194804", verificar campo "sistema_responsavel_cadastro" = "4"

Scenario: falta: sexo e data de nascimento - vao ficar em branco
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


