using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OrcamentistaEIndicadorVendedor
{
    public class OrcamentistaEIndicadorVendedorData : BaseData<TorcamentistaEIndicadorVendedor, TorcamentistaEIndicadorVendedorFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentistaEIndicadorVendedorData(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TorcamentistaEIndicadorVendedor Atualizar(TorcamentistaEIndicadorVendedor obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TorcamentistaEIndicadorVendedor obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentistaEIndicadorVendedor Inserir(TorcamentistaEIndicadorVendedor obj)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentistaEIndicadorVendedor> PorFiltro(TorcamentistaEIndicadorVendedorFiltro obj)
        {
            try
            {
                /*SELECT orc.Nome, orc.email, orc.nome, orc.IdParceiro, 
                   orc.ativo, toei.vendedor as IdVendedor FROM t_ORCAMENTISTA_E_INDICADOR_VENDEDOR orc 
                   INNER JOIN t_ORCAMENTISTA_E_INDICADOR toei ON toei.apelido = orc.IdParceiro 
                   WHERE orc.email = @login AND orc.senha = @senha*/
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var vendedorParceiro = from usr in db.TorcamentistaEIndicadorVendedor
                                           join par in db.TorcamentistaEindicadors on usr.IdIndicador equals par.Apelido
                                           select new TorcamentistaEIndicadorVendedor()
                                           {
                                               Id = usr.Id,
                                               Nome = usr.Nome,
                                               Email = usr.Email,
                                               Senha = usr.Senha,
                                               IdIndicador = usr.IdIndicador,
                                               Telefone = usr.Telefone,
                                               Celular = usr.Celular,
                                               Ativo = usr.Ativo,
                                               UsuarioCadastro = usr.UsuarioCadastro,
                                               UsuarioUltimaAlteracao = usr.UsuarioUltimaAlteracao,
                                               DataCadastro = usr.DataCadastro,
                                               DataUltimaAlteracao = usr.DataUltimaAlteracao,
                                               Loja = par.Loja,
                                               VendedorResponsavel = par.Vendedor
                                           };
                    if (!string.IsNullOrEmpty(obj.email))
                    {
                        vendedorParceiro = vendedorParceiro.Where(x => x.Email.ToUpper() == obj.email.ToUpper());
                    }
                    if (!string.IsNullOrEmpty(obj.senha))
                    {
                        vendedorParceiro = vendedorParceiro.Where(x => x.Senha == obj.senha);
                    }
                    if (!string.IsNullOrEmpty(obj.loja))
                    {
                        vendedorParceiro = vendedorParceiro.Where(x => x.Loja == obj.loja);
                    }
                    if (!string.IsNullOrEmpty(obj.IdIndicador))
                    {
                        vendedorParceiro = vendedorParceiro.Where(x => x.IdIndicador == obj.IdIndicador);
                    }
                    return vendedorParceiro.ToList();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
