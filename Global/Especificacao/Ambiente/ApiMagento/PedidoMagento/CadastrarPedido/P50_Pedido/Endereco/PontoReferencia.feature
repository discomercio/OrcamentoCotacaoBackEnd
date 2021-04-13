@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: PontoReferencia
	O campo de Ponto de referencia deve ser salvo no campo t_PEDIDO.NFe_texto_constar

Scenario: PontoReferencia - diferente de EndEtg_endereco_complemento
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

Scenario: PontoReferencia - igual de EndEtg_endereco_complemento
	#Comparar o conteúdo do ponto de referência
	#com o campo complemento. Se forem iguais,
	#não colocar em 'Constar na NF'.
	Given Pedido base
	When Informo "EnderecoEntrega.PontoReferencia" = "teste de ponto de referencia"
	When Informo "EndEtg_endereco_complemento" = "teste de ponto de referencia"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "NFe_texto_constar" = ""
	And Tabela "t_PEDIDO" registro criado, verificar campo "endereco_complemento" = "teste de ponto de referencia"

Scenario: PontoReferencia - EndEtg_endereco_complemento com mais de 60 caracteres
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

Scenario: PontoReferencia - EndEtg_endereco_complemento com mais de 60 caracteres sem Ponto de referencia
	#Se o campo complemento exceder o tamanho do BD e precisar ser truncado,
	#copiá-lo no campo 'Constar na NF', junto com o ponto de referência.
	#obs => truncar 57 caracteres e colocar (...)
	Given Pedido base
	When Informo "EnderecoEntrega.PontoReferencia" = ""
	When Informo "EndEtg_endereco_complemento" = "complemento endereço entrega 12 complemento endereço entrega 12"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "NFe_texto_constar" = "Complemento do endereço: complemento endereço entrega 12 complemento endereço entrega 12"
	And Tabela "t_PEDIDO" registro criado, verificar campo "endereco_complemento" = "complemento endereço entrega 12 complemento endereço entrega"