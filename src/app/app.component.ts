import { Component, OnInit } from '@angular/core';
import { TelaDesktopService } from './servicos/telaDesktop/telaDesktop.service';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { AutenticacaoService } from './servicos/autenticacao/autenticacao.service'
import { ImpressaoService } from './utils/impressao.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  telaDesktop = true;

  constructor(private readonly telaDesktopService: TelaDesktopService,
    public readonly autenticacaoService: AutenticacaoService,
    public readonly impressaoService:ImpressaoService,
    private readonly router: Router) {
    telaDesktopService.telaAtual$.subscribe(r => this.telaDesktop = r);
  }

  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        /*

        se estava em celular e foi para desktop, a rota 
        /prepedido/lista
        não existe, e deve ir para 
        /prepedido/consulta
        */
        if (this.telaDesktop) {
          if (event.url.toString() === "/prepedido/lista") {
            this.router.navigateByUrl("/prepedido/consulta");
          }
          if (event.url.toString() === "/pedido/lista") {
            this.router.navigateByUrl("/pedido/consulta");
          }
        }
      }
    });
  }

  logout() {
    this.autenticacaoService.authLogout();
    this.router.navigateByUrl("/login");
  }

  title = 'Sistema de pré-pedidos';
}
