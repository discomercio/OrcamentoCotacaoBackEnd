import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Location } from '@angular/common';
import * as jtw_decode from 'jwt-decode';

import { environment } from '../../../environments/environment'
import { Observable } from 'rxjs';
import { SSL_OP_CIPHER_SERVER_PREFERENCE } from 'constants';
import { Constantes } from 'src/app/dto/Constantes';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { AppComponent } from 'src/app/app.component';
import { query } from '@angular/animations';

/*
precisa instalar:
npm i jwt-decode --save

*/

@Injectable({
  providedIn: 'root'
})
export class AutenticacaoService {

  constructor(private readonly http: HttpClient, private readonly location: Location) {
    this.carregarLayout();
  }


  public alterarSenha(usuario: string, senha: string, senhaNova: string, senhaNovaConfirma: string): Observable<any> {
    return this.http.post(environment.apiUrl + 'acesso/alterarSenha', {
      apelido: usuario, senha: senha, senhaNova: senhaNova,
      senhaNovaConfirma: senhaNovaConfirma
    });
  }

  constantes = new Constantes();
  public usuarioApelidoParaAlterarSenha: string;
  public lembrarSenhaParaAlterarSenha: boolean;
  public senhaExpirada: boolean = false;
  salvar: boolean = false;
  public authLogin(usuario: string, senha: string, salvar: boolean, desligarFazendoLogin: () => void, _snackBar: MatSnackBar,
    router: Router, appComponent: AppComponent): void {
    this.lembrarSenhaParaAlterarSenha = salvar;
    this.usuarioApelidoParaAlterarSenha = usuario;
    var key = this.gerarChave();
    senha = this.CodificaSenha(senha, key);
    this.salvar = salvar;
    this._NomeUsuario = null;
    let msg = "";


    this.http.post(environment.apiUrl + 'acesso/fazerLogin', { apelido: usuario, senha: senha },
      {
        //estamos usando dessa forma, pois não estava aceitando uma "options" com mais de um parametro
        responseType: 'text',
        headers: new HttpHeaders({ 'Content-Type': 'application/json', 'responseType': 'text' })
      }).subscribe({
        next: (e) => {
          //aqui vai as condições que irão verificar se o retorno é um erro
          if (e.toString().length == 1) {
            if (e.toString() == this.constantes.ERR_USUARIO_BLOQUEADO) {
              msg = "ACESSO NEGADO";
              desligarFazendoLogin();

              _snackBar.open("Erro no login: " + msg, undefined, {
                duration: environment.esperaErros
              });
              return;
            }
            if (e.toString() == this.constantes.ERR_IDENTIFICACAO_LOJA) {
              msg = "OS DADOS INFORMADOS ESTÃO INCORRETOS.<BR>(Loja inválida)";
              desligarFazendoLogin();

              _snackBar.open("Erro no login: " + msg, undefined, {
                duration: environment.esperaErros
              });
              return;
            }
            if (e.toString() == this.constantes.ERR_SENHA_EXPIRADA) {
              this.senhaExpirada = true;
              router.navigateByUrl('/alterarsenha');
              return;
            }

          }

          //nunca desligamos o fazendo login porque, quando fizer, já vai para outra página, então nao fazemos this.fazendoLogin = false;
          this.setarToken(e);

          //o carregarEstilo já navega para a home
          appComponent.carregarEstilo(true);
        }
        ,
        error: (e) => {
          desligarFazendoLogin();
          msg = "" + ((e && e.message) ? e.message : e.toString());
          if (e && e.status === 400)
            msg = "usuário e senha inválidos."
          if (e && e.status === 0)
            msg = "servidor de autenticação não disponível."
          if (e && e.status === 500)
            msg = "erro interno no servidor de autenticação."


          _snackBar.open("Erro no login: " + msg, undefined, {
            duration: environment.esperaErros
          });

        },
      })
  }

  public authLogout(): void {
    this.authLogoutSemLayout();
    this.loja = null;
    this.carregarLayout();
  }

  public authLogoutSemLayout(): void {
    this.http.get(environment.apiUrl + 'acesso/fazerLogout').subscribe(
      e => {
        //nao fazemos nada..
      }
    );
    sessionStorage.setItem('token', "");
    localStorage.setItem('token', "");

    // localStorage.clear();
    // sessionStorage.clear();

    // sessionStorage.removeItem('token');
    // localStorage.removeItem('token');
  }

  public setarToken(token: string): void {
    if (this.salvar)
      localStorage.setItem("token", token);
    else
      sessionStorage.setItem("token", token);

    this.carregarLayout();
  }

  public gerarChave() {
    // gerar chave
    const fator: number = 1209;
    const cod_min: number = 35;
    const cod_max: number = 96;
    const tamanhoChave: number = 128;

    let chave: string = "";

    for (let i: number = 1; i < tamanhoChave; i++) {
      let k: number = (cod_max - cod_min) + 1;
      k *= fator;
      k = (k * i) + cod_min;
      k %= 128;
      chave += String.fromCharCode(k);
    }

    return chave;
  }

  public CodificaSenha(origem: string, chave: string): string {

    let i: number = 0;
    let i_chave: number = 0;
    let i_dado: number = 0;
    let s_origem: string = origem;
    let letra: string = "";
    let s_destino: string = "";

    if (s_origem.length > 15) {
      s_origem = s_origem.substr(0, 15);
    }

    for (i = 0; i < s_origem.length; i++) {
      letra = chave.substr(i, 1);
      i_chave = (letra.charCodeAt(0) * 2) + 1;
      i_dado = s_origem.substr(i, 1).charCodeAt(0) * 2;
      let contaMod = i_chave ^ i_dado;
      s_destino += String.fromCharCode(contaMod);
    }

    s_origem = s_destino;
    s_destino = "";
    let destino = "";

    for (i = 0; i < s_origem.length; i++) {
      letra = s_origem.substr(i, 1);
      i_chave = letra.charCodeAt(0);
      let hexNumber = i_chave.toString(16);

      while (hexNumber.length < 2) {
        hexNumber = hexNumber.padStart(2, '0');
      }
      destino += hexNumber;
    }
    while (destino.length < 30) {
      destino = "0" + destino;
    }
    s_destino = "0x" + destino;
    return s_destino;
  }

  public obterToken(): string {
    var ret = this.obterTokenInterno();
    //menos de 3 caracteres consiederamos algum codigo de erro
    if (ret && ret.toString().length < 3)
      ret = null;
    return ret;
  }
  private obterTokenInterno(): string {
    //tentamos nos dois lugares
    var ret = localStorage.getItem("token");
    if (ret)
      return ret;
    return sessionStorage.getItem("token");
  }

  public authEstaLogado(): boolean {
    const token = this.obterToken();

    if (!token)
      return false;
    if (token.trim() == "")
      return false;
    if (token == this.constantes.ERR_USUARIO_BLOQUEADO)
      return false;

    const user = jtw_decode(token);
    if (!user)
      return false;



    //vamos ver se extá expirado
    const expira: Date = new Date(user.exp * 1000);
    if (expira < new Date())
      return false;

    return true;
  }

  private _NomeUsuario: string = null;
  get authNomeUsuario(): string {
    if (this._NomeUsuario == null) {
      const token = this.obterToken();
      const user = jtw_decode(token);
      this._NomeUsuario = (user && user.nameid) ? user.nameid : "não logado";
    }
    return this._NomeUsuario;
  }

  private renovacaoPendnete: boolean = false;
  public renovarTokenSeNecessario(): void {
    const token = this.obterToken();
    if (!token)
      return;
    if (token.trim() == "")
      return;
    if (this.renovacaoPendnete)
      return;
    const user = jtw_decode(token);
    const expira: Date = new Date(user.exp * 1000);
    const milisegexpira: number = (expira as any) - (new Date() as any);
    var segexpira = milisegexpira / 1000;
    if (segexpira < environment.minutosRenovarTokenAntesExpirar * 60)
      this.renovarToken();
  }
  private renovarToken(): void {
    this.renovacaoPendnete = true;
    this.http.get(environment.apiUrl + 'acesso/RenovarToken').subscribe(
      {
        next: (e) => {
          this.setarToken(e as string);
          this.renovacaoPendnete = false;
        },
        error: () => { this.renovacaoPendnete = false; },
        complete: () => { this.renovacaoPendnete = false; }
      }
    );
  }



  //Gabriel criar metodo para carregar o css
  public arquivoLogo(): string {
    return this._logo;
  }
  public arquivoEstilos(): string {
    return this._estilo;
  }
  private _estilo: string = null;
  private _logo: string = null;
  private loja: string = null;
  private carregarLayout(): void {
    //tentamos obter a loja do token. se nao tiver, fica com null
    // let loja: string = null;
    if (this.authEstaLogado()) {
      const token = this.obterToken();
      const user = jtw_decode(token);
      this.loja = (user && user.family_name) ? user.family_name : null;
    }

    //define o estilo e o logo baseado na loja
    if (this.loja == null) {
      this._estilo = "";
      return;
    }
    if (this.loja == "205" ||
      this.loja == "206" ||
      this.loja == "207" ||
      this.loja == "208") {
      this._estilo = "assets/Unis.css";
      //passar o logo tb
      this._logo = "assets/LogoUnis.png";

      this.CarregarIconUnis();

    }
    else if(this.loja == "202"){
      this._logo = "assets/bonshop.png";
      this._estilo = "assets/shopVendas.css";

      this.CarregarIconBonshop();
    }
    else{
      this._estilo = "assets/shopVendas.css";
      this._logo = "assets/Logo-ShopVendas.png";
      this.CarregarIconShopVendas();
    }
    // if (this.loja == "203" ||
    //   this.loja == "204") {
    //   this._estilo = "assets/shopVendas.css";
    //   //passar o logo tb
    //   this._logo = "assets/Logo-ShopVendas.png";

    //   this.CarregarIconShopVendas();
    // }

  }
  //fim
  private CarregarIconShopVendas(): void {
    //teste para trocar o ico
    const head = document.getElementsByTagName('head')[0];
    let favicon = document.getElementById('favicon') as HTMLLinkElement;
    favicon.href = 'assets/favicon.ico';
    head.appendChild(favicon);

  }
  private CarregarIconUnis(): void {
    const head = document.getElementsByTagName('head')[0];
    let favicon = document.getElementById('favicon') as HTMLLinkElement;
    favicon.href = 'assets/icones/ico-unis-16x16.ico';
    head.appendChild(favicon);
  }

  private CarregarIconBonshop():void{
    const head = document.getElementsByTagName('head')[0];
    let favicon = document.getElementById('favicon') as HTMLLinkElement;
    favicon.href = 'assets/icones/ico-bonshop.ico';//alterar
    head.appendChild(favicon);
  }
  /*
  nao queremos expor este desnecessaruiamente
  na API, devemos usar o ID de usuário do token e não como um parametro extra
  e no cliente não devemos precisar...
  get authIdUsuario(): number {
    if (!this.authEstaLogado())
      return 0;

    const token = this.oauthService.getAccessToken();
    const user = jtw_decode(token);
    let ret: number = (user && user.sub) ? user.sub : 0;
    return ret;
  }
  */
  public BuscarImgFundo(): string {
    if (this.loja == "205" ||
      this.loja == "206" ||
      this.loja == "207" ||
      this.loja == "208") {
      return "url('/assets/background-unis-filtro-branco80.jpg')";
    }
    else{
      return "url('/assets/background-shopvendas.jpg')";
    }
    // if (this.loja == "202" ||
    //   this.loja == "203" ||
    //   this.loja == "204") {

    //   return "url('/assets/background-shopvendas.jpg')";
    // }
    
  }
  public buscarAlturaImg(): string {
    if (this.loja == "205" ||
      this.loja == "206" ||
      this.loja == "207" ||
      this.loja == "208") {
      return "calc(100vh - 53px)";
    }
    else{
      return "calc(100vh - 53px)";
    }
    // if (this.loja == "202" ||
    //   this.loja == "203" ||
    //   this.loja == "204") {
    //   return "calc(100vh - 53px)";
    // }
  }

  public buscarTamanhoImg(): string {
    if (this.loja == "205" ||
      this.loja == "206" ||
      this.loja == "207" ||
      this.loja == "208") {
      return "100%";
    }
    else{
      return "100%";
    }
    // if (this.loja == "202" ||
    //   this.loja == "203" ||
    //   this.loja == "204") {
    //   return "100%";
    // }
  }


}
