import { DtoProdutoCombo } from "./../../DtosTs/DtoProdutos/DtoProdutoCombo";
import { SelectProdInfo } from "../../DtosTs/DtoProdutos/SelectProdInfo";
import { Itens } from "../../FuncoesTs/Itens/Itens";
import { DtoPedido } from "../../DtosTs/DtoPedido/DtoPedido";
import { DtoPedidoProdutosPedido } from "../../DtosTs/DtoPedido/DtoPedidoProdutosPedido";
import { MoedaUtils } from "../../UtilTs/MoedaUtils/moedaUtils";
import { NovoPedidoDadosService } from "../../Services/NovoPepedidoDadosService";
import { DadosPagto } from "../../FuncoesTs/DadosPagto/DadosPagto";
import { EnumFormaPagto } from "../../FuncoesTs/DadosPagto/EnumFormaPagto";
import { DtoCoeficiente } from "../../DtosTs/DtoCoeficiente/DtoCoeficiente";
import { Constantes } from "../../UtilTs/Constantes/Constantes";
import { DtoProduto } from "../../DtosTs/DtoProdutos/DtoProduto";
import { ErrorModal } from "../Shared/Error";
import { PercentualMaxDescEComissao } from "../../DtosTs/DtoPedido/PercentualMaxDescEComissao";


//moedaUtils.formatarPorcentagemUmaCasa(i.Desconto)
declare var lstprodutos: DtoProdutoCombo;
declare var ProdutoDto: DtoProduto;
declare var itens: Itens;
itens = new Itens();
//essa variavel esta sendo usada para armazenar os itens selecionados pelo cliente
declare var lstProdSelecionados: DtoPedidoProdutosPedido[];
declare var window: any;
declare var indice: number;
declare var moedaUtils: MoedaUtils;
moedaUtils = new MoedaUtils();
declare var dadosPagto: DadosPagto;
dadosPagto = new DadosPagto();
declare var lstCoeficiente: DtoCoeficiente[];
declare var qtdeParcVisa: number;
//let formata = new MoedaUtils();
//alert("alterdado6 " + formata.formatarMoedaComPrefixo(123));

declare function modalError(): any;


function inicializaCampos(v: number) {
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
$("#chkComRa").click(() => {
    $("#chkSemRa").prop("checked", false);
    $("#divCOM").children().find("input").prop('disabled', false);
});
$("#chkSemRa").click(() => {
    $("#chkComRa").prop("checked", false);
    $("#divCOM").children().find("input").prop('disabled', true);
});


$("#divIndicadores").children().find("input").prop('disabled', true);
$("#chkSemIndicacao").prop("checked", true);
$("#chkSemIndicacao").click(() => {
    $("#chkComIndicacao").prop("checked", false);
    $("#divIndicadores").children().find("input").prop('disabled', true);
});
$("#chkComIndicacao").click(() => {
    $("#chkSemIndicacao").prop("checked", false);
    $("#divIndicadores").children().find("input").prop('disabled', false);
});

$("#chkAutomatico").prop("checked", true);
$("#divSelecaoCd").children().find("input").prop('disabled', true);
$("#chkAutomatico").click(() => {
    $("#chkManual").prop("checked", false);
    $("#divSelecaoCd").children().find("input").prop('disabled', true);
});
$("#chkManual").click(() => {
    $("#chkAutomatico").prop("checked", false);
    $("#divSelecaoCd").children().find("input").prop('disabled', false);
});

//limpando campos ao fechar a modal
$("#buscaproduto").keyup(function () {
    let buscaProduto: string = $("#buscaproduto").val().toString();

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

$("#percComissao").keyup(() => {
    let valor = $("#percComissao").val().toString();
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    $("#percComissao").val(moedaUtils.formatarMoedaSemPrefixo(val));
});

declare var percMaxDescEComissaoPorLoja: PercentualMaxDescEComissao;

window.VerificarPercMaxDescEComissao = (e: HTMLInputElement) => {
    debugger;

    let valor = e.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    e.value = moedaUtils.formatarMoedaSemPrefixo(val);

    
    let msgErro: string = "";
    let percMax = new PercentualMaxDescEComissao()
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
        let err = new ErrorModal();
        err.MostrarMsg(msgErro);
        return false;
    }
}

//como é chamadado diretamente do HTML, tem que estar na window
window.InserirProdutoLinha = () => {

    let selectProdInfo = new SelectProdInfo();
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

        itens.dtoProdutoCombo = lstprodutos;//pegando da tela
        itens.selectProdInfo = selectProdInfo;
        itens.dtoPedido = new DtoPedido();
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
            let err = new ErrorModal();
            err.MostrarMsg(itens.msgErro);
        }
    }
    else {
        return false;
    }
}

window.PreparaListaProdutosParaDataList = (): Array<string> => {
    let lstProdutosPreparadoParaDataList = new Array<string>();
    let t = {};
    console.log("preparando a lista");
    console.log(lstprodutos);
    lstprodutos.ProdutoDto.forEach((v) => {
        lstProdutosPreparadoParaDataList.push(v.Produto);
    });
    return lstProdutosPreparadoParaDataList;
}

function arrumarProdsRepetidosTeste() {

    let p = lstProdSelecionados;
    let exist: boolean = false;

    //é o que esta da vindo selecionado
    let lstPedidoProdutos = itens.dtoPedido.ListaProdutos;
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
        let t = JSON.stringify(lstProdSelecionados);

        lstPedidoProdutos.forEach(function (f) {
            let r: any = t.indexOf(f.NumProduto);
            if (r < 0) {
                f.Desconto = f.Desconto ? f.Desconto : 0;
                lstProdSelecionados.push(f);
            }
        });

    }
}

//calculamos os produtos e somamos o total
function totalPedido() {
    let total: number = lstProdSelecionados.reduce((sum, current) => sum + current.TotalItem, 0);
    $("#totalPedido").text(moedaUtils.formatarMoedaSemPrefixo(total));
    return
}

function removerTodosProdutos() {
    let tbody: any = $(".novoProduto").parent();
    let tbodyCount: number = tbody.children().length;
    console.log(tbodyCount);

    if (tbodyCount >= 4) {
        for (tbodyCount; tbodyCount > 4; tbodyCount--) {
            let trTotal = tbody.children()[tbodyCount - 1].className;
            if (trTotal != "trTotal") {
                tbody.children()[tbodyCount - 1].remove();
            }
        }
    }
    indice = 0;
}
declare var enumFormaPagto: EnumFormaPagto;

window.removerLinha = (v: HTMLElement) => {
    //fazer a perngunta para saber se confirma remover 
    //const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
    //    data: `Remover este item do pré-pedido?`
    //});
    if (true) {
        //v = campo html
        let t = v as HTMLElement;
        //pegando o tr
        let linha: any = t.closest("tr");// t.parentElement.parentElement.parentElement.parentElement;
        linha.remove();

        //pegando o td
        let codProduto: string = linha.cells[0].children[0].value;
        //pegando o produto para alterar o valor

        let produto: DtoPedidoProdutosPedido[] = new Array<DtoPedidoProdutosPedido>();
        produto = lstProdSelecionados.filter(e => e.NumProduto == codProduto);

        let i = lstProdSelecionados.indexOf(produto[0]);
        lstProdSelecionados.splice(i, 1);

        console.log(lstProdSelecionados);

        //recalcular o pedido
        //Gabriel
        PedidoAlterado();
        totalPedido();
    }

    zerarCamposDadosPagto();
}

//altera valor total do item digitado
window.digitouQtde = (v: HTMLInputElement) => {
    //v = campo html
    let t = v as HTMLElement;
    //pegando o tr
    let linha: any = t.parentElement.parentElement.parentElement;
    //pegando o td
    let codProduto: string = linha.cells[0].children[0].value;


    lstProdSelecionados = dadosPagto.dtoPedido.ListaProdutos;
    //pegando o produto para alterar o valor
    let produto: DtoPedidoProdutosPedido[] = lstProdSelecionados.filter(e => e.NumProduto == codProduto);
    produto[0].Qtde = parseInt(v.value);
    itens.digitouQte(produto[0]);
    //dadosPagto inicializar
    PedidoAlterado();

    itens.dtoPedido.ListaProdutos = lstProdSelecionados;

    RecalcularValoresSemCoeficiente(null);
    ////passando para o valor para tela
    linha.children[4].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(produto[0].VlUnitario);
    linha.children[5].textContent = moedaUtils.formatarMoedaSemPrefixo(produto[0].TotalItem);

    PedidoAlterado();//chamamos aqui para inicializar as variaveis
    //dadosPagto.prepedidoAlterado();
    zerarCamposDadosPagto();
}

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


window.digitouDesc = (v: HTMLInputElement) => {
    //let t = ((v.target) as HTMLInputElement).value;
    //let val: any = t.replace(/\D/g, '');
    let valor: string = v.value;
    let val: any = valor.replace(/\D/g, '');
    val = (val / 10).toFixed(2) + '';
    //passamos o valor formatado para o campo
    v.value = moedaUtils.formatarPorcentagemUmaCasa(val);

    //pegar o valor para buscar na lista
    let linha: any = v.parentElement.parentElement.parentElement;
    let numProduto: string = linha.children[0].children[0].value;
    let r: DtoPedidoProdutosPedido[] = new Array<DtoPedidoProdutosPedido>();
    r = lstProdSelecionados.filter(e => e.NumProduto == numProduto);
    //r[0].Desconto = val;
    itens.digitouDescValor(r[0], val);

    dadosPagto.dtoPedido.ListaProdutos = lstProdSelecionados;
    //RecalcularValoresSemCoeficiente(null);
    totalPedido();

    linha.children[5].textContent = moedaUtils.formatarMoedaSemPrefixo(r[0].TotalItem);
    linha.children[4].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);

    zerarCamposDadosPagto();

}

window.digitouVlVenda = (v: HTMLInputElement) => {
    let valor: string = v.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    v.value = moedaUtils.formatarMoedaSemPrefixo(val);

    let linha: any = v.parentElement.parentElement.parentElement;
    let numProduto: string = linha.children[0].children[0].value;
    let r: DtoPedidoProdutosPedido[] = new Array<DtoPedidoProdutosPedido>();
    r = lstProdSelecionados.filter(e => e.NumProduto == numProduto);

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
}
//PermiteRA
window.digitouPreco = (v: HTMLInputElement) => {
    let valor = v.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    v.value = moedaUtils.formatarMoedaSemPrefixo(val);

    let linha: any = v.parentElement.parentElement.parentElement;
    let numProduto: string = linha.children[0].children[0].value;
    let r: DtoPedidoProdutosPedido[] = new Array<DtoPedidoProdutosPedido>();
    r = lstProdSelecionados.filter(e => e.NumProduto == numProduto);

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
}

window.digitouVlEntrada = (v: HTMLInputElement) => {
    debugger;
    let valor = v.value;
    let val: any = valor.replace(/\D/g, '');

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
        let err = new ErrorModal();
        err.MostrarMsg(dadosPagto.msgErro);
        v.value = "";
        return false;
    }

    let selecione: any = $("#opcaoPagtoParcComEntrada").children()[0];
    $("#opcaoPagtoParcComEntrada").html(selecione);
    ($("#opcaoPagtoParcComEntrada") as any).formSelect();
    AtribuirListaParaSelect();
}

declare var novoPedidoDadosService: NovoPedidoDadosService;
novoPedidoDadosService = new NovoPedidoDadosService();
$("#totalPedido").ready(() => {
    //itens.dtoPedido.ListaProdutos = lstProdSelecionados;
    novoPedidoDadosService.dtoPedido = new DtoPedido();
    novoPedidoDadosService.dtoPedido.ListaProdutos = new Array<DtoPedidoProdutosPedido>();
    novoPedidoDadosService.dtoPedido.ListaProdutos = lstProdSelecionados;
    let totalP: string = $("#totalPedido").val().toString();
    totalP = novoPedidoDadosService.totalPedido().toString();
    if (totalP != "0") {
        $("#totalPedido").text(totalP);
    }
});

function InserirNovoProduto(produto: DtoPedidoProdutosPedido) {
    //esse é um exemplo de como clonar uma div e adicionar 
    //let novo = $(".novoProduto").clone();
    //novo.removeClass("novoProduto")
    //$(".novoProduto").parent().append(novo);

    /*
     * afazer: necessário pegar o index que esta armazenando os valores
     * trocar onde esta escrito "trocarporindex" e colocar o id
     * verificar se o produto tem avisos e qtde permitida e ra_status
     */
    let indice: number = null;

    indice = parseInt($("#indice").val().toString());

    if (indice.toString() == "NaN") {
        indice = 0;
        $("indice").val(indice);
    }

    let novo = $("<tr></tr>");
    let html = $(".novoProduto").html();
    html = html.replace(/trocarporindex/g, indice.toString());
    novo.html(html);
    novo.removeClass("novoProduto");
    novo.show();
    $(".trTotal").before(novo);
    //$(".novoProduto").parent().append(novo);

    let elem = novo.children();
    InscreveLinhaProduto(produto, indice);

    indice++;
    $("#indice").val(indice);
}
//iremos inicializar as variaveis
function PedidoAlterado() {

    //Forma pagamento
    $("#enumFormaPagto").prop('selectedIndex', 0);
    ($("#enumFormaPagto") as any).formSelect();

    totalPedido();

    InicializaDadosPagto();

    dadosPagto.tipoFormaPagto;
    dadosPagto.pedidoAlterado();
    zerarCamposDadosPagto();

    console.log("voltou do dadosPagto pedidoalterado");
    console.log(dadosPagto);
}

declare var FormaPagtoServidor: any;
function zerarCamposDadosPagto() {
    console.log("entrou no");

    let selecione: any;

    //A vista   
    $("#Avista").css("display", "none");
    selecione = $("#opcaoPagtoAvista").children()[0];
    $("#opcaoPagtoAvista").html(selecione);
    ($("#opcaoPagtoAvista") as any).formSelect();

    $("#meioPagtoAVista").prop('selectedIndex', 0);
    ($("#meioPagtoAVista") as any).formSelect();

    //ParcCartaoInternet
    $("#PagtoCartaoInternet").css("display", "none");
    selecione = $("#opcaoPagtoParcCartaoInternet").children()[0];
    $("#opcaoPagtoParcCartaoInternet").html(selecione);
    ($("#opcaoPagtoParcCartaoInternet") as any).formSelect();

    //ParcComEnt
    $("#ParcComEntrada").css("display", "none");
    $("#vlEntrada").val('');

    $("#meioPagtoEntrada").prop('selectedIndex', 0);
    ($("#meioPagtoEntrada") as any).formSelect();

    selecione = $("#opcaoPagtoParcComEntrada").children()[0];
    $("#opcaoPagtoParcComEntrada").html(selecione);
    ($("#opcaoPagtoParcComEntrada") as any).formSelect();

    $("#meioPagtoEntradaPrest").prop('selectedIndex', 0);
    ($("#meioPagtoEntradaPrest") as any).formSelect();

    $("#diasVenc").val('');

    //ParcUnica
    $("#ParcUnica").css("display", "none");
    selecione = $("#opcaoPagtoParcUnica").children()[0];
    $("#opcaoPagtoParcUnica").html(selecione);
    ($("#opcaoPagtoParcUnica") as any).formSelect();

    $("#meioPagtoParcUnica").prop('selectedIndex', 0);
    ($("#meioPagtoParcUnica") as any).formSelect();

    $("#diasVencParcUnica").val('');

    //ParcCartaoMaquineta
    $("#PagtoCartaoMaquineta").css("display", "none");
    selecione = $("#opcaoPagtoParcCartaoMaquineta").children()[0];
    $("#opcaoPagtoParcCartaoMaquineta").html(selecione);
    ($("#opcaoPagtoParcCartaoMaquineta") as any).formSelect();
}


function RecalcularValoresSemCoeficiente(v: any) {
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
            let err = new ErrorModal();
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
        lstProdSelecionados.forEach((value) => {
            InserirNovoProduto(value);
            VerificarEstoque(itens, value);
            VerificarProdutoAviso(itens, value);
            VerificarQtdeVendaPermitida(itens, value);
        });

        //alterando o total do pedido
        totalPedido();
    }
}

window.recalcularValoresComCoeficiente = (e: HTMLSelectElement) => {
    zerarCamposDadosPagto();

    InicializaDadosPagto();
    //let v = e.selectedIndex;
    let v = parseInt(e.selectedOptions[0].value);


    inicializaCampos(v);

    RecalcularValoresSemCoeficiente(v);

    inicializaCampos(v);
}

function AtribuirListaParaSelect() {

    if (dadosPagto.enumFormaPagto == 1) {
        //A vista   
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoAvista").append("<option selected>" + value + "</option>");
        });
        ($("#opcaoPagtoAvista") as any).formSelect();
    }
    if (dadosPagto.enumFormaPagto == 2) {
        //ParcCartaoInternet
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoParcCartaoInternet").append("<option>" + value + "</option>");
        });
        ($("#opcaoPagtoParcCartaoInternet") as any).formSelect();
    }
    if (dadosPagto.enumFormaPagto == 3) {
        //ParcComEnt
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoParcComEntrada").append("<option>" + value + "</option>");
        });
        ($("#opcaoPagtoParcComEntrada") as any).formSelect();
    }
    //NÃO ESTA SENDO USADO
    if (dadosPagto.enumFormaPagto == 4) {
        //ParcSemEnt
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoParcSemEntrada").append("<option>" + value + "</option>");
        });
        ($("#opcaoPagtoParcSemEntrada") as any).formSelect();
    }
    if (dadosPagto.enumFormaPagto == 5) {
        //ParcUnica
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoParcCartaoInternet").append("<option>" + value + "</option>");
        });
        ($("#opcaoPagtoParcCartaoInternet") as any).formSelect();
    }
    if (dadosPagto.enumFormaPagto == 6) {
        //ParcCartaoMaquineta
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoParcCartaoMaquineta").append("<option>" + value + "</option>");
        });
        ($("#opcaoPagtoParcCartaoMaquineta") as any).formSelect();
    }

}

function InicializaDadosPagto() {
    dadosPagto.coeficienteDto = new Array<DtoCoeficiente>();//lista de coeficiente
    dadosPagto.dtoPedido = new DtoPedido();//Pedido
    dadosPagto.dtoPedido.ListaProdutos = new Array<DtoPedidoProdutosPedido>();//lista de produtos selecionados
    dadosPagto.constantes = new Constantes();//será utilizado para comparação
    dadosPagto.moedaUtils = new MoedaUtils();
    dadosPagto.lstMsg = new Array();
    //dadosPagto.tipoFormaPagto = 

    dadosPagto.coeficienteDto = lstCoeficiente;//recebe a lista que veio do servidor
    dadosPagto.dtoPedido.ListaProdutos = lstProdSelecionados;//recebe lst dos produtos selcionados    
    dadosPagto.enumFormaPagto = enumFormaPagto;//variavel com a opção da forma de pagto selecionada
}

function InscreveLinhaProduto(produto: DtoPedidoProdutosPedido, index: any) {

    let texto: any = $("<div></div>");
    texto.html(produto.Descricao);

    $('[name="[' + index + '].NumProduto"]').text(produto.Fabricante + "/" + produto.NumProduto + " - " + texto.text());
    $('[name="[' + index + '].NumProduto"]').val(produto.NumProduto);//campo type hidden = passar para model
    $('[name="[' + index + '].Qtde"]').val(produto.Qtde);
    $('[name="[' + index + '].Preco"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.Preco));
    $('[name="[' + index + '].VlLista"]').text(moedaUtils.formatarMoedaSemPrefixo(produto.VlLista));

    $('[name="[' + index + '].Desconto"]').val(produto.Desconto);
    $('[name="[' + index + '].VlUnitario"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.VlUnitario));
    $('[name="[' + index + '].VlTotalItem"]').text(moedaUtils.formatarMoedaSemPrefixo(produto.TotalItem));

}

function VerificarEstoque(itens: Itens, produto: DtoPedidoProdutosPedido) {

    if (itens.estoqueExcedido(produto)) {
        let novo = $("<tr><\tr>");
        let html = $(".novoProdutoEstoque").html();
        novo.html(html);
        novo.removeClass(".novoProdutoEstoque");
        novo.show();
        $(".novoProduto").parent().append(novo);
    }
}

function VerificarProdutoAviso(itens: Itens, produto: DtoPedidoProdutosPedido) {

    if (itens.produtoTemAviso(produto)) {
        let novo = $("<tr><\tr>");
        let html = $(".novoProdutoAviso").html();
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

function VerificarQtdeVendaPermitida(itens: Itens, produto: DtoPedidoProdutosPedido) {
    itens.msgQtdePermitida;

    if (itens.produtoTemAviso(produto)) {
        let novo = $("<tr><\tr>");
        let html = $(".novoProdutoQtdeMaxPermitida").html();
        novo.html(html);
        novo.removeClass(".novoProdutoQtdeMaxPermitida");
        novo.show();
        $(".novoProduto").parent().append(novo);
    }
}

function continuar() {
    debugger;
    let err = new ErrorModal();
    //verificar se tem produtos com qtde maior que o permitido
    let q: number = 0;
    dadosPagto.dtoPedido.ListaProdutos.forEach(r => {
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

    let numeroPrepedido = this.activatedRoute.snapshot.params.numeroPrepedido;
    if (!!numeroPrepedido) {
        this.router.navigate(["../../observacoes"], { relativeTo: this.activatedRoute });
    }
    else {
        this.router.navigate(["../observacoes"], { relativeTo: this.activatedRoute });
    }
}
