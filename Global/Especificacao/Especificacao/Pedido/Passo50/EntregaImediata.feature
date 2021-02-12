@Especificacao.Pedido.PedidoFaltandoImplementarSteps
Feature: EntregaImediata

Background:
	Given Ignorar cenário no ambiente "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido"

Scenario: EntregaImediata
	#loja/PedidoNovoConsiste.asp
	#if (!blnFlag) {
	#	alert('Selecione uma opção para o campo "Entrega Imediata"');
	Given Pedido base
	When Informo "EntregaImediata" = ""
	Then Erro "Selecione uma opção para o campo "Entrega Imediata"

Scenario: PrevisaoEntrega
	#loja/PedidoNovoConsiste.asp
	#if (f.rb_etg_imediata[0].checked)
	#{
	#	if (trim(f.c_data_previsao_entrega.value) == "") {
	#		alert("Informe a data de previsão de entrega!");
	Given Pedido base
	When Informo "EntregaImediata" = "true"
	When Informo "PrevisaoEntrega" = ""
	Then Erro "Informe a data de previsão de entrega!"
	#if (retorna_so_digitos(formata_ddmmyyyy_yyyymmdd(f.c_data_previsao_entrega.value)) <= retorna_so_digitos(formata_ddmmyyyy_yyyymmdd('<%=formata_data(Date)%>'))) {
	#	alert("Data de previsão de entrega deve ser uma data futura!");
	Given Pedido base
	When Informo "etg_imediata" = "true"
	When Informo "PrevisaoEntrega" = "hoje"
	Then Erro "Data de previsão de entrega deve ser uma data futura!"
	Given Pedido base
	When Informo "etg_imediata" = "true"
	When Informo "PrevisaoEntrega" = "amanhã"
	Then Sem nenhum erro