///<reference path="../../stringUtils/stringUtils.ts>
var FormatarTelefone = /** @class */ (function () {
    function FormatarTelefone() {
    }
    //     ' ------------------------------------------------------------------------
    // '   TELEFONE_FORMATA
    // ' 
    FormatarTelefone.telefoneCelular = function (telefone, ddd) {
        if (!telefone || !ddd) {
            return "";
        }
        var s2 = "";
        if (!telefone || !ddd)
            return "";
        if (telefone.trim() == "" || ddd.trim() == "")
            return "";
        s2 = FormatarTelefone.telefone_formata(telefone);
        var s_aux = ddd.trim();
        if (s_aux != "")
            s2 = "(" + s_aux + ") " + s2;
        return s2;
    };
    FormatarTelefone.telefone_formata = function (telefone) {
        var s_tel = "";
        s_tel = "" + telefone;
        //s_tel = retorna_so_digitos(s_tel)
        s_tel = StringUtils.retorna_so_digitos(s_tel);
        if (!FormatarTelefone.telefone_ok(s_tel))
            return "";
        var i = s_tel.length - 4;
        s_tel = s_tel.substr(0, i) + "-" + s_tel.substr(i);
        return s_tel;
    };
    FormatarTelefone.telefone_ddd_formata = function (telefone, ddd) {
        var s = "";
        if (ddd.trim() != "")
            s = "(" + ddd.trim() + ") " + s;
        return s + FormatarTelefone.telefone_formata(telefone);
    };
    // ' ------------------------------------------------------------------------
    // '   TELEFONE OK?
    // ' 
    FormatarTelefone.telefone_ok = function (s_tel) {
        s_tel = StringUtils.retorna_so_digitos(s_tel);
        if (s_tel.length == 0 || s_tel.length >= 6)
            return true;
        return false;
    };
    // ' ------------------------------------------------------------------------
    // '   DDD OK?
    // ' 
    FormatarTelefone.ddd_ok = function (ddd) {
        var s_ddd = "" + ddd;
        s_ddd = StringUtils.retorna_so_digitos(s_ddd);
        if ((s_ddd.length == 0) || (s_ddd.length == 2)) {
            return true;
        }
        return false;
    };
    //máscara para digitar telefones de 8 ou 9 dígitos
    //usando o angular2-text-mask
    FormatarTelefone.mascaraTelefone = function (userInput) {
        if (!userInput)
            userInput = "";
        var numbers = StringUtils.retorna_so_digitos(userInput);
        if (numbers.length > 10) {
            return ['(', /\d/, /\d/, ')', ' ', /\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
        }
        else {
            return ['(', /\d/, /\d/, ')', ' ', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
        }
    };
    FormatarTelefone.SepararTelefone = function (s_tel) {
        var numeros = StringUtils.retorna_so_digitos(s_tel);
        var ret = new TelefoneSeparado();
        ret.Ddd = numeros.substr(0, 2);
        ret.Telefone = "";
        if (numeros.length > 2) {
            ret.Telefone = numeros.substr(2);
        }
        return ret;
    };
    return FormatarTelefone;
}());
var TelefoneSeparado = /** @class */ (function () {
    function TelefoneSeparado() {
    }
    return TelefoneSeparado;
}());
//# sourceMappingURL=formataTelefone.js.map