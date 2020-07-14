<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp" -->

<!-- #include file = "../global/TrataSessaoExpirada.asp"        -->

<%
'     =====================================
'	  FinCadUnidadeNegocioRateioLista.asp
'     =====================================
'
'
'	  S E R V E R   S I D E   S C R I P T I N G
'
'      SSSSSSS   EEEEEEEEE  RRRRRRRR   VVV   VVV  IIIII  DDDDDDDD    OOOOOOO   RRRRRRRR
'     SSS   SSS  EEE        RRR   RRR  VVV   VVV   III   DDD   DDD  OOO   OOO  RRR   RRR
'      SSS       EEE        RRR   RRR  VVV   VVV   III   DDD   DDD  OOO   OOO  RRR   RRR
'       SSSS     EEEEEE     RRRRRRRR   VVV   VVV   III   DDD   DDD  OOO   OOO  RRRRRRRR
'          SSS   EEE        RRR RRR     VVV VVV    III   DDD   DDD  OOO   OOO  RRR RRR
'     SSS   SSS  EEE        RRR  RRR     VVVVV     III   DDD   DDD  OOO   OOO  RRR  RRR
'      SSSSSSS   EEEEEEEEE  RRR   RRR     VVV     IIIII  DDDDDDDD    OOOOOOO   RRR   RRR
'
'
'	REVISADO P/ IE10


' _____________________________________________________________________________________________
'
'			I N I C I A L I Z A     P � G I N A     A S P    N O    S E R V I D O R
' _____________________________________________________________________________________________

	On Error GoTo 0
	Err.Clear
	
'	OBTEM USU�RIO
	dim usuario
	usuario = trim(Session("usuario_atual"))
	If (usuario = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO) 

'	CONECTA COM O BANCO DE DADOS
	Dim cn, rs, msg_erro
	If Not bdd_conecta(cn) then Response.Redirect("aviso.asp?id=" & ERR_CONEXAO)
	if Not cria_recordset_otimista(rs, msg_erro) then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_CRIAR_ADO)

	dim ordenacao_selecionada
	ordenacao_selecionada=Trim(request("ord"))




' ________________________________
' E X E C U T A _ C O N S U L T A
'
Sub executa_consulta
dim consulta, s, i, x, cab
dim r
dim w_plano_conta, w_rateio
dim strTextoStatusInativo, strTextoStatusAtivo
dim strRateio

	strTextoStatusInativo = "<span style='color:" & finStAtivoCor(COD_FIN_ST_ATIVO__INATIVO) & ";'>" & finStAtivoDescricao(COD_FIN_ST_ATIVO__INATIVO) & "</span>"
	strTextoStatusAtivo = "<span style='color:" & finStAtivoCor(COD_FIN_ST_ATIVO__ATIVO) & ";'>" & finStAtivoDescricao(COD_FIN_ST_ATIVO__ATIVO) & "</span>"

	w_plano_conta = 200
	w_rateio = 200
	
'	CABE�ALHO
	cab = _
			"<TABLE class='Q' cellSpacing=0>" & chr(13) & _
			"	<TR style='background:azure;' NOWRAP>" & chr(13) & _
			"		<TD width='" & w_plano_conta & "' NOWRAP class='MD MB'><P class='R' style='cursor:pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='FinCadUnidadeNegocioRateioLista.asp?ord=1" & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">&nbsp;Plano de Conta</P></TD>" & chr(13) & _
			"		<TD width='" & w_rateio & "' NOWRAP class='MD MB'><P class='R' style='cursor:pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='FinCadUnidadeNegocioRateioLista.asp?ord=2" & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">&nbsp;Rateio</P></TD>" & chr(13) & _
			"		<TD width='70' align='center' NOWRAP class='MB'><P class='R' style='cursor:pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='FinCadUnidadeNegocioRateioLista.asp?ord=3" & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">Status</P></TD>" & chr(13) & _
			"	</TR>" & chr(13)

	consulta = "SELECT" & _
					" tFUNR.*," & _
					" tFPCC.descricao AS descricao_plano_conta," & _
					" (SELECT TOP 1 descricao FROM t_FIN_UNIDADE_NEGOCIO_RATEIO_ITEM tFUNRI INNER JOIN t_FIN_UNIDADE_NEGOCIO tFUN ON (tFUNRI.id_unidade_negocio=tFUN.id) WHERE (tFUNRI.id_rateio=tFUNR.id) ORDER BY descricao) AS primeira_unidade_negocio" & _
				" FROM t_FIN_UNIDADE_NEGOCIO_RATEIO tFUNR" & _
					" INNER JOIN t_FIN_PLANO_CONTAS_CONTA tFPCC ON (tFUNR.id_plano_contas_conta=tFPCC.id) AND (tFUNR.natureza=tFPCC.natureza)" & _
				" ORDER BY "
	select case ordenacao_selecionada
		case "1": consulta = consulta & "id_plano_contas_conta, natureza, id"
		case "2": consulta = consulta & "primeira_unidade_negocio, id_plano_contas_conta, natureza"
		case "3": consulta = consulta & "st_ativo, id_plano_contas_conta, natureza"
		case else: consulta = consulta & "id_plano_contas_conta, natureza"
		end select

'	EXECUTA CONSULTA
	x=cab
	i=0
	
	set r = cn.Execute( consulta )

	while Not r.Eof
	'	CONTAGEM
		i = i + 1

	'	ALTERN�NCIA NAS CORES DAS LINHAS
		if (i AND 1)=0 then
			x = x & "	<TR NOWRAP style='background:#FFFFF0;'>" & chr(13)
		else
			x = x & "	<TR NOWRAP>" & chr(13)
			end if

	'>	PLANO DE CONTA
		x = x & _
			"		<TD class='MDB' width='" & w_plano_conta & "' valign='top'><P class='C' NOWRAP>&nbsp;" & _
			"<a href='javascript:fOPConcluir(" & chr(34) & r("id") & chr(34) & _
			")' title='clique para consultar o cadastro deste registro'>" & _
			normaliza_codigo(r("id_plano_contas_conta"),TAM_PLANO_CONTAS__CONTA) & " (" & Trim("" & r("natureza")) & ") - " & Ucase(Trim("" & r("descricao_plano_conta"))) & "</a></P></TD>" & chr(13)

	'>	RATEIO
		s = "SELECT " & _
				"*" & _
			" FROM t_FIN_UNIDADE_NEGOCIO_RATEIO_ITEM tFUNRI" & _
				" INNER JOIN t_FIN_UNIDADE_NEGOCIO tFUN ON (tFUNRI.id_unidade_negocio=tFUN.id)" & _
			" WHERE" & _
				" (tFUNRI.id_rateio = " & Trim("" & r("id")) & ")" & _
			" ORDER BY" & _
				" tFUN.descricao"
		if rs.State <> 0 then rs.Close
		rs.Open s, cn
		strRateio = ""
		do while Not rs.Eof
			strRateio = strRateio & _
						"<tr>" & _
						"<td align='right'>" & _
							"<p class='C'>" & Trim("" & rs("apelido")) & ": " & "</p>" & _
						"</td>" & _
						"<td>" & _
							"<p class='C'>" & formata_perc(rs("perc_rateio")) & "%" & "</p>" & _
						"</td>" & _
						"</tr>"
			rs.MoveNext
			loop
		
		if strRateio <> "" then
			strRateio = "<table cellspacing='0' cellpadding='1'>" & _
						strRateio & _
						"</table>"
			end if
			
		if strRateio = "" then strRateio = "&nbsp;"
		x = x & _
			"		<TD class='MDB' align='center' valign='top' NOWRAP>" & _
						"<P class='C'>" & _
						strRateio & _
						"</P>" & _
					"</TD>" & chr(13)
		
	'>	STATUS
		if Cstr(r("st_ativo"))=Cstr(COD_FIN_ST_ATIVO__INATIVO) then 
			s = strTextoStatusInativo
		else
			s = strTextoStatusAtivo
			end if
		x = x & _
			"		<TD class='MB' align='center' valign='top' NOWRAP><P class='Cc'>&nbsp;" & s & "</P></TD>" & chr(13)

		x = x & _
			"	</TR>" & chr(13)

		if (i mod 100) = 0 then
			Response.Write x
			x = ""
			end if

		r.MoveNext
		wend


'	MOSTRA TOTAL DE REGISTROS
	x = x & _
		"	<TR NOWRAP style='background: #FFFFDD'>" & chr(13) & _
		"		<TD COLSPAN='3' NOWRAP><P class='Cd'>" & "TOTAL:&nbsp;&nbsp;&nbsp;" & cstr(i) & "&nbsp;&nbsp;registro(s)" & "</P></TD>" & chr(13) & _
		"	</TR>" & chr(13)

'	FECHA TABELA
	x = x & _
		"</TABLE>" & chr(13)
	
	Response.write x

	r.close
	set r=nothing

end sub
%>







<%
'	  C L I E N T   S I D E   S C R I P T I N G
'
'      CCCCCCC   LLL        IIIII  EEEEEEEEE  NNN   NNN  TTTTTTTTT EEEEEEEEE
'     CCC   CCC  LLL         III   EEE        NNNN  NNN     TTT    EEE
'     CCC        LLL         III   EEE        NNNNN NNN     TTT    EEE
'     CCC        LLL         III   EEEEEE     NNN NNNNN     TTT    EEEEEE
'     CCC        LLL         III   EEE        NNN  NNNN     TTT    EEE
'     CCC   CCC  LLL   LLL   III   EEE        NNN   NNN     TTT    EEE
'      CCCCCCC   LLLLLLLLL  IIIII  EEEEEEEEE  NNN   NNN     TTT    EEEEEEEEE
%>

<html>


<head>
	<title>CENTRAL ADMINISTRATIVA</title>
	</head>

<script language="JavaScript" type="text/javascript">
window.status='Aguarde, executando consulta ...';

function fOPConcluir(s_id){
	window.status = "Aguarde ...";
	fOP.id_selecionado.value=s_id;
	fOP.submit();
}
</script>

<link href="<%=URL_FILE__E_CSS%>" Rel="stylesheet" Type="text/css">
<link href="<%=URL_FILE__EPRINTER_CSS%>" Rel="stylesheet" Type="text/css" media="print">


<body onload="window.status='Conclu�do';" link=#000000 alink=#000000 vlink=#000000>

<!--  I D E N T I F I C A � � O  -->
<table width="100%" cellPadding="4" CellSpacing="0" style="border-bottom:1px solid black">
<tr>
	<td align="RIGHT" vAlign="BOTTOM" NOWRAP><span class="PEDIDO">Rela��o de Rateios Cadastrados</span>
	<br><span class="Rc">
		<a href="resumo.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="retorna para p�gina inicial" class="LPagInicial">p�gina inicial</a>&nbsp;&nbsp;&nbsp;
		<a href="sessaoencerra.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="encerra a sess�o do usu�rio" class="LSessaoEncerra">encerra</a>
		</span></td>
</tr>
</table>


<!--  RELA��O DE REGISTROS CADASTRADOS  -->
<br>
<center>
<form method="post" action="FinCadUnidadeNegocioRateioEdita.asp" id="fOP" name="fOP">
<%=MontaCampoFormSessionCtrlInfo(Session("SessionCtrlInfo"))%>
<INPUT type=HIDDEN name='id_selecionado' id="id_selecionado" value=''>
<INPUT type=HIDDEN name='operacao_selecionada' id="operacao_selecionada" value='<%=OP_CONSULTA%>'>
<% executa_consulta %>
</form>

<br>

<p class="TracoBottom"></p>

<table class="notPrint" cellSpacing="0">
<tr>
	<td align="CENTER"><a href="FinCadUnidadeNegocioRateioMenu.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="volta para a p�gina anterior">
		<img src="../botao/voltar.gif" width="176" height="55" border="0"></a></td>
</tr>
</table>

</center>


</body>
</html>


<%
	if rs.State <> 0 then rs.Close
	set rs = nothing

'	FECHA CONEXAO COM O BANCO DE DADOS
	cn.Close
	set cn = nothing
%>