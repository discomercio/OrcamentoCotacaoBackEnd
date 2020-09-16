@ignore
Feature: RaIndicador

Scenario: sem indicador não pode ter RA
	#loja/PedidoNovoConfirma.asp
	#Se não tiver indicador e tentar criar um pedido com RA, tem que dar erro
	#	dim permite_RA_status
	#	permite_RA_status = 0
	#	if alerta = "" then
	#		if c_indicador <> "" then
	#			if Not le_orcamentista_e_indicador(c_indicador, r_orcamentista_e_indicador, msg_erro) then
	#				alerta = "Falha ao recuperar os dados do indicador!!<br>" & msg_erro
	#			else
	#				permite_RA_status = r_orcamentista_e_indicador.permite_RA_status
	#				end if
	#			end if
	#		end if
	When Fazer esta validação

