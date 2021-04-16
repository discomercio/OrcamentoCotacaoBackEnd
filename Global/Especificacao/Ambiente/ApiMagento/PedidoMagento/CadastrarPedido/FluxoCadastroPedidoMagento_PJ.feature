Feature: FLuxoCadastroPedidoMagento - PJ
#
#Resumo: magento não aceita PJ
#
#magento: problema no cadastro de PJ, vai puxar do estoque errado se for contribuinte de ICMS.
#Hoje não usa, mas é importante ter o recurso.
#O problema é: se a gente presumir o ICMS da PJ, vamos criar o pedido pegando do estoque errado.
#Hamilton vai conversar com Karina para saber como funciona. Mas é um BELO problema.
#
#Boa tarde
#Edu  conversei com a Karina e ficou decidido que neste primeiro momento a integração com o Magento 
#não irá tratar os pedidos de clientes PJ. Esses pedidos continuarão sendo cadastrados através do 
#processo semi-automático. Então creio que seria melhor fazer normalmente a validação do campo de 
#contribuinte ICMS para rejeitar os pedidos que vierem sem essa informação p/ garantir a consistência 
#dos dados caso seja enviado um pedido de cliente PJ.
#
#Conversei com o time e pegando alguns pontos que eles comentaram é melhor seguir com semi-automático mesmo e no futuro se surgir alguma ideia ou solução a gente adapta. 
#
#Resumo: API do Magento para PJ não aceita nenhum pedido, tods serão feitos no semi-automático

@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
Scenario: Fluxo de cadastro do magento - PJ - não é aceito
	Given Pedido base cliente PJ
	Then Erro "A API Magento somente aceita pedidos para PF (EnderecoCadastralCliente.Endereco_tipo_pessoa)."



Scenario: Fluxo de cadastro do magento - PJ - para quando for implementado
#
#Ao cadastrar o cliente:
#- se for PJ, deixar o pedido st_etg_imediata = 1 (não)
#	e colocar Endereco_contribuinte_icms_status = inicial, Endereco_ie = vazio
#Entrega Imediata
#Se o cliente for PJ, sempre colocar com entrega imediata NÃO
#
#Endereço
#Se o cliente for PJ, será feita a comparação do endereço de entrega com o endereço de cobrança. Se forem iguais, descartar o endereço de entrega. Se forem diferentes, assumir que há endereço de entrega a ser usado.
#
#Se for PJ, se endereço de entrega for igual do endeereço de cobrança, apaga o endereço de entrega.
#	Comparação de endereço: qualquer campo diferente é diferente (inclusive complemento)
#Depois disso, se o cliente não existir, cadastrar com o endereço de cobrança
#
#Contribuinte ICMS
#Para cliente PJ, quando o cliente for cadastrado automaticamente, manter o campo contribuinte_icms_status com o status default (zero).
#
#centro de dsitribuição: o magento tem mas não usamos. Nem vamos expor esse flag.
#
