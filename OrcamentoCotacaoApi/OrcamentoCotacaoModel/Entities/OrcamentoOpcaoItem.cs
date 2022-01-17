using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacao.Data.Entities
{
    public class OrcamentoOpcaoItem
    {
        public long Id { get; set; }
        public long IdOrcamentoCotacao_opcao { get; set; }
        public long IdOrcamentoCotacao_opcao_item_sku { get; set; }
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public decimal PrecoLista { get; set; }
        public float CustoFinancFornecCoeficiente { get; set; }
        public decimal CustoFinancFornecPrecoLista { get; set; }
        public decimal PrecoNf { get; set; }
        public decimal PrecoVenda { get; set; }
        public float DescDado { get; set; }
        public int Qtde { get; set; }
        public string UsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public DateTime? DataUltimaAlteracao { get; set; }
    }
}
