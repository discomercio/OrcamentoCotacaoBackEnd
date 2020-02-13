import { StringUtils } from "../../UtilTs/stringUtils/stringUtils";
import { CpfCnpjUtils } from "../../UtilTs/CpfCnpjUtils/CpfCnpjUtils";



declare var stringUtils: StringUtils;
declare var cpfcnpjUtils: CpfCnpjUtils;

debugger;
let cpfCnpj = $('#cpf_cnpj').val().toString();
$('#cpf_cnpj').val(CpfCnpjUtils.cnpj_cpf_formata(cpfCnpj));