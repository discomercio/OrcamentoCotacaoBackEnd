using ClassesBase;
using System;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacaoOpcao : IModel
    {
        public int Id { get; set; }
        public int IdOrcamentoCotacao { get; set; }
        public decimal VlTotal { get; set; }
        public decimal? ValorTotalComRA { get; set; }
        public string Observacoes { get; set; }
        public string UsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }
    }
}
