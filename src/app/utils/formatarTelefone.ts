import { StringUtils } from './stringUtils';

export class FormatarTelefone {
    //     ' ------------------------------------------------------------------------
    // '   TELEFONE_FORMATA
    // ' 
    static telefone_formata(telefone: string): string {
        let s_tel = "";
        s_tel = "" + telefone;
        //s_tel = retorna_so_digitos(s_tel)
        s_tel = StringUtils.retorna_so_digitos(s_tel);


        if (!FormatarTelefone.telefone_ok(s_tel))
            return "";

        let i = s_tel.length - 4;
        s_tel = s_tel.substr(1, i) + "-" + s_tel.substr(i + 1);

        return s_tel;
    }

    static telefone_ddd_formata(telefone: string, ddd: string): string {
        let s = "";
        if (ddd.trim() != "")
            s = "(" + ddd.trim() + ") " + s;
        return s + FormatarTelefone.telefone_formata(telefone);
    }

    // ' ------------------------------------------------------------------------
    // '   TELEFONE OK?
    // ' 
    static telefone_ok(s_tel: string): boolean {
        s_tel = StringUtils.retorna_so_digitos(s_tel);
        if (s_tel.length == 0 || s_tel.length >= 6)
            return true;
        return false;
    }

    // ' ------------------------------------------------------------------------
    // '   DDD OK?
    // ' 
    static ddd_ok(ddd: string): boolean {
        let s_ddd: string = "" + ddd;
        s_ddd = StringUtils.retorna_so_digitos(s_ddd);
        if ((s_ddd.length == 0) || (s_ddd.length == 2)) {
            return true;
        }
        return false;
    }



    //máscara para telefones de 8 ou 9 dígitos
    //usando o angular2-text-mask
    static mascaraTelefone(userInput: string) {
        let numbers = StringUtils.retorna_so_digitos(userInput);
        if (numbers.length > 10) {
            return ['(', /\d/, /\d/, ')', ' ', /\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
        } else {
            return ['(', /\d/, /\d/, ')', ' ', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
        }
    }

    static SepararTelefone(s_tel: string): TelefoneSeparado {
        let numeros = StringUtils.retorna_so_digitos(s_tel);
        let ret = new TelefoneSeparado();
        ret.Ddd = numeros.substr(0, 2);
        ret.Telefone = "";
        if (numeros.length > 2) {
            ret.Telefone = numeros.substr(2);
        }
        return ret;
    }


}


class TelefoneSeparado {
    public Ddd: string;
    public Telefone: string;
}
