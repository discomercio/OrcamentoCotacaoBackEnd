using System.Threading.Tasks;

namespace InfraIdentity
{
    public interface IServicoAutenticacaoProvider
    {
        Task<InfraIdentity.UsuarioLogin> ObterUsuario(string apelido, string senha, string ip = null);
    }
}