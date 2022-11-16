namespace OrcamentoCotacaoBusiness.Models.Response
{
    public sealed class ExpiracaoSenhaResponseViewModel : IViewModelResponse
    {
        public ExpiracaoSenhaResponseViewModel(bool sucesso, string mensagemRetorno)
        {
            Sucesso = sucesso;
            MensagemRetorno = mensagemRetorno;
        }

        public bool Sucesso { get; private set; }
        public string MensagemRetorno { get; private set; }
    }
}