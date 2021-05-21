@Especificacao.Pedido.Passo10.CamposSimples
@GerenciamentoBanco
Feature: Validar permissões

Background: Reiniciar permissão
	Given Reiniciar banco ao terminar cenário
	#ignoramos no prepedido pq as permissoes são diferentes
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"

@ListaDependencias
Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo10.PermissoesListaDependencias"
	Given Implementado em "Especificacao.Pedido.Pedido.PedidoListaDependencias"

Scenario: Validar permissão de criação
	#em loja/resumo.asp:
	#if operacao_permitida(OP_LJA_CADASTRA_NOVO_PEDIDO, s_lista_operacoes_permitidas) then
	#
	#em loja/ClienteEdita.asp
	#<% if operacao_permitida(OP_LJA_CADASTRA_NOVO_PEDIDO, s_lista_operacoes_permitidas) then %>
	Given Pedido base
	#And Não possuo a permissão "OP_LJA_CADASTRA_NOVO_PEDIDO"
	And Tabela "t_OPERACAO" apagar registro com campo "id" = "OP_LJA_CADASTRA_NOVO_PEDIDO"
	Then Erro "Usuário não tem permissão para criar pedido (OP_LJA_CADASTRA_NOVO_PEDIDO)"

Scenario: OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO
#loja/PedidoNovoConsiste.asp
#	if operacao_permitida(OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO, s_lista_operacoes_permitidas) then intColSpan = intColSpan + 1
#nesse caso, instalador_instala fica vazio
#temos que verificar que não posso dar essa iinformação se não tiver a permissão

	#ignoramos na API magneto porque não podemos informar esse campo, ele é fixo
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

	Given Pedido base
	#And Não possuo a permissão "OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO"
	And Tabela "t_OPERACAO" apagar registro com campo "id" = "OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO"
	When Informo "InstaladorInstala" = "1"
	Then Erro "Usuário não tem permissão para informar o campo InstaladorInstala (OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO)"


