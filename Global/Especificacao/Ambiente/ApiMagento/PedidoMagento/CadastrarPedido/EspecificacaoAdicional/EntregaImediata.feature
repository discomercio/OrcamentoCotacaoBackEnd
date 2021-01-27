@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional

Feature: EntregaImediata

#Entrega Imediata
#Se o cliente for PF, sempre colocar com entrega imediata SIM

Scenario: EntregaImediata
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "st_etg_imediata" = "2"
	