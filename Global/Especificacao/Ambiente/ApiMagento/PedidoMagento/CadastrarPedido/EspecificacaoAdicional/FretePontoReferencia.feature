@ignore
Feature: FretePontoReferencia

Scenario: campo "frete" -> se for <> 0, vamos usar o indicador. se for 0, sem indicador
	#Frete/RA
	#Valor de Frete: analisar se há valor de frete para definir se o pedido terá RA ou não.
	#Se houver frete, deve-se automaticamente informar que o pedido possui RA e selecionar o indicador 'FRETE'.
	When Fazer esta validação

Scenario: campo "frete" salvo em t_PEDIDO.vl_frete
	#O valor do frete no momento não estamos tratando no sistema, mas acho que podemos salvar a informação no campo t_PEDIDO.vl_frete
	#Por enquanto, todos os registros estão c/ esse valor zerado e o campo não é usado em nenhum lugar, mas já que temos
	#a possibilidade de salvar essa informação, creio que deveríamos gravar nesse campo
	When Fazer esta validação

Scenario: Ponto de Referência
	#Colocar a informação do ponto de referência no campo 'Constar na NF'. Comparar o conteúdo do ponto de referência
	#com o campo complemento. Se forem iguais, não colocar em 'Constar na NF'. Se o campo complemento exceder o
	#tamanho do BD e precisar ser truncado, copiá-lo no campo 'Constar na NF', junto com o ponto de referência.
	#Campo t_pedido.NFe_texto_constar
	When Fazer esta validação


