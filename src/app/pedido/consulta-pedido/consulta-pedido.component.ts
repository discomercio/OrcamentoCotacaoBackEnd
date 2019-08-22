import { Component, OnInit } from '@angular/core';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';

@Component({
  selector: 'app-consulta-pedido',
  templateUrl: './consulta-pedido.component.html',
  styleUrls: ['./consulta-pedido.component.scss']
})
export class ConsultaPedidoComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(telaDesktopService: TelaDesktopService) {
    super(telaDesktopService);

  }

  ngOnInit() {
  }

}
