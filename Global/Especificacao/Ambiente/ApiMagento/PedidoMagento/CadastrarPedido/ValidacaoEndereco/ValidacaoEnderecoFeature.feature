@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: ValidacaoEnderecoFeature

#mais um conjunto de testes para criar, específicos para o magento (e vai ter que desabilitar alguns do pedido para o magento....)
#Pedidos do magento validamos Cidade contra o IGBE e UF contra o CEP informado. Não validamos nenhum outro campo do endereço.
#Se o CEP não existir, aceitamos o que veio e só validar a cidade.
#confirmando: se o magento mandar um CEP que não temos, aceitamos e só validamos a cdiade
#A validação do município com relação ao cadastro do IBGE como fazemos no cadastramento do pré-pedido/pedido
#creio que seria melhor fazermos sim, senão isso só será percebido no momento do faturamento
#Mas os demais campos eu creio que é melhor não fazer
#Eventualmente surgem CEPs novos que precisamos cadastrar manualmente no sistema, já que não temos
#uma atualização regular da base
#CEP sem 8 digitos rejeitamos, mas CEP que não tem na nossa base aceitamos
@ignore
Scenario:validar endereco
	When fazer as validacoes conforme os comentários acima

Scenario: Validação de cidade X IBGE e UF X CEP - sucesso
	Given Pedido base
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "SP"
	When Informo "EndEtg_cep" = "02045080"
	When Informo "EndEtg_endereco" = "teste"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "teste"
	Then Sem nenhum erro

@ignore
Scenario: Validação de CEP que não existe na base - sucesso
	#o cep 01010-900 não existe na base e deve ser aceito no magento
	#deve validar apenas a qtde de caracteres do CEP = 8
	#validar a Cidade com o IBGE??
	Given Pedido base
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "XX"
	When Informo "EndEtg_cep" = "01010900"
	When Informo "EndEtg_endereco" = "teste"
	When Informo "EndEtg_endereco_numero" = "97"
	When Informo "EndEtg_endereco_complemento" = "teste"
	When Informo "EndEtg_bairro" = "teste"
	Then Sem nenhum erro

Scenario: Validação de CEP que não existe na base - CEP menor que 8 dígitos
	#existe verificação de 5 digitos mas, na base não temos para testar isso
	Given Pedido base
	When Informo "EndEtg_cidade" = "São Paulo"
	When Informo "EndEtg_uf" = "XX"
	When Informo "EndEtg_cep" = "0101090"
	Then Erro "CEP INVÁLIDO."