using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InfraIdentity
{
    public interface IValidarCredenciaisServico
    {
        Task<bool> CredenciaisValidas(string apelido);
    }


    public class ValidarCredenciais : ISecurityTokenValidator
    {
        private readonly IValidarCredenciaisServico validarCredenciaisServico;
        private int _maxTokenSizeInBytes = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
        private JwtSecurityTokenHandler _tokenHandler;

        public ValidarCredenciais(IValidarCredenciaisServico validarCredenciaisServico)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            this.validarCredenciaisServico = validarCredenciaisServico;
        }

        public bool CanValidateToken
        {
            get
            {
                return true;
            }
        }

        public int MaximumTokenSizeInBytes
        {
            get
            {
                return _maxTokenSizeInBytes;
            }
            set
            {
                _maxTokenSizeInBytes = value;
            }
        }

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            //How to access HttpContext/IP address from here?
            var principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
            
            //Validar o usuario
            if (principal != null && validatedToken != null)
            {
                
                ServicoDecodificarToken servico = new ServicoDecodificarToken();
                string usuario = servico.ObterApelidoOrcamentista(principal);

                if (!validarCredenciaisServico.CredenciaisValidas(usuario).Result)
                {
                    securityToken += "1";

                    //principal.Claims = new List<Claim>();
                    validatedToken = null;
                    principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
                    //acesso nao permitido
                    //throw new SecurityTokenValidationException($"Erro: usuario {usuario} não tem mais permissões de acesso.");
                }
            }

            return principal;
        }
    }

}

