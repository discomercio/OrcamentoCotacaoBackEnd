@Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
@GerenciamentoBanco
Feature: ValidacaoEstoque2.feature

Scenario: Validar estoque 2 - Feito em outro arquivo
#loja/PedidoNovoConsiste.asp
#loja/PedidoNovoConfirma.asp
#exatamente o mesmo código nas duas

	#		if alerta="" then
	#		'OBTÉM DISPONIBILIDADE DO PRODUTO NO ESTOQUE
	#		for iRegra=LBound(vProdRegra) to UBound(vProdRegra)
	#			if Trim(vProdRegra(iRegra).produto) <> "" then
	#				for iCD=LBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD) to UBound(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD)
	#					if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente > 0) And _
	#						( (id_nfe_emitente_selecao_manual = 0) Or (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente = id_nfe_emitente_selecao_manual) ) then
	#						'VERIFICA SE O CD ESTÁ HABILITADO
	#						'IMPORTANTE: A SELEÇÃO MANUAL DE CD PERMITE O USO DE CD DESATIVADO
	#						if (vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).st_inativo = 0) Or (id_nfe_emitente_selecao_manual <> 0) then
	#							idxItem = Lbound(v_item) - 1
	#							for iItem=Lbound(v_item) to Ubound(v_item)
	#								if (vProdRegra(iRegra).fabricante = v_item(iItem).fabricante) And (vProdRegra(iRegra).produto = v_item(iItem).produto) then
	#									idxItem = iItem
	#									exit for
	#									end if
	#								next
	#							if idxItem < Lbound(v_item) then
	#								alerta=texto_add_br(alerta)
	#								alerta=alerta & "Falha ao localizar o produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto & " na lista de produtos a ser processada"
	#							else
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.fabricante = v_item(idxItem).fabricante
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.produto = v_item(idxItem).produto
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.descricao = v_item(idxItem).descricao
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.descricao_html = v_item(idxItem).descricao_html
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada = v_item(idxItem).qtde
	#								vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_estoque = 0
	#								if Not estoque_verifica_disponibilidade_integral_v2(vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).id_nfe_emitente, vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque) then
	#									alerta=texto_add_br(alerta)
	#									alerta=alerta & "Falha ao tentar consultar disponibilidade no estoque do produto (" & vProdRegra(iRegra).fabricante & ")" & vProdRegra(iRegra).produto
	#									end if
	#								end if
	#							end if
	#						end if
	#
	#					if alerta <> "" then exit for
	#					next
	#				end if
	#
	#			if alerta <> "" then exit for
	#			next
	#		end if 'if alerta=""
	#Validação feita em outro lugar: "A rotina estoque_verifica_disponibilidade_integral_v2 só dá erro se der erro no banco de dados. A lógiga em si está coberta pelo Especificacao\Pedido\Passo60\Gravacao\FluxoGravacaoPedido.feature"
	Given Validação feita em outro arquivo



