@Especificacao.Pedido.PedidoFaltandoImplementarSteps
Feature: LimitePedidosRepetidosApiAngular

#cada ambiente tem um sistema diferente de limite de pedidos repetidos
Background:
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Ignorar cenário no ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido"
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	   

Scenario: LimitePedidosRepetidos iguais Api Angular
	Given Definir appsettings limite pedido igual = "2"
	And Reinciar appsettings no final da feature
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "excedido limite de pedido igual"

Scenario: LimitePedidosRepetidos por cpf Api Angular
	Given Definir appsettings limite pedido por cpf = "2"
	And Reinciar appsettings no final da feature
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "excedido limite de pedido por cpf"

