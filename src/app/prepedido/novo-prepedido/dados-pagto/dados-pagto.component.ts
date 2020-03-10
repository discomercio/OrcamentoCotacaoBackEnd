import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NovoPrepedidoDadosService } from '../novo-prepedido-dados.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { MatDialog } from '@angular/material';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MoedaUtils } from 'src/app/utils/moedaUtils';
import { Constantes } from 'src/app/dto/Constantes';
import { PassoPrepedidoBase } from '../passo-prepedido-base';
import { FormaPagtoDto } from 'src/app/dto/FormaPagto/FormaPagtoDto';
import { PrepedidoBuscarService } from 'src/app/servicos/prepedido/prepedido-buscar.service';
import { EnumFormaPagto } from './enum-forma-pagto';
import { CoeficienteDto } from 'src/app/dto/Produto/CoeficienteDto';
import { EnumTipoPagto } from './tipo-forma-pagto';
import { BreakpointObserver } from '@angular/cdk/layout';


@Component({
  selector: 'app-dados-pagto',
  templateUrl: './dados-pagto.component.html',
  styleUrls: ['./dados-pagto.component.scss']
})
export class DadosPagtoComponent extends PassoPrepedidoBase implements OnInit {

  public enumFormaPagto: EnumFormaPagto;
  //para usar o enum 
  public EnumFormaPagto = EnumFormaPagto;


  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly location: Location,
    router: Router,
    novoPrepedidoDadosService: NovoPrepedidoDadosService,
    public readonly alertaService: AlertaService,
    public readonly dialog: MatDialog,
    telaDesktopService: TelaDesktopService,
    public readonly prepedidoBuscarService: PrepedidoBuscarService
  ) {
    super(telaDesktopService, router, novoPrepedidoDadosService);
  }

  mascaraNum() {
    return [/\d/, /\d/, /\d/];
  }

  ngOnInit() {

    this.buscarQtdeParcCartaoVisa();
    this.verificarEmProcesso();
    this.buscarFormaPagto();
    this.buscarCoeficiente(null);
    setTimeout(() => {
      this.montaFormaPagtoExistente();
    }, 300);

  }

  //#region navegação
  voltar() {
    this.location.back();
  }
  continuar() {


    this.router.navigate(["../confirmar-prepedido"], { relativeTo: this.activatedRoute });
  }
  //#endregion



  public podeContinuar(mostrarMsg: boolean): boolean {
    if (this.enumFormaPagto == 0) {

      //necessário verificar se os dados de pagto estão corretamente preenchido
      if (mostrarMsg) {
        this.alertaService.mostrarMensagem("Favor escolher uma forma de pagamento!");
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
    this.prePedidoDto.FormaPagtoCriacao.Tipo_parcelamento = this.enumFormaPagto;
    this.prePedidoDto.VlTotalDestePedido = this.totalPedido();
    if (this.enumFormaPagto == 1) {
      //A vista      
      this.prePedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
      this.prePedidoDto.FormaPagtoCriacao.Op_av_forma_pagto = this.meioPagtoAVista.toString();//meio de pagamento
      this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas = 1;
    }
    if (this.enumFormaPagto == 2) {
      //ParcCartaoInternet
      this.prePedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
      this.prePedidoDto.FormaPagtoCriacao.C_pc_qtde = this.qtde; //passar a qtde de parcelas para uma variávl "qtdeParcelas"
      this.prePedidoDto.FormaPagtoCriacao.C_pc_valor = this.valor;
      this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas = this.qtde;
    }
    if (this.enumFormaPagto == 3) {
      //ParcComEnt
      this.prePedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
      this.prePedidoDto.FormaPagtoCriacao.Op_pce_entrada_forma_pagto = this.meioPagtoEntrada.toString();//meio de pagamento
      this.prePedidoDto.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = this.meioPagtoEntradaPrest.toString();//meio de pagamento
      this.prePedidoDto.FormaPagtoCriacao.C_pce_entrada_valor = this.vlEntrada;
      this.prePedidoDto.FormaPagtoCriacao.C_pce_prestacao_qtde = this.qtde;
      this.prePedidoDto.FormaPagtoCriacao.C_pce_prestacao_valor = this.valor;
      this.prePedidoDto.FormaPagtoCriacao.C_pce_prestacao_periodo = this.diasVenc != null ?
        parseInt(this.diasVenc.toString().replace("_", "")) : this.diasVenc;
      this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas = this.qtde + 1;//c_pce_prestacao_qtde + 1
    }
    //NÃO ESTA SENDO USADO
    if (this.enumFormaPagto == 4) {
      //ParcSemEnt
      this.prePedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
      this.prePedidoDto.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto = "";//meio de pagamento
      this.prePedidoDto.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto = "";//meio de pagamento
      this.prePedidoDto.FormaPagtoCriacao.C_pse_prim_prest_valor = 0;
      this.prePedidoDto.FormaPagtoCriacao.C_pse_prim_prest_apos = 0;
      this.prePedidoDto.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
      this.prePedidoDto.FormaPagtoCriacao.C_pse_demais_prest_valor = 0;
      this.prePedidoDto.FormaPagtoCriacao.C_pse_demais_prest_periodo = 0;
      this.prePedidoDto.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
    }
    if (this.enumFormaPagto == 5) {
      //ParcUnica
      this.prePedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
      this.prePedidoDto.FormaPagtoCriacao.Op_pu_forma_pagto = this.meioPagtoParcUnica.toString();//meio de pagamento
      this.prePedidoDto.FormaPagtoCriacao.C_pu_valor = this.valor;
      this.prePedidoDto.FormaPagtoCriacao.C_pu_vencto_apos = this.diasVencParcUnica != null ?
        parseInt(this.diasVencParcUnica.toString().replace("_", "")) : this.diasVencParcUnica;
      this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas = 1;
    }
    if (this.enumFormaPagto == 6) {
      //ParcCartaoMaquineta
      this.prePedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
      this.prePedidoDto.FormaPagtoCriacao.C_pc_maquineta_qtde = this.qtde;
      this.prePedidoDto.FormaPagtoCriacao.C_pc_maquineta_valor = this.valor;
      this.prePedidoDto.FormaPagtoCriacao.C_pc_maquineta_qtde = this.qtde;
      this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas = this.qtde;
    }

  }

  validarFormaPagto(mostrarMsg: boolean): boolean {

    let retorno = true;
    // this.verificaParcelamento();
    if (this.validarOpcoesPagto(mostrarMsg)) {
      if (this.enumFormaPagto == 1 && !this.meioPagtoAVista)//avista
      {
        if (mostrarMsg) {
          this.alertaService.mostrarMensagem("Favor selecionar o meio de pagamento á vista");
          retorno = false;
        }
      }
      if (this.enumFormaPagto == 2 && (!this.qtde || !this.valor))//ParcCartaoInternet
      {
        if (mostrarMsg) {
          this.alertaService.mostrarMensagem("Favor selecionar corretamente o meio de pagamento " +
            "para Parcelado no Cartão (Internet).");
          retorno = false;
        }
      }
      if (this.enumFormaPagto == 3) {
        if (this.meioPagtoEntrada && this.meioPagtoEntradaPrest)
        //ParcComEnt
        {
          if (!!this.vlEntrada && this.vlEntrada == 0.00) {
            this.alertaService.mostrarMensagem("Favor preencher o valor de entrada!");
            retorno = false;
          }
          if (this.meioPagtoEntradaPrest != 5 && this.meioPagtoEntradaPrest != 7 && !this.diasVenc) {
            if (mostrarMsg) {
              this.alertaService.mostrarMensagem("Favor informar corretamente os dados para pagamento Parcelado com Entrada!");
              retorno = false;
            }
          }
        }
        else {
          if (mostrarMsg) {
            this.alertaService.mostrarMensagem("Favor informar corretamente os dados para pagamento Parcelado com Entrada!");
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
          this.alertaService.mostrarMensagem("Favor informar corretamente os dados para pagamento Parcela Única!");
          retorno = false;
        }
      }
      if (this.enumFormaPagto == 6 && (!this.qtde || !this.valor))//ParcCartaoMaquineta
      {
        if (mostrarMsg) {
          this.alertaService.mostrarMensagem("Favor selecionar  corretamente o meio de pagamento " +
            "para Parcelado no Cartão (Maquineta).");
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
          if (mostrarMsg)
            this.alertaService.mostrarMensagem("Favor selecionar o campo Parcelamento para À vista!");
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
          if (mostrarMsg)
            this.alertaService.mostrarMensagem("Favor selecionar o Parcelamento para Cartão (Internet)!");
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
            if (mostrarMsg)
              this.alertaService.mostrarMensagem("Favor selecionar o Parcelamento para Parcela com entrada!");
            return false;
          }
          else {
            this.verificaParcelamento(this.opcaoPagtoParcComEntrada);
            return true;
          }
        }
        else {
          if (mostrarMsg)
            this.alertaService.mostrarMensagem("Favor informar o valor de entrada!");
          return false;
        }
      }
      if (this.enumFormaPagto == 4) {
        //ParcSemEntrada
      }
      if (this.enumFormaPagto == 5) {
        if (!this.opcaoPagtoParcUnica) {
          if (mostrarMsg)
            this.alertaService.mostrarMensagem("Favor selecionar o Parcelamento para Parcela Única!");
          return false;
        }
        else {
          this.verificaParcelamento(this.opcaoPagtoParcUnica);
          return true;
        }
      }
      if (this.enumFormaPagto == 6) {
        if (!this.opcaoPagtoParcCartaoMaquineta) {
          if (mostrarMsg)
            this.alertaService.mostrarMensagem("Favor selecionar o Parcelamento para Pagamento com cartão (Maquineta)!");
          return false;
        }
        else {
          this.verificaParcelamento(this.opcaoPagtoParcCartaoMaquineta);
          return true;
        }
      }
    }
    else {
      if (mostrarMsg)
        this.alertaService.mostrarMensagem("Favor selecionar uma Forma de Pagamento!");
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
  buscarFormaPagto() {
    return this.prepedidoBuscarService.buscarFormaPagto(this.prePedidoDto.DadosCliente.Tipo).subscribe({
      next: (r: FormaPagtoDto) => {
        if (!!r) {
          this.formaPagtoDto = r;
        }
        else {
          this.alertaService.mostrarMensagem("Erro ao carregar a lista de forma de pagamentos")
        }
      },
      error: (r: FormaPagtoDto) => this.alertaService.mostrarErroInternet(r)
    })
  }

  //Método esta funcionando
  coeficienteDto: CoeficienteDto[];
  buscarCoeficiente(callback: () => void) {
    return this.prepedidoBuscarService.buscarCoeficiente(this.prePedidoDto.ListaProdutos).subscribe({
      next: (r: CoeficienteDto[]) => {
        if (!!r) {
          this.coeficienteDto = r;
          if (callback)
            callback();
        }
        else {
          this.alertaService.mostrarMensagem("Erro ao carregar a lista de coeficientes dos fabricantes")
        }
      },
      error: (r: CoeficienteDto) => this.alertaService.mostrarErroInternet(r)
    })
  }

  // foi solicitado que a qtde de parcelas disponível será baseada na
  // qtde de parcelas disponível no cartão Visa(PRAZO_LOJA)
  //então faremos a busca pela API
  qtdeParcVisa: number;
  public buscarQtdeParcCartaoVisa(): void {
    this.prepedidoBuscarService.buscarQtdeParcCartaoVisa().subscribe({
      next: (r: number) => {
        if (!!r) {
          this.qtdeParcVisa = r;
        }
        else {
          this.alertaService.mostrarMensagem("Erro ao carregar a quantidade de parcelas!");
        }
      },
      error: (r: number) => this.alertaService.mostrarErroInternet(r)
    })
  }



  //chamado quando algum item do prepedido for alterado
  //aqui é feito a limpeza do select da forma de pagamento
  public prepedidoAlterado() {
    this.recalcularValoresComCoeficiente(this.enumFormaPagto);
    this.montarListaParcelamento(this.enumFormaPagto);
    this.opcaoPagtoParcComEntrada = null;
    this.opcaoPagtoAvista = null;
    this.opcaoPagtoParcCartaoInternet = null;
    this.opcaoPagtoParcCartaoMaquineta = null;
    this.opcaoPagtoParcUnica = null;
  }

  constantes = new Constantes();
  lstMsg: string[] = [];
  tipoFormaPagto: string = '';
  recalcularValoresComCoeficiente(enumFP: number): void {
    //na mudança da forma de pagto iremos zerar todos os campos
    // this.zerarTodosCampos();

    //verificar EnumTipoPagto para passar o valor do tipo "AV, SE, CE"
    this.tipoFormaPagto = this.verificaEnum(enumFP);
    //aisamos que está carregando...
    this.lstMsg = new Array();
    this.lstMsg.push("Carregando dados....");

    this.buscarCoeficiente(() => this.listarValoresComCoeficiente(this.tipoFormaPagto, enumFP));

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

  listarValoresComCoeficiente(txtFormaPagto: string, enumFP: number): void {

    //this.lstMsg
    let valor = 0
    // this.lstMsg = new Array();
    //pegar o valor do item e multiplicar com o coeficiente
    let lstCoeficiente = this.coeficienteDto;
    this.prePedidoDto = this.novoPrepedidoDadosService.prePedidoDto;
    let lstProdutos = this.prePedidoDto.ListaProdutos;
    let constant = this.constantes;
    if (this.enumFormaPagto) {
      for (var x = 0; x < lstProdutos.length; x++) {
        for (var i = 0; i < lstCoeficiente.length; i++) {
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
            lstProdutos[x].Fabricante == lstCoeficiente[i].Fabricante &&
            lstCoeficiente[i].TipoParcela == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
            txtFormaPagto == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
            lstProdutos[x].VlUnitario = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
            lstProdutos[x].VlTotalItem = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
            lstProdutos[x].VlLista = lstProdutos[x].Preco;
            lstProdutos[x].TotalItem = (lstProdutos[x].Preco * lstProdutos[x].Qtde) * (1 - lstProdutos[x].Desconto / 100);
            //this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' + (lstProdutos[x].TotalItem).toFixed(2));
            break;
          }
          else if (this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA &&
            lstProdutos[x].Fabricante == lstCoeficiente[i].Fabricante &&
            lstCoeficiente[i].TipoParcela == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
            txtFormaPagto == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {
              
            lstProdutos[x].VlUnitario = ((lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100));
            lstProdutos[x].VlTotalItem = (lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
            lstProdutos[x].VlLista = lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente;
            lstProdutos[x].TotalItem = ((lstProdutos[x].Preco * lstProdutos[x].Qtde) * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
            //this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' +
            // (lstProdutos[x].TotalItem / lstCoeficiente[i].QtdeParcelas).toFixed(2));
          }
          //parcelado sem entrada
          
          if ((
            this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELADO_CARTAO ||
            this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA) &&
            lstProdutos[x].Fabricante == lstCoeficiente[i].Fabricante &&
            lstCoeficiente[i].TipoParcela == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
            txtFormaPagto == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
              
            lstProdutos[x].VlUnitario = ((lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100));
            lstProdutos[x].VlTotalItem = (lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
            lstProdutos[x].VlLista = lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente;
            lstProdutos[x].TotalItem = ((lstProdutos[x].Preco * lstProdutos[x].Qtde) * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
            // this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' +
            //   (lstProdutos[x].TotalItem / lstCoeficiente[i].QtdeParcelas).toFixed(2));
          }
        }
      }
    }
    this.montarListaParcelamento(enumFP);
    // return this.lstMsg;
  }

  montarListaParcelamento(enumFP: number): string[] {
    this.lstMsg = new Array();
    let lstCoeficiente = this.coeficienteDto;
    let vlTotalPedido = this.prePedidoDto.ListaProdutos.reduce((sum, prod) => sum + prod.TotalItem, 0);
    let cont = 0;
    if (enumFP) {
      for (let i = 0; i < lstCoeficiente.length; i++) {
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
            
            this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + " X " +
              this.moedaUtils.formatarMoedaComPrefixo(vlTotalPedido / lstCoeficiente[i].QtdeParcelas));
            cont++;
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
    this.prePedidoDto = this.novoPrepedidoDadosService.prePedidoDto;
    let lstProdutos = this.prePedidoDto.ListaProdutos;
    let lstCoeficiente = this.coeficienteDto;
    if (!!this.vlEntrada && this.vlEntrada != 0.00) {
      var vltotal = this.prePedidoDto.ListaProdutos.reduce((sum, current) => sum + current.TotalItem, 0);

      if (this.vlEntrada > vltotal) {
        this.alertaService.mostrarMensagem("Valor da entrada é maior que o total do Pré-pedido!");
        this.vlEntrada = null;
      }
      else {
        vltotal = vltotal - this.vlEntrada;
        for (var i = 0; i < this.qtdeParcVisa; i++) {
          if (lstCoeficiente[i].TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {
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

    if (this.prePedidoDto.FormaPagtoCriacao.Tipo_parcelamento) {
      this.prePedidoDto.FormaPagtoCriacao.Tipo_parcelamento;

      switch (this.prePedidoDto.FormaPagtoCriacao.Tipo_parcelamento.toString()) {
        case this.constantes.COD_FORMA_PAGTO_A_VISTA:
          //A vista
          this.enumFormaPagto = EnumFormaPagto.Avista;//forma de pagamento
          this.meioPagtoAVista = parseInt(this.prePedidoDto.FormaPagtoCriacao.Op_av_forma_pagto);//deposito ou...
          this.opcaoPagtoAvista = this.montaParcelamentoExistente();//recebe a descrição (1 X R$ 00,00)
          break;
        case this.constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
          //ParcUnica
          this.enumFormaPagto = EnumFormaPagto.ParcUnica;//forma de pagamento
          this.meioPagtoParcUnica = parseInt(this.prePedidoDto.FormaPagtoCriacao.Op_pu_forma_pagto);//deposito ou...
          this.opcaoPagtoParcUnica = this.montaParcelamentoExistente();//recebe a descrição (1 X R$ 00,00)
          this.diasVencParcUnica = this.prePedidoDto.FormaPagtoCriacao.C_pu_vencto_apos;//dias para venc.
          break;
        case this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
          //ParcCartaoInternet
          this.enumFormaPagto = EnumFormaPagto.ParcCartaoInternet;//forma de pagamento
          this.opcaoPagtoParcCartaoInternet = this.montaParcelamentoExistente();//recebe a descrição (1 X R$ 00,00)
          break;
        case this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
          //ParcCartaoMaquineta
          this.enumFormaPagto = EnumFormaPagto.ParcCartaoMaquineta;//forma de pagamento
          this.opcaoPagtoParcCartaoMaquineta = this.montaParcelamentoExistente();//recebe a descrição (1 X R$ 00,00)
          break;
        case this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
          //ParcComEnt
          this.enumFormaPagto = EnumFormaPagto.ParcComEnt;//forma de pagamento
          this.vlEntrada = this.prePedidoDto.FormaPagtoCriacao.C_pce_entrada_valor;//valor de entrada
          this.meioPagtoEntrada = parseInt(this.prePedidoDto.FormaPagtoCriacao.Op_pce_entrada_forma_pagto);//deposito ou...
          this.meioPagtoEntradaPrest = parseInt(this.prePedidoDto.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto);//deposito ou...
          this.opcaoPagtoParcComEntrada = this.montaParcelamentoExistente();//recebe a descrição (1 X R$ 00,00)
          this.diasVenc = this.prePedidoDto.FormaPagtoCriacao.C_pce_prestacao_periodo;//recebe os dias de vencimento
          break;
        case this.constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
          //ParcSemEnt
          this.prePedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
          this.prePedidoDto.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto = "";//meio de pagamento
          this.prePedidoDto.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto = "";//meio de pagamento
          this.prePedidoDto.FormaPagtoCriacao.C_pse_prim_prest_valor = 0;
          this.prePedidoDto.FormaPagtoCriacao.C_pse_prim_prest_apos = 0;
          this.prePedidoDto.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
          this.prePedidoDto.FormaPagtoCriacao.C_pse_demais_prest_valor = 0;
          this.prePedidoDto.FormaPagtoCriacao.C_pse_demais_prest_periodo = 0;
          this.prePedidoDto.FormaPagtoCriacao.C_pse_demais_prest_qtde = 0;
          break;
      };
    }
  }

  //metodo para montar o tipo de parcelamento que foi selecionado pelo usuário
  montaParcelamentoExistente(): string {
    let retorno = "";
    this.prePedidoDto.FormaPagtoCriacao.Tipo_parcelamento;
    this.recalcularValoresComCoeficiente(this.prePedidoDto.FormaPagtoCriacao.Tipo_parcelamento);

    switch (this.prePedidoDto.FormaPagtoCriacao.Tipo_parcelamento.toString()) {
      case this.constantes.COD_FORMA_PAGTO_A_VISTA:
        retorno = this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas + " X " +
          this.moedaUtils.formatarMoedaComPrefixo(this.prePedidoDto.VlTotalDestePedido);
        break;
      case this.constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
        //ParcUnica
        retorno = this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas + " X " +
          this.moedaUtils.formatarMoedaComPrefixo(this.prePedidoDto.FormaPagtoCriacao.C_pu_valor);
        break;
      case this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
        //ParcCartaoInternet
        retorno = this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas + " X " +
          this.moedaUtils.formatarMoedaComPrefixo(this.prePedidoDto.FormaPagtoCriacao.C_pc_valor);
        break;
      case this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
        //ParcCartaoMaquineta
        retorno = this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas + " X " +
          this.moedaUtils.formatarMoedaComPrefixo(this.prePedidoDto.FormaPagtoCriacao.C_pc_maquineta_valor);
        break;
      case this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
        //ParcComEnt
        retorno = this.prePedidoDto.FormaPagtoCriacao.C_pce_prestacao_qtde + " X " +
          this.moedaUtils.formatarMoedaComPrefixo(this.prePedidoDto.FormaPagtoCriacao.C_pce_prestacao_valor);
        break;
      case this.constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
        //ParcSemEnt
        break;
    };

    return retorno;
  }

}

