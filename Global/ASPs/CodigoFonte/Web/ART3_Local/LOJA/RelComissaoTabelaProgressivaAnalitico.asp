<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<% Response.Buffer=True %>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp"        -->

<!-- #include file = "../global/TrataSessaoExpirada.asp"        -->

<%
'     ===============================================
'	  RelComissaoTabelaProgressivaAnalitico.asp
'     ===============================================
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


	On Error GoTo 0
	Err.Clear

	dim usuario, loja
	usuario = Trim(Session("usuario_atual"))
	loja = Trim(Session("loja_atual"))
	If (usuario = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO) 
	If (loja = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO) 

'	CONECTA COM O BANCO DE DADOS
	dim cn
	If Not bdd_conecta(cn) then Response.Redirect("aviso.asp?id=" & ERR_CONEXAO)

	dim s_lista_operacoes_permitidas
	s_lista_operacoes_permitidas = Trim(Session("lista_operacoes_permitidas"))

	dim dtMinDtInicialFiltroPeriodo, intMaxDiasDtInicialFiltroPeriodo
	dim strMinDtInicialFiltroPeriodoYYYYMMDD, strMinDtInicialFiltroPeriodoDDMMYYYY
	if operacao_permitida(OP_LJA_RESTRINGE_DT_INICIAL_FILTRO_PERIODO, s_lista_operacoes_permitidas) then
		intMaxDiasDtInicialFiltroPeriodo = obtem_max_dias_dt_inicial_filtro_periodo()
		dtMinDtInicialFiltroPeriodo = Date - intMaxDiasDtInicialFiltroPeriodo
		strMinDtInicialFiltroPeriodoYYYYMMDD = formata_data_yyyymmdd(dtMinDtInicialFiltroPeriodo)
		strMinDtInicialFiltroPeriodoDDMMYYYY = formata_data(dtMinDtInicialFiltroPeriodo)
	else
		strMinDtInicialFiltroPeriodoYYYYMMDD = ""
		strMinDtInicialFiltroPeriodoDDMMYYYY = ""
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
	<title>LOJA</title>
	</head>



<script src="<%=URL_FILE__GLOBAL_JS%>" Language="JavaScript" Type="text/javascript"></script>

<script language="JavaScript" type="text/javascript">
function fFILTROConfirma( f ) {
var s_de, s_ate;
var strDtRefYYYYMMDD, strDtRefDDMMYYYY;

	if (trim(f.c_dt_inicio.value)=="") {
		alert("Preencha a data de in�cio do per�odo!!");
		f.c_dt_inicio.focus();
		return;
		}

	if (trim(f.c_dt_termino.value)=="") {
		alert("Preencha a data de t�rmino do per�odo!!");
		f.c_dt_termino.focus();
		return;
		}

	if (trim(f.c_dt_inicio.value)!="") {
		if (!isDate(f.c_dt_inicio)) {
			alert("Data de in�cio inv�lida!!");
			f.c_dt_inicio.focus();
			return;
			}
		}

	if (trim(f.c_dt_termino.value)!="") {
		if (!isDate(f.c_dt_termino)) {
			alert("Data de t�rmino inv�lida!!");
			f.c_dt_termino.focus();
			return;
			}
		}

	s_de = trim(f.c_dt_inicio.value);
	s_ate = trim(f.c_dt_termino.value);
	if ((s_de!="")&&(s_ate!="")) {
		s_de=retorna_so_digitos(formata_ddmmyyyy_yyyymmdd(s_de));
		s_ate=retorna_so_digitos(formata_ddmmyyyy_yyyymmdd(s_ate));
		if (s_de > s_ate) {
			alert("Data de t�rmino � menor que a data de in�cio!!");
			f.c_dt_termino.focus();
			return;
			}
		}

//  Per�odo de consulta est� restrito por perfil de acesso?
	if (trim(f.c_MinDtInicialFiltroPeriodoYYYYMMDD.value)!="") {
		strDtRefDDMMYYYY = trim(f.c_dt_inicio.value);
		if (trim(strDtRefDDMMYYYY)!="") {
			strDtRefYYYYMMDD = retorna_so_digitos(formata_ddmmyyyy_yyyymmdd(strDtRefDDMMYYYY));
			if (strDtRefYYYYMMDD < f.c_MinDtInicialFiltroPeriodoYYYYMMDD.value) {
				alert("Data inv�lida para consulta: " + strDtRefDDMMYYYY + "\nO per�odo de consulta n�o pode compreender datas anteriores a " + f.c_MinDtInicialFiltroPeriodoDDMMYYYY.value + "!!");
				return;
				}
			}
		strDtRefDDMMYYYY = trim(f.c_dt_termino.value);
		if (trim(strDtRefDDMMYYYY)!="") {
			strDtRefYYYYMMDD = retorna_so_digitos(formata_ddmmyyyy_yyyymmdd(strDtRefDDMMYYYY));
			if (strDtRefYYYYMMDD < f.c_MinDtInicialFiltroPeriodoYYYYMMDD.value) {
				alert("Data inv�lida para consulta: " + strDtRefDDMMYYYY + "\nO per�odo de consulta n�o pode compreender datas anteriores a " + f.c_MinDtInicialFiltroPeriodoDDMMYYYY.value + "!!");
				return;
				}
			}
		}

	dCONFIRMA.style.visibility="hidden";
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


<body onload="fFILTRO.c_dt_inicio.focus();">
<center>

<form id="fFILTRO" name="fFILTRO" method="post" action="RelComissaoTabelaProgressivaAnaliticoExec.asp">
<%=MontaCampoFormSessionCtrlInfo(Session("SessionCtrlInfo"))%>
<input type="hidden" name="c_MinDtInicialFiltroPeriodoYYYYMMDD" id="c_MinDtInicialFiltroPeriodoYYYYMMDD" value='<%=strMinDtInicialFiltroPeriodoYYYYMMDD%>'>
<input type="hidden" name="c_MinDtInicialFiltroPeriodoDDMMYYYY" id="c_MinDtInicialFiltroPeriodoDDMMYYYY" value='<%=strMinDtInicialFiltroPeriodoDDMMYYYY%>'>

<!--  I D E N T I F I C A � � O   D A   T E L A  -->
<table width="649" cellPadding="4" CellSpacing="0" style="border-bottom:1px solid black">
<tr>
	<td align="right" valign="bottom"><span class="PEDIDO">Comiss�o aos Vendedores Anal�tico (Tabela Progressiva)</span>
	<br><span class="Rc">
		<a href="resumo.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="retorna para p�gina inicial" class="LPagInicial">p�gina inicial</a>&nbsp;&nbsp;&nbsp;
		<a href="sessaoencerra.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="encerra a sess�o do usu�rio" class="LSessaoEncerra">encerra</a>
		</span></td>
</tr>
</table>
<br>

<!--  TEXTO EXPLICATIVO -->  
<table width="350" cellPadding="0" CellSpacing="0">
<tr><td><p class="Expl">DICA</p></td></tr>
<tr><td>
	<p class="Expl">O caracter asterisco (*) pode ser usado para indicar que se deseja aceitar qualquer texto em seu lugar.</p>
	</td>
</tr>
</table>
<br>

<!--  PER�ODO  -->
<table class="Qx" cellSpacing="0">
	<tr bgColor="#FFFFFF">
	<td class="MT" NOWRAP><span class="PLTe">PER�ODO</span>
	<br>
		<table cellSpacing="0" cellPadding="0"><tr bgColor="#FFFFFF"><td>
		<input class="PLLc" maxlength="10" style="width:70px;" name="c_dt_inicio" id="c_dt_inicio" onblur="if (!isDate(this)) {alert('Data de in�cio inv�lida!'); this.focus();}" onkeypress="if (digitou_enter(true)) fFILTRO.c_dt_termino.focus(); filtra_data();"
			>&nbsp;<span class="PLLc" style="color:#808080;">&nbsp;at�&nbsp;</span>&nbsp;<input class="PLLc" maxlength="10" style="width:70px;" name="c_dt_termino" id="c_dt_termino" onblur="if (!isDate(this)) {alert('Data de t�rmino inv�lida!'); this.focus();}" onkeypress="if (digitou_enter(true)) fFILTRO.c_vendedor.focus(); filtra_data();">
			</td></tr>
		</table>
		</td></tr>

<!--  ID VENDEDOR  -->
	<tr bgColor="#FFFFFF">
	<td class="MDBE" NOWRAP><span class="PLTe">VENDEDOR</span>
	<br>
		<input maxlength="10" class="PLLe" style="width:150px;" name="c_vendedor" id="c_vendedor" onblur="this.value=ucase(this.value);" onkeypress="if (digitou_enter(true)) {bCONFIRMA.focus();bCONFIRMA.click();} filtra_nome_identificador();">
		</td></tr>

</table>

<!-- ************   SEPARADOR   ************ -->
<table width="649" cellPadding="4" CellSpacing="0" style="border-bottom:1px solid black">
<tr><td class="Rc">&nbsp;</td></tr>
</table>
<br>


<table width="649" cellSpacing="0">
<tr>
	<td><a name="bVOLTAR" id="bVOLTAR" href="javascript:history.back()" title="volta para a p�gina anterior">
		<img src="../botao/voltar.gif" width="176" height="55" border="0"></a></td>
	<td align="right"><div name="dCONFIRMA" id="dCONFIRMA"><a name="bCONFIRMA" id="bCONFIRMA" href="javascript:fFILTROConfirma(fFILTRO)" title="executa a consulta">
		<img src="../botao/confirmar.gif" width="176" height="55" border="0"></a></div>
	</td>
</tr>
</table>
</form>

</center>
</body>
</html>

<%
'	FECHA CONEXAO COM O BANCO DE DADOS
	cn.Close
	set cn = nothing
%>
