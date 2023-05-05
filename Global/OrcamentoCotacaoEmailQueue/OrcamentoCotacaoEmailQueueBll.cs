using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cfg.CfgOrcamentoCotacaoEmailTemplate;

namespace OrcamentoCotacaoEmailQueue
{
    public class OrcamentoCotacaoEmailQueueBll : BaseBLL<TorcamentoCotacaoEmailQueue, TorcamentoCotacaoEmailQueueFiltro>
    {
        private readonly CfgOrcamentoCotacaoEmailTemplateBll cfgOrcamentoCotacaoEmailTemplateBll;

        private OrcamentoCotacaoEmailQueueData _data { get; set; }

        public OrcamentoCotacaoEmailQueueBll(ContextoBdProvider contextoBdProvider, CfgOrcamentoCotacaoEmailTemplateBll _cfgOrcamentoCotacaoEmailTemplateBll) : base(new OrcamentoCotacaoEmailQueueData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoEmailQueueData(contextoBdProvider);
            cfgOrcamentoCotacaoEmailTemplateBll = _cfgOrcamentoCotacaoEmailTemplateBll;
        }

        public TorcamentoCotacaoEmailQueue InserirQueueComTemplateEHTML(int IdCfgOrcamentoCotacaoEmailTemplates, TorcamentoCotacaoEmailQueue orcamentoCotacaoEmailQueue, string[] tagHtml)
        {            
            try
            {
                List<TcfgOrcamentoCotacaoEmailTemplate> tcfgOrcamentoCotacaoEmailTemplates = cfgOrcamentoCotacaoEmailTemplateBll.PorFiltro(new TcfgOrcamentoCotacaoEmailTemplateFiltro() { Id = IdCfgOrcamentoCotacaoEmailTemplates });
                string emailTemplateBody = tcfgOrcamentoCotacaoEmailTemplates[0].EmailTemplateBody;

                orcamentoCotacaoEmailQueue.Subject = tcfgOrcamentoCotacaoEmailTemplates[0].EmailTemplateSubject;
                orcamentoCotacaoEmailQueue.DateCreated = DateTime.Now;
                emailTemplateBody = emailTemplateBody.Replace("{Cliente}", tagHtml[0]);
                emailTemplateBody = emailTemplateBody.Replace("{DadosEmpresa}", tagHtml[1]);
                emailTemplateBody = emailTemplateBody.Replace("{LinkOrcamento}", tagHtml[2]);
                emailTemplateBody = emailTemplateBody.Replace("{NroOrcamento}", tagHtml[3]);
                emailTemplateBody = emailTemplateBody.Replace("{URL_Base_Front}", tagHtml[4]);
                emailTemplateBody = emailTemplateBody.Replace("{LogoEmpresa}", tagHtml[5]);
                orcamentoCotacaoEmailQueue.Body = emailTemplateBody;

                var data = _data.Inserir(orcamentoCotacaoEmailQueue);

                return data;
            }
            catch (Exception)
            {
                throw new ArgumentException("Não foi possível cadastrar o orçamento. Problema no envio de e-mail!");
            }
        }

        public TorcamentoCotacaoEmailQueue InserirQueueComTemplateEHTMLComTransacao(int IdCfgOrcamentoCotacaoEmailTemplates,
            TorcamentoCotacaoEmailQueue orcamentoCotacaoEmailQueue, string[] tagHtml,
            ContextoBdGravacao contextoBdGravacao)
        {
            try
            {
                List<TcfgOrcamentoCotacaoEmailTemplate> tcfgOrcamentoCotacaoEmailTemplates = cfgOrcamentoCotacaoEmailTemplateBll.PorFiltro(new TcfgOrcamentoCotacaoEmailTemplateFiltro() { Id = IdCfgOrcamentoCotacaoEmailTemplates });
                string emailTemplateBody = tcfgOrcamentoCotacaoEmailTemplates[0].EmailTemplateBody;

                var emailTemplateSubject = tcfgOrcamentoCotacaoEmailTemplates[0].EmailTemplateSubject;
                emailTemplateSubject = emailTemplateSubject.Replace("{NumeroOrcamento}", tagHtml[3]);
                emailTemplateSubject = emailTemplateSubject.Replace("{Cliente}", tagHtml[0]);
                orcamentoCotacaoEmailQueue.Subject = emailTemplateSubject;

                orcamentoCotacaoEmailQueue.DateCreated = DateTime.Now;
                emailTemplateBody = emailTemplateBody.Replace("{Cliente}", tagHtml[0]);
                emailTemplateBody = emailTemplateBody.Replace("{DadosEmpresa}", tagHtml[1]);
                emailTemplateBody = emailTemplateBody.Replace("{LinkOrcamento}", tagHtml[2]);
                emailTemplateBody = emailTemplateBody.Replace("{NroOrcamento}", tagHtml[3]);
                emailTemplateBody = emailTemplateBody.Replace("{URL_Base_Front}", tagHtml[4]);
                emailTemplateBody = emailTemplateBody.Replace("{LogoEmpresa}", tagHtml[5]);
                orcamentoCotacaoEmailQueue.Body = emailTemplateBody;
                
                var data = _data.InserirComTransacao(orcamentoCotacaoEmailQueue, contextoBdGravacao);

                return data;
            }
            catch (Exception)
            {
                throw new ArgumentException("Não foi possível cadastrar o orçamento. Problema no envio de e-mail!");
            }
        }
    }
}
