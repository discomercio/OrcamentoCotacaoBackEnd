import { EnderecoEntregaClienteCadastroDto } from "../ClienteDto/EnderecoEntregaClienteCadastroDto";
import { PedidoProdutosPedidoDto } from "./PedidoProdutosPedidoDto";
import { FormaPagtoCriacaoDto } from "./FormaPagtoCriacaoDto";
import { DetalhesPedidoDto } from "./DetalhesPedidoDto";
import { DadosClienteCadastroDto } from "../ClienteDto/DadosClienteCadastroDto";

export class PedidoDto {
    NumeroPrePedido: string;
    //StatusHoraPedido: StatusPedidoDtoPedido;//Verificar se todos pedidos marcam a data também
    DataHoraPedido: string;
    DadosCliente: DadosClienteCadastroDto;
    EnderecoEntrega: EnderecoEntregaClienteCadastroDto;
    ListaProdutos: PedidoProdutosPedidoDto[];
    TotalFamiliaParcelaRA: number;
    PermiteRAStatus: number;
    OpcaoPossuiRA: string;
    CorTotalFamiliaRA: string;
    PercRT: number | null;
    ValorTotalDestePedidoComRA: number | null;
    VlTotalDestePedido: number | null;
    DetalhesPrepedido: DetalhesPedidoDto;
    FormaPagto: string[];
    FormaPagtoCriacao: FormaPagtoCriacaoDto;
    St_Orc_Virou_Pedido: boolean;//se virou pedido retornar esse campo
    NumeroPedido: string;//se virou pedido retornar esse campo    
}