@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: ValidacaoCep
	Pedidos do magento validamos Cidade contra o IGBE e UF contra o CEP informado. Não validamos nenhum outro campo do endereço.
	Se o CEP não existir, aceitamos o que veio e só validar a cidade e a UF no IBGE.
	confirmando: se o magento mandar um CEP que não temos, aceitamos e só validamos a cidade e UF.
	A validação do município com relação ao cadastro do IBGE como fazemos no cadastramento do pré-pedido/pedido
	creio que seria melhor fazermos sim, senão isso só será percebido no momento do faturamento
	Mas os demais campos eu creio que é melhor não fazer
	Eventualmente surgem CEPs novos que precisamos cadastrar manualmente no sistema, já que não temos
	uma atualização regular da base
	CEP sem 8 digitos rejeitamos, mas CEP que não tem na nossa base aceitamos

Scenario: Validacao - cidade X IBGE e UF X CEP - sucesso
	Given Pedido base
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_endereco" = "teste"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "teste"
	Then Sem nenhum erro

Scenario: Validacao - cidade X IBGE e UF X CEP - sucesso no cadastro de cliente
	Given Limpar tabela "t_CLIENTE"
	Given Reiniciar banco ao terminar cenário
	Given Pedido base
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_endereco" = "teste"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "teste"
	Then Sem nenhum erro
	#pedido sem cadastrar o cliente
	Given Pedido base
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_endereco" = "teste"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "teste"
	Then Sem nenhum erro

Scenario: Validacao - CEP que não existe na base - sucesso
	Given Limpar tabela "t_CLIENTE"
	Given Reiniciar banco ao terminar cenário
	#o cep 01010-900 não existe na base e deve ser aceito no magento
	#deve validar apenas a qtde de caracteres do CEP = 8
	#a cidade consta no IBGE
	#não deve validar UF, Endereço, Bairro
	Given Pedido base
	When Informo "Endereco_cnpj_cpf" = "14039603052"
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "MG"
	When Informo "EndEtg_cep" = "01010900"
	When Informo "EndEtg_endereco" = "teste"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "teste"
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "14039603052", verificar campo "cep" = "01010900"
	#pedido sem cadastrar o cliente
	Given Pedido base
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "MG"
	When Informo "EndEtg_cep" = "01010900"
	When Informo "EndEtg_endereco" = "teste"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "teste"
	Then Sem nenhum erro

Scenario: Validacao - CEP que não existe na base - CEP menor que 8 dígitos
	Given Limpar tabela "t_CLIENTE"
	Given Reiniciar banco ao terminar cenário
	#existe verificação de 5 digitos mas, na base não temos para testar isso
	Given Pedido base
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "MG"
	When Informo "EndEtg_cep" = "0101090"
	Then Erro "CEP INVÁLIDO."
	#agora cadastramos o cliente
	Given Pedido base
	Then Sem nenhum erro
	#pedido sem cadastrar o cliente
	Given Pedido base
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "MG"
	When Informo "EndEtg_cep" = "0101090"
	Then Erro "CEP INVÁLIDO."

Scenario: Validacao - CEP vazio
	Given Limpar tabela "t_CLIENTE"
	Given Reiniciar banco ao terminar cenário

	Given Pedido base
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "MG"
	When Informo "EndEtg_cep" = ""
	Then Erro "INFORME O CEP."
	#agora cadastramos o cliente
	Given Pedido base
	Then Sem nenhum erro
	#pedido sem cadastrar o cliente
	Given Pedido base
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "MG"
	When Informo "EndEtg_cep" = ""
	Then Erro "INFORME O CEP."


#se o não CEP existir e a cidade não estiver no IBGE, não podemos aceitar o pedido porque a nota fiscal não será emitida.
Scenario: Validacao - cidade que não consta no IBGE se o CEP não existir - erro
	Given Limpar tabela "t_CLIENTE"
	Given Reiniciar banco ao terminar cenário
	#cidade não consta no IBGE
	#cep não consta na base
	Given Pedido base
	When Informo "EndEtg_cidade" = "Abacate da Pedreira"
	When Informo "EndEtg_uf" = "AP"
	When Informo "EndEtg_cep" = "01010900"
	Then Erro "Município 'Abacate da Pedreira' não consta na relação de municípios do IBGE para a UF de 'AP'!"
	#agora cadastramos o cliente
	Given Pedido base
	Then Sem nenhum erro
	#pedido sem cadastrar o cliente
	Given Pedido base
	When Informo "EndEtg_cidade" = "Abacate da Pedreira"
	When Informo "EndEtg_uf" = "AP"
	When Informo "EndEtg_cep" = "01010900"
	Then Erro "Município 'Abacate da Pedreira' não consta na relação de municípios do IBGE para a UF de 'AP'!"


#se o CEP existir e a cidade não estiver no IBGE, não podemos aceitar o pedido porque a nota fiscal não será emitida.
Scenario: Validacao - cidade que não consta no IBGE se o CEP existir - erro
	Given Limpar tabela "t_CLIENTE"
	Given Reiniciar banco ao terminar cenário
	#cidade não consta no IBGE
	#cep consta na base
	Given Pedido base
	When Informo "EndEtg_cidade" = "Abacate da Pedreira"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	Then Erro "Município 'Abacate da Pedreira' não consta na relação de municípios do IBGE para a UF de 'SP'!"
	#agora cadastramos o cliente
	Given Pedido base
	Then Sem nenhum erro
	#pedido sem cadastrar o cliente
	Given Pedido base
	When Informo "EndEtg_cidade" = "Abacate da Pedreira"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	Then Erro "Município 'Abacate da Pedreira' não consta na relação de municípios do IBGE para a UF de 'SP'!"

Scenario: Validacao - Sem validação de cidade X CEP
	Given Limpar tabela "t_CLIENTE"
	Given Reiniciar banco ao terminar cenário
	Given Pedido base
	When Informo "EndEtg_cidade" = "Uma cidade em SP"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_endereco" = "teste"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "teste"
	Then Sem nenhum erro

	#o mesmo CEP agora com outra cidade
	Given Pedido base
	When Informo "EndEtg_cidade" = "Outra cidade em SP"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_endereco" = "teste"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "teste"
	Then Sem nenhum erro

Scenario: Validacao - Sem validação de endereço X CEP
	Given Limpar tabela "t_CLIENTE"
	Given Reiniciar banco ao terminar cenário
	Given Pedido base
	When Informo "EndEtg_cidade" = "Santo André"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_endereco" = "outro endereco"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "teste"
	Then Sem nenhum erro

Scenario: Validacao - Sem validação de bairro X CEP
	Given Limpar tabela "t_CLIENTE"
	Given Reiniciar banco ao terminar cenário
	Given Pedido base
	When Informo "EndEtg_cidade" = "Santo André"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_endereco" = "outro endereco"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "outro bairro"
	Then Sem nenhum erro

Scenario: Validacao - Sem validação de UF X CEP
	#Pedidos do magento validamos Cidade contra o IGBE e UF contra o CEP informado. Não validamos nenhum outro campo do endereço.
	Given Limpar tabela "t_CLIENTE"
	Given Reiniciar banco ao terminar cenário
	Given Pedido base
	When Informo "EndEtg_cidade" = "Santo André"
	When Informo "EndEtg_uf" = "RJ"
	#o CEP 02045080 está com UF SP
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_endereco" = "outro endereco"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "outro bairro"
	Then Erro "Estado não confere!"

	Given Pedido base
	When Informo "EndEtg_cidade" = "Santo André"
	When Informo "EndEtg_uf" = "SP"
	#o CEP 02045080 está com UF SP
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_endereco" = "outro endereco"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "outro bairro"
	Then Sem nenhum erro

Scenario: Validacao - Garante que não grava endereço de entrega
	#Garantir que o pedido PF nunca entra com endereço de entrega
	Given Pedido base
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro criado, verificar campo "EndEtg_cnpj_cpf" = "null"
	And Tabela "t_PEDIDO" registro criado, verificar campo "EndEtg_endereco" = "null"