@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.ValidacaoCampos
Feature: PedidoMagentoDto
#testar a validação campo a campo

#O teste "PedidoMagentoDto" não faz sentido fazer, pois validamos os pricipais campos conforme abaixo:
#Exigimos o endereço de entrega em CadastrarPedido.CriacaoCliente/MagentoCriacaoCliente_PfFeature
#Verificação de campos de enderço de entrega preenchidos em CadastrarPedido.EspecificacaoAdicional.EnderecoEntregaSomentePFFeature
#Verificação de telefones CadastrarPedido.CriacaoCliente.Pf_Telefones
#Campos obrigatórios estão concluídos em CadastrarPedido.EspecificacaoAdicional.MagentoCriacaoCliente_Pf_ObrigatoriosFeature
#Verificação de pagtos estão em CadastrarPedido.EspecificacaoAdicional.FormaPagtoCriacaoMagentoDtoFeature
#Verificação de campos de appsettings CadastrarPedido.EspecificacaoAdicional.CamposLidosAppsettingsFeature
#Verificação de produtos composto e preço que vier estão em CadastrarPedido.EspecificacaoAdicional.EspecificacaoMagentoFeature
#Verificação de frete e ponto de referência estão em CadastrarPedido.EspecificacaoAdicional.FretePontoReferenciaFeature
@ignore
Scenario: PedidoMagentoDto
	When Fazer esta validação

Scenario: InfCriacaoPedido Pedido_bs_x_ac
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = ""
	Then Erro "Favor informar o número do pedido Magento(Pedido_bs_x_ac)!"
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "12345678"
	Then Erro "Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!"
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "1234567890"
	Then Erro "Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!"
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456789"
	Then Sem nenhum erro
	#salvamos várias vezes, por nenhum bom motivo, só para testar mais...
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456799"
	Then Sem nenhum erro
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "223456799"
	Then Sem nenhum erro

Scenario: InfCriacaoPedido Marketplace_codigo_origem
	#/// Número do pedido no marketplace (opcional, se o pedido é do magento este campo não existe)
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "123"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = ""
	Then Erro "Informe o Marketplace_codigo_origem."

	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = ""
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "123"
	Then Erro "Informe o Pedido_bs_x_marketplace."


@ignore
Scenario: Pedido_bs_x_marketplace e Marketplace_codigo_origem já existem
#2) Seria necessário tratar a possibilidade de ocorrer acesso concorrente entre o cadastramento semi-automático e a integração.
#Em ambos os casos, seria importante verificar no instante final antes da efetivar o cadastramento do pedido se o número Magento e,
#caso exista, o número do pedido marketplace já estão cadastrados em algum pedido c/ st_entrega válido (diferente de cancelado).
	When Fazer esta validação


#afazer: precisa fazer alteração em todas as aplicações para passar o sistemaResponsavel
# Parcela única é validado apenas se o cliente for PJ, no caso do magento não faremos pedido para PJ mas,
# aceitamos Parcela única para PF
# isso ocorre em FormaPagoBll.ObterFormaPagto
@ignore
Scenario: Validar se o que expomos pelo ObterCodigoMarketplace foi informado
	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "123"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456789"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "123"
	Then Erro "Código Marketplace não encontrado."

	Given Pedido base
	And Informo "InfCriacaoPedido.Pedido_bs_x_marketplace" = "123"
	And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "123456789"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "019"
	And Informo "Tipo_Parcelamento" = "5"
	And Informo "C_pu_valor" = "3132.90"
	Then Sem nenhum erro



Scenario: EnderecoCadastralCliente
	#não precisamos validar porque o endereço de entregaécopiado sbre o endereço cadastral no caso de PF. E PJ não está sendo aceito pela API

Scenario: EnderecoCadastralCliente CPF diferente do principal
	Given Pedido base
	And Informo "EnderecoCadastralCliente.Endereco_cnpj_cpf" = "1"
	And Informo "pedidoMagentoDto.Cnpj_Cpf" = "2"
	Then Erro "Cnpj_Cpf está diferente de EnderecoEntrega.EndEtg_cnpj_cpf."

@ignore
Scenario: DetalhesPedidoMagentoDto
	#//nao existe o DetalhesPedidoMagentoDto. Os valores a usar são:
	#//St_Entrega_Imediata: se for PF, sim. Se for PJ, não
	#// PrevisaoEntregaData = null
	#// BemDeUso_Consumo = COD_ST_BEM_USO_CONSUMO_SIM
	#//InstaladorInstala = COD_INSTALADOR_INSTALA_NAO
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{pedido}", verificar campo "st_entrega_imediata" = "2"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{pedido}", verificar campo "PrevisaoEntregaData" = "null"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{pedido}", verificar campo "StBemUsoComum" = "1"
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{pedido}", verificar campo "InstaladorInstalaStatus" = "1"
	
