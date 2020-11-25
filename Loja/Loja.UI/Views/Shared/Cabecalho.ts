import { Constantes } from "../../UtilTs/Constantes/Constantes";
//OBS: não entendi o pq que temos que importar algo para remover o erro no window 

declare var usuarioLogado: any;
declare var window: any;

$(function () {
    if (usuarioLogado != "" && usuarioLogado != undefined) {
        let letra = usuarioLogado.substring(0, 2);
        let split = usuarioLogado.split(' ');
        if (split.length > 1) {
            letra = usuarioLogado.substring(0, 1) + split[split.length - 1].substring(0, 1);
        }
        setRandomColor();
        $("#avatar").text(letra.toUpperCase());
        $("#menu-usuario").attr("data-original-title", usuarioLogado);
    }
});

function setRandomColor() {
    $("#avatar").css("background-color", getRandomColor());
}
function getRandomColor() {
    let letters = '0123456789ABCDEF';
    let color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

declare var document: HTMLDocument;
window.BuscarTodosAvisos = () => {
    debugger;
    document.location.href = "/lojamvc/Home/Index/?novaloja=" + $('#cabecacomboLojas').val();

}