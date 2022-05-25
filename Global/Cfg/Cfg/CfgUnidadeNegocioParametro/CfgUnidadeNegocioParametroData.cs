using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cfg.CfgUnidadeNegocioParametro
{
    public class CfgUnidadeNegocioParametroData : BaseData<TcfgUnidadeNegocioParametro, TcfgUnidadeNegocioParametroFiltro>
    {

        private readonly ContextoBdProvider contextoProvider;

        public CfgUnidadeNegocioParametroData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TcfgUnidadeNegocioParametro Atualizar(TcfgUnidadeNegocioParametro obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TcfgUnidadeNegocioParametro obj)
        {
            throw new NotImplementedException();
        }

        public TcfgUnidadeNegocioParametro Inserir(TcfgUnidadeNegocioParametro obj)
        {
            throw new NotImplementedException();
        }

        public TcfgUnidadeNegocioParametro InserirComTransacao(TcfgUnidadeNegocioParametro model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgUnidadeNegocioParametro> PorFilroComTransacao(TcfgUnidadeNegocioParametroFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgUnidadeNegocioParametro> PorFiltro(TcfgUnidadeNegocioParametroFiltro obj)
        {
            throw new NotImplementedException();
        }
    }
}
