using System;
using System.Collections.Generic;
using System.Text;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.Acesso;
using UtilsGlobais;
using OrcamentoCotacaoBusiness.Enums;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class AcessoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public AcessoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<string> ValidarUsuario(string login, string senha_digitada_datastamp, bool somenteValidar)
        {
            login = login.ToUpper().Trim();

            var db = contextoProvider.GetContextoLeitura();
            //Validar o dados no bd
            var dados = from c in db.Tusuarios
                        where c.Usuario == login
                        select new
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
                            TipoUsuario = (int)Enums.Enums.TipoUsuario.VENDEDOR
                        };

            string retorno = null;
            var t = await dados.FirstOrDefaultAsync();

            //se o apelido nao existe
            if (t == null)
            {
                /*Busca de parceiros*/
                dados = from c in db.TorcamentistaEindicadors
                        where c.Apelido == login
                        select new
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
                            TipoUsuario = (int)Enums.Enums.TipoUsuario.PARCEIRO
                        };
                t = await dados.FirstOrDefaultAsync();
                /*Busca de parceiros*/
                if (t == null)
                {
                    /*Busca de vendedores de parceiros*/
                    dados = from c in db.TorcamentistaEindicadorVendedors
                            where c.Email == login
                            select new
                            {
                                Nome = c.Nome,
                                Senha = c.Senha,
                                Email = c.Email,
                                Datastamp = c.Senha,
                                Dt_Ult_Alteracao_Senha = c.DataUltimaAlteracao,
                                Bloqueado = !c.Ativo,
                                //c.Hab_Acesso_Sistema,
                                //c.statStatus,
                                Loja = c.Loja,
                                TipoUsuario = (int)Enums.Enums.TipoUsuario.VENDEDOR_DO_PARCEIRO
                            };
                    t = await dados.FirstOrDefaultAsync();
                    /*Busca de vendedores de parceiros*/
                }
                else
                {
                    return await Task.FromResult(retorno); // retorna null
                }
            }
            if (t.Datastamp == "")
                return await Task.FromResult(Constantes.ERR_USUARIO_BLOQUEADO);
            //if (t.Hab_Acesso_Sistema != 1)
            if (t.Bloqueado)
                return await Task.FromResult(Constantes.ERR_USUARIO_BLOQUEADO);
            //if (t.Status != "A")
            //    return await Task.FromResult(Constantes.ERR_USUARIO_BLOQUEADO);
            //if (t.Loja == "")
            //    return await Task.FromResult(Constantes.ERR_IDENTIFICACAO_LOJA);

            if (!somenteValidar)
            {
                //validar a senha
                var senha_digitada_decod = senha_digitada_datastamp;//, Constantes.FATOR_CRIPTO);

                //para garantir que sempre a as senhas são maiusculas iremos decodificar o datastamp 
                //e comparar os 2 convertido para maiusculas
                //var senha_banco_datastamp_decod = Util.decodificaDado(t.Datastamp, Constantes.FATOR_CRIPTO);

                //if (senha_digitada_decod.ToUpper().Trim() != senha_banco_datastamp_decod.ToUpper().Trim())
                if (senha_digitada_decod != t.Datastamp)
                    return await Task.FromResult(retorno);//retorna null

                //Fazer Update no bd
                using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    Tusuario usuario = await dbgravacao.Tusuarios
                    .Where(c => c.Usuario == login && c.Datastamp == senha_digitada_decod).FirstOrDefaultAsync();
                    usuario.Dt_Ult_Acesso = DateTime.Now;

                    await dbgravacao.SaveChangesAsync();
                    dbgravacao.transacao.Commit();
                }

                if (t.Dt_Ult_Alteracao_Senha == null)
                {
                    //Senha expirada, precisa mandar alguma valor de senha expirada
                    //coloquei o valor "4" para saber quando a senha esta expirada
                    return await Task.FromResult("4");
                }
            }

            return await Task.FromResult(t.Nome);
        }

        public async Task<List<TusuarioXLoja>> BuscarLojaUsuario(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var loja = (from c in db.TusuarioXLojas
                            //join usuario in db.TusuarioXLojas on c.Usuario = 
                        where c.Usuario == apelido
                        select c).ToList();

            return await Task.FromResult(loja);
        }

        public async Task<string> Buscar_unidade_negocio(List<TusuarioXLoja> lojas)
        {
            var db = contextoProvider.GetContextoLeitura();
            var unidade_negocio = ((from c in db.Tlojas
                                    join l in lojas on c.Loja equals l.Loja
                                    //where c.Loja == loja.Trim()
                                    select c.Unidade_Negocio).ToList());
            return string.Join(',', unidade_negocio);
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
                dbgravacao.TsessaoHistoricos.Add(sessaoHist);
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
                var sessaoHistTask = (from c in dbgravacao.TsessaoHistoricos
                                      where c.Usuario == apelido
                                      orderby c.DtHrInicio descending
                                      select c).FirstOrDefaultAsync();
                TsessaoHistorico sessaoHist = await sessaoHistTask;
                sessaoHist.DtHrTermino = DateTime.Now;

                dbgravacao.TsessaoHistoricos.Update(sessaoHist);
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
                string validou = await ValidarUsuario(alterarSenhaDto.Apelido, Util.codificaDado(senha, false), false);

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
                    TorcamentistaEindicador orcamentista = await (from c in dbgravacao.TorcamentistaEindicadors
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
    }
}
