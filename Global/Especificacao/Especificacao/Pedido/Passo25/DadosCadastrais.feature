@ignore
Feature: PedidoDadosCadastraisVerificarQueExecutou - VerificarQueExecutou
Precisamos verificar os dados cadastrais
Quando chama pelo ASP ou pela Loja, esta verificação não precisa ser feita porque vamos usar os dados do cliente já cadastrado.
Mas pela API precisa desta verificação

Scenario: PedidoDadosCadastraisVerificarQueExecutou Configuração
	Given Nome deste item "Especificacao.Pedido.Passo25.DadosCadastrais"
	Given Implementado em "Especificacao.Pedido.Pedido"
	And Fim da configuração
	Then Fazer
