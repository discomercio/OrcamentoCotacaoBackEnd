/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
define(["require", "exports", "../../DtosTs/DtoPedido/DtoPedidoProdutosPedido", "../../DtosTs/DtoProdutos/DtoProduto", "../../DtosTs/DtoPedido/DtoPedido", "../../UtilTs/MoedaUtils/moedaUtils"], function (require, exports, DtoPedidoProdutosPedido_1, DtoProduto_1, DtoPedido_1, moedaUtils_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Itens = /** @class */ (function () {
        function Itens() {
            //criaremos uma lista para armazenar os itens pelo item principal, independente se é produto composto
            this.lstProdSelectInfo = [];
            this.msgQtdePermitida = "";
        }
        Itens.prototype.mostrarProdutos = function (linha) {
            //const this.selectProdInfo = new SelectProdInfo();
            this.selectProdInfo.produtoComboDto = this.dtoProdutoCombo;
            if (linha) {
                debugger;
                this.selectProdInfo.Produto = linha.NumProduto;
                this.selectProdInfo.Fabricante = linha.Fabricante;
                this.selectProdInfo.Qte = linha.Qtde;
                this.EditarAdicionarProduto(linha);
            }
            else {
                this.EditarAdicionarProduto(linha);
            }
        };
        Itens.prototype.EditarAdicionarProduto = function (linha) {
            var dtoPedido = new DtoPedido_1.DtoPedido();
            var d = new DtoPedido_1.DtoPedido();
            if (this.selectProdInfo.ClicouOk) {
                //vamos editar ou adicionar um novo
                if (linha) {
                    //se mudou o produto, temos que mdar vários campos
                    if (linha.NumProduto !== this.selectProdInfo.Produto || linha.Fabricante !== this.selectProdInfo.Fabricante) {
                        //mudou o produto, temos que mudar muita coisa!
                        var filhosDiretos = this.filhosDeProdutoComposto(this.selectProdInfo);
                        if (!filhosDiretos) {
                            //não é produto composto
                            this.atualizarProduto(linha, this.selectProdInfo.Fabricante, this.selectProdInfo.Produto, this.selectProdInfo.Qte);
                        }
                        else {
                            //produto composto
                            //removemos o item atual e colocamostodos os novos  
                            debugger;
                            this.dtoPedido.ListaProdutos = this.dtoPedido.ListaProdutos.filter(function (el) { return el != linha; });
                            //colcoamos todos os novos
                            for (var i = 0; i < filhosDiretos.length; i++) {
                                var novo = new DtoPedidoProdutosPedido_1.PedidoProdutosDtoPedido();
                                this.dtoPedido.ListaProdutos.push(novo);
                                this.atualizarProduto(novo, filhosDiretos[i].Fabricante, filhosDiretos[i].Produto, this.selectProdInfo.Qte * filhosDiretos[i].Qtde);
                            }
                        }
                    }
                    else {
                        //o produto ficou o mesmo, só atualizamos, menos bagunça
                        this.atualizarProduto(linha, this.selectProdInfo.Fabricante, this.selectProdInfo.Produto, this.selectProdInfo.Qte);
                    }
                }
                else {
                    //adicionando
                    //Gabriel
                    //add produto para verificação de itens
                    this.lstProdSelectInfo.push(this.selectProdInfo);
                    //será necessário verificar se o produto que esta sendo inserido é composto
                    //pois um produto composto são 2 itens ou mais, mas será tratado como sendo 1 item
                    if (this.lstProdSelectInfo.length > 12) {
                        this.msgErro = "É permitido apenas 12 itens por Pré-Pedido!";
                        //this.alertaService.mostrarMensagem("É permitido apenas 12 itens por Pré-Pedido!");
                        return false;
                    }
                    //afazer: arrumar produtos filhos= arrumar o texto da descrição html e mostrar linha 
                    //se for produto simples
                    var filhosDiretos = this.filhosDeProdutoComposto(this.selectProdInfo);
                    if (!filhosDiretos) {
                        //não é produto composto
                        var novo = new DtoPedidoProdutosPedido_1.PedidoProdutosDtoPedido();
                        this.dtoPedido.ListaProdutos = new Array();
                        this.dtoPedido.ListaProdutos.push(novo);
                        this.atualizarProduto(novo, this.selectProdInfo.Fabricante, this.selectProdInfo.Produto, this.selectProdInfo.Qte);
                    }
                    else {
                        //produto composto
                        debugger;
                        this.dtoPedido.ListaProdutos = new Array();
                        for (var i = 0; i < filhosDiretos.length; i++) {
                            var novo = new DtoPedidoProdutosPedido_1.PedidoProdutosDtoPedido();
                            this.dtoPedido.ListaProdutos.push(novo);
                            this.atualizarProduto(novo, filhosDiretos[i].Fabricante, filhosDiretos[i].Produto, this.selectProdInfo.Qte * filhosDiretos[i].Qtde);
                        }
                    }
                }
            }
        };
        Itens.prototype.filhosDeProdutoComposto = function (selectProdInfo) {
            var registros = this.dtoProdutoCombo.ProdutoCompostoDto.filter(function (el) { return el.PaiFabricante === selectProdInfo.Fabricante && el.PaiProduto === selectProdInfo.Produto; });
            if (!registros) {
                return null;
            }
            if (registros.length <= 0) {
                return null;
            }
            return registros[0].Filhos;
        };
        Itens.prototype.atualizarProduto = function (linha, fabricante, produto, qtde) {
            var _a;
            var prodInfo = this.dtoProdutoCombo.ProdutoDto.filter(function (el) { return el.Fabricante === fabricante && el.Produto === produto; })[0];
            if (!prodInfo) {
                prodInfo = new DtoProduto_1.DtoProduto();
            }
            linha.Fabricante = fabricante;
            linha.NumProduto = produto;
            linha.Descricao = prodInfo.Descricao_html;
            linha.Qtde = qtde;
            linha.Preco = prodInfo.Preco_lista;
            linha.VlLista = prodInfo.Preco_lista;
            linha.VlUnitario = prodInfo.Preco_lista;
            this.digitouDescValor(linha, (_a = linha.Desconto) === null || _a === void 0 ? void 0 : _a.toString());
            this.digitouQte(linha);
        };
        Itens.prototype.digitouDescValor = function (i, v) {
            if (v == undefined) {
                return;
            }
            //se não alteraram nada, ignoramos
            if (i.Desconto === parseFloat(v) && i.Desconto == undefined) {
                return;
            }
            i.Desconto = parseFloat(v);
            //não deixa números negativos e nem maior que 100
            /*
            //pensando bem, deixa negativos sim!
            é que parece que tem caso na base com desconto negativo...
            if (i.Desconto <= 0) {
              i.Desconto = 0;
            }
            */
            if (i.Desconto > 100) {
                i.Desconto = 100;
            }
            if (i.Desconto) {
                i.VlUnitario = i.Preco * (1 - i.Desconto / 100);
                i.VlUnitario = parseFloat(i.VlUnitario.toFixed(2));
            }
            else {
                i.VlUnitario = i.Preco;
            }
            this.digitouQte(i);
        };
        Itens.prototype.digitouQte = function (i) {
            var moedaUtils = new moedaUtils_1.MoedaUtils();
            //necessário trazer e verificar a variavel "qtde_max_permitida" na tabela "T_produto_loja" 
            //para limitar a qtde de compra para o usuário
            if (i.Qtde <= 0) {
                i.Qtde = 1;
            }
            i.TotalItem = i.VlUnitario * i.Qtde; // VlUnitario = Vl Venda na tela
            //$("#totalPedido").text(moedaUtils.formatarMoedaSemPrefixo(i.TotalItem));
            //this.dadosPagto.prepedidoAlterado();
        };
        Itens.prototype.digitouDesc = function (e, i) {
            var valor = (e.target).value;
            var v = valor.replace(/\D/g, '');
            //tem 1 casa
            v = (v / 10).toFixed(2) + '';
            this.digitouDescValor(i, v);
        };
        Itens.prototype.arrumarProdsRepetidos = function () {
            debugger;
            var lp = this.dtoPedido.ListaProdutos;
            for (var i = 0; i < lp.length; i++) {
                var este = lp[i];
                //se tiver algum repetido, tiramos o proximo repetido
                var continaurBuscaRepetido = true;
                while (continaurBuscaRepetido) {
                    continaurBuscaRepetido = false;
                    var _loop_1 = function (irepetido) {
                        var repetido = lp[irepetido];
                        if (este.Fabricante === repetido.Fabricante && este.NumProduto == repetido.NumProduto) {
                            //repetido, tem que tirar este!
                            continaurBuscaRepetido = true;
                            este.Qtde += repetido.Qtde;
                            debugger;
                            this_1.dtoPedido.ListaProdutos = this_1.dtoPedido.ListaProdutos.filter(function (el) { return el !== repetido; });
                            lp = this_1.dtoPedido.ListaProdutos;
                            this_1.digitouQte(este);
                        }
                    };
                    var this_1 = this;
                    for (var irepetido = 0 + 1; irepetido < lp.length; irepetido++) {
                        _loop_1(irepetido);
                    }
                }
            }
        };
        Itens.prototype.estoqueExcedido = function (i) {
            var item = this.estoqueItem(i);
            //se nao achamos, dizemos que não tem que mostrar a mensagem não...
            if (!item) {
                return false;
            }
            if (item.Estoque < i.Qtde) {
                return true;
            }
            return false;
        };
        //mensagens de estoque
        Itens.prototype.estoqueItem = function (i) {
            if (!this.dtoProdutoCombo) {
                return null;
            }
            //procuramos esse item
            var item = this.dtoProdutoCombo.ProdutoDto.filter(function (el) { return el.Fabricante === i.Fabricante && el.Produto === i.NumProduto; });
            if (!item || item.length <= 0) {
                return null;
            }
            //achamos o item
            return item[0];
        };
        Itens.prototype.produtoTemAviso = function (i) {
            var item = this.estoqueItem(i);
            //se nao achamos, dizemos que não tem que mostrar a mensagem não...
            if (!item) {
                return false;
            }
            if (!item.Alertas || item.Alertas.trim() === "") {
                return false;
            }
            return true;
        };
        Itens.prototype.qtdeVendaPermitida = function (i) {
            //busca o item na lista
            this.msgQtdePermitida = "";
            debugger;
            var item = this.estoqueItem(i);
            if (!item) {
                return false;
            }
            if (i.Qtde > item.Qtde_Max_Venda) {
                this.msgQtdePermitida = "Quantidade solicitada excede a quantidade máxima de venda permitida!";
                return true;
            }
            else
                return false;
        };
        return Itens;
    }());
    exports.Itens = Itens;
});
//# sourceMappingURL=/scriptsJs/FuncoesTs/Itens/Itens.js.map