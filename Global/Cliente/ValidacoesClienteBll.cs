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
            bool validarReferenciasBancasUsandoLstBanco, //durante o cadastro do pedido não queremos validar as referências bancárias porque elas não existem
            List<Cliente.Dados.Referencias.RefBancariaClienteDados> lstRefBancaria,
            List<Cliente.Dados.Referencias.RefComercialClienteDados> lstRefComercial,
            List<string> lstErros, ContextoBdProvider contextoProvider, CepBll cepBll, IBancoNFeMunicipio bancoNFeMunicipio,
            List<Cliente.Dados.ListaBancoDados> lstBanco, bool flagMsg_IE_Cadastro_PF,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel, bool novoCliente)
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
                        await ValidarDadosCliente_PF(dadosCliente, cliente, lstErros, contextoProvider, sistemaResponsavel, novoCliente);
                    }
                    if (dadosCliente.Tipo == Constantes.ID_PJ)
                    {
                        tipoDesconhecido = false;
                        await ValidarDadosCliente_PJ(dadosCliente, cliente, lstErros, contextoProvider, sistemaResponsavel);
                        //vamos validar as referências
                        if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
                        {
                            if (validarReferenciasBancasUsandoLstBanco && lstBanco != null)
                            {
                                ValidarReferencias_Bancarias_Comerciais(lstRefBancaria, lstRefComercial,
                                lstErros, dadosCliente.Tipo, lstBanco);
                            }
                        }
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
        private static async Task ValidarDadosCliente_PF(Cliente.Dados.DadosClienteCadastroDados dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel, bool novoCliente)
        {

            if (string.IsNullOrEmpty(dadosCliente.Nome))
            {
                lstErros.Add("PREENCHA O NOME DO CLIENTE.");
            }

            if (string.IsNullOrEmpty(dadosCliente.Cnpj_Cpf))
            {
                lstErros.Add(MensagensErro.CPF_NAO_FORNECIDO);
            }
            //validamos o cpf que esta vindo
            string cpf_cnpjSoDig = Util.SoDigitosCpf_Cnpj(dadosCliente.Cnpj_Cpf);
            if (!Util.ValidaCPF(cpf_cnpjSoDig))
            {
                lstErros.Add(MensagensErro.CPF_INVALIDO);
            }

            //se não for cadastro de cliente validamos
            if (cliente != null && novoCliente)
            {
                string cpfCliente = Util.SoDigitosCpf_Cnpj(cliente.Cnpj_Cpf);
                //vamos validar o cpf
                if (cpfCliente != cpf_cnpjSoDig)
                {
                    lstErros.Add("O CPF do cliente esta divergindo do cadastro!");
                }
            }

            //vamos validar o sexo
            ValidarGenero(dadosCliente, lstErros, sistemaResponsavel, novoCliente);

            //validar nascimento
            ValidarNascimento(dadosCliente, lstErros, sistemaResponsavel, novoCliente);

            await ValidacoesClienteTelefones.ValidarTelefones_PF(dadosCliente, cliente, lstErros, contextoProvider, sistemaResponsavel);

            if (!string.IsNullOrEmpty(dadosCliente.Email))
            {
                Util.ValidarEmail(dadosCliente.Email, lstErros);
            }

            if (!string.IsNullOrEmpty(dadosCliente.EmailXml))
            {
                Util.ValidarEmailXml(dadosCliente.EmailXml, lstErros);
            }

            //vamos verificar os campos que não pertence ao tipo PF
            if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
            {
                if (!string.IsNullOrEmpty(dadosCliente.Contato))
                {
                    lstErros.Add("Se cliente é tipo PF não deve ter o campo contato preenchido.");
                }
            }
        }

        private static void ValidarNascimento(Cliente.Dados.DadosClienteCadastroDados dadosCliente, List<string> lstErros,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel, bool novoCliente)
        {
            if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ERP_WEBAPI &&
                novoCliente)
            {
                if (dadosCliente.Nascimento.HasValue)
                {
                    if (!DateTime.TryParse(dadosCliente.Nascimento.ToString(), out DateTime value))
                        lstErros.Add("Data de nascimento inválida!");
                    //vamos validar
                    if (dadosCliente.Nascimento.Value.Year <= 1900 ||
                        dadosCliente.Nascimento.Value.Year.ToString().Length < 4)
                        lstErros.Add("Data de nascimento inválida!");
                    if (dadosCliente.Nascimento.Value.Day >= DateTime.Now.Day &&
                        dadosCliente.Nascimento.Value.Month >= DateTime.Now.Month &&
                        dadosCliente.Nascimento.Value.Year >= DateTime.Now.Year)
                        lstErros.Add("Data de nascimento não pode ser igual ou maior que a data atual!");
                }                
            }
        }

        private static void ValidarGenero(Cliente.Dados.DadosClienteCadastroDados dadosCliente, List<string> lstErros,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel, bool novoCliente)
        {
            if (string.IsNullOrEmpty(dadosCliente.Sexo))
            {
                if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO &&
                    novoCliente)
                {
                    lstErros.Add(MensagensErro.GENERO_DO_CLIENTE_NAO_INFORMADO);
                }
            }
            var sexo = dadosCliente.Sexo ?? "";
            if (sexo.Length > 1 || (sexo != "M" && sexo != "F") &&
                sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO && novoCliente)
            {
                lstErros.Add("FORMATO DO TIPO DE SEXO INVÁLIDO!");

            }
        }
        private static async Task ValidarDadosCliente_PJ(Cliente.Dados.DadosClienteCadastroDados dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider, InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
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
            if (dadosCliente.ProdutorRural != (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL)
            {
                lstErros.Add("Se cliente é tipo PJ, não pode ser Produtor Rural");
            }
            //verificar de criar esses testes
            if (!string.IsNullOrEmpty(dadosCliente.Sexo))
            {
                lstErros.Add("Se cliente é tipo PJ, o sexo não deve ser preenchido.");
            }
            if (!string.IsNullOrEmpty(dadosCliente.Rg))
            {
                lstErros.Add("Se cliente é tipo PJ, o RG não deve ser preenchido.");
            }
            if (dadosCliente.Nascimento != null)
            {
                lstErros.Add("Se cliente é tipo PJ, o Nascimento não deve ser preenchido.");
            }


            if (string.IsNullOrEmpty(dadosCliente.Nome))
            {
                lstErros.Add("PREENCHA A RAZÃO SOCIAL DO CLIENTE.");
            }
            if (string.IsNullOrEmpty(dadosCliente.Cnpj_Cpf))
            {
                lstErros.Add("CNPJ NÃO FORNECIDO.");
            }

            string cpf_cnpjSoDig = Util.SoDigitosCpf_Cnpj(dadosCliente.Cnpj_Cpf);
            if (!Util.ValidaCNPJ(cpf_cnpjSoDig))
            {
                lstErros.Add(MensagensErro.CNPJ_INVALIDO);
            }
            //se for cadastro não confrontamos
            if (cliente != null)
            {
                string cnpjCliente = Util.SoDigitosCpf_Cnpj(cliente.Cnpj_Cpf);

                //vamos confrontar o cnpj
                if (cnpjCliente != cpf_cnpjSoDig)
                {
                    lstErros.Add("O CNPJ do cliente esta divergindo do cadastro!");
                }
            }

            //vamos validar o contato da empresa
            if (string.IsNullOrEmpty(dadosCliente.Contato))
            {
                lstErros.Add("INFORME O NOME DA PESSOA PARA CONTATO!");
            }
            if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
            {
                if (string.IsNullOrEmpty(dadosCliente.Email))
                {
                    lstErros.Add("É OBRIGATÓRIO INFORMAR UM ENDEREÇO DE E-MAIL!");
                }
            }
            if (!string.IsNullOrEmpty(dadosCliente.Email))
            {
                Util.ValidarEmail(dadosCliente.Email, lstErros);
            }
            //vamos validar os telefones
            await ValidacoesClienteTelefones.ValidarTelefones_PJ(dadosCliente, cliente, lstErros, contextoProvider, sistemaResponsavel);
        }

        private static async Task ValidarEnderecoCadastroCliente(Cliente.Dados.DadosClienteCadastroDados dadosCliente,
            List<string> lstErros, CepBll cepBll, ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio)
        {
            string cepSoDigito = dadosCliente.Cep.Replace(".", "").Replace("-", "");
            List<Cep.Dados.CepDados> lstCepDados = (await cepBll.BuscarPorCep(cepSoDigito)).ToList();

            if (lstCepDados.Count == 0)
            {
                lstErros.Add(MensagensErro.Cep_nao_existe);
            }

            if (string.IsNullOrEmpty(dadosCliente.Endereco))
            {
                lstErros.Add("PREENCHA O ENDEREÇO.");
            }
            if (string.IsNullOrEmpty(dadosCliente.Numero))
            {
                lstErros.Add("PREENCHA O NÚMERO DO ENDEREÇO.");
            }
            if (string.IsNullOrEmpty(dadosCliente.Bairro))
            {
                lstErros.Add("PREENCHA O BAIRRO.");
            }
            if (string.IsNullOrEmpty(dadosCliente.Cidade))
            {
                lstErros.Add("PREENCHA A CIDADE.");
            }

            if (string.IsNullOrEmpty(dadosCliente.Uf))
            {
                lstErros.Add("INFORME O UF.");
            }
            else
            {
                if (!Util.VerificaUf(dadosCliente.Uf))
                {
                    lstErros.Add("UF INVÁLIDA.");
                }
            }
            if (string.IsNullOrEmpty(dadosCliente.Cep))
            {
                lstErros.Add("INFORME O CEP.");
            }
            else
            {
                if (!Util.VerificaCep(dadosCliente.Cep))
                {
                    lstErros.Add("CEP INVÁLIDO.");
                }
            }

            //vamos verificar a quantidade de caracteres de cada campo
            VerificarQtdeCaracteresDoEndereco(dadosCliente, lstErros);

            //vamos buscar o cep e comparar os endereços 
            Cep.Dados.CepDados cepCliente = new Cep.Dados.CepDados()
            {
                Cep = dadosCliente.Cep,
                Endereco = dadosCliente.Endereco,
                Bairro = dadosCliente.Bairro,
                Cidade = dadosCliente.Cidade,
                Uf = dadosCliente.Uf
            };



            await VerificarEndereco(cepCliente, lstCepDados, lstErros, contextoProvider, bancoNFeMunicipio);
        }

        private static void VerificarQtdeCaracteresDoEndereco(Cliente.Dados.DadosClienteCadastroDados dadosCliente,
            List<string> lstErros)
        {
            if (dadosCliente.Endereco?.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
            {
                lstErros.Add("ENDEREÇO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    dadosCliente.Endereco.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");

            }
            if (dadosCliente.Numero?.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO_NUMERO)
            {
                lstErros.Add("NÚMERO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    dadosCliente.Numero.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    Constantes.MAX_TAMANHO_CAMPO_ENDERECO_NUMERO + " CARACTERES");

            }
            if (dadosCliente.Complemento?.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO_COMPLEMENTO)
            {
                lstErros.Add("COMPLEMENTO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    dadosCliente.Complemento.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    Constantes.MAX_TAMANHO_CAMPO_ENDERECO_COMPLEMENTO + " CARACTERES");

            }
            if (dadosCliente.Bairro?.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO_BAIRRO)
            {
                lstErros.Add("BAIRRO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    dadosCliente.Bairro.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    Constantes.MAX_TAMANHO_CAMPO_ENDERECO_BAIRRO + " CARACTERES");

            }
            if (dadosCliente.Cidade?.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO_CIDADE)
            {
                lstErros.Add("CIDADE EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    dadosCliente.Cidade.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    Constantes.MAX_TAMANHO_CAMPO_ENDERECO_CIDADE + " CARACTERES");
            }
        }

        private static void ValidarIE_Cliente(Cliente.Dados.DadosClienteCadastroDados dadosCliente, List<string> lstErros,
            ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio, bool flagMsg_IE_Cadastro_PF)
        {
            //verificar se validaremos qtde de caracteres

            if (string.IsNullOrEmpty(dadosCliente.Ie) &&
                dadosCliente.Contribuinte_Icms_Status ==
                (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
            {
                lstErros.Add("PREENCHA A INSCRIÇÃO ESTADUAL.");
            }
            if (dadosCliente.Tipo == Constantes.ID_PF)
            {
                if (dadosCliente.ProdutorRural != (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM &&
                   dadosCliente.ProdutorRural != (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)
                {
                    lstErros.Add("Produtor Rural inválido!");
                }

                if (dadosCliente.ProdutorRural == (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)
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

                if (dadosCliente.ProdutorRural == (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                {
                    if (dadosCliente.Contribuinte_Icms_Status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL)
                    {
                        lstErros.Add("Para ser cadastrado como Produtor Rural o contribuinte do ICMS não pode ter valor inicial!");
                    }

                    if (dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                        dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                    {
                        lstErros.Add("Contribuinte do ICMS inválido");
                    }

                    if (dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    {
                        lstErros.Add("Para ser cadastrado como Produtor Rural, " +
                            "é necessário ser contribuinte do ICMS e possuir nº de IE");
                    }

                    if (string.IsNullOrEmpty(dadosCliente.Ie) &&
                        dadosCliente.Contribuinte_Icms_Status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    {
                        lstErros.Add("Para ser cadastrado como Produtor Rural e contribuinte do ICMS" +
                            " é necessário possuir nº de IE");
                    }

                    if (!string.IsNullOrEmpty(dadosCliente.Ie) &&
                        dadosCliente.Contribuinte_Icms_Status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                    {
                        lstErros.Add("Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!");
                    }
                }
            }
            if (dadosCliente.Tipo == Constantes.ID_PJ)
            {
                if (dadosCliente.ProdutorRural != (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL)
                {
                    lstErros.Add("Se tipo cliente PJ, o valor de Produtor Rural tem quer ser inicial!");
                }

                if (dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                        dadosCliente.Contribuinte_Icms_Status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    lstErros.Add("Contribuinte do ICMS inválido");
                }

                if (!string.IsNullOrEmpty(dadosCliente.Ie) &&
                    dadosCliente.Contribuinte_Icms_Status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    lstErros.Add("Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!");
                }

                if (dadosCliente.Ie == "" &&
                    dadosCliente.Contribuinte_Icms_Status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                {
                    lstErros.Add("Para ser cadastrado como contribuinte do ICMS , " +
                            "é necessário possuir nº de IE");
                }
                if (dadosCliente.Contribuinte_Icms_Status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL)
                {
                    lstErros.Add("Preencha o contribuinte do ICMS corretamente.");
                }
            }

            ValidarIE(dadosCliente, lstErros, flagMsg_IE_Cadastro_PF);

        }

        private static void ValidarIE(Cliente.Dados.DadosClienteCadastroDados dadosCliente, List<string> lstErros,
            bool flagMsg_IE_Cadastro_PF)
        {
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

                if ((dadosCliente.Tipo == Constantes.ID_PF && dadosCliente.ProdutorRural ==
                    (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) ||
                    (dadosCliente.Tipo == Constantes.ID_PJ &&
                    dadosCliente.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM) ||
                    (dadosCliente.Tipo == Constantes.ID_PJ &&
                    dadosCliente.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO) && !string.IsNullOrEmpty(dadosCliente.Ie))
                {
                    VerificarInscricaoEstadualValida(dadosCliente.Ie, dadosCliente.Uf, lstErros, flagMsg_IE_Cadastro_PF);
                }
            }
        }

        private static void ValidarReferencias_Bancarias_Comerciais(List<Cliente.Dados.Referencias.RefBancariaClienteDados> lstRefBancaria,
            List<Cliente.Dados.Referencias.RefComercialClienteDados> lstRefComercial,
            List<string> lstErros, string tipoPessoa, List<Cliente.Dados.ListaBancoDados> lstBanco)
        {
            if (lstRefBancaria != null && lstRefBancaria.Count > 0)
            {
                if (tipoPessoa == Constantes.ID_PJ)
                {
                    if (lstRefBancaria.Count > Constantes.MAX_REF_BANCARIA_CLIENTE_PJ)
                    {
                        lstErros.Add("É permitido apenas " + Constantes.MAX_REF_BANCARIA_CLIENTE_PJ + " referência bancária!");
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
                    }
                    if (string.IsNullOrEmpty(x.Agencia))
                    {
                        lstErros.Add("Ref Bancária (" + x.Ordem.ToString() + "): informe o agência.");
                    }
                    if (string.IsNullOrEmpty(x.Conta))
                    {
                        lstErros.Add("Ref Bancária (" + x.Ordem.ToString() + "): informe o número da conta.");
                    }
                });

            }

            if (lstRefComercial != null && lstRefComercial.Count > 0)
            {
                if (lstRefComercial.Count > Constantes.MAX_REF_COMERCIAL_CLIENTE_PJ)
                {
                    lstErros.Add("É permitido apenas " + Constantes.MAX_REF_COMERCIAL_CLIENTE_PJ + " referências comerciais!");
                }

                var lsteRefComercialRepetido = lstRefComercial.GroupBy(o => o.Nome_Empresa)
                    .Where(g => g.Count() > 1)
                    .Select(y => new { Nome_Emprea = y.Key, Qtde = y.Count() })
                    .ToList();

                if (lsteRefComercialRepetido.Count > 0)
                {
                    lstErros.Add("Referência comercial: " + lsteRefComercialRepetido[0].Nome_Emprea +
                        " está duplicada " + lsteRefComercialRepetido[0].Qtde + " vezes!");
                }

                int i = 0;
                lstRefComercial.ForEach(x =>
                {
                    x.Ordem = i++;
                    if (string.IsNullOrEmpty(x.Nome_Empresa))
                    {
                        lstErros.Add("Ref Comercial (" + x.Ordem + "): informe o nome da empresa.");
                    }
                });
            }
        }

        public static void VerificarInscricaoEstadualValida(string ie, string uf, List<string> listaErros,
            bool flagMsg_IE_Cadastro_PF)
        {
            if (string.IsNullOrEmpty(ie))
            {
                listaErros.Add("IE (Inscrição Estadual) vazia!");
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

        public static async Task VerificarEndereco(Cep.Dados.CepDados cepCliente, List<Cep.Dados.CepDados> lstCepDados,
            List<string> lstErros, ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio)
        {
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
                    }
                }
            }

        }
    }
}
