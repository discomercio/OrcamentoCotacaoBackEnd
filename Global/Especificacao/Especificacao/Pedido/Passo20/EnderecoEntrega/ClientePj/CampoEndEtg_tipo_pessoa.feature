@ignore
@Especificacao/Pedido
@Ambiente/PrepedidoApi
@Ambiente/ApiUnis
Feature: Pedido de cliente PJ com endereço de entrega

Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj.CampoEndEtg_tipo_pessoa"
	Given Implementado em "Especificacao.Pedido.Pedido"

Scenario: Validar tipo de pessoa 1
#em loja/ClienteEdita.asp:
#var EndEtg_tipo_pessoa = $('input[name="EndEtg_tipo_pessoa"]:checked').val();
#loja/PedidoNovoConsiste.asp
                #if EndEtg_tipo_pessoa <> "PJ" and EndEtg_tipo_pessoa <> "PF" then
                #    alerta = "Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!!"

	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = ""
	Then Erro "Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!!"

Scenario: Validar tipo de pessoa 2
	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = "XX"
	Then Erro "Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!!"

Scenario: Validar tipo de pessoa 3
	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = "PJ"
	Then Sem erro "Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!!"

Scenario: Validar tipo de pessoa 4
	Given Pedido base cliente PJ com endereço de entrega
	When Informo "EndEtg_tipo_pessoa" = "PF"
	Then Sem erro "Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!!"

