<%@ Language=VBScript %>
<%OPTION EXPLICIT%>
<% Response.Buffer=True %>
<% Response.Expires=-1 %>
<!-- #include file = "../global/constantes.asp" -->
<!-- #include file = "../global/funcoes.asp"    -->
<!-- #include file = "../global/bdd.asp" -->
<!-- #include file = "../global/Braspag.asp"    -->

<%
'     =============================================================
'	  AjaxBraspagPagCancelOuEstorno.asp
'     =============================================================
'
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
	
	Server.ScriptTimeout = MAX_SERVER_SCRIPT_TIMEOUT_BRASPAG_EM_SEG
	
	dim usuario, id_pedido_pagto_braspag, id_pedido_pagto_braspag_pag, msg_erro
	usuario = Trim(Request("usuario"))
	id_pedido_pagto_braspag = Trim(Request("id_pedido_pagto_braspag"))
	id_pedido_pagto_braspag_pag = Trim(Request("id_pedido_pagto_braspag_pag"))
	
	if id_pedido_pagto_braspag = "" then
		Response.Write "Identificador n�o informado!!"
		Response.End
		end if
	
	if id_pedido_pagto_braspag_pag = "" then
		Response.Write "Identificador da opera��o com o Pagador n�o informado!!"
		Response.End
		end if
	
	if converte_numero(id_pedido_pagto_braspag) = 0 then
		Response.Write "Identificador informado � inv�lido (" & id_pedido_pagto_braspag & ")!!"
		Response.End
		end if
	
	if converte_numero(id_pedido_pagto_braspag_pag) = 0 then
		Response.Write "Identificador da opera��o com o Pagador � inv�lido (" & id_pedido_pagto_braspag_pag & ")!!"
		Response.End
		end if
	
	dim cn
	dim strResp
'	CONECTA COM O BANCO DE DADOS
	If Not bdd_conecta(cn) then
		msg_erro = erro_descricao(ERR_CONEXAO)
		Response.Write msg_erro
		Response.End
		end if
	
'	EXECUTA A CONSULTA P/ ATUALIZAR O STATUS E VERIFICAR SE A TRANSA��O EST� CAPTURADA E QUAL � A DATA DA CAPTURA
	dim trx_Get_1, r_rx_Get_1
	call BraspagProcessaConsulta_GetTransactionData(id_pedido_pagto_braspag, id_pedido_pagto_braspag_pag, usuario, trx_Get_1, r_rx_Get_1, msg_erro)
	if msg_erro <> "" then
		Response.Write msg_erro
		Response.End
		end if
	
	dim stGlobalStatus_1
	stGlobalStatus_1 = decodifica_GetTransactionDataResponseStatus_para_GlobalStatus(r_rx_Get_1.PAG_Status)
	if Trim("" & stGlobalStatus_1) <> Trim("" & BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__CAPTURADA) then
		Response.Write "N�o � poss�vel realizar o cancelamento/estorno porque a transa��o n�o est� com status 'Capturada'"
		Response.End
		end if
	
	if Trim("" & r_rx_Get_1.PAG_CapturedDate) = "" then
		Response.Write "N�o � poss�vel realizar o cancelamento/estorno porque a data de captura da transa��o n�o foi informada pela Braspag"
		Response.End
		end if
	
	dim blnRequestVoid, stGlobalStatus_2, stSucessoOperacao
	dim dtCapturedDate
	dtCapturedDate = converte_para_datetime_from_mmddyyyy_hhmmss_am_pm(r_rx_Get_1.PAG_CapturedDate)
	
'	O CANCELAMENTO (VOID) PODE SER FEITO AT� AS 23:59:59 DO MESMO DIA DA CAPTURA, AP�S ISSO DEVE SER FEITO UM ESTORNO (REFUND)
	if formata_data_yyyymmdd(Date) = formata_data_yyyymmdd(dtCapturedDate) then
		blnRequestVoid = True
	else
		blnRequestVoid = False
		end if
	
	if blnRequestVoid then
	'	CANCELAMENTO (VOID)
		dim trx_Void, r_rx_Void
		call BraspagProcessaRequisicao_VoidCreditCardTransaction(id_pedido_pagto_braspag, id_pedido_pagto_braspag_pag, usuario, trx_Void, r_rx_Void, msg_erro)
		if msg_erro <> "" then
			Response.Write msg_erro
			Response.End
			end if
	else
	'	ESTORNO (REFUND)
		dim trx_Refund, r_rx_Refund
		call BraspagProcessaRequisicao_RefundCreditCardTransaction(id_pedido_pagto_braspag, id_pedido_pagto_braspag_pag, usuario, trx_Refund, r_rx_Refund, msg_erro)
		if msg_erro <> "" then
			Response.Write msg_erro
			Response.End
			end if
		end if
	
'	REALIZA NOVA CONSULTA P/ OBTER O STATUS ATUALIZADO
	dim trx_Get_2, r_rx_Get_2
	call BraspagProcessaConsulta_GetTransactionData(id_pedido_pagto_braspag, id_pedido_pagto_braspag_pag, usuario, trx_Get_2, r_rx_Get_2, msg_erro)
	if msg_erro <> "" then
		Response.Write msg_erro
		Response.End
		end if
	
	stGlobalStatus_2 = decodifica_GetTransactionDataResponseStatus_para_GlobalStatus(r_rx_Get_2.PAG_Status)
	
	if blnRequestVoid then
		if r_rx_Void.PAG_Status = BRASPAG_PAGADOR_CARTAO_VOIDCREDITCARDTRANSACTIONRESPONSE_STATUS__VOID_CONFIRMED then
			stSucessoOperacao = "1"
		else
			stSucessoOperacao = "0"
			end if
		
	'	MONTA RESPOSTA
		strResp = "{ " & _
					"""id_pedido_pagto_braspag"" : """ & id_pedido_pagto_braspag & """," & _
					"""id_pedido_pagto_braspag_pag"" : """ & id_pedido_pagto_braspag_pag & """," & _
					"""PAG_TipoOperacao"" : """ & "VOID" & """," & _
					"""PAG_SucessoOperacao"" : """ & stSucessoOperacao & """," & _
					"""PAG_CorrelationId"" : """ & r_rx_Void.PAG_CorrelationId & """," & _
					"""PAG_BraspagTransactionId"" : """ & r_rx_Void.PAG_BraspagTransactionId & """," & _
					"""PAG_AcquirerTransactionId"" : """ & r_rx_Void.PAG_AcquirerTransactionId & """," & _
					"""PAG_Amount"" : """ & r_rx_Void.PAG_Amount & """," & _
					"""PAG_AuthorizationCode"" : """ & r_rx_Void.PAG_AuthorizationCode & """," & _
					"""PAG_ReturnCode"" : """ & r_rx_Void.PAG_ReturnCode & """," & _
					"""PAG_ReturnMessage"" : """ & r_rx_Void.PAG_ReturnMessage & """," & _
					"""PAG_GlobalStatus"" : """ & stGlobalStatus_2 & """," & _
					"""PAG_DescricaoGlobalStatus"" : """ & BraspagPagadorDescricaoGlobalStatus(stGlobalStatus_2) & """," & _
					"""PAG_GlobalStatus_atualizacao_data_hora"" : """ & formata_data_hora(Now) & """," & _
					"""PAG_ProofOfSale"" : """ & r_rx_Void.PAG_ProofOfSale & """," & _
					"""PAG_ServiceTaxAmount"" : """ & r_rx_Void.PAG_ServiceTaxAmount & """," & _
					"""PAG_ErrorCode"" : """ & r_rx_Void.PAG_ErrorCode & """," & _
					"""PAG_ErrorMessage"" : """ & r_rx_Void.PAG_ErrorMessage & """," & _
					"""PAG_faultcode"" : """ & r_rx_Void.PAG_faultcode & """," & _
					"""PAG_faultstring"" : """ & r_rx_Void.PAG_faultstring & """" & _
				" }"
	else
		if r_rx_Refund.PAG_Status = BRASPAG_PAGADOR_CARTAO_REFUNDCREDITCARDTRANSACTIONRESPONSE_STATUS__REFUND_CONFIRMED then
			stSucessoOperacao = "1"
		else
			stSucessoOperacao = "0"
			end if
		
	'	MONTA RESPOSTA
		strResp = "{ " & _
					"""id_pedido_pagto_braspag"" : """ & id_pedido_pagto_braspag & """," & _
					"""id_pedido_pagto_braspag_pag"" : """ & id_pedido_pagto_braspag_pag & """," & _
					"""PAG_TipoOperacao"" : """ & "REFUND" & """," & _
					"""PAG_SucessoOperacao"" : """ & stSucessoOperacao & """," & _
					"""PAG_CorrelationId"" : """ & r_rx_Refund.PAG_CorrelationId & """," & _
					"""PAG_BraspagTransactionId"" : """ & r_rx_Refund.PAG_BraspagTransactionId & """," & _
					"""PAG_AcquirerTransactionId"" : """ & r_rx_Refund.PAG_AcquirerTransactionId & """," & _
					"""PAG_Amount"" : """ & r_rx_Refund.PAG_Amount & """," & _
					"""PAG_AuthorizationCode"" : """ & r_rx_Refund.PAG_AuthorizationCode & """," & _
					"""PAG_ReturnCode"" : """ & r_rx_Refund.PAG_ReturnCode & """," & _
					"""PAG_ReturnMessage"" : """ & r_rx_Refund.PAG_ReturnMessage & """," & _
					"""PAG_GlobalStatus"" : """ & stGlobalStatus_2 & """," & _
					"""PAG_DescricaoGlobalStatus"" : """ & BraspagPagadorDescricaoGlobalStatus(stGlobalStatus_2) & """," & _
					"""PAG_GlobalStatus_atualizacao_data_hora"" : """ & formata_data_hora(Now) & """," & _
					"""PAG_ProofOfSale"" : """ & r_rx_Refund.PAG_ProofOfSale & """," & _
					"""PAG_ServiceTaxAmount"" : """ & r_rx_Refund.PAG_ServiceTaxAmount & """," & _
					"""PAG_ErrorCode"" : """ & r_rx_Refund.PAG_ErrorCode & """," & _
					"""PAG_ErrorMessage"" : """ & r_rx_Refund.PAG_ErrorMessage & """," & _
					"""PAG_faultcode"" : """ & r_rx_Refund.PAG_faultcode & """," & _
					"""PAG_faultstring"" : """ & r_rx_Refund.PAG_faultstring & """" & _
				" }"
		end if
	
	cn.Close
	set cn = nothing
	
'	ENVIA RESPOSTA
	Response.Write strResp
	Response.End
%>
