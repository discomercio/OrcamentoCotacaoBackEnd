export class PedidoProdutosPedidoDto {
    Fabricante: string;
    NumProduto: string;
    Descricao: string;
    Qtde: number | null;
    Faltando: number | null;
    CorFaltante: string;
    Preco: number | null;
    Preco_Lista: number | null;
    VlLista: number;
    Desconto: number | null;
    VlUnitario: number;
    VlTotalItem: number | null;
    VlTotalItemComRA: number | null;
    VlVenda: number | null;
    VlTotal: number | null;
    Comissao: number | null;
    TotalItem: number | null;
    AlterouValorRa: boolean | null;
    AlterouVlVenda: boolean | null;
}