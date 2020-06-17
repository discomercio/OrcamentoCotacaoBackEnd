using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InfraIdentity.ApiUnis
{
    public class RetornoObterTokenAutenticacaoApiUnis
    {
        public string TokenAcesso { get; set; }
        public List<string> ListaErros { get; set; } = new List<string>();
    }

    public interface IServicoAutenticacaoApiUnis
    {
        Task<RetornoObterTokenAutenticacaoApiUnis> ObterTokenAutenticacaoApiUnis(string usuario, string senha, string segredoToken, int validadeTokenMinutos, string role, IServicoAutenticacaoProviderApiUnis servicoAutenticacaoProviderApiUnis,
            string ip, string userAgent);
    }

    public interface IServicoAutenticacaoProviderApiUnis
    {
        Task<InfraIdentity.ApiUnis.UsuarioLoginApiUnis> ObterUsuarioApiUnis(string usuario, string senha, string ip, string userAgent);
    }

    public class ServicoAutenticacaoApiUnis : IServicoAutenticacaoApiUnis
    {
        public async Task<RetornoObterTokenAutenticacaoApiUnis> ObterTokenAutenticacaoApiUnis(string usuario, string senha, string segredoToken, int validadeTokenMinutos,
            string role, IServicoAutenticacaoProviderApiUnis servicoAutenticacaoProviderApiUnis,
            string ip, string userAgent)
        {
            var ret = new RetornoObterTokenAutenticacaoApiUnis();
            var user = await servicoAutenticacaoProviderApiUnis.ObterUsuarioApiUnis(usuario, senha, ip, userAgent);
            // retorna null se não tiver usuário
            if (user == null)
                return null;

            //Se retornar erro de usuário bloqueado ou senha expirada
            if (user.ListaErros.Count != 0)
            {
                ret.ListaErros = user.ListaErros;
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
            ret.TokenAcesso = tokenstr;
            return ret;
        }

    }
}


