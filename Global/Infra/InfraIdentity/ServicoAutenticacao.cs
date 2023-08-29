using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace InfraIdentity
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        public UsuarioLogin ObterTokenAutenticacao(
            UsuarioLogin login,
            string segredoToken,
            int validadeTokenMinutos,
            string BloqueioUsuarioLoginAmbiente,
            string role,
            IServicoAutenticacaoProvider servicoAutenticacaoProvider,
            string ip,
            out bool unidade_negocio_desconhecida)
        {
            var usuarioLogin = servicoAutenticacaoProvider.ObterUsuario(
                login.Apelido,
                login.Senha,
                BloqueioUsuarioLoginAmbiente,
                ip).Result;

            // retorna null se não tiver usuário
            if (usuarioLogin == null)
            {
                unidade_negocio_desconhecida = false;
                return null;
            }

            //Se retornar erro de usuário bloqueado ou senha expirada
            if (!string.IsNullOrEmpty(usuarioLogin.IdErro.ToString()) && usuarioLogin.IdErro != 0)
            {
                unidade_negocio_desconhecida = false;
                return usuarioLogin;
            }

            return GerarTokenAutenticacao(
                usuarioLogin,
                segredoToken,
                validadeTokenMinutos,
                role,
                out unidade_negocio_desconhecida);
        }

        public UsuarioLogin RenovarTokenAutenticacao(
            UsuarioLogin usuario,
            string segredoToken,
            int validadeTokenMinutos,
            string role,
            out bool unidade_negocio_desconhecida)
        {
            //vamos verificar se usuario ainda tem permissão
            //ainda nao estamos fazendo, deveráimos fazer?

            return GerarTokenAutenticacao(usuario, segredoToken, validadeTokenMinutos, role, out unidade_negocio_desconhecida);
        }

        private static UsuarioLogin GerarTokenAutenticacao(
            UsuarioLogin usuario,
            string segredoToken,
            int validadeTokenMinutos,
            string role,
            out bool unidade_negocio_desconhecida)
        {
            //unidade_negocio: BS ou VRF, se diferente dar erro no login
            if (String.IsNullOrEmpty(usuario.Unidade_negocio))
            {
                unidade_negocio_desconhecida = true;
                return null;
            }

            if (!usuario.Unidade_negocio.ToUpper().Trim().Contains("BS") && !(usuario.Unidade_negocio.ToUpper().Trim().Contains("VRF")))
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
                    new Claim(ClaimTypes.NameIdentifier, usuario.Apelido),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Surname, usuario.Loja),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("Idunidade_negocio", usuario.IdUnidade_negocio.HasValue ? usuario.IdUnidade_negocio.Value.ToString() : ""),
                    new Claim("unidade_negocio", !string.IsNullOrEmpty(usuario.Unidade_negocio)?usuario.Unidade_negocio:""),
                    //new Claim("Email", !string.IsNullOrEmpty(usuario.Email)? usuario.Email:""),
                    new Claim("TipoUsuario", usuario.TipoUsuario.HasValue?usuario.TipoUsuario.Value.ToString():""),
                    new Claim("Parceiro", !string.IsNullOrEmpty(usuario.IdParceiro)?usuario.IdParceiro:""),
                    new Claim("Vendedor", !string.IsNullOrEmpty(usuario.VendedorResponsavel)?usuario.VendedorResponsavel:""),
                    new Claim("Lojas", !string.IsNullOrEmpty(usuario.Loja)?usuario.Loja:""),
                    new Claim("Permissoes", usuario.Permissoes != null?string.Join(",",usuario.Permissoes):""),
                    new Claim("UsuarioLogin", JsonSerializer.Serialize(usuario)),
                    new Claim("Id", usuario.Id.ToString()),
                    new Claim("LojaLogada", usuario.Loja.Split(",").Count() > 1 ? "": usuario.Loja)
                }),
                Expires = DateTime.UtcNow.AddMinutes(validadeTokenMinutos),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var ret = tokenHandler.WriteToken(token);
            usuario.Token = ret;
            return usuario;
        }
    }
}


