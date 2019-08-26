import { Component, OnInit } from '@angular/core';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';

@Component({
  selector: 'app-consulta-prepedido',
  templateUrl: './consulta-prepedido.component.html',
  styleUrls: ['./consulta-prepedido.component.scss']
})
export class ConsultaPrepedidoComponent extends TelaDesktopBaseComponent implements OnInit {

  constructor(telaDesktopService: TelaDesktopService) {
    super(telaDesktopService);

  }

  ngOnInit() {
  }

}
