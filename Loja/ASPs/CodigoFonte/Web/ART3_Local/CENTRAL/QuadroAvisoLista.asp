<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp" -->

<!-- #include file = "../global/TrataSessaoExpirada.asp"        -->

<%
'     =======================================
'	  Q U A D R O A V I S O L I S T A . A S P
'     =======================================
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
	Dim cn
	If Not bdd_conecta(cn) then Response.Redirect("aviso.asp?id=" & ERR_CONEXAO)

	dim ordenacao_selecionada
	ordenacao_selecionada=Trim(request("ord"))






' ________________________________
' E X E C U T A _ C O N S U L T A
'
Sub executa_consulta
dim consulta, s, i, x, cab
dim r

  ' CABE�ALHO
	cab="<TABLE class='Q' cellSpacing=0 width='640'>" & chr(13) & _
		"	<TR style='background: #FFF0E0' NOWRAP>" & chr(13) & _
		"		<TD valign='bottom' align='left' nowrap class='MD MB' style='width:108px;'><P class='R' style='cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='quadroavisolista.asp?ord=1" & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">&nbsp;Data</P></TD>" & chr(13) & _
		"		<TD NOWRAP valign='bottom' class='MD MB' style='width:90px;'><P class='R' style='cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='quadroavisolista.asp?ord=2" & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">&nbsp;Publicado por</P></TD>" & chr(13) & _
		"		<TD NOWRAP valign='bottom' class='MD MB' style='width:35px;'><P class='R' style='cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='quadroavisolista.asp?ord=3" & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">&nbsp;Loja&nbsp;</P></TD>" & chr(13) & _
		"		<TD valign='bottom' class='MB' style='width:350px;'><P class='R'>Mensagem</P></TD>" & chr(13) & _
		"	</TR>" & chr(13)

	consulta= "SELECT * FROM t_AVISO ORDER BY "
	select case ordenacao_selecionada
		case "1": consulta = consulta & "dt_ult_atualizacao"
		case "2": consulta = consulta & "usuario, dt_ult_atualizacao"
		case "3": consulta = consulta & "destinatario, dt_ult_atualizacao"
		case else: consulta = consulta & "dt_ult_atualizacao"
		end select

  ' EXECUTA CONSULTA
	x=cab
	i=0
	
	set r = cn.Execute( consulta )

	while not r.eof 
	  ' CONTAGEM
		i = i + 1

		x = x & "	<TR NOWRAP>" & chr(13)

	 '> DATA
		x = x & "		<TD class='MDB' valign='top' NOWRAP><P class='Cn'>&nbsp;" & _
				"<a href='javascript:fOPConcluir(" & chr(34) & r("id") & chr(34) & _
				")' title='clique para consultar o cadastro deste aviso'>" & _
				formata_data_hora(r("dt_ult_atualizacao")) & "</a></P></TD>" & chr(13)

 	 '> USU�RIO
		x = x & "		<TD class='MDB' valign='top' NOWRAP><P class='Cn' NOWRAP>&nbsp;" & _
				"<a href='javascript:fOPConcluir(" & chr(34) & r("id") & chr(34) & _
				")' title='clique para consultar o cadastro deste aviso'>" & _
				r("usuario") & "</a></P></TD>" & chr(13)

 	 '> LOJA DESTINAT�RIA
		x = x & "		<TD class='MDB' NOWRAP align='center' valign='top'><P class='Cn' NOWRAP>&nbsp;" & _
				"<a href='javascript:fOPConcluir(" & chr(34) & r("id") & chr(34) & _
				")' title='clique para consultar o cadastro deste aviso'>" & _
				Trim("" & r("destinatario")) & "</a></P></TD>" & chr(13)

 	 '> MENSAGEM
 		if IsNull(r("mensagem")) then 
 			s=""
 		else 
 			s=r("mensagem")
 			end if
		s = substitui_caracteres(s, chr(13), "<br>")
		x = x & "		<TD class='MB'><P class='Cn'>" & _
				"<a href='javascript:fOPConcluir(" & chr(34) & r("id") & chr(34) & _
				")' title='clique para consultar o cadastro deste aviso'>" & _
				s & "</a></P></TD>" & chr(13)

		x = x & "	</TR>" & chr(13)

		if (i mod 100) = 0 then
			Response.Write x
			x = ""
			end if

		r.MoveNext
		wend


  ' MOSTRA TOTAL DE AVISOS
	x = x & "	<TR NOWRAP style='background: #FFFFDD'>" & chr(13) & _
			"		<TD COLSPAN='4' NOWRAP><P class='Cd'>" & "TOTAL:&nbsp;&nbsp;&nbsp;" & cstr(i) & "&nbsp;&nbsp;avisos" & "</P></TD>" & chr(13) & _
			"	</TR>" & chr(13)

  ' FECHA TABELA
	x = x & "</TABLE>" & chr(13)
	
	Response.write x

	r.close
	set r=nothing

End sub

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

function fOPConcluir(s_aviso){
	window.status = "Aguarde ...";
	fOP.aviso_selecionado.value=s_aviso;
	fOP.submit(); 
}

</script>

<link href="<%=URL_FILE__E_CSS%>" Rel="stylesheet" Type="text/css">
<link href="<%=URL_FILE__EPRINTER_CSS%>" Rel="stylesheet" Type="text/css" media="print">


<body onload="window.status='Conclu�do';" link=#000000 alink=#000000 vlink=#000000>

<!--  I D E N T I F I C A � � O  -->
<table width="100%" cellpadding="4" cellspacing="0" style="border-bottom:1px solid black">
<tr>
	<td align="right" valign="bottom" nowrap><span class="PEDIDO">Rela��o de Avisos Cadastrados</span>
	<br><span class="Rc">
		<a href="resumo.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="retorna para p�gina inicial" class="LPagInicial">p�gina inicial</a>&nbsp;&nbsp;&nbsp;
		<a href="sessaoencerra.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="encerra a sess�o do usu�rio" class="LSessaoEncerra">encerra</a>
		</span></td>
</tr>
</table>


<!--  RELA��O DE AVISOS  -->
<br>
<center>
<form method="post" action="quadroavisoedita.asp" id="fOP" name="fOP">
<%=MontaCampoFormSessionCtrlInfo(Session("SessionCtrlInfo"))%>
<input type="hidden" name="aviso_selecionado" id="aviso_selecionado" value=''>
<input type="hidden" name="operacao_selecionada" id="operacao_selecionada" value='<%=OP_CONSULTA%>'>
<% executa_consulta %>
</form>

<br>

<p class="TracoBottom"></p>

<table class="notPrint" cellSpacing="0">
<tr>
	<td align="center"><a href="quadroaviso.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="volta para a p�gina anterior">
		<img src="../botao/voltar.gif" width="176" height="55" border="0"></a></td>
</tr>
</table>

</center>

</body>
</html>


<%
'	FECHA CONEXAO COM O BANCO DE DADOS
	cn.Close
	set cn = nothing
%>