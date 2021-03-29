@ignore
Feature: 000-Roteiro_o_que_testar

Roteiro de cada teste:

1 - Escrever regra de consumo de estoque
2 - Definir estoque
3 - Cadastrar pedido
4 - Verificar slipt e quantidades dos pedidos criados
5 - Verificar posicao e movimento de estoque

Vamos colocar as tabelas completas de homologacao, as tabelas *WMS*, para ter massa de dados.

Testes:


100 - Um único produto com um único CD
	Pedido sem slipt
	Pedido com slipt totalmente atendido
	Pedido com slipt faltando produto

200 - Um único produto com um mais de um CD
	Resumo dos tests:

		210 - Definição de regras
			......
			Regra de consumo para esperar mercadoria CD1 para tipo_pessoa para estado UF
			......
		
		220 - ser atendido pelo CD 1 e pelo CD 2 = gera 01 filhote
		230 - atender pelo CD2 e o resto sem presença = gera 01 filhote
		240 - ficar com todos os produtos sem presença = somente pedido pai

		250 - Novas regras
			......
			Regra de consumo para esperar mercadoria CD2 para tipo_pessoa para estado UF
			......

		260 - ser atendido pelo CD 1 e pelo CD 2, esperando no CD2 = gera 02 filhote
		270 - atender pelo CD2 e o resto sem presença
		280 - ficar com todos os produtos sem presença


400 - Três produtos atendidos por CDs com a mesma prioridade
	Os três produtos possuem a mesma regra de consumo de estoque
	410 - O primeiro CD tem estoque para todos
	420 - O primeiro CD tem estoque para o produto 1 e o CD 2 para o produto 2
	430 - Idem mas não atende completamente; o produto 1 é vendido sem presença
	440 - Idem mas não atende completamente; o produto 1 e 2 são vendidos sem presença
500 - Três produtos atendidos por CDs com duas regras com prioridades diferentes
600 - Testar status de ativo das regras
	Já é feito na etapa 200 (desativamos as regras nas diferentes tabelas)


Testes não atuomatizados:
Fazer um roteiro manual para criar pedidos, dar entrada no estoque pelo ASP
e verficiar que a rotina que processa os produtos pendentes atende esses pedidos
e que o estoque ficou consistente.



Scenario: fazer
	Given falta fazer



