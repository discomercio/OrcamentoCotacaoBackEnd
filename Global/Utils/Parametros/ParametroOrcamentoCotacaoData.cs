﻿using InfraBanco;
using System.Collections.Generic;
using System.Linq;

namespace UtilsGlobais.Parametros
{
    public class ParametroOrcamentoCotacaoData
    {
        private readonly ContextoBdProvider _contextoProvider;

        public ParametroOrcamentoCotacaoData(ContextoBdProvider contextoProvider)
        {
            _contextoProvider = contextoProvider;
        }

        public ParametroOrcamentoCotacaoDto ObterParametros(int idUnidadeNegocio)
        {
            List<int> lista = new List<int>
            {
                14, //ModuloOrcamentoCotacao_ValidadeOrcamento_ValidadePadrao
                15, //ModuloOrcamentoCotacao_ValidadeOrcamento_PrazoProrrogacaoPadrao
                16, //ModuloOrcamentoCotacao_ValidadeOrcamento_QtdeMaxProrrogacao
                17, //ModuloOrcamentoCotacao_ValidadeOrcamento_MaxPrazoValidadeGlobal
            };

            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var saida = from p in db.TcfgParametro
                            join pu in db.TcfgUnidadeNegocioParametro on p.Id equals pu.IdCfgParametro
                            where
                                pu.IdCfgUnidadeNegocio == idUnidadeNegocio &&
                                lista.Contains(p.Id)
                            select new
                            {
                                p.Id,
                                p.Sigla,
                                pu.Valor,
                                pu.IdCfgUnidadeNegocio
                            };

                return new ParametroOrcamentoCotacaoDto
                {
                    QtdePadrao_DiasValidade = saida.FirstOrDefault(x => x.Id == 14).Valor,
                    QtdePadrao_DiasProrrogacao = saida.FirstOrDefault(x => x.Id == 15).Valor,

                    QtdeMaxProrrogacao = saida.FirstOrDefault(x => x.Id == 16).Valor,

                    QtdeGlobal_Validade = saida.FirstOrDefault(x => x.Id == 17).Valor,
                };
            }
        }
    }
}