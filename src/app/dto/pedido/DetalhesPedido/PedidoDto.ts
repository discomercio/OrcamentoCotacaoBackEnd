import { StatusPedidoDtoPedido } from './StatusPedidoDtoPedido';

import { DadosClienteCadastroDto } from '../../ClienteCadastro/DadosClienteCadastroDto';

import { EnderecoEntregaDtoClienteCadastro } from '../../ClienteCadastro/EnderecoEntregaDTOClienteCadastro';

import { PedidoProdutosDtoPedido } from './PedidoProdutosDtoPedido';

import { DetalhesNFPedidoDtoPedido } from './DetalhesNFPedidoDtoPedido';

import { DetalhesFormaPagamentos } from './DetalhesFormaPagamentos';

import { ProdutoDevolvidoDtoPedido } from './ProdutoDevolvidoDTOPedido';

import { PedidoPerdasDtoPedido } from './PedidoPerdasDtoPedido';

import { OcorrenciasDtoPedido } from './OcorrenciasDtoPedido';

import { BlocoNotasDtoPedido } from './BlocoNotasDtoPedido';

import { BlocoNotasDevolucaoMercadoriasDtoPedido } from './BlocoNotasDevolucaoMercadoriasDtoPedido';

export class PedidoDto {
    NumeroPedido: string;
    StatusHoraPedido: StatusPedidoDtoPedido;//Verificar se todos pedidos marcam a data também
    DataHoraPedido: Date | string | null;
    DadosCliente: DadosClienteCadastroDto;
    EnderecoEntrega: EnderecoEntregaDtoClienteCadastro;
    ListaProdutos: PedidoProdutosDtoPedido[];
    TotalFamiliaParcelaRA: number;
    PermiteRAStatus: number;
    OpcaoPossuiRA: string;
    PercRT: number | null;
    ValorTotalDestePedidoComRA: number | null;
    VlTotalDestePedido: number | null;
    DetalhesNF: DetalhesNFPedidoDtoPedido;
    DetalhesFormaPagto: DetalhesFormaPagamentos;
    ListaProdutoDevolvido: ProdutoDevolvidoDtoPedido[];
    ListaPerdas: PedidoPerdasDtoPedido[];
    ListaOcorrencia: OcorrenciasDtoPedido[];
    BlocoNotas: BlocoNotasDtoPedido;
    ListaBlocoNotasDevolucao: BlocoNotasDevolucaoMercadoriasDtoPedido[];


}

