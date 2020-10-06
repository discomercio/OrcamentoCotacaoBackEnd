@Especificacao.Pedido.Passo20.EnderecoEntrega
Feature: Validar endereco de entrega


Background: Pedido base com endereço de entrega (pedido e prepedido)
	Given Pedido base com endereço de entrega

	# Configuração
	Given No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido" erro "Selecione a justificativa do endereço de entrega!!" é "Código da justficativa inválida!"
	Given No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido" erro "Preencha o endereço de entrega!!" é "PREENCHA O ENDEREÇO DE ENTREGA."
	Given No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido" erro "Preencha o número do endereço de entrega!!" é "PREENCHA O NÚMERO DO ENDEREÇO DE ENTREGA."
	Given No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido" erro "Preencha o bairro do endereço de entrega!!" é "PREENCHA O BAIRRO DO ENDEREÇO DE ENTREGA."
	Given No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido" erro "Preencha a cidade do endereço de entrega!!" é "PREENCHA A CIDADE DO ENDEREÇO DE ENTREGA."
	Given No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido" erro "UF inválida no endereço de entrega!!" é "UF INVÁLIDA NO ENDEREÇO DE ENTREGA."
	Given No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido" erro "CEP inválido no endereço de entrega!!" é "CEP INVÁLIDO NO ENDEREÇO DE ENTREGA."
	Given No ambiente "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido" erro "Informe o CEP do endereço de entrega!!" é "CEP INVÁLIDO NO ENDEREÇO DE ENTREGA."

Scenario: Validação do endereço
#loja/ClienteEdita.asp
#rotina fNEWConcluir
#mensagens:
#                alert('Preencha o endereço de entrega!!');
#                alert('Preencha o número do endereço de entrega!!');
#...até...
#                alert('CEP inválido no endereço de entrega!!');

#loja/PedidoNovoConsiste.asp
#			if EndEtg_endereco = "" then
#				alerta="PREENCHA O ENDEREÇO DE ENTREGA."
#....até...
#				        alerta="Endereço de entrega: preencha a IE (Inscrição Estadual) com um número válido!!" & _
#						        "<br>" & "Certifique-se de que a UF do endereço de entrega corresponde à UF responsável pelo registro da IE."


#loja/PedidoNovoConfirma.asp
#if EndEtg_endereco="" then
#	alerta="PREENCHA O ENDEREÇO DE ENTREGA."
#elseif Len(EndEtg_endereco) > CLng(MAX_TAMANHO_CAMPO_ENDERECO) then
#	alerta="ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " & Cstr(Len(EndEtg_endereco)) & " CARACTERES<br>TAMANHO MÁXIMO: " & Cstr(MAX_TAMANHO_CAMPO_ENDERECO) & " CARACTERES"
#elseif EndEtg_endereco_numero="" then
#	alerta="PREENCHA O NÚMERO DO ENDEREÇO DE ENTREGA."
#elseif EndEtg_cidade="" then
#	alerta="PREENCHA A CIDADE DO ENDEREÇO DE ENTREGA."
#elseif EndEtg_uf="" then
#	alerta="PREENCHA A UF DO ENDEREÇO DE ENTREGA."
#elseif EndEtg_cep="" then
#	alerta="PREENCHA O CEP DO ENDEREÇO DE ENTREGA."


Scenario: Endereço
	When Informo "EndEtg_endereco" = ""
	Then Erro "Preencha o endereço de entrega!!"

@ignore
Scenario: Endereço tamanho
	#elseif Len(EndEtg_endereco) > CLng(MAX_TAMANHO_CAMPO_ENDERECO) then
	#	alerta="ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " & Cstr(Len(EndEtg_endereco)) & " CARACTERES<br>TAMANHO MÁXIMO: " & Cstr(MAX_TAMANHO_CAMPO_ENDERECO) & " CARACTERES"
	#MAX_TAMANHO_CAMPO_ENDERECO = 60;
	#                                          10        20        30       40         50        60
	When Informo "EndEtg_endereco" = "123456789012345678901234567890123456789012345678901234567890123"
	Then Erro regex "ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO.*"
@ignore
Scenario: Endereço tamanho 2
	#                                          10        20        30       40         50        60
	When Informo "EndEtg_endereco" = "12345678901234567890123456789012345678901234567890123456789"
	Then Sem Erro regex "ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO.*"

Scenario: EndEtg_endereco_numero
	When Informo "EndEtg_endereco_numero" = ""
	Then Erro "Preencha o número do endereço de entrega!!"

Scenario: EndEtg_bairro
	When Informo "EndEtg_bairro" = ""
	Then Erro "Preencha o bairro do endereço de entrega!!"

Scenario: EndEtg_cidade
	When Informo "EndEtg_cidade" = ""
	Then Erro "Preencha a cidade do endereço de entrega!!"

Scenario: Justificativa
loja/ClienteEdita.asp 
            if (trim(f.EndEtg_obs.value) == "") {
                alert('Selecione a justificativa do endereço de entrega!!');
                f.EndEtg_obs.focus();
                return;
	When Informo "EndEtg_obs" = ""
	Then Erro "Selecione a justificativa do endereço de entrega!!"

	#esta validação não está no ASP
	When Informo "EndEtg_obs" = "987"
	Then Erro "Código da justficativa inválida!"

Scenario: EndEtg_uf
	When Informo "EndEtg_uf" = ""
	Then Erro "UF inválida no endereço de entrega!!"
Scenario: EndEtg_uf 2
	When Informo "EndEtg_uf" = "XX"
	Then Erro "UF inválida no endereço de entrega!!"
Scenario: EndEtg_uf 3
	When Informo "EndEtg_uf" = "SS"
	Then Erro "UF inválida no endereço de entrega!!"
Scenario: EndEtg_uf 4
	When Informo "EndEtg_uf" = "SP"
	Then Sem erro "UF inválida no endereço de entrega!!"

Scenario: EndEtg_cep
	#também em 	#loja/PedidoNovoConfirma.asp
	#'	CEP
	#	if alerta = "" then
	#		if rb_end_entrega = "S" then
	#			if EndEtg_cep = "" then
	#				alerta = "Informe o CEP do endereço de entrega."
	When Informo "EndEtg_cep" = ""
	Then Erro "Informe o CEP do endereço de entrega!!"

Scenario: EndEtg_cep cep_ok
	#cep_ok: 	if ((len(s_cep)=0) Or (len(s_cep)=5) Or (len(s_cep)=8)) then cep_ok = True
	When Informo "EndEtg_cep" = "1234"
	Then Erro "CEP inválido no endereço de entrega!!"
Scenario: EndEtg_cep cep_ok 2
	When Informo "EndEtg_cep" = "123456"
	Then Erro "CEP inválido no endereço de entrega!!"
Scenario: EndEtg_cep cep_ok 3
	When Informo "EndEtg_cep" = "123456789"
	Then Erro "CEP inválido no endereço de entrega!!"
Scenario: EndEtg_cep cep_ok 4
	When Informo "EndEtg_cep" = "12345678"
	Then Sem erro "CEP inválido no endereço de entrega!!"

#estas validações não estão no ASP
Scenario: EndEtg_uf errado
	When Informo "EndEtg_uf" = "BA"
	Then Erro "Estado não confere!"

Scenario: EndEtg_cep errado
	When Informo "EndEtg_cep" = "12345678"
	Then Erro "Endereço Entrega: cep inválido!"

Scenario: EndEtg_cidade errado
	When Informo "EndEtg_cidade" = "12345678"
	Then Erro "Cidade não confere"

#validação da cidade que não está no IBGE
@ignore
#todo: afazer: clocar estes testes. Tewm que ver como inicializa o CEP
Scenario: EndEtg_cidade não no IBGE
	When Informo "EndEtg_cep" = "68912350"
	When Informo "EndEtg_cidade" = "Abacate da Pedreira"
	Then Erro "Cidade não está no IBGE (acertar a mensagem)"

	When Informo "EndEtg_cep" = "68912350"
	When Informo "EndEtg_cidade" = "Amapá"
	Then Sem nenhum erro

#se a cidade existir no OBGE, deve ser a mesma do CEP
@ignore
#todo: afazer: colocar estes testes. Tewm que ver como inicializa o CEP
Scenario: EndEtg_cidade não no IBGE 2
	When Informo "EndEtg_cep" = "04321001"
	When Informo "EndEtg_cidade" = "Santo André"
	Then Erro "Cidade inconsistente"

