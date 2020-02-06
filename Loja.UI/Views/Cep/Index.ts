

$("#btnModificar").click(function () {
    $('.teste').children().find('input').filter(()  => {
        if ($(this).prop('checked') == true) {
            var elem = $(this).parent().parent().parent();
            inscreve(elem[0].children);
        }

        return true;
    });
});

function montaTabela(data:any) {
    var cols = "";
    var lst = data["ListaCep"];

    if (lst.length > 0) {
        if ($('#msg').css("display", "block")) $('#msg').css("display", "none");
        $('.tabela_endereco').css("display", "block");
        for (var i = 0; i < lst.length; i++) {
            cols += "<tr id='linha' class='teste'>";
            cols += "<td>";
            cols += "<label><input class='with-gap check' type='radio' value='" + i + "'></input><span></span></label>";
            cols += "</td>";
            cols += "<td>" + lst[i].Cep + "</td>";
            cols += "<td>" + lst[i].Uf + "</td>";
            cols += "<td>" + lst[i].Cidade + "</td>";
            cols += "<td>" + lst[i].Bairro + "</td>";
            cols += "<td>" + lst[i].Endereco + "</td>";
            cols += "<td>" + lst[i].LogradouroComplemento + "</td></tr>";
            $("#tableBody").empty().append(cols);
        }

        $(".teste").click(function () {

            $(this).find('td').each(function (i) {
                if ($(this).find('label')) {
                    $(this).find('label').each(function (s) {
                        if ($(this).find('input')) {
                            $(this).find('input').each(function (p) {
                                debugger;
                                var cbs = document.getElementsByClassName("check");
                                //cbs = $(this)
                                for (var i = 0; i < cbs.length; i++) {
                                    //if (cbs[i] !== $(this)) cbs[i].checked = false;
                                }
                                $(this).prop('checked', true);
                            })
                        }
                    })
                }
            });
        });
    }
    else {
        if ($('.tabela_endereco').css("display", "block")) $('.tabela_endereco').css("display", "none");
        var msg = "<span> Endereço não encontrado!</span>";
        $("#msg").css("display", "block");
        $("#msg").empty().append(msg);
    }


}

function limparCamposEndEntrega() {
    $('#cepEntrega').val('');
    $('#ufEntrega').val('');
    $('#cidadeEntrega').val('');
    $('#bairroEntrega').val('');
    $('#enderecoEntrega').val('');
    $('#compEntrega').val('');
    $('#numEntrega').val('');
}

function inscreve(o:any) {
    //fazer a verificação para saber se tem valor no con[i]
    //para ativar os campos e bloquear a edição de alguns campos caso não seja vazio

    limparCamposEndEntrega();

    debugger;
    var con = $(o);
    $('#cepEntrega').val(con[1].textContent);
    $("#cep").addClass('valid');
    $("#lblcep").addClass('active');


    $('#ufEntrega').val(con[2].textContent);
    $('#ufEntrega').prop('readonly', true);
    $("#lblUfEntrega").addClass('active');

    $('#cidadeEntrega').val(con[3].textContent);
    $('#cidadeEntrega').prop('readonly', true);
    $("#lblCidadeEntrega").addClass('active');

    if (con[4].textContent.trim() != "") {
        $('#bairroEntrega').prop("readonly", true);
        $('#bairroEntrega').val(con[4].textContent);
        $("#lblBairroEntrega").addClass('active');
    }
    else {
        $('#bairroEntrega').prop("readonly", false);
    }

    if (con[5].textContent.trim() != "") {
        $('#enderecoEntrega').prop("readonly", true);
        $('#enderecoEntrega').val(con[5].textContent);
        $("#lblEnderecoEntrega").addClass('active');
    }
    else {
        $('#enderecoEntrega').prop("readonly", false);
    }

    if (con[6].textContent.trim() != "") {
        $('#compEntrega').val(con[6].textContent);
        $("#lblComplementoEntrega").addClass('active');
    }
}