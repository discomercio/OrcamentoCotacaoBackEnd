import { Location } from '@angular/common';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { NovoPrepedidoDadosService } from '../novo-prepedido-dados.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { MatDialog } from '@angular/material';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { Router, ActivatedRoute } from '@angular/router';
import { PrePedidoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrePedidoDto';
import { MoedaUtils } from 'src/app/utils/moedaUtils';
import { Constantes } from 'src/app/dto/Constantes';
import { CpfCnpjUtils } from 'src/app/utils/cpfCnpjUtils';
import { PassoPrepedidoBase } from '../passo-prepedido-base';
import { FormaPagtoDto } from 'src/app/dto/FormaPagto/FormaPagtoDto';
import { PrepedidoBuscarService } from 'src/app/servicos/prepedido/prepedido-buscar.service';
import { EnumFormaPagto } from './enum-forma-pagto';
import { CoeficienteDto } from 'src/app/dto/Produto/CoeficienteDto';
import { strictEqual } from 'assert';
import { Observable } from 'rxjs';
import { EnumTipoPagto } from './tipo-forma-pagto';
import { parse } from 'url';
import { FormaPagtoCriacaoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/FormaPagtoCriacaoDto';


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

  ngOnInit() {
    this.verificarEmProcesso();
    this.buscarFormaPagto();
    this.buscarCoeficiente(null);
  }

  //#region navegação
  voltar() {
    this.location.back();
  }
  continuar() {


    this.router.navigate(["../confirmar-prepedido"], { relativeTo: this.activatedRoute });
  }
  //#endregion



  public podeContinuar(): boolean {
    //afazer: verificar forma de pagto selecionada para incluir no DTO
    if (this.enumFormaPagto == 0) {
      this.alertaService.mostrarMensagem("Favor escolher uma forma de pagamento!");
    }
    else {
      //passar valores para o dto
      this.atribuirFormaPagtoParaDto();
      return true;
    }
    return false;
  }

  //Gabriel

  public opcaoPagto: string;
  public meioPagtoEntrada: string;
  public meioPagtoAVista: string;
  public meioPagtoEntradaPrest: string;
  public diasVenc: number;//pagto com entrada, dias para vencimento
  public meioPagtoParcUnica: string;
  public diasVencParcUnica: number;
  qtde: number;//qtde de parcelas
  valor: number;//valor da parcela

  atribuirFormaPagtoParaDto() {

    this.verificaParcelamento();

    if (this.enumFormaPagto == 1) {
      //A vista
      this.prePedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
      this.prePedidoDto.FormaPagtoCriacao.Op_av_forma_pagto = this.meioPagtoAVista;//meio de pagamento
      this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas = 1;
    }
    if (this.enumFormaPagto == 2) {
      //ParcCartaoInternet
      debugger;
      this.prePedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
      this.prePedidoDto.FormaPagtoCriacao.C_pc_qtde = this.qtde; //passar a qtde de parcelas para uma variávl "qtdeParcelas"
      this.prePedidoDto.FormaPagtoCriacao.C_pc_valor = this.valor;
      this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas = this.qtde;
    }
    if (this.enumFormaPagto == 3) {
      //ParcComEnt
      this.prePedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
      this.prePedidoDto.FormaPagtoCriacao.Op_pce_entrada_forma_pagto = this.meioPagtoEntrada;//meio de pagamento
      this.prePedidoDto.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = this.meioPagtoEntradaPrest;//meio de pagamento
      this.prePedidoDto.FormaPagtoCriacao.C_pce_entrada_valor = parseFloat(this.vlEntrada);
      this.prePedidoDto.FormaPagtoCriacao.C_pce_prestacao_qtde = this.qtde;
      this.prePedidoDto.FormaPagtoCriacao.C_pce_prestacao_valor = this.valor;
      this.prePedidoDto.FormaPagtoCriacao.C_pce_prestacao_periodo = this.diasVenc;
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
      this.prePedidoDto.FormaPagtoCriacao.Op_pu_forma_pagto = this.meioPagtoParcUnica;//meio de pagamento
      this.prePedidoDto.FormaPagtoCriacao.C_pu_valor = this.valor;
      this.prePedidoDto.FormaPagtoCriacao.C_pu_vencto_apos = this.diasVencParcUnica;
      this.prePedidoDto.FormaPagtoCriacao.Qtde_Parcelas = 1;
    }
    if (this.enumFormaPagto == 6) {
      //ParcCartaoMaquineta
      this.prePedidoDto.FormaPagtoCriacao.Rb_forma_pagto = this.enumFormaPagto.toString();
      this.prePedidoDto.FormaPagtoCriacao.C_pc_maquineta_qtde = this.qtde;
      this.prePedidoDto.FormaPagtoCriacao.C_pc_maquineta_valor = this.valor;
      this.prePedidoDto.FormaPagtoCriacao.C_pc_maquineta_qtde = this.qtde;
    }

  }

  verificaParcelamento() {
    if (!!this.opcaoPagto) {
      this.qtde = parseInt(this.opcaoPagto.substring(0, 1));
      this.valor = parseFloat(this.opcaoPagto.substring(6));
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
      error: (r: FormaPagtoDto) => this.alertaService.mostrarErroInternet()
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
      error: (r: CoeficienteDto) => this.alertaService.mostrarErroInternet()
    })
  }

  //chamado quando algum item do prepedido for alterado
  //aqui é feito a limpeza do select da forma de pagamento
  public prepedidoAlterado() {
    this.vlEntrada = "";
    this.enumFormaPagto = EnumFormaPagto.Selecionar;
  }

  constantes = new Constantes();
  lstMsg: string[] = [];
  tipoFormaPagto: string = '';
  recalcularValoresComCoeficiente(enumFP: number): void {
    //verificar EnumTipoPagto para passar o valor do tipo "AV, SE, CE"
    this.tipoFormaPagto = this.verificaEnum(enumFP);
    //aisamos que está carregando...
    this.lstMsg = new Array();
    this.lstMsg.push("Carregando dados....");

    this.buscarCoeficiente(() => this.listarValoresComCoeficiente(this.tipoFormaPagto));
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

  listarValoresComCoeficiente(enumFP: string): string[] {

    //this.lstMsg
    let valor = 0
    this.lstMsg = new Array();
    //pegar o valor do item e multiplicar com o coeficiente
    let lstCoeficiente = this.coeficienteDto;
    this.prePedidoDto = this.novoPrepedidoDadosService.prePedidoDto;
    let lstProdutos = this.prePedidoDto.ListaProdutos;
    let constant = this.constantes;

    for (var x = 0; x < lstProdutos.length; x++) {
      for (var i = 0; i < lstCoeficiente.length; i++) {
        //avista o coeficiente é 1, sendo assim não faz alteração nos valores
        // verificar os o enum de formaPagto 
        if (this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_A_VISTA &&
          lstProdutos[x].Fabricante == lstCoeficiente[i].Fabricante &&
          enumFP == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA) {
          lstProdutos[x].VlUnitario = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
          lstProdutos[x].VlTotalItem = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
          lstProdutos[x].VlLista = lstProdutos[x].Preco;
          lstProdutos[x].TotalItem = (lstProdutos[x].Preco * lstProdutos[x].Qtde) * (1 - lstProdutos[x].Desconto / 100);
          this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' + (lstProdutos[x].TotalItem).toFixed(2));
          break;
        }
        else if (this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELA_UNICA &&
          lstProdutos[x].Fabricante == lstCoeficiente[i].Fabricante &&
          lstCoeficiente[i].TipoParcela == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
          enumFP == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
          lstProdutos[x].VlUnitario = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
          lstProdutos[x].VlTotalItem = lstProdutos[x].Preco * (1 - lstProdutos[x].Desconto / 100);
          lstProdutos[x].VlLista = lstProdutos[x].Preco;
          lstProdutos[x].TotalItem = (lstProdutos[x].Preco * lstProdutos[x].Qtde) * (1 - lstProdutos[x].Desconto / 100);
          this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' + (lstProdutos[x].TotalItem).toFixed(2));
          break;
        }
        else if (this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA &&
          lstProdutos[x].Fabricante == lstCoeficiente[i].Fabricante &&
          lstCoeficiente[i].TipoParcela == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
          enumFP == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {
          lstProdutos[x].VlUnitario = ((lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100));
          lstProdutos[x].VlTotalItem = (lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
          lstProdutos[x].VlLista = lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente;
          lstProdutos[x].TotalItem = ((lstProdutos[x].Preco * lstProdutos[x].Qtde) * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
          this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' +
            (lstProdutos[x].TotalItem / lstCoeficiente[i].QtdeParcelas).toFixed(2));
        }
        //parcelado sem entrada
        if ((this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA ||
          this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELADO_CARTAO || 
          this.enumFormaPagto.toString() == constant.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA) &&
          lstProdutos[x].Fabricante == lstCoeficiente[i].Fabricante &&
          lstCoeficiente[i].TipoParcela == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
          enumFP == constant.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {
          lstProdutos[x].VlUnitario = ((lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100));
          lstProdutos[x].VlTotalItem = (lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
          lstProdutos[x].VlLista = lstProdutos[x].Preco * lstCoeficiente[i].Coeficiente;
          lstProdutos[x].TotalItem = ((lstProdutos[x].Preco * lstProdutos[x].Qtde) * lstCoeficiente[i].Coeficiente) * (1 - lstProdutos[x].Desconto / 100);
          this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' +
            (lstProdutos[x].TotalItem / lstCoeficiente[i].QtdeParcelas).toFixed(2));
        }
      }
    }

    return this.lstMsg;
  }



  vlEntrada: string;
  moedaUtils = new MoedaUtils();
  calcularParcelaComEntrada() {
    this.lstMsg = new Array();
    this.prePedidoDto = this.novoPrepedidoDadosService.prePedidoDto;
    let lstProdutos = this.prePedidoDto.ListaProdutos;
    let lstCoeficiente = this.coeficienteDto;
    this.vlEntrada = this.moedaUtils.formatarMoedaSemPrefixo(parseInt(this.vlEntrada));

    var vltotal = this.prePedidoDto.ListaProdutos.reduce((sum, current) => sum + current.TotalItem, 0);

    var te = parseInt(this.vlEntrada.replace('.', ''));
    vltotal = vltotal - te;

    if (parseInt(this.vlEntrada) > vltotal)
      this.alertaService.mostrarMensagem("Valor da entrada é maior que o total do Pré-pedido!");
    else {
      for (var x = 0; x < lstProdutos.length; x++) {
        for (var i = 0; i < lstCoeficiente.length; i++) {
          if (lstCoeficiente[i].TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {
            this.lstMsg.push(lstCoeficiente[i].QtdeParcelas + ' X R$' +
              (vltotal / lstCoeficiente[i].QtdeParcelas).toFixed(2));
          }
        }
      }
    }
    return this.lstMsg;
  }



}

