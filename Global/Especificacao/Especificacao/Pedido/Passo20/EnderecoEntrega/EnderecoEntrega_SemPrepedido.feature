@Especificacao.Pedido.Passo20.EnderecoEntrega
Feature: Validar endereco de entrega 2
Estes testes não se aplicam ao prepedido porque o flag já é um bool, não pode receber qualquer valor

Background: Configuração
	#na ApiUnis, ele exige que o cliente já esteja cadastrado, então não valida o CPF/CNPJ
	#por enquanto, ignoramos no prepedido inteiro
	Given Ignorar feature no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	#na Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido o OutroEndereco só pode ser bool
	Given Ignorar feature no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

Scenario: Informado
#loja/ClienteEdita.asp 
#rotina fNEWConcluir

#loja/PedidoNovoConsiste.asp
#if rb_end_entrega = "" then
#	alerta = "Não foi informado se o endereço de entrega é o mesmo do cadastro ou não."

	Given Pedido base
	When Informo "OutroEndereco" = "XX"
	Then Erro "Informe se o endereço de entrega será o mesmo endereço do cadastro ou não!!"
	Given Pedido base com endereço de entrega
	When Informo "OutroEndereco" = "XX"
	Then Erro "Informe se o endereço de entrega será o mesmo endereço do cadastro ou não!!"

