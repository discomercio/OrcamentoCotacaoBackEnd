import { Component, OnInit } from '@angular/core';
import { PrepedidoComboNumeroService } from 'src/app/servicos/prepedido/prepedido-combo-numero.service';
import { PrepedidoComboCpfcnpjService } from 'src/app/servicos/prepedido/prepedido-combo-cpfcnpj.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-consulta-base',
  templateUrl: './consulta-base.component.html',
  styleUrls: ['./consulta-base.component.scss']
})
export class ConsultaBaseComponent implements OnInit {

  //campos do formulário
  clienteBusca: string;
  optionsClienteBusca$: Observable<string[]>;
  numeroPrePedidoBusca: string;
  optionsNumeroPrePedidoBusca$: Observable<string[]>;
  dataInicial: Date;
  dataFinal: Date;
  tipoBuscaAndamento: boolean;
  tipoBuscaPedido: boolean;

  constructor(private prepedidoComboNumeroService: PrepedidoComboNumeroService,
    private prepedidoComboCpfcnpjService: PrepedidoComboCpfcnpjService) {

    //carrega os combos
    this.optionsNumeroPrePedidoBusca$ = this.prepedidoComboNumeroService.obter();
    this.optionsClienteBusca$ = this.prepedidoComboCpfcnpjService.obter();

  }

  ngOnInit() {
  }

}
