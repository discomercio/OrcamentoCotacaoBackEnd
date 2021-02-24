@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: FretePontoReferencia

@ignore
Scenario: campo "frete" -> se for <> 0, vamos usar o indicador. se for 0, sem indicador
	#Frete/RA
	#Valor de Frete: analisar se há valor de frete para definir se o pedido terá RA ou não.
	#- campo "frete" -> se for <> 0, vamos usar o indicador. se for 0, sem indicador
	#Se houver frete, deve-se automaticamente informar que o pedido possui RA e selecionar o indicador 'FRETE'.
	Given Pedido base
	#And Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "223456799"
	And Informo "InfCriacaoPedido.Marketplace_codigo_origem" = ""
	When Informo "Frete" = "10.00"
	When Informo "appsettings.Loja" = "201"	
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "indicador" = "FRETE"
	And Tabela "t_PEDIDO" registro criado, verificar campo "permite_RA_status" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "Opcao_Possui_RA" = "S"

Scenario: pedido sem indicador
	Given Pedido base
	When Informo "Frete" = "0"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "indicador" = ""
	And Tabela "t_PEDIDO" registro criado, verificar campo "permite_RA_status" = "1"
	And Tabela "t_PEDIDO" registro criado, verificar campo "Opcao_Possui_RA" = "N"

#esse teste esta sendo verificado no teste acima
@ignore
Scenario: campo "frete" salvo em t_PEDIDO.vl_frete
	#O valor do frete no momento não estamos tratando no sistema, mas acho que podemos salvar a informação no campo t_PEDIDO.vl_frete
	#Por enquanto, todos os registros estão c/ esse valor zerado e o campo não é usado em nenhum lugar, mas já que temos
	#a possibilidade de salvar essa informação, creio que deveríamos gravar nesse campo
	When Fazer esta validação


Scenario: Ponto de Referência - diferente de EndEtg_endereco_complemento
	#Colocar a informação do ponto de referência no campo 'Constar na NF'. Comparar o conteúdo do ponto de referência
	#com o campo complemento. Se forem iguais, não colocar em 'Constar na NF'.\n
	#Se o campo complemento exceder o tamanho do BD e precisar ser truncado,
	#copiá-lo no campo 'Constar na NF', junto com o ponto de referência.
	#
	Given Pedido base
	When Informo "EnderecoEntrega.PontoReferencia" = "teste de ponto de referencia"
	When Informo "EndEtg_endereco_complemento" = "outro texto"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "NFe_texto_constar" = "Ponto de referência: teste de ponto de referencia"
	And Tabela "t_PEDIDO" registro criado, verificar campo "endereco_complemento" = "outro texto"

Scenario:  Ponto de Referência - igual de EndEtg_endereco_complemento
	#Comparar o conteúdo do ponto de referência
	#com o campo complemento. Se forem iguais,
	#não colocar em 'Constar na NF'.
	Given Pedido base
	When Informo "EnderecoEntrega.PontoReferencia" = "teste de ponto de referencia"
	When Informo "EndEtg_endereco_complemento" = "teste de ponto de referencia"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "NFe_texto_constar" = ""
	And Tabela "t_PEDIDO" registro criado, verificar campo "endereco_complemento" = "teste de ponto de referencia"

Scenario:  Ponto de Referência - EndEtg_endereco_complemento com mais de 60 caracteres
	#Se o campo complemento exceder o tamanho do BD e precisar ser truncado,
	#copiá-lo no campo 'Constar na NF', junto com o ponto de referência.
	#obs => truncar 57 caracteres e colocar (...)
	Given Pedido base
	When Informo "EnderecoEntrega.PontoReferencia" = "teste de ponto de referencia"
	#                                                      10        20        30        40        50        60          
	When Informo "EndEtg_endereco_complemento" = "complemento endereço entrega 12 complemento endereço entrega 12"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "NFe_texto_constar" = "Complemento do endereço: complemento endereço entrega 12 complemento endereço entrega 12\nPonto de referência: teste de ponto de referencia"
	And Tabela "t_PEDIDO" registro criado, verificar campo "endereco_complemento" = "complemento endereço entrega 12 complemento endereço entrega"

Scenario:  Ponto de Referência - EndEtg_endereco_complemento com mais de 60 caracteres sem Ponto de referencia
	#Se o campo complemento exceder o tamanho do BD e precisar ser truncado,
	#copiá-lo no campo 'Constar na NF', junto com o ponto de referência.
	#obs => truncar 57 caracteres e colocar (...)
	Given Pedido base
	When Informo "EnderecoEntrega.PontoReferencia" = ""
	When Informo "EndEtg_endereco_complemento" = "complemento endereço entrega 12 complemento endereço entrega 12"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "NFe_texto_constar" = "Complemento do endereço: complemento endereço entrega 12 complemento endereço entrega 12"
	And Tabela "t_PEDIDO" registro criado, verificar campo "endereco_complemento" = "complemento endereço entrega 12 complemento endereço entrega"