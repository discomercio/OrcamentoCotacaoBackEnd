import { Component, OnInit, Input } from '@angular/core';
import { PrepedidoComboNumeroService } from 'src/app/servicos/prepedido/prepedido-combo-numero.service';
import { PrepedidoComboCpfcnpjService } from 'src/app/servicos/prepedido/prepedido-combo-cpfcnpj.service';
import { Observable } from 'rxjs';
import { DataUtils } from 'src/app/utils/dataUtils';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { PrepedidoBuscaService } from 'src/app/servicos/prepedido/prepedido-busca.service';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { Router } from '@angular/router';
import { PedidoBuscaService } from 'src/app/servicos/pedido/pedido-busca.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-consulta-base',
  templateUrl: './consulta-base.component.html',
  styleUrls: ['./consulta-base.component.scss']
})
export class ConsultaBaseComponent extends TelaDesktopBaseComponent implements OnInit {

  //se estamos em prepedidos ou em pedidos
  @Input() emPrepedidos: boolean = true;

  //listas para os combos
  optionsNumeroPrePedidoBusca$: Observable<string[]>;
  optionsClienteBusca$: Observable<string[]>;


  constructor(private prepedidoComboNumeroService: PrepedidoComboNumeroService,
    private _snackBar: MatSnackBar,
    private prepedidoComboCpfcnpjService: PrepedidoComboCpfcnpjService,
    telaDesktopService: TelaDesktopService,
    private router: Router,
    public pedidoBuscaService: PedidoBuscaService,
    public prepedidoBuscaService: PrepedidoBuscaService) {
    super(telaDesktopService);

    //carrega os combos
    this.optionsNumeroPrePedidoBusca$ = this.prepedidoComboNumeroService.obter();
    this.optionsClienteBusca$ = this.prepedidoComboCpfcnpjService.obter();

  }

  ngOnInit() {
  }

  buscar() {
    if (this.emPrepedidos) {
      //nenhuma busca, ligamos os dois
      if (!this.prepedidoBuscaService.paramsBuscaPrepedido.tipoBuscaAndamento && !this.prepedidoBuscaService.paramsBuscaPrepedido.tipoBuscaPedido) {
        this.prepedidoBuscaService.paramsBuscaPrepedido.tipoBuscaAndamento = true;
        this.prepedidoBuscaService.paramsBuscaPrepedido.tipoBuscaPedido = true;

        this._snackBar.open("Nenhum tipo estava selecionado, os dois tipos foram selecionados", "", {
          duration: environment.esperaAvisos
        });
      }

      this.prepedidoBuscaService.atualizar();
    }
    else {
      //nenhuma busca, ligamos os dois
      if (!this.pedidoBuscaService.paramsBuscaPedido.tipoBuscaEmAndamento && !this.pedidoBuscaService.paramsBuscaPedido.tipoBuscaEncerrado) {
        this.pedidoBuscaService.paramsBuscaPedido.tipoBuscaEmAndamento = true;
        this.pedidoBuscaService.paramsBuscaPedido.tipoBuscaEncerrado = true;

        this._snackBar.open("Nenhum tipo estava selecionado, os dois tipos foram selecionados", "", {
          duration: environment.esperaAvisos
        });
      }

      this.pedidoBuscaService.atualizar();
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
