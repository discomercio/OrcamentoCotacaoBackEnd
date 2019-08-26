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
    public readonly dialog: MatDialog
  ) {
    super(telaDesktopService);
  }

  emPrepedidos = true;
  numeroPrepedido = "";
  numeroPedido = "";
  prepedido: any = null;
  pedido: any = null;

  //avisamos de erros
  //temos um controle para não mostrar mensagens umas sobre as outras
  //só damos erro na primeria vez porque esta tela precisa voltar para outra para acessar os dados de novo
  private jaDeuErro = false;
  private deuErro(r: any) {
    if (r == null) return;
    if (this.jaDeuErro) return;
    this.jaDeuErro = true;
    const dialogRef = this.dialog.open(AlertDialogComponent, {
      width: '350px',
      data: `Ocorreu um erro ao acessar os dados. Verifique a conexão com a Internet.`
    });
  }
  ngOnInit() {

    //registra os observers e o tratamento de erro
    this.pedidoBuscarService.pedidos$.subscribe({
      next: (r) => { this.pedido = r; }
    });
    this.pedidoBuscarService.errosPedidos$.subscribe(
      {
        next: (r) => this.deuErro(r)
      });
    this.prepedidoBuscarService.pedidos$.subscribe({
      next: (r) => { this.prepedido = r; }
    });
    this.prepedidoBuscarService.errosPedidos$.subscribe(
      {
        next: (r) => this.deuErro(r)
      });

    //agora acessa o dado que vamos mostrar
    this.numeroPrepedido = this.activatedRoute.snapshot.params.numeroPrepedido;
    this.numeroPedido = this.activatedRoute.snapshot.params.numeroPedido;
    if (!!this.numeroPrepedido) {
      this.emPrepedidos = true;
      this.prepedidoBuscarService.atualizar(this.numeroPrepedido);
    }
    else {
      this.emPrepedidos = false;
      this.pedidoBuscarService.atualizar(this.numeroPedido);
    }
  }

  voltar() {
    this.location.back();
  }

}
