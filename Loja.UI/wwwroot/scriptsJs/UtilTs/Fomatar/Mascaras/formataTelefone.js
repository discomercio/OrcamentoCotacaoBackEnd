/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
define(["require", "exports", "../../stringUtils/stringUtils"], function (require, exports, stringUtils_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var FormatarTelefone = /** @class */ (function () {
        function FormatarTelefone() {
        }
        //     ' ------------------------------------------------------------------------
        // '   TELEFONE_FORMATA
        // ' 
        FormatarTelefone.telefone_formata = function (telefone) {
            var s_tel = "";
            s_tel = "" + telefone;
            //s_tel = retorna_so_digitos(s_tel)
            s_tel = stringUtils_1.StringUtils.retorna_so_digitos(s_tel);
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
            s_tel = stringUtils_1.StringUtils.retorna_so_digitos(s_tel);
            if (s_tel.length == 0 || s_tel.length >= 6)
                return true;
            return false;
        };
        // ' ------------------------------------------------------------------------
        // '   DDD OK?
        // ' 
        FormatarTelefone.ddd_ok = function (ddd) {
            var s_ddd = "" + ddd;
            s_ddd = stringUtils_1.StringUtils.retorna_so_digitos(s_ddd);
            if ((s_ddd.length == 0) || (s_ddd.length == 2)) {
                return true;
            }
            return false;
        };
        //máscara para digitar telefones de 8 ou 9 dígitos
        //usando o angular2-text-mask
        FormatarTelefone.mascaraTelefone = function (userInput) {
            var numbers = stringUtils_1.StringUtils.retorna_so_digitos(userInput);
            if (numbers.length > 10) {
                return ['(', /\d/, /\d/, ')', ' ', /\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
            }
            else {
                return ['(', /\d/, /\d/, ')', ' ', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
            }
        };
        FormatarTelefone.SepararTelefone = function (s_tel) {
            var numeros = stringUtils_1.StringUtils.retorna_so_digitos(s_tel);
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
    exports.FormatarTelefone = FormatarTelefone;
    var TelefoneSeparado = /** @class */ (function () {
        function TelefoneSeparado() {
        }
        return TelefoneSeparado;
    }());
});
//# sourceMappingURL=/scriptsJs/UtilTs/Fomatar/Mascaras/formataTelefone.js.map