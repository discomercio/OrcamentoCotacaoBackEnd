@GerenciamentoBanco
@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: CriacaoComUsuario

#do nosso lado, o que eles informarem como endereço de cobrança, usaremos p/ criar o cadastro principal,
#caso o cliente não exista ainda. Mas se o cliente já existir no sistema, não iremos atualizar o cadastro principal,
#iremos usar os dados somente no pedido
#
#
#edu> Pergunta: se o cliente já existir não atualizamos, certo? Mas e se ele tiver sido criado pelo magento, aí a informação seria mais atualizada
#hamilton> Acho que não dá p/ considerar c/ 100% de certeza se a informação estaria 100% atualizada. Teria que se analisar a data em que
#o cadastro foi criado no Magento
#ok. e usar o flag para indicar que esse t_cliente foi criado pelo magento (o sistema_responsavel_cadastro)

Background: reiniciar
	Given Reiniciar banco ao terminar cenário

Scenario: Testar criando o cliente e cadastrando o pedido em uma única operação (verificar que o sistema_responsavel_cadastro é o magento)
	Given Pedido base
	And Limpar tabela "t_CLIENTE"
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "14039603052", verificar campo "sistema_responsavel_cadastro" = "5"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "14039603052", verificar campo "sistema_responsavel_atualizacao" = "5"
	And Tabela "t_PEDIDO" registro criado, verificar campo "sistema_responsavel_cadastro" = "5"

Scenario: Testar cadastrando o pedido com um cliente já existente no banco (verificar que não atualiza os dados de cadastro)
	Given Pedido base
	And Limpar tabela "t_CLIENTE"
	Given Informo "OutroEndereco" = "true"
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_nome" = "Teste Cliente Magento 2"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "14039603052"
	When Informo "cnpj_cpf" = "14039603052"
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "14039603052", verificar campo "nome" = "Teste Cliente Magento 2"
	And Tabela "t_PEDIDO" registro criado, verificar campo "Endereco_nome" = "Teste Cliente Magento 2"

	#agora cadastramos outro pedido com o mesmo cliente
	Given Pedido base
	When Informo "EndEtg_endereco" = "Rua Professor Fábio Fanucchi"
	When Informo "EndEtg_endereco_numero" = "1"
	When Informo "EndEtg_bairro" = "Jardim São Paulo(Zona Norte)"
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_nome" = "Vivan do outro pedido"
	When Informo "EndEtg_tipo_pessoa" = "PF"
	When Informo "EnderecoCadastralCliente.Endereco_tipo_pessoa" = "PF"
	When Informo "EndEtg_cnpj_cpf" = "14039603052"
	When Informo "cnpj_cpf" = "14039603052"
	Then Sem nenhum erro
	#verifica que o cadastro na t_cliente não foi alterado
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "14039603052", verificar campo "nome" = "Teste Cliente Magento 2"
	And Tabela "t_PEDIDO" registro criado, verificar campo "Endereco_nome" = "Vivan do outro pedido"

