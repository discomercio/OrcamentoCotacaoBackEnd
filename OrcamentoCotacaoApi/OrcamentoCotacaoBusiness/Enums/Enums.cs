using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Enums
{
    public class Enums
    {
        public enum TipoUsuario
        {
            GESTOR = 0,
            VENDEDOR = 1,
            PARCEIRO = 2,
            VENDEDOR_DO_PARCEIRO = 3,
        }

        public enum ePermissao
        {
            AcessoAoModulo = 100100,
            AdministradorDoModulo = 100200,
            ParceiroIndicadorUsuarioMaster = 100300,
            SelecionarQualquerIndicadorDaLoja = 100400
        }
    }
}
