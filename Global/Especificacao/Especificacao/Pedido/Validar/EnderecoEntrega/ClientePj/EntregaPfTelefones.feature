@ignore
@Pedido
Feature: Pedido de cliente PJ com endereço de entrega PF - validação de telefones
em loja/ClienteEdita.asp:
                /*
                telefones PF:
                EndEtg_ddd_res
                EndEtg_tel_res
                EndEtg_ddd_cel
                EndEtg_tel_cel
                */

Background: Pedido base
	Given Pedido base cliente PJ com endereço de entrega PF


#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_res e EndEtg_tel_res
Scenario: EndEtg_ddd_res
	When Informo "EndEtg_ddd_res" = "1"
	Then Erro "Endereço de entrega: DDD inválido!!"

Scenario: EndEtg_ddd_res 2
	When Informo "EndEtg_ddd_res" = "123"
	Then Erro "Endereço de entrega: DDD inválido!!"

Scenario: EndEtg_tel_res
#	if ((len(s_tel)=0) Or (len(s_tel)>=6)) then telefone_ok = True
	When Informo "EndEtg_tel_res" = "123"
	Then Erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_tel_res 2
	When Informo "EndEtg_tel_res" = "12345"
	Then Erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_tel_res 3
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
	Then Erro "Endereço de entrega: preencha o DDD!!"

Scenario: EndEtg_ddd_res EndEtg_tel_res 2
	When Informo "EndEtg_tel_res" = ""
	And Informo "EndEtg_ddd_res" = "12"
	Then Erro "Endereço de entrega: preencha o telefone!!"

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_cel e EndEtg_tel_cel
Scenario: EndEtg_ddd_cel
	When Informo "EndEtg_ddd_cel" = "1"
	Then Erro "Endereço de entrega: DDD inválido!!"

Scenario: EndEtg_ddd_cel 2
	When Informo "EndEtg_ddd_cel" = "123"
	Then Erro "Endereço de entrega: DDD inválido!!"

Scenario: EndEtg_tel_cel
#	if ((len(s_tel)=0) Or (len(s_tel)>=6)) then telefone_ok = True
	When Informo "EndEtg_tel_cel" = "123"
	Then Erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_tel_cel 2
	When Informo "EndEtg_tel_cel" = "12345"
	Then Erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_tel_cel 3
	When Informo "EndEtg_tel_cel" = "123456"
	Then Sem erro "Endereço de entrega: telefone inválido!!"

Scenario: EndEtg_ddd_cel EndEtg_tel_cel
	When Informo "EndEtg_ddd_cel" = ""
	And Informo "EndEtg_tel_cel" = "123456"
	Then Erro "Endereço de entrega: preencha o DDD do celular."

Scenario: EndEtg_ddd_cel EndEtg_tel_cel 2
	When Informo "EndEtg_tel_cel" = ""
	And Informo "EndEtg_ddd_cel" = "12"
	Then Erro "Endereço de entrega: preencha o número do celular."

#-------------------------------------------------------------
#-------------------------------------------------------------
#-------------------------------------------------------------
#EndEtg_ddd_com e EndEtg_tel_com
#EndEtg_ddd_com_2 e EndEtg_tel_com_2
#estas validações não estão no ASP
Scenario: EndEtg_ddd_com
	When Informo "EndEtg_ddd_com" = "12"
	And Informo "EndEtg_tel_com" = "12345678"
	Then Erro "Endereço de entrega: PJ não pode ter EndEtg_ddd_com_2 (acertar a mensagem)"

Scenario: EndEtg_ddd_com_2
	When Informo "EndEtg_ddd_com" = "12"
	And Informo "EndEtg_tel_com" = "12345678"
	Then Erro "Endereço de entrega: PJ não pode ter EndEtg_ddd_com_2 (acertar a mensagem)"

