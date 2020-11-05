
declare var usuarioLogado: any;

$(function () {
    if (usuarioLogado != "" && usuarioLogado != undefined) {
        let split = usuarioLogado.split(' ');
        if (split.length > 1) {
            debugger;
        }
        else {
            let letra = usuarioLogado.substring(0, 2);
            setRandomColor();
            $("#avatar").text(letra.toUpperCase());
            $("#menu-usuario").attr("data-original-title", usuarioLogado);
        }

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