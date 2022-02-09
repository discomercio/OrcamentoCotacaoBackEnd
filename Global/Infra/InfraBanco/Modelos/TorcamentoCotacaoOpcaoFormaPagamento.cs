using Interfaces;
using System;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacaoOpcaoFormaPagamento : IModel
    {
        public int Id { get; set; }
        public int IdOrcamentoCotacaoOpcao { get; set; }
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public int QtdeParcelas { get; set; }
        public int Valores { get; set; }
        public DateTime DataCadastro { get; set; }
        public string UsuarioCadastro { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }
    }
}
