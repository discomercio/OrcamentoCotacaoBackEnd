namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class CoeficienteResponseViewModel : IViewModelResponse
    {
        public string Fabricante { get; set; }
        public string TipoParcela { get; set; }
        public int QtdeParcelas { get; set; }
        public float Coeficiente { get; set; }
    }
}
