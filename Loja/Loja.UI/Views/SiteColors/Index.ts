
class SiteColorsIndex {
    constructor() {
        $(document).ready(() => {

            window.addEventListener('message', event => {
                console.log(event);
                console.log(event.data);
            });


            //carregamos a página e tiramos o estilo de carregamento quando terminar
            this.iframe = $(this.seletorIframe)[0] as HTMLIFrameElement;
            $("body").addClass("carregando");
            this.iframe.src = this.urlIframe;
            this.iframe.onload = () => {
                $("body").removeClass("carregando");

                //para quando navegar dentro do frame, clicando em um link dirtamente dentro do iframe
                $(this.iframe.contentWindow).on("beforeunload", () =>{
                    $("body").addClass("carregando");
                });
            };
        });
    }
    public urlIframe: string;
    public seletorIframe: string;
    //este somente fica válido depois que o documento termina de carregar
    public iframe: HTMLIFrameElement;
}