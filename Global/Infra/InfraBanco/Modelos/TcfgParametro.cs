using Interfaces;
using System;

namespace InfraBanco.Modelos
{
    public class TcfgParametro : IModel
    {
        public int Id { get; set; }
        public short IdCfgDataType { get; set; }
        public string Sigla { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataHoraCadastro { get; set; }
        public DateTime? DataHoraUltAtualizacao { get; set; }
    }
}
