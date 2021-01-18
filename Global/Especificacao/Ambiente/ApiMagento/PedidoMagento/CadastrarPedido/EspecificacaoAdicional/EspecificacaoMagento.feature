@ignore
Feature: EspecificacaoMagento
Definições da ApiMagento

Paradigma de salvamento: fazer o mesmo que acontece com o processo semi-automático.
Se o semi-automático der erro, damos erro. Se aceitar, aceitamos.

Estoque: não é um problema. 


Scenario: preço: aceitamos o valor que vier do magento.
	#nao fazemos nenhuma crítica nos preços que chegam do magento
	When Fazer esta validação

Scenario: produtos: sempre virão divididos, nunca vai vir um produto composto.
	When Fazer esta validação

#Esse teste esta sendo feito em "EnderecoEntregaFeature.EntregaImediata"
Scenario: Entrega Imediata
#Se o cliente for PF, sempre colocar com entrega imediata SIM
#Se o cliente for PJ, sempre colocar com entrega imediata NÃO
	When Fazer esta validação


