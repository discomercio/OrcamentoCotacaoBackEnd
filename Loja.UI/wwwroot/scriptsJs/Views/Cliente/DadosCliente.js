/* nao editar, arquivo compilado pelo typescript*/ 
define(["require", "exports", "../../UtilTs/CpfCnpjUtils/CpfCnpjUtils"], function (require, exports, CpfCnpjUtils_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    debugger;
    var cpfCnpj = $('#cpf_cnpj').val().toString();
    $('#cpf_cnpj').val(CpfCnpjUtils_1.CpfCnpjUtils.cnpj_cpf_formata(cpfCnpj));
});
//# sourceMappingURL=/scriptsJs/Views/Cliente/DadosCliente.js.map