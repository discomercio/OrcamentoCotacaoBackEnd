using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OrcamentoCotacaoLink
{
    public class OrcamentoCotacaoLinkBll : BaseBLL<TorcamentoCotacaoLink, TorcamentoCotacaoLinkFiltro>
    {

        private OrcamentoCotacaoLinkData _data { get; set; }

        public OrcamentoCotacaoLinkBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentoCotacaoLinkData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoLinkData(contextoBdProvider);
        }

        public bool InserirOrcamentoCotacaoLink(TorcamentoCotacaoLink torcamentoCotacaoLink)
        {
            try
            {
                _data.Inserir(torcamentoCotacaoLink);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
