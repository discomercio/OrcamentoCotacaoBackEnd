import { CpfCnpjUtils } from "../../UtilTs/CpfCnpjUtils/CpfCnpjUtils";
import { StringUtils } from "../../UtilTs/stringUtils/stringUtils";


declare var window: any;
declare function ModalSimples(mensagem): any;

window.ValidarCpfCnpj = () => {
    if ($("#cpf_cnpj").val() != "") {
        let cpfCnpj :string  = $("#cpf_cnpj").val().toString();

        if (!CpfCnpjUtils.cnpj_cpf_ok(cpfCnpj)) {
            ModalSimples("CNPJ/CPF inválido.");
            return false;
        }
    }
    else {
        ModalSimples("CNPJ/CPF inválido ou vazio.");
        return false;
    }
    
}