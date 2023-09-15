namespace OrcamentoCotacaoBusiness.Models.Response
{
    public sealed class PermissaoOrcamentoResponse : ResponseBase
    {
        public bool VisualizarOrcamento { get; set; }
        public bool ProrrogarOrcamento { get; set; }
        public bool EditarOrcamento { get; set; }
        public bool CancelarOrcamento { get; set; }
        public bool ClonarOrcamento { get; set; }
        public bool ReenviarOrcamento { get; set; }
        public bool EditarOpcaoOrcamento { get; set; }
        public bool DesabilitarAprovarOpcaoOrcamento { get; set; }
        public bool MensagemOrcamento { get; set; }
        public bool NenhumaOpcaoOrcamento { get; set; }
        public bool DesabilitarBotoes { get; set; }
        public bool ExcluirOrcamento{ get; set; }
        public bool AnularOrcamentoAprovado{ get; set; }   
    }
}