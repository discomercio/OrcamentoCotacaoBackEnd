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


        if (!this.telefone_ok(s_tel))
            return "";

        let i = s_tel.length - 4;
        s_tel = s_tel.substr(1, i) + "-" + s_tel.substr(i + 1);

        return s_tel;
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

}