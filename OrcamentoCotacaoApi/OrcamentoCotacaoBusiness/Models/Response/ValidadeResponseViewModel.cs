namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ValidadeResponseViewModel
    {
        public int QtdeDiasValidade { get; set; }
        public int QtdeDiasProrrogacao { get; set; }
        public int QtdeMaxProrrogacao { get; set; }
        public int QtdeGlobalValidade { get; set; }
        public int MaxPeriodoConsultaFiltroPesquisa { get; set; }
        public int MaxPeriodoConsulta_RelatorioGerencial { get; set; }
        public int LimiteQtdeMaxOpcaoOrcamento { get; set; }
        public int LimiteQtdeItens { get; set; }
    }
}
