using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacao.Data.Entities
{
    public class ProdutosCompostos
    {
        public string PaiFabricante { get; set; }
        public string PaiFabricanteNome { get; set; }
        public string PaiProduto { get; set; }
        public string PaiDescricao { get; set; }
        public string ProdutoFilho { get; set; }
        public int ProdutoFilhoQtde { get; set; }
    }
}
