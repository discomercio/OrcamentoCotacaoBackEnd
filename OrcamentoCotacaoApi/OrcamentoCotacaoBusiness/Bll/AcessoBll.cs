using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraIdentity;
using Microsoft.EntityFrameworkCore;
using OrcamentoCotacaoBusiness.Models.Response;
using Prepedido.Dto;
using PrepedidoBusiness.Dto.Acesso;
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

        public UsuarioLogin ValidarUsuario(string login, string senha_digitada_datastamp, bool somenteValidar, out string msgErro)
        {
            login = login.ToUpper().Trim();
            msgErro = "";
            int? tipo = 0;

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
                                //c.Hab_Acesso_Sistema,
                                //c.statStatus,
                                Loja = c.Loja,
                                TipoUsuario = (int)Constantes.TipoUsuario.VENDEDOR,
                                Id = c.Id
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
                                Bloqueado = (c.Status == "I"),
                                //c.Hab_Acesso_Sistema,
                                //c.statStatus,
                                Loja = c.Loja,
                                TipoUsuario = (int)Constantes.TipoUsuario.PARCEIRO,
                                Id = c.IdIndicador
                            };
                    t = dados.FirstOrDefault();
                    /*Busca de parceiros*/
                    if (t == null)
                    {
                        /*Busca de vendedores de parceiros*/
                        dados = from c in db.TorcamentistaEindicadorVendedor
                                where c.Email == login
                                select new UsuarioLogin
                                {
                                    Nome = c.Nome,
                                    Email = c.Email,
                                    Datastamp = c.Datastamp,
                                    Dt_Ult_Alteracao_Senha = c.DataUltimaAlteracao,
                                    Bloqueado = !c.Ativo,
                                    //c.Hab_Acesso_Sistema,
                                    //c.statStatus,
                                    Loja = c.Loja,
                                    TipoUsuario = (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO,
                                    Id = c.Id
                                };
                        t = dados.FirstOrDefault();
                        if (t != null)
                        {
                            tipo = (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO;
                        }
                        /*Busca de vendedores de parceiros*/
                    }
                    else
                    {
                        tipo = (int)Constantes.TipoUsuario.PARCEIRO;
                    }
                }
                else
                {
                    tipo = (int)Constantes.TipoUsuario.GESTOR;
                }
                if (t == null)
                {
                    tipo = null;
                    msgErro = Constantes.ERR_USUARIO_NAO_CADASTRADO;
                    return null;// await Task.FromResult(Constantes.ERR_USUARIO_NAO_CADASTRADO);
                }
                if (t.Datastamp == "")
                {
                    msgErro = Constantes.ERR_USUARIO_BLOQUEADO;
                    return null;
                }
                //if (t.Bloqueado)
                //{
                //    msgErro = Constantes.ERR_USUARIO_BLOQUEADO;
                //    return null;
                //}

                if (!somenteValidar)
                {
                    //validar a senha
                    var senha_digitada_decod = senha_digitada_datastamp;//, Constantes.FATOR_CRIPTO);

                    //para garantir que sempre a as senhas são maiusculas iremos decodificar o datastamp 
                    //e comparar os 2 convertido para maiusculas
                    //var senha_banco_datastamp_decod = Util.decodificaDado(t.Datastamp, Constantes.FATOR_CRIPTO);

                    //if (senha_digitada_decod.ToUpper().Trim() != senha_banco_datastamp_decod.ToUpper().Trim())
                    if (senha_digitada_decod != t.Datastamp)
                    {
                        msgErro = Constantes.ERR_SENHA_INVALIDA;
                        return null;//await Task.FromResult(retorno);//retorna null
                    }

                    //Fazer Update no bd
                    using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                    {
                        switch (tipo)
                        {
                            case (int)Constantes.TipoUsuario.GESTOR:
                                Tusuario usuario = dbgravacao.Tusuario
                                .Where(c => c.Usuario == login && c.Datastamp == senha_digitada_decod).FirstOrDefault();
                                usuario.Dt_Ult_Acesso = DateTime.Now;
                                break;
                            case (int)Constantes.TipoUsuario.PARCEIRO:
                                TorcamentistaEindicador parceiro = dbgravacao.TorcamentistaEindicador
                                .Where(c => c.Apelido == login && c.Datastamp == senha_digitada_decod).FirstOrDefault();
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

                    if (t.Dt_Ult_Alteracao_Senha == null)
                    {
                        //Senha expirada, precisa mandar alguma valor de senha expirada
                        //coloquei o valor "4" para saber quando a senha esta expirada
                        msgErro = Constantes.ERR_SENHA_EXPIRADA;
                        return null;// await Task.FromResult("4");
                    }
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
                var usuario = ValidarUsuario(alterarSenhaDto.Apelido, Util.codificaDado(senha, false), false, out validou);

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
                await AtualizarSenhaVendedorAsync(atualizarSenhaDto.Apelido, senha_codificada);
            }

            if (atualizarSenhaDto.TipoUsuario == (int)Constantes.TipoUsuario.PARCEIRO)
            {
                await AtualizarSenhaParceiroAsync(atualizarSenhaDto.Apelido, senha_codificada);
            }

            if (atualizarSenhaDto.TipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR_DO_PARCEIRO)
            {
                await AtualizarSenhaVendedoParceiro(atualizarSenhaDto.Apelido, senha_codificada);
            }

            return new AtualizarSenhaResponseViewModel(true, "Alteração de senha realizada com sucesso.");
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
                return "A senha deve conter pelo menos 8 caracteres entre letras e dígitos.";
            }

            if (!regex.IsMatch(senha_nova_confirma))
            {
                return "A confirmação da nova senha deve conter pelo menos 8 caracteres entre letras e dígitos.";
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

        private async Task AtualizarSenhaVendedorAsync(string apelido, string senha)
        {
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR))
            {
                var usuario = await (from u in dbgravacao.Tusuario
                                     where u.Usuario == apelido.ToUpper().Trim()
                                     select u).FirstOrDefaultAsync();

                usuario.Senha = Util.GerarSenhaAleatoria();
                usuario.Datastamp = senha;
                usuario.Dt_Ult_Alteracao_Senha = DateTime.Now.Date;
                usuario.Dt_Ult_Atualizacao = DateTime.Now;

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

        private async Task AtualizarSenhaParceiroAsync(string apelido, string senha)
        {
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR))
            {
                var orcamentista = await (from u in dbgravacao.TorcamentistaEindicador
                                          where u.Apelido == apelido.ToUpper().Trim()
                                          select u).FirstOrDefaultAsync();

                orcamentista.Senha = Util.GerarSenhaAleatoria();
                orcamentista.Datastamp = senha;
                orcamentista.Dt_Ult_Alteracao_Senha = DateTime.Now.Date;
                orcamentista.Dt_Ult_Atualizacao = DateTime.Now;

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

        private async Task AtualizarSenhaVendedoParceiro(string apelido, string senha)
        {
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTISTA_E_INDICADOR))
            {
                var vendedorParceiro = await (from u in dbgravacao.TorcamentistaEIndicadorVendedor
                                              where u.Nome == apelido.ToUpper().Trim()
                                              select u).FirstOrDefaultAsync();

                vendedorParceiro.Senha = Util.GerarSenhaAleatoria();
                vendedorParceiro.Datastamp = senha;
                vendedorParceiro.DataUltimaAlteracao = DateTime.Now;
                vendedorParceiro.DataUltimaAlteracaoSenha = DateTime.Now;

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
        }
    }
}