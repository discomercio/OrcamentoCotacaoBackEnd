<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp" -->

<!-- #include file = "../global/TrataSessaoExpirada.asp"        -->

<%
'     ============================================
'	  CadIndicadorOpcoesFormaComoConheceuLista.asp
'     ============================================
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

'	CABE�ALHO
	cab =	"<TABLE class='Q' cellSpacing=0>" & chr(13) & _
			"	<TR style='background: #FFF0E0' NOWRAP>" & chr(13) & _
			"		<TD width='250' NOWRAP class='MD MB'><P class='R' style='cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='CadIndicadorOpcoesFormaComoConheceuLista.asp?ord=1" & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">&nbsp;OP��O</P></TD>" & chr(13) & _
			"		<TD width='50' NOWRAP class='MB'><P class='R' style='cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='CadIndicadorOpcoesFormaComoConheceuLista.asp?ord=2" & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">&nbsp;STATUS</P></TD>" & chr(13) & _
			"	</TR>" & chr(13)

	consulta= "SELECT " & _
				"*" & _
			" FROM t_CODIGO_DESCRICAO" & _
			" WHERE" & _
				" (grupo = '" & GRUPO_T_CODIGO_DESCRICAO__CAD_ORCAMENTISTA_E_INDICADOR__FORMA_COMO_CONHECEU & "')" & _
			" ORDER BY "
	
	select case ordenacao_selecionada
		case "1": consulta = consulta & "descricao"
		case "2": consulta = consulta & "st_inativo, descricao"
		case else: consulta = consulta & "descricao"
		end select

'	EXECUTA CONSULTA
	x=cab
	i=0
	
	set r = cn.Execute( consulta )

	do while not r.eof
	'	CONTAGEM
		i = i + 1

	'	ALTERN�NCIA NAS CORES DAS LINHAS
		if (i AND 1)=0 then
			x = x & "	<TR NOWRAP style='background: #FFF0E0'>" & chr(13)
		else
			x = x & "	<TR NOWRAP>" & chr(13)
			end if

 	 '> DESCRI��O
		x = x & "		<TD class='MDB' NOWRAP><P class='C' NOWRAP>&nbsp;" & _
			"<a href='javascript:fOPConcluir(" & chr(34) & r("codigo") & chr(34) & ")' title='clique para consultar o cadastro deste item'>" & _
			r("descricao") & _
			"</a>" & _
			"</P></TD>" & chr(13)

 	 '> STATUS
 		if r("st_inativo")=0 then 
 			s="<span style='color:#006600'>Dispon�vel</span>"
 		else 
 			s="<span style='color:#ff0000'>Indispon�vel</span>"
 			end if
		x = x & "		<TD class='MB' NOWRAP><P class='C'>&nbsp;" & _
			"<a href='javascript:fOPConcluir(" & chr(34) & r("codigo") & chr(34) & ")' title='clique para consultar o cadastro deste item'>" & _
			s & _
			"</a>" & _
			"</P></TD>" & chr(13)

		x = x & "	</TR>" & chr(13)

		if (i mod 100) = 0 then
			Response.Write x
			x = ""
			end if

		r.MoveNext
		loop


'	MOSTRA TOTAL DE REGISTROS
	x = x & _
		"	<TR NOWRAP style='background: #FFFFDD'>" & chr(13) & _
		"		<TD COLSPAN='2' NOWRAP><P class='Cd'>" & "TOTAL:&nbsp;&nbsp;&nbsp;" & cstr(i) & "&nbsp;&nbsp;registro(s)" & "</P></TD>" & chr(13) & _
		"	</TR>" & chr(13)

'	FECHA TABELA
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
	<td align="RIGHT" vAlign="BOTTOM" NOWRAP><span class="PEDIDO">Op��es para "Forma como conheceu a Bonshop"<br />(Cadastro de Indicadores)</span>
	<br><span class="Rc">
		<a href="resumo.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="retorna para p�gina inicial" class="LPagInicial">p�gina inicial</a>&nbsp;&nbsp;&nbsp;
		<a href="sessaoencerra.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="encerra a sess�o do usu�rio" class="LSessaoEncerra">encerra</a>
		</span></td>
</tr>
</table>


<!--  RELA��O DE OP��ES CADASTRADAS  -->
<br>
<center>
<form METHOD="POST" ACTION="CadIndicadorOpcoesFormaComoConheceuEdita.asp" id="fOP" name="fOP">
<%=MontaCampoFormSessionCtrlInfo(Session("SessionCtrlInfo"))%>
<INPUT type=HIDDEN name='id_selecionado' id="id_selecionado" value=''>
<INPUT type=HIDDEN name='operacao_selecionada' id="operacao_selecionada" value='<%=OP_CONSULTA%>'>
<% executa_consulta %>
</form>

<br>

<p class="TracoBottom"></p>

<table class="notPrint" cellSpacing="0">
<tr>
	<td align="CENTER"><a href="CadIndicadorOpcoesFormaComoConheceu.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="volta para a p�gina anterior">
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