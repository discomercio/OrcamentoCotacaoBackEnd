@Especificacao.Pedido.PedidoFaltandoImplementarSteps

Feature: InscricaoEstadual

Scenario: fazer
	Given fazer esta validação, no pedido e prepedido

#if rb_end_entrega = "S" then
#	if ( (EndEtg_tipo_pessoa = ID_PF) And (Cstr(EndEtg_produtor_rural_status) = Cstr(COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)) ) _
#		Or _
#		( (EndEtg_tipo_pessoa = ID_PJ) And (Cstr(EndEtg_contribuinte_icms_status) = Cstr(COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)) ) _
#		Or _
#		( (EndEtg_tipo_pessoa = ID_PJ) And (Cstr(EndEtg_contribuinte_icms_status) = Cstr(COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)) And (Trim(EndEtg_ie) <> "") ) then
#		if Not isInscricaoEstadualValida(EndEtg_ie, EndEtg_uf) then
#			alerta=texto_add_br(alerta)
#			alerta=alerta & "Endereço de entrega: preencha a IE (Inscrição Estadual) com um número válido!" & _
#					"<br>" & "Certifique-se de que a UF do endereço de entrega corresponde à UF responsável pelo registro da IE."
#			end if
#		end if
#	end if
#end if
Scenario: Validar IE endereco de entrega para PF - Sucesso
	Given Pedido base com endereco de entrega
	When Informo "EnderecoEntrega.EndEtg_produtor_rural_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = "749.201.682.501"
	Then Sem nehum erro

Scenario:Validar IE endereco de entrega para PF - IE inválido e vazio
	#IE inválida
	Given Pedido base com endereco de entrega
	When Informo "EnderecoEntrega.EndEtg_produtor_rural_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = "829514-56"
	Then Erro "Endereço de entrega: preencha a IE (Inscrição Estadual) com um número válido! Certifique-se de que a UF do endereço de entrega corresponde à UF responsável pelo registro da IE."
	#IE vazio
	Given Pedido base com endereco de entrega
	When Informo "EnderecoEntrega.EndEtg_produtor_rural_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = ""
	Then Erro "IE (Inscrição Estadual) vazia! "
	#UF vazia
	Given Pedido base com endereco de entrega
	When Informo "EnderecoEntrega.EndEtg_produtor_rural_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = "749.201.682.501"
	When Informo "EnderecoEntrega.EndEtg_uf" = ""
	Then Erro "UF (estado) vazio! "

Scenario:  Validar IE endereco de entrega para PJ - Sucesso Endereco entrega PF
	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EnderecoEntrega.EndEtg_produtor_rural_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = "749.201.682.501"
	Then Sem nenhum erro

Scenario: Validar IE endereco de entrega para PJ - Sucesso Endereco entrega PJ
	#Contribuinte Sim
	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = "749.201.682.501"
	Then Sem nenhum erro
	#Contribuinte Não com IE
	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "1"
	When Informo "EnderecoEntrega.EndEtg_ie" = "749.201.682.501"
	Then Sem nenhum erro

Scenario: Validar IE endereco de entrega para PJ - Endereco entrega PF IE inválido e vazio
	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EnderecoEntrega.EndEtg_produtor_rural_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = "829514-56"
	Then Erro "Endereço de entrega: preencha a IE (Inscrição Estadual) com um número válido! Certifique-se de que a UF do endereço de entrega corresponde à UF responsável pelo registro da IE."

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EnderecoEntrega.EndEtg_produtor_rural_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = ""
	Then Erro "IE (Inscrição Estadual) vazia! "

	Given Pedido base cliente PJ com endereço de entrega PF
	When Informo "EnderecoEntrega.EndEtg_produtor_rural_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = "749.201.682.501"
	When Informo "EnderecoEntrega.EndEtg_uf" = ""
	Then Erro "UF (estado) vazio! "

Scenario: Validar IE endereco de entrega para PJ - Endereco entrega PJ IE inválido e vazio
	#IE inválida contribuinte sim
	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = "829514-56"
	Then Erro "Endereço de entrega: preencha a IE (Inscrição Estadual) com um número válido! Certifique-se de que a UF do endereço de entrega corresponde à UF responsável pelo registro da IE."
	#IE vazio contribuinte sim
	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = ""
	Then Erro "IE (Inscrição Estadual) vazia! "
	#UF vazia contribuinte sim
	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = "749.201.682.501"
	When Informo "EnderecoEntrega.EndEtg_uf" = ""
	Then Erro "UF (estado) vazio! "
	#IE inválida contribuinte não
	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "2"
	When Informo "EnderecoEntrega.EndEtg_ie" = "829514-56"
	Then Erro "Endereço de entrega: preencha a IE (Inscrição Estadual) com um número válido! Certifique-se de que a UF do endereço de entrega corresponde à UF responsável pelo registro da IE."
	#UF vazia contribuinte não
	Given Pedido base cliente PJ com endereço de entrega PJ
	When Informo "EnderecoEntrega.EndEtg_contribuinte_icms_status" = "1"
	When Informo "EnderecoEntrega.EndEtg_ie" = "749.201.682.501"
	When Informo "EnderecoEntrega.EndEtg_uf" = ""
	Then Erro "UF (estado) vazio! "