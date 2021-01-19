@ignore
Feature: LogMagento
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

Scenario: LogMagento
When fazer esta validação

