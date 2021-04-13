@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
@GerenciamentoBanco
Feature: CamposOpcionais
	
Background: Configuracoes
	Given Reiniciar banco ao terminar cenário
	And Limpar tabela "t_CLIENTE"

Scenario: CamposOpcionais - email
	Given Pedido base
	When Informo "OutroEndereco" = "true"
	When Informo "EndEtg_email" = "gabriel.com.br"
	Then Erro "E-mail inválido!"

Scenario: CamposOpcionais - email xml
	Given Limpar tabela "t_CLIENTE"
	Given Pedido base
	When Informo "EndEtg_email_xml" = "gabriel.com.br"
	Then Erro "E-mail XML inválido!"

Scenario: CamposOpcionais - obs1 tamanho
	#loja/PedidoNovoConsiste.asp
	#s = "" + f.c_obs1.value;
	#if (s.length > MAX_TAM_OBS1) {
	#	alert('Conteúdo de "Observações " excede em ' + (s.length-MAX_TAM_OBS1) + ' caracteres o tamanho máximo de ' + MAX_TAM_OBS1 + '!!');
	Given Pedido base
	When Informo "Obs_1" com "501" caracteres
	Then Erro "regex .*Conteúdo de \"Observações\" excede em .*"
	Given Pedido base
	When Informo "Obs_1" com "500" caracteres
	Then Sem nenhum erro

Scenario: CamposOpcionais - c_nf_texto tamanho
	#NA CRIAÇÃO DO PEDIDO NO MAGENTO ESSE CAMPO ESTA SENDO CRIADO COMO NULL
	#loja/PedidoNovoConsiste.asp
	#s = "" + f.c_nf_texto.value;
	#if (s.length > MAX_TAM_NF_TEXTO) {
	#    alert('Conteúdo de "Constar na NF" excede em ' + (s.length-MAX_TAM_NF_TEXTO) + ' caracteres o tamanho máximo de ' + MAX_TAM_NF_TEXTO + '!!');
	Given Pedido base
	When Informo "EnderecoEntrega.PontoReferencia" com "801" caracteres
	Then Erro "regex .*Conteúdo de \"Constar na NF\" excede em.*"