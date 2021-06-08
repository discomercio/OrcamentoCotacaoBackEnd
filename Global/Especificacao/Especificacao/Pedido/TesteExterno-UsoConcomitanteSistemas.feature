@ignore
Feature: TesteExterno-UsoConcomitanteSistemas
	Vamos testar os vários sistemas em uso simultâneo.
	Escreemos o roteiro de testes aqui mas estes testes não são automatizados;
	marcamos aqui quando fizermos.


Scenario: TesteExterno-UsoConcomitanteSistemas
	#Colocar a API do magento no servidor
	#Fazer um batch para gerar, digamos, 20 pedidos pelo magento
	#	Para gerar pedidos, usar o programa em \arclube\Global\Testes\TestesApiMagento\TestesApiMagentoCadastrarPedido
	#Enquanto essa carga estiver rodando, criar um pedido pelo verdinho
	#
	#em 08/04/2021
	#edu: executadas duas intâncias do \arclube\Global\Testes\TestesApiMagento\TestesApiMagentoCadastrarPedido
	#e converti um prepedido em pedido pelo verdinho
	#tudo funcionou OK
	#
	#em 14/04/20121
	#gabriel: alterei a quantidade de pedido a ser inserido "TestesApiMagentoCadastrarPedido.exe",
	#executadas duas instâncias do \arclube\Global\Testes\TestesApiMagento\TestesApiMagentoCadastrarPedido
	#enquanto as duas intâncias inseriam pedidos, fui inserindo pedido usando o verdinho
	#as 2 instâncias pararam de inserir pedidos sem gerar exception,
	#olhei o log no appdev e tinha erro de deadlock
	#
	#Alteramos o nível de isolamento das transações para READ COMMITED e colocamos um bloqueio por operação
	#(quer dizer, apenas um cadastro de pedido é feito por vez, em todos os sisemas; no verdinho, na API Magento, na Loja, etc)
	#
	#ainda implementendo essas mudanças; refazer o teste depois que estiverem feitas
	Given Fazer este teste

