using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacao.Data.Entities
{
    public class ProdutoAlerta
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public string Mensagem { get; set; }
        public string Descricao { get; set; }
    }
}
