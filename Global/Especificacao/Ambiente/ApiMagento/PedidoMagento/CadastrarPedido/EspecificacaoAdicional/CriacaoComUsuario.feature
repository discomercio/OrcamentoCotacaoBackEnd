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



Scenario: Testar criando o usuário e cadastrando o pedido em uma única operação (verificar que o sistema_responsavel_cadastro é o magento)
	When Fazer esta validação

Scenario: Testar cadastrando o pedido com um usuário já existente no banco (verificar que não atualiza os dados de cadastro)
	When Fazer esta validação

