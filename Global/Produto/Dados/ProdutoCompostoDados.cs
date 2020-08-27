using System;
using System.Collections.Generic;
using System.Text;

namespace Produto.Dados
{
    public class ProdutoCompostoDados
    {
        public string PaiFabricante { get; set; }
        public string PaiFabricanteNome { get; set; }
        public string PaiProduto { get; set; }
        public decimal Preco_total_Itens { get; set; }
        public List<ProdutoFilhoDados> Filhos { get; set; }
    }
}
