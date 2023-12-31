﻿@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_ESTOQUE_ITEM

Background: Configuracao
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque

Scenario: Verificar qtde_disponivel - erro
#NÃO HÁ PRODUTOS SUFICIENTES NO ESTOQUE!!
#           if ((qtde_a_sair - qtde_autorizada_sem_presenca) > qtde_disponivel)
#           {
#               lstErros.Add("Produto " + id_produto + " do fabricante " + id_fabricante + ": faltam " +
#                   ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_disponivel) + " unidades no estoque (" +
#                   UtilsGlobais.Util.ObterApelidoEmpresaNfeEmitentesGravacao(id_nfe_emitente, dbGravacao) +
#                   ") para poder atender ao pedido.");
#               return false;
#           }

#este erro nunca pode acontecer no magento porque não verificamos qtde_autorizada_sem_presenca

#//nunca é feito porque a variável qtde_autorizada_sem_presenca é ajustada
#//exatamente para a quantidade de produtos faltando, então esta condição nunca acontece.
#//Essa verificação é feita em Pedido.Criacao.Passo60.Gravacao.Grava40:Executar(), mensagem quando o usuário não aceitou nenhum produto faltando


Scenario: Verificar t_ESTOQUE_ITEM - Qtde_utilizada
	#testoqueItem.Qtde_utilizada = (short)(qtde_utilizada_aux + qtde_movto);
	Given Pedido base
	Given Definir saldo de estoque = "40" para produto "um"
	When Lista de itens "0" informo "Qtde" = "10"
	When Deixar forma de pagamento consistente
	When Recalcular totais do pedido
	#When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "10", qtde_autorizada_sem_presenca = "50"
	Then Sem nenhum erro
	And Tabela "t_ESTOQUE_ITEM" registro pai e produto = "003220", verificar campo "qtde_utilizada" = "20"