﻿@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: ValidacaoEstoque3.feature

Background:
	Given Reiniciar banco ao terminar cenário

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
	#Validação feita em outro lugar: "Todo este fluxo é validado pelos testes de alocação nos CDs (em Especificacao\Pedido\Passo60\Gravacao\SplitEstoque\*)"
	#Testado em "Especificacao\Pedido\Passo60\Gravacao\SplitEstoque\*"
	Given Validação feita em outro arquivo

Scenario: Validar estoque 3 - produto sem alocação
	#loja/PedidoNovoConfirma.asp
	#'	HOUVE FALHA EM ALOCAR A QUANTIDADE REMANESCENTE?
	#	if qtde_a_alocar > 0 then
	#		alerta=texto_add_br(alerta)
	#		alerta=alerta & "Falha ao processar a alocação de produtos no estoque: restaram " & qtde_a_alocar & " unidades do produto (" & v_item(iItem).fabricante & ")" & v_item(iItem).produto & " que não puderam ser alocados na lista de produtos sem presença no estoque de nenhum CD"
	#		end if
	#	end if

	#ignoramos no prepedido inteiro
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	#não é possível gerar este erro porque a única forma é se não tiver nenhuma regra de consumo de estoque;
	#mas, se não tiver nenhuma regra, ele vai dar o erro "Falha na leitura da regra de consumo do estoque para a UF"
	#durante a carga das regras
	#portanto, não fazemos nada

