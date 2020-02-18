define(["require","exports","../../DtosTs/DtoPedido/DtoPedidoProdutosPedido","../../DtosTs/DtoPedido/DtoPedido","../../UtilTs/MoedaUtils/moedaUtils","../../DtosTs/DtoProdutos/ProdutoDto"],function(t,o,e,i,r,d){"use strict";Object.defineProperty(o,"__esModule",{value:!0});var s=function(){function t(){this.lstProdSelectInfo=[],this.msgQtdePermitida=""}return t.prototype.mostrarProdutos=function(t){this.selectProdInfo.produtoComboDto=this.dtoProdutoCombo,t?(this.selectProdInfo.Produto=t.NumProduto,this.selectProdInfo.Fabricante=t.Fabricante,this.selectProdInfo.Qte=t.Qtde,this.EditarAdicionarProduto(t)):this.EditarAdicionarProduto(t)},t.prototype.EditarAdicionarProduto=function(t){new i.DtoPedido,new i.DtoPedido;if(this.selectProdInfo.ClicouOk)if(t){if(t.NumProduto!==this.selectProdInfo.Produto||t.Fabricante!==this.selectProdInfo.Fabricante)if(d=this.filhosDeProdutoComposto(this.selectProdInfo)){this.dtoPedido.ListaProdutos=this.dtoPedido.ListaProdutos.filter(function(o){return o!=t});for(var o=0;o<d.length;o++){var r=new e.PedidoProdutosDtoPedido;this.dtoPedido.ListaProdutos.push(r),this.atualizarProduto(r,d[o].Fabricante,d[o].Produto,this.selectProdInfo.Qte*d[o].Qtde)}}else this.atualizarProduto(t,this.selectProdInfo.Fabricante,this.selectProdInfo.Produto,this.selectProdInfo.Qte);else this.atualizarProduto(t,this.selectProdInfo.Fabricante,this.selectProdInfo.Produto,this.selectProdInfo.Qte)}else{if(this.lstProdSelectInfo.push(this.selectProdInfo),this.lstProdSelectInfo.length>12)return this.msgErro="É permitido apenas 12 itens por Pré-Pedido!",!1;var d;if(d=this.filhosDeProdutoComposto(this.selectProdInfo)){this.dtoPedido.ListaProdutos=new Array;for(o=0;o<d.length;o++){r=new e.PedidoProdutosDtoPedido;this.dtoPedido.ListaProdutos.push(r),this.atualizarProduto(r,d[o].Fabricante,d[o].Produto,this.selectProdInfo.Qte*d[o].Qtde)}}else{var r=new e.PedidoProdutosDtoPedido;this.dtoPedido.ListaProdutos=new Array,this.dtoPedido.ListaProdutos.push(r),this.atualizarProduto(r,this.selectProdInfo.Fabricante,this.selectProdInfo.Produto,this.selectProdInfo.Qte)}}},t.prototype.filhosDeProdutoComposto=function(t){var o=this.dtoProdutoCombo.ProdutoCompostoDto.filter(function(o){return o.PaiFabricante===t.Fabricante&&o.PaiProduto===t.Produto});return o?o.length<=0?null:o[0].Filhos:null},t.prototype.atualizarProduto=function(t,o,e,i){var r,s=this.dtoProdutoCombo.ProdutoDto.filter(function(t){return t.Fabricante===o&&t.Produto===e})[0];s||(s=new d.ProdutoDto),t.Fabricante=o,t.NumProduto=e,t.Descricao=s.Descricao_html,t.Qtde=i,t.Preco=s.Preco_lista,t.VlLista=s.Preco_lista,t.VlUnitario=s.Preco_lista,this.digitouDescValor(t,null===(r=t.Desconto)||void 0===r?void 0:r.toString()),this.digitouQte(t)},t.prototype.digitouDescValor=function(t,o){void 0!=o&&(t.Desconto===parseFloat(o)&&void 0==t.Desconto||(t.Desconto=parseFloat(o),t.Desconto>100&&(t.Desconto=100),t.Desconto?(t.VlUnitario=t.Preco*(1-t.Desconto/100),t.VlUnitario=parseFloat(t.VlUnitario.toFixed(2))):t.VlUnitario=t.Preco,this.digitouQte(t)))},t.prototype.digitouQte=function(t){new r.MoedaUtils;t.Qtde<=0&&(t.Qtde=1),t.TotalItem=t.VlUnitario*t.Qtde},t.prototype.digitouDesc=function(t,o){var e=t.target.value.replace(/\D/g,"");e=(e/10).toFixed(2)+"",this.digitouDescValor(o,e)},t.prototype.arrumarProdsRepetidos=function(){for(var t=this.dtoPedido.ListaProdutos,o=0;o<t.length;o++)for(var e=t[o],i=!0;i;){i=!1;for(var r=function(o){var r=t[o];e.Fabricante===r.Fabricante&&e.NumProduto==r.NumProduto&&(i=!0,e.Qtde+=r.Qtde,d.dtoPedido.ListaProdutos=d.dtoPedido.ListaProdutos.filter(function(t){return t!==r}),t=d.dtoPedido.ListaProdutos,d.digitouQte(e))},d=this,s=1;s<t.length;s++)r(s)}},t.prototype.estoqueExcedido=function(t){var o=this.estoqueItem(t);return!!o&&o.Estoque<t.Qtde},t.prototype.estoqueItem=function(t){if(!this.dtoProdutoCombo)return null;var o=this.dtoProdutoCombo.ProdutoDto.filter(function(o){return o.Fabricante===t.Fabricante&&o.Produto===t.NumProduto});return!o||o.length<=0?null:o[0]},t.prototype.produtoTemAviso=function(t){var o=this.estoqueItem(t);return!!o&&!(!o.Alertas||""===o.Alertas.trim())},t.prototype.qtdeVendaPermitida=function(t){this.msgQtdePermitida="";var o=this.estoqueItem(t);return!!o&&(t.Qtde>o.Qtde_Max_Venda&&(this.msgQtdePermitida="Quantidade solicitada excede a quantidade máxima de venda permitida!",!0))},t}();o.Itens=s});