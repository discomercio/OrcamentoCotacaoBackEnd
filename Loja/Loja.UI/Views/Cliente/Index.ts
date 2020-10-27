import { CpfCnpjUtils } from "../../UtilTs/CpfCnpjUtils/CpfCnpjUtils";
import { StringUtils } from "../../UtilTs/stringUtils/stringUtils";


declare var window: any;
declare function swal(header, mensagem): any;

window.ValidarCpfCnpj = () => {
    if ($("#cpf_cnpj").val() != "") {
        let cpfCnpj :string  = $("#cpf_cnpj").val().toString();

        if (!CpfCnpjUtils.cnpj_cpf_ok(cpfCnpj)) {
            swal("Erro", "CNPJ/CPF inválido.");
            return false;
        }
    }
    else {
        swal("Erro", "CNPJ/CPF inválido ou vazio.");
        return false;
    }
    
}