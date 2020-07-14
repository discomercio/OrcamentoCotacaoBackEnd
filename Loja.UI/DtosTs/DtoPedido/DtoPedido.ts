///<reference path="../DtoCliente/DtoDadosClienteCadastro.ts" />
///<reference path="../DtoCliente/DtoEnderecoEntregaClienteCadastro.ts" />
///<reference path="../DtoPedido/DtoPedidoProdutosPedido.ts" />
///<reference path="../DtoPedido/DtoDetalhesPedido.ts" />
///<reference path="../DtoPedido/DtoFormaPagtoCriacao.ts" />

class DtoPedido {
    NumeroPrePedido: string;
    //StatusHoraPedido: StatusPedidoDtoPedido;//Verificar se todos pedidos marcam a data também
    DataHoraPedido: string;
    DadosCliente: DtoDadosClienteCadastro;
    EnderecoEntrega: DtoEnderecoEntregaClienteCadastro;
    ListaProdutos: DtoPedidoProdutosPedido[];
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