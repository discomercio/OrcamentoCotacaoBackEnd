using System;
using System.Collections.Generic;
using System.Text;

namespace Produto.Dados
{
    public class ProdutoCatalogoItemProdutosAtivosDados
	{
        public int Id { get; set; }
        public string Produto { get; set; }
        public string Fabricante { get; set; }
        public string FabricanteNome { get; set; }
        public string Descricao { get; set; }
        public string DescricaoCompleta { get; set; }
        public int IdPropriedade { get; set; }
        public string NomePropriedade { get; set; }
        public string ValorPropriedade { get; set; }
        public int Ordem { get; set; }
    }
}
