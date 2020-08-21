<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<% Response.Buffer=True %>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp" -->
<!-- #include file = "../global/Braspag.asp"    -->
<!-- #include file = "../global/BraspagCS.asp"    -->

<%
'     ===========================================
'	  P040PagtoOpcoes.asp
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

	dim s, usuario, loja, pedido_selecionado, id_pedido_base

	usuario = BRASPAG_USUARIO_CLIENTE

	pedido_selecionado = ucase(Trim(request("pedido_selecionado")))
	if (pedido_selecionado = "") then Response.Redirect("aviso.asp?id=" & ERR_PEDIDO_NAO_ESPECIFICADO)
	s = normaliza_num_pedido(pedido_selecionado)
	if s <> "" then pedido_selecionado = s
	id_pedido_base = retorna_num_pedido_base(pedido_selecionado)
	
	dim cnpj_cpf_selecionado
	cnpj_cpf_selecionado = retorna_so_digitos(Request("cnpj_cpf_selecionado"))
	
	dim i, n, s_descricao, s_qtde, s_vl_unitario, s_vl_total, m_vl_total, m_total_geral, m_frete
	dim n_itens
	
'	CONECTA AO BANCO DE DADOS
'	=========================
	dim cn, rs, msg_erro
	If Not bdd_conecta(cn) then Response.Redirect("aviso.asp?id=" & ERR_CONEXAO)
	if Not cria_recordset_otimista(rs, msg_erro) then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_CRIAR_ADO)
	
	dim r_pedido, v_item, alerta
	alerta = ""
	if Not le_pedido(id_pedido_base, r_pedido, msg_erro) then
		alerta = msg_erro
	else
		loja = r_pedido.loja
		if Not le_pedido_item_consolidado_familia(id_pedido_base, v_item, msg_erro) then alerta = msg_erro
		end if
	
	dim r_cliente
	set r_cliente = New cl_CLIENTE
	if alerta = "" then
		if x_cliente_bd(r_pedido.id_cliente, r_cliente) then
			with r_cliente
				if .cep = "" then
					if alerta <> "" then alerta = alerta & "<BR>"
					alerta = alerta & "� necess�rio preencher o CEP no cadastro do cliente."
					end if
				end with
			end if
		end if





' _____________________________________________________________________________________________
'
'									F  U  N  �  �  E  S 
' _____________________________________________________________________________________________

sub fecha_conexao_bd

	if rs.State <> 0 then rs.Close
	set rs=nothing

'	FECHA CONEXAO COM O BANCO DE DADOS
	cn.Close
	set cn = nothing
	
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


<%=DOCTYPE_LEGADO%>

<html>


<head>
	<title><%=SITE_CLIENTE_TITULO_JANELA%></title>
	</head>


<script src="<%=URL_FILE__GLOBAL_JS%>" language="JavaScript" type="text/javascript"></script>
<script src="<%=URL_FILE__SSL_JS%>" language="JavaScript" type="text/javascript"></script>

<script language="JavaScript" type="text/javascript">
function Navega(url) {
	window.location.href = url;
}

function fPEDConsulta() {
	fPED.action = "../ClienteCartao/PedidoConsulta.asp";
	window.status = "Aguarde ...";
	fPED.submit();
}

function fPAGTOConclui( f ) {
	f.action = "P050PagtoDadosCartao.asp";
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

<link href="<%=URL_FILE__E_CSS%>" rel="stylesheet" type="text/css">
<link href="<%=URL_FILE__EPRINTER_CSS%>" rel="stylesheet" type="text/css" media="print">
<link href="<%=URL_FILE__E_LOGO_TOP_BS_CSS%>" rel="stylesheet" type="text/css">

<style type="text/css">
body::before
{
	content: '';
	border: none;
	margin-top: 0px;
	margin-bottom: 0px;
	padding: 0px;
}
.LSEL
{
	font-family:Arial;
	font-size:12pt;
	font-weight:bold;
	color: #808080;
}
</style>


<% if alerta <> "" then %>
<!-- ************************************************************ -->
<!-- **********  P�GINA PARA EXIBIR MENSAGENS DE ERRO  ********** -->
<!-- ************************************************************ -->
<body onload="bVOLTAR.focus();" link=#000000 alink=#000000 vlink=#000000>
<center>
<br>
<!--  T E L A  -->
<p class="T">A V I S O</p>
<div class="MtAlerta" style="width:600px;font-weight:bold;" align="center"><p style='margin:5px 2px 5px 2px;'><%=alerta%></p></div>
<br><br>
<p class="TracoBottom"></p>
<table cellspacing="0">
<tr>
	<td align="center"><a name="bVOLTAR" href="javascript:history.back()"><img src="../botao/voltar.gif" width="176" height="55" border="0"></a></td>
</tr>
</table>
</center>
</body>
</html>

<% 
	fecha_conexao_bd
	Response.End
	end if
%>




<!-- ****************************************************************** -->
<!-- **********  P�GINA PARA EXIBIR RESUMO DO PEDIDO  ***************** -->
<!-- ****************************************************************** -->
<body onload="window.status='';">
<center>

<table class="notPrint" id="tbl_logotipo_bonshop" width="100%" cellpadding="0" cellspacing="0" border="0">
	<tr>
		<td align="center"><img alt="<%=SITE_CLIENTE_HEADER__ALT_IMG_TEXT%>" src="../imagem/<%=SITE_CLIENTE_HEADER__LOGOTIPO%>" /></td>
	</tr>
</table>
<table class="notPrint" id="pagina_tbl_cabecalho" cellspacing="0px" cellpadding="0px">
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

<form id="fPED" name="fPED" method="post">
<input type="hidden" name='pedido_selecionado' value='<%=pedido_selecionado%>'>
<input type="hidden" name='cnpj_cpf_selecionado' value='<%=cnpj_cpf_selecionado%>'>
</form>

<form id="fPAGTO" name="fPAGTO" method="post">
<input type="hidden" name='pedido_selecionado' value='<%=pedido_selecionado%>'>
<input type="hidden" name='cnpj_cpf_selecionado' value='<%=cnpj_cpf_selecionado%>'>

<!--  R E L A � � O   D E   P R O D U T O S  -->
<br>
<br>
<table class="Qx" cellspacing="0">

	<tr><td colspan="4" align="center"><span class="STP" style="font-size:14pt;">Pagamento</span></td></tr>
	<tr><td colspan="4" align="left">&nbsp;</td></tr>

	<tr bgcolor="#FFFFFF">
	<td class="MB" align="left"><span class="PLTe">Produto</span></td>
	<td class="MB" align="right"><span class="PLTd">Qtde</span></td>
	<td class="MB" align="right"><span class="PLTd">Valor Unit <%=SIMBOLO_MONETARIO%></span></td>
	<td class="MB" align="right"><span class="PLTd">Valor Total <%=SIMBOLO_MONETARIO%></span></td>
	</tr>

<%	m_total_geral=0
	n_itens = 0
	n = Lbound(v_item)-1
	for i=1 to MAX_ITENS
		n = n+1
		if n <= Ubound(v_item) then
			n_itens = n_itens + 1
			with v_item(n)
				s_descricao=.descricao
				s_qtde=.qtde
				s_vl_unitario=formata_moeda(.preco_NF)
				m_vl_total=.qtde * .preco_NF
				s_vl_total=formata_moeda(m_vl_total)
				m_total_geral=m_total_geral + m_vl_total
				end with
		else
			exit for
			end if
%>
	<tr>
	<td class="MDBE" align="left"><input name="c_descricao" id="c_descricao" class="PLLe" style="width:280px;"
		value='<%=s_descricao%>' readonly tabindex=-1></td>
	<td class="MDB" align="right"><input name="c_qtde" id="c_qtde" class="PLLd" style="width:40px;"
		value='<%=s_qtde%>' readonly tabindex=-1></td>
	<td class="MDB" align="right"><input name="c_vl_unitario" id="c_vl_unitario" class="PLLd" style="width:90px;"
		value='<%=s_vl_unitario%>' readonly tabindex=-1></td>
	<td class="MDB" align="right"><input name="c_vl_total" id="c_vl_total" class="PLLd" style="width:90px;" 
		value='<%=s_vl_total%>' readonly tabindex=-1></td>
	</tr>
<% next %>

<% if n_itens > 1 then %>
	<tr><td colspan="4" style="height:1px;" align="left"></td></tr>
	<tr>
	<td colspan="3" class="MD" align="left">&nbsp;</td>
	<td class="MTBD" align="right"><input name="c_total" id="c_total" class="PLLd" style="width:90px;" 
		value='<%=formata_moeda(m_total_geral)%>' readonly tabindex=-1></td>
	</tr>
<% end if %>

	<tr><td colspan="4" style="height:10px;" align="left"></td></tr>

	<%
		m_frete = r_pedido.vl_frete
		m_total_geral = m_total_geral + m_frete
	%>
	<tr>
	<td colspan="2" class="MD" align="left">&nbsp;</td>
	<td class="MTBD" align="right"><span class="PLTd">Frete <%=SIMBOLO_MONETARIO%></span></td>
	<td class="MTBD" align="right"><input name="c_frete" class="PLLd"
		value='<%=formata_moeda(m_frete)%>' readonly tabindex=-1></td>
	</tr>

	<tr><td colspan="4" style="height:10px;" align="left"></td></tr>

	<tr>
	<td colspan="2" class="MD" align="left">&nbsp;</td>
	<td class="MTBD" align="right"><span class="PLTd">Total <%=SIMBOLO_MONETARIO%></span></td>
	<td class="MTBD" align="right"><input name="c_total_geral" class="PLLd" style="width:90px;color:blue;" 
		value='<%=formata_moeda(m_total_geral)%>' readonly tabindex=-1></td>
	</tr>
</table>



<!-- ************   LINK PARA PROSSEGUIR COM PAGAMENTO   ************ -->
<br><br>
<table class="notPrint" width="449" cellpadding="0" cellspacing="5" border="0">
<tr>
	<td align="center"><span class="LSEL" style="margin-bottom:6px;">Pagar usando quantos cart�es?</span>
		&nbsp;
		<select name="c_qtde_cartoes" id="c_qtde_cartoes">
		<%=BraspagCS_monta_select_qtde_cartoes%>
		</select>
	</td>
</tr>
</table>


<!-- ************   SEPARADOR   ************ -->
<table width="649" cellpadding="4" cellspacing="0" style="border-bottom:1px solid black">
<tr><td class="Rc" align="left">&nbsp;</td></tr>
</table>


<!-- ************   BOT�ES   ************ -->
<table class="notPrint" width="649" cellpadding="0" cellspacing="0">
<tr>
	<td align="center"><a name="bVOLTAR" href="javascript:fPEDConsulta()" title="volta para a p�gina do pedido">
		<img src="../botao/voltar.gif" width="176" height="55" border="0"></a>
	</td>
	<td align="right"><div id="dPROXIMO">
		<a name="bPROXIMO" id="bPROXIMO" href="javascript:fPAGTOConclui(fPAGTO)" title="vai para a p�gina seguinte">
		<img src="../botao/proximo.gif" width="176" height="55" border="0"></a></div>
	</td>
</tr>
</table>

</form>

</center>

<% if SITE_CLIENTE_EXIBIR_LOGO_SSL then %>
<script language="JavaScript" type="text/javascript">
	logo_ssl_corner("../imagem/ssl/ssl_corner.gif");
</script>
<% end if %>

</body>
</html>


<%

'	FECHA CONEXAO COM O BANCO DE DADOS
	fecha_conexao_bd
	
%>