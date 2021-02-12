@Especificacao.Pedido.PedidoFaltandoImplementarSteps
Feature: LimitePedidosRepetidos

#cada ambiente tem um sistema diferente de limite de pedidos repetidos
Background:
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

Scenario: LimitePedidosRepetidos iguais Api Magento
	Given Definir appsettings limite pedido igual = "2"
	And Reinciar appsettings no final da feature
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "excedido limite de pedido igual"

Scenario: LimitePedidosRepetidos por cpf Api Magento
	Given Definir appsettings limite pedido por cpf = "2"
	And Reinciar appsettings no final da feature
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "excedido limite de pedido por cpf"

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