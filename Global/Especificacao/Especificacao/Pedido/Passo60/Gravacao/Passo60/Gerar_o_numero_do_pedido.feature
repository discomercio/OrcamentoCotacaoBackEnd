@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: Gerar_o_numero_do_pedido

Background: Background
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário

#When Lista de itens "0" informo "Qtde" = "20"
#When Lista de itens "0" informo "Fabricante" = "001"
#When Lista de itens "0" informo "Produto" = "001000"
#And Zerar todo o estoque
Scenario: Gerar_o_numero_do_pedido - magento
	#Gerar o número do pedido: caso maior que 1, colocar letras como sufixo
	#		'	Controla a quantidade de pedidos no auto-split
	#		'	pedido-base: indice_pedido=1
	#		'	pedido-filhote 'A' => indice_pedido=2
	#		'	pedido-filhote 'B' => indice_pedido=3
	#		'	etc
	#
	#PARA GERAR SPLIT
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	Given Usar produto "dois" como fabricante = "003", produto = "003221"
	When Lista de itens "0" informo "Qtde" = "100"
	When Lista de itens "1" informo "Qtde" = "100"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Given Zerar todo o estoque
	Given Definir saldo de estoque = "40" para produto "um"
	Given Definir saldo estoque = "40" para produto = "um" e id_nfe_emitente = "4003"
	Given Tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD" alterar registro id_wms_regra_cd_x_uf_x_pessoa = "666" e id_nfe_emitente = "4003", campo "st_inativo" = "0"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pedido" = "176368N-A"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pedido" = "176368N"

Scenario: Gerar_o_numero_do_pedido - Loja
	#Gerar o número do pedido: caso maior que 1, colocar letras como sufixo
	#		'	Controla a quantidade de pedidos no auto-split
	#		'	pedido-base: indice_pedido=1
	#		'	pedido-filhote 'A' => indice_pedido=2
	#		'	pedido-filhote 'B' => indice_pedido=3
	#		'	etc
	#
	#PARA GERAR SPLIT
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	Given Usar produto "dois" como fabricante = "003", produto = "003221"
	When Lista de itens "0" informo "Qtde" = "100"
	When Lista de itens "1" informo "Qtde" = "100"
	And Recalcular totais do pedido
	And Deixar forma de pagamento consistente
	Given Zerar todo o estoque
	Given Definir saldo de estoque = "40" para produto "um"
	Given Definir saldo estoque = "40" para produto = "um" e id_nfe_emitente = "4003"
	Given Tabela "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD" alterar registro id_wms_regra_cd_x_uf_x_pessoa = "667" e id_nfe_emitente = "4003", campo "st_inativo" = "0"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registros filhotes criados, verificar campo "pedido" = "176368N-A"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "pedido" = "176368N"