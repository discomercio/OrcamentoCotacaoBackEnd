using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
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
        //nao deveria precisar poruqe os testes são mono-thread, mas não custa colocar
        private static readonly object _lockObject = new object();
        private static bool _inicialziado = false;
        private readonly ContextoBdProvider contextoBdProvider;
        private readonly ContextoCepProvider contextoCepProvider;
        private readonly LogTestes logTestes = LogTestes.GetInstance();

        public InicializarBancoGeral(InfraBanco.ContextoBdProvider contextoBdProvider, InfraBanco.ContextoCepProvider contextoCepProvider)
        {
            this.contextoBdProvider = contextoBdProvider;
            this.contextoCepProvider = contextoCepProvider;
            Inicializar(false);
        }

        public void InicializarForcado()
        {
            _inicialziado = false;
            Inicializar(true);
        }
        public void Inicializar(bool apagarDadosExistentes)
        {
            if (!_inicialziado)
            {
                lock (_lockObject)
                {
                    _inicialziado = true;
                    logTestes.LogMensagem("InicializarBancoGeral Inicializar inicio");
                    InicalizarInterno(apagarDadosExistentes);
                    logTestes.LogMensagem("InicializarBancoGeral CEP inicio");
                    new InicializarBancoCep(contextoCepProvider).Inicializar(apagarDadosExistentes);
                    logTestes.LogMensagem("InicializarBancoGeral Inicializar fim");
                }
            }
        }

        private void InicalizarInterno(bool apagarDadosExistentes)
        {
            using (var db = contextoBdProvider.GetContextoGravacaoParaUsing())
            {
                InicializarTabela<Tcliente>(db.Tclientes, "Tcliente", db, apagarDadosExistentes);
                InicializarTabela<TcodigoDescricao>(db.tcodigoDescricaos, "TcodigoDescricao", db, apagarDadosExistentes);
                InicializarTabela<Tcontrole>(db.Tcontroles, "Tcontrole", db, apagarDadosExistentes);
                InicializarTabela<TorcamentistaEindicador>(db.TorcamentistaEindicadors, "TorcamentistaEindicador", db, apagarDadosExistentes);
                InicializarTabela<Tusuario>(db.Tusuarios, "Tusuario", db, apagarDadosExistentes);
                InicializarTabela<Tparametro>(db.Tparametros, "Tparametro", db, apagarDadosExistentes);
                InicializarTabela<Tperfil>(db.Tperfils, "Tperfil", db, apagarDadosExistentes);
                InicializarTabela<TperfilUsuario>(db.TperfilUsuarios, "TperfilUsuario", db, apagarDadosExistentes);
                db.SaveChanges();
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
            string nomeTabela,
            ContextoBdGravacao db,
            bool apagarDadosExistentes)
                where TipoDados : class
        {
            if (apagarDadosExistentes)
            {
                foreach (var c in dbSet)
                    dbSet.Remove(c);
            }


            var nomeArquivo = "Especificacao.Testes.Utils.BancoTestes.Dados." + nomeTabela + ".json";
            using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(nomeArquivo);
            if (stream == null)
            {
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
                public static string Apelido_com_ra = "Apelido_com_ra";
                public static string Apelido_sem_ra = "Apelido_sem_ra";
                public static string Apelido_sem_vendedor = "Apelido_sem_vendedor";
                public static string Apelido_sem_loja = "Apelido_sem_loja";
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
                Permite_RA_Status = 1
            });
            db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
            {
                Apelido = Dados.Orcamentista.Apelido_sem_ra.ToUpper(),
                Vendedor = Dados.Orcamentista.Apelido_sem_ra.ToUpper(),
                Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                Permite_RA_Status = 0
            });
            db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
            {
                Apelido = Dados.Orcamentista.Apelido_sem_vendedor.ToUpper(),
                Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                Permite_RA_Status = 0
            });
            db.TorcamentistaEindicadors.Add(new InfraBanco.Modelos.TorcamentistaEindicador()
            {
                Apelido = Dados.Orcamentista.Apelido_sem_loja.ToUpper(),
                Vendedor = Dados.Orcamentista.Apelido_sem_loja.ToUpper(),
                Loja = "loja nao e-commerce",
                Permite_RA_Status = 1
            });
            db.SaveChanges();
        }
        /*
                        private void Inicalizar()
                        {
                                db.Tparametros.Add(new InfraBanco.Modelos.Tparametro() { Id = Constantes.ID_PARAMETRO_PERC_DESAGIO_RA_LIQUIDA, Campo_Real = 25 });
                                db.Tparametros.Add(new InfraBanco.Modelos.Tparametro() { Id = Constantes.ID_PARAMETRO_Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal, Campo_inteiro = 1 });

                                db.Tcontroles.Add(new InfraBanco.Modelos.Tcontrole() { Id_Nsu = Constantes.NSU_CADASTRO_CLIENTES, Nsu = "000000645506" });
                                db.Tcontroles.Add(new InfraBanco.Modelos.Tcontrole() { Id_Nsu = Constantes.NSU_ORCAMENTO, Nsu = "000000000006" });

                                db.Tbancos.Add(new InfraBanco.Modelos.Tbanco() { Codigo = Testes.Automatizados.InicializarBanco.InicializarClienteDados.ClienteNaoCadastradoPJ().RefBancaria[0].Banco });


                                InicalizarProdutos(db);


                                db.SaveChanges();
                            }
                        }

                        private void InicalizarProdutos(ContextoBdGravacao db)
                        {
                            var Id_wms_regra_cd = 1;
                            var Id_wms_regra_cd_x_uf = 1;
                            short Spe_id_nfe_emitente = 1;
                            int Id_wms_regra_cd_x_uf_x_pessoa = 1;

                            db.TnfEmitentes.Add(new InfraBanco.Modelos.TnfEmitente()
                            {
                                Id = Spe_id_nfe_emitente,
                                St_Ativo = 1
                            });


                            var dado = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez();
                            var produtos = dado.ListaProdutos;
                            foreach (var item in produtos)
                            {
                                if (!db.Tfabricantes.Any(f => f.Fabricante == item.Fabricante))
                                {
                                    db.Tfabricantes.Add(new InfraBanco.Modelos.Tfabricante() { Fabricante = item.Fabricante });
                                    db.SaveChanges();
                                }

                                if (!db.Tprodutos.Any(f => f.Fabricante == item.Fabricante && f.Produto == item.Produto))
                                {
                                    db.Tprodutos.Add(new InfraBanco.Modelos.Tproduto()
                                    {
                                        Fabricante = item.Fabricante,
                                        Produto = item.Produto,
                                        Descontinuado = "N"
                                    });

                                    db.TprodutoLojas.Add(new InfraBanco.Modelos.TprodutoLoja()
                                    {
                                        Produto = item.Produto,
                                        Fabricante = item.Fabricante,
                                        Vendavel = "S",
                                        Loja = Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE,
                                        Preco_Lista = item.Preco_Lista / Convert.ToDecimal(item.CustoFinancFornecCoeficiente)
                                    });

                                    {
                                        //regras de consumo de estoque

                                        db.TprodutoXwmsRegraCds.Add(new InfraBanco.Modelos.TprodutoXwmsRegraCd()
                                        {
                                            Produto = item.Produto,
                                            Fabricante = item.Fabricante,
                                            Id_wms_regra_cd = Id_wms_regra_cd
                                        });

                                        db.TwmsRegraCds.Add(new InfraBanco.Modelos.TwmsRegraCd()
                                        {
                                            Id = Id_wms_regra_cd,
                                            Apelido = "wmsRegra.Apelido",
                                            Descricao = "wmsRegra.Descricao",
                                            St_inativo = 0
                                        });

                                        db.TwmsRegraCdXUfs.Add(new InfraBanco.Modelos.TwmsRegraCdXUf()
                                        {
                                            Id = Id_wms_regra_cd_x_uf,
                                            Id_wms_regra_cd = Id_wms_regra_cd,
                                            Uf = dado.EnderecoCadastralCliente.Endereco_uf
                                        });

                                        InicalizarProdutos_TwmsRegraCdXUfPessoas(db, Id_wms_regra_cd_x_uf, Spe_id_nfe_emitente, Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA, ref Id_wms_regra_cd_x_uf_x_pessoa);
                                        InicalizarProdutos_TwmsRegraCdXUfPessoas(db, Id_wms_regra_cd_x_uf, Spe_id_nfe_emitente, Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL, ref Id_wms_regra_cd_x_uf_x_pessoa);
                                        InicalizarProdutos_TwmsRegraCdXUfPessoas(db, Id_wms_regra_cd_x_uf, Spe_id_nfe_emitente, Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE, ref Id_wms_regra_cd_x_uf_x_pessoa);
                                        InicalizarProdutos_TwmsRegraCdXUfPessoas(db, Id_wms_regra_cd_x_uf, Spe_id_nfe_emitente, Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE, ref Id_wms_regra_cd_x_uf_x_pessoa);
                                        InicalizarProdutos_TwmsRegraCdXUfPessoas(db, Id_wms_regra_cd_x_uf, Spe_id_nfe_emitente, Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO, ref Id_wms_regra_cd_x_uf_x_pessoa);

                                        //Spe_id_nfe_emitente++;
                                        Id_wms_regra_cd_x_uf++;
                                        Id_wms_regra_cd++;
                                    }

                                    db.SaveChanges();
                                }
                            }

                            //coeficientes
                            foreach (var f in db.Tfabricantes)
                            {
                                db.TpercentualCustoFinanceiroFornecedors.Add(new InfraBanco.Modelos.TpercentualCustoFinanceiroFornecedor()
                                {
                                    Fabricante = f.Fabricante,
                                    Tipo_Parcelamento = dado.FormaPagtoCriacao.CustoFinancFornecTipoParcelamento,
                                    Qtde_Parcelas = (short)dado.FormaPagtoCriacao.C_pc_qtde.Value,
                                    Coeficiente = produtos.First(lp => lp.Fabricante == f.Fabricante).CustoFinancFornecCoeficiente
                                });
                            }
                        }

                        private static void InicalizarProdutos_TwmsRegraCdXUfPessoas(ContextoBdGravacao db, int Id_wms_regra_cd_x_uf, short Spe_id_nfe_emitente, string Tipo_pessoa,
                            ref int Id_wms_regra_cd_x_uf_x_pessoa)
                        {
                            db.TwmsRegraCdXUfPessoas.Add(new InfraBanco.Modelos.TwmsRegraCdXUfPessoa()
                            {
                                Id = Id_wms_regra_cd_x_uf_x_pessoa,
                                Tipo_pessoa = Tipo_pessoa,
                                Id_wms_regra_cd_x_uf = Id_wms_regra_cd_x_uf,
                                Spe_id_nfe_emitente = Spe_id_nfe_emitente,
                                St_inativo = 0
                            });

                            db.TwmsRegraCdXUfXPessoaXCds.Add(new InfraBanco.Modelos.TwmsRegraCdXUfXPessoaXCd()
                            {
                                Id_wms_regra_cd_x_uf_x_pessoa = Id_wms_regra_cd_x_uf_x_pessoa,
                                Id_nfe_emitente = Spe_id_nfe_emitente
                            });

                            Id_wms_regra_cd_x_uf_x_pessoa++;
                        }

                */
    }
}
