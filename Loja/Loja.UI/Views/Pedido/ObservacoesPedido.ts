import { DataUtils } from "../../UtilTs/DataUtils/DataUtils";

declare var window: any;
declare function swal(header, mensagem): any;
declare function swal(header, mensagem, tipo): any;

$(function () {
    //Entrega Imediata
    $(".toggle-group label").css("font-size", "12px");
    $(".toggle-group label").css("padding-top", "7.5px");

    $("#entregaImediata").change(function () {
        if ($("#entregaImediata").prop("checked") == false) {
            $("#entregaImediata").val("1");
            let divPai = $("#entregaImediata").parent().parent();
            divPai.css('margin-bottom', "0px");
            divPai.css("height", '30px');
            $("#div-previsao").css('height', '90px');
            $("#div-previsao").css('display', 'block');
        }
        if ($("#entregaImediata").prop("checked") == true) {
            $("#entregaImediata").val("2");
            let divPai = $("#entregaImediata").parent().parent();
            divPai.css('margin-bottom', "1.25em");
            divPai.css("height", '50px')
            $("#div-previsao").css('display', 'none');
        }
    });

    if ($("#entregaImediata").val() == "1") {
        $("#entregaImediata").prop("checked", false);
        $("#entregaImediata").val("1");

    }
    if ($("#entregaImediata").val() == "2") {
        $("#entregaImediata").prop("checked", true);
        $("#entregaImediata").val("2");
    }
    ////Bem de uso consumo
    $("#lblBemDeUso").change(function () {
        if ($("#bemDeUso").prop("checked") == false) {
            $("#bemDeUso").val("0");
        }
        if ($("#bemDeUso").prop("checked") == true) {
            $("#bemDeUso").val("1");
        }
    });
    $("#bemDeUso").change(function () {
        if ($("#bemDeUso").prop("checked") == false) {
            $("#bemDeUso").val("0");
        }
        if ($("#bemDeUso").prop("checked") == true) {
            $("#bemDeUso").val("1");
        }
    });

    if ($("#bemDeUso").val() == "0") {
        $("#bemDeUso").prop("checked", false);
        $("#bemDeUso").val("0");
    }
    if ($("#bemDeUso").val() == "1") {
        $("#bemDeUso").prop("checked", true);
        $("#bemDeUso").val("1");
    }
    //Instalador Instala    
    $("#InstaladorInstala").change(function () {
        if ($("#InstaladorInstala").prop("checked") == false) {
            $("#InstaladorInstala").val("1");
        }
        if ($("#InstaladorInstala").prop("checked") == true) {
            $("#InstaladorInstala").val("2");
        }
    });


    if ($("#InstaladorInstala").val() == "1") {
        $("#InstaladorInstala").prop("checked", false);
        $("#InstaladorInstala").val("1");
    }
    if ($("#InstaladorInstala").val() == "2") {
        $("#InstaladorInstala").prop("checked", true);
        $("#InstaladorInstala").val("2");
    }

    if ($("#entregaImediata").prop("checked") == false) {
        $("#msgEntregaImediata").show()
    }

    $("#cont").text(0);
});

window.Contador = (): any => {
    let msgEntrega: any = $("#observacoes").val();
    $("#cont").text(msgEntrega.length);
}

window.ValidarFormulario = (): any => {
    //valida entrega imediata
    if ($("#entregaImediata").prop("checked") == false) {
        //vamos validar a previsão de entrega
        if ($("#previsao").val() != "") {
            let previsao: any = $("#previsao").val();
            if (DataUtils.formata_formulario_date(previsao) <= new Date()) {
                $("#previsao").val("");
                swal("Erro", "A data para entrega deve ser posterior a data atual!");
                return false;
            }
        }
        else {
            swal("Erro", "Favor informar a data para entrega.");
            return false;
        }
    }

    var serializedForm = $("#formulario").serialize();
    $.ajax({
        url: ($("#formulario")[0] as any).action,
        type: "POST",
        data: serializedForm,
        success: function (result) {
            console.log(result);
            alert("success " + JSON.stringify(result));
        },
        error: function (result) {
            console.log(result);
            alert("Failed " + JSON.stringify(result));
        }
    });

    //$("#formulario").submit();
    return false;

    ////Modal de sucesso
    ////swal("Pedido " + pedido, "", "success");
    //let pedido = "1234N";
    //swal({
    //    title: "Pedido " + pedido,
    //    text: "Tela do pedido em construção!",
    //    type: "success",
    //    showCancelButton: false,
    //    cancelButtonText: "Cancelar",
    //    confirmButtonClass: "btn-primary",
    //    confirmButtonText: "Ok",
    //    closeOnConfirm: true,
    //    closeOnCancel: true
    //},
    //    function (ok: boolean) {
    //        if (ok) $("#formulario").submit();
    //    });
}
