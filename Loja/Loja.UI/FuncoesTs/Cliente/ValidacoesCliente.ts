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
        debugger;
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

        if (!dadosClienteCadastroDto.Nome || dadosClienteCadastroDto.Nome.trim() == "") {
            ret += 'Preencha o nome!<br>';
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
            ret += 'PREENCHA CNPJ/CPF<br>';
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
        let reduced: RefComercialDtoCliente[] = new Array();

        lstRef.forEach((item) => {
            //preciso fazer um filtro aqui
            var duplicado = reduced.filter(el => el.Nome_Empresa == item.Nome_Empresa);


            if (!duplicado) {
                reduced.push(item);
            }
            else {
                ret += "A Referência comercial " + item.Nome_Empresa + " já existe!<br>";
            }
        });

        return ret;
    }

    private validarRefComerial(ref: RefComercialDtoCliente): string {
        let ret: string = "";

        if (ref.Nome_Empresa.trim() == "") {
            ret += 'Informe o nome da empresa no cadastro de Referência Comercial!<br>';
        }

        return ret;
    }

}