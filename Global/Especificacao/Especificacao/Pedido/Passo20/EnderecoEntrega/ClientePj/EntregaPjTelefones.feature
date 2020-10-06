@ignore
@Especificacao/Pedido
Feature: Pedido de cliente PJ com endereço de entrega PJ - validação de telefones
em loja/ClienteEdita.asp:
                /*
                telefones PJ:
                EndEtg_ddd_com
                EndEtg_tel_com
                EndEtg_ramal_com
                EndEtg_ddd_com_2
                EndEtg_tel_com_2
                EndEtg_ramal_com_2
*/

Background: Pedido base
	Given Pedido base cliente PJ com endereço de entrega PJ

Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj.EntregaPjTelefones"
	Given Implementado em "Especificacao.Pedido.Pedido"
	And Fim da configuração


#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_com e EndEtg_tel_com
Scenario: EndEtg_ddd_com
	When Informo "EndEtg_ddd_com" = "1"
	Then Erro "Endereço de entrega: DDD inválido!!"

Scenario: EndEtg_ddd_com 2
	When Informo "EndEtg_ddd_com" = "123"
	Then Erro "Endereço de entrega: DDD inválido!!"

Scenario: EndEtg_tel_com
#	if ((len(s_tel)=0) Or (len(s_tel)>=6)) then telefone_ok = True
	When Informo "EndEtg_tel_com" = "123"
	Then Erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_tel_com 2
	When Informo "EndEtg_tel_com" = "12345"
	Then Erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_tel_com 3
	When Informo "EndEtg_tel_com" = "123456"
	Then Sem erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_ddd_com EndEtg_tel_com
                if ((f.EndEtg_ddd_com.value == "") && (f.EndEtg_tel_com.value != "")) {
                    alert('Endereço de entrega: preencha o DDD do telefone.');
                    f.EndEtg_ddd_com.focus();
                    return;
                }
	When Informo "EndEtg_ddd_com" = ""
	And Informo "EndEtg_tel_com" = "123456"
	Then Erro "Endereço de entrega: preencha o DDD do telefone."

Scenario: EndEtg_ddd_com EndEtg_tel_com 2
	When Informo "EndEtg_tel_com" = ""
	And Informo "EndEtg_ddd_com" = "12"
	Then Erro "Endereço de entrega: preencha o telefone."

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_com_2 e EndEtg_tel_com_2
Scenario: EndEtg_ddd_com_2
	When Informo "EndEtg_ddd_com_2" = "1"
	Then Erro "Endereço de entrega: DDD inválido!!"

Scenario: EndEtg_ddd_com_2 2
	When Informo "EndEtg_ddd_com_2" = "123"
	Then Erro "Endereço de entrega: DDD inválido!!"

Scenario: EndEtg_tel_com_2
#	if ((len(s_tel)=0) Or (len(s_tel)>=6)) then telefone_ok = True
	When Informo "EndEtg_tel_com_2" = "123"
	Then Erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_tel_com_2 2
	When Informo "EndEtg_tel_com_2" = "12345"
	Then Erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_tel_com_2 3
	When Informo "EndEtg_tel_com_2" = "123456"
	Then Sem erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_ddd_com_2 EndEtg_tel_com_2
	When Informo "EndEtg_ddd_com_2" = ""
	And Informo "EndEtg_tel_com_2" = "123456"
	Then Erro "Endereço de entrega: preencha o DDD do telefone."

Scenario: EndEtg_ddd_com_2 EndEtg_tel_com_2 2
	When Informo "EndEtg_tel_com_2" = ""
	And Informo "EndEtg_ddd_com_2" = "12"
	Then Erro "Endereço de entrega: preencha o telefone."

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ramal_com EndEtg_ramal_com_2
#estas validações não estão no ASP
Scenario: EndEtg_ramal_com
	When Informo "EndEtg_ramal_com" = "12"
	And Informo "EndEtg_ddd_com" = ""
	And Informo "EndEtg_tel_com" = ""
	Then Erro "Endereço de entrega: ramal sem telefone (acertar a mensagem)"

Scenario: EndEtg_ramal_com_2
	When Informo "EndEtg_ramal_com_2" = "12"
	And Informo "EndEtg_ddd_com_2" = ""
	And Informo "EndEtg_tel_com_2" = ""
	Then Erro "Endereço de entrega: ramal 2 sem telefone (acertar a mensagem)"

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_cel e EndEtg_tel_cel
#EndEtg_ddd_res e EndEtg_tel_res
#estas validações não estão no ASP
Scenario: EndEtg_ddd_cel
	When Informo "EndEtg_ddd_cel" = "12"
	And Informo "EndEtg_tel_cel" = "12345678"
	Then Erro "Endereço de entrega: PJ não pode ter DDDEndEtg_ddd_cel (acertar a mensagem)"

Scenario: EndEtg_ddd_res
	When Informo "EndEtg_ddd_res" = "12"
	And Informo "EndEtg_tel_res" = "12345678"
	Then Erro "Endereço de entrega: PJ não pode ter EndEtg_ddd_res (acertar a mensagem)"

Scenario: nos telefones, os símbolos devem ser removidos
	When Fazer esta validação

