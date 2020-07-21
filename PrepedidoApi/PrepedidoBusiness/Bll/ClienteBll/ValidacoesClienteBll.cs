﻿using System;
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

            public static string Municipio_nao_consta_na_relacao_IBGE(string municipio, string uf)
            {
                return "Município '" + municipio + "' não consta na relação de municípios do IBGE para a UF de '" + uf + "'!";
            }
        }

        public static async Task<bool> ValidarDadosCliente(DadosClienteCadastroDto dadosCliente,
            List<RefBancariaDtoCliente> lstRefBancaria, List<RefComercialDtoCliente> lstRefComercial,
            List<string> lstErros, ContextoBdProvider contextoProvider, CepBll cepBll, IBancoNFeMunicipio bancoNFeMunicipio,
            List<ListaBancoDto> lstBanco)
        {
            bool retorno;

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
                                return false;
                            }
                        }

                        if (lstRefComercial != null)
                        {
                            if (lstRefComercial.Count != 0)
                            {
                                lstErros.Add("Se cliente tipo PF, não deve constar referência comercial!");
                                return false;
                            }
                        }

                        tipoDesconhecido = false;
                        //vamos verificar e validar os dados referente ao cliente PF
                        retorno = await ValidarDadosCliente_PF(dadosCliente, cliente, lstErros, contextoProvider);
                    }
                    if (dadosCliente.Tipo == Constantes.ID_PJ)
                    {
                        tipoDesconhecido = false;
                        retorno = await ValidarDadosCliente_PJ(dadosCliente, cliente, lstErros, contextoProvider);
                        //vamos validar as referências
                        retorno = ValidarReferencias_Bancarias_Comerciais(lstRefBancaria, lstRefComercial,
                            lstErros, dadosCliente.Tipo, lstBanco);
                    }

                    if (tipoDesconhecido)
                        lstErros.Add(MensagensErro.Tipo_de_cliente_nao_e_PF_nem_PJ);

                    //validar endereço do cadastro                    
                    retorno = await ValidarEnderecoCadastroCliente(dadosCliente, lstErros, cepBll, contextoProvider,
                        bancoNFeMunicipio);

                    //vamos verificar o IE dos clientes
                    retorno = await ValidarIE_Cliente(dadosCliente, lstErros, contextoProvider, bancoNFeMunicipio);
                }
                else
                {
                    lstErros.Add(MensagensErro.INFORME_SE_O_CLIENTE_E_PF_OU_PJ);
                    retorno = false;
                }
            }
            else
            {
                lstErros.Add("DADOS DO CLIENTE ESTA VAZIO!");
                retorno = false;
            }

            return retorno;
        }

        private static async Task<bool> ValidarDadosCliente_PF(DadosClienteCadastroDto dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider)
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
                //vamos validar o cpf
                string cpf_cnpjSoDig = Util.SoDigitosCpf_Cnpj(dadosCliente.Cnpj_Cpf);
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
                        lstErros.Add(MensagensErro.GENERO_DO_CLIENTE_NAO_INFORMADO);
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

                        retorno = await ValidarTelefones_PF(dadosCliente, cliente, lstErros, contextoProvider);

                        if (!string.IsNullOrEmpty(dadosCliente.Email))
                        {
                            retorno = Util.ValidarEmail(dadosCliente.Email, lstErros);
                        }
                    }
                }
            }
            return retorno;
        }

        private static async Task<bool> ValidarTelefones_PF(DadosClienteCadastroDto dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider)
        {
            bool retorno = true;

            if (dadosCliente.Tipo == Constantes.ID_PF)
            {
                if (!string.IsNullOrEmpty(dadosCliente.TelComercial2) || !string.IsNullOrEmpty(dadosCliente.DddComercial2))
                {
                    lstErros.Add("Se cliente é tipo PF, não pode ter os campos de Telefone e DDD comercial 2 preenchidos!");
                    retorno = false;
                }
            }

            if (dadosCliente.Tipo == Constantes.ID_PF && string.IsNullOrEmpty(dadosCliente.TelefoneResidencial) &&
                string.IsNullOrEmpty(dadosCliente.TelComercial) && string.IsNullOrEmpty(dadosCliente.Celular))
            {
                lstErros.Add("PREENCHA PELO MENOS UM TELEFONE (RESIDENCIAL, COMERCIAL OU CELULAR).");
                retorno = false;
            }


            //CELULAR
            //passar tcliente
            retorno = await ValidarCelular(dadosCliente, cliente, lstErros, contextoProvider);
            //RESIDENCIAL
            retorno = await ValidarTelResidencial(dadosCliente, cliente, lstErros, contextoProvider);
            //COMERCIAL
            retorno = await ValidarTelCom(dadosCliente, cliente, lstErros, contextoProvider);

            return retorno;
        }

        private static async Task<bool> ValidarTelResidencial(DadosClienteCadastroDto dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider)
        {
            bool retorno = true;

            if (!string.IsNullOrEmpty(dadosCliente.DddResidencial) &&
                dadosCliente.DddResidencial.Length != 2)
            {
                lstErros.Add("DDD RESIDENCIAL INVÁLIDO.");
                retorno = false;
            }
            if (!string.IsNullOrEmpty(dadosCliente.TelefoneResidencial) &&
                dadosCliente.TelefoneResidencial.Length < 6)
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
            if (!string.IsNullOrEmpty(dadosCliente.TelefoneResidencial) &&
                string.IsNullOrEmpty(dadosCliente.DddResidencial))
            {
                lstErros.Add("PREENCHA O DDD RESIDENCIAL.");
                retorno = false;
            }

            if (lstErros.Count == 0)
            {
                if (!string.IsNullOrEmpty(dadosCliente.TelefoneResidencial) &&
                    !string.IsNullOrEmpty(dadosCliente.DddResidencial))
                {
                    if (!await ConfrontarTelefones(dadosCliente.DddResidencial, dadosCliente.TelefoneResidencial,
                    cliente?.Ddd_Res, cliente?.Tel_Res, dadosCliente.Cnpj_Cpf, dadosCliente.Tipo, lstErros, contextoProvider))
                        lstErros.Add("TELEFONE RESIDENCIAL (" + dadosCliente.DddResidencial + ") " + Util.FormatarTelefones(dadosCliente.TelefoneResidencial) +
                            " JÁ ESTÁ SENDO UTILIZADO NO CADASTRO DE OUTROS CLIENTES. Não foi possível concluir o cadastro.");
                }
            }

            return retorno;
        }

        private static async Task<bool> ValidarCelular(DadosClienteCadastroDto dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider)
        {
            bool retorno = true;

            if (!string.IsNullOrEmpty(dadosCliente.DddCelular) &&
                dadosCliente.DddCelular.Length != 2)
            {
                lstErros.Add("DDD CELULAR INVÁLIDO.");
                retorno = false;
            }
            if (!string.IsNullOrEmpty(dadosCliente.Celular) &&
                dadosCliente.Celular.Length < 6)
            {
                lstErros.Add("TELEFONE CELULAR INVÁLIDO.");
                retorno = false;
            }
            if (string.IsNullOrEmpty(dadosCliente.DddCelular) &&
               !string.IsNullOrEmpty(dadosCliente.Celular))
            {
                lstErros.Add("PREENCHA O DDD CELULAR.");
                retorno = false;
            }
            if (!string.IsNullOrEmpty(dadosCliente.DddCelular) &&
                string.IsNullOrEmpty(dadosCliente.Celular))
            {
                lstErros.Add("PREENCHA O TELEFONE CELULAR.");
                retorno = false;
            }


            if (lstErros.Count == 0)
            {
                if (!string.IsNullOrEmpty(dadosCliente.DddCelular) && !string.IsNullOrEmpty(dadosCliente.Celular))
                {
                    if (!await ConfrontarTelefones(dadosCliente.DddCelular, dadosCliente.Celular,
                    cliente?.Ddd_Cel, cliente?.Tel_Cel, dadosCliente.Cnpj_Cpf, dadosCliente.Tipo, lstErros, contextoProvider))
                        lstErros.Add("TELEFONE CELULAR (" + dadosCliente.DddCelular + ") " + Util.FormatarTelefones(dadosCliente.Celular) +
                            " JÁ ESTÁ SENDO UTILIZADO NO CADASTRO DE OUTROS CLIENTES. Não foi possível concluir o cadastro.");
                }
            }

            return retorno;
        }

        private static async Task<bool> ValidarTelCom(DadosClienteCadastroDto dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider)
        {
            bool retorno = true;

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

            if (!string.IsNullOrEmpty(dadosCliente.Ramal) && string.IsNullOrEmpty(dadosCliente.TelComercial))
            {
                lstErros.Add("Ramal comercial preenchido sem telefone!");
                retorno = false;
            }

            if (lstErros.Count == 0)
            {
                if (!string.IsNullOrEmpty(dadosCliente.DddComercial) &&
                    !string.IsNullOrEmpty(dadosCliente.TelComercial))
                {
                    if (!await ConfrontarTelefones(dadosCliente.DddComercial, dadosCliente.TelComercial,
                    cliente?.Ddd_Com, cliente?.Tel_Com, dadosCliente.Cnpj_Cpf, dadosCliente.Tipo, lstErros, contextoProvider))
                        lstErros.Add("TELEFONE COMERCIAL (" + dadosCliente.DddComercial + ") " + Util.FormatarTelefones(dadosCliente.TelComercial) +
                            " JÁ ESTÁ SENDO UTILIZADO NO CADASTRO DE OUTROS CLIENTES. Não foi possível concluir o cadastro.");
                }
            }

            return retorno;
        }

        private static async Task<bool> ValidarTelCom2(DadosClienteCadastroDto dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider)
        {
            bool retorno = true;

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

            if (!string.IsNullOrEmpty(dadosCliente.Ramal2) && string.IsNullOrEmpty(dadosCliente.TelComercial2))
            {
                lstErros.Add("Ramal comercial 2 preenchido sem telefone!");
                retorno = false;
            }

            if (lstErros.Count == 0)
            {
                if (!string.IsNullOrEmpty(dadosCliente.DddComercial2) &&
                    !string.IsNullOrEmpty(dadosCliente.TelComercial2))
                {
                    if (!await ConfrontarTelefones(dadosCliente.DddComercial2, dadosCliente.TelComercial2,
                    cliente?.Ddd_Com_2, cliente?.Tel_Com_2, dadosCliente.Cnpj_Cpf, dadosCliente.Tipo, lstErros, contextoProvider))
                        lstErros.Add("TELEFONE COMERCIAL (" + dadosCliente.DddComercial2 + ") " + Util.FormatarTelefones(dadosCliente.TelComercial2) +
                            " JÁ ESTÁ SENDO UTILIZADO NO CADASTRO DE OUTROS CLIENTES. Não foi possível concluir o cadastro.");
                }
            }

            return retorno;
        }

        private static async Task<bool> ConfrontarTelefones(string dddPrepedido, string telPrepedido, string dddCadastrado,
            string telCadastrado, string cpf_cnpj, string tipoCpf_Cnpj, List<string> lstErros,
            ContextoBdProvider contextoProvider)
        {
            if (dddPrepedido != dddCadastrado || telPrepedido != telCadastrado)
            {
                telPrepedido = Util.Telefone_SoDigito(telPrepedido);
                //se for cliente novo vamos passar o valor vazio para fazer a comparação dos dados
                telCadastrado = !string.IsNullOrWhiteSpace(telCadastrado) ? Util.Telefone_SoDigito(telCadastrado) : "";
                dddCadastrado = !string.IsNullOrWhiteSpace(dddCadastrado) ? dddCadastrado : "";

                if (dddPrepedido != dddCadastrado || telPrepedido != telCadastrado)
                {
                    //vamos verificar
                    if (dddPrepedido + telPrepedido == Constantes.TEL_BONSHOP_1 ||
                        dddPrepedido + telPrepedido == Constantes.TEL_BONSHOP_2 ||
                        dddPrepedido + telPrepedido == Constantes.TEL_BONSHOP_3)
                    {
                        lstErros.Add("NÃO É PERMITIDO UTILIZAR TELEFONES DA BONSHOP NO CADASTRO DE CLIENTES.");
                        return false;
                    }

                    //vamos confrontar com os cadastros NUM_MAXIMO_TELEFONES_REPETIDOS_CAD_CLIENTES
                    //entrada => ddd/tel/cpf_cnpj
                    int? qtdeRepetidos = await Util.VerificarTelefoneRepetidos
                        (dddPrepedido, telPrepedido, cpf_cnpj, tipoCpf_Cnpj, contextoProvider, lstErros);

                    if (qtdeRepetidos != null)
                    {
                        if (qtdeRepetidos > Constantes.NUM_MAXIMO_TELEFONES_REPETIDOS_CAD_CLIENTES) return false;
                    }
                }
            }

            return true;
        }

        private static async Task<bool> ValidarDadosCliente_PJ(DadosClienteCadastroDto dadosCliente, Tcliente cliente,
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
                    retorno = await ValidarTelefones_PJ(dadosCliente, cliente, lstErros, contextoProvider);
                }
            }

            return retorno;
        }

        private static async Task<bool> ValidarTelefones_PJ(DadosClienteCadastroDto dadosCliente, Tcliente cliente, List<string> lstErros,
            ContextoBdProvider contextoProvider)
        {
            bool retorno = true;

            if (!string.IsNullOrEmpty(dadosCliente.DddResidencial) || !string.IsNullOrEmpty(dadosCliente.TelefoneResidencial))
            {
                lstErros.Add("Se cliente é tipo PJ, não pode ter os campos de Telefone e DDD residencial preenchidos! ");
                retorno = false;
            }

            if (!string.IsNullOrEmpty(dadosCliente.DddCelular) || !string.IsNullOrEmpty(dadosCliente.Celular))
            {
                lstErros.Add("Se cliente é tipo PJ, não pode ter os campos de Telefone e DDD celular preenchidos! ");
                retorno = false;
            }

            if (dadosCliente.Tipo == Constantes.ID_PJ && string.IsNullOrEmpty(dadosCliente.TelComercial) &&
                string.IsNullOrEmpty(dadosCliente.TelComercial2))
            {
                lstErros.Add("PREENCHA AO MENOS UM TELEFONE (COMERCIAL OU COMERCIAL 2)!");
                retorno = false;
            }

            //com
            retorno = await ValidarTelCom(dadosCliente, cliente, lstErros, contextoProvider);

            //com 2
            retorno = await ValidarTelCom2(dadosCliente, cliente, lstErros, contextoProvider);

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
                lstErros.Add("PREENCHA O TELEFONE COMERCIAL 2.");
                retorno = false;
            }

            return retorno;
        }

        private static async Task<bool> ValidarEnderecoCadastroCliente(DadosClienteCadastroDto dadosCliente,
            List<string> lstErros, CepBll cepBll, ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio)
        {
            string cepSoDigito = dadosCliente.Cep.Replace(".", "").Replace("-", "");
            List<CepDto> lstCepDto = (await cepBll.BuscarPorCep(cepSoDigito)).ToList();

            bool retorno = true;

            if (lstCepDto.Count == 0)
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
                CepDto cepCliente = new CepDto()
                {
                    Cep = dadosCliente.Cep,
                    Endereco = dadosCliente.Endereco,
                    Bairro = dadosCliente.Bairro,
                    Cidade = dadosCliente.Cidade,
                    Uf = dadosCliente.Uf
                };
                if (!await VerificarEndereco(cepCliente, lstCepDto, lstErros, contextoProvider, bancoNFeMunicipio))
                {
                    retorno = false;
                }
            }

            return retorno;
        }

        private static async Task<bool> ValidarIE_Cliente(DadosClienteCadastroDto dadosCliente, List<string> lstErros,
            ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio)
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

            await ConsisteMunicipioIBGE(dadosCliente.Cidade, dadosCliente.Uf, lstErros, contextoProvider,
                bancoNFeMunicipio, true);

            return retorno;
        }

        private static bool ValidarReferencias_Bancarias_Comerciais(List<RefBancariaDtoCliente> lstRefBancaria,
            List<RefComercialDtoCliente> lstRefComercial, List<string> lstErros, string tipoPessoa, List<ListaBancoDto> lstBanco)
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

        public static async Task<bool> ConsisteMunicipioIBGE(string municipio, string uf,
            List<string> lstErros, ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio,
            bool mostrarMensagemErro)
        {
            var db = contextoProvider.GetContextoLeitura();

            if (string.IsNullOrEmpty(municipio))
            {
                if (mostrarMensagemErro)
                    lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                        "nenhum município foi informado!");
                return false;
            }

            if (string.IsNullOrEmpty(uf))
            {
                if (mostrarMensagemErro)
                    lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                        "a UF não foi informada!");
                return false;
            }

            else
            {
                if (uf.Length > 2)
                {
                    if (mostrarMensagemErro)
                        lstErros.Add("Não é possível consistir o município através da relação de municípios do IBGE: " +
                        "a UF é inválida (" + uf + ")!");
                    return false;
                }

            }

            if (lstErros.Count == 0)
            {
                List<NfeMunicipio> lst_nfeMunicipios = (await bancoNFeMunicipio.BuscarSiglaUf(uf, municipio, false, contextoProvider)).ToList();

                if (!lst_nfeMunicipios.Any())
                {
                    if (mostrarMensagemErro)
                        lstErros.Add(MensagensErro.Municipio_nao_consta_na_relacao_IBGE(municipio, uf));

                    return false;
                }
            }

            return true;
        }

        public static void VerificarInscricaoEstadualValida(string ie, string uf, List<string> listaErros)
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

            blnResultado = isInscricaoEstadualOkCom(ie, uf);
            if (!blnResultado)
            {
                listaErros.Add(MensagensErro.Preencha_a_IE_Inscricao_Estadual);
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

        public static async Task<bool> VerificarEndereco(CepDto cepCliente, List<CepDto> lstCepDto,
            List<string> lstErros, ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio)
        {
            bool retorno = true;

            string cepSoDigito = cepCliente.Cep.Replace(".", "").Replace("-", "");

            if (lstCepDto != null && lstCepDto.Count > 0)
            {
                foreach (var c in lstCepDto)
                {
                    //não verificamos a cidade porque ela deve estar no cadastro da NFE (IBGE) e não necessariamente igual à do CEP

                    //vamos verificar se tem endereco e bairro para verificar se foi alterado os dados de cep
                    if (!string.IsNullOrEmpty(c.Cep) && c.Cep != cepSoDigito)
                    {
                        lstErros.Add("Número do Cep não confere!");
                        retorno = false;
                    }

                    if (!string.IsNullOrEmpty(c.Endereco) &&
                        Util.RemoverAcentuacao(c.Endereco).ToUpper() !=
                        Util.RemoverAcentuacao(cepCliente.Endereco).ToUpper())
                    {
                        lstErros.Add("Endereço não confere!");
                        retorno = false;
                    }

                    if (!string.IsNullOrEmpty(c.Bairro) && Util.RemoverAcentuacao(c.Bairro).ToUpper() !=
                        Util.RemoverAcentuacao(cepCliente.Bairro).ToUpper())
                    {
                        lstErros.Add("Bairro não confere!");
                        retorno = false;
                    }

                    //vamos verificar se a cidade da lista de cep existe no IBGE para validar
                    if (!string.IsNullOrEmpty(cepCliente.Cidade) && !string.IsNullOrEmpty(c.Cidade))
                    {
                        if (await ConsisteMunicipioIBGE(c.Cidade, c.Uf, lstErros, contextoProvider, bancoNFeMunicipio, false))
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