@Especificacao.Pedido.PedidoFaltandoImplementarSteps
#ignorando porque naõ se aplica ao magento e na loja vamos verificar quando implementar
#@Especificacao.Pedido.Passo60
Feature: origem_pedido


Background: não executar no prepedido
	#ignoramos no prepedido inteiro
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	#no magento é testado separadamente
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

Scenario: somente para resolver a lista de dependencias
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

Scenario: Validar origem_pedido
	#loja/PedidoNovoConsiste.asp
	#	set r = cn.Execute("SELECT * FROM t_CODIGO_DESCRICAO WHERE (grupo='PedidoECommerce_Origem') AND (st_inativo=0) ORDER BY ordenacao")
	#	if ($("#c_loja").val()==NUMERO_LOJA_ECOMMERCE_AR_CLUBE){
	#		if ($("#c_origem_pedido").val() == ""){
	#			alert("Selecione a origem do pedido (marketplace)!");
	#			$("#c_origem_pedido").focus();
	#			return;
	#		}
	#o campo origem_pedido precisa ser um desses (se loja NUMERO_LOJA_ECOMMERCE_AR_CLUBE, ele é exigido)
	Given Pedido base
	When Informo "DadosCliente.Loja" = "201"
	When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = ""
	Then Erro "Selecione a origem do pedido (marketplace)!"
	Given Pedido base
	When Informo "DadosCliente.Loja" = "201"
	#"OrigemPedido" = "001" => Arclube (e-commerce)
	#"OrigemPedido" = "002" => Arclube (televendas)
	#"OrigemPedido" = "003" => Americanas
	#"OrigemPedido" = "004" => Submarino
	# inclui esses códigos caso tenha que ser alterado
	When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "001"
	Then Sem nenhum erro


Scenario: Validar origem_pedido2
	#		if ($("#c_pedido_ac").val() != "") {
	#		    if(retorna_so_digitos($("#c_pedido_ac").val()) != $("#c_pedido_ac").val()) {
	#		        alert("O número Magento deve conter apenas dígitos!");
	#		        $("#c_pedido_ac").focus();
	#		        return;
	#		    }
	#		}
	#	}
	Given Pedido base
	When Informo "DadosCliente.Loja" = "201"
	When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "001"
	When Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "1234567AA"
	Then Erro "O número Magento deve conter apenas dígitos!"


Scenario: Validar origem_pedido3
	#	if (FLAG_MAGENTO_PEDIDO_COM_INDICADOR)
	#	{
	#		if ($("#c_pedido_ac").val() != "") {
	#			if(retorna_so_digitos($("#c_pedido_ac").val()) != $("#c_pedido_ac").val()) {
	#				alert("O número Magento deve conter apenas dígitos!");
	#				$("#c_pedido_ac").focus();
	#				return;
	#			}
	#		}
	#	}
	#
	#verificar se o indicador é inserido de forma automática no magento
	Given Pedido base
	When Informo "DadosCliente.Loja" = "201"
	When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "001"
	When Informo "InfCriacaoPedido.Pedido_bs_x_ac" = "1234567AA"
	Then Erro "O número Magento deve conter apenas dígitos!"


Scenario: Validar origem_pedido4
	Given Pedido base
	When Informo "DadosCliente.Loja" = "201"
	When Informo "InfCriacaoPedido.Marketplace_codigo_origem" = "001"
	When Informo "InfCriacaoPedido.Pedido_bs_x_ac" = ""
	Then Erro "ver qual o erro"

