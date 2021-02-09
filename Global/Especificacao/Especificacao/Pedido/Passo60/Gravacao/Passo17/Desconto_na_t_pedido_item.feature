@ignore
Feature: Desconto_na_t_item
#Garantir que o desconto utilizado seja gravado na t_pedido_item

#loja/PedidoNovoConfirma.asp de linha 798 a 908 (VERIFICA CADA UM DOS PRODUTOS SELECIONADOS)
#'	VERIFICA CADA UM DOS PRODUTOS SELECIONADOS
#	dim desc_dado_arredondado
#	if alerta="" then
#		for i=Lbound(v_item) to Ubound(v_item)
#			with v_item(i)
#				s = "SELECT " & _
#						"*" & _
#					" FROM t_PRODUTO" & _
#						" INNER JOIN t_PRODUTO_LOJA" & _
#							" ON ((t_PRODUTO.fabricante=t_PRODUTO_LOJA.fabricante) AND (t_PRODUTO.produto=t_PRODUTO_LOJA.produto))" & _
#						" INNER JOIN t_FABRICANTE" & _
#							" ON (t_PRODUTO.fabricante=t_FABRICANTE.fabricante)" & _
#					" WHERE" & _
#						" (t_PRODUTO.fabricante='" & .fabricante & "')" & _
#						" AND (t_PRODUTO.produto='" & .produto & "')" & _
#						" AND (loja='" & loja & "')"
#				set rs = cn.execute(s)
#				if rs.Eof then
#					alerta=texto_add_br(alerta)
#					alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & " NÃO está cadastrado para a loja " & loja
#				else
#					.preco_lista = rs("preco_lista")
#					.margem = rs("margem")
#					.desc_max = rs("desc_max")
#					.comissao = rs("comissao")
#					.preco_fabricante = rs("preco_fabricante")
#					.vl_custo2 = rs("vl_custo2")
#					.descricao = Trim("" & rs("descricao"))
#					.descricao_html = Trim("" & rs("descricao_html"))
#					.ean = Trim("" & rs("ean"))
#					.grupo = Trim("" & rs("grupo"))
#                    .subgrupo = Trim("" & rs("subgrupo"))
#					.peso = rs("peso")
#					.qtde_volumes = rs("qtde_volumes")
#					.markup_fabricante = rs("markup")
#					.cubagem = rs("cubagem")
#					.ncm = Trim("" & rs("ncm"))
#					.cst = Trim("" & rs("cst"))
#					.descontinuado = Trim("" & rs("descontinuado"))
#					
#					.custoFinancFornecPrecoListaBase = .preco_lista
#					if c_custoFinancFornecTipoParcelamento = COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA then
#						coeficiente = 1
#					else
#						s = "SELECT " & _
#								"*" & _
#							" FROM t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR" & _
#							" WHERE" & _
#								" (fabricante = '" & .fabricante & "')" & _
#								" AND (tipo_parcelamento = '" & c_custoFinancFornecTipoParcelamento & "')" & _
#								" AND (qtde_parcelas = " & c_custoFinancFornecQtdeParcelas & ")"
#						set rs2 = cn.execute(s)
#						if rs2.Eof then
#							alerta=texto_add_br(alerta)
#							alerta=alerta & "Opção de parcelamento não disponível para fornecedor " & .fabricante & ": " & decodificaCustoFinancFornecQtdeParcelas(c_custoFinancFornecTipoParcelamento, c_custoFinancFornecQtdeParcelas) & " parcela(s)"
#						else
#							coeficiente = converte_numero(rs2("coeficiente"))
#							.preco_lista=converte_numero(formata_moeda(coeficiente*.preco_lista))
#							end if
#						end if
#					.custoFinancFornecCoeficiente = coeficiente
#					
#					if .preco_lista = 0 then 
#						.desc_dado = 0
#						desc_dado_arredondado = 0
#					else
#						.desc_dado = 100*(.preco_lista-.preco_venda)/.preco_lista
#						desc_dado_arredondado = converte_numero(formata_perc_desc(.desc_dado))
#						end if
#					
#					if desc_dado_arredondado > perc_comissao_e_desconto_a_utilizar then
#						if rs.State <> 0 then rs.Close
#						s = "SELECT " & _
#								"*" & _
#							" FROM t_DESCONTO" & _
#							" WHERE" & _
#								" (usado_status=0)" & _
#								" AND (cancelado_status=0)" & _
#								" AND (id_cliente='" & cliente_selecionado & "')" & _
#								" AND (fabricante='" & .fabricante & "')" & _
#								" AND (produto='" & .produto & "')" & _
#								" AND (loja='" & loja & "')" & _
#								" AND (data >= " & bd_formata_data_hora(Now-converte_min_to_dec(TIMEOUT_DESCONTO_EM_MIN)) & ")" & _
#							" ORDER BY" & _
#								" data DESC"
#						set rs=cn.execute(s)
#						if rs.Eof then
#							alerta=texto_add_br(alerta)
#							alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": desconto de " & formata_perc_desc(.desc_dado) & "% excede o máximo permitido."
#						else
#							if .desc_dado > rs("desc_max") then
#								alerta=texto_add_br(alerta)
#								alerta=alerta & "Produto " & .produto & " do fabricante " & .fabricante & ": desconto de " & formata_perc_desc(.desc_dado) & "% excede o máximo autorizado."
#							else
#								.abaixo_min_status=1
#								.abaixo_min_autorizacao=Trim("" & rs("id"))
#								.abaixo_min_autorizador=Trim("" & rs("autorizador"))
#								.abaixo_min_superv_autorizador=Trim("" & rs("supervisor_autorizador"))
#								If v_desconto(UBound(v_desconto)) <> "" Then
#									ReDim Preserve v_desconto(UBound(v_desconto) + 1)
#									v_desconto(UBound(v_desconto)) = ""
#									End If
#								v_desconto(UBound(v_desconto)) = Trim("" & rs("id"))
#								end if
#							end if
#						end if
#					end if
#				rs.Close
#				end with
#			next
#		end if
#	


Scenario: Autorizacao de desconto
	Given Verificar que as ifns são gravadas na t_pedido_item

