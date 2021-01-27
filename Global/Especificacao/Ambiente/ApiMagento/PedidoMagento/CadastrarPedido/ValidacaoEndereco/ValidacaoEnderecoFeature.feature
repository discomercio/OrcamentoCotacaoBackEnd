@ignore
Feature: ValidacaoEnderecoFeature

#    mais um conjunto de testes para criar, específicos para o magento (e vai ter que desabilitar alguns do pedido para o magento....)
#    Pedidos do magento validamos Cidade contra o IGBE e UF contra o CEP informado. Não validamos nenhum outro campo do endereço. Se o CEP não existir, aceitamos o que veio e só validar a cidade.
#    confirmando: se o magento mandar um CEP que não temos, aceitamos e só validamos a cdiade
#A validação do município com relação ao cadastro do IBGE como fazemos no cadastramento do pré-pedido/pedido
#creio que seria melhor fazermos sim, senão isso só será percebido no momento do faturamento
#Mas os demais campos eu creio que é melhor não fazer
#Eventualmente surgem CEPs novos que precisamos cadastrar manualmente no sistema, já que não temos
#uma atualização regular da base
#CEP sem 8 digitos rejeitamos, mas CEP que não tem na nossa base aceitamos

Scenario:validar endereco
	When fazer as validacoes conforme os comentários acima