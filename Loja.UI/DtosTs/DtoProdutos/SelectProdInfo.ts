import { DtoProdutoCombo } from "./DtoProdutoCombo";

export class SelectProdInfo {
    //entrada
    public produtoComboDto: DtoProdutoCombo;
    //entrada e saída
    public Fabricante: string;
    public Fabricante_Nome: string;
    public Produto: string;
    public Qte: number;

    //saída
    public ClicouOk: boolean;

}