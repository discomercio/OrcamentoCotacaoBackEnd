import { ProdutoFilhoDto } from "./ProdutoFilhoDto";


export class ProdutoCompostoDto {
    PaiFabricante: string;
    PaiFabricante_Nome: string;
    PaiProduto: string;
    Preco_total_Itens: number;
    Filhos: ProdutoFilhoDto[];
}