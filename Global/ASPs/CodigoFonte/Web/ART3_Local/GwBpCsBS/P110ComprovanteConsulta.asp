<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<% Response.Buffer=True %>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp" -->
<!-- #include file = "../global/Braspag.asp"    -->

<%
'     ===========================================
'	  P110ComprovanteConsulta.asp 
'     ===========================================
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


	On Error GoTo 0
	Err.Clear

	dim s, usuario
	usuario = BRASPAG_USUARIO_CLIENTE

	dim alerta
	alerta = ""

	dim id_pagto_gw_pag
	id_pagto_gw_pag = Trim(Request("id_pagto_gw_pag"))
	
	if id_pagto_gw_pag = "" then alerta = "Identificador do registro n�o informado."

	dim pedido_selecionado, cnpj_cpf_selecionado, id_pedido_base
	pedido_selecionado = Trim(Request("pedido_selecionado"))
	id_pedido_base = retorna_num_pedido_base(pedido_selecionado)
	cnpj_cpf_selecionado = Trim(Request("cnpj_cpf_selecionado"))

	
'	CONECTA AO BANCO DE DADOS
'	=========================
	dim cn, t_PAG, t_PAG_PAYMENT, msg_erro
	If Not bdd_conecta(cn) then Response.Redirect("aviso.asp?id=" & ERR_CONEXAO)
	If Not cria_recordset_otimista(t_PAG, msg_erro) then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_CRIAR_ADO)
	If Not cria_recordset_otimista(t_PAG_PAYMENT, msg_erro) then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_CRIAR_ADO)
	
'	IDENTIFICA AS TRANSA��ES QUE N�O FORAM BEM SUCEDIDAS P/ OCULT�-LAS NO RECIBO (EM PAGAMENTOS QUE USAM MAIS DE 1 CART�O)
	dim strScriptJS
	strScriptJS = ""
	if alerta = "" then
		s = "SELECT" & _
				" id" & _
			" FROM t_PAGTO_GW_PAG_PAYMENT" & _
			" WHERE" & _
				" (id_pagto_gw_pag = " & id_pagto_gw_pag & ")" & _
				" AND (" & _
					"(ult_GlobalStatus <> '" & BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__CAPTURADA & "')" & _
					" AND " & _
					"(ult_GlobalStatus <> '" & BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__AUTORIZADA & "')" & _
					")"
		if t_PAG_PAYMENT.State <> 0 then t_PAG_PAYMENT.Close
		t_PAG_PAYMENT.Open s, cn
		do while Not t_PAG_PAYMENT.Eof
			strScriptJS = strScriptJS & _
						"		$('.trBoxTrxId_" & t_PAG_PAYMENT("id") & "').hide();" & chr(13)
			t_PAG_PAYMENT.MoveNext
			loop
		
		if strScriptJS <> "" then
			strScriptJS = "<script type=" & chr(34) & "text/javascript" & chr(34) & ">" & chr(13) & _
							"	$(function() {" & chr(13) & _
								strScriptJS & _
							"	});" & chr(13) & _
							"</script>" & chr(13)
			end if
		end if

	dim recibo_url_css, recibo_html, s_link_css
	recibo_url_css = ""
	recibo_html = ""
	s_link_css = ""
	if alerta = "" then
		s = "SELECT * FROM t_PAGTO_GW_PAG WHERE (id = " & id_pagto_gw_pag & ")"
		if t_PAG.State <> 0 then t_PAG.Close
		t_PAG.Open s, cn
		if Not t_PAG.Eof then
			recibo_url_css = Trim("" & t_PAG("recibo_url_css"))
			recibo_html = Trim("" & t_PAG("recibo_html"))
			s_link_css = "<link href=" & chr(34) & recibo_url_css & chr(34) & " rel=" & chr(34) & " stylesheet" & chr(34) & " type=" & chr(34) & "text/css" & chr(34) & ">"
			end if
		end if
	
	if alerta = "" then
		if recibo_html = "" then alerta = "O comprovante n�o est� dispon�vel para reimpress�o"
		end if

	dim strScriptWindowName
	strScriptWindowName = _
				"<script language='JavaScript'>" & chr(13) & _
				"	window.name = '" & SITE_CLIENTE_TITULO_JANELA & "';" & chr(13) & _
				"</script>" & chr(13)
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


<%=DOCTYPE_LEGADO%>

<html>


<head>
	<title><%=SITE_CLIENTE_TITULO_JANELA%></title>
	</head>


<script src="<%=URL_FILE__JQUERY%>" language="JavaScript" type="text/javascript"></script>
<script src="<%=URL_FILE__GLOBAL_JS%>" language="JavaScript" type="text/javascript"></script>
<script src="<%=URL_FILE__SSL_JS%>" Language="JavaScript" Type="text/javascript"></script>

<% =strScriptWindowName %>

<script language="JavaScript" type="text/javascript">
window.status = '';

function Navega(url) {
	window.location.href = url;
}
</script>

<%=strScriptJS%>



<!-- C A S C A D I N G   S T Y L E   S H E E T

	 CCCCCCC    SSSSSSS    SSSSSSS
	CCC   CCC  SSS   SSS  SSS   SSS
	CCC        SSS        SSS
	CCC         SSSS       SSSS
	CCC            SSSS       SSSS
	CCC   CCC  SSS   SSS  SSS   SSS
	 CCCCCCC    SSSSSSS    SSSSSSS
-->

<link href="<%=URL_FILE__E_CSS%>" rel="stylesheet" type="text/css">
<link href="<%=URL_FILE__EPRINTER_CSS%>" rel="stylesheet" type="text/css" media="print">
<link href="<%=URL_FILE__E_LOGO_TOP_BS_CSS%>" Rel="stylesheet" Type="text/css">
<%=s_link_css%>

<style type="text/css">
body::before
{
	content: '';
	border: none;
	margin-top: 0px;
	margin-bottom: 0px;
	padding: 0px;
}
</style>



<% if alerta <> "" then %>
<!-- ************************************************************ -->
<!-- **********  P�GINA PARA EXIBIR MENSAGENS DE ERRO  ********** -->
<!-- ************************************************************ -->
<body onload="bVOLTAR.focus();window.status='';" link=#000000 alink=#000000 vlink=#000000>
<center>
<br>
<!--  T E L A  -->
<p class="T">A V I S O</p>
<div class="MtAlerta" style="width:600px;font-weight:bold;" align="center"><p style='margin:5px 2px 5px 2px;'><%=alerta%></p></div>
<br><br>
<p class="TracoBottom"></p>
<table cellspacing="0">
<tr>
	<td align="center"><a name="bVOLTAR" href="javascript:history.back();"><img src="../botao/voltar.gif" width="176" height="55" border="0"></a></td>
</tr>
</table>
</center>
</body>



<% else %>
<!-- ****************************************************************** -->
<!-- **********  P�GINA PARA EXIBIR DADOS DO RETORNO  ***************** -->
<!-- ****************************************************************** -->
<body onload="window.status='';">
<center>

<table id="tbl_logotipo_bonshop" width="100%" cellpadding="0" cellspacing="0" border="0">
	<tr>
		<td align="center"><img alt="<%=SITE_CLIENTE_HEADER__ALT_IMG_TEXT%>" src="../imagem/<%=SITE_CLIENTE_HEADER__LOGOTIPO%>" /></td>
	</tr>
</table>
<table id="pagina_tbl_cabecalho" cellspacing="0px" cellpadding="0px">
	<tbody>
		<tr style="height:78px;">
			<td id="topo_verde" colspan="3">
				<div id="moldura_do_letreiro">
					<div id="letreiro_div" style="display:block;"></div>
				</div>
				<div id="telefone"></div>
			</td>
		</tr>
		<tr>
			<td id="topo_azul" colspan="3">&nbsp;</td>
		</tr>
	</tbody>
</table>

<br />
<br />

<%=recibo_html%>

<br />


<!-- ************   SEPARADOR   ************ -->
<table class="notPrint" width="649" cellpadding="4" cellspacing="0" style="border-bottom:1px solid black">
<tr><td class="Rc" align="left">&nbsp;</td></tr>
</table>

<!-- ************   BOT�ES   ************ -->
<table class="notPrint" width="649" cellpadding="0" cellspacing="0">
<tr>
	<td align="left"><a name="bVOLTAR" href="javascript:history.back();" title="volta para p�gina inicial">
		<img src="../botao/voltar.gif" width="176" height="55" border="0"></a>
	</td>
	<td align="right">
		<a name="bIMPRIMIR" href="javascript:window.print();"><img src="../botao/imprimir.gif" width="176" height="55" border="0"></a>
	</td>
</tr>
</table>

</center>

<% if SITE_CLIENTE_EXIBIR_LOGO_SSL then %>
<script language="JavaScript" type="text/javascript">
	logo_ssl_corner("../imagem/ssl/ssl_corner.gif");
</script>
<% end if %>

</body>
<% end if %>

</html>


<%

'	FECHA CONEXAO COM O BANCO DE DADOS
	if t_PAG_PAYMENT.State <> 0 then t_PAG_PAYMENT.Close
	set t_PAG_PAYMENT=nothing

	if t_PAG.State <> 0 then t_PAG.Close
	set t_PAG=nothing

	cn.Close
	set cn = nothing
	
%>