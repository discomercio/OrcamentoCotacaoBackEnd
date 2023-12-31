<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<% Response.Buffer=True %>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp" -->

<!-- #include file = "../global/TrataSessaoExpirada.asp"        -->

<%
'     ====================================================
'	  P E R D A C O N S I S T E . A S P
'     ====================================================
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

	dim s, usuario, pedido_selecionado, s_valor, m_valor, c_obs, cadastrado
	usuario = Trim(Session("usuario_atual"))
	If (usuario = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO) 
	
'	CONECTA AO BANCO DE DADOS
'	=========================
	dim cn, rs
	If Not bdd_conecta(cn) then Response.Redirect("aviso.asp?id=" & ERR_CONEXAO)

	pedido_selecionado = ucase(Trim(request("c_pedido")))
	if (pedido_selecionado = "") then Response.Redirect("aviso.asp?id=" & ERR_PEDIDO_NAO_ESPECIFICADO)
	s = normaliza_num_pedido(pedido_selecionado)
	if s <> "" then pedido_selecionado = s

	s_valor = Trim(Request.Form("c_valor"))
	m_valor = converte_numero(s_valor)

	c_obs = Trim(Request.Form("c_obs"))

	dim alerta
	alerta = ""

	dim observacoes
	observacoes = ""
	
	if m_valor = 0 then
		alerta = "Valor inv�lido."
		end if
		
	if alerta = "" then
		s = "SELECT pedido FROM t_PEDIDO WHERE (pedido='" & pedido_selecionado & "')"
		set rs = cn.execute(s)
		if rs.Eof then
			alerta = "Pedido " & pedido_selecionado & " n�o est� cadastrado."
			end if
		end if

	dim vl_perda, vl_TotalFamiliaPrecoVenda, vl_TotalFamiliaPrecoNF, vl_TotalFamiliaPago, vl_TotalFamiliaDevolucaoPrecoVenda, vl_TotalFamiliaDevolucaoPrecoNF, st_pagto, v_pedido, msg_erro
	
	if alerta = "" then
	'	OBT�M OS VALORES A PAGAR, J� PAGO E O STATUS DE PAGAMENTO (PARA TODA A FAM�LIA DE PEDIDOS)
		if Not calcula_pagamentos(pedido_selecionado, vl_TotalFamiliaPrecoVenda, vl_TotalFamiliaPrecoNF, vl_TotalFamiliaPago, vl_TotalFamiliaDevolucaoPrecoVenda, vl_TotalFamiliaDevolucaoPrecoNF, st_pagto, msg_erro) then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_BD)
		if Not recupera_familia_pedido(pedido_selecionado, v_pedido, msg_erro) then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_BD)
		if Not calcula_valor_em_perdas(pedido_selecionado, vl_perda, msg_erro) then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_BD)

		if (m_valor + vl_TotalFamiliaDevolucaoPrecoNF + vl_perda) > vl_TotalFamiliaPrecoNF then
			observacoes = texto_add_br(observacoes)
			observacoes = observacoes & "O valor total em perdas e devolu��es ir� superar o valor do pedido."
			end if
		end if





' _____________________________________________________________________________________________
'
'									F  U  N  �  �  E  S 
' _____________________________________________________________________________________________

' ___________________________________
' EXIBE_FAMILIA_PEDIDO
'
function exibe_familia_pedido(byval pedido_selecionado, byref v_pedido)
const PEDIDOS_POR_LINHA = 3
dim i
dim n
dim x
	exibe_familia_pedido = ""

	x = "<table width='258' class='Q' cellspacing='0' cellpadding='0'>" & chr(13) & _
		"	<tr>" & chr(13)& _
		"		<td>" & chr(13) & _
		"			<p class='Rf'>Fam�lia de Pedidos</p>" & chr(13) & _
		"		</td>" & chr(13) & _
		"	</tr>" & chr(13) & _
		"	<tr>" & chr(13) & _
		"		<td>" & chr(13) & _
		"			<table width='100%' class='QT' cellSpacing='0'>" & chr(13) & _
		"				<tr>" & chr(13)
	
	n = 0
	for i = Lbound(v_pedido) to Ubound(v_pedido)
		if Trim(v_pedido(i))<>"" then
			n = n+1
			if n > PEDIDOS_POR_LINHA then 
				n = 1
				x = x & "				</tr>" & chr(13) & _
						"				<tr>" & chr(13)
				end if

			x = x & "					<td width='33.3%' class='L' style='text-align:left;"
			if Trim(v_pedido(i)) = pedido_selecionado then
				x = x & "color:navy;'>"
			else
				x = x & "color:black;'>"
				end if
			
			x = x & Trim(v_pedido(i))
			x = x & "</td>" & chr(13)
			end if
		next
	
	if (n Mod PEDIDOS_POR_LINHA)<> 0 then
		for i = ((n Mod PEDIDOS_POR_LINHA)+1) to PEDIDOS_POR_LINHA
			x = x & "					<td>&nbsp;</td>" & chr(13)
			next
		end if
	
	x = x & "				</tr>" & chr(13) & _
			"			</table>" & chr(13) & _
			"		</td>" & chr(13) & _
			"	</tr>" & chr(13) & _
			"</table>" & chr(13) & _
			"<br>"
	
	exibe_familia_pedido = x
end function

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
	<title>CENTRAL</title>
	</head>



<script src="<%=URL_FILE__GLOBAL_JS%>" Language="JavaScript" Type="text/javascript"></script>

<script language="JavaScript" type="text/javascript">
function fPERDAConfirma( f ) {
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
<link href="<%=URL_FILE__EPRINTER_CSS%>" Rel="stylesheet" Type="text/css" media="print">


<% if alerta <> "" then %>
<!-- ************************************************************ -->
<!-- **********  P�GINA PARA EXIBIR MENSAGENS DE ERRO  ********** -->
<!-- ************************************************************ -->
<body onload="bVOLTAR.focus();">
<center>
<br>
<!--  T E L A  -->
<p class="T">A V I S O</p>
<div class="MtAlerta" style="width:600px;font-weight:bold;" align="center"><p style='margin:5px 2px 5px 2px;'><%=alerta%></p></div>
<br><br>
<p class="TracoBottom"></p>
<table cellSpacing="0">
<tr>
	<td align="center"><a name="bVOLTAR" id="bVOLTAR" href="javascript:history.back()"><img src="../botao/voltar.gif" width="176" height="55" border="0"></a></td>
</tr>
</table>
</center>
</body>




<% else %>
<!-- ***************************************************************** -->
<!-- **********  P�GINA PARA EXIBIR DADOS PARA CONFIRMA��O  ********** -->
<!-- ***************************************************************** -->
<body onload="focus();">
<center>

<form id="fPERDA" name="fPERDA" method="post" action="PerdaConfirma.asp">
<%=MontaCampoFormSessionCtrlInfo(Session("SessionCtrlInfo"))%>
<input type="hidden" name="pedido_selecionado" id="pedido_selecionado" value="<%=pedido_selecionado%>">
<input type="hidden" name="c_obs" id="c_obs" value="<%=c_obs%>">

<!--  I D E N T I F I C A � � O   D A   T E L A  -->
<table width="649" cellPadding="4" CellSpacing="0" style="border-bottom:1px solid black">

<tr>
	<td align="right" valign="bottom"><p class="PEDIDO">Perda<span class="C">&nbsp;</span></p></td>
</tr>
</table>
<br>


<!-- ************   H� OBSERVA��ES?  ************ -->
<% if observacoes <> "" then %>
		<span class="Lbl">OBSERVA��ES</span>
		<div class='MtAviso' style="width:649px;FONT-WEIGHT:bold;border:1pt solid black;" align="center"><P style='margin:5px 2px 5px 2px;'><%=observacoes%></p></div>
		<br><br>
<% end if %>


<table class="Qx" cellSpacing="0">
	<!--  CLIENTE  -->
	<tr bgColor="#FFFFFF">
		<td class="MT"><span class="PLTe">Cliente</span>
		<br><span class="PLLe" style="width:250px;margin-right:2pt;" 
				><%=x_cliente_pedido(pedido_selecionado, cadastrado)%></span></td>
		</tr>
	<tr bgColor="#FFFFFF">
	<!--  OBS  -->
		<td class="MDBE"><span class="PLTe">Obs</span>
		<br><span class="PLLe" style="width:250px;margin-right:2pt;color:navy;"
				><%=c_obs%></span></td>
		</tr>
</table>
<br>

<!--  EXIBE A FAM�LIA DE PEDIDOS  -->
<%=exibe_familia_pedido(pedido_selecionado, v_pedido)%>

<!--  VALORES  -->
<table class="Qx" cellSpacing="0">
	<tr bgColor="#FFFFFF">
		<td class="MT" NOWRAP><span class="PLTe">Valor Total da Fam�lia de Pedidos</span>
		<br><input name="c_vl_pedido" id="c_vl_pedido" readonly tabindex=-1 class="PLLd" style="width:250px;margin-right:2pt;" 
				value="<%=formata_moeda(vl_TotalFamiliaPrecoNF)%>"></td>
		</tr>
	<tr bgColor="#FFFFFF">
			<td class="MDBE" NOWRAP><span class="PLTe">Valor Pago</span>
		<br><input name="c_vl_pago" id="c_vl_pago" readonly tabindex=-1 class="PLLd" style="width:250px;margin-right:2pt;color:<%if vl_TotalFamiliaPago >= 0 then Response.Write "black" else Response.Write "red"%>;" 
				value="<%=formata_moeda(vl_TotalFamiliaPago)%>"></td>
	</tr>
	<tr bgColor="#FFFFFF">
			<td class="MDBE" NOWRAP><span class="PLTe">Valor das Devolu��es</span>
		<br><input name="c_vl_devolucao" id="c_vl_devolucao" readonly tabindex=-1 class="PLLd" style="width:250px;margin-right:2pt;" 
				value="<%=formata_moeda(vl_TotalFamiliaDevolucaoPrecoNF)%>"></td>
	</tr>
	<tr bgColor="#FFFFFF">
			<td class="MDBE" NOWRAP><span class="PLTe">Valor das Perdas Anteriores</span>
		<br><input name="c_vl_perdas_anteriores" id="c_vl_perdas_anteriores" readonly tabindex=-1 class="PLLd" 
			style="width:250px;margin-right:2pt;" 
				value="<%=formata_moeda(vl_perda)%>"></td>
	</tr>
	<tr bgColor="#FFFFFF">
			<td class="MDBE" NOWRAP><span class="PLTe">Valor da Perda Atual</span>
		<br><input name="c_valor" id="c_valor" readonly tabindex=-1 class="PLLd" style="width:250px;margin-right:2pt;color:<%if m_valor >= 0 then Response.Write "navy" else Response.Write "red"%>;" 
				value="<%=formata_moeda(m_valor)%>"></td>
	</tr>
</table>


<!-- ************   SEPARADOR   ************ -->
<table width="649" cellPadding="4" CellSpacing="0" style="border-bottom:1px solid black">
<tr><td class="Rc">&nbsp;</td></tr>
</table>
<br>


<table class="notPrint" width="649" cellSpacing="0">
<tr>
	<td><a name="bVOLTAR" id="bVOLTAR" href="javascript:history.back()" title="volta para p�gina anterior">
		<img src="../botao/anterior.gif" width="176" height="55" border="0"></a></td>
	<td align="right"><div name="dCONFIRMA" id="dCONFIRMA">
	<a name="bCONFIRMA" id="bCONFIRMA" href="javascript:fPERDAConfirma(fPERDA)" title="confirma o registro do pagamento">
		<img src="../botao/confirmar.gif" width="176" height="55" border="0"></a></div>
	</td>
</tr>
</table>
</form>

</center>
</body>

<% end if %>

</html>


<%
	if rs.State <> 0 then rs.Close
	set rs = nothing

'	FECHA CONEXAO COM O BANCO DE DADOS
	cn.Close
	set cn = nothing
%>