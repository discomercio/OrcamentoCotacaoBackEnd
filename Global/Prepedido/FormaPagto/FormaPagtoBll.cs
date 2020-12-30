using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using InfraBanco.Constantes;
using InfraBanco;
using Prepedido.Dados.FormaPagto;

namespace Prepedido.FormaPagto
{
    public class FormaPagtoBll
    {
        private readonly ContextoBdProvider contextoProvider;

        public FormaPagtoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<FormaPagtoDados> ObterFormaPagto(string apelido, string tipo_pessoa)
        {
            FormaPagtoDados formaPagto = new FormaPagtoDados();

            //o apelido pode ser null
            apelido = (apelido ?? "");

            //implementar as buscas
            formaPagto.ListaAvista = (await ObterFormaPagtoAVista(apelido, tipo_pessoa)).ToList();
            if (tipo_pessoa == Constantes.ID_PJ)
                formaPagto.ListaParcUnica = (await ObterFormaPagtoParcUnica(apelido, tipo_pessoa)).ToList();
            formaPagto.ParcCartaoInternet = await ObterFlagParcCartaoInternet(apelido, tipo_pessoa);
            formaPagto.ParcCartaoMaquineta = await ObterFlagParcCartaoMaquineta(apelido, tipo_pessoa);
            formaPagto.ListaParcComEntrada = (await ObterFormaPagtoParcComEntrada(apelido, tipo_pessoa)).ToList();
            formaPagto.ListaParcComEntPrestacao = (await ObterFormaPagtoParcComEntPrestacao(apelido, tipo_pessoa)).ToList();

            return formaPagto;
        }

        private async Task<IEnumerable<AvistaDados>> ObterFormaPagtoAVista(string apelido, string tipo_pessoa)
        {
            var db = contextoProvider.GetContextoLeitura();

            var formaPagtoTask = from c in db.TformaPagtos
                                 where c.Hab_a_vista == 1 &&
                                       !(from d in db.torcamentistaEIndicadorRestricaoFormaPagtos
                                         where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                               d.Id_orcamentista_e_indicador == Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
                                               d.Tipo_cliente == tipo_pessoa &&
                                               d.St_restricao_ativa != 0
                                         select d.Id_forma_pagto).Contains(c.Id)
                                 orderby c.Ordenacao
                                 select new AvistaDados
                                 {
                                     Id = c.Id,
                                     Descricao = c.Descricao,
                                     Ordenacao = c.Ordenacao
                                 };

            var formaPagto = await formaPagtoTask.ToListAsync();

            return formaPagto;
        }

        private async Task<IEnumerable<ParcUnicaDados>> ObterFormaPagtoParcUnica(string apelido, string tipo_pessoa)
        {
            var db = contextoProvider.GetContextoLeitura();

            var formaPagtoTask = from c in db.TformaPagtos
                                 where c.Hab_parcela_unica == 1 &&
                                       !(from d in db.torcamentistaEIndicadorRestricaoFormaPagtos
                                         where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                               d.Id_orcamentista_e_indicador == Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
                                               d.Tipo_cliente == tipo_pessoa &&
                                               d.St_restricao_ativa != 0
                                         select d.Id_forma_pagto).Contains(c.Id)
                                 orderby c.Ordenacao
                                 select new ParcUnicaDados
                                 {
                                     Id = c.Id,
                                     Descricao = c.Descricao,
                                     Ordenacao = c.Ordenacao
                                 };

            var formaPagto = await formaPagtoTask.ToListAsync();

            return formaPagto;
        }

        private async Task<bool> ObterFlagParcCartaoInternet(string apelido, string tipo_pessoa)
        {
            bool retorno = false;

            var db = contextoProvider.GetContextoLeitura();

            var flagTask = from c in db.torcamentistaEIndicadorRestricaoFormaPagtos
                           where c.Id_orcamentista_e_indicador == apelido &&
                                 c.Id_forma_pagto == (short)Constantes.FormaPagto.ID_FORMA_PAGTO_CARTAO &&
                                 c.Tipo_cliente == tipo_pessoa &&
                                 c.St_restricao_ativa != 0
                           select c;

            var flag = await flagTask.ToListAsync();

            if (!flag.Any())
                retorno = true;

            return retorno;
        }

        private async Task<bool> ObterFlagParcCartaoMaquineta(string apelido, string tipo_pessoa)
        {
            bool retorno = false;

            var db = contextoProvider.GetContextoLeitura();

            var flagTask = from c in db.torcamentistaEIndicadorRestricaoFormaPagtos
                           where c.Id_orcamentista_e_indicador == apelido &&
                                 c.Id_forma_pagto == (short)Constantes.FormaPagto.ID_FORMA_PAGTO_CARTAO_MAQUINETA &&
                                 c.Tipo_cliente == tipo_pessoa &&
                                 c.St_restricao_ativa != 0
                           select c;

            var flag = await flagTask.ToListAsync();

            if (!flag.Any())
                retorno = true;

            return retorno;
        }

        private async Task<IEnumerable<ParcComEntradaDados>> ObterFormaPagtoParcComEntrada(string apelido, string tipo_pessoa)
        {
            var db = contextoProvider.GetContextoLeitura();

            var formaPagtoTask = from c in db.TformaPagtos
                                 where c.Hab_entrada == 1 &&
                                       !(from d in db.torcamentistaEIndicadorRestricaoFormaPagtos
                                         where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                               d.Id_orcamentista_e_indicador ==
                                               Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
                                               d.Tipo_cliente == tipo_pessoa &&
                                               d.St_restricao_ativa != 0
                                         select d.Id_forma_pagto).Contains(c.Id)
                                 orderby c.Ordenacao
                                 select new ParcComEntradaDados
                                 {
                                     Id = c.Id,
                                     Descricao = c.Descricao,
                                     Ordenacao = c.Ordenacao
                                 };

            var formaPagto = await formaPagtoTask.ToListAsync();

            return formaPagto;
        }

        private async Task<IEnumerable<ParcComEntPrestacaoDados>> ObterFormaPagtoParcComEntPrestacao(string apelido, string tipo_pessoa)
        {
            var db = contextoProvider.GetContextoLeitura();

            var formaPagtoTask = from c in db.TformaPagtos
                                 where c.Hab_prestacao == 1 &&
                                       !(from d in db.torcamentistaEIndicadorRestricaoFormaPagtos
                                         where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                               d.Id_orcamentista_e_indicador == Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
                                               d.Tipo_cliente == tipo_pessoa &&
                                               d.St_restricao_ativa != 0
                                         select d.Id_forma_pagto).Contains(c.Id)
                                 orderby c.Ordenacao
                                 select new ParcComEntPrestacaoDados
                                 {
                                     Id = c.Id,
                                     Descricao = c.Descricao,
                                     Ordenacao = c.Ordenacao
                                 };

            var formaPagto = await formaPagtoTask.ToListAsync();

            return formaPagto;
        }

        private async Task<IEnumerable<ParcSemEntradaPrimPrestDados>> ObterFormaPagtoParcSemEntPrimPrest(string apelido, string tipo_pessoa)
        {
            var db = contextoProvider.GetContextoLeitura();

            var formaPagtoTask = from c in db.TformaPagtos
                                 where c.Hab_prestacao == 1 &&
                                       !(from d in db.torcamentistaEIndicadorRestricaoFormaPagtos
                                         where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                               d.Id_orcamentista_e_indicador == Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
                                               d.Tipo_cliente == tipo_pessoa &&
                                               d.St_restricao_ativa != 0
                                         select d.Id_forma_pagto).Contains(c.Id)
                                 orderby c.Ordenacao
                                 select new ParcSemEntradaPrimPrestDados
                                 {
                                     Id = c.Id,
                                     Descricao = c.Descricao,
                                     Ordenacao = c.Ordenacao
                                 };

            var formaPagto = await formaPagtoTask.ToListAsync();

            return formaPagto;
        }

        private async Task<IEnumerable<ParcSemEntPrestacaoDados>> ObterFormaPagtoParcSemEntPrestacao(string apelido, string tipo_pessoa)
        {
            var db = contextoProvider.GetContextoLeitura();

            var formaPagtoTask = from c in db.TformaPagtos
                                 where c.Hab_prestacao == 1 &&
                                       !(from d in db.torcamentistaEIndicadorRestricaoFormaPagtos
                                         where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                               d.Id_orcamentista_e_indicador == Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
                                               d.Tipo_cliente == tipo_pessoa &&
                                               d.St_restricao_ativa != 0
                                         select d.Id_forma_pagto).Contains(c.Id)
                                 orderby c.Ordenacao
                                 select new ParcSemEntPrestacaoDados
                                 {
                                     Id = c.Id,
                                     Descricao = c.Descricao,
                                     Ordenacao = c.Ordenacao
                                 };

            var formaPagto = await formaPagtoTask.ToListAsync();

            return formaPagto;
        }

        //Foi solicitado que a qtde de parcelas máxima permitida 
        //seria baseada na qtde parcelas permitida pelo cartão Visa(PRAZO_LOJA)
        public async Task<int> BuscarQtdeParcCartaoVisa()
        {
            var db = contextoProvider.GetContextoLeitura();

            var qtdeTask = from c in db.TprazoPagtoVisanets
                           where c.Tipo == Constantes.COD_VISANET_PRAZO_PAGTO_LOJA
                           select c.Qtde_parcelas;
            int qtde = Convert.ToInt32(await qtdeTask.FirstOrDefaultAsync());

            return qtde;
        }
    }
}
