@Especificacao.Pedido.FluxoCriacaoPedido
Feature: Fluxo da criação do pedido
---
Fluxo no ERP/loja:
1 - Escolher cliente já cadastrado (em "loja/resumo.asp")
	envia para ClientePesquisa.asp
	se existe somente um cliente, envia para clienteedita.asp com OP_CONSULTA
2 - Confirmar (ou editar) dados cadastrais e informar endereço de entrega (em "loja/clienteedita.asp")
	se editar dados cadastrais, salva na t_cliente
	envia para PedidoNovoProdCompostoMask.asp ou pedidonovo.asp
3 - Escolher produtos, quantidades (em "loja/PedidoNovoProdCompostoMask.asp")
4 - Escolher indicador e RA e CD (somente se o indicador permitir RA) (em "loja/PedidoNovo.asp")
5 - Alterar valores e forma de pagamento e observações (entrega imediata, instalador instala, etc) (em "loja/PedidoNovoConsiste.asp")
	envia para PedidoNovoConfirma.asp
6 - Salvar o pedido (finaliza em "loja/pedido.asp")
--- 
Fluxo no módulo loja:
1 - Passo10: Escolher cliente já cadastrado
	Se o cliente não existir, ele deve ser cadastrado primeiro. (arquivo CLiente/FLuxoCadastroCliente - criar esse arquivo)
2 - Passo20: Confirmar (ou editar) dados cadastrais e informar endereço de entrega
	se editar dados cadastrais, salva na t_cliente
2.5 - Passo 25: somente na API. Validar dados cadastrais. Não existe na tela porque sempre se usa o atual do cliente.
3 - Passo30: Escolher indicador e RA e Modo de Seleção do CD 
4 - Passo40: Escolher produtos, quantidades e alterar valores e forma de pagamento
5 - Passo50: Informar observações (entrega imediata, instalador instala, etc) 
6 - Passo60: Salvar o pedido
--- 
Fluxo na ApaiMagento:
1 - Validar o pedido
2 - se o cliente não existir, cadastrar o cliente
3 - salvar o pedido
--- 
Fluxo na API:
Salvar o pedido (Passo60/Gravacao)
	Enviar todos os dados para cadastrar o pedido

Scenario: Fluxo da criação do pedido
	# documentado em CaminhoFelizAsp.feature
	When Tudo certo, só para aparecer na lista


