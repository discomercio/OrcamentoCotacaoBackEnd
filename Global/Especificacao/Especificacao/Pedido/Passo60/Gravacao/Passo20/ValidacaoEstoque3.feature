@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: ValidacaoEstoque3.feature

Background:
	Given Reiniciar banco ao terminar cenário

@ignore
Scenario: Validar estoque 3 - feito em outros arquivos
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
	When Validação feita em outro lugar: "Todo este fluxo é validado pelos testes de alocação nos CDs (em Especificacao\Pedido\Passo60\Gravacao\SplitEstoque\*)"
	When Testado em "Especificacao\Pedido\Passo60\Gravacao\SplitEstoque\*"

@ignore
Scenario: Validar estoque 3 - produto sem alocação
	#loja/PedidoNovoConfirma.asp
	#'	HOUVE FALHA EM ALOCAR A QUANTIDADE REMANESCENTE?
	#	if qtde_a_alocar > 0 then
	#		alerta=texto_add_br(alerta)
	#		alerta=alerta & "Falha ao processar a alocação de produtos no estoque: restaram " & qtde_a_alocar & " unidades do produto (" & v_item(iItem).fabricante & ")" & v_item(iItem).produto & " que não puderam ser alocados na lista de produtos sem presença no estoque de nenhum CD"
	#		end if
	#	end if
	When Fazer esta validação

@ignore
Scenario: descontinuado
	Given Tabela "t_PRODUTO" com fabricante = "003" e produto = "003221" alterar campo "descontinuado" = "S"
	Given Pedido base
	Then Erro "regex .*consta como 'descontinuado' e não há mais saldo suficiente no estoque para atender à quantidade solicitada.*"
#loja/PedidoNovoConsiste.asp
#						alerta=alerta & "Produto (" & v_item(i).fabricante & ")" & v_item(i).produto & " consta como 'descontinuado' e não há mais saldo suficiente no estoque para atender à quantidade solicitada."
#When Fazer esta validação