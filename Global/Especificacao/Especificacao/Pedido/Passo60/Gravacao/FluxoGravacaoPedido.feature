@ignore
Feature: FluxoGravacaoPedido

Fazer todas as validações (não documentado aqui)

10: 
LER AS REGRAS DE CONSUMO DO ESTOQUE
	Para cada produto no pedido:
		RECUPERA AS REGRAS DE CONSUMO DO ESTOQUE ASSOCIADAS AOS PRODUTOS
		VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK
		verifica se algum CD das regras está ativo (variável qtde_CD_ativo no ASP)
		

20:
'NO CASO DE SELEÇÃO MANUAL DO CD, VERIFICA SE O CD SELECIONADO ESTÁ HABILITADO EM TODAS AS REGRAS


INDICADOR: SE ESTE PEDIDO É COM INDICADOR E O CLIENTE AINDA NÃO TEM UM INDICADOR NO CADASTRO, ENTÃO CADASTRA ESTE.

Loop nos CDs a utilizar (vEmpresaAutoSplit)
	Gerar o número do pedido: caso maior que 1, colocar letras como sufixo
			'	Controla a quantidade de pedidos no auto-split
			'	pedido-base: indice_pedido=1
			'	pedido-filhote 'A' => indice_pedido=2
			'	pedido-filhote 'B' => indice_pedido=3
			'	etc
	Adiciona um novo pedido
	Campos que existem em todos os pedidos: pedido, loja, data, hora
	Campos que existem somente no pedido base, não nos filhotes:
		st_auto_split se tiver filhotes
		Campos transferidos: de linha 1800 rs("dt_st_pagto") = Date até 1887 rs("perc_limite_RA_sem_desagio") = perc_limite_RA_sem_desagio
	Campos que existem somente nos pedidos filhotes, não no base:
		linha 1892 rs("st_auto_split") = 1 até 1903 rs("forma_pagto")=""
	Transfere mais campos: linha 1907 até 2055
	Salva o registro em t_pedido

	Loop nas regras (vProdRegra)
		Se essa regra cobrir um dos itens do pedido, adicionar registro em t_PEDIDO_ITEM (linha 2090 até 2122)
		Note que a quantidade rs("qtde") é a que foi alocada para esse filhote pela regra, não a quantidade total do pedido inteiro
		A sequencia do t_PEDIDO_ITEM para esse pedido (base ou filhote) começa de 1 e é sequencial.
		SE qtde_solicitada > qtde_estoque, qtde_spe quantiade_estoque_sem_presença fica com o número de itens faltando
		chama rotina ESTOQUE_produto_saida_v2
		Atualzia alguma coisa do estoque desse item?????? linha 2145
		Marca se tem algum produto deste pedido sem presença no estoque (para mudar o status deste pedido)
		Monta o log
			
	Determina o st_entrega deste pedido (pai ou filhote)

	Para o pedido pai: chama a rotina calcula_total_RA_liquido_BD linha 2225 e atualiza vl_total_RA_liquido qtde_parcelas_desagio_RA st_tem_desagio_RA

	Para o pedido pai: linha 2251 
	SENHAS DE AUTORIZAÇÃO PARA DESCONTO SUPERIOR
	Caso tenha usado algum desconto superior ao limite, liberado pela t_DESCONTO, marca como usado

	VERIFICA SE O ENDEREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE (POSSÍVEL FRAUDE)
		linha 2289 - 1) VERIFICA SE O ENDEREÇO USADO É O DO PARCEIRO
		linha 2348 - 2)VERIFICA PEDIDOS DE OUTROS CLIENTES
		linha 2488 - ENDEREÇO DE ENTREGA (SE HOUVER) 1) VERIFICA SE O ENDEREÇO USADO É O DO PARCEIRO
		linha 2544 - ENDEREÇO DE ENTREGA (SE HOUVER) 2)VERIFICA PEDIDOS DE OUTROS CLIENTES
		linha 2685 - Se for o caso, marca analise_endereco_tratar_status no pedido 

Monta o log do pedido (variável s_log)
Monta o log dos itens do pedido 
Monta o log do auto-slipt (incluindo log dos filhotes)





Scenario: Add two numbers
	Given the first number is 50
	And the second number is 70
	When the two numbers are added
	Then the result should be 120