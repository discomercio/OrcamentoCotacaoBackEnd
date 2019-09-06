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

namespace PrepedidoBusiness.Bll
{
    public class ClienteBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public ClienteBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<List<string>> AtualizarClienteParcial(string apelido, DadosClienteCadastroDto dadosClienteCadastroDto)
        {
            /*
             * somente os seguintes campos serão atualizados:
             * produtor rural
             * inscrição estadual
             * tipo de contibuinte ICMS
             * */
            var db = contextoProvider.GetContexto();
            var retorno = new List<string>();

            var dados = from c in db.Tclientes
                        where c.Id == dadosClienteCadastroDto.Id
                        select c;
            var cli = await dados.FirstOrDefaultAsync();

            if (cli == null)
            {
                retorno.Add("Registro do cliente não encontrado.");
                return retorno;
            }

            //afazer: atualizar os dados do cliente e fazer o log
            //validar os campos necessarios
            if (dadosClienteCadastroDto.Contribuinte_Icms_Status == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM) &&
                dadosClienteCadastroDto.Ie != null || dadosClienteCadastroDto.Ie != "")
            {
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
                    //afazer: implementação  de produtor rural
                    cli.Produtor_Rural_Status = dadosClienteCadastroDto.ProdutorRural;
                    cli.Produtor_Rural_Data = DateTime.Now;
                    cli.Produtor_Rural_Data_Hora = DateTime.Now;
                    cli.Produtor_Rural_Usuario = apelido;
                }
                cli.Dt_Ult_Atualizacao = DateTime.Now;
                cli.Usuario_Ult_Atualizacao = apelido;

                db.SaveChanges();
            }

            //afazer: entender como é feito o Log
            //Essa parte esta na pagina ClienteAtualiza.asp linha 1113
            //bool salvouLog = Utils.Util.GravaLog(apelido, dadosClienteCadastroDto.Loja, "", dadosClienteCadastroDto.Id,
            //    Constantes.OP_LOG_CLIENTE_ALTERACAO,  contextoProvider);


            /*campos que serão salvos no log
             * rs("usuario") = usuario
             * rs("loja") = loja
             * rs("pedido") = pedido
             * rs("id_cliente") = id_cliente
             * rs("operacao") = operacao
             * rs("complemento") = complemento
            */



            //afazer: rfazer a rotina
            //afazer: validar IE conforme estado
            //afazer: deve ter um log com o apelido do orcamentista
            //para teste
            //ret.Add("Algum erro 1.");
            //ret.Add("Algum erro 2.");
            return retorno;
        }

        public async Task<ClienteCadastroDto> BuscarCliente(string cpf_cnpj)
        {
            var db = contextoProvider.GetContexto();

            var dadosCliente = db.Tclientes.Where(r => r.Cnpj_Cpf == cpf_cnpj)
                .FirstOrDefault();
            if (dadosCliente == null)
                return null;

            //afazer: Montar os 4 dto's para retornar para tela de cliente
            var dadosClienteTask = ObterDadosClienteCadastro(dadosCliente);
            var refBancariaTask = ObterReferenciaBancaria(dadosCliente);
            var refComercialTask = ObterReferenciaComercial(dadosCliente);

            ClienteCadastroDto cliente = new ClienteCadastroDto
            {
                DadosCliente = await dadosClienteTask,
                RefBancaria = (await refBancariaTask).ToList(),
                RefComercial = (await refComercialTask).ToList()
            };

            return await Task.FromResult(cliente);
        }

        public async Task<IEnumerable<ListaBancoDto>> ListarBancosCombo()
        {
            var db = contextoProvider.GetContexto();

            var bancos = from c in db.Tbancos
                         orderby c.Codigo
                         select new ListaBancoDto
                         {
                             Codigo = c.Codigo,
                             Descricao = c.Descricao
                         };

            return bancos;
        }

        public async Task<DadosClienteCadastroDto> ObterDadosClienteCadastro(Tcliente cli)
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
            var db = contextoProvider.GetContexto();

            var rBanco = from c in db.TclienteRefBancarias
                         where c.Id_Cliente == cli.Id
                         orderby c.Ordem
                         select c;

            foreach (var i in rBanco)
            {
                RefBancariaDtoCliente refBanco = new RefBancariaDtoCliente
                {
                    Banco = i.Banco,
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
            var db = contextoProvider.GetContexto();

            var rComercial = from c in db.TclienteRefComercials
                             where c.Id_Cliente == cli.Id
                             orderby c.Ordem
                             select c;

            foreach (var i in rComercial)
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
            string retorno = "";
            string id_cliente = "";

            var db = contextoProvider.GetContexto();
            var verifica = await (from c in db.Tclientes
                                  where c.Id == clienteDto.DadosCliente.Id
                                  select c.Id).FirstOrDefaultAsync();

            List<string> lstErros = ValidarDadosClientesCadastro(clienteDto.DadosCliente);

            if (verifica != null)
                lstErros.Add("REGISTRO COM ID=" + clienteDto.DadosCliente.Id + " JÁ EXISTE.");

            if (lstErros.Count <= 0)
            {
                DadosClienteCadastroDto cliente = clienteDto.DadosCliente;
                //salvar os dados CadastrarDadosClienteDto(DadosClienteCadastroDto clienteDto)
                id_cliente = await CadastrarDadosClienteDto(cliente, apelido);

                if (id_cliente.Length == 12)
                {
                    //afazer
                    //validar os dados de refBancaria
                    //Caso a validação da refBancaria esteja true

                    //RefBancariaDtoCliente bancaria = clienteDto.RefBancaria;
                    //RefComercialDtoCliente comercial = clienteDto.RefComercial;
                }
                else
                {
                    lstErros.Add(id_cliente);
                }
            }

            return lstErros;

        }
        /*afazer
         * verificar com o João
         * o que esta ocorrendo na pág ClienteAtualiza.asp linha 708 até linha 729
         */
        private Task<string> CadastrarRefBancaria(List<RefBancariaDtoCliente> lstRefBancaria, string apelido, string id_cliente)
        {
            string retorno = "";

            var db = contextoProvider.GetContexto();

            foreach(RefBancariaDtoCliente r in lstRefBancaria)
            {
                //TclienteRefBancaria cliRefBancaria = new TclienteRefBancaria
                //{
                //    Id_Cliente = id_cliente,
                //    Banco = r.Banco,
                //    Agencia = r.Agencia,
                //    Conta = r.Conta,
                //    Dt_Cadastro = DateTime.Now,
                //    Usuario_Cadastro = apelido,
                //    Ordem = 
                //};
            }

            return Task.FromResult(retorno);
        }

        private async Task<IEnumerable<string>> ConsisteMunicipioIBGE(string municipio, string uf)
        {
            List<string> erros = new List<string>();
            List<string> lstSugerida = new List<string>();
            var db = contextoProvider.GetContexto();
            string chave = "";
            string senhaCripto = "";

            if (string.IsNullOrEmpty(municipio))
                erros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                    "nenhum município foi informado!!");
            if (string.IsNullOrEmpty(uf))
                erros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                    "a UF não foi informada!!");
            if (uf.Length > 2)
                erros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                    "a UF é inválida (" + uf + ")!!");
            if (erros.Count == 0)
            {
                var nfEmitente = from c in db.TnfEmitentes
                                 where c.NFe_st_emitente_padrao == 1
                                 select new
                                 {
                                     c.NFe_T1_servidor_BD,
                                     c.NFe_T1_nome_BD,
                                     c.NFe_T1_usuario_BD,
                                     c.NFe_T1_senha_BD
                                 };
                if (nfEmitente == null)
                    erros.Add("Não há um emitente de NFe padrão definido no sistema!!");

                string senhaNfEmitente = nfEmitente.Select(r => r.NFe_T1_senha_BD).FirstOrDefault();

                chave = Utils.Util.GeraChave(Constantes.FATOR_BD);
                senhaCripto = Utils.Util.DecodificaSenha(senhaNfEmitente, chave);

                //afazer: Verificar com o João e Hamilton o metodo abaixo
                //BuscarSiglaUf(string servidor, string nomedb, string usuariodb, string senhadb)

            }

            return lstSugerida;
        }

        private string BuscarSiglaUf(string servidor, string nomedb, string usuariodb, string senhadb)
        {
            /*afazer:
             * Esse metodo necessita de acesso em outra base
             * Necessário verificar com o João e Hamilton 
             * Esse metodo esta na pág. BDD.asp linha 4840
             * Isso faz parte do metodo da validação para salvar a RefBancaria pág ClienteAtualiza.asp linha 329 
             * que faz a chamada para o metodo consiste_municipio_IBGE_ok(s_cidade, s_uf, s_lista_sugerida_municipios, msg_erro)
             */
            string retorno = "";

            string conexao = "Provider=SQLOLEDB;" +
                "Data Source=" + servidor + ";" +
                "Initial Catalog=" + nomedb + ";" +
                "User ID=" + usuariodb + ";" +
                "Password=" + senhadb + ";";



            return retorno;
        }

        private List<string> ValidarDadosClientesCadastro(DadosClienteCadastroDto cliente)
        {
            List<string> listaErros = new List<string>();

            if (cliente.Cnpj_Cpf == "")
                listaErros.Add("CNPJ / CPF NÃO FORNECIDO.");
            if (Utils.Util.ValidaCpf_Cnpj(cliente.Cnpj_Cpf))
                listaErros.Add("CNPJ/CPF INVÁLIDO.");
            if (cliente.Sexo != "M" || cliente.Sexo != "F")
                listaErros.Add("INDIQUE QUAL O SEXO.");
            if (cliente.Nome == "")
            {
                if (cliente.Tipo == Constantes.ID_PF)
                    listaErros.Add("PREENCHA O NOME DO CLIENTE.");
                if (cliente.Tipo == Constantes.ID_PJ)
                    listaErros.Add("PREENCHA A RAZÃO SOCIAL DO CLIENTE.");
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
            if (cliente.DddResidencial.Length != 2)
                listaErros.Add("DDD INVÁLIDO.");
            if (cliente.TelefoneResidencial.Length < 6)
                listaErros.Add("TELEFONE RESIDENCIAL INVÁLIDO.");
            if (cliente.DddResidencial != "" && cliente.TelefoneResidencial == "")
                listaErros.Add("PREENCHA O TELEFONE RESIDENCIAL.");
            if (cliente.DddResidencial == "" && cliente.TelefoneResidencial != "")
                listaErros.Add("PREENCHA O DDD.");
            if (cliente.DddComercial.Length != 2)
                listaErros.Add("DDD INVÁLIDO.");
            if (cliente.TelComercial.Length < 6)
                listaErros.Add("TELEFONE COMERCIAL INVÁLIDO.");
            if (cliente.DddComercial != "" && cliente.TelComercial == "")
                listaErros.Add("PREENCHA O TELEFONE COMERCIAL.");
            if (cliente.DddComercial == "" && cliente.TelComercial != "")
                listaErros.Add("PREENCHA O DDD.");
            if (cliente.Tipo == Constantes.ID_PF &&
                cliente.TelefoneResidencial == "" &&
                cliente.TelComercial == "" &&
                cliente.Celular == "")
                listaErros.Add("PREENCHA PELO MENOS UM TELEFONE.");
            if (cliente.Tipo == Constantes.ID_PJ &&
                cliente.TelComercial == "" &&
                cliente.TelComercial2 == "")
                listaErros.Add("PREENCHA O TELEFONE.");
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
                //if()
            }


            return listaErros;
        }

        //Verificar:com João
        private string VerificarInscricaoEstadualValida(string ie, string uf)
        {
            bool valida = false;
            string c = "";
            bool blnOk = true;
            int qtdeDig = 0;
            int num;
            string retorno = "";
            string ieNormalizado = "";

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
                if (qtdeDig < 2 || qtdeDig > 14)
                    retorno = "Preencha a IE (Inscrição Estadual) com um número válido!!" +
                            "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE.";
            }
            else
            {
                ieNormalizado = ie;
            }

            //verificar: olhar na pág Funcoes.asp linha 4375
            //PERGUNTAR PARA HAMILTON SE ADICIONA A DLL DllInscE32 OU SE QUER INCORPORAR O FONTE ou outra coisa

            return retorno;
        }

        private async Task<string> CadastrarDadosClienteDto(DadosClienteCadastroDto clienteDto, string apelido)
        {
            string retorno = "";
            string id_cliente = GerarIdCliente(Constantes.NSU_CADASTRO_CLIENTES);

            if (id_cliente.Length > 12)
                retorno = id_cliente;
            else
            {
                Tcliente tCliente = new Tcliente
                {
                    Id = id_cliente,
                    Dt_Cadastro = DateTime.Now,
                    Usuario_Cadastrado = apelido,
                    Indicador = apelido,
                    Cnpj_Cpf = clienteDto.Cnpj_Cpf,
                    Tipo = clienteDto.Tipo,
                    Ie = clienteDto.Ie,
                    Rg = clienteDto.Rg,
                    Nome = clienteDto.Nome,
                    Sexo = clienteDto.Sexo,
                    Contribuinte_Icms_Status = clienteDto.Contribuinte_Icms_Status,
                    Contribuinte_Icms_Data = DateTime.Now,
                    Contribuinte_Icms_Data_Hora = DateTime.Now,
                    Contribuinte_Icms_Usuario = apelido,
                    Produtor_Rural_Status = clienteDto.ProdutorRural,
                    Produtor_Rural_Data = DateTime.Now,
                    Produtor_Rural_Data_Hora = DateTime.Now,
                    Produtor_Rural_Usuario = apelido,
                    Endereco = clienteDto.Endereco,
                    Endereco_Numero = clienteDto.Numero,
                    Endereco_Complemento = clienteDto.Complemento,
                    Bairro = clienteDto.Bairro,
                    Cidade = clienteDto.Cidade,
                    Cep = clienteDto.Cep,
                    Uf = clienteDto.Uf,
                    Ddd_Res = clienteDto.DddResidencial,
                    Tel_Res = clienteDto.TelefoneResidencial,
                    Ddd_Com = clienteDto.DddComercial,
                    Tel_Com = clienteDto.TelComercial,
                    Ramal_Com = clienteDto.Ramal,
                    Contato = clienteDto.Contato,
                    Ddd_Com_2 = clienteDto.DddComercial2,
                    Tel_Com_2 = clienteDto.TelComercial2,
                    Ramal_Com_2 = clienteDto.Ramal2,
                    Dt_Nasc = clienteDto.Nascimento,
                    Filiacao = clienteDto.Observacao_Filiacao,
                    Obs_crediticias = "",
                    Midia = "",
                    Email = clienteDto.Email,
                    Email_Xml = clienteDto.EmailXml,
                    Dt_Ult_Atualizacao = DateTime.Now,
                    Usuario_Ult_Atualizacao = apelido
                };

                var db = contextoProvider.GetContexto();

                db.Add(tCliente);
                db.SaveChangesAsync();
                retorno = tCliente.Id;
            }

            return retorno;
        }

        private string GerarIdCliente(string id_nsu)
        {
            string retorno = "";
            int n_nsu = -1;
            string s = "0";
            int asc;
            char chr;
            int novoNsu = 0;

            var db = contextoProvider.GetContexto();

            if (id_nsu == "")
                retorno = "Não foi especificado o NSU a ser gerado!!";

            for (int i = 0; i <= 100; i++)
            {
                var ret = (from c in db.Tcontroles
                           where c.Id_Nsu == id_nsu
                           select c).FirstOrDefault();

                if (!string.IsNullOrEmpty(ret.Nsu))
                {
                    if (ret.Seq_Anual != 0)
                    {
                        if (DateTime.Now.Year > ret.Dt_Ult_Atualizacao.Year)
                        {
                            //afazer:terminar de montar
                            s = Normaliza_Codigo(s, Constantes.TAM_MAX_NSU);
                            ret.Dt_Ult_Atualizacao = DateTime.Now;
                            if (!String.IsNullOrEmpty(ret.Ano_Letra_Seq))
                            {
                                //Precisa revisar essa parte, pois lendo a doc do BD e analisando os dados na base não bate
                                asc = int.Parse(ret.Ano_Letra_Seq) + ret.Ano_Letra_Step;
                                chr = (char)asc;
                            }
                        }
                        n_nsu = int.Parse(ret.Nsu);

                    }
                }
                if (n_nsu < 0)
                {
                    retorno = "O NSU gerado é inválido!!";
                }
                n_nsu += 1;
                s = Convert.ToString(n_nsu);
                s = Normaliza_Codigo(s, Constantes.TAM_MAX_NSU);
                novoNsu = int.Parse(s);
                //para salvar o novo numero
                ret.Nsu = s;
                if (DateTime.Now > ret.Dt_Ult_Atualizacao)
                    ret.Dt_Ult_Atualizacao = DateTime.Now;

                retorno = ret.Nsu;

                try
                {
                    db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    retorno = "Não foi possível gerar o NSU, pois ocorreu o seguinte erro: " + ex.HResult + ":" + ex.Message;
                }
            }

            return retorno;
        }

        private static string Normaliza_Codigo(string cod, int tamanho_default)
        {
            string retorno = "";

            if (cod != "")
            {
                for (int i = 0; i < tamanho_default; i++)
                {
                    retorno += cod;
                }
            }

            return retorno;
        }

        //afazer: Método para trazer o combo de Justifica Endereco

    }
}
