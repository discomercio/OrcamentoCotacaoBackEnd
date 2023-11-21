using InfraBanco;
using InfraBanco.Constantes;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace UtilsGlobais.Parametros
{
    public class ParametroOrcamentoCotacaoData
    {
        private readonly ContextoBdProvider _contextoProvider;

        public ParametroOrcamentoCotacaoData(ContextoBdProvider contextoProvider)
        {
            _contextoProvider = contextoProvider;
        }

        public ParametroOrcamentoCotacaoDto ObterParametros(string lojaLogada)
        {
            List<int> lista = new List<int>
            {
                (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_ValidadeOrcamento_ValidadePadrao,
                (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_ValidadeOrcamento_PrazoProrrogacaoPadrao,
                (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_ValidadeOrcamento_QtdeMaxProrrogacao,
                (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_ValidadeOrcamento_MaxPrazoValidadeGlobal,
                (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_MaxPeriodoConsulta_FiltroPesquisa,
                (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_MaxPeriodoConsulta_RelatorioGerencial,
                (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_MaxQtdeOpcoes
            };

            using (var db = _contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var saida =
                    from p in db.TcfgParametro
                    join pu in db.TcfgUnidadeNegocioParametro on p.Id equals pu.IdCfgParametro
                    join un in db.TcfgUnidadeNegocio on pu.IdCfgUnidadeNegocio equals un.Id
                    join l in db.Tloja on un.Sigla equals l.Unidade_Negocio
                    where
                        lista.Contains(p.Id) &&
                        l.Loja == lojaLogada
                    select new
                    {
                        p.Id,
                        pu.Valor,
                        pu.IdCfgUnidadeNegocio
                    };

                ParametroOrcamentoCotacaoDto retorno = new ParametroOrcamentoCotacaoDto();

                retorno.QtdePadrao_DiasValidade = saida.FirstOrDefault(x => x.Id == (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_ValidadeOrcamento_ValidadePadrao).Valor;
                retorno.QtdePadrao_DiasProrrogacao = saida.FirstOrDefault(x => x.Id == (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_ValidadeOrcamento_PrazoProrrogacaoPadrao).Valor;
                retorno.QtdeMaxProrrogacao = saida.FirstOrDefault(x => x.Id == (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_ValidadeOrcamento_QtdeMaxProrrogacao).Valor;
                retorno.QtdeGlobal_Validade = saida.FirstOrDefault(x => x.Id == (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_ValidadeOrcamento_MaxPrazoValidadeGlobal).Valor;
                retorno.MaxQtdeOpcoes = int.Parse(saida.FirstOrDefault(x => x.Id == (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_MaxQtdeOpcoes).Valor);
                
                int periodoConsulta = 0;
                if (int.TryParse(saida.FirstOrDefault(x => x.Id == (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_MaxPeriodoConsulta_FiltroPesquisa).Valor, out periodoConsulta))
                {
                    retorno.MaxPeriodoConsultaFiltroPesquisa = periodoConsulta;
                }
                if (int.TryParse(saida.FirstOrDefault(x => x.Id == (int)Constantes.eCfgParametro.ModuloOrcamentoCotacao_MaxPeriodoConsulta_RelatorioGerencial).Valor, out periodoConsulta))
                {
                    retorno.MaxPeriodoConsulta_RelatorioGerencial = periodoConsulta;
                }

                var param = Util.BuscarRegistroParametro(Constantes.PEDIDOITEM_MAXQTDEITENS, _contextoProvider).Result;
                retorno.MaxQtdeItens = param.Campo_inteiro;

                return retorno;
            }
        }
    }
}