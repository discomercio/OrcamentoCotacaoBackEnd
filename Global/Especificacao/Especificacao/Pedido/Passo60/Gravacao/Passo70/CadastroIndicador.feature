@ignore
@Especificacao.Pedido.Passo60
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

Scenario: CadastroIndicador
	Given t_CLIENTE registro com "cpf_cnpj" = "352.704.458-24" alterar campo "indicador" = ""
	When Pedido base 
	When Informo "DadosCliente.Indicador_Orcamentista" = "POLITÉCNIC"
	Then Sem nenhum erro
	And Tabela "t_CLIENTE" registro com campo "cnpj_cpf" = "352.704.458-24", verificar campo "indicador" = "POLITÉCNIC"
	And Tabela "t_PEDIDO" registro criado, verificar campo "indicador" = "POLITÉCNIC"

@ignore
Scenario: Log indicador
	Given fazer esse teste
#incluir o teste de log e verificar se é incluído o log de cliente ou de pedido