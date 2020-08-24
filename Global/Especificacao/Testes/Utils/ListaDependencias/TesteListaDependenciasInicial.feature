@Testes.TesteListaDependencias.Inicial
Feature: Teste da Lista Dependencias parte 1 que deve ser executada primeiro
Usamos isto para garantir que um teste seja executado em dois ambientes.
Exemplo: o endereço cadastrarl deve ser validado no pedido e no prepedido.
Para não duplicar o teste, a implementação executa os testes nos dois ambientes ao mesmo tempo.

Então, neste exemplo, vamos descrever a funcionalidade no Pedido. No Prepedido, marcamos que essa validação existe com um .feature, que verifica se foi executada. 
Neste exemplo, este arquivo seria a descrição efetiva (do pedido) e o TesteListaDependenciasFinal seria o do prepedido, que somente garante que foi executada
através do pedido.

Explicando de novo: na implementação dos passos do pedido, testamos tanto o pedido quanto o prepedido. E na implementação do prepedido somente
garantimos que tenha sido executada no pedido.

Scenario: Teste
	Given Passo 1
	When Passo 2
	Then Passo 3
