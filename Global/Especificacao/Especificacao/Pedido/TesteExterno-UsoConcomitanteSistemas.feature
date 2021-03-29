@ignore
Feature: TesteExterno-UsoConcomitanteSistemas
	Vamos testar os vários sistemas em uso simultâneo.
	Escreemos o roteiro de testes aqui mas estes testes não são automatizados;
	marcamos aqui quando fizermos.


Scenario: TesteExterno-UsoConcomitanteSistemas
Colocar a API do magento no servidor
Fazer um batch para gerar, digamos, 20 pedidos pelo magento (em curl ou de qualquer forma que for fácil)
Enquanto essa carga estiver rodando, criar um pedido pelo verdinho
	Given Fazer este teste
