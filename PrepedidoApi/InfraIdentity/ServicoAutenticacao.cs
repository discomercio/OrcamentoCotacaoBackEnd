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
            
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(segredoToken);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Apelido),
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim(ClaimTypes.Surname, user.Loja),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(validadeTokenMinutos),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            //util para debugar:
            //Microsoft_IdentityModel_Logging_IdentityModelEventSource_ShowPII = true;

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var ret = tokenHandler.WriteToken(token);
            return ret;
        }

        public string RenovarTokenAutenticacao(string apelido, string nome, string loja, string segredoToken, int validadeTokenMinutos, string role)
        {
            //vamos verificar se usuario ainda tem permissão

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


