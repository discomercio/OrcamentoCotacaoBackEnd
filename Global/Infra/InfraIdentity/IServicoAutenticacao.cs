namespace InfraIdentity
{
    public interface IServicoAutenticacao
    {
        UsuarioLogin ObterTokenAutenticacao(
            UsuarioLogin login, 
            string segredoToken, 
            int validadeTokenMinutos, 
            string role,
            IServicoAutenticacaoProvider servicoAutenticacaoProvider,
            string ip,
            out bool unidade_negocio_desconhecida
            );

        UsuarioLogin RenovarTokenAutenticacao(
            UsuarioLogin login, 
            string segredoToken, 
            int validadeTokenMinutos, 
            string role, out bool unidade_negocio_desconhecida
            );
    }
}
