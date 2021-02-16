@Especificacao.Pedido.PedidoFaltandoImplementarSteps
#@Especificacao.Pedido.Passo40
Feature: Endereco e produtos
Validações do PedidoNovoProdCompostoMask

#no ASP, em loja/PedidoNovoProdCompostoMask.asp
#Scenario: Configuração
#	Given Nome deste item "Especificacao.Pedido.Passo40.PedidoNovoProdCompostoMask"
#	Given Implementado em "Especificacao.Pedido.Pedido"
Scenario: Número preenchido
loja/PedidoNovoProdCompostoMask.asp linha 62
	#loja/PedidoNovoProdCompostoMask.asp linha 62
	#	if Trim(r_cliente.endereco_numero) = "" then
	#		Response.Redirect("aviso.asp?id=" & ERR_CAD_CLIENTE_ENDERECO_NUMERO_NAO_PREENCHIDO)
	#	elseif Len(Trim(r_cliente.endereco)) > CLng(MAX_TAMANHO_CAMPO_ENDERECO) then
	#		Response.Redirect("aviso.asp?id=" & ERR_CAD_CLIENTE_ENDERECO_EXCEDE_TAMANHO_MAXIMO)
	#		end if
	Given Pedido base
	And Cadastro do cliente "endereco_numero" = ""
	Then Erro "ERR_CAD_CLIENTE_ENDERECO_NUMERO_NAO_PREENCHIDO"

Scenario: Tamanho do endereço
	#loja/PedidoNovoProdCompostoMask.asp linha 62
	#também em loja/PedidoNovo.asp
	#Implementado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\CriacaoCliente\CriacaoCliente_Pf_Obrigatorios.feature
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	And Cadastro do cliente "endereco" = "um texto muito grande, maior que TAMANHO MÁXIMO DO CAMPO ENDEREÇO DEVIDO À RESTRIÇÃO EXISTENTE NA NOTA FISCAL ELETRÔNICA - MAX_TAMANHO_CAMPO_ENDERECO = 60"
	Then Erro "ERR_CAD_CLIENTE_ENDERECO_EXCEDE_TAMANHO_MAXIMO"
	Given Pedido base
	#                                               10        20        30       40         50        60
	And Cadastro do cliente "endereco" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "ERR_CAD_CLIENTE_ENDERECO_EXCEDE_TAMANHO_MAXIMO"
	#
	Given Pedido base
	And Cadastro do cliente "endereco" = "123456789012345678901234567890123456789012345678901234567890"
	Then Sem erro "ERR_CAD_CLIENTE_ENDERECO_EXCEDE_TAMANHO_MAXIMO"

Scenario: ddd_res
	#'	DDD VÁLIDO?
	#	if Not ddd_ok(r_cliente.ddd_res) then
	#		if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
	#		alerta = alerta & "DDD do telefone residencial é inválido!!"
	#		end if
	Given Pedido base
	When Informo "ddd_res" = "1"
	Then Erro "DDD do telefone residencial é inválido!!"
	Given Pedido base
	When Informo "ddd_res" = "123"
	Then Erro "DDD do telefone residencial é inválido!!"
	Given Pedido base
	When Informo "ddd_res" = "12"
	Then Sem erro "DDD do telefone residencial é inválido!!"

Scenario: ddd_com
	#if Not ddd_ok(r_cliente.ddd_com) then
	#	if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
	#	alerta = alerta & "DDD do telefone comercial é inválido!!"
	#	end if
	Given Pedido base
	When Informo "ddd_com" = "1"
	Then Erro "DDD do telefone comercial é inválido!!"
	Given Pedido base
	When Informo "ddd_com" = "123"
	Then Erro "DDD do telefone comercial é inválido!!"
	Given Pedido base
	When Informo "ddd_com" = "12"
	Then Sem erro "DDD do telefone comercial é inválido!!"

Scenario: MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?
	#'	MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?
	#	if Not consiste_municipio_IBGE_ok(r_cliente.cidade, r_cliente.uf, s_lista_sugerida_municipios, msg_erro) then
	Given Pedido base
	When Informo "cidade" = "Cidade que não está no IBGE"
	Then Erro "Município .* não consta na relação de municípios do IBGE para a UF de .."
	#			if Not consiste_municipio_IBGE_ok(EndEtg_cidade, EndEtg_uf, s_lista_sugerida_municipios, msg_erro) then
	Given Pedido base
	When Informo "EndEtg_cidade" = "Cidade que não está no IBGE"
	Then Erro "Município .* não consta na relação de municípios do IBGE para a UF de .."

Scenario: Produtos e quantidades devem existir
	#if (b) {
	#	ha_item=true;
	#	if (trim(f.c_produto[i].value)=="") {
	#		alert("Informe o código do produto!!");
	#		f.c_produto[i].focus();
	#		return;
	#		}
	#	if (trim(f.c_qtde[i].value)=="") {
	#		alert("Informe a quantidade!!");
	#		f.c_qtde[i].focus();
	#		return;
	#		}
	#	if (parseInt(f.c_qtde[i].value)<=0) {
	#		alert("Quantidade inválida!!");
	#		f.c_qtde[i].focus();
	#		return;
	#		}
	#	}
	#}
	Given Pedido base
	When Lista de itens "0" informo "c_produto" = ""
	Then Erro "Informe o código do produto!!"
	Given Pedido base
	When Lista de itens "0" informo "c_qtde" = ""
	Then Erro "Informe a quantidade!!"
	Given Pedido base
	When Lista de itens "0" informo "c_qtde" = "0"
	Then Erro "Quantidade inválida!!"
	Given Pedido base
	When Lista de itens "0" informo "c_qtde" = "-1"
	Then Erro "Quantidade inválida!!"

Scenario: Não aceitamos pedidos vazios
	#if (!ha_item) {
	#	alert("Não há produtos na lista!!");
	#	f.c_fabricante[0].focus();
	#	return;
	#	}
	Given Pedido base
	When Lista de itens com "0" itens
	Then Erro "Não há produtos na lista!!"