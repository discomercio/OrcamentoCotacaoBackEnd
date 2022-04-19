﻿using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Loja.Dados;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Loja
{
    public class LojaData : BaseData<Tloja, TlojaFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public LojaData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public Tloja Atualizar(Tloja obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(Tloja obj)
        {
            throw new NotImplementedException();
        }

        public Tloja Inserir(Tloja obj)
        {
            throw new NotImplementedException();
        }

        public List<Tloja> PorFiltro(TlojaFiltro obj)
        {
            try
            {
                /*SELECT orc.Nome, orc.email, orc.nome, orc.IdParceiro, 
                   orc.ativo, toei.vendedor as IdVendedor FROM t_ORCAMENTISTA_E_INDICADOR_VENDEDOR orc 
                   INNER JOIN t_ORCAMENTISTA_E_INDICADOR toei ON toei.apelido = orc.IdParceiro 
                   WHERE orc.email = @login AND orc.senha = @senha*/
                using (var db = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var lojas = from L in db.Tlojas
                                select L;

                    if (!string.IsNullOrEmpty(obj.Loja))
                    {
                        lojas = lojas.Where(x => x.Loja == obj.Loja);
                    }

                    return lojas.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public PercMaxDescEComissaoDados BuscarPercMaxPorLoja(string loja)
        {
            var db = contextoProvider.GetContextoLeitura();

            return (from c in db.Tlojas
                          where c.Loja == loja
                          select new PercMaxDescEComissaoDados
                          {
                              PercMaxComissao = c.Perc_Max_Comissao,
                              PercMaxComissaoEDesconto = c.Perc_Max_Comissao_E_Desconto,
                              PercMaxComissaoEDescontoPJ = c.Perc_Max_Comissao_E_Desconto_Pj,
                              PercMaxComissaoEDescontoNivel2 = c.Perc_Max_Comissao_E_Desconto_Nivel2,
                              PercMaxComissaoEDescontoNivel2PJ = c.Perc_Max_Comissao_E_Desconto_Nivel2_Pj
                          }).FirstOrDefault();
        }

        public Tloja InserirComTransacao(Tloja model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<Tloja> PorFilroComTransacao(TlojaFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }
    }
}
