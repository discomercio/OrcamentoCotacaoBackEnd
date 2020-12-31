﻿@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional
Feature: FLuxoCadastroPedidoMagento - PF

Scenario: salvando o pedido base
	Given Pedido base
	Then Sem nenhum erro

Scenario: Fluxo de cadastro do magento
	Given Esta é a especificação, está sendo testado em outros .feature

#Paradigma de salvamento: fazer o mesmo que acontece com o processo semi-automático.
#Se o semi-automático der erro, damos erro. Se aceitar, aceitamos.


#Testado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\CriacaoCliente\CriacaoCliente_Pf.feature
#Testado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\EspecificacaoAdicional\EnderecoEntrega.feature
#    Endereço
#    Se o cliente for PF, sempre será usado somente o endereço de entrega como sendo o único endereço do cliente.
#
#    Se for PF, copio o endereço de entrega para o endereço de cobrança e apago o endereço de entrega.
#	    no caso de campos que só existam no endereço de cobrança (exemplo: telefone) mantemos o do endereço de cobrança e não exigimos o campo.
#    Depois disso, se o cliente não existir, cadastrar com o endereço de cobrança
#
#    Do nosso lado, o que eles informarem como endereço de cobrança, usaremos p/ criar o cadastro principal,
#    caso o cliente não exista ainda. Mas se o cliente já existir no sistema, não iremos atualizar o cadastro principal,
#    iremos usar os dados somente no pedido
#    usar o flag para indicar que esse t_cliente foi criado pelo magento (o sistema_responsavel_cadastro)
#    Ao chegar um pedido, se o cliente não existir, cadstramos ele imediatamente.
#    Ao cadastrar o cliente:
#    - se for PF, assumimos Endereco_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RURAL_NAO e Endereco_contribuinte_icms_status = NAO

#    ao criar o pedido, tem que cadastrar tb o cliente (sem o sexo)
#    eles mandam:
#    - dados cadastrais
#    - dados de cobrança
#    - dados de entrega
#    falta: sexo e data de nascimento - vao ficar em branco
#
#   Sexo
#   Retirar obrigatoriedade do preenchimento do sexo, permitindo deixá-lo vazio.
#
#201229 reuniao semanal.txt
#- fazer o endereço de entrega para PF obrigatório?
#sim, exigir.
#
#pergunta:
#	- exigir telefones com a lógica atual (exemplo: não permitir telefone comercial para PF)
#	atualmente, se é cliente PF, não aceitamos nenhum telefone.
#
#	lógica do endereço de entrega:
#	- se cliente PF, somente endereço e justificativa (proibimos os outros campos)
#	- se cliente PJ, tem telefones, CPF/CNPJ, IE, razão social
#	lógica do cadastro do cliente:
#	- se cliente PF, exige pelo menos um telefone
#
#resposta:
#	primeiro passar para o endereço de cobrança
#	não exigir telefones e aceitamos todos que recebermos


#Testado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\EspecificacaoAdicional\COD_FORMA_PAGTO_PARCELADO_CARTAO.feature
#	pergunta: se COD_FORMA_PAGTO_PARCELADO_CARTAO temos que usar os coeficientes do fabricante?
#	respsota: sim, mas precisa manter o valor do preço e o total da nota igual. Vamos colocar essa diferença no Vl Lista


#Testado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\EspecificacaoAdicional\FretePontoReferencia.feature
#	- campo "frete" -> se for <> 0, vamos usar o indicador. se for 0, sem indicador
#	O valor do frete no momento não estamos tratando no sistema, mas acho que podemos salvar a informação no campo t_PEDIDO.vl_frete
#	Por enquanto, todos os registros estão c/ esse valor zerado e o campo não é usado em nenhum lugar, mas já que temos a
#	possibilidade de salvar essa informação, creio que deveríamos gravar nesse campo
#	Frete/RA
#	Valor de Frete: analisar se há valor de frete para definir se o pedido terá RA ou não.
#	Se houver frete, deve-se automaticamente informar que o pedido possui RA e selecionar o indicador 'FRETE'.
#	//Orcamentista = "FRETE" (vamos ler do appsettings)
#	//Loja = "201" (vamos ler do appsettings)
#	//Vendedor = usuário que fez o login (ler do token)
#
#	Ponto de Referência
#	Colocar a informação do ponto de referência no campo 'Constar na NF'. Comparar o conteúdo do ponto de referência com o campo complemento.
#	Se forem iguais, não colocar em 'Constar na NF'. Se o campo complemento exceder o tamanho do BD e precisar ser truncado, copiá-lo no campo
#	'Constar na NF', junto com o ponto de referência.

#Testado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\EspecificacaoAdicional\Tipo_Parcelamento.feature
#    Pedido que vier do Markeplace deve ser Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELA_UNICA = "5"
#    e Op_pu_forma_pagto = "2" "Depósito" e C_pu_vencto_apos = 30 dias (Definido no appsettings)
#    Pedido que vier do Magento deve ser Tipo_Parcelamento = COD_FORMA_PAGTO_A_VISTA = "1"
#    e Op_av_forma_pagto = "6" "Boleto" ou Tipo_Parcelamento = COD_FORMA_PAGTO_PARCELADO_CARTAO = "2"
#    (Definido no appsettings)
#    Tipo_Parcelamento:
#        COD_FORMA_PAGTO_A_VISTA = "1",
#        COD_FORMA_PAGTO_PARCELADO_CARTAO = "2",
#        COD_FORMA_PAGTO_PARCELA_UNICA = "5",

#Testado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\EspecificacaoAdicional\EntregaImediata.feature
#	Entrega Imediata
#	Se o cliente for PF, sempre colocar com entrega imediata SIM

#Testado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\EspecificacaoAdicional\ProdutosPresencaEstoque.feature
#	pergunta:
#		No magento, caso o produto não tenha presença no estoque, salvamos o pedido normalmente?
#	resposta:
#		sim. quando entra o estoque, esse pedido é automaticamente suprido.
#
#	preço: aceitamos o valor que vier do magento. Não validamos o preço.
#


#Testado em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\ValidacaoCampos\PedidoMagentoDto.feature
#	2) Seria necessário tratar a possibilidade de ocorrer acesso concorrente entre o cadastramento semi-automático e a integração.
#	Em ambos os casos, seria importante verificar no instante final antes da efetivar o cadastramento do pedido se o número Magento e,
#	caso exista, o número do pedido marketplace já estão cadastrados em algum pedido c/ st_entrega válido (diferente de cancelado).
#
#	Validar se o que expomos pelo ObterCodigoMarketplace foi informado
#
#


#Estoque: não é um problema.
#
#centro de dsitribuição: o magento tem mas não usamos. Nem vamos expor esse flag.
#
#produtos: sempre virão divididos, nunca vai vir um produto composto.
#
