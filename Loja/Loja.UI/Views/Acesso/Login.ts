import { Loading } from "../../UtilTs/Loading/Loading";

$(function () {
    $("#loja").prop('autofocus', false);

    if ($("#loja").is(":focus"))
        $("#loja").addClass("fill");
    $("#loja").focusin(function () {
        $("#loja").addClass("fill");
    })
    if ($("#loja").val() != '')
        $("#loja").addClass("fill");

    if ($("#usuario").val() != '')
        $("#usuario").addClass("fill");
});

declare var window: any;
declare function swal(header: any, body: any);


window.continuar = () => {
    Loading.Carregando(true);
    //vamos validar o form
    if (!ValidarForm()) {
        swal("Dados inválidos", "Os dados informados estão inválidos!");
        Loading.Carregando(false);
        return false;
    }

}


function ValidarForm(): boolean {
    let loja: any = $("#loja").val();
    let usuario: any = $("#usuario").val();
    let senha: any = $("#senha").val();

    let validou: boolean = false;
    debugger;
    if (loja != undefined && loja != "" &&
        usuario != undefined && usuario != "" &&
        senha != undefined && senha != "") {
        validou = true;
    }

    return validou;
}