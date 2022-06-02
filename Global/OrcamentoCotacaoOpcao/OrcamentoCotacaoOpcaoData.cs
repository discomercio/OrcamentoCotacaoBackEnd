﻿using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OrcamentoCotacaoOpcao
{
    public class OrcamentoCotacaoOpcaoData: BaseData<TorcamentoCotacaoOpcao, TorcamentoCotacaoOpcaoFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public OrcamentoCotacaoOpcaoData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

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

        public TorcamentoCotacaoOpcao InserirComTransacao(TorcamentoCotacaoOpcao model, ContextoBdGravacao contextoBdGravacao)
        {
            contextoBdGravacao.Add(model);
            contextoBdGravacao.SaveChanges();
            return model;
        }

        public List<TorcamentoCotacaoOpcao> PorFilroComTransacao(TorcamentoCotacaoOpcaoFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TorcamentoCotacaoOpcao> PorFiltro(TorcamentoCotacaoOpcaoFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoLeitura())
                {
                    var saida = from c in db.torcamentoCotacaoOpcao
                                select c;

                    if (obj.IdOrcamentoCotacao != 0)
                    {
                        saida = saida.Where(x => x.IdOrcamentoCotacao == obj.IdOrcamentoCotacao);
                    }

                    return saida.ToList();
                }
            }
            catch (Exception ex) 
            {

                throw ex;
            }
        }
    }
}
