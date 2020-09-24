@ignore
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
	When Fazer esta validação

