import { StringUtils } from "../../UtilTs/stringUtils/stringUtils";
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

/* NOVA VALIDAÇÃO PARA FAZER
 *  Teste cadastro de cliente PF no Asp => 
 *  Campos obrigatórios => CPF, Produtor rural, nome, endereço e apenas 1 telefone preenchido
 *  Informações sobre a validação => 
 *      não é permitido alterar:
 *          cpf ou cnpj do cliente = readonly;
 *      Valida apenas se esta vazio:
 *          endereço, numero, bairro
 *      Valida no ibge: 
 *          cidade, estado
 *      Valida telefone normalmente
 *      
 *      
 *      
 */




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
    if ($("#tipo").val() == Constantes.ID_PF) {
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
    if ($("#uf").val() == "") {
        $("#uf").addClass("is-invalid");
    }
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
        //validar se o email esta ok
    }
});
$("#emailXml").blur(() => {
    if ($("#emailXml").val() == "") $("#emailXml").addClass("is-invalid");
    if ($("#emailXml").val()?.toString().length > 0 && $("#emailXml").val() != "") {
        $("#emailXml").removeClass("is-invalid");
        //verificar se o email esta ok
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

//Busca o cep ao sair do campo de cep do cadastro do cliente
window.DigitouCepCadastro = () => {
    let cep: string = $('#cep').val() as string;

    debugger;
    if (!!cep) {

        $.ajax({
            url: "../Cep/BuscarCep/",
            type: "GET",
            data: { cep: $('#cep').val() },
            dataType: "json",
            success: function (data) {
                if (!data || data.length !== 1) {
                    swal("", "CEP inválido ou não encontrado.");
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
        swal("", "É necessário informar um CEP válido!");
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
    dadosClienteCadastroDto.Contribuinte_Icms_Status = $("#contribuinte")?.val() as number;
    dadosClienteCadastroDto.Tipo = $("#tipoCliente").val() as string;
    dadosClienteCadastroDto.Observacao_Filiacao = $("#observacoes").val() as string;
    dadosClienteCadastroDto.Nascimento = $("#nascimento").val() as string;
    dadosClienteCadastroDto.Sexo = $("#sexo").val() as string;
    dadosClienteCadastroDto.Nome = $("#nome").val() as string;
    dadosClienteCadastroDto.ProdutorRural = $('#produtor').val() as number;
    dadosClienteCadastroDto.Endereco = $("#endereco").val() as string;
    dadosClienteCadastroDto.Numero = $("#numero").val() as string;
    dadosClienteCadastroDto.Complemento = $("#complemento").val() as string;
    dadosClienteCadastroDto.Bairro = $("#bairro").val() as string;
    dadosClienteCadastroDto.Cidade = $("#cidade").val() as string;
    dadosClienteCadastroDto.Uf = $("#uf").val() as string;
    dadosClienteCadastroDto.Cep = $("#endereco").val() as string;
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
    dadosClienteCadastroDto.Contato = $("#contato").val() as string;

    return dadosClienteCadastroDto;
}

function converterParaClienteCadastroDto(): ClienteCadastroDto {
    clienteCadastro.DadosCliente = dadosClienteCadastroDto;
    //vamos fazer um foreach para as referências
    let indice = $("#indice").val() as number;
    debugger;
    return clienteCadastro;
}

function converterParaRefBancaria(): RefBancariaDtoCliente {
    let ref: RefBancariaDtoCliente = new RefBancariaDtoCliente();



    return ref;
}

function converterParaRefComercial(): RefComercialDtoCliente {
    let ref: RefComercialDtoCliente = new RefComercialDtoCliente();

    return ref;
}

window.ValidarFormulario = () => {
    Loading.Carregando(true);

    let erroModal = new ErrorModal();
    let msg: string = "";

    if ($('#permiteEditar').val() == "True") {
        //CPF
        //vamos verificar se os campos obrigatórios estão vazios
        msg = verificarCamposObrigatorios();

        if (msg.length == 0) {
            if (cpfCnpj) {
                //vamos converter os dados do cliente para dadosClienteCadastroDto e passar para  
                dadosClienteCadastroDto = converterParaDadosClienteCadastroDto();
                clienteCadastro = converterParaClienteCadastroDto();
                //vamos validar em outro arquivo os dados do cliente
                //ESTOU AQUI, PREPARANDO OS DADOS PARA VALIDAR
                msg = validacoesCliente.ValidarDadosClienteCadastro(dadosClienteCadastroDto, lstIBGE, clienteCadastro);

                //verificar se tem msg para mostrar os erros na validação
            }
        }




        if (CpfCnpjUtils.cnpj_cpf_ok(CpfCnpjUtils.cnpj_cpf_formata(cpfCnpj))) {
            //Nova validação a ser feita
            //verificar se o cpf esta ok
            //verificar se os telefones estão ok
            //verificar se a cidade e estado corresponde ao cep ou algo desse tipo

            //validar campos de cadastro
            if ($('#cpf_cnpj').val() == '') {
                $("#cpf_cnpj").addClass("is-invalid");
                msg += "Favor informar CPF ou CNPJ<br>";
            }
            else {
                //vamos verificar se o cpf esta ok
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
            else {
                //vamos verificar se os telefones estão ok
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
        //if (!$('#indicador').val() || $('#indicador').val() == "undefined" || $('#indicador').val() == "0") {
        //    $("#indicador").addClass("is-invalid");
        //    msg += "Favor selecionar um indicador!<br>";
        //}

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

function verificarCamposObrigatorios() {
    let msg: string = "";

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



    return msg;
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


//CEP
$("#btnModificar").click(function () {
    let tr_linha: JQuery<HTMLElement> = $(".tr_linha").children().find(":checked").closest(".tr_linha");
    inscreve(tr_linha);
});

$('#btnBuscar').on('click', function () {
    $.ajax({
        url: "../Cep/BuscarCepPorEndereco/",
        type: "GET",
        data: { nendereco: $('#nendereco').val(), localidade: $('#localidade').val(), lstufs: $('#lstufs').val() },
        dataType: "json",
        success: function (t) {
            montaTabela(t);
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

function montaTabela(data: any) {
    var cols = "";
    var lst = data["ListaCep"];
    if (lst.length > 0) {
        if ($('#msg').css("display", "block")) {
            $('#msg').css("display", "none");
        }
        $('.tabela_endereco').css("display", "block");
        for (var i = 0; i < lst.length; i++) {
            cols += "<tr id='linha' class='tr_linha'>";
            cols += "<td style='width: 3vw!important;'>";
            cols += "<label><input class='check' type='radio' value='" + i + "'></input><span></span></label>";
            cols += "</td>";
            cols += "<td style='width: 7vw!important;'>" + lst[i].Cep + "</td>";
            cols += "<td style='width: 3vw!important;'>" + lst[i].Uf + "</td>";
            cols += "<td style='width: 10vw!important;'>" + lst[i].Cidade + "</td>";
            cols += "<td style='width: 10vw!important;'>" + lst[i].Bairro + "</td>";
            cols += "<td style='width: 10vw!important;'>" + lst[i].Endereco + "</td>";
            cols += "<td style='width: 10vw!important;'>" + lst[i].LogradouroComplemento + "</td></tr>";
            $("#tableBody").empty().append(cols);
        }

        $(".tr_linha").click(function () {

            $(this).find('td').each(function (i) {
                if ($(this).find('label')) {
                    $(this).find('label').each(function (s) {
                        if ($(this).find('input')) {
                            $(this).find('input').each(function (p) {

                                var cbs = document.getElementsByClassName("check");
                                //cbs = $(this)
                                for (var i = 0; i < cbs.length; i++) {
                                    //if (cbs[i] !== $(this)) cbs[i].checked = false;
                                }
                                $(this).prop('checked', true);
                            })
                        }
                    })
                }
            });
        });
    }
    else {
        if ($('.tabela_endereco').css("display", "block")) $('.tabela_endereco').css("display", "none");
        var msg = "<span> Endereço não encontrado!</span>";
        $("#msg").css("display", "block");
        $("#msg").empty().append(msg);
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
            cepEntrega.atribuirDadosParaEnderecoEntrega(cepDto);
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
    $(".tabela_endereco").remove();
    let a: HTMLSelectElement = $("#lstufs")[0] as HTMLSelectElement;
    a.selectedIndex = 0;
    $("#localidade").val("");
    $("#nendereco").val("");
    debugger;
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


function limparCamposEndereco() {
    $("#cep").val('');
    $("#endereco").val("");
    $("#numero").val("");
    $("#complemento").val("");
    $("#bairro").val("");
    $("#cidade").val("");
    $("#uf").val("");
}

function limparCepDto() {
    cepDto.Cep = "";
    cepDto.Uf = "";
    cepDto.Cidade = "";
    cepDto.Bairro = "";
    cepDto.Endereco = "";
}