import { CepDto } from "../../DtosTs/CepDto/CepDto";

export class CepEntrega {
    constructor() {

    };

    public atribuirDadosParaEnderecoEntrega(cepDto: CepDto): void {

        $('#cepEntrega').val(cepDto.Cep);
        $("#cep").addClass('valid');
        $("#lblcep").addClass('active');


        $('#ufEntrega').val(cepDto.Uf);
        $('#ufEntrega').prop('readonly', true);
        $("#lblUfEntrega").addClass('active');

        $('#cidadeEntrega').val(cepDto.Cidade);
        $('#cidadeEntrega').prop('readonly', true);
        $("#lblCidadeEntrega").addClass('active');

        if (cepDto.Bairro.trim() != "") {
            $('#bairroEntrega').prop("readonly", true);
            $('#bairroEntrega').val(cepDto.Bairro);
            $("#lblBairroEntrega").addClass('active');
        }
        else {
            $('#bairroEntrega').prop("readonly", false);
        }

        if (cepDto.Endereco.trim() != "") {
            $('#enderecoEntrega').prop("readonly", true);
            $('#enderecoEntrega').val(cepDto.Endereco);
            $("#lblEnderecoEntrega").addClass('active');
        }
        else {
            $('#enderecoEntrega').prop("readonly", false);
        }

    }

    public limparCamposEndEntrega(): void {
        $('#cepEntrega').val('');
        $('#ufEntrega').val('');
        $('#cidadeEntrega').val('');
        $('#bairroEntrega').val('');
        $('#enderecoEntrega').val('');
        $('#compEntrega').val('');
        $('#numEntrega').val('');
    }
}


//endereço de entrega para cliente PJ setado em PF
$(document).ready(function () {
    $("#EndEntregaTipoPF").prop("checked", true);
    $("#divEntregaPF").show();
    // cliente PJ em entrega PF
    ($("#endEntregaTelRes") as any).mask("(99) 9999-9999");
    ($("#endEntregaCel") as any).mask("(99) 99999-9999");

    //cliente PJ em entrega PJ
    ($("#endEntregaTelCom1") as any).mask("(99) 9999-9999");
    ($("#endEntregaTelCom2") as any).mask("(99) 9999-9999");
});

//Controla os campos de endereço de entrega para PJ
$("#rb_endEntrega_pf").click(function () {
    $("#divEntregaPF").show();
    $("#divEntregaPJ").hide();
    $("#EndEntregaTipoPF").prop("checked", true);
    if ($("#EndEntregaTipoPJ").is(":checked") == true)
        $("#EndEntregaTipoPJ").prop("checked", false);

});
$("#rb_endEntrega_pj").click(function () {
    $("#divEntregaPJ").show();
    $("#divEntregaPF").hide();
    $("#EndEntregaTipoPJ").prop("checked", true);
    if ($("#EndEntregaTipoPF").is(":checked") == true)
        $("#EndEntregaTipoPF").prop("checked", false);
});

declare var window: Window & typeof globalThis;






(window as any).buscarCep = () => {
    if ($('#cepEntrega').val() != null) {
        $('#naoseicep').addClass('pulse');


        $.ajax({
            url: "../Cep/BuscarCep/",
            type: "GET",
            data: { cep: $('#cepEntrega').val() },
            dataType: "json",
            success: function (data) {

                if (data[0].Endereco != "") {
                    $('#enderecoEntrega').prop("readonly", true);
                    $("#enderecoEntrega").val(data[0].Endereco);
                    $("#lblEnderecoEntrega").addClass('active');
                }
                else {
                    $('#enderecoEntrega').prop("readonly", false);
                }

                if (data[0].Bairro != "") {
                    $("#bairroEntrega").prop("readonly", true);
                    $("#bairroEntrega").val(data[0].Bairro);
                    $("#lblBairroEntrega").addClass('active');
                }
                else {
                    $("#bairroEntrega").prop("readonly", false);
                }

                if (data[0].Cidade != "") {
                    $("#cidadeEntrega").prop("readonly", true);
                    $("#cidadeEntrega").val(data[0].Cidade);
                    $("#lblCidadeEntrega").addClass('active');
                }
                else {
                    $("#cidadeEntrega").prop("readonly", false);
                }

                if (data[0].Uf != "") {
                    $("#ufEntrega").prop("readonly", true);
                    $("#ufEntrega").val(data[0].Uf);
                    $("#lblUfEntrega").addClass('active');
                }
                else {
                    $("#ufEntrega").prop("readonly", false);
                }

                if (data[0].logradouroComplemento != "" &&
                    data[0].logradouroComplemento != 'undefined' &&
                    data[0].logradouroComplemento != null) {

                    $('#compEntrega').val(data[0].LogradouroComplemento);
                    $('#lblComplementoEntrega').addClass('active');
                }

                $('#naoseicep').removeClass('pulse');
            },
            error: function (data) {
                console.log(data);
            }
        })
    }
}