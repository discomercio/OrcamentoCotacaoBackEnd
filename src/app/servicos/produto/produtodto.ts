//afazer: remover e passar para os DTOs

//método da API: ProdutoComboDto ListarProdutosCombo
export class ProdutoComboDto {
    public ProdutoDto: ProdutoDto[];
    public ProdutoCompostoDto: ProdutoCompostoDto[];
}

//esta lista depende, para fazer o cálculo do estoque, do DadosClienteCadastroDto.Tipo do cliente (PF ou PJ) e UF do cliente
//afazer: conformar com hamilton, está usando UF do endereço e não do endereço de entrega
export class ProdutoDto {
    public Fabricante: string;
    public Produto: string;
    public Descricao_html: string;
    public Preco_lista: number | null; //se for um produto composto, deve ser a soma dos preços dos componentes
    public Estoque: number | null; //null se for um produto composto
    public Alertas: string[];
}


//para facilitar a busca: CodigoFabProd: string;
//verificado no código: um produto SEMPRE pode ser vendido separadamente

// List<ProdutoCompostoDto> ListarProdutosCompostos
export class ProdutoCompostoDto {
    PaiFabricante: string;
    PaiProduto: string;
    Filhos: ProdutoFilhoDto[];
}
export class ProdutoFilhoDto {
    Fabricante: string;
    Produto: string;
    Qtde: number;
}
