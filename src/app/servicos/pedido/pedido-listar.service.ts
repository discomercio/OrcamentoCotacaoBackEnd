import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ParamsBuscaPedido } from './paramsBuscaPedido';
import { BehaviorSubject } from 'rxjs';
import { DataUtils } from 'src/app/utils/dataUtils';
import { environment } from 'src/environments/environment';
import { PedidosDtoPedido } from '../../dto/pedido/pedidosDtoPedido'

@Injectable({
  providedIn: 'root'
})
export class PedidoListarService {

  //a tela da busca edita diretamente as variáveis aqui dentro
  public paramsBuscaPedido: ParamsBuscaPedido = new ParamsBuscaPedido();

  constructor(private readonly http: HttpClient) {
    //incializa as datas
    this.paramsBuscaPedido.dataFinal = DataUtils.formataParaFormulario(new Date());
    this.paramsBuscaPedido.dataInicial = DataUtils.formataParaFormulario(DataUtils.somarDias(new Date(), -60));

  }

  public carregando: boolean = false;
  pedidos$: BehaviorSubject<PedidosDtoPedido[]> = new BehaviorSubject(new Array());
  errosPedidos$: BehaviorSubject<any> = new BehaviorSubject(null);

  public atualizar(): void {
    // Initialize Params Object
    let params = new HttpParams();

    //adiciona todos os parametros por nome
    //os nomes são todos iguais, devia ter um jeito de fazer isso automaticamente...
    params = params.append('clienteBusca', this.paramsBuscaPedido.clienteBusca);
    params = params.append('numPedido', this.paramsBuscaPedido.numeroPedido);
    params = params.append('dataInicial', this.paramsBuscaPedido.dataInicial);
    params = params.append('dataFinal', this.paramsBuscaPedido.dataFinal);
    /*
  //tipo de busca:
  tipoBusca:
          Todos = 0, PedidosEncerrados = 1, PedidosEmAndamento = 2
  */
    let tipoBusca: number = 0;
    if (this.paramsBuscaPedido.tipoBuscaEmAndamento && !this.paramsBuscaPedido.tipoBuscaEncerrado)
      tipoBusca = 2;
    if (!this.paramsBuscaPedido.tipoBuscaEmAndamento && this.paramsBuscaPedido.tipoBuscaEncerrado)
      tipoBusca = 1;
    params = params.append('tipoBusca', tipoBusca.toString());

    this.carregando = true;

    this.http.get<PedidosDtoPedido[]>(environment.apiUrl + 'pedido/listarPedidos', { params: params }).subscribe(
      {
        next: (r) => {
          this.carregando = false;
          this.pedidos$.next(r);
        },
        error: (err) => {
          this.carregando = false;
          this.errosPedidos$.next(err);
        },
        complete: () => {
          this.carregando = false;
        }

      }
    );
  }


}
