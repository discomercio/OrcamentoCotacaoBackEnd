import { ProdutoComboDto } from "../../DtosTs/ProdutosDto/ProdutoComboDto";
import { SelectProdInfo } from "../../DtosTs/ProdutosDto/SelectProdInfo";
import { Itens } from "../../FuncoesTs/Itens/Itens";
import { MoedaUtils } from "../../UtilTs/MoedaUtils/moedaUtils";
import { NovoPedidoDadosService } from "../../Services/NovoPepedidoDadosService";
import { DadosPagto } from "../../FuncoesTs/DadosPagto/DadosPagto";
import { EnumFormaPagto } from "../../FuncoesTs/DadosPagto/EnumFormaPagto";
import { Constantes } from "../../UtilTs/Constantes/Constantes";
import { ErrorModal } from "../Shared/Error";
import { ProdutoValidadoComEstoqueDto } from "../../DtosTs/ProdutosDto/ProdutoValidadoComEstoqueDto";
import { ProdutoDto } from "../../DtosTs/ProdutosDto/ProdutoDto";
import { PedidoProdutosPedidoDto } from "../../DtosTs/PedidoDto/PedidoProdutosPedidoDto";
import { CoeficienteDto } from "../../DtosTs/CoeficienteDto/CoeficienteDto";
import { PedidoDto } from "../../DtosTs/PedidoDto/PedidoDto";
import { FormaPagtoCriacaoDto } from "../../DtosTs/PedidoDto/FormaPagtoCriacaoDto";
import { RecalcularComCoeficiente } from "../../FuncoesTs/RecalcularComCoeficiente/RecalcularComCoeficiente";
import { PercentualMaximoDto } from "../../DtosTs/PedidoDto/PercentualMaximoDto";
import { MeioPagtoPreferenciais } from "../../DtosTs/FormaPagtoDto/MeioPagtoPreferenciais";
import { ObjetoSenhaDesconto } from "../../DtosTs/LojaDto/ObjetoSenhaDesconto";

declare var window: any;
declare var indice: number;
declare var qtdeParcVisa: number;
declare var permiteRAStatus: number;
declare var percComissao: number;//comissão informada em selecaoCD = perc_RT
declare var perc_RT_novo: number;
declare var FormaPagtoServidor: any;
declare var cdSelecionadoId: any;

declare var lstprodutos: ProdutoComboDto;
declare var produtoDto: ProdutoDto;
//essa variavel esta sendo usada para armazenar os itens selecionados pelo cliente
declare var lstProdSelecionados: PedidoProdutosPedidoDto[];
declare var lstCoeficiente: CoeficienteDto[];
declare var percentualMaximoDto: PercentualMaximoDto;
declare var enumFormaPagto: EnumFormaPagto;
declare var meiosPagtoPreferenciais: MeioPagtoPreferenciais;
declare var listaObjetoSenhaDesconto: ObjetoSenhaDesconto[];


itens = new Itens();
declare var itens: Itens;
declare var moedaUtils: MoedaUtils;
moedaUtils = new MoedaUtils();
declare var dadosPagto: DadosPagto;
dadosPagto = new DadosPagto();
declare var erroPercNovo: boolean;
erroPercNovo = false;
declare var novoPedidoDadosService: NovoPedidoDadosService;
novoPedidoDadosService = new NovoPedidoDadosService();
declare var lstErros: string[];
lstErros = new Array();

declare function swal(header, mensagem): any;
declare function swal(header, mensagem, tipo): any;
declare function AbrirModalProdutos(): any;

//alterando botão para add produtos 
$(window).resize(function () {
    //se redimensionar a tela 
    if (window.innerWidth < 663) {
        AlterarClasse();
    }
    if (window.innerWidth > 663) {
        $("#btn-addProd").removeClass("btn-sm");
    }
});
$(function () {
    //se a tela estive nessa resolução ao atualizar 
    if (window.innerWidth < 663) {
        AlterarClasse();
    }

    if (window.innerWidth > 663) {
        $("#btn-addProd").removeClass("btn-sm");
    }

});
function AlterarClasse() {
    $("#btn-addProd").addClass("btn-sm");
}

//abrir modal para seleção de produtos
window.AbrirModalProdutos = () => {
    //'#modal1' modal de produtos
    //todo: verificar pq dá erro de compilação
    ($("#modal1") as any).modal("show");
}

function inicializaCampos(v: number) {
    let div: any;
    var disp;
    if (v.toString() == "1") {
        div = document.getElementsByClassName("Avista");
        for (let i = 0; i < div.length; i++) {
            disp = div[i].style.display;
            if (disp == 'none') {
                div[i].style.display = 'block';
            }
        }
    }
    else {
        div = document.getElementsByClassName("Avista");
        if (div.length > 0) {
            for (let i = 0; i < div.length; i++) {
                div[i].style.display = 'none';
            }
        }
    }
    if (v.toString() == "5") {
        div = document.getElementsByClassName("ParcUnica");
        for (let i = 0; i < div.length; i++) {
            disp = div[i].style.display;
            if (disp == 'none') {
                div[i].style.display = 'block';
            }
        }
    }
    else {
        div = document.getElementsByClassName("ParcUnica");
        if (div.length > 0) {
            for (let i = 0; i < div.length; i++) {
                div[i].style.display = 'none';
            }
        }
    }
    if (v.toString() == "3") {
        div = document.getElementsByClassName("ParcComEntrada");
        for (let i = 0; i < div.length; i++) {
            disp = div[i].style.display;
            if (disp == 'none') {
                div[i].style.display = 'inline-flex';
            }
        }
    }
    else {
        div = document.getElementsByClassName("ParcComEntrada");
        if (div.length > 0) {
            for (let i = 0; i < div.length; i++) {
                div[i].style.display = 'none';
            }
        }
    }
    if (v.toString() == "2") {
        div = document.getElementsByClassName("PagtoCartaoInternet");
        for (let i = 0; i < div.length; i++) {
            disp = div[i].style.display;
            if (disp == 'none') {
                div[i].style.display = 'block';
            }
        }
    }
    else {
        div = document.getElementsByClassName("PagtoCartaoInternet");
        if (div.length > 0) {
            for (let i = 0; i < div.length; i++)
                div[i].style.display = 'none';
        }
    }
    if (v.toString() == "6") {
        div = document.getElementsByClassName("PagtoCartaoMaquineta");
        for (let i = 0; i < div.length; i++) {
            disp = div[i].style.display;
            if (disp == 'none') {
                div[i].style.display = 'block';
            }
        }
    }
    else {
        div = document.getElementsByClassName("PagtoCartaoMaquineta");
        if (div.length > 0) {
            for (let i = 0; i < div.length; i++)
                div[i].style.display = 'none';
        }
    }


}

$(".prod").click(function () {
    $(".prod input").prop('checked', false);
    $(this).find("input").prop('checked', true);
});

$("#btnModalProdutos").click(function () {
    AbrirModalProdutos();
    return true;
});

//buscando produtos
$("#buscaproduto").keyup(function () {
    let buscaProduto: string = $("#buscaproduto").val().toString();

    $("#tbody_modal tr").hide();
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
        $("#tbody_modal tr").show();
    }
});

$("#percComissao").keyup(() => {
    let valor = $("#percComissao").val().toString();
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    $("#percComissao").val(moedaUtils.formatarMoedaSemPrefixo(val));
});

window.VerificarPercMaxDescEComissao = (e: HTMLInputElement) => {
    let valor = e.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    e.value = moedaUtils.formatarMoedaSemPrefixo(val);

    let headerErro: string = "";
    let msgErro: string = "";
    let percMax = new PercentualMaximoDto()
    percMax = percentualMaximoDto;

    if (val < 0 || val > 100) {
        msgErro = "Percentual de comissão inválido.";
        swal(headerErro, msgErro);
        return false;
    }
    if (val > percentualMaximoDto.PercMaxComissao) {
        msgErro = "O percentual de comissão excede o máximo permitido.";
        swal(headerErro, msgErro);
        return false;
    }
}

//como é chamadado diretamente do HTML, tem que estar na window
window.InserirProdutoLinha = () => {
    let selectProdInfo = new SelectProdInfo();
    if ($("#qtde").val() != "" && $("#qtde").val() != "undefined") {

        //estou pegando a linha
        $("#tbody_modal").find('input:checked').parent().closest(".prod")
        //pegando os valores de fabricante e produto e qtde
        let fabricante: string = $("#tbody_modal").find('input:checked').parent().closest(".prod").children().find('input')[0].value;
        let produto: string = $("#tbody_modal").find('input:checked').parent().closest(".prod").children().find('input')[1].value;
        let qtde: number = parseInt($("#qtde").val().toString());

        selectProdInfo.Fabricante = fabricante;
        selectProdInfo.Produto = produto;
        selectProdInfo.Qte = qtde;

        itens.dtoProdutoCombo = lstprodutos;//pegando da tela
        itens.selectProdInfo = selectProdInfo;
        itens.selectProdInfo.ClicouOk = true;
        itens.pedidoDto = new PedidoDto();
        itens.mostrarProdutos(null);

        //estamos arrumando a qtde, verificando se existe e inserindo o produto
        arrumarProdsRepetidosTeste();
        //remover todas as linhas da tabela para adicionar novamente.
        removerTodosProdutos();

        //estamos passando o enum do dadosPagto para recalcular a lista de parcelamento
        RecalcularValoresSemCoeficiente(dadosPagto.enumFormaPagto, true, true);

        PedidoAlterado();

        //vamos limpar os campos da modal de produtos e limpar os checkbox
        $("#tbody_modal").find('input:checked').prop('checked', false);
        $("#qtde").val(1);
        $("#buscaproduto").val("");
        $("#tbody_modal tr").show();

        let headerErro: string = "Erro";
        if (itens.msgErro != "" && itens.msgErro != undefined) {
            swal(headerErro, itens.msgErro);
        }
    }
    else {
        return false;
    }
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

function removerTodosProdutos() {
    debugger;
    //let tbody: any = $(".novoProduto").parent();
    indice = Number($("#indice").val());
    let body_produtos: any = document.getElementById("body_produtos");
    let count = indice;
    for (count; count > 0; count--) {
        debugger;
        let tr: any = document.getElementsByClassName("trProduto_" + (count - 1));
        let qtde: any = tr.length;
        while (qtde != 0) {
            body_produtos.removeChild(tr[qtde -1]);
            qtde--;
        }
    }

    indice = 0;
    $("#indice").val(indice);
}

function AbrirModalParaRemoverProduto() {
    swal(
        {
            title: "Excluir Produto",
            text: "Tem certeza que deseja remover este produto?",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim, remove!",
            cancelButtonText: "Não, cancele!",
            closeOnConfirm: false,
            closeOnCancel: false
        },
        function (isConfirm) {
            if (isConfirm) {
                swal("Removido!", "Este produto foi removido", "success");
            }
        });
}

window.removerLinha = (v: HTMLElement) => {
    if (true) {
        let remove: boolean;
        AbrirModalParaRemoverProduto();

        //v = campo html
        let t = v as HTMLElement;
        //pegando o tr
        let linha: any = t.closest("tr");// t.parentElement.parentElement.parentElement.parentElement;
        let classeName = linha.className;
        debugger;
        $("." + classeName + "").remove();

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

    VerificarQtdeVendaPermitida(itens, produto[0], classeLinha);
    VerificarEstoque(itens, produto[0], classeLinha);

    //dadosPagto inicializar
    PedidoAlterado();

    itens.pedidoDto.ListaProdutos = lstProdSelecionados;

    zerarCamposDadosPagto(null);

    //aqui espera receber o enum que esta sendo usado
    RecalcularValoresSemCoeficiente(dadosPagto.enumFormaPagto, true, true);

    //passando para o valor para tela
    //linha.children[4].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(produto[0].VlUnitario);
    if (permiteRAStatus == 1) {
        linha.children[8].textContent = moedaUtils.formatarMoedaSemPrefixo(produto[0].TotalItem);

        somarRALiquido();
        somarRABruto();
    }
    else {
        linha.children[7].textContent = moedaUtils.formatarMoedaSemPrefixo(produto[0].TotalItem);
    }

    PedidoAlterado();//chamamos aqui para inicializar as variaveis
}

window.formatarDesc = (v: HTMLInputElement) => {
    let valor: string = v.value;
    let val: any = valor.replace(/\D/g, '');
    val = (val / 100).toFixed(2) + '';
    //pegar o valor para buscar na lista
    //let linha: any = v.parentElement.parentElement.parentElement;
    //let numProduto: string = linha.children[0].children[0].value;
    //let r: PedidoProdutosPedidoDto[] = new Array<PedidoProdutosPedidoDto>();
    //r = lstProdSelecionados.filter(e => e.NumProduto == numProduto);

    if (parseFloat(val) == 0 && val.length > 2) {
        val = "";
    }

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

    if (parseFloat(val) == 0 && val.length > 2) {
        val = r[0].VlUnitario.toString();
    }

    v.value = moedaUtils.formatarMoedaSemPrefixo(val);
}

window.formatarPreco_Lista = (v: HTMLInputElement) => {
    let valor: string = v.value;
    let val: string = valor.replace(/\D/g, '');
    val = (parseFloat(val) / 100).toFixed(2) + '';



    let linha: any = v.parentElement.parentElement.parentElement;
    let numProduto: string = linha.children[0].children[0].value;
    let r: PedidoProdutosPedidoDto[] = new Array<PedidoProdutosPedidoDto>();
    r = lstProdSelecionados.filter(e => e.NumProduto == numProduto);
    if (parseFloat(val) == 0 && val.length > 2) {
        val = r[0].Preco_Lista.toString();
    }

    v.value = moedaUtils.formatarMoedaSemPrefixo(parseFloat(val));
}

window.formatarVlEntrada = (v: HTMLInputElement) => {
    let valor = v.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    v.value = moedaUtils.formatarMoedaSemPrefixo(val);
}

window.digitouDesc = (v: HTMLInputElement) => {

    let valor: string = v.value;
    let val: any = valor.replace(/\D/g, '');
    val = (val / 100).toFixed(2) + '';

    //pegar o valor para buscar na lista
    let linha: any = v.parentElement.parentElement.parentElement;
    let numProduto: string = linha.children[0].value;
    let r: PedidoProdutosPedidoDto[] = new Array<PedidoProdutosPedidoDto>();
    r = lstProdSelecionados.filter(e => e.NumProduto == numProduto);
    r[0].Desconto = val;

    if (r[0].Desconto == 0 || r[0].Desconto.toString() == '') {

        r[0].AlterouVlVenda = false;
    } else {
        r[0].AlterouVlVenda = true;
    }

    //esta recalculando o desconto
    itens.digitouDescValor(r[0], val);

    dadosPagto.pedidoDto.ListaProdutos = lstProdSelecionados;

    dadosPagto.RecalcularListaProdutos();

    zerarCamposDadosPagto(null);

    RecalcularValoresSemCoeficiente(dadosPagto.enumFormaPagto, false, false);

    //precisamos recalcular os RA
    lstProdSelecionados = dadosPagto.pedidoDto.ListaProdutos;

    if (permiteRAStatus == 1) {
        linha.children[7].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);
        linha.children[8].textContent = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);
        linha.children[8].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlTotalItem);

        somarRALiquido();
        somarRABruto();
    }
    else {
        linha.children[6].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);
        linha.children[7].textContent = moedaUtils.formatarMoedaSemPrefixo(r[0].VlTotalItem);
        linha.children[7].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlTotalItem);
    }

    totalPedido();
    totalPedidoRA();
}

window.digitouVlVenda = (v: HTMLInputElement) => {

    let valor: string = v.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    v.value = moedaUtils.formatarMoedaSemPrefixo(val);

    let linha: any = v.parentElement.parentElement;
    let numProduto: string = linha.children[0].value;
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

    //verificar se permite RA
    if (permiteRAStatus == 1) {
        linha.children[6].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].Desconto);
        linha.children[7].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);
        linha.children[8].textContent = moedaUtils.formatarMoedaSemPrefixo(r[0].TotalItem);
    }
    else {
        linha.children[5].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].Desconto);
        linha.children[6].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);
        linha.children[7].textContent = moedaUtils.formatarMoedaSemPrefixo(r[0].TotalItem);

    }

    zerarCamposDadosPagto(true);
}

//vamos fazer um metodo para formatar o campo e outro metodo para recalcular os valores 
window.digitouVlEntrada = (v: HTMLInputElement) => {
    let valor = v.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    v.value = moedaUtils.formatarMoedaSemPrefixo(val);

    //dadosPagto.zerarTodosCampos();
    dadosPagto.vlEntrada = parseFloat(val);

    dadosPagto.recalcularValoresComCoeficiente(dadosPagto.enumFormaPagto);

    AtribuirListaParaSelect();
}

//PermiteRA
window.digitouPreco_Lista = (v: HTMLInputElement) => {
    let valor = v.value;
    let val: any = valor.replace(/\D/g, '');

    val = (val / 100).toFixed(2) + '';
    v.value = moedaUtils.formatarMoedaSemPrefixo(val);

    let linha: any = v.parentElement.parentElement.parentElement;
    let numProduto: string = linha.children[0].children[0].value;
    let r: PedidoProdutosPedidoDto[] = new Array<PedidoProdutosPedidoDto>();
    r = lstProdSelecionados.filter(e => e.NumProduto == numProduto);

    r[0].Preco_Lista = parseFloat(val);

    if (moedaUtils.formatarMoedaSemPrefixo(r[0].VlLista) === v.value) {
        r[0].AlterouValorRa = false;
    }
    else {
        r[0].AlterouValorRa = true;
    }

    PedidoAlterado();

    dadosPagto.pedidoDto.ListaProdutos = lstProdSelecionados;

    zerarCamposDadosPagto(null);

    RecalcularValoresSemCoeficiente(dadosPagto.enumFormaPagto, false, false);

    somarRALiquido();
    somarRABruto();

    //vamos somar o ra bruto

    itens.digitouQte(r[0]);

    //6 desconto / 7 vlunitario / 8 total

    linha.children[2].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].Preco_Lista);
    linha.children[6].children[0].children[0].value = moedaUtils.formatarPorcentagemUmaCasa(r[0].Desconto);
    linha.children[7].children[0].children[0].value = moedaUtils.formatarMoedaSemPrefixo(r[0].VlUnitario);
    linha.children[8].textContent = moedaUtils.formatarMoedaSemPrefixo(r[0].TotalItem);
}

function calcula_total_RA_liquido(percentual_desagio_RA_liquida: number, vl_total_RA: number): number {
    let vl_total_RA_liquido: number;
    if (vl_total_RA == 0 || vl_total_RA < 0) {
        vl_total_RA_liquido = 0.001;
    }
    else {
        vl_total_RA_liquido = vl_total_RA - (percentual_desagio_RA_liquida / 100) * vl_total_RA;
    }
    return vl_total_RA_liquido;
}

//fazer teste para calcular 0,00 nos RA
//Estamos verificando se temos produtos selecionados, caso não haja produtos, iremos verificar colocar 0,00 nos RA's
if (lstProdSelecionados.length == 0) {
    $('#totalValorRABruto').text(moedaUtils.formatarMoedaSemPrefixo(0.001));
    $('#totalValorRALiquido').text(moedaUtils.formatarMoedaSemPrefixo(0.001));
}

$('#totalValorRABruto').ready(() => {
    if (parseFloat($('#totalValorRABruto').text()) == 0 || $('#totalValorRABruto').text() == undefined) {
        $('#totalValorRABruto').text(moedaUtils.formatarMoedaSemPrefixo(0.001));
    }
})

function calcula_RA(vl_total_pedido: number, vl_total_RA: number): number {
    let retorno = vl_total_RA - vl_total_pedido;

    if (retorno == 0) {
        retorno = 0.001;
    }

    return retorno;
}

function somarRABruto(): void {
    let total: number = parseFloat(totalPedido().toFixed(2));
    let totalRa: number = parseFloat(totalPedidoRA().toFixed(2));

    $('#totalValorRABruto').text(moedaUtils.formatarMoedaSemPrefixo(
        calcula_RA(total, totalRa)));

    $('#totalValorRABrutoInput').val(moedaUtils.formatarMoedaSemPrefixo(
        calcula_RA(total, totalRa)));
}

function somarRALiquido(): void {
    let total = totalPedido();
    let totalRa = totalPedidoRA();

    let somaRA = parseFloat((totalRa - total).toFixed(2));

    $('#totalValorRALiquido').text(moedaUtils.formatarMoedaSemPrefixo(
        calcula_total_RA_liquido(Constantes.PERC_DESAGIO_RA_LIQUIDA, somaRA)));

    $('#totalValorRALiquidoInput').val(moedaUtils.formatarMoedaSemPrefixo(
        calcula_total_RA_liquido(Constantes.PERC_DESAGIO_RA_LIQUIDA, somaRA)));
}

function totalPedidoRA(): number {

    let totalRa = lstProdSelecionados.reduce((sum, current) => sum + current.Qtde * current.Preco_Lista, 0);

    $('#totalPedidoRABruto').text(moedaUtils.formatarMoedaSemPrefixo(totalRa));
    $("#totalPedidoRABrutoInput").val(moedaUtils.formatarMoedaSemPrefixo(totalRa));

    return totalRa;
}

//calculamos os produtos e somamos o total
function totalPedido(): number {
    debugger;
    let total: number = parseFloat(lstProdSelecionados.reduce((sum, current) => sum + current.TotalItem, 0).toFixed(2));

    $("#totalPedido").text(moedaUtils.formatarMoedaSemPrefixo(total));
    $("#totalPedidoInput").val(moedaUtils.formatarMoedaSemPrefixo(total));

    return total;
}

$('#diasVenc').keyup(function (this: any) {
    $(this).val(this.value.replace(/\D/g, ''));
});

$("#totalPedido").ready(() => {
    novoPedidoDadosService.pedidoDto = new PedidoDto();
    novoPedidoDadosService.pedidoDto.ListaProdutos = new Array<PedidoProdutosPedidoDto>();
    novoPedidoDadosService.pedidoDto.ListaProdutos = lstProdSelecionados;

    let totalP: string = $("#totalPedido").val().toString();

    if (totalP != "0") {
        $("#totalPedido").text(totalP);
    }
});

function InserirNovoProduto(produto: PedidoProdutosPedidoDto, editandoLinha: boolean, semVerificarServidor: boolean) {
    //esse é um exemplo de como clonar uma div e adicionar 
    //let novo = $(".novoProduto").clone();
    //novo.removeClass("novoProduto")
    //$(".novoProduto").parent().append(novo);

    let indice: number = null;

    indice = parseInt($("#indice").val().toString());

    if (indice.toString() == "NaN") {
        indice = 0;
        $("indice").val(indice);
    }

    let novaClasse: string = CopiarTRsMsgEProduto();
    //verificamos se mostraremos as msgs
    //chamar direto aqui
    if (editandoLinha && semVerificarServidor)
        verificarRegraProdutoCD(novaClasse, produto);

    InscreveLinhaProduto(produto, indice);

    indice++;
    $("#indice").val(indice);
}
//iremos inicializar as variaveis
function PedidoAlterado() {

    totalPedido();

    if (permiteRAStatus == 1) {
        totalPedidoRA();
    }

    InicializaDadosPagto();

    dadosPagto.tipoFormaPagto;
    dadosPagto.pedidoAlterado();
}

function zerarCamposDadosPagto(editandoLinha: boolean) {
    let selecione: any;

    //ParcCartaoInternet
    if (dadosPagto.enumFormaPagto == 2) {

        selecione = $("#opcaoPagtoParcCartaoInternet").children()[0];
        $("#opcaoPagtoParcCartaoInternet").html(selecione);
    }

    if (dadosPagto.enumFormaPagto == 3) {
    }

    if (dadosPagto.enumFormaPagto == 5) {
        //ParcUnica
        //$("#ParcUnica").css("display", "none");
        //selecione = $("#opcaoPagtoParcUnica").children()[0];
        //$("#opcaoPagtoParcUnica").html(selecione);
        //($("#opcaoPagtoParcUnica") as any).formSelect();

        //$("#meioPagtoParcUnica").prop('selectedIndex', 0);
        //($("#meioPagtoParcUnica") as any).formSelect();

        //$("#diasVencParcUnica").val('');
    }

    if (dadosPagto.enumFormaPagto == 6) {
        //ParcCartaoMaquineta
        //$("#PagtoCartaoMaquineta").css("display", "none");
        //selecione = $("#opcaoPagtoParcCartaoMaquineta").children()[0];
        //$("#opcaoPagtoParcCartaoMaquineta").html(selecione);
        //($("#opcaoPagtoParcCartaoMaquineta") as any).formSelect();
    }
}


function RecalcularValoresSemCoeficiente(v: any, editandoLinha: boolean, semVerificarServidor: boolean) {
    //zerarCamposDadosPagto(editandoLinha);
    InicializaDadosPagto();
    if (v == "" || v == undefined || v == null || v == 0) {
        v = 1;
    }

    zerarCamposDadosPagto(null);

    if (lstProdSelecionados.length > 0) {

        dadosPagto.enumFormaPagto = v;
        //estamos limpandos os campos
        //zerarCamposDadosPagto(editandoLinha);

        dadosPagto.qtdeParcVisa = qtdeParcVisa;
        dadosPagto.permiteRAStatus = permiteRAStatus;

        dadosPagto.recalcularValoresComCoeficiente(v);

        //teste para calcular com os produtos com coeficiente
        dadosPagto.RecalcularListaProdutos();
        lstProdSelecionados = dadosPagto.pedidoDto.ListaProdutos;

        let headerErro: string = "Erro";

        if (dadosPagto.msgErro != "" && dadosPagto.msgErro != undefined) {
            swal(headerErro, dadosPagto.msgErro);
            dadosPagto.msgErro = "";
            return false;
        }

        AtribuirListaParaSelect();
        //vamos apagar a linha de produtos selecionados e montar novamente com os valores atualizados
        //remover todas as linhas da tabela para adicionar novamente.

        if (editandoLinha) {
            removerTodosProdutos();
            lstProdSelecionados.forEach((value) => {
                InserirNovoProduto(value, editandoLinha, semVerificarServidor);
            });
        }

        //passando a lista recalculada
        lstProdSelecionados = dadosPagto.pedidoDto.ListaProdutos;

        //alterando o total do pedido        
        totalPedido();
        totalPedidoRA();
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
    novoProduto.css('background-color', "#f8f8ff");

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

// vamos alterar o select dos tipos de forma pagamento para utilizar radio
// todas as opções de pagamento ficarão visiveis a todo momento
// para isso iremos utilizar 2 blocos na mesma linha
// no bloco da esquerda iremos incluir os radio's com todos os tipos 
// no bloco da direita iremos deixar as respectivas div's escondidas e iremos mostrar apenas a que esta selecionada no radio
// ao alterar a seleção de radio iremos esconder todas e mostrar apenas a que esta selecionada
// SOBRE OS MÉTODOS
// iremos fazer um método para cada clique ou iremos passar o valor diretamenta 

window.recalcularValoresComCoeficiente = (e: HTMLInputElement) => {

    if (lstProdSelecionados.length == 0) {
        swal("Erro", "Selecione ao menos um produto!");
        return false;
    }

    zerarCamposDadosPagto(null);

    InicializaDadosPagto();
    let v = parseInt(e.value);

    inicializaCampos(v);
    //estou incluindo uma flag nesse metodo abaixo para não ir até a servidor para verificar, pois
    //estamos apenas recalculando com coeficiente
    RecalcularValoresSemCoeficiente(v, true, false);

    //á vista ou parc única
    if ((v == 1 || v == 5) && permiteRAStatus == 1) {
        somarRABruto();
        somarRALiquido();
    }
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

        removerTodosProdutos();
        lstProdSelecionados.forEach((value) => {
            InserirNovoProduto(value, false, false);
        });

        lstProdSelecionados = dadosPagto.pedidoDto.ListaProdutos;

        //alterando o total do pedido
        somarRALiquido();
        somarRABruto();
        totalPedido();
        totalPedidoRA();
    }

});


$("#opcaoPagtoParcComEntrada").on('change', () => {
    //recebe o componente html
    let valor: string = $("#opcaoPagtoParcComEntrada").val().toString();

    //vamos validar e descobrir a qtde de parcelas que esta sendo selecionado
    //validação feita em dadosPagto
    if (!!valor) {
        dadosPagto.opcaoPagtoParcComEntrada = valor;
        dadosPagto.RecalcularListaProdutos();

        removerTodosProdutos();
        lstProdSelecionados.forEach((value) => {
            InserirNovoProduto(value, false, false);
        });

        lstProdSelecionados = dadosPagto.pedidoDto.ListaProdutos;

        //alterando o total do pedido
        somarRALiquido();
        somarRABruto();
        totalPedido();
        totalPedidoRA();
    }
});


$("#opcaoPagtoParcCartaoMaquineta").on('change', () => {
    //recebe o componente html
    let valor: string = $("#opcaoPagtoParcCartaoMaquineta").val().toString();

    //vamos validar e descobrir a qtde de parcelas que esta sendo selecionado
    //validação feita em dadosPagto

    if (!!valor) {
        dadosPagto.opcaoPagtoParcCartaoMaquineta = valor;
        dadosPagto.RecalcularListaProdutos();

        removerTodosProdutos();
        lstProdSelecionados.forEach((value) => {
            InserirNovoProduto(value, false, false);
        });

        lstProdSelecionados = dadosPagto.pedidoDto.ListaProdutos;

        //alterando o total do pedido
        somarRALiquido();
        somarRABruto();
        totalPedido();
        totalPedidoRA();
    }
});

function AtribuirListaParaSelect() {

    if (dadosPagto.enumFormaPagto == 1) {
        //A vista   
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoAvista").append("<option selected>" + value + "</option>");
        });
    }
    if (dadosPagto.enumFormaPagto == 2) {
        //ParcCartaoInternet
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoParcCartaoInternet").append("<option>" + value + "</option>");
        });
    }
    if (dadosPagto.enumFormaPagto == 3) {
        //ParcComEnt
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoParcComEntrada").append("<option>" + value + "</option>");
        });
    }
    //NÃO ESTA SENDO USADO
    if (dadosPagto.enumFormaPagto == 4) {
        //ParcSemEnt
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoParcSemEntrada").append("<option>" + value + "</option>");
        });
    }

    if (dadosPagto.enumFormaPagto == 5) {
        //ParcUnica
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoParcUnica").append("<option selected>" + value + "</option>");
        });
    }
    if (dadosPagto.enumFormaPagto == 6) {
        //ParcCartaoMaquineta
        dadosPagto.lstMsg.forEach((value) => {
            $("#opcaoPagtoParcCartaoMaquineta").append("<option>" + value + "</option>");
        });
    }

}

function InicializaDadosPagto() {
    dadosPagto.coeficienteDto = new Array<CoeficienteDto>();//lista de coeficiente
    dadosPagto.pedidoDto = new PedidoDto();//Pedido
    dadosPagto.pedidoDto.ListaProdutos = new Array<PedidoProdutosPedidoDto>();//lista de produtos selecionados
    dadosPagto.moedaUtils = new MoedaUtils();
    dadosPagto.lstMsg = new Array();

    dadosPagto.coeficienteDto = lstCoeficiente;//recebe a lista que veio do servidor
    dadosPagto.pedidoDto.ListaProdutos = lstProdSelecionados;//recebe lst dos produtos selcionados   
}

function InscreveLinhaProduto(produto: PedidoProdutosPedidoDto, index: any) {
    console.log(index);
    let texto: any = $("<div></div>");
    texto.html(produto.Descricao);

    $('[name="[' + index + '].NumProduto"]').text(produto.Fabricante + "/" + produto.NumProduto + " - " + texto.text());
    $('[name="[' + index + '].NumProduto"]').val(produto.NumProduto);//campo type hidden = passar para model
    $('[name="[' + index + '].Fabricante"]').val(produto.Fabricante);
    $('[name="[' + index + '].Qtde"]').val(produto.Qtde);
    $('[name="[' + index + '].Preco_Lista"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.Preco_Lista));
    $('[name="[' + index + '].VlLista"]').text(moedaUtils.formatarMoedaSemPrefixo(produto.VlLista));
    $('[name="[' + index + '].VlLista"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.VlLista));
    $('[name="[' + index + '].Preco"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.Preco));

    $('[name="[' + index + '].Desconto"]').val(moedaUtils.formatarMoedaSemPrefixo(produto.Desconto));

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

function verificarRegraProdutoCD(novaclasse: string, produto: PedidoProdutosPedidoDto) {

    if (!!produto.NumProduto && !!produto.Fabricante) {
        $.ajax({
            url: "../Produtos/VerificarRegraProdutoCD/",
            type: "GET",
            data: { produto: JSON.stringify(produto), id_nfe_emitente_selecao_manual: cdSelecionadoId },
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
declare var tipoCliente: string;

//afazer: atribuir os tipos para as variaveis de percentual maximo e desconto
function obtem_perc_comissao_e_desconto_a_utilizar(): number {
    //receber a forma de pagto
    dadosPagto.enumFormaPagto;
    //receber o tipo do cliente
    tipoCliente;
    //recebe  meiosPagtoPreferenciais
    meiosPagtoPreferenciais
    //total do pedido
    let total: any = totalPedido();

    let vMPN2 = new Array();

    vMPN2 = meiosPagtoPreferenciais.Campo_texto.split(',');

    //é para verificar o tipo do meio de pagamento

    //pagto avista
    if (dadosPagto.enumFormaPagto.toString() == Constantes.COD_FORMA_PAGTO_A_VISTA) {
        if (dadosPagto.meioPagtoAVista.toString() == "") {
            return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoPJ :
                percentualMaximoDto.PercMaxComissaoEDesconto);
        }
        vMPN2.forEach((e) => {
            if (dadosPagto.meioPagtoAVista.toString() == e.toString()) {

                return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoNivel2PJ :
                    percentualMaximoDto.PercMaxComissaoEDescontoNivel2);
            }
        });

        return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoPJ :
            percentualMaximoDto.PercMaxComissaoEDesconto);
    }

    //Parcela ùnica
    if (dadosPagto.enumFormaPagto.toString() == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA) {
        if (dadosPagto.meioPagtoParcUnica.toString() == "") {
            return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoPJ :
                percentualMaximoDto.PercMaxComissaoEDesconto);
        }

        vMPN2.forEach((e) => {
            if (dadosPagto.meioPagtoParcUnica.toString() == e.toString()) {
                return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoNivel2PJ :
                    percentualMaximoDto.PercMaxComissaoEDescontoNivel2);
            }
        });

        return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoPJ :
            percentualMaximoDto.PercMaxComissaoEDesconto);
    }

    //Parcelado no Cartão (internet)
    if (dadosPagto.enumFormaPagto.toString() == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO) {
        vMPN2.forEach((e) => {
            if (Constantes.ID_FORMA_PAGTO_CARTAO == e.toString()) {
                return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoNivel2PJ :
                    percentualMaximoDto.PercMaxComissaoEDescontoNivel2);
            }
        });

        return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoPJ :
            percentualMaximoDto.PercMaxComissaoEDesconto);
    }

    //Parcelado no Cartão (maquineta)
    if (dadosPagto.enumFormaPagto.toString() == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA) {
        vMPN2.forEach((e) => {
            if (Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA == e.toString()) {
                return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoNivel2PJ :
                    percentualMaximoDto.PercMaxComissaoEDescontoNivel2);
            }
        });

        return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoPJ :
            percentualMaximoDto.PercMaxComissaoEDesconto);
    }

    let blnPreferencial: boolean = false;
    let vlNivel2: number;
    let vlNivel1: number;
    //Parcelado Com Entrada
    if (dadosPagto.enumFormaPagto.toString() == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA) {
        vMPN2.forEach((e) => {
            if (dadosPagto.meioPagtoEntrada.toString() == e.toString()) {
                blnPreferencial = true;
                return;
            }
        });

        if (blnPreferencial) {
            vlNivel2 = dadosPagto.vlEntrada;
        }
        else {
            vlNivel1 = dadosPagto.vlEntrada;
        }

        blnPreferencial = false;
        vMPN2.forEach((e) => {
            if (dadosPagto.meioPagtoEntrada.toString() == e.toString()) {
                blnPreferencial = true;
                return;
            }
        });

        if (blnPreferencial) {
            vlNivel2 += dadosPagto.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_valor *
                dadosPagto.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_qtde;
        }
        else {
            vlNivel1 += dadosPagto.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_valor *
                dadosPagto.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_qtde;
        }

        if (vlNivel2 > (totalPedido() / 2)) {
            return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoNivel2PJ :
                percentualMaximoDto.PercMaxComissaoEDescontoNivel2);
        }
        //	O meio de pagamento não é preferencial
        return (tipoCliente == Constantes.ID_PJ ? percentualMaximoDto.PercMaxComissaoEDescontoPJ :
            percentualMaximoDto.PercMaxComissaoEDesconto);
    }

    //Parcela sem entrada 

}

if (percComissao.toString() != "") {
    let valor: string = percComissao.toString();
    let val: any = valor.replace(/\D/g, '');
    val = (val * 1).toFixed(2) + '';

    $("#perComissao").text(moedaUtils.formatarMoedaSemPrefixo(val));
}

window.formataPercentual = () => {
    let valor: string = percComissao.toString();
    let val: any = valor.replace(/\D/g, '');
    val = (val / 100).toFixed(2) + '';

    $("#percComissao").text(val);
}

//iremos fazer a validação na tela
window.continuar = (): any => {
    //verificar se tem produtos com qtde maior que o permitido
    let q: number = 0;

    let headerErro: string = "Erro";

    if (dadosPagto.pedidoDto == null || dadosPagto.pedidoDto == undefined) {
        swal(headerErro, "Selecione ao menos um produto para continuar.");
        return false;
    } else {
        dadosPagto.pedidoDto.ListaProdutos.forEach(r => {
            if (itens.qtdeVendaPermitida(r)) {
                q++;
            }
        });

        if (q > 0) {
            swal(headerErro, "Produtos com quantidades maiores que a quantidade máxima permitida para venda!");
            return false;
        }
        //validação: tem que ter algum produto
        if (dadosPagto.pedidoDto.ListaProdutos.length === 0) {
            swal(headerErro, "Selecione ao menos um produto para continuar.");
            return false;
        }

        AtribuirFormaPagtoParaDadosPagto();
        //verifica se a forma de pgamento tem algum aviso
        if (!dadosPagto.podeContinuar(true)) {
            //verificar se a forma de pagamento está preenchida
            //necessário atribuir a forma de pagmento para dadosPagto

            if (dadosPagto.msgErro != "") {
                swal(headerErro, dadosPagto.msgErro);
                return false;
            }
            return;
        } else {

            //afazer: incluir campos que estão faltando e verificar a forma como é feito o calculo de RA do Pedido
            //será necessário alterar variavel no Back, pois estamos calculando o valor de RA mesmo quando não tem, 
            //sendo assim, a variavel que esta sendo verificada esta errada, pois aplicamos uma alteração na forma como é 
            //feito os cálculos de RA

            //Também será necessário fazer a busca de percentual e trazer na viewModel da tela para poder fazer a validação
            //quando inserimos um produto e permite RA, isso não esta sendo feito corretamente, precisamos corrigir
            //incluir os campos de RA liquido e RA bruto na parte da lista de produtos, pois quando Permite_RA
            //iremos calcular isso e mostrar na tela, vai dar trabalho... Preciso fazer de uma forma que isso não cause impacto
            //no que já esta pronto e funcionando
            if (dadosPagto.pedidoDto.PermiteRAStatus == 1) {
                //esse this.percentualVlPedidoRA ainda não existe
                if (this.percentualVlPedidoRA != 0 && this.percentualVlPedidoRA != undefined) {

                    let vlAux = (this.percentualVlPedidoRA / 100) * this.totalPedido();
                    if (this.somaRA > vlAux) {
                        swal(headerErro, "O valor total de RA excede o limite permitido!");
                        return;
                    }
                }
            }

            let perc_max_comissao_e_desconto_a_utilizar = obtem_perc_comissao_e_desconto_a_utilizar();

            let perc_desc_medio = calcula_desconto_medio();
            // Verifica se todos os produtos cujo desconto excedem o máximo permitido possuem senha de desconto disponível
            //afazer: essa var deverá ser inclusa na tela para mostrar que teve alteração no percentual escolhido na tela anterior
            perc_RT_novo = verifica_excedente_max_desconto(perc_max_comissao_e_desconto_a_utilizar, perc_desc_medio);

            if (erroPercNovo) {
                erroPercNovo = false;
                return false;
            }

            //linha 2230 pedidonovoconsiste
            //não vamos fazer pois é sobre a tela de observações

            //aqui finaliza - tudo ok!
            //vamos tentar retornar o objeto serializado para a tela mandar para o controller
            dadosPagto.novoPedidoDadosService.pedidoDto.FormaPagtoCriacao = dadosPagto.pedidoDto.FormaPagtoCriacao;
            dadosPagto.novoPedidoDadosService.pedidoDto.ListaProdutos = dadosPagto.pedidoDto.ListaProdutos;

            $("#lstProdutosSelecionados").val(itens.pedidoDto.ListaProdutos.toString());
            //criar metodo para passar os campos para FormaPagtoCriacao
            if (AtribuirValoresFormaPagtoCriacao()) {
                return true;
            }
        }
    }
}

function verifica_excedente_max_desconto(perc_max_comissao_e_desconto_a_utilizar: number, perc_desc_medio: number): number {
    let perc_senha_desconto: number = 0;
    let vl_preco_lista: number = 0;
    let vl_preco_venda: number = 0;
    let perc_desc: number = 0;

    let headerErro: string = "Erro";
    let msgErro: string = "";

    lstProdSelecionados.forEach((e) => {
        if (e.NumProduto != "") {
            perc_senha_desconto = 0;
            vl_preco_lista = e.Preco_Lista;
            vl_preco_venda = e.VlUnitario;
            if (vl_preco_lista == 0) {
                perc_desc = 0;
            }
            else {
                perc_desc = 100 * (vl_preco_lista - vl_preco_venda) / vl_preco_lista;
            }
            // Tem desconto: sim
            if (perc_desc != 0) {
                // Desconto excede limite máximo: sim
                if (perc_desc > perc_max_comissao_e_desconto_a_utilizar) {
                    // Tem senha de desconto?
                    if (listaObjetoSenhaDesconto.length > 0) {
                        listaObjetoSenhaDesconto.forEach((i) => {
                            if (i.Fabricante == e.Fabricante && i.Produto == e.NumProduto) {
                                perc_senha_desconto = i.Desc_Max;
                            }
                        });
                    }

                    // Tem senha de desconto: sim
                    if (perc_senha_desconto != 0) {
                        // Senha de desconto NÃO cobre desconto
                        if (perc_senha_desconto < perc_desc) {
                            //vamos armazenar uma lista de erros para verificar na hora que estiver validando
                            msgErro = "O desconto do produto '" + e.Descricao + "' (" +
                                perc_desc + " %) excede o máximo autorizado!";
                            erroPercNovo = true;
                            swal(headerErro, msgErro);
                            return;

                        }
                    }
                    // Não tem senha de desconto
                    else {
                        msgErro = "O desconto do produto '" + e.Descricao + "' (" +
                            moedaUtils.formatarMoedaSemPrefixo(perc_desc) + "%) excede o máximo permitido!";
                        erroPercNovo = true;
                        swal(headerErro, msgErro);
                        return;
                    }

                }
            }
        }
    });
    //não é possivel continuar    
    if (erroPercNovo) {
        return;
    }

    let perc_max_RT = percentualMaximoDto.PercMaxComissao;
    // Tem RT: sim
    if (percComissao != 0) {
        // RT excede limite máximo?
        if (percComissao > perc_max_RT) {
            msgErro = "Percentual de comissão excede o máximo permitido!";
            swal(headerErro, msgErro);
            return;
        }

        // Neste ponto, é certo que todos os produtos que possuem desconto estão dentro do máximo permitido
        // ou possuem senha de desconto autorizando.
        // Verifica-se agora se é necessário reduzir automaticamente o percentual da RT usando p/ o cálculo
        // o percentual de desconto médio.

        perc_RT_novo = Math.min(percComissao, (perc_max_comissao_e_desconto_a_utilizar - perc_desc_medio));
        if (perc_RT_novo < 0) perc_RT_novo = 0;

        // O percentual de RT será alterado automaticamente, solicita confirmação
        if (perc_RT_novo != percComissao) {
            //afazer: peciso fazer uma modal de confirmar para esperar o retorno disso
            let s = "A soma dos percentuais de comissão (" + moedaUtils.formatarMoedaSemPrefixo(percComissao) +
                "%) e de desconto médio do(s) produto(s) (" + moedaUtils.formatarMoedaSemPrefixo(perc_desc_medio) + "%) totaliza " +
                moedaUtils.formatarMoedaSemPrefixo((perc_desc_medio + percComissao)) + "% e excede o máximo permitido!!" +
                "\nA comissão será reduzida automaticamente para " + moedaUtils.formatarMoedaSemPrefixo(perc_RT_novo) + "%!!" +
                "\nContinua?";
            //aqui vamos ter que verificar se foi confirmado alterar automatico
            if (!confirm(s)) {
                s = "Operação cancelada!!";
                alert(s);
                erroPercNovo = true;

                return;
            }
            else {
                // Novo percentual de RT
                //f.c_perc_RT.value = formata_perc_RT(perc_RT_novo);
                //* OBS: não estamos mostrando nessa tela, verificar para deixar a com% visivel

                //aqui vamos alterar o desconto
                $("#perComissao").text(moedaUtils.formatarMoedaSemPrefixo(perc_RT_novo));
                $("#percComissao").val(parseFloat(moedaUtils.formatarMoedaSemPrefixo(perc_RT_novo)));
                return perc_RT_novo;
            }
        }
    }
}

function calcula_desconto_medio(): number {
    let vl_total_preco_lista: number = 0;
    let vl_total_preco_venda: number = 0;
    let perc_desc_medio: number = 0;


    lstProdSelecionados.forEach((e) => {
        if (e.NumProduto != "") {
            vl_total_preco_lista += e.Qtde * e.Preco_Lista;
            vl_total_preco_venda += e.Qtde * e.VlUnitario;
        }
    });
    if (vl_total_preco_lista == 0) {
        perc_desc_medio = 0;
    }
    else {
        perc_desc_medio = 100 * (vl_total_preco_lista - vl_total_preco_venda) / vl_total_preco_lista;
    }
    return perc_desc_medio;
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

