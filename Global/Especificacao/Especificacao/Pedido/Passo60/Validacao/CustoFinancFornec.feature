@ignore
Feature: CustoFinancFornec

Scenario: A forma de pagamento não foi informada (à vista, com entrada, sem entrada)."
#loja/PedidoNovoConfirma.asp
		#if (c_custoFinancFornecTipoParcelamento <> COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA) And _
		#   (c_custoFinancFornecTipoParcelamento <> COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) And _
		#   (c_custoFinancFornecTipoParcelamento <> COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) then
		#	alerta = "A forma de pagamento não foi informada (à vista, com entrada, sem entrada)."
	When Fazer esta validação

Scenario: Não foi informada a quantidade de parcelas para a forma de pagamento selecionada 
#loja/PedidoNovoConfirma.asp
		#if (c_custoFinancFornecTipoParcelamento = COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) Or _
		#   (c_custoFinancFornecTipoParcelamento = COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) then
		#	if converte_numero(c_custoFinancFornecQtdeParcelas) <= 0 then
		#		alerta = "Não foi informada a quantidade de parcelas para a forma de pagamento selecionada (" & descricaoCustoFinancFornecTipoParcelamento(c_custoFinancFornecTipoParcelamento) &  ")"
	When Fazer esta validação

