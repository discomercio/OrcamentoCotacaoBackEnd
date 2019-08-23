import { Component, OnInit, Input } from '@angular/core';
import { PrepedidoBuscaService } from 'src/app/servicos/prepedido/prepedido-busca.service';
import { PrepedidosCadastradosDtoPrepedido } from 'src/app/dto/prepedido/prepedidosCadastradosDtoPrepedido';
import { Observable } from 'rxjs';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { DataUtils } from 'src/app/utils/dataUtils';
import { MoedaUtils } from 'src/app/utils/erro/moedaUtils';
import { Location } from '@angular/common';
import { Sort } from '@angular/material/sort';
import { MatDialog, MatSnackBar } from '@angular/material';
import { PedidoBuscaService } from 'src/app/servicos/pedido/pedido-busca.service';
import { PedidosDtoPedido } from 'src/app/dto/pedido/pedidosDtoPedido';
import { environment } from 'src/environments/environment';
import { ConfirmationDialogComponent } from 'src/app/utils/confirmation-dialog/confirmation-dialog.component';
import { PrepedidoRemoverService } from 'src/app/servicos/prepedido/prepedido-remover.service';

@Component({
  selector: 'app-lista-base',
  templateUrl: './lista-base.component.html',
  styleUrls: ['./lista-base.component.scss']
})
export class ListaBaseComponent extends TelaDesktopBaseComponent implements OnInit {

  //se estamos em prepedidos ou em pedidos
  @Input() emPrepedidos: boolean = true;

  constructor(public prepedidoBuscaService: PrepedidoBuscaService,
    public pedidoBuscaService: PedidoBuscaService,
    private location: Location,
    telaDesktopService: TelaDesktopService,
    private _snackBar: MatSnackBar,
    private prepedidoRemoverService: PrepedidoRemoverService,
    public dialog: MatDialog) {
    super(telaDesktopService);

  }

  //para formatar as coisas
  dataFormatarTela = DataUtils.formatarTela;
  moedaUtils: MoedaUtils = new MoedaUtils();

  prepedidos$: Observable<PrepedidosCadastradosDtoPrepedido[]>;
  pedidos$: Observable<PedidosDtoPedido[]>;
  ngOnInit() {
    let __this = this;
    this.prepedidos$ = this.prepedidoBuscaService.prepedidos$;
    this.prepedidoBuscaService.errosPrepedidos$.subscribe(
      {
        next(r) {
          if (r == null) return;
          __this._snackBar.open("Ocorreu um erro ao fazer a busca de pré-pedidos. Verifique a conexão e tente novamente.", "", {
            duration: environment.esperaErros
          });
        }
      });
    this.prepedidoBuscaService.atualizar();

    this.pedidos$ = this.pedidoBuscaService.pedidos$;
    this.pedidoBuscaService.errosPedidos$.subscribe(
      {
        next(r) {
          if (r == null) return;
          __this._snackBar.open("Ocorreu um erro ao fazer a busca de pedidos. Verifique a conexão e tente novamente.", "", {
            duration: environment.esperaErros
          });
        }
      });
    this.pedidoBuscaService.atualizar();
  }

  voltar() {
    this.location.back();
  }
  displayedColumns: string[] = ['DataPrepedido', 'NumeroPrepedido', 'NomeCliente', 'Status', 'ValoTotal', 'Remover'];
  displayedColumnsPedido: string[] = ['DataPedido', 'NumeroPedido', 'NomeCliente', 'Status', 'ValoTotal'];


  removerPrepedido(numeroPrepedio: string): void {
    const __this = this;
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '350px',
      data: `Tem certeza de que deseja excluir o pedido ${numeroPrepedio}? `
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        __this.prepedidoRemoverService.remover(numeroPrepedio).subscribe(
          {
            next() {
              const msg = `Pré-pedido ${numeroPrepedio} removido.`;
              __this._snackBar.open(msg, undefined, {
                duration: environment.esperaErros
              });
              __this.prepedidoBuscaService.atualizar();
            },
            error() {
              const msg = `Erro: erro ao remover pré-pedido  ${numeroPrepedio}.`;
              __this._snackBar.open(msg, undefined, {
                duration: environment.esperaErros
              });
            },
          }
        );
      }
    });
  }
}

