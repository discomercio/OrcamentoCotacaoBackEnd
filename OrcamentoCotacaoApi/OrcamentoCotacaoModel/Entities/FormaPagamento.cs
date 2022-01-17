using OrcamentoCotacao.Data.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacao.Data.Entities
{
    public class FormaPagamento
    {
        public long Id { get; set; }
        public long IdOrcamentoCotacao_opcao { get; set; }
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public int QtdeParcelas { get; set; }
        public string Valores { get; set; }
        public DateTime DataCadastro { get; set; }
        public string UsuarioCadastro { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }
        
    }
}
