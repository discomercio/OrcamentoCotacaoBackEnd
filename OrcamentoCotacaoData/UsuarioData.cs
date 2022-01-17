using OrcamentoCotacaoBusiness.Entities;
using System;
using System.Threading.Tasks;
using UtilsGlobais;
using Dapper;
using OrcamentoCotacaoModel.Entities;
using System.Linq;
using System.Collections.Generic;

namespace OrcamentoCotacaoData
{
    public class UsuarioData : BaseData<Usuario, UsuarioFiltro>
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        private string SelectValidaUsuarioUnisQuery => "SELECT usuario as nome, email, loja FROM t_USUARIO WHERE  usuario = @login AND datastamp = @senha ";
        public UsuarioData(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<Usuario> ValidaUsuarioAsync(string login, string senha)
        {
            var senhaCodificada = SenhaBll.CodificaSenha(senha);
            using (var db = contextoProvider.GetContextoLeitura()) { 
                Usuario usuario = 
                
            //var usuario = await db.<Usuario>(SelectValidaUsuarioUnisQuery, new { login, senha = senhaCodificada }, transaction: dbTransaction);
            }

            if (usuario != null)
            {
                string selectPerfis = @"SELECT id_perfil FROM t_PERFIL_X_USUARIO WHERE usuario = @login
                    AND id_perfil in ('000000000001', '000000010005', '000000010014', '000000010024', 
                    '000000010036', '000000020005')";
                var perfis = await dbConn.QueryAsync<string>(selectPerfis, new { login }, transaction: dbTransaction);
                if (perfis.Any() && perfis.Count() > 0)
                {
                    usuario.TipoUsuario = 0;
                    return usuario;
                }

                usuario.TipoUsuario = 1;
                return usuario;
            }

            _logger.LogDebug(SelectValidaParceiroQuery);
            var usuarioParceiro = await dbConn.QueryFirstOrDefaultAsync<Usuario>(SelectValidaParceiroQuery, new { login, senha = senhaCodificada }, transaction: dbTransaction);

            if (usuarioParceiro != null)
            {
                usuarioParceiro.TipoUsuario = 2;
                return usuarioParceiro;
            }

            var vendedorParceiro = await dbConn.QueryFirstOrDefaultAsync<Usuario>(SelectValidaVendedorParceiroQuery, new { login, senha }, transaction: dbTransaction);

            if (vendedorParceiro != null)
                vendedorParceiro.TipoUsuario = 3;

            return vendedorParceiro;
        }

        public List<Usuario> PorFiltro(UsuarioFiltro obj)
        {
            throw new NotImplementedException();
        }

        public Usuario Inserir(Usuario obj)
        {
            throw new NotImplementedException();
        }

        public Usuario Atualizar(Usuario obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(Usuario obj)
        {
            throw new NotImplementedException();
        }
    }
}
