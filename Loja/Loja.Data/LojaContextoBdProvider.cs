using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Data
{
    //public class LojaContextoBdProvider
    //{
    //    public LojaContextoBdProvider(DbContextOptions<LojaContextoBdBasico> opt)
    //    {
    //        Opt = opt;
    //    }

    //    public DbContextOptions<LojaContextoBdBasico> Opt { get; }

    //    public LojaContextoBd GetContextoLeitura()
    //    {
    //        //para leitura, cada leitura com uma conexao nova
    //        return new LojaContextoBd(new LojaContextoBdBasico(Opt));
    //    }
    //    public LojaContextoBdGravacao GetContextoGravacaoParaUsing()
    //    {
    //        //para gravacao, todos compartilham a mesma coenxao (todos nesta instancia)
    //        //mas todos precisam estar dentro da transação!
    //        return new LojaContextoBdGravacao(new LojaContextoBdBasico(Opt));
    //    }
    //}
}
