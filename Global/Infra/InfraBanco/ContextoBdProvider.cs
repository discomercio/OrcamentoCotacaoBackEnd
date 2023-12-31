﻿using Microsoft.EntityFrameworkCore;
using System;

namespace InfraBanco
{
    public class ContextoBdProvider
    {
        private readonly DbContextOptions<ContextoBdBasico> Opt;
        private readonly ContextoBdGravacaoOpcoes ContextoBdGravacaoOpcoes;

        public ContextoBdProvider(DbContextOptions<ContextoBdBasico> opt, ContextoBdGravacaoOpcoes contextoBdGravacaoOpcoes)
        {
            this.Opt = opt;
            this.ContextoBdGravacaoOpcoes = contextoBdGravacaoOpcoes;
        }

        public ContextoBd GetContextoLeitura()
        {
            try
            {
                return new ContextoBd(new ContextoBdBasico(Opt));

            }
            catch
            {
                throw;
            }
        }
        public ContextoBdGravacao GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle bloqueioTControle)
        {
            //para gravacao, todos compartilham a mesma coenxao (todos nesta instancia)
            //mas todos precisam estar dentro da transação!
            return new ContextoBdGravacao(new ContextoBdBasico(Opt), ContextoBdGravacaoOpcoes, bloqueioTControle);
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
//            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
    }
}
