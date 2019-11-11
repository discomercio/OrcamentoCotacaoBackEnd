import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CepDto } from 'src/app/dto/Cep/CepDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CepService {

  constructor(private readonly http: HttpClient) { }

  public buscarCep(cep: string, endereco: string, uf: string, cidade: string): Observable<CepDto[]> {
    //adiciona todos os parametros por nome
    let params = new HttpParams();
    params = params.append('cep', cep);
    params = params.append('endereco', endereco);
    params = params.append('uf', uf);
    params = params.append('cidade', cidade);

    return this.http.get<CepDto[]>(environment.apiUrl + 'cep/buscarCep/', { params: params });
  }

  public BuscarUfs(): Observable<string[]> {
    return this.http.get<string[]>(environment.apiUrl + 'cep/buscarUfs');
  }

  public BuscarLocalidades(uf: string): Observable<string[]> {
    let params = new HttpParams();
    params = params.append('uf', uf);
    return this.http.get<string[]>(environment.apiUrl + 'cep/buscarLocalidades', { params: params });
  }

  public buscarCepPorEndereco(endereco: string, localidade: string, uf: string): Observable<CepDto[]> {
    let params = new HttpParams();
    params = params.append('endereco', endereco);
    params = params.append('localidade', localidade);
    params = params.append('uf', uf);
    return this.http.get<CepDto[]>(environment.apiUrl + 'cep/buscarCepPorEndereco', { params: params });
    //afazer: verificar a possibilidade de buscar apenas por estado
  }
}
