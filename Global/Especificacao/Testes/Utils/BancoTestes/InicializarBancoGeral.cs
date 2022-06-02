using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using Xunit;

namespace Especificacao.Testes.Utils.BancoTestes
{
    public class InicializarBancoGeral
    {
        //ára acessar o banco mais facilmente do debug
        public static InfraBanco.ContextoBd ObterContextoBd()
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            //precisa restaurar o banco
            var bd = servicos.GetRequiredService<InfraBanco.ContextoBdProvider>();
            return bd.GetContextoLeitura();
        }

        //nao deveria precisar poruqe os testes são mono-thread, mas não custa colocar
        private static readonly object _lockObject = new object();
        private static bool _inicialziado = false;
        private readonly ContextoBdProvider contextoBdProvider;
        private readonly ContextoCepProvider contextoCepProvider;
        private readonly LogTestes.LogTestes logTestes = LogTestes.LogTestes.GetInstance();

        public InicializarBancoGeral(InfraBanco.ContextoBdProvider contextoBdProvider, InfraBanco.ContextoCepProvider contextoCepProvider)
        {
            this.contextoBdProvider = contextoBdProvider;
            this.contextoCepProvider = contextoCepProvider;
            Inicializar();
        }

        public void InicializarForcado()
        {
            _inicialziado = false;
            Inicializar();
        }
        public void Inicializar()
        {
            if (!_inicialziado)
            {
                lock (_lockObject)
                {
                    _inicialziado = true;
                    logTestes.LogMemoria("InicializarBancoGeral Inicializar inicio");
                    InicalizarInterno();
                    logTestes.LogMensagem("InicializarBancoGeral CEP inicio");
                    new InicializarBancoCep(contextoCepProvider).Inicializar();
                    logTestes.LogMemoria("InicializarBancoGeral Inicializar fim");
                }
            }
        }

        private void InicalizarInterno()
        {
            var apagarDadosExistentes = true;
            using (var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                //estas, só apagamos
                InicializarTabela<TpedidoItem>(db.TpedidoItem, null, db, apagarDadosExistentes);
                InicializarTabela<Torcamento>(db.Torcamento, null, db, apagarDadosExistentes);
                InicializarTabela<TorcamentoItem>(db.TorcamentoItem, null, db, apagarDadosExistentes);
                InicializarTabela<Tdesconto>(db.Tdesconto, null, db, apagarDadosExistentes);
                InicializarTabela<TpedidoAnaliseEndereco>(db.TpedidoAnaliseEndereco, null, db, apagarDadosExistentes);
                InicializarTabela<TpedidoAnaliseEnderecoConfrontacao>(db.TpedidoAnaliseConfrontacao, null, db, apagarDadosExistentes);
                InicializarTabela<TusuarioXLoja>(db.TusuarioXLoja, null, db, apagarDadosExistentes);
                InicializarTabela<TclienteRefComercial>(db.TclienteRefComercial, null, db, apagarDadosExistentes);
                InicializarTabela<TclienteRefBancaria>(db.TclienteRefBancaria, null, db, apagarDadosExistentes);
                InicializarTabela<TestoqueLog>(db.TestoqueLog, null, db, apagarDadosExistentes);
                InicializarTabela<Tlog>(db.Tlog, null, db, apagarDadosExistentes);

                InicializarTabela<Tcliente>(db.Tcliente, "Tcliente", db, apagarDadosExistentes);
                InicializarTabela<TcodigoDescricao>(db.TcodigoDescricao, "TcodigoDescricao", db, apagarDadosExistentes);
                InicializarTabela<Tcontrole>(db.Tcontrole, "Tcontrole", db, apagarDadosExistentes);
                InicializarTabela<Testoque>(db.Testoque, "Testoque", db, apagarDadosExistentes);
                InicializarTabela<TestoqueItem>(db.TestoqueItem, "TestoqueItem", db, apagarDadosExistentes);
                InicializarTabela<TestoqueMovimento>(db.TestoqueMovimento, "TestoqueMovimento", db, apagarDadosExistentes);
                InicializarTabela<Tfabricante>(db.Tfabricante, "Tfabricante", db, apagarDadosExistentes);
                InicializarTabela<TfinControle>(db.TfinControle, "TfinControle", db, apagarDadosExistentes);
                InicializarTabela<TformaPagto>(db.TformaPagto, "TformaPagto", db, apagarDadosExistentes);
                InicializarTabela<Tloja>(db.Tloja, "Tlojas", db, apagarDadosExistentes);
                InicializarTabela<TnfEmitente>(db.TnfEmitente, "TnfEmitente", db, apagarDadosExistentes);
                InicializarTabela<Toperacao>(db.Toperacao, "Toperacao", db, apagarDadosExistentes);
                InicializarTabela<TorcamentistaEindicador>(db.TorcamentistaEindicador, "TorcamentistaEindicador", db, apagarDadosExistentes);
                InicializarTabela<TorcamentistaEIndicadorRestricaoFormaPagto>(db.TorcamentistaEIndicadorRestricaoFormaPagto, "TorcamentistaEIndicadorRestricaoFormaPagto", db, apagarDadosExistentes);
                InicializarTabela<Tparametro>(db.Tparametros, "Tparametro", db, apagarDadosExistentes);
                InicializarTabela<Tpedido>(db.Tpedido, "Tpedido", db, apagarDadosExistentes);
                InicializarTabela<TpercentualCustoFinanceiroFornecedor>(db.TpercentualCustoFinanceiroFornecedors, "TpercentualCustoFinanceiroFornecedors", db, apagarDadosExistentes);
                InicializarTabela<Tperfil>(db.Tperfil, "Tperfil", db, apagarDadosExistentes);
                InicializarTabela<TperfilItem>(db.TperfilIten, "TperfilItens", db, apagarDadosExistentes);
                InicializarTabela<TperfilUsuario>(db.TperfilUsuario, "TperfilUsuario", db, apagarDadosExistentes);
                InicializarTabela<Tproduto>(db.Tproduto, "Tproduto", db, apagarDadosExistentes);
                InicializarTabela<TprodutoLoja>(db.TprodutoLoja, "TprodutoLoja", db, apagarDadosExistentes);
                InicializarTabela<TprodutoXwmsRegraCd>(db.TprodutoXwmsRegraCds, "TprodutoXwmsRegraCd", db, apagarDadosExistentes);
                db.SaveChanges();
                db.transacao.Commit();
            }
            using (var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                InicializarTabela<Tusuario>(db.Tusuario, "Tusuario", db, apagarDadosExistentes);
                InicializarTabela<TwmsRegraCd>(db.TwmsRegraCd, "TwmsRegraCd", db, apagarDadosExistentes);
                db.SaveChanges();
                db.transacao.Commit();
            }
            using (var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                InicializarTabela<TwmsRegraCdXUf>(db.TwmsRegraCdXUf, "TwmsRegraCdXUf", db, apagarDadosExistentes);
                InicializarTabela<TwmsRegraCdXUfPessoa>(db.TwmsRegraCdXUfPessoa, "TwmsRegraCdXUfPessoa", db, apagarDadosExistentes);
                InicializarTabela<TwmsRegraCdXUfXPessoaXCd>(db.TwmsRegraCdXUfXPessoaXCd, "TwmsRegraCdXUfXPessoaXCd", db, apagarDadosExistentes);
                db.SaveChanges();
                db.transacao.Commit();
            }

            Inicalizar_TorcamentistaEindicador();

            /*
            var x = from c in contextoBdProvider.GetContextoLeitura().Tclientes select c;
            var xc = x.Count();
            var y = from c in contextoBdProvider.GetContextoLeitura().TorcamentistaEindicadors select c;
            var yc = y.Count();
            */
        }

        private void InicializarTabela<TipoDados>(Microsoft.EntityFrameworkCore.DbSet<TipoDados> dbSet,
            string? nomeTabela,
            ContextoBdGravacao db,
            bool apagarDadosExistentes)
                where TipoDados : class
        {
            if (apagarDadosExistentes)
            {
                foreach (var c in dbSet)
                    dbSet.Remove(c);
            }
            if (nomeTabela == null)
                return;

            var nomeArquivo = "Especificacao.Testes.Utils.BancoTestes.Dados." + nomeTabela + ".json";
            using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(nomeArquivo);
            if (stream == null)
            {
                Testes.Utils.LogTestes.LogOperacoes2.Excecao("InicializarTabela: " + nomeArquivo + $"StackTrace: '{Environment.StackTrace}'", this);
                Assert.Equal("", nomeArquivo + $"StackTrace: '{Environment.StackTrace}'");
                throw new NullReferenceException(nomeArquivo);
            }
            using StreamReader reader = new StreamReader(stream);
            var texto = reader.ReadToEnd();
            var clientes = JsonConvert.DeserializeObject<List<TipoDados>>(texto);

            foreach (var cliente in clientes)
                db.Add(cliente);
        }

        static public class Dados
        {
            static public class Orcamentista
            {
                public static string Apelido_com_ra = "Ap_com_ra";
                public static string Apelido_sem_ra = "Ap_sem_ra";
                public static string Apelido_sem_vendedor = "Ap_sem_ven";
                public static string Apelido_sem_loja = "Ap_sem_lj";
                public static string ApelidoNaoExiste = "XXX";
            }
        }

        private void Inicalizar_TorcamentistaEindicador()
        {
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            db.TorcamentistaEindicador.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
            {
                Apelido = Dados.Orcamentista.Apelido_com_ra.ToUpper(),
                Vendedor = Dados.Orcamentista.Apelido_com_ra.ToUpper(),
                Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                Razao_Social_Nome = "Teste",
                Permite_RA_Status = 1
            });
            db.TorcamentistaEindicador.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
            {
                Apelido = Dados.Orcamentista.Apelido_sem_ra.ToUpper(),
                Vendedor = Dados.Orcamentista.Apelido_sem_ra.ToUpper(),
                Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                Razao_Social_Nome = "Teste",
                Permite_RA_Status = 0
            });
            db.TorcamentistaEindicador.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
            {
                Apelido = Dados.Orcamentista.Apelido_sem_vendedor.ToUpper(),
                Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                Razao_Social_Nome = "Teste",
                Permite_RA_Status = 0
            });
            db.TorcamentistaEindicador.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
            {
                Apelido = Dados.Orcamentista.Apelido_sem_loja.ToUpper(),
                Vendedor = Dados.Orcamentista.Apelido_sem_loja.ToUpper(),
                Loja = "321",
                Razao_Social_Nome = "Teste",
                Permite_RA_Status = 1
            });
            db.SaveChanges();
            db.transacao.Commit();
        }

    }
}
