@ignore
@Especificacao/Pedido
@Implementacao/CaminhoFelizApi
Feature: CaminhoFelizApi
Caminho feliz da criação do pedido pela API
O mínimo que um pedido precisa para ser cadastrado pela API
Levantado a partir do ERP/loja/PedidoNovoConfirma.asp

Scenario: Cadastrar um pedido com o mínimo de informação possível
	Given Existe "login" = "usuario_sistema"
	And Existe "loja" = "202"
	And Existe cliente "cliente_id" = "000000000001" como PF
	And Existe produto "Fabricante" = "001", "Produto" = "001001", "Preco" = "123"
	#
	# Iniciar o pedido
	When Fiz login como "usuario_sistema" e escolhi a loja "202"
	And Pedido vazio
	And Informo "Cnpj_Cpf" = "687.307.550-77"
	And Informo "indicacao" = "não"
	And Informo "SelecaoCd" = "AUTOMATICO"
	And Informo "OutroEndereco" = "não"
	#
	#Tipo_Parcelamento: COD_FORMA_PAGTO_A_VISTA = "1"
	And Informo "FormaPagtoCriacao.Tipo_Parcelamento" = "1"
	#FormaPagtoCriacao.Op_av_forma_pagto: "Depósito" = 2
	And Informo "FormaPagtoCriacao.Op_av_forma_pagto" = "2"
	And Informo "FormaPagtoCriacao.CustoFinancFornecTipoParcelamento" = "AV"
	And Informo "FormaPagtoCriacao.CustoFinancFornecQtdeParcelas" = "0"
	#
	# somente 1 item no pedido
	And Informo "ListaProdutos.Fabricante" = "0001"
	And Informo "ListaProdutos.Produto" = "001001"
	And Informo "ListaProdutos.Qtde" = "1"
	And Informo "ListaProdutos.Desc_Dado" = "0"
	And Informo "ListaProdutos.Preco_Venda" = "123"
	And Informo "ListaProdutos.Preco_Fabricante" = "123"
	And Informo "ListaProdutos.Preco_Lista" = "123"
	And Informo "ListaProdutos.Preco_NF" = "123"
	And Informo "ListaProdutos.CustoFinancFornecCoeficiente" = "0"
	And Informo "ListaProdutos.CustoFinancFornecPrecoListaBase" = "123"
	#
	# totais do pedido
	And Informo "PermiteRAStatus" = "não"
	And Informo "ValorTotalDestePedidoComRA" = "123"
	And Informo "VlTotalDestePedido" = "123"
	#
	# Detalhes do pedido
	#EntregaImediata: COD_ETG_IMEDIATA_SIM = 2
	And Informo "DetalhesPrepedido.St_Entrega_Imediata" = "2"
	#BemDeUso_Consumo : COD_ST_BEM_USO_CONSUMO_SIM = 1
	And Informo "DetalhesPrepedido.BemDeUso_Consumo" = "1"
	#InstaladorInstala: COD_INSTALADOR_INSTALA_NAO = 1
	And Informo "DetalhesPrepedido.InstaladorInstala" = "1"
	#
	# Todos os campos informados
	And Salvo o pedido
	Then O pedido é criado
	And Campo "av_forma_pagto" = "2"
	# todo: terminar a lista de campos a verificar