using OrcamentoCotacao.Data.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacao.Data.Entities
{
    public class OrcamentoOpcao
    {
        public long Id { get; set; }
        public long IdOrcamentoCotacao { get; set; }
        public decimal VlTotal { get; set; }
        public decimal ValorTotalComRA { get; set; }
        public string Observacoes { get; set; }
        public string UsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }
    }
}
