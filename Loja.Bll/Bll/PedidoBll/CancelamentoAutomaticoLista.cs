using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Bll.PedidoBll
{
    class CancelamentoAutomaticoLista
    {
		/*
		 * 
		'---------------------------------------
		' _________________________________
		' PRAZO DE CANCELAMENTO DO PEDIDO
		'
		'---------------------------------------
		function lista_Cancelamento
		dim strSql,strWhereBase,s,r,i,n_reg,data_final,cont,vRelat,v_data_final(),v_nome(),v_pedido(),v_descricao(),v_vendedor(),strSqlVlPagoCartao

		strWhereBase = " (t1.st_entrega <> '" & ST_ENTREGA_ENTREGUE & "')" & _
										" AND (t1.st_entrega <> '" & ST_ENTREGA_CANCELADO & "')" & _
										" AND (t1.st_entrega <> '" & ST_ENTREGA_A_ENTREGAR & "')" & _
										" AND (Coalesce(tPedBase.st_pagto, '') <> '" & ST_PAGTO_PAGO & "')" & _
										" AND (Coalesce(tPedBase.st_pagto, '') <> '" & ST_PAGTO_PARCIAL & "')" & _
										" AND (tPedBase.loja = '" & loja & "')"
		' A LISTA DE LOJAS IMUNES FOI DESATIVADA EM 24/10/2019 A PEDIDO DA LILIAN/CARLOS
		'								" AND (tPedBase.loja NOT IN ('" & NUMERO_LOJA_VRF2 & "','" & NUMERO_LOJA_VRF3 & "','" & NUMERO_LOJA_VRF4 & "','" & NUMERO_LOJA_VRF5 & "','" & NUMERO_LOJA_VRF6 & "','" & NUMERO_LOJA_VRF7 & "','" & NUMERO_LOJA_VRF8 & "'))"

		if Not operacao_permitida(OP_LJA_CONSULTA_UNIVERSAL_PEDIDO_ORCAMENTO, s_lista_operacoes_permitidas) then
			strWhereBase = strWhereBase & " AND (tPedBase.vendedor = '" & usuario & "')"
			end if

		strSqlVlPagoCartao = " Coalesce(" & _
							"(" & _
							"SELECT" & _
								" SUM(payment.valor_transacao)" & _
							" FROM t_PAGTO_GW_PAG pag INNER JOIN t_PAGTO_GW_PAG_PAYMENT payment ON (pag.id = payment.id_pagto_gw_pag)" & _
							" WHERE" & _
								" (pag.pedido = t1.pedido_base)" & _
								" AND" & _
								"(" & _
									" (ult_GlobalStatus = '" & BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__CAPTURADA & "')" & _
									" OR" & _
									" (ult_GlobalStatus = '" & BRASPAG_PAGADOR_CARTAO_GLOBAL_STATUS__AUTORIZADA & "')" & _
								")" & _
							"), 0) AS vl_pago_cartao"

						strSql = "SELECT " & _
									"*" & _
								" FROM (" & _
									"SELECT" & _
										" t1.pedido," & _
										" t1.pedido_base," & _
										" Coalesce(t1.obs_2, '') AS obs_2," & _
										" t1.transportadora_selecao_auto_status," & _
										" Coalesce(t1.transportadora_id, '') AS transportadora_id," & _
										" t1.st_entrega," & _
										" 'Pendente Cartão de Crédito' AS analise_credito_descricao," & _
										" " & PRAZO_CANCEL_AUTO_PEDIDO_PENDENTE_CARTAO_CREDITO & " AS prazo_cancelamento," & _
										" tPedBase.analise_credito," & _
										" tPedBase.data_hora AS analise_credito_data," & _
										" tPedBase.data AS analise_credito_data_sem_hora," & _
										" tPedBase.vendedor," & _
										" nome," &_
										" Coalesce(Datediff(day, tPedBase.data, Convert(datetime, Convert(varchar(10), getdate(), 121), 121)), 0) AS dias_decorridos," & _
										" (" & _
											"SELECT Count(*) FROM t_PEDIDO t2 WHERE (t2.pedido_base = t1.pedido_base) AND (t2.st_auto_split = 0) AND (t2.tamanho_num_pedido > " & TAM_MIN_ID_PEDIDO & ")" & _
										") AS qtde_pedido_filhote," & _
										strSqlVlPagoCartao & _
									" FROM t_PEDIDO t1" & _
									" INNER JOIN t_PEDIDO AS tPedBase ON (t1.pedido_base=tPedBase.pedido)" & _
									" INNER JOIN t_CLIENTE on (t1.id_cliente = t_CLIENTE.id)" & _
									" WHERE" & _
										strWhereBase & _
										" AND (" & _
											"(tPedBase.analise_credito = " & COD_AN_CREDITO_ST_INICIAL & ") AND (tPedBase.st_forma_pagto_somente_cartao = 1)" & _
											" AND (Coalesce(Datediff(day, tPedBase.data, getdate()), 0) > (" & PRAZO_CANCEL_AUTO_PEDIDO_PENDENTE_CARTAO_CREDITO & " - " & PRAZO_EXIBICAO_CANCEL_AUTO_PEDIDO & "))" & _
										")" & _
									" UNION " & _
									"SELECT" & _
										" t1.pedido," & _
										" t1.pedido_base," & _
										" Coalesce(t1.obs_2, '') AS obs_2," & _
										" t1.transportadora_selecao_auto_status," & _
										" Coalesce(t1.transportadora_id, '') AS transportadora_id," & _
										" t1.st_entrega," & _
										" 'Crédito OK (aguardando depósito)' AS analise_credito_descricao," & _
										" " & PRAZO_CANCEL_AUTO_PEDIDO_CREDITO_OK_AGUARDANDO_DEPOSITO & " AS prazo_cancelamento," & _
										" tPedBase.analise_credito," & _
										" tPedBase.analise_credito_data," & _
										" tPedBase.analise_credito_data_sem_hora," & _
										" tPedBase.vendedor," & _
										" nome," &_
										" Coalesce(Datediff(day, tPedBase.analise_credito_data_sem_hora, Convert(datetime, Convert(varchar(10), getdate(), 121), 121)), 0) AS dias_decorridos," & _
										" (" & _
											"SELECT Count(*) FROM t_PEDIDO t2 WHERE (t2.pedido_base = t1.pedido_base) AND (t2.st_auto_split = 0) AND (t2.tamanho_num_pedido > " & TAM_MIN_ID_PEDIDO & ")" & _
										") AS qtde_pedido_filhote," & _
										strSqlVlPagoCartao & _
									" FROM t_PEDIDO t1" & _
									" INNER JOIN t_PEDIDO AS tPedBase ON (t1.pedido_base=tPedBase.pedido)" & _
									" INNER JOIN t_CLIENTE on (t1.id_cliente = t_CLIENTE.id)" & _
									" WHERE" & _
										strWhereBase & _
										" AND (" & _
											"(tPedBase.analise_credito = " & COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO & ")" & _
											" AND (tPedBase.analise_credito_data_sem_hora IS NOT NULL)" & _
											" AND (Coalesce(Datediff(day, tPedBase.analise_credito_data_sem_hora, getdate()), 0) > (" & PRAZO_CANCEL_AUTO_PEDIDO_CREDITO_OK_AGUARDANDO_DEPOSITO & " -  " & PRAZO_EXIBICAO_CANCEL_AUTO_PEDIDO & "))" & _
										")" & _
									" UNION " & _
									"SELECT" & _
										" t1.pedido," & _
										" t1.pedido_base," & _
										" Coalesce(t1.obs_2, '') AS obs_2," & _
										" t1.transportadora_selecao_auto_status," & _
										" Coalesce(t1.transportadora_id, '') AS transportadora_id," & _
										" t1.st_entrega," & _
										" 'Pendente Vendas' AS analise_credito_descricao," & _
										" " & PRAZO_CANCEL_AUTO_PEDIDO_PENDENTE_VENDAS & " AS prazo_cancelamento," & _
										" tPedBase.analise_credito," & _
										" tPedBase.analise_credito_data," & _
										" tPedBase.analise_credito_data_sem_hora," & _
										" tPedBase.vendedor," & _
										" nome," &_
										" Coalesce(Datediff(day, tPedBase.analise_credito_data_sem_hora, Convert(datetime, Convert(varchar(10), getdate(), 121), 121)), 0) AS dias_decorridos," & _
										" (" & _
											"SELECT Count(*) FROM t_PEDIDO t2 WHERE (t2.pedido_base = t1.pedido_base) AND (t2.st_auto_split = 0) AND (t2.tamanho_num_pedido > " & TAM_MIN_ID_PEDIDO & ")" & _
										") AS qtde_pedido_filhote," & _
										strSqlVlPagoCartao & _
									" FROM t_PEDIDO t1" & _
									" INNER JOIN t_PEDIDO AS tPedBase ON (t1.pedido_base=tPedBase.pedido)" & _
									" INNER JOIN t_CLIENTE on (t1.id_cliente = t_CLIENTE.id)" & _
									" WHERE" & _
										strWhereBase & _
										" AND (" & _
											"(tPedBase.analise_credito = " & COD_AN_CREDITO_PENDENTE_VENDAS & ")" & _
											" AND (tPedBase.analise_credito_data_sem_hora IS NOT NULL)" & _
											" AND (Coalesce(Datediff(day, tPedBase.analise_credito_data_sem_hora, getdate()), 0) >  (" & PRAZO_CANCEL_AUTO_PEDIDO_PENDENTE_VENDAS & " -  " & PRAZO_EXIBICAO_CANCEL_AUTO_PEDIDO & "))" & _
										")" & _
									") t" & _
								" WHERE" & _
									" (qtde_pedido_filhote = 0)" & _
									" AND (LEN(obs_2) = 0)" & _
									" AND (vl_pago_cartao = 0)" & _
									" AND ((transportadora_selecao_auto_status = 1) OR (LEN(Coalesce(transportadora_id,'')) = 0))" & _
								" ORDER BY" & _
									" analise_credito_data_sem_hora," & _
									" analise_credito," & _							
									" pedido"

			set r = cn.Execute(strSql)


			 s = "<table width='600' class='QS' cellSpacing='0' style='border-left:0px;'>" & chr(13) & _
						"<tr class='DefaultBkg'>" & chr(13) & _
						"<td style='background:#FFF' class='MD MB'> &nbsp; </td>" & chr(13) & _
						"<td class='MD MTB' align='left'><p class='R'>DATA FINAL</p></td>" & chr(13) & _
						"<td class='MD MTB' align='left'><p class='R'>PEDIDO</p></td>" & chr(13)
			if operacao_permitida(OP_LJA_CONSULTA_UNIVERSAL_PEDIDO_ORCAMENTO, s_lista_operacoes_permitidas) then
				s = s & "<td class='MD MTB' align='left'><p class='R'>VENDEDOR</p></td>" & chr(13)
			end if
			s = s & "<td class='MD MTB' align='left'><p class='R'>NOME DO CLIENTE</p></td>" & chr(13) & _
			"<td class='MTB' align='left'><p class='R'>ANÁLISE DE CRÉDITO</p></td>" & chr(13) & _                 
			"</tr>"
			i = 0
			cont = 0
			if not r.Eof then
				do while Not r.Eof 
					data_final = DateAdd("d",r("prazo_cancelamento"),r("analise_credito_data_sem_hora"))           
						n_reg = n_reg + 1
						redim preserve v_data_final(cont)
						redim preserve v_nome(cont) 
						redim preserve v_pedido(cont)
						redim preserve v_descricao(cont)
						redim preserve v_vendedor(cont)
						v_data_final(cont) = data_final
						v_nome(cont) = r("nome")
						v_pedido(cont) = r("pedido")
						v_descricao(cont) = r("analise_credito_descricao")
						v_vendedor(cont) = r("vendedor")
						cont = cont + 1                 
					r.MoveNext
				loop
		  ' ORDENAÇÃO       
				if n_reg <> 0 then
					redim vRelat(0)
					set vRelat(0) = New cl_CINCO_COLUNAS
					with vRelat(0)
						.c1 = ""
						.c2 = ""
						.c3 = ""
						.c4 = ""
						.c5 = ""
					end with
					if v_data_final(Ubound(v_data_final)) <> "" then
						for cont = 0 to Ubound(v_data_final)
							if Trim(vRelat(ubound(vRelat)).c1) <> "" then
								redim preserve vRelat(ubound(vRelat)+1)
								set vRelat(ubound(vRelat)) = New cl_CINCO_COLUNAS
							end if
							with vRelat(ubound(vRelat))
								.c1 =  v_data_final(cont)
								.c2 =  v_pedido(cont)
								.c3 =  v_nome(cont)
								.c4 =  v_descricao(cont)
								.c5 =  v_vendedor(cont)
							end with
						next
					end if
					ordena_cl_cinco_colunas vRelat, 0, Ubound(vRelat)
					n_reg = 0 
		  ' PREENCHE A TABELA         
					for cont = 0 to Ubound(v_data_final)    
							n_reg = n_reg + 1	        
							i = i + 1
							if (i AND 1)=0 then
								s = s & "<tr nowrap class='DefaultBkg'>" & chr(13)
							else
								s = s & "<tr nowrap>" & chr(13)
							end if
							s = s & "	<td class='tdReg' style='width:20px' nowrap><p class='Rd'>" & n_reg & ".</p></td>" & chr(13)
							s = s & "	<td class='tdDataF' nowrap><p class='C'>" & formata_data(vRelat(cont).c1) & "</p></td>" & chr(13)
							s = s & "   <td class='tdPedido' nowrap><p class='C'>&nbsp;<a href='javascript:fRELConsul(" & _
							chr(34) & vRelat(cont).c2 & chr(34) & _
							")' title='clique para consultar o pedido'>" & vRelat(cont).c2 & "</a></p></td>" & chr(13)
							if operacao_permitida(OP_LJA_CONSULTA_UNIVERSAL_PEDIDO_ORCAMENTO, s_lista_operacoes_permitidas) then
								s = s & "	<td class='tdVen' nowrap><p class='C'>" & vRelat(cont).c5 & "</p></td>" & chr(13)
							end if
							s = s & "	<td class='tdCliente'><p class='C'>" & vRelat(cont).c3 & "</p></td>" & chr(13)
							s = s & "	<td class='tdAnalise'><p class='C'>" & vRelat(cont).c4 & "</p></td>" & chr(13)      
							s = s & "</tr>" & chr(13)
					next
				end if    
			end if
			if i = 0 then
			s = s & "<tr nowrap class='DefaultBkg'>" & _
					"<td style='width:20px;background: #FFF;' class='MD ME'><p class='Rd'>0.</p></td>" & _
					"<td align='center' colspan='5'>" & _
					"<p class='C' style='color:red;letter-spacing:1px;'>NENHUM PEDIDO.</p>" & _
					"</td></tr>"
			end if

			s = s & "</table>" & chr(13)

			lista_Cancelamento = s

			r.close
			set r=nothing

		end function
		
		 * * */
	}
}
