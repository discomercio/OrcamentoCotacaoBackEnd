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

        //O IE está armazenando a chamada ajax no cache, sendo assim, não faz novas chamadas
        //forçamos para não guardar cache no IE
        $.ajaxSetup({ cache: false });

        BuscarAvisosNaoLidos();

        //não deixar fechar dropdown ao clicar dentro
        $("#drop_avisos").click((e) => {
            e.stopImmediatePropagation();
        });

        
    }
});

//ao abrir o dropdown
$("#sininho").on('click', function () {
    //vamos marcar na base que os avisos foram exibidos
    MarcarAvisosExibido();
});

function MarcarAvisosExibido() {
    let c: JQuery<HTMLInputElement> = $('.linhas input');

    //vamos pegar os id's e mandar para o controller
    let lstId: Array<string> = new Array();

    for (let i = 0; i < c.length; i++) {
        if (c[i].value != "") {
            lstId.push(c[i].value);
        }
    }
    //faz a chamada Ajax
    MarcarAvisoExibido(lstId);
}

function MarcarAvisoExibido(lst: Array<string>) {
    let url: string = "/lojamvc/Home/MarcarAvisoExibido";
    $.ajax({
        url: url,
        type: "POST",
        data: { lst: lst },
        dataType: "json",
        success: function () {
            //não fazemos nada
        },
        error: function (t) {
            swal("Erro", "Falha ao marcar avisos como exibidos!");
        }
    });
}

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
window.AlterarLoja = () => {
    RecarregarPagina();
}

function RecarregarPagina() {
    document.location.href = "/lojamvc/Home/Index/?novaloja=" + $('#cabecacomboLojas').val();
}

window.RemoverAvisosSelecionado = () => {

    //vamos buscar os itens selecionados
    let itens: Array<string> = new Array();

    let c: JQuery<HTMLInputElement> = $('.linhas input');

    for (let i = 0; i < c.length; i++) {
        if (c[i].checked == true) {
            itens.push(c[i].value);
        }
    }
    //vamos salvar os dados como lido
    NaoExibirMaisEssesAvisos(itens);
}

function NaoExibirMaisEssesAvisos(itens: string[]) {

    let url: string = "/lojamvc/Home/RemoverAvisos";

    $.ajax({
        url: url,
        type: "POST",
        data: { itens: itens },
        dataType: "json",
        success: function (t) {
            RemoverDaTela();
        },
        error: function () {
            swal("Erro", "Falha ao remover avisos!");
        }
    });
}

function RemoverDaTela() {
    document.getElementById("drop_avisos").scrollTop = 0;
    let c: JQuery<HTMLInputElement> = $('.linhas input');
    let item: HTMLInputElement;
    for (let i = 0; i < c.length; i++) {
        if (c[i].checked == true) {
            item = c[i];
            $(item).closest(".linhas").remove();
        }
    }
    //fechamos o dropdown
    $("#drop_avisos").removeClass("show");
    //atualizamos a qtde de msg
    c = $("#drop_avisos input[type=checkbox]");
    $("#qtdeMsg").text(c.length - 1);
}

function BuscarAvisosNaoLidos() {
    //ao alterar a loja estamos com problema, pois a url esta mantendo "/lojamvc/Home/"
    let url: string = "/lojamvc/Home/BuscarAvisosNaoLidos";
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function (t) {
            TratarCamposAvisos(t);
        },
        error: function (t) {
            swal("Erro", "Falha ao buscar avisos!");
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
                linha.children().find("#chk_").val(t[i].Id);
                linha.children().find("#chk_").prop('id', "chk_" + i);

                //tratando mensagem
                linha.children().find("#paragrafo-avisos").html(t[i].Mensagem);

                //divulgado em
                linha.children().find("#divulgado_").text("Divulgado em: " + DataUtils.formata_data_e_talvez_hora_hhmmss(t[i].Dt_ult_atualizacao));
                linha.children().find("#divulgado_").prop('id', "divulgado_" + i);

                linha.css('display', "block");

                $("#itemMsg_").parent().append(linha);
            }

            let btn_remover: string = "<li id='btn_remover'>" +
                "<button type='button' class='btn btn-primary col-xl-12' onclick='RemoverAvisosSelecionado()'> Remover Avisos </button>" +
                "</li>";
            $("#itemMsg_").parent().append(btn_remover);
        }
    }
}