using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cfg.CfgOperacao
{
    public class CfgOperacaoData : BaseData<TcfgOperacao, TcfgOperacaoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public CfgOperacaoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TcfgOperacao Atualizar(TcfgOperacao obj)
        {
            throw new NotImplementedException();
        }

        public TcfgOperacao AtualizarComTransacao(TcfgOperacao model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TcfgOperacao obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TcfgOperacao obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TcfgOperacao Inserir(TcfgOperacao obj)
        {
            throw new NotImplementedException();
        }

        public TcfgOperacao InserirComTransacao(TcfgOperacao model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgOperacao> PorFilroComTransacao(TcfgOperacaoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgOperacao> PorFiltro(TcfgOperacaoFiltro obj)
        {
            throw new NotImplementedException();
        }
    }
}
