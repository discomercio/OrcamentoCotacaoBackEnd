@ignore
@Especificacao.Pedido.Passo40
@GerenciamentoBanco
Feature: LojaIndicou

Background: Configurar lojas para teste
	#set r = cn.Execute("SELECT * FROM t_LOJA WHERE (comissao_indicacao > 0) ORDER BY CONVERT(smallint,loja)")
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_LOJA"
	And Novo registro na tabela "t_LOJA"
	And Novo registro em "t_LOJA", campo "loja" = "100"
	And Novo registro em "t_LOJA", campo "comissao_indicacao" = "0"
	And Gravar registro em "t_LOJA"

	And Novo registro na tabela "t_LOJA"
	And Novo registro em "t_LOJA", campo "loja" = "200"
	And Novo registro em "t_LOJA", campo "comissao_indicacao" = "1"
	And Gravar registro em "t_LOJA"


#lojas permitidas:
#set r = cn.Execute("SELECT * FROM t_LOJA WHERE (comissao_indicacao > 0) ORDER BY CONVERT(smallint,loja)")

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

#@ListaDependencias
#Scenario: Configuração
#	Given Nome deste item "Especificacao.Pedido.Passo40.LojaIndicou"
#	Given Implementado em "Especificacao.Pedido.Pedido"

Scenario: Seomnte pode existir se for vendedor externo

#também em loja/PedidoNovoConsiste.asp linha 3166
#<!--  VENDEDOR EXTERNO: LOJA QUE INDICOU  -->
#<% IF Session("vendedor_externo") THEN %>
#	<br>
#	<table class="Q" style="width:649px;" cellspacing="0">
#		<tr><td align="left"><p class="Rf">Loja que fez a indicação</p>
#			<input name="loja_indicou" id="loja_indicou" class="PLLd" style="width:30px;" readonly tabindex=-1
#				value='<%=s_loja_indicou%>'>&nbsp;-
#			<input name="nome_loja_indicou" id="nome_loja_indicou" class="PLLe" style="width:300px;" readonly tabindex=-1
#				value='<%=s_nome_loja_indicou%>'></td>
#		</tr>
#	</table>
#<% END IF %>

#loja/PedidoNovoConfirma.asp
#não precisa testar porque as lojas já são somente números
#	if Session("vendedor_externo") then
#		s_loja_indicou=retorna_so_digitos(Trim(request("loja_indicou")))
#		venda_externa=1
#		end if


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
	Then Sem nenhum erro

Scenario: Vendedor externo sem loja
	Given Pedido base
	When Usuario logado "vendedor_externo" = "1"
	And Informo "loja_indicou" = ""
	Then Sem nenhum erro

