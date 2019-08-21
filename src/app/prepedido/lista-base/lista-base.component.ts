import { Component, OnInit } from '@angular/core';
import { PrepedidoBuscaService } from 'src/app/servicos/prepedido/prepedido-busca.service';
import { PrepedidosCadastradosDtoPrepedido } from 'src/app/dto/prepedido/prepedidosCadastradosDtoPrepedido';
import { Observable } from 'rxjs';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { DataUtils } from 'src/app/utils/dataUtils';
import { MoedaUtils } from 'src/app/utils/erro/moedaUtils';
import { Location } from '@angular/common';
import {Sort} from '@angular/material/sort';

@Component({
  selector: 'app-lista-base',
  templateUrl: './lista-base.component.html',
  styleUrls: ['./lista-base.component.scss']
})
export class ListaBaseComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(public prepedidoBuscaService: PrepedidoBuscaService,
    private location: Location,
    telaDesktopService: TelaDesktopService) {
    super(telaDesktopService);

  }

  //para formatar as coisas
  dataFormatarTela = DataUtils.formatarTela;
  moedaUtils: MoedaUtils = new MoedaUtils();

  prepedidos$: Observable<PrepedidosCadastradosDtoPrepedido[]>;
  ngOnInit() {
    this.prepedidos$ = this.prepedidoBuscaService.prepedidos$;
    this.prepedidoBuscaService.atualizar();
    this.prepedidos$.subscribe(r => this.dataSourceTabelaDesktop = r);
  }

  voltar() {
    this.location.back();
  }
  displayedColumns: string[] = ['numeroPrepedido', 'dataPrePedido', 'nomeCliente', 'valoTotal'];
  dataSourceTabelaDesktop = null;
}

