import { ProdutoCompostoDto } from "./ProdutoCompostoDto";
import { ProdutoDto } from "./ProdutoDto";

export class ProdutoComboDto {
    ProdutoDto: ProdutoDto[];
    ProdutoCompostoDto: ProdutoCompostoDto[];
}