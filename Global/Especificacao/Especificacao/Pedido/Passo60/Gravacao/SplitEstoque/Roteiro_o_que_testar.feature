@ignore
Feature: Roteiro_o_que_testar

Roteiro de cada teste:

1 - Escrever regra de consumo de estoque
2 - Definir estoque
3 - Cadastrar pedido
4 - Verificar slipt e quantidades dos pedidos criados
5 - Verificar posicao e movimento de estoque

Testes:


Vamos colocar as tabelas completas de homologacao, as atbelas *WMS*, para ter massa de dados.

200 - Um único produto com um mais de um CD
	Rodar este teste com todas as combinações: cliente PF e uma com cliente PJ, contrib, prod rural, em mais de um estado
	Parâmetros: tipo_pessoa, UF, CD1, CD2, nivel para desabilitar
		210 - Definição de regras
			Desabilitar todas as regras ou pela regra, ou pela UF, ou pelo tipo_pessoa, ou pelo CD)
			Regra de consumo para CD1 para tipo_pessoa para estado UF
			Regra de consumo para CD2 para tipo_pessoa para estado UF
			Regra de consumo para esperar mercadoria CD1 para tipo_pessoa para estado UF
			Regra de consumo para CD3 para todos os outros tipo_pessoa e para todos os outros UF
		
		220 - ser atendido pelo CD 1 e pelo CD 2
			estoque CD1 de 45
			estoque CD2 de 33
			Criar pedido com 20 itens
				Gerar 1 pedido totalmente atendido, deixar 25 no CD1 e 33 CD2
			Criar pedido com 50 itens
				Gerar 1 pedido totalmente atendido com 1 filhote, deixar 0 no CD1 e 28 CD2
			
			Criar pedido com 100 itens
				Gerar 1 pedido pai e 1 filhote
				Pedido pai status = SPL
				Pedido filhote status = SEP

				Pedido pai id_nfe_emitente = CD1
				Pedido filhote id_nfe_emitente = CD2

				Estoque da t_estoque_item no CD1 ficou 0
				Estoque da t_estoque_item no CD2 ficou 0

				Pedido pai t_pedido_item quantidade = 45 + 22
				Pedido filhote t_pedido_item quantidade = 33

				t_estoque_movimento 45 para o pedido pai no estoque VDO
				t_estoque_movimento 22 para o pedido pai no estoque SPE
				t_estoque_movimento 33 para o pedido filhote no estoque VDO

		230 - atender pelo CD2 e o resto sem presença
			estoque CD1 de 0
			estoque CD2 de 31
			Criar pedido com 100 itens
				Gerar 1 pedido pai e 1 filhote
				Pedido pai status = ESP
				Pedido filhote status = SEP

				Pedido pai id_nfe_emitente = CD1
				Pedido filhote id_nfe_emitente = CD2

				Estoque da t_estoque_item no CD1 ficou 0
				Estoque da t_estoque_item no CD2 ficou 0

				Pedido pai t_pedido_item quantidade = 100 - 31
				Pedido filhote t_pedido_item quantidade = 31

				t_estoque_movimento 100 - 31 para o pedido pai no estoque SPE
				t_estoque_movimento 31 para o pedido filhote no estoque VDO


		240 - ficar com todos os produtos sem presença
			estoque CD1 de 0
			estoque CD2 de 0
			Criar pedido com 26 itens
				Gerar 1 pedido pai
				Pedido pai status = ESP

				Pedido pai id_nfe_emitente = CD1

				Estoque da t_estoque_item no CD1 ficou 0 
				Estoque da t_estoque_item no CD2 ficou 0 ---obvio!!!

				Pedido pai t_pedido_item quantidade = 26

				t_estoque_movimento 0 para o pedido pai no estoque VDO --nem pode ter o registo
				t_estoque_movimento 26 para o pedido pai no estoque SPE


		250 - Novas regras
			Regra de consumo para CD1 para tipo_pessoa para estado UF
			Regra de consumo para CD2 para tipo_pessoa para estado UF
			Regra de consumo para esperar mercadoria CD2 para tipo_pessoa para estado UF
			Regra de consumo para CD2 para outro tipo_pessoa para estado UF

		260 - ser atendido pelo CD 1 e pelo CD 2, esperando no CD2
			estoque CD1 de 45
			estoque CD2 de 33
			
			Criar pedido com 100 itens
				Gerar 1 pedido pai e 1 filhote
				Pedido pai status = SEP
				Pedido filhote status = SPL

				Pedido pai id_nfe_emitente = CD1
				Pedido filhote id_nfe_emitente = CD2

				Estoque da t_estoque_item no CD1 ficou 0
				Estoque da t_estoque_item no CD2 ficou 0

				Pedido pai t_pedido_item quantidade = 45
				Pedido filhote t_pedido_item quantidade = 33 + 22

				t_estoque_movimento 45 para o pedido pai no estoque VDO
				t_estoque_movimento 22 para o pedido filhote no estoque SPE
				t_estoque_movimento 33 para o pedido filhote no estoque VDO

		230 - atender pelo CD2 e o resto sem presença
			estoque CD1 de 0
			estoque CD2 de 31
			Criar pedido com 72 itens
				Gerar 1 pedido pai
				Pedido pai status = SPL

				Pedido pai id_nfe_emitente = CD2

				Estoque da t_estoque_item no CD1 ficou 0 ---obvio!!!
				Estoque da t_estoque_item no CD2 ficou 0

				Pedido pai t_pedido_item quantidade = 31 + 41

				t_estoque_movimento 31 para o pedido pai no estoque VDO
				t_estoque_movimento 41 para o pedido pai no estoque SPE

		240 - ficar com todos os produtos sem presença
			estoque CD1 de 0
			estoque CD2 de 0
			Criar pedido com 26 itens
				Gerar 1 pedido pai
				Pedido pai status = ESP

				Pedido pai id_nfe_emitente = CD2

				Estoque da t_estoque_item no CD1 ficou 0 ---obvio!!!
				Estoque da t_estoque_item no CD2 ficou 0

				Pedido pai t_pedido_item quantidade = 26

				t_estoque_movimento 0 para o pedido pai no estoque VDO --nem pode ter o registo
				t_estoque_movimento 26 para o pedido pai no estoque SPE

		

400 - Três produtos atendidos por CDs com a mesma prioridade
	Os três produtos possuem a mesma regra de consumo de estoque
	410 - O primeiro CD tem estoque para todos
	420 - O primeiro CD tem estoque para o produto 1 e o CD 2 para o produto 2
	430 - Idem mas não atende completamente; o produto 1 é vendido sem presença
	440 - Idem mas não atende completamente; o produto 1 e 2 são vendidos sem presença
500 - Três produtos atendidos por CDs com duas regras com prioridades diferentes
600 - Testar status de ativo das regras


Testes não atuomatizados:
Fazer um roteiro manual para criar pedidos, dar entrada no estoque pelo ASP
e verficiar que a rotina que processa os produtos pendentes atende esses pedidos
e que o estoque ficou consistente.




Scenario: fazer
	Given falta fazer



