
class Login {
    constructor() {

        $(document).ready(() => {
            this.form = $(this.selector);

            $("#loja").prop('autofocus', false);

            if ($("#loja").is(":focus"))
                $("#loja").addClass("fill");
            $("#loja").focusin(function () {
                $("#loja").addClass("fill");
            });

            $("#loja").addClass("fill");
            $("#senha").addClass("fill");
            $("#usuario").addClass("fill");

            this.form.on("submit", () => {

                $(this.selector).addClass("carregando");
                if (!this.ValidarForm()) {
                    swal("Dados inválidos", "Os dados informados estão inválidos!");
                    $(this.selector).removeClass("carregando");
                    return false;
                }

                $(this.selector).removeClass("carregando");
            });
        });
    }

    private ValidarForm(): boolean {
        let loja = $("#loja").val() as string;
        let usuario = $("#usuario").val() as string;
        let senha = $("#senha").val() as string;

        let validou: boolean = false;

        if (loja != undefined && loja != "" &&
            usuario != undefined && usuario != "" &&
            senha != undefined && senha != "") {
            validou = true;
        }

        return validou;
    }

    public loja: HTMLInputElement;
    public usuario: HTMLInputElement;
    public senha: HTMLInputElement;
    public form: JQuery<HTMLFormElement>;
    public selector: string;
}

//declare var window: any;
declare function swal(header, body);



