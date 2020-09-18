@ignore
Feature: Passo 60 Validação cliente

Scenario: Cliente existe
	Given Pedido base
	And Informo "CPF/CNPJ" = "1"
	Then Erro "Cliente não cadastrado"

Scenario: Cliente com CEP
#loja/PedidoNovoConfirma.asp
		#s = "SELECT * FROM t_CLIENTE WHERE (id='" & cliente_selecionado & "')"
		#set t_CLIENTE = cn.execute(s)
		#if Not t_CLIENTE.Eof then
		#	midia_selecionada = Trim("" & t_CLIENTE("midia"))
		#	indicador_original = Trim("" & t_CLIENTE("indicador"))
		#	tipo_cliente = Trim("" & t_CLIENTE("tipo"))
		#	if Trim("" & t_CLIENTE("cep")) = "" then alerta = "É necessário preencher o CEP no cadastro do cliente."
	When Fazer esta validação

Scenario: fakltando fazer
	#Incluir:
	#	- validar preços (desconto, preco de lista com o preco da tabela, etc, etc. Escrever todos os passos).
	#	- validar se o tipo de parcelamento é permitido para todos os produtos
	When Fazer esta validação

