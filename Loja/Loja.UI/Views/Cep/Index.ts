import { CepDto } from "../../DtosTs/CepDto/CepDto";
import { Constantes } from "../../UtilTs/Constantes/Constantes";
import { EnderecoEntregaClienteCadastroDto } from "../../DtosTs/ClienteDto/EnderecoEntregaClienteCadastroDto";

export class CepEntrega {
    constructor() {
        $(document).ready(() => {
            this.tipoClienteEntrega = Constantes.ID_PF;
            $("#EndEntregaTipoPF").prop("checked", true);
            $("#divEntregaPF").show();
            // cliente PJ em entrega PF
            ($("#endEntregaTelRes") as any).mask("(99) 9999-9999");
            ($("#endEntregaCel") as any).mask("(99) 99999-9999");

            //cliente PJ em entrega PJ
            ($("#endEntregaTelCom1") as any).mask("(99) 9999-9999");
            ($("#endEntregaTelCom2") as any).mask("(99) 9999-9999");


            $("#rb_endEntrega_pf").click(() => {
                this.tipoClienteEntrega = Constantes.ID_PF;
                $("#divEntregaPF").show();
                $("#divEntregaPJ").hide();
                $("#EndEntregaTipoPF").prop("checked", true);
                if ($("#EndEntregaTipoPJ").is(":checked") == true)
                    $("#EndEntregaTipoPJ").prop("checked", false);

            });
            $("#rb_endEntrega_pj").click(() => {
                this.tipoClienteEntrega = Constantes.ID_PJ;
                $("#divEntregaPJ").show();
                $("#divEntregaPF").hide();
                $("#EndEntregaTipoPJ").prop("checked", true);
                if ($("#EndEntregaTipoPF").is(":checked") == true)
                    $("#EndEntregaTipoPF").prop("checked", false);
            });
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

    public atribuirDadosParaEntrega(end:CepDto):void {
        $("#cepEntrega").val(end.Cep);
        ($("#cepEntrega") as any).mask("99999-999");
        $("#lblcep").addClass('active');

        if (!!end.Bairro) {
            $("#bairroEntrega").val(end.Bairro);
            $("#lblBairroEntrega").addClass('active');
        }
        if (!!end.Cidade) {
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
            $("#endereco").val(end.Endereco);
            $("#lbEntrega").addClass('active');
        }
        if (!!end.Uf) {
            $("#ufEntrega").val(end.Uf);
            $("#lblUfEntrega").addClass('active');
        }

        $("#numEntrega").val('');
        $("#compEntrega").val('');
    }

    public converterEntregaParaEnderecoEntregaClienteCadastroDto(): EnderecoEntregaClienteCadastroDto{
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


//endereço de entrega para cliente PJ setado em PF


//Controla os campos de endereço de entrega para PJ

declare var window: Window & typeof globalThis;
//declare var lstIBGE_Entrega1: Array<string>;
//lstIBGE_Entrega1 = new Array<string>();
//declare var tipoClienteEntrega: string;

(window as any).buscarCep = (el: JQuery<HTMLInputElement>) => {
    
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