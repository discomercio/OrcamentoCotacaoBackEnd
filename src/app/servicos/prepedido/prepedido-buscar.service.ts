import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { HttpParams, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { PedidoDto } from 'src/app/dto/pedido/detalhesPedido/PedidoDto';

@Injectable({
  providedIn: 'root'
})
export class PrepedidoBuscarService {

  public carregando: boolean = false;
  private pedidos$: Observable<PedidoDto> = new Observable();

  constructor(private readonly http: HttpClient) { }

  public atualizar(numeroPrePedido: string): Observable<PedidoDto> {
    // Initialize Params Object
    let params = new HttpParams();

    //adiciona todos os parametros por nome
    params = params.append('numeroPrePedido', numeroPrePedido);
    this.carregando = true;

    this.pedidos$ = Observable.create(observer => {
      this.http.get<any>(environment.apiUrl + 'prepedido/buscarPrePedido', { params: params }).toPromise()
        .then(response => {
          if(response)
            this.carregando = false;
          observer.next(response);
          observer.complete();
        })
        .catch(err => {
          observer.error(err);
        });
    });
    return this.pedidos$;
  }

}
