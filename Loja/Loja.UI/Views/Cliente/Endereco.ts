import { CepDto } from "../../DtosTs/CepDto/CepDto";
import { CepEntrega } from "../Cep/Index";
import {  } from "jquery"


declare var window: any;
declare var lstIBGE: Array<string>;
lstIBGE = new Array();
declare var cepDto: CepDto;
cepDto = new CepDto();
declare var cepEntrega: CepEntrega;
cepEntrega = new CepEntrega();

$(function () {
    BuscarUfs();
    
});

$("#modal1").on("hidden.bs.modal", function () {
    let tbody: JQuery<HTMLTableElement> = $(this).find("#tableBody") as JQuery<HTMLTableElement>;
    //caso haja dados da busca anterior, vamos limpar
    if (tbody.children().length > 1) {
        limparModal();
    }
});
/* ========== CHAMADAS DIRETAS DA TELA ====================================*/
/* BUSCA O CEP AO SAIR DO CAMPO DE CEP DO CADASTRO DO CLIENTE */
window.DigitouCepCadastro = () => {
    let cep: string = $('#cep').val() as string;
    $.ajax({
        url: "../Cep/BuscarCep/",
        type: "GET",
        data: { cep: $('#cep').val() },
        dataType: "json",
        success: function (data) {
            if (!data || data.length !== 1) {
                
                limparCamposEndereco();
                return false;
            }

            //vamos limpar os campos
            limparCamposEndereco();
            //temos endereço
            let end: CepDto = data[0];

            $("#cep").val(end.Cep);
            ($("#cep") as any).mask("99999-999");

            if (!!end.Bairro) {
                $("#bairro").val(end.Bairro);
                $("#lblBairro").addClass('active');
            }
            if (!!end.Cidade) {
                if (!!end.ListaCidadeIBGE && end.ListaCidadeIBGE.length > 0) {
                    $("#cidade").prop("readonly", false);
                    lstIBGE = end.ListaCidadeIBGE;
                }
                else {
                    $("#cidade").prop("readonly", true);
                    $("#cidade").val(end.Cidade);
                    $("#lblCidade").addClass('active');
                }
            }
            if (!!end.Endereco) {
                $("#endereco").val(end.Endereco);
                $("#lbEntrega").addClass('active');
            }
            if (!!end.Uf) {
                $("#uf").val(end.Uf);
                $("#lblUf").addClass('active');
            }

            $("#numero").val('');
            $("#complemento").val('');
        },
        error: function (data) {
            swal("Erro", "Falha ao buscar o cep!");
        }
    })
}

let tipo_busca_cep: Number = 0;
window.NaoSeiCep = (btn: JQuery<HTMLButtonElement>) => {    
    
    if (btn.val() == "1") {
        tipo_busca_cep = 1;
        return true;
    }
    if (btn.val() == "2") {
        tipo_busca_cep = 2;
        return true;
    }
    return false;
}

window.InscreverDadosEndereco = () => {
    let tr_linha: JQuery<HTMLElement> = $(".tr_linha").children().find(":checked").closest(".tr_linha");
    inscreveDadosEndereco(tr_linha);
}

window.BuscarCepPorEndereco = () => {
    $(".modal-content").addClass("carregando");
    $.ajax({
        url: "../Cep/BuscarCepPorEndereco/",
        type: "GET",
        data: { nendereco: $('#nendereco').val(), localidade: $('#localidade').val(), lstufs: $('#lstufs').val() },
        dataType: "json",
        success: function (t) {

            //afazer: verificar se a lista de ibge vem junto
            let end: CepDto = t[0];
            montaTabela(t);
        },
        error: function () {
            $(".modal-content").removeClass("carregando");
            swal("Erro", "Falha ao buscar endereços!");
        }
    });
}

//interface JQuery extends Iterable<HTMLElement>{
//    autocomplete:any
//}
window.BuscarLocalidades = (localidade: string) => {
    
    if (localidade != "") {
            $('#localidade').val("");
        }
        if ($('#nendereco').val() != "") {
            $('#nendereco').val("");
        }

        $.ajax({
            url: "../Cep/BuscarLocalidades/",
            type: "GET",
            data: { uf: localidade },
            dataType: "json",
            success: function (t) {
                (<any>$("#localidade")).autocomplete({
                    source: t,
                }) ;
            }
        });
}
/* ============ FIM DE CHAMADAS DIRETAS DA TELA ===========*/
//Monta campos, limpa dados, atribui na tela
function BuscarUfs() {
    $.ajax({
        url: "../Cep/BuscarUfs/",
        type: "GET",
        dataType: "json",
        success: function (data) {
            var select = $('#lstufs');
            $.each(data, function (i, d) {
                select.append("<option value='" + d + "'>" + d + "</option>");
            });
        }
    });
}

function inscreveDadosEndereco(linha: JQuery<HTMLElement>) {
    
    if (tipo_busca_cep != 0) {
        //vamos montar os dados em CepDto passar o[0]
        cepDto = montarCepDto(linha[0].children);

        if (tipo_busca_cep == 1) {
            //vamos limpar os campos de endereço do cadastro
            limparCamposEndereco();
            //vamos inscrever os dados nos campos
            atribuirDadosParaEnderecoCadastro(cepDto);
            //vamos zerar o tipo_busca_cep
        }
        if (tipo_busca_cep == 2) {
            //vamos limpar os campos de endereço de entrega
            cepEntrega.limparCamposEndEntrega();
            //vamos inscrever os dados nos campos
            cepEntrega.atribuirDadosParaEntrega(cepDto);
            //vamos zerar o tipo_busca_cep
            tipo_busca_cep = 0;
        }

        //vamos limpar o body da modal
        limparModal();
    }
}

function atribuirDadosParaEnderecoCadastro(cepDto: CepDto) {

    $("#cep").val(cepDto.Cep);
    ($("#cep") as any).mask("99999-999")
    $("#endereco").val(cepDto.Endereco);
    $("#numero").val("");
    $("#complemento").val("");
    $("#bairro").val(cepDto.Bairro);
    $("#cidade").val(cepDto.Cidade);
    $("#uf").val(cepDto.Uf);

    limparCepDto();
}

//Atribui os dados para a classe de CepDto
function montarCepDto(linha: HTMLCollection): CepDto {
    cepDto.Cep = linha[1].textContent;
    cepDto.Uf = linha[2].textContent;
    cepDto.Cidade = linha[3].textContent;
    cepDto.Bairro = linha[4].textContent;
    cepDto.Endereco = linha[5].textContent;
    return cepDto;
}

function limparCamposEndereco() {

    $("#endereco").val("");
    $("#endereco").removeClass('is-invalid');
    $("#numero").val("");
    $("#complemento").val("");
    $("#bairro").val("");
    $("#bairro").removeClass('is-invalid');
    $("#cidade").val("");
    $("#cidade").removeClass('is-invalid');
    $("#uf").val("");
    $("#uf").removeClass('is-invalid');
}

function limparCepDto() {
    cepDto.Cep = "";
    cepDto.Uf = "";
    cepDto.Cidade = "";
    cepDto.Bairro = "";
    cepDto.Endereco = "";
}

function limparModal() {
    $("#tableBody").children().remove();
    $(".tabela_endereco").hide();
    $("#msg").hide();

    let a: HTMLSelectElement = $("#lstufs")[0] as HTMLSelectElement;
    a.selectedIndex = 0;
    $("#localidade").val("");
    $("#nendereco").val("");
}

function montaTabela(data: any) {
    var cols = "";
    var lst = data["ListaCep"];
    if (!!lst) {
        if (lst.length > 0) {
            if ($('#msg').css("display", "block")) {
                $('#msg').css("display", "none");
            }
            $('.tabela_endereco').css("display", "block");

            for (var i = 0; i < lst.length; i++) {
                cols += "<tr id='linha' class='tr_linha' onclick='ChecarLinha($(this))'>";
                cols += "<td style='width: 3vw!important;'>";
                cols += "<label><input class='check' type='radio' value='" + i + "'></input><span></span></label>";
                cols += "</td>";
                cols += "<td style='width: 7vw!important;'>" + lst[i].Cep + "</td>";
                cols += "<td style='width: 3vw!important;'>" + lst[i].Uf + "</td>";
                cols += "<td>" + lst[i].Cidade + "</td>";
                cols += "<td>" + lst[i].Bairro + "</td>";
                cols += "<td>" + lst[i].Endereco + "</td>";
                cols += "<td>" + lst[i].LogradouroComplemento + "</td></tr>";
                $("#tableBody").empty().append(cols);

                $(".modal-content").removeClass("carregando");
            }
        }
        else {
            if ($('.tabela_endereco').css("display", "block")) $('.tabela_endereco').css("display", "none");
            var msg = "<span> Endereço não encontrado!</span>";
            $("#msg").css("display", "block");
            $("#msg").empty().append(msg);
        }
    }
    else {
        $(".modal-content").removeClass("carregando");
    }
}