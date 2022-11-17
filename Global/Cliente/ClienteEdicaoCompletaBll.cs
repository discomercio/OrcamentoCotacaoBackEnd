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
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

/*
 * estas rotinas são para atender a loja e outros módulos que precisem de edição completa
 * */

#if RELEASE_BANCO_LOJA || DEBUG_BANCO_DEBUG

namespace Cliente
{
    public partial class ClienteBll
    {
        public async Task<List<string>> AtualizarClienteParcialLoja(string apelido, Cliente.Dados.ClienteCadastroDados clienteCadastroDados,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel, bool edicaoCompleta)
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

            await Cliente.ValidacoesClienteBll.ValidarDadosCliente(clienteCadastroDados.DadosCliente, true, clienteCadastroDados.RefBancaria,
                clienteCadastroDados.RefComercial, lstErros, contextoProvider, cepBll, bancoNFeMunicipio, lstBanco, true, sistemaResponsavel, false);
            if (lstErros.Count > 0)
                return lstErros;

            string id = await BuscarIdCliente(clienteCadastroDados.DadosCliente.Cnpj_Cpf, contextoProvider.GetContextoLeitura());

            if (id != null && id == clienteCadastroDados.DadosCliente.Id)
            {
                if (lstErros.Count == 0)
                {
                    using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_CLIENTE))
                    {
                        Tcliente cli = await (from c in dbgravacao.Tcliente
                                              where c.Id == clienteCadastroDados.DadosCliente.Id
                                              select c).FirstOrDefaultAsync();

                        log = await Verificar_AlterouClienteCadastroDados(cli, clienteCadastroDados, apelido, sistemaResponsavel, edicaoCompleta,
                            dbgravacao, lstErros, lstBanco);
                        if (!string.IsNullOrEmpty(log))
                        {
                            log += MontarLogAlteracao_SistemaResponsavel(cli);
                            //afazer = verificar se é somente na loja que add o id cliente
                            if (edicaoCompleta)
                                log = "id=" + cli.Id + "; " + log;

                            cli.Dt_Ult_Atualizacao = DateTime.Now;
                            cli.Usuario_Ult_Atualizacao = apelido;

                            dbgravacao.Update(cli);
                            await dbgravacao.SaveChangesAsync();

                            bool salvouLog = UtilsGlobais.Util.GravaLog(dbgravacao, apelido, clienteCadastroDados.DadosCliente.Loja, "",
                                clienteCadastroDados.DadosCliente.Id, Constantes.OP_LOG_CLIENTE_ALTERACAO, log);
                            if (salvouLog)
                                dbgravacao.transacao.Commit();
                        }
                    }
                }
            }

            return lstErros;
        }

        public async Task<string> Verificar_AlterouClienteCadastroDados(Tcliente cli, Cliente.Dados.ClienteCadastroDados clienteCadastroDados, string apelido,
                    Constantes.CodSistemaResponsavel sistemaResponsavel, bool edicaoCompleta, ContextoBdGravacao dbGravacao,
                    List<string> lstErros, List<ListaBancoDados> lstBanco)
        {
            string log_retorno = "";
            //Montamos o log para alteração de Produtor Rural, Contribuinte ICMS e IE
            string logProdutor = MontarLogAlteracao_ProdutorRural(cli, clienteCadastroDados.DadosCliente, apelido);
            //IE
            string logIE = MontarLogAlteracao_IE(cli, clienteCadastroDados.DadosCliente, apelido);

            string logContribuinte = MontarLogAlteracao_ContribuinteICMS(cli, clienteCadastroDados.DadosCliente, apelido);
            //Se estiver em prepedido API ou ApiUnis
            if (!edicaoCompleta)
            {
                //AFAZER: PRECISO TESTAR EM TODAS AS CONDIÇÕES
                log_retorno = logIE + logContribuinte + logProdutor;
            }
            //Só poderemos alterar outros dados caso não seja alteração de dados feito pela Loja
            //OBS => verificar com o Edu se o magento irá atualizar o cadastro do cliente também
            if (sistemaResponsavel == Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS &&
                edicaoCompleta)
            {
                //id = 000000684244;

                //ie: "" => 361.289.183.714;
                log_retorno += logIE;
                //rg: "" => 12345666; nome: Teste PJ => Teste PJ 2; sexo: "" => M
                log_retorno += MontarLogAlteracao_RG_Nome_Sexo(cli, clienteCadastroDados.DadosCliente);
                //endereco: Rua Professor Fábio Fanucchiqsxqsx => Rua Leblon
                //bairro: Jardim São Paulo(Zona Norte)qsdqsx => Leblon
                //cidade: São Paulo => Rio de Janeiro
                //uf: SP => RJ
                //cep: 02045080 => 22441020
                log_retorno += MontarLogAlteracao_EnderecoCadastro(cli, clienteCadastroDados.DadosCliente);
                //ddd_res: 11 => 12; tel_res: 22255226 => 656686418;
                log_retorno += MontarLogAlteracao_Tel_Residencial(cli, clienteCadastroDados.DadosCliente);
                //ddd_com: 11 => 22; tel_com: 22667778 => 99999775; ramal_com: 1 => 12;
                log_retorno += MontarLogAlteracao_Tel_Comercial(cli, clienteCadastroDados.DadosCliente);
                //PJ => contato: Teste 1 => Teste 2; email: teste1 @mail.com => teste2@mail.com; endereco_numero: 1 => 2;
                //endereco_complemento: teste => teste 2;
                //PF => dt_nasc: 19 / 06 / 1984 => ""; filiacao: teste => teste2; email: gabriel @mail.com => gabriel2@mail.com;
                //endereco_numero: 10 => 1; endereco_complemento: apenas teste => teste;
                log_retorno += MontarLogAlteracao_CamposSoltos(cli, clienteCadastroDados.DadosCliente);
                //contribuinte_icms_status: 1 => 2;
                //contribuinte_icms_data: 08 / 01 / 2021 11:43:07 => 08 / 01 / 2021 11:48:22;
                //contribuinte_icms_data_hora: 08 / 01 / 2021 11:43:07 => 08 / 01 / 2021 11:48:22;
                log_retorno += logContribuinte;
                //produtor_rural_status: 1 => 2
                //produtor_rural_data: 22 / 12 / 2020 16:07:20 => 06 / 01 / 2021 19:36:47
                //produtor_rural_data_hora: 22 / 12 / 2020 16:07:20 => 06 / 01 / 2021 19:36:47
                log_retorno += logProdutor;
                //email_xml: teste1 @mailxml.com => teste2@mailxml.com; 
                log_retorno += MontarLogAlteracao_EmailXML(cli, clienteCadastroDados.DadosCliente);
                //ddd_cel: "" => 11; tel_cel: "" => 981660033
                log_retorno += MontarLogAlteracao_Tel_Celular(cli, clienteCadastroDados.DadosCliente);
                //ddd_com_2: 11 => 22; tel_com_2: 33667778 => 787887877; ramal_com_2: 2 => 22;
                log_retorno += MontarLogAlteracao_Tel_Comercial2(cli, clienteCadastroDados.DadosCliente);
                //Ref Bancária incluída: banco = 003; agencia = 01112; conta = 123458; ddd = 22; telefone = 75315995; 
                //contato = Teste ref bancária 2; Ref Bancária excluída: banco = 001; agencia = 01111; conta = 123456; ddd = 11; 
                //telefone = 32654578; contato = Teste ref bancária 1;
                log_retorno += await MontarLogAlteracao_Ref_Bancaria(cli, clienteCadastroDados, dbGravacao, lstErros, apelido);
                //Ref Comercial incluída: nome_empresa = Empresa 12; contato = Teste ref Com 12; ddd = 22; telefone = 95174125;
                //Ref Comercial incluída: nome_empresa = Empresa 22; contato = Teste ref Com 22; ddd = 22; telefone = 95112365;
                //Ref Comercial incluída: nome_empresa = Empresa 32; contato = Teste ref Com 32; ddd = 22; telefone = 95115996;
                //Ref Comercial excluída: nome_empresa = Empresa 1; contato = Teste ref Com 1; ddd = 11; telefone = 65456545;
                //Ref Comercial excluída: nome_empresa = Empresa 2; contato = Teste ref Com 2; ddd = 11; telefone = 565456544;
                //Ref Comercial excluída: nome_empresa = Empresa 3; contato = Teste ref Com 3; ddd = 11; telefone = 987878987
                log_retorno += await MontarLogAleracao_Ref_Comercial(cli, clienteCadastroDados, dbGravacao, lstErros, apelido, sistemaResponsavel, lstBanco);

            }

            return log_retorno;
        }

        private string MontarLogAlteracao_RG_Nome_Sexo(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados)
        {
            string log = "";
            string campo_vazio = "\"\"";

            if (dados.Tipo == Constantes.ID_PF)
            {
                if (dados.Rg != cli.Rg)
                {
                    log += "rg: " + (!string.IsNullOrEmpty(cli.Rg) ? cli.Rg : campo_vazio) +
                        " => " + (!string.IsNullOrEmpty(dados.Rg) ? dados.Rg : campo_vazio) + "; ";
                    cli.Rg = dados.Rg;
                }
            }


            if (dados.Nome != cli.Nome)
            {
                log += "nome: " + (!string.IsNullOrEmpty(cli.Nome) ? cli.Nome : campo_vazio) +
                   " => " + (!string.IsNullOrEmpty(dados.Nome) ? dados.Nome : campo_vazio) + "; ";
                cli.Nome = dados.Nome;
            }

            if (dados.Tipo == Constantes.ID_PF)
            {
                if (dados.Sexo != cli.Sexo)
                {
                    log += "sexo: " + (!string.IsNullOrEmpty(cli.Sexo) ? cli.Sexo : campo_vazio) +
                        " => " + (!string.IsNullOrEmpty(dados.Sexo) ? dados.Sexo : campo_vazio) + "; ";
                    cli.Sexo = dados.Sexo;
                }
            }

            return log;
        }

        private string MontarLogAlteracao_Tel_Residencial(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados)
        {
            string log = "";
            string campo_vazio = "\"\"";

            if (dados.Tipo == Constantes.ID_PF)
            {
                if (dados.DddResidencial != cli.Ddd_Res)
                {
                    log += "ddd_res: " + (!string.IsNullOrEmpty(cli.Ddd_Res) ? cli.Ddd_Res : campo_vazio) +
                        " => " + (!string.IsNullOrEmpty(dados.DddResidencial) ? dados.DddResidencial : campo_vazio) + "; ";
                    cli.Ddd_Res = dados.DddResidencial;
                }

                if (dados.TelefoneResidencial != cli.Tel_Res)
                {
                    log += "tel_res: " + (!string.IsNullOrEmpty(cli.Tel_Res) ? cli.Tel_Res : campo_vazio) +
                        " => " + (!string.IsNullOrEmpty(dados.TelefoneResidencial) ? dados.TelefoneResidencial : campo_vazio) + "; ";
                    cli.Tel_Res = dados.TelefoneResidencial;
                }

            }

            return log;
        }

        private string MontarLogAlteracao_Tel_Celular(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados)
        {
            string log = "";
            string campo_vazio = "\"\"";

            if (dados.Tipo == Constantes.ID_PF)
            {
                if (dados.DddCelular != cli.Ddd_Cel)
                {
                    log += "ddd_cel: " + (!string.IsNullOrEmpty(cli.Ddd_Cel) ? cli.Ddd_Cel : campo_vazio) +
                        " => " + (!string.IsNullOrEmpty(dados.DddCelular) ? dados.DddCelular : campo_vazio) + "; ";
                    cli.Ddd_Cel = dados.DddCelular;
                }
                if (dados.Celular != cli.Tel_Cel)
                {
                    log += "tel_cel: " + (!string.IsNullOrEmpty(cli.Tel_Cel) ? cli.Tel_Cel : campo_vazio) +
                        " => " + (!string.IsNullOrEmpty(dados.Celular) ? dados.Celular : campo_vazio) + "; ";
                    cli.Tel_Cel = dados.Celular;
                }
            }

            return log;
        }

        private string MontarLogAlteracao_Tel_Comercial(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados)
        {
            string log = "";
            string campo_vazio = "\"\"";

            if (dados.DddComercial != cli.Ddd_Com)
            {
                log += "ddd_com: " + (!string.IsNullOrEmpty(cli.Ddd_Com) ? cli.Ddd_Com : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.DddComercial) ? dados.DddComercial : campo_vazio) + "; ";
                cli.Ddd_Com = dados.DddComercial;
            }
            if (dados.TelComercial != cli.Tel_Com)
            {
                log += "tel_com: " + (!string.IsNullOrEmpty(cli.Tel_Com) ? cli.Tel_Com : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.TelComercial) ? dados.TelComercial : campo_vazio) + "; ";
                cli.Tel_Com = dados.TelComercial;
            }
            if (dados.Ramal != cli.Ramal_Com)
            {
                log += "ramal_com: " + (!string.IsNullOrEmpty(cli.Ramal_Com) ? cli.Ramal_Com : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Ramal) ? dados.Ramal : campo_vazio) + "; ";
                cli.Ramal_Com = dados.Ramal;
            }

            return log;
        }

        private string MontarLogAlteracao_Tel_Comercial2(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados)
        {
            string log = "";
            string campo_vazio = "\"\"";

            if (dados.Tipo == Constantes.ID_PJ)
            {
                if (dados.DddComercial2 != cli.Ddd_Com_2)
                {
                    log += "ddd_com_2: " + (!string.IsNullOrEmpty(cli.Ddd_Com_2) ? cli.Ddd_Com_2 : campo_vazio) +
                        " => " + (!string.IsNullOrEmpty(dados.DddComercial2) ? dados.DddComercial2 : campo_vazio) + "; ";
                    cli.Ddd_Com_2 = dados.DddComercial2;
                }
                if (dados.TelComercial2 != cli.Tel_Com_2)
                {
                    log += "tel_com_2: " + (!string.IsNullOrEmpty(cli.Tel_Com_2) ? cli.Tel_Com_2 : campo_vazio) +
                        " => " + (!string.IsNullOrEmpty(dados.TelComercial2) ? dados.TelComercial2 : campo_vazio) + "; ";
                    cli.Tel_Com_2 = dados.TelComercial2;
                }
                if (dados.Ramal2 != cli.Ramal_Com_2)
                {
                    log += "ramal_com_2: " + (!string.IsNullOrEmpty(cli.Ramal_Com_2) ? cli.Ramal_Com_2 : campo_vazio) +
                        " => " + (!string.IsNullOrEmpty(dados.Ramal2) ? dados.Ramal2 : campo_vazio) + "; ";
                    cli.Ramal_Com_2 = dados.Ramal2;
                }
            }

            return log;
        }

        private string MontarLogAlteracao_CamposSoltos(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados)
        {
            string log = "";
            string campo_vazio = "\"\"";

            if (dados.Tipo == Constantes.ID_PJ)
                if (dados.Contato != cli.Contato)
                {
                    log += "contato: " + (!string.IsNullOrEmpty(cli.Contato) ? cli.Contato : campo_vazio) +
                        " => " + (!string.IsNullOrEmpty(dados.Contato) ? dados.Contato : campo_vazio) + "; ";
                    cli.Contato = dados.Contato;
                }

            if (dados.Tipo == Constantes.ID_PF)
            {
                //DateTime data_banco = cli.Dt_Nasc ?? new DateTime();
                //DateTime data_dados = dados.Nascimento ?? new DateTime();
                //if (data_dados != data_banco)
                //{
                //    log += "dt_nasc: " + (data_banco != new DateTime() ? data_banco.ToString("dd/MM/yyyy") : campo_vazio) +
                //        " => " + (data_dados != new DateTime() ? data_dados.ToString("dd/MM/yyyy") : campo_vazio) + "; ";
                //    cli.Dt_Nasc = dados.Nascimento;
                //}
                if (dados.Observacao_Filiacao != cli.Filiacao)
                    log += "filiacao: " + (!string.IsNullOrEmpty(cli.Filiacao) ? cli.Filiacao : campo_vazio) +
                        " => " + (!string.IsNullOrEmpty(dados.Observacao_Filiacao) ? dados.Observacao_Filiacao : campo_vazio) + "; ";
                cli.Filiacao = dados.Observacao_Filiacao;
            }

            if (dados.Email != cli.Email)
            {
                log += "email: " + (!string.IsNullOrEmpty(cli.Email) ? cli.Email : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Email) ? dados.Email : campo_vazio) + "; ";
                cli.Email = dados.Email;
            }

            if (dados.Numero != cli.Endereco_Numero)
            {
                log += "endereco_numero: " + (!string.IsNullOrEmpty(cli.Endereco_Numero) ? cli.Endereco_Numero : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Numero) ? dados.Numero : campo_vazio) + "; ";
                cli.Endereco_Numero = dados.Numero;
            }

            if (dados.Complemento != cli.Endereco_Complemento)
            {
                log += "endereco_complemento: " + (!string.IsNullOrEmpty(cli.Endereco_Complemento) ? cli.Endereco_Complemento : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Complemento) ? dados.Complemento : campo_vazio) + "; ";
                cli.Endereco_Complemento = dados.Complemento;
            }

            return log;
        }

        private string MontarLogAlteracao_EmailXML(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados)
        {
            string log = "";
            if (dados.EmailXml != cli.Email_Xml)
            {
                string campo_vazio = "\"\"";
                log += "email_xml: " + (!string.IsNullOrEmpty(cli.Email_Xml) ? cli.Email_Xml : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.EmailXml) ? dados.EmailXml : campo_vazio) + "; ";
                cli.Email_Xml = dados.EmailXml;
            }

            return log;
        }

        private string MontarLogAlteracaoPF_Cadastro(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados)
        {
            string aux = "";
            string campo_vazio = "\"\"";

            if (dados.Observacao_Filiacao != cli.Filiacao)
                aux += "filiacao: " + (!string.IsNullOrEmpty(cli.Filiacao) ? cli.Filiacao : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Observacao_Filiacao) ? dados.Observacao_Filiacao : campo_vazio) + ";";
            if (dados.Email != cli.Email)
                aux += "email: " + (!string.IsNullOrEmpty(cli.Email) ? cli.Email : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Email) ? dados.Email : campo_vazio) + ";";
            if (dados.EmailXml != cli.Email_Xml)
                aux += "email_xml: " + (!string.IsNullOrEmpty(cli.Email_Xml) ? cli.Email_Xml : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.EmailXml) ? dados.EmailXml : campo_vazio) + ";";
            if (dados.Numero != cli.Endereco_Numero)
                aux += "endereco_numero: " + (!string.IsNullOrEmpty(cli.Endereco_Numero) ? cli.Endereco_Numero : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Numero) ? dados.Numero : campo_vazio) + ";";
            if (dados.Complemento != cli.Endereco_Complemento)
                aux += "endereco_complemento: " + (!string.IsNullOrEmpty(cli.Endereco_Complemento) ? cli.Endereco_Complemento : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Complemento) ? dados.Complemento : campo_vazio) + ";";

            return aux;
        }

        private string MontarLogAlteracao_EnderecoCadastro(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados)
        {
            string aux = "";
            string campo_vazio = "\"\"";

            if (dados.Endereco != cli.Endereco)
            {
                aux += "endereco: " + (!string.IsNullOrEmpty(cli.Endereco) ? cli.Endereco : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Endereco) ? dados.Endereco : campo_vazio) + "; ";
                cli.Endereco = dados.Endereco;
            }
            if (dados.Bairro != cli.Bairro)
            {
                aux += "bairro: " + (!string.IsNullOrEmpty(cli.Bairro) ? cli.Bairro : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Bairro) ? dados.Bairro : campo_vazio) + "; ";
                cli.Bairro = dados.Bairro;
            }
            if (dados.Cidade != cli.Cidade)
            {
                aux += "cidade: " + (!string.IsNullOrEmpty(cli.Cidade) ? cli.Cidade : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Cidade) ? dados.Cidade : campo_vazio) + "; ";
                cli.Cidade = dados.Cidade;
            }
            if (dados.Uf != cli.Uf)
            {
                aux += "uf: " + (!string.IsNullOrEmpty(cli.Uf) ? cli.Uf : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Uf) ? dados.Uf : campo_vazio) + "; ";
                cli.Uf = dados.Uf;
            }

            if (dados.Cep != cli.Cep)
            {
                aux += "cep: " + (!string.IsNullOrEmpty(cli.Cep) ? cli.Cep : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(dados.Cep) ? dados.Cep : campo_vazio) + "; ";
                cli.Cep = dados.Cep;
            }

            return aux;
        }

        private string MontarLogAlteracao_ProdutorRural(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados, string apelido)
        {
            string log = "";

            if (dados.Tipo == Constantes.ID_PF)
            {
                string campo_vazio = "\"\"";
                if (dados.ProdutorRural != cli.Produtor_Rural_Status)
                {
                    byte codProdutor = 0;

                    switch (dados.ProdutorRural)
                    {
                        case (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL:
                            codProdutor = (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL;
                            break;
                        case (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM:
                            codProdutor = (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM;
                            break;
                        case (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO:
                            codProdutor = (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO;
                            break;
                    }
                    //status
                    log += "produtor_rural_status: " + (byte)cli.Produtor_Rural_Status + " => " +
                            (byte)codProdutor + "; ";
                    cli.Produtor_Rural_Status = codProdutor;
                    //data
                    log += "produtor_rural_data: " + (cli.Produtor_Rural_Data.HasValue ? cli.Produtor_Rural_Data.ToString() : campo_vazio) +
                            " = > " + DateTime.Now + "; ";
                    cli.Produtor_Rural_Data = DateTime.Now;
                    //data_hora
                    log += "produtor_rural_data_hora: " + (cli.Produtor_Rural_Data_Hora.HasValue ?
                        cli.Produtor_Rural_Data_Hora.ToString() : campo_vazio) + " => " + DateTime.Now + "; ";
                    cli.Produtor_Rural_Data_Hora = DateTime.Now;
                    //usuário
                    if (apelido.ToUpper() != cli.Produtor_Rural_Usuario.ToUpper())
                    {
                        log += "produtor_rural_usuario: " + (!string.IsNullOrEmpty(cli.Produtor_Rural_Usuario)
                        ? cli.Produtor_Rural_Usuario.ToUpper() : campo_vazio) + " => " + apelido.ToUpper() + "; ";
                        cli.Produtor_Rural_Usuario = apelido.ToUpper();
                    }
                }
            }

            return log;
        }

        private string MontarLogAlteracao_ContribuinteICMS(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados, string apelido)
        {
            string log = "";
            byte cod_contribuinte = 0;
            bool alterou = false;
            switch (dados.Contribuinte_Icms_Status)
            {
                case (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL:
                    cod_contribuinte = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;
                    break;
                case (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO:
                    cod_contribuinte = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO;
                    break;
                case (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO:
                    cod_contribuinte = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
                    break;
                case (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM:
                    cod_contribuinte = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
                    break;
            }

            if (dados.Tipo == Constantes.ID_PF)
            {
                if (dados.ProdutorRural == (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM &&
                    dados.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                {
                    //é obrigatório ser contribuinte
                    if (dados.Contribuinte_Icms_Status != cli.Contribuinte_Icms_Status)
                    {
                        //status
                        log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " + cod_contribuinte + "; ";
                        cli.Contribuinte_Icms_Status = cod_contribuinte;
                        alterou = true;
                    }
                }
                if (dados.ProdutorRural != (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                {
                    if (dados.Contribuinte_Icms_Status != cli.Contribuinte_Icms_Status)
                    {
                        //status
                        log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " + cod_contribuinte + "; ";
                        cli.Contribuinte_Icms_Status = cod_contribuinte;
                        alterou = true;
                    }
                }
            }

            if (dados.Tipo == Constantes.ID_PJ)
            {
                if (dados.Contribuinte_Icms_Status != (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    //é obrigatório ser contribuinte
                    if (dados.Contribuinte_Icms_Status != cli.Contribuinte_Icms_Status)
                    {
                        //status
                        log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " + cod_contribuinte + "; ";
                        cli.Contribuinte_Icms_Status = cod_contribuinte;
                        alterou = true;
                    }
                }
                if (dados.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    if (dados.Contribuinte_Icms_Status != cli.Contribuinte_Icms_Status)
                    {
                        //status
                        log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " + cod_contribuinte + "; ";
                        cli.Contribuinte_Icms_Status = cod_contribuinte;
                        alterou = true;
                    }
                }
            }

            if (alterou)
            {
                log += "contribuinte_icms_data: " + cli.Contribuinte_Icms_Data + " => " + DateTime.Now + "; ";
                cli.Contribuinte_Icms_Data = DateTime.Now;

                log += "contribuinte_icms_data_hora: " + cli.Contribuinte_Icms_Data_Hora + " => " + DateTime.Now + "; ";
                cli.Contribuinte_Icms_Data_Hora = DateTime.Now;

                if ((!string.IsNullOrEmpty(cli.Contribuinte_Icms_Usuario) ? cli.Contribuinte_Icms_Usuario.ToUpper() : "") != apelido.ToUpper())
                {
                    //contribuinte_icms_usuario: 
                    log += "contribuinte_icms_usuario: " + (!string.IsNullOrEmpty(cli.Contribuinte_Icms_Usuario) ? cli.Contribuinte_Icms_Usuario : "") + " => " +
                        apelido.ToUpper() + "; ";
                    cli.Contribuinte_Icms_Usuario = apelido.ToUpper();
                }
            }

            return log;
        }

        private string MontarLogAlteracaoPF_ProdutorRural_Contribuinte_IE(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados, string apelido)
        {
            string log = "";
            bool contribuinte_diferente = false;
            bool ie_diferente = false;
            bool produtor_diferente = false;

            if (dados.ProdutorRural == (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)
            {
                if (dados.ProdutorRural != cli.Produtor_Rural_Status)
                {
                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL + "; ";
                    cli.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL;

                    log += "contribuinte_icms_data: " + (!cli.Contribuinte_Icms_Data.HasValue
                        ? "\"\"" : cli.Contribuinte_Icms_Data.ToString()) + " => " + DateTime.Now + "; ";
                    cli.Contribuinte_Icms_Data = DateTime.Now;

                    log += "contribuinte_icms_data_hora: " + (!cli.Contribuinte_Icms_Data_Hora.HasValue
                        ? "\"\"" : cli.Contribuinte_Icms_Data_Hora.ToString()) + " => " + DateTime.Now + "; ";
                    cli.Contribuinte_Icms_Data_Hora = DateTime.Now;

                    if (apelido.ToUpper() != cli.Contribuinte_Icms_Usuario?.ToUpper())
                    {
                        log += "contribuinte_icms_usuario: " + (!string.IsNullOrEmpty(cli.Contribuinte_Icms_Usuario)
                            ? cli.Contribuinte_Icms_Usuario.ToUpper() : "\"\"") + " => " + apelido.ToUpper() + "; ";
                        cli.Contribuinte_Icms_Usuario = apelido.ToUpper();
                    }

                    log += "produtor_rural_status: " + cli.Produtor_Rural_Status + " => " +
                        (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO + "; ";
                    cli.Produtor_Rural_Status = (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO;

                    log += "produtor_rural_data: " + (!cli.Produtor_Rural_Data.HasValue
                        ? "\"\"" : cli.Produtor_Rural_Data.ToString()) + " => " + DateTime.Now + "; ";
                    cli.Produtor_Rural_Data = DateTime.Now;

                    log += "produtor_rural_data_hora: " + (!cli.Produtor_Rural_Data_Hora.HasValue
                        ? "\"\"" : cli.Produtor_Rural_Data_Hora.ToString()) + " => " + DateTime.Now + "; ";
                    cli.Produtor_Rural_Data_Hora = DateTime.Now;

                    if (apelido.ToUpper() != cli.Produtor_Rural_Usuario?.ToUpper())
                    {
                        log += "produtor_rural_usuario: " + (!string.IsNullOrEmpty(cli.Produtor_Rural_Usuario)
                            ? cli.Produtor_Rural_Usuario.ToUpper() : "\"\"") + " => " + apelido.ToUpper() + "; ";
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
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM + "; ";
                    cli.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
                }
                if (dados.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO)
                {
                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO + "; ";
                    cli.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
                }
                if (dados.Contribuinte_Icms_Status == (short)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    log += "contribuinte_icms_status: " + cli.Contribuinte_Icms_Status + " => " +
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO + "; ";
                    cli.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO;
                }

                log += "contribuinte_icms_data: " + (!cli.Contribuinte_Icms_Data.HasValue
                    ? "\"\"" : cli.Contribuinte_Icms_Data.ToString()) + " => " + DateTime.Now + "; ";
                cli.Contribuinte_Icms_Data = DateTime.Now;

                log += "contribuinte_icms_data_hora: " + (!cli.Contribuinte_Icms_Data_Hora.HasValue
                    ? "\"\"" : cli.Contribuinte_Icms_Data_Hora.ToString()) + " => " + DateTime.Now + "; ";
                cli.Contribuinte_Icms_Data_Hora = DateTime.Now;

                if (cli.Contribuinte_Icms_Usuario?.ToUpper() != apelido.ToUpper())
                {
                    //contribuinte_icms_usuario: 
                    log += "contribuinte_icms_usuario: " + (!string.IsNullOrEmpty(cli.Contribuinte_Icms_Usuario)
                        ? cli.Contribuinte_Icms_Usuario.ToUpper() : "\"\"") + " => " + apelido.ToUpper() + "; ";
                    cli.Contribuinte_Icms_Usuario = apelido.ToUpper();
                }

            }

            if (produtor_diferente)
            {
                log += "produtor_rural_status: " + cli.Produtor_Rural_Status + " => " +
                    (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM + "; ";
                cli.Produtor_Rural_Status = (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM;

                if (cli.Produtor_Rural_Data == null)
                {
                    log += "produtor_rural_data: \"\" => " + DateTime.Now + "; ";
                }
                else
                {
                    log += "produtor_rural_data: " + (!cli.Produtor_Rural_Data.HasValue
                        ? "\"\"" : cli.Produtor_Rural_Data.ToString()) + " => " + DateTime.Now + "; ";
                }

                cli.Produtor_Rural_Data = DateTime.Now;

                //if (cli.Produtor_Rural_Data_Hora == null)
                //{
                //    log += "produtor_rural_data_hora: \"\" => " + DateTime.Now + "; ";
                //}
                //else
                //{
                //    log += "produtor_rural_data_hora: " + cli.Produtor_Rural_Data_Hora + " => " + DateTime.Now + "; ";
                //}

                log += "produtor_rural_data_hora: " + (!cli.Produtor_Rural_Data_Hora.HasValue
                    ? "\"\"" : cli.Produtor_Rural_Data_Hora.ToString()) + " => " + DateTime.Now + "; ";

                cli.Produtor_Rural_Data_Hora = DateTime.Now;

                if (cli.Produtor_Rural_Usuario?.ToUpper() != apelido.ToUpper())
                {
                    log += "produtor_rural_usuario: " + (!string.IsNullOrEmpty(cli.Produtor_Rural_Usuario)
                    ? cli.Produtor_Rural_Usuario.ToUpper() : "\"\"") + " => " + apelido.ToUpper() + "; ";
                    cli.Produtor_Rural_Usuario = apelido.ToUpper();
                }


                //if (!string.IsNullOrEmpty(cli.Produtor_Rural_Usuario))
                //{
                //    if (apelido.ToUpper() != cli.Produtor_Rural_Usuario.ToUpper())
                //    {
                //        log += "produtor_rural_usuario: " + cli.Produtor_Rural_Usuario.ToUpper() + " => " +
                //                apelido.ToUpper() + "; ";
                //        cli.Produtor_Rural_Usuario = apelido.ToUpper();
                //    }
                //}
                //else
                //{
                //    log += "produtor_rural_usuario: \"\" => " + apelido.ToUpper() + "; ";
                //    cli.Produtor_Rural_Usuario = apelido.ToUpper();
                //}
            }


            return log;
        }

        private string MontarLogAlteracao_SistemaResponsavel(Tcliente cli)
        {
            string log = "";

            if (cli.Sistema_responsavel_atualizacao != (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS)
            {
                log += "sistema_responsavel_atualizacao: " + cli.Sistema_responsavel_atualizacao + " => " +
                    (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS + "; ";
                cli.Sistema_responsavel_atualizacao = (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
            }

            return log;
        }

        private string MontarLogAlteracao_IE(Tcliente cli, Cliente.Dados.DadosClienteCadastroDados dados, string apelido)
        {
            string log = "";
            string campo_vazio = "\"\"";
            //bool alterou = false;
            if (dados.Tipo == Constantes.ID_PF)
            {
                if (dados.ProdutorRural == (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO)
                {
                    //não pode ter IE
                    log += "ie: " + cli.Ie + " => " + campo_vazio + "; ";
                    cli.Ie = dados.Ie;
                }
                if (dados.ProdutorRural == (byte)Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                {
                    //é obrigatório ser contribuinte o ICMS e ter IE
                    if (cli.Ie != dados.Ie)
                    {
                        //não pode estar vazio 
                        log += "ie: " + ((!string.IsNullOrEmpty(cli.Ie)) ? cli.Ie : campo_vazio) + " => " + dados.Ie + "; ";
                        cli.Ie = dados.Ie;
                    }
                }
            }

            if (dados.Tipo == Constantes.ID_PJ)
            {
                if (dados.Contribuinte_Icms_Status != (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    //Obrigatório ter IE
                    if (cli.Ie != dados.Ie)
                    {
                        log += "ie: " + ((!string.IsNullOrEmpty(cli.Ie)) ? cli.Ie : campo_vazio) + " => " +
                            (!string.IsNullOrEmpty(dados.Ie) ? dados.Ie : campo_vazio) + "; ";
                        cli.Ie = dados.Ie;
                    }
                }
                if (dados.Contribuinte_Icms_Status == (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO)
                {
                    if (cli.Ie != dados.Ie)
                    {
                        log += "ie: " + ((!string.IsNullOrEmpty(cli.Ie)) ? cli.Ie : campo_vazio) + " => " +
                            (!string.IsNullOrEmpty(dados.Ie) ? dados.Ie : campo_vazio) + "; ";
                        cli.Ie = dados.Ie;
                    }
                }
            }

            return log;
        }

        private async Task<string> MontarLogAlteracao_Ref_Bancaria(Tcliente cli,
            Cliente.Dados.ClienteCadastroDados clienteCadastroDados, ContextoBdGravacao dbGravacao, List<string> lstErros, string apelido)
        {
            string log = "";
            bool removeu = false;

            if (clienteCadastroDados.RefBancaria.Count > 0)
            {
                //List<Cliente.Dados.Referencias.RefBancariaClienteDados> lstrefBancaria = (await ObterReferenciaBancaria(cli)).ToList();
                List<TclienteRefBancaria> lstrefBancaria = (from c in dbGravacao.TclienteRefBancaria
                                                            where c.Id_Cliente == clienteCadastroDados.DadosCliente.Id &&
                                                                  c.Banco == clienteCadastroDados.RefBancaria[0].Banco &&
                                                                  c.Agencia == clienteCadastroDados.RefBancaria[0].Agencia &&
                                                                  c.Conta == clienteCadastroDados.RefBancaria[0].Conta
                                                            select c).ToList();
                //já existe então vamos verificar
                if (lstrefBancaria != null)
                {
                    if (lstrefBancaria.Count > 0)
                    {
                        //atualiza ref bancária
                        if (lstrefBancaria[0].Ddd != clienteCadastroDados.RefBancaria[0].Ddd
                            || lstrefBancaria[0].Telefone != clienteCadastroDados.RefBancaria[0].Telefone
                            || lstrefBancaria[0].Contato != clienteCadastroDados.RefBancaria[0].Contato)
                            log = await AtualizarRefBancaria(dbGravacao, lstErros, clienteCadastroDados, lstrefBancaria[0], log);
                        //não teve alteração no log e os campos estão diferentes, removemos
                        if (string.IsNullOrEmpty(log) && lstrefBancaria[0].Banco != clienteCadastroDados.RefBancaria[0].Banco
                            || lstrefBancaria[0].Agencia != clienteCadastroDados.RefBancaria[0].Agencia
                            || lstrefBancaria[0].Conta != clienteCadastroDados.RefBancaria[0].Conta)
                        {
                            log = await RemoverRefBancaria(dbGravacao, lstErros, clienteCadastroDados, log);
                            removeu = true;
                        }
                    }
                }
                //se não tem nada vamos cadastrar ou se teve alteração de banco ou agencia ou contato então foi excluido e vamos cadastrar uma nova
                if (lstrefBancaria == null || removeu)
                {
                    //se existir referência bancária na lista TclienteRefBancaria vamos excluir e depois adicionar
                    log = await CadastrarRefBancaria(dbGravacao, clienteCadastroDados.RefBancaria, apelido, clienteCadastroDados.DadosCliente.Id, log);
                }
            }

            return log;
        }

        private async Task<string> MontarLogAleracao_Ref_Comercial(Tcliente cli, Cliente.Dados.ClienteCadastroDados clienteCadastroDados,
            ContextoBdGravacao dbGravacao, List<string> lstErros, string apelido, Constantes.CodSistemaResponsavel sistemaResponsavel,
            List<ListaBancoDados> lstBanco)
        {
            string log = "";
            string logRemove = "";

            if (clienteCadastroDados.DadosCliente.Tipo == Constantes.ID_PJ && clienteCadastroDados.RefComercial.Count > 0)
            {
                foreach (var refDados in clienteCadastroDados.RefComercial)
                {
                    TclienteRefComercial refComercialBase = await (from c in dbGravacao.TclienteRefComercial
                                                                   where c.Id_Cliente == clienteCadastroDados.DadosCliente.Id &&
                                                                         c.Nome_Empresa == refDados.Nome_Empresa
                                                                   select c).FirstOrDefaultAsync();
                    //existe ref comercial para esse cliente
                    if (refComercialBase != null)
                    {
                        if (refComercialBase.Contato != refDados.Contato ||
                            refComercialBase.Ddd != refDados.Ddd ||
                            refComercialBase.Telefone != refDados.Telefone)
                            log += await AtualizarRefComercial(dbGravacao, lstErros, refDados, refComercialBase, log);
                    }
                    //não existe ref comercial com esse nome, vamos inserir
                    if (refComercialBase == null)
                    {
                        //como já temos uma rotina de cadastro de ref comercial que recebe uma lista, 
                        //vamos criar uma nova lista com apenas 1 registro para cadastrar essa ref comercial
                        List<Cliente.Dados.Referencias.RefComercialClienteDados> lstRefComercial = new List<Dados.Referencias.RefComercialClienteDados>();
                        lstRefComercial.Add(refDados);

                        log += await CadastrarRefComercial(dbGravacao, lstRefComercial, apelido, clienteCadastroDados.DadosCliente.Id, log);
                        logRemove += await RemoverRefComercial(dbGravacao, refDados, apelido, clienteCadastroDados.DadosCliente.Id, logRemove);
                    }
                }

                if (!string.IsNullOrEmpty(logRemove))
                    log += logRemove;
            }

            return log;
        }






        private async Task<string> RemoverRefBancaria(InfraBanco.ContextoBdGravacao dbGravacao, List<string> lstErros,
            Cliente.Dados.ClienteCadastroDados clienteCadastroDados, string log)
        {
            string campos_a_omitir_ref_bancaria = "|id_cliente|ordem|excluido_status|dt_cadastro|usuario_cadastro|";
            log = log + "Ref Bancária excluída: ";

            dbGravacao.Remove(clienteCadastroDados.RefBancaria[0]);
            await dbGravacao.SaveChangesAsync();

            TclienteRefBancaria refBancariaBase = await (from c in dbGravacao.TclienteRefBancaria
                                                         where c.Id_Cliente == clienteCadastroDados.DadosCliente.Id
                                                         select c).FirstOrDefaultAsync();
            if (refBancariaBase != null)
            {
                lstErros.Add("FALHA AO ALTERAR DADOS DE REF BANCÁRIA DO CLIENTE (" + clienteCadastroDados.DadosCliente.Nome + ")");
            }

            //Busca os nomes reais das colunas da tabela SQL
            log = UtilsGlobais.Util.MontaLog(clienteCadastroDados.RefBancaria[0], log, campos_a_omitir_ref_bancaria);

            return log;
        }

        private async Task<string> AtualizarRefBancaria(InfraBanco.ContextoBdGravacao dbgravacao, List<string> lstErros,
            Cliente.Dados.ClienteCadastroDados clienteCadastroDados, TclienteRefBancaria refBancariaBase, string log)
        {
            string logRetorno = log;

            string logRef = "Ref Bancária alterada (banco: " + refBancariaBase.Banco + ", ag: " + refBancariaBase.Agencia +
                ", conta: " + refBancariaBase.Conta + "): ";

            string aux = "";
            string campo_vazio = "\"\"";
            Cliente.Dados.Referencias.RefBancariaClienteDados refBancariaDados = clienteCadastroDados.RefBancaria[0];

            if (refBancariaBase.Ddd != refBancariaDados.Ddd)
            {
                aux = "ddd: " + (!string.IsNullOrEmpty(refBancariaBase.Ddd) ? refBancariaBase.Ddd : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(refBancariaDados.Ddd) ? refBancariaDados.Ddd : campo_vazio) + "";
                refBancariaBase.Ddd = refBancariaDados.Ddd;
            }

            if (refBancariaBase.Telefone != refBancariaDados.Telefone)
            {
                aux += (!string.IsNullOrEmpty(aux) ? "; " : "") + "telefone: " +
                    (!string.IsNullOrEmpty(refBancariaBase.Telefone) ? refBancariaBase.Telefone : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(refBancariaDados.Telefone) ? refBancariaDados.Telefone : campo_vazio) + "";
                refBancariaBase.Telefone = refBancariaDados.Telefone;
            }

            if (refBancariaBase.Contato != refBancariaDados.Contato)
            {
                aux += (!string.IsNullOrEmpty(aux) ? "; " : "") + "contato: " + (!string.IsNullOrEmpty(refBancariaBase.Contato)
                    ? refBancariaBase.Contato : campo_vazio) + " => " + (!string.IsNullOrEmpty(refBancariaDados.Contato)
                    ? refBancariaDados.Contato : campo_vazio) + "";
                refBancariaBase.Contato = refBancariaDados.Contato;
            }

            if (!string.IsNullOrEmpty(aux))
            {
                logRef += aux;
                logRetorno += logRef + "; ";
                //se alterar apenas ddd, telefone, contato apenas alteramos            
                dbgravacao.Update(refBancariaBase);
                await dbgravacao.SaveChangesAsync();

                //vamos confirmar o cadastro
                //TclienteRefBancaria alterouRef = await (from c in dbgravacao.TclienteRefBancarias
                //                                        where c.Id_Cliente == refBancariaBase.Id_Cliente &&
                //                                              c.Banco == refBancariaBase.Banco &&
                //                                              c.Agencia == refBancariaBase.Agencia &&
                //                                              c.Conta == refBancariaBase.Conta &&
                //                                              c.Ddd == refBancariaBase.Ddd &&
                //                                              c.Telefone == refBancariaBase.Telefone &&
                //                                              c.Contato == refBancariaBase.Conta &&
                //                                              c.Excluido_Status == 0
                //                                        select c).FirstOrDefaultAsync();

                //if(alterouRef == null)
                //{
                //    lstErros.Add("FALHA AO GRAVAR OS DADOS DA REF BANCÁRIA (" + refBancariaBase.Banco +  ": " & Err.Description & ").")
                //}
            }

            return logRetorno;
        }


        private async Task<string> RemoverRefComercial(InfraBanco.ContextoBdGravacao dbgravacao,
            Cliente.Dados.Referencias.RefComercialClienteDados refComercialDados, string apelido, string id_cliente, string log)
        {
            log += "Ref Comercial excluída: ";

            TclienteRefComercial refComercialBase = await (from c in dbgravacao.TclienteRefComercial
                                                           where c.Id_Cliente == id_cliente &&
                                                                 c.Ordem == refComercialDados.Ordem
                                                           select c).FirstOrDefaultAsync();

            if (refComercialBase != null)
            {
                dbgravacao.Remove(refComercialBase);
                await dbgravacao.SaveChangesAsync();
                string campos_a_omitir_ref_comercial = "|id_cliente|ordem|excluido_status|dt_cadastro|usuario_cadastro|";
                log = UtilsGlobais.Util.MontaLog(refComercialBase, log, campos_a_omitir_ref_comercial);
            }

            return log;
        }

        private async Task<string> AtualizarRefComercial(InfraBanco.ContextoBdGravacao dbgravacao, List<string> lstErros,
            Cliente.Dados.Referencias.RefComercialClienteDados refComercialDados, TclienteRefComercial refComercialBase, string log)
        {
            string logRetorno = log;

            string logRef = "Ref Comercial alterada (empresa: " + refComercialBase.Nome_Empresa + "): ";
            string aux = "";
            string campo_vazio = "\"\"";

            if (refComercialBase.Contato != refComercialDados.Contato)
            {
                aux += (!string.IsNullOrEmpty(aux) ? "; " : "") + "contato: " + (!string.IsNullOrEmpty(refComercialBase.Contato)
                    ? refComercialBase.Contato : campo_vazio) + " => " + (!string.IsNullOrEmpty(refComercialDados.Contato)
                    ? refComercialDados.Contato : campo_vazio) + "";
                refComercialBase.Contato = refComercialDados.Contato;
            }
            if (refComercialBase.Ddd != refComercialDados.Ddd)
            {
                aux = "ddd: " + (!string.IsNullOrEmpty(refComercialBase.Ddd) ? refComercialBase.Ddd : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(refComercialDados.Ddd) ? refComercialDados.Ddd : campo_vazio) + "";
                refComercialBase.Ddd = refComercialDados.Ddd;
            }

            if (refComercialBase.Telefone != refComercialDados.Telefone)
            {
                aux += (!string.IsNullOrEmpty(aux) ? "; " : "") + "telefone: " +
                    (!string.IsNullOrEmpty(refComercialBase.Telefone) ? refComercialBase.Telefone : campo_vazio) +
                    " => " + (!string.IsNullOrEmpty(refComercialDados.Telefone) ? refComercialDados.Telefone : campo_vazio) + "";
                refComercialBase.Telefone = refComercialDados.Telefone;
            }

            if (!string.IsNullOrEmpty(aux))
            {
                logRef += aux;
                logRetorno += logRef + "; ";

                dbgravacao.Update(refComercialBase);
                await dbgravacao.SaveChangesAsync();
            }

            return logRetorno;
        }

    }
}
#endif

