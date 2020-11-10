

$(window).resize(function () {
    //se redimensionar a tela 
    if (window.innerWidth < 768) {
        AlterarClasse();
    }
});

$(function () {
    //se a tela estive nessa resolução ao atualizar 
    if (window.innerWidth < 768) {
        AlterarClasse();
    }

});

function AlterarClasse() {
    $(".meu-header").removeClass("col-sm-6");
    $(".meu-header").removeClass("col-sm-12");
    $("#comboLojas").removeClass("col-sm-8");
    $("#comboLojas").addClass("col-sm-12");
}