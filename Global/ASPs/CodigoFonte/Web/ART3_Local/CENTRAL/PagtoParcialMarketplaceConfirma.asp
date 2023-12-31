<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<% Response.Buffer=True %>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp" -->

<!-- #include file = "../global/TrataSessaoExpirada.asp"        -->

<%
'     ========================================================
'	  PagtoParcialMarketplaceConfirma.asp
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


	class cl_PAGTO_PARCIAL_MARKETPLACE_CONFIRMA
		dim pedido_marketplace
		dim vl_parcela
		dim pedido_ERP
		dim pedido_base_ERP
		dim msg_erro
		end class

	On Error GoTo 0
	Err.Clear

	dim s, usuario, msg_erro, s_log, s_log_aux
	usuario = Trim(Session("usuario_atual"))
	If (usuario = "") then Response.Redirect("aviso.asp?id=" & ERR_SESSAO)

	dim alerta
	alerta=""

	dim i, iPedido
	dim c_dados_pagto, v_pedido, v_dados_pagto, v
	c_dados_pagto = Trim(Request("c_dados_pagto"))
	if c_dados_pagto = "" then Response.Redirect("aviso.asp?id=" & ERR_PARAMETRO_OBRIGATORIO_NAO_ESPECIFICADO)

	redim v_pedido(0)
	set v_pedido(Ubound(v_pedido))=New cl_PAGTO_PARCIAL_MARKETPLACE_CONFIRMA
	with v_pedido(Ubound(v_pedido))
		.pedido_marketplace = ""
		.vl_parcela = 0
		.pedido_ERP = ""
		.pedido_base_ERP = ""
		.msg_erro = ""
		end with

	v_dados_pagto = Split(c_dados_pagto, "|")
	for i=LBound(v_dados_pagto) to UBound(v_dados_pagto)
		v_dados_pagto(i) = Trim("" & v_dados_pagto(i))
		if v_dados_pagto(i) <> "" then
			if Trim(v_pedido(Ubound(v_pedido)).pedido_marketplace)<>"" then
				redim preserve v_pedido(Ubound(v_pedido)+1)
				set v_pedido(Ubound(v_pedido))=New cl_PAGTO_PARCIAL_MARKETPLACE_CONFIRMA
				with v_pedido(Ubound(v_pedido))
					.pedido_marketplace = ""
					.vl_parcela = 0
					.pedido_ERP = ""
					.pedido_base_ERP = ""
					.msg_erro = ""
					end with
				end if

			v = Split(v_dados_pagto(i), "=")
			with v_pedido(Ubound(v_pedido))
				.pedido_marketplace = Trim("" & v(LBound(v)))
				.pedido_ERP = Trim("" & v(LBound(v)+1))
				.pedido_base_ERP = retorna_num_pedido_base(.pedido_ERP)
				.vl_parcela = converte_numero(Trim("" & v(LBound(v)+2)))
				
				if .pedido_marketplace = "" then
					alerta=texto_add_br(alerta)
					alerta=alerta & "N� pedido marketplace n�o informado."
					end if

				if .pedido_ERP = "" then
					alerta=texto_add_br(alerta)
					alerta=alerta & "N� pedido ERP n�o informado."
					end if
				
				if .vl_parcela = 0 then
					alerta=texto_add_br(alerta)
					alerta=alerta & "Valor do pagamento parcial � inv�lido (pedido marketplace n� " & pedido_marketplace & ")"
					end if
				end with
			end if
		next

	dim vl_TotalFamiliaPrecoVenda_aux, vl_TotalFamiliaPrecoNF_aux, vl_TotalFamiliaPago_aux, vl_TotalFamiliaDevolucaoPrecoVenda_aux, vl_TotalFamiliaDevolucaoPrecoNF_aux, st_pagto_aux
	dim s_chave
	dim intNumParcelasPagtoCartao

'	CONECTA AO BANCO DE DADOS
'	=========================
	dim cn, rs
	If Not bdd_conecta(cn) then Response.Redirect("aviso.asp?id=" & ERR_CONEXAO)

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

		s_log = ""
		for iPedido = LBound(v_pedido) to UBound(v_pedido)
			if (alerta = "") And (v_pedido(iPedido).pedido_ERP <> "") then
				s_log_aux = ""
				with v_pedido(iPedido)
					if Not calcula_pagamentos(.pedido_ERP, vl_TotalFamiliaPrecoVenda_aux, vl_TotalFamiliaPrecoNF_aux, vl_TotalFamiliaPago_aux, vl_TotalFamiliaDevolucaoPrecoVenda_aux, vl_TotalFamiliaDevolucaoPrecoNF_aux, st_pagto_aux, msg_erro) then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_BD)
					if Not gera_nsu(NSU_PEDIDO_PAGAMENTO, s_chave, msg_erro) then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_GERAR_NSU)

					s = "INSERT INTO t_PEDIDO_PAGAMENTO" & _
						" (id, pedido, data, hora, valor, usuario, tipo_pagto)" & _
						" VALUES (" & _
						" '" & s_chave & "'" & _
						", '" & .pedido_ERP & "'" & _
						", " & bd_formata_data(Date) & _
						", '" & retorna_so_digitos(formata_hora(Now)) & "'" & _
						", " & bd_formata_numero(.vl_parcela) & _
						", '" & usuario & "'" & _
						", '" & COD_PAGTO_PARCIAL & "'" & _
						")"
					cn.Execute(s)
					if Err <> 0 then
						alerta=Cstr(Err) & ": " & Err.Description
					else
						s = "SELECT * FROM t_PEDIDO WHERE (pedido = '" & .pedido_base_ERP & "')"
						if rs.State <> 0 then rs.Close
						rs.Open s, cn
						if rs.Eof then
							alerta = "Pedido-base " & .pedido_base_ERP & " n�o foi encontrado."
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
				
						'	PAGO (QUITADO)
						'	~~~~~~~~~~~~~~
							if (vl_TotalFamiliaDevolucaoPrecoNF_aux + vl_TotalFamiliaPago_aux + .vl_parcela) >= (vl_TotalFamiliaPrecoNF_aux - MAX_VALOR_MARGEM_ERRO_PAGAMENTO) then
								if Trim("" & rs("st_pagto")) <> ST_PAGTO_PAGO then
									rs("dt_st_pagto") = Date
									rs("dt_hr_st_pagto") = Now
									rs("usuario_st_pagto") = usuario
									end if
								rs("st_pagto") = ST_PAGTO_PAGO
								s_log_aux = "quitado"
								if (vl_TotalFamiliaDevolucaoPrecoNF_aux + vl_TotalFamiliaPago_aux + .vl_parcela) > vl_TotalFamiliaPrecoNF_aux then 
									s_log_aux = s_log_aux & " (excedeu " & SIMBOLO_MONETARIO & " " & _
											formata_moeda((vl_TotalFamiliaDevolucaoPrecoNF_aux+vl_TotalFamiliaPago_aux+.vl_parcela)-vl_TotalFamiliaPrecoNF_aux) & ")"
								elseif (vl_TotalFamiliaDevolucaoPrecoNF_aux + vl_TotalFamiliaPago_aux + .vl_parcela) < vl_TotalFamiliaPrecoNF_aux then
									s_log_aux = s_log_aux & " (faltou " & SIMBOLO_MONETARIO & " " & _
											formata_moeda(vl_TotalFamiliaPrecoNF_aux-(vl_TotalFamiliaDevolucaoPrecoNF_aux+vl_TotalFamiliaPago_aux+.vl_parcela)) & ")"
									end if
							'	AN�LISE DE CR�DITO
								if Trim("" & rs("loja")) = NUMERO_LOJA_ECOMMERCE_AR_CLUBE then
									if (CLng(rs("analise_credito")) = CLng(COD_AN_CREDITO_ST_INICIAL)) then
										if s_log_aux <> "" then s_log_aux = s_log_aux & "; "
										s_log_aux = s_log_aux & " An�lise de cr�dito: " & descricao_analise_credito(rs("analise_credito")) & " => " & descricao_analise_credito(COD_AN_CREDITO_OK)
										rs("analise_credito") = CLng(COD_AN_CREDITO_OK)
										rs("analise_credito_data")=Now
										rs("analise_credito_usuario")=ID_USUARIO_SISTEMA
										end if
								else
								'	TRATAMENTO P/ O CASO EM QUE A CIELO INFORMA O STATUS "1 - EM ANDAMENTO" QUANDO ATIVA A P�GINA DE RETORNO, MAS ACABA
								'	AUTORIZANDO A TRANSA��O. NESTE CASO, O REGISTRO DO PAGAMENTO PRECISA SER FEITO MANUALMENTE NO PEDIDO.
									if (CLng(rs("analise_credito")) = CLng(COD_AN_CREDITO_ST_INICIAL)) And CLng(rs("st_forma_pagto_somente_cartao")) = 1 then
									'	SE HOUVER PARCELA PAGA EM CART�O, A EQUIPE DA AN�LISE DE CR�DITO DEVE VERIFICAR A NECESSIDADE DE PEDIR DOCUMENTA��O COMPLEMENTAR AO CLIENTE
										If (intNumParcelasPagtoCartao = 0) then
											if s_log_aux <> "" then s_log_aux = s_log_aux & "; "
											s_log_aux = s_log_aux & " An�lise de cr�dito: " & descricao_analise_credito(rs("analise_credito")) & " => " & descricao_analise_credito(COD_AN_CREDITO_OK)
											rs("analise_credito") = CLng(COD_AN_CREDITO_OK)
											rs("analise_credito_data")=Now
											rs("analise_credito_usuario")=ID_USUARIO_SISTEMA
										else
											if s_log_aux <> "" then s_log_aux = s_log_aux & "; "
											s_log_aux = s_log_aux & " An�lise de cr�dito: " & descricao_analise_credito(rs("analise_credito")) & " => " & descricao_analise_credito(COD_AN_CREDITO_PENDENTE_VENDAS)
											rs("analise_credito") = CLng(COD_AN_CREDITO_PENDENTE_VENDAS)
											rs("analise_credito_data")=Now
											rs("analise_credito_usuario")=ID_USUARIO_SISTEMA
											end if
										end if
									end if
						'	PAGAMENTO PARCIAL
						'	~~~~~~~~~~~~~~~~~
							elseif (vl_TotalFamiliaPago_aux + .vl_parcela) > 0 then
								if Trim("" & rs("st_pagto")) <> ST_PAGTO_PARCIAL then
									rs("dt_st_pagto") = Date
									rs("dt_hr_st_pagto") = Now
									rs("usuario_st_pagto") = usuario
									end if
								rs("st_pagto") = ST_PAGTO_PARCIAL
								s_log_aux = "pago parcial"
						'	N�O PAGO
						'	~~~~~~~~
							else
								if Trim("" & rs("st_pagto")) <> ST_PAGTO_NAO_PAGO then
									rs("dt_st_pagto") = Date
									rs("dt_hr_st_pagto") = Now
									rs("usuario_st_pagto") = usuario
									end if
								rs("st_pagto") = ST_PAGTO_NAO_PAGO
								s_log_aux = "n�o-pago"
								end if
				
							rs("vl_pago_familia") = vl_TotalFamiliaPago_aux + .vl_parcela
							s_log_aux = "; status=" & s_log_aux
							rs.Update
							if Err <> 0 then
								alerta=Cstr(Err) & ": " & Err.Description
								end if
							end if
						end if

					if s_log <> "" then s_log = s_log & " " & chr(13)
					s_log = s_log & .pedido_ERP & " (" & .pedido_marketplace & "): " & SIMBOLO_MONETARIO & " " & formata_moeda(.vl_parcela) & s_log_aux
					end with
				end if
			next 'for iPedido = LBound(v_pedido) to UBound(v_pedido)

		if alerta = "" then
			if s_log <> "" then
				grava_log usuario, "", "(BATCH)", "", OP_LOG_PEDIDO_PAGTO_PARCIAL, s_log
				end if
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
			end if 'if alerta = "" then
		end if 'if alerta = "" then
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