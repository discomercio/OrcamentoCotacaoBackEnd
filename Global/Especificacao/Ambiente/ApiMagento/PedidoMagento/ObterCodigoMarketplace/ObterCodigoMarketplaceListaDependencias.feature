@ListaDependencias
Feature: ListaDependencias

Scenario: Lista de verificações feitas
	Given Nome deste item "Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.ObterCodigoMarketplaceListaDependencias"
#teste da autenticação
	And Especificado em "Especificacao.Comuns.Api.Autenticacao"
#o teste em si
	And Especificado em "Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.ObterCodigoMarketplace"
	And Fim da configuração
