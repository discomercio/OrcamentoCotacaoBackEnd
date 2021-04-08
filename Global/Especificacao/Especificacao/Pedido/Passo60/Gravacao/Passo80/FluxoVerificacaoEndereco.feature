@ignore
Feature: FluxoVerificacaoEndereco
	#loja/PedidoNovoConfirma.asp
			#linha 2289 - 1) VERIFICA SE O ENDEREÇO USADO É O DO PARCEIRO
			#linha 2348 - 2)VERIFICA PEDIDOS DE OUTROS CLIENTES
			#linha 2488 - ENDEREÇO DE ENTREGA (SE HOUVER) 1) VERIFICA SE O ENDEREÇO USADO É O DO PARCEIRO
			#linha 2544 - ENDEREÇO DE ENTREGA (SE HOUVER) 2)VERIFICA PEDIDOS DE OUTROS CLIENTES
			#linha 2685 - Se for o caso, marca analise_endereco_tratar_status no pedido 


#			gerar um pedido base
#			mudar cpf
#			ver o fluxo esperado
#
#			a validação de endereço de entrega não será feita para o magento

Scenario: FluxoVerificacaoEndereco
#podemos fazer cada bloco em um arquivo separado
	When Fazer esta validação

