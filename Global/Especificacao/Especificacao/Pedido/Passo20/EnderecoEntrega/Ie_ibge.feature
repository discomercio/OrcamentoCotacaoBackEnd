@ignore
Feature: Ie_ibge

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
#if rb_end_entrega = "S" then
#	if Not consiste_municipio_IBGE_ok(EndEtg_cidade, EndEtg_uf, s_lista_sugerida_municipios, msg_erro) then
#		if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
#		if msg_erro <> "" then
#			alerta = alerta & msg_erro
#		else
#			alerta = alerta & "Endereço de entrega: município '" & EndEtg_cidade & "' não consta na relação de municípios do IBGE para a UF de '" & EndEtg_uf & "'!!"
#			if s_lista_sugerida_municipios <> "" then
#				alerta = alerta & "<br>" & _
#									"Localize o município na lista abaixo e verifique se a grafia está correta!!"
#				end if
#			end if
#		end if
#	end if
#end if
