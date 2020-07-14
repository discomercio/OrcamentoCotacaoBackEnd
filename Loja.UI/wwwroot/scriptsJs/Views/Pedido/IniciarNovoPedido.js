/// <reference path="../../../node_modules/@types/jquery/JQuery.d.ts" />
/// <reference path="../../UtilTs/MoedaUtils/moedaUtils.ts" />
$('#enumPagto').change(function () {
    var div;
    var disp;
    if ($('#enumPagto').val() == "1") {
        div = document.getElementById("Avista");
        disp = div.style.display;
        if (disp == 'none') {
            div.style.display = 'block';
        }
    }
    else {
        div = document.getElementById("Avista");
        if (div != null) {
            disp = div.style.display;
            div.style.display = 'none';
        }
    }
    if ($('#enumPagto').val() == "2") {
        if (disp == 'none') {
            div.style.display = 'block';
        }
    }
    else {
        div = document.getElementById("ParcUnica");
        if (div != null) {
            disp = div.style.display;
            div.style.display = 'none';
        }
    }
    if ($('#enumPagto').val() == "3") {
        div = document.getElementById("ParcComEntrada");
        disp = div.style.display;
        if (disp == 'none') {
            div.style.display = 'block';
        }
    }
    else {
        div = document.getElementById("ParcComEntrada");
        if (div != null) {
            disp = div.style.display;
            div.style.display = 'none';
        }
    }
    if ($('#enumPagto').val() == "5") {
        div = document.getElementById("PagtoCartaoInternet");
        disp = div.style.display;
        if (disp == 'none') {
            div.style.display = 'block';
        }
    }
    else {
        div = document.getElementById("PagtoCartaoInternet");
        if (div != null) {
            disp = div.style.display;
            div.style.display = 'none';
        }
    }
    if ($('#enumPagto').val() == "6") {
        div = document.getElementById("PagtoCartaoMaquineta");
        disp = div.style.display;
        if (disp == 'none') {
            div.style.display = 'block';
        }
    }
    else {
        div = document.getElementById("PagtoCartaoMaquineta");
        if (div != null) {
            disp = div.style.display;
            div.style.display = 'none';
        }
    }
});
$('#btnModificar').click(function () {
    $('.prod').children().find('input').filter(function () {
        if ($(this).prop('checked') == true) {
            var elem = $(this).parent().parent().parent();
            debugger;
            return true;
        }
        return false;
    });
});
//comenntario
$(".prod").click(function () {
    $(".prod input").prop('checked', false);
    $(this).find("input").prop('checked', true);
});
//let formata = new MoedaUtils();
//alert(formata.formatarMoedaComPrefixo(123));
//# sourceMappingURL=/scriptsJs/Views/Pedido/IniciarNovoPedido.js.map