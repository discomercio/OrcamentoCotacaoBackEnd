@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: LogMagento

Background: Configuracao
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário

#loja/PedidoNovoConfirma.asp
#montagem do log
#if loja = NUMERO_LOJA_ECOMMERCE_AR_CLUBE then
#	if Trim("" & rs("pedido_bs_x_marketplace"))<>"" then s_log = s_log & "; numero_pedido_marketplace=" & s_numero_mktplace
#	s_log = s_log & "; cod_origem_pedido=" & s_origem_pedido
#	end if
#
#if operacao_origem = OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO then
#	if s_log <> "" then s_log = s_log & ";"
#	s_log = s_log & " Operação de origem: cadastramento semi-automático de pedido do e-commerce (nº Magento=" & c_numero_magento & ", t_MAGENTO_API_PEDIDO_XML.id=" & id_magento_api_pedido_xml & ")"
#	end if
#end if ' if Not rs.Eof
Scenario: LogMagento - OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_LOG" pedido gerado e operacao = "OP_LOG_PEDIDO_NOVO", verificar campo "complemento" = "operação de origem: cadastramento semi-automático de pedido do e-commerce (nº magento pedido_bs_x_ac="

Scenario: LogMagento - NUMERO_LOJA_ECOMMERCE_AR_CLUBE
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	When Informo "appsettings.Loja" = "201"
	Then Sem nenhum erro
	And Tabela "t_LOG" pedido gerado e operacao = "OP_LOG_PEDIDO_NOVO", verificar campo "complemento" = "cod_origem_pedido=001;"