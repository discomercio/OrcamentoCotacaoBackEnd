@ignore
Feature: Coisas válidas somente para ID_PARAM_SITE = COD_SITE_ASSISTENCIA_TECNICA 

Scenario: pedBonshop (campo pedido_bs_x_at)
	When Fazer esta validação
			 # <!-------------- PEDIDO BONSHOP ---------------->
			 # <% if ID_PARAM_SITE = COD_SITE_ASSISTENCIA_TECNICA then %>
			 # <tr>
			 #   <td align="left" class="MC">&nbsp;</td>
			 #   <td align="left" class="MC" style="padding: 5px 0px">
			 #       <span class="C">Ref. Pedido Bonshop:</span>
			 #       <select id="pedBonshop" name="pedBonshop" style="width: 80px">
			 #           <option value="">&nbsp;</option>
			 #           <%
			 #           dim cn2
    #If Not bdd_BS_conecta(cn2) then Response.Redirect("aviso.asp?id=" & ERR_CONEXAO)
    #dim r, sqlString, strResp
    # 
    # sqlString = "SELECT pedido FROM t_PEDIDO" & _
    #          " INNER JOIN t_CLIENTE ON (t_CLIENTE.id = t_PEDIDO.id_cliente)" & _
    #          " WHERE t_CLIENTE.cnpj_cpf = '" & r_cliente.cnpj_cpf & "'" & _
    #          " AND (st_entrega = '" & ST_ENTREGA_ENTREGUE & "')" & _
    #          " ORDER BY data DESC, pedido"
    # set r = cn2.Execute(sqlString)
    # strResp = ""
    # do while Not r.eof
    #    strResp = strResp & "<option value='" & r("pedido") & "'>" 
    #    strResp = strResp & r("pedido")
    #    strResp = strResp & "</option>" & chr(13)
    #    r.MoveNext
    # loop
    # Response.Write strResp
    # r.close
    # set r=nothing
    # cn2.Close
    # set cn2 = nothing%>
			 #       </select>

#todo: verificar com o Hamilton o que fazer

