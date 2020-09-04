@Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Validacao.CpfCnpj
Feature: CpfCnpj

Scenario: Validar CPF
	When Prepedido base
	And Informo "CPF/CNPJ" = "089.617.758/99"
	Then Erro "Cliente não localizado"
	When Prepedido base
	And  Informo "CPF/CNPJ" = "089.617.758/990"
	Then Erro "Cliente não localizado"
	When Prepedido base
	And  Informo "CPF/CNPJ" = "089.617.758/00"
	Then Erro "Cliente não localizado"

	#somente digitos
	When Prepedido base
	And  Informo "CPF/CNPJ" = "352.704.458-24"
	Then Erro "Cliente não localizado"

	When Prepedido base
	And  Informo "CPF/CNPJ" = "35270445824"
	Then Sem erro "Cliente não localizado"

Scenario: Validar CNPJ
	When Prepedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-5"
	Then Erro "Cliente não localizado"
	When Prepedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-99"
	Then Erro "Cliente não localizado"
	When Prepedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-xx"
	Then Erro "Cliente não localizado"
	When Prepedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-11"
	Then Erro "Cliente não localizado"
	When Prepedido base
	And  Informo "CPF/CNPJ" = "12.584.718/0001-53"
	Then Erro "Cliente não localizado"

	#somente digitos
	When Prepedido base
	And  Informo "CPF/CNPJ" = "76.297.703/0001-95"
	Then Erro "Cliente não localizado"

	When Prepedido base
	And  Informo "CPF/CNPJ" = "76297703000195"
	Then Sem erro "Cliente não localizado"

