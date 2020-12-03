@ignore
@Especificacao.Pedido.Passo10.CamposSimples
Feature: Validar permissões

Background: Reiniciar permissão
	Given Reinciar permissões ao terminar a feature

@ListaDependencias
Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo10.Permissoes"
	Given Implementado em "Especificacao.Pedido.Pedido"


Scenario: Validar permissão de criação
	#em loja/resumo.asp:
	#if operacao_permitida(OP_LJA_CADASTRA_NOVO_PEDIDO, s_lista_operacoes_permitidas) then
	#
	#em loja/ClienteEdita.asp
	#<% if operacao_permitida(OP_LJA_CADASTRA_NOVO_PEDIDO, s_lista_operacoes_permitidas) then %>
	Given Pedido base
	And Não possuo a permissão "OP_LJA_CADASTRA_NOVO_PEDIDO"
	Then Erro "usuário não tem permissão"

Scenario: OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO
	#loja/PedidoNovoConsiste.asp
	#	if operacao_permitida(OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO, s_lista_operacoes_permitidas) then intColSpan = intColSpan + 1
	#nesse caso, instalador_instala fica vazio
	#temos que verificar que não posso dar essa iinformação se não tiver a permissão
	Given Pedido base
	And Não possuo a permissão "OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO"
	When Informo "instalador_instala" = "1"
	Then Erro "Usuário não tem permissão para informar o campo instalador_instala."


