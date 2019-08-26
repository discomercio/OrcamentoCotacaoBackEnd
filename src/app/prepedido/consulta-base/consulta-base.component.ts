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
    public readonly prepedidoListarService: PrepedidoListarService) {
    super(telaDesktopService);

    //carrega os combos
    this.optionsPrepedidoNumeroPrePedidoBusca$ = this.prepedidoComboNumeroService.obter();
    this.optionsPrepedidoClienteBusca$ = this.prepedidoComboCpfcnpjService.obter();
    this.optionsPedidoNumeroPrePedidoBusca$ = this.pedidoComboNumeroService.obter();
    this.optionsPedidoClienteBusca$ = this.pedidoComboCpfcnpjService.obter();

  }

  ngOnInit() {
  }

  buscar() {
    if (this.emPrepedidos) {
      //nenhuma busca, ligamos os dois
      if (!this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaAndamento && !this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaPedido) {
        this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaAndamento = true;
        this.prepedidoListarService.paramsBuscaPrepedido.tipoBuscaPedido = true;

        this._snackBar.open("Nenhum tipo estava selecionado, os dois tipos foram selecionados", "", {
          duration: environment.esperaAvisos
        });
      }

      this.prepedidoListarService.atualizar();
    }
    else {
      //nenhuma busca, ligamos os dois
      if (!this.pedidoListarService.paramsBuscaPedido.tipoBuscaEmAndamento && !this.pedidoListarService.paramsBuscaPedido.tipoBuscaEncerrado) {
        this.pedidoListarService.paramsBuscaPedido.tipoBuscaEmAndamento = true;
        this.pedidoListarService.paramsBuscaPedido.tipoBuscaEncerrado = true;

        this._snackBar.open("Nenhum tipo estava selecionado, os dois tipos foram selecionados", "", {
          duration: environment.esperaAvisos
        });
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
}
