import { Component, OnInit, Input } from '@angular/core';
import { PrepedidoListarService } from '../../servicos/prepedido/prepedido-listar.service';
import { PrepedidosCadastradosDtoPrepedido } from '../../../../src/app/dto/prepedido/prepedidosCadastradosDtoPrepedido';
import { Observable } from 'rxjs';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';
import { DataUtils } from '../../../../src/app/utils/dataUtils';
import { MoedaUtils } from '../../../../src/app/utils/moedaUtils';
import { Location } from '@angular/common';
import { Sort } from '@angular/material/sort';
import { MatDialog, MatSnackBar } from '@angular/material';
import { PedidoListarService } from '../../servicos/pedido/pedido-listar.service';
import { environment } from '../../../../src/environments/environment';
import { ConfirmationDialogComponent } from '../../../../src/app/utils/confirmation-dialog/confirmation-dialog.component';
import { PrepedidoRemoverService } from '../../../../src/app/servicos/prepedido/prepedido-remover.service';
import { Router } from '@angular/router';
import { AlertDialogComponent } from 'src/app/utils/alert-dialog/alert-dialog.component';
import { ImpressaoService } from 'src/app/utils/impressao.service';
import { PedidoDtoPedido } from 'src/app/dto/pedido/pedidosDtoPedido';
import { NovoPrepedidoDadosService } from '../novo-prepedido/novo-prepedido-dados.service';
import { PrepedidoBuscarService } from 'src/app/servicos/prepedido/prepedido-buscar.service';

@Component({
  selector: 'app-lista-base',
  templateUrl: './lista-base.component.html',
  styleUrls: ['./lista-base.component.scss']
})
export class ListaBaseComponent extends TelaDesktopBaseComponent implements OnInit {

  //se estamos em prepedidos ou em pedidos
  @Input() emPrepedidos: boolean = true;

  constructor(public readonly prepedidoListarService: PrepedidoListarService,
    public readonly pedidoListarService: PedidoListarService,
    private readonly location: Location,
    telaDesktopService: TelaDesktopService,
    private readonly _snackBar: MatSnackBar,
    private readonly prepedidoRemoverService: PrepedidoRemoverService,
    private readonly router: Router,
    public readonly impressaoService: ImpressaoService,
    public readonly novoPrepedidoDadosService: NovoPrepedidoDadosService,
    public readonly prepedidoBuscarService: PrepedidoBuscarService,
    public readonly dialog: MatDialog) {
    super(telaDesktopService);

  }

  //para formatar as coisas
  dataFormatarTela = DataUtils.formatarTela;
  moedaUtils: MoedaUtils = new MoedaUtils();

  prepedidos$: Observable<PrepedidosCadastradosDtoPrepedido[]>;
  pedidos$: Observable<PedidoDtoPedido[]>;
  ngOnInit() {
    this.jaDeuErro = false;
    /*
    usamos o setTimeout para evitar o 
    ExpressionChangedAfterItHasBeenCheckedError: Expression has changed after it was checked. Previous value: 'carregando: false'. Current value: 'carregando: true'.

    se estiver imprimindo, nao podemos usar o timeout
    */
    if (this.impressaoService.emImpressao()) {
      this.inscrever();
    }
    else {
      setTimeout(() => { this.inscrever(); }, 0);
    }
  }

  inscrever(): void {
    if (this.emPrepedidos) {
      this.prepedidos$ = this.prepedidoListarService.prepedidos$;
      this.prepedidoListarService.errosPrepedidos$.subscribe(
        {
          next: (r) => {
            this.deuErro(r);
          }
        });
      this.prepedidoListarService.atualizar();
    }
    else {
      this.pedidos$ = this.pedidoListarService.pedidos$;
      this.pedidoListarService.errosPedidos$.subscribe(
        {
          next: (r) => {
            this.deuErro(r);
          }
        });
      this.pedidoListarService.atualizar();
    }
  }

  //avisamos de erros
  //temos um controle para não mostrar mensagens umas sobre as outras
  private jaDeuErro = false;
  private deuErro(r: any) {
    if (r == null) return;
    if (this.jaDeuErro) return;
    this.jaDeuErro = true;
    const dialogRef = this.dialog.open(AlertDialogComponent, {
      width: '350px',
      data: `Ocorreu um erro ao acessar os dados. Verifique a conexão com a Internet.`
    });
    dialogRef.afterClosed().subscribe((result) => {
      //this.jaDeuErro = false;
    });
  }


  voltar() {
    this.location.back();
  }
  displayedColumns: string[] = ['DataPrepedido', 'NumeroPrepedido', 'NomeCliente', 'Status', 'ValoTotal', 'Remover'];
  displayedColumnsPedido: string[] = ['DataPedido', 'NumeroPedido', 'NomeCliente', 'Status', 'ValoTotal'];


  //para remover o pedido, temos uma confirmação antes
  emRemoverPrepedido = false;
  removerPrepedido(numeroPrepedio: string): void {
    this.emRemoverPrepedido = true;
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '350px',
      data: `Tem certeza de que deseja excluir o pedido ${numeroPrepedio}? `
    });
    dialogRef.afterClosed().subscribe((result) => {
      this.emRemoverPrepedido = false;
      if (result) {
        this.prepedidoRemoverService.remover(numeroPrepedio).subscribe(
          {
            next() {
              const msg = `Pré-pedido ${numeroPrepedio} removido.`;
              this._snackBar.open(msg, undefined, {
                duration: environment.esperaErros
              });
              this.prepedidoListarService.atualizar();
            },
            error() {
              const msg = `Erro: erro ao remover pré-pedido  ${numeroPrepedio}.`;
              this._snackBar.open(msg, undefined, {
                duration: environment.esperaErros
              });
            },
          }
        );
      }
    });
  }
  cliqueLinha(linha: any) {
    //temos que ignorar se tiver clicado sobre a lata de lixo!
    if (this.emRemoverPrepedido)
      return;
    if (linha.NumeroPedido) {
      this.router.navigate(['/pedido/detalhes', linha.NumeroPedido]);
      
    }
    else {
      //vamos ver os detalhes: this.router.navigate(['/prepedido/detalhes', linha.NumeroPrepedido]);
      this.prepedidoBuscarService.buscar(linha.NumeroPrepedido).subscribe({
        next: (r) => {
          if (r == null) {
            this.deuErro("Erro");
            return;
          }

          //virou pedido? vamos direto para o pedido
          if (r.St_Orc_Virou_Pedido) {
            this.router.navigate(['/pedido/detalhes', r.NumeroPedido]);
            return;
          }

          //detalhes do prepedido
          this.novoPrepedidoDadosService.setar(r);
          //também passamos o número do pré-pedido no link

          // this.router.navigate(['/novo-prepedido/itens', r.NumeroPrePedido]);
          this.router.navigate(['/prepedido/detalhes',r.NumeroPrePedido]);
        },
        error: (r) => this.deuErro(r)

      });
    }
  }
}

