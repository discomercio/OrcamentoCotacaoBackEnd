using System.Threading.Tasks;

namespace InfraIdentity
{
    public interface IServicoAutenticacaoProvider
    {
        Task<UsuarioLogin> ObterUsuario(string apelido, string senha, string bloqueioUsuarioLoginAmbiente, string ip = null);
    }
}