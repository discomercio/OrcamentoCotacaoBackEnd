@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
Feature: Detalhes
	Nao existe o DetalhesPedidoMagentoDto. 
	Os valores a usar são:
		St_Entrega_Imediata = COD_ETG_IMEDIATA_SIM (2)
		PrevisaoEntregaData = null
		BemDeUso_Consumo = COD_ST_BEM_USO_CONSUMO_SIM (1)
		InstaladorInstala = COD_INSTALADOR_INSTALA_NAO (1)
		GarantiaIndicador = Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO (0)

Scenario: DetalhesPedidoMagentoDto
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "st_etg_imediata" = "2"
	And Tabela "t_PEDIDO" registro criado, verificar campo "PrevisaoEntregaData" = "null"
	And Tabela "t_PEDIDO" registro criado, verificar campo "StBemUsoConsumo" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "InstaladorInstalaStatus" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "GarantiaIndicador" = "0"
