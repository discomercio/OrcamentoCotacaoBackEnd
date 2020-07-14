import { StringUtils } from './stringUtils';

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
            strResposta =  strResposta + " - " + s_aux;

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
}

