@Especificacao.Pedido.PedidoFaltandoImplementarSteps
Feature: LimitePedidosRepetidosApiUnis
	
Background: 
Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
Given Ignorar cenário no ambiente "Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.CadastrarPrepedidoPrepedidoApi"
Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"

Scenario: LimitePedidosRepetidos iguais Api Unis
	Given Definir appsettings limite pedido igual = "2"
	And Reinciar appsettings no final da feature
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "excedido limite de pedido igual"

Scenario: LimitePedidosRepetidos por cpf Api Unis
	Given Definir appsettings limite pedido por cpf = "2"
	And Reinciar appsettings no final da feature
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "excedido limite de pedido por cpf"