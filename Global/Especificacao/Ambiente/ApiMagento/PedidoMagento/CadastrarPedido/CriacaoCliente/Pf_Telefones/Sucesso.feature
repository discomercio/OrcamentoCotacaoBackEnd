@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: Sucesso


Background: Acertar banco de dados
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_CLIENTE"

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
Scenario: validar telefones - todos
	Given Pedido base
	When Informo "OutroEndereco" = "true"
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


	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = ""
	When Informo "EndEtg_tel_res" = ""
	When Informo "EndEtg_ddd_com" = "34"
	When Informo "EndEtg_tel_com" = "34567890"
	When Informo "EndEtg_ramal_com" = "5"
	When Informo "EndEtg_ddd_cel" = "45"
	When Informo "EndEtg_tel_cel" = "56789012"
	When Informo "EndEtg_ddd_com_2" = "56"
	When Informo "EndEtg_tel_com_2" = "56789012"
	When Informo "EndEtg_ramal_com_2" = "7"
	Then Sem nenhum erro


	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "12"
	When Informo "EndEtg_tel_res" = "12345678"
	When Informo "EndEtg_ddd_com" = ""
	When Informo "EndEtg_tel_com" = ""
	When Informo "EndEtg_ramal_com" = ""
	When Informo "EndEtg_ddd_cel" = "45"
	When Informo "EndEtg_tel_cel" = "56789012"
	When Informo "EndEtg_ddd_com_2" = "56"
	When Informo "EndEtg_tel_com_2" = "56789012"
	When Informo "EndEtg_ramal_com_2" = "7"
	Then Sem nenhum erro


	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "12"
	When Informo "EndEtg_tel_res" = "12345678"
	When Informo "EndEtg_ddd_com" = "34"
	When Informo "EndEtg_tel_com" = "34567890"
	When Informo "EndEtg_ramal_com" = "5"
	When Informo "EndEtg_ddd_cel" = ""
	When Informo "EndEtg_tel_cel" = ""
	When Informo "EndEtg_ddd_com_2" = "56"
	When Informo "EndEtg_tel_com_2" = "56789012"
	When Informo "EndEtg_ramal_com_2" = "7"
	Then Sem nenhum erro


	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = "12"
	When Informo "EndEtg_tel_res" = "12345678"
	When Informo "EndEtg_ddd_com" = "34"
	When Informo "EndEtg_tel_com" = "34567890"
	When Informo "EndEtg_ramal_com" = "5"
	When Informo "EndEtg_ddd_cel" = "45"
	When Informo "EndEtg_tel_cel" = "56789012"
	When Informo "EndEtg_ddd_com_2" = ""
	When Informo "EndEtg_tel_com_2" = ""
	When Informo "EndEtg_ramal_com_2" = ""
	Then Sem nenhum erro


Scenario: validar telefones - nenhum
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_ddd_res" = ""
	When Informo "EndEtg_tel_res" = ""
	When Informo "EndEtg_ddd_com" = ""
	When Informo "EndEtg_tel_com" = ""
	When Informo "EndEtg_ramal_com" = ""
	When Informo "EndEtg_ddd_cel" = ""
	When Informo "EndEtg_tel_cel" = ""
	When Informo "EndEtg_ddd_com_2" = ""
	When Informo "EndEtg_tel_com_2" = ""
	When Informo "EndEtg_ramal_com_2" = ""
	Then Sem nenhum erro

