
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
            debugger;
            this.iframe.contentDocument.getElementsByTagName('body')[0].style.cursor = "wait";

            this.iframe.onload = () => {

                //estamos tratando os link's que direcionam para resumo.asp
                this.AlterarHrefPagina();
                
                /* não podemos ir para a tela principal do asp.
                 * mesmo verificando todos os links das páginas asp vamos manter este bloco.
                 * sabendo que o asp pode redirecionar de forma automática para página principal do asp,
                 * criamos esse bloco para caso ocorra o redirecionamento para resumo.asp, iremos redirecionar 
                 * para a Home do mvc
                 */
                if (this.iframe.contentDocument.location.href.indexOf("resumo.asp") != -1) {
                    document.location.href = this.http;
                }

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

    private AlterarHrefPagina() {
        let lstLink: JQuery<HTMLAnchorElement>;
        lstLink = $(this.iframe.contentDocument).find("a");
        for (let i = 0; i < lstLink.length; i++) {
            let link = lstLink[i];

            if (link.href.indexOf("resumo.asp") != -1) {
                lstLink[i].setAttribute("target", "_parent");
                lstLink[i].setAttribute("href", this.http);
            }
        }
    }

    private AlterarHrefBtnVoltar() {

        let href = $(this.iframe.contentDocument).find("#bVOLTAR").attr("href");
        if (href.indexOf("resumo.asp") != -1) {
            $(this.iframe.contentDocument).find("#bVOLTAR").attr('target', "_parent");
            $(this.iframe.contentDocument).find("#bVOLTAR").attr('href', this.http);
        }
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
    public http: string = "/homologacaomvc/lojamvc";
    public urlBase: string = "http://localhost:9010";

}