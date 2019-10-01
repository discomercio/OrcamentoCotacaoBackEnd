using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PrepedidoBusiness;
using System.Linq;
using PrepedidoBusiness.Dtos.ClienteCadastro;
using PrepedidoBusiness.Dto.ClienteCadastro.Referencias;
using Microsoft.EntityFrameworkCore;
using PrepedidoBusiness.Dto.ClienteCadastro;
using InfraBanco.Constantes;
using System.Transactions;
using System.Reflection;

namespace PrepedidoBusiness.Bll
{
    public class ClienteBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;
        private readonly InfraBanco.ContextoNFeProvider contextoNFeProvider;

        public ClienteBll(InfraBanco.ContextoProvider contextoProvider, InfraBanco.ContextoCepProvider contextoCepProvider, InfraBanco.ContextoNFeProvider contextoNFeProvider)
        {
            this.contextoProvider = contextoProvider;
            this.contextoCepProvider = contextoCepProvider;
            this.contextoNFeProvider = contextoNFeProvider;
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
            string campos_a_omitir = "|dt_cadastro|usuario_cadastro|dt_ult_atualizacao|usuario_ult_atualizacao|";
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
                    using (TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        db = contextoProvider.GetContextoGravacao();

                        if (dadosClienteCadastroDto.Contribuinte_Icms_Status == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM) &&
                        dadosClienteCadastroDto.Ie != null || dadosClienteCadastroDto.Ie != "")
                        {
                            cli.Ie = dadosClienteCadastroDto.Ie;
                            if (dadosClienteCadastroDto.Contribuinte_Icms_Status != cli.Contribuinte_Icms_Status)
                            {
                                //fazer a implementação do contribuinte
                                cli.Contribuinte_Icms_Status = dadosClienteCadastroDto.Contribuinte_Icms_Status;
                                cli.Contribuinte_Icms_Data = DateTime.Now;
                                cli.Contribuinte_Icms_Data_Hora = DateTime.Now;
                                cli.Contribuinte_Icms_Usuario = apelido;
                            }
                            if (dadosClienteCadastroDto.Tipo == Constantes.ID_PF &&
                                dadosClienteCadastroDto.ProdutorRural != byte.Parse(Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL) &&
                                dadosClienteCadastroDto.ProdutorRural != cli.Produtor_Rural_Status)
                            {
                                cli.Produtor_Rural_Status = dadosClienteCadastroDto.ProdutorRural;
                                cli.Produtor_Rural_Data = DateTime.Now;
                                cli.Produtor_Rural_Data_Hora = DateTime.Now;
                                cli.Produtor_Rural_Usuario = apelido;
                            }
                            cli.Dt_Ult_Atualizacao = DateTime.Now;
                            cli.Usuario_Ult_Atualizacao = apelido;

                            db.Update(cli);
                            db.SaveChanges();

                            log = Utils.Util.MontaLog(cli, log, campos_a_omitir);
                            //Essa parte esta na pagina ClienteAtualiza.asp linha 1113
                            bool salvouLog = Utils.Util.GravaLog(apelido, dadosClienteCadastroDto.Loja, "", dadosClienteCadastroDto.Id,
                                Constantes.OP_LOG_CLIENTE_ALTERACAO, log, contextoProvider);
                            if (salvouLog)
                                trans.Complete();
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

        public async Task<ClienteCadastroDto> BuscarCliente(string cpf_cnpj)
        {
            var db = contextoProvider.GetContextoLeitura();

            var dadosCliente = db.Tclientes.Where(r => r.Cnpj_Cpf == cpf_cnpj)
                .FirstOrDefault();
            if (dadosCliente == null)
                return null;

            var dadosClienteTask = ObterDadosClienteCadastro(dadosCliente);
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
            //paraTeste
            //string apelido = "MARISARJ";
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

        public DadosClienteCadastroDto ObterDadosClienteCadastro(Tcliente cli)
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
                Contato = cli.Contato
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

            //teste
            //apelido = "MARISARJ";

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
                using (TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    string log = "";

                    db = contextoProvider.GetContextoGravacao();
                    DadosClienteCadastroDto cliente = clienteDto.DadosCliente;
                    id_cliente = await CadastrarDadosClienteDto(cliente, apelido, log);

                    //Por padrão o id do cliente tem 12 caracteres, caso não seja 12 caracteres esta errado
                    if (id_cliente.Length == 12)
                    {
                        await CadastrarRefBancaria(clienteDto.RefBancaria, apelido, id_cliente, log);
                        await CadastrarRefComercial(clienteDto.RefComercial, apelido, id_cliente, log);
                        //fazer a inserção de Log aqui.
                        bool gravouLog = Utils.Util.GravaLog(apelido, cliente.Loja, "", id_cliente,
                            Constantes.OP_LOG_CLIENTE_INCLUSAO, log, contextoProvider);
                        if (gravouLog)
                            trans.Complete();
                    }
                    else
                    {
                        lstErros.Add(id_cliente);
                    }
                }
            }
            return lstErros;
        }

        private async Task<string> CadastrarDadosClienteDto(DadosClienteCadastroDto clienteDto, string apelido, string log)
        {
            string retorno = "";
            List<string> lstRetorno = new List<string>();
            string id_cliente = await GerarIdCliente(Constantes.NSU_CADASTRO_CLIENTES);

            string campos_a_omitir = "dt_cadastro|usuario_cadastro|dt_ult_atualizacao|usuario_ult_atualizacao";

            lstRetorno.Add(id_cliente);

            if (id_cliente.Length > 12)
                retorno = id_cliente;
            else
            {
                Tcliente tCliente = new Tcliente
                {
                    Id = id_cliente,
                    Dt_Cadastro = DateTime.Now,
                    Usuario_Cadastrado = apelido.ToUpper(),
                    Indicador = apelido.ToUpper(),
                    Cnpj_Cpf = clienteDto.Cnpj_Cpf.Replace(".", "").Replace("/", "").Replace("-", ""),
                    Tipo = clienteDto.Tipo.ToUpper(),
                    Ie = clienteDto.Ie,
                    Rg = clienteDto.Rg,
                    Nome = clienteDto.Nome.ToUpper(),
                    Sexo = clienteDto.Sexo.ToUpper(),
                    Contribuinte_Icms_Status = clienteDto.Contribuinte_Icms_Status,
                    Contribuinte_Icms_Data = DateTime.Now,
                    Contribuinte_Icms_Data_Hora = DateTime.Now,
                    Contribuinte_Icms_Usuario = apelido.ToUpper(),
                    Produtor_Rural_Status = clienteDto.ProdutorRural,
                    Produtor_Rural_Data = DateTime.Now,
                    Produtor_Rural_Data_Hora = DateTime.Now,
                    Produtor_Rural_Usuario = apelido.ToUpper(),
                    Endereco = clienteDto.Endereco.ToUpper(),
                    Endereco_Numero = clienteDto.Numero,
                    Endereco_Complemento = clienteDto.Complemento,
                    Bairro = clienteDto.Bairro.ToUpper(),
                    Cidade = clienteDto.Cidade.ToUpper(),
                    Cep = clienteDto.Cep.Replace("-", ""),
                    Uf = clienteDto.Uf.ToUpper(),
                    Ddd_Res = clienteDto.DddResidencial,
                    Tel_Res = clienteDto.TelefoneResidencial,
                    Ddd_Com = clienteDto.DddComercial,
                    Tel_Com = clienteDto.TelComercial,
                    Ramal_Com = clienteDto.Ramal,
                    Contato = clienteDto.Contato.ToUpper(),
                    Ddd_Com_2 = clienteDto.DddComercial2,
                    Tel_Com_2 = clienteDto.TelComercial2,
                    Ramal_Com_2 = clienteDto.Ramal2,
                    Dt_Nasc = clienteDto.Nascimento,
                    Filiacao = clienteDto.Observacao_Filiacao.ToUpper(),
                    Obs_crediticias = "",
                    Midia = "",
                    Email = clienteDto.Email,
                    Email_Xml = clienteDto.EmailXml,
                    Dt_Ult_Atualizacao = DateTime.Now,
                    Usuario_Ult_Atualizacao = apelido.ToUpper()
                };

                var db = contextoProvider.GetContextoGravacao();

                //Busca os nomes reais das colunas da tabela SQL
                Utils.Util.MontaLog(tCliente, log, campos_a_omitir);

                db.Add(tCliente);
                await db.SaveChangesAsync();
                retorno = tCliente.Id;
            }

            return retorno;
        }

        private async Task<string> CadastrarRefBancaria(List<RefBancariaDtoCliente> lstRefBancaria, string apelido, string id_cliente, string log)
        {
            var db = contextoProvider.GetContextoGravacao();
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
                db.Add(cliRefBancaria);
                qtdeRef++;

                //Busca os nomes reais das colunas da tabela SQL
                log = Utils.Util.MontaLog(cliRefBancaria, log, campos_a_omitir_ref_bancaria);
            }

            await db.SaveChangesAsync();
            return log;
        }

        private async Task<string> CadastrarRefComercial(List<RefComercialDtoCliente> lstRefComercial, string apelido, string id_cliente, string log)
        {
            var db = contextoProvider.GetContextoGravacao();
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
                db.Add(c);
                qtdeRef++;
                log = Utils.Util.MontaLog(c, log, campos_a_omitir_ref_comercial);
            }

            await db.SaveChangesAsync();
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
        //afazer:as tabelas já estão disponiveis
        private async Task<IEnumerable<NfeMunicipio>> ConsisteMunicipioIBGE(string municipio, string uf, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();
            List<NfeMunicipio> lst_nfeMunicipios = new List<NfeMunicipio>();

            if (string.IsNullOrEmpty(municipio))
                lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                    "nenhum município foi informado!!");
            if (string.IsNullOrEmpty(uf))
                lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                    "a UF não foi informada!!");
            else
            {
                if (uf.Length > 2)
                    lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                        "a UF é inválida (" + uf + ")!!");
            }

            if (lstErros.Count == 0)
            {
                lst_nfeMunicipios = (await BuscarSiglaUf(uf, municipio)).ToList();

                if (!lst_nfeMunicipios.Any())
                {
                    lstErros.Add("Município '" + municipio + "' não consta na relação de municípios do IBGE para a UF de '" + uf + "'!!");
                }
            }

            return lst_nfeMunicipios;
        }

        /*afazer:
             * Esse metodo necessita de acesso em outra base
             * Necessário verificar com o João e Hamilton 
             * Esse metodo esta na pág. BDD.asp linha 4840
             * Isso faz parte do metodo da validação para salvar a RefBancaria pág ClienteAtualiza.asp linha 329 
             * que faz a chamada para o metodo consiste_municipio_IBGE_ok(s_cidade, s_uf, s_lista_sugerida_municipios, msg_erro)
             */
        private async Task<IEnumerable<NfeMunicipio>> BuscarSiglaUf(string uf, string municipio)
        {
            //verificar se passo a lista de erros
            string retorno = "";
            List<NfeMunicipio> lstNfeMunicipio = new List<NfeMunicipio>();

            var db = contextoNFeProvider.GetContextoLeitura();

            var nfeUFTask = (from c in db.NfeUfs
                             where c.SiglaUF == uf.ToUpper()
                             select c).FirstOrDefaultAsync();

            NfeUf nfeUf = await nfeUFTask;

            if (string.IsNullOrEmpty(nfeUf.CodUF))
                retorno = "Não é possível consistir o município através da relação de municípios do IBGE: " +
                    "a UF '" + uf + "' não foi localizada na relação do IBGE!!";
            else
            {
                string codUF = nfeUf.CodUF;

                var nfeMunicipioTask = (from c in db.NfeMunicipios
                                        where c.CodMunic.Contains(codUF) && c.Descricao == municipio
                                        select c).FirstOrDefaultAsync();

                if (nfeMunicipioTask != null)
                {
                    lstNfeMunicipio.Add(await nfeMunicipioTask);
                }
                else
                {
                    var lst_nfeMunicipioTask = from c in db.NfeMunicipios
                                               where c.CodMunic.Contains(codUF) &&
                                                     c.Descricao.Contains(municipio.Substring(municipio.Length - 1, 1))
                                               orderby c.Descricao
                                               select c;

                    if (await lst_nfeMunicipioTask.AnyAsync())
                    {
                        foreach (var p in lst_nfeMunicipioTask)
                        {
                            lstNfeMunicipio.Add(p);
                        }
                    }

                }
            }
            return lstNfeMunicipio;
        }

        private async Task ValidarDadosClientesCadastro(DadosClienteCadastroDto cliente, List<string> listaErros)
        {
            string cpf_cnpjSoDig = Utils.Util.SoDigitosCpf_Cnpj(cliente.Cnpj_Cpf);
            bool ehCpf = Utils.Util.ValidaCpf_Cnpj(cliente.Cnpj_Cpf);

            if (cliente.Cnpj_Cpf == "")
                listaErros.Add("CNPJ / CPF NÃO FORNECIDO.");
            //if (!Utils.Util.ValidaCpf_Cnpj(cliente.Cnpj_Cpf))
            //    listaErros.Add("CNPJ/CPF INVÁLIDO.");

            if (ehCpf)
            {
                if (cliente.Sexo != "M" && cliente.Sexo != "F")
                    listaErros.Add("INDIQUE QUAL O SEXO.");
                if (cliente.Nome == "")
                {
                    if (cliente.Tipo == Constantes.ID_PF)
                        listaErros.Add("PREENCHA O NOME DO CLIENTE.");
                }
                //a verificação de Nascimento esta sendo feita no cliente
                //if (cliente.Nascimento == null)
                //    listaErros.Add("DATA DE NASCIMENTO É INVÁLIDA.");

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
            else
            {
                if (cliente.Tipo == Constantes.ID_PJ && cliente.Nome == "")
                    listaErros.Add("PREENCHA A RAZÃO SOCIAL DO CLIENTE.");
                if (cliente.Tipo == Constantes.ID_PJ &&
                cliente.TelComercial == "" &&
                cliente.TelComercial2 == "")
                    listaErros.Add("PREENCHA O TELEFONE.");

                if (cliente.DddComercial.Length != 2 && cliente.DddComercial != "")
                    listaErros.Add("DDD INVÁLIDO.");
                else if (cliente.TelComercial.Length < 6 && cliente.TelComercial != "")
                    listaErros.Add("TELEFONE COMERCIAL INVÁLIDO.");
                else if (cliente.DddComercial != "" && cliente.TelComercial == "")
                    listaErros.Add("PREENCHA O TELEFONE COMERCIAL.");
                else if (cliente.DddComercial == "" && cliente.TelComercial != "")
                    listaErros.Add("PREENCHA O DDD.");
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
                if (cliente.Ie == "" ||
                Convert.ToString(cliente.Contribuinte_Icms_Status) == Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    listaErros.Add("Para ser cadastrado como Produtor Rural, " +
                        "é necessário ser contribuinte do ICMS e possuir nº de IE");

            string s_tabela_municipios_IBGE = "";
            if (cliente.Ie != "")
            {
                //afazer: terminar o metodo abaixo
                //string uf = VerificarInscricaoEstadualValida(cliente.Ie, cliente.Uf);
                //List<NfeMunicipio> lstNfeMunicipio = new List<NfeMunicipio>();
                //lstNfeMunicipio = (await ConsisteMunicipioIBGE(cliente.Cidade, cliente.Uf, listaErros)).ToList();

            }


            //return listaErros;
        }

        //afazer:ADICIONA A DLL DllInscE32
        private string VerificarInscricaoEstadualValida(string ie, string uf)
        {
            string c = "";
            bool blnOk = true;
            int qtdeDig = 0;
            int num;
            string retorno = "";

            if (ie != "ISENTO")
            {
                for (int i = 0; i < ie.Length; i++)
                {
                    c = ie.Substring(i, 1);
                    if (!int.TryParse(c, out num) && c != "." && c != "-" && c != "/")
                        blnOk = false;
                    if (int.TryParse(c, out num))
                        qtdeDig += 1;
                }
                if (qtdeDig < 2 && qtdeDig > 14)
                    retorno = "Preencha a IE (Inscrição Estadual) com um número válido!!" +
                            "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE.";
                else
                    retorno = ie;
            }
            else
            {
                retorno = ie;
            }

            //afazer: olhar na pág Funcoes.asp linha 4375
            //set objIE = CreateObject("ComPlusWrapper_DllInscE32.ComPlusWrapper_DllInscE32")
            //blnResultado = objIE.isInscricaoEstadualOk(strInscricaoEstadualNormalizado, uf)
            //Instalar A DLL DllInscE32 QUER INCORPORAR O FONTE ou outra coisa

            return retorno;
        }

        private async Task<string> GerarIdCliente(string id_nsu)
        {
            string retorno = "";
            int n_nsu = -1;
            string s = "0";
            int asc;
            char chr;

            var db = contextoProvider.GetContextoGravacao();

            if (id_nsu == "")
                retorno = "Não foi especificado o NSU a ser gerado!!";

            for (int i = 0; i <= 100; i++)
            {
                var ret = from c in db.Tcontroles
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
                                //Precisa revisar essa parte, pois lendo a doc do BD e analisando os dados na base não bate
                                asc = int.Parse(controle.Ano_Letra_Seq) + controle.Ano_Letra_Step;
                                chr = (char)asc;
                            }
                        }
                    }
                    n_nsu = int.Parse(controle.Nsu);
                }
                if (n_nsu < 0)
                {
                    retorno = "O NSU gerado é inválido!!";
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
                        db.Update(controle);
                        await db.SaveChangesAsync();
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
