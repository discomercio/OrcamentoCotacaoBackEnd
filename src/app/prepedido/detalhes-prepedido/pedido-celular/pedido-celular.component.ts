import { Component, OnInit, Input } from '@angular/core';
import { PedidoDto } from 'src/app/dto/pedido/detalhesPedido/PedidoDto';

@Component({
  selector: 'app-pedido-celular',
  templateUrl: './pedido-celular.component.html',
  styleUrls: ['./pedido-celular.component.scss']
})
export class PedidoCelularComponent implements OnInit {

  @Input() pedido: PedidoDto = null;

  constructor() { }

  ngOnInit() {
  }

}
