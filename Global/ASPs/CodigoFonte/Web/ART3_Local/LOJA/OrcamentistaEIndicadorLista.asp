<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<%Response.Buffer = False %>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp" -->

<!-- #include file = "../global/TrataSessaoExpirada.asp"        -->

<%
'     =============================================================
'	  O R C A M E N T I S T A E I N D I C A D O R L I S T A . A S P
'     =============================================================
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
	dim usuario, loja
	usuario = trim(Session("usuario_atual"))
	loja = Trim(Session("loja_atual"))
	If (usuario = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO) 
	If (loja = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO) 

'	CONECTA COM O BANCO DE DADOS
	Dim cn
	If Not bdd_conecta(cn) then Response.Redirect("aviso.asp?id=" & ERR_CONEXAO)

	'Par�metro 'ord' � a ordena��o realizada ao clicar no label de uma coluna da tabela de resultado
	'Par�metro 'ordenacao' � a ordena��o selecionada pelo usu�rio ao selecionar o tipo de consulta na p�gina de menu do cadastro de or�amentistas/indicadores
	dim ordenacao, ordenacao_selecionada
	ordenacao_selecionada=Trim("" & request("ord"))
	ordenacao = Trim("" & Request.Form("ordenacao"))
	if ordenacao_selecionada = "" then ordenacao_selecionada = ordenacao

	'MEMORIZA OP��O DE ORDENA��O FEITA NO MENU DE OR�AMENTISTAS/INDICADORES
	if ordenacao <> "" then
		call set_default_valor_texto_bd(usuario, "MenuOrcamentistaEIndicador|ordenacao", ordenacao)
		end if

	dim opcao_consulta
	opcao_consulta=UCase(Trim(request("op")))

    dim s_lista_operacoes_permitidas
	s_lista_operacoes_permitidas = Trim(Session("lista_operacoes_permitidas"))

' ________________________________
' E X E C U T A _ C O N S U L T A
'
Sub executa_consulta
dim consulta, s_where, s, i, x, cab, s_op, s_ddd, s_tel, s_telefones, strCidade, strUf
dim iLineNumber
dim r

	s_op = ""
	if opcao_consulta <> "" then s_op = "&op=" & opcao_consulta
	
  ' CABE�ALHO
	cab="<TABLE class='Q' cellSpacing=0 style='border-right:0;border-top:0;border-bottom:0'>" & chr(13)
	cab=cab & "<TR style='background: #FFF0E0'>"
	cab=cab & "<td align='left' class='MD MB MC' valign='top' style='min-width:30px;'></td>"
	cab=cab & "<TD width='90' nowrap class='MD MB MC' valign='bottom'><P class='R' style='cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='OrcamentistaEIndicadorLista.asp?ord=1" & s_op & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">Identifica��o</P></TD>"
	cab=cab & "<TD width='210' class='MD MB MC' valign='bottom'><P class='R' style='cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='OrcamentistaEIndicadorLista.asp?ord=2" & s_op & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">Nome</P></TD>"
	cab=cab & "<TD width='105' class='MD MB MC' valign='bottom'><P class='R' style='cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='OrcamentistaEIndicadorLista.asp?ord=7" & s_op & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">CPF/CNPJ</P></TD>"
	cab=cab & "<TD width='95' class='MD MB MC' valign='bottom'><P class='R'>Telefone</P></TD>"
    cab=cab & "<TD width='150' class='MD MB MC' valign='bottom'><P class='R'style='cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='OrcamentistaEIndicadorLista.asp?ord=3" & s_op & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ");'" & ">Cidade</P></TD>"
	cab=cab & "<TD width='90' nowrap class='MD MB MC' valign='bottom'><P class='R' style='font-weight:bold; cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='OrcamentistaEIndicadorLista.asp?ord=4" & s_op & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">Vendedor</P></TD>"
	cab=cab & "<TD width='50' nowrap class='MD MB MC' valign='bottom'><P class='R' style='cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='OrcamentistaEIndicadorLista.asp?ord=5" & s_op & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">Acesso Sistema</P></TD>"
	cab=cab & "<TD width='45' nowrap class='MB MD MC' valign='bottom'><P class='R' style='cursor: pointer;' title='clique para ordenar a lista por este campo' onclick=" & chr(34) & "window.location='OrcamentistaEIndicadorLista.asp?ord=6" & s_op & "&" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")) & "';" & chr(34) & ">Status</P></TD>"
	cab=cab & "<TD width='50' style='background-color:#fff' nowrap valign='middle'>&nbsp;</TD>"
	cab=cab & "</TR>" & chr(13)

	consulta = "SELECT * FROM t_ORCAMENTISTA_E_INDICADOR"

	s_where = ""
	if opcao_consulta = "A" then
		s_where = "status = 'A'"
	elseif opcao_consulta = "I" then
		s_where = "status = 'I'"
		end if
		
	if operacao_permitida(OP_LJA_CONSULTA_UNIVERSAL_PEDIDO_ORCAMENTO, s_lista_operacoes_permitidas) then
	    if s_where <> "" then s_where = s_where & " AND"
	    s_where = s_where & _
					"(" & _
						"(CONVERT(smallint, loja) = " & loja & ")" & _
						" OR " & _
						"(vendedor IN " & _
							"(" & _
								"SELECT DISTINCT " & _
									"usuario" & _
								" FROM t_USUARIO_X_LOJA" & _
								" WHERE" & _
									" (CONVERT(smallint, loja) = " & loja & ")" & _
							")" & _
						")" & _
					")"
	else
		'10/01/2020 - Unis - Desativa��o do acesso dos vendedores a todos os parceiros da Unis
		if (False And isLojaVrf(loja)) then
			if s_where <> "" then s_where = s_where & " AND"
			s_where = s_where & _
						"(" & _
							"(loja='" & loja & "'" & _
							")" & _
						")"
		else
			if s_where <> "" then s_where = s_where & " AND"
			s_where = s_where & _
						"(" & _
							"(vendedor='" & usuario & "'" & _
							")" & _
						")"
			end if
	end if
	
	if s_where <> "" then s_where = " WHERE " & s_where
	consulta = consulta & s_where
	
	consulta = consulta & " ORDER BY "
	select case ordenacao_selecionada
		case "1": consulta = consulta & "apelido"
		case "2": consulta = consulta & "razao_social_nome, apelido"
		case "3": consulta = consulta & "cidade"
		case "4": consulta = consulta & "vendedor, apelido"
		case "5": consulta = consulta & "hab_acesso_sistema"
		case "6": consulta = consulta & "status"
		case "7": consulta = consulta & "LEN(cnpj_cpf), cnpj_cpf"
		case "ID": consulta = consulta & "apelido"
		case "UF": consulta = consulta & "uf, cidade" & SQL_COLLATE_CASE_ACCENT & ", apelido"
		case else: consulta = consulta & "apelido"
		end select

  ' EXECUTA CONSULTA
	x=cab
	i=0
	iLineNumber = 0
	
	set r = cn.Execute( consulta )

	while not r.eof 
	  ' CONTAGEM
		i = i + 1
		iLineNumber = iLineNumber + 1

	  ' ALTERN�NCIA NAS CORES DAS LINHAS
		if (i AND 1)=0 then
			x=x & "<TR style='background: #FFF0E0'>"
		else
			x=x & "<TR>"
			end if

	 '> N� LINHA
		x=x & " <td class='MDB' align='right' valign='top'><span class='Rd' style='margin-right:2px;'>" & CStr(iLineNumber) & ".</span></td>"

	 '> APELIDO
		x=x & " <TD class='MDB' valign='top'><P class='C'>"
		x=x & "<a href='javascript:fOPConsultar(" & chr(34) & r("apelido") & chr(34)
		x=x & ")' title='clique para consultar o cadastro'>"
		x=x & r("apelido") & "</a></P></TD>"

	 '> NOME
		x=x & " <TD class='MDB' style='width:210px;' valign='top'><P class='C'>" 
		x=x & "<a href='javascript:fOPConsultar(" & chr(34) & r("apelido") & chr(34)
		x=x & ")' title='clique para consultar o cadastro'>"
		x=x & r("razao_social_nome_iniciais_em_maiusculas") & "</a></P></TD>"

	 '> CPF/CNPJ
		x=x & " <TD class='MDB' style='width:105px;' valign='top'><P class='C'>" 
		x=x & "<a href='javascript:fOPConsultar(" & chr(34) & r("apelido") & chr(34)
		x=x & ")' title='clique para consultar o cadastro'>"
		x=x & cnpj_cpf_formata(Trim("" & r("cnpj_cpf"))) & "</a></P></TD>"

	 '> TELEFONE
		s_telefones = ""
		s_ddd = Trim("" & r("ddd"))
		if s_ddd <> "" then s_ddd = "(" & s_ddd & ") "
		s_tel = Trim("" & r("telefone"))
		if s_tel <> "" then
			if s_telefones <> "" then s_telefones = s_telefones & "<br>"
			s_tel = telefone_formata(s_tel)
			s_telefones = s_telefones & s_ddd & s_tel
			end if
	'	FAX
		s_tel = Trim("" & r("fax"))
		if s_tel <> "" then
			if s_telefones <> "" then s_telefones = s_telefones & "<br>"
			s_tel = telefone_formata(s_tel)
			s_telefones = s_telefones & s_ddd & s_tel
			end if
	'	CELULAR
		s_ddd = Trim("" & r("ddd_cel"))
		if s_ddd <> "" then s_ddd = "(" & s_ddd & ") "
		s_tel = Trim("" & r("tel_cel"))
		if s_tel <> "" then
			if s_telefones <> "" then s_telefones = s_telefones & "<br>"
			s_tel = telefone_formata(s_tel)
			s_telefones = s_telefones & s_ddd & s_tel
			end if
	'	NEXTEL
		if Trim("" & r("nextel")) <> "" then
			if s_telefones <> "" then s_telefones = s_telefones & "<br>"
			s_telefones = s_telefones & Trim("" & r("nextel"))
			end if
		
		if s_telefones = "" then s_telefones = "&nbsp;"
		x=x & " <TD class='MDB' style='width:95px;' valign='top'><P class='C'>" & s_telefones & "</P></TD>"

    '>  CIDADE
		strCidade = iniciais_em_maiusculas(Trim("" & r("cidade")))
		strUF = Trim("" & r("uf"))
		if (strCidade <> "") And (strUF <> "") then 
			strCidade = strCidade & " / " & strUF 
		else 
			strCidade = strCidade & strUF
			end if
		if strCidade = "" then strCidade = "&nbsp;"
		x = x & "		<TD class='MD MB' valign='top' style='width:150px'>" & _
							"<P class='C'>" & strCidade & "</P>" & _
						"</TD>" & chr(13)

	 '> VENDEDOR
		s=Trim("" & r("vendedor"))
		if s="" then s="&nbsp;"
		x=x & " <TD class='MDB' valign='top' NOWRAP><P class='C'>" & s & "</P></TD>"

	 '> ACESSO AO SISTEMA
		if r("hab_acesso_sistema") = 1 then 
			s="<span style='color:#006600'>Liberado</span>"
		else 
			s="<span style='color:#ff0000'>Bloqueado</span>"
			end if
		x=x & " <TD class='MDB' valign='top' NOWRAP><P class='C'>" & s & "</P></TD>"

	 '> STATUS
		if Trim("" & r("status"))="A" then 
			s="<span style='color:#006600'>Ativo</span>"
		else 
			s="<span style='color:#ff0000'>Inativo</span>"
			end if
		x=x & " <TD class='MB MD' valign='top' NOWRAP><P class='C'>" & s & "</P></TD>"
		
	 '> CONSULTA / EDITA CADASTRO

		x=x & " <TD valign='middle' NOWRAP style='background-color:#fff;'><a href='javascript:fOPConsultar(""" & r("apelido") & """)'><img src='../imagem/lupa_20x20.png' style='border:0;width:18px;height:18px' title='Consultar cadastro'></a>"
		x=x & " <a href='javascript:fOPEditar(""" & r("apelido") & """)'><img src='../imagem/edita_20x20.gif' style='border:0;width:20px;height:20px' title='Editar cadastro'></a></TD>"
		
        x=x & "</TR>" & chr(13)

		if (i mod 100) = 0 then
			Response.Write x
			x = ""
			end if

		r.MoveNext
		wend


  ' MOSTRA TOTAL
	x=x & "<TR NOWRAP style='background: #FFFFDD'><TD COLSPAN='9' class='MD MB' NOWRAP><P class='Cd'>" & "TOTAL:&nbsp;&nbsp;&nbsp;" & cstr(i) & "</P></TD></TR>"

  ' FECHA TABELA
	x=x & "</TABLE>"
	

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
	<title>LOJA</title>
	</head>

<script language="JavaScript" type="text/javascript">
window.status='Aguarde, executando consulta ...';

function fOPConsultar(s_user){
	window.status = "Aguarde ...";
	fOP.id_selecionado.value = s_user;
	fOP.action = "OrcamentistaEIndicadorConsulta.asp";
	fOP.submit();
}
function fOPEditar(s_user) {
    window.status = "Aguarde ...";
    fOP.id_selecionado.value = s_user;
    fOP.action = "OrcamentistaEIndicadorEdita.asp";
    fOP.submit();
}

</script>

<link href="<%=URL_FILE__E_CSS%>" Rel="stylesheet" Type="text/css">
<link href="<%=URL_FILE__EPRINTER_CSS%>" Rel="stylesheet" Type="text/css" media="print">


<body onload="window.status='Conclu�do';" link=#000000 alink=#000000 vlink=#000000>

<!--  I D E N T I F I C A � � O  -->
<table width="100%" cellPadding="4" CellSpacing="0" style="border-bottom:1px solid black">
<tr>
	<td align="right" valign="bottom" nowrap><span class="PEDIDO">Rela��o de Or�amentistas / Indicadores Cadastrados</span>
	<br><span class="Rc">
		<a href="resumo.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="retorna para p�gina inicial" class="LPagInicial">p�gina inicial</a>&nbsp;&nbsp;&nbsp;
		<a href="sessaoencerra.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="encerra a sess�o do usu�rio" class="LSessaoEncerra">encerra</a>
		</span></td>
</tr>
</table>


<!--  RELA��O DE USU�RIOS  -->
<br>
<center>
<form method="post" id="fOP" name="fOP">
<%=MontaCampoFormSessionCtrlInfo(Session("SessionCtrlInfo"))%>
<input type="hidden" name='id_selecionado' id="id_selecionado" value=''>
<input type="hidden" name='operacao_selecionada' id="operacao_selecionada" value='<%=OP_CONSULTA%>'>
<input type="hidden" name="url_origem" id="url_origem" value="MenuOrcamentistaEIndicador.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" />

<% executa_consulta %>
</form>

<br>

<p class="TracoBottom"></p>

<table class="notPrint" cellSpacing="0">
<tr>
	<td align="center"><a href="MenuOrcamentistaEIndicador.asp<%= "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo"))%>" title="volta para a p�gina anterior">
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