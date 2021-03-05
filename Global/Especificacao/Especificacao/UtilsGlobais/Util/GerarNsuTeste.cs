using Especificacao.Testes.Utils.BancoTestes;
using InfraBanco;
using InfraBanco.Constantes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Especificacao.Especificacao.UtilsGlobais.Util
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class GerarNsuTeste : IDisposable
    {
        private readonly ContextoBdProvider contextoProvider = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos()
            .GetRequiredService<InfraBanco.ContextoBdProvider>();
        private readonly InicializarBancoGeral inicializarBanco = new Testes.Utils.BancoTestes.InicializarBancoGeral(
            Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<InfraBanco.ContextoBdProvider>(),
            Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<InfraBanco.ContextoCepProvider>());

        public GerarNsuTeste()
        {
            Testes.Utils.LogTestes.LogOperacoes2.MensagemEspecial("construtor GerarNsuTeste", this);
        }

        private void Apagar_t_CONTROLE()
        {
            using var db = contextoProvider.GetContextoGravacaoParaUsing();
            foreach (var c in db.Tcontroles)
                db.Tcontroles.Remove(c);
            db.SaveChanges();
            db.transacao.Commit();
        }


        [Fact]
        public void TesteSemNsu()
        {
            using var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing();
            Assert.ThrowsAnyAsync<ArgumentException>(() => global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, null)).Wait();
            Assert.ThrowsAnyAsync<ArgumentException>(() => global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, "um nsu que não existe")).Wait();
        }

        [Fact]
        public void NsuNull()
        {
            //null dá erro
            //mas o banco não permite Nsu = null! então não tem como ser gerado mesmo

            //sem numero dá erro
            Apagar_t_CONTROLE();
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                dbgravacao.Tcontroles.Add(new InfraBanco.Modelos.Tcontrole() { Id_Nsu = Constantes.NSU_CADASTRO_CLIENTES, Nsu = "nao-numero", Dt_Ult_Atualizacao = DateTime.Now.AddDays(-720) /* 2 anos para trás */ });
                dbgravacao.SaveChanges();
                dbgravacao.transacao.Commit();
                Assert.ThrowsAnyAsync<Exception>(() => global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES)).Wait();
            }

        }


        [Fact]
        public void Nsu_Sem_seq_anual()
        {
            //sem numero dá erro
            Apagar_t_CONTROLE();
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                dbgravacao.Tcontroles.Add(new InfraBanco.Modelos.Tcontrole() { Id_Nsu = Constantes.NSU_CADASTRO_CLIENTES, Nsu = "nao-numero", Dt_Ult_Atualizacao = DateTime.Now.AddDays(-720) /* 2 anos para trás */ });
                dbgravacao.SaveChanges();
                dbgravacao.transacao.Commit();
                Assert.ThrowsAnyAsync<Exception>(() => global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES)).Wait();
            }

            Funcionando();
        }

        [Fact]
        public void Funcionando()
        {
            //agora um que funciona...
            Apagar_t_CONTROLE();
            using var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing();
            dbgravacao.Tcontroles.Add(new InfraBanco.Modelos.Tcontrole() { Id_Nsu = Constantes.NSU_CADASTRO_CLIENTES, Nsu = "000000645506", Dt_Ult_Atualizacao = DateTime.Now.AddDays(-720) /* 2 anos para trás */ });
            dbgravacao.SaveChanges();
            dbgravacao.transacao.Commit();
            Assert.Equal("000000645507", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
            Assert.Equal("000000645508", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
            Assert.Equal("000000645509", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
            Assert.Equal("000000645510", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
            Assert.Equal("000000645511", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
            Assert.Equal("000000645512", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
            Assert.Equal("000000645513", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
        }




        [Fact]
        public void Nsu_Com_seq_anual()
        {
            //gerando normal
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                Apagar_t_CONTROLE();
                dbgravacao.Tcontroles.Add(new InfraBanco.Modelos.Tcontrole()
                {
                    Id_Nsu = Constantes.NSU_CADASTRO_CLIENTES,
                    Nsu = "001234567890",
                    Seq_Anual = 1,
                    Dt_Ult_Atualizacao = DateTime.Now
                });
                dbgravacao.SaveChanges();
                dbgravacao.transacao.Commit();
                Assert.Equal("001234567891", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
                Assert.Equal("001234567892", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
            }

            //agora mudamos a data
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                Apagar_t_CONTROLE();
                dbgravacao.Tcontroles.Add(new InfraBanco.Modelos.Tcontrole()
                {
                    Id_Nsu = Constantes.NSU_CADASTRO_CLIENTES,
                    Nsu = "001234567890",
                    Seq_Anual = 1,
                    Ano_Letra_Seq = "A",
                    Ano_Letra_Step = 2,
                    Dt_Ult_Atualizacao = DateTime.Now.AddYears(-10)
                });
                dbgravacao.SaveChanges();
                dbgravacao.transacao.Commit();
                Assert.Equal("000000000001", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
                Assert.Equal("000000000002", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
                Assert.Equal("000000000003", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
                Assert.Equal("000000000004", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
                Assert.Equal("000000000005", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);
                Assert.Equal("000000000006", global::UtilsGlobais.Nsu.GerarNsu(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES).Result);

                //tem que ter mudado a letra e a data
                var controle = (from c in dbgravacao.Tcontroles where c.Id_Nsu == Constantes.NSU_CADASTRO_CLIENTES select c).First();
                Assert.Equal("C", controle.Ano_Letra_Seq);

                //e a data foi atualizada
                Assert.Equal(DateTime.Now.Date, controle.Dt_Ult_Atualizacao.Date);
            }

            //e ver se continua funcioandno...
            Funcionando();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Testes.Utils.LogTestes.LogOperacoes2.MensagemEspecial("destrutor GerarNsuTeste", this);
                    this.inicializarBanco.InicializarForcado();
                }
                disposedValue = true;
            }
        }
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

    }
}
