@ignore
@Ambiente.ApiUnis.PrepedidoUnis.ListarStatusPrepedido.ListarStatusPrepedido
Feature: ListarStatusPrepedido

Scenario: Prepedido não existe
    Given Limpar tabela "t_PRAZO_PAGTO_VISANET"
	And Novo registro na tabela "t_PRAZO_PAGTO_VISANET"
	And Novo registro em "t_PRAZO_PAGTO_VISANET", campo "tipo" = "Constantes.COD_VISANET_PRAZO_PAGTO_LOJA"
	And Novo registro em "t_PRAZO_PAGTO_VISANET", campo "qtde_parcelas" = "123"
	And Gravar registro em "t_PRAZO_PAGTO_VISANET"
    Then Resposta "123"

Scenario: retorna o que existe 2
    Given Limpar tabela "t_PRAZO_PAGTO_VISANET"
	And Novo registro na tabela "t_PRAZO_PAGTO_VISANET"
	And Novo registro em "t_PRAZO_PAGTO_VISANET", campo "tipo" = "Constantes.COD_VISANET_PRAZO_PAGTO_LOJA"
	And Novo registro em "t_PRAZO_PAGTO_VISANET", campo "qtde_parcelas" = "321"
	And Gravar registro em "t_PRAZO_PAGTO_VISANET"
    Then Resposta "321"

Scenario: não existe 
#talvez a gente desse retornar 0... mas erro parece razoável.
    Given Limpar tabela "t_PRAZO_PAGTO_VISANET"
    #na primeira versão retornava erro...
	#Then Erro status code "401"
    Then Resposta "0"

