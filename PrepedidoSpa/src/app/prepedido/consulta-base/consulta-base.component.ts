import { Component, OnInit, Input } from '@angular/core';
import { PrepedidoComboNumeroService } from '../../../../src/app/servicos/prepedido/prepedido-combo-numero.service';
import { PrepedidoComboCpfcnpjService } from '../../../../src/app/servicos/prepedido/prepedido-combo-cpfcnpj.service';
import { Observable } from 'rxjs';
import { DataUtils } from '../../../../src/app/utils/dataUtils';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';
import { PrepedidoListarService } from '../../servicos/prepedido/prepedido-listar.service';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { Router } from '@angular/router';
import { PedidoListarService } from '../../servicos/pedido/pedido-listar.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from '../../../../src/environments/environment';
import { PedidoComboCpfcnpjService } from '../../../../src/app/servicos/pedido/pedido-combo-cpfcnpj.service';
import { PedidoComboNumeroService } from '../../../../src/app/servicos/pedido/pedido-combo-numero.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { isDate } from 'util';

@Component({
  selector: 'app-consulta-base',
  templateUrl: './consulta-base.component.html',
  styleUrls: ['./consulta-base.component.scss']
})
export class ConsultaBaseComponent extends TelaDesktopBaseComponent implements OnInit {

  //se estamos em prepedidos ou em pedidos
  @Input() emPrepedidos: boolean = true;

  //optionsClienteBusca
  //optionsNumeroPrePedidoBusca
  //listas para os combos
  optionsPrepedidoNumeroPrePedidoBusca$: Observable<string[]>;
  optionsPrepedidoClienteBusca$: Observable<string[]>;
  optionsPedidoNumeroPrePedidoBusca$: Observable<string[]>;
  optionsPedidoClienteBusca$: Observable<string[]>;



  constructor(private readonly prepedidoComboNumeroService: PrepedidoComboNumeroService,
    private readonly _snackBar: MatSnackBar,
    private readonly prepedidoComboCpfcnpjService: PrepedidoComboCpfcnpjService,
    private readonly pedidoComboCpfcnpjService: PedidoComboCpfcnpjService,
    private readonly pedidoComboNumeroService: PedidoComboNumeroService,
    telaDesktopService: TelaDesktopService,
    private readonly router: Router,
    public readonly pedidoListarService: PedidoListarService,
    public readonly prepedidoListarService: PrepedidoListarService,
    public readonly alertaService: AlertaService) {
    super(telaDesktopService);

    //carrega os combos
    this.optionsPrepedidoNumeroPrePedidoBusca$ = this.prepedidoComboNumeroService.obter();
    this.optionsPrepedidoClienteBusca$ = this.prepedidoComboCpfcnpjService.obter();
    this.optionsPedidoNumeroPrePedidoBusca$ = this.pedidoComboNumeroService.obter();
    this.optionsPedidoClienteBusca$ = this.pedidoComboCpfcnpjService.obter();

  }

  ngOnInit() {
  }

  //problema na comparação na linha 86
  public maxDate = new Date();
  public minDate = DataUtils.somarDias(new Date(), -60);

  buscar() {

    if (this.emPrepedidos) {
      //nenhuma busca, ligamos os dois
      if (!this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaAndamento &&
        !this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaPedido &&
        !this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaPedidoExcluidos) {
        this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaAndamento = true;
        this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaPedido = true;

        this._snackBar.open("Nenhum tipo estava selecionado, os dois tipos foram selecionados", "", {
          duration: environment.esperaAvisos
        });
      }
      if (!this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaAndamento &&
        !this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaPedido &&
        this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaPedidoExcluidos) {
        // buscando apenas os excluidos
        this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaAndamento = false;
        this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaPedido = false;
      }
      //vamos validar a data antes de tentar formatar
      let dataInicial = DataUtils.formata_formulario_date(this.prepedidoListarService.paramsBuscaPrepedido.dataInicial);

      if (!DataUtils.validarData(dataInicial)) {
        this.alertaService.mostrarMensagem("Data inicial inválida!");
        return;
      }

      if (dataInicial < this.minDate) {
        this._snackBar.open("É permitido realizar a busca dentro de um periodo de 60 dias atrás da Data atual", "", {
          duration: environment.esperaAvisos
        });
      }

      //antes de mandar fazer a busca vamos validar a data final
      let dataFinal: Date = DataUtils.formata_formulario_date(this.pedidoListarService.paramsBuscaPedido.dataFinal);
      if (!DataUtils.validarData(dataFinal)) {
        this.alertaService.mostrarMensagem("Data final inválida!");
        return;
      }
      this.prepedidoListarService.atualizar();
    }
    else {
      //nenhuma busca, ligamos os dois
      if (!this.pedidoListarService.paramsBuscaPedido.tipoBuscaEmAndamento &&
        !this.pedidoListarService.paramsBuscaPedido.tipoBuscaEncerrado) {
        this.pedidoListarService.paramsBuscaPedido.tipoBuscaEmAndamento = true;
        this.pedidoListarService.paramsBuscaPedido.tipoBuscaEncerrado = true;

        this._snackBar.open("Nenhum tipo estava selecionado, os dois tipos foram selecionados", "", {
          duration: environment.esperaAvisos
        });
      }

      let dataInicial = DataUtils.formata_formulario_date(this.pedidoListarService.paramsBuscaPedido.dataInicial);

      if (!DataUtils.validarData(dataInicial)) {
        this.alertaService.mostrarMensagem("Data inicial inválida!");
        return;
      }

      if (dataInicial < this.minDate) {
        this._snackBar.open("É permitido realizar a busca dentro de um periodo de 60 dias atrás da Data atual", "", {
          duration: environment.esperaAvisos
        });
      }
      //antes de mandar fazer a busca vamos validar a data final
      let dataFinal: Date = DataUtils.formata_formulario_date(this.pedidoListarService.paramsBuscaPedido.dataFinal);
      if (!DataUtils.validarData(dataFinal)) {
        this.alertaService.mostrarMensagem("Data final inválida!");
        return;
      }
      this.pedidoListarService.atualizar();
    }

    //na celular é outra tela, temos que navegar explicitamente
    if (!this.telaDesktop) {
      if (this.emPrepedidos)
        this.router.navigateByUrl('/prepedido/lista');
      else
        this.router.navigateByUrl('/pedido/lista');
    }
  }

  public checkExcluidos(): void {
    this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaAndamento = false;
    this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaPedido = false;
  }

  public checkBuscas(): void {
    if (this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaPedidoExcluidos)
      this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaPedidoExcluidos = false;
  }
}
