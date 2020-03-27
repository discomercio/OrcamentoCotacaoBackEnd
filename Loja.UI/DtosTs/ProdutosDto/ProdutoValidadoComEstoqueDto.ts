import { ProdutoDto } from "./ProdutoDto";

export class ProdutoValidadoComEstoqueDto {
    Produto: ProdutoDto;
    ListaErros: Array<string>;
}