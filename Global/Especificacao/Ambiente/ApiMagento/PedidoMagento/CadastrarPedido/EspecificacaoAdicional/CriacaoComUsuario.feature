@ignore
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



#FIQUEI CONFUSO, o nome da Feature e dos cenários estão como usuário mas, a explicação acima esta falando do cliente
#OBS: alterei os nomes para cliente
Scenario: Testar criando o cliente e cadastrando o pedido em uma única operação (verificar que o sistema_responsavel_cadastro é o magento)
	Given Pedido base
	And Limpar tabela "t_CLIENTE"
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "{valor_cnpj_cpf}", verificar campo "sistema_responsavel_cadastro" = "4"
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "{valor_cnpj_cpf}", verificar campo "sistema_responsavel_atualizacao" = "4"
	And Tabela "t_PEDIDO" registro com campo "cnpj_cpf" = "{valor_cnpj_cpf}", verificar campo "sistema_responsavel_cadastro" = "4"

# Acho que devemos criar uma rotina para confrontar os dados do cliente na t_CLIENTE ou
# cadastrar um cliente com outro sistema_responsavel_cadastro e verifcar se foi alterado o o campo sistema_responsavel_atualizacao
Scenario: Testar cadastrando o pedido com um cliente já existente no banco (verificar que não atualiza os dados de cadastro)
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "{valor_cnpj_cpf}", verificar campo "sistema_responsavel_atualizacao" = "2"
	And Tabela "t_PEDIDO" registro com campo "cnpj_cpf" = "{valor_cnpj_cpf}", verificar campo "sistema_responsavel_cadastro" = "4"


