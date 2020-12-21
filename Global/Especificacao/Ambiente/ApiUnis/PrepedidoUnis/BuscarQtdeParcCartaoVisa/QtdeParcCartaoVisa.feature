@Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.QtdeParcCartaoVisa
@GerenciamentoBanco
Feature: QtdeParcCartaoVisa


Scenario: retorna o que existe
	Given Reiniciar banco ao terminar cenário
            #var qtdeTask = from c in db.TprazoPagtoVisanets
            #               where c.Tipo == Constantes.COD_VISANET_PRAZO_PAGTO_LOJA
            #               select c.Qtde_parcelas;
            #int qtde = Convert.ToInt32(await qtdeTask.FirstOrDefaultAsync());
    Given Limpar tabela "t_PRAZO_PAGTO_VISANET"
	And Novo registro na tabela "t_PRAZO_PAGTO_VISANET"
	And Novo registro em "t_PRAZO_PAGTO_VISANET", campo "tipo" = "Constantes.COD_VISANET_PRAZO_PAGTO_LOJA"
	And Novo registro em "t_PRAZO_PAGTO_VISANET", campo "qtde_parcelas" = "123"
	And Gravar registro em "t_PRAZO_PAGTO_VISANET"
    Then Resposta "123"

Scenario: retorna o que existe 2
	Given Reiniciar banco ao terminar cenário
    Given Limpar tabela "t_PRAZO_PAGTO_VISANET"
	And Novo registro na tabela "t_PRAZO_PAGTO_VISANET"
	And Novo registro em "t_PRAZO_PAGTO_VISANET", campo "tipo" = "Constantes.COD_VISANET_PRAZO_PAGTO_LOJA"
	And Novo registro em "t_PRAZO_PAGTO_VISANET", campo "qtde_parcelas" = "321"
	And Gravar registro em "t_PRAZO_PAGTO_VISANET"
    Then Resposta "321"

Scenario: não existe 
#talvez a gente desse retornar 0... mas erro parece razoável.
	Given Reiniciar banco ao terminar cenário
    Given Limpar tabela "t_PRAZO_PAGTO_VISANET"
    #na primeira versão retornava erro...
	#Then Erro status code "401"
    Then Resposta "0"

