using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfg.CfgModulo
{
    public class CfgModuloData : BaseData<TcfgModulo, TcfgModuloFiltro>
    {
        private readonly ContextoBdProvider contextoBdProvider;

        public CfgModuloData(ContextoBdProvider contextoBdProvider)
        {
            this.contextoBdProvider = contextoBdProvider;
        }

        public TcfgModulo Atualizar(TcfgModulo obj)
        {
            throw new NotImplementedException();
        }

        public TcfgModulo AtualizarComTransacao(TcfgModulo model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TcfgModulo obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TcfgModulo obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TcfgModulo Inserir(TcfgModulo obj)
        {
            throw new NotImplementedException();
        }

        public TcfgModulo InserirComTransacao(TcfgModulo model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgModulo> PorFilroComTransacao(TcfgModuloFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgModulo> PorFiltro(TcfgModuloFiltro obj)
        {
            using(var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var saida = from c in db.TcfgModulo
                            select c;

                if (!string.IsNullOrEmpty(obj.Descricao))
                {
                    saida = saida.Where(x => x.Descricao == obj.Descricao);
                }

                return saida.ToList();
            }
        }
    }
}
