
class SiteColorsIndex {
    constructor() {
        $(document).ready(() => {

            //carregamos a página e tiramos o estilo de carregamento quando terminar
            this.iframeJquery = $(this.seletorIframe);
            this.iframe = this.iframeJquery[0];
            this.iframeJquery.parent().addClass("carregando");
            this.iframe.contentDocument.getElementsByTagName('body')[0].style.cursor = "wait";

            this.iframe.onload = () => {

                this.VerificarPaginaASP();

                this.iframe.contentDocument.getElementsByTagName('html')[0].style.overflowX = "hidden";
                //estamos tratando os link's que direcionam para resumo.asp
                this.AlterarHrefPagina();

                this.removerElementosTela();
                this.iframeJquery.parent().removeClass("carregando");

                //para quando navegar dentro do frame, clicando em um link dirtamente dentro do iframe
                $(this.iframe.contentWindow).on("beforeunload", () => {
                    this.iframeJquery.parent().addClass("carregando");
                });
            };

            if (document.URL)
                this.iframe.src = this.urlIframe;
        });
    }

    private VerificarPaginaASP() {
        /* não podemos ir para a tela principal do asp.
         * mesmo verificando todos os links das páginas asp vamos manter este bloco.
         * sabendo que o asp pode redirecionar de forma automática para página principal do asp,
         * criamos esse bloco para caso ocorra o redirecionamento para resumo.asp, iremos redirecionar 
         * para a Home do mvc
         */
        if (this.iframe.contentDocument.location.href.toLowerCase().indexOf("resumo.asp") != -1) {
            document.location.href = this.urlBase;
        }

        //Essa tela não esta tratando a caixa com msg no topo
        if (this.iframe.contentDocument.location.href.toLowerCase().indexOf("relvendasporboletoexec.asp") != -1) {
            this.iframe.contentDocument.styleSheets[1].removeRule(2)
        }

        //vamos acertar o layout de algumas página que não estão sendo tratadas
        if (this.iframe.contentDocument.location.href.toLowerCase().indexOf("senha.asp") != -1 ||
            this.iframe.contentDocument.location.href.toLowerCase().indexOf("menufuncoesadministrativas.asp") != -1 ||
            this.iframe.contentDocument.location.href.toLowerCase().indexOf("menuorcamentistaeindicador.asp") != -1) {
            //vamos tratar
            $(this.iframe.contentDocument).children().find(".PEDIDO").closest("table").hide();
        }
        //vamos mudar o direcionamento dos links 
        if (this.iframe.contentDocument.location.href.toLowerCase().indexOf("pedido.asp") != -1 ||
            this.iframe.contentDocument.location.href.toLowerCase().indexOf("orcamento.asp") != -1) {
            this.AlterarHrefCliente();
        }

        if (this.iframe.contentDocument.location.href.toLowerCase().indexOf("ajaxceppesqpopup.asp") != -1) {
            if (this.iframe.contentDocument.getElementById("bFechar").textContent == 'Fechar')
                this.iframe.contentDocument.getElementById("bFechar").textContent = 'Voltar';
            this.iframe.contentDocument.getElementById("bFechar").setAttribute('onclick', 'javascript:history.back();');
        }
    }

    private AlterarHrefPagina() {
        let lstLink: JQuery<HTMLAnchorElement>;
        lstLink = $(this.iframe.contentDocument).find("a");
        for (let i = 0; i < lstLink.length; i++) {
            let link = lstLink[i];

            if (link.href.toLowerCase().indexOf("resumo.asp") != -1) {
                lstLink[i].setAttribute("target", "_parent");
                lstLink[i].setAttribute("href", this.urlBase);
            }
        }
    }

    private AlterarHrefCliente() {

        let urlCliente: string = this.urlBase + "/Cliente/ValidarCliente?cpf_cnpj=";
        let lstLink: JQuery<HTMLAnchorElement>;
        lstLink = $(this.iframe.contentDocument).find("[href='javascript:fCLIEdita();']") as JQuery<HTMLAnchorElement>;
        let urlBase = this.urlBase;
        if (lstLink != undefined) {
            let cpf_cnpj: string = lstLink[0].children[0].textContent.trim();
            for (let i = 0; i < lstLink.length; i++) {
                lstLink[i].href = "#!";
                $(lstLink[i]).on("click", function () {
                    SiteColorsIndex.ValidarCliente(cpf_cnpj, urlBase);
                });
            }
        }
    }

    public static ValidarCliente(cpf_cnpj: string, urlBase: string): void {
        $.ajax({
            url: urlBase + "/Cliente/ValidarCliente",
            type: "GET",
            data: { cpf_cnpj: cpf_cnpj },
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data != undefined) {
                    document.location.href = urlBase + "/Cliente/BuscarCliente?cpf_cnpj=" + cpf_cnpj + "&&novoCliente=" + data;
                }
            },
            error: function (data) {
                swal("Erro", "Falha ai validar cliente!");
            }
        });
    }

    private removerElementosTela() {
        this.removerLinhaCopyright();
        $(this.iframe.contentDocument).find(".LSessaoEncerra").hide();
        $(this.iframe.contentDocument).find(".LPagInicial").attr('target', "_parent");

        //Orcamento.asp
        $(this.iframe.contentDocument).find("#divConsultaOrcamentoWrapper").hide();
        $(this.iframe.contentDocument).find("#divConsultaPedidoWrapper").hide();
    }

    private removerLinhaCopyright() {
        let css = this.iframe.contentDocument.styleSheets[0];
        if (css != undefined)
            for (let i: number = 0; i < css.cssRules.length; i++) {
                let estaRegra = (css.cssRules[i] as any);
                if (estaRegra.selectorText && estaRegra.selectorText.toString().toLowerCase() == "body::before") {
                    css.removeRule(i);
                    break;
                }
            }
    }


    public urlIframe: string;
    public seletorIframe: string;
    //este somente fica válido depois que o documento termina de carregar
    public iframe: HTMLIFrameElement;
    public iframeJquery: JQuery<HTMLIFrameElement>;
    public urlBase: string;
}