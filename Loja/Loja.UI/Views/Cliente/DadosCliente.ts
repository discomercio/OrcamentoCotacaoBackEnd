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
import { FormatarTelefone } from "../../UtilTs/Fomatar/Mascaras/formataTelefone";
import { EnderecoEntregaClienteCadastroDto } from "../../DtosTs/ClienteDto/EnderecoEntregaClienteCadastroDto";
import { contains } from "jquery";

declare var window: any;
declare function swal(header, msg): any;


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



$(function () {

    ($('#cepEntrega') as any).mask("00000-000");
    ($('#cep') as any).mask("00000-000");
    ($('#ie') as any).mask("000.000.000.000");
    

    

    MontarTelefonesTela();
    MaskTelRefComercial();
    MaskTelRefBancaria();
    //afazer: no carregamento de tela, verificar 
    //se é PF : não produtor, contribuinte e IE serão bloqueados
    //Se PJ : não contribuite, IE será bloqueado
    //obs: fazer um onchange para controlar o bloqueio e desbloqueio
});
/*=========== Não sei o CEP ===========*/


//$("#lstufs").ready(function () {
//    $.ajax({
//        url: "../Cep/BuscarUfs/",
//        type: "GET",
//        dataType: "json",
//        success: function (data) {
//            var select = $('#lstufs');
//            $.each(data, function (i, d) {
//                select.append("<option value='" + d + "'>" + d + "</option>");
//            });
//        }
//    });
//});







/*================FIM CONROLE DE CAMPOS =================*/

/* ========== CHAMADAS DIRETAS DA TELA ====================================*/
/* VALIDAÇÃO DE NOME */
window.VerificaCampo = (el: JQuery<HTMLInputElement>) => {
    let valor: string = el.val() as string;
    if (valor == "") el.addClass("is-invalid");
    if (!!valor && valor.toString().length > 0) el.removeClass("is-invalid");
}

window.VerificarTelefonePF = () => {
    let resTel: string = $("#telRes").val() as string;
    let celTel: string = $("#celular").val() as string;
    let comTel: string = $("#telCom").val() as string;
    
    if (!resTel || !celTel || !comTel) {
        $("#telRes").removeClass("is-invalid");
        $("#celular").removeClass("is-invalid");
        $("#telCom").removeClass("is-invalid");
    }
}

window.VerificarTelefonePJ = () => {
    let telCom: string = $("#telCom").val() as string;
    let telCom2: string = $("#telCom2").val() as string;

    if (!telCom || !telCom2) {
        $("#telCom").removeClass("is-invalid");
        $("#telCom2").removeClass("is-invalid");
    }
}

/* ADICIONAR REFERÊNCIA COMERCIAL */
window.AddRefComercial = () => {
    let index: number = Number($('#index').val());

    if (index > 2) {
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
    linha += "<input name='[" + index + "].Telefone' id='" + index + "-Telefone' type='text' class='form-control' onkeyup='IncluirMascara($(this))'  />";
    linha += "</div>"
    linha += "<input type='hidden' id='" + index + "'-Ordem' name='[" + index + "].Ordem' value='" + (index + 1) + "' /> ";
    linha += "</div>";

    $('#collapsible-body-comercial').append(linha);
    ($('#' + index + '-Telefone') as any).mask("(00) 0000-0000");

    index++;
    $('#index').val(index);
}
/* ADICIONAR REFERÊNCIA BANCÁRIA */
window.AddRefBancaria = () => {
    let indice: number = Number($('#indice').val());
    debugger;
    if (indice > 0) {
        swal("", "Máximo de 1 referência bancária.");
        return false;
    }
    let clone: any = $('#modelo_refBancaria').clone();
    let idNovo: string = "RefBancariabody-" + indice;
    clone.attr("id", idNovo);
    $("#card-body-bancaria").append(clone);
    clone.css("display", "inline-flex");
    $("#" + idNovo + "").click();
    indice++;
    $('#indice').val(indice);
}

/* MASCARA PARA TELEFONE */
window.IncluirMascara = (el: JQuery<HTMLInputElement>) => {
    MascaraTelefones(el);
}

/* MOSTRA A DIV DE ENDEREÇO DE ENTREGA */
window.mostraDiv = (el: JQuery<HTMLInputElement>) => {

    let div_outro_endereco: JQuery<HTMLDivElement> = $("#outro_endereco") as JQuery<HTMLDivElement>;

    if (div_outro_endereco[0].style.display == "none") {
        div_outro_endereco[0].style.display = "block";
        el.val("True");
        el.prop("checked", true);
        $('#mesmo').prop('checked', false);
    }
}

/* ESCONDE A DIV DE ENDEREÇO DE ENTREGA */
window.fechaDiv = (el: JQuery<HTMLInputElement>) => {

    let div_outro_endereco: JQuery<HTMLDivElement> = $("#outro_endereco") as JQuery<HTMLDivElement>;

    if (div_outro_endereco[0].style.display == "block") {
        div_outro_endereco[0].style.display = "none";
        el.prop("checked", true);
        $('#outro').val("False");
        $('#outro').prop('checked', false);
    }
}

/* MOSTRA OS CAMPOS DE CONTRIBUINTE E IE */
window.MostrarDivs = () => {
    if (Number($("#produtor").val()) == Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) {
        $("#div_ie").css('visibility', 'visible');
        $("#div_contribuinte").css('visibility', 'visible');
    }
    if (Number($("#produtor").val()) != Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) {
        $("#div_ie").css('visibility', 'hidden');
        $("#div_contribuinte").css('visibility', 'hidden');
        $("#ie").val();
        $("#contribuinte").val();
    }
}




/* VALIDAÇÃO DE FORMULÁRIO */
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

            //ESTOU AQUI!!
            //PRECISO VALIDAR O ENDEREÇO DE ENTREGA CASO NÃO ESTEJA CADASTRANDO
            //msg = validacoesCliente.ValidarDadosClienteCadastro(dadosClienteCadastroDto, lstIBGE, clienteCadastro);

            let cadastrando: boolean = $("#cadastrando").val() == "False" ? false : true as boolean;
            let outro_endereco: boolean = $('#outro').val() == "False" ? false : true as boolean;
            if (!cadastrando) {

                if (outro_endereco) {
                    //vamos converter para endereço entrega dto
                    //let endEntrega: EnderecoEntregaClienteCadastroDto = cepEntrega.converterEntregaParaEnderecoEntregaClienteCadastroDto();

                    //vamos validar o endereço de entrega
                    //msg += validacoesCliente.validarEnderecoEntregaDtoClienteCadastro(endEntrega, dadosClienteCadastroDto, lstIBGE);
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
function MascaraTelefones(el: JQuery<HTMLInputElement>): void {
    (el as any).mask("(00) 0000-00009");
    (el as any).focusout(function (event) {
        let target, phone, element;
        target = (event.currentTarget) ? event.currentTarget : event.srcElement;
        phone = target.value.replace(/\D/g, '');
        element = $(target);
        element.unmask();
        if (phone.length > 10) {
            element.mask("(00) 00000-0009");
        } else {
            element.mask("(00) 0000-0009");
        }
    });
}

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
    $('#telRes').val(dddRes + telRes);
    MascaraTelefones($('#telRes'));

    //CELULAR
    let dddCel: string = $('#dddCel').val() as string;
    let celular: string = $('#celular').val() as string;
    $('#celular').val(dddCel + celular);
    MascaraTelefones($('#celular'));

    //COMERCIAL
    let dddCom: string = $('#dddCom').val() as string;
    let telCom: string = $('#telCom').val() as string;
    $('#telCom').val(dddCom + telCom);
    MascaraTelefones($('#telCom'));
}

function MontarTelefonesPJ(): void {
    //COMERCIAL
    let pj_dddCom: string = $('#dddCom').val() as string;
    let pj_telCom: string = $('#telCom').val() as string;
    $('#telCom').val(pj_dddCom + pj_telCom);
    MascaraTelefones($('#telCom'));

    //COMERCIAL2
    let dddCom2: string = $('#dddCom2').val() as string;
    let telCom2: string = $('#telCom2').val() as string;
    $('#telCom2').val(dddCom2 + telCom2);
    MascaraTelefones($('#telCom2'));

}

function MaskTelRefBancaria() {
    let indice: number = $("#indiceBancaria").val() as number;
    if (!!indice) {
        for (let i = 0; i <= indice; i++) {
            let telBanco: string = $("#" + i + "-telBanco").val() as string;
            let dddBanco: string = $("#" + i + "-dddBanco").val() as string;

            if (!!dddBanco && !!telBanco) {
                $("#" + i + "-telBanco").val(dddBanco + telBanco);
            }
            MascaraTelefones($("#" + i + "-telBanco"));
        }
    }

}

function MaskTelRefComercial() {
    let index: number = $("#index").val() as number;
    if (!!index) {
        for (let i = 0; i < index; i++) {
            let telComercial: string = $("#" + i + "-Telefone").val() as string;
            let dddComercial: string = $("#" + i + "-Ddd").val() as string;
            MascaraTelefones($("#" + i + "-Telefone"));
        }
    }
}

function converterParaDadosClienteCadastroDto(): DadosClienteCadastroDto {
    //vamos converter o jquery para a classe

    dadosClienteCadastroDto.Loja = $("#loja").val() as string;
    dadosClienteCadastroDto.Indicador_Orcamentista = $("#usuario").val() as string;
    dadosClienteCadastroDto.Vendedor = "";
    dadosClienteCadastroDto.Id = $("#idCliente").val() as string;
    dadosClienteCadastroDto.Cnpj_Cpf = $("#cpfCnpj").val() as string;
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






//ARRUMAR O ALINHEMENTO DOS TD'S DA MODAL QUANDO VEM A LISTA DE CEPS
//incluir os campos de da memorizacao de endereço no cep









/* ========== FIM DAS FUNÇÕES =========== */