class MoedaUtils {
    constructor() {
        this.formatter = new Intl.NumberFormat(undefined, {
            style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2
        });
        this.formatter1casa = new Intl.NumberFormat(undefined, {
            style: 'decimal', minimumFractionDigits: 1, maximumFractionDigits: 1
        });
    }
    formatarMoedaComPrefixo(nro) {
        if (!!!nro)
            return "";
        return "R$ " + this.formatter.format(nro);
    }
    formatarMoedaSemPrefixo(nro) {
        if (!!!nro)
            return "";
        return this.formatter.format(nro);
    }
    formatarPorcentagemUmaCasa(nro) {
        if (!!!nro)
            return "";
        return this.formatter1casa.format(nro);
    }
    teste() {
        alert("isso é um teste");
    }
    teste2() {
        alert("esse é o teste2");
    }
}
//# sourceMappingURL=moedaUtils.js.map