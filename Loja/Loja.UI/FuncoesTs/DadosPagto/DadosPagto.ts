﻿import { EnumFormaPagto } from "./EnumFormaPagto";
import { Constantes } from "../../UtilTs/Constantes/Constantes";
import { EnumTipoPagto } from "./EnumTipoPagto";
import { MoedaUtils } from "../../UtilTs/MoedaUtils/moedaUtils";
import { NovoPedidoDadosService } from "../../Services/NovoPepedidoDadosService";
import { RecalcularComCoeficiente } from "../RecalcularComCoeficiente/RecalcularComCoeficiente";
import { PedidoDto } from "../../DtosTs/PedidoDto/PedidoDto";
import { CoeficienteDto } from "../../DtosTs/CoeficienteDto/CoeficienteDto";
import { FormaPagtoDto } from "../../DtosTs/FormaPagtoDto/FormaPagtoDto";
import { ErrorModal } from "../../Views/Shared/Error";

export class DadosPagto {
    public enumFormaPagto: EnumFormaPagto;
    //para usar o enum 
    public EnumFormaPagto = EnumFormaPagto;

    public criando = true;
    public pedidoDto: PedidoDto;


    public mascaraNum() {
        return [/\d/, /\d/, /\d/];
    }

    public msgErro: string;

    public podeContinuar(mostrarMsg: boolean): boolean {
        if (this.enumFormaPagto == 0) {

            //necessário verificar se os dados de pagto estão corretamente preenchido
            if (mostrarMsg) {
                //afazer: mostrar a modal de mensagem
                //this.alertaService.mostrarMensagem("Favor escolher uma forma de pagamento!");
                this.msgErro = "Favor escolher uma forma de pagamento!"
            }
        }
        else if (!this.validarFormaPagto(mostrarMsg)) {
            return false;
        }
        else {
            //passar valores para o dto
            this.atribuirFormaPagtoParaDto();
            return true;
        }
        return false;
    }

    public opcaoPagtoAvista: string;
    public opcaoPagtoParcUnica: string;
    public opcaoPagtoParcComEntrada: string;
    public opcaoPagtoParcCartaoInternet: string;
    public opcaoPagtoParcCartaoMaquineta: string;
    public meioPagtoEntrada: number;
    public meioPagtoAVista: number;
    public meioPagtoEntradaPrest: number;
    public diasVenc: number;//pagto com entrada, dias para vencimento
    public meioPagtoParcUnica: number;
    public diasVencParcUnica: number;
    qtde: number;//qtde de parcelas
    valor: number;//valor da parcela

    atribuirFormaPagtoParaDto() {
        this.pedidoDto.FormaPagtoCriacao.Tipo_parcelamento = this.enumFormaPagto;
        this.pedidoDto.VlTotalDestePedido = this.totalPedido();

        if (this.enumFormaPagto == 1) {
            //A vista      

            this.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
            this.pedidoDto.FormaPagtoCriacao.Op_av_forma_pagto = this.meioPagtoAVista.toString();//meio de pagamento
            this.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas = 1;
        }
        if (this.enumFormaPagto == 2) {
            //ParcCartaoInternet
            this.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
            this.pedidoDto.FormaPagtoCriacao.C_pc_qtde = this.qtde; //passar a qtde de parcelas para uma variávl "qtdeParcelas"
            this.pedidoDto.FormaPagtoCriacao.C_pc_valor = this.valor;
            this.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas = this.qtde;
        }
        if (this.enumFormaPagto == 3) {
            //ParcComEnt
            this.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
            this.pedidoDto.FormaPagtoCriacao.Op_pce_entrada_forma_pagto = this.meioPagtoEntrada.toString();//meio de pagamento
            this.pedidoDto.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = this.meioPagtoEntradaPrest.toString();//meio de pagamento
            this.pedidoDto.FormaPagtoCriacao.C_pce_entrada_valor = this.vlEntrada;
            this.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_qtde = this.qtde;
            this.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_valor = this.valor;
            this.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_periodo = this.diasVenc != null ?
                parseInt(this.diasVenc.toString().replace("_", "")) : this.diasVenc;
            this.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas = this.qtde + 1;//c_pce_prestacao_qtde + 1
        }
        //NÃO ESTA SENDO USADO
        if (this.enumFormaPagto == 4) {
            //ParcSemEnt
            this.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
            this.pedidoDto.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto = "";//meio de pagamento
            this.pedidoDto.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto = "";//meio de pagamento
            this.pedidoDto.FormaPagtoCriacao.C_pse_prim_prest_valor = 0;
            this.pedidoDto.FormaPagtoCriacao.C_pse_prim_prest_apos = 0;
            this.pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
            this.pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_valor = 0;
            this.pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_periodo = 0;
            this.pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
        }
        if (this.enumFormaPagto == 5) {
            //ParcUnica
            this.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
            this.pedidoDto.FormaPagtoCriacao.Op_pu_forma_pagto = this.meioPagtoParcUnica.toString();//meio de pagamento
            this.pedidoDto.FormaPagtoCriacao.C_pu_valor = this.valor;
            this.pedidoDto.FormaPagtoCriacao.C_pu_vencto_apos = this.diasVencParcUnica != null ?
                parseInt(this.diasVencParcUnica.toString().replace("_", "")) : this.diasVencParcUnica;
            this.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas = 1;
        }
        if (this.enumFormaPagto == 6) {
            //ParcCartaoMaquineta
            this.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
            this.pedidoDto.FormaPagtoCriacao.C_pc_maquineta_qtde = this.qtde;
            this.pedidoDto.FormaPagtoCriacao.C_pc_maquineta_valor = this.valor;
            this.pedidoDto.FormaPagtoCriacao.C_pc_maquineta_qtde = this.qtde;
            this.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas = this.qtde;
        }

    }
    erroModal = new ErrorModal();
    validarFormaPagto(mostrarMsg: boolean): boolean {
        let retorno = true;
        // this.verificaParcelamento();
        if (this.validarOpcoesPagto(mostrarMsg)) {
            if (this.enumFormaPagto == 1 && !this.meioPagtoAVista)//avista
            {
                if (mostrarMsg) {
                    //afazer: mostrar a modal de mensagem
                    //this.alertaService.mostrarMensagem("Favor selecionar o meio de pagamento á vista");
                    this.msgErro = "Favor selecionar o meio de pagamento á vista";
                    this.erroModal.MostrarMsg(this.msgErro);
                    retorno = false;
                }
            }
            if (this.enumFormaPagto == 2 && (!this.qtde || !this.valor))//ParcCartaoInternet
            {
                if (mostrarMsg) {
                    //afazer: mostrar a modal de mensagem
                    this.msgErro = "Favor selecionar corretamente o meio de pagamento " +
                        "para Parcelado no Cartão (Internet).";
                    this.erroModal.MostrarMsg(this.msgErro);
                    retorno = false;
                }
            }
            if (this.enumFormaPagto == 3) {
                if (this.meioPagtoEntrada && this.meioPagtoEntradaPrest)
                //ParcComEnt
                {
                    if (!!this.vlEntrada && this.vlEntrada == 0.00) {
                        //afazer: mostrar a modal de mensagem
                        this.msgErro = "Favor preencher o valor de entrada!";
                        this.erroModal.MostrarMsg(this.msgErro);
                        retorno = false;
                    }
                    if (this.meioPagtoEntradaPrest != 5 && this.meioPagtoEntradaPrest != 7 && !this.diasVenc) {
                        if (mostrarMsg) {
                            //afazer: mostrar a modal de mensagem
                            this.msgErro = "Favor informar corretamente os dados para pagamento Parcelado com Entrada!";
                            this.erroModal.MostrarMsg(this.msgErro);
                            retorno = false;
                        }
                    }
                }
                else {
                    if (mostrarMsg) {
                        //afazer: mostrar a modal de mensagem
                        this.msgErro = "Favor informar corretamente os dados para pagamento Parcelado com Entrada!";
                        this.erroModal.MostrarMsg(this.msgErro);
                        retorno = false;
                    }
                }
            }
            if (this.enumFormaPagto == 4)//ParcSemEnt
            {
                retorno = false;
            }

            if (this.enumFormaPagto == 5 &&
                (!this.meioPagtoParcUnica || !this.diasVencParcUnica))//ParcUnica
            {
                if (mostrarMsg) {
                    //afazer: mostrar a modal de mensagem
                    this.msgErro = "Favor informar corretamente os dados para pagamento Parcela Única!";
                    this.erroModal.MostrarMsg(this.msgErro);
                    retorno = false;
                }
            }
            if (this.enumFormaPagto == 6 && (!this.qtde || !this.valor))//ParcCartaoMaquineta
            {
                if (mostrarMsg) {
                    //afazer: mostrar a modal de mensagem
                    this.msgErro = "Favor selecionar  corretamente o meio de pagamento " +
                        "para Parcelado no Cartão (Maquineta).";
                    this.erroModal.MostrarMsg(this.msgErro);
                    retorno = false;
                }
            }
            return retorno;
        }
    }

    validarOpcoesPagto(mostrarMsg: boolean): boolean {
        if (this.enumFormaPagto) {
            if (this.enumFormaPagto == 1) {
                if (!this.opcaoPagtoAvista) {
                    if (mostrarMsg) {
                        //afazer: mostrar a modal de mensagem
                        this.msgErro = "Favor selecionar o campo Parcelamento para À vista!";
                        this.erroModal.MostrarMsg(this.msgErro);
                    }
                    return false;
                }
                else {
                    //pegamos a qtde e o valor
                    this.verificaParcelamento(this.opcaoPagtoAvista);
                    return true;
                }
            }
            if (this.enumFormaPagto == 2) {
                
                if (!this.opcaoPagtoParcCartaoInternet) {
                    if (mostrarMsg) {
                        //afazer: mostrar a modal de mensagem
                        this.msgErro = "Favor selecionar o Parcelamento para Cartão (Internet)!";
                        this.erroModal.MostrarMsg(this.msgErro);
                    }
                    return false;
                }
                else {
                    this.verificaParcelamento(this.opcaoPagtoParcCartaoInternet);
                    return true;
                }
            }
            if (this.enumFormaPagto == 3) {
                if (this.vlEntrada) {
                    if (!this.opcaoPagtoParcComEntrada) {
                        if (mostrarMsg) {
                            //afazer: mostrar a modal de mensagem
                            this.msgErro = "Favor selecionar o Parcelamento para Parcela com entrada!";
                            this.erroModal.MostrarMsg(this.msgErro);
                        }
                        return false;
                    }
                    else {
                        this.verificaParcelamento(this.opcaoPagtoParcComEntrada);
                        return true;
                    }
                }
                else {
                    if (mostrarMsg) {
                        //afazer: mostrar a modal de mensagem
                        this.msgErro = "Favor informar o valor de entrada!";
                        this.erroModal.MostrarMsg(this.msgErro);
                    }
                    return false;
                }
            }
            if (this.enumFormaPagto == 4) {
                //ParcSemEntrada
            }
            if (this.enumFormaPagto == 5) {
                if (!this.opcaoPagtoParcUnica) {
                    if (mostrarMsg) {
                        //afazer: mostrar a modal de mensagem
                        this.msgErro = "Favor selecionar o Parcelamento para Parcela Única!";
                        this.erroModal.MostrarMsg(this.msgErro);
                    }
                    return false;
                }
                else {
                    this.verificaParcelamento(this.opcaoPagtoParcUnica);
                    return true;
                }
            }
            if (this.enumFormaPagto == 6) {
                if (!this.opcaoPagtoParcCartaoMaquineta) {
                    if (mostrarMsg) {
                        //afazer: mostrar a modal de mensagem
                        this.msgErro = "Favor selecionar o Parcelamento para Pagamento com cartão (Maquineta)!";
                        this.erroModal.MostrarMsg(this.msgErro);
                    }
                    return false;
                }
                else {
                    this.verificaParcelamento(this.opcaoPagtoParcCartaoMaquineta);
                    return true;
                }
            }
        }
        else {
            if (mostrarMsg) {
                //afazer: mostrar a modal de mensagem
                this.msgErro = "Favor selecionar uma Forma de Pagamento!";
                this.erroModal.MostrarMsg(this.msgErro);
            }
            return false;
        }
    }

    verificaParcelamento(opcaoPagto: string) {
        if (!!opcaoPagto) {
            // this.qtde = parseInt(opcaoPagto.substring(0, 1));
            this.qtde = parseInt(opcaoPagto.slice(0, 2).trim());
            // this.valor = parseInt(this.opcaoPagto.replace('.', '').substring(6));
            //correção para não perder as casas decimais
            this.valor = parseFloat(opcaoPagto.substring(7).trim().replace('.', '').replace(',', '.'));
        }
    }

    formaPagtoDto: FormaPagtoDto;

    //Método esta funcionando
    public coeficienteDto: CoeficienteDto[];


    // foi solicitado que a qtde de parcelas disponível será baseada na
    // qtde de parcelas disponível no cartão Visa(PRAZO_LOJA)
    //então faremos a busca pela API
    qtdeParcVisa: number;
    permiteRAStatus: number;

    //chamado quando algum item do prepedido for alterado
    //aqui é feito a limpeza do select da forma de pagamento
    public pedidoAlterado() {
        this.recalcularValoresComCoeficiente(this.enumFormaPagto);
        this.montarListaParcelamento(this.enumFormaPagto);
        this.opcaoPagtoParcComEntrada = null;
        this.opcaoPagtoAvista = null;
        this.opcaoPagtoParcCartaoInternet = null;
        this.opcaoPagtoParcCartaoMaquineta = null;
        this.opcaoPagtoParcUnica = null;
    }

    
    lstMsg: string[] = [];
    tipoFormaPagto: string = '';
    coeficienteDtoNovo: CoeficienteDto[][];
    recalcularComCoeficiente = new RecalcularComCoeficiente();
    formaPagtoNum: number;
    public recalcularValoresComCoeficiente(enumFP: number): void {
        //na mudança da forma de pagto iremos zerar todos os campos
        //this.zerarTodosCampos();

        if (!!enumFP) {
            this.formaPagtoNum = enumFP;

            //verificar EnumTipoPagto para passar o valor do tipo "AV, SE, CE"
            this.tipoFormaPagto = this.verificaEnum(enumFP);
            //aisamos que está carregando...
            this.lstMsg = new Array();
            this.lstMsg.push("Carregando dados....");

            //aqui vamos filtrar a lista de coeficientes para trazer apenas as informações 
            //referentes aos produtos que estão inseridos
            this.recalcularComCoeficiente.permiteRAStatus = this.permiteRAStatus;
            this.recalcularComCoeficiente.pedidoDto = this.pedidoDto;
            this.recalcularComCoeficiente.lstProdutos = this.pedidoDto.ListaProdutos;
            this.recalcularComCoeficiente.lstCoeficientesCompleta = this.coeficienteDto;
            this.coeficienteDtoNovo = this.recalcularComCoeficiente.buscarCoeficienteFornecedores();
            this.lstMsg = new Array();
            this.lstMsg = this.recalcularComCoeficiente.CalcularTotalProdutosComCoeficiente(this.formaPagtoNum, this.coeficienteDtoNovo,
                this.tipoFormaPagto, this.qtdeParcVisa, this.vlEntrada);
            
            if (this.formaPagtoNum.toString() == Constantes.COD_FORMA_PAGTO_A_VISTA) {
                this.recalcularComCoeficiente.RecalcularListaProdutos(this.formaPagtoNum, this.coeficienteDtoNovo, this.tipoFormaPagto, 1);
                this.opcaoPagtoAvista = this.lstMsg[0];
            }
            if (this.formaPagtoNum.toString() == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA) {
                this.recalcularComCoeficiente.RecalcularListaProdutos(this.formaPagtoNum, this.coeficienteDtoNovo, this.tipoFormaPagto, 1);
                this.opcaoPagtoParcUnica = this.lstMsg[0];
            }

            //esse metodo esta buscandi no servidor;
            //this.listarValoresComCoeficiente(this.tipoFormaPagto, enumFP);
        }
    }

    RecalcularListaProdutos() {
        //vamos validar e descobrir a qtde de parcelas que esta sendo selecionado
        if (!this.validarFormaPagto(false)) {
            return false;
        }
        this.recalcularComCoeficiente.RecalcularListaProdutos(this.formaPagtoNum, this.coeficienteDtoNovo, this.tipoFormaPagto,
            this.qtde);

        this.pedidoDto.ListaProdutos = this.recalcularComCoeficiente.lstProdutos;
    }

    public zerarTodosCampos(): void {
        this.vlEntrada = null;
        this.meioPagtoEntrada = null;
        this.opcaoPagtoAvista = "";
        this.meioPagtoAVista = null;
        this.opcaoPagtoParcUnica = "";
        this.meioPagtoParcUnica = null;
        this.diasVencParcUnica = null;
        this.opcaoPagtoParcComEntrada = "";
        this.meioPagtoEntradaPrest = null;
        this.diasVenc = null;
        this.opcaoPagtoParcCartaoInternet = "";
        this.opcaoPagtoParcCartaoMaquineta = "";
    }

    enumTipoFP = EnumTipoPagto;
    verificaEnum(enumFP: number) {
        if (enumFP == EnumFormaPagto.Avista)
            return this.enumTipoFP.Avista.toString();
        else if (enumFP == EnumFormaPagto.ParcCartaoInternet)
            return this.enumTipoFP.ParcCartaoInternet.toString();
        else if (enumFP == EnumFormaPagto.ParcComEnt)
            return this.enumTipoFP.ParcComEnt.toString();
        else if (enumFP == EnumFormaPagto.ParcSemEnt)
            return this.enumTipoFP.ParcSemEnt.toString();
        else if (enumFP == EnumFormaPagto.ParcUnica)
            return this.enumTipoFP.ParcUnica.toString();
        else if (enumFP == EnumFormaPagto.ParcCartaoMaquineta)
            return this.enumTipoFP.ParcCartaoMaquineta.toString();
    }
    public novoPedidoDadosService: NovoPedidoDadosService;

    public totalPedido(): number {
        return this.pedidoDto.ListaProdutos.reduce((sum, current) => sum + current.TotalItem, 0);
    }

    listarValoresComCoeficiente(txtFormaPagto: string, enumFP: number): void {
        
        //this.lstMsg
        let valor = 0
        // this.lstMsg = new Array();
        //pegar o valor do item e multiplicar com o coeficiente
        let lstCoeficiente = this.coeficienteDto;
        //afazer: verificar esse serviço
        //this.pedidoDto = this.novoPedidoDadosService.dtoPedido;
        let lstProdutos = this.pedidoDto.ListaProdutos;
        //utilizamos essa lista quando for diferente de A vista
        let lstCoefFiltro: CoeficienteDto[];

        if (this.enumFormaPagto) {
            for (var x = 0; x < lstProdutos.length; x++) {
                //teste para filtrar a lista de coeficiente
                if (this.tipoFormaPagto != "AV") {

                    lstCoefFiltro = lstCoeficiente.filter(e => e.Fabricante == lstProdutos[x].Fabricante &&
                        e.TipoParcela == this.tipoFormaPagto);
                }

                for (var i = 0; i < this.qtdeParcVisa; i++) {
                    //avista o coeficiente é 1, sendo assim não faz alteração nos valores
                    // verificar os o enum de formaPagto 
                    if (this.enumFormaPagto.toString() == Constantes.COD_FORMA_PAGTO_A_VISTA &&
                        lstProdutos[x].Fabricante == lstCoeficiente[i].Fabricante &&
                        txtFormaPagto == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA) {
                        lstProdutos[x].VlUnitario = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
                        lstProdutos[x].VlTotalItem = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
                        lstProdutos[x].VlLista = lstProdutos[x].Preco;
                        lstProdutos[x].TotalItem = (lstProdutos[x].Preco * lstProdutos[x].Qtde) * (1 - lstProdutos[x].Desconto / 100);
                        //this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' + (lstProdutos[x].TotalItem).toFixed(2));            
                        break;
                    }
                    else if (this.enumFormaPagto.toString() == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA &&
                        lstProdutos[x].Fabricante == lstCoefFiltro[i].Fabricante &&
                        lstCoefFiltro[i].TipoParcela == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
                        txtFormaPagto == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
                        lstProdutos[x].VlUnitario = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
                        lstProdutos[x].VlTotalItem = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
                        lstProdutos[x].VlLista = lstProdutos[x].Preco;
                        lstProdutos[x].TotalItem = (lstProdutos[x].Preco * lstProdutos[x].Qtde) * (1 - lstProdutos[x].Desconto / 100);
                        //this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' + (lstProdutos[x].TotalItem).toFixed(2));
                        break;
                    }
                    else if (this.enumFormaPagto.toString() == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA &&
                        lstProdutos[x].Fabricante == lstCoefFiltro[i].Fabricante &&
                        lstCoefFiltro[i].TipoParcela == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
                        txtFormaPagto == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {
                        
                        lstProdutos[x].VlUnitario = ((lstProdutos[x].Preco * lstCoefFiltro[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100));
                        lstProdutos[x].VlTotalItem = (lstProdutos[x].Preco * lstCoefFiltro[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
                        lstProdutos[x].VlLista = lstProdutos[x].Preco * lstCoefFiltro[i].Coeficiente;
                        lstProdutos[x].TotalItem = ((lstProdutos[x].Preco * lstProdutos[x].Qtde) * lstCoefFiltro[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
                        //this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' +
                        // (lstProdutos[x].TotalItem / lstCoeficiente[i].QtdeParcelas).toFixed(2));
                    }
                    //parcelado sem entrada
                    if ((this.enumFormaPagto.toString() == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA ||
                        this.enumFormaPagto.toString() == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO ||
                        this.enumFormaPagto.toString() == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA) &&
                        lstProdutos[x].Fabricante == lstCoefFiltro[i].Fabricante &&
                        lstCoefFiltro[i].TipoParcela == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
                        txtFormaPagto == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
                        lstProdutos[x].VlUnitario = ((lstProdutos[x].Preco * lstCoefFiltro[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100));
                        lstProdutos[x].VlTotalItem = (lstProdutos[x].Preco * lstCoefFiltro[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
                        lstProdutos[x].VlLista = lstProdutos[x].Preco * lstCoefFiltro[i].Coeficiente;
                        lstProdutos[x].TotalItem = ((lstProdutos[x].Preco * lstProdutos[x].Qtde) * lstCoefFiltro[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
                        // this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' +
                        //   (lstProdutos[x].TotalItem / lstCoeficiente[i].QtdeParcelas).toFixed(2));
                    }
                }
            }
        }
        this.montarListaParcelamento(this.enumFormaPagto);
        // return this.lstMsg;
    }

    

    montarListaParcelamento(enumFP: number): string[] {
        this.lstMsg = new Array();
        let lstCoeficiente = this.coeficienteDto;
        let vlTotalPedido = this.pedidoDto.ListaProdutos.reduce((sum, prod) => sum + prod.TotalItem, 0);
        let cont = 0;
        if (enumFP) {
            for (let i = 0; i < lstCoeficiente.length; i++) {
                if (enumFP.toString() == Constantes.COD_FORMA_PAGTO_A_VISTA) {
                    this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + " X " +
                        this.moedaUtils.formatarMoedaComPrefixo(vlTotalPedido / lstCoeficiente[i].QtdeParcelas));
                    this.opcaoPagtoAvista = this.lstMsg[i];
                    break;
                }
                else if (enumFP.toString() == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA &&
                    lstCoeficiente[i].TipoParcela == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
                    this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + " X " +
                        this.moedaUtils.formatarMoedaComPrefixo(vlTotalPedido / lstCoeficiente[i].QtdeParcelas));
                    this.opcaoPagtoParcUnica = this.lstMsg[0];
                    break;
                }
                else if (enumFP.toString() == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA &&
                    lstCoeficiente[i].TipoParcela == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {
                    if (cont < this.qtdeParcVisa) {
                        this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + " X " +
                            this.moedaUtils.formatarMoedaComPrefixo(((vlTotalPedido - this.vlEntrada) / lstCoeficiente[i].QtdeParcelas)));
                        cont++;
                    }
                }
                else if ((enumFP.toString() == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA ||
                    enumFP.toString() == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO ||
                    enumFP.toString() == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA) &&
                    lstCoeficiente[i].TipoParcela == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
                    if (cont < this.qtdeParcVisa) {
                        cont++;
                        this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + " X " +
                            this.moedaUtils.formatarMoedaComPrefixo(vlTotalPedido / lstCoeficiente[i].QtdeParcelas));
                    }
                }
            }
        }
        return this.lstMsg;
    }

    public vlEntrada: number;
    moedaUtils = new MoedaUtils();
    calcularParcelaComEntrada() {

        this.opcaoPagtoParcComEntrada = "";

        this.lstMsg = new Array();
        //afazer: verificar esse serviço
        //this.pedidoDto = this.novoPedidoDadosService.dtoPedido;
        let lstProdutos = this.pedidoDto.ListaProdutos;
        let lstCoeficiente = this.coeficienteDto;
        if (!!this.vlEntrada && this.vlEntrada != 0.00) {
            var vltotal = this.pedidoDto.ListaProdutos.reduce((sum, current) => sum + current.TotalItem, 0);

            if (this.vlEntrada > vltotal) {
                //afazer: mostrar a modal de mensagem
                this.msgErro = "Valor da entrada é maior que o total do Pré-pedido!";

                this.vlEntrada = null;
            }
            else {
                vltotal = vltotal - this.vlEntrada;
                for (var i = 0; i < this.qtdeParcVisa; i++) {
                    if (lstCoeficiente[i].TipoParcela == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {
                        this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + " X " +
                            this.moedaUtils.formatarMoedaComPrefixo((vltotal / lstCoeficiente[i].QtdeParcelas)));
                    }
                }


            }
            return this.lstMsg;
        }

    }

    // digitouVlEntrada
    digitouVlEntrada(e: Event) {

        let valor = ((e.target) as HTMLInputElement).value;
        let v: any = valor.replace(/\D/g, '');
        v = (v / 100).toFixed(2) + '';
        this.vlEntrada = v;
        this.calcularParcelaComEntrada();
    }

    montaFormaPagtoExistente() {

        if (this.pedidoDto.FormaPagtoCriacao.Tipo_parcelamento) {
            this.pedidoDto.FormaPagtoCriacao.Tipo_parcelamento;

            switch (this.pedidoDto.FormaPagtoCriacao.Tipo_parcelamento.toString()) {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    //A vista
                    this.enumFormaPagto = EnumFormaPagto.Avista;//forma de pagamento
                    this.meioPagtoAVista = parseInt(this.pedidoDto.FormaPagtoCriacao.Op_av_forma_pagto);//deposito ou...
                    this.opcaoPagtoAvista = this.montaParcelamentoExistente();//recebe a descrição (1 X R$ 00,00)
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    //ParcUnica
                    this.enumFormaPagto = EnumFormaPagto.ParcUnica;//forma de pagamento
                    this.meioPagtoParcUnica = parseInt(this.pedidoDto.FormaPagtoCriacao.Op_pu_forma_pagto);//deposito ou...
                    this.opcaoPagtoParcUnica = this.montaParcelamentoExistente();//recebe a descrição (1 X R$ 00,00)
                    this.diasVencParcUnica = this.pedidoDto.FormaPagtoCriacao.C_pu_vencto_apos;//dias para venc.
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    //ParcCartaoInternet
                    this.enumFormaPagto = EnumFormaPagto.ParcCartaoInternet;//forma de pagamento
                    this.opcaoPagtoParcCartaoInternet = this.montaParcelamentoExistente();//recebe a descrição (1 X R$ 00,00)
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    //ParcCartaoMaquineta
                    this.enumFormaPagto = EnumFormaPagto.ParcCartaoMaquineta;//forma de pagamento
                    this.opcaoPagtoParcCartaoMaquineta = this.montaParcelamentoExistente();//recebe a descrição (1 X R$ 00,00)
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    //ParcComEnt
                    this.enumFormaPagto = EnumFormaPagto.ParcComEnt;//forma de pagamento
                    this.vlEntrada = this.pedidoDto.FormaPagtoCriacao.C_pce_entrada_valor;//valor de entrada
                    this.meioPagtoEntrada = parseInt(this.pedidoDto.FormaPagtoCriacao.Op_pce_entrada_forma_pagto);//deposito ou...
                    this.meioPagtoEntradaPrest = parseInt(this.pedidoDto.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto);//deposito ou...
                    this.opcaoPagtoParcComEntrada = this.montaParcelamentoExistente();//recebe a descrição (1 X R$ 00,00)
                    this.diasVenc = this.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_periodo;//recebe os dias de vencimento
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    //ParcSemEnt
                    this.pedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
                    this.pedidoDto.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto = "";//meio de pagamento
                    this.pedidoDto.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto = "";//meio de pagamento
                    this.pedidoDto.FormaPagtoCriacao.C_pse_prim_prest_valor = 0;
                    this.pedidoDto.FormaPagtoCriacao.C_pse_prim_prest_apos = 0;
                    this.pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
                    this.pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_valor = 0;
                    this.pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_periodo = 0;
                    this.pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
                    break;
            };
        }
    }

    //metodo para montar o tipo de parcelamento que foi selecionado pelo usuário
    montaParcelamentoExistente(): string {
        let retorno = "";
        this.pedidoDto.FormaPagtoCriacao.Tipo_parcelamento;
        this.recalcularValoresComCoeficiente(this.pedidoDto.FormaPagtoCriacao.Tipo_parcelamento);

        switch (this.pedidoDto.FormaPagtoCriacao.Tipo_parcelamento.toString()) {
            case Constantes.COD_FORMA_PAGTO_A_VISTA:
                retorno = this.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas + " X " +
                    this.moedaUtils.formatarMoedaComPrefixo(this.pedidoDto.VlTotalDestePedido);
                break;
            case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                //ParcUnica
                retorno = this.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas + " X " +
                    this.moedaUtils.formatarMoedaComPrefixo(this.pedidoDto.FormaPagtoCriacao.C_pu_valor);
                break;
            case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                //ParcCartaoInternet
                retorno = this.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas + " X " +
                    this.moedaUtils.formatarMoedaComPrefixo(this.pedidoDto.FormaPagtoCriacao.C_pc_valor);
                break;
            case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                //ParcCartaoMaquineta
                retorno = this.pedidoDto.FormaPagtoCriacao.Qtde_Parcelas + " X " +
                    this.moedaUtils.formatarMoedaComPrefixo(this.pedidoDto.FormaPagtoCriacao.C_pc_maquineta_valor);
                break;
            case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                //ParcComEnt
                retorno = this.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_qtde + " X " +
                    this.moedaUtils.formatarMoedaComPrefixo(this.pedidoDto.FormaPagtoCriacao.C_pce_prestacao_valor);
                break;
            case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                //ParcSemEnt
                break;
        };

        return retorno;
    }




}