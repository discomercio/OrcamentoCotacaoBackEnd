@ignore
Feature: Valores
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Limita o RA a um percentual do valor do pedido
	#loja/PedidoNovoConsiste.asp
	#//	Limita o RA a um percentual do valor do pedido
	#	if (converte_numero(f.c_PercVlPedidoLimiteRA.value)!=0) {
	#		vlAux = (converte_numero(f.c_PercVlPedidoLimiteRA.value)/100) * converte_numero(f.c_total_geral.value);
	#		if (($("#c_loja").val()!=NUMERO_LOJA_ECOMMERCE_AR_CLUBE)&&(!FLAG_MAGENTO_PEDIDO_COM_INDICADOR)){
	#			if (converte_numero(f.c_total_RA.value) > vlAux) {
	#				alert('O valor total de RA excede o limite permitido para este pedido!!');
	#				return;
	#			}
	#		}
	#	}
	Given Pedido base
	When Fazer esta validação
