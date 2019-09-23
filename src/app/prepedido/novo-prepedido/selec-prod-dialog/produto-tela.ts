import { ProdutoDto } from 'src/app/servicos/produto/produtodto';
import { StringUtils } from 'src/app/utils/stringUtils';

//para m sotrar um produto na tela
export class ProdutoTela {
    /**
     * Constroi a partir de um ProdutoDto
     */
    constructor(public produtoDto: ProdutoDto) {
        this.stringBusca = ProdutoTela.StringSimples(ProdutoTela.FabrProd(produtoDto.Fabricante, produtoDto.Produto) + StringUtils.TextoDeHtml(produtoDto.Descricao_html));
    }

    //a busca é feita contra esta string
    public stringBusca: string;

    //se esta está visível
    public visivel = true;

    //rotina para converter para o formato da busca: sem espaços em e minúsculas
    public static StringSimples(msg: string) {
        if (!msg)
            return "";
        msg = msg.toLowerCase().replace(/ /g, '');
        return msg;
    }

    //atualiza todoa visibilidade de todo mundo em um array
    public static AtualizarVisiveis(arr: ProdutoTela[], digitado: string) {
        digitado = ProdutoTela.StringSimples(digitado);
        for (let i = 0; i < arr.length; i++) {
            let este = arr[i];
            if (digitado === "" || este.stringBusca.indexOf(digitado) >= 0) {
                este.visivel = true;
            }
            else {
                este.visivel = false;
            }

        }
    }

    //junta o fabricante e o produto
    //usado na tela e no checkbox
    public static FabrProd(fabricante: string, produto: string): string {
        if (!fabricante || !produto) {
            return "";
        }
        return fabricante + "/" + produto;
    }
}