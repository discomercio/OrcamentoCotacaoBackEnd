@ignore
Feature: vl_aprov_auto_analise_credito
#loja/PedidoNovoCOnfirma.asp

#tratado no código em \arclube\Global\Pedido\Criacao\Passo60\Validacao\ConfigurarVariaveis.cs:ConfigurarVariaveisExecutar() Criacao.Execucao.Vl_aprov_auto_analise_credito
#'	OBTÉM O VALOR LIMITE P/ APROVAÇÃO AUTOMÁTICA DA ANÁLISE DE CRÉDITO
#	if alerta = "" then
#		s = "SELECT nsu FROM t_CONTROLE WHERE (id_nsu = '" & ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO & "')"
#		set rs = cn.execute(s)
#		if Not rs.Eof then
#			vl_aprov_auto_analise_credito = converte_numero(rs("nsu"))
#			end if
#		if rs.State <> 0 then rs.Close
#		end if
#
#					elseif vl_total <= vl_aprov_auto_analise_credito then
#						rs("analise_credito")=Clng(COD_AN_CREDITO_OK)
#						rs("analise_credito_data")=Now
#						rs("analise_credito_usuario")="AUTOMÁTICO"

Scenario: vl_aprov_auto_analise_credito automatica
	#precisa testar a formatação no NSU: 1,00 = 1, mas 1.00 = 100
	Given Fazer esta validacao

Scenario: vl_aprov_auto_analise_credito sem automatica
	Given Fazer esta validacao

