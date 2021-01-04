@Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj
Feature: Pedido de cliente PJ com endereço de entrega PJ
#em loja/ClienteEdita.asp:
#var EndEtg_tipo_pessoa = $('input[name="EndEtg_tipo_pessoa"]:checked').val();
#e linhas para baixo
#loja/PedidoNovoConsiste.asp
#                    if EndEtg_cnpj_cpf = "" or not cnpj_ok(EndEtg_cnpj_cpf) then
#....até....
#				        alerta="Endereço de entrega: preencha a IE (Inscrição Estadual) com um número válido!!" & _


Background: Api MAgento somente aceita pedidos PF
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base cliente PJ com endereço de entrega PJ
	Given No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido" erro "Endereço de entrega: se cliente é contribuinte do ICMS isento, não pode ter valor no campo de Inscrição Estadual!!" é "Endereço de entrega: se o Contribuinte ICMS é isento, o campo IE deve ser vazio!"
	Given No ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido" erro "Endereço de entrega: se cliente é contribuinte do ICMS isento, não pode ter valor no campo de Inscrição Estadual!!" é "Endereço de entrega: se o Contribuinte ICMS é isento, o campo IE deve ser vazio!"

Scenario: EndEtg_cnpj_cpf_PJ
	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EndEtg_cnpj_cpf" = ""
	Then Erro "Endereço de entrega: CNPJ inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EndEtg_cnpj_cpf" = "123"
	Then Erro "Endereço de entrega: CNPJ inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EndEtg_cnpj_cpf" = "435.434.870-51"
	Then Erro "Endereço de entrega: CNPJ inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EndEtg_cnpj_cpf" = "40.745.075/0001-00"
	Then Erro "Endereço de entrega: CNPJ inválido!!"

	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EndEtg_cnpj_cpf" = "40.745.075/0001-16"
	Then Sem erro "Endereço de entrega: CNPJ inválido!!"

Scenario: EndEtg_contribuinte_icms_status_PJ
	When Informo "EndEtg_contribuinte_icms_status" = "10"
	Then Erro "Endereço de entrega: valor de contribuinte do ICMS inválido!"

#Contribuinte_Icms_Status: INICIAL = 0, NAO = 1, SIM = 2, ISENTO = 3
#icms não: IE opcional
#icms sim: IE obrigatório
#icms isento: IE vazio
Scenario: EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	And Informo "EndEtg_ie" = ""
	Then Erro "Endereço de entrega: se o cliente é contribuinte do ICMS a inscrição estadual deve ser preenchida!!"

Scenario: EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM ISEN
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	And Informo "EndEtg_ie" = "ISEN"
	Then Erro "Endereço de entrega: se cliente é contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!!"

Scenario: EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO"
	And Informo "EndEtg_ie" = "ISEN"
	Then Erro "Endereço de entrega: se cliente é não contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!!"

Scenario: EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO 2
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO"
	#IE de são paulo
	And Informo "EndEtg_ie" = "715255502973"
	Then Sem nenhum erro

Scenario: EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO opcional
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO"
	And Informo "EndEtg_ie" = ""
	Then Sem nenhum erro

Scenario: EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO 1
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO"
	And Informo "EndEtg_ie" = "ISEN"
	Then Erro "Endereço de entrega: se cliente é contribuinte do ICMS isento, não pode ter valor no campo de Inscrição Estadual!!"

Scenario: EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO 2
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO"
	#IE de são paulo
	And Informo "EndEtg_ie" = "715255502973"
	Then Erro "Endereço de entrega: se cliente é contribuinte do ICMS isento, não pode ter valor no campo de Inscrição Estadual!!"

Scenario: EndEtg_nome
	When Informo "EndEtg_nome" = ""
	Then Erro "Endereço de Entrega: Preencha o nome/razão social no endereço de entrega!"

