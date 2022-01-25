using ClassesBase;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentistaEIndicadorVendedor
{
    public class OrcamentistaEIndicadorVendedorBll : BaseData<TorcamentistaEIndicadorVendedor, TorcamentistaEIndicadorVendedorFiltro>
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        public OrcamentistaEIndicadorVendedorBll(InfraBanco.ContextoBdProvider contextoProvider)
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
                    var usuario = from usr in db.TorcamentistaEIndicadorVendedor
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
                                      Loja = par.Loja
                                  };
                    if (!string.IsNullOrEmpty(obj.email))
                    {
                        usuario = usuario.Where(x => x.Email.ToUpper() == obj.email.ToUpper());
                    }
                    if (!string.IsNullOrEmpty(obj.senha))
                    {
                        usuario = usuario.Where(x => x.Senha == obj.senha);
                    }
                    return usuario.ToList();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
