@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: FluxoVerificacaoEndereco

Background: Configuracao
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"

#loja/PedidoNovoConfirma.asp
#linha 2289 - 1) VERIFICA SE O ENDEREÇO USADO É O DO PARCEIRO
#linha 2348 - 2)VERIFICA PEDIDOS DE OUTROS CLIENTES
#linha 2488 - ENDEREÇO DE ENTREGA (SE HOUVER) 1) VERIFICA SE O ENDEREÇO USADO É O DO PARCEIRO
#linha 2544 - ENDEREÇO DE ENTREGA (SE HOUVER) 2)VERIFICA PEDIDOS DE OUTROS CLIENTES
#linha 2685 - Se for o caso, marca analise_endereco_tratar_status no pedido
Scenario: Verificação de endereco - endereco do parceiro para magento
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	When Informo "EnderecoEntrega.EndEtg_endereco" = "Rua do Trabalhador"
	When Informo "EnderecoEntrega.EndEtg_endereco_numero" = "406"
	When Informo "EnderecoEntrega.EndEtg_cep" = "06550000"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO_ANALISE_ENDERECO" registro criado, verificar campo "tipo_endereco" = "C"
	And Tabela "t_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO" registro criado, verificar campo "tipo_endereco" = "P"

Scenario: Verificação de endereco - outros clientes
	Given Pedido base
	Then Sem nenhum erro
	Given Pedido base
	When Informo "cnpj_cpf" = "60082252017"
	When Informo "EndEtg_cnpj_cpf" = "60082252017"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO_ANALISE_ENDERECO" registro criado, verificar campo "tipo_endereco" = "C"
	And Tabela "t_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO" registro criado, verificar campo "tipo_endereco" = "M"

Scenario: Verificação de endereco - 50 pedidos com o mesmo endereco magento
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Reiniciar banco imediatamente
	Then Tabela "t_PEDIDO" verificar qtde de pedidos salvos "0"
	And Tabela "t_PEDIDO_ANALISE_ENDERECO" verificar qtde de itens salvos "0"
	And Tabela "t_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO" verificar qtde de itens salvos "0"
	Given Cadastra 50 pedidos com o mesmo endereco
	Then Tabela "t_PEDIDO" verificar qtde de pedidos salvos "51"
	And Tabela "t_PEDIDO_ANALISE_ENDERECO" verificar qtde de itens salvos "50"
	And Tabela "t_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO" verificar qtde de itens salvos "1275"

Scenario: Verificação de endereco - fazer todos testes para Loja
#precisa analisar todos os testes para saber se podemos utilizar os existentes para o magento