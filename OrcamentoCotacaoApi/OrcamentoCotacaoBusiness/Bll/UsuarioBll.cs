﻿using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Models.Request.Usuario;
using OrcamentoCotacaoBusiness.Models.Response.Usuario;
using System;
using System.Collections.Generic;
using System.Text;

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

            response.Usuarios = new List<UsuarioPorListaLojaResponse>();
            foreach(var usuario in usuarios)
            {
                var user = new UsuarioPorListaLojaResponse();
                user.Vendedor = usuario.Usuario;
                user.NomeIniciaisMaiusculo = usuario.Nome_Iniciais_Em_Maiusculas;
                response.Usuarios.Add(user);
            }
            
            response.Sucesso = true;
            return response;
        }
    }
}
