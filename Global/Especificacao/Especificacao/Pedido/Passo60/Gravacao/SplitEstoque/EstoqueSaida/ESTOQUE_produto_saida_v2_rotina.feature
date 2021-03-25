@Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.ESTOQUE_Produto_Saida_V2_RotinaSteps
@GerenciamentoBanco
Feature: ESTOQUE_produto_saida_v2_rotina

#vamos testar a rotina ESTOQUE_produto_saida_v2
Background: Configuracao
	Given Reiniciar banco ao terminar cenário
	Given Usar produto "um" como fabricante = "003", produto = "003220"
	And Usar produto "dois" como fabricante = "003", produto = "003221"
	And Zerar todo o estoque

#
#' --------------------------------------------------------------------
#'   ESTOQUE_PRODUTO_SAIDA_V2
#'   Retorno da função:
#'      False - Ocorreu falha ao tentar movimentar o estoque.
#'      True - Conseguiu fazer a movimentação do estoque.
#'   IMPORTANTE: sempre chame esta rotina dentro de uma transação para
#'      garantir a consistência dos registros entre as várias tabelas.
#'   Esta função processa a saída dos produtos do "estoque de venda"
#'   para o "estoque vendido".  No caso de não haver produtos sufi-
#'   cientes no "estoque de venda" e desde que esteja autorizado
#'   através do parâmetro "qtde_autorizada_sem_presenca", os produtos
#'   que faltam são colocados automaticamente na lista de produtos
#'   vendidos sem presença no estoque.
#function estoque_produto_saida_v2(byval id_usuario, byval id_pedido, _
#								byval id_nfe_emitente, byval id_fabricante, byval id_produto, _
#								byval qtde_a_sair, byval qtde_autorizada_sem_presenca, _
#								byref qtde_estoque_vendido, byref qtde_estoque_sem_presenca, _
#								byref msg_erro)
Scenario: ESTOQUE_produto_saida_v2_rotina normal
	Given Definir saldo de estoque = "40" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "10", qtde_autorizada_sem_presenca = "0"
	Then Retorno sucesso, qtde_estoque_vendido = "10", qtde_estoque_sem_presenca = "0"
	And Saldo de estoque = "30" para produto "um"
	And Movimento de estoque = "10" para produto "um"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "0" para produto "um"

Scenario: ESTOQUE_produto_saida_v2_rotina para estoque sem presença
	Given Definir saldo de estoque = "40" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "100", qtde_autorizada_sem_presenca = "60"
	Then Retorno sucesso, qtde_estoque_vendido = "40", qtde_estoque_sem_presenca = "60"
	And Saldo de estoque = "0" para produto "um"
	And Movimento de estoque = "40" para produto "um"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "60" para produto "um"

Scenario: Produtos spe
	Given Definir saldo de estoque = "40" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "50", qtde_autorizada_sem_presenca = "10"
	Then Retorno sucesso, qtde_estoque_vendido = "40", qtde_estoque_sem_presenca = "10"
	And Saldo de estoque = "0" para produto "um"
	And Movimento de estoque = "40" para produto "um"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "10" para produto "um"

Scenario: muitos produtos sem presenca
	#neste cenário, aceitamos o pedido - temos MAIS estoque do que o cliente aceitou
	Given Definir saldo de estoque = "40" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "50", qtde_autorizada_sem_presenca = "20"
	Then Retorno sucesso, qtde_estoque_vendido = "40", qtde_estoque_sem_presenca = "10"
	And Saldo de estoque = "0" para produto "um"
	And Movimento de estoque = "40" para produto "um"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "10" para produto "um"

Scenario: Sem produtos
	#o usuário não concordou em comprar sem presença no estoque
	Given Definir saldo de estoque = "40" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "50", qtde_autorizada_sem_presenca = "0"
	Then msg_erro "Produto 003220 do fabricante 003: faltam 10 unidades"

Scenario: Sem produtos - 2
	Given Definir saldo de estoque = "40" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "50", qtde_autorizada_sem_presenca = "5"
	Then msg_erro "Produto 003220 do fabricante 003: faltam 5 unidades"

#chamadas consecutivas para acumular
Scenario: chamadas acumuladas - com erro
	Given Definir saldo de estoque = "40" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "20", qtde_autorizada_sem_presenca = "0"
	Then Retorno sucesso, qtde_estoque_vendido = "20", qtde_estoque_sem_presenca = "0"
	And Saldo de estoque = "20" para produto "um"
	And Movimento de estoque = "20" para produto "um"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "0" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "15", qtde_autorizada_sem_presenca = "0"
	Then Retorno sucesso, qtde_estoque_vendido = "15", qtde_estoque_sem_presenca = "0"
	And Saldo de estoque = "5" para produto "um"
	And Movimento de estoque = "35" para produto "um"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "0" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "10", qtde_autorizada_sem_presenca = "0"
	#o erro tem que ser com 5 unidades
	#quando dá erro o banco fica em um estado inconsistente e a transação deve ter rollback. Nos testes não temos transações!
	Then msg_erro "Produto 003220 do fabricante 003: faltam 5 unidades"

#chamadas consecutivas para acumular
Scenario: chamadas acumuladas
	Given Definir saldo de estoque = "40" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "20", qtde_autorizada_sem_presenca = "0"
	Then Retorno sucesso, qtde_estoque_vendido = "20", qtde_estoque_sem_presenca = "0"
	And Saldo de estoque = "20" para produto "um"
	And Movimento de estoque = "20" para produto "um"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "0" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "15", qtde_autorizada_sem_presenca = "0"
	Then Retorno sucesso, qtde_estoque_vendido = "15", qtde_estoque_sem_presenca = "0"
	And Saldo de estoque = "5" para produto "um"
	And Movimento de estoque = "35" para produto "um"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "0" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "11", qtde_autorizada_sem_presenca = "8"
	Then Retorno sucesso, qtde_estoque_vendido = "5", qtde_estoque_sem_presenca = "6"
	And Saldo de estoque = "0" para produto "um"
	And Movimento de estoque = "40" para produto "um"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "6" para produto "um"
	#aqui já está sem estoque e esperando 6
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "12", qtde_autorizada_sem_presenca = "12"
	Then Retorno sucesso, qtde_estoque_vendido = "0", qtde_estoque_sem_presenca = "12"
	And Saldo de estoque = "0" para produto "um"
	And Movimento de estoque = "40" para produto "um"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "18" para produto "um"

Scenario: Respeita ordem FIFO
	Given Definir2 saldo de estoque = "12" para produto "um" com valor "100"
	Given Definir2 saldo de estoque = "14" para produto "um" com valor "222"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "15", qtde_autorizada_sem_presenca = "0"
	Then Saldo2 de estoque = "0" para produto "um" com valor "100"
	Then Saldo2 de estoque = "11" para produto "um" com valor "222"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "0" para produto "um"

#testar com mais de um produto
Scenario: FIFO com mais de um produto
	Given Definir2 saldo de estoque = "10" para produto "um" com valor "123"
	Given Definir2 saldo de estoque = "14" para produto "um" com valor "222"
	Given Definir2 saldo de estoque = "21" para produto "dois" com valor "333"
	Given Definir2 saldo de estoque = "24" para produto "dois" com valor "444"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "dois", qtde_a_sair = "44", qtde_autorizada_sem_presenca = "0"
	Then Saldo2 de estoque = "0" para produto "dois" com valor "333"
	Then Saldo2 de estoque = "1" para produto "dois" com valor "444"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "0" para produto "dois"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "15", qtde_autorizada_sem_presenca = "0"
	Then Saldo2 de estoque = "0" para produto "um" com valor "123"
	Then Saldo2 de estoque = "9" para produto "um" com valor "222"
	And Movimento ID_ESTOQUE_SEM_PRESENCA = "0" para produto "um"

@ignore
Scenario: Não conseguiu movimentar qtde suficiente
esta duplicado pois estou tendo entrar no erro
Produto " + id_produto + " do fabricante " + id_fabricante + 
": faltam " + ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_movimentada) + 
" unidades no estoque para poder atender ao pedido.
Não consigo entrar na mensagem de erro que quero
	Given Definir saldo de estoque = "50" para produto "um"
	When Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = "um", qtde_a_sair = "51", qtde_autorizada_sem_presenca = "1"
	Then msg_erro "Ajustar"