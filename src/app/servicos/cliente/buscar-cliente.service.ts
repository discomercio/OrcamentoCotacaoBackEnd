import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { Observable } from 'rxjs';
import { StringUtils } from 'src/app/utils/stringUtils';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';

@Injectable({
  providedIn: 'root'
})
export class BuscarClienteService {

  constructor(private readonly http: HttpClient) { }

  public buscar(cpfCnpj: string): Observable<ClienteCadastroDto> {
    //adiciona todos os parametros por nome
    let params = new HttpParams();
    params = params.append('cnpj_cpf', cpfCnpj);

    return this.http.get<ClienteCadastroDto>(environment.apiUrl + 'cliente/buscarCliente/' + StringUtils.retorna_so_digitos(cpfCnpj));
  }

  public atualizarCliente(dadosClienteCadastroDto:DadosClienteCadastroDto){
    return this.http.post<string[]>(environment.apiUrl + 'cliente/atualizarClienteparcial', dadosClienteCadastroDto);
  }
}

