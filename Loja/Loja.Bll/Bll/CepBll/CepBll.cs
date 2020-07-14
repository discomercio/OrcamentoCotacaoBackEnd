using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loja.Bll.Dto.CepDto;
using Loja.Data;
using Microsoft.EntityFrameworkCore;


namespace Loja.Bll.CepBll
{
    public class CepBll
    {
        private readonly ContextoCepProvider contextoCepProvider;

        public CepBll(ContextoCepProvider contextoCepProvider)
        {
            this.contextoCepProvider = contextoCepProvider;
        }
        public async Task<IEnumerable<CepDto>> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            List<CepDto> cepDto = new List<CepDto>();

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

            return cepDto;
        }

        public async Task<IEnumerable<CepDto>> BuscarPorCep(string cep)
        {
            var db = contextoCepProvider.GetContextoLeitura();

            var cepTask = ((from c in db.LogLogradouros
                            join d in db.LogBairros on c.Bai_nu_sequencial_ini equals d.Bai_nu_sequencial
                            join e in db.LogLocalidades on c.Loc_nu_sequencial equals e.Loc_nu_sequencial
                            where c.Cep_dig.EndsWith(cep)
                            select new CepDto
                            {
                                Cep = c.Cep_dig,
                                Uf = c.Ufe_sg,
                                Cidade = e.Loc_nosub,
                                Bairro = d.Bai_no,
                                Endereco = c.Log_tipo_logradouro + " " + c.Log_no,
                                LogradouroComplemento = c.Log_complemento
                            }).AsEnumerable()
                           .Union(from c in db.LogLocalidades
                                  where c.Cep_dig.EndsWith(cep)
                                  select new CepDto
                                  {
                                      Cep = c.Cep_dig,
                                      Uf = c.Ufe_sg,
                                      Cidade = c.Loc_nosub
                                  }).AsEnumerable()
                           .Union(from c in db.TcepLogradouros
                                  where c.Cep8_log.EndsWith(cep)
                                  select new CepDto
                                  {
                                      Cep = c.Cep8_log,
                                      Uf = c.Uf_log,
                                      Cidade = c.Nome_local,
                                      Bairro = c.Extenso_bai,
                                      Endereco = c.Abrev_tipo + " " + c.Nome_log,
                                      LogradouroComplemento = c.Comple_log
                                  }));

            List<CepDto> lst = new List<CepDto>();
            lst = cepTask.ToList();
            //var cepDto = await cepTask.ToList();

            return await Task.FromResult(lst);
        }

        public async Task<IEnumerable<CepDto>> BuscarPorEndereco(string endereco, string uf, string cidade)
        {
            var db = contextoCepProvider.GetContextoLeitura();

            var cepTask = ((from c in db.LogLogradouros
                            join d in db.LogBairros on c.Bai_nu_sequencial_ini equals d.Bai_nu_sequencial
                            join e in db.LogLocalidades on c.Loc_nu_sequencial equals e.Loc_nu_sequencial
                            where (c.Ufe_sg == uf) &&
                                  (e.Loc_nosub == cidade) &&
                                  (c.Log_no.Contains(endereco))
                            select new CepDto
                            {
                                Cep = c.Cep_dig,
                                Uf = c.Ufe_sg,
                                Cidade = e.Loc_nosub,
                                Bairro = d.Bai_no,
                                Endereco = c.Log_tipo_logradouro + " " + c.Log_no,
                                LogradouroComplemento = c.Log_complemento
                            }).Take(300)
                            .Union(from c in db.LogLocalidades
                                   where c.Ufe_sg == uf &&
                                         c.Loc_nosub == endereco &&
                                         c.Cep_dig.Length > 0
                                   select new CepDto
                                   {
                                       Cep = c.Cep_dig,
                                       Uf = c.Ufe_sg,
                                       Cidade = c.Loc_nosub
                                   })
                            .Union(from c in db.TcepLogradouros
                                   where c.Uf_log == uf &&
                                         c.Nome_local == endereco
                                   orderby c.Uf_log, c.Nome_local, c.Extenso_bai, c.Nome_log
                                   select new CepDto
                                   {
                                       Cep = c.Cep8_log,
                                       Uf = c.Uf_log,
                                       Cidade = c.Nome_local,
                                       Bairro = c.Extenso_bai,
                                       Endereco = c.Abrev_tipo + " " + c.Nome_log,
                                       LogradouroComplemento = c.Comple_log
                                   }).Take(300));

            var cepDto = await cepTask.ToListAsync();

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

        public async Task<IEnumerable<CepDto>> BuscarCepPorEndereco(string endereco, string cidade, string uf)
        {

            //var db = contextoCepProvider.GetContextoLeitura();

            List<CepDto> cepdto = new List<CepDto>();
            using (var db = contextoCepProvider.GetContextoLeitura())
            {
                using (var command = db.Database.GetDbConnection().CreateCommand())
                {
                    SqlParameter param = new SqlParameter();

                    if (endereco == null)
                        endereco = "";
                    param.Value = endereco.ToString();
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

                    //command.CommandText = "select * from LOG_LOGRADOURO";
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
                            cepdto.Add(new CepDto
                            {
                                Cep = Util.Util.FormataCep(result["cep"].ToString()),
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

            return cepdto;
        }
    }
}
