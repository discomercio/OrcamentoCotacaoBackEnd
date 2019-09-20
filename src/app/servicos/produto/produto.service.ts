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

  public listarProdutosCombo(uf: string, tipo: string): Observable<ProdutoComboDto> {
    let params = new HttpParams();
    params = params.append('uf', uf);
    params = params.append('tipo', tipo);

    //return this.http.get<ProdutoComboDto>(environment.apiUrl + 'produto/listarProdutosCombo', { params: params });

    //afazer: apra testes:
    const produtoComboDto = new ProdutoComboDto();
    produtoComboDto.Produtos = new Array();
    const ps = produtoComboDto.Produtos;
    ps.push({ Fabricante: "001", Produto: "001", Descricao_html: "teste", Preco_lista: 10, Estoque: 10, Alertas: new Array() });
    ps.push({ Fabricante: "002", Produto: "002", Descricao_html: "teste2", Preco_lista: 10, Estoque: 10, Alertas: new Array() });


    const studentsObservable: Observable<ProdutoComboDto> = new Observable(observer => {
      setTimeout(() => {
        observer.next(produtoComboDto);
      }, 1000);
    });

    return studentsObservable;

  }


}
