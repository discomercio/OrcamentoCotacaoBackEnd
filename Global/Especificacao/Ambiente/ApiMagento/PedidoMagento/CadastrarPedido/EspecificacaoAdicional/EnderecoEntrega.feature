@ignore
Feature: EnderecoEntrega (somente PF)
#Endereço
#Se o cliente for PF, sempre será usado somente o endereço de entrega como sendo o único endereço do cliente.
#Se o cliente for PJ, será feita a comparação do endereço de entrega com o endereço de cobrança. Se forem iguais, descartar o endereço de entrega. Se forem diferentes, assumir que há endereço de entrega a ser usado.
#
#O fluxo que vamos usar:
#Se for PF, copio o endereço de entrega para o endereço de cobrança e apago o endereço de entrega.
#Se for PJ, se endereço de entrega for igual ao endereço de cobrança, apaga o endereço de entrega.
#	Comparação de endereço: qualquer campo diferente é diferente (inclusive complemento e ponto de referência)
#Depois disso, se o cliente não existir, cadastrar com o endereço de cobrança

#201229 reuniao semanal.txt
#- fazer o endereço de entrega para PF obrigatório?
#sim, exigir. 


#Verificar se o pedido tem os campos de endereço de entrega preenchidos - não pode estar preenchido
#e os campos que estavam na entrega tem que estar no cadastral
Scenario: EnderecoEntrega PF
	Given Pedido base cliente PF
	And Limpar endereço de entrega
	Then Sem nenhum erro
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "EndEtg_cnpj_cpf" = ""
	And Tabela "t_PEDIDO" registro com campo "pedido" = "{número pedido}", verificar campo "EndEtg_endereco" = ""

