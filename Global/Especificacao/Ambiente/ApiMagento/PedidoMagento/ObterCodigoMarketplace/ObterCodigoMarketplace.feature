@Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.ObterCodigoMarketplace
Feature: ObterCodigoMarketplace

Scenario: retorna o que existe
#vamos testar somente se está retornando o nro certo de registros
	#var lstTcodigo = from c in db.TcodigoDescricaos
	#                 where c.Grupo == InfraBanco.Constantes.Constantes.GRUPO_T_CODIGO_DESCRICAO__PEDIDOECOMMERCE_ORIGEM &&
	#                       c.St_Inativo == 0
	#                 select c;
	Then Resposta com número certo de registros
