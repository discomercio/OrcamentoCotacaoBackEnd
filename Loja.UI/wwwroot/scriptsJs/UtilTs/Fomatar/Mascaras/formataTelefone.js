///<reference path="../../stringUtils/stringUtils.ts>
class FormatarTelefone {
    //     ' ------------------------------------------------------------------------
    // '   TELEFONE_FORMATA
    // ' 
    static telefoneCelular(telefone, ddd) {
        if (!telefone || !ddd) {
            return "";
        }
        let s2 = "";
        if (!telefone || !ddd)
            return "";
        if (telefone.trim() == "" || ddd.trim() == "")
            return "";
        s2 = FormatarTelefone.telefone_formata(telefone);
        let s_aux = ddd.trim();
        if (s_aux != "")
            s2 = "(" + s_aux + ") " + s2;
        return s2;
    }
    static telefone_formata(telefone) {
        let s_tel = "";
        s_tel = "" + telefone;
        //s_tel = retorna_so_digitos(s_tel)
        s_tel = StringUtils.retorna_so_digitos(s_tel);
        if (!FormatarTelefone.telefone_ok(s_tel))
            return "";
        let i = s_tel.length - 4;
        s_tel = s_tel.substr(0, i) + "-" + s_tel.substr(i);
        return s_tel;
    }
    static telefone_ddd_formata(telefone, ddd) {
        let s = "";
        if (ddd.trim() != "")
            s = "(" + ddd.trim() + ") " + s;
        return s + FormatarTelefone.telefone_formata(telefone);
    }
    // ' ------------------------------------------------------------------------
    // '   TELEFONE OK?
    // ' 
    static telefone_ok(s_tel) {
        s_tel = StringUtils.retorna_so_digitos(s_tel);
        if (s_tel.length == 0 || s_tel.length >= 6)
            return true;
        return false;
    }
    // ' ------------------------------------------------------------------------
    // '   DDD OK?
    // ' 
    static ddd_ok(ddd) {
        let s_ddd = "" + ddd;
        s_ddd = StringUtils.retorna_so_digitos(s_ddd);
        if ((s_ddd.length == 0) || (s_ddd.length == 2)) {
            return true;
        }
        return false;
    }
    //máscara para digitar telefones de 8 ou 9 dígitos
    //usando o angular2-text-mask
    static mascaraTelefone(userInput) {
        if (!userInput)
            userInput = "";
        let numbers = StringUtils.retorna_so_digitos(userInput);
        if (numbers.length > 10) {
            return ['(', /\d/, /\d/, ')', ' ', /\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
        }
        else {
            return ['(', /\d/, /\d/, ')', ' ', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
        }
    }
    static SepararTelefone(s_tel) {
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
}
//# sourceMappingURL=/scriptsJs/UtilTs/Fomatar/Mascaras/formataTelefone.js.map