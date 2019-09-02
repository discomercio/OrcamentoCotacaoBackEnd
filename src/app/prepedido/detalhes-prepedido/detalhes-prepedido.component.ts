import { Component, OnInit, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { Location } from '@angular/common';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';
import { PrepedidoBuscarService } from '../../../../src/app/servicos/prepedido/prepedido-buscar.service';
import { PedidoBuscarService } from '../../../../src/app/servicos/pedido/pedido-buscar.service';
import { Observable } from 'rxjs';
import { MatDialog } from '@angular/material';
import { AlertDialogComponent } from 'src/app/utils/alert-dialog/alert-dialog.component';
import { PedidoDto } from 'src/app/dto/pedido/detalhesPedido/PedidoDto';
import { ObjectUtils } from 'src/app/utils/objectUtils';
import { ImpressaoService } from 'src/app/utils/impressao.service';

@Component({
  selector: 'app-detalhes-prepedido',
  templateUrl: './detalhes-prepedido.component.html',
  styleUrls: ['./detalhes-prepedido.component.scss']
})
export class DetalhesPrepedidoComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(private readonly activatedRoute: ActivatedRoute,
    private readonly router: Router,
    telaDesktopService: TelaDesktopService,
    private readonly location: Location,
    public readonly prepedidoBuscarService: PrepedidoBuscarService,
    public readonly pedidoBuscarService: PedidoBuscarService,
    public readonly impressaoService:ImpressaoService,
    public readonly dialog: MatDialog
  ) {
    super(telaDesktopService);
  }

  emPrepedidos = true;
  numeroPrepedido = "";
  numeroPedido = "";
  prepedido: any = null;
  pedido: PedidoDto = null;

  //avisamos de erros
  //temos um controle para não mostrar mensagens umas sobre as outras
  private jaDeuErro = false;
  private deuErro(r: any) {
    if (r == null) return;
    if (this.jaDeuErro) return;
    this.jaDeuErro = true;
    const dialogRef = this.dialog.open(AlertDialogComponent, {
      width: '350px',
      data: `Ocorreu um erro ao acessar os dados. Verifique a conexão com a Internet. (Nota: esta tela ainda não foi implementada na API)`
    });
    dialogRef.afterClosed().subscribe((result) => {
      //somente na inicialização, ou dá várias vezes
      this.jaDeuErro = false;
    });
  }

  ngOnInit() {

    //para mostrar a espera qenquanto carrega (senão, ele mostra os dados do pedido anterior)
    this.prepedido = null;
    this.pedido = null;

    this.jaDeuErro = false;

    //agora acessa o dado que vamos mostrar
    this.numeroPrepedido = this.activatedRoute.snapshot.params.numeroPrepedido;
    this.numeroPedido = this.activatedRoute.snapshot.params.numeroPedido;
    if (!!this.numeroPrepedido) {
      this.emPrepedidos = true;
      this.prepedidoBuscarService.atualizar(this.numeroPrepedido).subscribe({
        next: (r) => {
          if (r == null) {
            this.deuErro("Erro");
            return;
          }
          this.prepedido = r;
        },
        error: (r) => this.deuErro(r)
      });
    }
    else {
      this.emPrepedidos = false;
      this.pedidoBuscarService.atualizar(this.numeroPedido).subscribe({
        next: (r) => {
          if (r == null) {
            this.deuErro("Erro");
            return;
          }
          this.pedido = r;
        },
        error: (r) => this.deuErro(r)
      });
    }
  }

  voltar() {
    this.location.back();
  }

}
