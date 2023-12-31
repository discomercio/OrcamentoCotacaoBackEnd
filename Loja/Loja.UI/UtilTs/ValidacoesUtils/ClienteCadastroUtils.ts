﻿import { FormatarEndereco } from "../Fomatar/FormatarEndereco";
import { Constantes } from "../Constantes/Constantes";
import { FormatarTelefone } from "../Fomatar/Mascaras/formataTelefone";
import { DadosClienteCadastroDto } from "../../DtosTs/ClienteDto/DadosClienteCadastroDto";

export class ClienteCadastroUtils {

    formata_endereco(p: DadosClienteCadastroDto): string {
        if (!p) {
            return "Sem endereço";
        }
        return new FormatarEndereco().formata_endereco(p.Endereco, p.Numero, p.Complemento, p.Bairro, p.Cidade, p.Uf, p.Cep);
    }

    //para dizer se é PF ou PJ
    ehPf(p: DadosClienteCadastroDto): boolean {
        if (!p) {
            return true;
        }
        if (p.Tipo)
            return p.Tipo == Constantes.ID_PF;
        //sem dados! qualqer opção serve...  
        return true;
    }

    telefone1(p: DadosClienteCadastroDto): string {
        if (!p) {
            return "";
        }
        let s = "";

        //pessoa física
        if (this.ehPf(p)) {
            if (!p.TelefoneResidencial)
                return "";
            if (p.TelefoneResidencial.trim() == "")
                return "";
            s = FormatarTelefone.telefone_formata(p.TelefoneResidencial);
            let s_aux = p.DddResidencial.trim();
            if (s_aux != "")
                s = "(" + s_aux + ") " + s;
            return s;
        }

        //pessoa jurídica
        let s2 = "";
        if (!p.TelComercial)
            return "";
        if (p.TelComercial.trim() == "")
            return "";

        s2 = FormatarTelefone.telefone_formata(p.TelComercial);
        let s_aux = p.DddComercial.trim();
        if (s_aux != "")
            s2 = "(" + s_aux + ") " + s2;
        s_aux = p.Ramal.trim();
        if (s_aux != "")
            s2 = s2 + "  - (Ramal " + s_aux + ")";
        return s2;
    }
    telefone2(p: DadosClienteCadastroDto): string {
        if (!p) {
            return "";
        }

        let s = "";
        //pessoa física
        if (this.ehPf(p)) {
            let s2 = "";
            if (!p.TelComercial)
                return "";
            if (p.TelComercial.trim() == "")
                return "";

            s2 = FormatarTelefone.telefone_formata(p.TelComercial);
            let s_aux = p.DddComercial.trim();
            if (s_aux != "")
                s2 = "(" + s_aux + ") " + s2;
            s_aux = p.Ramal.trim();
            if (s_aux != "")
                s2 = s2 + "  - (Ramal " + s_aux + ")";
            return s2;
        }

        if (!p.TelComercial2)
            return "";
        if (p.TelComercial2.trim() == "")
            return "";
        let s4 = FormatarTelefone.telefone_formata(p.TelComercial2.trim());
        let s_aux = p.DddComercial2.trim();
        if (s_aux != "")
            s4 = "(" + s_aux + ") " + s4;
        s_aux = p.Ramal2.trim();
        if (s_aux != "")
            s4 = s4 + "  - (Ramal " + s_aux + ")";
        return s4;
    }
    telefoneCelular(p: DadosClienteCadastroDto): string {
        if (!p) {
            return "";
        }
        let s2 = "";
        if (!p.Celular)
            return "";
        if (p.Celular.trim() == "")
            return "";

        s2 = FormatarTelefone.telefone_formata(p.Celular);
        let s_aux = p.DddCelular.trim();
        if (s_aux != "")
            s2 = "(" + s_aux + ") " + s2;
        return s2;
    }

    validarProdutorRural(dadosClienteCadastroDto: DadosClienteCadastroDto): DadosClienteCadastroDto {
        let constantes = Constantes;
        let clienteCadastroUtils = this;
        //se é produtor salvamos o contribuinte

        //se não for produtor rural vamos apagar os dados


        if (clienteCadastroUtils.ehPf(dadosClienteCadastroDto)) {
            if (dadosClienteCadastroDto.ProdutorRural == constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO) {
                //vamos apagar os dados de contribuinte e I.E.
                dadosClienteCadastroDto.Contribuinte_Icms_Status = constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;
                dadosClienteCadastroDto.Ie = "";
            }
        }

        return dadosClienteCadastroDto;
    }


    validarInscricaoestadualIcms(dadosClienteCadastroDto: DadosClienteCadastroDto): string {
        //retorna null se não tiver nenhum erro
        let constantes = Constantes;
        let clienteCadastroUtils = this;
        let msg: string = "";

        // copiado do ClienteEdita.asp
        if (clienteCadastroUtils.ehPf(dadosClienteCadastroDto)) {

            if ((dadosClienteCadastroDto.ProdutorRural !== constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                && (dadosClienteCadastroDto.ProdutorRural !== constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)) {
                msg += 'Informe se o cliente é produtor rural ou não!<br>';
            }
            if ((dadosClienteCadastroDto.ProdutorRural === constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) &&
                (dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)) {
                msg += 'Para ser cadastrado como Produtor Rural, é necessário ser contribuinte do ICMS e possuir nº de IE<br>';
            }
            if (dadosClienteCadastroDto.ProdutorRural == constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) {
                if ((dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                    && (dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    && (dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)) {
                    msg += 'Informe se o cliente é contribuinte do ICMS, não contribuinte ou isento!<br>';
                }
                if ((dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    && (!dadosClienteCadastroDto.Ie || (dadosClienteCadastroDto.Ie.trim() == ""))) {
                    msg += 'Se o cliente é contribuinte do ICMS a inscrição estadual deve ser preenchida!<br>';
                    //f.ie.focus();
                }
                if ((dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                    && (dadosClienteCadastroDto.Ie && (dadosClienteCadastroDto.Ie.toUpperCase().indexOf('ISEN') >= 0))) {
                    msg += 'Se cliente é não contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!<br>';
                    //f.ie.focus();
                }
                if ((dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    && (!dadosClienteCadastroDto.Ie || dadosClienteCadastroDto.Ie.toUpperCase().indexOf('ISEN') >= 0)) {
                    msg += 'Se cliente é contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!<br>';
                    //f.ie.focus();
                }
            }
        }
        else {
            //pessoa jurídica
            if ((dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                && (dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                && (dadosClienteCadastroDto.Contribuinte_Icms_Status !== constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)) {
                msg += 'Informe se o cliente é contribuinte do ICMS, não contribuinte ou isento!<br>';
            }
            if ((dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                && (!dadosClienteCadastroDto.Ie || (dadosClienteCadastroDto.Ie.trim() == ""))) {
                msg += 'Se o cliente é contribuinte do ICMS a inscrição estadual deve ser preenchida!<br>';
                //f.ie.focus();
            }
            if ((dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                && (dadosClienteCadastroDto.Ie && (dadosClienteCadastroDto.Ie.toUpperCase().indexOf('ISEN') >= 0))) {
                msg += 'Se cliente é não contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!<br>';
                //f.ie.focus();
            }
            if ((dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                && (!dadosClienteCadastroDto.Ie || dadosClienteCadastroDto.Ie.toUpperCase().indexOf('ISEN') >= 0)) {
                msg += 'Se cliente é contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!<br>';
                //f.ie.focus();
            }
        }

        // Verifica se o campo IE está vazio quando contribuinte ICMS = isento
        if (clienteCadastroUtils.ehPf(dadosClienteCadastroDto)) {
            if (dadosClienteCadastroDto.ProdutorRural && dadosClienteCadastroDto.ProdutorRural !== constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO) {
                if (dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO) {
                    if ((dadosClienteCadastroDto.Ie && (dadosClienteCadastroDto.Ie.trim() !== ""))) {
                        msg += "Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!<br>";
                        // fCAD.ie.focus();
                    }
                }
            }
        }
        else {
            //pessoa jurídica
            if (dadosClienteCadastroDto.Contribuinte_Icms_Status === constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO) {
                if ((dadosClienteCadastroDto.Ie && (dadosClienteCadastroDto.Ie.trim() !== ""))) {
                    msg += "Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!<br>";
                    // fCAD.ie.focus();
                }
            }
        }

        //tudo certo!
        return msg;
    }

    //inicializa um DadosClienteCadastroDto
    //temos problemas se os campos forem null
    public static inicializarDadosClienteCadastroDto(obj: DadosClienteCadastroDto) {
        //evita null
        if (!obj) {
            return;
        }
        obj.Loja = "";
        obj.Indicador_Orcamentista = "";
        obj.Vendedor = "";
        obj.Id = "";
        //eset não! obj.Cnpj_Cpf="";
        obj.Rg = "";
        obj.Ie = "";
        obj.Tipo = "";
        obj.Observacao_Filiacao = "";
        obj.Nome = "";
        obj.Endereco = "";
        obj.Numero = "";
        obj.Complemento = "";
        obj.Bairro = "";
        obj.Cidade = "";
        obj.Uf = "";
        obj.Cep = "";
        obj.DddResidencial = "";
        obj.TelefoneResidencial = "";
        obj.DddComercial = "";
        obj.TelComercial = "";
        obj.Ramal = "";
        obj.DddCelular = "";
        obj.Celular = "";
        obj.TelComercial2 = "";
        obj.DddComercial2 = "";
        obj.Ramal2 = "";
        obj.Email = "";
        obj.EmailXml = "";
        obj.Contato = "";

        obj.Contribuinte_Icms_Status = 0;
        obj.ProdutorRural = 0;
    }

}