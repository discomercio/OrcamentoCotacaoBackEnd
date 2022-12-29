using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraIdentity;
using Microsoft.EntityFrameworkCore;
using OrcamentoCotacaoBusiness.Models.Response;
using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtilsGlobais;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class AcessoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public AcessoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public UsuarioLogin ValidarUsuario(
            string login, 
            string senha_digitada_datastamp, 
            bool somenteValidar,
            string ip,
            out string msgErro)
        {
            login = login.ToUpper().Trim();
            msgErro = "";
            var usuarioLogin = new UsuarioLogin();

            using (var db = contextoProvider.GetContextoLeitura())
            {
                //Validar o dados no bd
                var dados = from c in db.Tusuario
                            where c.Usuario == login
                            select new UsuarioLogin
                            {
                                Nome = c.Nome,
                                Senha = c.Senha,
                                Email = c.Email,
                                Datastamp = c.Datastamp,
                                Dt_Ult_Alteracao_Senha = c.Dt_Ult_Alteracao_Senha,
                                Bloqueado = c.Bloqueado.HasValue ? (c.Bloqueado.Value == 1 ? true : false) : false,
                                Loja = c.Loja,
                                TipoUsuario = (int)Constantes.TipoUsuario.VENDEDOR,
                                Id = c.Id,
                                StLoginBloqueadoAutomatico = c.StLoginBloqueadoAutomatico
                            };

                var t = dados.FirstOrDefault();

                //se o apelido nao existe
                if (t == null)
                {
                    /*Busca de parceiros*/
                    dados = from c in db.TorcamentistaEindicador
                            where c.Apelido == login
                            select new UsuarioLogin
                            {
                                Nome = c.Razao_Social_Nome,
                                Senha = c.Senha,
                                Email = "",
                                Datastamp = c.Datastamp,
                                Dt_Ult_Alteracao_Senha = c.Dt_Ult_Alteracao_Senha,
                                Bloqueado = c.Status != "A",
                                AcessoHabilitado = c.Hab_Acesso_Sistema == 1,
                                Loja = c.Loja,
                                TipoUsuario = (int)Constantes.TipoUsuario.PARCEIRO,
                                Id = c.IdIndicador,
                                StLoginBloqueadoAutomatico = c.StLoginBloqueadoAutomatico
                            };
                    t = dados.FirstOrDefault();

                    /*Busca de parceiros*/
                    if (t == null)
                    {
                        /*Busca de vendedores de parceiros*/
                        dados = from c in db.TorcamentistaEindicadorVendedor
                                join d in db.TorcamentistaEindicador on c.IdIndicador equals d.IdIndicador
                                where c.Email == login
                                select new UsuarioLogin
                                {
                                    Nome = c.Nome,
                                    Email = c.Email,
                                    Datastamp = c.Datastamp,
                                    Dt_Ult_Alteracao_Senha = c.DataUltimaAlteracao,
                                    Bloqueado = d.Status != "A" || !c.Ativo,
                                    AcessoHabilitado = d.Hab_Acesso_Sistema == 1,
                                    Loja = c.Loja,
                                    TipoUsuario = (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO,
                                    Id = c.Id,
                                    StLoginBloqueadoAutomatico = c.StLoginBloqueadoAutomatico
                                };
                        t = dados.FirstOrDefault();
                    }
                }
                else
                {
                    t.AcessoHabilitado = true;
                }
                if (t == null)
                {
                    RegistrarTentativasLogin(null, null, login, ip, Constantes.ERR_USUARIO_NAO_CADASTRADO);
                    msgErro = Constantes.ERR_USUARIO_NAO_CADASTRADO;
                    return null;
                }

                if (t.Datastamp == "")
                {
                    RegistrarTentativasLogin(t.Id, t.TipoUsuario.Value, login, ip, Constantes.ERR_USUARIO_BLOQUEADO);
                    msgErro = Constantes.ERR_USUARIO_BLOQUEADO;
                    return null;
                }

                if (t.Bloqueado)
                {
                    RegistrarTentativasLogin(t.Id, t.TipoUsuario.Value, login, ip, Constantes.ERR_USUARIO_INATIVO);
                    msgErro = Constantes.ERR_USUARIO_INATIVO;
                    return null;
                }

                if (!t.AcessoHabilitado)
                {
                    RegistrarTentativasLogin(t.Id, t.TipoUsuario.Value, login, ip, Constantes.ERR_USUARIO_BLOQUEADO);
                    msgErro = Constantes.ERR_USUARIO_BLOQUEADO;
                    return null;
                }

                if (t.StLoginBloqueadoAutomatico)
                {
                    RegistrarTentativasLogin(t.Id, t.TipoUsuario.Value, login, ip, Constantes.ERR_USUARIO_BLOQUEADO_AUTOMATICO);
                    msgErro = Constantes.ERR_USUARIO_BLOQUEADO;
                    return null;
                }

                if (!somenteValidar)
                {
                    if (senha_digitada_datastamp != t.Datastamp)
                    {
                        RegistrarTentativasLogin(t.Id, t.TipoUsuario.Value, login, ip, Constantes.ERR_SENHA_INVALIDA);
                        msgErro = Constantes.ERR_SENHA_INVALIDA;
                        return null;
                    }

                    RegistrarTentativasLogin(t.Id, t.TipoUsuario.Value, login, ip, string.Empty);

                    //Fazer Update no bd
                    using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                    {
                        switch (t.TipoUsuario)
                        {
                            case (int)Constantes.TipoUsuario.GESTOR:
                                Tusuario usuario = dbgravacao.Tusuario
                                .Where(c => c.Usuario == login && c.Datastamp == senha_digitada_datastamp).FirstOrDefault();
                                usuario.Dt_Ult_Acesso = DateTime.Now;
                                break;
                            case (int)Constantes.TipoUsuario.PARCEIRO:
                                TorcamentistaEindicador parceiro = dbgravacao.TorcamentistaEindicador
                                .Where(c => c.Apelido == login && c.Datastamp == senha_digitada_datastamp).FirstOrDefault();
                                parceiro.Dt_Ult_Acesso = DateTime.Now;
                                break;
                            //TODO: Incluir campo de data ultimo login na tabela de vendedor do parceiro
                            //case (int)Enums.Enums.TipoUsuario.VENDEDOR_DO_PARCEIRO:
                            //    TorcamentistaEIndicadorVendedor vendedorParceiro = await dbgravacao.TorcamentistaEIndicadorVendedor
                            //    .Where(c => c.Email == login && c.Senha == senha_digitada_decod).FirstOrDefaultAsync();
                            //    vendedorParceiro.DataUltimoLogin = DateTime.Now;
                            //    break;
                            default:
                                break;
                        }
                        //dbgravacao.SaveChanges();
                        //dbgravacao.transacao.Commit();
                    }

                    /* RETORNA E EXIGE A TROCA DE SENHA
                    if (t.Dt_Ult_Alteracao_Senha == null)
                    {
                        //Senha expirada, precisa mandar alguma valor de senha expirada
                        //coloquei o valor "4" para saber quando a senha esta expirada
                        msgErro = Constantes.ERR_SENHA_EXPIRADA;
                        return null;// await Task.FromResult("4");
                    }*/
                }

                return t;
            }
        }

        public async Task<List<TusuarioXLoja>> BuscarLojaUsuario(string apelido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await (from ul in db.TusuarioXLoja
                              join l in db.Tloja on ul.Loja equals l.Loja
                              join n in db.TcfgUnidadeNegocio on l.Unidade_Negocio equals n.Sigla
                              join p in db.TcfgUnidadeNegocioParametro on n.Id equals p.IdCfgUnidadeNegocio
                              where ul.Usuario == apelido && p.IdCfgParametro == 13 && p.Valor == "1"
                              select ul)
                            .ToListAsync();
            }
        }

        public async Task<string> Buscar_unidade_negocio(List<TusuarioXLoja> lojas)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var unidade_negocio = await ((from c in db.Tloja
                                              join l in lojas on c.Loja equals l.Loja
                                              select c.Unidade_Negocio).ToListAsync());

                return string.Join(',', unidade_negocio);
            }
        }

        public async Task GravarSessaoComTransacao(string ip, string apelido, string userAgent)
        {
            List<TusuarioXLoja> loja = await BuscarLojaUsuario(apelido);

            //inserir na t_SESSAO_HISTORICO
            TsessaoHistorico sessaoHist = new TsessaoHistorico
            {
                Usuario = apelido,
                SessionCtrlTicket = "",
                DtHrInicio = DateTime.Now,
                DtHrTermino = null,
                Loja = string.Join(",", loja.Select(x => x.Loja)),
                Modulo = "ORCTO",
                IP = ip,
                UserAgent = userAgent
            };

            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                dbgravacao.TsessaoHistorico.Add(sessaoHist);
                await dbgravacao.SaveChangesAsync();
                dbgravacao.transacao.Commit();
            }
        }

        public string geraChave()
        {
            const int fator = 1209;
            const int cod_min = 35;
            const int cod_max = 96;
            const int tamanhoChave = 128;

            string chave = "";

            for (int i = 1; i < tamanhoChave; i++)
            {
                int k = (cod_max - cod_min) + 1;
                k *= fator;
                k = (k * i) + cod_min;
                k %= 128;
                chave += (char)k;
            }

            return chave;
        }

        public async Task FazerLogout(string apelido)
        {
            //proteção adicional, só por proteger mesmo
            if (string.IsNullOrEmpty(apelido))
                return;

            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var sessaoHistTask = (from c in dbgravacao.TsessaoHistorico
                                      where c.Usuario == apelido
                                      orderby c.DtHrInicio descending
                                      select c).FirstOrDefaultAsync();
                TsessaoHistorico sessaoHist = await sessaoHistTask;
                sessaoHist.DtHrTermino = DateTime.Now;

                dbgravacao.TsessaoHistorico.Update(sessaoHist);
                await dbgravacao.SaveChangesAsync();
                dbgravacao.transacao.Commit();
            }
        }

        public async Task<string> AlterarSenha(AlterarSenhaDto alterarSenhaDto)
        {
            //trazer as senha codificadas para ser decodificada aqui
            string retorno = "";
            string senha_aleatoria = "";

            if (!string.IsNullOrEmpty(alterarSenhaDto.Senha) && !string.IsNullOrEmpty(alterarSenhaDto.SenhaNova) &&
                !string.IsNullOrEmpty(alterarSenhaDto.SenhaNovaConfirma))
            {
                string senha = Util.decodificaDado(alterarSenhaDto.Senha, Constantes.FATOR_CRIPTO).ToUpper().Trim();
                string senha_nova = Util.decodificaDado(alterarSenhaDto.SenhaNova, Constantes.FATOR_CRIPTO).ToUpper().Trim();
                string senha_nova_confirma = Util.decodificaDado(alterarSenhaDto.SenhaNovaConfirma,
                    Constantes.FATOR_CRIPTO).ToUpper().Trim();

                if (senha_nova.Length < 5)
                {
                    return retorno = "A nova senha deve possuir no mínimo 5 caracteres.";
                }
                if (senha_nova_confirma.Length < 5)
                {
                    return retorno = "A confirmação da nova senha deve possuir no mínimo 5 caracteres.";
                }
                if (senha_nova != senha_nova_confirma)
                {
                    return retorno = "A confirmação da nova senha está incorreta.";
                }
                if (senha == senha_nova)
                {
                    return retorno = "A nova senha deve ser diferente da senha atual.";
                }
                if (senha_nova == alterarSenhaDto.Apelido.ToUpper().Trim())
                {
                    return retorno = "A nova senha não pode ser igual ao identificador do usuário!";
                }
                //valida as credenciais
                string validou;
                var usuario = ValidarUsuario(alterarSenhaDto.Apelido, Util.codificaDado(senha, false), false, string.Empty, out validou);

                if (validou == Constantes.ERR_IDENTIFICACAO_LOJA)
                {
                    return retorno = "Erro na identificação da loja.";
                }

                if (validou == Constantes.ERR_USUARIO_BLOQUEADO)
                {
                    return retorno = "Usuário bloqueado.";
                }

                if (validou == null)
                {
                    return retorno = "Senha atual incorreta.";
                }

                //vamos alterar a senha na base de dados
                using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR))
                {
                    TorcamentistaEindicador orcamentista = await (from c in dbgravacao.TorcamentistaEindicador
                                                                  where c.Apelido == alterarSenhaDto.Apelido.ToUpper().Trim()
                                                                  select c).FirstOrDefaultAsync();

                    senha_aleatoria = Util.GerarSenhaAleatoria();

                    string senha_codificada = Util.codificaDado(senha_nova, false);

                    if (string.IsNullOrEmpty(senha_codificada))
                        throw new ArgumentException("Falha na codificação de senha.");

                    orcamentista.Datastamp = senha_codificada;
                    orcamentista.Dt_Ult_Alteracao_Senha = DateTime.Now.Date;
                    orcamentista.Dt_Ult_Atualizacao = DateTime.Now;
                    orcamentista.Senha = senha_aleatoria;

                    //vamos gravar o log
                    if (Util.GravaLog(dbgravacao, alterarSenhaDto.Apelido, orcamentista.Loja, "", "",
                        Constantes.OP_LOG_SENHA_ALTERACAO, "SENHA ALTERADA PELO ORÇAMENTISTA"))
                    {
                        dbgravacao.Update(orcamentista);
                        await dbgravacao.SaveChangesAsync();

                        dbgravacao.transacao.Commit();
                    }
                }
            }

            return retorno;
        }

        public async Task<AtualizarSenhaResponseViewModel> AtualizarSenhaAsync(AtualizarSenhaDto atualizarSenhaDto)
        {
            if (string.IsNullOrEmpty(atualizarSenhaDto.Apelido)
               || string.IsNullOrEmpty(atualizarSenhaDto.Senha)
               || string.IsNullOrEmpty(atualizarSenhaDto.NovaSenha)
               || string.IsNullOrEmpty(atualizarSenhaDto.ConfirmacaoSenha)
               )
            {
                return new AtualizarSenhaResponseViewModel(false, "Favor preencher todos os campos.");
            }

            var senha_digitada = atualizarSenhaDto.Senha;
            var senha = Util.decodificaDado(atualizarSenhaDto.Senha, Constantes.FATOR_CRIPTO).Trim();
            var senha_nova = Util.decodificaDado(atualizarSenhaDto.NovaSenha, Constantes.FATOR_CRIPTO).Trim();
            var senha_nova_confirma = Util.decodificaDado(atualizarSenhaDto.ConfirmacaoSenha, Constantes.FATOR_CRIPTO).Trim();

            var atualizacaoSenhaMensagem = SenhaValida(
                atualizarSenhaDto.Apelido,
                senha,
                senha_nova,
                senha_nova_confirma);

            if (atualizacaoSenhaMensagem.Length > 0)
            {
                return new AtualizarSenhaResponseViewModel(false, atualizacaoSenhaMensagem);
            }

            var senha_codificada = Util.codificaDado(senha_nova, false);

            if (string.IsNullOrEmpty(senha_codificada))
                throw new ArgumentException("Falha na codificação de senha.");

            if (atualizarSenhaDto.TipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
            {
                await AtualizarSenhaVendedorAsync(atualizarSenhaDto.Apelido, senha_codificada, senha_digitada);
            }

            if (atualizarSenhaDto.TipoUsuario == (int)Constantes.TipoUsuario.PARCEIRO)
            {
                await AtualizarSenhaParceiroAsync(atualizarSenhaDto.Apelido, senha_codificada, senha_digitada);
            }

            if (atualizarSenhaDto.TipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
            {
                await AtualizarSenhaVendedoParceiro(atualizarSenhaDto.Apelido, senha_codificada, senha_digitada);
            }

            return new AtualizarSenhaResponseViewModel(true, "Alteração de senha realizada com sucesso.");
        }

        public async Task<ExpiracaoSenhaResponseViewModel> VerificarExpiracao(AtualizarSenhaDto atualizarSenhaDto)
        {

            using (var db = contextoProvider.GetContextoLeitura())
            {


                bool expirada = false;


                if (atualizarSenhaDto.TipoUsuario == 1)
                {
                    var usuario = await (from u in db.Tusuario
                                         where u.Usuario == atualizarSenhaDto.Apelido.ToUpper().Trim()
                                         select u).FirstOrDefaultAsync();


                    if (usuario.Dt_Ult_Alteracao_Senha == null) expirada = true;

                }
                else if (atualizarSenhaDto.TipoUsuario == 2)
                {
                    var orcamentista = await (from u in db.TorcamentistaEindicador
                                              where u.Apelido == atualizarSenhaDto.Apelido.ToUpper().Trim()
                                              select u).FirstOrDefaultAsync();

                    if (orcamentista.Dt_Ult_Alteracao_Senha == null) expirada = true;

                }
                else
                {
                    var vendedorParceiro = await (from u in db.TorcamentistaEindicadorVendedor
                                                  where u.Email == atualizarSenhaDto.Apelido.ToUpper().Trim()
                                                  select u).FirstOrDefaultAsync();

                    if (vendedorParceiro.DataUltimaAlteracaoSenha == null) expirada = true;

                }

                return new ExpiracaoSenhaResponseViewModel(expirada, "Senha expirada ou primeiro acesso.");

            }

        }

        private string SenhaValida(
            string usuario,
            string senha,
            string senha_nova,
            string senha_nova_confirma)
        {
            var regex = new Regex(@"^(?=.*[0-9])(?=.*[a-zA-Z])[a-zA-Z0-9]{8,}$");

            if (!regex.IsMatch(senha_nova))
            {
                return "A senha deve ter de 8 a 15 caracteres com pelo menos uma letra e um número.";
            }

            if (!regex.IsMatch(senha_nova_confirma))
            {
                return "A confirmação da nova senha deve ter de 8 a 15 caracteres com pelo menos uma letra e um número.";
            }

            if (senha_nova != senha_nova_confirma)
            {
                return "A confirmação da nova senha está incorreta.";
            }

            if (senha == senha_nova)
            {
                return "A nova senha deve ser diferente da senha atual.";
            }

            if (senha_nova == usuario.ToUpper().Trim())
            {
                return "A nova senha não pode ser igual ao identificador do usuário!";
            }

            return string.Empty;
        }

        private async Task AtualizarSenhaVendedorAsync(string apelido, string senha, string senha_digitada)
        {
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR))
            {
                var usuario = await (from u in dbgravacao.Tusuario
                                     where u.Usuario == apelido.ToUpper().Trim()
                                     select u).FirstOrDefaultAsync();


                if (usuario.Datastamp != senha_digitada)
                {
                    throw new ArgumentException("A senha digitada não é a mesma cadastrada no sistema!");
                }

                usuario.Senha = Util.GerarSenhaAleatoria();
                usuario.Datastamp = senha;
                usuario.Dt_Ult_Alteracao_Senha = DateTime.Now.Date;
                usuario.Dt_Ult_Atualizacao = DateTime.Now;

                // Campo da tabela t_log hoje comporta até 20 e no futuro iremos utilizar outra tabela de log v2
                if (apelido.Length > 20)
                {
                    apelido = apelido.Substring(0, 20);
                }

                var novoLog = Util.GravaLog(
                                            dbgravacao,
                                            apelido,
                                            usuario.Loja,
                                            "",
                                            "",
                                            Constantes.OP_LOG_SENHA_ALTERACAO,
                                            "SENHA ALTERADA PELO USUARIO");

                if (novoLog)
                {
                    dbgravacao.Update(usuario);
                    await dbgravacao.SaveChangesAsync();

                    dbgravacao.transacao.Commit();
                }
            }
        }

        private async Task AtualizarSenhaParceiroAsync(string apelido, string senha, string senha_digitada)
        {
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR))
            {
                var orcamentista = await (from u in dbgravacao.TorcamentistaEindicador
                                          where u.Apelido == apelido.ToUpper().Trim()
                                          select u).FirstOrDefaultAsync();

                if (orcamentista.Datastamp != senha_digitada)
                {
                    throw new ArgumentException("A senha digitada não é a mesma cadastrada no sistema!");
                }


                orcamentista.Senha = Util.GerarSenhaAleatoria();
                orcamentista.Datastamp = senha;
                orcamentista.Dt_Ult_Alteracao_Senha = DateTime.Now.Date;
                orcamentista.Dt_Ult_Atualizacao = DateTime.Now;

                // Campo da tabela t_log hoje comporta até 20 e no futuro iremos utilizar outra tabela de log v2
                if (apelido.Length > 20)
                {
                    apelido = apelido.Substring(0, 20);
                }

                var novoLog = Util.GravaLog(
                                            dbgravacao,
                                            apelido,
                                            orcamentista.Loja,
                                            "",
                                            "",
                                            Constantes.OP_LOG_SENHA_ALTERACAO,
                                            "SENHA ALTERADA PELO ORÇAMENTISTA");

                if (novoLog)
                {
                    dbgravacao.Update(orcamentista);
                    await dbgravacao.SaveChangesAsync();

                    dbgravacao.transacao.Commit();
                }
            }
        }

        private async Task<string> AtualizarSenhaVendedoParceiro(string apelido, string senha, string senha_digitada)
        {
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR))
            {
                var vendedorParceiro = await (from u in dbgravacao.TorcamentistaEIndicadorVendedor
                                              where u.Email == apelido.ToUpper().Trim()
                                              select u).FirstOrDefaultAsync();

                if (vendedorParceiro.Datastamp != senha_digitada)
                {
                    throw new ArgumentException("A senha digitada não é a mesma cadastrada no sistema!");
                }

                vendedorParceiro.Senha = Util.GerarSenhaAleatoria();
                vendedorParceiro.Datastamp = senha;
                vendedorParceiro.DataUltimaAlteracao = DateTime.Now;
                vendedorParceiro.DataUltimaAlteracaoSenha = DateTime.Now;


                // Campo da tabela t_log hoje comporta até 20 e no futuro iremos utilizar outra tabela de log v2
                if (apelido.Length > 20)
                {
                    apelido = apelido.Substring(0, 20);
                }

                var novoLog = Util.GravaLog(
                                            dbgravacao,
                                            apelido,
                                            vendedorParceiro.Loja,
                                            "",
                                            "",
                                            Constantes.OP_LOG_SENHA_ALTERACAO,
                                            "SENHA ALTERADA PELO VENDEDOR DO PARCEIRO");



                if (novoLog)
                {
                    dbgravacao.Update(vendedorParceiro);
                    await dbgravacao.SaveChangesAsync();

                    dbgravacao.transacao.Commit();
                }
            }

            return string.Empty;
        }

        private async Task RegistrarTentativasLogin(
            int? idUsuario,
            int? idTipoUsuario,
            string login,
            string ip,
            string mensagemErro)
        {
            var notificarUserAdmin = false;
            var sistemaResponsavel = 6;
            var mensagemMotivo = string.Empty;

            if (mensagemErro == Constantes.ERR_SENHA_INVALIDA)
            {
                mensagemMotivo = "'001' - Senha inválida";
                mensagemMotivo = "001";
            }

            if (mensagemErro == Constantes.ERR_USUARIO_BLOQUEADO)
            {
                mensagemMotivo = "'002' - Usuário encontra - se bloqueado manualmente";
                mensagemMotivo = "002";
            }

            if (mensagemErro == Constantes.ERR_USUARIO_BLOQUEADO_AUTOMATICO)
            {
                mensagemMotivo = "'003' - Usuário encontra - se bloqueado automaticamente pelo controle de login";
                mensagemMotivo = "003";
            }

            if (mensagemErro == Constantes.ERR_USUARIO_NAO_CADASTRADO)
            {
                mensagemMotivo = "'004' - Usuário não cadastrado";
                mensagemMotivo = "004";
            }

            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR))
            {
                var tloginHistorico = new TloginHistorico()
                {
                    DataHora = DateTime.Now,
                    IdTipoUsuarioContexto = idTipoUsuario,
                    IdUsuario = idUsuario,
                    Ip = ip,
                    sistema_responsavel = sistemaResponsavel,
                    StSucesso = string.IsNullOrEmpty(mensagemErro) ? true : false,
                    Login = login,
                    Motivo = mensagemMotivo
                };

                dbgravacao.Add(tloginHistorico);

                if (idUsuario.HasValue && idTipoUsuario.HasValue)
                {

                    var parametroBloqueio = await (from b in dbgravacao.Tparametros
                                                   where b.Id == "MAX_TENTATIVAS_LOGIN"
                                                   select b).FirstOrDefaultAsync();

                    if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR)
                    {
                        var usuario = await (from c in dbgravacao.Tusuario
                                             where c.Id == idUsuario
                                             select c).FirstOrDefaultAsync();

                        if (mensagemErro == Constantes.ERR_SENHA_INVALIDA)
                        {
                            usuario.QtdeConsecutivaFalhaLogin = usuario.QtdeConsecutivaFalhaLogin + 1;

                            if (parametroBloqueio.Campo_inteiro == usuario.QtdeConsecutivaFalhaLogin)
                            {
                                usuario.StLoginBloqueadoAutomatico = true;
                                usuario.DataHoraBloqueadoAutomatico = DateTime.Now;
                                usuario.EnderecoIpBloqueadoAutomatico = ip;
                                notificarUserAdmin = true;
                            }
                        }
                        else
                        {
                            usuario.QtdeConsecutivaFalhaLogin = 0;
                        }

                        dbgravacao.Update(usuario);
                    }

                    if (idTipoUsuario == (int)Constantes.TipoUsuario.PARCEIRO)
                    {
                        var parceiro = await (from c in dbgravacao.TorcamentistaEindicador
                                              where c.IdIndicador == idUsuario
                                              select c).FirstOrDefaultAsync();

                        if (mensagemErro == Constantes.ERR_SENHA_INVALIDA)
                        {
                            parceiro.QtdeConsecutivaFalhaLogin = parceiro.QtdeConsecutivaFalhaLogin + 1;

                            if (parametroBloqueio.Campo_inteiro == parceiro.QtdeConsecutivaFalhaLogin)
                            {
                                parceiro.StLoginBloqueadoAutomatico = true;
                                parceiro.DataHoraBloqueadoAutomatico = DateTime.Now;
                                parceiro.EnderecoIpBloqueadoAutomatico = ip;
                                notificarUserAdmin = true;
                            }
                        }
                        else
                        {
                            parceiro.QtdeConsecutivaFalhaLogin = 0;
                        }

                        dbgravacao.Update(parceiro);
                    }

                    if (idTipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
                    {
                        var parceiroParceiro = await (from c in dbgravacao.TorcamentistaEIndicadorVendedor
                                                      where c.Id == idUsuario
                                                      select c).FirstOrDefaultAsync();

                        if (mensagemErro == Constantes.ERR_SENHA_INVALIDA)
                        {
                            parceiroParceiro.QtdeConsecutivaFalhaLogin = parceiroParceiro.QtdeConsecutivaFalhaLogin + 1;

                            if (parametroBloqueio.Campo_inteiro == parceiroParceiro.QtdeConsecutivaFalhaLogin)
                            {
                                parceiroParceiro.StLoginBloqueadoAutomatico = true;
                                parceiroParceiro.DataHoraBloqueadoAutomatico = DateTime.Now;
                                parceiroParceiro.EnderecoIpBloqueadoAutomatico = ip;
                                notificarUserAdmin = true;
                            }
                        }
                        else
                        {
                            parceiroParceiro.QtdeConsecutivaFalhaLogin = 0;
                        }

                        dbgravacao.Update(parceiroParceiro);
                    }

                    if (notificarUserAdmin)
                    {
                        var parametroRemetente = await (from b in dbgravacao.Tparametros
                                                        where b.Id == "EmailRemetenteAlertaLoginBloqueadoAutomatico"
                                                        select b).FirstOrDefaultAsync();

                        var parametroDestinatario = await (from b in dbgravacao.Tparametros
                                                           where b.Id == "EmailDestinatarioAlertaLoginBloqueadoAutomatico"
                                                           select b).FirstOrDefaultAsync();

                        var parametroAssunto = await (from b in dbgravacao.Tparametros
                                                      where b.Id == "SubjectEmailAlertaLoginBloqueadoAutomatico"
                                                      select b).FirstOrDefaultAsync();

                        var parametroMensagem = await (from b in dbgravacao.Tparametros
                                                       where b.Id == "BodyEmailAlertaLoginBloqueadoAutomatico"
                                                       select b).FirstOrDefaultAsync();

                        var dataBloqueio = DateTime.Now.ToString();

                        parametroAssunto.Campo_texto = parametroAssunto.Campo_texto
                            .Replace("[AMBIENTE]", "TESTE")
                            .Replace("[LOGIN_USUARIO]", login)
                            .Replace("[DATA_HORA_BLOQUEIO]", dataBloqueio);

                        parametroMensagem.Campo_texto = parametroMensagem.Campo_texto
                            .Replace("[AMBIENTE]", "TESTE")
                            .Replace("[LOGIN_USUARIO]", login)
                            .Replace("[IdTipoUsuarioContexto]", idTipoUsuario.ToString())
                            .Replace("[IdUsuario]", idUsuario.ToString())
                            .Replace("[IP]", ip)
                            .Replace("[DATA_HORA_BLOQUEIO]", dataBloqueio)
                            .Replace("[MAX_TENTATIVAS_LOGIN]", parametroBloqueio.Campo_inteiro.ToString());

                        if (parametroRemetente != null)
                        {
                            var emailSndsvcRemetente = await (from b in dbgravacao.TemailLsndsvcRemetente
                                                              where b.email_remetente == parametroRemetente.Campo_texto
                                                              && b.st_envio_mensagem_habilitado == 1
                                                              select b).FirstOrDefaultAsync();

                            var ultimoIdTemailSndsvcMensagem = await (from b in dbgravacao.TemailSndsvcMensagem.OrderByDescending(p => p.id)
                                                                      select b.id).FirstOrDefaultAsync();

                            var tEmailSndsvcMensagem = new TemailSndsvcMensagem()
                            {
                                id = (ultimoIdTemailSndsvcMensagem + 1),
                                id_remetente = emailSndsvcRemetente.id,
                                dt_cadastro = DateTime.Now.Date,
                                dt_hr_cadastro = DateTime.Now,
                                assunto = parametroAssunto.Campo_texto,
                                corpo_mensagem = parametroMensagem.Campo_texto,
                                destinatario_To = parametroDestinatario.Campo_texto,
                                destinatario_Cc = null,
                                destinatario_CCo = null,
                                dt_hr_agendamento_envio = null,
                                qtde_tentativas_realizadas = 0,
                                st_enviado_sucesso = 0,
                                dt_hr_enviado_sucesso = null,
                                st_falhou_em_definitivo = 0,
                                dt_hr_falhou_em_definitivo = null,
                                resultado_ult_tentativa_envio = null,
                                dt_hr_ult_tentativa_envio = null,
                                msg_erro_ult_tentativa_envio = null,
                                st_processamento_mensagem = 0,
                                st_envio_cancelado = 0,
                                dt_hr_envio_cancelado = null,
                                usuario_envio_cancelado = null,
                                replyToMsg = null,
                                st_replyToMsg = 0
                            };

                            dbgravacao.Add(tEmailSndsvcMensagem);
                        }
                    }
                }

                try
                {
                    await dbgravacao.SaveChangesAsync();
                    dbgravacao.transacao.Commit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}