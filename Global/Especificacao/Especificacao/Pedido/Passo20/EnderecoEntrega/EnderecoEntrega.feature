@ignore
@Especificacao.Pedido
Feature: Validar endereco de entrega

Background: Pedido base com endereço de entrega (pedido e prepedido)
	Given Pedido base com endereço de entrega

Scenario: Informado
loja/ClienteEdita.asp 
rotina fNEWConcluir
	Given Pedido base
	When Informo "OutroEndereco" = "XX"
	Then Erro "Informe se o endereço de entrega será o mesmo endereço do cadastro ou não!!"
	Given Pedido base com endereço de entrega
	When Informo "OutroEndereco" = "XX"
	Then Erro "Informe se o endereço de entrega será o mesmo endereço do cadastro ou não!!"

Scenario: Validação do endereço
loja/ClienteEdita.asp 
rotina fNEWConcluir
mensagens:
                alert('Preencha o endereço de entrega!!');
                alert('Preencha o número do endereço de entrega!!');
...até...
                alert('CEP inválido no endereço de entrega!!');

Scenario: Endereço
	When Informo "EndEtg_endereco" = ""
	Then Erro "Preencha o endereço de entrega!!"

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
	Then Erro "UF inconsistente com o CEP  (acertar a mensagem)"

Scenario: EndEtg_cep errado
	When Informo "EndEtg_cep" = "12345678"
	Then Erro "CEP não existe (acertar a mensagem)"

Scenario: EndEtg_cidade errado
	When Informo "EndEtg_cidade" = "12345678"
	Then Erro "EndEtg_cidade não existe (acertar a mensagem)"

