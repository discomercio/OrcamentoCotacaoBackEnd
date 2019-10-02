using PrepedidoBusiness.Dto.FormaPagto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using InfraBanco.Constantes;

namespace PrepedidoBusiness.Bll
{
    public class FormaPagtoBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public FormaPagtoBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<FormaPagtoDto> ObterFormaPagto(string apelido, string tipo_pessoa)
        {
            FormaPagtoDto formaPagto = new FormaPagtoDto();

            //implementar as buscas
            formaPagto.ListaAvista = (await ObterFormaPagtoAVista(apelido, tipo_pessoa)).ToList();
            formaPagto.ParcCartaoInternet = await ObterFlagParcCartaoInternet(apelido, tipo_pessoa);
            formaPagto.ParcCartaoMaquineta = await ObterFlagParcCartaoMaquineta(apelido, tipo_pessoa);
            formaPagto.ListaParcComEntrada = (await ObterFormaPagtoParcComEntrada(apelido, tipo_pessoa)).ToList();
            formaPagto.ListaParcComEntPrestacao = (await ObterFormaPagtoParcComEntPrestacao(apelido, tipo_pessoa)).ToList();
            formaPagto.ListaParcSemEntPrimPrest = (await ObterFormaPagtoParcSemEntPrimPrest(apelido, tipo_pessoa)).ToList();
            formaPagto.ListaParcSemEntPrestacao = (await ObterFormaPagtoParcSemEntPrestacao(apelido, tipo_pessoa)).ToList();  


            return formaPagto;
        }

        private async Task<IEnumerable<AvistaDto>> ObterFormaPagtoAVista(string apelido, string tipo_pessoa)
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
                                 select new AvistaDto
                                 {
                                     Id = c.Id,
                                     Descricao = c.Descricao,
                                     Ordenacao = c.Ordenacao
                                 };

            var formaPagto = await formaPagtoTask.ToListAsync();

            return formaPagto;
        }

        private async Task<IEnumerable<ParcUnicaDto>> ObterFormaPagtoParcUnica(string apelido, string tipo_pessoa)
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
                                 select new ParcUnicaDto
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
                                 c.Id_forma_pagto == short.Parse(Constantes.ID_FORMA_PAGTO_CARTAO) &&
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
                                 c.Id_forma_pagto == short.Parse(Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA) &&
                                 c.Tipo_cliente == tipo_pessoa &&
                                 c.St_restricao_ativa != 0
                           select c;

            var flag = await flagTask.ToListAsync();

            if (!flag.Any())
                retorno = true;

            return retorno;
        }

        private async Task<IEnumerable<ParcComEntradaDto>> ObterFormaPagtoParcComEntrada(string apelido, string tipo_pessoa)
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
                                 select new ParcComEntradaDto
                                 {
                                     Id = c.Id,
                                     Descricao = c.Descricao,
                                     Ordenacao = c.Ordenacao
                                 };

            var formaPagto = await formaPagtoTask.ToListAsync();

            return formaPagto;
        }

        private async Task<IEnumerable<ParcComEntPrestacaoDto>> ObterFormaPagtoParcComEntPrestacao(string apelido, string tipo_pessoa)
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
                                 select new ParcComEntPrestacaoDto
                                 {
                                     Id = c.Id,
                                     Descricao = c.Descricao,
                                     Ordenacao = c.Ordenacao
                                 };

            var formaPagto = await formaPagtoTask.ToListAsync();

            return formaPagto;
        }

        private async Task<IEnumerable<ParcSemEntradaPrimPrestDto>> ObterFormaPagtoParcSemEntPrimPrest(string apelido, string tipo_pessoa)
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
                                 select new ParcSemEntradaPrimPrestDto
                                 {
                                     Id = c.Id,
                                     Descricao = c.Descricao,
                                     Ordenacao = c.Ordenacao
                                 };

            var formaPagto = await formaPagtoTask.ToListAsync();

            return formaPagto;
        }

        private async Task<IEnumerable<ParcSemEntPrestacao>> ObterFormaPagtoParcSemEntPrestacao(string apelido, string tipo_pessoa)
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
                                 select new ParcSemEntPrestacao
                                 {
                                     Id = c.Id,
                                     Descricao = c.Descricao,
                                     Ordenacao = c.Ordenacao
                                 };

            var formaPagto = await formaPagtoTask.ToListAsync();

            return formaPagto;
        }
    }
}
