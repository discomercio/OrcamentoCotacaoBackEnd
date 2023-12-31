<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<% Response.Buffer=True %>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp" -->

<!-- #include file = "../global/TrataSessaoExpirada.asp"        -->

<%
'     ========================================================
'	  PagtoQuitacaoMarketplaceConfirma.asp
'     ========================================================
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

	class cl_PAGTO_QUITACAO_MARKETPLACE_CONFIRMA
		dim pedido_marketplace
		dim pedido_ERP
		end class

	On Error GoTo 0
	Err.Clear

	dim s, usuario, msg_erro, s_log, lista_pedido_marketplace_x_ERP
	dim v, v_lista, v_pedido, i, achou
	usuario = Trim(Session("usuario_atual"))
	If (usuario = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO) 

	dim alerta
	alerta=""

'	OBT�M DADOS DO FORMUL�RIO
	lista_pedido_marketplace_x_ERP = ucase(Trim(request("lista_pedido_marketplace_x_ERP")))
	if (lista_pedido_marketplace_x_ERP = "") then Response.Redirect("aviso.asp?id=" & ERR_PEDIDO_NAO_ESPECIFICADO)
	
	redim v_pedido(0)
	set v_pedido(Ubound(v_pedido))=New cl_PAGTO_QUITACAO_MARKETPLACE_CONFIRMA
	with v_pedido(Ubound(v_pedido))
		.pedido_marketplace = ""
		.pedido_ERP = ""
		end with

	v_lista = split(lista_pedido_marketplace_x_ERP, "|", -1)
	achou=False
	for i=Lbound(v_lista) to Ubound(v_lista)
		if Trim(v_lista(i)) <> "" then
			v = Split(v_lista(i), "=", -1)
			if (LBound(v) = UBound(v)) Or (Trim("" & v(LBound(v))) = "") Or (Trim("" & v(UBound(v))) = "") then
				alerta=texto_add_br(alerta)
				alerta=alerta & "Rela��o de pedidos com formato inv�lido na linha " & Cstr(i+1) & ": " & v_lista(i)
			else
				if Trim("" & v_pedido(UBound(v_pedido)).pedido_marketplace) <> "" then
					redim preserve v_pedido(UBound(v_pedido)+1)
					set v_pedido(Ubound(v_pedido))=New cl_PAGTO_QUITACAO_MARKETPLACE_CONFIRMA
					end if
				achou = True
				with v_pedido(Ubound(v_pedido))
					.pedido_marketplace = Trim("" & v(0))
					.pedido_ERP = normaliza_num_pedido(v(1))
					end with
				end if
			end if
		next

	if Not achou then Response.Redirect("aviso.asp?id=" & ERR_PEDIDO_NAO_ESPECIFICADO)

	achou=False
	for i=LBound(v_pedido) to UBound(v_pedido)
		if (Trim("" & v_pedido(i).pedido_marketplace) = "") And (Trim("" & v_pedido(i).pedido_ERP) = "") then
			' NOP
		else
			if (Trim("" & v_pedido(i).pedido_marketplace) <> "") And (Trim("" & v_pedido(i).pedido_ERP) <> "") then
				achou = True
			else
				alerta=texto_add_br(alerta)
				alerta=alerta & "Falha ao tentar estabelecer o v�nculo entre o n� pedido marketplace e ERP (n� marketplace = " & Trim("" & v_pedido(i).pedido_marketplace) & ", n� ERP = " & Trim("" & v_pedido(i).pedido_ERP) & ")"
				end if
			end if
		next

	if Not achou then Response.Redirect("aviso.asp?id=" & ERR_PEDIDO_NAO_ESPECIFICADO)

	dim st_pagto, vl_TotalFamiliaPrecoVenda, vl_TotalFamiliaPrecoNF, vl_TotalFamiliaPago, vl_TotalFamiliaDevolucaoPrecoVenda, vl_TotalFamiliaDevolucaoPrecoNF, id_pedido_base, s_chave
	dim intNumParcelasPagtoCartao

'	CONECTA AO BANCO DE DADOS
'	=========================
	dim cn, rs
	If Not bdd_conecta(cn) then Response.Redirect("aviso.asp?id=" & ERR_CONEXAO)
	
	s_log = ""
	
	if alerta = "" then
	'	~~~~~~~~~~~~~
		cn.BeginTrans
	'	~~~~~~~~~~~~~
		If Not cria_recordset_pessimista(rs, msg_erro) then 
		'	~~~~~~~~~~~~~~~~
			cn.RollbackTrans
		'	~~~~~~~~~~~~~~~~
			Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_CRIAR_ADO)
			end if

		for i=Lbound(v_pedido) to Ubound(v_pedido)
			if v_pedido(i).pedido_ERP <> "" then
				if Not calcula_pagamentos(v_pedido(i).pedido_ERP, vl_TotalFamiliaPrecoVenda, vl_TotalFamiliaPrecoNF, vl_TotalFamiliaPago, vl_TotalFamiliaDevolucaoPrecoVenda, vl_TotalFamiliaDevolucaoPrecoNF, st_pagto, msg_erro) then 
				'	~~~~~~~~~~~~~~~~
					cn.RollbackTrans
				'	~~~~~~~~~~~~~~~~
					Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_BD)
					end if
					
				if vl_TotalFamiliaPrecoNF >= (vl_TotalFamiliaPago + vl_TotalFamiliaDevolucaoPrecoNF) then
					if Not gera_nsu(NSU_PEDIDO_PAGAMENTO, s_chave, msg_erro) then 
					'	~~~~~~~~~~~~~~~~
						cn.RollbackTrans
					'	~~~~~~~~~~~~~~~~
						Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_GERAR_NSU)
						end if

					id_pedido_base = retorna_num_pedido_base(v_pedido(i).pedido_ERP)
					
					s = "INSERT INTO t_PEDIDO_PAGAMENTO" & _
						" (id, pedido, pedido_marketplace, data, hora, valor, usuario, tipo_pagto)" & _
						" VALUES (" & _
						" '" & s_chave & "'" & _
						", '" & v_pedido(i).pedido_ERP & "'" & _
						", '" & v_pedido(i).pedido_marketplace & "'" & _
						", " & bd_formata_data(Date) & _
						", '" & retorna_so_digitos(formata_hora(Now)) & "'" & _
						", " & bd_formata_numero(vl_TotalFamiliaPrecoNF-vl_TotalFamiliaPago-vl_TotalFamiliaDevolucaoPrecoNF) & _
						", '" & usuario & "'" & _
						", '" & COD_PAGTO_QUITACAO & "'" & _
						")"
					cn.Execute(s)
					if Err <> 0 then
						alerta=texto_add_br(alerta)
						alerta=alerta & Cstr(Err) & ": " & Err.Description
					else
						s = "SELECT * FROM t_PEDIDO WHERE (pedido = '" & id_pedido_base & "')"
						if rs.State <> 0 then rs.Close
						rs.Open s, cn
						if rs.Eof then
							alerta=texto_add_br(alerta)
							alerta=alerta & "Pedido-base " & id_pedido_base & " n�o foi encontrado."
						else
						'	CALCULA QUANTIDADE DE PARCELAS EM CART�O DE CR�DITO
							intNumParcelasPagtoCartao = 0
							if Trim("" & rs("tipo_parcelamento")) = Trim("" & COD_FORMA_PAGTO_A_VISTA) then
								if Trim("" & rs("av_forma_pagto")) = Trim("" & ID_FORMA_PAGTO_CARTAO) then intNumParcelasPagtoCartao = 1
							elseif Trim("" & rs("tipo_parcelamento")) = Trim("" & COD_FORMA_PAGTO_PARCELA_UNICA) then
								if Trim("" & rs("pu_forma_pagto")) = Trim("" & ID_FORMA_PAGTO_CARTAO) then intNumParcelasPagtoCartao = 1
							elseif Trim("" & rs("tipo_parcelamento")) = Trim("" & COD_FORMA_PAGTO_PARCELADO_CARTAO) then
								intNumParcelasPagtoCartao = rs("pc_qtde_parcelas")
							elseif Trim("" & rs("tipo_parcelamento")) = Trim("" & COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA) then
								'NOP
							elseif Trim("" & rs("tipo_parcelamento")) = Trim("" & COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA) then
							'	ENTRADA + PRESTA��ES
								if Trim("" & rs("pce_forma_pagto_entrada")) = Trim("" & ID_FORMA_PAGTO_CARTAO) then intNumParcelasPagtoCartao = intNumParcelasPagtoCartao + 1
								if Trim("" & rs("pce_forma_pagto_prestacao")) = Trim("" & ID_FORMA_PAGTO_CARTAO) then intNumParcelasPagtoCartao = intNumParcelasPagtoCartao + rs("pce_prestacao_qtde")
							elseif Trim("" & rs("tipo_parcelamento")) = Trim("" & COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA) then
							'	1� PRESTA��O + DEMAIS PRESTA��ES
								if Trim("" & rs("pse_forma_pagto_prim_prest")) = Trim("" & ID_FORMA_PAGTO_CARTAO) then intNumParcelasPagtoCartao = intNumParcelasPagtoCartao + 1
								if Trim("" & rs("pse_forma_pagto_demais_prest")) = Trim("" & ID_FORMA_PAGTO_CARTAO) then intNumParcelasPagtoCartao = intNumParcelasPagtoCartao + rs("pse_demais_prest_qtde")
								end if

							if Trim("" & rs("st_pagto")) <> ST_PAGTO_PAGO then
								rs("dt_st_pagto") = Date
								rs("dt_hr_st_pagto") = Now
								rs("usuario_st_pagto") = usuario
								end if
							rs("st_pagto") = ST_PAGTO_PAGO
							rs("vl_pago_familia") = vl_TotalFamiliaPrecoNF-vl_TotalFamiliaDevolucaoPrecoNF
							rs.Update
							if Err <> 0 then 
								alerta=texto_add_br(alerta)
								alerta=alerta & Cstr(Err) & ": " & Err.Description
								end if
						'	INFORMA��ES PARA O LOG
							if s_log <> "" then s_log = s_log & "; "
							s_log = s_log & v_pedido(i).pedido_ERP & " (n� marketplace: " & v_pedido(i).pedido_marketplace & ")" & " = " & SIMBOLO_MONETARIO & " " & formata_moeda(vl_TotalFamiliaPrecoNF-vl_TotalFamiliaPago-vl_TotalFamiliaDevolucaoPrecoNF)
						'	AN�LISE DE CR�DITO
							if Trim("" & rs("loja")) = NUMERO_LOJA_ECOMMERCE_AR_CLUBE then
								if (CLng(rs("analise_credito")) = CLng(COD_AN_CREDITO_ST_INICIAL)) then
									if s_log <> "" then s_log = s_log & "; "
									s_log = s_log & " An�lise de cr�dito: " & descricao_analise_credito(rs("analise_credito")) & " => " & descricao_analise_credito(COD_AN_CREDITO_OK)
									rs("analise_credito") = CLng(COD_AN_CREDITO_OK)
									rs("analise_credito_data")=Now
									rs("analise_credito_usuario")=ID_USUARIO_SISTEMA
									rs.Update
									end if
							else
							'	TRATAMENTO P/ O CASO EM QUE A CIELO INFORMA O STATUS "1 - EM ANDAMENTO" QUANDO ATIVA A P�GINA DE RETORNO, MAS ACABA
							'	AUTORIZANDO A TRANSA��O. NESTE CASO, O REGISTRO DO PAGAMENTO PRECISA SER FEITO MANUALMENTE NO PEDIDO.
								if (CLng(rs("analise_credito")) = CLng(COD_AN_CREDITO_ST_INICIAL)) And (CLng(rs("st_forma_pagto_somente_cartao")) = 1) then
								'	SE HOUVER PARCELA PAGA EM CART�O, A EQUIPE DA AN�LISE DE CR�DITO DEVE VERIFICAR A NECESSIDADE DE PEDIR DOCUMENTA��O COMPLEMENTAR AO CLIENTE
									If (intNumParcelasPagtoCartao = 0) then
										if s_log <> "" then s_log = s_log & "; "
										s_log = s_log & " An�lise de cr�dito: " & descricao_analise_credito(rs("analise_credito")) & " => " & descricao_analise_credito(COD_AN_CREDITO_OK)
										rs("analise_credito") = CLng(COD_AN_CREDITO_OK)
										rs("analise_credito_data")=Now
										rs("analise_credito_usuario")=ID_USUARIO_SISTEMA
										rs.Update
									else
										if s_log <> "" then s_log = s_log & "; "
										s_log = s_log & " An�lise de cr�dito: " & descricao_analise_credito(rs("analise_credito")) & " => " & descricao_analise_credito(COD_AN_CREDITO_PENDENTE_VENDAS)
										rs("analise_credito") = CLng(COD_AN_CREDITO_PENDENTE_VENDAS)
										rs("analise_credito_data")=Now
										rs("analise_credito_usuario")=ID_USUARIO_SISTEMA
										rs.Update
										end if
									end if
								end if
							end if
						end if
					end if
				end if
			
		'	SE HOUVE ERRO, CANCELA O LA�O
			if alerta <> "" then exit for
			next

		if alerta = "" then
			grava_log usuario, "", "", "", OP_LOG_PEDIDO_PAGTO_QUITACAO, s_log
		'	~~~~~~~~~~~~~~
			cn.CommitTrans
		'	~~~~~~~~~~~~~~
			if Err=0 then 
				Response.Redirect("resumo.asp" & "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")))
			else
				alerta=Cstr(Err) & ": " & Err.Description
				end if
		else
		'	~~~~~~~~~~~~~~~~
			cn.RollbackTrans
		'	~~~~~~~~~~~~~~~~
			end if
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
	<title>CENTRAL</title>
	</head>



<script src="<%=URL_FILE__GLOBAL_JS%>" Language="JavaScript" Type="text/javascript"></script>



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
<table class="notPrint" cellSpacing="0">
<tr>
	<td align="center"><a name="bVOLTAR" id="bVOLTAR" href="javascript:history.back()"><img src="../botao/voltar.gif" width="176" height="55" border="0"></a></td>
</tr>
</table>
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