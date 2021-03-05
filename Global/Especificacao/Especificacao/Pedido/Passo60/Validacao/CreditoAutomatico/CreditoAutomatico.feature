@Especificacao.Pedido.PedidoFaltandoImplementarSteps

Feature: CreditoAutomatico

Scenario: CreditoAutomatico - valor menor que Vl_aprov_auto_analise_credito
	#valor total <= Vl_aprov_auto_analise_credito
	#tpedido.Analise_Credito = 2;
	#tpedido.Analise_Credito_Usuario = AUTOMÁTICO;
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vendedor" = "usario da loja"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito" = "2"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito_usuario" = "AUTOMÁTICO"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratar_status" = "0"

Scenario: CreditoAutomatico - loja 01
	Given Pedido base
	When Informo "loja" = "01"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "loja" = "01"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vendedor" = "usario da loja"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito" = "2"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito_usuario" = "AUTOMÁTICO"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratar_status" = "0"

Scenario: CreditoAutomatico - loja 02
	Given Pedido base
	When Informo "loja" = "02"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "loja" = "02"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vendedor" = "usario da loja"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito" = "2"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito_usuario" = "AUTOMÁTICO"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratar_status" = "0"

Scenario: CreditoAutomatico - IsLojaGarantia
	#Tloja tloja = await (from l in Criacao.ContextoProvider.GetContextoLeitura().Tlojas
	#                                where l.Loja == pedido.Ambiente.Loja
	#                                select l).FirstOrDefaultAsync();
	#           IsLojaGarantia = false;
	#           if (tloja != null && tloja.Unidade_Negocio == Constantes.COD_UNIDADE_NEGOCIO_LOJA__GARANTIA)
	#               IsLojaGarantia = true;
	Given Pedido base
	When Informo "loja" = "alterar a Unidade_Negocio = GAR "
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "loja" = "loja alterada"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vendedor" = "usario da loja"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito" = "2"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito_usuario" = "AUTOMÁTICO"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratar_status" = "0"

#teste 3 =>
#	loja = 201 && pagamento á vista && pagamento em dinheiro
#	tpedido.Analise_Credito = 8;
#	tpedido.Analise_Credito_Usuario = AUTOMÁTICO;
Scenario: CreditoAutomatico - Loja Bonshop 212 á vista no dinheiro
	#teste 5 =>
	#	loja = 212 && pagamento á vista && pagamento por deposito = 2 || pagamento por boleto = 6
	#	tpedido.Analise_Credito = 9;
	#    tpedido.Analise_Credito_Usuario = AUTOMÁTICO;
	Given Pedido base
	When Informo "loja" = "212"
	When Informo "Tipo_Parcelamento" = "1"
	When Informo "tipo da forma de pagamento a vista no" = "2"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "loja" = "212"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vendedor" = "usario da loja"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito" = "9"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito_usuario" = "AUTOMÁTICO"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratar_status" = "0"

Scenario: CreditoAutomatico - Loja Bonshop 212 á vista no boleto
	#teste 5 =>
	#	loja = 212 && pagamento á vista && pagamento por deposito = 2 || pagamento por boleto = 6
	#	tpedido.Analise_Credito = 9;
	#    tpedido.Analise_Credito_Usuario = AUTOMÁTICO;
	Given Pedido base
	When Informo "loja" = "212"
	When Informo "Tipo_Parcelamento" = "1"
	When Informo "tipo da forma de pagamento a vista no" = "2"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "loja" = "212"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vendedor" = "usario da loja"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito" = "9"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito_usuario" = "AUTOMÁTICO"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratar_status" = "0"

Scenario: CreditoAutomatico - Loja Bonshop 212 cartão maquineta
	#teste 6 =>
	#	loja = 212 && pagamento no cartão maquineta = 6
	#	tpedido.Analise_Credito = 8;
	#    tpedido.Analise_Credito_Usuario = AUTOMÁTICO;
	Given Pedido base
	When Informo "loja" = "212"
	When Informo "Tipo_Parcelamento" = "cartão maquineta"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "loja" = "212"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "vendedor" = "usario da loja"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito" = "8"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_credito_usuario" = "AUTOMÁTICO"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "analise_endereco_tratar_status" = "0"