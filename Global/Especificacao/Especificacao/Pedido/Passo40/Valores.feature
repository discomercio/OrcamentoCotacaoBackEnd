@ignore
Feature: Valores

Background: Limita o RA a um percentual do valor do pedido
	Given Pedido base
	Given Reiniciar banco ao terminar cenário
	Given Atualizar tabela "t_CONTROLE", campo "id" = "PercVlPedidoLimiteRA", alterar "nsu" para "10"
	When Informo "total_RA" = "especial: metade do total do pedido"

#loja/PedidoNovoConsiste.asp
#//	Limita o RA a um percentual do valor do pedido
#	if (converte_numero(f.c_PercVlPedidoLimiteRA.value)!=0) {
#		vlAux = (converte_numero(f.c_PercVlPedidoLimiteRA.value)/100) * converte_numero(f.c_total_geral.value);
#		if (($("#c_loja").val()!=NUMERO_LOJA_ECOMMERCE_AR_CLUBE)&&(!FLAG_MAGENTO_PEDIDO_COM_INDICADOR)){
#			if (converte_numero(f.c_total_RA.value) > vlAux) {
#				alert('O valor total de RA excede o limite permitido para este pedido!!');
#				return;
#			}
#		}
#	}

#	c_PercVlPedidoLimiteRA = obtem_PercVlPedidoLimiteRA()
#function obtem_PercVlPedidoLimiteRA
#dim msg_erro
#	obtem_PercVlPedidoLimiteRA = converte_numero(le_parametro_bd(ID_PARAM_PercVlPedidoLimiteRA, msg_erro))
#end function
#function le_parametro_bd(byval id_nsu, byref msg_erro)
#	s = "SELECT nsu FROM t_CONTROLE WHERE (id_nsu = '" & id_nsu & "')"
#Const ID_PARAM_PercVlPedidoLimiteRA					= "PercVlPedidoLimiteRA"			' NESTE CASO, O REGISTRO É USADO P/ ARMAZENAR UM PARÂMETRO E NÃO P/ GERAR UM NSU!!

#Const NUMERO_LOJA_ECOMMERCE_AR_CLUBE = "201"
#Const NUMERO_LOJA_OLD03 = "300"
#Const NUMERO_LOJA_OLD03_BONIFICACAO = "301"
#Const NUMERO_LOJA_OLD03_ASSISTENCIA = "302"
#Const NUMERO_LOJA_MARCELO_ARTVEN = "305"
#Const NUMERO_LOJA_TRANSFERENCIA = "01"
#Const NUMERO_LOJA_KITS = "02"

Scenario: loja 301
	Given Loja atual = "301"
	Then Erro "O valor total de RA excede o limite permitido para este pedido!!"

Scenario: loja NUMERO_LOJA_ECOMMERCE_AR_CLUBE
	Given Loja atual = "201"
	Then Sem nenhum erro

Scenario: loja 301 sem RA
	Given Loja atual = "301"
	When Informo "total_RA" = "especial: total do pedido"
	Then Sem nenhum erro

Scenario: validar Desc_Dado
	#verificar se o Desc_Dado está consistente com o Preco_NF, Preco_Lista, Preco_Venda
	Given fazer esta validacao
