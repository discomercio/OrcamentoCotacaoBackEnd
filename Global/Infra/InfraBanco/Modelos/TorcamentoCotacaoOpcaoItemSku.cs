using ClassesBase;
using System;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacaoOpcaoItemSku : IModel
    {
        public int Id { get; set; }
        public int IdOrcamentoCotacaoOpcao { get; set; }
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public string PrecoLista { get; set; }
        public string CustoFinancFornecCoeficiente { get; set; }
        public string PrecoNf { get; set; }
        public string PrecoVenda { get; set; }
        public string DescDado { get; set; }
        public string Qtde { get; set; }
        public string UsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        //public string UsuarioUltimaAlteracao { get; set; }
        //public DateTime? DataUltimaAlteracao { get; set; }
        public string CustoFinancFornecPrecoLista { get; set; }
    }
}

