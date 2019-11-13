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
    private readonly router: Router) {
    telaDesktopService.telaAtual$.subscribe(r => this.telaDesktop = r);

  }


  ngOnInit(): void {

    this.carregarEstilo(false);

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

  //carrega o estilo conforme o cliente. se for o caso, espera carregar para ir para a home
  //chamado do LoginformularioComponent
  public carregarEstilo(fazendoLogin: boolean): void {
    const head = document.getElementsByTagName('head')[0];

    //se já tiver o estilo nomeado, remove ele
    let estiloAtual = document.getElementById(
      'estiloCliente'
    ) as HTMLLinkElement;
    if (estiloAtual) {
      estiloAtual.parentNode.removeChild(estiloAtual);
    }

    //cria de novo
    const style = document.createElement('link');
    style.id = 'estiloCliente';
    style.rel = 'stylesheet';
    style.href = this.autenticacaoService.arquivoEstilos();
    if (fazendoLogin) {
      //se estiver fazenod o login, ao terminar de carregar vamos para a home
      style.onload = () => {
        this.router.navigate(['/']);
      };
    }

    head.appendChild(style);
  }

  logout() {

    //fazer a mudança de estilos
    this.autenticacaoService.authLogout();
    this.carregarEstilo(false);
    this.router.navigateByUrl("/login");
  }

  title = 'Sistema de pré-pedidos';
}
