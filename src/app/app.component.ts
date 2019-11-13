import { Component, OnInit, Inject } from '@angular/core';
import { TelaDesktopService } from './servicos/telaDesktop/telaDesktop.service';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { AutenticacaoService } from './servicos/autenticacao/autenticacao.service'
import { ImpressaoService } from './utils/impressao.service';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  telaDesktop = true;

  constructor(private readonly telaDesktopService: TelaDesktopService,
    public readonly autenticacaoService: AutenticacaoService,
    public readonly impressaoService: ImpressaoService,
    private readonly router: Router,
    @Inject(DOCUMENT) private document: Document) {
    telaDesktopService.telaAtual$.subscribe(r => this.telaDesktop = r);

  }


  ngOnInit(): void {

    // this.loadStyle(this.autenticacaoService.arquivoEstilos());

    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {

        this.loadStyle(this.autenticacaoService.arquivoEstilos());
        //recarrega o estilo sempre que navegou

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

  public loadStyle(styleName: string):void {
    const head = this.document.getElementsByTagName('head')[0];

    let themeLink = this.document.getElementById(
      'client-theme'
    ) as HTMLLinkElement;
    if (themeLink) {
      themeLink.href = styleName;
    } else {
      const style = this.document.createElement('link');
      style.id = 'client-theme';
      style.rel = 'stylesheet';
      style.href = styleName;
      
      head.appendChild(style);
    }
  }



  logout() {
    
    //fazer a mudança de estilos
    this.autenticacaoService.authLogout();
    // this.loadStyle(this.autenticacaoService.arquivoEstilos());
    this.router.navigateByUrl("/login");
  }

  title = 'Sistema de pré-pedidos';
}
