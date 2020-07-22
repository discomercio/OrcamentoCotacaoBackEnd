@Pedido
Feature: Fluxo da criação do pedido
---
Fluxo no ERP/loja:
1 - Escolher cliente já cadastrado (em "loja/resumo.asp")
	envia para ClientePesquisa.asp
	se existe somente um cliente, envia para clienteedita.asp com OP_CONSULTA
2 - Confirmar (ou editar) dados cadastrais e informar endereço de entrega (em "loja/clienteedita.asp")
	envia para PedidoNovoProdCompostoMask.asp ou pedidonovo.asp
3 - Escolher produtos, quantidades (em "loja/PedidoNovoProdCompostoMask.asp")
4 - Escolher indicador e RA (em "loja/PedidoNovo.asp")
5 - Alterar valores e forma de pagamento e observações (entrega imediata, instalador instala, etc) (em "loja/PedidoNovoConsiste.asp")
	envia para PedidoNovoConfirma.asp
6 - Salvar o pedido (finaliza em "loja/pedido.asp")
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

Scenario: Cadastrar o pedido com o mínimo de informação possível
# documentado em CaminhoFelizAsp.feature


