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

  public JustificativaEndEntregaComboTemporario(){
    //afazer: chamar a API
    //por enquanto, retornamos uma lista fixa
   const ret:JustificativaEndEntregaComboDto []= new Array();
   ret.push({Codigo:"001", Descricao:"Casa de Veraneio"});
   ret.push({Codigo:"002", Descricao:"Doação"});
   ret.push({Codigo:"003", Descricao:"Nova Unidade da Empresa/filial"});
   ret.push({Codigo:"004", Descricao:"Parente do Proprietário (Pais, Filhos e Irmãos)"});
   ret.push({Codigo:"005", Descricao:"Residência do Proprietário"});
   ret.push({Codigo:"006", Descricao:"Endereço Comercial do Proprietário"});
   ret.push({Codigo:"008", Descricao:"Endereço da Obra"});
   ret.push({Codigo:"009", Descricao:"Endereço Novo Cliente"});
   ret.push({Codigo:"010", Descricao:"Acerto Interno"});

   return ret;
  }

}

export class JustificativaEndEntregaComboDto{
    public Codigo:string;
    public Descricao:string;
}
