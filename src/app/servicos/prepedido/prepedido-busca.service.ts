import { Injectable } from '@angular/core';
import { ParamsBuscaPrepedido } from './paramsBuscaPrepedido';
import { DataUtils } from 'src/app/utils/dataUtils';
import { PrepedidosCadastradosDtoPrepedido } from 'src/app/dto/prepedido/prepedidosCadastradosDtoPrepedido';
import { Observable, BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PrepedidoBuscaService {

  //a tela da busca edita diretamente as variáveis aqui dentro
  public paramsBuscaPrepedido: ParamsBuscaPrepedido = new ParamsBuscaPrepedido();

  constructor(private http: HttpClient) {
    //incializa as datas
    this.paramsBuscaPrepedido.dataInicial = DataUtils.formataParaFormulario(new Date());
    this.paramsBuscaPrepedido.dataFinal = DataUtils.formataParaFormulario(DataUtils.somarDias(new Date(), -60));

  }

  prepedidos$: BehaviorSubject<PrepedidosCadastradosDtoPrepedido[]> = new BehaviorSubject(new Array());

  public atualizar(): void {
    // Initialize Params Object
    let params = new HttpParams();

    //adiciona todos os parametros por nome
    //os nomes são todos iguais, devia ter um jeito de fazer isso automaticamente...
    params = params.append('clienteBusca', this.paramsBuscaPrepedido.clienteBusca);
    params = params.append('numeroPrePedido', this.paramsBuscaPrepedido.numeroPrePedido);
    params = params.append('dataInicial', this.paramsBuscaPrepedido.dataInicial);
    params = params.append('dataFinal', this.paramsBuscaPrepedido.dataFinal);
    //TODO: tipo de busca, não consta na API

    //TODO: mostrar erros
    this.http.get<PrepedidosCadastradosDtoPrepedido[]>(environment.apiUrl + 'prepedido/listarPrePedidos', { params: params }).subscribe(
      r=> this.prepedidos$.next(r)
    );
  }


}
