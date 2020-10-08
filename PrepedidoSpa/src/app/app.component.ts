import { Component, OnInit, Inject } from '@angular/core';
import { TelaDesktopService } from './servicos/telaDesktop/telaDesktop.service';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { AutenticacaoService } from './servicos/autenticacao/autenticacao.service'
import { ImpressaoService } from './utils/impressao.service';
import { DOCUMENT } from '@angular/common';
import * as jtw_decode from 'jwt-decode';
import { AlertaService } from './utils/alert-dialog/alerta.service';
import { ConsultaBaseComponent } from './prepedido/consulta-base/consulta-base.component';
import { PedidoBuscarService } from './servicos/pedido/pedido-buscar.service';
import { PrepedidoBuscarService } from './servicos/prepedido/prepedido-buscar.service';
import { PedidoListarService } from './servicos/pedido/pedido-listar.service';
import { PrepedidoListarService } from './servicos/prepedido/prepedido-listar.service';
import { environment } from 'src/environments/environment';

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
    private readonly pedidoListarService: PedidoListarService,
    private readonly prepedidoListarService: PrepedidoListarService) {
    telaDesktopService.telaAtual$.subscribe(r => this.telaDesktop = r);

  }
  public logo: string = null;


  ngOnInit(): void {

    //proteção para não publicar se a verificação da versão da API não estiver correta
    if (environment.production && (environment.versaoApi == 'DEBUG' || environment.versaoApi == 'SUBSTITUIR_VERSAO_API')) {
      alert("Ocorreu algum erro no processo de compilação. Por favor, avise o suporte técnico.")
    }

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

    this.logo = this.autenticacaoService.arquivoLogo();
    if (fazendoLogin) {
      //se estiver fazenod o login, ao terminar de carregar vamos para a home

      style.onload = () => {
        this.router.navigate(['/']);
      };
    }

    head.appendChild(style);
  }

  logout() {
    this.logo = null;
    //fazer a mudança de estilos

    //aqui estamos devolvendo o ico da Arclube
    const head = document.getElementsByTagName('head')[0];
    let favicon = document.getElementById('favicon') as HTMLLinkElement;
    favicon.href = 'favicon.ico';
    head.appendChild(favicon);
    this.autenticacaoService.authLogout();

    this.carregarEstilo(false);

    AlertaService.mostrandoErroNaoAutorizado = false;
    this.router.navigateByUrl("/login");

    //vamos criar uma método que irá chamar os métodos que serão criados dentro 
    //de todos os serviços para que seja limpo todos as variaveis
    //AFAZER: NÃO SEI SE FAZ A CHAMADA DO METODO AQUI
    this.pedidoListarService.limpar(true);
    this.prepedidoListarService.limpar(true);
  }

  title = 'Sistema de pedidos';
}
