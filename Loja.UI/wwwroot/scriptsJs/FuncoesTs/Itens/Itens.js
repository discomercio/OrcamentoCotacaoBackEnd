/// <reference path="../../DtosTs/DtoPedido/DtoPedidoProdutosPedido.ts" />
/// <reference path="../../DtosTs/DtoProdutos/DtoProdutoCombo.ts" />
/// <reference path="../../DtosTs/DtoProdutos/SelectProdInfo.ts" />
///<reference path="../../DtosTs/DtoPedido/DtoPedido.ts" />
class Itens {
    constructor() {
        //criaremos uma lista para armazenar os itens pelo item principal, independente se é produto composto
        this.lstProdSelectInfo = [];
    }
    mostrarProdutos(linha) {
        //const this.selectProdInfo = new SelectProdInfo();
        this.selectProdInfo.produtoComboDto = this.dtoProdutoCombo;
        this.selectProdInfo.ClicouOk = false;
        if (linha) {
            this.selectProdInfo.Produto = linha.NumProduto;
            this.selectProdInfo.Fabricante = linha.Fabricante;
            this.selectProdInfo.Qte = linha.Qtde;
            this.EditarAdicionarProduto(linha);
        }
    }
    EditarAdicionarProduto(linha) {
        if (this.selectProdInfo.ClicouOk) {
            //vamos editar ou adicionar um novo
            if (linha) {
                //editando
                //se mudou o produto, temos que mdar vários campos
                if (linha.NumProduto !== this.selectProdInfo.Produto || linha.Fabricante !== this.selectProdInfo.Fabricante) {
                    //mudou o produto, temos que mudar muita coisa!
                    const filhosDiretos = this.filhosDeProdutoComposto(this.selectProdInfo);
                    if (!filhosDiretos) {
                        //não é produto composto
                        this.atualizarProduto(linha, this.selectProdInfo.Fabricante, this.selectProdInfo.Produto, this.selectProdInfo.Qte);
                    }
                    else {
                        //produto composto
                        //removemos o item atual e colocamostodos os novos
                        this.dtoPedido.ListaProdutos = this.dtoPedido.ListaProdutos.filter(el => el != linha);
                        //colcoamos todos os novos
                        for (let i = 0; i < filhosDiretos.length; i++) {
                            let novo = new DtoPedidoProdutosPedido();
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
                    //this.alertaService.mostrarMensagem("É permitido apenas 12 itens por Pré-Pedido!");
                    return false;
                }
                //se for produto simples
                const filhosDiretos = this.filhosDeProdutoComposto(this.selectProdInfo);
                if (!filhosDiretos) {
                    //não é produto composto
                    let novo = new DtoPedidoProdutosPedido();
                    this.dtoPedido.ListaProdutos.push(novo);
                    this.atualizarProduto(novo, this.selectProdInfo.Fabricante, this.selectProdInfo.Produto, this.selectProdInfo.Qte);
                }
                else {
                    //produto composto
                    for (let i = 0; i < filhosDiretos.length; i++) {
                        let novo = new DtoPedidoProdutosPedido();
                        this.dtoPedido.ListaProdutos.push(novo);
                        this.atualizarProduto(novo, filhosDiretos[i].Fabricante, filhosDiretos[i].Produto, this.selectProdInfo.Qte * filhosDiretos[i].Qtde);
                    }
                }
            }
            //vamos arrumar eventuais produtos repetidos
            this.arrumarProdsRepetidos();
        }
    }
    filhosDeProdutoComposto(selectProdInfo) {
        const registros = this.dtoProdutoCombo.ProdutoCompostoDto.filter(el => el.PaiFabricante === selectProdInfo.Fabricante && el.PaiProduto === selectProdInfo.Produto);
        if (!registros) {
            return null;
        }
        if (registros.length <= 0) {
            return null;
        }
        return registros[0].Filhos;
    }
    atualizarProduto(linha, fabricante, produto, qtde) {
        let prodInfo = this.dtoProdutoCombo.ProdutoDto.filter(el => el.Fabricante === fabricante && el.Produto === produto)[0];
        if (!prodInfo) {
            prodInfo = new DtoProduto();
        }
        linha.Fabricante = fabricante;
        linha.NumProduto = produto;
        linha.Descricao = prodInfo.Descricao_html;
        //Obs: string;
        linha.Qtde = qtde;
        //Permite_Ra_Status: number;
        //BlnTemRa: boolean;
        linha.Preco = prodInfo.Preco_lista;
        linha.VlLista = prodInfo.Preco_lista;
        linha.VlUnitario = prodInfo.Preco_lista;
        if (!linha.Desconto) {
            linha.Desconto = 0;
        }
        this.digitouDescValor(linha, linha.Desconto.toString());
        this.digitouQte(linha);
    }
    digitouDescValor(i, v) {
        //se não alteraram nada, ignoramos
        if (i.Desconto === Number.parseFloat(v))
            return;
        i.Desconto = Number.parseFloat(v);
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
            i.VlUnitario = Number.parseFloat(i.VlUnitario.toFixed(2));
        }
        else {
            i.VlUnitario = i.Preco;
        }
        this.digitouQte(i);
    }
    digitouQte(i) {
        //necessário trazer e verificar a variavel "qtde_max_permitida" na tabela "T_produto_loja" 
        //para limitar a qtde de compra para o usuário
        if (i.Qtde <= 0) {
            i.Qtde = 1;
        }
        i.TotalItem = i.VlUnitario * i.Qtde; // VlUnitario = Vl Venda na tela
        //this.dadosPagto.prepedidoAlterado();
    }
    //consolidamos produtos repetidos
    arrumarProdsRepetidos() {
        let lp = this.dtoPedido.ListaProdutos;
        for (let i = 0; i < lp.length; i++) {
            let este = lp[i];
            //se tiver algum repetido, tiramos o proximo repetido
            let continaurBuscaRepetido = true;
            while (continaurBuscaRepetido) {
                continaurBuscaRepetido = false;
                for (let irepetido = i + 1; irepetido < lp.length; irepetido++) {
                    let repetido = lp[irepetido];
                    if (este.Fabricante === repetido.Fabricante && este.NumProduto == repetido.NumProduto) {
                        //repetido, tem que tirar este!
                        continaurBuscaRepetido = true;
                        este.Qtde += repetido.Qtde;
                        this.dtoPedido.ListaProdutos = this.dtoPedido.ListaProdutos.filter(el => el !== repetido);
                        lp = this.dtoPedido.ListaProdutos;
                        this.digitouQte(este);
                    }
                }
            }
        }
    }
}
//# sourceMappingURL=/scriptsJs/FuncoesTs/Itens/Itens.js.map