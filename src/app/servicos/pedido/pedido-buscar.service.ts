import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpParams, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PedidoBuscarService {

  public carregando: boolean = false;
  pedidos$: BehaviorSubject<any> = new BehaviorSubject(new Object());
  errosPedidos$: BehaviorSubject<any> = new BehaviorSubject(null);

  constructor(private readonly http: HttpClient) { }

  public atualizar(numeroPedido: string): void {
    // Initialize Params Object
    let params = new HttpParams();

    //adiciona todos os parametros por nome
    params = params.append('numeroPedido', numeroPedido);
    this.carregando = true;

    this.http.get<any>(environment.apiUrl + 'pedido/buscarPedido', { params: params }).subscribe(
      {
        next: (r) => {
          this.carregando = false;
          this.pedidos$.next(r);
        },
        error: (err) => {
          this.carregando = false;
          this.errosPedidos$.next(err);
        },
        complete: () => {
          this.carregando = false;
        }

      }
    );
  }

}