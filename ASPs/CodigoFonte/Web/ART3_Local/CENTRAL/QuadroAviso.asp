<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp"        -->

<!-- #include file = "../global/TrataSessaoExpirada.asp"        -->

<%

'     =============================
'	  Q U A D R O A V I S O . A S P
'     =============================
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
	dim s, usuario, usuario_nome
	usuario = trim(Session("usuario_atual"))
	usuario_nome = Trim(Session("usuario_nome_atual"))
	If (usuario = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO) 

	dim s_lista_operacoes_permitidas
	s_lista_operacoes_permitidas = Trim(Session("lista_operacoes_permitidas"))
	
'	VERIFICA PERMISS�O DE ACESSO DO USU�RIO
	if Not operacao_permitida(OP_CEN_CADASTRO_AVISOS, s_lista_operacoes_permitidas) then 
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
var s_dest, s_op, b;
	
	s_dest="";
	s_op="";
	
	if (f.rb_op[0].checked) {
		s_dest="quadroavisoedita.asp";
		s_op=OP_INCLUI;
		}
		
	if (f.rb_op[1].checked) {
		s_dest="quadroavisolista.asp";
		}

	if (f.rb_op[2].checked) {
		s_dest="QuadroAvisoLimpaAntigos.asp";
		if (trim(f.c_dt_corte.value)=="") {
			alert("Especifique a data de corte para apagar as mensagens antigas!!");
			f.c_dt_corte.focus();
			return;
			}
		else if (!isDate(f.c_dt_corte)) {
			alert("Data de corte � uma data inv�lida!!");
			f.c_dt_corte.focus();
			return;
			}
		b=window.confirm('Confirma a limpeza das mensagens anteriores a ' + f.c_dt_corte.value + '?');
		if (!b) return;
		}
		
	if (s_dest=="") {
		alert("Escolha uma das op��es!!");
		return false;
		}
	
	window.status = "Aguarde ...";
	f.operacao_selecionada.value=s_op;
	f.action=s_dest;
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



<body onload="focus();">

<!--  MENU SUPERIOR -->
<table width="100%" cellPadding="4" CellSpacing="0" style="border-bottom:1px solid black">

<tr>
	<td align="right" valign="bottom"><p class="PEDIDO">CENTRAL&nbsp;&nbsp;ADMINISTRATIVA<br>
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
<input type="hidden" name="operacao_selecionada" id="operacao_selecionada" value=''>

<span class="T">CADASTRO DO QUADRO DE AVISOS</span>
<div class="QFn" align="CENTER">
<table class="TFn">
	<tr>
		<td NOWRAP>
			<input type="radio" id="rb_op" name="rb_op" value="1" class="CBOX"><span class="rbLink" onclick="fOP.rb_op[0].click(); fOP.bEXECUTAR.click();">Cadastrar Novo</span><br>
			<input type="radio" id="rb_op" name="rb_op" value="2" class="CBOX"><span class="rbLink" onclick="fOP.rb_op[1].click(); fOP.bEXECUTAR.click();">Consultar Lista</span><br>
			<input type="radio" id="rb_op" name="rb_op" value="3" class="CBOX"><span class="rbLink" onclick="fOP.rb_op[2].click(); fOP.bEXECUTAR.click();">Apagar mensagens anteriores a&nbsp;</span><input class="Cc" maxlength="10" style="width:70px;" name="c_dt_corte" id="c_dt_corte" onblur="if (!isDate(this)) {alert('Data inv�lida!'); this.focus();}" onkeypress="if (digitou_enter(true)) fOP.bEXECUTAR.focus(); else fOP.rb_op[2].click(); filtra_data();" onclick="fOP.rb_op[2].click();">
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
	<td align="center"><a href="MenuCadastro.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="volta para a p�gina anterior">
		<img src="../botao/voltar.gif" width="176" height="55" border="0"></a></td>
</tr>
</table>

</center>

</body>
</html>
