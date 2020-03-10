﻿using System;
using System.Collections.Generic;
using System.Text;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.Acesso;

namespace PrepedidoBusiness.Bll
{
    public class AcessoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public AcessoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<string> ValidarUsuario(string apelido, string senha, bool somenteValidar)
        {
            //apelido = "SALOMÃO";

            var db = contextoProvider.GetContextoLeitura();
            //Validar o dados no bd
            var dados = from c in db.TorcamentistaEindicadors
                        where c.Apelido == apelido.ToUpper()
                        select new
                        {
                            c.Razao_Social_Nome,
                            c.Senha,
                            c.Datastamp,
                            c.Dt_Ult_Alteracao_Senha,
                            c.Hab_Acesso_Sistema,
                            c.Status,
                            c.Loja
                        };

            string retorno = null;
            var t = await dados.FirstOrDefaultAsync();

            //se o apelido nao existe
            if (t == null)
                return await Task.FromResult(retorno);
            if (t.Datastamp == "")
                return await Task.FromResult(Constantes.ERR_USUARIO_BLOQUEADO);
            if (t.Hab_Acesso_Sistema != 1)
                return await Task.FromResult(Constantes.ERR_USUARIO_BLOQUEADO);
            if (t.Status != "A")
                return await Task.FromResult(Constantes.ERR_USUARIO_BLOQUEADO);
            if (t.Loja == "")
                return await Task.FromResult(Constantes.ERR_IDENTIFICACAO_LOJA);

            if (!somenteValidar)
            {
                //validar a senha
                //SLM112233 - SALOMÃO
                var senhaCodificada = DecodificaSenha(senha).ToUpper();

                var senha_banco = DecodificaSenha(t.Datastamp);
                if (senha.ToUpper() != t.Datastamp.ToUpper())
                {
                    return await Task.FromResult(retorno);
                }
                if (senhaCodificada != senha_banco)
                    return await Task.FromResult(retorno);

                //Fazer Update no bd
                using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
                {
                    TorcamentistaEindicador orcamentista = await dbgravacao.TorcamentistaEindicadors
                    .Where(c => c.Apelido == apelido).SingleAsync();
                    orcamentista.Dt_Ult_Acesso = DateTime.Now;

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

            return await Task.FromResult(t.Razao_Social_Nome);
        }

        public async Task<string> BuscarLojaUsuario(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var loja = (from c in db.TorcamentistaEindicadors
                        where c.Apelido == apelido
                        select c.Loja).Single();

            return await Task.FromResult(loja);
        }

        public async Task GravarSessaoComTransacao(string ip, string apelido, string userAgent)
        {
            //afazer: realizar a validação do usuário antes de gravar a sessão na tabela


            string loja = await BuscarLojaUsuario(apelido);

            //inserir na t_SESSAO_HISTORICO
            TsessaoHistorico sessaoHist = new TsessaoHistorico
            {
                Usuario = apelido,
                SessionCtrlTicket = "",
                DtHrInicio = DateTime.Now,
                DtHrTermino = null,
                Loja = loja,
                Modulo = "ORCTO",
                IP = ip,
                UserAgent = userAgent
            };

            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
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

        public string DecodificaSenha(string origem)
        {
            string chave = geraChave();
            string s_destino = "";
            string s_origem = origem;
            int i = s_origem.Length - 2;

            s_origem = s_origem.Substring(s_origem.Length - i, i);
            s_origem = s_origem.ToUpper();

            string s;
            string codificar = "";

            for (i = 1; i <= s_origem.Length; i++)
            {
                s = s_origem.Substring((i - 1), 2);
                if (s != "00")
                {
                    codificar = s_origem.Substring((i - 1), (s_origem.Length - i + 1));
                    break;
                }
                i++;
            }

            for (i = 0; i < codificar.Length; i++)
            {
                s = codificar.Substring(i, 2);
                int hexNumber = int.Parse(s, System.Globalization.NumberStyles.HexNumber);
                s_destino += (char)(hexNumber);
                i++;
            }
            s_origem = s_destino;
            s_destino = "";

            string letra;

            for (i = 0; i < s_origem.Length; i++)
            {
                //pega a letra
                letra = chave.Substring(i, 1);
                //Converte para char
                int i_chave = (Convert.ToChar(letra) * 2) + 1;
                int i_dado = Convert.ToChar(s_origem.Substring(i, 1));

                int contaMod = i_chave ^ i_dado;
                contaMod /= 2;
                s_destino += (char)contaMod;
            }

            return s_destino;
        }

        public string CodificaSenha(string origem)
        {
            string chave = geraChave();
            int i_chave = 0;
            int i_dado = 0;
            string s_origem = origem;
            string letra = "";
            string s_destino = "";

            if (s_origem.Length > 15)
            {
                s_origem = s_origem.Substring(0, 15);
            }

            for (int i = 0; i < s_origem.Length; i++)
            {
                letra = chave.Substring(i, 1);
                i_chave = (Convert.ToChar(letra) * 2) + 1;
                i_dado = Convert.ToChar(s_origem.Substring(i, 1)) * 2;
                int contaMod = i_chave ^ i_dado;
                s_destino += (char)contaMod;
            }

            s_origem = s_destino;
            s_destino = "";
            string destino = "";

            for (int i = 0; i < s_origem.Length; i++)
            {
                letra = s_origem.Substring(i, 1);
                i_chave = (Convert.ToChar(letra));
                string hexNumber = i_chave.ToString("X");

                while (hexNumber.Length < 2)
                {
                    hexNumber += "0";
                }
                destino += hexNumber;
            }

            while (destino.Length < 30)
            {
                destino = "0" + destino;
            }
            s_destino = "0x" + destino.ToUpper();

            return s_destino;
        }

        public async Task FazerLogout(string apelido)
        {
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {

                //O orcamentista não é salvo nessa tabela
                //o Usuario é o usuario_cadastro da tabela t_ORCAMENTISTA_E_INDICADOR
                //CAMILA IRÁ VERIFICAR COM O HAMILTON

                //Tusuario usuario = db.Tusuarios.
                //    Where(r => r.Usuario == apelido)
                //    .Single();
                ////atualiza a tabela t_USUARIO
                //usuario.SessionCtrlTicket = null;
                //usuario.SessionCtrlLoja = null;
                //usuario.SessionCtrlModulo = null;
                //usuario.SessionCtrlDtHrLogon = null;

                //atualiza a tabela t_SESSAO_HISTORICO
                //TsessaoHistorico sessaoHist = dbgravacao.TsessaoHistoricos
                //    .Where(r => r.Usuario == apelido
                //             && r.DtHrInicio >= DateTime.Now.AddHours(-1))
                //    .SingleOrDefault();
                //sessaoHist.DtHrTermino = DateTime.Now;

                var sessaoHistTask = (from c in dbgravacao.TsessaoHistoricos
                                      where c.Usuario == apelido
                                      orderby c.DtHrInicio descending
                                      select c).FirstOrDefaultAsync();
                TsessaoHistorico sessaoHist = await sessaoHistTask;
                sessaoHist.DtHrTermino = DateTime.Now;

                dbgravacao.TsessaoHistoricos.Add(sessaoHist);
                //dbgravacao.TsessaoHistoricos.Update(sessaoHist);
                await dbgravacao.SaveChangesAsync();
                dbgravacao.transacao.Commit();
            }
        }

        public async Task<string> AlterarSenha(AlterarSenhaDto alterarSenhaDto)
        {
            string retorno = "";
            string senha_aleatoria = "";

            if (!string.IsNullOrEmpty(alterarSenhaDto.Senha) && !string.IsNullOrEmpty(alterarSenhaDto.SenhaNova) &&
                !string.IsNullOrEmpty(alterarSenhaDto.SenhaNovaConfirma))
            {
                if (alterarSenhaDto.SenhaNova.Length < 5)
                {
                    return retorno = "A nova senha deve possuir no mínimo 5 caracteres.";
                }
                if (alterarSenhaDto.SenhaNovaConfirma.Length < 5)
                {
                    return retorno = "A confirmação da nova senha deve possuir no mínimo 5 caracteres.";
                }
                if (alterarSenhaDto.SenhaNova != alterarSenhaDto.SenhaNovaConfirma)
                {
                    return retorno = "A confirmação da nova senha está incorreta.";
                }
                if (alterarSenhaDto.Senha == alterarSenhaDto.SenhaNova)
                {
                    return retorno = "A nova senha deve ser diferente da senha atual.";
                }

                //valida as credenciais
                string validou = ValidarUsuario(alterarSenhaDto.Apelido, CodificaSenha(alterarSenhaDto.Senha), false).Result;
                if (validou == Constantes.ERR_USUARIO_BLOQUEADO)
                {
                    return retorno = "Usuário bloqueado.";
                }

                if(validou == null)
                {
                    return retorno = "Senha atual incorreta."; 
                }

                //vamos alterar a senha na base de dados
                using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
                {
                    TorcamentistaEindicador orcamentista = await (from c in dbgravacao.TorcamentistaEindicadors
                                                                  where c.Apelido == alterarSenhaDto.Apelido
                                                                  select c).FirstOrDefaultAsync();


                    //se retornar codigo 7 é pq esta bloqueado

                    //não esta bloqueado, apenas com a senha expirada e estamos alterando a senha nesse momento

                    //vamos validar se a senha esta correta antes de alterar a senha



                    senha_aleatoria = Utils.Util.GerarSenhaAleatoria();

                    orcamentista.Datastamp = CodificaSenha(alterarSenhaDto.SenhaNova);
                    orcamentista.Dt_Ult_Alteracao_Senha = DateTime.Now.Date;
                    orcamentista.Dt_Ult_Atualizacao = DateTime.Now;
                    orcamentista.Senha = senha_aleatoria;

                    //vamos gravar o log
                    if (Utils.Util.GravaLog(dbgravacao, alterarSenhaDto.Apelido, orcamentista.Loja, "", "", Constantes.OP_LOG_SENHA_ALTERACAO, "SENHA ALTERADA PELO ORÇAMENTISTA"))
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
