/* nao editar, arquivo compilado pelo typescript*/ 
define(["require", "exports", "../stringUtils/stringUtils"], function (require, exports, stringUtils_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var CpfCnpjUtils = /** @class */ (function () {
        function CpfCnpjUtils() {
        }
        //copiado de http://www.receita.fazenda.gov.br/aplicacoes/atcta/cpf/funcoes.js
        //Verifica se CPF é válido
        CpfCnpjUtils.prototype.ReceitaFederalTestaCPF = function (strCPF) {
            var Soma;
            var Resto;
            Soma = 0;
            //strCPF  = RetiraCaracteresInvalidos(strCPF,11);
            if (strCPF == "00000000000")
                return false;
            for (var i = 1; i <= 9; i++)
                Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
            Resto = (Soma * 10) % 11;
            if ((Resto == 10) || (Resto == 11))
                Resto = 0;
            if (Resto != parseInt(strCPF.substring(9, 10)))
                return false;
            Soma = 0;
            for (var i = 1; i <= 10; i++)
                Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
            Resto = (Soma * 10) % 11;
            if ((Resto == 10) || (Resto == 11))
                Resto = 0;
            if (Resto != parseInt(strCPF.substring(10, 11)))
                return false;
            return true;
        };
        //vamos usar estes:
        CpfCnpjUtils.cnpj_cpf_ok = function (cnpj_cpf) {
            var s;
            s = "" + cnpj_cpf;
            s = stringUtils_1.StringUtils.retorna_so_digitos(cnpj_cpf);
            if (s.length == 11) {
                if (CpfCnpjUtils.cpf_ok(s))
                    return true;
            }
            else if (s.length == 14) {
                if (CpfCnpjUtils.cnpj_ok(s))
                    return true;
            }
            else if (s.length == 0) {
                return true;
            }
            return false;
        };
        CpfCnpjUtils.cpf_ok = function (cpf) {
            var d, i, tudo_igual;
            var s_cpf;
            s_cpf = "" + cpf;
            s_cpf = stringUtils_1.StringUtils.retorna_so_digitos(s_cpf);
            // VERIFICA OS 'CHECK DIGITS' DO CPF
            if (s_cpf == "")
                return true;
            if (s_cpf.length != 11)
                return false;
            // DÍGITOS TODOS IGUAIS?
            tudo_igual = true;
            for (i = 0; i < (s_cpf.length - 1); i++)
                if (s_cpf.substring(i, i + 1) != s_cpf.substring(i + 1, i + 2)) {
                    tudo_igual = false;
                    break;
                }
            if (tudo_igual)
                return false;
            //  VERIFICA O PRIMEIRO CHECK DIGIT
            d = 0;
            for (i = 1; i <= 9; i++)
                d = d + (11 - i) * parseInt(s_cpf.substring(i - 1, i), 10);
            d = 11 - (d % 11);
            if (d > 9)
                d = 0;
            if (d != parseInt(s_cpf.substring(9, 10), 10))
                return false;
            // VERIFICA O SEGUNDO CHECK DIGIT
            d = 0;
            for (i = 2; i <= 10; i++)
                d = d + (12 - i) * parseInt(s_cpf.substring(i - 1, i), 10);
            d = 11 - (d % 11);
            if (d > 9)
                d = 0;
            if (d != parseInt(s_cpf.substring(10, 11), 10))
                return false;
            return true;
        };
        CpfCnpjUtils.cnpj_ok = function (cnpj) {
            var d, i, p1, p2, tudo_igual;
            var s_cnpj = "";
            s_cnpj = "" + cnpj;
            s_cnpj = stringUtils_1.StringUtils.retorna_so_digitos(s_cnpj);
            p1 = "543298765432";
            p2 = "6543298765432";
            if (s_cnpj == "")
                return true;
            if (s_cnpj.length != 14)
                return false;
            // DÍGITOS TODOS IGUAIS?
            tudo_igual = true;
            for (i = 0; i < (s_cnpj.length - 1); i++)
                if (s_cnpj.substring(i, i + 1) != s_cnpj.substring(i + 1, i + 2)) {
                    tudo_igual = false;
                    break;
                }
            if (tudo_igual)
                return false;
            // VERIFICA O PRIMEIRO CHECK DIGIT
            d = 0;
            for (i = 0; i < 12; i++)
                d = d + parseInt(p1.substring(i, i + 1), 10) * parseInt(s_cnpj.substring(i, i + 1), 10);
            d = 11 - (d % 11);
            if (d > 9)
                d = 0;
            if (d != parseInt(s_cnpj.substring(12, 13), 10))
                return false;
            // VERIFICA O SEGUNDO CHECK DIGIT
            d = 0;
            for (i = 0; i < 13; i++)
                d = d + parseInt(p2.substring(i, i + 1), 10) * parseInt(s_cnpj.substring(i, i + 1), 10);
            d = 11 - (d % 11);
            if (d > 9)
                d = 0;
            if (d != parseInt(s_cnpj.substring(13, 14), 10))
                return false;
            return true;
        };
        CpfCnpjUtils.cnpj_cpf_formata = function (cnpj_cpf) {
            var s = "" + cnpj_cpf;
            s = stringUtils_1.StringUtils.retorna_so_digitos(s);
            if (s.length == 11) {
                s = this.cpf_formata(s);
            }
            else if (s.length == 14) {
                s = this.cnpj_formata(s);
            }
            return s;
        };
        CpfCnpjUtils.cnpj_formata = function (cnpj) {
            var s_cnpj = "" + cnpj;
            s_cnpj = stringUtils_1.StringUtils.retorna_so_digitos(s_cnpj);
            if ((s_cnpj == "") || (!this.cnpj_ok(s_cnpj)))
                return s_cnpj;
            s_cnpj = s_cnpj.substring(0, 2) + "." + s_cnpj.substring(2, 5) + "." + s_cnpj.substring(5, 8) + "/" + s_cnpj.substring(8, 12) + "-" + s_cnpj.substring(12, 14);
            return s_cnpj;
        };
        CpfCnpjUtils.cpf_formata = function (cpf) {
            var s_cpf = "" + cpf;
            s_cpf = stringUtils_1.StringUtils.retorna_so_digitos(s_cpf);
            if ((s_cpf == "") || (!this.cpf_ok(s_cpf)))
                return s_cpf;
            s_cpf = s_cpf.substring(0, 3) + "." + s_cpf.substring(3, 6) + "." + s_cpf.substring(6, 9) + "/" + s_cpf.substring(9, 11);
            return s_cpf;
        };
        return CpfCnpjUtils;
    }());
    exports.CpfCnpjUtils = CpfCnpjUtils;
});
//# sourceMappingURL=/scriptsJs/UtilTs/CpfCnpjUtils/CpfCnpjUtils.js.map