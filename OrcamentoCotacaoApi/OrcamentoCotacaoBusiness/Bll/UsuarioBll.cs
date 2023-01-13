using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Models.Request.Usuario;
using OrcamentoCotacaoBusiness.Models.Response.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class UsuarioBll
    {
        private readonly Usuario.UsuarioBll _usuarioBll;
        private readonly ILogger<UsuarioBll> _logger;
        public UsuarioBll(Usuario.UsuarioBll usuarioBll, ILogger<UsuarioBll> logger)
        {
            _usuarioBll = usuarioBll;
            _logger = logger;
        }

        public UsuariosPorListaLojasResponse BuscarVendedoresPorListaLojas(UsuariosPorListaLojasRequest request)
        {
            var response = new UsuariosPorListaLojasResponse();
            response.Sucesso = false;

            var usuarios = _usuarioBll.PorFiltro(new InfraBanco.Modelos.Filtros.TusuarioFiltro() { Lojas = request.Lojas });

            if(usuarios == null)
            {
                response.Mensagem = "Ops! Ocorreu um erro ao buscar lista de vendedores.";
                return response;
            }

            usuarios = usuarios.GroupBy(x => x.Nome_Iniciais_Em_Maiusculas).Select(x => x.First()).ToList();

            response.Usuarios = new List<UsuarioPorListaLojaResponse>();
            foreach(var usuario in usuarios)
            {
                var user = new UsuarioPorListaLojaResponse();
                user.Id = usuario.Id;
                user.Vendedor = usuario.Usuario;
                user.NomeIniciaisMaiusculo = usuario.Nome_Iniciais_Em_Maiusculas;
                response.Usuarios.Add(user);
            }

            response.Usuarios = response.Usuarios.OrderBy(u => u.NomeIniciaisMaiusculo).ToList();
            
            response.Sucesso = true;
            return response;
        }
    }
}
