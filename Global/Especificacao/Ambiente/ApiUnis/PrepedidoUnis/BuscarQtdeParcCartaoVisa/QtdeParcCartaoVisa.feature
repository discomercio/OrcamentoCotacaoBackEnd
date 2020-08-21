Feature: BuscarQtdeParcCartaoVisa

Background:
	Given Nome deste item "Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.QtdeParcCartaoVisa"
	And Implementado em "Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.ListaExecucao"	
    And Reiniciar banco quando terminar o teste

Scenario: retorna o que existe
            var qtdeTask = from c in db.TprazoPagtoVisanets
                           where c.Tipo == Constantes.COD_VISANET_PRAZO_PAGTO_LOJA
                           select c.Qtde_parcelas;
            int qtde = Convert.ToInt32(await qtdeTask.FirstOrDefaultAsync());
    Given Limpar tabela "t_PRAZO_PAGTO_VISANET"
	And Novo registro em "t_PRAZO_PAGTO_VISANET"
	And Novo registro "tipo" = "Constantes.COD_VISANET_PRAZO_PAGTO_LOJA"
	And Novo registro "qtde_parcelas" = "123"
	And Gravar registro 
    Then Resposta "QtdeParcCartaoVisa" = "123"

Scenario: retorna o que existe 2
    Given Limpar tabela "t_PRAZO_PAGTO_VISANET"
	And Novo registro em "t_PRAZO_PAGTO_VISANET"
	And Novo registro "tipo" = "Constantes.COD_VISANET_PRAZO_PAGTO_LOJA"
	And Novo registro "qtde_parcelas" = "321"
	And Gravar registro 
    Then Resposta "QtdeParcCartaoVisa" = "321"

Scenario: não existe 
#talvez a gente desse retornar 0... mas erro parece razoável.
    Given Limpar tabela "t_PRAZO_PAGTO_VISANET"
    #Then Resposta "QtdeParcCartaoVisa" = "0"
	Then Erro status code "401"

