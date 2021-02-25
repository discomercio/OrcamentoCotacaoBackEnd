using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Constantes
{
    /*
     * mais constantes, principalmente as que estão diferentes do ASP
     * */

    public static partial class Constantes
    {

        #region enum para COD_SITE
        public enum Cod_site
        {
            COD_SITE_ARTVEN_BONSHOP,
            COD_SITE_ARTVEN_FABRICANTE,
            COD_SITE_ASSISTENCIA_TECNICA
        }
        public static string Cod_site_para_string(Cod_site cod_Site)
        {
            var ret = "Desconhecido";
            switch (cod_Site)
            {
                case Cod_site.COD_SITE_ARTVEN_BONSHOP:
                    ret = Constantes.COD_SITE_ARTVEN_BONSHOP;
                    break;
                case Cod_site.COD_SITE_ARTVEN_FABRICANTE:
                    ret = Constantes.COD_SITE_ARTVEN_FABRICANTE;
                    break;
                case Cod_site.COD_SITE_ASSISTENCIA_TECNICA:
                    ret = Constantes.COD_SITE_ASSISTENCIA_TECNICA;
                    break;
            }
            return ret;
        }
        #endregion

        #region enum para Op_origem__pedido_novo
        public enum Op_origem__pedido_novo
        {
            OP_ORIGEM__NAO_DETERMINADO,
            OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO
        }
        public static string Op_origem__pedido_novo_para_string(Op_origem__pedido_novo op_origem)
        {
            var ret = "Desconhecido";
            switch (op_origem)
            {
                case Op_origem__pedido_novo.OP_ORIGEM__NAO_DETERMINADO:
                    ret = "Desconhecido";
                    break;
                case Op_origem__pedido_novo.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO:
                    ret = Constantes.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO;
                    break;
            }
            return ret;
        }
        #endregion

    }
}
