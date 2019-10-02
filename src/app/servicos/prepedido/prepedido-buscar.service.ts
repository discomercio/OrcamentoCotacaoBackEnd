import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { HttpParams, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { PedidoDto } from 'src/app/dto/pedido/detalhesPedido/PedidoDto2';
import { PrePedidoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrePedidoDto';

@Injectable({
  providedIn: 'root'
})
export class PrepedidoBuscarService {

  public carregando: boolean = false;
  private pedidos$: Observable<PrePedidoDto> = new Observable();

  constructor(private readonly http: HttpClient) { }

  public buscar(numeroPrePedido: string): Observable<PrePedidoDto> {
    // Initialize Params Object
    let params = new HttpParams();

    //adiciona todos os parametros por nome
    params = params.append('numPrepedido', numeroPrePedido);
    this.carregando = true;

    this.pedidos$ = Observable.create(observer => {
      this.http.get<any>(environment.apiUrl + 'prepedido/buscarPrePedido', { params: params }).toPromise()
        .then(response => {
          if (response)
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

  public Obter_Permite_RA_Status(): Observable<number> {
    return this.http.get<any>(environment.apiUrl + 'prepedido/obter_permite_ra_status');
  }

  public cadastrarPrepedido(prePedidoDto: PrePedidoDto): Observable<string[]> {
    return this.http.post<string[]>(environment.apiUrl + 'prepedido/cadastrarPrepedido', prePedidoDto);
  }
}
