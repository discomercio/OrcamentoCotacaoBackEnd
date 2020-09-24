@ignore
Feature: Validação do CD
todo: terminar Validação do CD

Background: Reiniciar banco
	Given Reiniciar permissões do usuário quando terminar o teste

Scenario: Configuração
	Given Nome deste item "Especificacao.Pedido.Passo30.CD"
	Given Implementado em "Especificacao.Pedido.Pedido"
	And Fim da configuração

Scenario: Lista de CDs disponíveis
	Given afazer todo: terminar de fazer

Scenario: Validar permissão de CD
	Given Pedido base
	And Usuário sem permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "c_id_nfe_emitente_selecao_manual" = "1"
	Then Erro "Usuário não tem permissão de especificar o CD

Scenario: Validar CD escolhido caso manual
	Given Pedido base
	And Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD
	When Informo "c_id_nfe_emitente_selecao_manual" = "1"
	Then Sem Erro "Usuário não tem permissão de especificar o CD


Scenario: Validar que escolheu um CD
	When Fazer esta validação

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

Scenario: Validar CD escolhido
	When Fazer esta validação
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
	When informo "selecao_cd" = "MODO_SELECAO_CD__MANUAL"
	And informo "id_nfe_emitente_selecao_manual" = "0"
	Then Erro "O CD selecionado manualmente é inválido"
