
class SiteColorsIndex {
    constructor() {
        $(document).ready(() => {


            //carregamos a página e tiramos o estilo de carregamento quando terminar
            this.iframeJquery = $(this.seletorIframe);
            this.iframe = this.iframeJquery[0];
            this.iframeJquery.parent().addClass("carregando");
            this.iframe.onload = () => {
                this.removerElementosTela();
                this.iframeJquery.parent().removeClass("carregando");

                //para quando navegar dentro do frame, clicando em um link dirtamente dentro do iframe
                $(this.iframe.contentWindow).on("beforeunload", () => {
                    this.iframeJquery.parent().addClass("carregando");
                });
            };
            this.iframe.src = this.urlIframe;
        });
    }

    private removerElementosTela() {
        this.removerLinhaCopyright();
        $(this.iframe.contentDocument).find(".LSessaoEncerra").hide();
        $(this.iframe.contentDocument).find(".LPagInicial").hide();

        //Orcamento.asp
        $(this.iframe.contentDocument).find("#divConsultaOrcamentoWrapper").hide();
        $(this.iframe.contentDocument).find("#divConsultaPedidoWrapper").hide();

        //bota voltar, só na pagina que foi carrega pelo MVC
        if (this.iframe.contentDocument.location.href.indexOf(this.urlIframe) >= 0) {
            $(this.iframe.contentDocument).find('#bVOLTAR').hide();
            $(this.iframe.contentDocument).find('#bVOLTA').hide();
        }
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
}