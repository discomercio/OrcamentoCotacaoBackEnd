import { ProdutoComboDto } from 'src/app/servicos/produto/produtodto';

//controle para a seleção do produto
export class SelecProdInfo {

    //entrada
    public produtoComboDto: ProdutoComboDto;

    //entrada e saída
    public Fabricante: string;
    public Produto: string;
    public Qte: number;

    //saída
    public ClicouOk: boolean;

}

