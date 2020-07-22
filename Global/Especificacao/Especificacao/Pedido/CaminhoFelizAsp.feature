@Pedido
@CaminhoFelizAsp @SemTestes
Feature: CaminhoFelizAsp
	Caminho feliz da criação do pedido na loja (ASP)
	Operações mínimas para cadastar um pedido
	Está aqui somente por documentação, não é efetivamente testado

Scenario: Cadastar um pedido na loja
	Given Fiz login 
	And Estou na página "loja/resumo.asp"
	When No bloco "NOVO PEDIDO" digito o CPF/CNPJ "089.617.758/04" e clico em "EXECUTAR CONSULTA"
	Then Vou para página "loja/clienteedita.asp?cliente_selecionado=000000246890"
	When Seleciono a opção "O mesmo endereço do cadastro" e clico em "PEDIDO"
	Then Vou para página "loja/PedidoNovoProdCompostoMask.asp"
	When Seleciono o produto "003243" e quantidade "1" e clico em "PRÓXIMO"
	Then Vou para página "loja/PedidoNovo.asp"
	When Seleciono a opção "Sem Indicação" e clico em "PRÓXIMO"
	Then Vou para página "loja/PedidoNovoConsiste.asp"
	When Seleciono a opção "Entrega Imediata" como "Sim"
	And Seleciono a opção "Bem de Uso/Consumo" como "Sim"
	And Seleciono a opção "Instalador Instala" como "Sim"
	And Seleciono a opção "À Vista" como "Dinheiro"
	And Clico em "CONFIRMAR"
	Then O pedido é criado
	And Vou para página "loja/pedido.asp?pedido_selecionado=176324N"


