@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: Especificacao\Pedido\Passo60\Gravacao\Passo60\Itens\Log

Background: Configuracao

#Given Reiniciar banco
@ignore
Scenario: Log dos itens - acho que esse é de split
	#loja/PedidoNovoConfirma.asp
	#'	LOG
	#	if s_log_item_autosplit <> "" then s_log_item_autosplit = s_log_item_autosplit & chr(13)
	#	s_log_item_autosplit = s_log_item_autosplit & "(" & .fabricante & ")" & .produto & ":" & _
	#				" Qtde Solicitada = " & vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada & "," & _
	#				" Qtde Sem Presença Autorizada = " & Cstr(qtde_spe) & "," & _
	#				" Qtde Estoque Vendido = " & Cstr(qtde_estoque_vendido_aux) & "," & _
	#				" Qtde Sem Presença = " & Cstr(qtde_estoque_sem_presenca_aux)
	When Fazer esta validação

Scenario: Log dos itens - pedido pai
	#MONTA LOG DOS ITENS
	#for i=Lbound(v_item) to Ubound(v_item)
	#	with v_item(i)
	#		if s_log <> "" then s_log=s_log & ";" & chr(13)
	#		s_log = s_log & _
	#				log_produto_monta(.qtde, .fabricante, .produto) & _
	#				"; preco_lista=" & formata_texto_log(.preco_lista) & _
	#				"; desc_dado=" & formata_texto_log(.desc_dado) & _
	#				"; preco_venda=" & formata_texto_log(.preco_venda) & _
	#				"; preco_NF=" & formata_texto_log(.preco_NF) & _
	#				"; custoFinancFornecCoeficiente=" & formata_texto_log(.custoFinancFornecCoeficiente) & _
	#				"; custoFinancFornecPrecoListaBase=" & formata_texto_log(.custoFinancFornecPrecoListaBase)
	#		if .qtde_estoque_vendido<>0 then s_log = s_log & "; estoque_vendido=" & formata_texto_log(.qtde_estoque_vendido)
	#		if .qtde_estoque_sem_presenca<>0 then s_log = s_log & "; estoque_sem_presenca=" & formata_texto_log(.qtde_estoque_sem_presenca)
	#
	#		if converte_numero(.abaixo_min_status) <> 0 then
	#			s_log = s_log & _
	#					"; abaixo_min_status=" & formata_texto_log(.abaixo_min_status) & _
	#					"; abaixo_min_autorizacao=" & formata_texto_log(.abaixo_min_autorizacao) & _
	#					"; abaixo_min_autorizador=" & formata_texto_log(.abaixo_min_autorizador) & _
	#					"; abaixo_min_superv_autorizador=" & formata_texto_log(.abaixo_min_superv_autorizador)
	#			end if
	#		end with
	#	next
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Pedido base
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_LOG" pedido gerado e operacao = "OP_LOG_PEDIDO_NOVO", verificar campo "complemento" = "2x003220(003); preco_lista=626,58; desc_dado=0; preco_venda=626,58; preco_NF=626,58; custoFinancFornecCoeficiente=1; custoFinancFornecPrecoListaBase=626,58; estoque_vendido=2;\r 2x003221(003); preco_lista=939,87; desc_dado=0; preco_venda=939,87; preco_NF=939,87; custoFinancFornecCoeficiente=1; custoFinancFornecPrecoListaBase=939,87; estoque_vendido=2"

Scenario: Log dos itens - sem presenca de estoque
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque
	Given Definir saldo de estoque = "0" para produto "um"
	Given Definir saldo de estoque = "0" para produto "dois"
	Given Pedido base
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_LOG" pedido gerado e operacao = "OP_LOG_PEDIDO_NOVO", verificar campo "complemento" = "2x003220(003); preco_lista=626,58; desc_dado=0; preco_venda=626,58; preco_NF=626,58; custoFinancFornecCoeficiente=1; custoFinancFornecPrecoListaBase=626,58; estoque_sem_presenca=2;\r 2x003221(003); preco_lista=939,87; desc_dado=0; preco_venda=939,87; preco_NF=939,87; custoFinancFornecCoeficiente=1; custoFinancFornecPrecoListaBase=939,87; estoque_sem_presenca=2"

Scenario: Log dos itens - estoque vendido e sem presenca de estoque Magento
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	#ignoramos na loja, pois se rodar nos 2 ambientes não iremos obter o mesmo resultado
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque
	Given Definir saldo de estoque = "1" para produto "um"
	Given Definir saldo de estoque = "1" para produto "dois"
	Given Pedido base
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_LOG" pedido gerado e operacao = "OP_LOG_PEDIDO_NOVO", verificar campo "complemento" = "2x003220(003); preco_lista=626,58; desc_dado=0; preco_venda=626,58; preco_NF=626,58; custoFinancFornecCoeficiente=1; custoFinancFornecPrecoListaBase=626,58; estoque_vendido=1; estoque_sem_presenca=1;\r 2x003221(003); preco_lista=939,87; desc_dado=0; preco_venda=939,87; preco_NF=939,87; custoFinancFornecCoeficiente=1; custoFinancFornecPrecoListaBase=939,87; estoque_vendido=1; estoque_sem_presenca=1"

Scenario: Log dos itens - estoque vendido e sem presenca de estoque Loja
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	#ignoramos no magento, pois se rodar nos 2 ambientes não iremos obter o mesmo resultado
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque
	Given Definir saldo de estoque = "1" para produto "um"
	Given Definir saldo de estoque = "1" para produto "dois"
	Given Pedido base
	When Recalcular totais do pedido
	When Deixar forma de pagamento consistente
	Then Sem nenhum erro
	And Tabela "t_LOG" pedido gerado e operacao = "OP_LOG_PEDIDO_NOVO", verificar campo "complemento" = "2x003220(003); preco_lista=626,58; desc_dado=0; preco_venda=626,58; preco_NF=626,58; custoFinancFornecCoeficiente=1; custoFinancFornecPrecoListaBase=626,58; estoque_vendido=1; estoque_sem_presenca=1;\r 2x003221(003); preco_lista=939,87; desc_dado=0; preco_venda=939,87; preco_NF=939,87; custoFinancFornecCoeficiente=1; custoFinancFornecPrecoListaBase=939,87; estoque_vendido=1; estoque_sem_presenca=1"