using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacao.Data.Entities
{
    public class ProdutoSimples
    {
        public string Fabricante { get; set; }

        public string FabricanteNome { get; set; }

        public string Produto { get; set; }

        public string DescricaoHtml { get; set; }

        public string Descricao { get; set; }

        public decimal? PrecoLista { get; set; }

        public double CoeficienteDeCalculo { get; set; }

        public short? QtdeMaxVenda { get; set; }

        public float? DescMax { get; set; }

        public int Estoque { get; set; }

        public string Alertas { get; set; }

        public int Qtde { get; set; }

    }
}
