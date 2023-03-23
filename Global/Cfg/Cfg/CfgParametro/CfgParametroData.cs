using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfg.CfgParametro
{
    public class CfgParametroData: BaseData<TcfgParametro, TcfgParametroFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public CfgParametroData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TcfgParametro Atualizar(TcfgParametro obj)
        {
            throw new NotImplementedException();
        }

        public TcfgParametro AtualizarComTransacao(TcfgParametro model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TcfgParametro obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TcfgParametro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TcfgParametro Inserir(TcfgParametro obj)
        {
            throw new NotImplementedException();
        }

        public TcfgParametro InserirComTransacao(TcfgParametro model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgParametro> PorFilroComTransacao(TcfgParametroFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgParametro> PorFiltro(TcfgParametroFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var parametros = from c in db.TcfgParametro
                                     select c;

                    if (obj.Id.HasValue)
                    {
                        parametros = parametros.Where(x => x.Id == obj.Id);
                    }

                    return parametros.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
