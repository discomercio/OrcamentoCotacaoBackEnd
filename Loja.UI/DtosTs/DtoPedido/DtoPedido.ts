import { DtoEnderecoEntregaClienteCadastro } from "../DtoCliente/DtoEnderecoEntregaClienteCadastro";
import { PedidoProdutosDtoPedido } from "./DtoPedidoProdutosPedido";
import { DtoDetalhesPedido } from "./DtoDetalhesPedido";
import { DtoFormaPagtoCriacao } from "./DtoFormaPagtoCriacao";

export class DtoPedido {
    NumeroPrePedido: string;
    //StatusHoraPedido: StatusPedidoDtoPedido;//Verificar se todos pedidos marcam a data também
    DataHoraPedido: string;
    DadosCliente: DtoDadosClienteCadastro;
    EnderecoEntrega: DtoEnderecoEntregaClienteCadastro;
    ListaProdutos: PedidoProdutosDtoPedido[];
    TotalFamiliaParcelaRA: number;
    PermiteRAStatus: number;
    OpcaoPossuiRA: string;
    CorTotalFamiliaRA: string;
    PercRT: number | null;
    ValorTotalDestePedidoComRA: number | null;
    VlTotalDestePedido: number | null;
    DetalhesPrepedido: DtoDetalhesPedido;
    FormaPagto: string[];
    FormaPagtoCriacao: DtoFormaPagtoCriacao;
    St_Orc_Virou_Pedido: boolean;//se virou pedido retornar esse campo
    NumeroPedido: string;//se virou pedido retornar esse campo    
}