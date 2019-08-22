import { Component, OnInit } from '@angular/core';
import { TelaDesktopService } from './servicos/telaDesktop/telaDesktop.service';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  telaDesktop: boolean = true;

  constructor(private telaDesktopService: TelaDesktopService,
    private router: Router) {
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
          if (event.url == "/prepedido/lista") {
            this.router.navigateByUrl("/prepedido/consulta");
          }
          if (event.url == "/pedido/lista") {
            this.router.navigateByUrl("/pedido/consulta");
          }
        }
      }
    });
  }
  title = 'Sistema de pré-pedidos';
}
