import { CpfCnpjUtils } from "../../UtilTs/CpfCnpjUtils/CpfCnpjUtils";
import { ErrorModal } from "../Shared/Error";
import { Callbacks } from "jquery";
import { StringUtils } from "../../UtilTs/stringUtils/stringUtils";


declare var window: any;
declare function swal(corpo: any, func: any): any;

window.ValidarCpfCnpj = () => {

    let erroModal = new ErrorModal();
    let msg: string = "";

    if ($("#cpf_cnpj").val() != "") {
        let cpfCnpj: string = StringUtils.retorna_so_digitos($("#cpf_cnpj").val().toString());

        if (!CpfCnpjUtils.cnpj_cpf_ok(cpfCnpj)) {
            msg = "CNPJ/CPF inválido.";
            erroModal.ModalInnerHTML(msg);
            return false;
        }

        ValidarCliente(cpfCnpj);
    }
    else {
        msg = "CNPJ/CPF inválido ou vazio.";
        erroModal.ModalInnerHTML(msg);
        return false;
    }
}

function ValidarCliente(cpf_cnpj: string): any {
    let erroModal = new ErrorModal();
    
    $.ajax({
        url: "../Cliente/ValidarCliente/",
        type: "GET",
        data: { cpf_cnpj: cpf_cnpj },
        dataType: "json",
        success: function (data) {
            debugger;
            $("#novoCliente").val(data);
            if (data) {
                ModalConfirma();
            }
            else {
                $("#formulario").submit();
            }
        },
        error: function () {
            erroModal.ModalInnerHTML("Falha ao buscar o cliente!");
            return false;
        }
    });
}

function ModalConfirma() {
    // Passamos true para confirmar o fechamento da modal
    //estamos verificando se clicou em "Cadastrar = true" ou "Cancelar = false"
    //no caso de clicar "Cancelar" voltamos para a tela para inserir novo cpf/cnpj
    let retorno: boolean;
    swal({
        title: "Cliente não cadastrado!",
        text: "Deseja cadastrar o cliente?",
        type: "warning",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Cadastrar",
        closeOnConfirm: true,
        closeOnCancel: true
    },
        function (ok: boolean) {
            if (ok) $("#formulario").submit();
        });
}

