﻿@Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj
Feature: Pedido de cliente PJ com endereço de entrega PF
#em loja/ClienteEdita.asp:
#if (f.EndEtg_cnpj_cpf_PF.value == "" || !cpf_ok(f.EndEtg_cnpj_cpf_PF.value)) {
#e linhas para baixo
#também em loja/PedidoNovoConsiste.asp

Background: Api MAgento somente aceita pedidos PF
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

	Given Pedido base cliente PJ com endereço de entrega PF

Scenario: EndEtg_cnpj_cpf
	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf" = ""
	Then Erro "Endereço de entrega: CPF inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf" = "123"
	Then Erro "Endereço de entrega: CPF inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf" = "40.745.075/0001-16"
	Then Erro "Endereço de entrega: CPF inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf" = "40.745.075/0001-00"
	Then Erro "Endereço de entrega: CPF inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf" = "435.434.870-01"
	Then Erro "Endereço de entrega: CPF inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EndEtg_cnpj_cpf" = "435.434.870-51"
	Then Sem erro "Endereço de entrega: CPF inválido!!"

#COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL = 0,
#COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = 1,
#COD_ST_CLIENTE_PRODUTOR_RURAL_SIM = 2
Scenario: EndEtg_produtor_rural_status
	When Informo "EndEtg_produtor_rural_status" = "33"
	Then Erro "regex Endereço de entrega: valor de produtor rural invá.*"

Scenario: EndEtg_produtor_rural_status 2
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL"
	Then Erro "regex Endereço de entrega: valor de produtor rural invá.*"

Scenario: EndEtg_produtor_rural_status EndEtg_contribuinte_icms_status
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	And Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO"
	Then Erro "Endereço de entrega: para ser cadastrado como Produtor Rural, é necessário ser contribuinte do ICMS e possuir nº de IE!!"

Scenario: EndEtg_produtor_rural_status EndEtg_contribuinte_icms_status 2
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	And Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO"
	Then Erro "Endereço de entrega: para ser cadastrado como Produtor Rural, é necessário ser contribuinte do ICMS e possuir nº de IE!!"


#COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL = 0,
#COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO = 1,
#COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM = 2,
#COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO = 3
Scenario: EndEtg_contribuinte_icms_status
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	When Informo "EndEtg_contribuinte_icms_status" = "6"
	Then Erro "Endereço de entrega: valor de contribuinte do ICMS inválido!"

Scenario: EndEtg_contribuinte_icms_status COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	And Informo "EndEtg_ie" = ""
	Then Erro "Endereço de entrega: se o cliente é contribuinte do ICMS a inscrição estadual deve ser preenchida!!"

Scenario: EndEtg_contribuinte_icms_status COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO"
	And Informo "EndEtg_ie" = "ISEN"
	Then Erro "Endereço de entrega: se cliente é não contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!!"

Scenario: EndEtg_contribuinte_icms_status COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM ISEN
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	And Informo "EndEtg_ie" = "ISEN"
	Then Erro "Endereço de entrega: se cliente é contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!!"

Scenario: EndEtg_contribuinte_icms_status COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO"
	And Informo "EndEtg_ie" = "123456"
	Then Erro "Endereço de entrega: se o Contribuinte ICMS é isento, o campo IE deve ser vazio!"

Scenario: EndEtg_nome
	When Informo "EndEtg_nome" = ""
	Then Erro "regex Endereço de Entrega: Preencha o nome/razão social.*"

