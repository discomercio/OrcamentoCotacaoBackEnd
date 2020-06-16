using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InfraIdentity.ApiUnis
{
    public class RetornoObterTokenAutenticacaoApiUnis
    {
        public string Token { get; set; }
        public int IdErro { get; set; } = 0;
    }

    public interface IServicoAutenticacaoApiUnis
    {
        Task<RetornoObterTokenAutenticacaoApiUnis> ObterTokenAutenticacaoApiUnis(string usuario, string senha, string segredoToken, int validadeTokenMinutos, string role, IServicoAutenticacaoProviderApiUnis servicoAutenticacaoProviderApiUnis);
    }

    public interface IServicoAutenticacaoProviderApiUnis
    {
        Task<InfraIdentity.ApiUnis.UsuarioLoginApiUnis> ObterUsuarioApiUnis(string usuario, string senha);
    }

    public class ServicoAutenticacaoApiUnis : IServicoAutenticacaoApiUnis
    {
        public async Task<RetornoObterTokenAutenticacaoApiUnis> ObterTokenAutenticacaoApiUnis(string usuario, string senha, string segredoToken, int validadeTokenMinutos, string role, IServicoAutenticacaoProviderApiUnis servicoAutenticacaoProviderApiUnis)
        {
            var ret = new RetornoObterTokenAutenticacaoApiUnis();
            var user = await servicoAutenticacaoProviderApiUnis.ObterUsuarioApiUnis(usuario, senha);
            // retorna null se não tiver usuário
            if (user == null)
                return null;

            //Se retornar erro de usuário bloqueado ou senha expirada
            if (user.IdErro != 0)
            {
                ret.IdErro = user.IdErro;
                return ret;
            }
            
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(segredoToken);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Usuario),
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(validadeTokenMinutos),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            //util para debugar:
            //Microsoft_IdentityModel_Logging_IdentityModelEventSource_ShowPII = true;

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenstr = tokenHandler.WriteToken(token);
            ret.Token = tokenstr;
            ret.IdErro = 0;
            return ret;
        }

    }
}


