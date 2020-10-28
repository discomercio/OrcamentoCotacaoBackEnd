
export class Loading {
    public static Carregando(carregando:boolean):void {
        if (carregando) {
            $("form").addClass("carregando");
        }
        if (!carregando) {
            $("form").removeClass("carregando");
        }
    }
}