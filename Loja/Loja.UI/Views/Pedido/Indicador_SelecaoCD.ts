import { MoedaUtils } from "../../UtilTs/MoedaUtils/moedaUtils";
import { ErrorModal } from "../Shared/Error";
import { PercentualMaximoDto } from "../../DtosTs/PedidoDto/PercentualMaximoDto";
import { IndicadorDto } from "../../DtosTs/IndicadorDto/IndicadorDto";


//declarações
declare var moedaUtils: MoedaUtils;
moedaUtils = new MoedaUtils();
declare var percentualMaximoDto: PercentualMaximoDto;
declare function modalError(): any;
declare var window: any;
declare var indicadorOriginal: string;
declare var listaIndicadoresDto: IndicadorDto[];
declare function modalError(): any;


$("#divCOM").children().find("input").prop('disabled', true);

$("#chkSemRa").prop("checked", true);

$("#chkComRa").click(() => {
    $("#chkSemRa").prop("checked", false);
    $("[name='comRA']").val(1);
    //$("#chkSemRa").val(0);
    //$("#chkComRa").val(1);
});

$("#chkSemRa").click(() => {
    $("#chkComRa").prop("checked", false);
    $("[name='comRA']").val(0);
    //$("#chkComRa").val(0);
    //$("#chkSemRa").val(1);
});

if ($("#chkSemRa").prop("checked") == true) {
    $("[name='comRA']").val(0);
    //$("#chkSemRa").val(1);
    //$("#chkComRa").val(0);
}
if ($("#chkSemRa").prop("checked") == false) {
    $("[name='comRA']").val(1);
    //$("#chkSemRa").val(0);
    //$("#chkComRa").val(1);
}

$("#divIndicadores").children().find("input").prop('disabled', true);
//$("#chkSemIndicacao").prop("checked", true);
$("#chkSemIndicacao").click(() => {
    $("#chkComIndicacao").prop("checked", false);
    $("[name='comIndicacao']").val("0");
    
    $("#divIndicadores").children().find("input").prop('disabled', true);
    $("#divCOM").children().find("input").prop('disabled', true);
    LimparListaIndicadores();
});

if ($("#chkComIndicacao").prop('checked') == true) {

    //$("#divIndicadores").children().find("input").prop('disabled', false);
    $("#chkComIndicacao").prop('checked', true);
    $("[name='comIndicacao']").val("1");

    $("#divComRA").prop('disabled', true);
    $("#divSemRA").prop('disabled', true);

    $("#divCOM").children().find("input").prop('disabled', false);
}

$("#divComRA").children().find("input").prop('disabled', true);
$("#divSemRA").children().find("input").prop('disabled', true);
$("#chkComIndicacao").click(() => {
    $("#chkSemIndicacao").prop("checked", false);
    $("[name='comIndicacao']").val("1");

    $("#divIndicadores").children().find("input").prop('disabled', false);
    
    $("#divCOM").children().find("input").prop('disabled', false);

    if ($("#indicador option").length <= 1)
        MontarListaIndicadores();
});

$("#divSelecaoCd").children().find("input").prop('disabled', true);
$("#chkAutomatico").click(() => {
    $("#chkAutomatico").val(1);
    $("#chkManual").prop("checked", false);
    $("#chkManual").val(0);
    $("#divSelecaoCd").children().find("input").prop('disabled', true);
    $("#msgCD").text('');
});
if ($("#chkManual").prop('checked') == true) {
    $("#chkManual").val(1);
    $("#chkManual").prop('checked', true);
    $("#chkManual").val(0);
    $("#divSelecaoCd").children().find("input").prop('disabled', false);
    $("#msgCD").text('');
}

$("#chkManual").click(() => {
    $("#chkManual").val(1);
    $("#chkAutomatico").prop("checked", false);
    $("#chkAutomatico").val(0);
    $("#divSelecaoCd").children().find("input").prop('disabled', false);
    $("#msgCD").text('');
});

$("#percComissao").keyup(() => {
    moedaUtils = new MoedaUtils();
    let valor = $("#percComissao").val().toString();
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    $("#percComissao").val(moedaUtils.formatarMoedaSemPrefixo(val));
});

$("#selecaoCd").change(function () {
    $("#msgCD").text('');

});





//afazer: montar o select da lista de indicadores, ao clicar com indicação
function MontarListaIndicadores() {
    if (listaIndicadoresDto.length > 0) {
        listaIndicadoresDto.forEach((indicadorDto) => {
            $("#indicador").append("<option value=" + indicadorDto.Apelido + ">" + indicadorDto.Apelido + " - " + indicadorDto.RazaoSocial + "</option>");
        });

        ($("#indicador") as any).formSelect();
    }
}

function LimparListaIndicadores() {
    $("#indicador").empty();

    ($("#indicador") as any).formSelect();
}


window.VerificaPermiteRA = (e: HTMLSelectElement) => {
    debugger;
    let apelido = e.selectedOptions[0].value;

    let indicadorDto: IndicadorDto = listaIndicadoresDto.filter(x => x.Apelido == apelido)[0];

    if (indicadorDto.PermiteRA == 1) {
        //vamos desabiltar os campos de RA
        $("#divComRA").children().find("input").prop('disabled', false);
        $("#divSemRA").children().find("input").prop('disabled', false);
    }
}


window.formataPercComissao = (e: HTMLInputElement) => {
    moedaUtils = new MoedaUtils();
    let valor = e.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    e.value = moedaUtils.formatarMoedaSemPrefixo(val);
}

window.VerificarPercMaxDescEComissao = (e: HTMLInputElement) => {
    let valor = e.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    e.value = moedaUtils.formatarMoedaSemPrefixo(val);


    let msgErro: string = "";
    let percMax = new PercentualMaximoDto()
    percMax = percentualMaximoDto;

    if (val < 0 || val > 100) {
        msgErro = "Percentual de comissão inválido.";
    }
    if (val > percentualMaximoDto.PercMaxComissao) {
        msgErro = "O percentual de comissão excede o máximo permitido.";
    }

    if (msgErro != "") {
        //chama a modal caso seja maior ou tiver erros
        modalError();
        let err = new ErrorModal();
        err.MostrarMsg(msgErro);
        return false;
    }
}

function ValidarCamposSelecaoCD(): boolean {
    let err = new ErrorModal();
    let retorno: boolean = true;

    if ($("#chkManual").prop("checked") == false && $("#chkAutomatico").prop("checked") == false) {
        modalError();
        err.MostrarMsg("Necessário selecionar o modo de seleção do CD");
        retorno = false;
    }

    if ($("#chkManual").prop("checked") == true) {

        let selecaoCD: any = $("#selecaoCd").val();
        if (selecaoCD == "0") {
            modalError();
            err.MostrarMsg("É necessário selecionar o CD que irá atender o pedido(sem auto-split)!");
            retorno = false;
        }
    }

    return retorno;
}

function ValidarCamposIndicador(): boolean {
    let err = new ErrorModal();
    let retorno: boolean = true;

    if ($("#chkSemIndicacao").prop('checked') == false && $("#chkComIndicacao").prop('checked') == false) {
        modalError();
        err.MostrarMsg("Informe se o pedido é com ou sem indicação!");
        return false;
    }

    //afazer: validar se é com indicação, devemos validar os dados do indicador
    if ($("#chkComIndicacao").prop('checked') == true) {
        if ($("#indicador").val() == "" || $("#indicador").val() == undefined) {
            modalError();
            err.MostrarMsg('Selecione o "indicador"!');
            return false;
        }
        debugger;
        if ($("#chkSemRa").prop("checked") == false && $("#chkComRa").prop("checked") == false) {
            modalError();
            err.MostrarMsg("Informe se o pedido possui RA ou não!");
            return false;
        }

        //O indicador informado agora é diferente do indicador original no cadastro do cliente?
        //afazer: preciso do indicador original
        if (indicadorOriginal != $("#indicador").val()) {

            if (flagConfirmaIndicadorDiferente == undefined || !flagConfirmaIndicadorDiferente) {

                let msg = "O indicador selecionado é diferente do indicador que consta no cadastro deste cliente. " +
                    "\n\n##################################################\nFAVOR COMUNICAR AO GERENTE!!\n##################################################\n\nContinua mesmo assim ? ";
                //afazer:modal de continuar para mostrar o continuar

                $("#msg").append(msg);
                AbrirModalConfirm();
                return false;
            }
        }
    }

    return retorno;
}


//if (!flagConfirmaIndicadorDiferente) {
declare function AbrirModalConfirm(): any;
declare var flagConfirmaIndicadorDiferente: boolean;
window.DeclinarIndicadorDigerente = () => {
    flagConfirmaIndicadorDiferente = false;
}

window.ConfirmarIndicadorDiferente = () => {
    flagConfirmaIndicadorDiferente = true;

    if (continuar()) {
        $("#form").submit();
    }

    
}

function continuar() : boolean {
    //validar o cd
    if (!ValidarCamposSelecaoCD())
        return false;
    //valida os campos de indicador
    if (!ValidarCamposIndicador())
        return false;

    return true;
}

window.continuar = (): any => {
    debugger;
    if (continuar() == false) {
        return false;
    }
        
    
    

}
