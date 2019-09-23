import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ProdutoComboDto, ProdutoDto } from './produtodto';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProdutoService {

  constructor(private readonly http: HttpClient) { }

  public listarProdutosCombo(idCliente: string): Observable<ProdutoComboDto> {
    let params = new HttpParams();
    params = params.append('loja', "202"); //temporario
    params = params.append('id_cliente', idCliente);

    return this.http.get<ProdutoComboDto>(environment.apiUrl + 'produto/buscarProduto', { params: params });

  }


}
