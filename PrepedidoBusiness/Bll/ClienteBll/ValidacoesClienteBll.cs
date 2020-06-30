using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using InfraBanco;
using Microsoft.EntityFrameworkCore;
using InfraBanco.Constantes;
using PrepedidoBusiness.Utils;
using InfraBanco.Modelos;
using PrepedidoBusiness.Dto.ClienteCadastro;
using PrepedidoBusiness.Dto.ClienteCadastro.Referencias;
using System.Reflection;
using PrepedidoBusiness.Dto.Cep;

namespace PrepedidoBusiness.Bll.ClienteBll
{
    public class ValidacoesClienteBll
    {
        public static async Task<bool> ValidarDadosCliente(DadosClienteCadastroDto dadosCliente,
            List<RefBancariaDtoCliente> lstRefBancaria, List<RefComercialDtoCliente> lstRefComercial,
            List<string> lstErros, ContextoBdProvider contextoProvider, CepBll cepBll)
        {
            bool retorno = false;

            if (dadosCliente != null)
            {
                //existe dados
                if (!string.IsNullOrEmpty(dadosCliente.Tipo))
                {
                    //é cliente PF
                    if (dadosCliente.Tipo == Constantes.ID_PF)
                    {
                        //vamos verificar e validar os dados referente ao cliente PF
                        retorno = ValidarDadosCliente_PF(dadosCliente, lstErros);
                    }
                    if (dadosCliente.Tipo == Constantes.ID_PJ)
                    {
                        retorno = ValidarDadosCliente_PJ(dadosCliente, lstErros);
                        //vamos validar as referências
                        retorno = ValidarReferencias_Bancarias_Comerciais(lstRefBancaria, lstRefComercial,
                            lstErros);
                    }

                    //validar endereço do cadastro                    
                    retorno = await ValidarEnderecoCadastroClienteUnis(dadosCliente, lstErros, cepBll);

                    //vamos verificar o IE dos clientes
                    retorno = await ValidarIE_Cliente(dadosCliente, lstErros, contextoProvider);
                }
            }
            else
            {
                lstErros.Add("DADOS DO CLIENTE ESTA VAZIO!");
                retorno = false;
            }

            return retorno;
        }

        private static bool ValidarDadosCliente_PF(DadosClienteCadastroDto dadosCliente, List<string> lstErros)
        {
            bool retorno = true;

            if (string.IsNullOrEmpty(dadosCliente.Nome))
            {
                lstErros.Add("PREENCHA O NOME DO CLIENTE.");
                retorno = false;
            }
            if (string.IsNullOrEmpty(dadosCliente.Cnpj_Cpf))
            {
                lstErros.Add("CPF NÃO FORNECIDO.");
                retorno = false;
            }
            else
            {
                //vamos validar o cpf
                string cpf_cnpjSoDig = Util.SoDigitosCpf_Cnpj(dadosCliente.Cnpj_Cpf);
                if (!Util.ValidaCPF(cpf_cnpjSoDig))
                {
                    lstErros.Add("CPF INVÁLIDO.");
                    retorno = false;
                }
                else
                {
                    //vamos validar o gênero do cliente
                    if (string.IsNullOrEmpty(dadosCliente.Sexo))
                    {
                        lstErros.Add("GÊNERO DO CLIENTE NÃO INFORMADO!.");
                        retorno = false;
                    }
                    else
                    {
                        if (dadosCliente.Sexo.Length > 1)
                        {
                            lstErros.Add("FORMATO DO TIPO DE SEXO INVÁLIDO!");
                            retorno = false;
                        }
                        if (dadosCliente.Sexo != "M" && dadosCliente.Sexo != "F")
                        {
                            lstErros.Add("INDIQUE QUAL O SEXO.");
                            retorno = false;
                        }

                        retorno = ValidarTelefones_PF(dadosCliente, lstErros);
                    }
                }
            }
            return retorno;
        }

        private static bool ValidarTelefones_PF(DadosClienteCadastroDto dadosCliente, List<string> lstErros)
        {
            bool retorno = true;

            if (dadosCliente.Tipo == Constantes.ID_PF && string.IsNullOrEmpty(dadosCliente.TelefoneResidencial) &&
                string.IsNullOrEmpty(dadosCliente.TelComercial) && string.IsNullOrEmpty(dadosCliente.Celular))
            {
                lstErros.Add("PREENCHA PELO MENOS UM TELEFONE.");
                retorno = false;
            }
            if (dadosCliente.DddResidencial.Length != 2 &&
                !string.IsNullOrEmpty(dadosCliente.DddResidencial))
            {
                lstErros.Add("DDD INVÁLIDO.");
                retorno = false;
            }
            if (dadosCliente.TelefoneResidencial.Length < 6 &&
                !string.IsNullOrEmpty(dadosCliente.TelefoneResidencial))
            {
                lstErros.Add("TELEFONE RESIDENCIAL INVÁLIDO.");
                retorno = false;
            }
            if (!string.IsNullOrEmpty(dadosCliente.DddResidencial) &&
                string.IsNullOrEmpty(dadosCliente.TelefoneResidencial))
            {
                lstErros.Add("PREENCHA O TELEFONE RESIDENCIAL.");
                retorno = false;
            }
            if (string.IsNullOrEmpty(dadosCliente.DddResidencial) &&
                !string.IsNullOrEmpty(dadosCliente.TelefoneResidencial))
            {
                lstErros.Add("PREENCHA O DDD.");
                retorno = false;
            }
            if (!string.IsNullOrEmpty(dadosCliente.TelComercial) &&
                string.IsNullOrEmpty(dadosCliente.DddComercial))
            {
                lstErros.Add("PREENCHA O DDD COMERCIAL.");
                retorno = false;
            }
            if (!string.IsNullOrEmpty(dadosCliente.DddComercial) &&
                string.IsNullOrEmpty(dadosCliente.TelComercial))
            {
                lstErros.Add("PREENCHA O TELEFONE COMERCIAL.");
                retorno = false;
            }
            if (!string.IsNullOrEmpty(dadosCliente.TelComercial) &&
                dadosCliente.TelComercial.Length < 6)
            {
                lstErros.Add("TELEFONE COMERCIAL INVÁLIDO.");
                retorno = false;
            }
            return retorno;
        }

        private static bool ValidarDadosCliente_PJ(DadosClienteCadastroDto dadosCliente, List<string> lstErros)
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
                if (!Util.ValidaCNPJ(cpf_cnpjSoDig))
                {
                    lstErros.Add("CNPJ INVÁLIDO.");
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
                    //vamos validar os telefones
                    retorno = ValidarTelefones_PJ(dadosCliente, lstErros);
                }
            }

            return retorno;
        }

        private static bool ValidarTelefones_PJ(DadosClienteCadastroDto dadosCliente, List<string> lstErros)
        {
            bool retorno = true;

            if (dadosCliente.Tipo == Constantes.ID_PJ && string.IsNullOrEmpty(dadosCliente.TelComercial) &&
                string.IsNullOrEmpty(dadosCliente.TelComercial2))
            {
                lstErros.Add("PREENCHA AO MENOS UM TELEFONE!");
                retorno = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(dadosCliente.TelComercial))
                {
                    if (Util.Telefone_SoDigito(dadosCliente.TelComercial).Length < 6)
                    {
                        lstErros.Add("TELEFONE COMERCIAL INVÁLIDO.");
                        retorno = false;
                    }
                    if (!string.IsNullOrEmpty(dadosCliente.DddComercial))
                    {
                        if (dadosCliente.DddComercial.Length != 2)
                        {
                            lstErros.Add("DDD DO TELEFONE COMERCIAL INVÁLIDO.");
                            retorno = false;
                        }
                    }
                    else
                    {
                        lstErros.Add("PREENCHA O DDD DO TELEFONE COMERCIAL.");
                        retorno = false;
                    }
                }
                if (!string.IsNullOrEmpty(dadosCliente.DddComercial) &&
                    string.IsNullOrEmpty(dadosCliente.TelComercial))
                {
                    lstErros.Add("PREENCHA O TELEFONE COMERCIAL.");
                    retorno = false;
                }

                if (!string.IsNullOrEmpty(dadosCliente.TelComercial2))
                {
                    if (Util.Telefone_SoDigito(dadosCliente.TelComercial2).Length < 6)
                    {
                        lstErros.Add("TELEFONE COMERCIAL2 INVÁLIDO.");
                        retorno = false;
                    }
                    if (!string.IsNullOrEmpty(dadosCliente.DddComercial2))
                    {
                        if (dadosCliente.DddComercial2.Length != 2)
                        {
                            lstErros.Add("DDD DO TELEFONE COMERCIAL2 INVÁLIDO.");
                            retorno = false;
                        }
                    }
                    else
                    {
                        lstErros.Add("PREENCHA O DDD DO TELEFONE COMERCIAL2.");
                        retorno = false;
                    }
                }
                if (!string.IsNullOrEmpty(dadosCliente.DddComercial2) &&
                    string.IsNullOrEmpty(dadosCliente.TelComercial2))
                {
                    lstErros.Add("PREENCHA O TELEFONE COMERCIAL.");
                    retorno = false;
                }
            }

            return retorno;
        }

        private static async Task<bool> ValidarEnderecoCadastroClienteUnis(DadosClienteCadastroDto dadosCliente,
            List<string> lstErros, CepBll cepBll)
        {
            string cepSoDigito = dadosCliente.Cep.Replace(".", "").Replace("-", "");
            List<CepDto> lstCepDto = (await cepBll.BuscarPorCep(cepSoDigito)).ToList();

            bool retorno = true;

            if (lstCepDto.Count == 0)
            {
                lstErros.Add("Cep não existe!");
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
                CepDto cepCliente = new CepDto()
                {
                    Cep = dadosCliente.Cep,
                    Endereco = dadosCliente.Endereco,
                    Bairro = dadosCliente.Bairro,
                    Cidade = dadosCliente.Cidade,
                    Uf = dadosCliente.Uf
                };
                if (!VerificarEndereco(cepCliente, lstCepDto, lstErros))
                {
                    retorno = false;
                }
            }

            return retorno;
        }

        private static async Task<bool> ValidarIE_Cliente(DadosClienteCadastroDto dadosCliente, List<string> lstErros,
            ContextoBdProvider contextoProvider)
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
                if (dadosCliente.ProdutorRural == (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                {
                    if (dadosCliente.Ie == "" &&
                    dadosCliente.Contribuinte_Icms_Status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    {
                        lstErros.Add("Para ser cadastrado como Produtor Rural e contribuinte do ICMS" +
                            "é necessário possuir nº de IE");
                        retorno = false;
                    }
                }
            }
            if (dadosCliente.Tipo == Constantes.ID_PJ)
            {
                if (dadosCliente.Ie == "" &&
                    dadosCliente.Contribuinte_Icms_Status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                {
                    lstErros.Add("Para ser cadastrado como contribuinte do ICMS , " +
                            "é necessário possuir nº de IE");
                    retorno = false;
                }
                if (string.IsNullOrEmpty(dadosCliente.Ie) &&
                    dadosCliente.Contribuinte_Icms_Status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                {
                    lstErros.Add("Se o cliente é não contribuinte do ICMS a inscrição estadual deve ser preenchida!");
                }
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

                if (lstErros.Count == 0)
                    VerificarInscricaoEstadualValida(dadosCliente.Ie, dadosCliente.Uf, lstErros);
            }

            List<NfeMunicipio> lstNfeMunicipio = new List<NfeMunicipio>();
            lstNfeMunicipio = (await ConsisteMunicipioIBGE(dadosCliente.Cidade, dadosCliente.Uf, lstErros,
                contextoProvider)).ToList();

            return retorno;
        }

        private static bool ValidarReferencias_Bancarias_Comerciais(List<RefBancariaDtoCliente> lstRefBancaria,
            List<RefComercialDtoCliente> lstRefComercial, List<string> lstErros)
        {
            bool retorno = true;

            if (lstRefBancaria != null && lstRefBancaria.Count > 0)
            {
                lstRefBancaria.ForEach(x =>
                {
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

            if (lstRefComercial != null && lstRefComercial.Count > 0)
            {
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

            return retorno;
        }

        public static async Task<IEnumerable<NfeMunicipio>> ConsisteMunicipioIBGE(string municipio, string uf,
            List<string> lstErros, ContextoBdProvider contextoProvider)
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
                lst_nfeMunicipios = (await Util.BuscarSiglaUf(uf, municipio, false, contextoProvider)).ToList();

                if (!lst_nfeMunicipios.Any())
                {
                    lstErros.Add("Município '" + municipio + "' não consta na relação de municípios do IBGE para a UF de '" + uf + "'!");
                }
            }

            return lst_nfeMunicipios;
        }

        public static void VerificarInscricaoEstadualValida(string ie, string uf, List<string> listaErros)
        {
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
                    listaErros.Add("Preencha a IE (Inscrição Estadual) com um número válido! " +
                            "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE.");
                    return;
                }

            }

            bool blnResultado;

            blnResultado = isInscricaoEstadualOkCom(ie, uf, listaErros);
            if (!blnResultado)
            {
                listaErros.Add("Preencha a IE (Inscrição Estadual) com um número válido! " +
                            "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE.");
            }
        }

        private static bool isInscricaoEstadualOkCom(string ie, string uf, List<string> listaErros)
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

        public static bool VerificarEndereco(CepDto cepCliente, List<CepDto> lstCepDto,
            List<string> listaErros)
        {
            bool retorno = true;

            string cepSoDigito = cepCliente.Cep.Replace(".", "").Replace("-", "");

            if (lstCepDto != null && lstCepDto.Count > 0)
            {
                foreach (var c in lstCepDto)
                {
                    //vamos verificar se tem endereco e bairro para verificar se foi alterado os dados de cep
                    if (!string.IsNullOrEmpty(c.Cep) && !string.IsNullOrEmpty(c.Endereco) &&
                        !string.IsNullOrEmpty(c.Bairro) && !string.IsNullOrEmpty(c.Cidade) &&
                        !string.IsNullOrEmpty(c.Uf))
                    {
                        if (c.Cep != cepSoDigito)
                        {
                            listaErros.Add("Número do Cep não confere!");
                            retorno = false;
                        }
                        if (Util.RemoverAcentuacao(c.Endereco).ToUpper() !=
                            Util.RemoverAcentuacao(cepCliente.Endereco).ToUpper())
                        {
                            listaErros.Add("Endereço não confere!");
                            retorno = false;
                        }
                        if (Util.RemoverAcentuacao(c.Bairro).ToUpper() !=
                            Util.RemoverAcentuacao(cepCliente.Bairro).ToUpper())
                        {
                            listaErros.Add("Bairro não confere!");
                            retorno = false;
                        }
                        if (Util.RemoverAcentuacao(c.Cidade).ToUpper() !=
                            Util.RemoverAcentuacao(cepCliente.Cidade).ToUpper())
                        {
                            listaErros.Add("Cidade não confere!");
                            retorno = false;
                        }
                        if (c.Uf.ToUpper() != cepCliente.Uf.ToUpper())
                        {
                            listaErros.Add("Estado não confere!");
                            retorno = false;
                        }
                    }
                    else
                    {
                        if (c.Cep != cepSoDigito)
                        {
                            listaErros.Add("Número do Cep diferente!");
                            retorno = false;
                        }
                        if (Util.RemoverAcentuacao(c.Cidade.ToUpper()) !=
                            Util.RemoverAcentuacao(cepCliente.Cidade.ToUpper()))
                        {
                            listaErros.Add("Cidade não confere!");
                            retorno = false;
                        }
                        if (c.Uf.ToUpper() != cepCliente.Uf.ToUpper())
                        {
                            listaErros.Add("Estado não confere!");
                            retorno = false;
                        }
                    }
                }
            }

            return retorno;
        }
    }
}
