using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using InfraBanco;
using Microsoft.EntityFrameworkCore;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using InfraBanco.Constantes;
using PrepedidoBusiness.Utils;

namespace PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll
{
    public class ValidacoesClienteUnisBll
    {
        public static async Task<bool> ValidarOrcamentista(string apelido, string loja, ContextoBdProvider contexto)
        {
            bool retorno = false;

            var db = contexto.GetContextoLeitura();

            string orcamentista = await (from c in db.TorcamentistaEindicadors
                                         where c.Apelido == apelido &&
                                               c.Loja == loja
                                         select c.Apelido).FirstOrDefaultAsync();

            if (orcamentista != null)
                retorno = true;

            return retorno;
        }

        public static bool ValidarDadosCliente(ClienteCadastroUnisDto cliente, List<string> lstErros)
        {
            bool retorno = false;

            if (cliente != null)
            {
                //existe dados
                if (!string.IsNullOrEmpty(cliente.DadosCliente.Tipo))
                {
                    //é cliente PF
                    if (cliente.DadosCliente.Tipo == Constantes.ID_PF)
                    {
                        //vamos verificar e validar os dados referente ao cliente PF
                        retorno = ValidarDadosCliente_PF(cliente.DadosCliente, lstErros);
                    }
                    if (cliente.DadosCliente.Tipo == Constantes.ID_PJ)
                    {
                        retorno = ValidarDadosCliente_PJ(cliente.DadosCliente, lstErros);
                        //vamos validar as referências
                        retorno = ValidarReferencias_Bancarias_Comerciais(cliente.RefBancaria, cliente.RefComercial,
                            lstErros);
                    }

                    //validar endereço do cadastro                    
                    retorno = ValidarEnderecoCadastroClienteUnis(cliente.DadosCliente, lstErros);

                    //vamos verificar o IE dos clientes
                    retorno = ValidarIE_ClienteUnis(cliente.DadosCliente, lstErros);
                }
            }
            else
            {
                lstErros.Add("DADOS DO CLIENTE ESTA VAZIO!");
            }

            return retorno;
        }

        private static bool ValidarDadosCliente_PF(DadosClienteCadastroUnisDto dadosCliente, List<string> lstErros)
        {
            /*
            -Para CPF:
            *-Verificar se tem nome do cliente; OK
            *-Validar CPF;
            *-Verificar se Sexo tem 1 caracteres e validar o tipo Sexo => M ou F;
            *-Verificar Telefones:
            *-Verificar se tem dados em todos os campos de telefone(Residencial, Comercial), 
                *pois é obrigatório um número de telefone;
            *-Verificar se DDD tem 2 caracteres dos telefones enviados;
            *-Verificar se a quantidade de caracteres dos telefones estão dentro do permitido;
            * */

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

        private static bool ValidarTelefones_PF(DadosClienteCadastroUnisDto dadosCliente, List<string> lstErros)
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

        private static bool ValidarDadosCliente_PJ(DadosClienteCadastroUnisDto dadosCliente, List<string> lstErros)
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

        private static bool ValidarTelefones_PJ(DadosClienteCadastroUnisDto dadosCliente, List<string> lstErros)
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
                        if (dadosCliente.DddComercial.Length != 2)
                        {
                            lstErros.Add("DDD DO TELEFONE COMERCIAL2 INVÁLIDO.");
                            retorno = false;
                        }
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

        private static bool ValidarEnderecoCadastroClienteUnis(DadosClienteCadastroUnisDto dadosCliente,
            List<string> lstErros)
        {
            bool retorno = true;

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
            }

            return retorno;
        }

        private static bool ValidarIE_ClienteUnis(DadosClienteCadastroUnisDto dadosCliente, List<string> lstErros)
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
                        lstErros.Add("Para ser cadastrado como Produtor Rural e contribuinte do ICMS" +
                            "é necessário possuir nº de IE");
                    retorno = false;
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
            }

            return retorno;
        }

        private static bool ValidarReferencias_Bancarias_Comerciais(List<RefBancariaClienteUnisDto> lstRefBancaria,
            List<RefComercialClienteUnisDto> lstRefComercial, List<string> lstErros)
        {
            bool retorno = true;

            if (lstRefBancaria.Count > 0)
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

            if (lstRefComercial.Count > 0)
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
    }
}
