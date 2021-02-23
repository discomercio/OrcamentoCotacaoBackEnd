@ignore
@GerenciamentoBanco
Feature: 200-Um_produto_dois_cds

Background: Background
	Given Reiniciar banco ao terminar cenário

Scenario Outline: 200 - Um único produto com um mais de um CD
	#Vamos rodar este teste com todas as combinações: cliente PF, cliente PJ, contrib, prod rural, em mais de um estado
	#Parâmetros fixos: CD1, CD2, CD3 (PROD1 implícito)
	#Parâmetros: <tipo_pessoa>, <UF>, <local_desabilitar_regras>, <testar_magento>
	#			201 - Setup - no magento somente temos PF
	When 			Testar na API Magento: "<testar_magento>"
	#			210 - Definição de regras
	When 			Desabilitar todas as regras por "<local_desabilitar_regras>" para PROD1
	And 			Regra de consumo para CD1 para tipo_pessoa para estado UF
	And 			Regra de consumo para CD2 para tipo_pessoa para estado UF
	And 			Regra de consumo para esperar mercadoria CD1 para tipo_pessoa para estado UF
	And 			Regra de consumo para CD3 para todos os outros tipo_pessoa e para todos os outros UF
	#
	#	 		220 - ser atendido pelo CD 1 e pelo CD 2
	And 			Zerar o estoque do produto e colocar estoque de outros produtos
	And 			Estoque CD1 de 45
	And 			Estoque CD2 de 33
	And 			Criar pedido com 20 itens
	And 				Gerar 1 pedido pai sem filhotes
	And 				Pedido pai status = SEP
	And 				Pedido pai id_nfe_emitente = CD1
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD1 = (45 - 20)
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD2 = 33
	#
	And 			Zerar o estoque do produto e colocar estoque de outros produtos
	And 			Estoque CD1 de 45
	And 			Estoque CD2 de 33
	And 			Criar pedido com 50 itens
	And 				Gerar 1 pedido pai e 1 filhote
	#
	And 				Pedido pai status = SEP
	And 				Pedido pai id_nfe_emitente = CD1
	And 				Pedido pai t_pedido_item quantidade = 45
	#
	And 				Pedido filhote status = SEP
	And 				Pedido filhote id_nfe_emitente = CD2
	And 				Pedido filhote t_pedido_item quantidade = (50 - 45)
	#
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD1 = 0
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD2 = 33 - (50 - 45) = 28
	#
	And 			Zerar o estoque do produto e colocar estoque de outros produtos
	And 			Estoque CD1 de 45
	And 			Estoque CD2 de 33
	And 			Criar pedido com 100 itens
	And 				Gerar 1 pedido pai e 1 filhote
	#
	And 				Pedido pai status = SPL
	And 				Pedido pai id_nfe_emitente = CD1
	And 				Pedido pai t_pedido_item quantidade = 45 + 22
	#
	And 				Pedido filhote status = SEP
	And 				Pedido filhote id_nfe_emitente = CD2
	And 				Pedido filhote t_pedido_item quantidade = 33
	#
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD1 = 0
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD2 = 0
	#
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai" com 2 registros
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 45, estoque = VDO, operacao = VDA
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 22, estoque = SPE, operacao = VDA
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido filhote" com 1 registros
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido filhote", qtde = 33, estoque = VDO, operacao = VDA
	#
	#			230 - atender pelo CD2 e o resto sem presença
	And 			Zerar o estoque do produto e colocar estoque de outros produtos
	And 			estoque CD1 de 0
	And 			estoque CD2 de 31
	And 			Criar pedido com 100 itens
	And 				Gerar 1 pedido pai e 1 filhote
	#
	And 				Pedido pai status = ESP
	And 				Pedido pai id_nfe_emitente = CD1
	And 				Pedido pai t_pedido_item quantidade = 100 - 31
	#
	And 				Pedido filhote status = SEP
	And 				Pedido filhote id_nfe_emitente = CD2
	And 				Pedido filhote t_pedido_item quantidade = 31
	#
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD1 = 0
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD2 = 0
	#
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai" com 1 registros
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 100 - 31, estoque = SPE, operacao = VDA
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido filhote" com 1 registros
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido filhote", qtde = 31, estoque = VDO, operacao = VDA
	#
	#
	#			240 - ficar com todos os produtos sem presença
	And 			Zerar o estoque do produto e colocar estoque de outros produtos
	And 			estoque CD1 de 0
	And 			estoque CD2 de 0
	And 			Criar pedido com 26 itens
	And 				Gerar 1 pedido pai sem filhotes
	#
	And 				Pedido pai status = ESP
	And 				Pedido pai id_nfe_emitente = CD1
	#
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD1 = 0
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD2 = 0 #obvio!!!
	#
	And 				Pedido pai t_pedido_item quantidade = 26
	#
	#And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 26, estoque = VDO --naõ pode ter o registo
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai" com 1 registros
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 26, estoque = SPE, operacao = VDA
	#
	#
	#			250 - Novas regras
	And 			Desabilitar todas as regras por <local_desabilitar_regras> #para desabilitar as regras que criamos em 210
	And 			Regra de consumo para CD1 para tipo_pessoa para estado UF
	And 			Regra de consumo para CD2 para tipo_pessoa para estado UF
	And 			Regra de consumo para esperar mercadoria CD2 para tipo_pessoa para estado UF
	And 			Regra de consumo para CD3 para outro tipo_pessoa para estado UF
	#
	#			260 - ser atendido pelo CD 1 e pelo CD 2, esperando no CD2
	And 			Zerar o estoque do produto e colocar estoque de outros produtos
	And 			estoque CD1 de 45
	And 			estoque CD2 de 33
	#
	And 			Criar pedido com 100 itens
	And 				Gerar 1 pedido pai e 1 filhote
	#
	And 				Pedido pai status = SEP
	And 				Pedido pai id_nfe_emitente = CD1
	And 				Pedido pai t_pedido_item quantidade = 45
	#
	And 				Pedido filhote status = SPL
	And 				Pedido filhote id_nfe_emitente = CD2
	And 				Pedido filhote t_pedido_item quantidade = 33 + 22
	#
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD1 = 0
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD2 = 0
	#
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai" com 1 registros
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 45, estoque = VDO, operacao = VDA
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido filhote" com 2 registros
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido filhote", qtde = 33, estoque = VDO, operacao = VDA
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 22, estoque = SPE, operacao = VDA
	#
	#			270 - atender pelo CD2 e o resto sem presença
	And 			Zerar o estoque do produto e colocar estoque de outros produtos
	And 			estoque CD1 de 0
	And 			estoque CD2 de 31
	And 			Criar pedido com 72 itens
	And 				Gerar 1 pedido pai sem filhotes
	#
	And 				Pedido pai status = SPL
	And 				Pedido pai id_nfe_emitente = CD2
	And 				Pedido pai t_pedido_item quantidade = 31 + 41
	#
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD1 = 0 #obvio!
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD2 = 0
	#
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai" com 2 registros
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 31, estoque = VDO, operacao = VDA
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 41, estoque = SPE, operacao = VDA
	#
	#			280 - ficar com todos os produtos sem presença
	And 			Zerar o estoque do produto e colocar estoque de outros produtos
	And 			estoque CD1 de 0
	And 			estoque CD2 de 0
	And 			Criar pedido com 26 itens
	And 				Gerar 1 pedido pai sem filhotes
	#
	And 				Pedido pai status = ESP
	And 				Pedido pai id_nfe_emitente = CD2
	And 				Pedido pai t_pedido_item quantidade = 26
	#
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD1 = 0 #obvio!
	And 				Saldo de estoque na t_ESTOQUE_ITEM no CD2 = 0
	#
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai" com 1 registros
	And 				t_ESTOQUE_MOVIMENTO pedido = "pedido pai", qtde = 26, estoque = SPE, operacao = VDA
	#
	Examples:
		#testar_magento somente aceita PF
		| tipo_pessoa | UF | local_desabilitar_regras | testar_magento |
		| PF          | SP | xxx                      | sim            |
		| PR          | SP | xxx                      | não            |
		| PJC         | SP | xxx                      | não            |
		| PJNC        | SP | xxx                      | não            |
		| PJI         | SP | xxx                      | não            |
#descrevendo os códigos:
#
#'	CÓDIGOS DE TIPO DE PESSOA USADOS NA REGRA DE CONSUMO DO ESTOQUE (MULTI CD)
#	Const COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA = "PF"
#	Const COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL = "PR"
#	Const COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE = "PJC"
#	Const COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE = "PJNC"
#	Const COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO = "PJI"

#'   STATUS DE ENTREGA DO PEDIDO
#	Const ST_ENTREGA_ESPERAR		            = "ESP"  ' NENHUMA MERCADORIA SOLICITADA ESTÁ DISPONÍVEL
#	Const ST_ENTREGA_SPLIT_POSSIVEL             = "SPL"  ' PARTE DA MERCADORIA ESTÁ DISPONÍVEL PARA ENTREGA
#	Const ST_ENTREGA_SEPARAR		            = "SEP"	 ' TODA A MERCADORIA ESTÁ DISPONÍVEL E JÁ PODE SER SEPARADA PARA ENTREGA
#	Const ST_ENTREGA_A_ENTREGAR		            = "AET"	 ' A TRANSPORTADORA JÁ SEPAROU A MERCADORIA PARA ENTREGA
#	Const ST_ENTREGA_ENTREGUE		            = "ETG"	 ' MERCADORIA FOI ENTREGUE
#	Const ST_ENTREGA_CANCELADO		            = "CAN"	 ' VENDA FOI CANCELADA

#'	TIPOS DE ESTOQUE
#	Const ID_ESTOQUE_VENDA				= "VDA"
#	Const ID_ESTOQUE_VENDIDO			= "VDO"
#	Const ID_ESTOQUE_SEM_PRESENCA		= "SPE"
#	Const ID_ESTOQUE_KIT				= "KIT"
#	Const ID_ESTOQUE_SHOW_ROOM			= "SHR"
#	Const ID_ESTOQUE_DANIFICADOS		= "DAN"
#	Const ID_ESTOQUE_DEVOLUCAO			= "DEV"
#	Const ID_ESTOQUE_ROUBO				= "ROU"
#	Const ID_ESTOQUE_ENTREGUE			= "ETG"
#
#'	OPERAÇÕES (MOVIMENTOS) DO ESTOQUE
#	Const OP_ESTOQUE_ENTRADA			= "CAD"
#	Const OP_ESTOQUE_VENDA				= "VDA"
#	Const OP_ESTOQUE_CONVERSAO_KIT		= "KIT"
#	Const OP_ESTOQUE_TRANSFERENCIA		= "TRF"
#	Const OP_ESTOQUE_ENTREGA			= "ETG"
#	Const OP_ESTOQUE_DEVOLUCAO			= "DEV"
