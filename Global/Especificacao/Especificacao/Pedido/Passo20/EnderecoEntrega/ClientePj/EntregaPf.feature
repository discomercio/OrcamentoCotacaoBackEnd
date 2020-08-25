@ignore
@Especificacao/Pedido
Feature: Pedido de cliente PJ com endereço de entrega PF
em loja/ClienteEdita.asp:
if (f.EndEtg_cnpj_cpf_PF.value == "" || !cpf_ok(f.EndEtg_cnpj_cpf_PF.value)) {
e linhas para baixo

Background: Pedido base
	Given Pedido base cliente PJ com endereço de entrega PF

Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj.EntregaPf"
	Given Implementado em "Especificacao.Pedido.Pedido"
	And Fim da configuração

Scenario: EndEtg_cnpj_cpf_PJ
	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf_PJ" = ""
	Then Erro "Endereço de entrega: CPF inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf_PJ" = "123"
	Then Erro "Endereço de entrega: CPF inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf_PJ" = "40.745.075/0001-16"
	Then Erro "Endereço de entrega: CPF inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf_PJ" = "40.745.075/0001-00"
	Then Erro "Endereço de entrega: CPF inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf_PJ" = "435.434.870-01"
	Then Erro "Endereço de entrega: CPF inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf_PJ" = "435.434.870-51"
	Then sEM Erro "Endereço de entrega: CPF inválido!!"

#COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL = 0,
#COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = 1,
#COD_ST_CLIENTE_PRODUTOR_RURAL_SIM = 2
Scenario: EndEtg_produtor_rural_status_PF
	When Informo "EndEtg_produtor_rural_status_PF" = "3"
	Then Erro "Endereço de entrega: informe se o cliente é produtor rural ou não!!"

Scenario: EndEtg_produtor_rural_status_PF 2
	When Informo "EndEtg_produtor_rural_status_PF" = "COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL"
	Then Erro "Endereço de entrega: informe se o cliente é produtor rural ou não!!"

Scenario: EndEtg_produtor_rural_status_PF EndEtg_contribuinte_icms_status_PF
	When Informo "EndEtg_produtor_rural_status_PF" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	And Informo "EndEtg_contribuinte_icms_status_PF" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO"
	Then Erro "Endereço de entrega: para ser cadastrado como Produtor Rural, é necessário ser contribuinte do ICMS e possuir nº de IE!!"

Scenario: EndEtg_produtor_rural_status_PF EndEtg_contribuinte_icms_status_PF 2
	When Informo "EndEtg_produtor_rural_status_PF" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	And Informo "EndEtg_contribuinte_icms_status_PF" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO"
	Then Erro "Endereço de entrega: para ser cadastrado como Produtor Rural, é necessário ser contribuinte do ICMS e possuir nº de IE!!"


#COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL = 0,
#COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO = 1,
#COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM = 2,
#COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO = 3
Scenario: EndEtg_contribuinte_icms_status_PF
	When Informo "EndEtg_contribuinte_icms_status_PF" = "6"
	Then Erro "Endereço de entrega: informe se o cliente é contribuinte do ICMS, não contribuinte ou isento!!"

Scenario: EndEtg_contribuinte_icms_status_PF COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM
	When Informo "EndEtg_contribuinte_icms_status_PF" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	And Informo "EndEtg_ie_PF" = ""
	Then Erro "Endereço de entrega: se o cliente é contribuinte do ICMS a inscrição estadual deve ser preenchida!!"

Scenario: EndEtg_contribuinte_icms_status_PF COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO
	When Informo "EndEtg_contribuinte_icms_status_PF" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO"
	And Informo "EndEtg_ie_PF" = "ISEN"
	Then Erro "Endereço de entrega: se cliente é não contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!!"

Scenario: EndEtg_contribuinte_icms_status_PF COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM ISEN
	When Informo "EndEtg_contribuinte_icms_status_PF" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	And Informo "EndEtg_ie_PF" = "ISEN"
	Then Erro "Endereço de entrega: se cliente é contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!!"

Scenario: EndEtg_contribuinte_icms_status_PF COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO
	When Informo "EndEtg_contribuinte_icms_status_PF" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO"
	And Informo "EndEtg_ie_PF" = "123456"
	Then Erro "Endereço de entrega: se o Contribuinte ICMS é isento, o campo IE deve ser vazio!"

Scenario: EndEtg_nome
	When Informo "EndEtg_nome" = ""
	Then Erro "Preencha o nome no endereço de entrega!!"

