<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<% Response.Buffer=True %>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp" -->
<!-- #include file = "../global/Global.asp" -->

<!-- #include file = "../global/TrataSessaoExpirada.asp"        -->

<%
'     =================================
'	  OrcamentoNovoProdCompostoMask.asp
'     =================================
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

	dim i, usuario, loja, cliente_selecionado, r_cliente, msg_erro
	usuario = Trim(Session("usuario_atual"))
	loja = Trim(Session("loja_atual"))
	If (usuario = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO) 
	If (loja = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO) 

	cliente_selecionado = Trim(request("cliente_selecionado"))
	if (cliente_selecionado = "") then Response.Redirect("aviso.asp?id=" & ERR_CLIENTE_NAO_ESPECIFICADO)
	
	dim s_lista_operacoes_permitidas
	s_lista_operacoes_permitidas = Trim(Session("lista_operacoes_permitidas"))
	
	dim cn
	If Not bdd_conecta(cn) then Response.Redirect("aviso.asp?id=" & ERR_CONEXAO)

	set r_cliente = New cl_CLIENTE
	if Not x_cliente_bd(cliente_selecionado, r_cliente) then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_BD)
	
	if Trim(r_cliente.endereco_numero) = "" then
		Response.Redirect("aviso.asp?id=" & ERR_CAD_CLIENTE_ENDERECO_NUMERO_NAO_PREENCHIDO)
	elseif Len(Trim(r_cliente.endereco)) > CLng(MAX_TAMANHO_CAMPO_ENDERECO) then
		Response.Redirect("aviso.asp?id=" & ERR_CAD_CLIENTE_ENDERECO_EXCEDE_TAMANHO_MAXIMO)
		end if
		
	dim rb_end_entrega, EndEtg_endereco, EndEtg_endereco_numero, EndEtg_endereco_complemento
	dim EndEtg_bairro, EndEtg_cidade, EndEtg_uf, EndEtg_cep,EndEtg_obs
	rb_end_entrega = Trim(Request.Form("rb_end_entrega"))
	EndEtg_endereco = Trim(Request.Form("EndEtg_endereco"))
	EndEtg_endereco_numero = Trim(Request.Form("EndEtg_endereco_numero"))
	EndEtg_endereco_complemento = Trim(Request.Form("EndEtg_endereco_complemento"))
	EndEtg_bairro = Trim(Request.Form("EndEtg_bairro"))
	EndEtg_cidade = Trim(Request.Form("EndEtg_cidade"))
	EndEtg_uf = Trim(Request.Form("EndEtg_uf"))
	EndEtg_cep = Trim(Request.Form("EndEtg_cep"))
    EndEtg_obs = Trim(Request.Form("EndEtg_obs"))
	dim alerta
	alerta = ""
	
'	CONSIST�NCIAS P/ EMISS�O DE NFe
	dim s_lista_sugerida_municipios
	dim v_lista_sugerida_municipios
	dim iCounterLista, iNumeracaoLista
	dim s_tabela_municipios_IBGE
	s_tabela_municipios_IBGE = ""
	if alerta = "" then
	'	DDD V�LIDO?
		if Not ddd_ok(r_cliente.ddd_res) then
			if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
			alerta = alerta & "DDD do telefone residencial � inv�lido!!"
			end if
			
		if Not ddd_ok(r_cliente.ddd_com) then
			if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
			alerta = alerta & "DDD do telefone comercial � inv�lido!!"
			end if
			
	'	I.E. � V�LIDA?
		if r_cliente.tipo = ID_PJ then
            if r_cliente.ie <> "" then
			    if Not isInscricaoEstadualValida(r_cliente.ie, r_cliente.uf) then
				    if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
				    alerta=alerta & "Corrija a IE (Inscri��o Estadual) com um n�mero v�lido!!" & _
						    "<br>" & "Certifique-se de que a UF informada corresponde � UF respons�vel pelo registro da IE."
				    end if
            end if
		end if

	'	MUNIC�PIO DE ACORDO C/ TABELA DO IBGE?
		if Not consiste_municipio_IBGE_ok(r_cliente.cidade, r_cliente.uf, s_lista_sugerida_municipios, msg_erro) then
			if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
			if msg_erro <> "" then
				alerta = alerta & msg_erro
			else
				alerta = alerta & "Munic�pio '" & r_cliente.cidade & "' n�o consta na rela��o de munic�pios do IBGE para a UF de '" & r_cliente.uf & "'!!"
				if s_lista_sugerida_municipios <> "" then
					alerta = alerta & "<br>" & _
									  "Localize o munic�pio na lista abaixo e verifique se a grafia est� correta!!"
					v_lista_sugerida_municipios = Split(s_lista_sugerida_municipios, chr(13))
					iNumeracaoLista=0
					for iCounterLista=LBound(v_lista_sugerida_municipios) to UBound(v_lista_sugerida_municipios)
						if Trim("" & v_lista_sugerida_municipios(iCounterLista)) <> "" then
							iNumeracaoLista=iNumeracaoLista+1
							s_tabela_municipios_IBGE = s_tabela_municipios_IBGE & _
												"	<tr>" & chr(13) & _
												"		<td align='right'>" & chr(13) & _
												"			<span class='N'>&nbsp;" & Cstr(iNumeracaoLista) & "." & "</span>" & chr(13) & _
												"		</td>" & chr(13) & _
												"		<td align='left'>" & chr(13) & _
												"			<span class='N'>" & Trim("" & v_lista_sugerida_municipios(iCounterLista)) & "</span>" & chr(13) & _
												"		</td>" & chr(13) & _
												"	</tr>" & chr(13)
							end if
						next
					
					if s_tabela_municipios_IBGE <> "" then
						s_tabela_municipios_IBGE = _
								"<table cellspacing='0' cellpadding='1'>" & chr(13) & _
								"	<tr>" & chr(13) & _
								"		<td align='center'>" & chr(13) & _
								"			<p class='N'>" & "Rela��o de munic�pios de '" & r_cliente.uf & "' que se iniciam com a letra '" & Ucase(left(r_cliente.cidade,1)) & "'" & "</p>" & chr(13) & _
								"		</td>" & chr(13) & _
								"	</tr>" & chr(13) & _
								"	<tr>" & chr(13) & _
								"		<td align='center'>" & chr(13) &_
								"			<table cellspacing='0' border='1'>" & chr(13) & _
												s_tabela_municipios_IBGE & _
								"			</table>" & chr(13) & _
								"		</td>" & chr(13) & _
								"	</tr>" & chr(13) & _
								"</table>" & chr(13)
						end if
					end if
				end if
			end if
		end if

	if alerta = "" then
		if rb_end_entrega = "S" then
		'	MUNIC�PIO DE ACORDO C/ TABELA DO IBGE?
			if Not consiste_municipio_IBGE_ok(EndEtg_cidade, EndEtg_uf, s_lista_sugerida_municipios, msg_erro) then
				if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
				if msg_erro <> "" then
					alerta = alerta & msg_erro
				else
					alerta = alerta & "Munic�pio '" & EndEtg_cidade & "' n�o consta na rela��o de munic�pios do IBGE para a UF de '" & EndEtg_uf & "'!!"
					if s_lista_sugerida_municipios <> "" then
						alerta = alerta & "<br>" & _
										  "Localize o munic�pio na lista abaixo e verifique se a grafia est� correta!!"
						v_lista_sugerida_municipios = Split(s_lista_sugerida_municipios, chr(13))
						iNumeracaoLista=0
						for iCounterLista=LBound(v_lista_sugerida_municipios) to UBound(v_lista_sugerida_municipios)
							if Trim("" & v_lista_sugerida_municipios(iCounterLista)) <> "" then
								iNumeracaoLista=iNumeracaoLista+1
								s_tabela_municipios_IBGE = s_tabela_municipios_IBGE & _
													"	<tr>" & chr(13) & _
													"		<td align='right'>" & chr(13) & _
													"			<span class='N'>&nbsp;" & Cstr(iNumeracaoLista) & "." & "</span>" & chr(13) & _
													"		</td>" & chr(13) & _
													"		<td align='left'>" & chr(13) & _
													"			<span class='N'>" & Trim("" & v_lista_sugerida_municipios(iCounterLista)) & "</span>" & chr(13) & _
													"		</td>" & chr(13) & _
													"	</tr>" & chr(13)
								end if
							next

						if s_tabela_municipios_IBGE <> "" then
							s_tabela_municipios_IBGE = _
									"<table cellspacing='0' cellpadding='1'>" & chr(13) & _
									"	<tr>" & chr(13) & _
									"		<td align='center'>" & chr(13) & _
									"			<p class='N'>" & "Rela��o de munic�pios de '" & EndEtg_uf & "' que se iniciam com a letra '" & Ucase(left(EndEtg_cidade,1)) & "'" & "</p>" & chr(13) & _
									"		</td>" & chr(13) & _
									"	</tr>" & chr(13) & _
									"	<tr>" & chr(13) & _
									"		<td align='center'>" & chr(13) &_
									"			<table cellspacing='0' border='1'>" & chr(13) & _
													s_tabela_municipios_IBGE & _
									"			</table>" & chr(13) & _
									"		</td>" & chr(13) & _
									"	</tr>" & chr(13) & _
									"</table>" & chr(13)
							end if
						end if
					end if
				end if 'if Not consiste_municipio_IBGE_ok()
			end if 'if rb_end_entrega = "S"
		end if 'if alerta = ""

	dim s_campo_inicial
	if isLojaHabilitadaProdCompostoECommerce(loja) then
		s_campo_inicial = "c_produto"
	else
		s_campo_inicial = "c_fabricante"
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


<%=DOCTYPE_LEGADO%>

<html>


<head>
	<title>LOJA</title>
	</head>



<script src="<%=URL_FILE__JQUERY%>" language="JavaScript" type="text/javascript"></script>
<script src="<%=URL_FILE__GLOBAL_JS%>" language="JavaScript" type="text/javascript"></script>
<script src="<%=URL_FILE__AJAX_JS%>" language="JavaScript" type="text/javascript"></script>

<script type="text/javascript">
	$(function () {
		$(".tdTitFabr").hide();
		$(".tdDadosFabr").hide();
		$(".tdDadosProd").addClass("ME");
		$("#divAjaxRunning").css('filter', 'alpha(opacity=60)'); // TRANSPAR�NCIA NO IE8
	});

	//Every resize of window
	$(window).resize(function() {
		sizeDivAjaxRunning();
	});

	//Every scroll of window
	$(window).scroll(function() {
		sizeDivAjaxRunning();
	});

	//Dynamically assign height
	function sizeDivAjaxRunning() {
		var newTop = $(window).scrollTop() + "px";
		$("#divAjaxRunning").css("top", newTop);
	}
</script>

<script language="JavaScript" type="text/javascript">
var objAjaxConsultaDadosProduto;

function trataRespostaAjaxConsultaDadosProduto() {
var f, strResp, i, j, xmlDoc, oNodes;
var strFabricante,strProduto, strStatus, strPrecoLista, strDescricao, strTabelaOrigem, strMsgErro;
	f=fPED;
	if (objAjaxConsultaDadosProduto.readyState == AJAX_REQUEST_IS_COMPLETE) {
		strResp = objAjaxConsultaDadosProduto.responseText;
		if (strResp=="") {
			alert("Falha ao consultar a descri��o!!");
			window.status="Conclu�do";
			$("#divAjaxRunning").hide();
			return;
			}
		
		if (strResp!="") {
			try {
				xmlDoc = objAjaxConsultaDadosProduto.responseXML.documentElement;
				for (i = 0; i < xmlDoc.getElementsByTagName("ItemConsulta").length; i++) {
					//  Fabricante
					oNodes = xmlDoc.getElementsByTagName("fabricante")[i];
					if (oNodes.childNodes.length > 0) strFabricante = oNodes.childNodes[0].nodeValue; else strFabricante = "";
					if (strFabricante == null) strFabricante = "";
					//  Produto
					oNodes = xmlDoc.getElementsByTagName("produto")[i];
					if (oNodes.childNodes.length > 0) strProduto = oNodes.childNodes[0].nodeValue; else strProduto = "";
					if (strProduto == null) strProduto = "";
					// Tabela Origem
					oNodes = xmlDoc.getElementsByTagName("tabela_origem")[i];
					if (oNodes.childNodes.length > 0) strTabelaOrigem = oNodes.childNodes[0].nodeValue; else strTabelaOrigem = "";
					if (strTabelaOrigem == null) strTabelaOrigem = "";
					//  Status
					oNodes = xmlDoc.getElementsByTagName("status")[i];
					if (oNodes.childNodes.length > 0) strStatus = oNodes.childNodes[0].nodeValue; else strStatus = "";
					if (strStatus == null) strStatus = "";
					if (strStatus == "OK") {
						//  Descri��o
						oNodes = xmlDoc.getElementsByTagName("descricao")[i];
						if (oNodes.childNodes.length > 0) strDescricao = oNodes.childNodes[0].nodeValue; else strDescricao = "";
						if (strDescricao == null) strDescricao = "";
						if (strDescricao != "") {
							for (j = 0; j < f.c_fabricante.length; j++) {
								if (
								((f.c_fabricante[j].value == strFabricante) && (f.c_produto[j].value == strProduto))
								||
								((f.c_fabricante[j].value == "") && (f.c_produto[j].value == strProduto))
								) {
									//  Percorre o la�o at� o final para o caso do usu�rio ter digitado o mesmo produto em v�rias linhas
									//	(apesar de que isso n�o ser� aceito pelas consist�ncias que ser�o feitas).
									if (f.c_fabricante[j].value == "") f.c_fabricante[j].value = strFabricante;
									f.c_descricao[j].value = strDescricao;
									f.c_fabricante[j].style.color = "black";
									f.c_produto[j].style.color = "black";
								}
							}
						}
						//  Pre�o
						oNodes = xmlDoc.getElementsByTagName("precoLista")[i];
						if (oNodes.childNodes.length > 0) strPrecoLista = oNodes.childNodes[0].nodeValue; else strPrecoLista = "";
						if (strPrecoLista == null) strPrecoLista = "";
						//  Atualiza o pre�o
						if ((strPrecoLista == "") && (strTabelaOrigem.toUpperCase() != "T_EC_PRODUTO_COMPOSTO")) {
							alert("Falha na consulta do pre�o do produto " + strProduto);
						}
						else {
							for (j = 0; j < f.c_fabricante.length; j++) {
								if (
									((f.c_fabricante[j].value == strFabricante) && (f.c_produto[j].value == strProduto))
									||
									((f.c_fabricante[j].value == "") && (f.c_produto[j].value == strProduto))
									) {
									//  Percorre o la�o at� o final para o caso do usu�rio ter digitado o mesmo produto em v�rias linhas
									//	(apesar de que isso n�o ser� aceito pelas consist�ncias que ser�o feitas).
									f.c_preco_lista[j].value = strPrecoLista;
								}
							}
						}
					}
					else {
						//  Mensagem de Erro
						oNodes = xmlDoc.getElementsByTagName("msg_erro")[i];
						if (oNodes.childNodes.length > 0) strMsgErro = oNodes.childNodes[0].nodeValue; else strMsgErro = "";
						if (strMsgErro == null) strMsgErro = "";
						for (j = 0; j < f.c_fabricante.length; j++) {
							//  Percorre o la�o at� o final para o caso do usu�rio ter digitado o mesmo produto em v�rias linhas
							//	(apesar de que isso n�o ser� aceito pelas consist�ncias que ser�o feitas).
							if ((f.c_fabricante[j].value == strFabricante) && (f.c_produto[j].value == strProduto)) {
								f.c_fabricante[j].style.color = COR_AJAX_CONSULTA_DADOS_PRODUTO__INEXISTENTE;
								f.c_produto[j].style.color = COR_AJAX_CONSULTA_DADOS_PRODUTO__INEXISTENTE;
							}
						}
						alert("Falha ao consultar os dados do produto " + strProduto + "\n" + strMsgErro);
					}
				}
			}
			catch (e) {
				alert("Falha na consulta dos dados do produto!!\n" + e.message);
			}
			}
		window.status="Conclu�do";
		$("#divAjaxRunning").hide();
		}
}

function consultaDadosProduto(intIndice) {
var f, i, strProdutoSelecionado, strUrl;
	f=fPED;
	if (trim(f.c_produto[intIndice].value)=="") return;

	objAjaxConsultaDadosProduto = GetXmlHttpObject();
	if (objAjaxConsultaDadosProduto == null) {
		alert("O browser N�O possui suporte ao AJAX!!");
		return;
		}
		
	f.c_fabricante[intIndice].value = "";
	strProdutoSelecionado=f.c_fabricante[intIndice].value + "|" + f.c_produto[intIndice].value;
	
	window.status="Aguarde, consultando descri��o ...";
	$("#divAjaxRunning").show();
	
	strUrl = "../Global/AjaxConsultaDadosProdutoBD.asp";
	strUrl += "?listaProdutos=" + strProdutoSelecionado;
	strUrl += "&loja=" + f.c_loja.value;
	//  Prevents server from using a cached file
	strUrl=strUrl+"&sid="+Math.random()+Math.random();
	objAjaxConsultaDadosProduto.onreadystatechange = trataRespostaAjaxConsultaDadosProduto;
	objAjaxConsultaDadosProduto.open("GET", strUrl, true);
	objAjaxConsultaDadosProduto.send(null);
}

function trataLimpaLinha(intIndice) {
var f;
	f=fPED;
	if ((trim(f.c_fabricante[intIndice].value)=="")&&(trim(f.c_produto[intIndice].value)=="")) {
		f.c_qtde[intIndice].value="";
		f.c_descricao[intIndice].value="";
		f.c_preco_lista[intIndice].value="";
		}
}

function LimparLinha(f, intIdx) {
	f.c_fabricante[intIdx].value = "";
	f.c_produto[intIdx].value = "";
	f.c_qtde[intIdx].value = "";
	f.c_descricao[intIdx].value = "";
	f.c_preco_lista[intIdx].value = "";
	f.c_produto[intIdx].focus();
}

function fPEDConfirma( f ) {
var i, b, ha_item;
	ha_item=false;
	for (i=0; i < f.c_produto.length; i++) {
		b=false;
		if (trim(f.c_fabricante[i].value)!="") b=true;
		if (trim(f.c_produto[i].value)!="") b=true;
		if (trim(f.c_qtde[i].value)!="") b=true;
		
		if (b) {
			ha_item=true;
			if (trim(f.c_produto[i].value)=="") {
				alert("Informe o c�digo do produto!!");
				f.c_produto[i].focus();
				return;
				}
			if (trim(f.c_qtde[i].value)=="") {
				alert("Informe a quantidade!!");
				f.c_qtde[i].focus();
				return;
				}
			if (parseInt(f.c_qtde[i].value)<=0) {
				alert("Quantidade inv�lida!!");
				f.c_qtde[i].focus();
				return;
				}
			}
		}
		
	if (!ha_item) {
		alert("N�o h� produtos na lista!!");
		f.c_fabricante[0].focus();
		return;
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

<link href="<%=URL_FILE__E_CSS%>" rel="stylesheet" type="text/css">
<link href="<%=URL_FILE__EPRINTER_CSS%>" rel="stylesheet" type="text/css" media="print">

<style type="text/css">
#divAjaxRunning
{
	position:absolute;
	top:0;
	left:0;
	width:100%;
	height:100%;
	z-index:1001;
	background-color:grey;
	opacity: .6;
}
.AjaxImgLoader
{
	position: absolute;
	left: 50%;
	top: 50%;
	margin-left: -128px; /* -1 * image width / 2 */
	margin-top: -128px;  /* -1 * image height / 2 */
	display: block;
}
</style>

<% if alerta <> "" then %>
<!-- ************************************************************ -->
<!-- **********  P�GINA PARA EXIBIR MENSAGENS DE ERRO  ********** -->
<!-- ************************************************************ -->
<body>
<center>
<br>
<!--  T E L A  -->
<p class="T">A V I S O</p>
<div class="MtAlerta" style="width:600px;font-weight:bold;" align="center"><P style='margin:5px 2px 5px 2px;'><%=alerta%></p></div>
<% if s_tabela_municipios_IBGE <> "" then %>
	<br /><br />
	<%=s_tabela_municipios_IBGE%>
<% end if %>
<br><br>
<p class="TracoBottom"></p>
<table cellspacing="0">
<tr>
	<td align="center"><a name="bVOLTAR" id="bVOLTAR" href="javascript:history.back()"><img src="../botao/voltar.gif" width="176" height="55" border="0"></a></td>
</tr>
</table>
</center>
</body>


<% else %>
<body onload="if (trim(fPED.<%=s_campo_inicial%>[0].value)=='') fPED.<%=s_campo_inicial%>[0].focus();">

<center>
    
<form id="fPED" name="fPED" method="post" action="OrcamentoNovo.asp">
<%=MontaCampoFormSessionCtrlInfo(Session("SessionCtrlInfo"))%>
<input type="hidden" name="c_loja" id="c_loja" value='<%=loja%>'>
<input type="hidden" name="cliente_selecionado" id="cliente_selecionado" value='<%=cliente_selecionado%>'>
<input type="hidden" name="rb_end_entrega" id="rb_end_entrega" value='<%=rb_end_entrega%>'>
<input type="hidden" name="EndEtg_endereco" id="EndEtg_endereco" value="<%=EndEtg_endereco%>">
<input type="hidden" name="EndEtg_endereco_numero" id="EndEtg_endereco_numero" value="<%=EndEtg_endereco_numero%>">
<input type="hidden" name="EndEtg_endereco_complemento" id="EndEtg_endereco_complemento" value="<%=EndEtg_endereco_complemento%>">
<input type="hidden" name="EndEtg_bairro" id="EndEtg_bairro" value="<%=EndEtg_bairro%>">
<input type="hidden" name="EndEtg_cidade" id="EndEtg_cidade" value="<%=EndEtg_cidade%>">
<input type="hidden" name="EndEtg_uf" id="EndEtg_uf" value="<%=EndEtg_uf%>">
<input type="hidden" name="EndEtg_cep" id="EndEtg_cep" value="<%=EndEtg_cep%>">
<input type="hidden" name="EndEtg_obs" id="EndEtg_obs" value='<%=EndEtg_obs%>'>
<!-- AJAX EM ANDAMENTO -->
<div id="divAjaxRunning" style="display:none;"><img src="../Imagem/ajax_loader_gray_256.gif" class="AjaxImgLoader"/></div>


<!--  I D E N T I F I C A � � O   D O   P E D I D O -->
<br />
<table width="749" cellpadding="4" cellspacing="0" style="border-bottom:1px solid black">
<tr>
	<td align="right" valign="bottom"><span class="PEDIDO">Pedido Novo</span></td>
</tr>
</table>
<br>

<!--  R E L A � � O   D E   P R O D U T O S  -->
<table class="Qx" cellspacing="0">
	<tr bgColor="#FFFFFF">
	<td class="MB tdTitFabr" align="left"><span class="PLTe">Fabr</span></td>
	<td class="MB tdTitProd" align="left"><span class="PLTe">Produto</span></td>
	<td class="MB" align="right"><span class="PLTd">Qtde</span></td>
	<td class="MB" align="left"><span class="PLTe">Descri��o</span></td>
	<td class="MB" align="right"><span class="PLTd">VL Unit</span></td>
	<td align="left">&nbsp;</td>
	</tr>
<% for i=1 to MAX_ITENS %>
	<tr>
	<td class="MDBE tdDadosFabr" align="left"><input name="c_fabricante" id="c_fabricante" class="PLLe" maxlength="4" style="width:30px;" onkeypress="if (digitou_enter(true)) fPED.c_produto[<%=Cstr(i-1)%>].focus(); filtra_fabricante();" onblur="this.value=normaliza_codigo(this.value,TAM_MIN_FABRICANTE);trataLimpaLinha(<%=Cstr(i-1)%>);"></td>
	<td class="MDB tdDadosProd" align="left"><input name="c_produto" id="c_produto" class="PLLe" maxlength="8" style="width:60px;" onkeypress="if (digitou_enter(true)) fPED.c_qtde[<%=Cstr(i-1)%>].focus(); filtra_produto();" onblur="this.value=normaliza_produto(this.value);consultaDadosProduto(<%=Cstr(i-1)%>);trataLimpaLinha(<%=Cstr(i-1)%>);"></td>
	<td class="MDB" align="right"><input name="c_qtde" id="c_qtde" class="PLLd" maxlength="4" style="width:30px;" onkeypress="if (digitou_enter(true)) {if (<%=Cstr(i)%>==fPED.c_qtde.length) bCONFIRMA.focus(); else fPED.<%=s_campo_inicial%>[<%=Cstr(i)%>].focus();} filtra_numerico();"></td>
	<td class="MDB" align="left"><input name="c_descricao" id="c_descricao" class="PLLe" style="width:427px;" readonly tabindex=-1></td>
	<td class="MDB" align="right"><input name="c_preco_lista" id="c_preco_lista" class="PLLd" style="width:62px;" readonly tabindex=-1></td>
	<td align="left">
		<a name="bLimparLinha" href="javascript:LimparLinha(fPED,<%=Cstr(i-1)%>)" title="limpa o conte�do desta linha"><img src="../botao/botao_x_red.gif" style="vertical-align:bottom;margin-bottom:1px;" width="20" height="20" border="0"></a>
	</td>
	</tr>
<% next %>
</table>

<br>

<!-- ************   SEPARADOR   ************ -->
<table width="749" cellpadding="4" cellspacing="0" style="border-bottom:1px solid black">
<tr><td class="Rc" align="left">&nbsp;</td></tr>
</table>
<br>


<table class="notPrint" width="749" cellspacing="0">
<tr>
	<td align="left"><a name="bVOLTAR" id="bVOLTAR" href="javascript:history.back();" title="volta para a p�gina anterior">
		<img src="../botao/voltar.gif" width="176" height="55" border="0"></a></td>
	<td align="right"><div name="dCONFIRMA" id="dCONFIRMA">
		<a name="bCONFIRMA" id="bCONFIRMA" href="javascript:fPEDConfirma(fPED)" title="segue para pr�xima tela">
		<img src="../botao/proximo.gif" width="176" height="55" border="0"></a></div>
	</td>
</tr>
</table>
</form>

</center>
</body>
<% end if %>

</html>

<%
'	FECHA CONEXAO COM O BANCO DE DADOS
	cn.Close
	set cn = nothing
%>