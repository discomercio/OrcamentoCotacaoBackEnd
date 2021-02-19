using Especificacao.Testes.Utils.InjecaoDependencia;
using Especificacao.Testes.Utils.ListaDependencias;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Testes.Utils.BancoTestes
{
    [Binding, Scope(Tag = "GerenciamentoBanco")]
    public class GerenciamentoBancoSteps
    {
        private readonly ContextoBdProvider contextoBdProvider;
        public GerenciamentoBancoSteps()
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            this.contextoBdProvider = servicos.GetRequiredService<InfraBanco.ContextoBdProvider>();
        }

        #region Limpar tabela 
        [Given(@"Limpar tabela ""(.*)""")]
        public void GivenLimparTabela(string tabela)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.LimparTabela(tabela, this);
            LimparTabela(tabela);
        }

        private void LimparTabela(string tabela)
        {
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            switch (tabela)
            {
                case "t_PRODUTO_LOJA":
                    LimparTabelaDbSet<TprodutoLoja>(db.TprodutoLojas);
                    break;
                case "t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR":
                    LimparTabelaDbSet<TpercentualCustoFinanceiroFornecedor>(db.TpercentualCustoFinanceiroFornecedors);
                    break;
                case "t_PRAZO_PAGTO_VISANET":
                    LimparTabelaDbSet<TprazoPagtoVisanet>(db.TprazoPagtoVisanets);
                    break;
                case "t_CLIENTE":
                    LimparTabelaDbSet<Tcliente>(db.Tclientes);
                    break;
                default:
                    Testes.Utils.LogTestes.LogOperacoes2.Excecao($"Especificacao.Testes.Utils.BancoTestes.InicializarBancoGeral.LimparTabela nome de tabela desconhecido: {tabela}" + $"StackTrace: '{Environment.StackTrace}'", this);
                    throw new ArgumentException($"Especificacao.Testes.Utils.BancoTestes.InicializarBancoGeral.LimparTabela nome de tabela desconhecido: {tabela}");
            }
            db.SaveChanges();
            db.transacao.Commit();
        }

        public static void LimparTabelaDbSet<TipoDados>(Microsoft.EntityFrameworkCore.DbSet<TipoDados> dbSet)
            where TipoDados : class
        {
            foreach (var c in dbSet)
                dbSet.Remove(c);
        }
        #endregion

        #region Reiniciar banco 
        private bool reiniciarBancoAoTerminarCenario = false;
        [Given(@"Reiniciar banco ao terminar cenário")]
        public void GivenReiniciarBancoAoTerminarCenario()
        {
            reiniciarBancoAoTerminarCenario = true;
        }
        [AfterScenario]
        public void AfterScenario()
        {
            if (!reiniciarBancoAoTerminarCenario)
                return;
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            //precisa restaurar o banco
            var bd = new Testes.Utils.BancoTestes.InicializarBancoGeral(servicos.GetRequiredService<InfraBanco.ContextoBdProvider>(), servicos.GetRequiredService<InfraBanco.ContextoCepProvider>());
            bd.InicializarForcado();
        }
        #endregion


        [Then(@"Tabela ""t_CLIENTE"" registro com campo ""cnpj_cpf"" = ""(.*)"", verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabela_t_CLIENTE_RegistroComCampo_cnpj_cpf_VerificarCampo(string valor_cnpj_cpf, string campo, string valor_desejado)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_CLIENTE", "cnpj_cpf", valor_cnpj_cpf, campo, valor_desejado, this);

            var db = this.contextoBdProvider.GetContextoLeitura();
            var registro = (from cliente in db.Tclientes where cliente.Cnpj_Cpf == valor_cnpj_cpf select cliente).ToList();
            //só deve ter um registro
            Assert.Single(registro);

            //tiramos um clone
            string original = Newtonsoft.Json.JsonConvert.SerializeObject(registro[0]);
            Tcliente copia = Newtonsoft.Json.JsonConvert.DeserializeObject<Tcliente>(original);
            if (!WhenInformoCampo.InformarCampo(campo, valor_desejado, copia))
                throw new Exception($"Campo {campo} não encontrado em Tcliente");
            string desejado = Newtonsoft.Json.JsonConvert.SerializeObject(copia);
            if (desejado != original)
                LogTestes.LogTestes.ErroNosTestes($"ThenTabelaRegistroComCampoVerificarCampo t_CLIENTE campo {campo} valor errado, {desejado}, {original}");
            Assert.Equal(desejado, original);
        }

        public void TabelaT_PEDIDORegistroVerificarCampo(List<string> listaPedidos, string campo, string valor_desejado)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO", "pedido", String.Join("; ", listaPedidos), campo, valor_desejado, this);

            var db = this.contextoBdProvider.GetContextoLeitura();
            var registros = (from pedido in db.Tpedidos where listaPedidos.Contains(pedido.Pedido) select pedido).ToList();
            //deve ter um ou mais registros
            Assert.True(registros.Any());

            foreach (var registro in registros)
            {
                //tiramos um clone
                string original = Newtonsoft.Json.JsonConvert.SerializeObject(registro);
                Tpedido copia = Newtonsoft.Json.JsonConvert.DeserializeObject<Tpedido>(original);
                if (!WhenInformoCampo.InformarCampo(campo, valor_desejado, copia))
                    throw new Exception($"Campo {campo} não encontrado em Tpedido");
                string desejado = Newtonsoft.Json.JsonConvert.SerializeObject(copia);
                if (desejado != original)
                    LogTestes.LogTestes.ErroNosTestes($"ThenTabelaRegistroComCampoVerificarCampo t_PEDIDO campo {campo} valor errado, {desejado}, {original}");
                Assert.Equal(desejado, original);
            }
        }

        public void TabelaT_PEDIDO_ITEMRegistroVerificarCampo(int item, string pedido, string campo, string valor_desejado)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO_ITEM", "pedido", pedido, campo, valor_desejado, this);

            var db = this.contextoBdProvider.GetContextoLeitura();
            var registros = (from registro in db.TpedidoItems
                             where registro.Pedido.Contains(pedido) &&
                             registro.Sequencia == item
                             select registro).ToList();
            //deve ter um ou mais registros
            Assert.True(registros.Any());

            foreach (var registro in registros)
            {
                //tiramos um clone
                string original = Newtonsoft.Json.JsonConvert.SerializeObject(registro);
                TpedidoItem copia = Newtonsoft.Json.JsonConvert.DeserializeObject<TpedidoItem>(original);
                if (!WhenInformoCampo.InformarCampo(campo, valor_desejado, copia))
                    throw new Exception($"Campo {campo} não encontrado em TpedidoItem");
                string desejado = Newtonsoft.Json.JsonConvert.SerializeObject(copia);
                if (desejado != original)
                    LogTestes.LogTestes.ErroNosTestes($"ThenTabelaRegistroComCampoVerificarCampo t_PEDIDO_ITEM campo {campo} valor errado, {desejado}, {original}");
                Assert.Equal(desejado, original);
            }
        }

        [Given(@"Tabela ""t_OPERACAO"" apagar registro com campo ""id"" = ""(.*)""")]
        public void GivenTabelaApagarRegistroComCampo(string valorBusca)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaApagarRegistroComCampo("t_OPERACAO", "id", valorBusca, this);

            int valorInt = -1;
            switch (valorBusca)
            {
                case "OP_LJA_CADASTRA_NOVO_PEDIDO":
                    valorInt = Constantes.OP_LJA_CADASTRA_NOVO_PEDIDO;
                    break;
                case "OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO":
                    valorInt = Constantes.OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO;
                    break;
                default:
                    throw new ArgumentException($"GivenTabelaApagarRegistroComCampo desconhecido: {valorBusca}");
            }

            using var db = this.contextoBdProvider.GetContextoGravacaoParaUsing();
            var registro = (from operacao in db.Toperacaos where operacao.Id == valorInt select operacao).First();
            db.Toperacaos.Remove(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_USUARIO"" apagar registro com campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaT_USUARIOApagarRegistroComCampo(string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaApagarRegistroComCampo("t_USUARIO", "usuario", valor, this);

            //int valorInt = -1;
            //switch (valor)
            //{
            //    case "OP_LJA_CADASTRA_NOVO_PEDIDO":
            //        valorInt = Constantes.OP_LJA_CADASTRA_NOVO_PEDIDO;
            //        break;
            //    case "OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO":
            //        valorInt = Constantes.OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO;
            //        break;
            //    default:
            //        throw new ArgumentException($"GivenTabelaApagarRegistroComCampo desconhecido: {valorBusca}");
            //}

            using var db = this.contextoBdProvider.GetContextoGravacaoParaUsing();
            var registro = (from usuario in db.Tusuarios where usuario.Usuario == valor select usuario).First();
            db.Tusuarios.Remove(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela t_CLIENTE registro com cpf_cnpj = ""(.*)"" alterar campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaT_CLIENTERegistroComCpf_CnpjAlterarCampo(string valor_cnpj_cpf, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaAlterarRegistroComCampo("t_CLIENTE", "cpf_cnpj", valor, this);

            var db = this.contextoBdProvider.GetContextoGravacaoParaUsing();
            var registro = (from cliente in db.Tclientes
                            where cliente.Cnpj_Cpf == valor_cnpj_cpf
                            select cliente).FirstOrDefault();

            Assert.NotNull(registro);

            if (!WhenInformoCampo.InformarCampo(campo, valor, registro))
                Assert.Equal("campo desconhecido", campo);

            db.Update(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_LOJA"" com loja = ""(.*)"" alterar campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaT_LOJARegistroComLojaAlterarCampo(string valor_loja, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaAlterarRegistroComCampo("t_LOJA", "loja", valor, this);

            var db = this.contextoBdProvider.GetContextoGravacaoParaUsing();
            var registro = (from loja in db.Tlojas
                            where loja.Loja == valor_loja
                            select loja).FirstOrDefault();

            Assert.NotNull(registro);

            if (!WhenInformoCampo.InformarCampo(campo, valor, registro))
                Assert.Equal("campo desconhecido", campo);

            db.Update(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_PARAMETRO"" com id = ""(.*)"" alterar campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaT_PARAMETRORegistroComIdAlterarCampo(string valor_id, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaAlterarRegistroComCampo("t_PARAMETRO", "loja", valor, this);

            var db = this.contextoBdProvider.GetContextoGravacaoParaUsing();
            var registro = (from parametro in db.Tparametros
                            where parametro.Id == valor_id
                            select parametro).FirstOrDefault();

            Assert.NotNull(registro);

            if (!WhenInformoCampo.InformarCampo(campo, valor, registro))
                Assert.Equal("campo desconhecido", campo);

            db.Update(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Novo registro em ""(.*)"", campo ""(.*)"" = ""(.*)""")]
        public void GivenNovoRegistroEmCampo(string tabela, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.NovoRegistroEmCampo(tabela, campo, valor, this);

            switch (tabela)
            {
                case "t_PRAZO_PAGTO_VISANET":
                    NovoRegistroEm_t_PRAZO_PAGTO_VISANET(tabela, campo, valor);
                    break;
                case "t_PRODUTO_LOJA":
                    NovoRegistroEm_t_PRODUTO_LOJA(tabela, campo, valor);
                    break;
                case "t_DESCONTO":
                    NovoRegistroEm_t_DESCONTO(tabela, campo, valor);
                    break;

                default:
                    Assert.Equal("", $"{tabela} desconhecido");
                    break;
            }
        }

        [Given(@"Novo registro na tabela ""(.*)""")]
        public void GivenNovoRegistroNaTabela(string tabela)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.NovoRegistroNaTabela(tabela, this);
            switch (tabela)
            {
                case "t_PRAZO_PAGTO_VISANET":
                    Assert.Equal("t_PRAZO_PAGTO_VISANET", tabela);
                    tprazoPagtoVisanet = new TprazoPagtoVisanet();
                    break;
                case "t_PRODUTO_LOJA":
                    Assert.Equal("t_PRODUTO_LOJA", tabela);
                    tprodutoLoja = new TprodutoLoja();
                    break;
                case "t_DESCONTO":
                    Assert.Equal("t_DESCONTO", tabela);
                    tdesconto = new Tdesconto();
                    break;

                default:
                    Assert.Equal("", $"{tabela} desconhecido");
                    break;
            }
        }

        [Given(@"Gravar registro em ""(.*)""")]
        public void GivenGravarRegistroEm(string tabela)
        {
            switch (tabela)
            {
                case "t_PRAZO_PAGTO_VISANET":
                    GravarRegistroNa_t_PRAZO_PAGTO_VISANET(tabela);
                    break;
                case "t_PRODUTO_LOJA":
                    GravarRegistroNa_t_PRODUTO_LOJA(tabela);
                    break;
                case "t_DESCONTO":
                    GravarRegistroNa_t_DESCONTO(tabela);
                    break;
                default:
                    Assert.Equal("", $"{tabela} desconhecido");
                    break;
            }
        }

        #region Criação de novo registro t_DESCONTO
        private Tdesconto tdesconto = new Tdesconto();
        private void GravarRegistroNa_t_DESCONTO(string tabela)
        {
            Assert.Equal("t_DESCONTO", tabela);
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.GravarRegistroEm(tabela, this);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            db.TprazoPagtoVisanets.Add(tprazoPagtoVisanet);
            db.SaveChanges();
            db.transacao.Commit();
        }
        private void NovoRegistroEm_t_DESCONTO(string tabela, string campo, string valor)
        {
            Assert.Equal("t_DESCONTO", tabela);
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.NovoRegistroEmCampo(tabela, campo, valor, this);
            switch (campo)
            {
                case "usado_status":
                    if (!short.TryParse(valor, out short usado_status))
                        Assert.Equal("", $"{valor} não é numero");
                    tdesconto.Usado_status = usado_status;
                    break;
                case "cancelado_status":
                    if (!short.TryParse(valor, out short cancelado_status))
                        Assert.Equal("", $"{valor} não é numero");
                    tdesconto.Cancelado_status = cancelado_status;
                    break;
                case "fabricante":
                    tdesconto.Fabricante = valor;
                    break;
                case "produto":
                    tdesconto.Produto = valor;
                    break;
                case "loja":
                    tdesconto.Loja = valor;
                    break;
                case "data":
                    if (!DateTime.TryParse(valor, out DateTime data))
                        Assert.Equal("", $"{valor} não é uma Data");
                    tdesconto.Data = data;
                    break;
                default:
                    Assert.Equal("", $"{campo} desconhecido");
                    break;
            }
        }
        #endregion

        #region Criação de novo registro t_PRAZO_PAGTO_VISANET
        private TprazoPagtoVisanet tprazoPagtoVisanet = new InfraBanco.Modelos.TprazoPagtoVisanet();
        private void GravarRegistroNa_t_PRAZO_PAGTO_VISANET(string tabela)
        {
            Assert.Equal("t_PRAZO_PAGTO_VISANET", tabela);
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.GravarRegistroEm(tabela, this);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            db.TprazoPagtoVisanets.Add(tprazoPagtoVisanet);
            db.SaveChanges();
            db.transacao.Commit();
        }
        private void NovoRegistroEm_t_PRAZO_PAGTO_VISANET(string tabela, string campo, string valor)
        {
            Assert.Equal("t_PRAZO_PAGTO_VISANET", tabela);
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.NovoRegistroEmCampo(tabela, campo, valor, this);
            switch (campo)
            {
                case "tipo":
                    switch (valor)
                    {
                        case "Constantes.COD_VISANET_PRAZO_PAGTO_LOJA":
                            tprazoPagtoVisanet.Tipo = Constantes.COD_VISANET_PRAZO_PAGTO_LOJA;
                            break;
                        default:
                            Assert.Equal("", $"{valor} desconhecido");
                            break;
                    }
                    break;
                case "qtde_parcelas":
                    if (!short.TryParse(valor, out short qtdeParcelas))
                        Assert.Equal("", $"{valor} não é numero");
                    tprazoPagtoVisanet.Qtde_parcelas = qtdeParcelas;
                    break;

                default:
                    Assert.Equal("", $"{campo} desconhecido");
                    break;
            }
        }
        #endregion

        #region Criação de novo registro t_PRODUTO_LOJA
        private TprodutoLoja tprodutoLoja = new TprodutoLoja();
        private void GravarRegistroNa_t_PRODUTO_LOJA(string tabela)
        {
            Assert.Equal("t_PRODUTO_LOJA", tabela);
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.GravarRegistroEm(tabela, this);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            db.TprodutoLojas.Add(tprodutoLoja);
            db.SaveChanges();
            db.transacao.Commit();
        }
        private void NovoRegistroEm_t_PRODUTO_LOJA(string tabela, string campo, string valor)
        {
            Assert.Equal("t_PRODUTO_LOJA", tabela);
            switch (campo)
            {
                case "fabricante":
                    tprodutoLoja.Fabricante = valor;
                    break;
                case "produto":
                    tprodutoLoja.Produto = valor;
                    break;
                case "loja":
                    tprodutoLoja.Loja = valor;
                    break;
                case "preco_lista":
                    if (!decimal.TryParse(valor, out decimal preco_lista))
                        Assert.Equal("", $"{valor} não é um decimal");
                    tprodutoLoja.Preco_Lista = preco_lista;
                    break;
                case "vendavel":
                    tprodutoLoja.Vendavel = valor;
                    break;
                case "excluido_status":
                    if (!short.TryParse(valor, out short saida))
                        Assert.Equal("", $"{valor} não é número");
                    tprodutoLoja.Excluido_status = saida;
                    break;
                case "qtde_max_venda":
                    if (!short.TryParse(valor, out short qtdeMax))
                        Assert.Equal("", $"{valor} não é número");
                    tprodutoLoja.Qtde_Max_Venda = qtdeMax;
                    break;

                default:
                    Assert.Equal("", $"{campo} desconhecido");
                    break;
            }
        }
        #endregion
    }
}

