import { PedidoProdutosDtoPedido } from './PedidoProdutosDtoPedido';
import {DetalhesNFPedidoDtoPedido } from './DetalhesNFPedidoDtoPedido';
import {DetalhesFormaPagamentos } from './DetalhesFormaPagamentos';
import { DadosClienteCadastroDto } from '../../ClienteCadastro/DadosClienteCadastroDto';

export class PedidoDto     {
        NumeroPedido:    string;
         StatusHoraPedido: string;//Verificar se todos pedidos marcam a data também
    DataHoraPedido: Date | string;
    DadosCliente: DadosClienteCadastroDto;
    ListaProdutos: PedidoProdutosDtoPedido[];
    DetalhesNF: DetalhesNFPedidoDtoPedido;
    DetalhesFormaPagto: DetalhesFormaPagamentos;
} 
