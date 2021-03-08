@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: CadastroIndicador

Background: Configracao
	Given Ignorar cenário no ambiente "Especificacao.Prepedido.PrepedidoSteps"
	Given Reiniciar banco ao terminar cenário

#loja/PedidoNovoConfirma.asp
#'	INDICADOR: SE ESTE PEDIDO É COM INDICADOR E O CLIENTE AINDA NÃO TEM UM INDICADOR NO CADASTRO, ENTÃO CADASTRA ESTE.
#	if rb_indicacao = "S" then
#		if indicador_original = "" then
#			s="UPDATE t_CLIENTE SET indicador='" & c_indicador & "' WHERE (id='" & cliente_selecionado & "')"
#			cn.Execute(s)
#			s_log_cliente_indicador = "Cadastrado o indicador '" & c_indicador & "' no cliente id=" & cliente_selecionado
#			end if
#		end if
@ignore
Scenario: CadastroIndicador
	Given Tabela t_CLIENTE registro com cpf_cnpj = "35270445824" alterar campo "indicador" = ""
	Given Pedido base
	When Informo "DadosCliente.Indicador_Orcamentista" = "POLITÉCNIC"
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "35270445824", verificar campo "indicador" = "POLITÉCNIC"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "indicador" = "POLITÉCNIC"

@ignore
Scenario: Log indicador
	#Given Tabela t_CLIENTE registro com cpf_cnpj = "14039603052" alterar campo "indicador" = ""
	Given Pedido base
	When Informo "Frete" = "10.00"
	Then Sem nenhum erro
	And Tabela "t_LOG" pedido gerado e operacao = "OP_LOG_PEDIDO_NOVO", verificar campo "complemento" = "cadastrado o indicador 'frete' no cliente id=000000645637;"

Scenario: CadastroIndicador - magento
	Given Ignorar cenário no ambiente "Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"
	Given Pedido base
	When Informo "Frete" = "10.00"
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "14039603052", verificar campo "indicador" = "FRETE"
	And Tabela "t_PEDIDO" registro pai criado, verificar campo "indicador" = "FRETE"