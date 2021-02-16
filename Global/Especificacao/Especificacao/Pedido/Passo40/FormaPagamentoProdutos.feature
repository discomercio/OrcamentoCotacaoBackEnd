@Especificacao.Pedido.PedidoFaltandoImplementarSteps
Feature: FormaPagamentoProdutos

Background:
	#Implementado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\EspecificacaoAdicional\FormaPagtoCriacaoMagento\FormaPagtoCriacaoMagentofeature.feature
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

Scenario: A forma de pagamento não está disponível para o(s) produto(s)

#loja/PedidoNovoConsiste.asp
#			if (f.c_preco_lista[i].style.color.toLowerCase()==COR_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__INEXISTENTE.toLowerCase()) {
#		strMsgErro="A forma de pagamento " + KEY_ASPAS + f.c_custoFinancFornecParcelamentoDescricao.value.toLowerCase() + KEY_ASPAS + " não está disponível para o(s) produto(s):"+strMsgErro;
#A cor COR_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__INEXISTENTE
#depende da resposta de "../Global/AjaxCustoFinancFornecConsultaPrecoBD.asp"
#COR_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__INEXISTENTE é determinado pela página AjaxCustoFinancFornecConsultaPrecoBD.asp
#Condições:
#strSql = _
#	"SELECT " & _
#		"*" & _
#	" FROM t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR" & _
#	" WHERE" & _
#		" (fabricante = '" & vResp(intCounter).fabricante & "')" & _
#		" AND (tipo_parcelamento = '" & strTipoParcelamento & "')" & _
#		" AND (qtde_parcelas = " & strQtdeParcelas & ")"
#
#		e
#						"SELECT " & _
#	"*" & _
#" FROM t_PRODUTO" & _
#	" INNER JOIN t_PRODUTO_LOJA" & _
#		" ON (t_PRODUTO.fabricante=t_PRODUTO_LOJA.fabricante) AND (t_PRODUTO.produto=t_PRODUTO_LOJA.produto)" & _
#" WHERE" & _
#	" (t_PRODUTO.fabricante = '" & vResp(intCounter).fabricante & "')" & _
#	" AND (t_PRODUTO.produto = '" & vResp(intCounter).produto & "')" & _
#	" AND (CONVERT(smallint,loja) = " & strLoja & ")"
Scenario: A forma de pagamento não está disponível para o(s) produto(s) - t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR
	Given Pedido base
	#passar o pedido com esse tipo_parcelamento
	When Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "custoFinancFornecQtdeParcelas" = "2"
	Then Sem nenhum erro
	#se passou, vamos estragar para forçar o erro
	Given Reiniciar banco ao terminar cenário
	And Pedido base
	And Limpar tabela "t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR"
	Then Erro "regex .*não está disponível para o(s) produto(s).*"

Scenario: A forma de pagamento não está disponível para o(s) produto(s) - t_PRODUTO_LOJA
	Given Pedido base
	#passar o pedido com esse tipo_parcelamento
	When Informo "custoFinancFornecTipoParcelamento" = "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA"
	When Informo "custoFinancFornecQtdeParcelas" = "2"
	Then Sem nenhum erro
	#se passou, vamos estragar para forçar o erro
	Given Reiniciar banco ao terminar cenário
	And Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"
	Then Erro "regex .*não está disponível para o(s) produto(s).*"