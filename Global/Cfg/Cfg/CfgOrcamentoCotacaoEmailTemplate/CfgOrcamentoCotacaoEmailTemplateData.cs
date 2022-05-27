using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cfg.CfgOrcamentoCotacaoEmailTemplate
{
    public class CfgOrcamentoCotacaoEmailTemplateData : BaseData<TcfgOrcamentoCotacaoEmailTemplate, TcfgOrcamentoCotacaoEmailTemplateFiltro>
    {
        private readonly ContextoBdProvider contextoProvider;

        public CfgOrcamentoCotacaoEmailTemplateData(ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }
        public TcfgOrcamentoCotacaoEmailTemplate Atualizar(TcfgOrcamentoCotacaoEmailTemplate obj)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(TcfgOrcamentoCotacaoEmailTemplate obj)
        {
            throw new NotImplementedException();
        }

        public TcfgOrcamentoCotacaoEmailTemplate Inserir(TcfgOrcamentoCotacaoEmailTemplate obj)
        {
            throw new NotImplementedException();
        }

        public TcfgOrcamentoCotacaoEmailTemplate InserirComTransacao(TcfgOrcamentoCotacaoEmailTemplate model, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgOrcamentoCotacaoEmailTemplate> PorFilroComTransacao(TcfgOrcamentoCotacaoEmailTemplateFiltro obj, ContextoBdGravacao contextoBdGravacao)
        {
            throw new NotImplementedException();
        }

        public List<TcfgOrcamentoCotacaoEmailTemplate> PorFiltro(TcfgOrcamentoCotacaoEmailTemplateFiltro obj)
        {
            List<TcfgOrcamentoCotacaoEmailTemplate> lista = null;

            using (var db = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var templates = from t in db.TcfgOrcamentoCotacaoEmailTemplates
                                select t;

                if (obj.Id.HasValue)
                {
                    templates = templates.Where(x => x.Id == obj.Id);
                }
                if(obj.IdCfgUnidadeNegocio.HasValue)
                {
                    templates = templates.Where(x => x.IdCfgUnidadeNegocio == obj.IdCfgUnidadeNegocio);
                }

                lista = templates.ToList();

            }

            return lista;
        }
    }
}
