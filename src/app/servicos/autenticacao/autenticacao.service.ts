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

  constructor(private http: HttpClient, private location: Location) {
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
    this.http.get(environment.apiUrl + 'acesso/fazerLogout');
    sessionStorage.setItem('token', "");
    localStorage.setItem('token', "");
  }

  public setarToken(token: string): void {
    if (this.salvar)
      localStorage.setItem("token", token);
    else
      sessionStorage.setItem("token", token);
  }

  private obterToken(): string {
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
    return true;
  }

  private _NomeUsuario: string = null;
  get authNomeUsuario(): string {
    if (this._NomeUsuario == null) {
      const token = this.obterToken();
      const user = jtw_decode(token);
      if (user)
        this._NomeUsuario = (user && user.nameid) ? user.nameid : "não logado";
    }
    return this._NomeUsuario;
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

}
