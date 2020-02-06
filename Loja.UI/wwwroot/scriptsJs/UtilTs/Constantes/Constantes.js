/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
/* nao editar, arquivo compilado pelo typescript*/ 
define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Constantes = /** @class */ (function () {
        function Constantes() {
            //' CÓDIGOS QUE IDENTIFICAM SE É PESSOA FÍSICA OU JURÍDICA
            this.ID_PF = "PF";
            this.ID_PJ = "PJ";
            //'	NÚMERO DE LINHAS DO CAMPO "OBS I" DO PEDIDO
            this.MAX_LINHAS_OBS1 = 5;
            //'   NÚMERO DE LINHAS DO CAMPO "TEXTO CONSTAR NF" DO PEDIDO
            this.MAX_LINHAS_NF_TEXTO_CONSTAR = 2;
            //' CÓDIGOS P/ ENTREGA IMEDIATA
            this.COD_ETG_IMEDIATA_ST_INICIAL = 0;
            this.COD_ETG_IMEDIATA_NAO = 1;
            this.COD_ETG_IMEDIATA_SIM = 2;
            this.COD_ETG_IMEDIATA_NAO_DEFINIDO = 10; //' PEDIDOS ANTIGOS QUE JÁ ESTAVAM NA BASE
            // ' CÓDIGOS P/ FLAG "BEM DE USO/CONSUMO"
            this.COD_ST_BEM_USO_CONSUMO_NAO = 0;
            this.COD_ST_BEM_USO_CONSUMO_SIM = 1;
            this.COD_ST_BEM_USO_CONSUMO_NAO_DEFINIDO = 10; //  ' PEDIDOS ANTIGOS QUE JÁ ESTAVAM NA BASE
            // '	CÓDIGOS PARA O CAMPO "INSTALADOR INSTALA"
            this.COD_INSTALADOR_INSTALA_NAO_DEFINIDO = 0;
            this.COD_INSTALADOR_INSTALA_NAO = 1;
            this.COD_INSTALADOR_INSTALA_SIM = 2;
            //' CÓDIGOS P/ STATUS QUE INDICA SE CLIENTE É OU NÃO CONTRIBUINTE DO ICMS
            this.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL = 0;
            this.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO = 1;
            this.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM = 2;
            this.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO = 3;
            //' CÓDIGOS P/ STATUS QUE INDICA SE CLIENTE É OU NÃO PRODUTOR RURAL
            this.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL = 0;
            this.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = 1;
            this.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM = 2;
            //'	NÚMERO DE LINHAS DO CAMPO "FORMA DE PAGAMENTO" DO PEDIDO
            this.MAX_LINHAS_FORMA_PAGTO = 5;
            //' CÓDIGOS P/ FLAG "GarantiaIndicadorStatus"
            this.COD_GARANTIA_INDICADOR_STATUS__NAO = 0;
            this.COD_GARANTIA_INDICADOR_STATUS__SIM = 1;
            this.COD_GARANTIA_INDICADOR_STATUS__NAO_DEFINIDO = 10; //  ' PEDIDOS ANTIGOS QUE JÁ ESTAVAM NA BASE
            // QUANTIDADE MÁXIMA DE REGISTROS COM OPÇÕES DE PARCELAMENTO P/ CADA FORNECEDOR
            this.MAX_LINHAS_TABELA_CUSTO_FINANCEIRO_FORNECEDOR = 24;
            this.MAX_DECIMAIS_COEFICIENTE_CUSTO_FINANCEIRO_FORNECEDOR = 6;
            this.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA = "SE";
            this.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA = "CE";
            this.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA = "AV";
            //FORMA DE PAGAMENTO
            this.COD_FORMA_PAGTO_A_VISTA = "1";
            this.COD_FORMA_PAGTO_PARCELADO_CARTAO = "2";
            this.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA = "3";
            this.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA = "4";
            this.COD_FORMA_PAGTO_PARCELA_UNICA = "5";
            this.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA = "6";
        }
        return Constantes;
    }());
    exports.Constantes = Constantes;
});
//# sourceMappingURL=/scriptsJs/UtilTs/Constantes/Constantes.js.map