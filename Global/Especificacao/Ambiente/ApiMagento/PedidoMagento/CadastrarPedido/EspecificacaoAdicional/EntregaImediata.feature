Feature: EntregaImediata

#Entrega Imediata
#Se o cliente for PF, sempre colocar com entrega imediata SIM
@ignore
Scenario: EntregaImediata
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro com campo "cnpj_cpf" = "", verificar campo "EntregaImediata" = "2"
	