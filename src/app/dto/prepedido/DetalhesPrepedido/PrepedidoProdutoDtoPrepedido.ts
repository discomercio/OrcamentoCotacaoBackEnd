export class PrepedidoProdutoDtoPrepedido {
    Fabricante: string;
    Fabricante_Nome: string;
    NumProduto: string;
    Descricao: string;
    Obs: string;
    Qtde: number | null;
    Permite_Ra_Status: number;
    BlnTemRa: boolean;
    Preco: number | null;
    Preco_Lista: number | null;
    VlLista: number;
    Desconto: number | null;
    VlUnitario: number;
    VlTotalItem: number | null;
    VlTotalRA: number;
    Comissao: number | null;
    TotalItemRA: number | null;
    TotalItem: number | null;
    AlterouValorRa:boolean| null;
    AlterouVlVenda:boolean| null;
    //verificar a necessidade dessa variavel
    Qtde_estoque_total_disponivel: number | null;

}
