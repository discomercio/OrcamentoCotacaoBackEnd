define(["require","exports","../../UtilTs/CpfCnpjUtils/CpfCnpjUtils"],function(p,e,t){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var n=$("#cpf_cnpj").val().toString();$("#cpf_cnpj").val(t.CpfCnpjUtils.cnpj_cpf_formata(n))});