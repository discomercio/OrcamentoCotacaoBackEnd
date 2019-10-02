import { PrepedidoProdutoDtoPrepedido } from './PrepedidoProdutoDtoPrepedido';
import { DadosClienteCadastroDto } from '../../ClienteCadastro/DadosClienteCadastroDto';
import { EnderecoEntregaDtoClienteCadastro } from '../../ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { DetalhesDtoPrepedido } from './DetalhesDtoPrepedido';
import { FormaPagtoCriacaoDto } from './FormaPagtoCriacaoDto';

export class PrePedidoDto {
    NumeroPrePedido: string;
    //StatusHoraPedido: StatusPedidoDtoPedido;//Verificar se todos pedidos marcam a data também
    DataHoraPedido: string;
    DadosCliente: DadosClienteCadastroDto;
    EnderecoEntrega: EnderecoEntregaDtoClienteCadastro;
    ListaProdutos: PrepedidoProdutoDtoPrepedido[];
    TotalFamiliaParcelaRA: number;
    PermiteRAStatus: number;
    OpcaoPossuiRA: string;
    CorTotalFamiliaRA: string;
    PercRT: number | null;
    ValorTotalDestePedidoComRA: number | null;
    VlTotalDestePedido: number | null;
    DetalhesPrepedido: DetalhesDtoPrepedido;
    FormaPagto: string[];
    FormaPagtoCriacao: FormaPagtoCriacaoDto;
    St_Orc_Virou_Pedido: boolean;//se virou pedido retornar esse campo
    NumeroPedido: string;//se virou pedido retornar esse campo
}
