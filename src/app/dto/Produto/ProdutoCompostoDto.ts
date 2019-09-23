import { ProdutoFilhoDto } from '../Prepedido/ProdutoFilhoDto';

export class ProdutoCompostoDto {
    PaiFabricante: string;
    PaiProduto: string;
    Preco_total_Itens: number;
    Filhos: ProdutoFilhoDto[];
}
