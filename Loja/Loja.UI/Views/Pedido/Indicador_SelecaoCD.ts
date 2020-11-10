import { MoedaUtils } from "../../UtilTs/MoedaUtils/moedaUtils";
import { PercentualMaximoDto } from "../../DtosTs/PedidoDto/PercentualMaximoDto";
import { IndicadorDto } from "../../DtosTs/IndicadorDto/IndicadorDto";


//declarações
declare function swal(header, mensagem): any;
declare var window: any;
declare var indicadorOriginal: string;
declare var flagConfirmaIndicadorDiferente: boolean;

declare var moedaUtils: MoedaUtils;
moedaUtils = new MoedaUtils();
declare var percentualMaximoDto: PercentualMaximoDto;
declare var listaIndicadoresDto: IndicadorDto[];

declare function AbrirModalConfirm(): any;

$("#chkSemRa").prop("checked", true);

$("#chkComRa").click(() => {
    $("#chkSemRa").prop("checked", false);
    $("[name='comRA']").val(1);
    $("#RA").css("opacity", "1");
    $("#percComissao").prop("disabled", false);
});

$("#chkSemRa").click(() => {
    $("#chkComRa").prop("checked", false);
    $("[name='comRA']").val(0);
    $("#RA").css("opacity", "0.7");
    $("#percComissao").prop("disabled", true);
});

if ($("#chkSemRa").prop("checked") == true) {
    $("[name='comRA']").val(0);
    $("#RA").css("opacity", "0.7");
    $("#percComissao").prop("disabled", true);
}

if ($("#chkSemRa").prop("checked") == false) {
    $("[name='comRA']").val(1);
    $("#RA").css("opacity", "1");
    $("#percComissao").prop("disabled", false);
}

if ($("#chkSemIndicacao").prop('checked') == true) {
    $("#chkComIndicacao").prop("checked", false);
    $("[name='comIndicacao']").val("0");
    $("#indicador").prop("disabled", true);
}

$("#chkSemIndicacao").click(() => {
    $("#chkComIndicacao").prop("checked", false);    
    $("[name='comIndicacao']").val("0");
    $("#indicador").prop("disabled", true);
});

if ($("#chkComIndicacao").prop('checked') == true) {
    $("#chkComIndicacao").prop('checked', true);
    $("[name='comIndicacao']").val("1");
    $("#indicador").prop("disabled", false);
}

$("#chkComIndicacao").click(() => {
    $("#chkSemIndicacao").prop("checked", false);
    $("[name='comIndicacao']").val("1");
    $("#indicador").prop("disabled", false);
});

$("#chkAutomatico").click(() => {
    $("#chkAutomatico").val(1);
    $("#chkManual").prop("checked", false);    
    $("#chkManual").val(0);
    $("#msgCD").text('');
    $("#cd").css("opacity", "0.7");
});

if ($("#chkManual").prop('checked') == true) {
    $("#chkManual").val(1);
    $("#chkManual").prop('checked', true);
    $("#chkManual").val(0);
    $("#msgCD").text('');
    $("#cd").css("opacity", "1");
}

$("#chkManual").click(() => {
    $("#chkManual").val(1);
    $("#chkAutomatico").prop("checked", false);
    $("#chkAutomatico").val(0);
    $("#msgCD").text('');
    $("#cd").css("opacity", "1");
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

window.VerificaPermiteRA = (e: HTMLSelectElement) => {
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

    let headerErro: string = "Erro";
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
        swal(headerErro,msgErro);
    }
}

function ValidarCamposSelecaoCD(): boolean {
    let headerErro: string = "Erro";
    let msgErro: string = "";
    let retorno: boolean = true;

    if ($("#chkManual").prop("checked") == false && $("#chkAutomatico").prop("checked") == false) {
        msgErro = "Necessário selecionar o modo de seleção do CD";
        swal(headerErro, msgErro);
        retorno = false;
    }

    if ($("#chkManual").prop("checked") == true) {

        let selecaoCD: any = $("#selecaoCd").val();
        if (selecaoCD == "0") {
            msgErro = "É necessário selecionar o CD que irá atender o pedido(sem auto-split)!";
            swal(headerErro, msgErro);
            retorno = false;
        }
    }

    return retorno;
}

function ValidarCamposIndicador(): boolean {
    let headerErro: string = "Erro";
    let msgErro: string = "";
    let retorno: boolean = true;

    if ($("#chkSemIndicacao").prop('checked') == false && $("#chkComIndicacao").prop('checked') == false) {
        msgErro = "Informe se o pedido é com ou sem indicação!";
        swal(headerErro, msgErro);
        return false;
    }

    //afazer: validar se é com indicação, devemos validar os dados do indicador
    if ($("#chkComIndicacao").prop('checked') == true) {
        if ($("#indicador").val() == "" || $("#indicador").val() == undefined) {
            msgErro = 'Selecione o "indicador"!';
            swal(headerErro, msgErro);
            return false;
        }
        debugger;
        if ($("#chkSemRa").prop("checked") == false && $("#chkComRa").prop("checked") == false) {
            msgErro = "Informe se o pedido possui RA ou não!";
            swal(headerErro, msgErro);
            return false;
        }

        //O indicador informado agora é diferente do indicador original no cadastro do cliente?
        //afazer: preciso do indicador original
        if (indicadorOriginal != $("#indicador").val()) {

            if (flagConfirmaIndicadorDiferente == undefined || !flagConfirmaIndicadorDiferente) {

                let msg = "O indicador selecionado é diferente do indicador que consta no cadastro deste cliente. " +
                    "\n\n##################################################\nFAVOR COMUNICAR AO GERENTE!!\n##################################################\n\nContinua mesmo assim ? ";
                //afazer:modal de continuar para mostrar o continuar

                //$("#msg").append(msg);
                //AbrirModalConfirm();
                return false;
            }
        }
    }

    return retorno;
}

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
    if (continuar() == false) {
        return false;
    }
}
