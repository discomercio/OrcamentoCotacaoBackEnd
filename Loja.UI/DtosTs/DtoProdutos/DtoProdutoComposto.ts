import { DtoProdutoFilho } from "./DtoProdutoFilho";

export class DtoProdutoComposto {
    PaiFabricante: string;
    PaiFabricante_Nome: string;
    PaiProduto: string;
    Preco_total_Itens: number;
    Filhos: DtoProdutoFilho[];
}