import { DtoAviso } from "../Avisos/DtoAviso";
import { DtoParcUnica } from "./DtoParcUnica";
import { DtoParcComEntrada } from "./DtoParcComEntrada";
import { DtoParcComEntPrestacao } from "./DtoParcComEntPrestacao";
import { DtoParcSemEntradaPrimPrest } from "./DtoParcSemEntradaPrimPrest";
import { DtoParcSemEntPrestacao } from "./DtoParcSemEntPrestacao";

export class DtoFormaPagto {
    ListaAvista: DtoAviso[];
    ListaParcUnica: DtoParcUnica[];
    ParcCartaoInternet: boolean;
    ParcCartaoMaquineta: boolean;
    ListaParcComEntrada: DtoParcComEntrada[];
    ListaParcComEntPrestacao: DtoParcComEntPrestacao[];
    ListaParcSemEntPrimPrest: DtoParcSemEntradaPrimPrest[];
    ListaParcSemEntPrestacao: DtoParcSemEntPrestacao[];
}