import { ProdutoComboDto } from "./ProdutoComboDto";

export class SelectProdInfo {
    //entrada
    public produtoComboDto: ProdutoComboDto;
    //entrada e saída
    public Fabricante: string;
    public Fabricante_Nome: string;
    public Produto: string;
    public Qte: number;

    //saída
    public ClicouOk: boolean;

}