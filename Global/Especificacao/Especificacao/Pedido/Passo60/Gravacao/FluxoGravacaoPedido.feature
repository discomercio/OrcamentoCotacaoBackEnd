@Especificacao.Pedido.FluxoCriacaoPedido
Feature: FluxoGravacaoPedido

arquivo loja/PedidoNovoConfirma.asp

Passo01: Gerar o NSU do pedido (para bloquear transações concorrentes)

Passo10: Fazer todas as validações (documentado em FluxoCriacaoPedido.feature e nos passos dele).

Passo15: Verificar pedidos repetidos

Passo20: LER AS REGRAS DE CONSUMO DO ESTOQUE
	rotina obtemCtrlEstoqueProdutoRegra (arquivo bdd.asp)
		tipo_pessoa: especificado em Passo20/multi_cd_regra_determina_tipo_pessoa.feature
		rotina obtemCtrlEstoqueProdutoRegra validações especificado em Passo20/obtemCtrlEstoqueProdutoRegra.feature

	Traduzindo: para cada produto:
		Dado o produto, UF, tipo_cliente, contribuinte_icms_status, produtor_rural_status             
		Descobrir em t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD os CDs que atendem em ordem de prioridade
		Lê as tabelas t_PRODUTO_X_WMS_REGRA_CD, t_WMS_REGRA_CD_X_UF, t_WMS_REGRA_CD_X_UF_X_PESSOA, t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD


Passo25:  VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK - linha 1010
		Verifica se todos os produtos possuem regra ativa e não bloqueada e ao menos um CD ativo.
	'NO CASO DE SELEÇÃO MANUAL DO CD, VERIFICA SE O CD SELECIONADO ESTÁ HABILITADO EM TODAS AS REGRAS - linha 1047
		No caso de CD manual, verifica se o CD tem regra ativa


Passo30: OBTÉM DISPONIBILIDADE DO PRODUTO NO ESTOQUE - linha 1083
	Para todas as regras linha 1086
		Se o CD deve ser usado (manual ou auto)
		para todos os CDs da regra linha 1088
			Procura esse produto na lista de produtos linha 1095
			estoque_verifica_disponibilidade_integral_v2 em estoque.asp, especificado em Passo30/estoque_verifica_disponibilidade_integral_v2.feature
				'Calcula quantidade em estoque no CD especificado

	Traduzindo:
		Calcula o estoque de cada produto em cada CD que pode ser usado


Passo40: Verifica se a disponibilidade do estoque foi alterada - Linha 1159
	HÁ PRODUTO C/ ESTOQUE INSUFICIENTE (SOMANDO-SE O ESTOQUE DE TODAS AS EMPRESAS CANDIDATAS) - linha 1127

	Porque: avisamos o usuário que existem produtos sem presença no estoque e, no momento de salvar, os produtos sem presença no estoque foram alterados.
	No caso da ApiMagento, não temos essa verificação


Passo50: ANALISA A QUANTIDADE DE PEDIDOS QUE SERÃO CADASTRADOS (AUTO-SPLIT) - linha 1184
			'	OS CD'S ESTÃO ORDENADOS DE ACORDO C/ A PRIORIZAÇÃO DEFINIDA PELA REGRA DE CONSUMO DO ESTOQUE
			'	SE O PRIMEIRO CD HABILITADO NÃO PUDER ATENDER INTEGRALMENTE A QUANTIDADE SOLICITADA DO PRODUTO,
			'	A QUANTIDADE RESTANTE SERÁ CONSUMIDA DOS DEMAIS CD'S.
			'	SE HOUVER ALGUMA QUANTIDADE RESIDUAL P/ FICAR NA LISTA DE PRODUTOS SEM PRESENÇA NO ESTOQUE:
			'		1) SELEÇÃO AUTOMÁTICA DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD DEFINIDO P/ TAL
			'		2) SELEÇÃO MANUAL DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD SELECIONADO MANUALMENTE

	Para cada produto:
		Aloca a quantidade solicitada nos CDs ordenados até alocar todos.
		Se não conseguir alocar todos, marca a quantidade residual no CD manual ou no CD de t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente

Passo55: Contagem de pedidos a serem gravados - Linha 1286
	'	CONTAGEM DE EMPRESAS QUE SERÃO USADAS NO AUTO-SPLIT, OU SEJA, A QUANTIDADE DE PEDIDOS QUE SERÁ CADASTRADA, 
	JÁ QUE CADA PEDIDO SE REFERE AO ESTOQUE DE UMA EMPRESA

	Conta todos os CDs que tem alguma quantidade solicitada.


Passo60: criar pedidos
	Loop nos CDs a utilizar
		Gerar o número do pedido: Passo60/Gerar_o_numero_do_pedido.feature
		Adiciona um novo pedido
		Preenche os campos do pedido: Passo60/Preenche_os_campos_do_pedido.feature
			a maioria no pai e filhotes, alguns só no pai, alguns só nos filhotes
		Salva o registro em t_pedido

		Loop nas regras: 
			Especificado em Passo60/Itens/Gerar_t_PEDIDO_ITEM.feature
				Se essa regra cobrir um dos itens do pedido, adicionar registro em t_PEDIDO_ITEM (linha 2090 até 2122)
				Note que a quantidade rs("qtde") é a que foi alocada para esse filhote pela regra, não a quantidade total do pedido inteiro
				A sequencia do t_PEDIDO_ITEM para esse pedido (base ou filhote) começa de 1 e é sequencial.
			Se qtde_solicitada > qtde_estoque, qtde_spe (quantidade_sen_presença_estoque) fica com o número de itens faltando
			chama rotina ESTOQUE_produto_saida_v2, em Passo60/Itens/ESTOQUE_produto_saida_v2.feature
				A quantidade deste item ou efetivamente sai do estoque (atualizando t_ESTOQUE_ITEM)
				ou entra como venda sem presença no estoque (novo registro na tabela t_ESTOQUE_MOVIMENTO, operacao = OP_ESTOQUE_VENDA, estoque = ID_ESTOQUE_SEM_PRESENCA)
			Monta o log do item - Passo60/Itens/Log.feature
			
		Determina o status st_entrega deste pedido (Passo60/st_entrega.feature)

Passo70: ajustes adicionais no pedido pai
	No pedido pai atualiza campos de RA (Passo70/calcula_total_RA_liquido_BD.feture)

	Caso tenha usado algum desconto superior ao limite, liberado pela t_DESCONTO, marca como usado (Passo70/Senhas_de_autorizacao_para_desconto_superior.feature)

	INDICADOR: SE ESTE PEDIDO É COM INDICADOR E O CLIENTE AINDA NÃO TEM UM INDICADOR NO CADASTRO, ENTÃO CADASTRA ESTE. (Passo70/CadastroIndicador.feature)


Passo80: VERIFICA SE O ENDEREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE (POSSÍVEL FRAUDE)
	Passo80/FluxoVerificacaoEndereco.feature


Passo90: log (Passo90/Log.feature)




Scenario: Fluxo da gravação do pedido
	When Pedido base
	Then Sem nenhum erro


