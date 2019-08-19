import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class PrepedidoComboCpfcnpjService {

  constructor(private http: HttpClient) { }
  public obter(): Observable<string[]> {
    return this.http.get<string[]>(environment.apiUrl + 'prepedido/listarCpfPrepedidosCombo');
  }

}
