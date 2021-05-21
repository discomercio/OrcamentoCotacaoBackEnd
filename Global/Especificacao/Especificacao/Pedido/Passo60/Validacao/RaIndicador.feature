@Especificacao.Pedido.PedidoFaltandoImplementarSteps
Feature: RaIndicador

Scenario: sem indicador não pode ter RA - sem erro
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
	Given Pedido base
	When Informo "NomeIndicador" = "POLITÉCNIC"
	When Informo "OpcaoPossuiRA" = "S"
	Then Sem nenhum erro

Scenario: sem indicador não pode ter RA - erro
	Given Pedido base
	When Informo "NomeIndicador" = ""
	When Informo "OpcaoPossuiRA" = "S"
	Then Erro "pegar o erro"

Scenario: sem indicador não pode ter RA - indicador nao existe
	Given Pedido base
	When Informo "NomeIndicador" = "XPXPXPX"
	When Informo "OpcaoPossuiRA" = "S"
	Then Erro "pegar o erro"

Scenario: sem indicador não pode ter RA - indicador nao existe 2
	Given Pedido base
	When Informo "NomeIndicador" = "XPXPXPX"
	When Informo "OpcaoPossuiRA" = "N"
	Then Erro "pegar o erro"

