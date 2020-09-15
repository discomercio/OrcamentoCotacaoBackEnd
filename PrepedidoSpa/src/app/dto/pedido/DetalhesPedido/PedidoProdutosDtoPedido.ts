export class PedidoProdutosDtoPedido {
    Fabricante: string;
    Produto: string;
    Descricao: string;
    Qtde: number | null;
    Faltando: number | null;
    CorFaltante: string;
    CustoFinancFornecPrecoListaBase: number | null;
    Preco_Lista: number;
    Desc_Dado: number | null;
    Preco_Venda: number;
    VlTotalItem: number | null;
    VlTotalItemComRA: number | null;
    Comissao: number | null;
}
