using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TcfgUnidadeNegocioParametro : IModel
    {
        public int Id { get; set; }
        public int? IdCfgUnidadeNegocio { get; set; }
        public int IdCfgParametro { get; set; }
        public string Valor { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataHoraCadastro { get; set; }
        public DateTime? DataHoraUltAtualizacao { get; set; }
    }
}
