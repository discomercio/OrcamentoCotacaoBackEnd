using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PrepedidoBusiness.Dtos.ClienteCadastro;
using PrepedidoBusiness.Dto.ClienteCadastro.Referencias;
using Microsoft.EntityFrameworkCore;
using PrepedidoBusiness.Dto.ClienteCadastro;
using InfraBanco.Constantes;
using System.Reflection;
using PrepedidoBusiness.Dto.Cep;
using System.Data.SqlClient;

namespace PrepedidoBusiness.Bll
{
    public class ClienteBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;

        public ClienteBll(InfraBanco.ContextoBdProvider contextoProvider,
            InfraBanco.ContextoCepProvider contextoCepProvider)
        {
            this.contextoProvider = contextoProvider;
            this.contextoCepProvider = contextoCepProvider;
        }

        public string Verificar_AletrouDadosPF(Tcliente cli, DadosClienteCadastroDto dados, string apelido)
        {
            string log = "";
            bool contribuinte_diferente = false;
            bool ie_diferente = false;
            bool produtor_diferente = false;

            if (dados.ProdutorRural == byte.Parse(Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO))
            {
                if (dados.ProdutorRural != cli.Produtor_Rural_Status)
                {
                    log += "ie: " + cli.Ie + " => \"\"; ";
                    cli.Ie = "";

                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                        Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL + "; ";
                    cli.Contribuinte_Icms_Status = byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL);

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
                        Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO + "; ";
                    cli.Produtor_Rural_Status = byte.Parse(Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO);

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

            if (dados.ProdutorRural == byte.Parse(Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM))
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
                if (dados.Contribuinte_Icms_Status == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM))
                {
                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                                    Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM + "; ";
                    cli.Contribuinte_Icms_Status =
                        byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM);
                }
                if (dados.Contribuinte_Icms_Status == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO))
                {
                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                                    Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO + "; ";
                    cli.Contribuinte_Icms_Status =
                        byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO);
                }
                if (dados.Contribuinte_Icms_Status == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO))
                {
                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                                    Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO + "; ";
                    cli.Contribuinte_Icms_Status =
                        byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO);
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
                            Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM + "; ";
                cli.Produtor_Rural_Status = byte.Parse(Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM);

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

            if (cli.Sistema_responsavel_atualizacao != Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS)
            {
                log += "sistema_responsavel_atualizacao: " + cli.Sistema_responsavel_atualizacao + " => " +
                    Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS + "; ";
                cli.Sistema_responsavel_atualizacao = Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
                cli.Sistema_responsavel_cadastro = Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
            }

            return log;
        }

        public string Verificar_AlterouDadosPJ(Tcliente cli, DadosClienteCadastroDto dados, string apelido)
        {
            string log = "";

            bool alterou = false;

            if (dados.Contribuinte_Icms_Status != cli.Contribuinte_Icms_Status)
            {
                if (dados.Contribuinte_Icms_Status == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM))
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
                                    Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM + "; ";
                    cli.Contribuinte_Icms_Status = byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM);

                    alterou = true;
                }

                if (dados.Contribuinte_Icms_Status == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO))
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
                        Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO + "; ";
                    cli.Contribuinte_Icms_Status = byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO);

                    alterou = true;
                }

                if (dados.Contribuinte_Icms_Status == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO))
                {
                    log += "ie: " + cli.Ie + " => \"\"; ";
                    //não pode ter IE
                    cli.Ie = "";

                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                        Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO + "; ";
                    cli.Contribuinte_Icms_Status = byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO);

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
                if (dados.Contribuinte_Icms_Status == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO))
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
                    if (cli.Sistema_responsavel_atualizacao != Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS)
                    {
                        log += "sistema_responsavel_atualizacao: " + cli.Sistema_responsavel_atualizacao + " => " +
                            Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS + "; ";

                        cli.Sistema_responsavel_atualizacao = Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
                        cli.Sistema_responsavel_cadastro = Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
                    }
                }
            }

            return log;
        }

        public async Task<List<string>> AtualizarClienteParcial(string apelido, DadosClienteCadastroDto dadosClienteCadastroDto)
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
            await ValidarDadosClientesCadastro(dadosClienteCadastroDto, lstErros);

            var dados = from c in db.Tclientes
                        where c.Id == dadosClienteCadastroDto.Id
                        select c;
            var cli = await dados.FirstOrDefaultAsync();

            if (cli != null)
            {
                if (lstErros.Count == 0)
                {
                    //comparar os log em todos os casos de PF
                    if (dadosClienteCadastroDto.Tipo == Constantes.ID_PF)
                    {
                        log = Verificar_AletrouDadosPF(cli, dadosClienteCadastroDto, apelido);
                    }
                    if (dadosClienteCadastroDto.Tipo == Constantes.ID_PJ)
                    {
                        log = Verificar_AlterouDadosPJ(cli, dadosClienteCadastroDto, apelido);
                    }

                    if (!string.IsNullOrEmpty(log))
                    {
                        using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
                        {
                            cli.Dt_Ult_Atualizacao = DateTime.Now;
                            cli.Usuario_Ult_Atualizacao = apelido;

                            dbgravacao.Update(cli);
                            dbgravacao.SaveChanges();

                            bool salvouLog = Utils.Util.GravaLog(dbgravacao, apelido, dadosClienteCadastroDto.Loja, "", dadosClienteCadastroDto.Id,
                                Constantes.OP_LOG_CLIENTE_ALTERACAO, log);
                            if (salvouLog)
                                dbgravacao.transacao.Commit();
                        }
                    }
                }
            }
            else
            {
                lstErros.Add("Registro do cliente não encontrado.");
            }

            return lstErros;
        }

        public async Task<ClienteCadastroDto> BuscarCliente(string cpf_cnpj, string apelido)
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

            ClienteCadastroDto cliente = new ClienteCadastroDto
            {
                DadosCliente = dadosClienteTask,
                RefBancaria = (await refBancariaTask).ToList(),
                RefComercial = (await refComercialTask).ToList()
            };

            return await Task.FromResult(cliente);
        }

        public async Task<IEnumerable<ListaBancoDto>> ListarBancosCombo()
        {
            var db = contextoProvider.GetContextoLeitura();

            var bancos = from c in db.Tbancos
                         orderby c.Codigo
                         select new ListaBancoDto
                         {
                             Codigo = c.Codigo,
                             Descricao = c.Descricao
                         };

            return await bancos.ToListAsync();
        }

        public async Task<IEnumerable<EnderecoEntregaJustificativaDto>> ListarComboJustificaEndereco(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            string loja = await (from c in db.TorcamentistaEindicadors
                                 where c.Apelido == apelido
                                 select c.Loja).FirstOrDefaultAsync();

            var retorno = from c in db.TcodigoDescricaos
                          where c.Grupo == Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA &&
                          (c.Lojas_Habilitadas == null || c.Lojas_Habilitadas.Length == 0 || c.Lojas_Habilitadas.Contains("|" + loja + "|")) &&
                          (c.St_Inativo == 0 || c.Codigo == "")
                          select new { c.Codigo, c.Descricao };

            List<EnderecoEntregaJustificativaDto> lst = new List<EnderecoEntregaJustificativaDto>();

            foreach (var r in await retorno.ToListAsync())
            {
                EnderecoEntregaJustificativaDto jus = new EnderecoEntregaJustificativaDto
                {
                    EndEtg_cod_justificativa = r.Codigo,
                    EndEtg_descricao_justificativa = r.Descricao
                };
                lst.Add(jus);
            }
            return lst;
        }

        public DadosClienteCadastroDto ObterDadosClienteCadastro(Tcliente cli, string loja)
        {
            DadosClienteCadastroDto dados = new DadosClienteCadastroDto
            {
                Id = cli.Id,
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

        private async Task<IEnumerable<RefBancariaDtoCliente>> ObterReferenciaBancaria(Tcliente cli)
        {
            List<RefBancariaDtoCliente> lstRef = new List<RefBancariaDtoCliente>();
            var db = contextoProvider.GetContextoLeitura();

            //selecionamos as referências bancárias já incluindo a descrição do banco
            var rBanco = from c in db.TclienteRefBancarias
                         join banco in db.Tbancos on c.Banco equals banco.Codigo
                         where c.Id_Cliente == cli.Id
                         orderby c.Ordem
                         select new { c.Banco, c.Agencia, c.Conta, c.Contato, c.Ddd, c.Telefone, banco.Descricao };

            foreach (var i in await rBanco.ToListAsync())
            {
                RefBancariaDtoCliente refBanco = new RefBancariaDtoCliente
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

        private async Task<IEnumerable<RefComercialDtoCliente>> ObterReferenciaComercial(Tcliente cli)
        {
            List<RefComercialDtoCliente> lstRefComercial = new List<RefComercialDtoCliente>();
            var db = contextoProvider.GetContextoLeitura();

            var rComercial = from c in db.TclienteRefComercials
                             where c.Id_Cliente == cli.Id
                             orderby c.Ordem
                             select c;

            foreach (var i in await rComercial.ToListAsync())
            {
                RefComercialDtoCliente rCom = new RefComercialDtoCliente
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

        public async Task<IEnumerable<string>> CadastrarCliente(ClienteCadastroDto clienteDto, string apelido)
        {
            string id_cliente = "";

            var db = contextoProvider.GetContextoLeitura();
            var verifica = await (from c in db.Tclientes
                                  where c.Id == clienteDto.DadosCliente.Id
                                  select c.Id).FirstOrDefaultAsync();

            List<string> lstErros = new List<string>();

            //Na validação do cadastro é feito a consistencia de Municipio
            await ValidarDadosClientesCadastro(clienteDto.DadosCliente, lstErros);
            ValidarRefBancaria(clienteDto.RefBancaria, lstErros);
            ValidarRefComercial(clienteDto.RefComercial, lstErros);

            if (verifica != null)
                lstErros.Add("REGISTRO COM ID=" + clienteDto.DadosCliente.Id + " JÁ EXISTE.");

            if (lstErros.Count <= 0)
            {
                using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
                {
                    string log = "";

                    DadosClienteCadastroDto cliente = clienteDto.DadosCliente;
                    Tcliente clienteCadastrado = new Tcliente();
                    id_cliente = await CadastrarDadosClienteDto(dbgravacao, cliente, apelido, log, cliente.Loja, clienteCadastrado);

                    //Por padrão o id do cliente tem 12 caracteres, caso não seja 12 caracteres esta errado
                    if (id_cliente.Length == 12)
                    {
                        string campos_a_omitir = "dt_cadastro|usuario_cadastro|dt_ult_atualizacao|usuario_ult_atualizacao";

                        log = Utils.Util.MontaLog(clienteCadastrado, log, campos_a_omitir);

                        if (clienteDto.DadosCliente.Tipo == Constantes.ID_PJ)
                        {
                            log = await CadastrarRefBancaria(dbgravacao, clienteDto.RefBancaria, apelido, id_cliente, log);
                            log = await CadastrarRefComercial(dbgravacao, clienteDto.RefComercial, apelido, id_cliente, log);
                        }

                        bool gravouLog = Utils.Util.GravaLog(dbgravacao, apelido, cliente.Loja, "", id_cliente,
                                Constantes.OP_LOG_CLIENTE_INCLUSAO, log);
                        if (gravouLog)
                            dbgravacao.transacao.Commit();

                    }
                    else
                    {
                        lstErros.Add(id_cliente);
                    }
                }
            }
            return lstErros;
        }

        private async Task<string> CadastrarDadosClienteDto(InfraBanco.ContextoBdGravacao dbgravacao,
            DadosClienteCadastroDto clienteDto, string apelido, string log, string loja, Tcliente tCliente)
        {
            string retorno = "";
            List<string> lstRetorno = new List<string>();
            string id_cliente = await GerarIdCliente(dbgravacao, Constantes.NSU_CADASTRO_CLIENTES);

            lstRetorno.Add(id_cliente);

            if (id_cliente.Length > 12)
                retorno = id_cliente;
            else
            {
                tCliente.Id = id_cliente;
                tCliente.Dt_Cadastro = DateTime.Now;
                tCliente.Usuario_Cadastrado = apelido.ToUpper();
                tCliente.Indicador = apelido.ToUpper();
                tCliente.Cnpj_Cpf = clienteDto.Cnpj_Cpf.Replace(".", "").Replace("/", "").Replace("-", "");
                tCliente.Tipo = clienteDto.Tipo.ToUpper();
                tCliente.Ie = clienteDto.Ie;
                tCliente.Rg = clienteDto.Rg;
                tCliente.Nome = clienteDto.Nome;
                tCliente.Sexo = clienteDto.Sexo;
                tCliente.Contribuinte_Icms_Status = clienteDto.Contribuinte_Icms_Status;
                tCliente.Contribuinte_Icms_Data = DateTime.Now;
                tCliente.Contribuinte_Icms_Data_Hora = DateTime.Now;
                tCliente.Contribuinte_Icms_Usuario = apelido.ToUpper();
                tCliente.Produtor_Rural_Status = clienteDto.ProdutorRural;
                tCliente.Produtor_Rural_Data = DateTime.Now;
                tCliente.Produtor_Rural_Data_Hora = DateTime.Now;
                tCliente.Produtor_Rural_Usuario = apelido.ToUpper();
                tCliente.Endereco = clienteDto.Endereco;
                tCliente.Endereco_Numero = clienteDto.Numero;
                tCliente.Endereco_Complemento = clienteDto.Complemento;
                tCliente.Bairro = clienteDto.Bairro;
                tCliente.Cidade = clienteDto.Cidade;
                tCliente.Cep = clienteDto.Cep.Replace("-", "");
                tCliente.Uf = clienteDto.Uf;
                tCliente.Ddd_Res = clienteDto.DddResidencial;
                tCliente.Tel_Res = clienteDto.TelefoneResidencial;
                tCliente.Ddd_Com = clienteDto.DddComercial;
                tCliente.Tel_Com = clienteDto.TelComercial;
                tCliente.Ramal_Com = clienteDto.Ramal;
                tCliente.Contato = clienteDto.Contato;
                tCliente.Ddd_Com_2 = clienteDto.DddComercial2;
                tCliente.Tel_Com_2 = clienteDto.TelComercial2;
                tCliente.Ramal_Com_2 = clienteDto.Ramal2;
                tCliente.Ddd_Cel = clienteDto.DddCelular;
                tCliente.Tel_Cel = clienteDto.Celular;
                tCliente.Dt_Nasc = clienteDto.Nascimento;
                tCliente.Filiacao = clienteDto.Observacao_Filiacao;
                tCliente.Obs_crediticias = "";
                tCliente.Midia = "";
                tCliente.Email = clienteDto.Email;
                tCliente.Email_Xml = clienteDto.EmailXml;
                tCliente.Dt_Ult_Atualizacao = DateTime.Now;
                tCliente.Usuario_Ult_Atualizacao = apelido.ToUpper();
                tCliente.Sistema_responsavel_cadastro = Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
                tCliente.Sistema_responsavel_atualizacao = Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
            };

            dbgravacao.Add(tCliente);
            await dbgravacao.SaveChangesAsync();
            retorno = tCliente.Id;

            return retorno;
        }

        private async Task<string> CadastrarRefBancaria(InfraBanco.ContextoBdGravacao dbgravacao, List<RefBancariaDtoCliente> lstRefBancaria, string apelido, string id_cliente, string log)
        {
            int qtdeRef = 1;
            string campos_a_omitir_ref_bancaria = "id_cliente|ordem|excluido_status|dt_cadastro|usuario_cadastro";

            log = log + "Ref Bancária incluída: ";

            foreach (RefBancariaDtoCliente r in lstRefBancaria)
            {

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
                log = Utils.Util.MontaLog(cliRefBancaria, log, campos_a_omitir_ref_bancaria);
            }

            await dbgravacao.SaveChangesAsync();
            return log;
        }

        private async Task<string> CadastrarRefComercial(InfraBanco.ContextoBdGravacao dbgravacao, List<RefComercialDtoCliente> lstRefComercial, string apelido, string id_cliente, string log)
        {
            int qtdeRef = 1;

            string campos_a_omitir_ref_comercial = "id_cliente|ordem|excluido_status|dt_cadastro|usuario_cadastro";

            log = log + "Ref Comercial incluída: ";

            foreach (RefComercialDtoCliente r in lstRefComercial)
            {
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
                log = Utils.Util.MontaLog(c, log, campos_a_omitir_ref_comercial);
            }

            await dbgravacao.SaveChangesAsync();
            return log;
        }

        private List<string> ValidarRefBancaria(List<RefBancariaDtoCliente> lstRefBancaria, List<string> lstErros)
        {
            for (int i = 0; i < lstRefBancaria.Count; i++)
            {
                if (string.IsNullOrEmpty(lstRefBancaria[i].Banco))
                    lstErros.Add("Ref Bancária (" + lstRefBancaria[i].Ordem.ToString() + "): informe o banco.");
                if (string.IsNullOrEmpty(lstRefBancaria[i].Agencia))
                    lstErros.Add("Ref Bancária (" + lstRefBancaria[i].Ordem.ToString() + "): informe o agência.");
                if (string.IsNullOrEmpty(lstRefBancaria[i].Conta))
                    lstErros.Add("Ref Bancária (" + lstRefBancaria[i].Ordem.ToString() + "): informe o número da conta.");
            }

            return lstErros;
        }

        private List<string> ValidarRefComercial(List<RefComercialDtoCliente> lstRefComercial, List<string> lstErros)
        {
            for (int i = 0; i < lstRefComercial.Count; i++)
            {
                lstRefComercial[i].Ordem = i;
                if (string.IsNullOrEmpty(lstRefComercial[i].Nome_Empresa))
                    lstErros.Add("Ref Comercial (" + lstRefComercial[i].Ordem + "): informe o nome da empresa.");
            }

            return lstErros;
        }

        private async Task<IEnumerable<NfeMunicipio>> ConsisteMunicipioIBGE(string municipio, string uf, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();
            List<NfeMunicipio> lst_nfeMunicipios = new List<NfeMunicipio>();

            if (string.IsNullOrEmpty(municipio))
                lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                    "nenhum município foi informado!");
            if (string.IsNullOrEmpty(uf))
                lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                    "a UF não foi informada!");
            else
            {
                if (uf.Length > 2)
                    lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                        "a UF é inválida (" + uf + ")!");
            }

            if (lstErros.Count == 0)
            {
                lst_nfeMunicipios = (await BuscarSiglaUf(uf, municipio)).ToList();

                if (!lst_nfeMunicipios.Any())
                {
                    lstErros.Add("Município '" + municipio + "' não consta na relação de municípios do IBGE para a UF de '" + uf + "'!");
                }
            }

            return lst_nfeMunicipios;
        }

        public async Task<IEnumerable<NfeMunicipio>> BuscarSiglaUf(string uf, string municipio)
        {
            List<NfeMunicipio> lstNfeMunicipio = new List<NfeMunicipio>();

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

            sqlBuilder.Password = Utils.Util.decodificaDado(nova_conexao.NFe_T1_senha_BD, Constantes.FATOR_BD);


            string providerString = sqlBuilder.ToString();

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
                            else
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
                        }
                    }
                }
            }

            return lstNfeMunicipio;
        }


        private async Task ValidarDadosClientesCadastro(DadosClienteCadastroDto cliente, List<string> listaErros)
        {
            string cpf_cnpjSoDig = Utils.Util.SoDigitosCpf_Cnpj(cliente.Cnpj_Cpf);


            if (cliente.Cnpj_Cpf == "")
                listaErros.Add("CNPJ / CPF NÃO FORNECIDO.");

            if (Utils.Util.ValidaCPF(cpf_cnpjSoDig))
            {
                if (cliente.Sexo != "M" && cliente.Sexo != "F")
                    listaErros.Add("INDIQUE QUAL O SEXO.");
                if (cliente.Nome == "")
                {
                    if (cliente.Tipo == Constantes.ID_PF)
                        listaErros.Add("PREENCHA O NOME DO CLIENTE.");
                }

                if (cliente.Tipo == Constantes.ID_PF &&
                cliente.TelefoneResidencial == "" &&
                cliente.TelComercial == "" &&
                cliente.Celular == "")
                    listaErros.Add("PREENCHA PELO MENOS UM TELEFONE.");
                else if (cliente.DddResidencial.Length != 2 && cliente.DddResidencial != "")
                    listaErros.Add("DDD INVÁLIDO.");
                else if (cliente.TelefoneResidencial.Length < 6 && cliente.TelefoneResidencial != "")
                    listaErros.Add("TELEFONE RESIDENCIAL INVÁLIDO.");
                else if (cliente.DddResidencial != "" && cliente.TelefoneResidencial == "")
                    listaErros.Add("PREENCHA O TELEFONE RESIDENCIAL.");
                else if (cliente.DddResidencial == "" && cliente.TelefoneResidencial != "")
                    listaErros.Add("PREENCHA O DDD.");
                else if (cliente.TelComercial != "" && cliente.DddComercial == "")
                    listaErros.Add("PREENCHA O DDD COMERCIAL.");
                else if (cliente.DddComercial != "" && cliente.TelComercial == "")
                    listaErros.Add("PREENCHA O TELEFONE COMERCIAL.");
            }
            if (Utils.Util.ValidaCNPJ(cpf_cnpjSoDig))
            {
                if (cliente.Tipo == Constantes.ID_PJ)
                {
                    if (cliente.Nome == "")
                        listaErros.Add("PREENCHA A RAZÃO SOCIAL DO CLIENTE.");
                    if (cliente.TelComercial == "" && cliente.TelComercial2 == "")
                        listaErros.Add("PREENCHA O TELEFONE.");
                    if (cliente.DddComercial.Length != 2 && cliente.DddComercial != "")
                        listaErros.Add("DDD INVÁLIDO.");
                    else if (cliente.TelComercial.Length < 6 && cliente.TelComercial != "")
                        listaErros.Add("TELEFONE COMERCIAL INVÁLIDO.");
                    else if (cliente.DddComercial != "" && cliente.TelComercial == "")
                        listaErros.Add("PREENCHA O TELEFONE COMERCIAL.");
                    else if (cliente.DddComercial == "" && cliente.TelComercial != "")
                        listaErros.Add("PREENCHA O DDD.");
                    if (string.IsNullOrEmpty(cliente.Contato))
                        listaErros.Add("Informe o nome da pessoa para contato!");
                    if (string.IsNullOrEmpty(cliente.Email))
                        listaErros.Add("É obrigatório informar um endereço de e-mail!");
                }
            }

            if (cliente.Endereco == "")
                listaErros.Add("PREENCHA O ENDEREÇO.");
            if (cliente.Endereco.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
                listaErros.Add("ENDEREÇO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    cliente.Endereco.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");
            if (cliente.Endereco == "")
                listaErros.Add("PREENCHA O NÚMERO DO ENDEREÇO.");
            if (cliente.Bairro == "")
                listaErros.Add("PREENCHA O BAIRRO.");
            if (cliente.Cidade == "")
                listaErros.Add("PREENCHA A CIDADE.");
            if (!Utils.Util.VerificaUf(cliente.Uf))
                listaErros.Add("UF INVÁLIDA.");
            if (cliente.Cep == "")
                listaErros.Add("INFORME O CEP.");
            if (!Utils.Util.VerificaCep(cliente.Cep))
                listaErros.Add("CEP INVÁLIDO.");


            if (cliente.Ie == "" &&
                Convert.ToString(cliente.Contribuinte_Icms_Status) == Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                listaErros.Add("PREENCHA A INSCRIÇÃO ESTADUAL.");
            if (Convert.ToString(cliente.ProdutorRural) == Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                if (cliente.Ie == "" &&
                Convert.ToString(cliente.Contribuinte_Icms_Status) == Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    listaErros.Add("Para ser cadastrado como Produtor Rural, " +
                        "é necessário ser contribuinte do ICMS e possuir nº de IE");

            if (!string.IsNullOrEmpty(cliente.Ie))
            {

                string uf = VerificarInscricaoEstadualValida(cliente.Ie, cliente.Uf, listaErros);
                List<NfeMunicipio> lstNfeMunicipio = new List<NfeMunicipio>();
                lstNfeMunicipio = (await ConsisteMunicipioIBGE(cliente.Cidade, cliente.Uf, listaErros)).ToList();

            }
            //vamos verificar novamente o endereço, pois o usuário pode buscar o cep e 
            //depois alterar o nome da rua, UF, cidade, bairro
            await VerificarEndereco(cliente, listaErros);

        }

        private async Task VerificarEndereco(DadosClienteCadastroDto cliente, List<string> listaErros)
        {
            CepBll cep = new CepBll(contextoCepProvider);
            List<CepDto> cepDto = new List<CepDto>();
            string cepSoDigito = cliente.Cep.Replace(".", "").Replace("-", "");
            cepDto = (await cep.BuscarPorCep(cepSoDigito)).ToList();
            foreach (var c in cepDto)
            {
                /*
                 * Só podemos verificar se o cep, pois pode ser que o cep não tem endereço
                 */

                if (c.Cep != cepSoDigito)
                    listaErros.Add("Número do Cep diferente!");


                if (Utils.Util.RemoverAcentuacao(c.Cidade.ToUpper()) != Utils.Util.RemoverAcentuacao(cliente.Cidade.ToUpper()) ||
                    c.Uf.ToUpper() != cliente.Uf.ToUpper())
                    listaErros.Add("Os dados informados estão divergindo da base de dados!");
            }
        }

        private string VerificarInscricaoEstadualValida(string ie, string uf, List<string> listaErros)
        {
            string c = "";
            int qtdeDig = 0;
            int num;
            string retorno = "";
            bool blnResultado = false;

            if (ie != "ISENTO")
            {
                for (int i = 0; i < ie.Length; i++)
                {
                    c = ie.Substring(i, 1);
                    if (!int.TryParse(c, out num) && c != "." && c != "-" && c != "/")
                        blnResultado = false;
                    if (int.TryParse(c, out num))
                        qtdeDig += 1;
                }
                if (qtdeDig < 2 && qtdeDig > 14)
                    retorno = "Preencha a IE (Inscrição Estadual) com um número válido! " +
                            "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE.";
                else
                    retorno = ie;
            }
            else
            {
                retorno = ie;
            }

            blnResultado = isInscricaoEstadualOkCom(ie, uf, listaErros);
            if (!blnResultado)
            {
                listaErros.Add("Preencha a IE (Inscrição Estadual) com um número válido! " +
                            "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE.");
            }

            return retorno;
        }

        private bool isInscricaoEstadualOkCom(string ie, string uf, List<string> listaErros)
        {
            var ReflectionUtilsMemberAccess =
          BindingFlags.Public | BindingFlags.NonPublic |
          BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;

            ie = ie.Replace(".", "").Replace("-", "");
            // this works with both .NET 4.5+ and .NET Core 2.0+

            string progId = "ComPlusWrapper_DllInscE32.ComPlusWrapper_DllInscE32";
            Type type = Type.GetTypeFromProgID(progId);
            object inst = Activator.CreateInstance(type);


            bool result = (bool)inst.GetType().InvokeMember("isInscricaoEstadualOk",
                ReflectionUtilsMemberAccess | BindingFlags.InvokeMethod, null, inst,
                new object[2]
                {
                    ie, uf
                });

            return result;
        }

        private async Task<string> GerarIdCliente(InfraBanco.ContextoBdGravacao dbgravacao, string id_nsu)
        {
            string retorno = "";
            int n_nsu = -1;
            string s = "0";
            int asc;
            char chr;

            if (id_nsu == "")
                retorno = "Não foi especificado o NSU a ser gerado!";

            for (int i = 0; i <= 100; i++)
            {
                var ret = from c in dbgravacao.Tcontroles
                          where c.Id_Nsu == id_nsu
                          select c;

                var controle = await ret.FirstOrDefaultAsync();


                if (!string.IsNullOrEmpty(controle.Nsu))
                {
                    if (controle.Seq_Anual != 0)
                    {
                        if (DateTime.Now.Year > controle.Dt_Ult_Atualizacao.Year)
                        {
                            s = Utils.Util.Normaliza_Codigo(s, Constantes.TAM_MAX_NSU);
                            controle.Dt_Ult_Atualizacao = DateTime.Now;
                            if (!String.IsNullOrEmpty(controle.Ano_Letra_Seq))
                            {
                                asc = int.Parse(controle.Ano_Letra_Seq) + controle.Ano_Letra_Step;
                                chr = (char)asc;
                            }
                        }
                    }
                    n_nsu = int.Parse(controle.Nsu);
                }
                if (n_nsu < 0)
                {
                    retorno = "O NSU gerado é inválido!";
                }
                n_nsu += 1;
                s = Convert.ToString(n_nsu);
                s = Utils.Util.Normaliza_Codigo(s, Constantes.TAM_MAX_NSU);
                if (s.Length == 12)
                {
                    i = 101;
                    //para salvar o novo numero
                    controle.Nsu = s;
                    if (DateTime.Now > controle.Dt_Ult_Atualizacao)
                        controle.Dt_Ult_Atualizacao = DateTime.Now;

                    retorno = controle.Nsu;

                    try
                    {
                        dbgravacao.Update(controle);
                        await dbgravacao.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        retorno = "Não foi possível gerar o NSU, pois ocorreu o seguinte erro: " + ex.HResult + ":" + ex.Message;
                    }
                }
            }

            return retorno;
        }



    }
}
