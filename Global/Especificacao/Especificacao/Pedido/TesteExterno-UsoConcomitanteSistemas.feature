@ignore
Feature: TesteExterno-UsoConcomitanteSistemas
	Vamos testar os vários sistemas em uso simultâneo.
	Escreemos o roteiro de testes aqui mas estes testes não são automatizados;
	marcamos aqui quando fizermos.


Scenario: TesteExterno-UsoConcomitanteSistemas
Colocar a API do magento no servidor
Fazer um batch para gerar, digamos, 20 pedidos pelo magento
	Para gerar pedidos, usar o programa em \arclube\Global\Testes\TestesApiMagento\TestesApiMagentoCadastrarPedido
Enquanto essa carga estiver rodando, criar um pedido pelo verdinho

#em 08/04/2021
#executadas duas intâncias do \arclube\Global\Testes\TestesApiMagento\TestesApiMagentoCadastrarPedido
#e converti um prepedio em pedido pelo verdinho
#tudo funcionou OK

