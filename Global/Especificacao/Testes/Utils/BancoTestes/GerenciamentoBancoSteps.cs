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
using System.Linq;
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
        }

        private void LimparTabelaDbSet<TipoDados>(Microsoft.EntityFrameworkCore.DbSet<TipoDados> dbSet)
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
        public void ThenTabelaRegistroComCampoVerificarCampo(string valor_cnpj_cpf, string campo, string valor_desejado)
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

    }
}

