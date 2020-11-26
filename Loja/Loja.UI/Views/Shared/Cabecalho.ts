import { Constantes } from "../../UtilTs/Constantes/Constantes";
import { DataUtils } from "../../UtilTs/DataUtils/DataUtils";
//OBS: não entendi o pq que temos que importar algo para remover o erro no window 

declare var usuarioLogado: any;
declare var window: any;

$(function () {
    if (usuarioLogado != "" && usuarioLogado != undefined) {
        let letra = usuarioLogado.substring(0, 2);
        let split = usuarioLogado.split(' ');
        if (split.length > 1) {
            letra = usuarioLogado.substring(0, 1) + split[split.length - 1].substring(0, 1);
        }
        setRandomColor();
        $("#avatar").text(letra.toUpperCase());
        $("#menu-usuario").attr("data-original-title", usuarioLogado);

        //vamos buscar via ajax os avisos
        BuscarAvisosNaoLidos();

        //não deixar fechar dropdown ao clicar dentro
        $("#drop_avisos").click((e) => {
            e.stopImmediatePropagation();
        })
    }
});

function setRandomColor() {
    $("#avatar").css("background-color", getRandomColor());
}
function getRandomColor() {
    let letters = '0123456789ABCDEF';
    let color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

declare var document: HTMLDocument;
window.BuscarTodosAvisos = () => {
    debugger;
    document.location.href = "/lojamvc/Home/Index/?novaloja=" + $('#cabecacomboLojas').val();

}

function BuscarAvisosNaoLidos() {
    $.ajax({
        url: "../lojamvc/Home/BuscarAvisosNaoLidos",
        type: "GET",
        //data: { nendereco: $('#nendereco').val(), localidade: $('#localidade').val(), lstufs: $('#lstufs').val() },
        dataType: "json",
        success: function (t) {
            TratarCamposAvisos(t);
        }
    });
}

function TratarCamposAvisos(t: any) {
    if (t != undefined) {
        $("#qtdeMsg").text(t.length);
        if (t.length > 0) {

            for (let i = 0; i < t.length; i++) {

                let linha: JQuery<HTMLElement> = $("#itemMsg_").clone();
                linha.prop('id', "itemMsg_" + i);
                //tratando checkbox
                if (t[i].Id == undefined) {
                    console.log(t[i]);
                    debugger;

                }

                linha.children().find("#chk_").val(t[i].Id);
                linha.children().find("#chk_").prop('id', "chk_" + i);

                //tratando mensagem
                //t[i].Mensagem.replace("\r\n", "<br\>");
                linha.children().find("#paragrafo-avisos").html(t[i].Mensagem);

                //divulgado em
                linha.children().find("#divulgado_").text("Divulgado em: " + DataUtils.formata_data_e_talvez_hora_hhmmss(t[i].Dt_ult_atualizacao));
                linha.children().find("#divulgado_").prop('id', "divulgado_" + i);

                linha.css('display', "block");

                $("#itemMsg_").parent().append(linha);
            }

            let btn_remover: string = "<li id='btn_remover'>" +
                "<button type='button' class='btn btn-primary col-xl-12' > Remover Avisos </button>" +
                "</li>";
            $("#itemMsg_").parent().append(btn_remover);
        }


    }
}