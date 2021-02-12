
@Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj
Feature: Pedido de cliente PJ com endereço de entrega PF - validação de telefones

#em loja/ClienteEdita.asp:
#                /*
#                telefones PF:
#                EndEtg_ddd_res
#                EndEtg_tel_res
#                EndEtg_ddd_cel
#                EndEtg_tel_cel
#                */
#também em loja/PedidoNovoConsiste.asp
Background: Api MAgento somente aceita pedidos PF
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base cliente PJ com endereço de entrega PF

#@ignore
#Scenario: Configuração
#	Given Nome deste item "Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj.EntregaPfTelefones"
#	Given Implementado em "Especificacao.Pedido.Pedido"

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_res e EndEtg_tel_res
Scenario: EndEtg_ddd_res
	When Informo "EndEtg_ddd_res" = "1"
	Then Erro "Endereço de entrega: ddd residencial inválido."

Scenario: EndEtg_ddd_res 2
	When Informo "EndEtg_ddd_res" = "123"
	Then Erro "Endereço de entrega: ddd residencial inválido."


Scenario: EndEtg_tel_res
	#	if ((len(s_tel)=0) Or (len(s_tel)>=6)) then telefone_ok = True
	When Informo "EndEtg_tel_res" = "123"
	Then Erro "Endereço de entrega: telefone residencial inválido."


Scenario: EndEtg_tel_res 2
	When Informo "EndEtg_tel_res" = "12345"
	Then Erro "Endereço de entrega: telefone residencial inválido."


Scenario: EndEtg_tel_res 3
	When Informo "EndEtg_tel_res" = "123456789012"
	Then Erro "Endereço de entrega: telefone residencial inválido."

Scenario: EndEtg_tel_res 4
	When Informo "EndEtg_tel_res" = "123456"
	Then Sem erro "Endereço de entrega: telefone inválido!!"


Scenario: EndEtg_ddd_res EndEtg_tel_res
                if ((trim(f.EndEtg_ddd_res.value) != "") || (trim(f.EndEtg_tel_res.value) != "")) {
                    if (trim(f.EndEtg_ddd_res.value) == "") {
                        alert('Endereço de entrega: preencha o DDD!!');
                        f.EndEtg_ddd_res.focus();
                        return;
                    }
	When Informo "EndEtg_ddd_res" = ""
	And Informo "EndEtg_tel_res" = "123456"
	Then Erro "Endereço de entrega: preencha o ddd residencial."


Scenario: EndEtg_ddd_res EndEtg_tel_res 2
	When Informo "EndEtg_tel_res" = ""
	And Informo "EndEtg_ddd_res" = "12"
	Then Erro "Endereço de entrega: preencha o telfone residencial."

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_cel e EndEtg_tel_cel
Scenario: EndEtg_ddd_cel
	When Informo "EndEtg_ddd_cel" = "1"
	Then Erro "Endereço de entrega: ddd do celular inválido."

Scenario: EndEtg_ddd_cel 2
	When Informo "EndEtg_ddd_cel" = "123"
	Then Erro "Endereço de entrega: ddd do celular inválido."


Scenario: EndEtg_tel_cel
	#	if ((len(s_tel)=0) Or (len(s_tel)>=6)) then telefone_ok = True
	When Informo "EndEtg_tel_cel" = "123"
	Then Erro "Endereço de entrega: telefone celular inválido."


Scenario: EndEtg_tel_cel 2
	When Informo "EndEtg_tel_cel" = "12345"
	Then Erro "Endereço de entrega: telefone celular inválido."


Scenario: EndEtg_tel_cel 3
	When Informo "EndEtg_tel_cel" = "123456789012"
	Then Erro "Endereço de entrega: telefone celular inválido."

Scenario: EndEtg_tel_cel 4
	When Informo "EndEtg_tel_cel" = "123456"
	Then Sem erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_ddd_cel EndEtg_tel_cel
	When Informo "EndEtg_ddd_cel" = ""
	And Informo "EndEtg_tel_cel" = "123456"
	Then Erro "Endereço de entrega: preencha o DDD do celular."

Scenario: EndEtg_ddd_cel EndEtg_tel_cel 2
	When Informo "EndEtg_tel_cel" = ""
	And Informo "EndEtg_ddd_cel" = "12"
	Then Erro "Endereço de entrega: preencha o telefone do celular."

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_com e EndEtg_tel_com
#EndEtg_ddd_com_2 e EndEtg_tel_com_2
#estas validações não estão no ASP
Scenario: EndEtg_ddd_com
	When Informo "EndEtg_ddd_com" = "12"
	And Informo "EndEtg_tel_com" = "12345678"
	Then Erro "Endereço de entrega: se tipo pessoa PF, não pode conter telefone comercial."


Scenario: EndEtg_ddd_com_2
	When Informo "EndEtg_ddd_com" = "12"
	And Informo "EndEtg_tel_com" = "12345678"
	Then Erro "Endereço de entrega: se tipo pessoa PF, não pode conter telefone comercial."


Scenario: nos telefones, os símbolos devem ser removidos
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	When Informo "EndEtg_contribuinte_icms_status" = "0"
	When Informo "EndEtg_ie" = ""
	When Informo "EndEtg_tel_cel" = "1234-5678"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_tel_cel" = "12345678"


Scenario: nos telefones, os símbolos devem ser removidos 2
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	When Informo "EndEtg_contribuinte_icms_status" = "0"
	When Informo "EndEtg_ie" = ""
	When Informo "EndEtg_tel_cel" = "123,.;5678"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_tel_cel" = "1235678"