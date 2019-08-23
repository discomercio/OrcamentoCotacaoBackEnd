import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { environment } from '../../../environments/environment'
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class PrepedidoRemoverService {

  constructor(private http: HttpClient) { }
  public remover(numeroPrepedido): Observable<any> {
    return this.http.put(environment.apiUrl + 'prepedido/removerPrePedido/' + numeroPrepedido, numeroPrepedido);
  }

}
