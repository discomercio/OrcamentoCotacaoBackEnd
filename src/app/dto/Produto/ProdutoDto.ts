export class ProdutoDto {
    //esta lista depende, para fazer o cálculo do estoque, do DadosClienteCadastroDto.Tipo do cliente (PF ou PJ) e UF do cliente
    //afazer: confirmar com hamilton, está usando UF do endereço e não do endereço de entrega
    Fabricante: string;
    Produto: string;
    Descricao_html: string;
    Preco_lista: number | null;
    Estoque: number;
    Alertas: string;
}
