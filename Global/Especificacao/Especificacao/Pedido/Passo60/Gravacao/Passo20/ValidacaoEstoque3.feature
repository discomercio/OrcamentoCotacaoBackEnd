@ignore
Feature: ValidacaoEstoque3.feature

Scenario: Validar estoque 3
#loja/PedidoNovoConsiste.asp
#loja/PedidoNovoConfirma.asp
#exatamente o mesmo código nas duas, exceto:
#	loja/PedidoNovoConfirma.asp tem um bloco a mais para avisar de estoque que está desabilitado

	#Desde:
	#'	HÁ PRODUTO C/ ESTOQUE INSUFICIENTE (SOMANDO-SE O ESTOQUE DE TODAS AS EMPRESAS CANDIDATAS)
	#	erro_produto_indisponivel = False
	#	if alerta="" then
	#.....até.....
	#'	CONTAGEM DE EMPRESAS QUE SERÃO USADAS NO AUTO-SPLIT, OU SEJA, A QUANTIDADE DE PEDIDOS QUE SERÁ CADASTRADA, JÁ QUE CADA PEDIDO SE REFERE AO ESTOQUE DE UMA EMPRESA
	#	dim qtde_empresa_selecionada, lista_empresa_selecionada
	#(inclusive esse bloco)
	When FAzer esta validação, ver se precisa mesmo e justificar
	When Feito no FluxoGravacaoPedido.feature


Scenario: descontinuado
	#loja/PedidoNovoConsiste.asp
	#						alerta=alerta & "Produto (" & v_item(i).fabricante & ")" & v_item(i).produto & " consta como 'descontinuado' e não há mais saldo suficiente no estoque para atender à quantidade solicitada."
	When Fazer esta validação

