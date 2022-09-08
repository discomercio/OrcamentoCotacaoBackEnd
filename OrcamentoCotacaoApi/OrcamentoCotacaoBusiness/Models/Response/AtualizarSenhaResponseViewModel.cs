namespace OrcamentoCotacaoBusiness.Models.Response
{
    public sealed class AtualizarSenhaResponseViewModel : IViewModelResponse
    {
        public AtualizarSenhaResponseViewModel(bool sucesso, string mensagemRetorno)
        {
            Sucesso = sucesso;
            MensagemRetorno = mensagemRetorno;
        }

        public bool Sucesso { get; private set; }
        public string MensagemRetorno { get; private set; }
    }
}