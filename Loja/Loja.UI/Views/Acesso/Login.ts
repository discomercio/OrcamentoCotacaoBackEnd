
$(function () {
    $("#loja").prop('autofocus', false);

    if ($("#loja").is(":focus"))
        $("#loja").addClass("fill");
    $("#loja").focusin(function () {
        $("#loja").addClass("fill");
    })
    if ($("#loja").val() != '')
        $("#loja").addClass("fill");

    if ($("#usuario").val() != '')
        $("#usuario").addClass("fill");
});