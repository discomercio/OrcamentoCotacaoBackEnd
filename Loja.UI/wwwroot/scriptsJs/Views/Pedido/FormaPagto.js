/// <reference path="../../DtosTs/DtoCoeficiente/DtoCoeficiente.ts" />
/// <reference path="../../DtosTs/DtoPedido/DtoPedidoProdutosPedido.ts" />
/// <reference path="../../UtilTs/Constantes/Constantes.ts" />
/// <reference path="../../UtilTs/MoedaUtils/moedaUtils.ts" />
//variaveis referentes a tela
let opcaoPagtoAvista;
let opcaoPagtoParcUnica;
let opcaoPagtoParcComEntrada;
let opcaoPagtoParcCartaoInternet;
let opcaoPagtoParcCartaoMaquineta;
let meioPagtoEntrada;
let meioPagtoAVista;
let meioPagtoEntradaPrest;
let diasVenc; //pagto com entrada, dias para vencimento
let meioPagtoParcUnica;
let diasVencParcUnica;
let qtde; //qtde de parcelas
let valor; //valor da parcela
let qtdeParcVisa; //qtde de parcelas 
let vlEntrada;
class FormaPagto {
    constructor() {
        this.lstMsg = [];
        this.lstCoeficiente = [];
        this.lstProdutos = [];
    }
    montarListaParcelamento(enumFP) {
        this.lstMsg = new Array();
        //let lstCoeficiente = this.coeficienteDto;
        let vlTotalPedido = this.lstProdutos.reduce((sum, prod) => sum + prod.TotalItem, 0);
        let cont = 0;
        if (enumFP) {
            for (let i = 0; i < this.lstCoeficiente.length; i++) {
                if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_A_VISTA) {
                    this.lstMsg.push(this.lstCoeficiente[i].QtdeParcelas + " X " +
                        this.moedaUtils.formatarMoedaComPrefixo(vlTotalPedido / this.lstCoeficiente[i].QtdeParcelas));
                    opcaoPagtoAvista = this.lstMsg[i];
                    break;
                }
                else if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELA_UNICA &&
                    this.lstCoeficiente[i].TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
                    this.lstMsg.push(this.lstCoeficiente[i].QtdeParcelas + " X " +
                        this.moedaUtils.formatarMoedaComPrefixo(vlTotalPedido / this.lstCoeficiente[i].QtdeParcelas));
                    opcaoPagtoParcUnica = this.lstMsg[0];
                    break;
                }
                else if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA &&
                    this.lstCoeficiente[i].TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {
                    if (cont < qtdeParcVisa) {
                        this.lstMsg.push(this.lstCoeficiente[i].QtdeParcelas + " X " +
                            this.moedaUtils.formatarMoedaComPrefixo(((vlTotalPedido - vlEntrada) / this.lstCoeficiente[i].QtdeParcelas)));
                        cont++;
                    }
                }
                else if ((enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA ||
                    enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO ||
                    enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA) &&
                    this.lstCoeficiente[i].TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
                    if (cont < qtdeParcVisa) {
                        cont++;
                        this.lstMsg.push(this.lstCoeficiente[i].QtdeParcelas + " X " +
                            this.moedaUtils.formatarMoedaComPrefixo(vlTotalPedido / this.lstCoeficiente[i].QtdeParcelas));
                    }
                }
            }
        }
        return this.lstMsg;
    }
}
//# sourceMappingURL=/scriptsJs/Views/Pedido/FormaPagto.js.map