@Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj
Feature: Pedido de cliente PJ com endereço de entrega PJ - validação de telefones

#em loja/ClienteEdita.asp:
#                /*
#                telefones PJ:
#                EndEtg_ddd_com
#                EndEtg_tel_com
#                EndEtg_ramal_com
#                EndEtg_ddd_com_2
#                EndEtg_tel_com_2
#                EndEtg_ramal_com_2
#*/
#também em loja/PedidoNovoConsiste.asp
Background: Api MAgento somente aceita pedidos PF
	#o magento testa isto separadamente, em Ambiente\ApiMagento\PedidoMagento\CadastrarPedido\CriacaoCliente\Pf_Telefones\*.feature
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"
	Given Pedido base cliente PJ com endereço de entrega PJ

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_com e EndEtg_tel_com
Scenario: EndEtg_ddd_com
	When Informo "EndEtg_ddd_com" = "1"
	Then Erro "Endereço de entrega: ddd do telefone comercial inválido!"

Scenario: EndEtg_ddd_com 2
	When Informo "EndEtg_ddd_com" = "123"
	Then Erro "Endereço de entrega: ddd do telefone comercial inválido!"

Scenario: EndEtg_tel_com
	#	if ((len(s_tel)=0) Or (len(s_tel)>=6)) then telefone_ok = True
	When Informo "EndEtg_tel_com" = "123"
	Then Erro "Endereço de entrega: telefone comercial inválido!"

Scenario: EndEtg_tel_com 2
	When Informo "EndEtg_tel_com" = "123456789012"
	Then Erro "Endereço de entrega: telefone comercial inválido!"

Scenario: EndEtg_tel_com 3
	When Informo "EndEtg_tel_com" = "123456"
	Then Sem erro "Endereço de entrega: telefone comercial inválido!"

Scenario: EndEtg_ddd_com EndEtg_tel_com
                if ((f.EndEtg_ddd_com.value == "") && (f.EndEtg_tel_com.value != "")) {
                    alert('Endereço de entrega: preencha o DDD do telefone.');
                    f.EndEtg_ddd_com.focus();
                    return;
                }
	When Informo "EndEtg_ddd_com" = ""
	And Informo "EndEtg_tel_com" = "123456"
	Then Erro "Endereço de entrega: preencha o ddd do telefone comercial!"

Scenario: EndEtg_ddd_com EndEtg_tel_com 2
	When Informo "EndEtg_tel_com" = ""
	And Informo "EndEtg_ddd_com" = "12"
	Then Erro "Endereço de entrega: preencha o telefone comercial!"

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_com_2 e EndEtg_tel_com_2
Scenario: EndEtg_ddd_com_2
	When Informo "EndEtg_ddd_com_2" = "1"
	When Informo "EndEtg_tel_com_2" = "123456"
	Then Erro "Endereço de entrega: ddd do telefone comercial 2 inválido!"

Scenario: EndEtg_ddd_com_2 2
	When Informo "EndEtg_ddd_com_2" = "123"
	When Informo "EndEtg_tel_com_2" = "123456"
	Then Erro "Endereço de entrega: ddd do telefone comercial 2 inválido!"

Scenario: EndEtg_tel_com_2
	#	if ((len(s_tel)=0) Or (len(s_tel)>=6)) then telefone_ok = True
	When Informo "EndEtg_ddd_com_2" = "12"
	When Informo "EndEtg_tel_com_2" = "123"
	Then Erro "Endereço de entrega: telefone comercial 2 inválido!"

Scenario: EndEtg_tel_com_2 2
	When Informo "EndEtg_ddd_com_2" = "12"
	When Informo "EndEtg_tel_com_2" = "123456789012"
	Then Erro "Endereço de entrega: telefone comercial 2 inválido!"

Scenario: EndEtg_tel_com_2 3
	When Informo "EndEtg_tel_com_2" = "123456"
	Then Sem erro "Endereço de entrega: telefone comercial 2 inválido!"

Scenario: EndEtg_ddd_com_2 EndEtg_tel_com_2
	When Informo "EndEtg_ddd_com_2" = ""
	And Informo "EndEtg_tel_com_2" = "123456"
	Then Erro "Endereço de entrega: preencha o ddd do telefone comercial 2!"

Scenario: EndEtg_ddd_com_2 EndEtg_tel_com_2 2
	When Informo "EndEtg_tel_com_2" = ""
	And Informo "EndEtg_ddd_com_2" = "12"
	Then Erro "Endereço de entrega: preencha o telefone comercial 2!"

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ramal_com EndEtg_ramal_com_2
#estas validações não estão no ASP
Scenario: EndEtg_ramal_com
	When Informo "EndEtg_ramal_com" = "12"
	And Informo "EndEtg_ddd_com" = ""
	And Informo "EndEtg_tel_com" = ""
	Then Erro "Endereço de entrega: Ramal do telefone comercial preenchido sem telefone comercial"

Scenario: EndEtg_ramal_com_2
	When Informo "EndEtg_ramal_com_2" = "12"
	And Informo "EndEtg_ddd_com_2" = ""
	And Informo "EndEtg_tel_com_2" = ""
	Then Erro "Endereço de entrega: Ramal do telefone comercial 2 preenchido sem telefone comercial 2!"

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_cel e EndEtg_tel_cel
#EndEtg_ddd_res e EndEtg_tel_res
#estas validações não estão no ASP
Scenario: EndEtg_ddd_cel
	When Informo "EndEtg_ddd_cel" = "12"
	And Informo "EndEtg_tel_cel" = "12345678"
	Then Erro "Endereço de entrega: se tipo pessoa PJ, não pode conter DDD celular e telefone celular!"

Scenario: EndEtg_ddd_res
	When Informo "EndEtg_ddd_res" = "12"
	And Informo "EndEtg_tel_res" = "12345678"
	Then Erro "Endereço de entrega: se tipo pessoa PJ, não pode conter DDD residencial e telefone residencial!"

@ignore
Scenario: nos telefones, os símbolos devem ser removidos
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	When Informo "EndEtg_contribuinte_icms_status" = "1"
	When Informo "EndEtg_tel_com" = "1234-5678"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_tel_com" = "12345678"

@ignore
Scenario: nos telefones, os símbolos devem ser removidos 2
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	When Informo "EndEtg_contribuinte_icms_status" = "1"
	When Informo "EndEtg_tel_com" = "123,.;5678"
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "EndEtg_tel_com" = "1235678"