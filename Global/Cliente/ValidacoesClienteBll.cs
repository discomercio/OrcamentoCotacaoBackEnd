using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using System.Reflection;
using UtilsGlobais;
using Cep;
using Microsoft.EntityFrameworkCore;

namespace Cliente
{
    public class ValidacoesClienteBll
    {
        public static class MensagensErro
        {
            public static string Cep_nao_existe = "Cep não existe!";
            public static string Estado_nao_confere = "Estado não confere!";
            public static string Preencha_a_IE_Inscricao_Estadual = "Preencha a IE (Inscrição Estadual) com um número válido! " +
                            "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE.";
            public static string CPF_INVALIDO = "CPF INVÁLIDO.";
            public static string CPF_NAO_FORNECIDO = "CPF NÃO FORNECIDO.";
            public static string GENERO_DO_CLIENTE_NAO_INFORMADO = "GÊNERO DO CLIENTE NÃO INFORMADO!.";
            public static string INFORME_SE_O_CLIENTE_E_PF_OU_PJ = "INFORME SE O CLIENTE É PF OU PJ!";
            public static string Tipo_de_cliente_nao_e_PF_nem_PJ = "Tipo de cliente não é PF nem PJ.";
            public static string CNPJ_INVALIDO = "CNPJ INVÁLIDO.";
        }

        public static async Task ValidarDadosCliente(Cliente.Dados.DadosClienteCadastroDados dadosCliente,
            List<Cliente.Dados.Referencias.RefBancariaClienteDados> lstRefBancaria,
            List<Cliente.Dados.Referencias.RefComercialClienteDados> lstRefComercial,
            List<string> lstErros, ContextoBdProvider contextoProvider, CepBll cepBll, IBancoNFeMunicipio bancoNFeMunicipio,
            List<Cliente.Dados.ListaBancoDados> lstBanco, bool flagMsg_IE_Cadastro_PF,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
        {
            if (dadosCliente != null)
            {
                //existe dados
                if (!string.IsNullOrEmpty(dadosCliente.Tipo))
                {
                    var db = contextoProvider.GetContextoLeitura();

                    Tcliente cliente = await (from c in db.Tclientes
                                              where c.Cnpj_Cpf == dadosCliente.Cnpj_Cpf &&
                                                    c.Tipo == dadosCliente.Tipo
                                              select c).FirstOrDefaultAsync();


                    bool tipoDesconhecido = true;
                    //é cliente PF
                    if (dadosCliente.Tipo == Constantes.ID_PF)
                    {
                        if (lstRefBancaria != null)
                        {
                            if (lstRefBancaria.Count != 0)
                            {
                                lstErros.Add("Se cliente tipo PF, não deve constar referência bancária!");
                            }
                        }

                        if (lstRefComercial != null)
                        {
                            if (lstRefComercial.Count != 0)
                            {
                                lstErros.Add("Se cliente tipo PF, não deve constar referência comercial!");
                            }
                        }

                        tipoDesconhecido = false;
                        //vamos verificar e validar os dados referente ao cliente PF
                        await ValidarDadosCliente_PF(dadosCliente, cliente, lstErros, contextoProvider, sistemaResponsavel);
                    }
                    if (dadosCliente.Tipo == Constantes.ID_PJ)
                    {
                        tipoDesconhecido = false;
                        await ValidarDadosCliente_PJ(dadosCliente, cliente, lstErros, contextoProvider);
                        //vamos validar as referências
                        ValidarReferencias_Bancarias_Comerciais(lstRefBancaria, lstRefComercial,
                            lstErros, dadosCliente.Tipo, lstBanco);
                    }

                    if (tipoDesconhecido)
                        lstErros.Add(MensagensErro.Tipo_de_cliente_nao_e_PF_nem_PJ);

                    //validar endereço do cadastro                    
                    await ValidarEnderecoCadastroCliente(dadosCliente, lstErros, cepBll, contextoProvider,
                        bancoNFeMunicipio);
                    VerificarCaracteresInvalidosEnderecoCadastral(dadosCliente, lstErros);


                    //vamos verificar o IE dos clientes
                    if (dadosCliente.Tipo == Constantes.ID_PJ ||
                        dadosCliente.Tipo == Constantes.ID_PF)
                        ValidarIE_Cliente(dadosCliente, lstErros, contextoProvider,
                            bancoNFeMunicipio, flagMsg_IE_Cadastro_PF);

                    await CepBll.ConsisteMunicipioIBGE(dadosCliente.Cidade, dadosCliente.Uf, lstErros, contextoProvider,
                        bancoNFeMunicipio, true);
                }
                else
                {
                    lstErros.Add(MensagensErro.INFORME_SE_O_CLIENTE_E_PF_OU_PJ);
                }
            }
            else
            {
                lstErros.Add("DADOS DO CLIENTE ESTA VAZIO!");
            }
        }

        private static void VerificarCaracteresInvalidosEnderecoCadastral(
            Cliente.Dados.DadosClienteCadastroDados dados, List<string> lstErros)
        {
            //proteção contra null
            dados.Endereco ??= "";

            string caracteres;
            if (UtilsGlobais.Util.IsTextoValido(dados.Endereco, out caracteres).Length > 0)
                lstErros.Add("O CAMPO Endereco DO ENDEREÇO POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            if (UtilsGlobais.Util.IsTextoValido(dados.Numero ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO Numero DO ENDEREÇO POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            if (UtilsGlobais.Util.IsTextoValido(dados.Complemento ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO Complemento DO ENDEREÇO POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            if (UtilsGlobais.Util.IsTextoValido(dados.Bairro ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO Bairro DO ENDEREÇO POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            if (UtilsGlobais.Util.IsTextoValido(dados.Cidade ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO Cidade DO ENDEREÇO POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            if (UtilsGlobais.Util.IsTextoValido(dados.Nome ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO Nome DO ENDEREÇO POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);

            //vamos verificar se o endereço de entrega esta com os valores corretos
            if (dados.Endereco.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
                lstErros.Add("ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br> TAMANHO ATUAL: " +
                    dados.Endereco.Length + " CARACTERES <br> TAMANHO MÁXIMO: " +
                    Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");

            if (UtilsGlobais.Util.IsTextoValido(dados.Rg ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO Rg POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);
            if (UtilsGlobais.Util.IsTextoValido(dados.Observacao_Filiacao ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO Observacao_Filiacao POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);
            if (UtilsGlobais.Util.IsTextoValido(dados.Email ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO Email POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);
            if (UtilsGlobais.Util.IsTextoValido(dados.EmailXml ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO EmailXml POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);
            if (UtilsGlobais.Util.IsTextoValido(dados.Contato ?? "", out caracteres).Length > 0)
                lstErros.Add("O CAMPO Contato POSSUI UM OU MAIS CARACTERES INVÁLIDOS: " + caracteres);
        }

        private static async Task<bool> ValidarDadosCliente_PF(Cliente.Dados.DadosClienteCadastroDados dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
        {
            bool retorno = true;

            if (string.IsNullOrEmpty(dadosCliente.Nome))
            {
                lstErros.Add("PREENCHA O NOME DO CLIENTE.");
                retorno = false;
            }

            if (string.IsNullOrEmpty(dadosCliente.Cnpj_Cpf))
            {
                lstErros.Add(MensagensErro.CPF_NAO_FORNECIDO);
                retorno = false;
            }
            else
            {
                string cpf_cnpjSoDig = Util.SoDigitosCpf_Cnpj(dadosCliente.Cnpj_Cpf);

                if (cliente != null)
                {
                    //vamos confrontar o cpf 
                    string cpfCliente = Util.SoDigitosCpf_Cnpj(cliente.Cnpj_Cpf);

                    //vamos validar o cpf

                    if (cpfCliente != cpf_cnpjSoDig)
                    {
                        lstErros.Add("O CPF do cliente esta divergindo do cadastro!");
                        return false;
                    }
                }

                if (!Util.ValidaCPF(cpf_cnpjSoDig))
                {
                    lstErros.Add(MensagensErro.CPF_INVALIDO);
                    retorno = false;
                }
                else
                {
                    //vamos validar o gênero do cliente
                    if (string.IsNullOrEmpty(dadosCliente.Sexo))
                    {
                        if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ERP_WEBAPI)
                        {
                            lstErros.Add(MensagensErro.GENERO_DO_CLIENTE_NAO_INFORMADO);
                            retorno = false;
                        }
                    }
                    else
                    {
                        if (dadosCliente.Sexo.Length > 1 &&
                            sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ERP_WEBAPI)
                        {
                            lstErros.Add("FORMATO DO TIPO DE SEXO INVÁLIDO!");
                            retorno = false;
                        }
                        if (dadosCliente.Sexo != "M" && dadosCliente.Sexo != "F" &&
                            sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ERP_WEBAPI)
                        {
                            lstErros.Add("INDIQUE QUAL O SEXO.");
                            retorno = false;
                        }

                        retorno = await ValidacoesClienteTelefones.ValidarTelefones_PF(dadosCliente, cliente, lstErros, contextoProvider);

                        if (!string.IsNullOrEmpty(dadosCliente.Email))
                        {
                            retorno = Util.ValidarEmail(dadosCliente.Email, lstErros);
                        }
                    }
                }

                //vamos verificar os campos que não pertence ao tipo PF
                if (!string.IsNullOrEmpty(dadosCliente.Contato))
                {
                    lstErros.Add("Se cliente é tipo PF não deve ter o campo contato preenchido.");
                }
            }
            return retorno;
        }

        private static async Task<bool> ValidarDadosCliente_PJ(Cliente.Dados.DadosClienteCadastroDados dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider)
        {
            /*
            -Para CNPJ:
            *-Validar se tem Razao Social;
            *-Validar CNPJ;
            *-Verificar se tem dados em todos os campos de telefone(Comercial, Comercial2), 
                *pois é obrigatório um número de telefone;
            *-Verificar se tem dados em todos os campos de telefone(Residencial, Comercial), 
                *pois é obrigatório um número de telefone;
            *-Verificar se DDD tem 2 caracteres dos telefones enviados;
            *-Verificar se a quantidade de caracteres dos telefones estão dentro do permitido;
            *-Verificar se tem Contato da empresa;
            *-Verificar se tem E - mail;
            *-Verificar se tem Ref.Bancária e validar caso exista;
            *-Verificar se tem Ref.Comercial e validar caso exista;
            * */

            bool retorno = true;
            if (dadosCliente.ProdutorRural != (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL)
            {
                lstErros.Add("Se cliente é tipo PJ, não pode ser Produtor Rural");
                retorno = false;
            }
            //verificar de criar esses testes
            if (!string.IsNullOrEmpty(dadosCliente.Sexo))
            {
                lstErros.Add("Se cliente é tipo PJ, o sexo não deve ser preenchido.");
                retorno = false;
            }
            if (!string.IsNullOrEmpty(dadosCliente.Rg))
            {
                lstErros.Add("Se cliente é tipo PJ, o RG não deve ser preenchido.");
                retorno = false;
            }
            if (dadosCliente.Nascimento != null)
            {
                lstErros.Add("Se cliente é tipo PJ, o Nascimento não deve ser preenchido.");
                retorno = false;
            }


            if (string.IsNullOrEmpty(dadosCliente.Nome))
            {
                lstErros.Add("PREENCHA A RAZÃO SOCIAL DO CLIENTE.");
                retorno = false;
            }
            if (string.IsNullOrEmpty(dadosCliente.Cnpj_Cpf))
            {
                lstErros.Add("CNPJ NÃO FORNECIDO.");
                retorno = false;
            }
            else
            {
                string cpf_cnpjSoDig = Util.SoDigitosCpf_Cnpj(dadosCliente.Cnpj_Cpf);

                if (cliente != null)
                {
                    //vamos confrontar o cnpj
                    string cnpjCliente = Util.SoDigitosCpf_Cnpj(cliente.Cnpj_Cpf);

                    if (cnpjCliente != cpf_cnpjSoDig)
                    {
                        lstErros.Add("O CNPJ do cliente esta divergindo do cadastro!");
                        return false;
                    }
                }

                if (!Util.ValidaCNPJ(cpf_cnpjSoDig))
                {
                    lstErros.Add(MensagensErro.CNPJ_INVALIDO);
                    retorno = false;
                }
                else
                {
                    //vamos validar o contato da empresa
                    if (string.IsNullOrEmpty(dadosCliente.Contato))
                    {
                        lstErros.Add("INFORME O NOME DA PESSOA PARA CONTATO!");
                        retorno = false;
                    }
                    if (string.IsNullOrEmpty(dadosCliente.Email))
                    {
                        lstErros.Add("É OBRIGATÓRIO INFORMAR UM ENDEREÇO DE E-MAIL!");
                        retorno = false;
                    }
                    if (!string.IsNullOrEmpty(dadosCliente.Email))
                    {
                        retorno = Util.ValidarEmail(dadosCliente.Email, lstErros);
                    }
                    //vamos validar os telefones
                    retorno = await ValidacoesClienteTelefones.ValidarTelefones_PJ(dadosCliente, cliente, lstErros, contextoProvider);
                }
            }

            return retorno;
        }

        private static async Task<bool> ValidarEnderecoCadastroCliente(Cliente.Dados.DadosClienteCadastroDados dadosCliente,
            List<string> lstErros, CepBll cepBll, ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio)
        {
            string cepSoDigito = dadosCliente.Cep.Replace(".", "").Replace("-", "");
            List<Cep.Dados.CepDados> lstCepDados = (await cepBll.BuscarPorCep(cepSoDigito)).ToList();

            bool retorno = true;

            if (lstCepDados.Count == 0)
            {
                lstErros.Add(MensagensErro.Cep_nao_existe);
                return false;
            }


            if (string.IsNullOrEmpty(dadosCliente.Endereco))
            {
                lstErros.Add("PREENCHA O ENDEREÇO.");
                retorno = false;
            }
            else
            {
                if (dadosCliente.Endereco.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
                {
                    lstErros.Add("ENDEREÇO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                        dadosCliente.Endereco.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                        Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");
                    retorno = false;

                }
                if (string.IsNullOrEmpty(dadosCliente.Numero))
                {
                    lstErros.Add("PREENCHA O NÚMERO DO ENDEREÇO.");
                    retorno = false;
                }
                if (string.IsNullOrEmpty(dadosCliente.Bairro))
                {
                    lstErros.Add("PREENCHA O BAIRRO.");
                    retorno = false;
                }
                if (string.IsNullOrEmpty(dadosCliente.Cidade))
                {
                    lstErros.Add("PREENCHA A CIDADE.");
                    retorno = false;
                }

                if (string.IsNullOrEmpty(dadosCliente.Uf))
                {
                    lstErros.Add("INFORME O UF.");
                    retorno = false;
                }
                else
                {
                    if (!Util.VerificaUf(dadosCliente.Uf))
                    {
                        lstErros.Add("UF INVÁLIDA.");
                        retorno = false;
                    }
                }
                if (string.IsNullOrEmpty(dadosCliente.Cep))
                {
                    lstErros.Add("INFORME O CEP.");
                    retorno = false;
                }
                else
                {
                    if (!Util.VerificaCep(dadosCliente.Cep))
                    {
                        lstErros.Add("CEP INVÁLIDO.");
                        retorno = false;
                    }
                }
                //vamos buscar o cep e comparar os endereços 
                Cep.Dados.CepDados cepCliente = new Cep.Dados.CepDados()
                {
                    Cep = dadosCliente.Cep,
                    Endereco = dadosCliente.Endereco,
                    Bairro = dadosCliente.Bairro,
                    Cidade = dadosCliente.Cidade,
                    Uf = dadosCliente.Uf
                };
                if (!await VerificarEndereco(cepCliente, lstCepDados, lstErros, contextoProvider, bancoNFeMunicipio))
                {
                    retorno = false;
                }
            }

            return retorno;
        }

        private static bool ValidarIE_Cliente(Cliente.Dados.DadosClienteCadastroDados dadosCliente, List<string> lstErros,
            ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio, bool flagMsg_IE_Cadastro_PF)
        {
            bool retorno = true;

            //verificar se validaremos qtde de caracteres

            if (string.IsNullOrEmpty(dadosCliente.Ie) &&
                dadosCliente.Contribuinte_Icms_Status ==
                (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
            {
                lstErros.Add("PREENCHA A INSCRIÇÃO ESTADUAL.");
                retorno = false;
            }
            if (dadosCliente.Tipo == Constantes.ID_PF)
            {
                if (dadosCliente.ProdutorRural != (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM &&
                   dadosCliente.ProdutorRural != (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)
                {
                    lstErros.Add("Produtor Rural inválido!");
                    return false;
                }

                if (dadosCliente.ProdutorRural == (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)
                {
                    if (dadosCliente.Contribuinte_Icms_Status != (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL)
                    {
                        lstErros.Add("Se cliente é não Produtor Rural, contribuinte do ICMS tem que ter valor inicial!");
                    }
                    // causa erros nos testes essa validação
                    if (!string.IsNullOrEmpty(dadosCliente.Ie))
                    {
                        lstErros.Add("Se o cliente é não Produtor Rural, o IE não deve ser preenchido!");
                    }
                }

                if (dadosCliente.ProdutorRural == (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                {
                    if (dadosCliente.Contribuinte_Icms_Status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL)
                    {
                        lstErros.Add("Para ser cadastrado como Produtor Rural o contribuinte do ICMS não pode ter valor inicial!");
                        retorno = false;
                    }

                    if (dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                        dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                    {
                        lstErros.Add("Contribuinte do ICMS inválido");
                        return false;
                    }

                    if (dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    {
                        lstErros.Add("Para ser cadastrado como Produtor Rural, " +
                            "é necessário ser contribuinte do ICMS e possuir nº de IE");
                        retorno = false;
                    }

                    if (string.IsNullOrEmpty(dadosCliente.Ie) &&
                        dadosCliente.Contribuinte_Icms_Status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    {
                        lstErros.Add("Para ser cadastrado como Produtor Rural e contribuinte do ICMS" +
                            " é necessário possuir nº de IE");
                        retorno = false;
                    }

                    if (!string.IsNullOrEmpty(dadosCliente.Ie) &&
                        dadosCliente.Contribuinte_Icms_Status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                    {
                        lstErros.Add("Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!");
                        retorno = false;
                    }
                }
            }
            if (dadosCliente.Tipo == Constantes.ID_PJ)
            {
                if (dadosCliente.ProdutorRural != (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL)
                {
                    lstErros.Add("Se tipo cliente PJ, o valor de Produtor Rural tem quer ser inicial!");
                    return false;
                }

                if (dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                        dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    lstErros.Add("Contribuinte do ICMS inválido");
                    return false;
                }

                if (!string.IsNullOrEmpty(dadosCliente.Ie) &&
                    dadosCliente.Contribuinte_Icms_Status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    lstErros.Add("Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!");
                    retorno = false;
                }

                if (dadosCliente.Ie == "" &&
                    dadosCliente.Contribuinte_Icms_Status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                {
                    lstErros.Add("Para ser cadastrado como contribuinte do ICMS , " +
                            "é necessário possuir nº de IE");
                    retorno = false;
                }
                //if (!string.IsNullOrEmpty(dadosCliente.Ie) &&
                //    dadosCliente.Contribuinte_Icms_Status ==
                //    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                //{
                //    lstErros.Add("Se o cliente é não contribuinte do ICMS a inscrição estadual deve ser preenchida!");
                //}
                if (dadosCliente.Contribuinte_Icms_Status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL)
                {
                    lstErros.Add("Preencha o contribuinte do ICMS corretamente.");
                    retorno = false;
                }
            }

            if (!string.IsNullOrEmpty(dadosCliente.Ie))
            {
                if (dadosCliente.Contribuinte_Icms_Status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                    dadosCliente.Ie.ToUpper().IndexOf("ISEN") > -1)
                    lstErros.Add("Se cliente é não contribuinte do ICMS, " +
                        "não pode ter o valor ISENTO no campo de Inscrição Estadual!");

                if (dadosCliente.Contribuinte_Icms_Status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                    dadosCliente.Ie.ToUpper().IndexOf("ISEN") > -1)
                    lstErros.Add("Se cliente é contribuinte do ICMS, " +
                        "não pode ter o valor ISENTO no campo de Inscrição Estadual!");

                //if (lstErros.Count == 0)
                VerificarInscricaoEstadualValida(dadosCliente.Ie, dadosCliente.Uf, lstErros,
                    flagMsg_IE_Cadastro_PF);
            }

            return retorno;
        }

        private static bool ValidarReferencias_Bancarias_Comerciais(List<Cliente.Dados.Referencias.RefBancariaClienteDados> lstRefBancaria,
            List<Cliente.Dados.Referencias.RefComercialClienteDados> lstRefComercial,
            List<string> lstErros, string tipoPessoa, List<Cliente.Dados.ListaBancoDados> lstBanco)
        {
            bool retorno = true;
            if (lstRefBancaria != null && lstRefBancaria.Count > 0)
            {
                if (tipoPessoa == Constantes.ID_PJ)
                {
                    if (lstRefBancaria.Count > Constantes.MAX_REF_BANCARIA_CLIENTE_PJ)
                    {
                        lstErros.Add("É permitido apenas " + Constantes.MAX_REF_BANCARIA_CLIENTE_PJ + " referência bancária!");
                        retorno = false;
                    }
                }

                lstRefBancaria.ForEach(x =>
                {
                    var codigoBanco = lstBanco.Where(y => y.Codigo == x.Banco).Select(y => y.Codigo).FirstOrDefault();
                    if (codigoBanco == null)
                    {
                        lstErros.Add("Ref Bancária: código do banco inválido");
                    }

                    if (string.IsNullOrEmpty(x.Banco))
                    {
                        lstErros.Add("Ref Bancária (" + x.Ordem.ToString() + "): informe o banco.");
                        retorno = false;
                    }
                    if (string.IsNullOrEmpty(x.Agencia))
                    {
                        lstErros.Add("Ref Bancária (" + x.Ordem.ToString() + "): informe o agência.");
                        retorno = false;
                    }
                    if (string.IsNullOrEmpty(x.Conta))
                    {
                        lstErros.Add("Ref Bancária (" + x.Ordem.ToString() + "): informe o número da conta.");
                        retorno = false;
                    }
                });



            }

            if (retorno)
            {
                if (lstRefComercial != null && lstRefComercial.Count > 0)
                {
                    if (lstRefComercial.Count > Constantes.MAX_REF_COMERCIAL_CLIENTE_PJ)
                    {
                        lstErros.Add("É permitido apenas " + Constantes.MAX_REF_COMERCIAL_CLIENTE_PJ + " referências comerciais!");
                        retorno = false;
                    }

                    var lsteRefComercialRepetido = lstRefComercial.GroupBy(o => o.Nome_Empresa)
                        .Where(g => g.Count() > 1)
                        .Select(y => new { Nome_Emprea = y.Key, Qtde = y.Count() })
                        .ToList();

                    if (lsteRefComercialRepetido.Count > 0)
                    {
                        lstErros.Add("Referência comercial: " + lsteRefComercialRepetido[0].Nome_Emprea +
                            " está duplicada " + lsteRefComercialRepetido[0].Qtde + " vezes!");
                        retorno = false;
                    }

                    int i = 0;
                    lstRefComercial.ForEach(x =>
                    {
                        x.Ordem = i++;
                        if (string.IsNullOrEmpty(x.Nome_Empresa))
                        {
                            lstErros.Add("Ref Comercial (" + x.Ordem + "): informe o nome da empresa.");
                            retorno = false;
                        }
                    });
                }
            }

            return retorno;
        }

        public static void VerificarInscricaoEstadualValida(string ie, string uf, List<string> listaErros,
            bool flagMsg_IE_Cadastro_PF)
        {
            if (string.IsNullOrEmpty(ie))
            {
                listaErros.Add("IE (Inscrição Estadual) vazia! ");
                return;
            }
            if (string.IsNullOrEmpty(uf))
            {
                listaErros.Add("UF (estado) vazio! ");
                return;
            }

            string c;
            int qtdeDig = 0;
            int num;

            if (ie.ToUpper() != "ISENTO")
            {
                for (int i = 0; i < ie.Length; i++)
                {
                    c = ie.Substring(i, 1);

                    if (int.TryParse(c, out num))
                        qtdeDig += 1;
                }
                if (qtdeDig < 2 && qtdeDig > 14)
                {
                    listaErros.Add(MensagensErro.Preencha_a_IE_Inscricao_Estadual);
                    return;
                }

            }

            bool blnResultado;

            //se o cliente for PF e tiver iE não podemos validar
            blnResultado = isInscricaoEstadualOkCom(ie, uf);
            if (!blnResultado)
            {
                if (!flagMsg_IE_Cadastro_PF)
                {
                    //essa deve aparecer quando for cadastro de pepedido PJ e cadastro novo de cliente  
                    listaErros.Add(MensagensErro.Preencha_a_IE_Inscricao_Estadual);
                    return;
                }
                else
                {
                    //só iremos entrar aqui se for atualização de cadastro pelo angular
                    //essa msg só deve aparecer quando é cadastro de prepedido PF que vem da Unis
                    //ou atualização de cadastro
                    //Não tem possibilidade do pedido PF entrar aqui, pois se ele alterar o IE,  
                    //na validação da atualização do cadastro ele vai entrar aqui
                    listaErros.Add("Inscrição Estadual inválida pra esse estado (" + uf.ToUpper() + "). " +
                        "Caso o cliente esteja em outro estado, entre em contato com o suporte " +
                        "para alterar o cadastro do cliente");

                    return;
                }
            }
        }

        private static bool isInscricaoEstadualOkCom(string ie, string uf)
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

        public static async Task<bool> VerificarEndereco(Cep.Dados.CepDados cepCliente, List<Cep.Dados.CepDados> lstCepDados,
            List<string> lstErros, ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio)
        {
            bool retorno = true;

            string cepSoDigito = cepCliente.Cep.Replace(".", "").Replace("-", "");

            if (lstCepDados != null && lstCepDados.Count > 0)
            {
                foreach (var c in lstCepDados)
                {
                    //não verificamos a cidade porque ela deve estar no cadastro da NFE (IBGE) e não necessariamente igual à do CEP

                    //vamos verificar se tem endereco e bairro para verificar se foi alterado os dados de cep
                    if (!string.IsNullOrEmpty(c.Cep) && c.Cep != cepSoDigito)
                    {
                        lstErros.Add("Número do Cep não confere!");
                        retorno = false;
                    }

                    /* REUNIÃO 21/07/2020 com HAMILTON => solicitado remoção da confrontação de endereço e bairro
                     * 
                     *  cadastros de clientes que estão com nome da rua diferente da tabela de cep 
                     *  ex: cliente "01.824.328/0001-95", contém "AV" no cadastro, na tabela de cep retorna "Avenida" 
                     *  e isso difere na confrontação de endereço
                     */


                    //vamos verificar se a cidade da lista de cep existe no IBGE para validar
                    if (!string.IsNullOrEmpty(cepCliente.Cidade) && !string.IsNullOrEmpty(c.Cidade))
                    {
                        if (await CepBll.ConsisteMunicipioIBGE(c.Cidade, c.Uf, lstErros, contextoProvider, bancoNFeMunicipio, false))
                        {
                            if (Util.RemoverAcentuacao(c.Cidade.ToUpper()) != Util.RemoverAcentuacao(cepCliente.Cidade.ToUpper()))
                            {
                                lstErros.Add("Cidade não confere");

                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(c.Uf) && c.Uf.ToUpper() != cepCliente.Uf.ToUpper())
                    {
                        lstErros.Add(MensagensErro.Estado_nao_confere);
                        retorno = false;
                    }
                }
            }

            return retorno;
        }
    }
}
