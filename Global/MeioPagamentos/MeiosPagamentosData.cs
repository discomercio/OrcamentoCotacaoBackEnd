using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InfraBanco.Constantes;

namespace MeioPagamentos
{
    public class MeiosPagamentosData : BaseData<TcfgPagtoMeioStatus, TcfgPagtoMeioStatusFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public MeiosPagamentosData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public TcfgPagtoMeioStatus Atualizar(TcfgPagtoMeioStatus obj)
        {
            throw new NotImplementedException();
        }

        public TcfgPagtoMeioStatus AtualizarComTransacao(TcfgPagtoMeioStatus model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TcfgPagtoMeioStatus obj)
        {
            throw new NotImplementedException();
        }

        public void ExcluirComTransacao(TcfgPagtoMeioStatus obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public TcfgPagtoMeioStatus Inserir(TcfgPagtoMeioStatus obj)
        {
            throw new NotImplementedException();
        }

        public TcfgPagtoMeioStatus InserirComTransacao(TcfgPagtoMeioStatus model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgPagtoMeioStatus> PorFilroComTransacao(TcfgPagtoMeioStatusFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgPagtoMeioStatus> PorFiltro(TcfgPagtoMeioStatusFiltro obj)
        {
            try
            {
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var saida = from meiosPagto in db.TcfgPagtoMeioStatus
                                select meiosPagto;

                    if (obj.IncluirTcfgPagtoMeio)
                    {
                        saida = saida.Include(x => x.TcfgPagtoMeio);
                    }
                    if (obj.IdCfgModulo != 0)
                    {
                        saida = saida.Where(x => x.IdCfgModulo == obj.IdCfgModulo);
                    }
                    if (obj.IdCfgTipoPessoaCliente != 0)
                    {
                        saida = saida.Where(x => x.IdCfgTipoPessoaCliente == obj.IdCfgTipoPessoaCliente);
                    }
                    if (obj.IdCfgTipoUsuario != 0)
                    {
                        saida = saida.Where(x => x.IdCfgTipoUsuarioPerfil == obj.IdCfgTipoUsuario);
                    }
                    if (obj.PedidoComIndicador.HasValue)
                    {
                        saida = saida.Where(x => x.PedidoComIndicador == obj.PedidoComIndicador);
                    }
                    if (obj.Habilitado.HasValue)
                    {
                        saida = saida.Where(x => x.Habilitado == obj.Habilitado);
                    }
                    if (obj.IdCfgPagtoForma != 0)
                    {
                        saida = saida.Where(x => x.IdCfgPagtoForma == obj.IdCfgPagtoForma);
                    }
                    if (obj.IncluirTorcamentistaEIndicadorRestricaoFormaPagtos)
                    {
                        saida = from c in saida
                                  where !(from d in db.TorcamentistaEIndicadorRestricaoFormaPagto
                                          where (d.Id_orcamentista_e_indicador == obj.Apelido.ToUpper() ||
                                                d.Id_orcamentista_e_indicador == Constantes.ID_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FP_TODOS) &&
                                                d.Tipo_cliente == (obj.IdCfgTipoPessoaCliente == 1 ? Constantes.ID_PF : Constantes.ID_PJ) &&
                                                d.St_restricao_ativa != 0
                                          select d.Id_forma_pagto).Contains((short)c.IdCfgPagtoMeio)
                                  select c;
                    }

                        return saida.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
