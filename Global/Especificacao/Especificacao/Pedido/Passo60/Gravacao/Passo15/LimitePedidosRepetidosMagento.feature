@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
@GerenciamentoBanco
Feature: LimitePedidoRepetidosMagento

Background:
	Given Reiniciar appsettings
	Given Reiniciar banco ao terminar cenário
	Given Limpar tabela "t_PEDIDO"

Scenario: LimitePedidosRepetidos iguais Api Magento - sucesso
	When Informo "limitePedidos.pedidoIgual_tempo_em_segundos" = "5600"
	When Informo "limitePedidos.pedidoIgual" = "3"
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro

Scenario: LimitePedidosRepetidos iguais Api Magento - erro
	When Informo "limitePedidos.pedidoIgual_tempo_em_segundos" = "5600"
	When Informo "limitePedidos.pedidoIgual" = "2"
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "regex .*Um pedido idêntico já foi gravado com o número"

Scenario: LimitePedidosRepetidos por cpf Api Magento - sucesso
	When Informo "limitePedidos.pedidosMesmoCpfCnpj_TempoSegundos" = "5600"
	When Informo "limitePedidos.porCpf" = "2"
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro

Scenario: LimitePedidosRepetidos por cpf Api Magento - erro
	When Informo "limitePedidos.porCpf" = "2"
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	Then Erro "regex .*Um pedido para o mesmo CPF/CNPJ foi gravado recentemente com o número"