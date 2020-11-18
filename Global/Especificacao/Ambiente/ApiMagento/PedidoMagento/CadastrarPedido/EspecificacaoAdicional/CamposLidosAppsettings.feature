﻿@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: CamposLidosAppsettings

@ListaDependencias
Scenario: AdicionarDependencia
	Given AdicionarDependencia ambiente = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", especificacao = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional.CamposLidosAppsettings"

@ListaDependencias
Scenario: Lista de verificações feitas
	Given Nome deste item "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional.CamposLidosAppsettings"
	And Implementado em "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias"


Scenario: Orcamentista = "FRETE" (vamos ler do appsettings) - precisa existir
	#Vamos colocar um indicador no appsettings que não esteja cadastrado no banco e ver se ele testa
	Given Reiniciar appsettings
	And Pedido base
	And Informo "appsettings.Orcamentista" = "um orçamentista que não existe"
	Then Erro "O Orçamentista não existe!"

	Given Reiniciar appsettings
	And Pedido base
	Then Sem nenhum erro

@ignore
Scenario: Orcamentista = "FRETE" (vamos ler do appsettings)
	#vamos verificar se salvou onde deveria
	#vamos testar com o KONAR
	When Fazer esta validação

@ignore
Scenario: Loja = "201" (vamos ler do appsettings)
	When Fazer esta validação

@ignore
Scenario: Vendedor = usuário que fez o login (ler do token)
	When Fazer esta validação
