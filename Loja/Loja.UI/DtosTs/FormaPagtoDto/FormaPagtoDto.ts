import { ParcUnicaDto } from "./ParcUnicaDto";
import { ParcComEntradaDto } from "./ParcComEntradaDto";
import { ParcComEntPrestacaoDto } from "./ParcComEntPrestacaoDto";
import { ParcSemEntradaPrimPrestDto } from "./ParcSemEntradaPrimPrestDto";
import { ParcSemEntPrestacaoDto } from "./ParcSemEntPrestacaoDto";
import { AvisoDto } from "../Avisos/AvisoDto";

export class FormaPagtoDto {
    ListaAvista: AvisoDto[];
    ListaParcUnica: ParcUnicaDto[];
    ParcCartaoInternet: boolean;
    ParcCartaoMaquineta: boolean;
    ListaParcComEntrada: ParcComEntradaDto[];
    ListaParcComEntPrestacao: ParcComEntPrestacaoDto[];
    ListaParcSemEntPrimPrest: ParcSemEntradaPrimPrestDto[];
    ListaParcSemEntPrestacao: ParcSemEntPrestacaoDto[];
}