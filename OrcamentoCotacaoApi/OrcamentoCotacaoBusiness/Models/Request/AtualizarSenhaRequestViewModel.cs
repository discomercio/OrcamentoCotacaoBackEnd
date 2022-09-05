namespace OrcamentoCotacaoBusiness.Models.Request
{
    public sealed class AtualizarSenhaRequestViewModel
    {
        public int TipoUsuario { get; set; }
        public string Apelido { get; set; }
        public string Senha { get; set; }
        public string NovaSenha { get; set; }
        public string ConfirmacaoSenha { get; set; }
    }
}