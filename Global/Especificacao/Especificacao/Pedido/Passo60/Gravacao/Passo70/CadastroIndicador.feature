
@Especificacao.Pedido.Passo60
@GerenciamentoBanco
Feature: CadastroIndicador

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
	Given fazer esse teste
#incluir o teste de log e verificar se é incluído o log de cliente ou de pedido