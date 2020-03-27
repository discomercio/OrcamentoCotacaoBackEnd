import { ProdutoComboDto } from "../../DtosTs/ProdutosDto/ProdutoComboDto";
import { SelectProdInfo } from "../../DtosTs/ProdutosDto/SelectProdInfo";
import { Itens } from "../../FuncoesTs/Itens/Itens";
import { MoedaUtils } from "../../UtilTs/MoedaUtils/moedaUtils";
import { NovoPedidoDadosService } from "../../Services/NovoPepedidoDadosService";
import { DadosPagto } from "../../FuncoesTs/DadosPagto/DadosPagto";
import { EnumFormaPagto } from "../../FuncoesTs/DadosPagto/EnumFormaPagto";
import { Constantes } from "../../UtilTs/Constantes/Constantes";
import { ErrorModal } from "../Shared/Error";
import { PercentualMaxDescEComissao } from "../../DtosTs/PedidoDto/PercentualMaxDescEComissao";
import { ProdutoValidadoComEstoqueDto } from "../../DtosTs/ProdutosDto/ProdutoValidadoComEstoqueDto";
import { ProdutoDto } from "../../DtosTs/ProdutosDto/ProdutoDto";
import { PedidoProdutosPedidoDto } from "../../DtosTs/PedidoDto/PedidoProdutosPedidoDto";
import { CoeficienteDto } from "../../DtosTs/CoeficienteDto/CoeficienteDto";
import { PedidoDto } from "../../DtosTs/PedidoDto/PedidoDto";
import { FormaPagtoCriacaoDto } from "../../DtosTs/PedidoDto/FormaPagtoCriacaoDto";


//moedaUtils.formatarPorcentagemUmaCasa(i.Desconto)
declare var lstprodutos: ProdutoComboDto;
declare var produtoDto: ProdutoDto;
declare var itens: Itens;
itens = new Itens();
//essa variavel esta sendo usada para armazenar os itens selecionados pelo cliente
declare var lstProdSelecionados: PedidoProdutosPedidoDto[];
declare var window: any;
declare var indice: number;
declare var moedaUtils: MoedaUtils;
moedaUtils = new MoedaUtils();
declare var dadosPagto: DadosPagto;
dadosPagto = new DadosPagto();
declare var lstCoeficiente: CoeficienteDto[];
declare var qtdeParcVisa: number;
declare var permiteRAStatus: number;
//let formata = new MoedaUtils();
//alert("alterdado6 " + formata.formatarMoedaComPrefixo(123));

declare function modalError(): any;
declare function AbrirModalProdutos(): any;


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
        div = document.getElementById("ParcUnica");
        disp = div.style.display;
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
    $("#chkSemRa").val(0);
    $("#divCOM").children().find("input").prop('disabled', false);
    $("#chkComRa").val(1);
});
if ($("#chkSemRa").prop("checked") == true) {
    $("#chkSemRa").val(1);
}
if ($("#chkSemRa").prop("checked") == false) {
    $("#chkSemRa").val(0);
}

$("#chkSemRa").click(() => {
    $("#chkComRa").prop("checked", false);
    $("#chkComRa").val(0);
    $("#divCOM").children().find("input").prop('disabled', true);
    $("#chkSemRa").val(1);
});
if ($("#chkSemRa").prop("checked") == true) {
    $("#chkSemRa").val(1);
}
if ($("#chkSemRa").prop("checked") == false) {
    $("#chkSemRa").val(0);
}


$("#divIndicadores").children().find("input").prop('disabled', true);
//$("#chkSemIndicacao").prop("checked", true);
$("#chkSemIndicacao").click(() => {
    $("#chkComIndicacao").prop("checked", false);
    $("#chkComIndicacao").val(0);
    $("#divIndicadores").children().find("input").prop('disabled', true);
    $("#chkSemIndicacao").val(1);
});

if ($("#chkComIndicacao").prop('checked') == true) {
    $("#divIndicadores").children().find("input").prop('disabled', false);
    $("#chkComIndicacao").prop('checked', true);
    $("#chkComIndicacao").val(1);
}

$("#chkComIndicacao").click(() => {
    $("#chkSemIndicacao").prop("checked", false);
    $("#chkSemIndicacao").val(0);
    $("#divIndicadores").children().find("input").prop('disabled', false);
    $("#chkComIndicacao").val(1);
});

$("#btnModalProdutos").click(function () {
    let err = new ErrorModal();

    if ($("#chkSemIndicacao").prop('checked') == false && $("#chkComIndicacao").prop('checked') == false) {
        modalError();
        err.MostrarMsg("Informe se o pedido é com indicação ou não!");
        return false;
    }
    if ($("#chkComIndicacao").prop('checked') == true &&
        ($("#indicador").val() == "0" || $("#indicador").val() == undefined)) {
        modalError();
        err.MostrarMsg('Selecione o "indicador"!');
        return false;
    }

    if ($("#chkManual").prop("checked") == false && $("#chkAutomatico").prop("checked") == false) {
        modalError();
        err.MostrarMsg("Necessário selecionar o modo de seleção do CD");
        return false;
    }
    else if ($("#chkManual").prop("checked") == true) {

        let selecaoCD: any = $("#selecaoCd").val();
        if (selecaoCD == "0") {
            modalError();
            err.MostrarMsg("É necessário selecionar o CD que irá atender o pedido(sem auto-split)!");
            return false;
        }
    }
    AbrirModalProdutos();
    return true;
});


//$("#chkAutomatico").prop("checked", false);
$("#divSelecaoCd").children().find("input").prop('disabled', true);
$("#chkAutomatico").click(() => {
    $("#chkAutomatico").val(1);
    $("#chkManual").prop("checked", false);
    $("#chkManual").val(0);
    $("#divSelecaoCd").children().find("input").prop('disabled', true);
    $("#msgCD").text('');
    removerTodosProdutos();
    lstProdSelecionados = new Array<PedidoProdutosPedidoDto>();
    zerarCamposDadosPagto(true);
    totalPedido();
});
if ($("#chkManual").prop('checked') == true) {
    $("#chkManual").val(1);
    $("#chkManual").prop('checked', true);
    $("#chkManual").val(0);
    $("#divSelecaoCd").children().find("input").prop('disabled', false);
    $("#msgCD").text('');
}

$("#chkManual").click(() => {
    $("#chkManual").val(1);
    $("#chkAutomatico").prop("checked", false);
    $("#chkAutomatico").val(0);
    $("#divSelecaoCd").children().find("input").prop('disabled', false);
    $("#msgCD").text('');
    removerTodosProdutos();
    lstProdSelecionados = new Array<PedidoProdutosPedidoDto>();
    zerarCamposDadosPagto(true);
    totalPedido();
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

$("#selecaoCd").change(function () {
    $("#msgCD").text('');
    removerTodosProdutos();
    lstProdSelecionados = new Array<PedidoProdutosPedidoDto>();
    totalPedido();

});



declare var percMaxDescEComissaoPorLoja: PercentualMaxDescEComissao;

window.VerificarPercMaxDescEComissao = (e: HTMLInputElement) => {
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
        itens.pedidoDto = new PedidoDto();
        itens.mostrarProdutos(null);


        //estamos arrumando a qtde, verificando se existe e inserindo o produto
        arrumarProdsRepetidosTeste();
        //remover todas as linhas da tabela para adicionar novamente.
        removerTodosProdutos();

        RecalcularValoresSemCoeficiente(null, false);

        PedidoAlterado();
        //limpar tirar o check do produto selecionado
        $('.prod').children().find('input').filter(function () {
            if ($(this).prop('checked') == true)
                $(this).prop('checked', false);
            return true;
        });

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



function verificaRegras(novaClasse: string, produto: PedidoProdutosPedidoDto): void {

    verificarRegraProdutoCD(novaClasse, produto);
}

window.PreparaListaProdutosParaDataList = (): Array<string> => {
    let lstProdutosPreparadoParaDataList = new Array<string>();
    let t = {};

    lstprodutos.ProdutoDto.forEach((v) => {
        lstProdutosPreparadoParaDataList.push(v.Produto);
    });
    return lstProdutosPreparadoParaDataList;
}

function arrumarProdsRepetidosTeste() {

    let p = lstProdSelecionados;
    let exist: boolean = false;

    //é o que esta da vindo selecionado
    let lstPedidoProdutos = itens.pedidoDto.ListaProdutos;
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
    $("#totalPedidoInput").val(moedaUtils.formatarMoedaSemPrefixo(total));
    return
}

function removerTodosProdutos() {
    let tbody: any = $(".novoProduto").parent();
    let tbodyCount: number = tbody.children().length;


    if (tbodyCount >= 4) {
        for (tbodyCount; tbodyCount > 4; tbodyCount--) {
            let trTotal = tbody.children()[tbodyCount - 1].className;
            if (trTotal != "trTotal") {
                tbody.children()[tbodyCount - 1].remove();
            }
        }
    }

    indice = 0;
    $("#indice").val(indice);
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
        let classeName = linha.className;
        $("." + classeName + "").remove();
        //linha.remove();

        //pegando o td
        let codProduto: string = linha.cells[0].children[0].value;
        //pegando o produto para alterar o valor

        let produto: PedidoProdutosPedidoDto[] = new Array<PedidoProdutosPedidoDto>();
        produto = lstProdSelecionados.filter(e => e.NumProduto == codProduto);

        let i = lstProdSelecionados.indexOf(produto[0]);
        lstProdSelecionados.splice(i, 1);


        let indice: number = parseInt($("#indice").val().toString());
        $("#indice").val(indice--);

        //recalcular o pedido
        //Gabriel
        PedidoAlterado();
        totalPedido();
    }

    zerarCamposDadosPagto(false);
}

//altera valor total do item digitado
window.digitouQtde = (v: HTMLInputElement) => {
    //v = campo html
    let t = v as HTMLElement;
    //pegando o tr
    let linha: any = t.parentElement.parentElement.parentElement;
    //pegando o td
    let codProduto: string = linha.cells[0].children[0].value;


    lstProdSelecionados = dadosPagto.pedidoDto.ListaProdutos;
    //pegando o produto para alterar o valor
    let produto: PedidoProdutosPedidoDto[] = lstProdSelecionados.filter(e => e.NumProduto == codProduto);
    produto[0].Qtde = parseInt(v.value);
    itens.digitouQte(produto[0]);
    let classeLinha = linha.className;
    debugger;
    VerificarQtdeVendaPermitida(itens, produto[0], classeLinha);
    VerificarEstoque(itens, produto[0], classeLinha);


    //dadosPagto inicializar
    PedidoAlterado();

    itens.pedidoDto.ListaProdutos = lstProdSelecionados;


    //aqui espera receber o enum que esta sendo usado
    RecalcularValoresSemCoeficiente(dadosPagto.enumFormaPagto, true);


    ////passando para o valor para tela
    //linha.children[4].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(produto[0].VlUnitario);
    linha.children[7].textContent = moedaUtils.formatarMoedaSemPrefixo(produto[0].TotalItem);

    PedidoAlterado();//chamamos aqui para inicializar as variaveis
    //dadosPagto.prepedidoAlterado();
    //zerarCamposDadosPagto(true);
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
window.formatarDesc = (v: HTMLInputElement) => {
    //let valor = ((e.target) as HTMLInputElement).value;
    //let v: any = valor.replace(/\D/g, '');
    //v = (v / 100).toFixed(2) + '';

    //i.Desconto = v;

    let valor: string = v.value;
    let val: any = valor.replace(/\D/g, '');
    val = (val / 100).toFixed(2) + '';
    //pegar o valor para buscar na lista
    let linha: any = v.parentElement.parentElement.parentElement;
    let numProduto: string = linha.children[0].children[0].value;
    let r: PedidoProdutosPedidoDto[] = new Array<PedidoProdutosPedidoDto>();
    r = lstProdSelecionados.filter(e => e.NumProduto == numProduto);

    v.value = moedaUtils.formatarMoedaSemPrefixo(val);
}

window.formataVlVenda = (v: HTMLInputElement) => {
    let valor: string = v.value;
    let val: any = valor.replace(/\D/g, '');
    val = (val / 100).toFixed(2) + '';

    let linha: any = v.parentElement.parentElement.parentElement;
    let numProduto: string = linha.children[0].children[0].value;
    let r: PedidoProdutosPedidoDto[] = new Array<PedidoProdutosPedidoDto>();
    r = lstProdSelecionados.filter(e => e.NumProduto == numProduto);

    v.value = moedaUtils.formatarMoedaSemPrefixo(val);
}

window.digitouDesc = (v: HTMLInputElement) => {
    //let t = ((v.target) as HTMLInputElement).value;
    //let val: any = t.replace(/\D/g, '');
    let valor: string = v.value;
    let val: any = valor.replace(/\D/g, '');
    val = (val / 100).toFixed(2) + '';
    //passamos o valor formatado para o campo
    //v.value = moedaUtils.formatarMoedaSemPrefixo(val);


    //pegar o valor para buscar na lista
    let linha: any = v.parentElement.parentElement.parentElement;
    let numProduto: string = linha.children[0].children[0].value;
    let r: PedidoProdutosPedidoDto[] = new Array<PedidoProdutosPedidoDto>();
    r = lstProdSelecionados.filter(e => e.NumProduto == numProduto);
    r[0].Desconto = val;

    if (r[0].Desconto == 0 || r[0].Desconto.toString() == '') {
        r[0].AlterouVlVenda = false;
    } else {
        r[0].AlterouVlVenda = true;
    }
    debugger;
    itens.digitouDescValor(r[0], val);

    dadosPagto.pedidoDto.ListaProdutos = lstProdSelecionados;

    RecalcularValoresSemCoeficiente(dadosPagto.enumFormaPagto, true);

    totalPedido();

    linha.children[6].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);
    //linha.children[7].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlTotalItem);
    //linha.children[7].textContent = moedaUtils.formatarMoedaSemPrefixo(r[0].VlTotalItem);

    //zera os campos de lista de parcelamento calculados
    //zerarCamposDadosPagto(true);

}

window.digitouVlVenda = (v: HTMLInputElement) => {
    let valor: string = v.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    v.value = moedaUtils.formatarMoedaSemPrefixo(val);

    let linha: any = v.parentElement.parentElement.parentElement;
    let numProduto: string = linha.children[0].children[0].value;
    let r: PedidoProdutosPedidoDto[] = new Array<PedidoProdutosPedidoDto>();
    r = lstProdSelecionados.filter(e => e.NumProduto == numProduto);

    r[0].VlUnitario = parseFloat(val);

    if (r[0].VlLista == r[0].VlUnitario) {
        r[0].AlterouVlVenda = false;
    } else {
        r[0].AlterouVlVenda = true;
    }

    //calcula o desconto
    r[0].Desconto = 100 * (r[0].Preco - r[0].VlUnitario) / r[0].Preco;
    r[0].Desconto = parseFloat(r[0].Desconto.toFixed(1));

    itens.digitouQte(r[0]);
    //dadosPagto inicializar
    PedidoAlterado();
    debugger;

    linha.children[5].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].Desconto);
    linha.children[6].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);
    linha.children[7].textContent = moedaUtils.formatarMoedaSemPrefixo(r[0].TotalItem);

    zerarCamposDadosPagto(true);
}
//PermiteRA
window.digitouPreco = (v: HTMLInputElement) => {
    let valor = v.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    v.value = moedaUtils.formatarMoedaSemPrefixo(val);

    let linha: any = v.parentElement.parentElement.parentElement;
    let numProduto: string = linha.children[0].children[0].value;
    let r: PedidoProdutosPedidoDto[] = new Array<PedidoProdutosPedidoDto>();
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

    zerarCamposDadosPagto(true);
}

$('#diasVenc').keyup(function (this: any) {
    $(this).val(this.value.replace(/\D/g, ''));
});

window.digitouVlEntrada = (v: HTMLInputElement) => {
    let valor = v.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    v.value = moedaUtils.formatarMoedaSemPrefixo(val);

    dadosPagto.vlEntrada = parseFloat(val);

    dadosPagto.calcularParcelaComEntrada();
    //this.vlEntrada = v;
    //this.calcularParcelaComEntrada();
    //tranformar o valor sem prefixo


    if (dadosPagto.msgErro != "" && dadosPagto.msgErro != undefined) {
        modalError();
        let err = new ErrorModal();
        err.MostrarMsg(dadosPagto.msgErro);
        v.value = "";
        dadosPagto.msgErro = "";
        zerarCamposDadosPagto(false);

        $("#enumFormaPagto").prop('selectedIndex', 0);
        ($("#enumFormaPagto") as any).formSelect();
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
    //itens.pedidoDto.ListaProdutos = lstProdSelecionados;
    novoPedidoDadosService.pedidoDto = new PedidoDto();
    novoPedidoDadosService.pedidoDto.ListaProdutos = new Array<PedidoProdutosPedidoDto>();
    novoPedidoDadosService.pedidoDto.ListaProdutos = lstProdSelecionados;
    let totalP: string = $("#totalPedido").val().toString();
    //totalP = novoPedidoDadosService.totalPedido().toString();
    if (totalP != "0") {
        $("#totalPedido").text(totalP);
    }
});

function InserirNovoProduto(produto: PedidoProdutosPedidoDto, editandoLinha: boolean) {
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

    //if (!editandoLinha)
    indice = parseInt($("#indice").val().toString());

    if (indice.toString() == "NaN") {
        indice = 0;
        $("indice").val(indice);
    }

    let novaClasse: string = CopiarTRsMsgEProduto();
    //verificamos se mostraremos as msgs
    //chamar direto aqui
    verificarRegraProdutoCD(novaClasse, produto);

    //VerificarEstoque(itens, produto, novaClasse);
    ////verificar aqui
    //VerificarQtdeVendaPermitida(itens, produto, novaClasse);

    InscreveLinhaProduto(produto, indice);

    indice++;
    $("#indice").val(indice);
}
//iremos inicializar as variaveis
function PedidoAlterado() {

    //Forma pagamento
    //$("#enumFormaPagto").prop('selectedIndex', 0);
    //($("#enumFormaPagto") as any).formSelect();
    totalPedido();

    InicializaDadosPagto();

    dadosPagto.tipoFormaPagto;
    dadosPagto.pedidoAlterado();
    //zerarCamposDadosPagto(true);


}

declare var FormaPagtoServidor: any;
function zerarCamposDadosPagto(editandoLinha: boolean) {


    let selecione: any;

    //if (editandoLinha) {
    //    $("#enumFormaPagto").prop('selectedIndex', 0);
    //    ($("#enumFormaPagto") as any).formSelect();
    //}

    if (dadosPagto.enumFormaPagto == 1) {
        //A vista   
        //$("#Avista").css("display", "none");
        selecione = $("#opcaoPagtoAvista").children()[0];
        $("#opcaoPagtoAvista").html(selecione);
        ($("#opcaoPagtoAvista") as any).formSelect();

        $("#meioPagtoAVista").prop('selectedIndex', 0);
        ($("#meioPagtoAVista") as any).formSelect();
    }

    //ParcCartaoInternet
    if (dadosPagto.enumFormaPagto == 2) {

        selecione = $("#opcaoPagtoParcCartaoInternet").children()[0];
        $("#opcaoPagtoParcCartaoInternet").html(selecione);
        ($("#opcaoPagtoParcCartaoInternet") as any).formSelect();
    }

    if (dadosPagto.enumFormaPagto == 3) {
        //ParcComEnt
        //$("#ParcComEntrada").css("display", "none");
        $("#vlEntrada").val('');

        //$("#meioPagtoEntrada").prop('selectedIndex', 0);
        ($("#meioPagtoEntrada") as any).formSelect();

        selecione = $("#opcaoPagtoParcComEntrada").children()[0];
        $("#opcaoPagtoParcComEntrada").html(selecione);
        ($("#opcaoPagtoParcComEntrada") as any).formSelect();

        $("#meioPagtoEntradaPrest").prop('selectedIndex', 0);
        ($("#meioPagtoEntradaPrest") as any).formSelect();

        //$("#diasVenc").val('');
    }

    if (dadosPagto.enumFormaPagto == 4) {
        //ParcUnica
        //$("#ParcUnica").css("display", "none");
        selecione = $("#opcaoPagtoParcUnica").children()[0];
        $("#opcaoPagtoParcUnica").html(selecione);
        ($("#opcaoPagtoParcUnica") as any).formSelect();

        $("#meioPagtoParcUnica").prop('selectedIndex', 0);
        ($("#meioPagtoParcUnica") as any).formSelect();

        //$("#diasVencParcUnica").val('');
    }

    if (dadosPagto.enumFormaPagto == 5) {
        //ParcCartaoMaquineta
        //$("#PagtoCartaoMaquineta").css("display", "none");
        selecione = $("#opcaoPagtoParcCartaoMaquineta").children()[0];
        $("#opcaoPagtoParcCartaoMaquineta").html(selecione);
        ($("#opcaoPagtoParcCartaoMaquineta") as any).formSelect();
    }
}


function RecalcularValoresSemCoeficiente(v: any, editandoLinha: boolean) {
    zerarCamposDadosPagto(editandoLinha);
    InicializaDadosPagto();
    if (v == "" || v == undefined || v == null || v == 0) {
        v = 1;
    }

    if (lstProdSelecionados.length > 0) {

        dadosPagto.enumFormaPagto = v;
        //estamos limpandos os campos
        zerarCamposDadosPagto(editandoLinha);

        dadosPagto.qtdeParcVisa = qtdeParcVisa;
        dadosPagto.permiteRAStatus = permiteRAStatus;

        dadosPagto.recalcularValoresComCoeficiente(v);


        if (dadosPagto.msgErro != "" && dadosPagto.msgErro != undefined) {
            modalError();
            let err = new ErrorModal();
            err.MostrarMsg(dadosPagto.msgErro);
            dadosPagto.msgErro = "";
            return false;
        }

        //afazer: mandar a lista para o select correspondente;
        AtribuirListaParaSelect();

        //vamos apagar a linha de produtos selecionados e montar novamente com os valores atualizados
        //remover todas as linhas da tabela para adicionar novamente.


        if (!editandoLinha) {
            removerTodosProdutos();
            lstProdSelecionados.forEach((value) => {
                InserirNovoProduto(value, editandoLinha);
            });
        }


        //passando a lista recalculada
        lstProdSelecionados = dadosPagto.pedidoDto.ListaProdutos;

        //alterando o total do pedido
        totalPedido();
    }
}


//estamos add e escondendo as msg para fazer a verificação
function CopiarTRsMsgEProduto(): string {
    let indice: number = null;

    indice = parseInt($("#indice").val().toString());

    if (indice.toString() == "NaN") {
        indice = 0;
        $("indice").val(indice);
    }

    //criar nova classe
    let novaClasse = "trProduto_" + indice;


    //novoProduto
    let novoProduto = CriarLinha();
    let htmlnovoProduto = $(".novoProduto").html();
    htmlnovoProduto = htmlnovoProduto.replace(/trocarporindex/g, indice.toString());
    novoProduto.html(htmlnovoProduto);
    novoProduto.removeClass(".novoProduto");
    novoProduto.addClass(novaClasse);
    $(".trTotal").before(novoProduto);

    //novoProdutoEstoque    
    let novoProdEstoque = CriarLinha();
    let htmlNovoProdEstoque = $(".novoProdutoEstoque").html();
    novoProdEstoque.html(htmlNovoProdEstoque);
    novoProdEstoque.removeClass(".novoProdutoEstoque");
    novoProdEstoque.addClass(novaClasse);
    novoProdEstoque.css("display", "none");
    $(".trTotal").before(novoProdEstoque);

    //novoProdutoQtdeMaxPermitida  display:none
    let novoProdutoQtdeMaxPermitida = CriarLinha();
    let htmlnovoProdutoQtdeMaxPermitida = $(".novoProdutoQtdeMaxPermitida").html();
    novoProdutoQtdeMaxPermitida.html(htmlnovoProdutoQtdeMaxPermitida);
    novoProdutoQtdeMaxPermitida.removeClass(".novoProdutoQtdeMaxPermitida");
    novoProdutoQtdeMaxPermitida.addClass(novaClasse);
    novoProdutoQtdeMaxPermitida.css("display", "none");
    $(".trTotal").before(novoProdutoQtdeMaxPermitida);

    //novoProdutoAviso
    let novoProdutoAviso = CriarLinha();
    let htmlnovoProdutoAviso = $(".novoProdutoAviso").html();
    novoProdutoAviso.html(htmlnovoProdutoAviso);
    novoProdutoAviso.removeClass(".novoProdutoAviso");
    novoProdutoAviso.addClass(novaClasse);
    novoProdutoAviso.css("display", "none");
    $(".trTotal").before(novoProdutoAviso);

    indice++;
    $("#indice").val(indice);



    function CriarLinha() {
        return $('<tr><\tr>');
    }
    return novaClasse;

}

window.recalcularValoresComCoeficiente = (e: HTMLSelectElement) => {

    //zerarCamposDadosPagto();

    InicializaDadosPagto();
    //let v = e.selectedIndex;
    let v = parseInt(e.selectedOptions[0].value);


    inicializaCampos(v);

    RecalcularValoresSemCoeficiente(v, false);

    inicializaCampos(v);
}

//Escuta as mudanças do select opcaoPagtoParcCartaoInternet
$("#opcaoPagtoParcCartaoInternet").on('change', () => {

    //recebe o componente html
    let valor: string = $("#opcaoPagtoParcCartaoInternet").val().toString();


    //vamos validar e descobrir a qtde de parcelas que esta sendo selecionado
    //validação feita em dadosPagto
    if (!!valor) {
        dadosPagto.opcaoPagtoParcCartaoInternet = valor;
        dadosPagto.RecalcularListaProdutos();
        debugger;
        removerTodosProdutos();
        lstProdSelecionados.forEach((value) => {
            InserirNovoProduto(value, false);
        });

        lstProdSelecionados = dadosPagto.pedidoDto.ListaProdutos;

        //alterando o total do pedido
        totalPedido();
    }

});

$("#opcaoPagtoParcComEntrada").on('change', () => {
    //recebe o componente html
    let valor: string = $("#opcaoPagtoParcComEntrada").val().toString();

    //vamos validar e descobrir a qtde de parcelas que esta sendo selecionado
    //validação feita em dadosPagto
    if (!!valor) {
        dadosPagto.opcaoPagtoParcCartaoInternet = valor;
        dadosPagto.RecalcularListaProdutos();
        debugger;
        removerTodosProdutos();
        lstProdSelecionados.forEach((value) => {
            InserirNovoProduto(value, false);
        });

        lstProdSelecionados = dadosPagto.pedidoDto.ListaProdutos;

        //alterando o total do pedido
        totalPedido();
    }
});

$("#opcaoPagtoParcCartaoMaquineta").on('change', () => {
    //recebe o componente html
    let valor: string = $("#opcaoPagtoParcCartaoMaquineta").val().toString();

    //vamos validar e descobrir a qtde de parcelas que esta sendo selecionado
    //validação feita em dadosPagto
    if (!!valor) {
        dadosPagto.opcaoPagtoParcCartaoInternet = valor;
        dadosPagto.RecalcularListaProdutos();
        debugger;
        removerTodosProdutos();
        lstProdSelecionados.forEach((value) => {
            InserirNovoProduto(value, false);
        });

        lstProdSelecionados = dadosPagto.pedidoDto.ListaProdutos;

        //alterando o total do pedido
        totalPedido();
    }
});


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
        debugger;
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoParcCartaoInternet").append("<option>" + value + "</option>");
        });

        //($("#opcaoPagtoParcCartaoInternet") as any).addEventListener('onclick', 'RecalcularListaProdutos(this)');
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
            $("#opcaoPagtoParcUnica").append("<option selected>" + value + "</option>");
        });
        ($("#opcaoPagtoParcUnica") as any).formSelect();
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
    dadosPagto.coeficienteDto = new Array<CoeficienteDto>();//lista de coeficiente
    dadosPagto.pedidoDto = new PedidoDto();//Pedido
    dadosPagto.pedidoDto.ListaProdutos = new Array<PedidoProdutosPedidoDto>();//lista de produtos selecionados
    dadosPagto.constantes = new Constantes();//será utilizado para comparação
    dadosPagto.moedaUtils = new MoedaUtils();
    dadosPagto.lstMsg = new Array();
    //dadosPagto.tipoFormaPagto = 

    dadosPagto.coeficienteDto = lstCoeficiente;//recebe a lista que veio do servidor
    dadosPagto.pedidoDto.ListaProdutos = lstProdSelecionados;//recebe lst dos produtos selcionados    
    //dadosPagto.enumFormaPagto = enumFormaPagto;//variavel com a opção da forma de pagto selecionada


}

function InscreveLinhaProduto(produto: PedidoProdutosPedidoDto, index: any) {
    console.log(index);
    let texto: any = $("<div></div>");
    texto.html(produto.Descricao);

    $('[name="[' + index + '].NumProduto"]').text(produto.Fabricante + "/" + produto.NumProduto + " - " + texto.text());
    $('[name="[' + index + '].NumProduto"]').val(produto.NumProduto);//campo type hidden = passar para model
    $('[name="[' + index + '].Fabricante"]').val(produto.Fabricante);
    $('[name="[' + index + '].Qtde"]').val(produto.Qtde);
    $('[name="[' + index + '].Preco"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.Preco));
    $('[name="[' + index + '].VlLista"]').text(moedaUtils.formatarMoedaSemPrefixo(produto.VlLista));
    $('[name="[' + index + '].VlLista"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.VlLista));


    $('[name="[' + index + '].Desconto"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.Desconto));
    debugger;

    $('[name="[' + index + '].VlUnitario"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.VlUnitario));

    $('[name="[' + index + '].VlTotalItem"]').text(moedaUtils.formatarMoedaSemPrefixo(produto.TotalItem));
    $('[name="[' + index + '].VlTotalItem"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.TotalItem));

}

function VerificarEstoque(itens: Itens, produto: PedidoProdutosPedidoDto, novaClasse: string) {

    //será necessário alterar a verificação pq não estamos trazendo o estoque ao carregar todos produtos
    let filho = $("." + novaClasse + "").find("[name='estoqueIndisponivel']");
    if (itens.estoqueExcedido(produto)) {
        filho.parent().show();
    }
    else {
        filho.parent().hide();
    }
}

function VerificarProdutoAviso(itens: Itens, produto: PedidoProdutosPedidoDto) {

    if (itens.produtoTemAviso(produto)) {
        let novo = $("<tr><\tr>");
        let html = $(".novoProdutoAviso").html();
        novo.html(html);
        novo.removeClass(".novoProdutoAviso");
        novo.show();
        $(".novoProduto").parent().append(novo);
    }
}

function verificarRegraProdutoCD(novaclasse: string, produto: PedidoProdutosPedidoDto) {

    let cdManual = $("#chkManual").prop('checked');
    let selecaoCd = $("#selecaoCd").val();
    if (!!produto.NumProduto && !!produto.Fabricante) {
        $.ajax({
            url: "../Produtos/VerificarRegraProdutoCD/",
            type: "GET",
            data: { produto: JSON.stringify(produto), id_nfe_emitente_selecao_manual: selecaoCd },
            dataType: "json",
            success: function (data) {

                if (!!data) {
                    AdicionarMsgProdutoECD(data, novaclasse);
                }
            }
        });
    }


}

//Metodo para atribuir as msg de retorno sobre o produto e o CD
function AdicionarMsgProdutoECD(v: ProdutoValidadoComEstoqueDto, novaClasse: string): void {
    //precisa criar um campo para receber a msg sobre o CD selecionado

    let produtoValidadoComEstoque: ProdutoValidadoComEstoqueDto = new ProdutoValidadoComEstoqueDto();
    produtoValidadoComEstoque = v;

    //vamos verificar o retorno para mostrar as msg que estão retornando do servidor
    if (!!produtoValidadoComEstoque) {

        //pai.hide();
        produtoValidadoComEstoque.ListaErros.forEach(function (linha) {
            if (linha.indexOf("PRODUTO SEM PRESENÇA") != -1) {
                let span = $("." + novaClasse + "").find('[name="produtoAviso"]');
                let pai = span.parent().parent();
                span.text(linha);
                span.css('color', 'red');
                pai.show();
            }
            if (linha.indexOf("define o CD") != -1) {
                //msg referente ao cd
                $("#divMsgCD").css('display', 'block');
                $("#msgCD").text(linha);
                produtoValidadoComEstoque.Produto
            }
        });

        //vamos atribuir o produto estoque ao produto da lista lstProdutos
        AtribuirEstoqueAoProduto(produtoValidadoComEstoque.Produto);
    }


}

function AtribuirEstoqueAoProduto(produto: ProdutoDto) {
    lstprodutos.ProdutoDto.filter((e) => {
        if (e.Produto == produto.Produto && e.Fabricante == produto.Fabricante) {

            e.Estoque = produto.Estoque;
        }
    });
}

function VerificarQtdeVendaPermitida(itens: Itens, produto: PedidoProdutosPedidoDto, novaClasse: string) {
    itens.msgQtdePermitida;

    //será necessário alterar a verificação
    let filho = $("." + novaClasse + "").find("[name='msgQtdePermitida']");
    //msgQtdePermitida = filho span do tr
    if (itens.qtdeVendaPermitida(produto)) {
        filho.text(itens.msgQtdePermitida);
        filho.parent().parent().show(); //pai
    }
    else {
        //esconderemos a linha
        filho.parent().parent().hide(); //pai
    }
}

//declare function continuar(): any;

//window.continuar = () => {

//    if (!continuar()) {
//        return false;
//    }
//    //continuar();
//}



//iremos fazer a validação na tela
window.continuar = (): any => {

    let err = new ErrorModal();
    //verificar se tem produtos com qtde maior que o permitido
    let q: number = 0;

    if ($("#chkSemIndicacao").prop('checked') == false && $("#chkComIndicacao").prop('checked') == false) {
        modalError();
        err.MostrarMsg("Informe se o pedido é com ou sem indicação!");
        return false;
    }

    if (dadosPagto.pedidoDto == null || dadosPagto.pedidoDto == undefined) {
        modalError();
        err.MostrarMsg("Selecione ao menos um produto para continuar.");
        return false;
    } else {
        dadosPagto.pedidoDto.ListaProdutos.forEach(r => {
            if (itens.qtdeVendaPermitida(r)) {
                q++;
            }
        });

        if (q > 0) {
            modalError();
            err.MostrarMsg("Produtos com quantidades maiores que a quantidade máxima permitida para venda!");
            //this.alertaService.mostrarMensagem("Produtos com quantidades maiores que a quantidade máxima permitida para venda!");
            return;
        }
        //validação: tem que ter algum produto
        if (dadosPagto.pedidoDto.ListaProdutos.length === 0) {
            modalError();
            err.MostrarMsg("Selecione ao menos um produto para continuar.");

            //this.alertaService.mostrarMensagem("Selecione ao menos um produto para continuar.");
            return;
        }


        AtribuirFormaPagtoParaDadosPagto();
        //verifica se a forma de pgamento tem algum aviso
        if (!dadosPagto.podeContinuar(true)) {
            //verificar se a forma de pagamento está preenchida
            //necessário atribuir a forma de pagmento para dadosPagto

            if (dadosPagto.msgErro != "") {
                modalError();
                err.MostrarMsg(dadosPagto.msgErro);
                return false;
            }
            return;
        } else {

            //vamos tentar retornar o objeto serializado para a tela mandar para o controller
            dadosPagto.novoPedidoDadosService.pedidoDto.FormaPagtoCriacao = dadosPagto.pedidoDto.FormaPagtoCriacao;
            dadosPagto.novoPedidoDadosService.pedidoDto.ListaProdutos = dadosPagto.pedidoDto.ListaProdutos;

            //$("#lstProdutosSelecionados").val(dadosPagto.novoPedidoDadosService.pedidoDto.ListaProdutos.toString());
            $("#lstProdutosSelecionados").val(itens.pedidoDto.ListaProdutos.toString());
            //criar metodo para passar os campos para FormaPagtoCriacao
            if (AtribuirValoresFormaPagtoCriacao()) {
                return true;
            }
            //AtribuirValoresFormaPagtoCriacao();
        }



    }
}

function AtribuirValoresFormaPagtoCriacao(): boolean {

    let totalPedido = $("#totalPedido").text().trim();
    $("#Tipo_parcelamento").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Tipo_parcelamento);
    $("#TotalDestePedido").val(moedaUtils.formatarMoedaSemPrefixo(dadosPagto.pedidoDto.VlTotalDestePedido));

    if (dadosPagto.enumFormaPagto == 1) {
        $("#Rb_forma_pagto").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto);
        $("#Op_av_forma_pagto").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Op_av_forma_pagto);
        $("#Qtde_Parcelas").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas);
        return true;
    }
    //ParcCartaoInternet
    if (dadosPagto.enumFormaPagto == 2) {
        //Verificar se há necessidade de passar o valores para esse 
        $("#Rb_forma_pagto").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto);
        $("#C_pc_qtde").val(dadosPagto.pedidoDto.FormaPagtoCriacao.C_pc_qtde);
        $("#C_pc_valor").val(moedaUtils.formatarMoedaSemPrefixo(dadosPagto.pedidoDto.FormaPagtoCriacao.C_pc_valor));
        $("#Qtde_Parcelas").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas);
        return true;
    }
    //ParcComEnt
    if (dadosPagto.enumFormaPagto == 3) {
        //dadosPagto.vlEntrada já foi passado para calcular os valores com coeficiente
        $("#Rb_forma_pagto").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto);
        $("#Op_pce_entrada_forma_pagto").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Op_pce_entrada_forma_pagto);
        $("#Op_pce_prestacao_forma_pagto").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto);
        $("#C_pce_entrada_valor").val(moedaUtils.formatarMoedaSemPrefixo(
            dadosPagto.pedidoDto.FormaPagtoCriacao.C_pce_entrada_valor));
        $("#C_pce_prestacao_qtde").val(dadosPagto.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_qtde);
        $("#C_pce_prestacao_valor").val(moedaUtils.formatarMoedaSemPrefixo(
            dadosPagto.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_valor));
        $("#C_pce_prestacao_periodo").val(dadosPagto.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_periodo);
        $("#Qtde_Parcelas").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas);
        return true;
    }
    //ParcSemEnt não esta sendo usado
    if (dadosPagto.enumFormaPagto == 4) {

    }

    //ParcUnica
    if (dadosPagto.enumFormaPagto == 5) {

        $("#Rb_forma_pagto").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto);
        $("#Op_pu_forma_pagto").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Op_pu_forma_pagto);
        $("#C_pu_valor").val(moedaUtils.formatarMoedaSemPrefixo(
            dadosPagto.pedidoDto.FormaPagtoCriacao.C_pu_valor));
        $("#C_pu_vencto_apos").val(dadosPagto.pedidoDto.FormaPagtoCriacao.C_pu_vencto_apos);
        $("#Qtde_Parcelas").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas);
        return true;
    }
    //ParcCartaoMaquineta
    if (dadosPagto.enumFormaPagto == 6) {
        $("#Rb_forma_pagto").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto);
        $("#C_pc_maquineta_qtde").val(dadosPagto.pedidoDto.FormaPagtoCriacao.C_pc_maquineta_qtde);
        $("#C_pc_maquineta_valor").val(moedaUtils.formatarMoedaSemPrefixo(
            dadosPagto.pedidoDto.FormaPagtoCriacao.C_pc_maquineta_valor));
        $("#C_pc_maquineta_qtde").val(dadosPagto.pedidoDto.FormaPagtoCriacao.C_pc_maquineta_qtde);
        $("#Qtde_Parcelas").val(dadosPagto.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas);
        return true;
    }
}

declare var testeJson: string;

function AtribuirFormaPagtoParaDadosPagto(): void {
    let err = new ErrorModal();

    //vamos inicializar o dadosPagto.pedidoDto.FormaPagtoCriacao
    dadosPagto.pedidoDto.FormaPagtoCriacao = new FormaPagtoCriacaoDto();

    dadosPagto.novoPedidoDadosService = new NovoPedidoDadosService();
    dadosPagto.novoPedidoDadosService.criarNovo();
    dadosPagto.pedidoDto.FormaPagtoCriacao = new FormaPagtoCriacaoDto();

    //A vista
    if (dadosPagto.enumFormaPagto == 1) {
        dadosPagto.meioPagtoAVista = parseInt($("#meioPagtoAVista").val().toString());
        dadosPagto.opcaoPagtoAvista = $("#opcaoPagtoAvista").val().toString();
    }
    //ParcCartaoInternet
    if (dadosPagto.enumFormaPagto == 2) {
        //Verificar se há necessidade de passar o valores para esse 
        dadosPagto.opcaoPagtoParcCartaoInternet = $("#opcaoPagtoParcCartaoInternet").val().toString();
    }
    //ParcComEnt
    if (dadosPagto.enumFormaPagto == 3) {
        //dadosPagto.vlEntrada já foi passado para calcular os valores com coeficiente
        dadosPagto.meioPagtoEntradaPrest = parseInt($("#meioPagtoEntradaPrest").val().toString());
        dadosPagto.meioPagtoEntrada = parseInt($("#meioPagtoEntrada").val().toString());
        dadosPagto.diasVenc = parseInt($("#diasVenc").val().toString());
        dadosPagto.opcaoPagtoParcComEntrada = $("#opcaoPagtoParcComEntrada").val().toString();

    }
    //ParcSemEnt não esta sendo usado
    if (dadosPagto.enumFormaPagto == 4) {

    }
    //ParcUnica
    if (dadosPagto.enumFormaPagto == 5) {
        dadosPagto.opcaoPagtoParcCartaoInternet = $("#opcaoPagtoParcCartaoInternet").val().toString();
        dadosPagto.meioPagtoParcUnica = parseInt($("#meioPagtoParcUnica").val().toString());
        dadosPagto.diasVencParcUnica = parseInt($("#diasVencParcUnica").val().toString());
    }
    //ParcCartaoMaquineta
    if (dadosPagto.enumFormaPagto == 6) {
        dadosPagto.opcaoPagtoParcCartaoMaquineta = $("#opcaoPagtoParcCartaoMaquineta").val().toString();
    }

}
