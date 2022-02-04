using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco
{
    public class ContextoBdProvider : IDisposable
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
            catch (Exception e)
            {
                throw;
            }
            //para leitura, cada leitura com uma conexao nova
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
