﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using InfraBanco.Constantes;
using InfraBanco;
using FormaPagamento.Dados;
using InfraBanco.Modelos;
using MeioPagamentos;

namespace FormaPagamento
{
    public class FormaPagtoBll
    {
        private readonly ContextoBdProvider contextoProvider;

        public readonly FormaPagamentoData _formaPagamentoData;
        public FormaPagtoBll(InfraBanco.ContextoBdProvider contextoProvider, FormaPagamentoData formaPagamentoData)
        {
            this.contextoProvider = contextoProvider;
            this._formaPagamentoData = formaPagamentoData;
        }

        public async Task<FormaPagtoDados> ObterFormaPagto(string apelido, string tipo_pessoa,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
        {
            FormaPagtoDados formaPagto = new FormaPagtoDados();

            //o apelido pode ser null
            apelido = (apelido ?? "");

            //implementar as buscas
            formaPagto.ListaAvista = (await ObterFormaPagtoAVista(apelido, tipo_pessoa)).ToList();
            //parcela unica: se PJ ou no magento aceita sempre
            if (tipo_pessoa == Constantes.ID_PJ
                || sistemaResponsavel == Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
            {
                formaPagto.ListaParcUnica = (await ObterFormaPagtoParcUnica(apelido, tipo_pessoa)).ToList();
            }
            formaPagto.ParcCartaoInternet = await ObterFlagParcCartaoInternet(apelido, tipo_pessoa);
            formaPagto.ParcCartaoMaquineta = await ObterFlagParcCartaoMaquineta(apelido, tipo_pessoa);
            formaPagto.ListaParcComEntrada = (await ObterFormaPagtoParcComEntrada(apelido, tipo_pessoa)).ToList();
            formaPagto.ListaParcComEntPrestacao = (await ObterFormaPagtoParcComEntPrestacao(apelido, tipo_pessoa)).ToList();

            return formaPagto;
        }

        public async Task<int> GetMaximaQtdeParcelasCartaoVisa()
        {
            var qtdeParcelas = 0;

            using (var db = contextoProvider.GetContextoLeitura())
            {
             /*
                private string COD_VISANET_PRAZO_PAGTO_LOJA = "PRAZO_LOJA";
                string SelectQtdeMaxParcelaCartaoVisa = "SELECT qtde_parcelas FROM t_PRAZO_PAGTO_VISANET WHERE tipo = @COD_VISANET_PRAZO_PAGTO_LOJA";
             */
                var formaPagto = await (from c in db.TprazoPagtoVisanet
                                 where c.Tipo == Constantes.COD_VISANET_PRAZO_PAGTO_LOJA
                                 select c.Qtde_parcelas).FirstOrDefaultAsync();

                qtdeParcelas = formaPagto;
            }

            return qtdeParcelas;
        }

        private async Task<IEnumerable<AvistaDados>> ObterFormaPagtoAVista(string apelido, string tipo_pessoa)
        {
            List<AvistaDados> formaPagto;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                formaPagto = await (from c in db.TformaPagto
                                    where c.Hab_a_vista == 1 &&
                                          !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
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
                                    }).ToListAsync();
            }

            return formaPagto;
        }

        private async Task<IEnumerable<ParcUnicaDados>> ObterFormaPagtoParcUnica(string apelido, string tipo_pessoa)
        {
            List<ParcUnicaDados> formaPagto;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                formaPagto = await (from c in db.TformaPagto
                                    where c.Hab_parcela_unica == 1 &&
                                          !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
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
                                    }).ToListAsync();
            }

            return formaPagto;
        }

        private async Task<bool> ObterFlagParcCartaoInternet(string apelido, string tipo_pessoa)
        {
            bool retorno = false;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                var flag = await (from c in db.TorcamentistaEIndicadorRestricaoFormaPagto
                                  where c.Id_orcamentista_e_indicador == apelido &&
                                        c.Id_forma_pagto == (short)Constantes.FormaPagto.ID_FORMA_PAGTO_CARTAO &&
                                        c.Tipo_cliente == tipo_pessoa &&
                                        c.St_restricao_ativa != 0
                                  select c).ToListAsync();

                if (!flag.Any())
                    retorno = true;
            }

            return retorno;
        }

        private async Task<bool> ObterFlagParcCartaoMaquineta(string apelido, string tipo_pessoa)
        {
            bool retorno = false;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                var flag = await (from c in db.TorcamentistaEIndicadorRestricaoFormaPagto
                                  where c.Id_orcamentista_e_indicador == apelido &&
                                        c.Id_forma_pagto == (short)Constantes.FormaPagto.ID_FORMA_PAGTO_CARTAO_MAQUINETA &&
                                        c.Tipo_cliente == tipo_pessoa &&
                                        c.St_restricao_ativa != 0
                                  select c).ToListAsync();

                if (!flag.Any())
                    retorno = true;
            }

            return retorno;
        }

        private async Task<IEnumerable<ParcComEntradaDados>> ObterFormaPagtoParcComEntrada(string apelido, string tipo_pessoa)
        {
            List<ParcComEntradaDados> formaPagto;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                formaPagto = await (from c in db.TformaPagto
                                    where c.Hab_entrada == 1 &&
                                          !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
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
                                    }).ToListAsync();
            }

            return formaPagto;
        }

        private async Task<IEnumerable<ParcComEntPrestacaoDados>> ObterFormaPagtoParcComEntPrestacao(string apelido, string tipo_pessoa)
        {
            List<ParcComEntPrestacaoDados> formaPagto;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                formaPagto = await (from c in db.TformaPagto
                                    where c.Hab_prestacao == 1 &&
                                          !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
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
                                    }).ToListAsync();
            }

            return formaPagto;
        }

        private async Task<IEnumerable<ParcSemEntradaPrimPrestDados>> ObterFormaPagtoParcSemEntPrimPrest(string apelido, string tipo_pessoa)
        {
            List<ParcSemEntradaPrimPrestDados> formaPagto;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                formaPagto = await (from c in db.TformaPagto
                                    where c.Hab_prestacao == 1 &&
                                          !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
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
                                    }).ToListAsync();
            }

            return formaPagto;
        }

        private async Task<IEnumerable<ParcSemEntPrestacaoDados>> ObterFormaPagtoParcSemEntPrestacao(string apelido, string tipo_pessoa)
        {
            List<ParcSemEntPrestacaoDados> formaPagto;

            using (var db = contextoProvider.GetContextoLeitura())
            {
                formaPagto = await (from c in db.TformaPagto
                                    where c.Hab_prestacao == 1 &&
                                          !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
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
                                    }).ToListAsync();
            }

            return formaPagto;
        }

        //Foi solicitado que a qtde de parcelas máxima permitida 
        //seria baseada na qtde parcelas permitida pelo cartão Visa(PRAZO_LOJA)
        public async Task<int> BuscarQtdeParcCartaoVisa()
        {
            return await _formaPagamentoData.GetMaximaQtdeParcelasCartaoVisa();
        }

        public List<TcfgPagtoFormaStatus> BuscarFormasPagtos(bool incluirTcfgPagtoForma, Constantes.Modulos modulo,
            string tipoCliente, bool comIndicador, bool habilitado, Constantes.eTipoUsuarioPerfil tipoUsuario)
        {
            return _formaPagamentoData.PorFiltro(new InfraBanco.Modelos.Filtros.TcfgPagtoFormaStatusFiltro()
            {
                IdCfgModulo = (short)modulo,
                IdCfgTipoPessoaCliente = (short)(tipoCliente == Constantes.ID_PF ? 1 : 2),
                IdCfgTipoUsuarioPerfil = (short)tipoUsuario,
                PedidoComIndicador = (byte)(comIndicador ? 1 : 0),
                Habilitado = (byte)(habilitado ? 1 : 0),
                IncluirTcfgPagtoForma = incluirTcfgPagtoForma
            });
        }

    }
}