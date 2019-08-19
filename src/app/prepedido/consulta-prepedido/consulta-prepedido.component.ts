import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { PrepedidoComboNumeroService } from 'src/app/servicos/prepedido/prepedido-combo-numero.service';
import { PrepedidoComboCpfcnpjService } from 'src/app/servicos/prepedido/prepedido-combo-cpfcnpj.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-consulta-prepedido',
  templateUrl: './consulta-prepedido.component.html',
  styleUrls: ['./consulta-prepedido.component.scss']
})
export class ConsultaPrepedidoComponent implements OnInit {

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
