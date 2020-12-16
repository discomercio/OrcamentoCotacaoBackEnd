@Especificacao.Pedido.Passo20.EnderecoEntrega
Feature: Ie no endereço de entrega

Background: Configuração
	#na ApiMagento não tem IE
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

Scenario: Validar IE sem erro
	Given Pedido base cliente PJ com endereço de entrega
	When Endereço de entrega do estado "SP"
	When Informo "EndEtg_ie" = ""
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL"
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_NAO"
	Then Sem nenhum erro

	Given Pedido base cliente PJ com endereço de entrega
	When Endereço de entrega do estado "BA"
	When Informo "EndEtg_ie" = ""
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL"
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_NAO"
	Then Sem nenhum erro

Scenario: Validar IE erro 1
	#loja/PedidoNovoConsiste.asp
	#			        if Not isInscricaoEstadualValida(EndEtg_ie, EndEtg_uf) then
	#				        alerta="Endereço de entrega: preencha a IE (Inscrição Estadual) com um número válido!!" & _
	#						        "<br>" & "Certifique-se de que a UF do endereço de entrega corresponde à UF responsável pelo registro da IE."
	#				        end if
	#			        end if
	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_ie" = "123"
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	Then Erro "regex preencha a IE \(Inscrição Estadual\) com um número válido.*"

Scenario: Validar IE  erro 2 - com outro estado
	Given Pedido base cliente PJ com endereço de entrega
	When Endereço de entrega do estado "BA"
	When Informo "EndEtg_ie" = "151.313.629"
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	Then Erro "regex preencha a IE \(Inscrição Estadual\) com um número válido.*"

Scenario: Validar IE  certo 2 - com outro estado
	Given Pedido base cliente PJ com endereço de entrega
	When Endereço de entrega do estado "BA"
	When Informo "EndEtg_ie" = "51.313.629"
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	Then Sem nenhum erro

Scenario: Validar IE  erro 2 - com outro estado 2
	Given Pedido base cliente PJ com endereço de entrega
	When Endereço de entrega do estado "SP"
	When Informo "EndEtg_ie" = "115.327.961.119"
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	Then Sem nenhum erro

Scenario: Validar IE  erro 2 - com outro estado 2 errado
	Given Pedido base cliente PJ com endereço de entrega
	When Endereço de entrega do estado "SP"
	#este está errado
	When Informo "EndEtg_ie" = "115.327.961.118"
	When Informo "EndEtg_contribuinte_icms_status" = "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM"
	When Informo "EndEtg_produtor_rural_status" = "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM"
	Then Erro "regex preencha a IE \(Inscrição Estadual\) com um número válido.*"

