using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraBanco.Modelos
{
    [Table("t_CFG_ORCAMENTO_COTACAO_ENDPOINT_FILTER")]
    public class TcfgOrcamentoCotacaoEndpointFilter : IModel
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Endpoint")]
        public string Endpoint { get; set; }

        [Column("Delay")]
        public int Delay { get; set; }

        [Column("Observacoes")]
        public string Observacoes { get; set; }
    }
}
