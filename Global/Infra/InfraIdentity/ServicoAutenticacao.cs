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
        string ObterTokenAutenticacao(string apelido, string senha, string segredoToken, int validadeTokenMinutos, string role,
            IServicoAutenticacaoProvider servicoAutenticacaoProvider, out bool unidade_negocio_desconhecida, out UsuarioLogin usuario);
        string RenovarTokenAutenticacao(string apelido, string nome, string loja, string segredoToken, int validadeTokenMinutos, string role, string unidade_negocio, out bool unidade_negocio_desconhecida);
    }

    public interface IServicoAutenticacaoProvider
    {
        Task<InfraIdentity.UsuarioLogin> ObterUsuario(string apelido, string senha);
    }

    public class ServicoAutenticacao : IServicoAutenticacao
    {
        public string ObterTokenAutenticacao(string apelido, string senha, string segredoToken, int validadeTokenMinutos, string role,
            IServicoAutenticacaoProvider servicoAutenticacaoProvider, out bool unidade_negocio_desconhecida, out UsuarioLogin usuario)
        {
            UsuarioLogin user = null;

            user = servicoAutenticacaoProvider.ObterUsuario(apelido, senha).Result;

            usuario = user;
            // retorna null se não tiver usuário
            if (user == null)
            {
                unidade_negocio_desconhecida = false;
                return null;
            }

            //Se retornar erro de usuário bloqueado ou senha expirada
            if (!string.IsNullOrEmpty(user.IdErro.ToString()) && user.IdErro != 0)
            {
                unidade_negocio_desconhecida = false;
                return user.IdErro.ToString();
            }

            return GerarTokenAutenticacao(user.Apelido, user.Nome, user.Loja, segredoToken, validadeTokenMinutos, role, user.Unidade_negocio, out unidade_negocio_desconhecida);
        }

        public string RenovarTokenAutenticacao(string apelido, string nome, string loja, string segredoToken, int validadeTokenMinutos,
            string role, string unidade_negocio, out bool unidade_negocio_desconhecida)
        {
            //vamos verificar se usuario ainda tem permissão
            //ainda nao estamos fazendo, deveráimos fazer?

            return GerarTokenAutenticacao(apelido, nome, loja, segredoToken, validadeTokenMinutos, role, unidade_negocio, out unidade_negocio_desconhecida);
        }

        private static string GerarTokenAutenticacao(string apelido, string nome, string loja, string segredoToken, int validadeTokenMinutos,
            string role, string unidade_negocio, out bool unidade_negocio_desconhecida)
        {
            //unidade_negocio: BS ou VRF, se diferente dar erro no login
            if (String.IsNullOrEmpty(unidade_negocio))
            {
                unidade_negocio_desconhecida = true;
                return null;
            }
            if (!unidade_negocio.ToUpper().Trim().Contains("BS") && !(unidade_negocio.ToUpper().Trim().Contains("VRF")))
            {
                unidade_negocio_desconhecida = true;
                return null;
            }
            unidade_negocio_desconhecida = false;

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
                    new Claim(ClaimTypes.Role, role),
                    new Claim("unidade_negocio", unidade_negocio),
                    //new Claim("Email", !string.IsNullOrEmpty(usuario.Email)? usuario.Email:""),
                    new Claim("TipoUsuario", "GESTOR"),//!string.IsNullOrEmpty(usuario.TipoUsuario)?usuario.TipoUsuario:""),
                    new Claim("Parceiro", "AR OESTE"),//!string.IsNullOrEmpty(usuario.IdParceiro)?usuario.IdParceiro:""),
                    new Claim("Vendedor", "MARIO"),//!string.IsNullOrEmpty(IdVendedor)?IdVendedor:""),
                    new Claim("Lojas", !string.IsNullOrEmpty(loja)?loja:""),
                    new Claim("UnidadeNegocio", !string.IsNullOrEmpty(unidade_negocio)?unidade_negocio:"")
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


