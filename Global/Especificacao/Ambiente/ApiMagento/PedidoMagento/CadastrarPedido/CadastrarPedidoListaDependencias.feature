@ListaDependencias
Feature: ListaDependencias

Scenario: Lista de verificações feitas
	Given Nome deste item "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias"
#teste da autenticação
	And Especificado em "Especificacao.Comuns.Api.Autenticacao"
#EspecificacaoAdicional
	And Especificado em "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional.CamposLidosAppsettings"

