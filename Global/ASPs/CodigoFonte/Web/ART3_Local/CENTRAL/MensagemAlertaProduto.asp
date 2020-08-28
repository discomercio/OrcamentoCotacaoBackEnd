<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp"        -->

<!-- #include file = "../global/TrataSessaoExpirada.asp"        -->

<%

'     ===========================
'	  MensagemAlertaProduto.asp
'     ===========================
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
'						I N I C I A L I Z A     P � G I N A     A S P
' _____________________________________________________________________________________________


	On Error GoTo 0
	Err.Clear
	
'	OBTEM USU�RIO
	dim s, usuario, usuario_nome, intIdx
	usuario = trim(Session("usuario_atual"))
	usuario_nome = Trim(Session("usuario_nome_atual"))
	If (usuario = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO) 

'	VERIFICA PERMISS�O DE ACESSO DO USU�RIO
	dim s_lista_operacoes_permitidas
	s_lista_operacoes_permitidas = Trim(Session("lista_operacoes_permitidas"))
	
	if Not operacao_permitida(OP_CEN_CAD_MENSAGEM_ALERTA_PRODUTOS, s_lista_operacoes_permitidas) then 
		Response.Redirect("aviso.asp?id=" & ERR_ACESSO_INSUFICIENTE)
		end if

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

<script src="<%=URL_FILE__GLOBAL_JS%>" Language="JavaScript" Type="text/javascript"></script>

<script language="JavaScript" type="text/javascript">
function fOPConcluir( f ){
var intIdx,s_dest,s_op,s_apelido;
	
	s_dest="";
	s_op="";
	s_apelido="";
	
	intIdx=0;
	if (f.rb_op[intIdx].checked) {
		s_dest="MensagemAlertaProdutoEdita.asp";
		s_op=OP_INCLUI;
		s_apelido=f.c_novo.value;
		if (trim(f.c_novo.value)=="") {
			alert("Forne�a o identificador para a nova mensagem de alerta!!");
			f.c_novo.focus();
			return false;
			}
		}
	
	intIdx++;
	if (f.rb_op[intIdx].checked) {
		s_dest="MensagemAlertaProdutoEdita.asp";
		s_op=OP_CONSULTA;
		s_apelido=f.c_consulta.value;
		if (trim(f.c_consulta.value)=="") {
			alert("Especifique a mensagem a ser consultada!!");
			f.c_consulta.focus();
			return false;
			}
		}
	
	intIdx++;
	if (f.rb_op[intIdx].checked) {
		s_dest="MensagemAlertaProdutoLista.asp";
		}

	if (s_dest=="") {
		alert("Escolha uma das op��es!!");
		return false;
		}
	
	f.alerta_selecionado.value=s_apelido;
	f.operacao_selecionada.value=s_op;
	
	f.action=s_dest;
	window.status = "Aguarde ...";
	f.submit(); 
}

</script>



<!-- C A S C A D I N G   S T Y L E   S H E E T

	 CCCCCCC    SSSSSSS    SSSSSSS
	CCC   CCC  SSS   SSS  SSS   SSS
	CCC        SSS        SSS
	CCC         SSSS       SSSS
	CCC            SSSS       SSSS
	CCC   CCC  SSS   SSS  SSS   SSS
	 CCCCCCC    SSSSSSS    SSSSSSS
-->

<link href="<%=URL_FILE__E_CSS%>" Rel="stylesheet" Type="text/css">



<body onload="focus()">

<!--  MENU SUPERIOR -->
<table width="100%" cellPadding="4" CellSpacing="0" style="border-bottom:1px solid black">

<tr>
	<td align="RIGHT" vAlign="BOTTOM"><p class="PEDIDO">CENTRAL&nbsp;&nbsp;ADMINISTRATIVA<br>
	<%	s = usuario_nome
		if s = "" then s = usuario
		s = x_saudacao & ", " & s
		s = "<span class='Cd'>" & s & "</span><br>"
	%>
	<%=s%>
	<span class="Rc">
		<a href="resumo.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="retorna para p�gina inicial" class="LPagInicial">p�gina inicial</a>&nbsp;&nbsp;&nbsp;
		<a href="senha.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="altera a senha atual do usu�rio" class="LAlteraSenha">altera senha</a>&nbsp;&nbsp;&nbsp;
		<a href="sessaoencerra.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="encerra a sess�o do usu�rio" class="LSessaoEncerra">encerra</a>
		</span></p></td>
	</tr>

</table>


<center>
<!--  ***********************************************************************************************  -->
<!--  F O R M U L � R I O                         												       -->
<!--  ***********************************************************************************************  -->
<form method="post" id="fOP" name="fOP" onsubmit="if (!fOPConcluir(fOP)) return false">
<%=MontaCampoFormSessionCtrlInfo(Session("SessionCtrlInfo"))%>
<input type="hidden" name='alerta_selecionado' id="alerta_selecionado" value=''>
<input type="hidden" name='operacao_selecionada' id="operacao_selecionada" value=''>

<span class="T">Mensagens de Alerta para Produtos</span>
<div class="QFn" align="CENTER">
<table class="TFn">
	<tr>
		<td NOWRAP>
			<% intIdx = 0 %>
			<input type="radio" id="rb_op" name="rb_op" value="NOVO" class="CBOX" onclick="fOP.c_novo.focus();"><span style="cursor:default" onclick="fOP.rb_op[<%=CStr(intIdx)%>].click(); fOP.c_novo.focus();">Cadastrar Nova</span>&nbsp;
				<input name="c_novo" id="c_novo" type="text" maxlength="12" size="14" onblur="this.value=trim(this.value);" onclick="fOP.rb_op[<%=CStr(intIdx)%>].click();" onkeypress="this.click(); if (digitou_enter(true) && tem_info(this.value)) {this.value=trim(this.value); fOPConcluir(fOP);} filtra_alfanumerico();"><br>
			<% intIdx = intIdx + 1 %>
			<input type="radio" id="rb_op" name="rb_op" value="CONSULTA" class="CBOX" onclick="fOP.c_consulta.focus();"><span style="cursor:default" onclick="fOP.rb_op[<%=CStr(intIdx)%>].click(); fOP.c_consulta.focus();">Consultar</span>&nbsp;
				<input name="c_consulta" id="c_consulta" type="text" maxlength="12" size="14" onblur="this.value=trim(this.value);" onclick="fOP.rb_op[<%=CStr(intIdx)%>].click();" onkeypress="this.click(); if (digitou_enter(true) && tem_info(this.value)) {this.value=trim(this.value); fOPConcluir(fOP);} filtra_alfanumerico();"><br>
			<% intIdx = intIdx + 1 %>
			<input type="radio" id="rb_op" name="rb_op" value="LISTA" class="CBOX"><span class="rbLink" onclick="fOP.rb_op[<%=CStr(intIdx)%>].click(); fOP.bEXECUTAR.click();">Consultar Lista</span>
			</td>
		</tr>
	</table>

	<span class="R" style="margin: 4 10 0 10">&nbsp;</span>
	<input name="bEXECUTAR" id="bEXECUTAR" type="submit" class="Botao" value="EXECUTAR" title="executa">
	<p class="R" style="margin: 0 10 0 10">&nbsp;</p>

</div>
</form>

<p class="TracoBottom"></p>

<table cellSpacing="0">
<tr>
	<td align="CENTER"><a href="MenuCadastro.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="volta para a p�gina anterior">
		<img src="../botao/voltar.gif" width="176" height="55" border="0"></a></td>
</tr>
</table>

</center>

</body>
</html>