export class MoedaUtils {
    formatter = new Intl.NumberFormat(undefined, {
        style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2
    });

    public formatarMoeda(nro: number) {
        if (!!!nro)
            return "";
        return "R$ " + this.formatter.format(nro);
    }
}