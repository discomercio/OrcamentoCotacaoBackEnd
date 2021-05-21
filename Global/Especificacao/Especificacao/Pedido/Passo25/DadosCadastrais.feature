@Especificacao.Pedido.PedidoFaltandoImplementarSteps
#@ignore
Feature: PedidoDadosCadastraisVerificarQueExecutou - VerificarQueExecutou

#Precisamos verificar os dados cadastrais
#Quando chama pelo ASP ou pela Loja, esta verificação não precisa ser feita porque vamos usar os dados do cliente já cadastrado.
#Mas pela API precisa desta verificação
#todo: afazer: fazer estas validações
#Scenario: PedidoDadosCadastraisVerificarQueExecutou Configuração
#	Given Nome deste item "Especificacao.Pedido.Passo25.DadosCadastrais"
#	Given Implementado em "Especificacao.Pedido.Pedido"
#	Then Fazer

Scenario: fazer
	Given fazer esta validação, no pedido e prepedido

#if alerta = "" then
#	if ( (EndCob_tipo_pessoa = ID_PF) And (Cstr(EndCob_produtor_rural_status) = Cstr(COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)) ) _
#		Or _
#		( (EndCob_tipo_pessoa = ID_PJ) And (Cstr(EndCob_contribuinte_icms_status) = Cstr(COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)) ) _
#		Or _
#		( (EndCob_tipo_pessoa = ID_PJ) And (Cstr(EndCob_contribuinte_icms_status) = Cstr(COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)) And (EndCob_ie <> "") ) then
#		if Not isInscricaoEstadualValida(EndCob_ie, EndCob_uf) then
#			alerta=texto_add_br(alerta)
#			alerta=alerta & "Preencha a IE (Inscrição Estadual) com um número válido!" & _
#					"<br>" & "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE."
#			end if
#		end if
Scenario: Validar IE de Endereco Cadastral para PF
	
	Given Pedido base cliente PF
	When Informo "DadosCliente.Ie" = "829514-56"
	Then Erro "Preencha a IE (Inscrição Estadual) com um número válido! Certifique-se de que a UF do endereço de entrega corresponde à UF responsável pelo registro da IE."

	Given Pedido base cliente PF
	When Informo "DadosCliente.Uf" = ""
	Then Erro "UF (estado) vazio! "

Scenario: Validar IE de Endereco Cadastral para PJ
	#Contribuinte sim
	Given Pedido base cliente PJ
	When Informo "DadosCliente.ProdutorRural" = "0"
	When Informo "DadosCliente.Contribuinte_Icms_Status" = "2"
	When Informo "DadosCliente.Ie" = "829514-56"
	Then Erro "Preencha a IE (Inscrição Estadual) com um número válido! Certifique-se de que a UF do endereço de entrega corresponde à UF responsável pelo registro da IE."

	Given Pedido base cliente PJ
	When Informo "DadosCliente.ProdutorRural" = "0"
	When Informo "DadosCliente.Contribuinte_Icms_Status" = "2"
	When Informo "DadosCliente.Ie" = "749.201.682.501"
	When Informo "DadosCliente.Uf" = ""
	Then Erro "UF (estado) vazio! "
	#Contribuinte não
	Given Pedido base cliente PJ
	When Informo "DadosCliente.ProdutorRural" = "0"
	When Informo "DadosCliente.Contribuinte_Icms_Status" = "1"
	When Informo "DadosCliente.Ie" = "829514-56"
	Then Erro "Preencha a IE (Inscrição Estadual) com um número válido! Certifique-se de que a UF do endereço de entrega corresponde à UF responsável pelo registro da IE."

	Given Pedido base cliente PJ
	When Informo "DadosCliente.ProdutorRural" = "0"
	When Informo "DadosCliente.Contribuinte_Icms_Status" = "1"
	When Informo "DadosCliente.Ie" = "749.201.682.501"
	When Informo "DadosCliente.Uf" = ""
	Then Erro "UF (estado) vazio! "

#if alerta="" then
#'	MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?
#	dim s_lista_sugerida_municipios
#	if Not consiste_municipio_IBGE_ok(EndCob_cidade, EndCob_uf, s_lista_sugerida_municipios, msg_erro) then
#		if alerta <> "" then alerta = alerta & "<br><br>" & String(80,"=") & "<br><br>"
#		if msg_erro <> "" then
#			alerta = alerta & msg_erro
#		else
#			alerta = alerta & "Município '" & EndCob_cidade & "' não consta na relação de municípios do IBGE para a UF de '" & EndCob_uf & "'!!"
#			if s_lista_sugerida_municipios <> "" then
#				alerta = alerta & "<br>" & _
#									"Localize o município na lista abaixo e verifique se a grafia está correta!!"
#				end if
#			end if
#		end if
Scenario: Validar Endereco com IBGE
	Given Pedido base
	When Informo "DadosCliente.Cep" = "68912350"
	When Informo "DadosCliente.Cidade" = "Abacate da Pedreira"
	Then Erro "Cidade não está no IBGE (acertar a mensagem)"

	Given Pedido base
	When Informo "DadosCliente.Cep" = "68912350"
	When Informo "DadosCliente.Cidade" = "Amapá"
	Then Sem nenhum erro

Scenario: Validar Endereco com IBGE 2
	Given Pedido base
	When Informo "DadosCliente.Cep" = "04321001"
	When Informo "DadosCliente.Cidade" = "Santo André"
	Then Erro "Cidade inconsistente"