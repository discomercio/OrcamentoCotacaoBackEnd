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
    public class ValidacoesClienteTelefones
    {
        internal static async Task ValidarTelefones_PJ(Cliente.Dados.DadosClienteCadastroDados dadosCliente, Tcliente cliente, List<string> lstErros,
            ContextoBdProvider contextoProvider, InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
        {
            TelefonesSomenteComDigitos(dadosCliente);

            if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
            {
                if (!string.IsNullOrEmpty(dadosCliente.DddResidencial) ||
                !string.IsNullOrEmpty(dadosCliente.TelefoneResidencial))
                {
                    lstErros.Add("Se cliente é tipo PJ, não pode ter os campos de Telefone e DDD residencial preenchidos! ");
                }

                if (!string.IsNullOrEmpty(dadosCliente.DddCelular) ||
                    !string.IsNullOrEmpty(dadosCliente.Celular))
                {
                    lstErros.Add("Se cliente é tipo PJ, não pode ter os campos de Telefone e DDD celular preenchidos! ");
                }

                if (dadosCliente.Tipo == Constantes.ID_PJ && string.IsNullOrEmpty(dadosCliente.TelComercial) &&
                    string.IsNullOrEmpty(dadosCliente.TelComercial2))
                {
                    lstErros.Add("PREENCHA AO MENOS UM TELEFONE (COMERCIAL OU COMERCIAL 2)!");
                }
            }

            //com
            if (!string.IsNullOrEmpty(dadosCliente.TelComercial) ||
                !string.IsNullOrEmpty(dadosCliente.DddComercial) ||
                !string.IsNullOrEmpty(dadosCliente.Ramal))
            {
                await ValidarTelCom(dadosCliente, cliente, lstErros, contextoProvider, sistemaResponsavel);
            }

            //com 2
            if (!string.IsNullOrEmpty(dadosCliente.TelComercial2) ||
                !string.IsNullOrEmpty(dadosCliente.DddComercial2) ||
                !string.IsNullOrEmpty(dadosCliente.Ramal2))
            {
                await ValidarTelCom2(dadosCliente, cliente, lstErros, contextoProvider, sistemaResponsavel);
            }

        }

        internal static async Task ValidarTelefones_PF(Cliente.Dados.DadosClienteCadastroDados dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider, InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
        {
            TelefonesSomenteComDigitos(dadosCliente);


            if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
            {
                if (dadosCliente.Tipo == Constantes.ID_PF)
                {
                    if (!string.IsNullOrEmpty(dadosCliente.TelComercial2) ||
                        !string.IsNullOrEmpty(dadosCliente.DddComercial2) ||
                        !string.IsNullOrEmpty(dadosCliente.Ramal2))
                    {
                        lstErros.Add("Se cliente é tipo PF, não pode ter os campos de Telefone, DDD e ramal comercial 2 preenchidos!");
                    }
                }

                if (dadosCliente.Tipo == Constantes.ID_PF && string.IsNullOrEmpty(dadosCliente.TelefoneResidencial) &&
                    string.IsNullOrEmpty(dadosCliente.TelComercial) && string.IsNullOrEmpty(dadosCliente.Celular))
                {
                    lstErros.Add("PREENCHA PELO MENOS UM TELEFONE (RESIDENCIAL, COMERCIAL OU CELULAR).");
                }
            }

            //CELULAR
            if (!string.IsNullOrEmpty(dadosCliente.Celular) || !string.IsNullOrEmpty(dadosCliente.DddCelular))
            {
                await ValidarCelular(dadosCliente, cliente, lstErros, contextoProvider, sistemaResponsavel);
            }
            //RESIDENCIAL
            if (!string.IsNullOrEmpty(dadosCliente.TelefoneResidencial) || !string.IsNullOrEmpty(dadosCliente.DddResidencial))
            {
                await ValidarTelResidencial(dadosCliente, cliente, lstErros, contextoProvider, sistemaResponsavel);
            }
            //COMERCIA
            if (!string.IsNullOrEmpty(dadosCliente.TelComercial) ||
                !string.IsNullOrEmpty(dadosCliente.DddComercial) ||
                !string.IsNullOrEmpty(dadosCliente.Ramal))
            {
                await ValidarTelCom(dadosCliente, cliente, lstErros, contextoProvider, sistemaResponsavel);
            }

            if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
            {
                if (!string.IsNullOrEmpty(dadosCliente.TelComercial2) ||
                !string.IsNullOrEmpty(dadosCliente.DddComercial2) ||
                !string.IsNullOrEmpty(dadosCliente.Ramal2))
                {
                    lstErros.Add("Se cliente é tipo PF, não deve ter DDD comercial 2, " +
                        "telefone comercial 2 e ramal comercial 2 preenchidos.");
                }
            }

            //no magento, aceitamos com2, então temos que validar
            if (sistemaResponsavel == Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
            {
                if (!string.IsNullOrEmpty(dadosCliente.TelComercial2) ||
                !string.IsNullOrEmpty(dadosCliente.DddComercial2) ||
                !string.IsNullOrEmpty(dadosCliente.Ramal2))
                {
                    await ValidarTelCom2(dadosCliente, cliente, lstErros, contextoProvider, sistemaResponsavel);
                }
            }

        }

        //deixa somente dígitos nos telefones
        //devemos fazer isso antes das validações porque, por exemplo, o tamanho mínimo e máximo deve ser validado contra a versão sem dígitos
        private static void TelefonesSomenteComDigitos(Cliente.Dados.DadosClienteCadastroDados dadosCliente)
        {
            if (!string.IsNullOrEmpty(dadosCliente.DddResidencial))
                dadosCliente.DddResidencial = Util.Telefone_SoDigito(dadosCliente.DddResidencial);

            if (!string.IsNullOrEmpty(dadosCliente.TelefoneResidencial))
                dadosCliente.TelefoneResidencial = Util.Telefone_SoDigito(dadosCliente.TelefoneResidencial);

            if (!string.IsNullOrEmpty(dadosCliente.DddComercial))
                dadosCliente.DddComercial = Util.Telefone_SoDigito(dadosCliente.DddComercial);

            if (!string.IsNullOrEmpty(dadosCliente.TelComercial))
                dadosCliente.TelComercial = Util.Telefone_SoDigito(dadosCliente.TelComercial);

            if (!string.IsNullOrEmpty(dadosCliente.Ramal))
                dadosCliente.Ramal = Util.Telefone_SoDigito(dadosCliente.Ramal);

            if (!string.IsNullOrEmpty(dadosCliente.DddCelular))
                dadosCliente.DddCelular = Util.Telefone_SoDigito(dadosCliente.DddCelular);

            if (!string.IsNullOrEmpty(dadosCliente.Celular))
                dadosCliente.Celular = Util.Telefone_SoDigito(dadosCliente.Celular);

            if (!string.IsNullOrEmpty(dadosCliente.TelComercial2))
                dadosCliente.TelComercial2 = Util.Telefone_SoDigito(dadosCliente.TelComercial2);

            if (!string.IsNullOrEmpty(dadosCliente.DddComercial2))
                dadosCliente.DddComercial2 = Util.Telefone_SoDigito(dadosCliente.DddComercial2);

            if (!string.IsNullOrEmpty(dadosCliente.Ramal2))
                dadosCliente.Ramal2 = Util.Telefone_SoDigito(dadosCliente.Ramal2);
        }

        private static async Task ValidarTelResidencial(Cliente.Dados.DadosClienteCadastroDados dadosCliente, Tcliente cliente,
                List<string> lstErros, ContextoBdProvider contextoProvider, InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
        {

            if (!string.IsNullOrEmpty(dadosCliente.DddResidencial) &&
                dadosCliente.DddResidencial.Length != 2)
            {
                lstErros.Add("DDD RESIDENCIAL INVÁLIDO.");
            }
            if (!string.IsNullOrEmpty(dadosCliente.TelefoneResidencial) &&
                dadosCliente.TelefoneResidencial.Length < 6)
            {
                lstErros.Add("TELEFONE RESIDENCIAL INVÁLIDO.");
            }
            if (!string.IsNullOrEmpty(dadosCliente.TelefoneResidencial) &&
                dadosCliente.TelefoneResidencial.Length > 11)
            {
                lstErros.Add("TELEFONE RESIDENCIAL INVÁLIDO.");
            }
            if (!string.IsNullOrEmpty(dadosCliente.DddResidencial) &&
                string.IsNullOrEmpty(dadosCliente.TelefoneResidencial))
            {
                lstErros.Add("PREENCHA O TELEFONE RESIDENCIAL.");
            }
            if (!string.IsNullOrEmpty(dadosCliente.TelefoneResidencial) &&
                string.IsNullOrEmpty(dadosCliente.DddResidencial))
            {
                lstErros.Add("PREENCHA O DDD RESIDENCIAL.");
            }

            if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
            {
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
            }

        }

        private static async Task ValidarCelular(Cliente.Dados.DadosClienteCadastroDados dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider, InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
        {

            if (!string.IsNullOrEmpty(dadosCliente.DddCelular) &&
                dadosCliente.DddCelular.Length != 2)
            {
                lstErros.Add("DDD CELULAR INVÁLIDO.");
            }
            if (!string.IsNullOrEmpty(dadosCliente.Celular) &&
                dadosCliente.Celular.Length < 6)
            {
                lstErros.Add("TELEFONE CELULAR INVÁLIDO.");
            }
            if (!string.IsNullOrEmpty(dadosCliente.Celular) &&
                dadosCliente.Celular.Length > 11)
            {
                lstErros.Add("TELEFONE CELULAR INVÁLIDO.");
            }
            if (string.IsNullOrEmpty(dadosCliente.DddCelular) &&
               !string.IsNullOrEmpty(dadosCliente.Celular))
            {
                lstErros.Add("PREENCHA O DDD CELULAR.");
            }
            if (!string.IsNullOrEmpty(dadosCliente.DddCelular) &&
                string.IsNullOrEmpty(dadosCliente.Celular))
            {
                lstErros.Add("PREENCHA O TELEFONE CELULAR.");
            }

            if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
            {
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
            }

        }

        private static async Task ValidarTelCom(Cliente.Dados.DadosClienteCadastroDados dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider, InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
        {

            if (!string.IsNullOrEmpty(dadosCliente.TelComercial) &&
                string.IsNullOrEmpty(dadosCliente.DddComercial))
            {
                lstErros.Add("PREENCHA O DDD COMERCIAL.");
            }
            if (!string.IsNullOrEmpty(dadosCliente.DddComercial) &&
                dadosCliente.DddComercial.Length != 2)
            {
                lstErros.Add("DDD DO TELEFONE COMERCIAL INVÁLIDO.");
            }

            if (!string.IsNullOrEmpty(dadosCliente.DddComercial) &&
                string.IsNullOrEmpty(dadosCliente.TelComercial))
            {
                lstErros.Add("PREENCHA O TELEFONE COMERCIAL.");
            }
            if (!string.IsNullOrEmpty(dadosCliente.TelComercial) &&
                dadosCliente.TelComercial.Length < 6)
            {
                lstErros.Add("TELEFONE COMERCIAL INVÁLIDO.");
            }
            if (!string.IsNullOrEmpty(dadosCliente.TelComercial) &&
                dadosCliente.TelComercial.Length > 11)
            {
                lstErros.Add("TELEFONE COMERCIAL INVÁLIDO.");
            }

            if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
            {
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
            }

            if (!string.IsNullOrEmpty(dadosCliente.Ramal) &&
                (string.IsNullOrEmpty(dadosCliente.TelComercial) ||
                 string.IsNullOrEmpty(dadosCliente.DddComercial)))
            {
                lstErros.Add("Ramal comercial preenchido sem telefone!");
            }
        }

        private static async Task ValidarTelCom2(Cliente.Dados.DadosClienteCadastroDados dadosCliente, Tcliente cliente,
            List<string> lstErros, ContextoBdProvider contextoProvider, InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
        {
            if (!string.IsNullOrEmpty(dadosCliente.TelComercial2))
            {
                if (Util.Telefone_SoDigito(dadosCliente.TelComercial2).Length > 11)
                {
                    lstErros.Add("TELEFONE COMERCIAL2 INVÁLIDO.");
                }
                if (Util.Telefone_SoDigito(dadosCliente.TelComercial2).Length < 6)
                {
                    lstErros.Add("TELEFONE COMERCIAL2 INVÁLIDO.");
                }
                if (!string.IsNullOrEmpty(dadosCliente.DddComercial2))
                {
                    if (dadosCliente.DddComercial2.Length != 2)
                    {
                        lstErros.Add("DDD DO TELEFONE COMERCIAL2 INVÁLIDO.");
                    }
                }
                else
                {
                    lstErros.Add("PREENCHA O DDD DO TELEFONE COMERCIAL2.");
                }
            }

            if (!string.IsNullOrEmpty(dadosCliente.DddComercial2) &&
                string.IsNullOrEmpty(dadosCliente.TelComercial2))
            {
                lstErros.Add("PREENCHA O TELEFONE COMERCIAL2.");
            }

            if (!string.IsNullOrEmpty(dadosCliente.DddComercial2) &&
                dadosCliente.DddComercial2.Length != 2)
            {
                lstErros.Add("DDD DO TELEFONE COMERCIAL2 INVÁLIDO.");
            }

            if (sistemaResponsavel != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO)
            {
                if (lstErros.Count == 0)
                {
                    if (!string.IsNullOrEmpty(dadosCliente.DddComercial2) &&
                        !string.IsNullOrEmpty(dadosCliente.TelComercial2))
                    {
                        if (!await ConfrontarTelefones(dadosCliente.DddComercial2, dadosCliente.TelComercial2,
                        cliente?.Ddd_Com_2, cliente?.Tel_Com_2, dadosCliente.Cnpj_Cpf, dadosCliente.Tipo, lstErros, contextoProvider))
                            lstErros.Add("TELEFONE COMERCIAL2 (" + dadosCliente.DddComercial2 + ") " + Util.FormatarTelefones(dadosCliente.TelComercial2) +
                                " JÁ ESTÁ SENDO UTILIZADO NO CADASTRO DE OUTROS CLIENTES. Não foi possível concluir o cadastro.");
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosCliente.Ramal2) &&
                (string.IsNullOrEmpty(dadosCliente.TelComercial2) ||
                 string.IsNullOrEmpty(dadosCliente.DddComercial2)))
            {
                lstErros.Add("Ramal comercial 2 preenchido sem telefone!");
            }
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

    }
}
