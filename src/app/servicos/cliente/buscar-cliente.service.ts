import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { Observable } from 'rxjs';
import { StringUtils } from 'src/app/utils/stringUtils';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';
import { EnderecoEntregaJustificativaDto } from 'src/app/dto/ClienteCadastro/EnderecoEntregaJustificativaDto';

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

  public JustificativaEndEntregaComboTemporario():EnderecoEntregaJustificativaDto[]{
    //afazer: chamar a API
    //por enquanto, retornamos uma lista fixa
   const ret:EnderecoEntregaJustificativaDto []= new Array();
   ret.push({EndEtg_cod_justificativa:"001", EndEtg_descricao_justificativa:"Casa de Veraneio"});
   ret.push({EndEtg_cod_justificativa:"002", EndEtg_descricao_justificativa:"Doação"});
   ret.push({EndEtg_cod_justificativa:"003", EndEtg_descricao_justificativa:"Nova Unidade da Empresa/filial"});
   ret.push({EndEtg_cod_justificativa:"004", EndEtg_descricao_justificativa:"Parente do Proprietário (Pais, Filhos e Irmãos)"});
   ret.push({EndEtg_cod_justificativa:"005", EndEtg_descricao_justificativa:"Residência do Proprietário"});
   ret.push({EndEtg_cod_justificativa:"006", EndEtg_descricao_justificativa:"Endereço Comercial do Proprietário"});
   ret.push({EndEtg_cod_justificativa:"008", EndEtg_descricao_justificativa:"Endereço da Obra"});
   ret.push({EndEtg_cod_justificativa:"009", EndEtg_descricao_justificativa:"Endereço Novo Cliente"});
   ret.push({EndEtg_cod_justificativa:"010", EndEtg_descricao_justificativa:"Acerto Interno"});

   return ret;
  }

}

export class JustificativaEndEntregaComboDto{
    public Codigo:string;
    public Descricao:string;
}
