import { EnderecoEntregaClienteCadastroDto } from "./EnderecoEntregaClienteCadastroDto";

export class DadosClienteCadastroDto {
    Loja: string;
    Indicador_Orcamentista: string;
    Vendedor: string;
    Id: string;
    Cnpj_Cpf: string;
    Rg: string;
    Ie: string;
    Contribuinte_Icms_Status: number;
    Tipo: string;
    Observacao_Filiacao: string;
    Nascimento: Date | string | null;
    Sexo: string;
    Nome: string;
    ProdutorRural: number;
    Endereco: string;
    Numero: string;
    Complemento: string;
    Bairro: string;
    Cidade: string;
    Uf: string;
    Cep: string;
    DddResidencial: string;
    TelefoneResidencial: string;
    DddComercial: string;
    TelComercial: string;
    Ramal: string;
    DddCelular: string;
    Celular: string;
    TelComercial2: string;
    DddComercial2: string;
    Ramal2: string;
    Email: string;
    EmailXml: string;
    Contato: string;

    public static DadosClienteCadastroDtoDeEnderecoEntregaDtoClienteCadastro(end: EnderecoEntregaClienteCadastroDto): DadosClienteCadastroDto {
        let dados: DadosClienteCadastroDto = new DadosClienteCadastroDto();
        dados.Cnpj_Cpf = end.EndEtg_cnpj_cpf;
        dados.Rg = end.EndEtg_rg;
        dados.Ie = end.EndEtg_ie;
        dados.Contribuinte_Icms_Status = end.EndEtg_contribuinte_icms_status;
        dados.Tipo = end.EndEtg_tipo_pessoa;
        dados.Nome = end.EndEtg_nome;
        dados.ProdutorRural = end.EndEtg_produtor_rural_status;
        dados.DddResidencial = end.EndEtg_ddd_res;
        dados.TelefoneResidencial = end.EndEtg_tel_res;
        dados.DddComercial = end.EndEtg_ddd_com;
        dados.TelComercial = end.EndEtg_tel_com;
        dados.Ramal = end.EndEtg_ramal_com != null ? end.EndEtg_ramal_com : "";
        dados.DddCelular = end.EndEtg_ddd_cel;
        dados.Celular = end.EndEtg_tel_cel;
        dados.DddComercial2 = end.EndEtg_ddd_com_2;
        dados.TelComercial2 = end.EndEtg_tel_com_2;
        dados.Ramal2 = end.EndEtg_ramal_com_2 != null ? end.EndEtg_ramal_com_2 : "";
        dados.Email = end.EndEtg_email;
        dados.EmailXml = end.EndEtg_email_xml;

        return dados;
    }
}