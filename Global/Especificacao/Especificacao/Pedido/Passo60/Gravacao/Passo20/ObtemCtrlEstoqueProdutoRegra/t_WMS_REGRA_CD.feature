@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: t_WMS_REGRA_CD

Background: Configuracao
	Given Reiniciar banco ao terminar cenário

#	SELECT * FROM t_WMS_REGRA_CD WHERE id = id_wms_regra_cd
#		se nenhum registro, erro
#		se mais de um registro, erro (não está no ASP)
#Não podemos fazer o teste para duplicar porque fabricante e produto são chaves
Scenario: t_WMS_REGRA_CD - sem regra
	Given Tabela "t_WMS_REGRA_CD" apagar registro do fabricante = "003" e produto = "003220"
	Given Pedido base
	Then Erro "Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': regra associada ao produto (003)003220 não foi localizada no banco de dados (Id=5)"
	#mensagem esperada esta sendo alterada??
	#Then Erro "Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': regra associada ao produto (003)003220 não está cadastrada para a UF 'SP' (Id=5)"