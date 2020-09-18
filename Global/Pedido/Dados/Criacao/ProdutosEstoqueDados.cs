using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.Criacao
{
    public class ProdutosEstoqueDados
    {
        public string Produto { get; set; }
        public int Qtde { get; set; }
        public int Qtde_Utilizada { get; set; }
        public short Id_nfe_emitente { get; set; }
    }
}
