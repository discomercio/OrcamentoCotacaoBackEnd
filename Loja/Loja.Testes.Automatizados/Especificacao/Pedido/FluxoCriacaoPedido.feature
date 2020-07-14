Feature: Fluxo da criação do pedido
---
Fluxo no ERP/loja:
1 - Escolher cliente já cadastrado
2 - Confirmar (ou editar) dados cadastrais e informar endereço de entrega
3 - Escolher produtos, quantidades
4 - Escolher indicador e RA
5 - Alterar valores e forma de pagamento e observações (entrega imediata, instalador instala, etc) 
6 - Salvar o pedido
--- 
Fluxo no módulo loja:
1 - Escolher cliente já cadastrado
2 - Confirmar (ou editar) dados cadastrais e informar endereço de entrega
3 - Escolher indicador e RA e Modo de Seleção do CD 
4 - Escolher produtos, quantidades e alterar valores e forma de pagamento
5 - Informar observações (entrega imediata, instalador instala, etc) 
6 - Salvar o pedido
--- 
Fluxo na API:
Salvar o pedido
	Enviar todos os dados para cadastrar o pedido

Scenario: Cadastrar Pedido com o mínimo de informação possível
	Given Existe "login" = "usuario_sistema"
	And Existe "loja" = "202"
