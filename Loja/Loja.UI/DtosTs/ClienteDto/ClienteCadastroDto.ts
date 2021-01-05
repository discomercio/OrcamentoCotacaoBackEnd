import { RefBancariaDtoCliente } from "./RefBancariaDtoCliente";
import { RefComercialDtoCliente } from "./RefComercialDtoCliente";
import { DadosClienteCadastroDto } from "./DadosClienteCadastroDto";

export class ClienteCadastroDto {
    DadosCliente: DadosClienteCadastroDto;
    RefBancaria: RefBancariaDtoCliente[];
    RefComercial: RefComercialDtoCliente[];
}