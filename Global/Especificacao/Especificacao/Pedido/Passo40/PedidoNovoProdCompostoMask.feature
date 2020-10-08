@ignore
Feature: Endereco
Validações do PedidoNovoProdCompostoMask
#no ASP, em loja/PedidoNovoProdCompostoMask.asp

Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo40.PedidoNovoProdCompostoMask"
	Given Implementado em "Especificacao.Pedido.Pedido"


Scenario: Número preenchido
loja/PedidoNovoProdCompostoMask.asp linha 62
#loja/PedidoNovoProdCompostoMask.asp linha 62
#	if Trim(r_cliente.endereco_numero) = "" then
#		Response.Redirect("aviso.asp?id=" & ERR_CAD_CLIENTE_ENDERECO_NUMERO_NAO_PREENCHIDO)
#	elseif Len(Trim(r_cliente.endereco)) > CLng(MAX_TAMANHO_CAMPO_ENDERECO) then
#		Response.Redirect("aviso.asp?id=" & ERR_CAD_CLIENTE_ENDERECO_EXCEDE_TAMANHO_MAXIMO)
#		end if

	When Pedido base
	And  Cadastro do cliente "endereco_numero" = ""
	Then Erro "ERR_CAD_CLIENTE_ENDERECO_NUMERO_NAO_PREENCHIDO"

Scenario: Tamanho do endereço
loja/PedidoNovoProdCompostoMask.asp linha 62
também em loja/PedidoNovo.asp
	When Pedido base
	And  Cadastro do cliente "endereco" = "um texto muito grande, maior que TAMANHO MÁXIMO DO CAMPO ENDEREÇO DEVIDO À RESTRIÇÃO EXISTENTE NA NOTA FISCAL ELETRÔNICA - MAX_TAMANHO_CAMPO_ENDERECO = 60"
	Then Erro "ERR_CAD_CLIENTE_ENDERECO_EXCEDE_TAMANHO_MAXIMO"
	When Pedido base
	#                                               10        20        30       40         50        60    
	And  Cadastro do cliente "endereco" = "1234567890123456789012345678901234567890123456789012345678901"
	Then Erro "ERR_CAD_CLIENTE_ENDERECO_EXCEDE_TAMANHO_MAXIMO"
	And  Cadastro do cliente "endereco" = "123456789012345678901234567890123456789012345678901234567890"
	Then Sem erro "ERR_CAD_CLIENTE_ENDERECO_EXCEDE_TAMANHO_MAXIMO"

Scenario: ddd_res
	'	DDD VÁLIDO?
		if Not ddd_ok(r_cliente.ddd_res) then
			if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
			alerta = alerta & "DDD do telefone residencial é inválido!!"
			end if
	When Pedido base
	And Informo "ddd_res" = "1"
	Then Erro "DDD do telefone residencial é inválido!!"
			
	When Pedido base
	And Informo "ddd_res" = "123"
	Then Erro "DDD do telefone residencial é inválido!!"
			
	When Pedido base
	And Informo "ddd_res" = "12"
	Then Sem erro "DDD do telefone residencial é inválido!!"
			
Scenario: ddd_com
		if Not ddd_ok(r_cliente.ddd_com) then
			if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
			alerta = alerta & "DDD do telefone comercial é inválido!!"
			end if
	When Pedido base
	And Informo "ddd_com" = "1"
	Then Erro "DDD do telefone comercial é inválido!!"
			
	When Pedido base
	And Informo "ddd_com" = "123"
	Then Erro "DDD do telefone comercial é inválido!!"
			
	When Pedido base
	And Informo "ddd_com" = "12"
	Then Sem erro "DDD do telefone comercial é inválido!!"
			
			
Scenario: MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?
	#'	MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?
	#	if Not consiste_municipio_IBGE_ok(r_cliente.cidade, r_cliente.uf, s_lista_sugerida_municipios, msg_erro) then
	When Pedido base
	And Informo "cidade" = "Cidade que não está no IBGE"
	Then erro regex "Município .* não consta na relação de municípios do IBGE para a UF de .."
			
#			if Not consiste_municipio_IBGE_ok(EndEtg_cidade, EndEtg_uf, s_lista_sugerida_municipios, msg_erro) then
	When Pedido base
	And Informo "EndEtg_cidade" = "Cidade que não está no IBGE"
	Then erro regex "Município .* não consta na relação de municípios do IBGE para a UF de .."

@ignore
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
	When Pedido base
	And Informo lista de itens linha "1" campo "c_produto" = ""
	Then erro "Informe o código do produto!!"
	When Pedido base
	And Informo lista de itens linha "1" campo "c_qtde" = ""
	Then erro "Informe a quantidade!!"
	When Pedido base
	And Informo lista de itens linha "1" campo "c_qtde" = "0"
	Then erro "Quantidade inválida!!"
	When Pedido base
	And Informo lista de itens linha "1" campo "c_qtde" = "-1"
	Then erro "Quantidade inválida!!"


@ignore
Scenario: Não aceitamos pedidos vazios
	#if (!ha_item) {
	#	alert("Não há produtos na lista!!");
	#	f.c_fabricante[0].focus();
	#	return;
	#	}
	When Pedido base
	And Removo todos os itenss da lista de itens
	Then erro "Não há produtos na lista!!"

@ignore
Scenario: Verificar produtos
	#Loja/Global/AjaxConsultaDadosProdutoBD.asp 
	#strSql = _
	#				"SELECT " & _
	#					"*" & _
	#				" FROM t_PRODUTO" & _
	#					" INNER JOIN t_PRODUTO_LOJA" & _
	#						" ON (t_PRODUTO.fabricante=t_PRODUTO_LOJA.fabricante) AND (t_PRODUTO.produto=t_PRODUTO_LOJA.produto)" & _
	#				" WHERE" & _
	#					" (CONVERT(smallint,loja) = " & strLoja & ")" & _
	#					" AND (t_PRODUTO.produto = '" & vResp(intCounter).produto & "')"
	#			if Trim(vResp(intCounter).fabricante) <> "" then
	#				blnFabricanteInformado = True
	#				strSql = strSql & _
	#					" AND (t_PRODUTO.fabricante = '" & vResp(intCounter).fabricante & "')"
	#			else
	#				blnFabricanteInformado = False
	#				strSql = strSql & _
	#					" AND (t_PRODUTO.excluido_status = 0)" & _
	#					" AND (t_PRODUTO_LOJA.excluido_status = 0)"
	#				end if
Then afazer: Verificar que os produtos existen com esse critério


