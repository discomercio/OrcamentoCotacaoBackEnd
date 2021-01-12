﻿import { StringUtils } from "../../UtilTs/stringUtils/stringUtils";
import { CpfCnpjUtils } from "../../UtilTs/CpfCnpjUtils/CpfCnpjUtils";
import { Constantes } from "../../UtilTs/Constantes/Constantes";
import { ErrorModal } from "../Shared/Error";
import { Loading } from "../../UtilTs/Loading/Loading";
import { CepDto } from "../../DtosTs/CepDto/CepDto";
import { CepEntrega } from "../Cep/Index";
import { ValidacoesCliente } from "../../FuncoesTs/Cliente/ValidacoesCliente";
import { ClienteCadastroDto } from "../../DtosTs/ClienteDto/ClienteCadastroDto";
import { RefBancariaDtoCliente } from "../../DtosTs/ClienteDto/RefBancariaDtoCliente";
import { RefComercialDtoCliente } from "../../DtosTs/ClienteDto/RefComercialDtoCliente";
import { DadosClienteCadastroDto } from "../../DtosTs/ClienteDto/DadosClienteCadastroDto";
import { FormatarTelefone } from "../../UtilTs/Fomatar/Mascaras/formataTelefone";
import { EnderecoEntregaClienteCadastroDto } from "../../DtosTs/ClienteDto/EnderecoEntregaClienteCadastroDto";
import { contains } from "jquery";

declare var window: any;
declare function swal(header, msg): any;

declare var cepDto: CepDto;
cepDto = new CepDto();
declare var cepEntrega: CepEntrega;
cepEntrega = new CepEntrega();
declare var validacoesCliente: ValidacoesCliente;
validacoesCliente = new ValidacoesCliente();
declare var dadosClienteCadastroDto: DadosClienteCadastroDto;
dadosClienteCadastroDto = new DadosClienteCadastroDto();
declare var clienteCadastro: ClienteCadastroDto;
clienteCadastro = new ClienteCadastroDto();
declare var lstRefBancaria: Array<RefBancariaDtoCliente>;
lstRefBancaria = new Array();
declare var lstRefComercial: Array<RefComercialDtoCliente>;
lstRefComercial = new Array();
declare var lstIBGE: Array<string>;
lstIBGE = new Array();

let cpfCnpj = $('#cpf_cnpj').val().toString();
$('#cpf_cnpj').val(CpfCnpjUtils.cnpj_cpf_formata(cpfCnpj));
$("#cpf_cnpj").prop("readonly", true);
$(function () {

    ($('#cepEntrega') as any).mask("00000-000");
    ($('#cep') as any).mask("00000-000");
    ($('#ie') as any).mask("000.000.000.000");

    $('#celular').blur(function () {
        if ($('#celular').val().toString().length == 14) {
            ($('#celular') as any).mask("(99) 9999-9999");
        }
        else {
            ($('#celular') as any).mask("(99) 99999-9999");
        }
    });

    $('#telCom').blur(function () {
        if ($('#telCom').val() != "") {
            $('#telCom').removeClass("is-invalid");
        }
    });

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
            index = 1;
            $('#index').val(index);
        }

        if (index == 3) {
            swal("", "Máximo de 3 referências comerciais.");
            return false;
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
        linha += "<input name='[" + index + "].Telefone' id='" + index + "-Telefone' type='text' class='form-control' onblur='IncluirMascara($(this))' onfocus='RemoveMascara($(this))' />";
        linha += "</div>"
        linha += "<input type='hidden' id='" + index + "'-Ordem' name='[" + index + "].Ordem' value='" + (index + 1) + "' /> ";
        linha += "</div>";

        $('#collapsible-body-comercial').append(linha);
        ($('#' + index + '-Telefone') as any).mask("(00) 0000-0000");

        index++;
        $('#index').val(index);
    });

    $('#addRefBancaria').on('click', function () {
        let indiceBancaria: string = $("#indiceBancaria").val() as string;
        if (!$("#RefBancariabody-" + indiceBancaria + "").is(":visible")) {
            $("#RefBancariabody-" + indiceBancaria + "").show();
            $("#RefBancariabody-" + indiceBancaria + "").css('display', 'inline-flex');
            return;
        }

        swal("Erro", "Máximo de 1 referência bancária.");
        return false;
    });

    //verificar produtor rural no inicio da tela
    if ($("#tipo").val() == Constantes.ID_PF) {
        if (Number($("#produtor").val()) == Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) {
            $("#div_ie").css('visibility', 'visible');
            $("#div_contribuinte").css('visibility', 'visible');
        }
    }

    MontarTelefonesTela();
    ColocarMascaraNosTelefones();

});

/* =============== CONTROLE DE CAMPOS ================== */
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
/*=========== Endereço ===========*/
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
if ($("#numero").val() != "") {
    $("#numero").removeClass("is-invalid");
}
$("#bairro").blur(() => {
    if ($("#bairro").val() == "") $("#bairro").addClass("is-invalid");
    if ($("#bairro").val()?.toString().length > 0 && $("#bairro").val() != "") {
        $("#bairro").removeClass("is-invalid");
    }
});
//para caso seja preenchido de forma automática
$("#bairro").change(function () {
    if ($("#bairro").val() != "") {
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
    if ($("#uf").val() == "") {
        $("#uf").addClass("is-invalid");
    }
    if ($("#uf").val()?.toString().length > 0 && $("#uf").val() != "") {
        $("#uf").removeClass("is-invalid");
    }
});
/*=========== TELEFONES PF ===========*/
$("#telRes").blur(() => {
    if ($("#telRes").val() != '' ||
        $("#celular").val() != '' ||
        $("#telCom").val() != '') {
        $("#telRes").removeClass("is-invalid");
        $("#celular").removeClass("is-invalid");
        $("#telCom").removeClass("is-invalid");
    };
});
$("#celular").blur(() => {
    if ($("#telRes").val() != '' ||
        $("#celular").val() != '' ||
        $("#telCom").val() != '') {
        $("#telRes").removeClass("is-invalid");
        $("#celular").removeClass("is-invalid");
        $("#telCom").removeClass("is-invalid");
    };
});
$("#telCom").blur(() => {
    if ($("#telRes").val() != '' ||
        $("#celular").val() != '' ||
        $("#telCom").val() != '') {
        $("#telRes").removeClass("is-invalid");
        $("#celular").removeClass("is-invalid");
        $("#telCom").removeClass("is-invalid");
    };
});
/*=========== TELEFONES PJ ===========*/
$("#telCom").blur(() => {
    if ($("#telCom").val() != "" ||
        $("#telCom2").val() != "") {
        $("#telCom").removeClass("is-invalid");
        $("#telCom2").removeClass("is-invalid");
    }
});
$("#telCom2").blur(() => {
    if ($("#telCom").val() != "" ||
        $("#telCom2").val() != "") {
        $("#telCom").removeClass("is-invalid");
        $("#telCom2").removeClass("is-invalid");
    }
});

/*=========== E-mail ===========*/
$("#email").blur(() => {
    if ($("#email").val() == "") $("#email").addClass("is-invalid");
    if ($("#email").val()?.toString().length > 0 && $("#email").val() != "") {
        $("#email").removeClass("is-invalid");
        //validar se o email esta ok
    }
});
/*=========== E-mail XML ===========*/
$("#emailXml").blur(() => {
    if ($("#emailXml").val() == "") $("#emailXml").addClass("is-invalid");
    if ($("#emailXml").val()?.toString().length > 0 && $("#emailXml").val() != "") {
        $("#emailXml").removeClass("is-invalid");
        //verificar se o email esta ok
    }
});
/*=========== Produtor Rural ===========*/
$("#produtor").change(() => {
    if ($("#produtor").val() == "") $("#produtor").addClass("is-invalid");
    if ($("#produtor").val()?.toString().length > 0 && $("#produtor").val() != "") {
        $("#produtor").removeClass("is-invalid");
    }
});
/*=========== Não sei o CEP ===========*/
$("#btnModificar").click(function () {
    let tr_linha: JQuery<HTMLElement> = $(".tr_linha").children().find(":checked").closest(".tr_linha");
    inscreve(tr_linha);
});
$('#btnBuscar').on('click', function () {
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
})
$("#lstufs").ready(function () {
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
});

let tipo_busca_cep: Number = 0;
$(".btnCepCadastro").click(function () {
    let button: HTMLButtonElement = this as HTMLButtonElement;
    if (button.value == "1") {
        tipo_busca_cep = 1;
        return true;
    }

    if (button.value == "2") {
        tipo_busca_cep = 2;
        return true;
    }

    return false;
});

//PJ
$("#razao").blur(function () {
    if ($("#razao").val() != "") {
        $("#razao").removeClass("is-invalid");
    }
});
$("#contribuinte").blur(function () {
    if ($("#contribuinte").val() != "") {
        $("#contribuinte").removeClass("is-invalid");
    }
});
$("#contatoNaEmpresa").blur(function () {
    if ($("#contatoNaEmpresa").val() != "") {
        $("#contatoNaEmpresa").removeClass("is-invalid");
    }
});


$("#modal1").on("hidden.bs.modal", function () {
    let tbody: JQuery<HTMLTableElement> = $(this).find("#tableBody") as JQuery<HTMLTableElement>;
    //caso haja dados da busca anterior, vamos limpar
    if (tbody.children().length > 1) {
        limparModal();
    }
});

/*================FIM CONROLE DE CAMPOS =================*/

/* ========== CHAMADAS DIRETAS DA TELA ====================================*/
window.RemoveMascara = (el: JQuery<HTMLInputElement>) => {
    let valor: string = StringUtils.retorna_so_digitos(el.val() as string);
    (el as any).unmask();
    el.val(valor);
}
//inclui mascara para Ref's
window.IncluirMascara = (el: JQuery<HTMLInputElement>) => {

    let valor: string = StringUtils.retorna_so_digitos(el.val() as string);
    if (!!valor) {
        if (valor.length < 6) {
            swal("Erro", "Telefone inválido!");
            return false;
        }
        if (valor.length >= 6 && valor.length <= 8) {
            //não tem ddd            
            (el as any).mask(MascaraParaTelefones("", valor));
        }
        if (valor.length > 8) {
            //tem ddd
            let s = FormatarTelefone.SepararTelefone(valor);
            el.next().val(s.Ddd);
            (el as any).mask(MascaraParaTelefones(s.Ddd, s.Telefone));
        }
    }
}
//mostra a div de endereço de entrega
window.mostraDiv = (el: JQuery<HTMLInputElement>) => {

    let div_outro_endereco: JQuery<HTMLDivElement> = $("#outro_endereco") as JQuery<HTMLDivElement>;

    if (div_outro_endereco[0].style.display == "none") {
        div_outro_endereco[0].style.display = "block";
        el.val("True");
        el.prop("checked", true);
        $('#mesmo').prop('checked', false);
    }
}
//esconde a div de endereço de entrega
window.fechaDiv = (el: JQuery<HTMLInputElement>) => {

    let div_outro_endereco: JQuery<HTMLDivElement> = $("#outro_endereco") as JQuery<HTMLDivElement>;

    if (div_outro_endereco[0].style.display == "block") {
        div_outro_endereco[0].style.display = "none";
        el.prop("checked", true);
        $('#outro').val("False");
        $('#outro').prop('checked', false);
    }
}
//Mostra os campos de contribuinte e IE
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
//Busca o cep ao sair do campo de cep do cadastro do cliente
window.DigitouCepCadastro = () => {
    let cep: string = $('#cep').val() as string;

    if (!!cep) {

        $.ajax({
            url: "../Cep/BuscarCep/",
            type: "GET",
            data: { cep: $('#cep').val() },
            dataType: "json",
            success: function (data) {
                if (!data || data.length !== 1) {
                    swal("Erro", "CEP inválido ou não encontrado.");
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
    else {
        swal("Erro", "É necessário informar um CEP válido!");
    }
}
//Validação de formulário
window.ValidarFormulario = () => {
    Loading.Carregando(true);

    let erroModal = new ErrorModal();
    let msg: string = "";

    if ($('#permiteEditar').val() == "True") {

        //vamos verificar se os campos obrigatórios estão vazios
        msg = verificarCamposObrigatorios();

        if (msg.length == 0) {

            //vamos converter os dados do cliente para dadosClienteCadastroDto e passar para  validar
            dadosClienteCadastroDto = converterParaDadosClienteCadastroDto();
            //vamos converter os telefones para validar
            dadosClienteCadastroDto = converterTelefones(dadosClienteCadastroDto);

            clienteCadastro = converterParaClienteCadastroDto();

            //vamos validar em outro arquivo os dados do cliente
            debugger;
            //ESTOU AQUI!!
            //PRECISO VALIDAR O ENDEREÇO DE ENTREGA CASO NÃO ESTEJA CADASTRANDO
            msg = validacoesCliente.ValidarDadosClienteCadastro(dadosClienteCadastroDto, lstIBGE, clienteCadastro);

            let cadastrando: boolean = $("#cadastrando").val() == "False" ? false : true as boolean;
            let outro_endereco: boolean = $('#outro').val() == "False" ? false : true as boolean;
            if (!cadastrando) {

                if (outro_endereco) {
                    //vamos converter para endereço entrega dto
                    let endEntrega: EnderecoEntregaClienteCadastroDto = cepEntrega.converterEntregaParaEnderecoEntregaClienteCadastroDto();

                    //vamos validar o endereço de entrega
                    msg += validacoesCliente.validarEnderecoEntregaDtoClienteCadastro(endEntrega, dadosClienteCadastroDto, lstIBGE);
                }
            }

            //verificar se tem msg para mostrar os erros na validação
            if (msg.length > 0) {
                Loading.Carregando(false);
                msg = "<b>Lista de erros:</b><br>" + msg;
                erroModal.ModalInnerHTML(msg);
                return false;
            }

        }
        else {
            if (msg.length > 0) {
                Loading.Carregando(false);
                msg = "<b>Preencha os campos marcados como obrigatório:</b><br>" + msg;
                erroModal.ModalInnerHTML(msg);
                return false;
            }
        }

        //vamos passar os telefones de ref
        ajustarTelRefComercial(clienteCadastro.RefComercial);
        ajustarTelRefBancaria(clienteCadastro.RefBancaria);
    }
}

window.ChecarLinha = (el: JQuery<HTMLTableElement>) => {
    $(".tr_linha").children().find("input:checked").prop("checked", false);
    el.children().find(".check").prop("checked", true);
}


/* ============ FIM DE CHAMADAS DIRETAS DA TELA ===========*/

/* ============ FUNÇÕES ===========*/
function MontarTelefonesTela(): void {
    //Montar telefones PF
    if ($("#tipo").val() == Constantes.ID_PF)
        MontarTelefonesPF();

    //Montar telefones PJ
    if ($("#tipo").val() == Constantes.ID_PJ)
        MontarTelefonesPJ();
}

function MontarTelefonesPF(): void {
    //RESIDENCIAL
    let dddRes: string = $('#dddRes').val() as string;
    let telRes: string = $('#telRes').val() as string;
    if (!!dddRes && !!telRes) {
        $('#telRes').val(dddRes + telRes);
    }
    ($('#telRes') as any).mask(MascaraParaTelefones(dddRes, telRes));
    //CELULAR
    let dddCel: string = $('#dddCel').val() as string;
    let celular: string = $('#celular').val() as string;
    if (!!dddCel && !!celular)
        $('#celular').val(dddCel + celular);
    ($('#celular') as any).mask(MascaraParaTelefones(dddCel, celular));
    //COMERCIAL
    let dddCom: string = $('#dddCom').val() as string;
    let telCom: string = $('#telCom').val() as string;
    if (!!dddCom && !!telCom)
        $('#telCom').val(dddCom + telCom);
    ($('#telCom') as any).mask(MascaraParaTelefones(dddCom, telCom));
}

function MontarTelefonesPJ(): void {

    //COMERCIAL
    let pj_dddCom: string = $('#dddCom').val() as string;
    let pj_telCom: string = $('#telCom').val() as string;
    if (!!pj_dddCom && !!pj_telCom)
        $('#telCom').val(pj_dddCom + pj_telCom);
    ($('#telCom') as any).mask(MascaraParaTelefones(pj_dddCom, pj_telCom));
    //COMERCIAL2
    let dddCom2: string = $('#dddCom2').val() as string;
    let telCom2: string = $('#telCom2').val() as string;
    if (!!dddCom2 && !!telCom2)
        $('#telCom2').val(dddCom2 + telCom2);
    ($('#telCom2') as any).mask(MascaraParaTelefones(dddCom2, telCom2));
}

function MascaraParaTelefones(ddd: string, tel: string): string {
    let mascara: string = "";
    if (!!ddd) {
        mascara = "(99) ";
    }
    if (!!tel) {
        //é celular
        if (tel.length > 8)
            mascara = mascara + "99999-9999";
        if (tel.length == 8)
            mascara = mascara + "9999-9999";
        if (tel.length < 8)
            mascara = mascara + "999-9999";
    }

    return mascara;
}

function ColocarMascaraNosTelefones() {

    //mascara de telefone para ref bancaria
    let indice: number = $("#indiceBancaria").val() as number;
    if (!!indice) {
        let cont: number = indice > 0 ? indice - 1 : 0;
        for (let i = 0; i <= indice; i++) {
            let telBanco: string = $("#" + cont + "-telBanco").val() as string;
            let dddBanco: string = $("#" + cont + "-dddBanco").val() as string;

            if (!!dddBanco && !!telBanco) {
                $("#" + cont + "-telBanco").val(dddBanco + telBanco);
            }
            ($("#" + cont + "-telBanco") as any).mask(MascaraParaTelefones(dddBanco, telBanco));
        }
    }

    //mascara de telefone para ref comercial
    let index = $("#index").val();
    if (!!index) {
        let cont: number = index > 0 ? indice - 1 : 0;
        for (let i = 0; i <= index; i++) {
            let telComercial: string = $("#" + cont + "-Telefone").val() as string;
            let dddComercial: string = $("#" + cont + "-Ddd").val() as string;

            if (!!dddComercial && !!telComercial) {
                $("#" + cont + "-Telefone").val(dddComercial + telComercial);
            }
            ($("#" + cont + "-Telefone") as any).mask(MascaraParaTelefones(dddComercial, telComercial));
            cont++;
        }
    }

}

function converterParaDadosClienteCadastroDto(): DadosClienteCadastroDto {
    //vamos converter o jquery para a classe

    dadosClienteCadastroDto.Loja = $("#loja").val() as string;
    dadosClienteCadastroDto.Indicador_Orcamentista = $("#usuario").val() as string;
    dadosClienteCadastroDto.Vendedor = "";
    dadosClienteCadastroDto.Id = $("#idCliente").val() as string;
    dadosClienteCadastroDto.Cnpj_Cpf = cpfCnpj;
    dadosClienteCadastroDto.Rg = $("#rg").val() as string;
    dadosClienteCadastroDto.Ie = $("#ie").val() as string;
    dadosClienteCadastroDto.Contribuinte_Icms_Status = parseInt($("#contribuinte")?.val() as string);
    dadosClienteCadastroDto.Tipo = $("#tipo").val() as string;
    dadosClienteCadastroDto.Observacao_Filiacao = $("#observacoes").val() as string;
    dadosClienteCadastroDto.Nascimento = $("#nascimento").val() as string;
    dadosClienteCadastroDto.Sexo = $("#sexo").val() as string;
    dadosClienteCadastroDto.Nome = dadosClienteCadastroDto.Tipo == Constantes.ID_PF ? $("#nome").val() as string : $("#razao").val() as string;
    dadosClienteCadastroDto.ProdutorRural = parseInt($('#produtor').val() as string);
    dadosClienteCadastroDto.Endereco = $("#endereco").val() as string;
    dadosClienteCadastroDto.Numero = $("#numero").val() as string;
    dadosClienteCadastroDto.Complemento = $("#complemento").val() as string;
    dadosClienteCadastroDto.Bairro = $("#bairro").val() as string;
    dadosClienteCadastroDto.Cidade = $("#cidade").val() as string;
    dadosClienteCadastroDto.Uf = $("#uf").val() as string;
    dadosClienteCadastroDto.Cep = $("#cep").val() as string;
    dadosClienteCadastroDto.DddResidencial = "";//precisa desconverter os telefones 
    dadosClienteCadastroDto.TelefoneResidencial = $("#telRes").val() as string;
    dadosClienteCadastroDto.DddComercial = "";
    dadosClienteCadastroDto.TelComercial = $("#telCom").val() as string;
    dadosClienteCadastroDto.Ramal = $("#ramal").val() as string;
    dadosClienteCadastroDto.DddCelular = "";//precisa desconverter os telefones 
    dadosClienteCadastroDto.Celular = $("#celular").val() as string;
    dadosClienteCadastroDto.TelComercial2 = $("#telCom2").val() as string;
    dadosClienteCadastroDto.DddComercial2 = "";//precisa desconverter os telefones 
    dadosClienteCadastroDto.Ramal2 = $("#ramal2").val() as string;
    dadosClienteCadastroDto.Email = $("#email").val() as string;
    dadosClienteCadastroDto.EmailXml = $("#emailXml").val() as string;
    dadosClienteCadastroDto.Contato = $("#contatoNaEmpresa").val() as string;

    return dadosClienteCadastroDto;
}

function converterParaClienteCadastroDto(): ClienteCadastroDto {
    clienteCadastro = new ClienteCadastroDto();
    //vamos passar dadosCliente para validar no caso de PJ
    clienteCadastro.DadosCliente = dadosClienteCadastroDto;

    //vamos montar o clienteCadatro para validar caso seja PJ
    if (dadosClienteCadastroDto.Tipo == Constantes.ID_PJ) {
        clienteCadastro.RefBancaria = new Array<RefBancariaDtoCliente>();
        clienteCadastro.RefBancaria = converterParaRefBancaria();
        clienteCadastro.RefComercial = new Array<RefComercialDtoCliente>();
        clienteCadastro.RefComercial = converterParaRefComercial();
    }

    return clienteCadastro;
}

function converterParaRefComercial(): Array<RefComercialDtoCliente> {
    let indiceComercial = $("#indiceComercial").val() as number;
    lstRefComercial = new Array<RefComercialDtoCliente>();
    for (let i = 0; i < indiceComercial; i++) {
        //pegamos os valores na tela
        let nome_Empresa: string = $("#" + i + "-Nome_Empresa").val() as string;
        let contato: string = $("#" + i + "-Contato").val() as string;
        let telefone: string = $("#" + i + "-Telefone").val() as string;
        let ddd: string = $("#" + i + "-Ddd").val() as string;

        //vamos passar para referência comercial
        let refComercial: RefComercialDtoCliente = new RefComercialDtoCliente();
        refComercial.Nome_Empresa = nome_Empresa;
        refComercial.Contato = contato;
        refComercial.Telefone = telefone;
        refComercial.Ddd = ddd;
        refComercial = converterTelefoneRefComercial(refComercial);

        lstRefComercial.push(refComercial);
    }

    return lstRefComercial;
}

function ajustarTelRefComercial(lstRefComercial: Array<RefComercialDtoCliente>): void {
    let indiceComercial = $("#indiceComercial").val() as number;
    for (let i = 0; i < indiceComercial; i++) {
        $("#" + i + "-Telefone").val(lstRefComercial[i].Telefone);
    }
}
function ajustarTelRefBancaria(lstRefBancaria: Array<RefBancariaDtoCliente>): void {
    let indiceBancaria = $("#indiceBancaria").val() as number;
    for (let i = 0; i < indiceBancaria; i++) {
        $("#" + i + "-telBanco").val(lstRefBancaria[i].Telefone);
    }
}
function converterParaRefBancaria(): Array<RefBancariaDtoCliente> {
    //vamos fazer um foreach para as referências
    let indiceBacaria = $("#indiceBancaria").val() as number;
    lstRefBancaria = new Array<RefBancariaDtoCliente>();

    for (let i = 0; i < indiceBacaria; i++) {
        let banco: string = $("#" + i + "-banco").val() as string;
        let agencia: string = $("#" + i + "-agencia").val() as string;
        let conta: string = $("#" + i + "-conta").val() as string;
        let telBanco: string = $("#" + i + "-telBanco").val() as string;
        let dddBanco: string = $("#" + i + "-dddBanco").val() as string;
        let contatoBanco: string = $("#" + i + "-contatoBanco").val() as string;

        //vamos passar para referência bancária
        let refBancaria: RefBancariaDtoCliente = new RefBancariaDtoCliente();
        refBancaria.Banco = banco;
        refBancaria.Agencia = agencia;
        refBancaria.Conta = conta;
        refBancaria.Contato = contatoBanco;
        refBancaria.Telefone = telBanco;
        refBancaria.Ddd = dddBanco;

        refBancaria = converterTelefoneRefBancaria(refBancaria);

        lstRefBancaria.push(refBancaria);
    }

    return lstRefBancaria;
}

function verificarCamposObrigatorios() {
    let msg: string = "";

    msg += verificarCamposObrigatoriosCliente();

    msg += verificarCamposObrigatoriosEndereco();

    return msg;
}

function verificarCamposObrigatoriosCliente(): string {
    let msg: string = "";

    if ($("#tipo").val() == Constantes.ID_PF) {
        if ($('#cpf_cnpj').val() == '') {
            $("#cpf_cnpj").addClass("is-invalid");
            msg += "Favor informar CPF ou CNPJ<br>";
        }
        if ($("#nome").val() == '') {
            $("#nome").addClass("is-invalid");
            msg += "Favor informar nome do cliente<br>";
        }
        if ($("#telRes").val() == '' &&
            $("#celular").val() == '' &&
            $("#telCom").val() == '') {
            $("#telRes").addClass("is-invalid");
            $("#celular").addClass("is-invalid");
            $("#telCom").addClass("is-invalid");
            msg += "Favor preencher ao menos um número de telefone!<br>";
        }
    }

    if ($("#tipo").val() == Constantes.ID_PJ) {
        if ($("#razao").val() == "") {
            msg += "Favor informar a Razão Social do cliente<br>";
            $("#razao").addClass("is-invalid");
        }
        if ($("#contatoNaEmpresa").val() == "") {
            msg += "Favor informar o nome da pessoa para contato!<br>";
            $("#contatoNaEmpresa").addClass("is-invalid");
        }
        if ($("#email").val() == "") {
            msg += "É obrigatório informar um endereço de e-mail.<br>";
            $("#email").addClass("is-invalid");
        }
        if ($("#telCom").val() == "" &&
            $("#telCom2").val() == "") {
            msg += "Favor preencher ao menos um número de telefone!<br>";
            $("#telCom").addClass("is-invalid");
            $("#telCom2").addClass("is-invalid");
        }
        if ($("#cep").val() == "") {
            msg += "Preencha o campo CEP.<br>";
            $("#cep").addClass("is-invalid");
        }
        if ($("#endereco").val() == "") {
            msg += "Preencha o campo endereço.<br>";
            $("#endereco").addClass("is-invalid");
        }
        if ($("#numero").val() == "") {
            msg += "Preencha o número do endereço.<br>";
            $("#numero").addClass("is-invalid");
        }
        if ($("#bairro").val() == "") {
            msg += "Preencha o bairro do endereço.<br>";
            $("#bairro").addClass("is-invalid");
        }
        if ($("#cidade").val() == "") {
            msg += "Preencha a cidade do endereço.<br>";
            $("#cidade").addClass("is-invalid");
        }
        if ($("#uf").val() == "") {
            msg += "Preencha o UF do endereço.<br>";
            $("#uf").addClass("is-invalid");
        }
    }

    return msg;
}

function verificarCamposObrigatoriosEndereco(): string {
    let msg: string = "";

    if ($("#cep").val() == "") {
        $("#cep").addClass("is-invalid");
        msg += "Informe o CEP!<br>"
    }
    if ($("#endereco").val() == "") {
        $("#endereco").addClass("is-invalid");
        msg += "Informe o endereço!<br>"
    }
    if ($("#numero").val() == "") {
        $("#numero").addClass("is-invalid");
        msg += "Informe o número do endereço!<br>"
    }
    if ($("#bairro").val() == "") {
        $("#bairro").addClass("is-invalid");
        msg += "Informe o bairro do endereço!<br>"
    }
    if ($("#cidade").val() == "") {
        $("#cidade").addClass("is-invalid");
        msg += "Informe a cidade do endereço!<br>"
    }
    if ($("#uf").val() == "") {
        $("#uf").addClass("is-invalid");
        msg += "Informe o UF do endereço!<br>"
    }

    return msg;
}

function converterTelefoneRefBancaria(ref: RefBancariaDtoCliente): RefBancariaDtoCliente {
    let s: any;
    if (!!ref.Telefone && !!ref.Ddd) {
        s = FormatarTelefone.SepararTelefone(ref.Telefone);
        ref.Telefone = s.Telefone;
        ref.Ddd = s.Ddd;
    }
    if (!!ref.Telefone && !ref.Ddd) {
        ref.Telefone = ref.Telefone.replace("-", "");
    }

    return ref;
}

function converterTelefoneRefComercial(ref: RefComercialDtoCliente): RefComercialDtoCliente {
    let s: any;

    if (!!ref.Telefone && !!ref.Ddd) {
        s = FormatarTelefone.SepararTelefone(ref.Telefone);
        ref.Telefone = s.Telefone;
        ref.Ddd = s.Ddd;
    }
    if (!!ref.Telefone && !ref.Ddd) {
        ref.Telefone = ref.Telefone.replace("-", "");
    }
    return ref;
}

function converterTelefones(dados: DadosClienteCadastroDto): DadosClienteCadastroDto {

    let s: any;
    if (!!dados.TelefoneResidencial) {
        s = FormatarTelefone.SepararTelefone(dados.TelefoneResidencial);
        dados.TelefoneResidencial = s.Telefone;
        $("#telRes").val(dados.TelefoneResidencial);
        dados.DddResidencial = s.Ddd;
        $("#dddRes").val(dados.DddResidencial);
    }

    if (!!dados.Celular) {
        s = FormatarTelefone.SepararTelefone(dados.Celular);
        dados.Celular = s.Telefone;
        $("#celular").val(dados.Celular);
        dados.DddCelular = s.Ddd;
        $("#dddCel").val(dados.DddCelular);
    }

    if (!!dados.TelComercial) {
        s = FormatarTelefone.SepararTelefone(dados.TelComercial);
        dados.TelComercial = s.Telefone;
        $("#telCom").val(dados.TelComercial);
        dados.DddComercial = s.Ddd;
        $("#dddCom").val(dados.DddComercial);
    }

    if (dados.Tipo == Constantes.ID_PJ) {
        if (!!dados.TelComercial2) {
            s = FormatarTelefone.SepararTelefone(dados.TelComercial2);
            dados.TelComercial2 = s.Telefone;
            $("#telCom2").val(dados.TelComercial2);
            dados.DddComercial2 = s.Ddd;
            $("#dddCom2").val(dados.DddComercial2);

        }

        //for (let i = 0; i < this.clienteCadastroDto.RefBancaria.length; i++) {
        //    let este = this.clienteCadastroDto.RefBancaria[i];
        //    let s = FormatarTelefone.SepararTelefone(este.Telefone);
        //    este.Telefone = s.Telefone;
        //    este.Ddd = s.Ddd;
        //}

        ////converter referências comerciais
        //for (let i = 0; i < this.clienteCadastroDto.RefComercial.length; i++) {
        //    let este = this.clienteCadastroDto.RefComercial[i];
        //    let s = FormatarTelefone.SepararTelefone(este.Telefone);
        //    este.Telefone = s.Telefone;
        //    este.Ddd = s.Ddd;
        //}
    }



    return dados;
}
//OBS => deixando aqui para caso haja necessidade de utilização
function desconverterTelefones() {
    {
        this.dadosClienteCadastroDto.TelefoneResidencial = this.dadosClienteCadastroDto.DddResidencial + this.dadosClienteCadastroDto.TelefoneResidencial;

        this.dadosClienteCadastroDto.Celular = this.dadosClienteCadastroDto.DddCelular + this.dadosClienteCadastroDto.Celular;

        this.dadosClienteCadastroDto.TelComercial = this.dadosClienteCadastroDto.DddComercial + this.dadosClienteCadastroDto.TelComercial;

        this.dadosClienteCadastroDto.TelComercial2 = this.dadosClienteCadastroDto.DddComercial2 + this.dadosClienteCadastroDto.TelComercial2;
    }

    //converter referências bancárias
    for (let i = 0; i < this.clienteCadastroDto.RefBancaria.length; i++) {
        let este = this.clienteCadastroDto.RefBancaria[i];
        este.Telefone = este.Ddd + este.Telefone;
    }

    //converter referências comerciais
    for (let i = 0; i < this.clienteCadastroDto.RefComercial.length; i++) {
        let este = this.clienteCadastroDto.RefComercial[i];
        este.Telefone = este.Ddd + este.Telefone;
    }

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

//Atribui os dados para a classe de CepDto
function montarCepDto(linha: HTMLCollection): CepDto {
    cepDto.Cep = linha[1].textContent;
    cepDto.Uf = linha[2].textContent;
    cepDto.Cidade = linha[3].textContent;
    cepDto.Bairro = linha[4].textContent;
    cepDto.Endereco = linha[5].textContent;
    return cepDto;
}

//Monta campos, limpa dados, atribui na tela
function inscreve(linha: JQuery<HTMLElement>) {
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

//ARRUMAR O ALINHEMENTO DOS TD'S DA MODAL QUANDO VEM A LISTA DE CEPS
//incluir os campos de da memorizacao de endereço no cep

function limparModal() {
    $("#tableBody").children().remove();
    $(".tabela_endereco").hide();
    $("#msg").hide();

    let a: HTMLSelectElement = $("#lstufs")[0] as HTMLSelectElement;
    a.selectedIndex = 0;
    $("#localidade").val("");
    $("#nendereco").val("");
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

    //vamos limpar o CepDto
    limparCepDto();
}

function limparCamposEndereco() {
    $("#cep").val('');
    $("#cep").removeClass('is-invalid');
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

/* ========== FIM DAS FUNÇÕES =========== */