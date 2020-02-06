/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    //variaveis referentes a tela
    var opcaoPagtoAvista;
    var opcaoPagtoParcUnica;
    var opcaoPagtoParcComEntrada;
    var opcaoPagtoParcCartaoInternet;
    var opcaoPagtoParcCartaoMaquineta;
    var meioPagtoEntrada;
    var meioPagtoAVista;
    var meioPagtoEntradaPrest;
    var diasVenc; //pagto com entrada, dias para vencimento
    var meioPagtoParcUnica;
    var diasVencParcUnica;
    var qtde; //qtde de parcelas
    var valor; //valor da parcela
    var qtdeParcVisa; //qtde de parcelas 
    var vlEntrada;
    var FormaPagto = /** @class */ (function () {
        function FormaPagto() {
            this.lstMsg = [];
            this.lstCoeficiente = [];
            this.lstProdutos = [];
        }
        FormaPagto.prototype.montarListaParcelamento = function (enumFP) {
            this.lstMsg = new Array();
            //let lstCoeficiente = this.coeficienteDto;
            var vlTotalPedido = this.lstProdutos.reduce(function (sum, prod) { return sum + prod.TotalItem; }, 0);
            var cont = 0;
            if (enumFP) {
                for (var i = 0; i < this.lstCoeficiente.length; i++) {
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
        };
        return FormaPagto;
    }());
    exports.FormaPagto = FormaPagto;
});
//# sourceMappingURL=/scriptsJs/Views/Pedido/FormaPagto.js.map