@Especificacao/Pedido
@ignore
Feature: PedidoDadosCadastraisVerificarQueExecutou - VerificarQueExecutou
Precisamos verificar os dados cadastrais
Quando chama pelo ASP ou pela Loja, esta verificação não precisa ser feita porque vamos usar os dados do cliente já cadastrado.
Mas pela API precisa desta verificação

Scenario: VerificarQueExecutou
	Then PedidoDadosCadastraisVerificarQueExecutou
