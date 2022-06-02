using Loja.Bll.Dto.FormaPagtoDto;
using Loja.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Bll.FormaPagtoBll
{
    public class FormaPagtoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public FormaPagtoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<FormaPagtoDto> ObterFormaPagto(string apelido, string tipo_pessoa, string loja, int comIndicador)
        {
            FormaPagtoDto formaPagto = new FormaPagtoDto();

            //implementar as buscas
            formaPagto.ListaAvista = (await ObterFormaPagtoAVista(apelido, tipo_pessoa, loja, comIndicador)).ToList();
            if (tipo_pessoa == Constantes.Constantes.ID_PJ)
                formaPagto.ListaParcUnica = (await ObterFormaPagtoParcUnica(apelido, tipo_pessoa)).ToList();
            formaPagto.ParcCartaoInternet = await ObterFlagParcCartaoInternet(apelido, tipo_pessoa);
            formaPagto.ParcCartaoMaquineta = await ObterFlagParcCartaoMaquineta(apelido, tipo_pessoa);
            formaPagto.ListaParcComEntrada = (await ObterFormaPagtoParcComEntrada(apelido, tipo_pessoa, loja, comIndicador)).ToList();
            formaPagto.ListaParcComEntPrestacao = (await ObterFormaPagtoParcComEntPrestacao(apelido, tipo_pessoa)).ToList();

            //formaPagto.ListaParcSemEntPrimPrest = (await ObterFormaPagtoParcSemEntPrimPrest(apelido, tipo_pessoa)).ToList();
            //formaPagto.ListaParcSemEntPrestacao = (await ObterFormaPagtoParcSemEntPrestacao(apelido, tipo_pessoa)).ToList();



            return formaPagto;
        }

        private async Task<IEnumerable<AvistaDto>> ObterFormaPagtoAVista(string apelido, string tipo_pessoa, string loja,
            int comIndicador)
        {
            var db = contextoProvider.GetContextoLeitura();

            List<AvistaDto> lstMeioPagto = new List<AvistaDto>();

            if (loja != Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE && comIndicador == 1)
            {
                lstMeioPagto = await (from c in db.TformaPagto
                                      where c.Hab_a_vista == 1 &&
                                            !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
                                              where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                                    d.Id_orcamentista_e_indicador ==
                                                         Constantes.Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
                                                    d.Tipo_cliente == tipo_pessoa &&
                                                    d.St_restricao_ativa != 0
                                              select d.Id_forma_pagto).Contains(c.Id)
                                      orderby c.Ordenacao
                                      select new AvistaDto
                                      {
                                          Id = c.Id,
                                          Descricao = c.Descricao,
                                          Ordenacao = c.Ordenacao
                                      }).ToListAsync();
            }
            else
            {
                lstMeioPagto = await (from c in db.TformaPagto
                                      where c.Hab_a_vista == 1
                                      orderby c.Ordenacao ascending
                                      select new AvistaDto
                                      {
                                          Id = c.Id,
                                          Descricao = c.Descricao,
                                          Ordenacao = c.Ordenacao
                                      }).ToListAsync();
            }

            return lstMeioPagto;
        }

        private async Task<IEnumerable<ParcUnicaDto>> ObterFormaPagtoParcUnica(string apelido, string tipo_pessoa)
        {
            var db = contextoProvider.GetContextoLeitura();

            var formaPagtoTask = from c in db.TformaPagto
                                 where c.Hab_parcela_unica == 1 &&
                                       !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
                                         where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                               d.Id_orcamentista_e_indicador ==
                                                    Constantes.Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
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

            var flagTask = from c in db.TorcamentistaEIndicadorRestricaoFormaPagto
                           where c.Id_orcamentista_e_indicador == apelido &&
                                 c.Id_forma_pagto == short.Parse(Constantes.Constantes.ID_FORMA_PAGTO_CARTAO) &&
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

            var flagTask = from c in db.TorcamentistaEIndicadorRestricaoFormaPagto
                           where c.Id_orcamentista_e_indicador == apelido &&
                                 c.Id_forma_pagto == short.Parse(Constantes.Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA) &&
                                 c.Tipo_cliente == tipo_pessoa &&
                                 c.St_restricao_ativa != 0
                           select c;

            var flag = await flagTask.ToListAsync();

            if (!flag.Any())
                retorno = true;

            return retorno;
        }

        private async Task<IEnumerable<ParcComEntradaDto>> ObterFormaPagtoParcComEntrada(string apelido, string tipo_pessoa, string loja,
            int comIndicador)
        {
            var db = contextoProvider.GetContextoLeitura();

            List<ParcComEntradaDto> lst_parc_com_entrada = new List<ParcComEntradaDto>();

            if (comIndicador == 1)
            {
                lst_parc_com_entrada = await (from c in db.TformaPagto
                                              where c.Hab_parcela_unica == 1 &&
                                                    !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
                                                      where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                                            d.Id_orcamentista_e_indicador ==
                                                                 Constantes.Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
                                                            d.Tipo_cliente == tipo_pessoa &&
                                                            d.St_restricao_ativa != 0
                                                      select d.Id_forma_pagto).Contains(c.Id)
                                              orderby c.Ordenacao
                                              select new ParcComEntradaDto
                                              {
                                                  Id = c.Id,
                                                  Descricao = c.Descricao,
                                                  Ordenacao = c.Ordenacao
                                              }).ToListAsync();
            }
            else
            {
                lst_parc_com_entrada = await (from c in db.TformaPagto
                                              where c.Hab_a_vista == 1
                                              orderby c.Ordenacao ascending
                                              select new ParcComEntradaDto
                                              {
                                                  Id = c.Id,
                                                  Descricao = c.Descricao,
                                                  Ordenacao = c.Ordenacao
                                              }).ToListAsync();
            }

            return lst_parc_com_entrada;
        }

        private async Task<IEnumerable<ParcComEntPrestacaoDto>> ObterFormaPagtoParcComEntPrestacao(string apelido, string tipo_pessoa)
        {
            var db = contextoProvider.GetContextoLeitura();

            var formaPagtoTask = from c in db.TformaPagto
                                 where c.Hab_prestacao == 1 &&
                                       !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
                                         where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                               d.Id_orcamentista_e_indicador ==
                                                    Constantes.Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
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
            //COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA
            //COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA
            var db = contextoProvider.GetContextoLeitura();

            var formaPagtoTask = from c in db.TformaPagto
                                 where c.Hab_prestacao == 1 &&
                                       !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
                                         where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                               d.Id_orcamentista_e_indicador ==
                                                    Constantes.Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
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

        private async Task<IEnumerable<ParcSemEntPrestacaoDto>> ObterFormaPagtoParcSemEntPrestacao(string apelido, string tipo_pessoa)
        {
            var db = contextoProvider.GetContextoLeitura();

            var formaPagtoTask = from c in db.TformaPagto
                                 where c.Hab_prestacao == 1 &&
                                       !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
                                         where (d.Id_orcamentista_e_indicador == apelido.ToUpper() ||
                                               d.Id_orcamentista_e_indicador ==
                                                    Constantes.Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
                                               d.Tipo_cliente == tipo_pessoa &&
                                               d.St_restricao_ativa != 0
                                         select d.Id_forma_pagto).Contains(c.Id)
                                 orderby c.Ordenacao
                                 select new ParcSemEntPrestacaoDto
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

            var qtdeTask = from c in db.TprazoPagtoVisanet
                           where c.Tipo == Constantes.Constantes.COD_VISANET_PRAZO_PAGTO_LOJA
                           select c.Qtde_parcelas;
            int qtde = Convert.ToInt32(await qtdeTask.FirstOrDefaultAsync());

            return qtde;
        }


        public async Task<IEnumerable<SelectListItem>> MontarListaFormaPagto(string usuario, string tipoPessoa, string loja,
            int comIndicador)
        {
            var lst = await ObterFormaPagto(usuario, tipoPessoa, loja, comIndicador);
            //        Selecionar = 0,
            //Avista = 1,
            //ParcCartaoInternet = 2,
            //ParcComEnt = 3,
            //ParcSemEnt = 4,
            //ParcUnica = 5,
            //ParcCartaoMaquineta = 6
            List<SelectListItem> enumPagto = new List<SelectListItem>();
            enumPagto.Add(new SelectListItem { Value = "0", Text = "Selecionar" });

            if (lst.ListaAvista != null)
                enumPagto.Add(new SelectListItem { Value = "1", Text = "À vista" });

            if (lst.ListaParcUnica != null)
                enumPagto.Add(new SelectListItem { Value = "5", Text = "Parcela única" });

            if (lst.ListaParcComEntrada != null)
                enumPagto.Add(new SelectListItem { Value = "3", Text = "Parcelado com entrada" });

            if (lst.ListaParcSemEntPrimPrest != null)
                enumPagto.Add(new SelectListItem { Value = "4", Text = "Parcelado sem entrada" });

            if (lst.ParcCartaoInternet)
                enumPagto.Add(new SelectListItem { Value = "2", Text = "Parcelado Cartão Internet" });

            if (lst.ParcCartaoMaquineta)
                enumPagto.Add(new SelectListItem { Value = "6", Text = "Parcelado Cartão Maquineta" });

            return enumPagto;
        }
    }
}
