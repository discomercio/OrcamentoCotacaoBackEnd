@Especificacao.Testes.Utils.ListaDependencias
Feature: VerificacaoFinalListaDependencias

Temos um sistema de ListaDependencias: um arquivo .feature pode ser "repetido" por outros.

Por exemplo, a autenticação da API: está esepcificada em 
Especificacao\Comuns\Api\Autenticacao\Autenticacao.feature
e a lsita de dependências está em 
Especificacao\Comuns\Api\Autenticacao\AutenticacaoListaDependencias.feature

Quer dizer, Especificacao\Comuns\Api\Autenticacao\Autenticacao.feature lista como testamos a autenticação.
Esse teste é feito em vários lugares; por exemplo, em:
	And Implementado em "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedidoListaDependencias"
	And Implementado em "Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.BuscarQtdeParcCartaoVisaListaDependencias"
	And Implementado em "Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.ObterCodigoMarketplaceListaDependencias"

Com esses registros, a gente marca exatamente onde cada teste é feito.
Desabilitamos pq não estamos usando mais, vamos removeer as listas de dependências

Scenario: VerificacaoFinalListaDependencias
	#só chamamos a rotina para fazer a verificação final
	#este teste provavelmente só passa quando todos os testes forem executados
	#desabilitamos pq não estamos usando mais, vamos removeer as listas de dependências
	#Given VerificacaoFinalListaDependencias
	Given ApagarMapaComChamadas.txt
#salvar o MapaComChamadas.txt demora tipo 40 segundos, gera um arquivo de tipo 400 mega
#Given SalvarMapaComChamadas.txt


