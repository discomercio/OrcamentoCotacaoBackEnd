using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orcamento
{
    public class OrcamentoOpcaoBll : BaseData<TorcamentoCotacaoOpcao, TorcamentoCotacaoOpcaoFiltro>
    {
        public TorcamentoCotacaoOpcao Atualizar(TorcamentoCotacaoOpcao obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TorcamentoCotacaoOpcao obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcao Inserir(TorcamentoCotacaoOpcao obj)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoOpcao> PorFiltro(TorcamentoCotacaoOpcaoFiltro obj)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcao GetById(string id)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcao InserirComTransacao(TorcamentoCotacaoOpcao model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoOpcao> PorFilroComTransacao(TorcamentoCotacaoOpcaoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TorcamentoCotacaoOpcao AtualizarComTransacao(TorcamentoCotacaoOpcao model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TorcamentoCotacaoOpcao obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }
    }
}
