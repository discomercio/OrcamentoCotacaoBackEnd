import { CepDto } from "../../DtosTs/CepDto/CepDto";
import { Constantes } from "../../UtilTs/Constantes/Constantes";
import { EnderecoEntregaClienteCadastroDto } from "../../DtosTs/ClienteDto/EnderecoEntregaClienteCadastroDto";

export class CepEntrega {
    constructor() {
        $(document).ready(() => {
            this.tipoClienteEntrega = Constantes.ID_PF;
            $("#EndEntregaTipoPF").click();            
        });
    }

    public tipoClienteEntrega: string;
    public lstIBGE_Entrega: string[] = new Array();

    public limparCamposEndEntrega(): void {
        $('#cepEntrega').val('');
        $('#ufEntrega').val('');
        $('#cidadeEntrega').val('');
        $('#bairroEntrega').val('');
        $('#enderecoEntrega').val('');
        $('#compEntrega').val('');
        $('#numEntrega').val('');
    }

    public atribuirDadosParaEntrega(end: CepDto): void {
        $("#cepEntrega").val(end.Cep);
        ($("#cepEntrega") as any).mask("99999-999");
        $("#lblcep").addClass('active');

        if (!!end.Bairro) {
            $("#bairroEntrega").val(end.Bairro);
            $("#lblBairroEntrega").addClass('active');
        }
        if (!!end.Cidade) {
            debugger;
            if (!!end.ListaCidadeIBGE && end.ListaCidadeIBGE.length > 0) {
                $("#cidadeEntrega").prop("readonly", false);

                //lstIBGE_Entrega1 = end.ListaCidadeIBGE;
            }
            else {
                $("#cidadeEntrega").prop("readonly", true);
                $("#cidadeEntrega").val(end.Cidade);
                $("#lblCidadeEntrega").addClass('active');
            }
        }
        if (!!end.Endereco) {
            $("#enderecoEntrega").val(end.Endereco);
            $("#lbEntrega").addClass('active');
        }
        if (!!end.Uf) {
            $("#ufEntrega").val(end.Uf);
            $("#lblUfEntrega").addClass('active');
        }

        $("#numEntrega").val('');
        $("#compEntrega").val('');
    }

    public converterEntregaParaEnderecoEntregaClienteCadastroDto(): EnderecoEntregaClienteCadastroDto {
        let enderecoEntregaClienteDto: EnderecoEntregaClienteCadastroDto = new EnderecoEntregaClienteCadastroDto();

        enderecoEntregaClienteDto.EndEtg_endereco = $("#enderecoEntrega").val() as string;
        enderecoEntregaClienteDto.EndEtg_endereco_numero = $("#numEntrega").val() as string;
        enderecoEntregaClienteDto.EndEtg_endereco_complemento = $("#compEntrega").val() as string;
        enderecoEntregaClienteDto.EndEtg_bairro = $("#bairroEntrega").val() as string;
        enderecoEntregaClienteDto.EndEtg_cidade = $("#cidadeEntrega").val() as string;
        enderecoEntregaClienteDto.EndEtg_uf = $("#ufEntrega").val() as string;
        enderecoEntregaClienteDto.EndEtg_cep = $("#cepEntrega").val() as string;
        //codigo da justificativa, preenchdio quando está criando (do spa para a api)
        enderecoEntregaClienteDto.EndEtg_cod_justificativa = $("#justificativa").val() as string;
        //descrição da justificativa, preenchdio para mostrar (da api para o spa)
        //enderecoEntregaClienteDto.EndEtg_descricao_justificativa = $("#").val() as string;
        //se foi selecionado um endereco diferente para a entrega (do spa para a api)
        enderecoEntregaClienteDto.OutroEndereco = $("#outro").val() == "False" ? false : true as boolean;

        //novos campos
        enderecoEntregaClienteDto.EndEtg_email = $("#endEntregaEmail").val() as string;
        enderecoEntregaClienteDto.EndEtg_email_xml = $("#endEntregaEmailXml").val() as string;
        enderecoEntregaClienteDto.EndEtg_nome = this.tipoClienteEntrega == Constantes.ID_PF ?
            $("#endEntregaNome").val() as string : $("#endEntregaRazao").val() as string;
        enderecoEntregaClienteDto.EndEtg_ddd_res = $("#endEntregaDddRes").val() as string;
        enderecoEntregaClienteDto.EndEtg_tel_res = $("#endEntregaTelRes").val() as string;
        enderecoEntregaClienteDto.EndEtg_ddd_com = $("#endEntregaDddCom1").val() as string;
        enderecoEntregaClienteDto.EndEtg_tel_com = $("#endEntregaTelCom1").val() as string;
        enderecoEntregaClienteDto.EndEtg_ramal_com = $("#endEntregaRamal1").val() as string;
        enderecoEntregaClienteDto.EndEtg_ddd_cel = $("#endEntregaDddCel").val() as string;
        enderecoEntregaClienteDto.EndEtg_tel_cel = $("#endEntregaCel").val() as string;
        enderecoEntregaClienteDto.EndEtg_ddd_com_2 = $("#endEntregaDddCom2").val() as string;
        enderecoEntregaClienteDto.EndEtg_tel_com_2 = $("#endEntregaTelCom2").val() as string;
        enderecoEntregaClienteDto.EndEtg_ramal_com_2 = $("#endEntregaRamal2").val() as string;
        enderecoEntregaClienteDto.EndEtg_tipo_pessoa = this.tipoClienteEntrega == Constantes.ID_PF ? Constantes.ID_PF : Constantes.ID_PJ;
        enderecoEntregaClienteDto.EndEtg_cnpj_cpf = this.tipoClienteEntrega == Constantes.ID_PF ?
            $("#endEntregaCPF").val() as string : $("#endEntregaCNPJ").val() as string;
        enderecoEntregaClienteDto.EndEtg_contribuinte_icms_status = $("#endEntregaContribuinte").val() as number;
        enderecoEntregaClienteDto.EndEtg_produtor_rural_status = $("#endEntregaProdutor").val() as number;
        enderecoEntregaClienteDto.EndEtg_ie = $("#endEntregaIE").val() as string;
        enderecoEntregaClienteDto.EndEtg_rg = $("#endEntregaRg").val() as string;
        enderecoEntregaClienteDto.St_memorizacao_completa_enderecos = $("#endEntregaStMemorizacao").val() as number;

        return enderecoEntregaClienteDto;
    }
}

declare var window: any;

window.IncluirMascara = (el: JQuery<HTMLInputElement>) => {
    MascaraTelefones(el);
}

window.TipoPessoaEntrega = (el: JQuery<HTMLInputElement>) => {
    let tipoPessoa: string = (el.val() as string)
    if (tipoPessoa == Constantes.ID_PF)
        PessoaFisica();
    if (tipoPessoa == Constantes.ID_PJ)
        PessoaJuridica();
}

window.buscarCep = (el: JQuery<HTMLInputElement>) => {

    if (!!el.val()) {
        $('.container_cep').addClass('carregando');

        $.ajax({
            url: "../Cep/BuscarCep/",
            type: "GET",
            data: { cep: $('#cepEntrega').val() },
            dataType: "json",
            success: function (data) {
                if (!data || data.length !== 1) {
                    swal("Erro", "CEP inválido ou não encontrado.");
                    return false;
                }
                //vamos limpar os campos de entrega
                let cepEntrega: CepEntrega = new CepEntrega();
                cepEntrega.limparCamposEndEntrega();

                let end: CepDto = data[0];
                cepEntrega.atribuirDadosParaEntrega(end);

                $('.container_cep').removeClass('carregando');
            },
            error: function (data) {
                swal("Erro", "Falha ao buscar endereço!");
                $('.container_cep').removeClass('carregando');
            }
        })
    }
    else {
        swal("Erro", "É necessário informar um CEP válido!");
    }
}

/*============== Funções =================*/
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

function PessoaFisica(): void {
    $("#divEntregaPF").show();
    $("#divEntregaPJ").hide();
    $("#EndEntregaTipoPF").prop("checked", true);
    if ($("#EndEntregaTipoPJ").is(":checked") == true)
        $("#EndEntregaTipoPJ").prop("checked", false);
    MascaraTelefones($("#endEntregaTelRes"));
    MascaraTelefones($("#endEntregaCel"));

}

function PessoaJuridica(): void {
    $("#divEntregaPJ").show();
    $("#divEntregaPF").hide();
    $("#EndEntregaTipoPJ").prop("checked", true);
    if ($("#EndEntregaTipoPF").is(":checked") == true)
        $("#EndEntregaTipoPF").prop("checked", false);

    MascaraTelefones($("#endEntregaTelCom1"));
    MascaraTelefones($("#endEntregaTelCom2"));
}

declare function swal(header, body);

