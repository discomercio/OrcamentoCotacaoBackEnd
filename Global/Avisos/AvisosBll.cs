using Avisos.Dados;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avisos
{
    public class AvisosBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public AvisosBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<AvisoDados>> BuscarAvisosNaoLidos(string loja, string usuario)
        {
            var db = contextoProvider.GetContextoLeitura();
            //vamos buscar todos os avisos
            List<Taviso> avisos = (await UtilsGlobais.Util.BuscarAvisos(loja, usuario, contextoProvider)).ToList();

            //vamos buscar os avisos lidos
            List<AvisoDados> ret = new List<AvisoDados>();

            if (avisos != null)
            {
                foreach (var i in avisos)
                {
                    ret.Add(new AvisoDados
                    {
                        Id = i.Id,
                        Usuario = i.Usuario,
                        Mensagem = i.Mensagem,
                        Destinatario = i.Destinatario,
                        Dt_ult_atualizacao = i.Dt_ult_atualizacao
                    });
                }
            }

            return ret;
        }

        public async Task<bool> RemoverAvisos(string loja, string usuario, List<string> itens)
        {
            bool retorno = false;
            //vamos verificar se o aviso existe e obter a info para log
            List<TavisoLido> avisoLido = (await UtilsGlobais.Util.BuscarAvisosLidos(usuario, contextoProvider)).ToList();

            if (avisoLido != null)
            {
                //pegamos apenas o que não tem na lista de avisos lidos
                List<string> lstNaoLido = (from c in itens
                                           where (!(from d in avisoLido
                                                    where d.Usuario == usuario
                                                    select d.Id).Contains(c))
                                           select c).ToList();

                if (lstNaoLido.Count > 0)
                {
                    using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
                    {
                        //marcamos como lido
                        retorno = await GravarAvisoLido(lstNaoLido, usuario, dbgravacao);
                        if (retorno)
                        {
                            //vamos garantir que salvamos o log
                            retorno = false;
                            //Montar log de aviso
                            string log = await MontarLogAvisoLido(lstNaoLido);

                            //gravamos o log
                            retorno = UtilsGlobais.Util
                                .GravaLog(dbgravacao, usuario, loja, "", "", Constantes.OP_LOG_AVISO_LIDO, log);

                            dbgravacao.transacao.Commit();
                        }
                    }
                }
            }

            return retorno;
        }

        private async Task<bool> GravarAvisoLido(List<string> avisosLidos, string usuario, ContextoBdGravacao dbGravacao)
        {
            bool retorno = false;
            List<TavisoLido> lstTavisoLido = new List<TavisoLido>();

            foreach (var i in avisosLidos)
            {
                TavisoLido avisoLido = new TavisoLido
                {
                    Id = i,
                    Usuario = usuario.ToUpper(),
                    Data = DateTime.Now
                };

                dbGravacao.Add(avisoLido);
                retorno = true;
            }

            await dbGravacao.SaveChangesAsync();

            return retorno;
        }

        private async Task<string> MontarLogAvisoLido(List<string> lstNaoLido)
        {
            string log = "";
            foreach (var i in lstNaoLido)
            {
                if (!string.IsNullOrEmpty(log))
                    log += "; ";

                log += DateTime.Now + " (id=" + i + ")";
            }

            if (!string.IsNullOrEmpty(log))
            {
                log = "Leitura do aviso divulgado em: " + log;
            }

            return await Task.FromResult(log);
        }
    }
}
