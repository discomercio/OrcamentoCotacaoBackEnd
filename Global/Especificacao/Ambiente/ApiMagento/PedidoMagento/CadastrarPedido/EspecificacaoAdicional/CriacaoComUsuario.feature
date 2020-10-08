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

Scenario: falta: sexo e data de nascimento - vao ficar em branco
	#Sexo: Retirar obrigatoriedade do preenchimento do sexo, permitindo deixá-lo vazio.
	#cadastrar o usuário pelo magento e verificar que sexo e nascimento estão em branco (sexo fica string vazia e nascimento fica NULL)
	When Fazer esta validação

Scenario: Endereco_produtor_rural_status e Endereco_contribuinte_icms_status
#Ao cadastrar o cliente:
#- se for PF, assumimos Endereco_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RURAL_NAO e Endereco_contribuinte_icms_status = NAO
#- se for PJ, deixar o pedido st_etg_imediata = 1 (não)
#	e colocar Endereco_contribuinte_icms_status = inicial, Endereco_ie = vazio
#Contribuinte ICMS
#Para cliente PJ, quando o cliente for cadastrado automaticamente, manter o campo contribuinte_icms_status com o status default (zero).

	When Fazer esta validação

