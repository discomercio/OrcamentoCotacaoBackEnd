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
            //todo: tirar parametro
            var apagarDadosExistentes = true;
            using (var db = contextoBdProvider.GetContextoGravacaoParaUsing())
            {
                //estas, só apagamos
                InicializarTabela<TpedidoItem>(db.TpedidoItems, null, db, apagarDadosExistentes);
                InicializarTabela<Torcamento>(db.Torcamentos, null, db, apagarDadosExistentes);
                InicializarTabela<TorcamentoItem>(db.TorcamentoItem, null, db, apagarDadosExistentes);
                InicializarTabela<Tdesconto>(db.Tdescontos, null, db, apagarDadosExistentes);
                InicializarTabela<TpedidoAnaliseEndereco>(db.TpedidoAnaliseEnderecos, null, db, apagarDadosExistentes);
                InicializarTabela<TpedidoAnaliseEnderecoConfrontacao>(db.TpedidoAnaliseConfrontacaos, null, db, apagarDadosExistentes);
                InicializarTabela<TusuarioXLoja>(db.TusuarioXLojas, null, db, apagarDadosExistentes);
                InicializarTabela<TclienteRefComercial>(db.TclienteRefComercials, null, db, apagarDadosExistentes);
                InicializarTabela<TclienteRefBancaria>(db.TclienteRefBancarias, null, db, apagarDadosExistentes);

                InicializarTabela<Tcliente>(db.Tclientes, "Tcliente", db, apagarDadosExistentes);
                InicializarTabela<TcodigoDescricao>(db.TcodigoDescricaos, "TcodigoDescricao", db, apagarDadosExistentes);
                InicializarTabela<Tcontrole>(db.Tcontroles, "Tcontrole", db, apagarDadosExistentes);
                InicializarTabela<Testoque>(db.Testoques, "Testoque", db, apagarDadosExistentes);
                InicializarTabela<TestoqueItem>(db.TestoqueItems, "TestoqueItem", db, apagarDadosExistentes);
                InicializarTabela<TestoqueLog>(db.TestoqueLogs, null, db, apagarDadosExistentes);
                InicializarTabela<TestoqueMovimento>(db.TestoqueMovimentos, "TestoqueMovimento", db, apagarDadosExistentes);
                InicializarTabela<Tfabricante>(db.Tfabricantes, "Tfabricante", db, apagarDadosExistentes);
                InicializarTabela<TfinControle>(db.TfinControles, "TfinControle", db, apagarDadosExistentes);
                InicializarTabela<TformaPagto>(db.TformaPagtos, "TformaPagto", db, apagarDadosExistentes);
                InicializarTabela<Tloja>(db.Tlojas, "Tlojas", db, apagarDadosExistentes);
                InicializarTabela<TnfEmitente>(db.TnfEmitentes, "TnfEmitente", db, apagarDadosExistentes);
                InicializarTabela<Toperacao>(db.Toperacaos, "Toperacao", db, apagarDadosExistentes);
                InicializarTabela<TorcamentistaEindicador>(db.TorcamentistaEindicadors, "TorcamentistaEindicador", db, apagarDadosExistentes);
                InicializarTabela<TorcamentistaEIndicadorRestricaoFormaPagto>(db.TorcamentistaEIndicadorRestricaoFormaPagtos, "TorcamentistaEIndicadorRestricaoFormaPagto", db, apagarDadosExistentes);
                InicializarTabela<Tparametro>(db.Tparametros, "Tparametro", db, apagarDadosExistentes);
                InicializarTabela<Tpedido>(db.Tpedidos, "Tpedido", db, apagarDadosExistentes);
                InicializarTabela<TpercentualCustoFinanceiroFornecedor>(db.TpercentualCustoFinanceiroFornecedors, "TpercentualCustoFinanceiroFornecedors", db, apagarDadosExistentes);
                InicializarTabela<Tperfil>(db.Tperfils, "Tperfil", db, apagarDadosExistentes);
                InicializarTabela<TperfilItem>(db.TperfilItens, "TperfilItens", db, apagarDadosExistentes);
                InicializarTabela<TperfilUsuario>(db.TperfilUsuarios, "TperfilUsuario", db, apagarDadosExistentes);
                InicializarTabela<Tproduto>(db.Tprodutos, "Tproduto", db, apagarDadosExistentes);
                InicializarTabela<TprodutoLoja>(db.TprodutoLojas, "TprodutoLoja", db, apagarDadosExistentes);
                InicializarTabela<TprodutoXwmsRegraCd>(db.TprodutoXwmsRegraCds, "TprodutoXwmsRegraCd", db, apagarDadosExistentes);
                InicializarTabela<Tusuario>(db.Tusuarios, "Tusuario", db, apagarDadosExistentes);
                InicializarTabela<TwmsRegraCd>(db.TwmsRegraCds, "TwmsRegraCd", db, apagarDadosExistentes);
                InicializarTabela<TwmsRegraCdXUf>(db.TwmsRegraCdXUfs, "TwmsRegraCdXUf", db, apagarDadosExistentes);
                InicializarTabela<TwmsRegraCdXUfPessoa>(db.TwmsRegraCdXUfPessoas, "TwmsRegraCdXUfPessoa", db, apagarDadosExistentes);
                InicializarTabela<TwmsRegraCdXUfXPessoaXCd>(db.TwmsRegraCdXUfXPessoaXCds, "TwmsRegraCdXUfXPessoaXCd", db, apagarDadosExistentes);
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
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
            {
                Apelido = Dados.Orcamentista.Apelido_com_ra.ToUpper(),
                Vendedor = Dados.Orcamentista.Apelido_com_ra.ToUpper(),
                Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                Razao_Social_Nome= "Teste",
                Permite_RA_Status = 1
            });
            db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
            {
                Apelido = Dados.Orcamentista.Apelido_sem_ra.ToUpper(),
                Vendedor = Dados.Orcamentista.Apelido_sem_ra.ToUpper(),
                Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                Razao_Social_Nome = "Teste",
                Permite_RA_Status = 0
            });
            db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
            {
                Apelido = Dados.Orcamentista.Apelido_sem_vendedor.ToUpper(),
                Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                Razao_Social_Nome = "Teste",
                Permite_RA_Status = 0
            });
            db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
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
