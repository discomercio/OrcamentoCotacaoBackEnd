@ignore
Feature: PedidoMagentoDto
#testar a validação campo a campo

Scenario: PedidoMagentoDto
	When Fazer esta validação

Scenario: InfCriacaoPedido
	When Fazer esta validação

Scenario: EnderecoCadastralCliente
	When Fazer esta validação

Scenario: DetalhesPedidoMagentoDto
	#//nao existe o DetalhesPedidoMagentoDto. Os valores a usar são:
	#//St_Entrega_Imediata: se for PF, sim. Se for PJ, não
	#// PrevisaoEntregaData = null
	#// BemDeUso_Consumo = COD_ST_BEM_USO_CONSUMO_SIM
	#//InstaladorInstala = COD_INSTALADOR_INSTALA_NAO
	When Fazer esta validação
