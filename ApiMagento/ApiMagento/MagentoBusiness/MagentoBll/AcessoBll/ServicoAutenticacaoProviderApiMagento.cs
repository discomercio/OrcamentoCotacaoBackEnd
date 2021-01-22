using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraIdentity.ApiMagento;
using MagentoBusiness.MagentoDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MagentoBusiness.MagentoBll.AcessoBll
{
    public class ServicoAutenticacaoProviderApiMagento : IServicoAutenticacaoProviderApiMagento
    {
        private readonly ContextoBdProvider contextoProvider;

        public ServicoAutenticacaoProviderApiMagento(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        private static readonly string Erro_ERR_IDENTIFICACAO = "OS DADOS INFORMADOS NA IDENTIFICAÇÃO ESTÃO INCORRETOS.";

        //retorna null se nao exisitr (ou se a senha estiver errada)
        public async Task<UsuarioLoginApiMagento> ObterUsuarioApiMagento(string usuarioOriginal, string senha, string ip, string userAgent,
            string ApelidoPerfilLiberaAcessoApiMagento)
        {
            var ret = new UsuarioLoginApiMagento();

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
            var senha_banco_datastamp_decod = UtilsGlobais.Util.decodificaDado(usuario.Datastamp, Constantes.FATOR_CRIPTO);
            if (senha_banco_datastamp_decod.ToUpper() != senha.ToUpper())
            {
                ret.ListaErros.Add(Erro_ERR_IDENTIFICACAO + " (senha)");
                return ret;
            }


            //Verificamos se tem o perfil APIUNIS
            /*
			s = "SELECT Count(*) AS qtde FROM t_PERFIL_X_USUARIO INNER JOIN t_PERFIL ON t_PERFIL_X_USUARIO.id_perfil=t_PERFIL.id" & _
				" INNER JOIN t_PERFIL_ITEM ON t_PERFIL.id=t_PERFIL_ITEM.id_perfil" & _
				" INNER JOIN t_OPERACAO ON t_PERFIL_ITEM.id_operacao=t_OPERACAO.id" & _
				" WHERE (t_PERFIL_X_USUARIO.usuario='" & usuario & "')" & _
				" AND (t_OPERACAO.modulo='" & COD_OP_MODULO_CENTRAL & "')"
							*/
            var db = contextoProvider.GetContextoLeitura();
            var perfil = await (from p in db.Tperfils
                                join pu in db.TperfilUsuarios on p.Id equals pu.Id_perfil
                                where p.Apelido.ToUpper() == ApelidoPerfilLiberaAcessoApiMagento.ToUpper() &&
                                      pu.Usuario.ToUpper() == usuarioMaisuculas && p.St_inativo == 0
                                select p.Id).FirstOrDefaultAsync();
            if (perfil == null)
            {
                ret.ListaErros.Add("USUÁRIO NÃO TEM ACESSO AO PERFIL APIMAGENTO");
                return ret;
            }

            /*
             * vamos ignorar a necessidade de alterar a senha
            //if IsNull(dt_ult_alteracao_senha) then Response.Redirect("senha.asp" & "?" & MontaCampoQueryStringSessionCtrlInfo(Session("SessionCtrlInfo")))
            */


            //verificar com HAmilton se este formato está OK
            var strSessionCtrlTicket = $"{usuarioMaisuculas} - {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}";
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
                        SessaoSeguinteModulo = Constantes.SESSION_CTRL_MODULO_APIMAGENTO
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
                Modulo = Constantes.SESSION_CTRL_MODULO_APIMAGENTO,
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
            tusuario.SessionCtrlModulo = Constantes.SESSION_CTRL_MODULO_APIMAGENTO;
            tusuario.SessionCtrlLoja = null;
            tusuario.SessionCtrlTicket = strSessionCtrlTicket;
            tusuario.SessionTokenModuloCentral = null;
            tusuario.DtHrSessionTokenModuloCentral = null;

            dbgravacao.Update(tusuario);
            dbgravacao.SaveChanges();
        }

        public async Task FazerLogout(string usuario, LogoutResultadoMagentoDto logoutResultadoUnisDto)
        {
            logoutResultadoUnisDto.ListaErros.RemoveAll(r => true);

            //atualizar dados
            using var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing();
            var tusuario = await (from u in dbgravacao.Tusuarios
                                  where u.Usuario.ToUpper() == usuario.ToUpper()
                                  select u).FirstOrDefaultAsync();
            if (tusuario == null)
            {
                logoutResultadoUnisDto.ListaErros.Add($"Usuário não encontrado: {usuario}");
                return;
            }

            var sessionCtrlTicketAnterior = tusuario.SessionCtrlTicket;
            tusuario.SessionCtrlTicket = null;
            tusuario.SessionCtrlLoja = null;
            tusuario.SessionCtrlModulo = null;
            tusuario.SessionCtrlDtHrLogon = null;
            dbgravacao.Update(tusuario);
            dbgravacao.SaveChanges();

            var tsessaoHistorico = await (from h in dbgravacao.TsessaoHistoricos
                                          where h.Usuario.ToUpper() == usuario.ToUpper()
                                          && h.DtHrInicio >= DateTime.Now.AddDays(-1)
                                          && h.SessionCtrlTicket == sessionCtrlTicketAnterior
                                          orderby h.DtHrInicio descending
                                          select h).FirstOrDefaultAsync();
            if (tsessaoHistorico != null)
            {
                tsessaoHistorico.DtHrTermino = DateTime.Now;
                dbgravacao.Update(tsessaoHistorico);
                dbgravacao.SaveChanges();
            }

            dbgravacao.transacao.Commit();
        }
    }
}
