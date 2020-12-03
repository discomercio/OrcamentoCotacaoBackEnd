@ignore
@Especificacao.Pedido.Passo40
Feature: Endereco e produtos - t_PRODUTO_LOJA
Validações do PedidoNovoProdCompostoMask
#no ASP, em loja/PedidoNovoProdCompostoMask.asp

Background: Marca para reiniciar o banco
	Given Reiniciar banco ao terminar cenário

Scenario: Verificar produtos em t_PRODUTO_LOJA
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

Scenario: Verificar produtos - sem erro
	#vamos garantir que está salvando co o banco padrão
	Given Pedido base
	Then Sem nenhum erro

Scenario: Verificar produtos - sem t_PRODUTO_LOJA
	Given Pedido base
	#não gravamos nada em t_PRODUTO_LOJA
	Then Erro "Produto não cadastrado para loja"

Scenario: Verificar produtos - registro montado na mão - OK
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	Then Sem nenhum erro

Scenario: Verificar produtos - excluido_status
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "1"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	Then Erro "Produto não cadastrado para loja"

Scenario: Verificar produtos - excluido_status 2
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "1"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	Then Erro "Produto não cadastrado para loja"

Scenario: Verificar produtos - outra loja
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "987"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "987"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	Then Erro "Produto não cadastrado para loja"

Scenario: Verificar produtos - incompleto (sem todos os produtos)
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	#produto errado
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "903221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	Then Erro "Produto não cadastrado para loja"

#em loja/PedidoNovoConsiste.asp
#					if Ucase(Trim("" & rs("vendavel"))) <> "S" then
#						alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & " NÃO está disponível para venda."
#					elseif .qtde > rs("qtde_max_venda") then
#						alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": quantidade " & cstr(.qtde) & " excede o máximo permitido."
Scenario: Verificar produtos t_PRODUTO_LOJA - vendavel
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "N"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	Then Erro "regex .*NÃO está disponível para venda."

Scenario: Verificar produtos t_PRODUTO_LOJA - qtde_max_venda
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "0"
	And Gravar registro em "t_PRODUTO_LOJA"

	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "especial: loja do pedido"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"

	Then Erro "regex .*excede o máximo permitido."
