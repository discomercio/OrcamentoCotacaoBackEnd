function limparCamposEndEntrega(){$("#cepEntrega").val(""),$("#ufEntrega").val(""),$("#cidadeEntrega").val(""),$("#bairroEntrega").val(""),$("#enderecoEntrega").val(""),$("#compEntrega").val(""),$("#numEntrega").val("")}function inscreve(e){limparCamposEndEntrega();var t=$(e);$("#cepEntrega").val(t[1].textContent),$("#cep").addClass("valid"),$("#lblcep").addClass("active"),$("#ufEntrega").val(t[2].textContent),$("#ufEntrega").prop("readonly",!0),$("#lblUfEntrega").addClass("active"),$("#cidadeEntrega").val(t[3].textContent),$("#cidadeEntrega").prop("readonly",!0),$("#lblCidadeEntrega").addClass("active"),""!=t[4].textContent.trim()?($("#bairroEntrega").prop("readonly",!0),$("#bairroEntrega").val(t[4].textContent),$("#lblBairroEntrega").addClass("active")):$("#bairroEntrega").prop("readonly",!1),""!=t[5].textContent.trim()?($("#enderecoEntrega").prop("readonly",!0),$("#enderecoEntrega").val(t[5].textContent),$("#lblEnderecoEntrega").addClass("active")):$("#enderecoEntrega").prop("readonly",!1),""!=t[6].textContent.trim()&&($("#compEntrega").val(t[6].textContent),$("#lblComplementoEntrega").addClass("active"))}$("#btnModificar").click(function(){var e=this;$(".teste").children().find("input").filter(function(){1==$(e).prop("checked")&&inscreve($(e).parent().parent().parent()[0].children);return!0})}),window.montaTabela=function(e){var t="",a=e.ListaCep;if(a.length>0){$("#msg").css("display","block")&&$("#msg").css("display","none"),$(".tabela_endereco").css("display","block");for(var n=0;n<a.length;n++)t+="<tr id='linha' class='teste'>",t+="<td>",t+="<label><input class='with-gap check' type='radio' value='"+n+"'></input><span></span></label>",t+="</td>",t+="<td>"+a[n].Cep+"</td>",t+="<td>"+a[n].Uf+"</td>",t+="<td>"+a[n].Cidade+"</td>",t+="<td>"+a[n].Bairro+"</td>",t+="<td>"+a[n].Endereco+"</td>",t+="<td>"+a[n].LogradouroComplemento+"</td></tr>",$("#tableBody").empty().append(t);$(".teste").click(function(){$(this).find("td").each(function(e){$(this).find("label")&&$(this).find("label").each(function(e){$(this).find("input")&&$(this).find("input").each(function(e){for(var t=document.getElementsByClassName("check"),a=0;a<t.length;a++);$(this).prop("checked",!0)})})})})}else{$(".tabela_endereco").css("display","block")&&$(".tabela_endereco").css("display","none");$("#msg").css("display","block"),$("#msg").empty().append("<span> Endereço não encontrado!</span>")}};