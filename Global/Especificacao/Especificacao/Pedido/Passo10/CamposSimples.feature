@Especificacao.Pedido
@ignore
Feature: Validar campos simples

Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo10.CamposSimples"
	Given Implementado em "Especificacao.Pedido.Pedido"
	#na ApiUnis, ele exige que o cliente já esteja cadastrado, então não valida o CPF/CNPJ
	And No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido" erro "CNPJ/CPF inválido!!" é "Cliente não localizado"
	And Fim da configuração

Scenario: Validar CPF
em loja/resumo.asp:
<input name="cnpj_cpf_selecionado" id="cnpj_cpf_selecionado" type="text" maxlength="18" size="20" onblur="if (!cnpj_cpf_ok(this.value)) {alert('CNPJ/CPF inválido!!');this.focus();} else this.value=cnpj_cpf_formata(this.value);" onkeypress="if (digitou_enter(true) && tem_info(this.value) && cnpj_cpf_ok(this.value)) {this.value=cnpj_cpf_formata(this.value); fCPConcluir(fCP);} filtra_cnpj_cpf();">

	When Pedido base
	And Informo "CPF/CNPJ" = "089.617.758/99"
	Then Erro "CNPJ/CPF inválido!!"
	When Pedido base
	And  Informo "CPF/CNPJ" = "089.617.758/990"
	Then Erro "CNPJ/CPF inválido!!"
	When Pedido base
	And  Informo "CPF/CNPJ" = "089.617.758/00"
	Then Erro "CNPJ/CPF inválido!!"

	When Pedido base
	And  Informo "CPF/CNPJ" = "089.617.758/04"
	Then Sem erro "CNPJ/CPF inválido!!"

Scenario: Validar CNPJ
	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-5"
	Then Erro "CNPJ/CPF inválido!!"
	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-99"
	Then Erro "CNPJ/CPF inválido!!"
	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-xx"
	Then Erro "CNPJ/CPF inválido!!"
	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-11"
	Then Erro "CNPJ/CPF inválido!!"
	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-53"
	Then Erro "CNPJ/CPF inválido!!"

	When Pedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-51"
	Then Sem erro "CNPJ/CPF inválido!!"

