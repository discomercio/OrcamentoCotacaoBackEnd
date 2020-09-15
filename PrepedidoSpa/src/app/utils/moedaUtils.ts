import createNumberMask from 'text-mask-addons/dist/createNumberMask'

export class MoedaUtils {
    //
    formatter = new Intl.NumberFormat('pt-br', {
        style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2
    });
    formatter1casa = new Intl.NumberFormat('pt-br', {
        style: 'decimal', minimumFractionDigits: 1, maximumFractionDigits: 1
    });

    public formatarMoedaComPrefixo(nro: number) {
        if (!!!nro)
            return "";
        return "R$ " + this.formatter.format(nro);
    }

    public formatarMoedaSemPrefixo(nro: number) {
        if (!!!nro)
            return "";
        return this.formatter.format(nro);
    }

    public formatarPorcentagemUmaCasa(nro: number) {
        
        if (!!!nro)
            return "";
        // return this.formatter1casa.format(nro);
        let teste = this.formatter.format(nro);
        return teste;
    }

    public formatarPorcentagemUmaCasaReturnZero(nro: number) {
        if (!!!nro)
            return "0,0";
        return this.formatter1casa.format(nro);
    }
    public formatarValorDuasCasaReturnZero(nro: number) {
        if (!!!nro)
            return "0,00";
        return this.formatter.format(nro);
    }

    public formatarDecimal(valor: number): number {
        return Number.parseFloat(valor.toFixed(2));
    }
    //máscara digitar valores
    //usando o angular2-text-mask
    //e o npm i text-mask-addons --save
    //ops, alteração: nao estamos usando porque não funcionou como a gente queria


    public mascaraValores() {
        return createNumberMask({
            //https://github.com/text-mask/text-mask/tree/master/addons/#readme
            /*
    prefix (string): what to display before the amount. Defaults to '$'.
    suffix (string): what to display after the amount. Defaults to empty string.
    includeThousandsSeparator (boolean): whether or not to separate thousands. Defaults to to true.
    thousandsSeparatorSymbol (string): character with which to separate thousands. Default to ','.
    allowDecimal (boolean): whether or not to allow the user to enter a fraction with the amount. Default to false.
    decimalSymbol (string): character that will act as a decimal point. Defaults to '.'
    decimalLimit (number): how many digits to allow after the decimal. Defaults to 2
    integerLimit (number): limit the length of the integer number. Defaults to null for unlimited
    requireDecimal (boolean): whether or not to always include a decimal point and placeholder for decimal digits after the integer. Defaults to false.
    allowNegative (boolean): whether or not to allow negative numbers. Defaults to false
    allowLeadingZeroes (boolean): whether or not to allow leading zeroes. Defaults to false
    */
            prefix: '',
            suffix: '',
            thousandsSeparatorSymbol: '.',
            decimalSymbol: ',',
            allowDecimal: true,
            requireDecimal: true
        });
    }

}