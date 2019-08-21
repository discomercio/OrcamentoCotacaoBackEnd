import { Component, OnInit } from '@angular/core';
import { PrepedidoComboNumeroService } from 'src/app/servicos/prepedido/prepedido-combo-numero.service';
import { PrepedidoComboCpfcnpjService } from 'src/app/servicos/prepedido/prepedido-combo-cpfcnpj.service';
import { Observable } from 'rxjs';
import { DataUtils } from 'src/app/utils/dataUtils';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { PrepedidoBuscaService } from 'src/app/servicos/prepedido/prepedido-busca.service';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { Router } from '@angular/router';

@Component({
  selector: 'app-consulta-base',
  templateUrl: './consulta-base.component.html',
  styleUrls: ['./consulta-base.component.scss']
})
export class ConsultaBaseComponent extends TelaDesktopBaseComponent implements OnInit {

  //listas para os combos
  optionsNumeroPrePedidoBusca$: Observable<string[]>;
  optionsClienteBusca$: Observable<string[]>;


  constructor(private prepedidoComboNumeroService: PrepedidoComboNumeroService,
    private prepedidoComboCpfcnpjService: PrepedidoComboCpfcnpjService,
    telaDesktopService: TelaDesktopService,
    private router: Router,
    public prepedidoBuscaService: PrepedidoBuscaService) {
    super(telaDesktopService);

    //carrega os combos
    this.optionsNumeroPrePedidoBusca$ = this.prepedidoComboNumeroService.obter();
    this.optionsClienteBusca$ = this.prepedidoComboCpfcnpjService.obter();

  }

  ngOnInit() {
  }

  buscar() {
    this.prepedidoBuscaService.atualizar();

    //na celular é outra tela, temos que navegar explicitamente
    if (!this.telaDesktop)
      this.router.navigateByUrl('/prepedido/lista');
  }
}
