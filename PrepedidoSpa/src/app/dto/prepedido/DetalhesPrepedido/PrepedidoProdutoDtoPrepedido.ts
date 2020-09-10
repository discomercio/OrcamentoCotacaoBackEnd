export class PrepedidoProdutoDtoPrepedido {
    Fabricante: string;
    Fabricante_Nome: string;
    NormalizacaoCampos_Produto: string;
    Descricao: string;
    Obs: string;
    Qtde: number | null;
    Permite_Ra_Status: number;
    BlnTemRa: boolean;
    NormalizacaoCampos_CustoFinancFornecPrecoListaBase: number | null;
    NormalizacaoCampos_Preco_NF: number | null;
    NormalizacaoCampos_Preco_Lista: number;
    NormalizacaoCampos_Desc_Dado: number | null;
    NormalizacaoCampos_Preco_Venda: number;
    VlTotalItem: number | null;
    VlTotalRA: number;
    Comissao: number | null;
    TotalItemRA: number | null;
    TotalItem: number | null;
    AlterouValorRa: boolean | null;
    AlterouVlVenda: boolean | null;
    //verificar a necessidade dessa variavel
    Qtde_estoque_total_disponivel: number | null;
    ProdutoPai: string
    CustoFinancFornecCoeficiente : number |null;
}
