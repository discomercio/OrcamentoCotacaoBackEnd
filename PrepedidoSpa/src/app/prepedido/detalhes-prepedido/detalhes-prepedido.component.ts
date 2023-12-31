import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { Location } from '@angular/common';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';
import { PrepedidoBuscarService } from '../../../../src/app/servicos/prepedido/prepedido-buscar.service';
import { PedidoBuscarService } from '../../../../src/app/servicos/pedido/pedido-buscar.service';
import { MatDialog } from '@angular/material';
import { PedidoDto } from 'src/app/dto/pedido/detalhesPedido/PedidoDto2';
import { ImpressaoService } from 'src/app/utils/impressao.service';
import { PrePedidoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrePedidoDto';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { AutenticacaoService } from 'src/app/servicos/autenticacao/autenticacao.service';

@Component({
  selector: 'app-detalhes-prepedido',
  templateUrl: './detalhes-prepedido.component.html',
  styleUrls: ['./detalhes-prepedido.component.scss']
})
export class DetalhesPrepedidoComponent extends TelaDesktopBaseComponent implements OnInit, AfterViewInit {

  constructor(private readonly activatedRoute: ActivatedRoute,
    private readonly router: Router,
    telaDesktopService: TelaDesktopService,
    private readonly location: Location,
    public readonly prepedidoBuscarService: PrepedidoBuscarService,
    public readonly pedidoBuscarService: PedidoBuscarService,
    public readonly impressaoService: ImpressaoService,
    public readonly dialog: MatDialog,
    private readonly autenticacaoService: AutenticacaoService
    // public readonly alertaService: AlertaService
  ) {

    super(telaDesktopService);
  }

  emPrepedidos = true;
  numeroPrepedido = "";
  numeroPedido = "";
  prepedido: PrePedidoDto = null;
  pedido: PedidoDto = null;

  //avisamos de erros
  //temos um controle para não mostrar mensagens umas sobre as outras


  private jaDeuErro = true;
  private deuErro(r: any) {
    if (r == null) return;
    if (this.jaDeuErro) return;
    this.jaDeuErro = true;

    let alertaService = new AlertaService(this.dialog, this.router);

    alertaService.mostrarErroInternet(r);
    // const dialogRef = this.dialog.open(AlertDialogComponent, {
    //   width: '350px',
    //   data: `Ocorreu um erro ao acessar os dados. Verifique a conexão com a Internet.`
    // });
    // dialogRef.afterClosed().subscribe((result) => {
    //   //somente na inicialização, ou dá várias vezes
    //   this.jaDeuErro = false;
    // });
  }


  //verifica se vamos mostrar um pedido ou um prepedio
  calcularEmPrepedidos() {
    //agora acessa o dado que vamos mostrar
    
    this.numeroPrepedido = this.activatedRoute.snapshot.params.numeroPrepedido;
    this.numeroPedido = this.activatedRoute.snapshot.params.numeroPedido;
    if (!!this.numeroPrepedido) {
      this.emPrepedidos = true;
    }
    else {
      this.emPrepedidos = false;
    }
  }
  
  ngOnInit() {
    this.testeURL = localStorage.getItem('ultima_url');
    this.calcularEmPrepedidos()

    if (this.impressaoService.emImpressao()) {

      let w: any = window;
      this.pedido = w.pedido;
      this.prepedido = w.prepedido;
      return;
    }

     
    //para mostrar a espera enquanto carrega (senão, ele mostra os dados do pedido anterior)
    this.prepedido = null;
    this.pedido = null;

    this.jaDeuErro = false;
    //agora acessa o dado que vamos mostrar
    if (this.emPrepedidos) {
     
      this.prepedidoBuscarService.buscar(this.numeroPrepedido).subscribe({
        next: (r) => {
          if (r == null) {
            this.deuErro("Erro");
            return;
          }
          this.prepedido = r;
          let w: any = window;
          w.prepedido = this.prepedido;
          if (this.prepedido.St_Orc_Virou_Pedido) {
            this.router.navigate(['/pedido/detalhes', this.prepedido.NumeroPedido]);
          }
        },
        error: (r) => this.deuErro(r)
      });
    }
    else {
      this.pedidoBuscarService.atualizar(this.numeroPedido).subscribe({
        next: (r) => {
          if (r == null) {
            this.deuErro("Erro");
            return;
          }
          this.pedido = r;
          let w: any = window;
          w.pedido = this.pedido;
        },
        error: (r) => this.deuErro(r)
      });
    }
  }

  ngAfterViewInit() {
    localStorage.removeItem('ultima_url');
  }

  public testeURL: string = "";
  voltar() {
    //preciso verificar a url anterior para não direcionar para observações 
    if (this.testeURL) {
      if (this.testeURL.indexOf('observacoes') >= 0) {
        this.router.navigate(["/"]);
      }
    }
    else {
      this.location.back();
    }
  }

}
