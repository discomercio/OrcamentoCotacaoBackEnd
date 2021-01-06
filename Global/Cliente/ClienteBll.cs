using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InfraBanco.Constantes;
using Cep;
using Cliente.Dados;
using InfraBanco;

namespace Cliente
{
    public class ClienteBll
    {
        public static class MensagensErro
        {
            public static string REGISTRO_COM_ID_JA_EXISTE(string clienteDadosClienteId) { return "REGISTRO COM ID = " + clienteDadosClienteId + " JÁ EXISTE."; }
        }


        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;
        private readonly CepBll cepBll;
        private readonly IBancoNFeMunicipio bancoNFeMunicipio;

        public ClienteBll(InfraBanco.ContextoBdProvider contextoProvider,
                    InfraBanco.ContextoCepProvider contextoCepProvider, CepBll cepBll,
                    IBancoNFeMunicipio bancoNFeMunicipio)
        {
            this.contextoProvider = contextoProvider;
            this.contextoCepProvider = contextoCepProvider;
            this.cepBll = cepBll;
            this.bancoNFeMunicipio = bancoNFeMunicipio;
        }

        public string Verificar_AletrouDadosPF(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados, string apelido)
        {
            string log = "";
            bool contribuinte_diferente = false;
            bool ie_diferente = false;
            bool produtor_diferente = false;

            if (dados.ProdutorRural == (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)
            {
                if (dados.ProdutorRural != cli.Produtor_Rural_Status)
                {
                    log += "ie: " + cli.Ie + " => \"\"; ";
                    cli.Ie = "";

                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                        Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL + "; ";
                    cli.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;

                    log += "contribuinte_icms_data: " + cli.Contribuinte_Icms_Data + " => " + DateTime.Now + "; ";
                    cli.Contribuinte_Icms_Data = DateTime.Now;

                    log += "contribuinte_icms_data_hora: " + cli.Contribuinte_Icms_Data_Hora + " => " + DateTime.Now + "; ";
                    cli.Contribuinte_Icms_Data_Hora = DateTime.Now;

                    if (apelido.ToUpper() != cli.Contribuinte_Icms_Usuario.ToUpper())
                    {
                        log += "contribuinte_icms_usuario: " + cli.Contribuinte_Icms_Usuario.ToUpper() + " => " +
                                apelido.ToUpper() + "; ";
                        cli.Contribuinte_Icms_Usuario = apelido.ToUpper();
                    }

                    log += "produtor_rural_status: " + cli.Produtor_Rural_Status + " => " +
                        Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO + "; ";
                    cli.Produtor_Rural_Status = (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO;

                    log += "produtor_rural_data: " + cli.Produtor_Rural_Data + " => " + DateTime.Now + "; ";
                    cli.Produtor_Rural_Data = DateTime.Now;

                    log += "produtor_rural_data_hora: " + cli.Produtor_Rural_Data_Hora + " => " + DateTime.Now + "; ";
                    cli.Produtor_Rural_Data_Hora = DateTime.Now;

                    if (apelido.ToUpper() != cli.Produtor_Rural_Usuario.ToUpper())
                    {
                        log += "produtor_rural_usuario: " + cli.Produtor_Rural_Usuario.ToUpper() + " => " +
                                apelido.ToUpper() + "; ";
                        cli.Produtor_Rural_Usuario = apelido.ToUpper();
                    }
                }

            }

            if (dados.ProdutorRural == (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
            {
                if (dados.Contribuinte_Icms_Status != cli.Contribuinte_Icms_Status)
                {
                    contribuinte_diferente = true;
                }

                if (dados.Ie != cli.Ie)
                {
                    ie_diferente = true;
                }
                if (dados.ProdutorRural != cli.Produtor_Rural_Status)
                {
                    produtor_diferente = true;
                }
            }

            if (ie_diferente)
            {
                if (string.IsNullOrEmpty(dados.Ie))
                {
                    log += "ie: " + cli.Ie + " => \"\"; ";
                }
                else
                {
                    if (string.IsNullOrEmpty(cli.Ie))
                    {
                        log += "ie: \"\" => " + dados.Ie + "; ";
                    }
                    else
                    {
                        log += "ie: " + cli.Ie + " => " + dados.Ie + "; ";
                    }
                }

                cli.Ie = dados.Ie;
            }

            if (contribuinte_diferente)
            {
                if (dados.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                {
                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                                    Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM + "; ";
                    cli.Contribuinte_Icms_Status =
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
                }
                if (dados.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                {
                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                                    Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO + "; ";
                    cli.Contribuinte_Icms_Status =
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
                }
                if (dados.Contribuinte_Icms_Status == (short)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                                    Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO + "; ";
                    cli.Contribuinte_Icms_Status =
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO;
                }

                log += "contribuinte_icms_data: " + cli.Contribuinte_Icms_Data + " => " + DateTime.Now + "; ";
                cli.Contribuinte_Icms_Data = DateTime.Now;

                log += "contribuinte_icms_data_hora: " + cli.Contribuinte_Icms_Data_Hora + " => " + DateTime.Now + "; ";
                cli.Contribuinte_Icms_Data_Hora = DateTime.Now;

                if (cli.Contribuinte_Icms_Usuario.ToUpper() != apelido.ToUpper())
                {
                    //contribuinte_icms_usuario: 
                    log += "contribuinte_icms_usuario: " + cli.Contribuinte_Icms_Usuario.ToUpper() + " => " +
                        apelido.ToUpper() + "; ";
                    cli.Contribuinte_Icms_Usuario = apelido.ToUpper();
                }

            }

            if (produtor_diferente)
            {
                log += "produtor_rural_status: " + cli.Produtor_Rural_Status + " => " +
                            Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM + "; ";
                cli.Produtor_Rural_Status = (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM;

                if (cli.Produtor_Rural_Data == null)
                {
                    log += "produtor_rural_data: \"\" => " + DateTime.Now + "; ";
                }
                else
                {
                    log += "produtor_rural_data: " + cli.Produtor_Rural_Data + " => " + DateTime.Now + "; ";
                }

                cli.Produtor_Rural_Data = DateTime.Now;

                if (cli.Produtor_Rural_Data_Hora == null)
                {
                    log += "produtor_rural_data_hora: \"\" => " + DateTime.Now + "; ";
                }
                else
                {
                    log += "produtor_rural_data_hora: " + cli.Produtor_Rural_Data_Hora + " => " + DateTime.Now + "; ";
                }

                cli.Produtor_Rural_Data_Hora = DateTime.Now;

                if (!string.IsNullOrEmpty(cli.Produtor_Rural_Usuario))
                {
                    if (apelido.ToUpper() != cli.Produtor_Rural_Usuario.ToUpper())
                    {
                        log += "produtor_rural_usuario: " + cli.Produtor_Rural_Usuario.ToUpper() + " => " +
                                apelido.ToUpper() + "; ";
                        cli.Produtor_Rural_Usuario = apelido.ToUpper();
                    }
                }
                else
                {
                    log += "produtor_rural_usuario: \"\" => " + apelido.ToUpper() + "; ";
                    cli.Produtor_Rural_Usuario = apelido.ToUpper();
                }
            }

            if (cli.Sistema_responsavel_atualizacao != (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS)
            {
                log += "sistema_responsavel_atualizacao: " + cli.Sistema_responsavel_atualizacao + " => " +
                    Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS + "; ";
                cli.Sistema_responsavel_atualizacao = (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
                cli.Sistema_responsavel_cadastro = (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
            }

            return log;
        }

        public string Verificar_AlterouDadosPJ(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados)
        {
            string log = "";

            bool alterou = false;

            if (dados.Contribuinte_Icms_Status != cli.Contribuinte_Icms_Status)
            {
                if (dados.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                {
                    if (cli.Ie != dados.Ie)
                    {
                        if (string.IsNullOrEmpty(cli.Ie))
                        {
                            log += "ie: \"\" => " + dados.Ie + "; ";
                        }
                        else
                        {
                            log += "ie: " + cli.Ie + " => " + dados.Ie + "; ";
                        }

                        cli.Ie = dados.Ie;
                    }

                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                                    Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM + "; ";
                    cli.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;

                    alterou = true;
                }

                if (dados.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                {
                    if (cli.Ie != dados.Ie)
                    {
                        if (string.IsNullOrEmpty(dados.Ie))
                        {
                            log += "ie: " + cli.Ie + " => \"\"; ";
                        }

                        if (string.IsNullOrEmpty(cli.Ie))
                        {
                            log += "ie: \"\" => " + dados.Ie + "; ";
                        }

                        cli.Ie = string.IsNullOrEmpty(dados.Ie) ? "" : dados.Ie;
                    }

                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                        Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO + "; ";
                    cli.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;

                    alterou = true;
                }

                if (dados.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    log += "ie: " + cli.Ie + " => \"\"; ";
                    //não pode ter IE
                    cli.Ie = "";

                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                        Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO + "; ";
                    cli.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO;

                    alterou = true;
                }

                if (alterou)
                {
                    log += "contribuinte_icms_data: " + cli.Contribuinte_Icms_Data + " => " + DateTime.Now + "; ";
                    cli.Contribuinte_Icms_Data = DateTime.Now;

                    log += "contribuinte_icms_data_hora: " + cli.Contribuinte_Icms_Data_Hora + " => " + DateTime.Now + "; ";
                    cli.Contribuinte_Icms_Data_Hora = DateTime.Now;


                }
            }
            if (dados.Contribuinte_Icms_Status == cli.Contribuinte_Icms_Status)
            {
                if (dados.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                {
                    if (cli.Ie != dados.Ie)
                    {
                        if (string.IsNullOrEmpty(dados.Ie))
                        {
                            log += "ie: " + cli.Ie + " => \"\"; ";
                        }

                        if (string.IsNullOrEmpty(cli.Ie))
                        {
                            log += "ie: \"\" => " + dados.Ie + "; ";
                        }

                        cli.Ie = string.IsNullOrEmpty(dados.Ie) ? "" : dados.Ie;

                        alterou = true;
                    }
                }

                if (alterou)
                {
                    if (cli.Sistema_responsavel_atualizacao != (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS)
                    {
                        log += "sistema_responsavel_atualizacao: " + cli.Sistema_responsavel_atualizacao + " => " +
                            Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS + "; ";

                        cli.Sistema_responsavel_atualizacao = (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
                        cli.Sistema_responsavel_cadastro = (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
                    }
                }
            }

            return log;
        }

        public async Task<List<string>> AtualizarClienteParcial(string apelido, Cliente.Dados.DadosClienteCadastroDados dadosClienteCadastroDados, InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
        {
            /*
             * somente os seguintes campos serão atualizados:
             * produtor rural
             * inscrição estadual
             * tipo de contibuinte ICMS
             * */
            var db = contextoProvider.GetContextoLeitura();
            string log = "";

            List<string> lstErros = new List<string>();
            List<Cliente.Dados.ListaBancoDados> lstBanco = (await ListarBancosCombo()).ToList();

            await Cliente.ValidacoesClienteBll.ValidarDadosCliente(dadosClienteCadastroDados, null, null, lstErros,
                contextoProvider, cepBll, bancoNFeMunicipio, lstBanco, true, sistemaResponsavel, false);
            if (lstErros.Count > 0)
                return lstErros;


            var dados = from c in db.Tclientes
                        where c.Id == dadosClienteCadastroDados.Id
                        select c;
            var cli = await dados.FirstOrDefaultAsync();

            if (cli != null)
            {
                if (lstErros.Count == 0)
                {
                    //comparar os log em todos os casos de PF
                    if (dadosClienteCadastroDados.Tipo == Constantes.ID_PF)
                    {
                        log = Verificar_AletrouDadosPF(cli, dadosClienteCadastroDados, apelido);
                    }
                    if (dadosClienteCadastroDados.Tipo == Constantes.ID_PJ)
                    {
                        log = Verificar_AlterouDadosPJ(cli, dadosClienteCadastroDados);
                    }

                    if (!string.IsNullOrEmpty(log))
                    {
                        using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
                        {
                            cli.Dt_Ult_Atualizacao = DateTime.Now;
                            cli.Usuario_Ult_Atualizacao = apelido;

                            dbgravacao.Update(cli);
                            dbgravacao.SaveChanges();

                            bool salvouLog = UtilsGlobais.Util.GravaLog(dbgravacao, apelido, dadosClienteCadastroDados.Loja, "", dadosClienteCadastroDados.Id,
                                Constantes.OP_LOG_CLIENTE_ALTERACAO, log);
                            if (salvouLog)
                                dbgravacao.transacao.Commit();
                        }
                    }
                }
            }

            return lstErros;
        }

        public async Task<Cliente.Dados.ClienteCadastroDados> BuscarCliente(string cpf_cnpj, string apelido)
        {

            var db = contextoProvider.GetContextoLeitura();

            var dadosCliente = db.Tclientes.Where(r => r.Cnpj_Cpf == cpf_cnpj)
                .FirstOrDefault();

            if (dadosCliente == null)
                return null;

            var lojaOrcamentista = (from c in db.TorcamentistaEindicadors
                                    where c.Apelido == apelido
                                    select c.Loja).FirstOrDefaultAsync();

            var dadosClienteTask = ObterDadosClienteCadastro(dadosCliente, await lojaOrcamentista);
            var refBancariaTask = ObterReferenciaBancaria(dadosCliente);
            var refComercialTask = ObterReferenciaComercial(dadosCliente);

            Cliente.Dados.ClienteCadastroDados cliente = new Cliente.Dados.ClienteCadastroDados
            {
                DadosCliente = dadosClienteTask,
                RefBancaria = (await refBancariaTask).ToList(),
                RefComercial = (await refComercialTask).ToList()
            };

            return await Task.FromResult(cliente);
        }

        public async Task<IEnumerable<Cliente.Dados.ListaBancoDados>> ListarBancosCombo()
        {
            var db = contextoProvider.GetContextoLeitura();

            var bancos = from c in db.Tbancos
                         orderby c.Codigo
                         select new Cliente.Dados.ListaBancoDados
                         {
                             Codigo = c.Codigo,
                             Descricao = c.Descricao
                         };

            return await bancos.ToListAsync();
        }

        public async Task<IEnumerable<Cliente.Dados.EnderecoEntregaJustificativaDados>> ListarComboJustificaEndereco(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            string loja = await (from c in db.TorcamentistaEindicadors
                                 where c.Apelido == apelido
                                 select c.Loja).FirstOrDefaultAsync();

            return await ListarComboJustificaEnderecoPorLoja(db, loja);
        }

        public async Task<IEnumerable<EnderecoEntregaJustificativaDados>> ListarComboJustificaEnderecoPorLoja(ContextoBd db, string loja)
        {
            var retorno = from c in db.TcodigoDescricaos
                          where c.Grupo == Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA &&
                          (c.Lojas_Habilitadas == null || c.Lojas_Habilitadas.Length == 0 || c.Lojas_Habilitadas.Contains("|" + loja + "|")) &&
                          (c.St_Inativo == 0 || c.Codigo == "")
                          select new { c.Codigo, c.Descricao };

            List<Cliente.Dados.EnderecoEntregaJustificativaDados> lst = new List<Cliente.Dados.EnderecoEntregaJustificativaDados>();

            foreach (var r in await retorno.ToListAsync())
            {
                Cliente.Dados.EnderecoEntregaJustificativaDados jus = new Cliente.Dados.EnderecoEntregaJustificativaDados
                {
                    EndEtg_cod_justificativa = !string.IsNullOrEmpty(r.Codigo) && r.Codigo.Length == 1 && r.Codigo != "0" ?
                    "00" + r.Codigo : r.Codigo,
                    EndEtg_descricao_justificativa = r.Descricao
                };
                lst.Add(jus);
            }
            return lst;
        }

        public Cliente.Dados.DadosClienteCadastroDados ObterDadosClienteCadastro(Tcliente cli, string loja)
        {
            Cliente.Dados.DadosClienteCadastroDados dados = new Cliente.Dados.DadosClienteCadastroDados
            {
                Id = cli.Id,
                Indicador_Orcamentista = cli.Indicador,
                Cnpj_Cpf = cli.Cnpj_Cpf,
                Rg = cli.Rg,
                Ie = cli.Ie,
                Contribuinte_Icms_Status = cli.Contribuinte_Icms_Status,
                Tipo = cli.Tipo,
                Observacao_Filiacao = cli.Filiacao,
                Nascimento = cli.Dt_Nasc,
                Sexo = cli.Sexo,
                Nome = cli.Nome,
                ProdutorRural = cli.Produtor_Rural_Status,
                DddResidencial = cli.Ddd_Res,
                TelefoneResidencial = cli.Tel_Res,
                DddComercial = cli.Ddd_Com,
                TelComercial = cli.Tel_Com,
                Ramal = cli.Ramal_Com,
                DddCelular = cli.Ddd_Cel,
                Celular = cli.Tel_Cel,
                DddComercial2 = cli.Ddd_Com_2,
                TelComercial2 = cli.Tel_Com_2,
                Ramal2 = cli.Ramal_Com_2,
                Email = cli.Email,
                EmailXml = cli.Email_Xml,
                Endereco = cli.Endereco,
                Numero = cli.Endereco_Numero,
                Complemento = cli.Endereco_Complemento,
                Bairro = cli.Bairro,
                Cidade = cli.Cidade,
                Uf = cli.Uf,
                Cep = cli.Cep,
                Contato = cli.Contato,
                Loja = loja
            };

            return dados;
        }

        private async Task<IEnumerable<Cliente.Dados.Referencias.RefBancariaClienteDados>> ObterReferenciaBancaria(Tcliente cli)
        {
            List<Cliente.Dados.Referencias.RefBancariaClienteDados> lstRef = new List<Cliente.Dados.Referencias.RefBancariaClienteDados>();
            var db = contextoProvider.GetContextoLeitura();

            //selecionamos as referências bancárias já incluindo a descrição do banco
            var rBanco = from c in db.TclienteRefBancarias
                         join banco in db.Tbancos on c.Banco equals banco.Codigo
                         where c.Id_Cliente == cli.Id
                         orderby c.Ordem
                         select new { c.Banco, c.Agencia, c.Conta, c.Contato, c.Ddd, c.Telefone, banco.Descricao };

            foreach (var i in await rBanco.ToListAsync())
            {
                Cliente.Dados.Referencias.RefBancariaClienteDados refBanco = new Cliente.Dados.Referencias.RefBancariaClienteDados
                {
                    Banco = i.Banco,
                    BancoDescricao = i.Descricao,
                    Agencia = i.Agencia,
                    Conta = i.Conta,
                    Contato = i.Contato,
                    Ddd = i.Ddd,
                    Telefone = i.Telefone
                };
                lstRef.Add(refBanco);
            }

            return lstRef;
        }

        private async Task<IEnumerable<Cliente.Dados.Referencias.RefComercialClienteDados>> ObterReferenciaComercial(Tcliente cli)
        {
            List<Cliente.Dados.Referencias.RefComercialClienteDados> lstRefComercial = new List<Cliente.Dados.Referencias.RefComercialClienteDados>();
            var db = contextoProvider.GetContextoLeitura();

            var rComercial = from c in db.TclienteRefComercials
                             where c.Id_Cliente == cli.Id
                             orderby c.Ordem
                             select c;

            foreach (var i in await rComercial.ToListAsync())
            {
                Cliente.Dados.Referencias.RefComercialClienteDados rCom = new Cliente.Dados.Referencias.RefComercialClienteDados
                {
                    Nome_Empresa = i.Nome_Empresa,
                    Contato = i.Contato,
                    Ddd = i.Ddd,
                    Telefone = i.Telefone
                };

                lstRefComercial.Add(rCom);
            }

            return lstRefComercial;
        }

        /*
         * Incluímos a var "string usuarioCadastro" para permitir que a ApiUnis possa cadastrar outro
         * usuário ao invés do Orçamentista
         */
        public async Task<IEnumerable<string>> CadastrarCliente(Cliente.Dados.ClienteCadastroDados clienteCadastroDados, string indicador,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavelCadastro,
            string usuario_cadastro)
        {
            string id_cliente = "";

            var db = contextoProvider.GetContextoLeitura();
            var verifica = await (from c in db.Tclientes
                                  where c.Cnpj_Cpf == clienteCadastroDados.DadosCliente.Cnpj_Cpf
                                  select c.Id).FirstOrDefaultAsync();

            List<string> lstErros = new List<string>();

            if (verifica != null)
            {
                lstErros.Add(MensagensErro.REGISTRO_COM_ID_JA_EXISTE(verifica));
                return lstErros;
            }

            //passar lista de bancos para validar
            List<Cliente.Dados.ListaBancoDados> lstBanco = (await ListarBancosCombo()).ToList();
            await Cliente.ValidacoesClienteBll.ValidarDadosCliente(clienteCadastroDados.DadosCliente,
                clienteCadastroDados.RefBancaria,
                clienteCadastroDados.RefComercial,
                lstErros, contextoProvider, cepBll, bancoNFeMunicipio, lstBanco, false, sistemaResponsavelCadastro, true);
            if (lstErros.Count != 0)
                return lstErros;

            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                string log = "";

                Cliente.Dados.DadosClienteCadastroDados cliente = clienteCadastroDados.DadosCliente;
                Tcliente clienteCadastrado = new Tcliente();
                id_cliente = await CadastrarDadosClienteDados(dbgravacao, cliente, indicador, clienteCadastrado,
                    sistemaResponsavelCadastro, usuario_cadastro);

                //Por padrão o id do cliente tem 12 caracteres, caso não seja 12 caracteres esta errado
                if (id_cliente.Length == 12)
                {
                    string campos_a_omitir = "dt_cadastro|usuario_cadastro|dt_ult_atualizacao|usuario_ult_atualizacao";

                    log = UtilsGlobais.Util.MontaLog(clienteCadastrado, log, campos_a_omitir);

                    if (clienteCadastroDados.DadosCliente.Tipo == Constantes.ID_PJ)
                    {
                        log = await CadastrarRefBancaria(dbgravacao, clienteCadastroDados.RefBancaria, usuario_cadastro, id_cliente, log);
                        log = await CadastrarRefComercial(dbgravacao, clienteCadastroDados.RefComercial, usuario_cadastro, id_cliente, log);
                    }

                    bool gravouLog = UtilsGlobais.Util.GravaLog(dbgravacao, usuario_cadastro, cliente.Loja, "", id_cliente,
                            Constantes.OP_LOG_CLIENTE_INCLUSAO, log);
                    if (gravouLog)
                        dbgravacao.transacao.Commit();

                }
                else
                {
                    lstErros.Add("Erro: id_cliente com tamanho diferente de 12.");
                }
            }


            return lstErros;
        }

        private async Task<string> CadastrarDadosClienteDados(InfraBanco.ContextoBdGravacao dbgravacao,
            Cliente.Dados.DadosClienteCadastroDados clienteDados, string indicador, Tcliente tCliente,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavelCadastro,
            string usuario_cadastro)
        {
            string id_cliente = await GerarIdCliente(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES);

            if (usuario_cadastro != null)
                usuario_cadastro = usuario_cadastro.ToUpper();
            if (indicador != null)
                indicador = indicador.ToUpper();

            tCliente.Id = id_cliente;
            tCliente.Dt_Cadastro = DateTime.Now;
            tCliente.Usuario_Cadastro = usuario_cadastro;
            tCliente.Indicador = indicador ?? ""; //não deve ser null
            tCliente.Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(clienteDados.Cnpj_Cpf);
            tCliente.Tipo = clienteDados.Tipo.ToUpper();
            tCliente.Ie = clienteDados.Ie;
            tCliente.Rg = clienteDados.Rg;
            tCliente.Nome = clienteDados.Nome;
            tCliente.Sexo = clienteDados.Sexo;
            tCliente.Contribuinte_Icms_Status = clienteDados.Contribuinte_Icms_Status;
            tCliente.Contribuinte_Icms_Data = DateTime.Now;
            tCliente.Contribuinte_Icms_Data_Hora = DateTime.Now;
            tCliente.Contribuinte_Icms_Usuario = usuario_cadastro;
            tCliente.Produtor_Rural_Status = clienteDados.ProdutorRural;
            if (clienteDados.ProdutorRural != (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL)
            {
                tCliente.Produtor_Rural_Data = DateTime.Now;
                tCliente.Produtor_Rural_Data_Hora = DateTime.Now;
                tCliente.Produtor_Rural_Usuario = usuario_cadastro;
            }

            tCliente.Endereco = clienteDados.Endereco;
            tCliente.Endereco_Numero = clienteDados.Numero;
            tCliente.Endereco_Complemento = clienteDados.Complemento;
            tCliente.Bairro = clienteDados.Bairro;
            tCliente.Cidade = clienteDados.Cidade;
            tCliente.Cep = clienteDados.Cep.Replace("-", "");
            tCliente.Uf = clienteDados.Uf;
            tCliente.Ddd_Res = clienteDados.DddResidencial;
            tCliente.Tel_Res = clienteDados.TelefoneResidencial;
            tCliente.Ddd_Com = clienteDados.DddComercial;
            tCliente.Tel_Com = clienteDados.TelComercial;
            tCliente.Ramal_Com = clienteDados.Ramal;
            tCliente.Contato = clienteDados.Contato == null ? "" : clienteDados.Contato;
            tCliente.Ddd_Com_2 = clienteDados.DddComercial2;
            tCliente.Tel_Com_2 = clienteDados.TelComercial2;
            tCliente.Ramal_Com_2 = clienteDados.Ramal2;
            tCliente.Ddd_Cel = clienteDados.DddCelular;
            tCliente.Tel_Cel = clienteDados.Celular;

            //definição em 20200930: data de nascimento é somente a data, sem hora
            if (clienteDados.Nascimento.HasValue)
                tCliente.Dt_Nasc = clienteDados.Nascimento.Value.Date;
            else
                tCliente.Dt_Nasc = null;

            tCliente.Filiacao = clienteDados.Observacao_Filiacao == null ? "" : clienteDados.Observacao_Filiacao;
            tCliente.Obs_crediticias = "";
            tCliente.Midia = "";
            tCliente.Email = clienteDados.Email;
            tCliente.Email_Xml = clienteDados.EmailXml;
            tCliente.Dt_Ult_Atualizacao = DateTime.Now;
            tCliente.Usuario_Ult_Atualizacao = usuario_cadastro;
            tCliente.Sistema_responsavel_cadastro = (int)sistemaResponsavelCadastro;
            tCliente.Sistema_responsavel_atualizacao = (int)sistemaResponsavelCadastro;

            dbgravacao.Add(tCliente);
            await dbgravacao.SaveChangesAsync();
            return id_cliente;
        }

        private async Task<string> CadastrarRefBancaria(InfraBanco.ContextoBdGravacao dbgravacao, List<Cliente.Dados.Referencias.RefBancariaClienteDados> lstRefBancaria, string apelido, string id_cliente, string log)
        {
            int qtdeRef = 1;
            string campos_a_omitir_ref_bancaria = "id_cliente|ordem|excluido_status|dt_cadastro|usuario_cadastro";

            foreach (Cliente.Dados.Referencias.RefBancariaClienteDados r in lstRefBancaria)
            {
                log = log + "Ref Bancária incluída: ";
                TclienteRefBancaria cliRefBancaria = new TclienteRefBancaria
                {
                    Id_Cliente = id_cliente,
                    Banco = r.Banco,
                    Agencia = r.Agencia,
                    Conta = r.Conta,
                    Dt_Cadastro = DateTime.Now,
                    Usuario_Cadastro = apelido,
                    Ordem = (short)(qtdeRef),
                    Ddd = r.Ddd,
                    Telefone = r.Telefone,
                    Contato = r.Contato,
                    Excluido_Status = 0
                };
                dbgravacao.Add(cliRefBancaria);
                qtdeRef++;

                //Busca os nomes reais das colunas da tabela SQL
                log = UtilsGlobais.Util.MontaLog(cliRefBancaria, log, campos_a_omitir_ref_bancaria);
            }

            await dbgravacao.SaveChangesAsync();
            return log;
        }

        private async Task<string> CadastrarRefComercial(InfraBanco.ContextoBdGravacao dbgravacao,
            List<Cliente.Dados.Referencias.RefComercialClienteDados> lstRefComercial, string apelido, string id_cliente, string log)
        {
            int qtdeRef = 1;

            string campos_a_omitir_ref_comercial = "id_cliente|ordem|excluido_status|dt_cadastro|usuario_cadastro";

            foreach (Cliente.Dados.Referencias.RefComercialClienteDados r in lstRefComercial)
            {
                log = log + "Ref Comercial incluída: ";
                TclienteRefComercial c = new TclienteRefComercial
                {
                    Id_Cliente = id_cliente,
                    Nome_Empresa = r.Nome_Empresa,
                    Dt_Cadastro = DateTime.Now,
                    Usuario_Cadastro = apelido,
                    Ordem = (short)qtdeRef,//verificar se esta sendo inserido a qtde na validação
                    Contato = r.Contato,
                    Ddd = r.Ddd,
                    Telefone = r.Telefone,
                    Excluido_Status = 0
                };
                dbgravacao.Add(c);
                qtdeRef++;
                log = UtilsGlobais.Util.MontaLog(c, log, campos_a_omitir_ref_comercial);
            }

            await dbgravacao.SaveChangesAsync();
            return log;
        }

        private Task<string> GerarIdCliente(InfraBanco.ContextoBdGravacao dbgravacao, string id_nsu)
        {
            return UtilsGlobais.Util.GerarNsu(dbgravacao, id_nsu);
        }

        public async Task<bool> ClienteExiste(string cpf_cnpj)
        {
            var db = contextoProvider.GetContextoLeitura();
            cpf_cnpj = UtilsGlobais.Util.SoDigitosCpf_Cnpj(cpf_cnpj);
            var retorno = await ((from c in db.Tclientes
                                  where c.Cnpj_Cpf == cpf_cnpj
                                  select c.Id).AnyAsync());
            return retorno;
        }


        public async Task<string> BuscaListaOperacoesPermitidas(string apelido)
        {
            string retorno = "";
            var db = contextoProvider.GetContextoLeitura();
            //            SELECT DISTINCT id_operacao
            //FROM t_PERFIL
            //INNER JOIN t_PERFIL_ITEM ON t_PERFIL.id = t_PERFIL_ITEM.id_perfil
            //INNER JOIN t_PERFIL_X_USUARIO ON t_PERFIL.id = t_PERFIL_X_USUARIO.id_perfil
            //INNER JOIN t_OPERACAO ON(t_PERFIL_ITEM.id_operacao= t_OPERACAO.id)
            //WHERE(t_PERFIL_X_USUARIO.usuario = 'pragmatica') AND
            //      (t_PERFIL.st_inativo = 0) AND
            //      (t_OPERACAO.st_inativo = 0)
            //ORDER BY id_operacao

            var lstTask = (from c in db.Tperfils
                          .Include(r => r.TperfilItem)
                          .Include(r => r.TperfilUsuario)
                          .Include(r => r.TperfilItem.Toperacao)
                           where c.TperfilUsuario.Usuario == apelido &&
                                 c.St_inativo == 0 &&
                                 c.TperfilItem.Toperacao.St_inativo == 0
                           orderby c.TperfilItem.Id_operacao
                           select c.TperfilItem.Id_operacao).Distinct();



            foreach (var i in await lstTask.ToListAsync())
            {
                retorno = retorno + "|" + i;
            }

            if (!string.IsNullOrEmpty(retorno))
            {
                if (retorno.Substring(retorno.Length - 1, 1) != "" || retorno.Substring(retorno.Length - 1, 1) != "|")
                    retorno = retorno + "|";
            }

            return retorno;
        }
    }
}
