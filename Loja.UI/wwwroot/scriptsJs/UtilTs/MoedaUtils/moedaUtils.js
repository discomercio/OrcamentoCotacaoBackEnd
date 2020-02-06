/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var MoedaUtils = /** @class */ (function () {
        function MoedaUtils() {
            this.formatter = new Intl.NumberFormat(undefined, {
                style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2
            });
            this.formatter1casa = new Intl.NumberFormat(undefined, {
                style: 'decimal', minimumFractionDigits: 1, maximumFractionDigits: 1
            });
        }
        MoedaUtils.prototype.formatarMoedaComPrefixo = function (nro) {
            if (!!!nro)
                return "";
            return "R$ " + this.formatter.format(nro);
        };
        MoedaUtils.prototype.formatarMoedaSemPrefixo = function (nro) {
            if (!!!nro)
                return "";
            return this.formatter.format(nro);
        };
        MoedaUtils.prototype.formatarPorcentagemUmaCasa = function (nro) {
            if (!!!nro)
                return "";
            return this.formatter1casa.format(nro);
        };
        MoedaUtils.prototype.teste = function () {
            alert("isso é um teste");
        };
        MoedaUtils.prototype.teste2 = function () {
            alert("esse é o teste2");
        };
        return MoedaUtils;
    }());
    exports.MoedaUtils = MoedaUtils;
});
//# sourceMappingURL=/scriptsJs/UtilTs/MoedaUtils/moedaUtils.js.map