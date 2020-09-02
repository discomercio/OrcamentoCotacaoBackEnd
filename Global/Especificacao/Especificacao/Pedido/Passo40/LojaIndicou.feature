﻿@ignore
Feature: LojaIndicou

Background: Configurar lojas para teste
	set r = cn.Execute("SELECT * FROM t_LOJA WHERE (comissao_indicacao > 0) ORDER BY CONVERT(smallint,loja)")
	Given Limpar tabela "t_LOJA"

	And Novo registro em "t_LOJA"
	And Novo registro "loja" = "100"
	And Novo registro "comissao_indicacao" = "0"
	And Gravar registro 

	And Novo registro em "t_LOJA"
	And Novo registro "loja" = "200"
	And Novo registro "comissao_indicacao" = "1"
	And Gravar registro 

    And Reiniciar banco quando terminar o teste


	#loja/PedidoNovoConsiste.asp
	#dim s_loja_indicou, s_nome_loja_indicou
	#if Session("vendedor_externo") then
	#	s_loja_indicou=retorna_so_digitos(Trim(request("loja_indicou")))
	#	s_nome_loja_indicou = ""
	#	if s_loja_indicou = "" then
	#		alerta=texto_add_br(alerta)
	#		alerta = alerta & "Não foi especificada a loja que fez a indicação."
	#	else
	#		s = "SELECT * FROM t_LOJA WHERE (loja='" & s_loja_indicou & "')"
	#		set rs = cn.execute(s)
	#		if rs.Eof then 
	#			alerta=texto_add_br(alerta)
	#			alerta = alerta & "Loja " & s_loja_indicou & " não está cadastrada."
	#		else
	#			s_nome_loja_indicou = Trim("" & rs("nome"))
	#			if s_nome_loja_indicou = "" then s_nome_loja_indicou = Trim("" & rs("razao_social"))
	#			end if
	#		if rs.State <> 0 then rs.Close
	#		end if
	#	end if

Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo40.LojaIndicou"
	Given Implementado em "Especificacao.Pedido.Pedido"
	And Fim da configuração

Scenario: Seomnte pode existir se for vendedor externo
	Given Pedido base
	When Usuario logado "vendedor_externo" = "0"
	And Informo "loja_indicou" = "xx"
	Then Erro "loja_indicou permitidio somente para vendedores externos"

	Given Pedido base
	When Usuario logado "vendedor_externo" = "0"
	And Informo "loja_indicou" = "200"
	Then Erro "loja_indicou permitidio somente para vendedores externos"

Scenario: Vendedor externo
	Given Pedido base
	When Usuario logado "vendedor_externo" = "1"
	And Informo "loja_indicou" = "xx"
	Then Erro "loja_indicou não existe"

	Given Pedido base
	When Usuario logado "vendedor_externo" = "1"
	And Informo "loja_indicou" = "100"
	Then Erro "loja_indicou não existe"

	Given Pedido base
	When Usuario logado "vendedor_externo" = "1"
	And Informo "loja_indicou" = "200"
	Then Sem erro

Scenario: Vendedor externo sem loja
	Given Pedido base
	When Usuario logado "vendedor_externo" = "1"
	And Informo "loja_indicou" = ""
	Then Sem erro

