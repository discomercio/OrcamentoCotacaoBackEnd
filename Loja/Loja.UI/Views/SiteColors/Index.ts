
class SiteColorsIndex {
    constructor() {

        /*
         * fazer uma função para trocar o href que contém resumo.asp para home mvc ou history.back()
         * adicionar e incluir :
         *      href => passar a url e incluir target="_top"
         *      window.parent.location.href= "http://www.google.com";
         */

        $(document).ready(() => {

            //carregamos a página e tiramos o estilo de carregamento quando terminar
            this.iframeJquery = $(this.seletorIframe);
            this.iframe = this.iframeJquery[0];
            this.iframeJquery.parent().addClass("carregando");

            this.iframe.onload = () => {
                let pagina = this.iframe.contentDocument.URL.replace("http://localhost:9010", "");
                if (this.listaPaginas.indexOf(pagina) != -1) {
                    //vamos mapear os hrefs

                    this.AlterarHrefBtnVoltar();
                }
                else {
                    //vamos verificar a página para saber se tem algum link para resumo.asp
                    this.AlterarHrefPagina();
                }

                if (this.iframeJquery[0].src.indexOf("resumo.asp") != -1) {
                    alert("estamos na resumo.asp");
                }
                //não podemos ir para a tela principal do asp
                if (this.iframe.contentDocument.location.href.indexOf("resumo.asp") != -1) {
                    document.location.href = "/lojamvc";
                }

                //this.removerElementosTela();
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

    private AlterarHrefPagina() {
        let lstLink: JQuery<HTMLAnchorElement>;
        let http: string = "/lojamvc";
        lstLink = $(this.iframe.contentDocument).find("a");
        for (let i = 0; i < lstLink.length; i++) {
            let link = lstLink[i];

            if (link.href.indexOf("resumo.asp") != -1) {
                lstLink[i].setAttribute("target", "_parent");
                lstLink[i].setAttribute("href", http);
            }
        }
    }

    private AlterarHrefBtnVoltar() {
        let http: string = "/lojamvc";

        let href = $(this.iframe.contentDocument).find("#bVOLTAR").attr("href");
        if (href.indexOf("resumo.asp") != -1) {
            debugger;
            $(this.iframe.contentDocument).find("#bVOLTAR").attr('target', "_parent");
            $(this.iframe.contentDocument).find("#bVOLTAR").attr('href', http);
        }
    }

    private removerElementosTela() {
        this.removerLinhaCopyright();
        $(this.iframe.contentDocument).find(".LSessaoEncerra").hide();
        $(this.iframe.contentDocument).find(".LPagInicial").hide();

        //Orcamento.asp
        $(this.iframe.contentDocument).find("#divConsultaOrcamentoWrapper").hide();
        $(this.iframe.contentDocument).find("#divConsultaPedidoWrapper").hide();


        //bota voltar, só na pagina que foi carrega pelo MVC
        //if (this.iframe.contentDocument.location.href.indexOf(this.urlIframe) >= 0) {
        //    $(this.iframe.contentDocument).find('#bVOLTAR').hide();
        //    $(this.iframe.contentDocument).find('#bVOLTA').hide();
        //}
    }
    private removerLinhaCopyright() {
        let css = this.iframe.contentDocument.styleSheets[0];
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
    public listaPaginas: string[] = new Array();
}