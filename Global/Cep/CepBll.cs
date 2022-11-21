using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using InfraBanco;
using InfraBanco.Modelos;
using Cep.Dados;

namespace Cep
{
    public class CepBll
    {
        public static class MensagensErro
        {
            public static string Municipio_nao_consta_na_relacao_IBGE(string municipio, string uf)
            {
                return "Município '" + municipio + "' não consta na relação de municípios do IBGE para a UF de '" + uf + "'!";
            }
        }
        
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;
        private readonly IBancoNFeMunicipio bancoNFeMunicipio;
        private readonly ContextoBdProvider contextoProvider;

        public CepBll(InfraBanco.ContextoCepProvider contextoCepProvider, IBancoNFeMunicipio bancoNFeMunicipio,
            ContextoBdProvider contextoProvider)
        {
            this.contextoCepProvider = contextoCepProvider;
            this.bancoNFeMunicipio = bancoNFeMunicipio;
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<CepDados>> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            List<CepDados> cepDto = new List<CepDados>();

            if (!string.IsNullOrEmpty(cep))
            {
                cep = cep.Replace("-", "");

                if (cep.Length == 8)
                {
                    cepDto = (await BuscarPorCep(cep)).ToList();
                }
            }
            else if (!string.IsNullOrEmpty(endereco) && !string.IsNullOrEmpty(uf) && !string.IsNullOrEmpty(cidade))
            {
                endereco = endereco.Replace("Rua", "").Replace("R", "").Replace(":", "").Replace("Av", "").Replace("Avenida", "").Replace(".", "").Trim();

                cepDto = (await BuscarPorEndereco(endereco, uf, cidade)).ToList();
            }

            if (cepDto.Count > 0)
            {
                //vamos validar para saber se a cidade existe no IBGE
                List<string> lstErros = new List<string>();
                //se não consistir vamos busca a lista de Cidades com base na UF
                if (!await ConsisteMunicipioIBGE(cepDto[0].Cidade, cepDto[0].Uf, lstErros,
                    contextoProvider, bancoNFeMunicipio, false))
                {
                    //vamos busca a lista de cidades com base na UF
                    List<UFeMunicipiosDados> lstMunicipio = (await bancoNFeMunicipio.BuscarSiglaTodosUf(contextoProvider, cepDto[0].Uf, "")).ToList();
                    cepDto[0].ListaCidadeIBGE = new List<string>();
                    //vamos atribuir para a classe de cep uma Lista com todas as cidades do estado
                    if (lstMunicipio.Count > 0)
                    {
                        lstMunicipio.ForEach(x =>
                        {
                            if (x.ListaMunicipio.Count > 0)
                            {
                                x.ListaMunicipio.ForEach(y =>
                                {
                                    cepDto[0].ListaCidadeIBGE.Add(y.Descricao);
                                });
                            }
                        });
                    }
                }
            }

            return cepDto;
        }

        public async Task<IEnumerable<CepDados>> BuscarPorCep(string cep)
        {
            var db = contextoCepProvider.GetContextoLeitura();

            List<CepDados> lista1 = await (from c in db.LogLogradouros
                                         join d in db.LogBairros on c.Bai_nu_sequencial_ini equals d.Bai_nu_sequencial
                                         join e in db.LogLocalidades on c.Loc_nu_sequencial equals e.Loc_nu_sequencial
                                         where EF.Functions.Like(c.Cep_dig, cep+"%")
                                         select new CepDados
                                         {
                                             Cep = c.Cep_dig,
                                             Uf = c.Ufe_sg,
                                             Cidade = e.Loc_nosub,
                                             Bairro = d.Bai_no,
                                             Endereco = c.Log_tipo_logradouro + " " + c.Log_no,
                                             LogradouroComplemento = c.Log_complemento
                                         }).ToListAsync();

            List<CepDados> lista2 = await (from c in db.LogGrandeUsuarios
                                           join d in db.LogLogradouros on c.Log_nu_sequencial equals d.Log_nu_sequencial
                                           join e in db.LogBairros on c.Bai_nu_sequencial equals e.Bai_nu_sequencial
                                           join f in db.LogLocalidades on c.Loc_nu_sequencial equals f.Loc_nu_sequencial
                                           where EF.Functions.Like(c.Cep_dig, cep + "%")
                                           select new CepDados
                                           {
                                               Cep = c.Cep_dig,
                                               Uf = c.Ufe_sg,
                                               Cidade = f.Loc_nosub,
                                               Bairro = e.Bai_no,
                                               Endereco = d.Log_no,
                                               LogradouroComplemento = c.Ggru_no
                                           }).ToListAsync();

            List<CepDados> lista3 = await (from c in db.LogLocalidades
                                         where EF.Functions.Like(c.Cep_dig, cep + "%")
                                           select new CepDados
                                         {
                                             Cep = c.Cep_dig,
                                             Uf = c.Ufe_sg,
                                             Cidade = c.Loc_nosub
                                         }).ToListAsync();

            List<CepDados> lista4 = await (from c in db.TcepLogradouros
                                         where EF.Functions.Like(c.Cep8_log, cep + "%")
                                           select new CepDados
                                         {
                                             Cep = c.Cep8_log,
                                             Uf = c.Uf_log,
                                             Cidade = c.Nome_local,
                                             Bairro = c.Extenso_bai,
                                             Endereco = c.Abrev_tipo + " " + c.Nome_log,
                                             LogradouroComplemento = c.Comple_log
                                         }).ToListAsync();

            var cepDto = lista1.Union(lista2).Union(lista3).Union(lista4).OrderBy(x => x.Cep);

            return cepDto;
        }

        public async Task<IEnumerable<CepDados>> BuscarPorEndereco(string endereco, string uf, string cidade)
        {
            var db = contextoCepProvider.GetContextoLeitura();

            List<CepDados> lista1 = await (from c in db.LogLogradouros
                                         join d in db.LogBairros on c.Bai_nu_sequencial_ini equals d.Bai_nu_sequencial
                                         join e in db.LogLocalidades on c.Loc_nu_sequencial equals e.Loc_nu_sequencial
                                         where (c.Ufe_sg == uf) &&
                                               (e.Loc_nosub == cidade) &&
                                               (c.Log_no.Contains(endereco))
                                         select new CepDados
                                         {
                                             Cep = c.Cep_dig,
                                             Uf = c.Ufe_sg,
                                             Cidade = e.Loc_nosub,
                                             Bairro = d.Bai_no,
                                             Endereco = c.Log_tipo_logradouro + " " + c.Log_no,
                                             LogradouroComplemento = c.Log_complemento
                                         }).OrderBy(x => x.Endereco).Take(300).ToListAsync();

            List<CepDados> lista2 = await (from c in db.LogGrandeUsuarios
                                           join d in db.LogLogradouros on c.Log_nu_sequencial equals d.Log_nu_sequencial
                                           join e in db.LogBairros on c.Bai_nu_sequencial equals e.Bai_nu_sequencial
                                           join f in db.LogLocalidades on c.Loc_nu_sequencial equals f.Loc_nu_sequencial
                                           where (c.Ufe_sg == uf) &&
                                                 (f.Loc_nosub == cidade) &&
                                                 (d.Log_no.Contains(endereco))
                                           select new CepDados
                                           {
                                               Cep = c.Cep_dig,
                                               Uf = c.Ufe_sg,
                                               Cidade = f.Loc_nosub,
                                               Bairro = e.Bai_no,
                                               Endereco = d.Log_no,
                                               LogradouroComplemento = c.Ggru_no
                                           }).OrderBy(x => x.Cep).Take(300).ToListAsync();

            List<CepDados> lista3 = await (from c in db.LogLocalidades
                                         where c.Ufe_sg == uf &&
                                               c.Loc_nosub == endereco &&
                                               c.Cep_dig.Length > 0
                                         select new CepDados
                                         {
                                             Cep = c.Cep_dig,
                                             Uf = c.Ufe_sg,
                                             Cidade = c.Loc_nosub
                                         }).OrderBy(x => x.Cep).Take(300).ToListAsync();

            List<CepDados> lista4 = await (from c in db.TcepLogradouros
                                         where c.Uf_log == uf &&
                                               c.Nome_local == endereco
                                         orderby c.Uf_log, c.Nome_local, c.Extenso_bai, c.Nome_log
                                         select new CepDados
                                         {
                                             Cep = c.Cep8_log,
                                             Uf = c.Uf_log,
                                             Cidade = c.Nome_local,
                                             Bairro = c.Extenso_bai,
                                             Endereco = c.Abrev_tipo + " " + c.Nome_log,
                                             LogradouroComplemento = c.Comple_log
                                         }).OrderBy(x => x.Endereco).Take(300).ToListAsync();

            var cepDto = lista1.Union(lista2).Union(lista3).Union(lista4).OrderBy(x => x.Cep);

            return cepDto;
        }

        public async Task<IEnumerable<string>> BuscarUfs()
        {
            var db = contextoCepProvider.GetContextoLeitura();

            var lstUfsTask = (from c in db.LogLogradouros
                              select c.Ufe_sg).Distinct();

            var lstUf = await lstUfsTask.ToListAsync();

            return lstUf;
        }

        public async Task<IEnumerable<string>> BuscarLocalidades(string uf)
        {
            var db = contextoCepProvider.GetContextoLeitura();

            var lstLocalidadesTask = from c in db.LogLocalidades
                                     where c.Ufe_sg == uf
                                     select c.Loc_nosub;

            var lstLocalidades = await lstLocalidadesTask.ToListAsync();

            return lstLocalidades;
        }

        public async Task<IEnumerable<CepDados>> BuscarCepPorEndereco(string endereco, string cidade, string uf)
        {
            List<CepDados> cepdto = new List<CepDados>();

            if (string.IsNullOrEmpty(endereco)) endereco = "";
            else endereco = endereco.Replace("Rua", "").Replace("R. ", "").Replace("R ", "")
                    .Replace("Avenida", "").Replace("Av. ", "").Replace("Av ", "")
                    .Replace("Travessa", "").Replace("T. ", "").Replace("T ", "")
                    .Replace(".", "").Replace(":", "").Trim();

            using (var db = contextoCepProvider.GetContextoLeitura())
            {
                using (var command = db.Database.GetDbConnection().CreateCommand())
                {
                    SqlParameter param = new SqlParameter();

                    if (endereco == null)
                        endereco = "";
                    param.Value = endereco;
                    param.ParameterName = "@logradouro";
                    command.Parameters.Add(param);

                    SqlParameter paramLike = new SqlParameter();
                    paramLike.Value = "%" + endereco + "%";
                    paramLike.ParameterName = "@logradouroLike";

                    command.Parameters.Add(paramLike);


                    if (!string.IsNullOrWhiteSpace(uf))
                    {
                        SqlParameter paramuF = new SqlParameter();
                        paramuF.Value = uf;
                        paramuF.ParameterName = "@UF";
                        command.Parameters.Add(paramuF);
                    }
                    if (!string.IsNullOrWhiteSpace(cidade))
                    {
                        SqlParameter paramCidade = new SqlParameter();
                        paramCidade.Value = "%" + cidade + "%";
                        paramCidade.ParameterName = "@cidade";
                        command.Parameters.Add(paramCidade);
                    }

                    string sql = "SELECT TOP 100 'LOGRADOURO' AS tabela_origem," +
                                        "LOG_LOGRADOURO.CEP_DIG AS cep," +
                                        "LOG_LOGRADOURO.UFE_SG AS uf," +
                                        "LOG_LOCALIDADE.LOC_NOSUB AS localidade," +
                                        "LOG_BAIRRO.BAI_NO AS bairro_extenso," +
                                        "LOG_BAIRRO.BAI_NO_ABREV AS bairro_abreviado," +
                                        "LOG_LOGRADOURO.LOG_TIPO_LOGRADOURO AS logradouro_tipo," +
                                        "LOG_LOGRADOURO.LOG_NO AS logradouro_nome," +
                                        "LOG_LOGRADOURO.LOG_COMPLEMENTO AS logradouro_complemento " +
                        "FROM LOG_LOGRADOURO " +
                        "INNER JOIN LOG_BAIRRO ON LOG_LOGRADOURO.BAI_NU_SEQUENCIAL_INI = LOG_BAIRRO.BAI_NU_SEQUENCIAL " +
                        "INNER JOIN LOG_LOCALIDADE ON LOG_LOGRADOURO.LOC_NU_SEQUENCIAL = LOG_LOCALIDADE.LOC_NU_SEQUENCIAL " +
                        "WHERE " +
                            "LOG_LOGRADOURO.LOG_NO LIKE @logradouroLike COLLATE Latin1_General_CI_AI ";
                    if (!string.IsNullOrWhiteSpace(uf))
                    {
                        sql += " AND LOG_LOGRADOURO.UFE_SG = @UF  COLLATE Latin1_General_CI_AI ";
                    }
                    if (!string.IsNullOrWhiteSpace(cidade))
                    {
                        sql += " AND LOG_LOCALIDADE.LOC_NOSUB LIKE @cidade  COLLATE Latin1_General_CI_AI ";
                    }
                    sql +=
                        " UNION " +
                        "SELECT TOP 100 'LOCALIDADE' AS tabela_origem," +
                                "CEP_DIG AS cep," +
                                "UFE_SG AS uf," +
                                "LOC_NOSUB AS localidade," +
                                "'' AS bairro_extenso," +
                                "'' AS bairro_abreviado," +
                                "'' AS logradouro_tipo," +
                                "'' AS logradouro_nome," +
                                "'' AS logradouro_complemento " +
                        "FROM LOG_LOCALIDADE " +
                        "WHERE " +
                            "1=1 ";
                    if (!string.IsNullOrWhiteSpace(uf))
                    {
                        sql += " AND UFE_SG = @UF  COLLATE Latin1_General_CI_AI ";
                    }
                    //se o usuario especificou algum endereco, nao pegamos desta tabela porque ela nao possui endereco
                    if (!string.IsNullOrWhiteSpace(cidade) && string.IsNullOrWhiteSpace(endereco))
                    {
                        sql += " AND LOC_NOSUB LIKE @cidade  COLLATE Latin1_General_CI_AI ";
                    }
                    else
                    {
                        //se nao tem localidade, nao lemos nada da LOG_LOCALIDADE
                        sql += " AND 1=0 ";
                    }
                    sql +=
                        "UNION " +
                        "SELECT TOP 100 'LOGRADOURO' AS tabela_origem," +
                                "cep8_log COLLATE Latin1_General_CI_AI AS cep," +
                                "uf_log COLLATE Latin1_General_CI_AI AS uf," +
                                "nome_local COLLATE Latin1_General_CI_AI AS localidade," +
                                "extenso_bai COLLATE Latin1_General_CI_AI AS bairro_extenso," +
                                "abrev_bai COLLATE Latin1_General_CI_AI AS bairro_abreviado," +
                                "abrev_tipo COLLATE Latin1_General_CI_AI AS logradouro_tipo," +
                                "nome_log COLLATE Latin1_General_CI_AI AS logradouro_nome," +
                                "comple_log COLLATE Latin1_General_CI_AI AS logradouro_complemento " +
                        "FROM t_CEP_LOGRADOURO " +
                        "WHERE " +
                            "nome_log like @logradouroLike COLLATE Latin1_General_CI_AI ";
                    if (!string.IsNullOrWhiteSpace(uf))
                    {
                        sql += " AND uf_log = @UF  COLLATE Latin1_General_CI_AI ";
                    }
                    if (!string.IsNullOrWhiteSpace(cidade))
                    {
                        sql += " AND nome_local LIKE @cidade  COLLATE Latin1_General_CI_AI ";
                    }
                    sql += "ORDER BY uf, localidade, bairro_extenso, cep, logradouro_nome";

                    command.CommandText = sql;

                    db.Database.OpenConnection();
                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (result.Read())
                        {
                            cepdto.Add(new CepDados
                            {
                                Cep = result["cep"].ToString(),
                                Uf = result["uf"].ToString(),
                                Cidade = result["localidade"].ToString(),
                                Bairro = result["bairro_extenso"].ToString(),
                                Endereco = result["logradouro_tipo"].ToString() + " " + result["logradouro_nome"].ToString(),//c.Log_tipo_logradouro + " " + c.Log_no
                                LogradouroComplemento = result["logradouro_complemento"].ToString()
                            });
                        }
                    }
                }
            }

            if (cepdto.Count > 0)
            {
                //vamos validar para saber se a cidade existe no IBGE
                List<string> lstErros = new List<string>();
                //se não consistir vamos busca a lista de Cidades com base na UF
                if (!await ConsisteMunicipioIBGE(cepdto[0].Cidade, cepdto[0].Uf, lstErros,
                    contextoProvider, bancoNFeMunicipio, false))
                {
                    //vamos busca a lista de cidades com base na UF
                    List<UFeMunicipiosDados> lstMunicipio = (await bancoNFeMunicipio.BuscarSiglaTodosUf(contextoProvider, cepdto[0].Uf, "")).ToList();
                    cepdto[0].ListaCidadeIBGE = new List<string>();
                    //vamos atribuir para a classe de cep uma Lista com todas as cidades do estado
                    if (lstMunicipio.Count > 0)
                    {
                        lstMunicipio.ForEach(x =>
                        {
                            if (x.ListaMunicipio.Count > 0)
                            {
                                x.ListaMunicipio.ForEach(y =>
                                {
                                    cepdto[0].ListaCidadeIBGE.Add(y.Descricao);
                                });
                            }
                        });
                    }
                }
            }


            return cepdto;
        }

        public static async Task<bool> ConsisteMunicipioIBGE(string municipio, string uf,
            List<string> lstErros, ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio,
            bool mostrarMensagemErro)
        {
            var db = contextoProvider.GetContextoLeitura();

            if (string.IsNullOrEmpty(municipio))
            {
                if (mostrarMensagemErro)
                    lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                        "nenhum município foi informado!");
                return false;
            }

            if (string.IsNullOrEmpty(uf))
            {
                if (mostrarMensagemErro)
                    lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                        "a UF não foi informada!");
                return false;
            }

            else
            {
                if (uf.Length > 2)
                {
                    if (mostrarMensagemErro)
                        lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                        "a UF é inválida (" + uf + ")!");
                    return false;
                }

            }

            if (lstErros.Count == 0)
            {
                List<NfeMunicipio> lst_nfeMunicipios = (await bancoNFeMunicipio.BuscarSiglaUf(uf, municipio, false, contextoProvider)).ToList();

                if (!lst_nfeMunicipios.Any())
                {
                    if (mostrarMensagemErro)
                        lstErros.Add(MensagensErro.Municipio_nao_consta_na_relacao_IBGE(municipio, uf));

                    return false;
                }
            }

            return true;
        }

    }
}
