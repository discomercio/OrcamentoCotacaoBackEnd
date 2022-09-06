namespace Prepedido.Dto
{
    public sealed class AtualizarSenhaDto
    {
        public int TipoUsuario { get; set; }
        public string Apelido { get; set; }
        public string Senha { get; set; }
        public string NovaSenha { get; set; }
        public string ConfirmacaoSenha { get; set; }
    }
}