﻿@ignore
Feature: origem_pedido

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
	When Fazer esta validação

Scenario: Validar origem_pedido2
	#loja/PedidoNovoConsiste.asp
	#		if ($("#c_loja").val()==NUMERO_LOJA_ECOMMERCE_AR_CLUBE){
	#		if ($("#c_origem_pedido").val() == ""){
	#			alert("Selecione a origem do pedido (marketplace)!");
	#			$("#c_origem_pedido").focus();
	#			return;
	#		}
	#
	#		if ($("#c_pedido_ac").val() != "") {
	#		    if(retorna_so_digitos($("#c_pedido_ac").val()) != $("#c_pedido_ac").val()) {
	#		        alert("O número Magento deve conter apenas dígitos!");
	#		        $("#c_pedido_ac").focus();
	#		        return;
	#		    }
	#		}
	#	}
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
	When Fazer esta validação