﻿@Especificacao.Pedido.Passo10.CamposSimples
Feature: Validar campos simples Pf

Background: Configuração
	#na ApiUnis, ele exige que o cliente já esteja cadastrado, então não valida o CPF/CNPJ
	#por enquanto, ignoramos no prepedido inteiro
	Given Ignorar feature no ambiente "Especificacao.Prepedido.PrepedidoSteps"

@ListaDependencias
Scenario: CamposSimples ListaDependencias Configuração
	Given Nome deste item "Especificacao.Pedido.Passo10.CamposSimplesPfListaDependencias"
	Given Implementado em "Especificacao.Pedido.Pedido.PedidoListaDependencias"

Scenario: Validar CPF
em loja/resumo.asp:
<input name="cnpj_cpf_selecionado" id="cnpj_cpf_selecionado" type="text" maxlength="18" size="20" onblur="if (!cnpj_cpf_ok(this.value)) {alert('CNPJ/CPF inválido!!');this.focus();} else this.value=cnpj_cpf_formata(this.value);" onkeypress="if (digitou_enter(true) && tem_info(this.value) && cnpj_cpf_ok(this.value)) {this.value=cnpj_cpf_formata(this.value); fCPConcluir(fCP);} filtra_cnpj_cpf();">

	When Pedido base
	And Informo "CPF/CNPJ" = "089.617.758/99"
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	Then Erro "CPF INVÁLIDO."
	When Pedido base
	And  Informo "CPF/CNPJ" = "089.617.758/990"
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	Then Erro "CPF INVÁLIDO."
	When Pedido base
	And  Informo "CPF/CNPJ" = "089.617.758/00"
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	Then Erro "CPF INVÁLIDO."

	When Pedido base
	And  Informo "CPF/CNPJ" = "089.617.758/04"
	And  Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	Then Sem Erro "CPF INVÁLIDO."
