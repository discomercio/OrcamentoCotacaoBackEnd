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
import { MatDialog } from '@angular/material';
import { PedidoBuscaService } from 'src/app/servicos/pedido/pedido-busca.service';
import { PedidosDtoPedido } from 'src/app/dto/pedido/pedidosDtoPedido';

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
          //TODO: mostrar erro em uma caixa de diálogo personalizada
          window.alert("Ocorreu um erro ao fazer a busca de pré-pedidos. Verifique a conexão e tente novamente.");
        }
      });
    this.prepedidoBuscaService.atualizar();

    this.pedidos$ = this.pedidoBuscaService.pedidos$;
    this.pedidoBuscaService.errosPedidos$.subscribe(
      {
        next(r) {
          if (r == null) return;
          //TODO: mostrar erro em uma caixa de diálogo personalizada
          window.alert("Ocorreu um erro ao fazer a busca de pedidos. Verifique a conexão e tente novamente.");
        }
      });
    this.pedidoBuscaService.atualizar();
  }

  voltar() {
    this.location.back();
  }
  displayedColumns: string[] = ['NumeroPrepedido', 'NomeCliente', 'Status', 'ValoTotal'];
  displayedColumnsPedido: string[] = ['NumeroPedido', 'NomeCliente', 'Status', 'ValoTotal'];


}

