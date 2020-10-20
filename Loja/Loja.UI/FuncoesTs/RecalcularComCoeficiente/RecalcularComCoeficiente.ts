import { DadosPagto } from "../DadosPagto/DadosPagto";
import { Constantes } from "../../UtilTs/Constantes/Constantes";
import { ProdutosCalculados } from "../DadosPagto/EnumTipoPagto";
import { CoeficienteDto } from "../../DtosTs/CoeficienteDto/CoeficienteDto";
import { MoedaUtils } from "../../UtilTs/MoedaUtils/moedaUtils";
import { NovoPedidoDadosService } from "../../Services/NovoPepedidoDadosService";
import { PedidoDto } from "../../DtosTs/PedidoDto/PedidoDto";
import { PedidoProdutosPedidoDto } from "../../DtosTs/PedidoDto/PedidoProdutosPedidoDto";

export class RecalcularComCoeficiente {
    lstProdutos: PedidoProdutosPedidoDto[] = new Array();
    lstCoeficientesCompleta: CoeficienteDto[] = new Array();
    lstCoeficienteFiltrada: CoeficienteDto[][] = new Array(Array());
    pedidoDto = new PedidoDto();


    buscarCoeficienteFornecedores(): CoeficienteDto[][] {

        const distinct = (value, index, self) => {
            return self.indexOf(value) === index;
        }

        //this.dadosPagto.coeficienteDto é uma lista com todos os coeficientes
        //vamos montar uma lista de lista de coeficiente


        let fornecedores: string[] = new Array();

        this.lstProdutos.forEach(element => {
            fornecedores.push(element.Fabricante);
        });

        let fornecDistinct = fornecedores.filter(distinct);

        this.lstCoeficienteFiltrada = new Array();
        fornecDistinct.forEach(y => {
            this.lstCoeficienteFiltrada.push(
                this.lstCoeficientesCompleta.filter(x => x.Fabricante == y));
        });


        return this.lstCoeficienteFiltrada;

    }
    novoPrepedidoDadosService: NovoPedidoDadosService;
    constantes = new Constantes();
    lstProdutosCalculados: ProdutosCalculados[];
    ProdutosCalculados: ProdutosCalculados;
    permiteRAStatus: number;
    CalcularTotalProdutosComCoeficiente(enumFP: number, coeficienteDtoNovo: CoeficienteDto[][],
        tipoFormaPagto: string, qtdeParcVisa: number, vlEntrada: number): string[] {

        this.lstProdutosCalculados = new Array();
        this.ProdutosCalculados = new ProdutosCalculados();
        let lstMsg: string[] = new Array();
        let lstCoeficiente = coeficienteDtoNovo;
        //let lstProdutos = this.novoPrepedidoDadosService.pedidoDto.ListaProdutos;
        let alterouValorRA: boolean = false;
        let totalProduto = 0;

        let coeficienteFornec: CoeficienteDto[];
        //vamos calcular os produtos com os respectivos coeficientes e atribuir a uma variavel de total do prepedido

        let cont = 0;
        if (this.lstProdutos.length > 0) {
            lstCoeficiente.forEach(element => {
                this.lstProdutos.forEach(p => {

                    if (!!tipoFormaPagto && !!enumFP) {

                        //precisamos verificar se o valor de ra foi alterado para calcular as parcelas com base no valor digitado


                        //A vista
                        if (tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA &&
                            enumFP.toString() == this.constantes.COD_FORMA_PAGTO_A_VISTA) {
                            coeficienteFornec = element.filter(x => x.Fabricante == p.Fabricante);
                            if (!!coeficienteFornec[0]) {
                                if (coeficienteFornec[0].Fabricante == p.Fabricante) {
                                    this.ProdutosCalculados = new ProdutosCalculados();

                                    if (!!this.permiteRAStatus && this.permiteRAStatus == 1) {
                                        p.VlTotalItem = p.AlterouValorRa && p.AlterouValorRa != undefined ? p.Preco_Lista * p.Qtde : p.Preco * p.Qtde;
                                        this.ProdutosCalculados.QtdeParcela = 1;
                                        this.ProdutosCalculados.Valor = p.VlTotalItem;
                                        this.lstProdutosCalculados.push(this.ProdutosCalculados);
                                    } else {
                                        if (p.Desconto > 0) {
                                            let totalproduto = p.Preco;
                                            let desconto = (totalproduto - p.VlUnitario) * 100 / p.Preco;
                                            p.VlTotalItem = (totalproduto - ((totalproduto * desconto) / 100)) * p.Qtde;
                                            this.ProdutosCalculados.QtdeParcela = 1;
                                            this.ProdutosCalculados.Valor = p.VlTotalItem;
                                            this.lstProdutosCalculados.push(this.ProdutosCalculados);
                                        } else {
                                            p.VlTotalItem = p.Preco * p.Qtde;
                                            this.ProdutosCalculados.QtdeParcela = 1;
                                            this.ProdutosCalculados.Valor = p.VlTotalItem;
                                            this.lstProdutosCalculados.push(this.ProdutosCalculados);
                                        }
                                    }

                                    return;

                                }
                            }
                        }

                        //Parcela única
                        if (tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
                            enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELA_UNICA) {
                            totalProduto = (p.Preco * p.Qtde) * (1 - p.Desconto / 100);

                            coeficienteFornec = element.filter(x => x.Fabricante == p.Fabricante &&
                                x.TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA);

                            if (!!coeficienteFornec[0]) {
                                if (coeficienteFornec[0].Fabricante == p.Fabricante) {
                                    this.ProdutosCalculados = new ProdutosCalculados();

                                    if (!!this.permiteRAStatus && this.permiteRAStatus == 1) {
                                        p.VlTotalItem = p.AlterouValorRa && p.AlterouValorRa != undefined ?
                                            (p.Preco_Lista * p.Qtde) : (p.Preco * coeficienteFornec[0].Coeficiente) * p.Qtde;
                                        this.ProdutosCalculados.QtdeParcela = coeficienteFornec[0].QtdeParcelas;
                                        this.ProdutosCalculados.Valor = p.VlTotalItem;
                                        this.lstProdutosCalculados.push(this.ProdutosCalculados);
                                    }
                                    else {
                                        if (p.Desconto > 0) {
                                            let totalproduto = p.Preco * coeficienteFornec[0].Coeficiente;
                                            let desconto = (totalproduto - p.VlUnitario) * 100 /
                                                (p.Preco * coeficienteFornec[0].Coeficiente);
                                            p.VlTotalItem = (totalproduto - ((totalproduto * desconto) / 100)) * p.Qtde;
                                            this.ProdutosCalculados.QtdeParcela = coeficienteFornec[0].QtdeParcelas;
                                            this.ProdutosCalculados.Valor = p.VlTotalItem;
                                            this.lstProdutosCalculados.push(this.ProdutosCalculados);
                                        }
                                        else {
                                            p.VlTotalItem = (p.Preco * coeficienteFornec[0].Coeficiente) * p.Qtde;
                                            this.ProdutosCalculados.QtdeParcela = coeficienteFornec[0].QtdeParcelas;
                                            this.ProdutosCalculados.Valor = p.VlTotalItem;
                                            this.lstProdutosCalculados.push(this.ProdutosCalculados);

                                        }
                                    }

                                    return;
                                }
                            }
                        }
                        //Parcela com entrada
                        if (tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
                            enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA) {
                            if (!!vlEntrada && vlEntrada != 0.00) {

                                this.vlEntrada = vlEntrada;

                                totalProduto = (p.Preco * p.Qtde) * (1 - p.Desconto / 100);

                                coeficienteFornec = element.filter(x => x.Fabricante == p.Fabricante &&
                                    x.TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA);

                                coeficienteFornec.forEach(x => {
                                    this.ProdutosCalculados = new ProdutosCalculados();
                                    if (!!this.permiteRAStatus && this.permiteRAStatus == 1) {
                                        p.VlTotalItem = p.AlterouValorRa && p.AlterouValorRa != undefined ? (p.Preco_Lista * p.Qtde) :
                                            (p.Preco * x.Coeficiente) * p.Qtde;
                                        this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                                        this.ProdutosCalculados.Valor = p.VlTotalItem;
                                        this.lstProdutosCalculados.push(this.ProdutosCalculados);
                                    } else {
                                        if (p.Desconto > 0) {
                                            let totalproduto = p.Preco * x.Coeficiente;
                                            let desconto = (totalproduto - p.VlUnitario) * 100 /
                                                (p.Preco * x.Coeficiente);
                                            p.VlTotalItem = (totalproduto - ((totalproduto * desconto) / 100)) * p.Qtde;
                                            this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                                            this.ProdutosCalculados.Valor = p.VlTotalItem;
                                            this.lstProdutosCalculados.push(this.ProdutosCalculados);
                                        }
                                        else {
                                            p.VlTotalItem = (p.Preco * x.Coeficiente) * p.Qtde;
                                            this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                                            this.ProdutosCalculados.Valor = p.VlTotalItem;
                                            this.lstProdutosCalculados.push(this.ProdutosCalculados);

                                        }
                                    }


                                });
                            }
                        }

                        //cartão internet e maquineta
                        if (tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
                            (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO ||
                                enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)) {

                            coeficienteFornec = element.filter(x => x.Fabricante == p.Fabricante &&
                                x.TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA);

                            coeficienteFornec.forEach(x => {
                                this.ProdutosCalculados = new ProdutosCalculados();
                                if (!!this.permiteRAStatus && this.permiteRAStatus == 1) {
                                    p.VlTotalItem = p.AlterouValorRa && p.AlterouValorRa != undefined ? (p.Preco_Lista * p.Qtde) :
                                        (p.Preco * x.Coeficiente) * p.Qtde;
                                    this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                                    this.ProdutosCalculados.Valor = p.VlTotalItem;
                                    this.lstProdutosCalculados.push(this.ProdutosCalculados);
                                } else {
                                    if (p.Desconto > 0) {
                                        let totalproduto = p.Preco * x.Coeficiente;
                                        let desconto = (totalproduto - p.VlUnitario) * 100 /
                                            (p.Preco * x.Coeficiente);
                                        p.VlTotalItem = (totalproduto - ((totalproduto * desconto) / 100)) * p.Qtde;
                                        this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                                        this.ProdutosCalculados.Valor = p.VlTotalItem;
                                        this.lstProdutosCalculados.push(this.ProdutosCalculados);
                                    }
                                    else {
                                        p.VlTotalItem = (p.Preco * x.Coeficiente) * p.Qtde;
                                        this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                                        this.ProdutosCalculados.Valor = p.VlTotalItem;
                                        this.lstProdutosCalculados.push(this.ProdutosCalculados);

                                    }
                                }


                            });
                        }
                    }

                })
                //vamos verificar se é avista para sair do foreach do coeficiente que não é utilizado para este tipo
                if (!!enumFP)
                    if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_A_VISTA) {
                        return false;
                    }

            })
        }
        else {
            //afazer: trocar para msg da mosal
            //this.alertaService.mostrarMensagem("Favor selecionar um Produto!");
            return;
        }
        //precisamos retornar a lista de string para mostrar no Parcelamento
        lstMsg = this.CalcularTotalOrcamento(qtdeParcVisa, enumFP);

        return lstMsg;
    }

    //valor de entrada
    vlEntrada: number;

    moedaUtils = new MoedaUtils();
    CalcularTotalOrcamento(qtdeParcVisa: number, enumFP: number): string[] {
        let lstMsg: string[] = new Array();
        if (!!enumFP) {
            if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_A_VISTA) {

                let filtrarParcela = this.lstProdutosCalculados.filter(x => x.QtdeParcela == 1);
                let valorTotalParc = filtrarParcela.reduce((sum, prod) => sum + prod.Valor, 0);
                lstMsg.push(this.moedaUtils.formatarMoedaComPrefixo(valorTotalParc));
                // "1 X " +
            }
            else {
                if (!!qtdeParcVisa) {

                    for (let i = 0; i <= qtdeParcVisa; i++) {
                        let filtrarParcela = this.lstProdutosCalculados.filter(x => x.QtdeParcela == i);

                        let valorTotalParc = parseFloat(filtrarParcela.reduce((sum, prod) => sum + prod.Valor, 0).toFixed(2));

                        if (!!valorTotalParc) {
                            if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA) {
                                //vamos testar para saber se parcela com entrada
                                if (!!this.vlEntrada) {
                                    if (this.vlEntrada > valorTotalParc) {
                                        //afazer: alterar para chamar a modal
                                        //this.alertaService.mostrarMensagem("Valor da entrada é maior que o total do Pré-pedido!");
                                        this.vlEntrada = null;
                                        lstMsg = new Array();
                                        return;
                                    }
                                    else {
                                        valorTotalParc = valorTotalParc - this.vlEntrada;
                                    }
                                }
                            }
                            lstMsg.push(i + " X " +
                                this.moedaUtils.formatarMoedaComPrefixo(parseFloat((valorTotalParc / i).toFixed(2))));
                        }
                    }
                }
            }
        }
        return lstMsg;
    }

    //vamos alterar os valores dos produtos que estão no serviço
    RecalcularListaProdutos(enumFP: number, coeficienteDtoNovo: CoeficienteDto[][],
        tipoFormaPagto: string, qtdeParc: number): void {

        let coeficiente: CoeficienteDto[];
        //Aqui iremos verificar se
        if (!!enumFP) {
            if (this.lstProdutos.length > 0) {
                coeficienteDtoNovo.forEach(coef => {

                    //vamos alterar diretamente no serviço
                    this.lstProdutos.forEach(produto => {

                        //vamos verificar se é pagto á vista
                        if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_A_VISTA &&
                            tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA) {

                            if (produto.AlterouVlVenda) {
                                //devemos alterar o valor de desconto
                                //valor com coeficiente - valor venda * 100 / valor com coeficiente
                                produto.Desconto = (produto.Preco - produto.VlUnitario) * 100 / produto.Preco;
                                produto.TotalItem = (produto.Preco * produto.Qtde) * (1 - produto.Desconto / 100);
                                produto.VlTotalItem = (produto.Preco * produto.Qtde) * (1 - produto.Desconto / 100);
                                produto.VlLista = produto.Preco;//só altera se calcular coeficiente

                            } else {
                                produto.VlUnitario = produto.Preco;
                                produto.VlTotalItem = (produto.Preco * produto.Qtde);
                                produto.VlLista = produto.Preco;//só altera se calcular coeficiente
                                produto.TotalItem = (produto.Preco * produto.Qtde);
                            }
                            if (!produto.AlterouValorRa || produto.AlterouValorRa == undefined) {
                                produto.Preco_Lista = produto.Preco;
                            }
                        }

                        //vamos testar se é pagto parcela única
                        if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELA_UNICA &&
                            tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {

                            //vamos filtrar o coeficiente
                            coeficiente = coef.filter(x => x.Fabricante == produto.Fabricante &&
                                x.TipoParcela == tipoFormaPagto && x.QtdeParcelas == qtdeParc);

                            coeficiente.forEach(x => {
                                if (produto.AlterouVlVenda) {

                                    produto.Desconto = ((produto.Preco * x.Coeficiente) - produto.VlUnitario) * 100 /
                                        (produto.Preco * x.Coeficiente);
                                    produto.TotalItem = produto.VlUnitario * produto.Qtde;
                                    produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                                }
                                else {
                                    produto.VlUnitario = (produto.Preco * x.Coeficiente);
                                    produto.VlTotalItem = (produto.Preco * x.Coeficiente);
                                    produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                                    produto.TotalItem = ((produto.Preco * produto.Qtde) * x.Coeficiente);
                                }

                                if (!produto.AlterouValorRa || produto.AlterouValorRa == undefined) {
                                    produto.Preco_Lista = (produto.Preco * x.Coeficiente);
                                }

                            });
                        }

                        //vamos testar se é pagto com entrada 
                        if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA &&
                            tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {

                            coeficiente = coef.filter(x => x.Fabricante == produto.Fabricante &&
                                x.TipoParcela == tipoFormaPagto && x.QtdeParcelas == qtdeParc);

                            coeficiente.forEach(x => {
                                if (produto.AlterouVlVenda) {

                                    produto.Desconto = ((produto.Preco * x.Coeficiente) - produto.VlUnitario) * 100 /
                                        (produto.Preco * x.Coeficiente);
                                    produto.TotalItem = produto.VlUnitario * produto.Qtde;
                                    produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                                }
                                else {
                                    produto.VlUnitario = (produto.Preco * x.Coeficiente);
                                    produto.VlTotalItem = (produto.Preco * x.Coeficiente);
                                    produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                                    produto.TotalItem = ((produto.Preco * produto.Qtde) * x.Coeficiente);
                                }
                                if (!produto.AlterouValorRa || produto.AlterouValorRa == undefined) {
                                    produto.Preco_Lista = (produto.Preco * x.Coeficiente);
                                }

                            });
                        }

                        //vamos testar se é pagto com Cartão e Maquininha
                        if ((enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO ||
                            enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA) &&
                            tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {

                            //preciso pegar a qtde de parcelas que foi selecionado

                            coeficiente = coef.filter(x => x.Fabricante == produto.Fabricante &&
                                x.TipoParcela == tipoFormaPagto && x.QtdeParcelas == qtdeParc);

                            coeficiente.forEach(x => {
                                if (produto.AlterouVlVenda) {

                                    produto.Desconto = ((produto.Preco * x.Coeficiente) - produto.VlUnitario) * 100 /
                                        (produto.Preco * x.Coeficiente);
                                    produto.TotalItem = produto.VlUnitario * produto.Qtde;
                                    produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                                }
                                else {
                                    produto.VlUnitario = (produto.Preco * x.Coeficiente);
                                    produto.VlTotalItem = (produto.Preco * x.Coeficiente);
                                    produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                                    produto.TotalItem = ((produto.Preco * produto.Qtde) * x.Coeficiente);
                                }
                                if (!produto.AlterouValorRa || produto.AlterouValorRa == undefined) {
                                    produto.Preco_Lista = (produto.Preco * x.Coeficiente);
                                }

                            });
                        }
                    });
                });
            }
        }
    }
}