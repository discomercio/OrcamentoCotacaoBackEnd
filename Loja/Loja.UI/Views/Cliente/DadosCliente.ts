import { StringUtils } from "../../UtilTs/stringUtils/stringUtils";
import { CpfCnpjUtils } from "../../UtilTs/CpfCnpjUtils/CpfCnpjUtils";
import { Constantes } from "../../UtilTs/Constantes/Constantes";
import { isNumeric } from "jquery";
import { DataUtils } from "../../UtilTs/DataUtils/DataUtils";
import { ErrorModal } from "../Shared/Error";
import { Loading } from "../../UtilTs/Loading/Loading";

declare var window: any;
declare function swal(header, msg): any;

let cpfCnpj = $('#cpf_cnpj').val().toString();
$('#cpf_cnpj').val(CpfCnpjUtils.cnpj_cpf_formata(cpfCnpj));



$(function () {

    ($('#cepEntrega') as any).mask("00000-000");
    ($('#cep') as any).mask("00000-000");
    ($('#ie') as any).mask("000.000.000.000");
    ($('#telRes') as any).mask("(00) 0000-0000");
    //$('#celular').mask("(00) 00000-0000");
    $('#celular').blur(function () {
        if ($('#celular').val().toString().length == 14) {
            ($('#celular') as any).mask("(99) 9999-9999");
        }
        else {
            ($('#celular') as any).mask("(99) 99999-9999");
        }
    });

    ($('#telCom') as any).mask("(00) 0000-0000");

    $('#telCom2').blur(function () {
        if ($('#telCom2').val().toString().length == 14) {
            ($('#telCom2') as any).mask("(99) 9999-9999");
        }
        else {
            ($('#telCom2') as any).mask("(99) 99999-9999");
        }
    })

    $('#addRefComercial').on('click', function () {
        let index: number = Number($('#index').val());

        if (index == undefined) {
            index = 0;
            $('#index').val(index);
        }

        let linha: string = "<hr /><div class='form-group row'>";
        linha += "<div class='col-sm-4' style='text-align: initial;'>";
        linha += "<label class='col-form-label' for='Nome_Empresa'>NOME DA EMPRESA</label>";
        linha += "<input name='[" + index + "].Nome_Empresa' id='" + index + "-Nome_Empresa' type='text' class='form-control' />";
        linha += "</div>";
        linha += "<div class='col-sm-4' style='text-align: initial;'>";
        linha += "<label class='col-form-label' for='Contato'>CONTATO</label>";
        linha += "<input name='[" + index + "].Contato' id='" + index + "-Contato' type='text' class='form-control'/>";
        linha += "</div>";
        linha += "<div class='col-sm-4' style='text-align: initial;'>";
        linha += "<label class='col-form-label' for='Telefone'>TELEFONE</label>";
        linha += "<input name='[" + index + "].Telefone' id='" + index + "-Telefone' type='text' class='form-control' />";
        linha += "</div></div>";

        $('#collapsible-body-comercial').append(linha);
        ($('#' + index + '-Telefone') as any).mask("(00) 0000-0000");

        index++;
        $('#index').val(index);
    });

    //mascara de telefone para ref bancaria
    $("#0-telBanco").blur(function () {
        if ($('#0-telBanco').val().toString().length == 14) {
            ($('#0-telBanco') as any).mask("(99) 9999-9999");
        }
        else {
            ($('#0-telBanco') as any).mask("(99) 99999-9999");
        }
    });


    $('#addRefBancaria').on('click', function () {
        if (!$("#RefBancariabody-0").is(":visible")) {
            $("#RefBancariabody-0").show();
            $("#RefBancariabody-0").css('display', 'inline-flex');
            return;
        }

        swal("Erro", "Máximo de 1 referência bancária.");
        return false;
    });


    //verificar produtor rural no inicio da tela
    if ($("#tipo").val() == "PF") {
        if (Number($("#produtor").val()) == Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) {
            $("#div_ie").css('visibility', 'visible');
            $("#div_contribuinte").css('visibility', 'visible');
        }
    }


});

//CONTROLE DE CAMPOS
//===========================
//Tipo PF
//Dados pessoais
$("#nome").blur(() => {
    if ($("#nome").val() == "") $("#nome").addClass("is-invalid");
    if ($("#nome").val()?.toString().length > 0 && $("#nome").val() != "") {
        $("#nome").removeClass("is-invalid");
    }
});
$("#cpf_cnpj").blur(() => {
    if ($("#cpf_cnpj").val() == "") $("#cpf_cnpj").addClass("is-invalid");
    if ($("#cpf_cnpj").val()?.toString().length > 0 && $("#cpf_cnpj").val() != "") {
        $("#cpf_cnpj").removeClass("is-invalid");
    }
});
$("#nascimento").blur(() => {
    debugger;
    if (!new Date($("#nascimento").val()?.toString()).getTime()) {
        $("#nascimento").addClass("is-invalid");
    }
    else {
        $("#nascimento").removeClass("is-invalid");
        return false;
    };

    if (new Date($("#nascimento").val()?.toString()).getTime()) {
        $("#nascimento").removeClass("is-invalid");
    }
});
//Endereço
$("#cep").blur(() => {
    if ($("#cep").val() == "") $("#cep").addClass("is-invalid");
    if ($("#cep").val()?.toString().length > 0 && $("#cep").val() != "") {
        $("#cep").removeClass("is-invalid");
    }
});
$("#cep").blur(() => {
    if ($("#cep").val() == "") $("#cep").addClass("is-invalid");
    if ($("#cep").val()?.toString().length > 0 && $("#cep").val() != "") {
        $("#cep").removeClass("is-invalid");
    }
});
$("#numero").blur(() => {
    if ($("#numero").val() == "") $("#numero").addClass("is-invalid");
    if ($("#numero").val()?.toString().length > 0 && $("#numero").val() != "") {
        $("#numero").removeClass("is-invalid");
    }
});
$("#bairro").blur(() => {
    if ($("#bairro").val() == "") $("#bairro").addClass("is-invalid");
    if ($("#bairro").val()?.toString().length > 0 && $("#bairro").val() != "") {
        $("#bairro").removeClass("is-invalid");
    }
});
$("#cidade").blur(() => {
    if ($("#cidade").val() == "") $("#cidade").addClass("is-invalid");
    if ($("#cidade").val()?.toString().length > 0 && $("#cidade").val() != "") {
        $("#cidade").removeClass("is-invalid");
    }
});
$("#uf").blur(() => {
    if ($("#uf").val() == "") $("#uf").addClass("is-invalid");
    if ($("#uf").val()?.toString().length > 0 && $("#uf").val() != "") {
        $("#uf").removeClass("is-invalid");
    }
});
//Telefones

//E-mail
$("#email").blur(() => {
    if ($("#email").val() == "") $("#email").addClass("is-invalid");
    if ($("#email").val()?.toString().length > 0 && $("#email").val() != "") {
        $("#email").removeClass("is-invalid");
    }
});
$("#emailXml").blur(() => {
    if ($("#emailXml").val() == "") $("#emailXml").addClass("is-invalid");
    if ($("#emailXml").val()?.toString().length > 0 && $("#emailXml").val() != "") {
        $("#emailXml").removeClass("is-invalid");
    }
});
//Produtor Rural
$("#produtor").change(() => {
    if ($("#produtor").val() == "") $("#produtor").addClass("is-invalid");
    if ($("#produtor").val()?.toString().length > 0 && $("#produtor").val() != "") {
        $("#produtor").removeClass("is-invalid");
    }
});

//FIM CONROLE DE CAMPOS

window.MostrarDivs = () => {
    if (Number($("#produtor").val()) == Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) {
        $("#div_ie").css('visibility', 'visible');
        $("#div_contribuinte").css('visibility', 'visible');
    }
    if (Number($("#produtor").val()) != Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) {
        $("#div_ie").css('visibility', 'hidden');
        $("#div_contribuinte").css('visibility', 'hidden');
    }
}

window.DigitouCepCadastro = () => {
    if ($('#cep').val() != null) {

        $.ajax({
            url: "../Cep/BuscarCep/",
            type: "GET",
            data: { cep: $('#cep').val() },
            dataType: "json",
            success: function (data) {

                if (data[0].Endereco != "") {
                    $('#endereco').prop("readonly", true);
                    $("#endereco").val(data[0].Endereco);
                    $("#lbEntrega").addClass('active');
                }
                else {
                    $('#enderecoEntrega').prop("readonly", false);
                }

                if (data[0].Bairro != "") {
                    $("#bairro").prop("readonly", true);
                    $("#bairro").val(data[0].Bairro);
                    $("#lblBairro").addClass('active');
                }
                else {
                    $("#bairro").prop("readonly", false);
                }

                if (data[0].Cidade != "") {
                    $("#cidade").prop("readonly", true);
                    $("#cidade").val(data[0].Cidade);
                    $("#lblCidade").addClass('active');
                }
                else {
                    $("#cidade").prop("readonly", false);
                }

                if (data[0].Uf != "") {
                    $("#uf").prop("readonly", true);
                    $("#uf").val(data[0].Uf);
                    $("#lblUf").addClass('active');
                }
                else {
                    $("#uf").prop("readonly", false);
                }

                if (data[0].logradouroComplemento != "" &&
                    data[0].logradouroComplemento != 'undefined' &&
                    data[0].logradouroComplemento != null) {

                    $('#complemento').val(data[0].LogradouroComplemento);
                    $('#lblComplemento').addClass('active');
                }
                $("#numero").val('');
                $("#complemento").val('');
                //$('#naoseicep').removeClass('pulse');
            },
            error: function (data) {
                swal("Erro", "Falha ao buscar o cep!");
            }
        })
    }
}

window.ValidarFormulario = () => {
    Loading.Carregando(true);

    let erroModal = new ErrorModal();
    let msg: string = "";

    if ($('#permiteEditar').val() == "True") {
        //CPF
        if (CpfCnpjUtils.cnpj_cpf_ok(CpfCnpjUtils.cnpj_cpf_formata(cpfCnpj))) {
            //validar campos de cadastro
            if ($('#cpf_cnpj').val() == '') {
                $("#cpf_cnpj").addClass("is-invalid");
                msg += "Favor informar CPF ou CNPJ<br>";
            }
            if ($("#nome").val() == '') {
                $("#nome").addClass("is-invalid");
                msg += "Favor informar nome do cliente<br>";
            }
            if ($("#nascimento").val() == '') {
                $("#nascimento").addClass("is-invalid");
                msg += "Favor informar nascimento<br>";
            }
            if ($("#sexo").val() == '') {
                msg += "Favor informar sexo<br>";
            }
            if ($("#telRes").val() == '' &&
                $("#celular").val() == '' &&
                $("#telCom").val() == '') {
                $("#telRes").addClass("is-invalid");
                $("#celular").addClass("is-invalid");
                $("#telCom").addClass("is-invalid");
                msg += "Favor preencher ao menos um número de telefone!<br>";
            }
            if ($("#email").val() == '') {
                $("#email").addClass("is-invalid");
                msg += "Favor informar e-mail!<br>";
            }

            if (msg.length > 0) {
                Loading.Carregando(false);
                msg = "<b>Preencha os campos marcados como obrigatório:</b><br>" + msg;
                erroModal.ModalInnerHTML(msg);
                return false;
            }
        }
        //CNPJ
        if (cpfCnpj.length == 14) {
            if ($("#razao").val() == '') {
                $("#razao").addClass("is-invalid");
                msg += "Favor informar Razão Social!<br>";
            }
            if ($("#cpf_cnpj").val() == '') {
                $("#cpf_cnpj").addClass("is-invalid");
                msg += "Favor informar CNPJ!<br>";
            }
            if ($("#telCom").val() == '' &&
                $("#telCom2").val() == '' &&
                $("#celular").val() == '') {
                $("#telCom").addClass("is-invalid");
                $("#telCom2").addClass("is-invalid");
                $("#celular").addClass("is-invalid");
                msg += "Favor informar ao menos um número de telefone!<br>";
            }

            if (msg.length > 0) {
                Loading.Carregando(false);
                msg = "<b>Preencha os campos marcados como obrigatório:</b><br>" + msg;
                erroModal.ModalInnerHTML(msg);
                return false;
            }
        }
        if ($("#cep").val() == '') {
            $("#cep").addClass("is-invalid");
            msg += "Favor informar cep<br>";
        }
        if ($("#endereco").val() == '') {
            $("#endereco").addClass("is-invalid");
            msg += "Favor informar endereco!<br>";
        }
        if ($("#numero").val() == '') {
            $("#numero").addClass("is-invalid");
            msg += "Favor informar número!<br>";
        }
        if ($("#bairro").val() == '') {
            $("#bairro").addClass("is-invalid");
            msg += "Favor informar bairro!<br>";
        }
        if ($("#cidade").val() == '') {
            $("#cidade").addClass("is-invalid");
            msg += "Favor informar cidade!<br>";
        }
        if ($("#uf").val() == '') {
            $("#uf").addClass("is-invalid");
            msg += "Favor informar UF!<br>";
        }

        if (msg.length > 0) {
            Loading.Carregando(false);
            msg = "<b>Preencha os campos marcados como obrigatório:</b><br>" + msg;
            erroModal.ModalInnerHTML(msg);
            return false;
        }
    }
    else {
        cpfCnpj = StringUtils.retorna_so_digitos($('#cpf_cnpj').text());
    }

    //afazer: validar endereço de cadastro



    let validou: boolean;
    //verificar se é produtor rural
    //1 = Não / 2 = sim
    validou = ValidarProdutorIcms(cpfCnpj);

    if (validou) {
        //se estiver tudo ok seguimos
        if (!$('#indicador').val() || $('#indicador').val() == "undefined" || $('#indicador').val() == "0") {
            $("#indicador").addClass("is-invalid");
            msg += "Favor selecionar um indicador!<br>";
        }

        if (msg.length > 0) {
            Loading.Carregando(false);
            erroModal.ModalInnerHTML(msg);
            return false;
        }

        if ($('#outro').prop("checked")) {
            //verifica se o endereço de entrega esta preenchido
            $('#outro').val("true");
            validou = ValidarEnderecoEntrega();
        }
    }
    return validou;
}

function ValidarProdutorIcms(cpfCnpj) {
    let retorno: boolean = true;

    let erroModal = new ErrorModal();
    let msg: string = "";

    let ie: string = $('#ie').val()?.toString();
    let produtor: string = $('#produtor').val().toString();
    let contribuinte: string = $("#contribuinte")?.val().toString();

    if (cpfCnpj.length == 11) {
        if (produtor == "") {
            $('#produtor').addClass("is-invalid");
            msg += 'Informe se o cliente é produtor rural ou não!<br>';
            retorno = false;
        }
        if (produtor != "1" && produtor != "2") {
            $('#produtor').addClass("is-invalid");
            msg += 'Informe se o cliente é produtor rural ou não!<br>';
            retorno = false;
        }
        //é produtor rural
        if (produtor == "2") {
            //contribuinte
            //1 = não / 2 = sim / 3 = isento           

            if (contribuinte != "1" && contribuinte != "2" && contribuinte != "3") {
                $("#contribuinte").addClass("is-invalid");
                msg += 'Informe se o cliente é contribuinte do ICMS, não contribuinte ou isento!<br>';
                retorno = false;
            }
            if (contribuinte == "1" && (ie.trim() == "")) {
                $("#contribuinte").addClass("is-invalid");
                msg = 'Se o cliente é contribuinte do ICMS a inscrição estadual deve ser preenchida!<br>';
                retorno = false;
            }
            if (contribuinte == "1" && ie && ie.toUpperCase().indexOf("ISEN") >= 0) {
                $("#contribuinte").addClass("is-invalid");
                msg += 'Se cliente é não contribuinte do ICMS, não pode ter o valor ISENTO ' +
                    'no campo de Inscrição Estadual!<br>';
                retorno = false;
            }
            if (contribuinte == "2" && (ie.toUpperCase().indexOf("ISEN") >= 0)) {
                $("#contribuinte").addClass("is-invalid");
                msg += 'Se cliente é contribuinte do ICMS, não pode ter o valor ISENTO ' +
                    'no campo de Inscrição Estadual!<br>';
                retorno = false;
            }
        }

        if (msg.length > 0) {
            Loading.Carregando(false);
            msg = "<b>Preencha os campos marcados como obrigatório: </b><br>" + msg;
            erroModal.ModalInnerHTML(msg);
            return retorno;
        }
    }
    else {
        ie = StringUtils.retorna_so_digitos(ie);

        if (contribuinte != "0" && contribuinte != "1" && contribuinte != "2" && contribuinte != "3") {
            $("#contribuinte").addClass("is-invalid");
            msg += 'Informe se o cliente é contribuinte do ICMS, não contribuinte ou isento!<br>';
            retorno = false;
        }
        if (contribuinte == "2" && (ie == "")) {
            $('#ie').addClass("is-invalid");
            msg += 'Se o cliente é contribuinte do ICMS a inscrição estadual deve ser preenchida!<br>';
            retorno = false;
        }
        if (contribuinte == "1" && ie != "") {
            if (!$.isNumeric(ie)) {
                if (ie.toUpperCase().indexOf("ISEN") >= 0) {
                    $('#ie').addClass("is-invalid");
                    msg += 'Se cliente é não contribuinte do ICMS, não pode ter o valor ISENTO ' +
                        'no campo de Inscrição Estadual!<br>';
                    retorno = false;
                }
            }
        }
        if (contribuinte == "2" && ie != "") {
            if (!$.isNumeric(ie)) {
                if (ie.toUpperCase().indexOf("ISEN") >= 0) {
                    $('#ie').addClass("is-invalid");
                    msg += 'Se cliente é contribuinte do ICMS, não pode ter o valor ISENTO ' +
                        'no campo de Inscrição Estadual!<br>';
                    retorno = false;
                }
            }
        }

        if (msg.length > 0) {
            Loading.Carregando(false);
            msg = "<b>Preencha os campos marcados como obrigatório: </b><br>" + msg;
            erroModal.ModalInnerHTML(msg);
            return retorno;
        }
    }

    if (cpfCnpj.length == 11) {
        if (produtor && produtor != "1") {
            if (contribuinte == "3") {
                if (ie && ie != "") {
                    $("#ie").addClass("is-invalid");
                    msg += "Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!<br>";
                    retorno = false;
                }
            }
        }
    }
    else {
        if (contribuinte == "3") {
            if (ie && ie != "") {
                $("#ie").addClass("is-invalid");
                msg = "Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!<br>";
                retorno = false;
            }
        }
    }

    if (msg.length > 0) {
        Loading.Carregando(false);
        msg = "<b>Preencha os campos marcados como obrigatório: </b><br>" + msg;
        erroModal.ModalInnerHTML(msg);
        return retorno;
    }

    return retorno
}

function ValidarEnderecoEntrega() {
    let erroModal = new ErrorModal();
    let msg: string = "";

    let cepEntrega: string = $('#cepEntrega').val().toString();
    let enderecoEntrega: string = $('#enderecoEntrega').val().toString();
    let numEntrega: string = $('#numEntrega').val().toString();
    let ufEntrega: string = $('#ufEntrega').val().toString();
    let cidadeEntrega: string = $('#cidadeEntrega').val().toString();
    let justificaticaEntrega: string = $('#justificativa').val().toString();

    if (!cepEntrega || ufEntrega === "" || cidadeEntrega === "") {
        $('#cepEntrega').addClass("is-invalid");
        msg += "Caso seja selecionado outro endereço, informe um CEP válido!<br>";
    }
    if (enderecoEntrega === "") {
        $('#enderecoEntrega').addClass("is-invalid");
        msg += "Caso seja selecionado outro endereço, informe um endereço!<br>";
    }
    if (justificaticaEntrega === "") {
        $('#justificativa').addClass("is-invalid");
        msg += "Caso seja selecionado outro endereço, selecione a justificativa do endereço de entrega!<br>";
    }
    if (numEntrega === "") {
        $('#numEntrega').addClass("is-invalid");
        msg += "Caso seja selecionado outro endereço, preencha o número do endereço de entrega!<br>";
    }

    if (msg.length > 0) {
        Loading.Carregando(false);
        msg = "<b>Preencha os campos marcados como obrigatório: </b><br>" + msg;
        erroModal.ModalInnerHTML(msg);
        return false;
    }
}
