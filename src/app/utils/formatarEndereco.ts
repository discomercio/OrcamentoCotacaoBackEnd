import { StringUtils } from './stringUtils';
import { PrePedidoDto } from '../dto/Prepedido/DetalhesPrepedido/PrePedidoDto';
import { CpfCnpjUtils } from './cpfCnpjUtils';
import { Constantes } from '../dto/Constantes';
import { FormatarTelefone } from './formatarTelefone';
import { EnderecoEntregaDtoClienteCadastro } from '../dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';

export class FormatarEndereco {
    formata_endereco(endereco: string, endereco_numero: string, endereco_complemento: string, bairro: string, cidade: string, uf: string, cep: string): string {

        //copiado do sistema em ASP
        let s_aux = "", strResposta = "";
        if (endereco && endereco.trim() != "")
            strResposta = endereco.trim();

        s_aux = endereco_numero ? endereco_numero.trim() : "";
        if (s_aux != "")
            strResposta = strResposta + ", " + s_aux;

        s_aux = endereco_complemento ? endereco_complemento.trim() : "";
        if (s_aux != "")
            strResposta = strResposta + " - " + s_aux;

        s_aux = bairro ? bairro.trim() : "";
        if (s_aux != "")
            strResposta = strResposta + " - " + s_aux;

        s_aux = cidade ? cidade.trim() : "";
        if (s_aux != "")
            strResposta = strResposta + " - " + s_aux;

        s_aux = uf ? uf.trim() : "";
        if (s_aux != "")
            strResposta = strResposta + " - " + s_aux;

        s_aux = cep ? cep.trim() : "";
        if (s_aux != "")
            strResposta = strResposta + " - " + this.cep_formata(s_aux);

        return strResposta;
    }

    //     ' ------------------------------------------------------------------------
    // '   CEP_FORMATA
    // ' 
    cep_formata(cep: string): string {
        let s_cep = StringUtils.retorna_so_digitos(cep);

        if (!this.cep_ok(s_cep))
            return "";
        s_cep = s_cep.substr(0, 5) + "-" + s_cep.substr(5, 3);
        return s_cep;
    }

    // ' ------------------------------------------------------------------------
    // '   CEP OK?
    // ' 
    cep_ok(cep: string): boolean {
        let s_cep = StringUtils.retorna_so_digitos(cep);
        if (s_cep.length == 0 || s_cep.length == 5 || s_cep.length == 8)
            return true;
        return false;
    }

    constante: Constantes = new Constantes();
    montarEnderecoEntregaPF(enderecoEntrega: EnderecoEntregaDtoClienteCadastro, sEndereco: string): string {
        let sCabecalho: string = "";
        let aux: string = "";
        let sTelefones: string = "";
        let retorno: string = "";

        sCabecalho = enderecoEntrega.EndEtg_nome + " \nCPF: " + CpfCnpjUtils.cnpj_cpf_formata(enderecoEntrega.EndEtg_cnpj_cpf);
        aux = "";
        if (enderecoEntrega.EndEtg_produtor_rural_status == this.constante.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) {
            if (enderecoEntrega.EndEtg_contribuinte_icms_status == this.constante.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                aux = "Sim (Não contribuinte)";
            if (enderecoEntrega.EndEtg_contribuinte_icms_status == this.constante.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                aux = "Sim (IE: " + enderecoEntrega.EndEtg_ie + ")";
            if (enderecoEntrega.EndEtg_contribuinte_icms_status == this.constante.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                aux = "Sim (Isento)";
        }

        if (enderecoEntrega.EndEtg_produtor_rural_status == this.constante.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)
            aux = "Não";

        if (aux != "") {
            sCabecalho += " - Produtor rural: " + aux;
        }

        sCabecalho += "\n";

        //fomatar telefones
        //tel residencial e celular
        let telRes: string = "";
        let telCel: string = "";
        if (!enderecoEntrega.EndEtg_tel_res && enderecoEntrega.EndEtg_tel_res != "")
            telRes = FormatarTelefone.formatarDDDTelRamal(enderecoEntrega.EndEtg_ddd_res, enderecoEntrega.EndEtg_tel_res, "");
        if (!enderecoEntrega.EndEtg_tel_cel && enderecoEntrega.EndEtg_tel_cel != "")
            telCel = FormatarTelefone.formatarDDDTelRamal(enderecoEntrega.EndEtg_ddd_cel, enderecoEntrega.EndEtg_tel_cel, "");

        sTelefones = "";
        if ((!telRes && telRes != "") || (!telCel && telCel != ""))
            sTelefones = "\n";

        if (!telRes && telRes != "") {
            sTelefones += "Telefone " + telRes;
            if (!telCel && telCel != "")
                sTelefones += " - ";
        }

        if (!telCel && telCel != "")
            sTelefones += "Celular " + telCel;

        retorno = sCabecalho + sEndereco + sTelefones + "\n" + enderecoEntrega.EndEtg_descricao_justificativa;

        return retorno;
    }

    montarEnderecoEntregaPJ(enderecoEntrega: EnderecoEntregaDtoClienteCadastro, sEndereco: string): string {
        let sCabecalho: string = "";
        let aux: string = "";
        let sTelefones: string = "";
        let retorno: string = "";

        sCabecalho = enderecoEntrega.EndEtg_nome + "<br>CNPJ: " + CpfCnpjUtils.cnpj_cpf_formata(enderecoEntrega.EndEtg_cnpj_cpf);

        if (enderecoEntrega.EndEtg_contribuinte_icms_status == this.constante.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
            aux = "Não";
        if (enderecoEntrega.EndEtg_contribuinte_icms_status == this.constante.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
            aux = "Sim (IE: " + enderecoEntrega.EndEtg_ie + ")";
        if (enderecoEntrega.EndEtg_contribuinte_icms_status == this.constante.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
            aux = "Isento";

        if (!aux)
            sCabecalho += " - Contribuinte ICMS: " + aux;

        sCabecalho += "<br>";

        let telCom: string = "";
        let telCom2: string = "";
        if (!enderecoEntrega.EndEtg_tel_com && enderecoEntrega.EndEtg_tel_com != "")
            telCom = FormatarTelefone.formatarDDDTelRamal(enderecoEntrega.EndEtg_ddd_com, enderecoEntrega.EndEtg_tel_com,
                enderecoEntrega.EndEtg_ramal_com);
        if (!enderecoEntrega.EndEtg_tel_com_2 && enderecoEntrega.EndEtg_tel_com_2 != "")
            telCom2 = FormatarTelefone.formatarDDDTelRamal(enderecoEntrega.EndEtg_ddd_com_2, enderecoEntrega.EndEtg_tel_com_2,
                enderecoEntrega.EndEtg_ramal_com_2);

        sTelefones = "";
        if ((!telCom && telCom != "") || (!telCom2 && telCom2 != ""))
            sTelefones = "<br>Telefone ";

        if (!telCom && telCom != "") {
            sTelefones += telCom;
            if (!telCom2 && telCom2 != "")
                sTelefones += " - ";
        }

        if (!telCom2 && telCom2 != "")
            sTelefones += telCom2;

        retorno = sCabecalho + sEndereco + sTelefones + "\n" + enderecoEntrega.EndEtg_descricao_justificativa;

        return retorno;
    }
}

