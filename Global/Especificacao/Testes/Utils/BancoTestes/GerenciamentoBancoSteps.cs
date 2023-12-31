﻿using Especificacao.Testes.Utils.InjecaoDependencia;
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
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            switch (tabela)
            {
                case "t_PRODUTO_LOJA":
                    LimparTabelaDbSet<TprodutoLoja>(db.TprodutoLoja);
                    break;
                case "t_PERCENTUAL_CUSTO_FINANCEIRO_FORNECEDOR":
                    LimparTabelaDbSet<TpercentualCustoFinanceiroFornecedor>(db.TpercentualCustoFinanceiroFornecedors);
                    break;
                case "t_PRAZO_PAGTO_VISANET":
                    LimparTabelaDbSet<TprazoPagtoVisanet>(db.TprazoPagtoVisanet);
                    break;
                case "t_CLIENTE":
                    LimparTabelaDbSet<Tcliente>(db.Tcliente);
                    break;
                case "t_PEDIDO":
                    LimparTabelaDbSet<Tpedido>(db.Tpedido);
                    LimparTabelaDbSet<TpedidoItem>(db.TpedidoItem);
                    break;
                case "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD":
                    LimparTabelaDbSet<TwmsRegraCdXUfXPessoaXCd>(db.TwmsRegraCdXUfXPessoaXCd);
                    break;
                case "t_NFe_EMITENTE":
                    LimparTabelaDbSet<TnfEmitente>(db.TnfEmitente);
                    break;
                case "t_DESCONTO":
                    LimparTabelaDbSet<Tdesconto>(db.Tdesconto);
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
        [Given(@"Reiniciar banco imediatamente")]
        public void GivenReiniciarBancoImediatamente()
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            //precisa restaurar o banco
            var bd = new Testes.Utils.BancoTestes.InicializarBancoGeral(servicos.GetRequiredService<InfraBanco.ContextoBdProvider>(), servicos.GetRequiredService<InfraBanco.ContextoCepProvider>());
            bd.InicializarForcado();
        }

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
            GivenReiniciarBancoImediatamente();
        }
        #endregion


        [Then(@"Tabela ""t_CLIENTE"" registro com campo ""cnpj_cpf"" = ""(.*)"", verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabela_t_CLIENTE_RegistroComCampo_cnpj_cpf_VerificarCampo(string valor_cnpj_cpf, string campo, string valor_desejado)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_CLIENTE", "cnpj_cpf", valor_cnpj_cpf, campo, valor_desejado, this);

            var db = this.contextoBdProvider.GetContextoLeitura();
            var registro = (from cliente in db.Tcliente where cliente.Cnpj_Cpf == valor_cnpj_cpf select cliente).ToList();
            //só deve ter um registro
            Assert.Single(registro);
            VerificarCampoEmRegistro.VerificarRegistro<Tcliente>(campo, valor_desejado, registro[0]);
        }




        public void TabelaT_PEDIDORegistroVerificarCampo(List<string> listaPedidos, string campo, string valor_desejado)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO", "pedido", String.Join("; ", listaPedidos), campo, valor_desejado, this);

            var db = this.contextoBdProvider.GetContextoLeitura();
            var registros = (from pedido in db.Tpedido where listaPedidos.Contains(pedido.Pedido) select pedido).ToList();
            //deve ter um ou mais registros
            Assert.True(registros.Any());


            foreach (var registro in registros)
            {
                VerificarCampoEmRegistro.VerificarRegistro<Tpedido>(campo, valor_desejado, registro);
            }
        }

        public void TabelaT_PEDIDO_ITEMRegistroVerificarCampo(int item, string pedido, string campo, string valor_desejado)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO_ITEM", "pedido", pedido, campo, valor_desejado, this);

            var db = this.contextoBdProvider.GetContextoLeitura();
            var registros = (from registro in db.TpedidoItem
                             where registro.Pedido == pedido &&
                             registro.Sequencia == item
                             select registro).ToList();
            //deve ter um ou mais registros
            Assert.True(registros.Any());

            foreach (var registro in registros)
            {
                VerificarCampoEmRegistro.VerificarRegistro<TpedidoItem>(campo, valor_desejado, registro);
            }
        }

        public void TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(TpedidoItem item, string tipo_estoque, string campo, string valor, string pedido)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("_ESTOQUE_MOVIMENTO", "pedido", pedido, campo, valor, this);

            var db = this.contextoBdProvider.GetContextoLeitura();
            var registros = (from registro in db.TestoqueMovimento
                             where registro.Pedido == pedido &&
                                   registro.Produto == item.Produto &&
                                   registro.Fabricante == item.Fabricante &&
                                   registro.Estoque == tipo_estoque.ToUpper()
                             select registro).ToList();
            //deve ter um ou mais registros
            Assert.True(registros.Any());

            foreach (var registro in registros)
            {
                VerificarCampoEmRegistro.VerificarRegistro<TestoqueMovimento>(campo, valor, registro);
            }
        }

        public void TabelaT_ESTOQUE_MOVIMENTORegistroDoPedidoVerificarCampo(TpedidoItem item, string tipo_estoque, string campo, int valor, string pedido)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("_ESTOQUE_MOVIMENTO", "pedido", pedido, campo, valor.ToString(), this);

            var db = this.contextoBdProvider.GetContextoLeitura();
            var registros = (from registro in db.TestoqueMovimento
                             where registro.Pedido == pedido &&
                                   registro.Produto == item.Produto &&
                                   registro.Fabricante == item.Fabricante &&
                                   registro.Estoque == tipo_estoque.ToUpper()
                             select registro.Qtde).ToList();
            if (registros == null)
                Assert.Equal(0, valor);

            if (registros != null)
            {
                foreach (var registro in registros)
                {
                    int reg = registro ?? 0;
                    Assert.Equal(reg, valor);
                }
            }
        }

        public void TabelaT_ESTOQUE_ITEMRegistroPaiEProdutoVerificarCampo(TpedidoItem item, string campo, string valor)
        {
            var id_estoque = BuscarIdEstoqueMovimento(item);
            //if (string.IsNullOrEmpty(id_estoque))
            //{
            //    //se tiver sem id_estoque é porque esta sem presença no estoque
            //    Assert.Equal("pedido gerado sem id_estoque", campo);
            //}
            //estoqueItem.Id_estoque == id_estoque &&
            var db = this.contextoBdProvider.GetContextoLeitura();
            var registros = (from estoqueItem in db.TestoqueItem
                             where estoqueItem.Fabricante == item.Fabricante &&
                                   estoqueItem.Produto == item.Produto
                             select estoqueItem);

            Assert.True(registros.Any());

            foreach (var registro in registros)
            {
                VerificarCampoEmRegistro.VerificarRegistro<TestoqueItem>(campo, valor, registro);
            }
        }

        public void TabelaT_ESTOQUE_ITEMVerificarSaldo(int id_nfe_emitente, int saldo, TpedidoItem item)
        {
            var db = this.contextoBdProvider.GetContextoLeitura();
            var lotes = (from ei in db.TestoqueItem
                         join e in db.Testoque on ei.Id_estoque equals e.Id_estoque
                         where e.Id_nfe_emitente == id_nfe_emitente &&
                               ei.Fabricante == item.Fabricante &&
                               ei.Produto == item.Produto
                         orderby e.Data_entrada, ei.Id_estoque
                         select new
                         {
                             Saldo = (short?)(ei.Qtde - ei.Qtde_utilizada)
                         });
            Assert.True(lotes.Any());

            var saldoTotal = lotes.Select(x => x).Sum(x => x.Saldo);

            Assert.Equal(saldo, saldoTotal);
        }

        public void TabelaT_ESTOQUERegistroPaiVerificarCampo(List<TpedidoItem> itens, string campo, string valor)
        {
            foreach (var item in itens)
            {
                var id_estoque = BuscarIdEstoqueMovimento(item);
                if (string.IsNullOrEmpty(id_estoque))
                {
                    Assert.Equal("pedido gerado sem id_estoque", campo);
                }

                var db = this.contextoBdProvider.GetContextoLeitura();
                var registros = (from estoque in db.Testoque
                                 where estoque.Id_estoque == id_estoque
                                 select estoque);

                Assert.True(registros.Any());

                string valor_desejado = "";
                foreach (var registro in registros)
                {
                    switch (valor)
                    {
                        case "data atual":
                            valor_desejado = ProcessarDataAtual();
                            break;

                        default:
                            Assert.Equal("", $"{valor} desconhecido");
                            break;
                    }
                    VerificarCampoEmRegistro.VerificarRegistro<Testoque>(campo, valor_desejado, registro);
                }
            }

        }

        private static string ProcessarDataAtual()
        {
            string valor_desejado;
            if (!ProvedorServicos.UsarSqlServerNosTestesAutomatizados)
                valor_desejado = Newtonsoft.Json.JsonConvert.SerializeObject(DateTime.Now.Date).Replace("\"", "");
            else
                valor_desejado = DateTime.Now.Date.ToString();
            return valor_desejado;
        }

        public void TabelaT_ESTOQUE_LOGPedidoGeradoVerificarCampo(string pedido, string operacao, string produto, string campo, string valor)
        {
            //OP_ESTOQUE_LOG_VENDA
            //OP_ESTOQUE_LOG_VENDA_SEM_PRESENCA
            switch (operacao)
            {
                case "OP_ESTOQUE_LOG_VENDA":
                    operacao = InfraBanco.Constantes.Constantes.OP_ESTOQUE_LOG_VENDA;
                    break;
                case "OP_ESTOQUE_LOG_VENDA_SEM_PRESENCA":
                    operacao = InfraBanco.Constantes.Constantes.OP_ESTOQUE_LOG_VENDA_SEM_PRESENCA;
                    break;
                default:
                    Assert.Equal("Operação", $"{operacao} desconhecido");
                    break;
            }
            var db = contextoBdProvider.GetContextoLeitura();
            var registros = (from estoqueLog in db.TestoqueLogs
                             where estoqueLog.Pedido_estoque_destino == pedido &&
                                   estoqueLog.Produto == produto &&
                                   estoqueLog.Operacao == operacao
                             select estoqueLog).ToList();

            Assert.True(registros.Any());
            string valor_desejado = "";
            foreach (var registro in registros)
            {

                if (valor == "data atual")
                    valor_desejado = ProcessarDataAtual();
                else
                    valor_desejado = valor;

                VerificarCampoEmRegistro.VerificarRegistro<TestoqueLog>(campo, valor_desejado, registro);
            }
        }

        public void TabelaT_LOGPedidoGeradoEOperacaoVerificarCampo(string pedido, string operacao, string campo, string valor)
        {
            //OP_LOG_PEDIDO_NOVO
            //OP_LOG_ORCAMENTO_NOVO
            //OP_LOG_CLIENTE_INCLUSAO
            switch (operacao)
            {
                case "OP_LOG_PEDIDO_NOVO":
                    operacao = InfraBanco.Constantes.Constantes.OP_LOG_PEDIDO_NOVO;
                    break;
                case "OP_LOG_ORCAMENTO_NOVO":
                    operacao = InfraBanco.Constantes.Constantes.OP_LOG_ORCAMENTO_NOVO;
                    break;
                case "OP_LOG_CLIENTE_INCLUSAO":
                    operacao = InfraBanco.Constantes.Constantes.OP_LOG_CLIENTE_INCLUSAO;
                    break;
                default:
                    Assert.Equal("Operação", $"{operacao} desconhecido");
                    break;
            }
            var db = contextoBdProvider.GetContextoLeitura();
            string idCliente = "";
            if (operacao.ToUpper() == InfraBanco.Constantes.Constantes.OP_LOG_CLIENTE_INCLUSAO)
            {
                idCliente = (from c in db.Tpedido
                             where c.Pedido == pedido
                             select c.Id_Cliente).FirstOrDefault();
            }
            var registros = (from log in db.Tlog
                             where (log.Pedido == pedido || log.Id_Cliente == idCliente) &&
                                   log.Operacao == operacao
                             select log).ToList();

            Assert.True(registros.Any());

            if (registros.Count > 1)
                Assert.Equal("Erro", $"t_LOG tem mais de um registro para o pedido ({pedido}) com operação ({operacao}).");

            foreach (var registro in registros)
            {
                switch (campo)
                {
                    case "usuario":
                        Assert.Equal(registro.Usuario.ToUpper(), valor.ToUpper());
                        break;
                    case "loja":
                        Assert.Equal(registro.Loja, valor);
                        break;
                    case "pedido":
                        Assert.Equal(registro.Pedido.ToUpper(), valor.ToUpper());
                        break;
                    case "id_cliente":
                        if (registro.Id_Cliente.Count() != 12)
                            Assert.Equal("", $"{campo} não contém a quantidade correta de carcateres.");
                        Assert.Equal(registro.Id_Cliente, valor);
                        break;
                    case "data":
                        DateTime data_atual = DateTime.Now;
                        Assert.Equal(registro.Data.Date, data_atual.Date);
                        Assert.Equal(registro.Data.Hour, data_atual.Hour);
                        Assert.Equal(registro.Data.Minute, data_atual.Minute);
                        break;
                    case "complemento":
                        if (valor.Contains("\\r")) valor = valor.Replace("\\r", "\r");
                        //if (valor.Contains("\\n")) valor = valor.Replace("\\n", "\n");
                        Assert.Contains(valor.ToLower(), registro.Complemento.ToLower());
                        break;

                    default:
                        Assert.Equal("", $"{campo} desconhecido");
                        break;
                }
            }
        }

        public void TabelaT_PRODUTO_X_WMS_REGRA_CDFabricanteEProdutoVerificarCampo(string fabricante, string produto, string campo, string valor_desejado)
        {
            var db = this.contextoBdProvider.GetContextoLeitura();

            var registros = (from prodRegraCd in db.TprodutoXwmsRegraCd
                             where prodRegraCd.Fabricante == fabricante &&
                                   prodRegraCd.Produto == produto
                             select prodRegraCd).ToList();

            Assert.True(registros.Any());

            if (registros.Count > 1)
                Assert.Equal("Erro", $"PRODUTO_X_WMS_REGRA_CD tem mais de um registro para o fabricante ({fabricante}) e produto ({produto}).");

            foreach (var registro in registros)
            {
                switch (campo)
                {
                    case "id_wms_regra_cd":
                        if (int.TryParse(valor_desejado, out int valor))
                            Assert.Equal(valor, registro.Id_wms_regra_cd);
                        break;
                    default:
                        Assert.Equal("", $"{campo} desconhecido");
                        break;
                }
            }
        }

        public void TabelaT_PEDIDO_ANALISE_ENDERECORegistroCriadoVerificarCampo(string pedido, string campo, string valor)
        {
            var db = contextoBdProvider.GetContextoLeitura();
            var registros = (from c in db.TpedidoAnaliseEnderecos
                             where c.Pedido == pedido
                             select c).ToList();
            Assert.True(registros.Any());

            foreach (var registro in registros)
            {
                VerificarCampoEmRegistro.VerificarRegistro<TpedidoAnaliseEndereco>(campo, valor, registro);
            }

        }

        public void TabelaT_PEDIDO_ANALISE_ENDERECO_CONFRONTACAORegistroCriadoVerificarCampo(string pedido, string campo, string valor)
        {
            var db = contextoBdProvider.GetContextoLeitura();
            var registros = (from c in db.TpedidoAnaliseEnderecos
                             where c.Pedido == pedido
                             select c).ToList();
            Assert.True(registros.Any());

            foreach (var registro in registros)
            {
                var registros2 = (from c in db.TpedidoAnaliseEnderecoConfrontacao
                                  where c.Id_pedido_analise_endereco == registro.Id
                                  select c).ToList();
                Assert.True(registros2.Any());

                foreach (var registro2 in registros2)
                {
                    VerificarCampoEmRegistro.VerificarRegistro<TpedidoAnaliseEnderecoConfrontacao>(campo, valor, registro2);
                }
            }
        }

        [Given(@"Tabela ""t_OPERACAO"" apagar registro com campo ""id"" = ""(.*)""")]
        public void GivenTabelaT_operacao_ApagarRegistroComCampo(string valorBusca)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaApagarRegistroComCampo("t_OPERACAO", "id", valorBusca, this);

            int valorInt = -1;
#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (valorBusca)
#pragma warning restore IDE0066 // Convert switch statement to expression
            {
                case "OP_LJA_CADASTRA_NOVO_PEDIDO":
                    valorInt = Constantes.OP_LJA_CADASTRA_NOVO_PEDIDO;
                    break;
                case "OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO":
                    valorInt = Constantes.OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO;
                    break;
                case "OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO":
                    valorInt = Constantes.OP_LJA_EXIBIR_CAMPO_RT_AO_CADASTRAR_NOVO_PEDIDO;
                    break;
                default:
                    throw new ArgumentException($"GivenTabelaApagarRegistroComCampo desconhecido: {valorBusca}");
            }

            using var db = this.contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            var registro = (from operacao in db.Toperacao where operacao.Id == valorInt select operacao).First();
            db.Toperacao.Remove(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_USUARIO"" apagar registro com campo usuario = ""(.*)""")]
        public void GivenTabelaT_USUARIOApagarRegistroComCampo(string valor)
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

            using var db = this.contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            var registro = (from usuario in db.Tusuario where usuario.Usuario == valor select usuario).First();
            db.Tusuario.Remove(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""T_CODIGO_DESCRICAO"" registro com campo grupo ""(.*)"" e codigo ""(.*)"", alterar campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaT_CODIGO_DESCRICAORegistroComCampoGrupoECodigoParametro__Campo_Flag(string grupo, string codigo, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaAlterarRegistroComCampo("T_CODIGO_DESCRICAO", "apelido", valor, this);
            using var db = this.contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            var registro = (from r in db.TcodigoDescricao
                            where r.Grupo == grupo &&
                                  r.Codigo == codigo
                            select r).FirstOrDefault();

            Assert.NotNull(registro);

            if (!WhenInformoCampo.InformarCampo(campo, valor, registro))
                Assert.Equal("campo desconhecido", campo);

            db.Update(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }


        [Given(@"Tabela ""t_ORCAMENTISTA_E_INDICADOR"" registro apelido = ""(.*)"", alterar campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaT_ORCAMENTISTA_E_INDICADORRegistroCampoAlterarCampo(string apelido, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaAlterarRegistroComCampo("t_ORCAMENTISTA_E_INDICADOR", "apelido", valor, this);

            using var db = this.contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            var registro = (from orcamentista in db.TorcamentistaEindicador
                            where orcamentista.Apelido.ToUpper() == apelido.ToUpper()
                            select orcamentista).FirstOrDefault();

            Assert.NotNull(registro);

            if (!WhenInformoCampo.InformarCampo(campo, valor, registro))
                Assert.Equal("campo desconhecido", campo);

            db.Update(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela t_CLIENTE registro com cpf_cnpj = ""(.*)"" alterar campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaT_CLIENTERegistroComCpf_CnpjAlterarCampo(string valor_cnpj_cpf, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaAlterarRegistroComCampo("t_CLIENTE", "cpf_cnpj", valor, this);

            using var db = this.contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            var registro = (from cliente in db.Tcliente
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

            using var db = this.contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            var registro = (from loja in db.Tloja
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

            using var db = this.contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
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

        [Given(@"Tabela ""t_PRODUTO_X_WMS_REGRA_CD"" fabricante = ""(.*)"" e produto = ""(.*)"", alterar registro do campo ""(.*)"" = ""(.*)""")]
        public void TabelaT_PRODUTO_X_WMS_REGRA_CDFabricanteAlterarRegistroDoCampo(string fabricante, string produto, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaAlterarRegistroComCampo("t_PRODUTO_X_WMS_REGRA_CD", campo, valor, this);
            using var db = this.contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            var registro = (from prodRegraCd in db.TprodutoXwmsRegraCds
                            where prodRegraCd.Fabricante == fabricante &&
                                  prodRegraCd.Produto == produto
                            select prodRegraCd).FirstOrDefault();

            Assert.NotNull(registro);

            if (!WhenInformoCampo.InformarCampo(campo, valor, registro))
                Assert.Equal("campo desconhecido", campo);

            db.Update(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_PRODUTO_X_WMS_REGRA_CD"" duplicar regra para fabricante = ""(.*)"" e produto = ""(.*)"" com id_wms_regra_cd = ""(.*)""")]
        public void GivenTabelaT_PRODUTO_X_WMS_REGRA_CDDuplicarRegraParaFabricanteEProduto(string fabricante, string produto, int id_wms_regra_cd)
        {
            //Não podemos duplicar um produto, porque fabricante e produto são chaves
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.GravarRegistroEm("t_PRODUTO_X_WMS_REGRA_CD", this);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);

            var registro = (from prodRegraCd in db.TprodutoXwmsRegraCds
                            where prodRegraCd.Fabricante == fabricante &&
                                  prodRegraCd.Produto == produto
                            select prodRegraCd).FirstOrDefault();

            TprodutoXwmsRegraCd duplicado = new TprodutoXwmsRegraCd()
            {
                Fabricante = registro.Fabricante,
                Produto = registro.Produto,
                Id_wms_regra_cd = id_wms_regra_cd
            };

            db.Add(duplicado);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_PRODUTO_X_WMS_REGRA_CD"" apagar registro do fabricante = ""(.*)"" e produto = ""(.*)""")]
        public void GivenTabelaT_PRODUTO_X_WMS_REGRA_CDApagarRegistroDoFabricante(string fabricante, string produto)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaApagarRegistroComCampo("t_PRODUTO_X_WMS_REGRA_CD", "fabricante e produto", "Fabricante:(" + fabricante + ") e produto:(" + produto + ")", this);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);

            var registro = (from prodRegraCd in db.TprodutoXwmsRegraCds
                            where prodRegraCd.Fabricante == fabricante &&
                                  prodRegraCd.Produto == produto
                            select prodRegraCd).FirstOrDefault();

            db.Remove(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_WMS_REGRA_CD"" apagar registro do fabricante = ""(.*)"" e produto = ""(.*)""")]
        public void GivenTabelaT_WMS_REGRA_CDApagarRegistroDoFabricante(string fabricante, string produto)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaApagarRegistroComCampo("t_WMS_REGRA_CD", "fabricante e produto", "Fabricante:(" + fabricante + ") e produto:(" + produto + ")", this);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);

            var t_produtoXwmsRegraCds = (from prodRegraCd in db.TprodutoXwmsRegraCds
                                         where prodRegraCd.Fabricante == fabricante &&
                                               prodRegraCd.Produto == produto
                                         select prodRegraCd).ToList();

            Assert.Single(t_produtoXwmsRegraCds);

            var registros = (from regraCd in db.TwmsRegraCd
                             where regraCd.Id == t_produtoXwmsRegraCds[0].Id_wms_regra_cd
                             select regraCd).ToList();

            foreach (var r in registros) db.Remove(r);

            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_WMS_REGRA_CD_X_UF"" apagar registro do id_wms_regra_cd = ""(.*)"" da UF = ""(.*)""")]
        public void GivenTabelaT_WMS_REGRA_CD_X_UFApagarRegistroDoId_Wms_Regra_CdDaUF(int id_wms_regra_cd, string uf)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaApagarRegistroComCampo("t_WMS_REGRA_CD_X_UF", "id_wms_regra_cd e UF", "id_wms_regra_cd:(" + id_wms_regra_cd + ") e UF:(" + uf + ")", this);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);

            var registros = (from regraCdUF in db.TwmsRegraCdXUf
                             where regraCdUF.Id_wms_regra_cd == id_wms_regra_cd &&
                                   regraCdUF.Uf == uf
                             select regraCdUF).ToList();
            foreach (var r in registros) db.Remove(r);

            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_WMS_REGRA_CD_X_UF"" duplicar registro do id_wms_regra_cd = ""(.*)"" da UF = ""(.*)"" se não UsarSqlServerNosTestesAutomatizados")]
        public void GivenTabelaT_WMS_REGRA_CD_X_UFDuplicarRegistroDoId_Wms_Regra_CdDaUF(int id_wms_regra_cd, string uf)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.GravarRegistroEm("t_WMS_REGRA_CD_X_UF", this);

            //nao fazemos se estiver rodando contra o sql server real
            if (Testes.Utils.InjecaoDependencia.ProvedorServicos.UsarSqlServerNosTestesAutomatizados)
                return;

            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);

            var registro = (from regraCdUF in db.TwmsRegraCdXUf
                            where regraCdUF.Id_wms_regra_cd == id_wms_regra_cd &&
                                  regraCdUF.Uf == uf
                            select regraCdUF).FirstOrDefault();

            Assert.NotNull(registro);

            TwmsRegraCdXUf regraCdXUf = new TwmsRegraCdXUf()
            {
                Id = 163,
                Id_wms_regra_cd = id_wms_regra_cd,
                St_inativo = 0,
                Uf = uf
            };

            db.Add(regraCdXUf);
            db.SaveChanges();
            db.transacao.Commit();

        }

        [Given(@"Tabela ""t_WMS_REGRA_CD_X_UF_X_PESSOA"" apagar registro id = ""(.*)"" e tipo de pessoa = ""(.*)""")]
        public void GivenTabelaT_WMS_REGRA_CD_X_UF_X_PESSOAApagarRegistroId_Wms_Regra_Cd_X_UfPF(int id, string tipo_pessoa)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaApagarRegistroComCampo("t_WMS_REGRA_CD_X_UF_X_PESSOA", "id e tipo pessoa", "id_wms_regra_cd_x_uf:(" + id + ") e tipo pessoa:(" + tipo_pessoa + ")", this);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);

            var registros = (from regraCdUFPessoa in db.TwmsRegraCdXUfPessoa
                             where regraCdUFPessoa.Id_wms_regra_cd_x_uf == id &&
                                   regraCdUFPessoa.Tipo_pessoa == tipo_pessoa
                             select regraCdUFPessoa).ToList();

            foreach (var r in registros) db.Remove(r);

            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_WMS_REGRA_CD_X_UF_X_PESSOA"" duplicar registro id_wms_regra_cd_x_uf = ""(.*)"" e tipo de pessoa = ""(.*)"" com id = ""(.*)"" se não UsarSqlServerNosTestesAutomatizados")]
        public void GivenTabelaT_WMS_REGRA_CD_X_UF_X_PESSOADuplicarRegistroIdETipoDePessoa(int Id_wms_regra_cd_x_uf, string tipo_pessoa, int id)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.GravarRegistroEm("t_WMS_REGRA_CD_X_UF", this);

            //nao fazemos se estiver rodando contra o sql erver real
            if (Testes.Utils.InjecaoDependencia.ProvedorServicos.UsarSqlServerNosTestesAutomatizados)
                return;

            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);

            var registro = (from regraCdUFPessoa in db.TwmsRegraCdXUfPessoa
                            where regraCdUFPessoa.Id_wms_regra_cd_x_uf == Id_wms_regra_cd_x_uf &&
                                  regraCdUFPessoa.Tipo_pessoa == tipo_pessoa
                            select regraCdUFPessoa).FirstOrDefault();

            Assert.NotNull(registro);

            TwmsRegraCdXUfPessoa regraCdXUfXPessoa = new TwmsRegraCdXUfPessoa()
            {
                Id = id,
                Id_wms_regra_cd_x_uf = Id_wms_regra_cd_x_uf,
                Spe_id_nfe_emitente = registro.Spe_id_nfe_emitente,
                St_inativo = registro.St_inativo,
                Tipo_pessoa = tipo_pessoa
            };

            db.Add(regraCdXUfXPessoa);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_WMS_REGRA_CD_X_UF_X_PESSOA"" registro id_wms_regra_cd_x_uf = ""(.*)"" e tipo de pessoa = ""(.*)"", alterar campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaT_WMS_REGRA_CD_X_UF_X_PESSOARegistroId_Wms_Regra_Cd_X_UfETipoDePessoaAlterarCampo(int id_wms_regra_cd_x_uf, string tipo_pessoa, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaAlterarRegistroComCampo("t_WMS_REGRA_CD_X_UF_X_PESSOA", campo, valor, this);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);

            var registro = (from regraCdUFPessoa in db.TwmsRegraCdXUfPessoa
                            where regraCdUFPessoa.Id_wms_regra_cd_x_uf == id_wms_regra_cd_x_uf &&
                                  regraCdUFPessoa.Tipo_pessoa == tipo_pessoa
                            select regraCdUFPessoa).FirstOrDefault();

            Assert.NotNull(registro);

            if (!WhenInformoCampo.InformarCampo(campo, valor, registro))
                Assert.Equal("campo desconhecido", campo);

            db.Update(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_NFe_EMITENTE"" registro tipo de pessoa = ""(.*)"" e id_wms_regra_cd_x_uf = ""(.*)"", alterar campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaT_NFe_EMITENTERegistroTipoDePessoaAlterarCampo(string tipo_pessoa, int id_wms_regra_cd_x_uf, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaAlterarRegistroComCampo("t_NFe_EMITENTE", campo, valor, this);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            //TwmsRegraCdXUfPessoas.id_wms_regra_cd_x_uf = 134
            //TwmsRegraCdXUfPessoas.Tipo_pessoa = PF e PR para prepedido
            var registro = (from nfeEmitente in db.TnfEmitente
                            join regraCdUFPessoa in db.TwmsRegraCdXUfPessoa on nfeEmitente.Id equals regraCdUFPessoa.Spe_id_nfe_emitente
                            where regraCdUFPessoa.Tipo_pessoa == tipo_pessoa &&
                                  regraCdUFPessoa.Id_wms_regra_cd_x_uf == id_wms_regra_cd_x_uf
                            select nfeEmitente).FirstOrDefault();

            Assert.NotNull(registro);

            if (campo == "st_ativo") campo = "St_Ativo";

            if (!WhenInformoCampo.InformarCampo(campo, valor, registro))
                Assert.Equal("campo desconhecido", campo);

            db.Update(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        [Given(@"Tabela ""t_NFe_EMITENTE"" verificar registro tipo de pessoa = ""(.*)"" e id_wms_regra_cd_x_uf = ""(.*)"",campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaVerificarRegistroTipoDePessoaCampo(string tipo_pessoa, int id_wms_regra_cd_x_uf, string campo, int valor)
        {
            var db = contextoBdProvider.GetContextoLeitura();
            var registro = (from nfeEmitente in db.TnfEmitente
                            join regraCdUFPessoa in db.TwmsRegraCdXUfPessoa on nfeEmitente.Id equals regraCdUFPessoa.Spe_id_nfe_emitente
                            where regraCdUFPessoa.Tipo_pessoa == tipo_pessoa &&
                                  regraCdUFPessoa.Id_wms_regra_cd_x_uf == id_wms_regra_cd_x_uf
                            select nfeEmitente).FirstOrDefault();

            Assert.NotNull(registro);

            switch (campo)
            {
                case "st_ativo":
                    Assert.Equal(registro.St_Ativo, valor);
                    break;
                default:
                    Assert.Equal("", $"{campo} desconhecido");
                    break;
            }
        }

        [Given(@"Tabela ""t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD"" alterar registro id_wms_regra_cd_x_uf_x_pessoa = ""(.*)"" e id_nfe_emitente = ""(.*)"", campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaAlterarRegistroId_Wms_Regra_Cd_X_UfSt_Inativo(int id_wms_regra_cd_x_uf_x_pessoa, int id_nfe_emitente, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaAlterarRegistroComCampo("t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD", campo, valor, this);
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);

            var registro = (from regra in db.TwmsRegraCdXUfXPessoaXCd
                            where regra.Id_nfe_emitente == id_nfe_emitente &&
                                  regra.Id_wms_regra_cd_x_uf_x_pessoa == id_wms_regra_cd_x_uf_x_pessoa
                            select regra).ToList();

            Assert.True(registro.Any());
            if (registro.Count > 1)
                Assert.Equal("Erro", $"t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD tem mais de um registro para o id_wms_regra_cd_x_uf ({id_nfe_emitente}).");

            foreach (var r in registro)
            {
                if (campo == "st_inativo") campo = "St_inativo";

                if (!WhenInformoCampo.InformarCampo(campo, valor, r))
                    Assert.Equal("campo desconhecido", campo);

                db.Update(r);
            }



            db.SaveChanges();
            db.transacao.Commit();

        }


        [Given(@"Tabela ""t_PRODUTO"" com fabricante = ""(.*)"" e produto = ""(.*)"" alterar campo ""(.*)"" = ""(.*)""")]
        public void GivenTabelaComFabricanteEProdutoAlterarCampo(string fabricante, string produto, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaAlterarRegistroComCampo("t_PRODUTO", "loja", valor, this);

            using var db = this.contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            var registro = (from tproduto in db.Tproduto
                            where tproduto.Fabricante == fabricante &&
                            tproduto.Produto == produto
                            select tproduto).FirstOrDefault();

            Assert.NotNull(registro);

            if (!WhenInformoCampo.InformarCampo(campo, valor, registro))
                Assert.Equal("campo desconhecido", campo);

            db.Update(registro);
            db.SaveChanges();
            db.transacao.Commit();
        }

        public string? BuscarIdEstoqueMovimento(TpedidoItem pedidoItem)
        {
            var db = contextoBdProvider.GetContextoLeitura();
            var idEstoque = (from estoque in db.TestoqueMovimento
                             where estoque.Pedido == pedidoItem.Pedido &&
                                   estoque.Produto == pedidoItem.Produto &&
                                   estoque.Fabricante == pedidoItem.Fabricante
                             select estoque.Id_Estoque).FirstOrDefault();

            return idEstoque;
        }

        public List<TpedidoItem> BuscarItensPedido(string pedido)
        {
            var db = contextoBdProvider.GetContextoLeitura();
            var itensPedido = (from itens in db.TpedidoItem
                               where itens.Pedido == pedido
                               select itens).ToList();

            return itensPedido;
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
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            db.TprazoPagtoVisanet.Add(tprazoPagtoVisanet);
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
                    tdesconto.Data = DateTime.Now;
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
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            db.TprazoPagtoVisanet.Add(tprazoPagtoVisanet);
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
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            db.TprodutoLoja.Add(tprodutoLoja);
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
        [Given(@"Validação feita em outro arquivo")]
        public void GivenValidacaoFeitaEmOutroArquivo()
        {
            //não fazemos nada mesmo, é só documentação
        }

        public void VerificarQtdePedidosSalvos(int qtde)
        {
            var db = contextoBdProvider.GetContextoLeitura();
            var registro = (from c in db.Tpedido
                            select c.Pedido).Count();

            Assert.Equal(qtde, registro);
        }

        public void TabelaT_PEDIDO_ANALISE_ENDERECOVerificarQtdeDeItensSalvos(int qtde)
        {
            var db = contextoBdProvider.GetContextoLeitura();
            var registro = (from c in db.TpedidoAnaliseEnderecos
                            select c.Id).Count();

            Assert.Equal(qtde, registro);
        }
        public void TabelaT_PEDIDO_ANALISE_ENDERECO_CONFRONTACAOVerificarQtdeDeItensSalvos(int qtde)
        {
            var db = contextoBdProvider.GetContextoLeitura();
            var registro = (from c in db.TpedidoAnaliseEnderecoConfrontacao
                            select c.Id_pedido_analise_endereco).Count();

            Assert.Equal(qtde, registro);
        }

    }
}

