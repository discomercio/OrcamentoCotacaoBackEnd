﻿Feature: Validar permissões

Scenario: Validar permissão de criação
em loja/resumo.asp:
if operacao_permitida(OP_LJA_CADASTRA_NOVO_PEDIDO, s_lista_operacoes_permitidas) then

em loja/ClienteEdita.asp
<% if operacao_permitida(OP_LJA_CADASTRA_NOVO_PEDIDO, s_lista_operacoes_permitidas) then %>
	Given Não possuo a permissão "OP_LJA_CADASTRA_NOVO_PEDIDO"
	When Crio um pedido
	Then Erro "verificar mensagem de erro