import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Location } from '@angular/common';
import * as jtw_decode from 'jwt-decode';

import { environment } from '../../../environments/environment'
import { Observable } from 'rxjs';

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

  salvar: boolean = false;
  public authLogin(usuario: string, senha: string, salvar: boolean): Observable<any> {
    this.salvar = salvar;
    this._NomeUsuario = null;

    let params = new HttpParams();
    params = params.append('apelido', usuario);
    params = params.append('senha', senha);

    return this.http.get(environment.apiUrl + 'acesso/fazerLogin', { params: params, responseType: 'text' });
  }

  public authLogout(): void {
    debugger;
    this.http.get(environment.apiUrl + 'acesso/fazerLogout').subscribe(
      e => {
        //nao fazemos nada..
      }
    );
    sessionStorage.setItem('token', "");
    localStorage.setItem('token', "");
    this.carregarLayout();
  }

  public setarToken(token: string): void {
    if (this.salvar)
      localStorage.setItem("token", token);
    else
      sessionStorage.setItem("token", token);

    this.carregarLayout();
  }

  public obterToken(): string {
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
  private carregarLayout(): void {
    //tentamos obter a loja do token. se nao tiver, fica com null
    let loja: string = null;
    if (this.authEstaLogado()) {
      const token = this.obterToken();
      const user = jtw_decode(token);
      loja = (user && user.family_name) ? user.family_name : null;
    }

    //define o estilo e o logo baseado na loja
    if (loja == null) {
      this._estilo = "";
      //passar o logo tb
      // this._logo = "../assets/log_unis.png"
      return;
    }
    if (loja == "205" ||
      loja == "206" ||
      loja == "207" ||
      loja == "208") {
      this._estilo = "../assets/Unis.css";
      //passar o logo tb
      this._logo = "../assets/LogoUnis.png"
    }
    if(loja == "202" ||
       loja == "203" || 
       loja == "204") {
      this._estilo = "../assets/shopVendas.css";
      //passar o logo tb
      this._logo = "../assets/Logo-ShopVendas.png";
    }

  }
  //fim


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

}
