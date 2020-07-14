import { Component, OnInit, Input } from '@angular/core';
import { TelaDesktopBaseComponent } from '../../../../src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from '../../../../src/app/servicos/telaDesktop/telaDesktop.service';
import { Router } from '@angular/router';
import { url } from 'inspector';
import { AutenticacaoService } from 'src/app/servicos/autenticacao/autenticacao.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends TelaDesktopBaseComponent implements OnInit {

  @Input() menuDesktop = false;

  constructor(telaDesktopService: TelaDesktopService,
    public readonly router: Router, public readonly autenticacao: AutenticacaoService) {
    super(telaDesktopService);

  }

  ngOnInit() {
  }

  public BuscarImgFundo(): string {
    return this.autenticacao.BuscarImgFundo();
  }

  public buscarAlturaImg(): string {
    return this.autenticacao.buscarAlturaImg();
  }

  public buscarTamanhoImg(): string {
    return this.autenticacao.buscarTamanhoImg();
  }

  cliqueNovoPrepedido() {
    this.router.navigateByUrl('/novo-prepedido');
  }
}
