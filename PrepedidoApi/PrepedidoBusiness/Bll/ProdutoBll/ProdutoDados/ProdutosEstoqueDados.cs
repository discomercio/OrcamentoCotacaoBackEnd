﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Bll.ProdutoBll.ProdutoDados
{
    public class ProdutosEstoqueDados
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public int Qtde { get; set; }
        public int Qtde_Utilizada { get; set; }
        public short Id_nfe_emitente { get; set; }
    }
}
