﻿import { SelectProdInfo } from "../../DtosTs/ProdutosDto/SelectProdInfo";
import { MoedaUtils } from "../../UtilTs/MoedaUtils/moedaUtils";
import { DadosPagto } from "../DadosPagto/DadosPagto";
import { ProdutoComboDto } from "../../DtosTs/ProdutosDto/ProdutoComboDto";
import { ProdutoDto } from "../../DtosTs/ProdutosDto/ProdutoDto";
import { PedidoProdutosPedidoDto } from "../../DtosTs/PedidoDto/PedidoProdutosPedidoDto";
import { PedidoDto } from "../../DtosTs/PedidoDto/PedidoDto";

export class Itens {
    public dtoProdutoCombo: ProdutoComboDto;
    public selectProdInfo: SelectProdInfo;
    public pedidoDto: PedidoDto;
    public pedidoProdutosPedidoDto: PedidoProdutosPedidoDto;
    public dadosPagto: DadosPagto;
    //iremos colocar uma variavel para ser feito a verificação de msg de erros
    public msgErro: string;


    public mostrarProdutos(linha: PedidoProdutosPedidoDto) {
        //const this.selectProdInfo = new SelectProdInfo();
        this.selectProdInfo.produtoComboDto = this.dtoProdutoCombo;

        if (linha) {
            
            this.selectProdInfo.Produto = linha.NumProduto;
            this.selectProdInfo.Fabricante = linha.Fabricante;
            this.selectProdInfo.Qte = linha.Qtde;

            this.EditarAdicionarProduto(linha);
        } else {
            this.EditarAdicionarProduto(linha);
        }
    }

    //criaremos uma lista para armazenar os itens pelo item principal, independente se é produto composto
    public lstProdSelectInfo: SelectProdInfo[] = [];
    public EditarAdicionarProduto(linha: PedidoProdutosPedidoDto) {
        let pedidoDto = new PedidoDto();
        let d = new PedidoDto();
        if (this.selectProdInfo.ClicouOk) {
            //vamos editar ou adicionar um novo
            if (linha) {
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

                        this.pedidoDto.ListaProdutos = this.pedidoDto.ListaProdutos.filter(el => el != linha);

                        //colcoamos todos os novos
                        for (let i = 0; i < filhosDiretos.length; i++) {
                            let novo = new PedidoProdutosPedidoDto();
                            this.pedidoDto.ListaProdutos.push(novo);
                            this.atualizarProduto(novo, filhosDiretos[i].Fabricante,
                                filhosDiretos[i].Produto, this.selectProdInfo.Qte * filhosDiretos[i].Qtde);
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
                const filhosDiretos = this.filhosDeProdutoComposto(this.selectProdInfo);
                if (!filhosDiretos) {
                    //não é produto composto
                    let novo = new PedidoProdutosPedidoDto();
                    this.pedidoDto.ListaProdutos = new Array<PedidoProdutosPedidoDto>();
                    this.pedidoDto.ListaProdutos.push(novo);
                    this.atualizarProduto(novo, this.selectProdInfo.Fabricante, this.selectProdInfo.Produto, this.selectProdInfo.Qte);
                }
                else {
                    //produto composto
                    
                    this.pedidoDto.ListaProdutos = new Array<PedidoProdutosPedidoDto>();
                    for (let i = 0; i < filhosDiretos.length; i++) {
                        let novo = new PedidoProdutosPedidoDto();
                        this.pedidoDto.ListaProdutos.push(novo);
                        this.atualizarProduto(novo, filhosDiretos[i].Fabricante, filhosDiretos[i].Produto, this.selectProdInfo.Qte * filhosDiretos[i].Qtde);
                    }
                }
            }
        }

    }

    public filhosDeProdutoComposto(selectProdInfo: SelectProdInfo) {

        const registros = this.dtoProdutoCombo.ProdutoCompostoDto.filter(
            el => el.PaiFabricante === selectProdInfo.Fabricante && el.PaiProduto === selectProdInfo.Produto);

        if (!registros) {
            return null;
        }
        if (registros.length <= 0) {
            return null;
        }
        return registros[0].Filhos;
    }

    public atualizarProduto(linha: PedidoProdutosPedidoDto, fabricante: string, produto: string, qtde: number) {
        let prodInfo = this.dtoProdutoCombo.ProdutoDto.filter(el => el.Fabricante === fabricante && el.Produto === produto)[0];


        if (!prodInfo) {
            prodInfo = new ProdutoDto();
        }
        linha.Fabricante = fabricante;
        linha.NumProduto = produto;
        linha.Descricao = prodInfo.Descricao_html;
        linha.Qtde = qtde;

        linha.Preco = prodInfo.Preco_lista;
        linha.VlLista = prodInfo.Preco_lista;
        linha.VlUnitario = prodInfo.Preco_lista;

        this.digitouDescValor(linha, linha.Desconto?.toString());
        this.digitouQte(linha);
    }

    public digitouDescValor(i: PedidoProdutosPedidoDto, v: string) {

        if (v == undefined) {
            return;
        }
        
        //se não alteraram nada, ignoramos
        if (i.Desconto.toString() == v && i.Desconto == undefined) {
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
            i.VlUnitario = i.VlLista * (1 - i.Desconto / 100);
            i.VlUnitario = parseFloat(i.VlUnitario.toFixed(2));
        }
        else {
            i.VlUnitario = i.Preco;
        }
        this.digitouQte(i);
    }
         
    public digitouQte(i: PedidoProdutosPedidoDto) {
        let moedaUtils: MoedaUtils = new MoedaUtils();
        //necessário trazer e verificar a variavel "qtde_max_permitida" na tabela "T_produto_loja" 
        //para limitar a qtde de compra para o usuário

        if (i.Qtde <= 0) {
            i.Qtde = 1;
        }
        i.TotalItem = i.VlUnitario * i.Qtde; // VlUnitario = Vl Venda na tela

        
        //$("#totalPedido").text(moedaUtils.formatarMoedaSemPrefixo(i.TotalItem));
        //this.dadosPagto.prepedidoAlterado();

    }

    public digitouDesc(e: Event, i: PedidoProdutosPedidoDto) {
        let valor = ((e.target) as HTMLInputElement).value;
        let v: any = valor.replace(/\D/g, '');
        //tem 1 casa
        v = (v / 10).toFixed(2) + '';
        this.digitouDescValor(i, v);
    }

    public arrumarProdsRepetidos() {
        
        let lp = this.pedidoDto.ListaProdutos;
        for (let i = 0; i < lp.length; i++) {
            let este = lp[i];

            //se tiver algum repetido, tiramos o proximo repetido
            let continaurBuscaRepetido = true;
            while (continaurBuscaRepetido) {
                continaurBuscaRepetido = false;
                for (let irepetido = 0 + 1; irepetido < lp.length; irepetido++) {
                    let repetido = lp[irepetido];
                    if (este.Fabricante === repetido.Fabricante && este.NumProduto == repetido.NumProduto) {
                        //repetido, tem que tirar este!
                        continaurBuscaRepetido = true;
                        este.Qtde += repetido.Qtde;
                        
                        this.pedidoDto.ListaProdutos = this.pedidoDto.ListaProdutos.filter(el => el !== repetido);
                        lp = this.pedidoDto.ListaProdutos;
                        this.digitouQte(este);
                    }
                }
            }
        }
    }

    public estoqueExcedido(i: PedidoProdutosPedidoDto): boolean {
        const item = this.estoqueItem(i);
        //se nao achamos, dizemos que não tem que mostrar a mensagem não...
        if (!item) {
            return false;
        }
        if (item.Estoque < i.Qtde) {
            return true;
        }
        return false;
    }

    //mensagens de estoque
    public estoqueItem(i: PedidoProdutosPedidoDto): ProdutoDto {
        if (!this.dtoProdutoCombo) {
            return null;
        }

        //procuramos esse item
        const item = this.dtoProdutoCombo.ProdutoDto.filter(el => el.Fabricante === i.Fabricante && el.Produto === i.NumProduto);
        if (!item || item.length <= 0) {
            return null;
        }
        //achamos o item
        return item[0];
    }

    public produtoTemAviso(i: PedidoProdutosPedidoDto): boolean {
        const item = this.estoqueItem(i);
        //se nao achamos, dizemos que não tem que mostrar a mensagem não...
        if (!item) {
            return false;
        }
        if (!item.Alertas || item.Alertas.trim() === "") {
            return false;
        }
        return true;
    }

    public msgQtdePermitida: string = "";
    public qtdeVendaPermitida(i: PedidoProdutosPedidoDto): boolean {
        //busca o item na lista
        this.msgQtdePermitida = "";
        
        const item = this.estoqueItem(i);
        if (!item) {
            return false;
        }

        if (i.Qtde > item.Qtde_Max_Venda) {
            this.msgQtdePermitida = "Quantidade solicitada excede a quantidade máxima de venda permitida!";
            return true;
        }
        else
            return false;

    }
}