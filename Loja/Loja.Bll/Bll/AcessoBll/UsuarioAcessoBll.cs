using InfraBanco.Modelos;
using Loja.Bll.Dto.AvisosDto;
using Loja.Bll.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace Loja.Bll.Bll.AcessoBll
{
    public class UsuarioAcessoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly ClienteBll.ClienteBll clienteBll;
        private readonly ILogger<UsuarioAcessoBll> logger;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;
        private readonly Avisos.AvisosBll avisosBll;

        public UsuarioAcessoBll(InfraBanco.ContextoBdProvider contextoProvider, ClienteBll.ClienteBll clienteBll, ILogger<UsuarioAcessoBll> logger,
            ILogger<UsuarioLogado> loggerUsuarioLogado, Avisos.AvisosBll avisosBll)
        {
            this.contextoProvider = contextoProvider;
            this.clienteBll = clienteBll;
            this.logger = logger;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
            this.avisosBll = avisosBll;
        }

        public class LoginUsuarioRetorno
        {
            public Tusuario? Tusuario;
            public bool Sucesso = false;
            public bool PrecisaAlterarSenha = false;
            public bool Usuario_bloqueado = false;
            public bool Loja_nao_existe = false;
            public bool Loja_sem_acesso = false;
        }
        public async Task LogoutUsuario(UsuarioLogado usuarioLogado)
        {
            logger.LogInformation($"LogoutUsuario: {usuarioLogado.Usuario_atual}");
            /*
                        strSQL = "UPDATE t_USUARIO SET" & _
                                    " SessionCtrlTicket = NULL," & _
                                    " SessionCtrlLoja = NULL," & _
                                    " SessionCtrlModulo = NULL," & _
                                    " SessionCtrlDtHrLogon = NULL," & _
                                    " SessionTokenModuloLoja = NULL," & _
                                    " DtHrSessionTokenModuloLoja = NULL" & _
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
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                var rsUsuario = await (from u in dbgravacao.Tusuarios
                                       where usuarioLogado.Usuario_atual == u.Usuario.Trim().ToUpper()
                                       select u).FirstOrDefaultAsync();
                if (rsUsuario == null)
                    return;

                var ticket = rsUsuario.SessionCtrlTicket;

                rsUsuario.SessionCtrlTicket = null;
                rsUsuario.SessionCtrlLoja = null;
                rsUsuario.SessionCtrlModulo = null;
                rsUsuario.SessionCtrlDtHrLogon = null;
                rsUsuario.SessionTokenModuloLoja = null;
                rsUsuario.DtHrSessionTokenModuloLoja = null;

                var rsHistorico = await (from u in dbgravacao.TsessaoHistoricos
                                         where usuarioLogado.Usuario_atual == u.Usuario.Trim().ToUpper()
                                         //nao testamos a data de início porque agora as sessões podem duram MUITO tempo
                                         && u.SessionCtrlTicket == ticket
                                         select u).FirstOrDefaultAsync();
                if (rsHistorico != null)
                {
                    rsHistorico.DtHrTermino = DateTime.Now;
                }

                await dbgravacao.SaveChangesAsync();
                dbgravacao.transacao.Commit();
            }
            logger.LogInformation($"LogoutUsuario finalizado: {usuarioLogado.Usuario_atual}");


            /*
             * este não fazemos (ainda):
             * 
             * 
                    '   LIMPA EVENTUAIS LOCKS REMANESCENTES NOS RELATÓRIOS
                        strSQL = "UPDATE tCRUP SET" & _
                                " locked = 0," & _
                                " cod_motivo_lock_released = " & CTRL_RELATORIO_CodMotivoLockReleased_SessaoEncerradaLoja & "," & _
                                " dt_hr_lock_released = getdate()" & _
                            " FROM t_CTRL_RELATORIO_USUARIO_X_PEDIDO tCRUP INNER JOIN t_CTRL_RELATORIO tCR ON (tCRUP.id_relatorio = tCR.id)" & _
                            " WHERE" & _
                                " (tCR.modulo = 'LOJA')" & _
                                " AND (tCRUP.usuario = '" & QuotedStr(Trim(Session("usuario_atual"))) & "')" & _
                                " AND (locked = 1)"
                        cn.Execute(strSQL)
            * */

        }
        public async Task<LoginUsuarioRetorno> LoginUsuario(string usuario, string senha, string loja, ISession httpContextSession,
        Configuracao configuracao, string remoteIpAddress, string userAgent)
        {
            logger.LogInformation($"LoginUsuario inicio: usuario={usuario} remoteIpAddress={remoteIpAddress} userAgent={userAgent}");

            var ret = new LoginUsuarioRetorno();
            ret.Sucesso = false;
            ret.Tusuario = null;

            //verificar se a loja existe
            var lojaExiste = (from c in contextoProvider.GetContextoLeitura().Tlojas
                              where c.Loja == loja
                              select c).Any();
            if (!lojaExiste)
            {
                logger.LogInformation($"LoginUsuario Loja_nao_existe: usuario={usuario} remoteIpAddress={remoteIpAddress} userAgent={userAgent}");
                ret.Loja_nao_existe = true;
                return ret;
            }


            //vamos ver se existe
            usuario = usuario.Trim().ToUpper();
            Tusuario rs = await UsuarioCarregar(usuario);
            if (rs == null)
            {
                logger.LogInformation($"LoginUsuario usuario nao existe: usuario={usuario} remoteIpAddress={remoteIpAddress} userAgent={userAgent}");
                return ret;
            }

            //'	TEM SENHA?
            if (string.IsNullOrWhiteSpace(rs.Datastamp))
                return ret;

            //'	ACESSO BLOQUEADO?
            if (rs.Bloqueado.HasValue && rs.Bloqueado == 0)
            {
                //nao bloqueado
            }
            else
            {
                //bloqueado
                logger.LogInformation($"LoginUsuario Usuario_bloqueado: usuario={usuario} remoteIpAddress={remoteIpAddress} userAgent={userAgent}");
                ret.Usuario_bloqueado = true;
                return ret;
            }

            var usuarioTemAcessoLoja = false;
            var Lista_operacoes_permitidas = await clienteBll.BuscaListaOperacoesPermitidas(usuario);
            if (UsuarioLogado.Operacao_permitida_estatica(Constantes.Constantes.OP_CEN_ACESSO_TODAS_LOJAS, Lista_operacoes_permitidas))
                usuarioTemAcessoLoja = true;
            /*
            if vendedor_loja then
                s = "SELECT loja FROM t_USUARIO_X_LOJA WHERE (usuario='" & usuario & "') AND (CONVERT(smallint,loja)=" & loja & ")"
                set rs2 = cn.Execute(s)
                if Err <> 0 then Response.Redirect("aviso.asp?id=" & ERR_FALHA_OPERACAO_BD)
                if Not rs2.Eof then cadastrado = true
                end if
                */
            if (rs.Vendedor_Loja != 0 && !usuarioTemAcessoLoja)
            {
                loja = loja ?? "";
                var existeLoja = await (from usuarioXloja in contextoProvider.GetContextoLeitura().TusuarioXLojas
                                        where usuarioXloja.Usuario == usuario
                                        && usuarioXloja.Loja == loja
                                        select 1).AnyAsync();
                if (existeLoja)
                    usuarioTemAcessoLoja = true;
                if (!existeLoja)
                {
                    //já voltamos daqui....
                    logger.LogInformation($"LoginUsuario Loja_sem_acesso: usuario={usuario} remoteIpAddress={remoteIpAddress} userAgent={userAgent}");
                    ret.Loja_sem_acesso = true;
                    return ret;
                }
            }


            if (!usuarioTemAcessoLoja)
            {
                logger.LogInformation($"LoginUsuario Loja_sem_acesso e não é Vendedor_Loja: usuario={usuario} remoteIpAddress={remoteIpAddress} userAgent={userAgent}");
                return ret;
            }

            /*
             * 
             * compara a senha
             * 
				s = Trim("" & rs("datastamp"))
				chave = gera_chave(FATOR_BD)
				decodifica_dado s, senha_real, chave
				if UCase(trim(senha_real)) <> UCase(trim(senha)) then 
					if senha_real <> "" then senha = ""
					end if
				end if
*/

            var senha_banco = UtilsGlobais.SenhaBll.DecodificaSenha(rs.Datastamp);
            senha = senha.ToUpper() ?? "";
            if (senha != senha_banco)
            {
                logger.LogInformation($"LoginUsuario senha != senha_banco: usuario={usuario} remoteIpAddress={remoteIpAddress} userAgent={userAgent}");
                return ret;
            }

            /*
             * log do login
             * 
                    s = "UPDATE t_USUARIO SET" & _
                            " dt_ult_acesso = " & bd_formata_data_hora(Now) & _
                            ", SessionCtrlDtHrLogon = " & bd_formata_data_hora(Session("DataHoraLogon")) & _
                            ", SessionCtrlModulo = '" & SESSION_CTRL_MODULO_LOJA & "'" & _
                            ", SessionCtrlLoja = '" & loja & "'" & _
                            ", SessionCtrlTicket = '" & strSessionCtrlTicket & "'" & _
                            ", SessionTokenModuloLoja = newid()" & _
                            ", DtHrSessionTokenModuloLoja = getdate()" & _
                        " WHERE" & _
                            " (usuario = '" & usuario & "')"
                    cn.Execute(s)

                    s = "INSERT INTO t_SESSAO_HISTORICO (" & _
                            "Usuario, " & _
                            "SessionCtrlTicket, " & _
                            "DtHrInicio, " & _
                            "Loja, " & _
                            "Modulo, " & _
                            "IP, " & _
                            "UserAgent" & _
                        ") VALUES (" & _
                            "'" & QuotedStr(usuario) & "'," & _
                            "'" & strSessionCtrlTicket & "'," & _
                            bd_formata_data_hora(Session("DataHoraLogon")) & "," & _
                            "'" & loja & "'," & _
                            "'" & SESSION_CTRL_MODULO_LOJA & "'," & _
                            "'" & QuotedStr(Trim("" & Request.ServerVariables("REMOTE_ADDR"))) & "'," & _
                            "'" & QuotedStr(Trim("" & Request.ServerVariables("HTTP_USER_AGENT"))) & "'" & _
                        ")"
                    cn.Execute(s)
            * 
             * 
             * */
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                var rsUsuario = await (from u in dbgravacao.Tusuarios
                                       where usuario == u.Usuario.Trim().ToUpper()
                                       select u).FirstOrDefaultAsync();
                if (rsUsuario == null)
                    return ret;

                //vamos usar um GUID para identificar esse login, e é o mesmo que usamos 
                var ticket = Guid.NewGuid();
                rsUsuario.Dt_Ult_Acesso = DateTime.Now;
                rsUsuario.SessionCtrlDtHrLogon = DateTime.Now;
                rsUsuario.SessionCtrlModulo = Constantes.Constantes.SESSION_CTRL_MODULO_LOJA;
                rsUsuario.SessionCtrlLoja = loja;
                rsUsuario.SessionCtrlTicket = ticket.ToString();
                rsUsuario.SessionTokenModuloLoja = ticket;
                rsUsuario.DtHrSessionTokenModuloLoja = DateTime.Now;

                dbgravacao.TsessaoHistoricos.Add(new TsessaoHistorico()
                {
                    Usuario = usuario,
                    SessionCtrlTicket = ticket.ToString(),
                    DtHrInicio = DateTime.Now,
                    Loja = loja,
                    Modulo = Constantes.Constantes.SESSION_CTRL_MODULO_LOJA,
                    IP = remoteIpAddress,
                    UserAgent = userAgent
                });
                await dbgravacao.SaveChangesAsync();
                dbgravacao.transacao.Commit();
            }


            ret.Tusuario = rs;
            ret.Sucesso = true;
            if (!rs.Dt_Ult_Alteracao_Senha.HasValue)
                ret.PrecisaAlterarSenha = true;

            //cria a session
            logger.LogInformation($"LoginUsuario CriarSessao: usuario={usuario} remoteIpAddress={remoteIpAddress} userAgent={userAgent}");
            UsuarioLogado.CriarSessao(loggerUsuarioLogado, usuario, httpContextSession, clienteBll, this, configuracao);
            logger.LogInformation($"LoginUsuario CriarSessao feito: usuario={usuario} remoteIpAddress={remoteIpAddress} userAgent={userAgent}");
            return ret;
        }

        public async Task<Tusuario> UsuarioCarregar(string usuario)
        {
            return await (from u in contextoProvider.GetContextoLeitura().Tusuarios
                          where usuario == u.Usuario.Trim().ToUpper()
                          select u).FirstOrDefaultAsync();
        }

        public class LojaPermtidaUsuario
        {
            public LojaPermtidaUsuario(string nome, string id)
            {
                Nome = nome;
                Id = id;
            }

            public string Nome { get; set; }
            public string Id { get; set; }
        }
        public async Task<List<LojaPermtidaUsuario>> Loja_troca_rapida_monta_itens_select_a_partir_banco(string strUsuario, string? id_default)
        {
            var ret = new List<LojaPermtidaUsuario>();

            if (string.IsNullOrWhiteSpace(strUsuario))
                return ret;

            var query = from usuarioXloja in contextoProvider.GetContextoLeitura().TusuarioXLojas
                        where usuarioXloja.Usuario == strUsuario
                        select new LojaPermtidaUsuario(usuarioXloja.Tloja.Nome, usuarioXloja.Tloja.Loja);
            var lista = await query.ToListAsync();

            //'	LEMBRE-SE: O USUÁRIO QUE TEM PERMISSÃO DE ACESSO A TODAS AS LOJAS PODE
            //'	ACESSAR UMA LOJA QUE NÃO ESTÁ CADASTRADA EM t_USUARIO_X_LOJA
            if (!string.IsNullOrWhiteSpace(id_default))
            {
                var lojaDefaultQuery = from loja in contextoProvider.GetContextoLeitura().Tlojas
                                       where loja.Loja == id_default
                                       select new LojaPermtidaUsuario(loja.Nome, loja.Loja);
                var lojaDefault = await lojaDefaultQuery.FirstOrDefaultAsync();
                if (lojaDefault != null)
                    lista.Add(lojaDefault);
            }
            return lista;
        }

        //ao chavear a loja atual, precisamos gravar na t_usuario
        //para que o sistema em ASP consiga pegar a alteração na loja
        //é o mesmo comportamento do verdinho
        public async Task Loja_troca_rapida_gravar_tusuario(string usuario, string loja)
        {
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                var rsUsuario = await (from u in dbgravacao.Tusuarios
                                       where usuario == u.Usuario.Trim().ToUpper()
                                       select u).FirstOrDefaultAsync();
                if (rsUsuario == null)
                    throw new ArgumentException($"Loja_troca_rapida_gravar_tusuario: Usuário {usuario} não encontrado no banco de dados");

                rsUsuario.SessionCtrlModulo = Constantes.Constantes.SESSION_CTRL_MODULO_LOJA;
                rsUsuario.SessionCtrlLoja = loja;

                await dbgravacao.SaveChangesAsync();
                dbgravacao.transacao.Commit();
            }
        }

        public async Task<string?> Loja_nome(string loja)
        {
            var query = from l in contextoProvider.GetContextoLeitura().Tlojas
                        where l.Loja == loja
                        select l.Nome;
            var lista = await query.FirstOrDefaultAsync();
            return lista;
        }

        public async Task<IEnumerable<AvisoDto>> BuscarAvisosNaoLidos(string loja, string usuario)
        {
            var ret = await avisosBll.BuscarAvisosNaoLidos(loja, usuario);
            return AvisoDto.AvisoDto_De_AvisoDados(ret.ToList());
        }

        public async Task<bool> RemoverAvisos(string loja, string usuario, List<string> itens)
        {
            return await avisosBll.RemoverAvisos(loja, usuario.ToUpper(), itens);
        }

        public async Task<bool> MarcarAvisoExibido(List<string> lst, string usuario, string loja)
        {
            return await avisosBll.MarcarAvisoExibido(lst, usuario.ToUpper(), loja);
        }

        public async Task<bool> AtualizarSessionCtrlTicket(UsuarioLogado usuarioLogado)
        {
            bool retorno = false;
            

            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                var rsUsuario = await (from u in dbgravacao.Tusuarios
                                       where usuarioLogado.Usuario_nome_atual.Trim().ToUpper() == u.Usuario.Trim().ToUpper()
                                       select u).FirstOrDefaultAsync();
                if (rsUsuario == null)
                    return false;

                var ticket = Guid.NewGuid();
                rsUsuario.Dt_Ult_Acesso = DateTime.Now;
                rsUsuario.SessionCtrlDtHrLogon = DateTime.Now;
                rsUsuario.SessionCtrlModulo = Constantes.Constantes.SESSION_CTRL_MODULO_LOJA;
                rsUsuario.SessionCtrlLoja = usuarioLogado.Loja_atual_id;
                rsUsuario.SessionCtrlTicket = ticket.ToString();
                rsUsuario.SessionTokenModuloLoja = ticket;
                rsUsuario.DtHrSessionTokenModuloLoja = DateTime.Now;


                dbgravacao.Update(rsUsuario);
                await dbgravacao.SaveChangesAsync();

                dbgravacao.transacao.Commit();


                usuarioLogado.LimparCacheInfsTusuario();

                retorno = true;
            }

            return retorno;
        }
    }
}

