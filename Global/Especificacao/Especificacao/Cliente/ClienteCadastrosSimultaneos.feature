@Especificacao.Cliente.ClienteCadastrosSimultaneos
@GerenciamentoBanco
Feature: ClienteCadastrosSimultaneos
#Fazer cadastros simultâneos de clientes


#este teste somente vai funcioanr no SQL Server real quando tivemos o sistema de bloqueio em funcionamento
Scenario: ClienteCadastrosSimultaneos
	Given Reiniciar banco ao terminar cenário
	Given Testar cadastros simultâneos com pedidosPorThread = "5" e numeroThreads = "10"

