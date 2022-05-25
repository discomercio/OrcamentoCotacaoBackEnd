using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cfg.CfgUnidadeNegocio
{
    public class CfgUnidadeNegocioData : BaseData<TcfgUnidadeNegocio, TcfgUnidadeNegocioFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public CfgUnidadeNegocioData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }
        public TcfgUnidadeNegocio Atualizar(TcfgUnidadeNegocio obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TcfgUnidadeNegocio obj)
        {
            throw new NotImplementedException();
        }

        public TcfgUnidadeNegocio Inserir(TcfgUnidadeNegocio obj)
        {
            throw new NotImplementedException();
        }

        public TcfgUnidadeNegocio InserirComTransacao(TcfgUnidadeNegocio model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgUnidadeNegocio> PorFilroComTransacao(TcfgUnidadeNegocioFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgUnidadeNegocio> PorFiltro(TcfgUnidadeNegocioFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var unidadeNegocio = from L in db.TcfgUnidadeNegocio
                                         select L;

                    if (!string.IsNullOrEmpty(obj.Sigla))
                    {
                        unidadeNegocio = unidadeNegocio.Where(x => x.Sigla == obj.Sigla);
                    }

                    return unidadeNegocio.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
