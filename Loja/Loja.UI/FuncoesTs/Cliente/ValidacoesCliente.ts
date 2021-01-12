import { CpfCnpjUtils } from "../../UtilTs/CpfCnpjUtils/CpfCnpjUtils";
import { StringUtils } from "../../UtilTs/stringUtils/stringUtils";
import { Constantes } from "../../UtilTs/Constantes/Constantes";
import { ValidacoesUtils } from "../../UtilTs/ValidacoesUtils/ValidacoesUtils";
import { FormatarEndereco } from "../../UtilTs/Fomatar/FormatarEndereco";
import { ClienteCadastroUtils } from "../../UtilTs/ValidacoesUtils/ClienteCadastroUtils";
import { FormatarTelefone } from "../../UtilTs/Fomatar/Mascaras/formataTelefone";
import { ClienteCadastroDto } from "../../DtosTs/ClienteDto/ClienteCadastroDto";
import { RefComercialDtoCliente } from "../../DtosTs/ClienteDto/RefComercialDtoCliente";
import { RefBancariaDtoCliente } from "../../DtosTs/ClienteDto/RefBancariaDtoCliente";
import { DadosClienteCadastroDto } from "../../DtosTs/ClienteDto/DadosClienteCadastroDto";
import { EnderecoEntregaClienteCadastroDto } from "../../DtosTs/ClienteDto/EnderecoEntregaClienteCadastroDto";

export class ValidacoesCliente {
    constructor() {

    }


    //public dadosClienteCadastro = new DadosClienteCadastroDto();


    public ValidarDadosClienteCadastro(dados: DadosClienteCadastroDto, lstCidadesIBGE: string[],
        clienteCadastroDto: ClienteCadastroDto): string {
        let retorno = "";

        retorno = this.validarGeral(dados, true);

        let ehPf: boolean = dados.Tipo == Constantes.ID_PF;
        //validações específicas para PF e PJ
        if (ehPf) {
            retorno += this.validarGeralPf(dados);
        }
        else {
            retorno += this.validarGeralPj(dados, true);
        }

        retorno += this.validarEndereco(dados, lstCidadesIBGE);

        //inscricao estadual
        retorno += new ClienteCadastroUtils().validarInscricaoestadualIcms(dados);

        //validar telefone
        retorno += this.validarTelefones(dados, ehPf, true);

        //validar referências bancárias
        //não exigimos um número de referências, mas as que foram criadas devem estar preenchidas
        if (!ehPf) {
            for (let i = 0; i < clienteCadastroDto.RefBancaria.length; i++) {
                let este = clienteCadastroDto.RefBancaria[i];
                retorno += this.validarRefBancaria(este);
            }

            //validar referências comerciais
            //não exigimos um número de referências, mas as que foram criadas devem estar preenchidas    
            for (let i = 0; i < clienteCadastroDto.RefComercial.length; i++) {
                let este = clienteCadastroDto.RefComercial[i];
                retorno += this.validarRefComerial(este);
            }

            retorno += this.verificarRefComercialDuplicada(clienteCadastroDto.RefComercial);
        }


        return retorno;
    }

    private validarGeral(dadosClienteCadastroDto: DadosClienteCadastroDto, ehObrigatorio: boolean): string {
        let ret: string = "";
        
        if (!dadosClienteCadastroDto.Nome) {
            ret += 'Preencha o Nome/Razão Social!<br>';
        }
        if (!!dadosClienteCadastroDto.Cnpj_Cpf || (dadosClienteCadastroDto.Cnpj_Cpf.trim() !== "")) {
            if (CpfCnpjUtils.cnpj_cpf_ok(dadosClienteCadastroDto.Cnpj_Cpf)) {
                let cpf_cnpj = StringUtils.retorna_so_digitos(dadosClienteCadastroDto.Cnpj_Cpf);
                if (dadosClienteCadastroDto.Tipo == Constantes.ID_PF &&
                    cpf_cnpj.length > 11) {
                    ret += 'CPF inválido!<br>';
                }
                if (dadosClienteCadastroDto.Tipo == Constantes.ID_PJ) {
                    if (cpf_cnpj.length > 14 || cpf_cnpj.length < 14) {
                        ret += 'CNPJ inválido!<br>';
                    }
                }
            }
            else {
                ret += 'CNPJ/CPF inválido!<br>';
            }
        } else {
            ret += 'Preencha CNPJ/CPF<br>';
        }

        if (ehObrigatorio) {
            //se for PJ é obrigatório
            if (dadosClienteCadastroDto.Tipo == Constantes.ID_PJ) {
                //se estiver vazio
                if (!ValidacoesUtils.email_ok(dadosClienteCadastroDto.Email)) {
                    ret += 'E-mail inválido!<br>';
                }
            }
            if (dadosClienteCadastroDto.Tipo == Constantes.ID_PF) {
                if (!!dadosClienteCadastroDto.Email || dadosClienteCadastroDto.Email !== "") {
                    if (!ValidacoesUtils.email_ok(dadosClienteCadastroDto.Email)) {
                        ret += 'E-mail inválido!<br>';
                    }
                }
            }

            if (!!dadosClienteCadastroDto.EmailXml || dadosClienteCadastroDto.EmailXml !== "") {
                if (!ValidacoesUtils.email_ok(dadosClienteCadastroDto.EmailXml)) {
                    ret += 'E-mail (XML) inválido!<br>';
                }
            }
        }

        return ret;
    }

    private validarGeralPj(dadosClienteCadastroDto: DadosClienteCadastroDto, ehObrigatorio: boolean): string {
        let ret: string = "";

        if (ehObrigatorio) {
            let s = dadosClienteCadastroDto.Contato.trim();
            if (s === "") {
                ret = 'Informe o nome da pessoa para contato!<br>';
            }
        }

        return ret;
    }

    private validarGeralPf(dadosClienteCadastroDto: DadosClienteCadastroDto): string {
        let ret: string = "";

        let s = dadosClienteCadastroDto.Sexo;
        if (!!s) {
            if (!ValidacoesUtils.sexo_ok(s)) {
                ret = 'Indique qual o sexo!<br>';
            }
        }

        return ret;
    }

    private validarEndereco(dadosClienteCadastroDto: DadosClienteCadastroDto, lstCidadesIBGE: string[]): string {
        let ret: string = "";

        if (dadosClienteCadastroDto.Endereco.trim() === "") {
            ret += 'Preencha o endereço!<br>';
        }
        else {
            if (dadosClienteCadastroDto.Endereco.length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO) {
                ret += "ENDEREÇO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    "" + dadosClienteCadastroDto.Endereco.length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    "" + Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES<br>";
            }
        }

        if (dadosClienteCadastroDto.Numero.trim() === "") {
            ret += 'Preencha o número do endereço!<br>';
        }

        if (dadosClienteCadastroDto.Bairro.trim() === "") {
            ret += 'Preencha o bairro!<br>';
        }

        if (dadosClienteCadastroDto.Cidade.trim() === "") {
            ret += 'Preencha a cidade!<br>';
        }

        let s = dadosClienteCadastroDto.Uf.trim();
        if ((s === "") || (!ValidacoesUtils.uf_ok(s))) {
            ret += 'UF inválida!<br>';
        }

        if (dadosClienteCadastroDto.Cep.toString().trim() === "") {
            ret += 'Informe o CEP!<br>';
        }
        
        if (!new FormatarEndereco().cep_ok(dadosClienteCadastroDto.Cep.toString())) {
            ret += 'CEP inválido!<br>';
        }
        
        //vamos verificar se tem lista de cidades do IBGE, se tiver é pq a cidade do cep não existe no IBGE
        if (!!lstCidadesIBGE && lstCidadesIBGE.length > 0) {
            //a cidade do cep não consta no cadastro do IBGE e deve ter sido alterada, então vamos comparar
            if (dadosClienteCadastroDto.Cidade.trim() !== "") {
                if (lstCidadesIBGE.indexOf(dadosClienteCadastroDto.Cidade) == -1) {
                    //não existe a cidade
                    ret += "A cidade informada não consta no cadastro do IBGE para esse estado.<br>";
                }
            }
        }

        return ret;
    }

    private validarTelefones(dadosClienteCadastroDto: DadosClienteCadastroDto, ehPf: boolean, ehObrigatorio: boolean): string {
        let ret: string = "";
        //começo Gabriel alterando
        //vamos verificar se os telefones estão OK!
        if (ehPf) {
            ret += this.validarTelefonesPF(dadosClienteCadastroDto, ehObrigatorio);
        }
        else {
            ret += this.validarTelefonesPJ(dadosClienteCadastroDto, ehObrigatorio);
        }
        //fim Gabriel alterando

        return ret;
    }

    private validarTelefonesPF(dadosClienteCadastroDto: DadosClienteCadastroDto, ehObrigatorio: boolean): string {

        let ret: string = "";

        if (ehObrigatorio) {
            if (dadosClienteCadastroDto.TelefoneResidencial.trim() == "" && dadosClienteCadastroDto.DddResidencial == "" &&
                dadosClienteCadastroDto.Celular == "" && dadosClienteCadastroDto.DddCelular == "" &&
                dadosClienteCadastroDto.TelComercial == "" && dadosClienteCadastroDto.DddComercial == "") {
                ret += 'Preencha pelo menos um telefone!<br>';
                return ret;
            }
        }

        if (dadosClienteCadastroDto.TelefoneResidencial.trim() != "" &&
            dadosClienteCadastroDto.DddResidencial == "") {
            ret += 'Preencha o DDD residencial!<br>';
        }
        if (dadosClienteCadastroDto.TelefoneResidencial.trim() == "" &&
            dadosClienteCadastroDto.DddResidencial != "") {
            ret += 'Preencha o telefone residencial!<br>';
        }

        if (dadosClienteCadastroDto.Celular != "" &&
            dadosClienteCadastroDto.DddCelular == "") {
            ret += 'Preencha o DDD do celular.<br>';
        }
        if (dadosClienteCadastroDto.Celular == "" &&
            dadosClienteCadastroDto.DddCelular != "") {
            ret += 'Preencha o número do celular.<br>';
        }

        if (dadosClienteCadastroDto.TelComercial != "" &&
            dadosClienteCadastroDto.DddComercial == "") {
            ret += 'Preencha o DDD comercial!<br>';
        }
        if (dadosClienteCadastroDto.TelComercial == "" &&
            dadosClienteCadastroDto.DddComercial != "") {
            ret += 'Preencha o telefone comercial!<br>';
        }


        if (dadosClienteCadastroDto.TelComercial == "" &&
            dadosClienteCadastroDto.Ramal != "") {
            ret += "Ramal comercial preenchido sem telefone!<br>";
        }

        //vamos validar
        if (dadosClienteCadastroDto.DddResidencial != "" &&
            dadosClienteCadastroDto.TelefoneResidencial != "") {
            if (!FormatarTelefone.ddd_ok(dadosClienteCadastroDto.DddResidencial)) {
                ret += 'DDD residencial inválido!<br>';
            }
            if (!FormatarTelefone.telefone_ok(dadosClienteCadastroDto.TelefoneResidencial)) {
                ret += 'Telefone residencial inválido!<br>';
            }
        }

        if (dadosClienteCadastroDto.DddCelular != "" &&
            dadosClienteCadastroDto.Celular != "") {
            if (!FormatarTelefone.ddd_ok(dadosClienteCadastroDto.DddCelular)) {
                ret += 'DDD celular inválido!<br>';
            }
            if (!FormatarTelefone.telefone_ok(dadosClienteCadastroDto.Celular)) {
                ret += 'Telefone celular inválido!<br>';
            }
        }

        if (dadosClienteCadastroDto.DddComercial != "" &&
            dadosClienteCadastroDto.TelComercial != "") {
            if (!FormatarTelefone.ddd_ok(dadosClienteCadastroDto.DddComercial)) {
                ret += 'DDD comercial inválido!<br>';
            }
            if (!FormatarTelefone.telefone_ok(dadosClienteCadastroDto.TelComercial)) {
                ret += 'Telefone comercial inválido!<br>';
            }
        }

        return ret;
    }

    private validarTelefonesPJ(dadosClienteCadastroDto: DadosClienteCadastroDto, ehObrigatorio: boolean): string {
        let ret: string = "";

        if (ehObrigatorio) {
            if (dadosClienteCadastroDto.TelComercial.trim() == "" && dadosClienteCadastroDto.DddComercial.trim() == "" &&
                dadosClienteCadastroDto.TelComercial2.trim() == "" && dadosClienteCadastroDto.DddComercial2.trim() == "") {
                ret += 'Preencha ao menos um telefone!<br>';
                return ret;
            }
        }

        if (dadosClienteCadastroDto.TelComercial.trim() == "" &&
            dadosClienteCadastroDto.DddComercial.trim() != "") {
            ret += 'Preencha o telefone comercial!<br>';
        }
        if (dadosClienteCadastroDto.TelComercial.trim() != "" &&
            dadosClienteCadastroDto.DddComercial.trim() == "") {
            ret += 'Preencha o DDD comercial!<br>';
        }

        if (dadosClienteCadastroDto.TelComercial2.trim() == "" &&
            dadosClienteCadastroDto.DddComercial2.trim() != "") {
            ret += 'Preencha o telefone comercial 2!<br>';
        }
        if (dadosClienteCadastroDto.TelComercial2.trim() != "" &&
            dadosClienteCadastroDto.DddComercial2.trim() == "") {
            ret += 'Preencha o DDD comercial 2 !<br>';
        }

        if (dadosClienteCadastroDto.TelComercial == "" &&
            dadosClienteCadastroDto.Ramal != "") {
            ret += "Ramal comercial preenchido sem telefone!<br>";
        }

        if (dadosClienteCadastroDto.TelComercial2 == "" &&
            dadosClienteCadastroDto.Ramal2 != "") {
            ret += "Ramal comercial 2 preenchido sem telefone!<br>";
        }

        //vamos verificar se os tel comerciais estão ok
        if (dadosClienteCadastroDto.DddComercial != "" &&
            dadosClienteCadastroDto.TelComercial != "") {
            if (!FormatarTelefone.ddd_ok(dadosClienteCadastroDto.DddComercial)) {
                ret += 'DDD comercial inválido!<br>';
            }

            if (!FormatarTelefone.telefone_ok(dadosClienteCadastroDto.TelComercial)) {
                ret += 'Telefone comercial inválido!<br>';
            }
        }

        if (dadosClienteCadastroDto.DddComercial2 != "" &&
            dadosClienteCadastroDto.TelComercial2 != "") {
            if (!FormatarTelefone.ddd_ok(dadosClienteCadastroDto.DddComercial2)) {
                ret += 'DDD comercial 2 inválido!<br>';
            }
            if (!FormatarTelefone.telefone_ok(dadosClienteCadastroDto.TelComercial2)) {
                ret += 'Telefone comercial 2 inválido!<br>';
            }
        }

        return ret;
    }

    private validarRefBancaria(ref: RefBancariaDtoCliente): string {
        let ret: string = "";

        if (ref.Banco.trim() === "") {
            ret += 'Informe o banco no cadastro de Referência Bancária!<br>';
        }
        if (ref.Agencia.trim() === "") {
            ret += 'Informe a agência no cadastro de Referência Bancária!<br>';
        }
        if (ref.Conta.trim() === "") {
            ret += 'Informe o número da conta no cadastro de Referência Bancária!<br>';
        }
        return ret;
    }

    private verificarRefComercialDuplicada(lstRef: RefComercialDtoCliente[]): string {
        let ret: string = "";
        for (let i = 0; i < lstRef.length; i++) {
            let duplicados: RefComercialDtoCliente[] = new Array();
            //preciso fazer um filtro aqui
            duplicados = lstRef.filter(el => el.Nome_Empresa == lstRef[i].Nome_Empresa);

            if (duplicados.length > 1) {
                ret = "A Referência comercial " + lstRef[i].Nome_Empresa + " já existe!<br>";
                break;
            }
        }

        return ret;
    }

    private validarRefComerial(ref: RefComercialDtoCliente): string {
        let ret: string = "";

        if (ref.Nome_Empresa.trim() == "") {
            ret += 'Informe o nome da empresa no cadastro de Referência Comercial!<br>';
        }

        return ret;
    }

    public validarEnderecoEntregaDtoClienteCadastro(endEtg: EnderecoEntregaClienteCadastroDto,
        dadosClienteCadastroDto: DadosClienteCadastroDto, lstCidadeIBGE: string[]): string {

        let ret: string = "";

        ret += this.validarEnderecoEntrega(endEtg, dadosClienteCadastroDto.Tipo, lstCidadeIBGE);

        if (dadosClienteCadastroDto.Tipo == Constantes.ID_PF) {
            //vamos passar automático
            endEtg.EndEtg_tipo_pessoa = dadosClienteCadastroDto.Tipo;
            endEtg.EndEtg_nome = dadosClienteCadastroDto.Nome;
            endEtg.EndEtg_cnpj_cpf = dadosClienteCadastroDto.Cnpj_Cpf;
            endEtg.EndEtg_rg = dadosClienteCadastroDto.Rg;
            endEtg.EndEtg_email = dadosClienteCadastroDto.Email;
            endEtg.EndEtg_email_xml = dadosClienteCadastroDto.EmailXml;
            endEtg.EndEtg_produtor_rural_status = dadosClienteCadastroDto.ProdutorRural;
            endEtg.EndEtg_contribuinte_icms_status = dadosClienteCadastroDto.Contribuinte_Icms_Status;
            endEtg.EndEtg_ie = dadosClienteCadastroDto.Ie;
        }
        if (dadosClienteCadastroDto.Tipo == Constantes.ID_PJ) {
            endEtg.EndEtg_email = dadosClienteCadastroDto.Email;
            endEtg.EndEtg_email_xml = dadosClienteCadastroDto.EmailXml;
        }
        //vamos converter a entrega para dados cliente para validar
        let dadosClienteCadastroDto_deEnderecoEntrega = DadosClienteCadastroDto.DadosClienteCadastroDtoDeEnderecoEntregaDtoClienteCadastro(endEtg);

        //valida cpf, cnpj, email e emailxml
        if (dadosClienteCadastroDto.Tipo == Constantes.ID_PJ)
            ret += this.validarGeral(dadosClienteCadastroDto_deEnderecoEntrega, false);

        //valida contribuinteICMS e IE
        if (dadosClienteCadastroDto.Tipo == Constantes.ID_PJ) {
            ret += new ClienteCadastroUtils().validarInscricaoestadualIcms(dadosClienteCadastroDto_deEnderecoEntrega);

            //se produtor rural = não altera o valor de contribuinte e Ie
            this.validarProdutorRural(dadosClienteCadastroDto_deEnderecoEntrega);
        }

        let ehPf: boolean = dadosClienteCadastroDto.Tipo == Constantes.ID_PF ? true : false;

        if (dadosClienteCadastroDto_deEnderecoEntrega.Tipo == Constantes.ID_PJ)
            ret += this.validarTelefones(dadosClienteCadastroDto, ehPf, false);


        if (ret.length > 0) {
           
            let msgSplit: string[] = new Array<string>();
            msgSplit = ret.split("<br>");
            ret = "";
            msgSplit.forEach(x => {
                if (!!x)
                    ret += "<b>Endereço de entrega:</b> " + x + "<br>";
            });

            //validacoes.forEach(x => {
            //    msgErrosEndEtg.push("Endereço de entrega: " + x);
            //});
        }

        return ret;
    }

    private validarEnderecoEntrega(end: EnderecoEntregaClienteCadastroDto, tipoCliente: string, lstCidadesIBGE: string[]): string {
        let ret: string = "";
        let retorno = true;
        if (end.OutroEndereco) {
            if (!end.EndEtg_cod_justificativa || end.EndEtg_cod_justificativa.trim() === "") {
                ret += "Caso seja selecionado outro endereço, selecione a justificativa do endereço!<br>";
                return ret;
            }
            if (tipoCliente == Constantes.ID_PJ) {
                if (!end.EndEtg_tipo_pessoa || end.EndEtg_tipo_pessoa.trim() === "" ||
                    end.EndEtg_tipo_pessoa != Constantes.ID_PF && end.EndEtg_tipo_pessoa != Constantes.ID_PJ) {
                    ret += "Necessário escolher Pessoa Jurídica ou Pessoa Física!<br>";
                    return ret;
                }

                if (!end.EndEtg_tipo_pessoa || end.EndEtg_tipo_pessoa.trim() === "" ||
                    end.EndEtg_tipo_pessoa != Constantes.ID_PF && end.EndEtg_tipo_pessoa != Constantes.ID_PJ) {
                    ret += "Necessário escolher Pessoa Jurídica ou Pessoa Física!<br>";
                    return ret;
                }
            }

            if (!end.EndEtg_endereco || end.EndEtg_endereco.trim() === "") {
                ret += "Caso seja selecionado outro endereço, informe um endereço!<br>";
                return ret;
            }
            //somente número, o resto é feito pelo CEP
            if (!end.EndEtg_endereco_numero || end.EndEtg_endereco_numero.trim() === "") {
                ret += "Caso seja selecionado outro endereço, preencha o número do endereço!<br>";
                return ret;
            }
            if (!end.EndEtg_bairro || end.EndEtg_bairro.trim() === "") {
                ret += "Caso seja selecionado outro endereço, informe um bairro!<br>";
                return ret;
            }
            if (!end.EndEtg_cidade || end.EndEtg_cidade.trim() === "") {
                ret += "Caso seja selecionado outro endereço, informe uma cidade!<br>";
                return ret;
            }

            //vamos verificar se tem lista de cidades do IBGE, se tiver é pq a cidade do cep não existe no IBGE
            if (!!lstCidadesIBGE && lstCidadesIBGE.length > 0) {
                //a cidade do cep não consta no cadastro do IBGE e deve ter sido alterada, então vamos comparar
                if (end.EndEtg_cidade.trim() !== "") {
                    if (lstCidadesIBGE.indexOf(end.EndEtg_cidade) == -1) {
                        //não existe a cidade
                        ret += "A cidade informada não consta no cadastro do IBGE para esse estado.<br>";
                    }
                }
            }
        }
        return ret;
    }

    private validarProdutorRural(dadosClienteCadastroDto: DadosClienteCadastroDto): DadosClienteCadastroDto {
        let clienteCadastroUtils = this;
        //se é produtor salvamos o contribuinte

        //se não for produtor rural vamos apagar os dados


        if (dadosClienteCadastroDto.Tipo == Constantes.ID_PF) {
            if (dadosClienteCadastroDto.ProdutorRural == Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO) {
                //vamos apagar os dados de contribuinte e I.E.
                dadosClienteCadastroDto.Contribuinte_Icms_Status = Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;
                dadosClienteCadastroDto.Ie = "";
            }
        }

        return dadosClienteCadastroDto;
    }
}