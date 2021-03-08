@Especificacao.Pedido.Passo60.Gravacao.Passo80.CompararEndereco.feature
Feature: CompararEndereco
#Testar a rotina IsEnderecoIgual

Scenario: IsEnderecoIgual base
	When IsEnderecoIgual: "end_logradouro_1" = "end_logradouro_1"
	When IsEnderecoIgual: "end_numero_1"     = "end_numero_1"
	When IsEnderecoIgual: "end_cep_1"        = "end_cep_1"
	When IsEnderecoIgual: "end_logradouro_2" = "end_logradouro_2"
	When IsEnderecoIgual: "end_numero_2"     = "end_numero_2"
	When IsEnderecoIgual: "end_cep_2"        = "end_cep_2"
	Then IsEnderecoIgual: não

Scenario: IsEnderecoIgual 1
	When IsEnderecoIgual: "end_logradouro_1" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_1"     = "101"
	When IsEnderecoIgual: "end_cep_1"        = "02408150"
	When IsEnderecoIgual: "end_logradouro_2" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_2"     = "101"
	When IsEnderecoIgual: "end_cep_2"        = "02408150"
	Then IsEnderecoIgual: sim

Scenario: IsEnderecoIgual cep
	When IsEnderecoIgual: "end_logradouro_1" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_1"     = "101"
	When IsEnderecoIgual: "end_cep_1"        = "02408150"
	When IsEnderecoIgual: "end_logradouro_2" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_2"     = "101"
	When IsEnderecoIgual: "end_cep_2"        = "02408151"
	Then IsEnderecoIgual: não

Scenario: IsEnderecoIgual 3
	When IsEnderecoIgual: "end_logradouro_1" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_1"     = "101"
	When IsEnderecoIgual: "end_cep_1"        = "02408150"
	When IsEnderecoIgual: "end_logradouro_2" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_2"     = "102"
	When IsEnderecoIgual: "end_cep_2"        = "02408150"
	Then IsEnderecoIgual: não

Scenario: IsEnderecoIgual numeros
	When IsEnderecoIgual: "end_logradouro_1" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_1"     = "101"
	When IsEnderecoIgual: "end_cep_1"        = "02408150"
	When IsEnderecoIgual: "end_logradouro_2" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_2"     = "101/102"
	When IsEnderecoIgual: "end_cep_2"        = "02408150"
	Then IsEnderecoIgual: sim

	When IsEnderecoIgual: "end_logradouro_1" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_1"     = "110/111"
	When IsEnderecoIgual: "end_cep_1"        = "02408150"
	When IsEnderecoIgual: "end_logradouro_2" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_2"     = "111/112"
	When IsEnderecoIgual: "end_cep_2"        = "02408150"
	Then IsEnderecoIgual: sim

	When IsEnderecoIgual: "end_logradouro_1" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_1"     = "110/111"
	When IsEnderecoIgual: "end_cep_1"        = "02408150"
	When IsEnderecoIgual: "end_logradouro_2" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_2"     = "112/113"
	When IsEnderecoIgual: "end_cep_2"        = "02408150"
	Then IsEnderecoIgual: não

Scenario: IsEnderecoIgual numeros com letras
	When IsEnderecoIgual: "end_logradouro_1" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_1"     = "101 cj 22"
	When IsEnderecoIgual: "end_cep_1"        = "02408150"
	When IsEnderecoIgual: "end_logradouro_2" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_2"     = "101 cj 22"
	When IsEnderecoIgual: "end_cep_2"        = "02408150"
	Then IsEnderecoIgual: sim

	When IsEnderecoIgual: "end_logradouro_1" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_1"     = "101 cj 22"
	When IsEnderecoIgual: "end_cep_1"        = "02408150"
	When IsEnderecoIgual: "end_logradouro_2" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_2"     = "101 cj 23"
	When IsEnderecoIgual: "end_cep_2"        = "02408150"
	Then IsEnderecoIgual: não

Scenario: IsEnderecoIgual rua
	When IsEnderecoIgual: "end_logradouro_1" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_1"     = "101"
	When IsEnderecoIgual: "end_cep_1"        = "02408150"
	When IsEnderecoIgual: "end_logradouro_2" = "Avenida Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_2"     = "101"
	When IsEnderecoIgual: "end_cep_2"        = "02408150"
	Then IsEnderecoIgual: sim

Scenario: IsEnderecoIgual rua 2
	When IsEnderecoIgual: "end_logradouro_1" = "Rua Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_1"     = "101"
	When IsEnderecoIgual: "end_cep_1"        = "02408150"
	When IsEnderecoIgual: "end_logradouro_2" = "Esquina da Francisco Pecoraro"
	When IsEnderecoIgual: "end_numero_2"     = "101"
	When IsEnderecoIgual: "end_cep_2"        = "02408150"
	Then IsEnderecoIgual: não

Scenario: IsEnderecoIgual caixa
	When IsEnderecoIgual: "end_logradouro_1" = "rua francisco pecoraro"
	When IsEnderecoIgual: "end_numero_1"     = "101"
	When IsEnderecoIgual: "end_cep_1"        = "02408150"
	When IsEnderecoIgual: "end_logradouro_2" = "AVENIDA FRANCISCO PECORARO"
	When IsEnderecoIgual: "end_numero_2"     = "101"
	When IsEnderecoIgual: "end_cep_2"        = "02408150"
	Then IsEnderecoIgual: sim

