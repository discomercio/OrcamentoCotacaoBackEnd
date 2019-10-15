using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco
{
    public class ContextoBdProvider
    {
        public ContextoBdProvider(DbContextOptions<ContextoBdBasico> opt)
        {
            Opt = opt;
        }

        public DbContextOptions<ContextoBdBasico> Opt { get; }

        public ContextoBd GetContextoLeitura()
        {
            //para leitura, cada leitura com uma conexao nova
            return new ContextoBd(new ContextoBdBasico(Opt));
        }
        public ContextoBdGravacao GetContextoGravacao()
        {
            //para gravacao, todos compartilham a mesma coenxao (todos nesta instancia)
            //mas todos precisam estar dentro da transação!
            //para usar:  using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            return new ContextoBdGravacao(new ContextoBdBasico(Opt));
        }
    }
}
