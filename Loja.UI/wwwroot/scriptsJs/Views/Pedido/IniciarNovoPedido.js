define(["require", "exports", "../../DtosTs/DtoProdutos/SelectProdInfo", "../../FuncoesTs/Itens/Itens", "../../DtosTs/DtoPedido/DtoPedido", "../../UtilTs/MoedaUtils/moedaUtils", "../../Services/NovoPepedidoDadosService", "../../FuncoesTs/DadosPagto/DadosPagto", "../../UtilTs/Constantes/Constantes", "../Shared/Error", "../../DtosTs/DtoPedido/PercentualMaxDescEComissao"], function (require, exports, SelectProdInfo_1, Itens_1, DtoPedido_1, moedaUtils_1, NovoPepedidoDadosService_1, DadosPagto_1, Constantes_1, Error_1, PercentualMaxDescEComissao_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    itens = new Itens_1.Itens();
    moedaUtils = new moedaUtils_1.MoedaUtils();
    dadosPagto = new DadosPagto_1.DadosPagto();
    function inicializaCampos(v) {
        var div;
        var disp;
        if (v.toString() == "1") {
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
        if (v.toString() == "5") {
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
        if (v.toString() == "3") {
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
        if (v.toString() == "2") {
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
        if (v.toString() == "6") {
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
    }
    $(".prod").click(function () {
        $(".prod input").prop('checked', false);
        $(this).find("input").prop('checked', true);
    });
    $("#divCOM").children().find("input").prop('disabled', true);
    $("#chkSemRa").prop("checked", true);
    $("#chkComRa").click(function () {
        $("#chkSemRa").prop("checked", false);
        $("#divCOM").children().find("input").prop('disabled', false);
    });
    $("#chkSemRa").click(function () {
        $("#chkComRa").prop("checked", false);
        $("#divCOM").children().find("input").prop('disabled', true);
    });
    $("#divIndicadores").children().find("input").prop('disabled', true);
    $("#chkSemIndicacao").prop("checked", true);
    $("#chkSemIndicacao").click(function () {
        $("#chkComIndicacao").prop("checked", false);
        $("#divIndicadores").children().find("input").prop('disabled', true);
    });
    $("#chkComIndicacao").click(function () {
        $("#chkSemIndicacao").prop("checked", false);
        $("#divIndicadores").children().find("input").prop('disabled', false);
    });
    $("#chkAutomatico").prop("checked", true);
    $("#divSelecaoCd").children().find("input").prop('disabled', true);
    $("#chkAutomatico").click(function () {
        $("#chkManual").prop("checked", false);
        $("#divSelecaoCd").children().find("input").prop('disabled', true);
    });
    $("#chkManual").click(function () {
        $("#chkAutomatico").prop("checked", false);
        $("#divSelecaoCd").children().find("input").prop('disabled', false);
    });
    //limpando campos ao fechar a modal
    $("#buscaproduto").keyup(function () {
        var buscaProduto = $("#buscaproduto").val().toString();
        $(".tabelaProd tr").hide();
        if (buscaProduto != "") {
            $(".prod").filter(function () {
                if ($(this).text().toLowerCase().indexOf(buscaProduto) >= 0) {
                    $(this).show();
                    if ($(this).next().hasClass("trfilho")) {
                        $(this).next().show();
                        $(this).next().next().show();
                    }
                }
                return true;
            });
        }
        else {
            $(".tabelaProd tr").show();
        }
    });
    $("#percComissao").keyup(function () {
        var valor = $("#percComissao").val().toString();
        var val = valor.replace(/\D/g, '');
        val = (val / 100).toFixed(2) + '';
        $("#percComissao").val(moedaUtils.formatarMoedaSemPrefixo(val));
    });
    window.VerificarPercMaxDescEComissao = function (e) {
        debugger;
        var valor = e.value;
        var val = valor.replace(/\D/g, '');
        val = (val / 100).toFixed(2) + '';
        e.value = moedaUtils.formatarMoedaSemPrefixo(val);
        var msgErro = "";
        var percMax = new PercentualMaxDescEComissao_1.PercentualMaxDescEComissao();
        percMax = percMaxDescEComissaoPorLoja;
        if (val < 0 || val > 100) {
            msgErro = "Percentual de comissão inválido.";
        }
        if (val > percMaxDescEComissaoPorLoja.PercMaxComissao) {
            msgErro = "O percentual de comissão excede o máximo permitido.";
        }
        if (msgErro != "") {
            //chama a modal caso seja maior ou tiver erros
            modalError();
            var err = new Error_1.ErrorModal();
            err.MostrarMsg(msgErro);
            return false;
        }
    };
    //como é chamadado diretamente do HTML, tem que estar na window
    window.InserirProdutoLinha = function () {
        var selectProdInfo = new SelectProdInfo_1.SelectProdInfo();
        //estou pegando a linha
        if ($("#qtde").val() != "" && $("#qtde").val() != "undefined") {
            $('.prod').children().find('input').filter(function () {
                if ($(this).prop('checked') == true) {
                    var elem = $(this).parent().parent().parent();
                    elem.children().each(function () {
                        selectProdInfo.Fabricante = elem.children()[0].textContent.trim();
                        selectProdInfo.Produto = elem.children()[1].textContent.trim();
                        selectProdInfo.Qte = parseInt($("#qtde").val().toString());
                        selectProdInfo.ClicouOk = true;
                    });
                    return true;
                }
            });
            itens.dtoProdutoCombo = lstprodutos; //pegando da tela
            itens.selectProdInfo = selectProdInfo;
            itens.dtoPedido = new DtoPedido_1.DtoPedido();
            itens.mostrarProdutos(null);
            //essa lista é a que esta sendo utilizada para pegar valores da tela 
            //estamos arrumando a qtde, verificando se existe e inserindo o produto
            arrumarProdsRepetidosTeste();
            //remover todas as linhas da tabela para adicionar novamente.
            removerTodosProdutos();
            RecalcularValoresSemCoeficiente(null);
            PedidoAlterado();
            //limpar tirar o check do produto selecionado
            $('.prod').children().find('input').filter(function () {
                if ($(this).prop('checked') == true)
                    $(this).prop('checked', false);
                return true;
            });
            console.log("InserirProdutoLinha itens.msgErro: " + itens.msgErro);
            if (itens.msgErro != "" && itens.msgErro != undefined) {
                modalError();
                var err = new Error_1.ErrorModal();
                err.MostrarMsg(itens.msgErro);
            }
        }
        else {
            return false;
        }
    };
    window.PreparaListaProdutosParaDataList = function () {
        var lstProdutosPreparadoParaDataList = new Array();
        var t = {};
        console.log("preparando a lista");
        console.log(lstprodutos);
        lstprodutos.ProdutoDto.forEach(function (v) {
            lstProdutosPreparadoParaDataList.push(v.Produto);
        });
        return lstProdutosPreparadoParaDataList;
    };
    function arrumarProdsRepetidosTeste() {
        var p = lstProdSelecionados;
        var exist = false;
        //é o que esta da vindo selecionado
        var lstPedidoProdutos = itens.dtoPedido.ListaProdutos;
        if (p.length > 0) {
            p.forEach(function (value) {
                lstPedidoProdutos.forEach(function (prod) {
                    if (value.Fabricante == prod.Fabricante && value.NumProduto == prod.NumProduto) {
                        value.Qtde++;
                        itens.digitouQte(value);
                        //teste
                        prod.Qtde++;
                        itens.digitouQte(prod);
                        //dadosPagto inicializar
                        PedidoAlterado();
                        exist = true;
                    }
                });
            });
            lstProdSelecionados = p;
        }
        if (!exist) {
            lstPedidoProdutos.forEach(function (prod) {
                prod.Desconto = prod.Desconto ? prod.Desconto : 0;
                lstProdSelecionados.push(prod);
            });
        }
        else {
            //convertemos para verificar se os produtos selecionado existe
            var t_1 = JSON.stringify(lstProdSelecionados);
            lstPedidoProdutos.forEach(function (f) {
                var r = t_1.indexOf(f.NumProduto);
                if (r < 0) {
                    f.Desconto = f.Desconto ? f.Desconto : 0;
                    lstProdSelecionados.push(f);
                }
            });
        }
    }
    //calculamos os produtos e somamos o total
    function totalPedido() {
        var total = lstProdSelecionados.reduce(function (sum, current) { return sum + current.TotalItem; }, 0);
        $("#totalPedido").text(moedaUtils.formatarMoedaSemPrefixo(total));
        return;
    }
    function removerTodosProdutos() {
        var tbody = $(".novoProduto").parent();
        var tbodyCount = tbody.children().length;
        console.log(tbodyCount);
        if (tbodyCount >= 4) {
            for (tbodyCount; tbodyCount > 4; tbodyCount--) {
                var trTotal = tbody.children()[tbodyCount - 1].className;
                if (trTotal != "trTotal") {
                    tbody.children()[tbodyCount - 1].remove();
                }
            }
        }
        indice = 0;
    }
    window.removerLinha = function (v) {
        //fazer a perngunta para saber se confirma remover 
        //const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
        //    data: `Remover este item do pré-pedido?`
        //});
        if (true) {
            //v = campo html
            var t = v;
            //pegando o tr
            var linha = t.closest("tr"); // t.parentElement.parentElement.parentElement.parentElement;
            linha.remove();
            //pegando o td
            var codProduto_1 = linha.cells[0].children[0].value;
            //pegando o produto para alterar o valor
            var produto = new Array();
            produto = lstProdSelecionados.filter(function (e) { return e.NumProduto == codProduto_1; });
            var i = lstProdSelecionados.indexOf(produto[0]);
            lstProdSelecionados.splice(i, 1);
            console.log(lstProdSelecionados);
            //recalcular o pedido
            //Gabriel
            PedidoAlterado();
            totalPedido();
        }
        zerarCamposDadosPagto();
    };
    //altera valor total do item digitado
    window.digitouQtde = function (v) {
        //v = campo html
        var t = v;
        //pegando o tr
        var linha = t.parentElement.parentElement.parentElement;
        //pegando o td
        var codProduto = linha.cells[0].children[0].value;
        lstProdSelecionados = dadosPagto.dtoPedido.ListaProdutos;
        //pegando o produto para alterar o valor
        var produto = lstProdSelecionados.filter(function (e) { return e.NumProduto == codProduto; });
        produto[0].Qtde = parseInt(v.value);
        itens.digitouQte(produto[0]);
        //dadosPagto inicializar
        PedidoAlterado();
        itens.dtoPedido.ListaProdutos = lstProdSelecionados;
        RecalcularValoresSemCoeficiente(null);
        ////passando para o valor para tela
        linha.children[4].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(produto[0].VlUnitario);
        linha.children[5].textContent = moedaUtils.formatarMoedaSemPrefixo(produto[0].TotalItem);
        PedidoAlterado(); //chamamos aqui para inicializar as variaveis
        //dadosPagto.prepedidoAlterado();
        zerarCamposDadosPagto();
    };
    //Dessa forma eu consigo utilizar a modal de erro
    //essa function esta em Views/Shared/Error.cshtml
    //criamos um Error.ts para inserir a msg na modal de erros
    /*
     * Exemplo de como acessar:
     *      modalError();
     *      let err = new ErroModal();
     *      err.MostrarMsg("passar msg aqui");
     *  fazendo esses passos já estaremos solicitando que mostre a modal com a msg
     */
    window.digitouDesc = function (v) {
        //let t = ((v.target) as HTMLInputElement).value;
        //let val: any = t.replace(/\D/g, '');
        var valor = v.value;
        var val = valor.replace(/\D/g, '');
        val = (val / 10).toFixed(2) + '';
        //passamos o valor formatado para o campo
        v.value = moedaUtils.formatarPorcentagemUmaCasa(val);
        //pegar o valor para buscar na lista
        var linha = v.parentElement.parentElement.parentElement;
        var numProduto = linha.children[0].children[0].value;
        var r = new Array();
        r = lstProdSelecionados.filter(function (e) { return e.NumProduto == numProduto; });
        //r[0].Desconto = val;
        itens.digitouDescValor(r[0], val);
        dadosPagto.dtoPedido.ListaProdutos = lstProdSelecionados;
        //RecalcularValoresSemCoeficiente(null);
        totalPedido();
        linha.children[5].textContent = moedaUtils.formatarMoedaSemPrefixo(r[0].TotalItem);
        linha.children[4].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);
        zerarCamposDadosPagto();
    };
    window.digitouVlVenda = function (v) {
        var valor = v.value;
        var val = valor.replace(/\D/g, '');
        val = (val / 100).toFixed(2) + '';
        v.value = moedaUtils.formatarMoedaSemPrefixo(val);
        var linha = v.parentElement.parentElement.parentElement;
        var numProduto = linha.children[0].children[0].value;
        var r = new Array();
        r = lstProdSelecionados.filter(function (e) { return e.NumProduto == numProduto; });
        r[0].VlUnitario = parseFloat(val);
        //calcula o desconto
        r[0].Desconto = 100 * (r[0].Preco - r[0].VlUnitario) / r[0].Preco;
        r[0].Desconto = parseFloat(r[0].Desconto.toFixed(1));
        itens.digitouQte(r[0]);
        //dadosPagto inicializar
        PedidoAlterado();
        linha.children[3].children[0].children[0].value = moedaUtils.formatarPorcentagemUmaCasa(r[0].Desconto);
        linha.children[4].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);
        linha.children[5].textContent = moedaUtils.formatarMoedaSemPrefixo(r[0].TotalItem);
        zerarCamposDadosPagto();
    };
    //PermiteRA
    window.digitouPreco = function (v) {
        var valor = v.value;
        var val = valor.replace(/\D/g, '');
        val = (val / 100).toFixed(2) + '';
        v.value = moedaUtils.formatarMoedaSemPrefixo(val);
        var linha = v.parentElement.parentElement.parentElement;
        var numProduto = linha.children[0].children[0].value;
        var r = new Array();
        r = lstProdSelecionados.filter(function (e) { return e.NumProduto == numProduto; });
        r[0].Preco = parseFloat(val);
        if (r[0].Desconto) {
            r[0].VlUnitario = r[0].Preco * (1 - r[0].Desconto / 100);
            r[0].VlUnitario = parseFloat(r[0].VlUnitario.toFixed(2));
        }
        else {
            r[0].VlUnitario = r[0].Preco;
        }
        itens.digitouQte(r[0]);
        //dadosPagto inicializar
        PedidoAlterado();
        linha.children[2].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].Preco);
        linha.children[3].children[0].children[0].value = moedaUtils.formatarPorcentagemUmaCasa(r[0].Desconto);
        linha.children[4].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);
        linha.children[5].textContent = moedaUtils.formatarMoedaSemPrefixo(r[0].TotalItem);
        zerarCamposDadosPagto();
    };
    window.digitouVlEntrada = function (v) {
        debugger;
        var valor = v.value;
        var val = valor.replace(/\D/g, '');
        val = (val / 100).toFixed(2) + '';
        v.value = moedaUtils.formatarMoedaSemPrefixo(val);
        debugger;
        dadosPagto.vlEntrada = parseFloat(val);
        dadosPagto.calcularParcelaComEntrada();
        //this.vlEntrada = v;
        //this.calcularParcelaComEntrada();
        //tranformar o valor sem prefixo
        console.log("digitouVlEntrada dadosPagto.msgErro: " + dadosPagto.msgErro);
        if (dadosPagto.msgErro != "" && dadosPagto.msgErro != undefined) {
            modalError();
            var err = new Error_1.ErrorModal();
            err.MostrarMsg(dadosPagto.msgErro);
            v.value = "";
            return false;
        }
        var selecione = $("#opcaoPagtoParcComEntrada").children()[0];
        $("#opcaoPagtoParcComEntrada").html(selecione);
        $("#opcaoPagtoParcComEntrada").formSelect();
        AtribuirListaParaSelect();
    };
    novoPedidoDadosService = new NovoPepedidoDadosService_1.NovoPedidoDadosService();
    $("#totalPedido").ready(function () {
        //itens.dtoPedido.ListaProdutos = lstProdSelecionados;
        novoPedidoDadosService.dtoPedido = new DtoPedido_1.DtoPedido();
        novoPedidoDadosService.dtoPedido.ListaProdutos = new Array();
        novoPedidoDadosService.dtoPedido.ListaProdutos = lstProdSelecionados;
        var totalP = $("#totalPedido").val().toString();
        totalP = novoPedidoDadosService.totalPedido().toString();
        if (totalP != "0") {
            $("#totalPedido").text(totalP);
        }
    });
    function InserirNovoProduto(produto) {
        //esse é um exemplo de como clonar uma div e adicionar 
        //let novo = $(".novoProduto").clone();
        //novo.removeClass("novoProduto")
        //$(".novoProduto").parent().append(novo);
        /*
         * afazer: necessário pegar o index que esta armazenando os valores
         * trocar onde esta escrito "trocarporindex" e colocar o id
         * verificar se o produto tem avisos e qtde permitida e ra_status
         */
        var indice = null;
        indice = parseInt($("#indice").val().toString());
        if (indice.toString() == "NaN") {
            indice = 0;
            $("indice").val(indice);
        }
        var novo = $("<tr></tr>");
        var html = $(".novoProduto").html();
        html = html.replace(/trocarporindex/g, indice.toString());
        novo.html(html);
        novo.removeClass("novoProduto");
        novo.show();
        $(".trTotal").before(novo);
        //$(".novoProduto").parent().append(novo);
        var elem = novo.children();
        InscreveLinhaProduto(produto, indice);
        indice++;
        $("#indice").val(indice);
    }
    //iremos inicializar as variaveis
    function PedidoAlterado() {
        //Forma pagamento
        $("#enumFormaPagto").prop('selectedIndex', 0);
        $("#enumFormaPagto").formSelect();
        totalPedido();
        InicializaDadosPagto();
        dadosPagto.tipoFormaPagto;
        dadosPagto.pedidoAlterado();
        zerarCamposDadosPagto();
        console.log("voltou do dadosPagto pedidoalterado");
        console.log(dadosPagto);
    }
    function zerarCamposDadosPagto() {
        console.log("entrou no");
        var selecione;
        //A vista   
        $("#Avista").css("display", "none");
        selecione = $("#opcaoPagtoAvista").children()[0];
        $("#opcaoPagtoAvista").html(selecione);
        $("#opcaoPagtoAvista").formSelect();
        $("#meioPagtoAVista").prop('selectedIndex', 0);
        $("#meioPagtoAVista").formSelect();
        //ParcCartaoInternet
        $("#PagtoCartaoInternet").css("display", "none");
        selecione = $("#opcaoPagtoParcCartaoInternet").children()[0];
        $("#opcaoPagtoParcCartaoInternet").html(selecione);
        $("#opcaoPagtoParcCartaoInternet").formSelect();
        //ParcComEnt
        $("#ParcComEntrada").css("display", "none");
        $("#vlEntrada").val('');
        $("#meioPagtoEntrada").prop('selectedIndex', 0);
        $("#meioPagtoEntrada").formSelect();
        selecione = $("#opcaoPagtoParcComEntrada").children()[0];
        $("#opcaoPagtoParcComEntrada").html(selecione);
        $("#opcaoPagtoParcComEntrada").formSelect();
        $("#meioPagtoEntradaPrest").prop('selectedIndex', 0);
        $("#meioPagtoEntradaPrest").formSelect();
        $("#diasVenc").val('');
        //ParcUnica
        $("#ParcUnica").css("display", "none");
        selecione = $("#opcaoPagtoParcUnica").children()[0];
        $("#opcaoPagtoParcUnica").html(selecione);
        $("#opcaoPagtoParcUnica").formSelect();
        $("#meioPagtoParcUnica").prop('selectedIndex', 0);
        $("#meioPagtoParcUnica").formSelect();
        $("#diasVencParcUnica").val('');
        //ParcCartaoMaquineta
        $("#PagtoCartaoMaquineta").css("display", "none");
        selecione = $("#opcaoPagtoParcCartaoMaquineta").children()[0];
        $("#opcaoPagtoParcCartaoMaquineta").html(selecione);
        $("#opcaoPagtoParcCartaoMaquineta").formSelect();
    }
    function RecalcularValoresSemCoeficiente(v) {
        zerarCamposDadosPagto();
        InicializaDadosPagto();
        debugger;
        if (v == "" || v == undefined || v == null || v == 0) {
            v = 1;
        }
        if (lstProdSelecionados.length > 0) {
            dadosPagto.enumFormaPagto = v;
            //estamos limpandos os campos
            zerarCamposDadosPagto();
            dadosPagto.qtdeParcVisa = qtdeParcVisa;
            dadosPagto.recalcularValoresComCoeficiente(v);
            console.log("recalcularValoresComCoeficiente dadosPagto.msgErro: " + dadosPagto.msgErro);
            if (dadosPagto.msgErro != "" && dadosPagto.msgErro != undefined) {
                modalError();
                var err = new Error_1.ErrorModal();
                err.MostrarMsg(dadosPagto.msgErro);
                return false;
            }
            console.log(dadosPagto);
            //afazer: mandar a lista para o select correspondente;
            AtribuirListaParaSelect();
            //vamos apagar a linha de produtos selecionados e montar novamente com os valores atualizados
            //remover todas as linhas da tabela para adicionar novamente.
            removerTodosProdutos();
            //passando a lista recalculada
            lstProdSelecionados = dadosPagto.dtoPedido.ListaProdutos;
            //estamos adicionando os produtos na tela
            lstProdSelecionados.forEach(function (value) {
                InserirNovoProduto(value);
                VerificarEstoque(itens, value);
                VerificarProdutoAviso(itens, value);
                VerificarQtdeVendaPermitida(itens, value);
            });
            //alterando o total do pedido
            totalPedido();
        }
    }
    window.recalcularValoresComCoeficiente = function (e) {
        zerarCamposDadosPagto();
        InicializaDadosPagto();
        //let v = e.selectedIndex;
        var v = parseInt(e.selectedOptions[0].value);
        inicializaCampos(v);
        RecalcularValoresSemCoeficiente(v);
        inicializaCampos(v);
    };
    function AtribuirListaParaSelect() {
        if (dadosPagto.enumFormaPagto == 1) {
            //A vista   
            dadosPagto.lstMsg.forEach(function (value) {
                $("#opcaoPagtoAvista").append("<option selected>" + value + "</option>");
            });
            $("#opcaoPagtoAvista").formSelect();
        }
        if (dadosPagto.enumFormaPagto == 2) {
            //ParcCartaoInternet
            dadosPagto.lstMsg.forEach(function (value) {
                $("#opcaoPagtoParcCartaoInternet").append("<option>" + value + "</option>");
            });
            $("#opcaoPagtoParcCartaoInternet").formSelect();
        }
        if (dadosPagto.enumFormaPagto == 3) {
            //ParcComEnt
            dadosPagto.lstMsg.forEach(function (value) {
                $("#opcaoPagtoParcComEntrada").append("<option>" + value + "</option>");
            });
            $("#opcaoPagtoParcComEntrada").formSelect();
        }
        //NÃO ESTA SENDO USADO
        if (dadosPagto.enumFormaPagto == 4) {
            //ParcSemEnt
            dadosPagto.lstMsg.forEach(function (value) {
                $("#opcaoPagtoParcSemEntrada").append("<option>" + value + "</option>");
            });
            $("#opcaoPagtoParcSemEntrada").formSelect();
        }
        if (dadosPagto.enumFormaPagto == 5) {
            //ParcUnica
            dadosPagto.lstMsg.forEach(function (value) {
                $("#opcaoPagtoParcCartaoInternet").append("<option>" + value + "</option>");
            });
            $("#opcaoPagtoParcCartaoInternet").formSelect();
        }
        if (dadosPagto.enumFormaPagto == 6) {
            //ParcCartaoMaquineta
            dadosPagto.lstMsg.forEach(function (value) {
                $("#opcaoPagtoParcCartaoMaquineta").append("<option>" + value + "</option>");
            });
            $("#opcaoPagtoParcCartaoMaquineta").formSelect();
        }
    }
    function InicializaDadosPagto() {
        dadosPagto.coeficienteDto = new Array(); //lista de coeficiente
        dadosPagto.dtoPedido = new DtoPedido_1.DtoPedido(); //Pedido
        dadosPagto.dtoPedido.ListaProdutos = new Array(); //lista de produtos selecionados
        dadosPagto.constantes = new Constantes_1.Constantes(); //será utilizado para comparação
        dadosPagto.moedaUtils = new moedaUtils_1.MoedaUtils();
        dadosPagto.lstMsg = new Array();
        //dadosPagto.tipoFormaPagto = 
        dadosPagto.coeficienteDto = lstCoeficiente; //recebe a lista que veio do servidor
        dadosPagto.dtoPedido.ListaProdutos = lstProdSelecionados; //recebe lst dos produtos selcionados    
        dadosPagto.enumFormaPagto = enumFormaPagto; //variavel com a opção da forma de pagto selecionada
    }
    function InscreveLinhaProduto(produto, index) {
        var texto = $("<div></div>");
        texto.html(produto.Descricao);
        $('[name="[' + index + '].NumProduto"]').text(produto.Fabricante + "/" + produto.NumProduto + " - " + texto.text());
        $('[name="[' + index + '].NumProduto"]').val(produto.NumProduto); //campo type hidden = passar para model
        $('[name="[' + index + '].Qtde"]').val(produto.Qtde);
        $('[name="[' + index + '].Preco"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.Preco));
        $('[name="[' + index + '].VlLista"]').text(moedaUtils.formatarMoedaSemPrefixo(produto.VlLista));
        $('[name="[' + index + '].Desconto"]').val(produto.Desconto);
        $('[name="[' + index + '].VlUnitario"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.VlUnitario));
        $('[name="[' + index + '].VlTotalItem"]').text(moedaUtils.formatarMoedaSemPrefixo(produto.TotalItem));
    }
    function VerificarEstoque(itens, produto) {
        if (itens.estoqueExcedido(produto)) {
            var novo = $("<tr><\tr>");
            var html = $(".novoProdutoEstoque").html();
            novo.html(html);
            novo.removeClass(".novoProdutoEstoque");
            novo.show();
            $(".novoProduto").parent().append(novo);
        }
    }
    function VerificarProdutoAviso(itens, produto) {
        if (itens.produtoTemAviso(produto)) {
            var novo = $("<tr><\tr>");
            var html = $(".novoProdutoAviso").html();
            novo.html(html);
            novo.removeClass(".novoProdutoAviso");
            novo.show();
            $(".novoProduto").parent().append(novo);
        }
        //itens.dtoPedido.ListaProdutos.forEach(function (i) {
        //    if (itens.produtoTemAviso(i)) {
        //        let novo = $("<tr><\tr>");
        //        let html = $(".novoProdutoAviso").html();
        //        novo.html(html);
        //        novo.removeClass(".novoProdutoAviso");
        //        novo.show();
        //        $(".novoProduto").parent().append(novo);
        //    }
        //});
    }
    function VerificarQtdeVendaPermitida(itens, produto) {
        itens.msgQtdePermitida;
        if (itens.produtoTemAviso(produto)) {
            var novo = $("<tr><\tr>");
            var html = $(".novoProdutoQtdeMaxPermitida").html();
            novo.html(html);
            novo.removeClass(".novoProdutoQtdeMaxPermitida");
            novo.show();
            $(".novoProduto").parent().append(novo);
        }
    }
    function continuar() {
        debugger;
        var err = new Error_1.ErrorModal();
        //verificar se tem produtos com qtde maior que o permitido
        var q = 0;
        dadosPagto.dtoPedido.ListaProdutos.forEach(function (r) {
            if (itens.qtdeVendaPermitida(r)) {
                q++;
            }
        });
        //this.prePedidoDto.ListaProdutos.forEach(r => {
        //    if (this.qtdeVendaPermitida(r)) {
        //        q++;
        //    }
        //});
        if (q > 0) {
            modalError();
            err.MostrarMsg("Produtos com quantidades maiores que a quantidade máxima permitida para venda!");
            //this.alertaService.mostrarMensagem("Produtos com quantidades maiores que a quantidade máxima permitida para venda!");
            return;
        }
        //validação: tem que ter algum produto
        if (dadosPagto.dtoPedido.ListaProdutos.length === 0) {
            modalError();
            err.MostrarMsg("Selecione ao menos um produto para continuar.");
            //this.alertaService.mostrarMensagem("Selecione ao menos um produto para continuar.");
            return;
        }
        //verifica se a forma de pgamento tem algum aviso
        if (!dadosPagto.podeContinuar(true)) {
            return;
        }
        var numeroPrepedido = this.activatedRoute.snapshot.params.numeroPrepedido;
        if (!!numeroPrepedido) {
            this.router.navigate(["../../observacoes"], { relativeTo: this.activatedRoute });
        }
        else {
            this.router.navigate(["../observacoes"], { relativeTo: this.activatedRoute });
        }
    }
});
//# sourceMappingURL=/scriptsJs/Views/Pedido/IniciarNovoPedido.js.map