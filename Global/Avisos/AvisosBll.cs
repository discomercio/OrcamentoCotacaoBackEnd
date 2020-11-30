using Avisos.Dados;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Taviso>> BuscarTodosAvisos(List<string> lst)
        {
            var db = contextoProvider.GetContextoLeitura();
            var ret = (from c in db.Tavisos
                       where (from d in lst
                              select d).Contains(c.Id)
                       select c).ToListAsync();

            return await ret;
        }

        public async Task<IEnumerable<Taviso>> BuscarTodosAvisosNaoLidos(string loja, string usuario)
        {
            var db = contextoProvider.GetContextoLeitura();
            var ret = (from c in db.Tavisos
                       where (!(from d in db.TavisoLidos
                                where d.Usuario == usuario
                                select d.Id).Contains(c.Id)) &&
                       ((c.Destinatario == "") ||
                             (c.Destinatario == null) ||
                             (c.Destinatario == loja))
                       select c).OrderByDescending(x => x.Dt_ult_atualizacao).ToListAsync();


            return await ret;
        }

        public async Task<IEnumerable<TavisoLido>> BuscarAvisosLidos(string usuario)
        {
            var db = contextoProvider.GetContextoLeitura();

            var ret = (from c in db.TavisoLidos
                       where c.Usuario == usuario
                       select c).ToListAsync();

            return await ret;
        }

        public async Task<IEnumerable<TavisoExibido>> BuscarAvisosExibidos(string usuario)
        {
            var db = contextoProvider.GetContextoLeitura();

            var ret = (from c in db.TavisoExibidos
                       where c.Usuario == usuario
                       select c).ToListAsync();

            return await ret;
        }

        public async Task<IEnumerable<AvisoDados>> BuscarAvisosNaoLidos(string loja, string usuario)
        {
            var db = contextoProvider.GetContextoLeitura();
            //vamos buscar todos os avisos
            List<Taviso> avisos = (await BuscarTodosAvisosNaoLidos(loja, usuario)).ToList();

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
            List<TavisoLido> avisoLido = (await BuscarAvisosLidos(usuario)).ToList();

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

        public async Task<bool> MarcarAvisoExibido(List<string> lst, string usuario, string loja)
        {
            bool retorno = false;
            List<Taviso> avisos = (await BuscarTodosAvisos(lst)).ToList();
            List<TavisoExibido> avisosExibidos = (await BuscarAvisosExibidos(usuario)).ToList();
            if (avisos != null)
            {
                if (lst.Count > 0)
                {
                    //vamos pegar o que não existe mais em avisos
                    List<string> lstNaoExiste = (from c in avisos
                                                 where !(from d in lst
                                                         select d).Contains(c.Id)
                                                 select c.Id).ToList();

                    //vamos verificar o que não existe para remover da lista enviada
                    if(lstNaoExiste != null)
                    {
                        if(lstNaoExiste.Count > 0)
                        {
                            foreach(var i in lstNaoExiste)
                            {
                                string id = (from c in lst
                                             where c == i
                                             select c).FirstOrDefault();

                                lst.Remove(id);
                            }
                        }
                    }

                    //pegando avisos não exibidos
                    List<string> lstAvisoNaoExibido = (from c in lst
                                                       where !(from d in avisosExibidos
                                                               where d.Usuario == usuario
                                                               select d.Id).Contains(c)
                                                       select c).ToList();

                    List<TavisoExibido> lstAvisosJaExibidos = (from c in avisosExibidos
                                                               where (from d in lst
                                                                      select d).Contains(c.Id) &&
                                                                      c.Usuario == usuario
                                                               select c).ToList();

                    using (var dbGravacao = contextoProvider.GetContextoGravacaoParaUsing())
                    {
                        //gravar os avisos não exibidos
                        if (lstAvisoNaoExibido.Count > 0)
                        {
                            await GravarAvisoExibido(lstAvisoNaoExibido, usuario, dbGravacao);
                        }

                        //atualiza avisos ja exibidos
                        if (lstAvisosJaExibidos.Count > 0)
                        {
                            await AtualizaAvisoJaExibido(lstAvisosJaExibidos, dbGravacao);
                        }

                        dbGravacao.transacao.Commit();
                        retorno = true;
                    }
                }
            }
            return retorno;
        }

        private async Task GravarAvisoExibido(List<string> lstAvisoNaoExibido, string usuario,
            ContextoBdGravacao dbGravacao)
        {
            foreach (var i in lstAvisoNaoExibido)
            {
                TavisoExibido naoExibido = new TavisoExibido
                {
                    Id = i,
                    Usuario = usuario,
                    Dt_hr_ult_exibicao = DateTime.Now
                };

                dbGravacao.Add(naoExibido);
            }

            await dbGravacao.SaveChangesAsync();
        }

        private async Task AtualizaAvisoJaExibido(List<TavisoExibido> lstAvisosJaExibidos, ContextoBdGravacao dbGravacao)
        {
            foreach (var i in lstAvisosJaExibidos)
            {
                i.Dt_hr_ult_exibicao = DateTime.Now;
                dbGravacao.Update(i);
            }

            await dbGravacao.SaveChangesAsync();
        }
    }
}

