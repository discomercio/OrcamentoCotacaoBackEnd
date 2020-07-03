﻿using InfraBanco;
using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.Cep;

namespace PrepedidoBusiness.Utils
{
    public interface IBancoNFeMunicipio
    {
        Task<IEnumerable<UFeMunicipiosDto>> BuscarSiglaTodosUf(ContextoBdProvider contextoProvider);
        Task<IEnumerable<NfeMunicipio>> BuscarSiglaUf(string uf, string municipio, bool buscaParcial, ContextoBdProvider contextoProvider);
        Task<string> MontarProviderStringParaNFeMunicipio(ContextoBdProvider contextoProvider);
    }

    public class BancoNFeMunicipio : IBancoNFeMunicipio
    {
        public async Task<IEnumerable<NfeMunicipio>> BuscarSiglaUf(string uf, string municipio, bool buscaParcial,
            ContextoBdProvider contextoProvider)
        {
            List<NfeMunicipio> lstNfeMunicipio = new List<NfeMunicipio>();

            string providerString = await MontarProviderStringParaNFeMunicipio(contextoProvider);

            using (SqlConnection sql = new SqlConnection(providerString))
            {
                SqlParameter param = new SqlParameter();
                param.Value = uf.ToUpper();
                param.ParameterName = "@UF";

                string query = "SELECT *  FROM NFE_UF WHERE (SiglaUF = @UF)";

                SqlCommand command = new SqlCommand(query, sql);
                command.Parameters.Add(param);

                command.Connection.Open();

                using (var result = await command.ExecuteReaderAsync())
                {
                    if (result != null)
                    {
                        NfeUf nfeUF = new NfeUf();
                        while (result.Read())
                        {
                            nfeUF.CodUF = result["CodUF"].ToString();
                            nfeUF.SiglaUF = result["SiglaUf"].ToString();
                        };

                        command.Connection.Close();

                        query = "SELECT * FROM NFE_MUNICIPIO WHERE (CodMunic LIKE @nfeUF_CodUF) AND " +
                            "(Descricao = @municipio COLLATE Latin1_General_CI_AI)";

                        command = new SqlCommand(query, sql);

                        param = new SqlParameter();
                        param.Value = nfeUF.CodUF + Constantes.BD_CURINGA_TODOS;
                        param.ParameterName = "@nfeUF_CodUF";
                        command.Parameters.Add(param);

                        SqlParameter param2 = new SqlParameter();
                        param2.Value = municipio;
                        param2.ParameterName = "@municipio";
                        command.Parameters.Add(param2);

                        command.Connection.Open();

                        using (var result2 = await command.ExecuteReaderAsync())
                        {
                            if (result2 != null)
                            {
                                while (result2.Read())
                                {
                                    NfeMunicipio nfeMunicipio = new NfeMunicipio
                                    {
                                        CodMunic = result2["CodMunic"].ToString(),
                                        Descricao = result2["Descricao"].ToString()
                                    };

                                    lstNfeMunicipio.Add(nfeMunicipio);
                                }
                            }
                            else if (buscaParcial)
                            {
                                command.Connection.Close();

                                query = "SELECT * FROM NFE_MUNICIPIO WHERE (CodMunic LIKE @nfeUF_CodUF) AND " +
                                    "(Descricao LIKE @municipio COLLATE Latin1_General_CI_AI)";

                                command = new SqlCommand(query, sql);

                                param = new SqlParameter();
                                param.Value = nfeUF.CodUF + Constantes.BD_CURINGA_TODOS;
                                param.ParameterName = "@nfeUF_CodUF";
                                command.Parameters.Add(param);

                                param2 = new SqlParameter();
                                param2.Value = municipio.Substring(municipio.Length - 1, 1) + Constantes.BD_CURINGA_TODOS;
                                param2.ParameterName = "@municipio";
                                command.Parameters.Add(param2);


                                command.Connection.Open();

                                using (var result3 = await command.ExecuteReaderAsync())
                                {
                                    if (result3 != null)
                                    {
                                        while (result3.Read())
                                        {
                                            NfeMunicipio nfeMunicipio = new NfeMunicipio
                                            {
                                                CodMunic = result3["CodMunic"].ToString(),
                                                Descricao = result3["Descricao"].ToString()
                                            };

                                            lstNfeMunicipio.Add(nfeMunicipio);
                                        }
                                    }
                                }

                                command.Connection.Close();
                            }
                            else
                            {
                                command.Connection.Close();
                            }
                        }
                    }
                }
            }

            return lstNfeMunicipio;
        }

        public async Task<string> MontarProviderStringParaNFeMunicipio(ContextoBdProvider contextoProvider)
        {
            var db = contextoProvider.GetContextoLeitura();

            //buscando os dados para se conectar no servidor de banco de dados
            TnfEmitente nova_conexao = await (from c in db.TnfEmitentes
                                              where c.NFe_st_emitente_padrao == 1
                                              select new TnfEmitente
                                              {
                                                  NFe_T1_nome_BD = c.NFe_T1_nome_BD,
                                                  NFe_T1_servidor_BD = c.NFe_T1_servidor_BD,
                                                  NFe_T1_usuario_BD = c.NFe_T1_usuario_BD,
                                                  NFe_T1_senha_BD = c.NFe_T1_senha_BD
                                              }).FirstOrDefaultAsync();

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = nova_conexao.NFe_T1_servidor_BD;
            sqlBuilder.InitialCatalog = nova_conexao.NFe_T1_nome_BD;
            sqlBuilder.UserID = nova_conexao.NFe_T1_usuario_BD;

            sqlBuilder.Password = PrepedidoBusiness.Utils.Util.decodificaDado(nova_conexao.NFe_T1_senha_BD, Constantes.FATOR_BD);

            string providerString = sqlBuilder.ToString();

            return providerString;
        }

        public async Task<IEnumerable<UFeMunicipiosDto>> BuscarSiglaTodosUf(ContextoBdProvider contextoProvider)
        {
            List<UFeMunicipiosDto> lstUF_Municipio = new List<UFeMunicipiosDto>();
            List<MunicipioDto> lstMunicipios = new List<MunicipioDto>();

            string providerString = await MontarProviderStringParaNFeMunicipio(contextoProvider);


            using (SqlConnection sql = new SqlConnection(providerString))
            {
                string query = "SELECT *  FROM NFE_UF";

                SqlCommand command = new SqlCommand(query, sql);

                command.Connection.Open();

                using (var result = await command.ExecuteReaderAsync())
                {
                    if (result != null)
                    {

                        while (result.Read())
                        {
                            UFeMunicipiosDto ufMinicipio = new UFeMunicipiosDto();
                            ufMinicipio.Codigo = result["CodUF"].ToString();
                            ufMinicipio.Descricao = result["Descricao"].ToString();
                            ufMinicipio.SiglaUF = result["SiglaUf"].ToString();
                            ufMinicipio.ListaMunicipio = new List<MunicipioDto>();

                            lstUF_Municipio.Add(ufMinicipio);
                        };

                        command.Connection.Close();

                        query = "SELECT * FROM NFE_MUNICIPIO";

                        command = new SqlCommand(query, sql);

                        command.Connection.Open();

                        using (var result2 = await command.ExecuteReaderAsync())
                        {
                            if (result2 != null)
                            {

                                while (result2.Read())
                                {
                                    MunicipioDto municipio = new MunicipioDto();
                                    municipio.Codigo = result2["CodMunic"].ToString();
                                    municipio.Descricao = result2["Descricao"].ToString();
                                    municipio.DescricaoSemAcento = result2["DescricaoSemAcento"].ToString();

                                    lstMunicipios.Add(municipio);
                                }
                            }
                            else
                            {
                                command.Connection.Close();
                            }
                        }

                        lstUF_Municipio.ForEach(x =>
                        {
                            lstMunicipios.ForEach(y =>
                            {
                                string substr = y.Codigo.Substring(0, 2);

                                if (x.Codigo == substr)
                                {
                                    x.ListaMunicipio.Add(new MunicipioDto()
                                    {
                                        Codigo = y.Codigo,
                                        Descricao = y.Descricao,
                                        DescricaoSemAcento = y.DescricaoSemAcento
                                    });
                                }
                            });
                        });
                    }
                }
            }

            return lstUF_Municipio;
        }
    }
}
