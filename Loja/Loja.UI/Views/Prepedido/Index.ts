import { MoedaUtils } from "../../UtilTs/MoedaUtils/moedaUtils";

var moeda = new MoedaUtils();
(window as any).formatarMoedaSemPrefixo = (r: any) => moeda.formatarMoedaSemPrefixo(r);

