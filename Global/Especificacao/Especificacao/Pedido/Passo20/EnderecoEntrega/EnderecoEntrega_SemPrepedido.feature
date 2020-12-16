@Especificacao.Pedido.Passo20.EnderecoEntrega
Feature: Validar endereco de entrega 2
Estes testes não se aplicam ao prepedido porque o flag já é um bool, não pode receber qualquer valor

Background: Configuração
	#na ApiUnis, ele exige que o cliente já esteja cadastrado, então não valida o CPF/CNPJ
	#por enquanto, ignoramos no prepedido inteiro
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	#na Ambiente.ApiMagento.PedidoMagento.CadastrarPedido o OutroEndereco só pode ser bool
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	#na "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido" o OutroEndereco só pode ser bool
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"

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

