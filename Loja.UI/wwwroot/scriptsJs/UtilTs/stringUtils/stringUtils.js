define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var StringUtils = /** @class */ (function () {
        function StringUtils() {
        }
        StringUtils.retorna_so_digitos = function (msg) {
            return msg.replace(/\D/g, "");
        };
        //não é 100% confiável, mas funciona
        //se tiver entrada de usuário, devemos converter por algum element oculto no browser (setando o html e pegando o texto)
        //para casos onde a origem seja confiável, podemos usar esta rotina
        StringUtils.TextoDeHtml = function (html) {
            if (!html)
                return "";
            return html.replace(/<[^>]*>?/gm, '');
        };
        return StringUtils;
    }());
    exports.StringUtils = StringUtils;
});
//# sourceMappingURL=/scriptsJs/UtilTs/stringUtils/stringUtils.js.map