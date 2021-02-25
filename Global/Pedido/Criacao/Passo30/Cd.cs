using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Criacao.Passo30
{
    partial class Passo30
    {
        private void Cd()
        {
			//todo: passo30 Cd
			/*
@Especificacao.Pedido.Passo30
Scenario: Validar permissão de CD
	Given Pedido base
	And Usuário sem permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "selecao_cd" = "MODO_SELECAO_CD__MANUAL"
	When Informo "c_id_nfe_emitente_selecao_manual" = "1"
	Then Erro "Usuário não tem permissão de especificar o CD"

Scenario: Validar CD escolhido caso manual
	Given Pedido base
	And Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "selecao_cd" = "MODO_SELECAO_CD__MANUAL"
	When Informo "c_id_nfe_emitente_selecao_manual" = "1"
	Then Sem Erro "Usuário não tem permissão de especificar o CD"


Scenario: Validar que escolheu um CD
	#OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD = c_ExibirCamposModoSelecaoCD
	#	if (f.c_ExibirCamposModoSelecaoCD.value=="S")
	#	{
	#		if ((!f.rb_selecao_cd[0].checked)&&(!f.rb_selecao_cd[1].checked))
	#		{
	#			strMsgErro="É necessário informar o modo de seleção do CD (auto-split)!";
	#			alert(strMsgErro);
	#			return;
	#		}
	#
	#		if (f.rb_selecao_cd[1].checked)
	#		{
	#			if (trim(f.c_id_nfe_emitente_selecao_manual.value)=="")
	#			{
	#				strMsgErro="É necessário selecionar o CD que irá atender o pedido (sem auto-split)!";
	#				alert(strMsgErro);
	#				f.c_id_nfe_emitente_selecao_manual.focus();
	#				return;
	#			}
	#		}
	#	}
	#
	Given Pedido base
	And Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "selecao_cd" = "MODO_SELECAO_CD__ um valor inválido"
	Then Erro "É necessário informar o modo de seleção do CD (auto-split)!"

Scenario: Validar que escolheu um CD 2
	Given Pedido base
	And Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "selecao_cd" = "MODO_SELECAO_CD__MANUAL"
	When Informo "c_id_nfe_emitente_selecao_manual" = ""
	Then Erro "É necessário selecionar o CD que irá atender o pedido (sem auto-split)!"


#Se rb_selecao_cd_auto = MODO_SELECAO_CD__AUTOMATICO então não pode escolher c_id_nfe_emitente_selecao_manual
#Se rb_selecao_cd_manual = MODO_SELECAO_CD__MANUAL então tem que escolher c_id_nfe_emitente_selecao_manual
#Valores do c_id_nfe_emitente_selecao_manual:
#strSql = "SELECT" & _
#			" id," & _
#			" apelido," & _
#			" razao_social" & _
#		" FROM t_NFe_EMITENTE" & _
#		" WHERE" & _
#			" (st_ativo <> 0)" & _
#			" AND (st_habilitado_ctrl_estoque <> 0)"

#temos os seguintes c_id_nfe_emitente_selecao_manual: 4001, 4003, 4006, 4903
#como st_ativo = 0: 9999
#como st_habilitado_ctrl_estoque = 0:  9998
Scenario: Validar CD escolhido st_ativo
	Given Pedido base
	And Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "selecao_cd" = "MODO_SELECAO_CD__MANUAL"
	When Informo "c_id_nfe_emitente_selecao_manual" = "9999"
	Then Erro "É necessário selecionar o CD que irá atender o pedido (sem auto-split)!"

Scenario: Validar CD escolhido st_habilitado_ctrl_estoque
	Given Pedido base
	And Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "selecao_cd" = "MODO_SELECAO_CD__MANUAL"
	When Informo "c_id_nfe_emitente_selecao_manual" = "9998"
	Then Erro "É necessário selecionar o CD que irá atender o pedido (sem auto-split)!"

Scenario: Validar CD escolhido 
	Given Pedido base
	And Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "selecao_cd" = "MODO_SELECAO_CD__MANUAL"
	When Informo "c_id_nfe_emitente_selecao_manual" = "4001"
	Then Sem erro "É necessário selecionar o CD que irá atender o pedido (sem auto-split)!"

Scenario: Validar CD escolhido 2
	Given Pedido base
	And Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "selecao_cd" = "MODO_SELECAO_CD__MANUAL"
	When Informo "c_id_nfe_emitente_selecao_manual" = "4003"
	Then Sem erro "É necessário selecionar o CD que irá atender o pedido (sem auto-split)!"

Scenario: Validar CD escolhido 3
	Given Pedido base
	And Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "selecao_cd" = "MODO_SELECAO_CD__MANUAL"
	When Informo "c_id_nfe_emitente_selecao_manual" = "4002"
	Then Erro "É necessário selecionar o CD que irá atender o pedido (sem auto-split)!"


Scenario: MODO_SELECAO_CD__MANUAL exige c_id_nfe_emitente_selecao_manual
#loja/PedidoNovoConsiste.asp
#		if rb_selecao_cd = MODO_SELECAO_CD__MANUAL then
#			id_nfe_emitente_selecao_manual = converte_numero(c_id_nfe_emitente_selecao_manual)
#			if id_nfe_emitente_selecao_manual = 0 then
#				alerta=alerta & "O CD selecionado manualmente é inválido"

#loja/PedidoNovoConfirma.asp
#if rb_selecao_cd = MODO_SELECAO_CD__MANUAL then
#	id_nfe_emitente_selecao_manual = converte_numero(c_id_nfe_emitente_selecao_manual)
#	if id_nfe_emitente_selecao_manual = 0 then
#		alerta=alerta & "O CD selecionado manualmente é inválido"

	Given Pedido base
	When Informo "selecao_cd" = "MODO_SELECAO_CD__MANUAL"
	And Informo "id_nfe_emitente_selecao_manual" = "0"
	Then Erro "O CD selecionado manualmente é inválido"

	Given Pedido base
	When Informo "selecao_cd" = "MODO_SELECAO_CD__MANUAL"
	And Informo "id_nfe_emitente_selecao_manual" = "xxx"
	Then Erro "O CD selecionado manualmente é inválido"

             * */
		}
	}
}
