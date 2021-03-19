@Especificacao.Pedido.Passo40
@GerenciamentoBanco
Feature: Endereco e produtos - t_PRODUTO_LOJA
Validações do PedidoNovoProdCompostoMask

#no ASP, em loja/PedidoNovoProdCompostoMask.asp
Background: Marca para reiniciar o banco
	Given Reiniciar banco ao terminar cenário
	Given No ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido" mapear erro "Produto cód.(003220) do fabricante cód.(003) não existe!" para "regex .*Produto.*não cadastrado para a loja."
	Given No ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido" mapear erro "Produto cód.(003221) do fabricante cód.(003) não existe!" para "regex .*Produto.*não cadastrado para a loja."
	Given No ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido" mapear erro "Erro do produto 003220 para mapear" para "regex .*Produto.*não cadastrado para a loja."
	Given No ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido" mapear erro "Erro do produto 003221 para mapear" para "regex .*Produto.*não cadastrado para a loja."
	Given No ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido" mapear erro "Erro do produto 003220 para mapear" para "regex .*Produto.*não está cadastrado para a loja."
	Given No ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido" mapear erro "Erro do produto 003221 para mapear" para "regex .*Produto.*não está cadastrado para a loja."
	Given No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido" mapear erro "Erro do produto 003220 para mapear" para "Produto cód.(003220) do fabricante cód.(003) não existe!"
	Given No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido" mapear erro "Erro do produto 003221 para mapear" para "Produto cód.(003221) do fabricante cód.(003) não existe!"
	Given No ambiente "Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.CadastrarPrepedidoPrepedidoApi" mapear erro "Erro do produto 003220 para mapear" para "Produto cód.(003220) do fabricante cód.(003) não existe!"
	Given No ambiente "Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.CadastrarPrepedidoPrepedidoApi" mapear erro "Erro do produto 003221 para mapear" para "Produto cód.(003221) do fabricante cód.(003) não existe!"

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
	And Limpar tabela "t_PRODUTO_LOJA"
	#não gravamos nada em t_PRODUTO_LOJA
	#Then Erro "Produto cód.(003220) do fabricante cód.(003) não existe!"
	Then Erro "Erro do produto 003220 para mapear"

Scenario: Verificar produtos - registro montado na mão - OK
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	And Limpar tabela "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "626.58"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "939.87"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	Given Pedido base
	Then Sem nenhum erro

Scenario: Verificar produtos - registro montado na mão - OK Magento
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	And Limpar tabela "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "201"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "626.58"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "201"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "939.87"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	Given Pedido base
	Then Sem nenhum erro

Scenario: Verificar produtos - excluido_status
	Esse teste não esta passando!
	Seguindo o código comentado do "AjaxConsultaDadosProdutoBD.asp" ele tem a condição de que se 
	o fabricante estiver vazio ele busca por "excluido_status = 0"
	o campo "excluido_status = 0" não era utilizado, ele foi inserido para esse teste
	essa busca de produtos não existe em nossa aplicação!!
	#
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "626.58"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "1"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "939.87"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	Then Erro "Produto cód.(003220) do fabricante cód.(003) não existe!"

Scenario: Verificar produtos - excluido_status 2
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "626.58"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "939.87"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "1"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	Then Erro "Produto cód.(003221) do fabricante cód.(003) não existe!"

#
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
	#Then Erro "Produto cód.(003220) do fabricante cód.(003) não existe!"
	Then Erro "Erro do produto 003220 para mapear"

Scenario: Verificar produtos - incompleto (sem todos os produtos)
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "626.58"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	#produto errado
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "903221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "939.87"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	#Then Erro "Produto cód.(003221) do fabricante cód.(003) não existe!"
	Then Erro "Erro do produto 003221 para mapear"

#
##em loja/PedidoNovoConsiste.asp
##					if Ucase(Trim("" & rs("vendavel"))) <> "S" then
##						alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & " NÃO está disponível para venda."
##					elseif .qtde > rs("qtde_max_venda") then
##						alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": quantidade " & cstr(.qtde) & " excede o máximo permitido."
Scenario: Verificar produtos t_PRODUTO_LOJA - vendavel
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "626.58"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "N"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "939.87"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	#Then Erro "Erro do produto 003220 para mapear"
	Then Erro "Produto cód.(003220) do fabricante cód.(003) não existe!"

Scenario: Verificar produtos t_PRODUTO_LOJA - qtde_max_venda
	#no prepedido esta validação não eiste
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "626.58"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "0"
	And Gravar registro em "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "202"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "939.87"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	Then Erro "regex .*excede o máximo permitido."

Scenario: Verificar produtos t_PRODUTO_LOJA - qtde_max_venda Magento
	#no prepedido esta validação não eiste
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	And Limpar tabela "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003220"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "201"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "626.58"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "0"
	And Gravar registro em "t_PRODUTO_LOJA"
	And Novo registro na tabela "t_PRODUTO_LOJA"
	And Novo registro em "t_PRODUTO_LOJA", campo "fabricante" = "003"
	And Novo registro em "t_PRODUTO_LOJA", campo "produto" = "003221"
	And Novo registro em "t_PRODUTO_LOJA", campo "loja" = "201"
	And Novo registro em "t_PRODUTO_LOJA", campo "preco_lista" = "939.87"
	And Novo registro em "t_PRODUTO_LOJA", campo "excluido_status" = "0"
	And Novo registro em "t_PRODUTO_LOJA", campo "vendavel" = "S"
	And Novo registro em "t_PRODUTO_LOJA", campo "qtde_max_venda" = "10000"
	And Gravar registro em "t_PRODUTO_LOJA"
	Then Erro "regex .*excede o máximo permitido."