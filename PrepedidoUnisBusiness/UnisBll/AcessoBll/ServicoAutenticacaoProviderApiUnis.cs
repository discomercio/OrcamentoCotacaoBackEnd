using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraIdentity;
using InfraIdentity.ApiUnis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.AcessoBll
{
    /*
     * efetivamente faz o lofgin; quer dizer, efetivamente verifica a senha
     * */
    public class ServicoAutenticacaoProviderApiUnis : InfraIdentity.ApiUnis.IServicoAutenticacaoProviderApiUnis
    {
        private readonly ContextoBdProvider contextoProvider;

        public ServicoAutenticacaoProviderApiUnis(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        private static string Erro_ERR_IDENTIFICACAO = "OS DADOS INFORMADOS NA IDENTIFICAÇÃO ESTÃO INCORRETOS.";

        //retorna null se nao exisitr (ou se a senha estiver errada)
        public async Task<UsuarioLoginApiUnis> ObterUsuarioApiUnis(string usuarioOriginal, string senha, string ip, string userAgent)
        {
            var ret = new UsuarioLoginApiUnis();

            //trabalhamos sempre com maiúsuculas
            var usuarioMaisuculas = usuarioOriginal.ToUpper().Trim();

            var usuario = await (from u in contextoProvider.GetContextoLeitura().Tusuarios
                                 where u.Usuario.ToUpper() == usuarioMaisuculas
                                 select new
                                 {
                                     u.Nome,
                                     u.SessionCtrlTicket,
                                     u.SessionCtrlDtHrLogon,
                                     u.SessionCtrlLoja,
                                     u.SessionCtrlModulo,
                                     u.Datastamp,
                                     u.Bloqueado,
                                     u.Dt_Ult_Alteracao_Senha
                                 }).FirstOrDefaultAsync();
            if (usuario == null)
            {
                ret.ListaErros.Add(Erro_ERR_IDENTIFICACAO);
                return ret;
            }


            //'	TEM SENHA?
            //	if Trim("" & rs("datastamp")) = "" then usuario_bloqueado=true
            //'	ACESSO BLOQUEADO?
            //	if rs("bloqueado")<>0 then usuario_bloqueado=true
            //if usuario_bloqueado then Response.Redirect("aviso.asp?id=" & ERR_USUARIO_BLOQUEADO)
            if (string.IsNullOrEmpty(usuario.Datastamp))
            {
                ret.ListaErros.Add("ACESSO NEGADO (sem senha)");
                return ret;
            }
            if (usuario.Bloqueado.HasValue && usuario.Bloqueado.Value != 0)
            {
                ret.ListaErros.Add("ACESSO NEGADO (bloqueado)");
                return ret;
            }


            //verificar a senha
            var senha_banco_datastamp_decod = PrepedidoBusiness.Utils.Util.decodificaDado(usuario.Datastamp, Constantes.FATOR_CRIPTO);
            if (senha_banco_datastamp_decod.ToUpper() != senha.ToUpper())
            {
                ret.ListaErros.Add(Erro_ERR_IDENTIFICACAO + " (senha)");
                return ret;
            }


            //TODO: verificar com o Hamilton como fazer. PROVISÓRIO: verificamos se tem o perfil APIUNIS
            /*
			s = "SELECT Count(*) AS qtde FROM t_PERFIL_X_USUARIO INNER JOIN t_PERFIL ON t_PERFIL_X_USUARIO.id_perfil=t_PERFIL.id" & _
				" INNER JOIN t_PERFIL_ITEM ON t_PERFIL.id=t_PERFIL_ITEM.id_perfil" & _
				" INNER JOIN t_OPERACAO ON t_PERFIL_ITEM.id_operacao=t_OPERACAO.id" & _
				" WHERE (t_PERFIL_X_USUARIO.usuario='" & usuario & "')" & _
				" AND (t_OPERACAO.modulo='" & COD_OP_MODULO_CENTRAL & "')"
							*/
            var perfil = await (from p in contextoProvider.GetContextoLeitura().Tperfils.Include(r => r.TperfilUsuario)
                                where p.Apelido == "APIUNIS" && p.TperfilUsuario.Usuario.ToUpper() == usuarioMaisuculas && p.St_inativo == 0
                                select p.Id).FirstOrDefaultAsync();
            if (perfil == null)
            {
                ret.ListaErros.Add("USUÁRIO NÃO TEM ACESSO AO PERFIL APIUNIS");
                return ret;
            }

            /*
             * vamos ignorar a necessidade de alterar a senha
            //if IsNull(dt_ult_alteracao_senha) then Response.Redirect("senha.asp" & "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")))
            */


            //verificar com HAmilton se este formato está OK
            var strSessionCtrlTicket = $"{usuarioMaisuculas} - {DateTime.Now.ToString("yyyy/dd/MM HH:mm:ss.fff")}";
            //strSessionCtrlTicket = PrepedidoBusiness.Utils.Util.codificaDado(strSessionCtrlTicket, true);


            //atualizar dados
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                GravarTsessaoHistorico(dbgravacao, usuarioMaisuculas, strSessionCtrlTicket, ip, userAgent);
                AtualizarTusuario(dbgravacao, usuarioMaisuculas, strSessionCtrlTicket);


                //inserir em TsessaoAbandonada
                if (!string.IsNullOrEmpty(usuario.SessionCtrlTicket))
                {
                    //strMensagemAviso = "A sessão anterior não foi encerrada corretamente.<br>Para segurança da sua identidade, <i>sempre</i> encerre a sessão clicando no link <i>'encerra'</i>.<br>Esta ocorrência será gravada no histórico de auditoria."
                    //strMensagemAvisoPopUp = "**********   A T E N Ç Ã O ! !   **********\nA sessão anterior não foi encerrada corretamente.\nPara segurança da sua identidade, SEMPRE encerre a sessão clicando no link ENCERRA.\nEsta ocorrência será gravada no histórico de auditoria!!"
                    var sessaoAbandonada = new TsessaoAbandonada()
                    {
                        Usuario = usuarioOriginal,
                        SessaoAbandonadaDtHrInicio = usuario.SessionCtrlDtHrLogon ?? DateTime.Now,
                        SessaoAbandonadaLoja = usuario.SessionCtrlLoja,
                        SessaoAbandonadaModulo = usuario.SessionCtrlModulo,
                        SessaoSeguinteDtHrInicio = DateTime.Now,
                        SessaoSeguinteLoja = "",
                        SessaoSeguinteModulo = Constantes.SESSION_CTRL_MODULO_APIUNIS
                    };
                    dbgravacao.TsessaoAbandonadas.Add(sessaoAbandonada);
                    await dbgravacao.SaveChangesAsync();
                }

                dbgravacao.transacao.Commit();
            }


            ret.Nome = usuario.Nome;
            ret.Usuario = usuarioOriginal;
            return ret;
        }

        private void GravarTsessaoHistorico(ContextoBdGravacao dbgravacao, string usuario, string strSessionCtrlTicket, string ip, string userAgent)
        {
            //inserir na t_SESSAO_HISTORICO
            TsessaoHistorico sessaoHist = new TsessaoHistorico
            {
                Usuario = usuario,
                SessionCtrlTicket = strSessionCtrlTicket,
                DtHrInicio = DateTime.Now,
                DtHrTermino = null,
                Loja = "",
                Modulo = Constantes.SESSION_CTRL_MODULO_APIUNIS,
                IP = ip,
                UserAgent = userAgent
            };

            dbgravacao.TsessaoHistoricos.Add(sessaoHist);
            dbgravacao.SaveChanges();
        }

        private void AtualizarTusuario(ContextoBdGravacao dbgravacao, string usuarioMaisuculas, string strSessionCtrlTicket)
        {
            var tusuario = (from u in dbgravacao.Tusuarios
                                  where u.Usuario.ToUpper() == usuarioMaisuculas
                                  select u).FirstOrDefault();
            tusuario.Dt_Ult_Acesso = DateTime.Now;
            tusuario.SessionCtrlDtHrLogon = DateTime.Now;
            tusuario.SessionCtrlModulo = Constantes.SESSION_CTRL_MODULO_APIUNIS;
            tusuario.SessionCtrlLoja = null;
            tusuario.SessionCtrlTicket = strSessionCtrlTicket;
            tusuario.SessionTokenModuloCentral = null;
            tusuario.DtHrSessionTokenModuloCentral = null;

            dbgravacao.Update(tusuario);
            dbgravacao.SaveChanges();
        }

        public async void FazerLogout(string tokenAcesso)
        {
            //todo: login afazer 

            /*
                        strSQL = "UPDATE t_USUARIO SET" & _
                                    " SessionCtrlTicket = NULL," & _
                                    " SessionCtrlLoja = NULL," & _
                                    " SessionCtrlModulo = NULL," & _
                                    " SessionCtrlDtHrLogon = NULL," & _
                                    " SessionTokenModuloCentral = NULL," & _
                                    " DtHrSessionTokenModuloCentral = NULL" & _
                                " WHERE" & _
                                    " usuario = '" & QuotedStr(Trim(Session("usuario_atual"))) & "'"
                        cn.Execute(strSQL)

                        strSQL = "UPDATE t_SESSAO_HISTORICO SET" & _
                                    " DtHrTermino = " & bd_formata_data_hora(Now) & _
                                 " WHERE" & _
                                    " usuario = '" & QuotedStr(Trim("" & Session("usuario_atual"))) & "'" & _
                                    " AND DtHrInicio >= " & bd_formata_data_hora(Now-1) & _
                                    " AND SessionCtrlTicket = '" & Trim(Session("SessionCtrlTicket")) & "'"
                        cn.Execute(strSQL)
            */
        }

    }
}
