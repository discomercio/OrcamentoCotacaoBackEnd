import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';
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

  @ViewChild("divprincipal", { static: true }) divprincipal: ElementRef;
  ngAfterViewInit() {
    setTimeout(() => {

      //para acertar o scale
      let elem = this.divprincipal.nativeElement;
      const folga = 20 + 40;
      const escala = (window.innerWidth - folga) / (elem.clientWidth);
      elem.style.transform = "scale( " + escala + ")";

      //e para desligar o scroll do pai dele
      const pai = document.getElementById("content");
      pai.style.overflowX = "hidden";
    }, 1);
  }

}
