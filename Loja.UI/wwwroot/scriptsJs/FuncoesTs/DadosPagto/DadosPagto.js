/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
define(["require", "exports", "./EnumFormaPagto", "../../UtilTs/Constantes/Constantes", "./EnumTipoPagto", "../../UtilTs/MoedaUtils/moedaUtils"], function (require, exports, EnumFormaPagto_1, Constantes_1, EnumTipoPagto_1, moedaUtils_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var DadosPagto = /** @class */ (function () {
        function DadosPagto() {
            //para usar o enum 
            this.EnumFormaPagto = EnumFormaPagto_1.EnumFormaPagto;
            this.criando = true;
            this.constantes = new Constantes_1.Constantes();
            this.lstMsg = [];
            this.tipoFormaPagto = '';
            this.enumTipoFP = EnumTipoPagto_1.EnumTipoPagto;
            this.moedaUtils = new moedaUtils_1.MoedaUtils();
        }
        DadosPagto.prototype.mascaraNum = function () {
            return [/\d/, /\d/, /\d/];
        };
        DadosPagto.prototype.podeContinuar = function (mostrarMsg) {
            if (this.enumFormaPagto == 0) {
                //necessário verificar se os dados de pagto estão corretamente preenchido
                if (mostrarMsg) {
                    //afazer: mostrar a modal de mensagem
                    //this.alertaService.mostrarMensagem("Favor escolher uma forma de pagamento!");
                    this.msgErro = "Favor escolher uma forma de pagamento!";
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
        };
        DadosPagto.prototype.atribuirFormaPagtoParaDto = function () {
            this.dtoPedido.FormaPagtoCriacao.Tipo_parcelamento = this.enumFormaPagto;
            this.dtoPedido.VlTotalDestePedido = this.novoPedidoDadosService.totalPedido();
            if (this.enumFormaPagto == 1) {
                //A vista      
                this.dtoPedido.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
                this.dtoPedido.FormaPagtoCriacao.Op_av_forma_pagto = this.meioPagtoAVista.toString(); //meio de pagamento
                this.dtoPedido.FormaPagtoCriacao.Qtde_Parcelas = 1;
            }
            if (this.enumFormaPagto == 2) {
                //ParcCartaoInternet
                this.dtoPedido.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
                this.dtoPedido.FormaPagtoCriacao.C_pc_qtde = this.qtde; //passar a qtde de parcelas para uma variávl "qtdeParcelas"
                this.dtoPedido.FormaPagtoCriacao.C_pc_valor = this.valor;
                this.dtoPedido.FormaPagtoCriacao.Qtde_Parcelas = this.qtde;
            }
            if (this.enumFormaPagto == 3) {
                //ParcComEnt
                this.dtoPedido.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
                this.dtoPedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto = this.meioPagtoEntrada.toString(); //meio de pagamento
                this.dtoPedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = this.meioPagtoEntradaPrest.toString(); //meio de pagamento
                this.dtoPedido.FormaPagtoCriacao.C_pce_entrada_valor = this.vlEntrada;
                this.dtoPedido.FormaPagtoCriacao.C_pce_prestacao_qtde = this.qtde;
                this.dtoPedido.FormaPagtoCriacao.C_pce_prestacao_valor = this.valor;
                this.dtoPedido.FormaPagtoCriacao.C_pce_prestacao_periodo = this.diasVenc != null ?
                    parseInt(this.diasVenc.toString().replace("_", "")) : this.diasVenc;
                this.dtoPedido.FormaPagtoCriacao.Qtde_Parcelas = this.qtde + 1; //c_pce_prestacao_qtde + 1
            }
            //NÃO ESTA SENDO USADO
            if (this.enumFormaPagto == 4) {
                //ParcSemEnt
                this.dtoPedido.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
                this.dtoPedido.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto = ""; //meio de pagamento
                this.dtoPedido.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto = ""; //meio de pagamento
                this.dtoPedido.FormaPagtoCriacao.C_pse_prim_prest_valor = 0;
                this.dtoPedido.FormaPagtoCriacao.C_pse_prim_prest_apos = 0;
                this.dtoPedido.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
                this.dtoPedido.FormaPagtoCriacao.C_pse_demais_prest_valor = 0;
                this.dtoPedido.FormaPagtoCriacao.C_pse_demais_prest_periodo = 0;
                this.dtoPedido.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
            }
            if (this.enumFormaPagto == 5) {
                //ParcUnica
                this.dtoPedido.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
                this.dtoPedido.FormaPagtoCriacao.Op_pu_forma_pagto = this.meioPagtoParcUnica.toString(); //meio de pagamento
                this.dtoPedido.FormaPagtoCriacao.C_pu_valor = this.valor;
                this.dtoPedido.FormaPagtoCriacao.C_pu_vencto_apos = this.diasVencParcUnica != null ?
                    parseInt(this.diasVencParcUnica.toString().replace("_", "")) : this.diasVencParcUnica;
                this.dtoPedido.FormaPagtoCriacao.Qtde_Parcelas = 1;
            }
            if (this.enumFormaPagto == 6) {
                //ParcCartaoMaquineta
                this.dtoPedido.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
                this.dtoPedido.FormaPagtoCriacao.C_pc_maquineta_qtde = this.qtde;
                this.dtoPedido.FormaPagtoCriacao.C_pc_maquineta_valor = this.valor;
                this.dtoPedido.FormaPagtoCriacao.C_pc_maquineta_qtde = this.qtde;
                this.dtoPedido.FormaPagtoCriacao.Qtde_Parcelas = this.qtde;
            }
        };
        DadosPagto.prototype.validarFormaPagto = function (mostrarMsg) {
            var retorno = true;
            // this.verificaParcelamento();
            if (this.validarOpcoesPagto(mostrarMsg)) {
                if (this.enumFormaPagto == 1 && !this.meioPagtoAVista) //avista
                 {
                    if (mostrarMsg) {
                        //afazer: mostrar a modal de mensagem
                        //this.alertaService.mostrarMensagem("Favor selecionar o meio de pagamento á vista");
                        this.msgErro = "Favor selecionar o meio de pagamento á vista";
                        retorno = false;
                    }
                }
                if (this.enumFormaPagto == 2 && (!this.qtde || !this.valor)) //ParcCartaoInternet
                 {
                    if (mostrarMsg) {
                        //afazer: mostrar a modal de mensagem
                        this.msgErro = "Favor selecionar corretamente o meio de pagamento " +
                            "para Parcelado no Cartão (Internet).";
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
                            retorno = false;
                        }
                        if (this.meioPagtoEntradaPrest != 5 && this.meioPagtoEntradaPrest != 7 && !this.diasVenc) {
                            if (mostrarMsg) {
                                //afazer: mostrar a modal de mensagem
                                this.msgErro = "Favor informar corretamente os dados para pagamento Parcelado com Entrada!";
                                retorno = false;
                            }
                        }
                    }
                    else {
                        if (mostrarMsg) {
                            //afazer: mostrar a modal de mensagem
                            this.msgErro = "Favor informar corretamente os dados para pagamento Parcelado com Entrada!";
                            retorno = false;
                        }
                    }
                }
                if (this.enumFormaPagto == 4) //ParcSemEnt
                 {
                    retorno = false;
                }
                if (this.enumFormaPagto == 5 &&
                    (!this.meioPagtoParcUnica || !this.diasVencParcUnica)) //ParcUnica
                 {
                    if (mostrarMsg) {
                        //afazer: mostrar a modal de mensagem
                        this.msgErro = "Favor informar corretamente os dados para pagamento Parcela Única!";
                        retorno = false;
                    }
                }
                if (this.enumFormaPagto == 6 && (!this.qtde || !this.valor)) //ParcCartaoMaquineta
                 {
                    if (mostrarMsg) {
                        //afazer: mostrar a modal de mensagem
                        this.msgErro = "Favor selecionar  corretamente o meio de pagamento " +
                            "para Parcelado no Cartão (Maquineta).";
                        retorno = false;
                    }
                }
                return retorno;
            }
        };
        DadosPagto.prototype.validarOpcoesPagto = function (mostrarMsg) {
            if (this.enumFormaPagto) {
                if (this.enumFormaPagto == 1) {
                    if (!this.opcaoPagtoAvista) {
                        if (mostrarMsg) {
                            //afazer: mostrar a modal de mensagem
                            this.msgErro = "Favor selecionar o campo Parcelamento para À vista!";
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
                }
                return false;
            }
        };
        DadosPagto.prototype.verificaParcelamento = function (opcaoPagto) {
            if (!!opcaoPagto) {
                // this.qtde = parseInt(opcaoPagto.substring(0, 1));
                this.qtde = parseInt(opcaoPagto.slice(0, 2).trim());
                // this.valor = parseInt(this.opcaoPagto.replace('.', '').substring(6));
                //correção para não perder as casas decimais
                this.valor = parseFloat(opcaoPagto.substring(7).trim().replace('.', '').replace(',', '.'));
            }
        };
        //chamado quando algum item do prepedido for alterado
        //aqui é feito a limpeza do select da forma de pagamento
        DadosPagto.prototype.pedidoAlterado = function () {
            this.recalcularValoresComCoeficiente(this.enumFormaPagto);
            this.montarListaParcelamento(this.enumFormaPagto);
            this.opcaoPagtoParcComEntrada = null;
            this.opcaoPagtoAvista = null;
            this.opcaoPagtoParcCartaoInternet = null;
            this.opcaoPagtoParcCartaoMaquineta = null;
            this.opcaoPagtoParcUnica = null;
        };
        DadosPagto.prototype.recalcularValoresComCoeficiente = function (enumFP) {
            //na mudança da forma de pagto iremos zerar todos os campos
            this.zerarTodosCampos();
            //verificar EnumTipoPagto para passar o valor do tipo "AV, SE, CE"
            this.tipoFormaPagto = this.verificaEnum(enumFP);
            //aisamos que está carregando...
            this.lstMsg = new Array();
            this.lstMsg.push("Carregando dados....");
            //esse metodo esta buscandi no servidor;
            this.listarValoresComCoeficiente(this.tipoFormaPagto, enumFP);
        };
        DadosPagto.prototype.zerarTodosCampos = function () {
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
        };
        DadosPagto.prototype.verificaEnum = function (enumFP) {
            if (enumFP == EnumFormaPagto_1.EnumFormaPagto.Avista)
                return this.enumTipoFP.Avista.toString();
            else if (enumFP == EnumFormaPagto_1.EnumFormaPagto.ParcCartaoInternet)
                return this.enumTipoFP.ParcCartaoInternet.toString();
            else if (enumFP == EnumFormaPagto_1.EnumFormaPagto.ParcComEnt)
                return this.enumTipoFP.ParcComEnt.toString();
            else if (enumFP == EnumFormaPagto_1.EnumFormaPagto.ParcSemEnt)
                return this.enumTipoFP.ParcSemEnt.toString();
            else if (enumFP == EnumFormaPagto_1.EnumFormaPagto.ParcUnica)
                return this.enumTipoFP.ParcUnica.toString();
            else if (enumFP == EnumFormaPagto_1.EnumFormaPagto.ParcCartaoMaquineta)
                return this.enumTipoFP.ParcCartaoMaquineta.toString();
        };
        DadosPagto.prototype.listarValoresComCoeficiente = function (txtFormaPagto, enumFP) {
            var _this = this;
            //this.lstMsg
            var valor = 0;
            // this.lstMsg = new Array();
            //pegar o valor do item e multiplicar com o coeficiente
            var lstCoeficiente = this.coeficienteDto;
            //afazer: verificar esse serviço
            //this.dtoPedido = this.novoPedidoDadosService.dtoPedido;
            var lstProdutos = this.dtoPedido.ListaProdutos;
            var constant = this.constantes;
            //utilizamos essa lista quando for diferente de A vista
            var lstCoefFiltro;
            if (this.enumFormaPagto) {
                for (var x = 0; x < lstProdutos.length; x++) {
                    //teste para filtrar a lista de coeficiente
                    if (this.tipoFormaPagto != "AV") {
                        lstCoefFiltro = lstCoeficiente.filter(function (e) { return e.Fabricante == lstProdutos[x].Fabricante &&
                            e.TipoParcela == _this.tipoFormaPagto; });
                    }
                    for (var i = 0; i < this.qtdeParcVisa; i++) {
                        //avista o coeficiente é 1, sendo assim não faz alteração nos valores
                        // verificar os o enum de formaPagto 
                        if (this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_A_VISTA &&
                            lstProdutos[x].Fabricante == lstCoeficiente[i].Fabricante &&
                            txtFormaPagto == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA) {
                            lstProdutos[x].VlUnitario = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
                            lstProdutos[x].VlTotalItem = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
                            lstProdutos[x].VlLista = lstProdutos[x].Preco;
                            lstProdutos[x].TotalItem = (lstProdutos[x].Preco * lstProdutos[x].Qtde) * (1 - lstProdutos[x].Desconto / 100);
                            //this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' + (lstProdutos[x].TotalItem).toFixed(2));            
                            break;
                        }
                        else if (this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELA_UNICA &&
                            lstProdutos[x].Fabricante == lstCoefFiltro[i].Fabricante &&
                            lstCoefFiltro[i].TipoParcela == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
                            txtFormaPagto == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
                            lstProdutos[x].VlUnitario = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
                            lstProdutos[x].VlTotalItem = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
                            lstProdutos[x].VlLista = lstProdutos[x].Preco;
                            lstProdutos[x].TotalItem = (lstProdutos[x].Preco * lstProdutos[x].Qtde) * (1 - lstProdutos[x].Desconto / 100);
                            //this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' + (lstProdutos[x].TotalItem).toFixed(2));
                            break;
                        }
                        else if (this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA &&
                            lstProdutos[x].Fabricante == lstCoefFiltro[i].Fabricante &&
                            lstCoefFiltro[i].TipoParcela == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
                            txtFormaPagto == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {
                            //debugger;
                            lstProdutos[x].VlUnitario = ((lstProdutos[x].Preco * lstCoefFiltro[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100));
                            lstProdutos[x].VlTotalItem = (lstProdutos[x].Preco * lstCoefFiltro[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
                            lstProdutos[x].VlLista = lstProdutos[x].Preco * lstCoefFiltro[i].Coeficiente;
                            lstProdutos[x].TotalItem = ((lstProdutos[x].Preco * lstProdutos[x].Qtde) * lstCoefFiltro[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
                            //this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' +
                            // (lstProdutos[x].TotalItem / lstCoeficiente[i].QtdeParcelas).toFixed(2));
                        }
                        //parcelado sem entrada
                        if ((this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA ||
                            this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELADO_CARTAO ||
                            this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA) &&
                            lstProdutos[x].Fabricante == lstCoefFiltro[i].Fabricante &&
                            lstCoefFiltro[i].TipoParcela == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
                            txtFormaPagto == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
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
            this.montarListaParcelamento(enumFP);
            // return this.lstMsg;
        };
        DadosPagto.prototype.montarListaParcelamento = function (enumFP) {
            this.lstMsg = new Array();
            var lstCoeficiente = this.coeficienteDto;
            var vlTotalPedido = this.dtoPedido.ListaProdutos.reduce(function (sum, prod) { return sum + prod.TotalItem; }, 0);
            var cont = 0;
            if (enumFP) {
                for (var i = 0; i < lstCoeficiente.length; i++) {
                    if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_A_VISTA) {
                        this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + " X " +
                            this.moedaUtils.formatarMoedaComPrefixo(vlTotalPedido / lstCoeficiente[i].QtdeParcelas));
                        this.opcaoPagtoAvista = this.lstMsg[i];
                        break;
                    }
                    else if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELA_UNICA &&
                        lstCoeficiente[i].TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
                        this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + " X " +
                            this.moedaUtils.formatarMoedaComPrefixo(vlTotalPedido / lstCoeficiente[i].QtdeParcelas));
                        this.opcaoPagtoParcUnica = this.lstMsg[0];
                        break;
                    }
                    else if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA &&
                        lstCoeficiente[i].TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {
                        if (cont < this.qtdeParcVisa) {
                            this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + " X " +
                                this.moedaUtils.formatarMoedaComPrefixo(((vlTotalPedido - this.vlEntrada) / lstCoeficiente[i].QtdeParcelas)));
                            cont++;
                        }
                    }
                    else if ((enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA ||
                        enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO ||
                        enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA) &&
                        lstCoeficiente[i].TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
                        if (cont < this.qtdeParcVisa) {
                            cont++;
                            this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + " X " +
                                this.moedaUtils.formatarMoedaComPrefixo(vlTotalPedido / lstCoeficiente[i].QtdeParcelas));
                        }
                    }
                }
            }
            return this.lstMsg;
        };
        DadosPagto.prototype.calcularParcelaComEntrada = function () {
            this.opcaoPagtoParcComEntrada = "";
            this.lstMsg = new Array();
            //afazer: verificar esse serviço
            //this.dtoPedido = this.novoPedidoDadosService.dtoPedido;
            var lstProdutos = this.dtoPedido.ListaProdutos;
            var lstCoeficiente = this.coeficienteDto;
            if (!!this.vlEntrada && this.vlEntrada != 0.00) {
                var vltotal = this.dtoPedido.ListaProdutos.reduce(function (sum, current) { return sum + current.TotalItem; }, 0);
                if (this.vlEntrada > vltotal) {
                    debugger;
                    //afazer: mostrar a modal de mensagem
                    this.msgErro = "Valor da entrada é maior que o total do Pré-pedido!";
                    this.vlEntrada = null;
                    debugger;
                }
                else {
                    debugger;
                    vltotal = vltotal - this.vlEntrada;
                    for (var i = 0; i < this.qtdeParcVisa; i++) {
                        if (lstCoeficiente[i].TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {
                            this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + " X " +
                                this.moedaUtils.formatarMoedaComPrefixo((vltotal / lstCoeficiente[i].QtdeParcelas)));
                        }
                    }
                }
                debugger;
                return this.lstMsg;
            }
        };
        // digitouVlEntrada
        DadosPagto.prototype.digitouVlEntrada = function (e) {
            var valor = (e.target).value;
            var v = valor.replace(/\D/g, '');
            v = (v / 100).toFixed(2) + '';
            this.vlEntrada = v;
            this.calcularParcelaComEntrada();
        };
        DadosPagto.prototype.montaFormaPagtoExistente = function () {
            if (this.dtoPedido.FormaPagtoCriacao.Tipo_parcelamento) {
                this.dtoPedido.FormaPagtoCriacao.Tipo_parcelamento;
                switch (this.dtoPedido.FormaPagtoCriacao.Tipo_parcelamento.toString()) {
                    case this.constantes.COD_FORMA_PAGTO_A_VISTA:
                        //A vista
                        this.enumFormaPagto = EnumFormaPagto_1.EnumFormaPagto.Avista; //forma de pagamento
                        this.meioPagtoAVista = parseInt(this.dtoPedido.FormaPagtoCriacao.Op_av_forma_pagto); //deposito ou...
                        this.opcaoPagtoAvista = this.montaParcelamentoExistente(); //recebe a descrição (1 X R$ 00,00)
                        break;
                    case this.constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                        //ParcUnica
                        this.enumFormaPagto = EnumFormaPagto_1.EnumFormaPagto.ParcUnica; //forma de pagamento
                        this.meioPagtoParcUnica = parseInt(this.dtoPedido.FormaPagtoCriacao.Op_pu_forma_pagto); //deposito ou...
                        this.opcaoPagtoParcUnica = this.montaParcelamentoExistente(); //recebe a descrição (1 X R$ 00,00)
                        this.diasVencParcUnica = this.dtoPedido.FormaPagtoCriacao.C_pu_vencto_apos; //dias para venc.
                        break;
                    case this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                        //ParcCartaoInternet
                        this.enumFormaPagto = EnumFormaPagto_1.EnumFormaPagto.ParcCartaoInternet; //forma de pagamento
                        this.opcaoPagtoParcCartaoInternet = this.montaParcelamentoExistente(); //recebe a descrição (1 X R$ 00,00)
                        break;
                    case this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                        //ParcCartaoMaquineta
                        this.enumFormaPagto = EnumFormaPagto_1.EnumFormaPagto.ParcCartaoMaquineta; //forma de pagamento
                        this.opcaoPagtoParcCartaoMaquineta = this.montaParcelamentoExistente(); //recebe a descrição (1 X R$ 00,00)
                        break;
                    case this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                        //ParcComEnt
                        this.enumFormaPagto = EnumFormaPagto_1.EnumFormaPagto.ParcComEnt; //forma de pagamento
                        this.vlEntrada = this.dtoPedido.FormaPagtoCriacao.C_pce_entrada_valor; //valor de entrada
                        this.meioPagtoEntrada = parseInt(this.dtoPedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto); //deposito ou...
                        this.meioPagtoEntradaPrest = parseInt(this.dtoPedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto); //deposito ou...
                        this.opcaoPagtoParcComEntrada = this.montaParcelamentoExistente(); //recebe a descrição (1 X R$ 00,00)
                        this.diasVenc = this.dtoPedido.FormaPagtoCriacao.C_pce_prestacao_periodo; //recebe os dias de vencimento
                        break;
                    case this.constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                        //ParcSemEnt
                        this.dtoPedido.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
                        this.dtoPedido.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto = ""; //meio de pagamento
                        this.dtoPedido.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto = ""; //meio de pagamento
                        this.dtoPedido.FormaPagtoCriacao.C_pse_prim_prest_valor = 0;
                        this.dtoPedido.FormaPagtoCriacao.C_pse_prim_prest_apos = 0;
                        this.dtoPedido.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
                        this.dtoPedido.FormaPagtoCriacao.C_pse_demais_prest_valor = 0;
                        this.dtoPedido.FormaPagtoCriacao.C_pse_demais_prest_periodo = 0;
                        this.dtoPedido.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
                        break;
                }
                ;
            }
        };
        //metodo para montar o tipo de parcelamento que foi selecionado pelo usuário
        DadosPagto.prototype.montaParcelamentoExistente = function () {
            var retorno = "";
            this.dtoPedido.FormaPagtoCriacao.Tipo_parcelamento;
            this.recalcularValoresComCoeficiente(this.dtoPedido.FormaPagtoCriacao.Tipo_parcelamento);
            switch (this.dtoPedido.FormaPagtoCriacao.Tipo_parcelamento.toString()) {
                case this.constantes.COD_FORMA_PAGTO_A_VISTA:
                    retorno = this.dtoPedido.FormaPagtoCriacao.Qtde_Parcelas + " X " +
                        this.moedaUtils.formatarMoedaComPrefixo(this.dtoPedido.VlTotalDestePedido);
                    break;
                case this.constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    //ParcUnica
                    retorno = this.dtoPedido.FormaPagtoCriacao.Qtde_Parcelas + " X " +
                        this.moedaUtils.formatarMoedaComPrefixo(this.dtoPedido.FormaPagtoCriacao.C_pu_valor);
                    break;
                case this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    //ParcCartaoInternet
                    retorno = this.dtoPedido.FormaPagtoCriacao.Qtde_Parcelas + " X " +
                        this.moedaUtils.formatarMoedaComPrefixo(this.dtoPedido.FormaPagtoCriacao.C_pc_valor);
                    break;
                case this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    //ParcCartaoMaquineta
                    retorno = this.dtoPedido.FormaPagtoCriacao.Qtde_Parcelas + " X " +
                        this.moedaUtils.formatarMoedaComPrefixo(this.dtoPedido.FormaPagtoCriacao.C_pc_maquineta_valor);
                    break;
                case this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    //ParcComEnt
                    retorno = this.dtoPedido.FormaPagtoCriacao.C_pce_prestacao_qtde + " X " +
                        this.moedaUtils.formatarMoedaComPrefixo(this.dtoPedido.FormaPagtoCriacao.C_pce_prestacao_valor);
                    break;
                case this.constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    //ParcSemEnt
                    break;
            }
            ;
            return retorno;
        };
        return DadosPagto;
    }());
    exports.DadosPagto = DadosPagto;
});
//# sourceMappingURL=/scriptsJs/FuncoesTs/DadosPagto/DadosPagto.js.map