import { CpfCnpjUtils } from "../../UtilTs/CpfCnpjUtils/CpfCnpjUtils";
import { ErrorModal } from "../Shared/Error";


declare var window: any;

window.ValidarCpfCnpj = () => {

    let erroModal = new ErrorModal();
    let msg: string = "";

    if ($("#cpf_cnpj").val() != "") {
        let cpfCnpj :string  = $("#cpf_cnpj").val().toString();

        if (!CpfCnpjUtils.cnpj_cpf_ok(cpfCnpj)) {
            msg = "CNPJ/CPF inválido.";
            erroModal.ModalInnerHTML(msg);
            return false;
        }
    }
    else {
        msg = "CNPJ/CPF inválido ou vazio.";
        erroModal.ModalInnerHTML(msg);
        return false;
    }
    
}