using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InfraIdentity.ApiMagento
{
    public class RetornoObterTokenAutenticacaoApiMagento
    {
        public string TokenAcesso { get; set; }
        public List<string> ListaErros { get; set; } = new List<string>();
    }

    public interface IServicoAutenticacaoApiMagento
    {
        Task<RetornoObterTokenAutenticacaoApiMagento> ObterTokenAutenticacaoApiMagento(string usuario, string senha, string segredoToken, int validadeTokenMinutos, string role, IServicoAutenticacaoProviderApiMagento servicoAutenticacaoProviderApiMagento,
            string ip, string userAgent, string ApelidoPerfilLiberaAcessoApiMagento);
    }

    public interface IServicoAutenticacaoProviderApiMagento
    {
        Task<InfraIdentity.ApiMagento.UsuarioLoginApiMagento> ObterUsuarioApiMagento(string usuario, string senha, string ip, string userAgent, string ApelidoPerfilLiberaAcessoApiMagento);
    }

    public class ServicoAutenticacaoApiMagento : IServicoAutenticacaoApiMagento
    {
        public async Task<RetornoObterTokenAutenticacaoApiMagento> ObterTokenAutenticacaoApiMagento(string usuario, string senha, string segredoToken, int validadeTokenMinutos,
            string role, IServicoAutenticacaoProviderApiMagento servicoAutenticacaoProviderApiMagento,
            string ip, string userAgent, string ApelidoPerfilLiberaAcessoApiMagento)
        {
            var ret = new RetornoObterTokenAutenticacaoApiMagento();
            var user = await servicoAutenticacaoProviderApiMagento.ObterUsuarioApiMagento(usuario, senha, ip, userAgent, ApelidoPerfilLiberaAcessoApiMagento);
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
