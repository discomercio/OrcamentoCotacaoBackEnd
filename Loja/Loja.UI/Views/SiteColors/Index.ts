
class SiteColorsIndex {
    constructor() {
        $(document).ready(() => {

            window.addEventListener('message', event => {
                console.log(event);
                console.log(event.data);
            });


            //carregamos a página e tiramos o estilo de carregamento quando terminar
            this.iframeJquery = $(this.seletorIframe);
            this.iframe = this.iframeJquery[0];
            this.iframeJquery.parent().addClass("carregando");
            this.iframe.src = this.urlIframe;
            this.iframe.onload = () => {
                this.removerElementosTela();
                this.iframeJquery.parent().removeClass("carregando");

                //para quando navegar dentro do frame, clicando em um link dirtamente dentro do iframe
                $(this.iframe.contentWindow).on("beforeunload", () => {
                    this.iframeJquery.parent().addClass("carregando");
                });
            };
        });
    }

    private removerElementosTela()
    {
        this.removerLinhaCopyright();
        $(this.iframe.contentDocument).find(".LSessaoEncerra").hide();
        $(this.iframe.contentDocument).find(".LPagInicial").hide();
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