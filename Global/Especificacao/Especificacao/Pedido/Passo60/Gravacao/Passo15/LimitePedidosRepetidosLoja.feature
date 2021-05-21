@Especificacao.Pedido.PedidoFaltandoImplementarSteps
Feature: LimitePedidosRepetidosLoja

Background:
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"

Scenario: LimitePedidosRepetidos iguais Loja
	Given Definir appsettings limite pedido igual = "2"
	And Reinciar appsettings no final da feature
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "excedido limite de pedido igual"

Scenario: LimitePedidosRepetidos por cpf Loja
	Given Definir appsettings limite pedido por cpf = "2"
	And Reinciar appsettings no final da feature
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "excedido limite de pedido por cpf"