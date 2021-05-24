import { CpfCnpjUtils } from "../../../UtilTs/CpfCnpjUtils/CpfCnpjUtils";

let cpfCnpj = $('#cpf_cnpj').val().toString();
$('#cpf_cnpj').val(CpfCnpjUtils.cnpj_cpf_formata(cpfCnpj));
$("#cpf_cnpj").prop("readonly", true);
