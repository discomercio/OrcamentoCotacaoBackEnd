using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InfraIdentity
{
    public interface IServicoAutenticacao
    {
        Task<string> ObterTokenAutenticacao(string apelido, string senha, string segredoToken, int validadeTokenMinutos, string role, IServicoAutenticacaoProvider servicoAutenticacaoProvider);
        string RenovarTokenAutenticacao(string apelido, string nome, string loja, string segredoToken, int validadeTokenMinutos, string role);
    }

    public interface IServicoAutenticacaoProvider
    {
        Task<InfraIdentity.UsuarioLogin> ObterUsuario(string apelido, string senha);
    }

    public class ServicoAutenticacao : IServicoAutenticacao
    {
        public async Task<string> ObterTokenAutenticacao(string apelido, string senha, string segredoToken, int validadeTokenMinutos, string role, IServicoAutenticacaoProvider servicoAutenticacaoProvider)
        {
            var user = await servicoAutenticacaoProvider.ObterUsuario(apelido, senha);
            // retorna null se não tiver usuário
            if (user == null)
                return null;

            //Se retornar erro de usuário bloqueado ou senha expirada
            if (!string.IsNullOrEmpty(user.IdErro.ToString()) && user.IdErro != 0)
            {
                return user.IdErro.ToString();
            }
            
            return GerarTokenAutenticacao(user.Apelido, user.Nome, user.Loja, segredoToken, validadeTokenMinutos, role);
        }

        public string RenovarTokenAutenticacao(string apelido, string nome, string loja, string segredoToken, int validadeTokenMinutos, string role)
        {
            //vamos verificar se usuario ainda tem permissão
            //ainda nao estamos fazendo, deveráimos fazer?

            return GerarTokenAutenticacao(apelido, nome, loja, segredoToken, validadeTokenMinutos, role);
        }

        private static string GerarTokenAutenticacao(string apelido, string nome, string loja, string segredoToken, int validadeTokenMinutos, string role)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(segredoToken);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, apelido),
                    new Claim(ClaimTypes.Name, nome),
                    new Claim(ClaimTypes.Surname, loja),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(validadeTokenMinutos),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var ret = tokenHandler.WriteToken(token);
            return ret;
        }
    }
}


