using Loja.Bll.Dto;
using Loja.Bll.Dto.ClienteDto;
using Loja.Bll.CepBll;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Loja.Bll.Dto.CepDto;
using InfraBanco.Modelos;

namespace Loja.Bll.ClienteBll
{
    public class ClienteBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;
        private readonly InfraBanco.ContextoNFeProvider contextoNFeProvider;
        private readonly Cliente.ClienteBll clienteBll;

        public ClienteBll(InfraBanco.ContextoBdProvider contextoProvider, InfraBanco.ContextoCepProvider contextoCepProvider,
            InfraBanco.ContextoNFeProvider contextoNFeProvider, Cliente.ClienteBll clienteBll)
        {
            this.contextoProvider = contextoProvider;
            this.contextoCepProvider = contextoCepProvider;
            this.contextoNFeProvider = contextoNFeProvider;
            this.clienteBll = clienteBll;
        }

        //Esse metodo é utilizado em praticamente todas as páginas 
        //verificar para a necessidade de utilizar uma "Session"

        public async Task<string> BuscaListaOperacoesPermitidas(string apelido)
        {
            string retorno = "";
            var db = contextoProvider.GetContextoLeitura();
            //            SELECT DISTINCT id_operacao
            //FROM t_PERFIL
            //INNER JOIN t_PERFIL_ITEM ON t_PERFIL.id = t_PERFIL_ITEM.id_perfil
            //INNER JOIN t_PERFIL_X_USUARIO ON t_PERFIL.id = t_PERFIL_X_USUARIO.id_perfil
            //INNER JOIN t_OPERACAO ON(t_PERFIL_ITEM.id_operacao= t_OPERACAO.id)
            //WHERE(t_PERFIL_X_USUARIO.usuario = 'pragmatica') AND
            //      (t_PERFIL.st_inativo = 0) AND
            //      (t_OPERACAO.st_inativo = 0)
            //ORDER BY id_operacao

            var lstTask = (from c in db.Tperfils
                          .Include(r => r.TperfilItem)
                          .Include(r => r.TperfilUsuario)
                          .Include(r => r.TperfilItem.Toperacao)
                           where c.TperfilUsuario.Usuario == apelido &&
                                 c.St_inativo == 0 &&
                                 c.TperfilItem.Toperacao.St_inativo == 0
                           orderby c.TperfilItem.Id_operacao
                           select c.TperfilItem.Id_operacao).Distinct();



            foreach (var i in await lstTask.ToListAsync())
            {
                retorno = retorno + "|" + i;
            }

            if (!String.IsNullOrWhiteSpace(retorno))
            {
                if (retorno.Substring(retorno.Length - 1, 1) != "" || retorno.Substring(retorno.Length - 1, 1) != "|")
                    retorno = retorno + "|";
            }

            return retorno;
        }

        //function obtem_nivel_acesso_bloco_notas_pedido(ByRef cnBancoDados, byval usuario)
        public async Task<int> NivelAcessoBlocoNotasPedido(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var nivelTask = from c in db.Tperfils
                            join d in db.TperfilUsuarios on c.Id equals d.Id_perfil
                            where d.Usuario == apelido
                            select c;

            var nivel = await nivelTask.OrderByDescending(x => x.Nivel_acesso_bloco_notas_pedido).Select(x => (int?)x.Nivel_acesso_bloco_notas_pedido).FirstOrDefaultAsync();
            if (!nivel.HasValue)
                nivel = Constantes.Constantes.COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__NAO_DEFINIDO;

            return nivel.Value;
        }

        //function obtem_nivel_acesso_chamado_pedido(ByRef cnBancoDados, byval usuario)
        public async Task<int> NivelAcessoChamadoPedido(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var nivelTask = from c in db.Tperfils
                            join d in db.TperfilUsuarios on c.Id equals d.Id_perfil
                            where d.Usuario == apelido
                            select c;

            var nivel = await nivelTask.OrderByDescending(x => x.Nivel_acesso_chamado).Select(x => (int?)x.Nivel_acesso_chamado).FirstOrDefaultAsync();
            if (!nivel.HasValue)
                nivel = Constantes.Constantes.COD_NIVEL_ACESSO_CHAMADO_PEDIDO__NAO_DEFINIDO;

            return nivel.Value;
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

        public async Task<bool> ValidarCliente(string cpf_cnpj)
        {
            bool retorno = false;

            var db = contextoProvider.GetContextoLeitura();

            var clienteTask = from c in db.Tclientes
                              where c.Cnpj_Cpf == cpf_cnpj
                              select c.Cnpj_Cpf;
            string cliente = await clienteTask.FirstOrDefaultAsync();

            if (cliente == cpf_cnpj)
                retorno = true;

            return retorno;
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
            var refBancariaTask = ObterReferenciaBancaria(dadosCliente.Id);
            var refComercialTask = ObterReferenciaComercial(dadosCliente.Id);

            ClienteCadastroDto cliente = new ClienteCadastroDto
            {
                DadosCliente = dadosClienteTask,
                RefBancaria = (await refBancariaTask).ToList(),
                RefComercial = (await refComercialTask).ToList()
            };

            return await Task.FromResult(cliente);
        }

        public DadosClienteCadastroDto ObterDadosClienteCadastro(Tcliente cli, string loja)
        {
            DadosClienteCadastroDto dados = new DadosClienteCadastroDto
            {
                Id = cli.Id,
                Cnpj_Cpf = Util.Util.FormatCpf_Cnpj_Ie(cli.Cnpj_Cpf),
                Rg = cli.Rg,
                Ie = Util.Util.FormatCpf_Cnpj_Ie(cli.Ie),
                Contribuinte_Icms_Status = cli.Contribuinte_Icms_Status,
                Tipo = cli.Tipo,
                Observacao_Filiacao = cli.Filiacao,
                Nascimento = cli.Dt_Nasc,
                Sexo = cli.Sexo,
                Nome = cli.Nome,
                Indicador_Orcamentista = cli.Indicador,
                ProdutorRural = cli.Produtor_Rural_Status,
                DddResidencial = cli.Ddd_Res,
                TelefoneResidencial = Util.Util.FormataTelefone(cli.Ddd_Res, cli.Tel_Res),
                DddComercial = cli.Ddd_Com,
                TelComercial = Util.Util.FormataTelefone(cli.Ddd_Com, cli.Tel_Com),
                DddComercial2 = cli.Ddd_Com_2,
                TelComercial2 = Util.Util.FormataTelefone(cli.Ddd_Com_2, cli.Tel_Com_2),
                Ramal = cli.Ramal_Com,
                DddCelular = cli.Ddd_Cel,
                Celular = Util.Util.FormataTelefone(cli.Ddd_Cel, cli.Tel_Cel),
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

        private async Task<IEnumerable<RefBancariaDtoCliente>> ObterReferenciaBancaria(string id_cliente)
        {
            List<RefBancariaDtoCliente> lstRef = new List<RefBancariaDtoCliente>();
            var db = contextoProvider.GetContextoLeitura();

            //selecionamos as referências bancárias já incluindo a descrição do banco
            var rBanco = from c in db.TclienteRefBancarias
                         join banco in db.Tbancos on c.Banco equals banco.Codigo
                         where c.Id_Cliente == id_cliente
                         orderby c.Ordem
                         select new { c.Banco, c.Agencia, c.Conta, c.Contato, c.Ddd, c.Telefone, banco.Descricao };

            foreach (var i in await rBanco.ToListAsync())
            {
                RefBancariaDtoCliente refBanco = new RefBancariaDtoCliente
                {
                    Banco = i.Banco,
                    BancoDescricao = i.Descricao,
                    Agencia = i.Agencia,
                    ContaBanco = i.Conta,
                    ContatoBanco = i.Contato,
                    DddBanco = i.Ddd,
                    TelefoneBanco = Util.Util.FormataTelefone(i.Ddd, i.Telefone)
                };
                lstRef.Add(refBanco);
            }

            return lstRef;
        }

        private async Task<IEnumerable<RefComercialDtoCliente>> ObterReferenciaComercial(string id_cliente)
        {
            List<RefComercialDtoCliente> lstRefComercial = new List<RefComercialDtoCliente>();
            var db = contextoProvider.GetContextoLeitura();

            var rComercial = from c in db.TclienteRefComercials
                             where c.Id_Cliente == id_cliente
                             orderby c.Ordem
                             select c;

            foreach (var i in await rComercial.ToListAsync())
            {
                RefComercialDtoCliente rCom = new RefComercialDtoCliente
                {
                    Nome_Empresa = i.Nome_Empresa,
                    Contato = i.Contato,
                    Ddd = i.Ddd,
                    Telefone = Util.Util.FormataTelefone(i.Ddd, i.Telefone)
                };

                lstRefComercial.Add(rCom);
            }

            return lstRefComercial;
        }

        public async Task<IEnumerable<string>> BuscarListaIndicadores(string indicador, string usuarioSistema, string loja)
        {
            List<string> lst = (await Util.Util.BuscarListaOrcamentistaEIndicador(contextoProvider, indicador, usuarioSistema, loja)).ToList();

            return lst;
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
                          where c.Grupo == Loja.Bll.Constantes.Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA &&
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

        public async Task<IEnumerable<string>> Novo_CadastrarCliente(ClienteCadastroDto clienteDto, string usuario)
        {
            List<string> ret = (await clienteBll.CadastrarCliente(ClienteCadastroDto.ClienteCadastroDados_De_ClienteCadastroDto(clienteDto), usuario.Trim(),
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS, usuario)).ToList();

            return ret;
        }

        public async Task<IEnumerable<string>> CadastrarCliente(ClienteCadastroDto clienteDto, string apelido, string loja)
        {
            string id_cliente = "";

            //teste
            //apelido = "MARISARJ";

            var db = contextoProvider.GetContextoLeitura();
            var verifica = await (from c in db.Tclientes
                                  where c.Id == clienteDto.DadosCliente.Id
                                  select c.Id).FirstOrDefaultAsync();

            List<string> lstErros = new List<string>();

            id_cliente = verifica;

            //Na validação do cadastro é feito a consistencia de Municipio
            await ValidarDadosClientesCadastro(clienteDto.DadosCliente, lstErros);
            ValidarRefBancaria(clienteDto.RefBancaria, lstErros);
            ValidarRefComercial(clienteDto.RefComercial, lstErros);

            if (lstErros.Count <= 0)
            {
                using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
                {
                    string log = "";

                    if (verifica != null)
                    {
                        if (id_cliente.Length == 12)
                        {
                            //se ele existe iremos fazer um update no cadastro do cliente
                            await AtualizarCadastroCliente(dbgravacao, clienteDto.DadosCliente, apelido, loja);
                            //atualizar as referencias bancárias
                            await AtualizarRefBancaria(dbgravacao, id_cliente, loja, clienteDto.RefBancaria, apelido);
                            //atualizar as referencias comerciais
                            await AtualizarRefComercial(dbgravacao, id_cliente, loja, clienteDto.RefComercial, apelido);
                            //gravar o log com Constantes.Constantes.OP_LOG_CLIENTE_ALTERACAO
                            //bool gravouLog = Util.Util.GravaLog(dbgravacao, apelido, clienteDto.DadosCliente.Loja, "", id_cliente,
                            //    Constantes.Constantes.OP_LOG_CLIENTE_ALTERACAO, log, contextoProvider);
                            //if (gravouLog)
                            //{
                            dbgravacao.transacao.Commit();
                            //}
                        }
                    }
                    else
                    {
                        //Inclusão de cliente
                        DadosClienteCadastroDto cliente = new DadosClienteCadastroDto();
                        cliente = clienteDto.DadosCliente;
                        id_cliente = await CadastrarDadosClienteDto(dbgravacao, cliente, apelido, log);

                        //Por padrão o id do cliente tem 12 caracteres, caso não seja 12 caracteres esta errado
                        if (id_cliente.Length == 12)
                        {
                            await CadastrarRefBancaria(dbgravacao, clienteDto.RefBancaria, apelido, id_cliente, log);
                            await CadastrarRefComercial(dbgravacao, clienteDto.RefComercial, apelido, id_cliente, log);
                            //fazer a inserção de Log aqui.
                            bool gravouLog = Util.Util.GravaLog(dbgravacao, apelido, cliente.Loja, "", id_cliente,
                                Constantes.Constantes.OP_LOG_CLIENTE_INCLUSAO, log, contextoProvider);
                            if (gravouLog)
                                dbgravacao.transacao.Commit();
                        }
                        else
                        {
                            lstErros.Add(id_cliente);
                        }
                    }
                }
            }
            return lstErros;
        }
        public async Task AtualizarCadastroCliente(InfraBanco.ContextoBdGravacao dbgravacao,
            DadosClienteCadastroDto dadosCliente, string apelido, string loja)
        {
            List<string> lstRetorno = new List<string>();

            string campos_a_omitir = "dt_cadastro|usuario_cadastro|dt_ult_atualizacao|usuario_ult_atualizacao";
            //é necessário fazer a busca na base para poder comparar os dados para montagem do log;

            var cliente = (from c in dbgravacao.Tclientes
                           where c.Id == dadosCliente.Id
                           select c).FirstOrDefault();

            //tclienteBase é os dados que já estão guardados e serão utilizados para comparação
            Tcliente tClienteBase = new Tcliente();
            tClienteBase = cliente;

            cliente.Id = dadosCliente.Id;
            cliente.Dt_Cadastro = DateTime.Now;
            cliente.Usuario_Cadastro = tClienteBase.Usuario_Cadastro.ToUpper();
            cliente.Indicador = dadosCliente.Indicador_Orcamentista;
            cliente.Cnpj_Cpf = Util.Util.SoDigitosCpf_Cnpj(dadosCliente.Cnpj_Cpf);
            cliente.Tipo = dadosCliente.Tipo.ToUpper();
            cliente.Ie = dadosCliente.Ie == null ? dadosCliente.Ie = "" : dadosCliente.Ie;
            cliente.Rg = dadosCliente.Rg == null ? dadosCliente.Rg = "" : dadosCliente.Rg;
            cliente.Nome = dadosCliente.Nome;
            cliente.Sexo = dadosCliente.Sexo == null ? dadosCliente.Sexo = null : dadosCliente.Sexo.ToUpper();
            if (dadosCliente.Contribuinte_Icms_Status != 0 &&
                dadosCliente.Contribuinte_Icms_Status != tClienteBase.Contribuinte_Icms_Status)
            {
                cliente.Contribuinte_Icms_Status = dadosCliente.Contribuinte_Icms_Status;
                cliente.Contribuinte_Icms_Data = DateTime.Now;
                cliente.Contribuinte_Icms_Data_Hora = DateTime.Now;
                cliente.Contribuinte_Icms_Usuario = apelido.ToUpper();
            }
            else
            {
                cliente.Contribuinte_Icms_Status = tClienteBase.Contribuinte_Icms_Status;
                cliente.Contribuinte_Icms_Data = tClienteBase.Contribuinte_Icms_Data;
                cliente.Contribuinte_Icms_Data_Hora = tClienteBase.Contribuinte_Icms_Data_Hora;
                cliente.Contribuinte_Icms_Usuario = tClienteBase.Contribuinte_Icms_Usuario;
            }
            if (dadosCliente.ProdutorRural != 0 && dadosCliente.ProdutorRural != tClienteBase.Produtor_Rural_Status)
            {
                cliente.Produtor_Rural_Status = dadosCliente.ProdutorRural;
                cliente.Produtor_Rural_Data = DateTime.Now;
                cliente.Produtor_Rural_Data_Hora = DateTime.Now;
                cliente.Produtor_Rural_Usuario = apelido.ToUpper();
            }
            else
            {
                cliente.Produtor_Rural_Status = tClienteBase.Produtor_Rural_Status;
                cliente.Produtor_Rural_Data = tClienteBase.Produtor_Rural_Data;
                cliente.Produtor_Rural_Data_Hora = tClienteBase.Produtor_Rural_Data_Hora;
                cliente.Produtor_Rural_Usuario = tClienteBase.Produtor_Rural_Usuario;
            }
            cliente.Endereco = dadosCliente.Endereco;
            cliente.Endereco_Numero = dadosCliente.Numero;
            cliente.Endereco_Complemento = dadosCliente.Complemento;
            cliente.Bairro = dadosCliente.Bairro;
            cliente.Cidade = dadosCliente.Cidade;
            cliente.Cep = dadosCliente.Cep.Replace("-", "");
            cliente.Uf = dadosCliente.Uf.ToUpper();
            cliente.Ddd_Res = dadosCliente.DddResidencial == null ?
                dadosCliente.DddResidencial = "" : dadosCliente.DddResidencial;
            cliente.Tel_Res = dadosCliente.TelefoneResidencial == null ?
                dadosCliente.TelefoneResidencial = "" : dadosCliente.TelefoneResidencial.Replace("-", "").Trim();
            cliente.Ddd_Com = dadosCliente.DddComercial == null ?
                dadosCliente.DddComercial = "" : dadosCliente.DddComercial;
            cliente.Tel_Com = dadosCliente.TelComercial == null ?
                dadosCliente.TelComercial = "" : dadosCliente.TelComercial.Replace("-", "").Trim();
            cliente.Ramal_Com = dadosCliente.Ramal == null ?
                dadosCliente.Ramal = "" : dadosCliente.Ramal;
            cliente.Contato = dadosCliente.Contato == null ?
                dadosCliente.Contato = "" : dadosCliente.Contato;
            cliente.Ddd_Com_2 = dadosCliente.DddComercial2 == null ?
                dadosCliente.DddComercial2 = "" : dadosCliente.DddComercial2;
            cliente.Tel_Com_2 = dadosCliente.TelComercial2 == null ?
                dadosCliente.TelComercial2 = "" : dadosCliente.TelComercial2.Replace("-", "").Trim();
            cliente.Ramal_Com_2 = dadosCliente.Ramal2 == null ?
                dadosCliente.Ramal2 = "" : dadosCliente.Ramal2;
            cliente.Ddd_Cel = dadosCliente.DddCelular == null ?
                dadosCliente.DddCelular = "" : dadosCliente.DddCelular;
            cliente.Tel_Cel = dadosCliente.Celular == null ?
                dadosCliente.Celular = "" : dadosCliente.Celular.Replace("-", "").Trim();
            cliente.Dt_Nasc = dadosCliente.Nascimento;
            cliente.Filiacao = dadosCliente.Observacao_Filiacao == null ?
                dadosCliente.Observacao_Filiacao = "" : dadosCliente.Observacao_Filiacao;
            cliente.Obs_crediticias = "";
            cliente.Midia = "";
            cliente.Email = dadosCliente.Email == null ?
                dadosCliente.Email = "" : dadosCliente.Email;
            cliente.Email_Xml = dadosCliente.EmailXml == null ?
                dadosCliente.EmailXml = "" : dadosCliente.EmailXml;
            cliente.Dt_Ult_Atualizacao = DateTime.Now;
            cliente.Usuario_Ult_Atualizacao = apelido.ToUpper();

            string log = Util.Util.MontaLogAlteracao(cliente, tClienteBase, campos_a_omitir);

            if (log != "" && log != null)
            {
                log = "id=" + tClienteBase.Id + "; " + log;

                bool salvouLog = Util.Util.GravaLog(dbgravacao, apelido, loja, "",
                    cliente.Id, Constantes.Constantes.OP_LOG_CLIENTE_ALTERACAO, log, contextoProvider);
            }

            dbgravacao.Update(cliente);
            await dbgravacao.SaveChangesAsync();
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
                    using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
                    {
                        if (dadosClienteCadastroDto.Contribuinte_Icms_Status == byte.Parse(Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM) &&
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
                            if (dadosClienteCadastroDto.Tipo == Constantes.Constantes.ID_PF &&
                                dadosClienteCadastroDto.ProdutorRural != byte.Parse(Constantes.Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL) &&
                                dadosClienteCadastroDto.ProdutorRural != cli.Produtor_Rural_Status)
                            {
                                cli.Produtor_Rural_Status = dadosClienteCadastroDto.ProdutorRural;
                                cli.Produtor_Rural_Data = DateTime.Now;
                                cli.Produtor_Rural_Data_Hora = DateTime.Now;
                                cli.Produtor_Rural_Usuario = apelido;
                            }
                            cli.Dt_Ult_Atualizacao = DateTime.Now;
                            cli.Usuario_Ult_Atualizacao = apelido;

                            dbgravacao.Update(cli);
                            dbgravacao.SaveChanges();

                            log = Util.Util.MontaLog(cli, log, campos_a_omitir);
                            //Essa parte esta na pagina ClienteAtualiza.asp linha 1113
                            bool salvouLog = Util.Util.GravaLog(dbgravacao, apelido, dadosClienteCadastroDto.Loja, "", dadosClienteCadastroDto.Id,
                                Constantes.Constantes.OP_LOG_CLIENTE_ALTERACAO, log, contextoProvider);
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

        private async Task ValidarDadosClientesCadastro(DadosClienteCadastroDto cliente, List<string> listaErros)
        {
            string cpf_cnpjSoDig = Util.Util.SoDigitosCpf_Cnpj(cliente.Cnpj_Cpf);
            bool ehCpf = Util.Util.ValidaCpf_Cnpj(cliente.Cnpj_Cpf);

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
                    if (cliente.Tipo == Constantes.Constantes.ID_PF)
                        listaErros.Add("PREENCHA O NOME DO CLIENTE.");
                }
                //a verificação de Nascimento esta sendo feita no cliente
                //if (cliente.Nascimento == null)
                //    listaErros.Add("DATA DE NASCIMENTO É INVÁLIDA.");

                if (cliente.Tipo == Constantes.Constantes.ID_PF &&
                cliente.TelefoneResidencial == "" &&
                cliente.TelComercial == "" &&
                cliente.Celular == "")
                    listaErros.Add("PREENCHA PELO MENOS UM TELEFONE.");
                if (cliente.TelefoneResidencial != "")
                {
                    //atribuimos o ddd e o telefone para o objeto
                    cliente.DddResidencial = Util.Util.MontarDDD(cliente.TelefoneResidencial);
                    cliente.TelefoneResidencial = Util.Util.MontarTelefone(cliente.TelefoneResidencial);

                    if (cliente.DddResidencial.Length != 2 && cliente.DddResidencial != "")
                        listaErros.Add("DDD INVÁLIDO.");
                    if (cliente.TelefoneResidencial.Length < 6 && cliente.TelefoneResidencial != "")
                        listaErros.Add("TELEFONE RESIDENCIAL INVÁLIDO.");
                }
                if (cliente.DddResidencial != "" && cliente.TelefoneResidencial == "")
                    listaErros.Add("PREENCHA O TELEFONE RESIDENCIAL.");
                if (cliente.DddResidencial == "" && cliente.TelefoneResidencial != "")
                    listaErros.Add("PREENCHA O DDD.");
                if (cliente.TelComercial != "" && cliente.TelComercial != null)
                {
                    cliente.DddComercial = Util.Util.MontarDDD(cliente.TelComercial);
                    cliente.TelComercial = Util.Util.MontarTelefone(cliente.TelComercial);

                    if (cliente.TelComercial != "" && cliente.DddComercial == "")
                        listaErros.Add("PREENCHA O DDD COMERCIAL.");
                    if (cliente.DddComercial != "" && cliente.TelComercial == "")
                        listaErros.Add("PREENCHA O TELEFONE COMERCIAL.");
                }
                if (cliente.Celular != "" && cliente.Celular != null)
                {
                    cliente.DddCelular = Util.Util.MontarDDD(cliente.Celular);
                    cliente.Celular = Util.Util.MontarTelefone(cliente.Celular);
                }
            }
            else
            {
                if (cliente.Tipo == Constantes.Constantes.ID_PJ && cliente.Nome == "")
                    listaErros.Add("PREENCHA A RAZÃO SOCIAL DO CLIENTE.");
                if (cliente.Tipo == Constantes.Constantes.ID_PJ &&
                cliente.TelComercial == "" && cliente.TelComercial2 == "" ||
                cliente.TelComercial == null && cliente.TelComercial2 == null)
                    listaErros.Add("PREENCHA O TELEFONE.");
                if (cliente.TelComercial != "" && cliente.TelComercial != null)
                {
                    cliente.DddComercial = Util.Util.MontarDDD(cliente.TelComercial);
                    cliente.TelComercial = Util.Util.MontarTelefone(cliente.TelComercial).Replace("-", "").Trim();

                    if (cliente.DddComercial.Length != 2 && cliente.DddComercial != "")
                        listaErros.Add("DDD INVÁLIDO.");
                    if (cliente.TelComercial.Length < 6 && cliente.TelComercial != "")
                        listaErros.Add("TELEFONE COMERCIAL INVÁLIDO.");
                    if (cliente.DddComercial != "" && cliente.TelComercial == "")
                        listaErros.Add("PREENCHA O TELEFONE COMERCIAL.");
                    if (cliente.DddComercial == "" && cliente.TelComercial != "")
                        listaErros.Add("PREENCHA O DDD.");
                }

                if (cliente.TelComercial2 != "" && cliente.TelComercial2 != null)
                {
                    cliente.DddComercial2 = Util.Util.MontarDDD(cliente.TelComercial2);
                    cliente.TelComercial2 = Util.Util.MontarTelefone(cliente.TelComercial2);

                    if (cliente.DddComercial2.Length != 2 && cliente.DddComercial2 != "")
                        listaErros.Add("DDD INVÁLIDO.");
                    if (cliente.TelComercial2.Length < 6 && cliente.TelComercial2 != "")
                        listaErros.Add("TELEFONE COMERCIAL INVÁLIDO.");
                    if (cliente.DddComercial2 != "" && cliente.TelComercial2 == "")
                        listaErros.Add("PREENCHA O TELEFONE COMERCIAL 2.");
                    if (cliente.DddComercial2 == "" && cliente.TelComercial2 != "")
                        listaErros.Add("PREENCHA O DDD.");
                }
            }

            if (cliente.Endereco == "")
                listaErros.Add("PREENCHA O ENDEREÇO.");
            if (cliente.Endereco.Length > Constantes.Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
                listaErros.Add("ENDEREÇO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    cliente.Endereco.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    Constantes.Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");
            if (cliente.Endereco == "")
                listaErros.Add("PREENCHA O NÚMERO DO ENDEREÇO.");
            if (cliente.Bairro == "")
                listaErros.Add("PREENCHA O BAIRRO.");
            if (cliente.Cidade == "")
                listaErros.Add("PREENCHA A CIDADE.");
            if (!Util.Util.VerificaUf(cliente.Uf))
                listaErros.Add("UF INVÁLIDA.");
            if (cliente.Cep == "")
                listaErros.Add("INFORME O CEP.");
            if (!Util.Util.VerificaCep(cliente.Cep))
                listaErros.Add("CEP INVÁLIDO.");


            if (cliente.Ie == "" &&
                Convert.ToString(cliente.Contribuinte_Icms_Status) == Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                listaErros.Add("PREENCHA A INSCRIÇÃO ESTADUAL.");
            if (Convert.ToString(cliente.ProdutorRural) == Constantes.Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                if (cliente.Ie == "" ||
                Convert.ToString(cliente.Contribuinte_Icms_Status) == Constantes.Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                    listaErros.Add("Para ser cadastrado como Produtor Rural, " +
                        "é necessário ser contribuinte do ICMS e possuir nº de IE");

            //string s_tabela_municipios_IBGE = "";
            //parateste: esse bloco esta comentado para podermos testar alterações com um I.E ficticio
            if (cliente.Ie != "" && cliente.Ie != null)
            {
                //afazer: terminar o metodo abaixo
                string uf = VerificarInscricaoEstadualValida(cliente.Ie, cliente.Uf, listaErros);
                List<NfeMunicipio> lstNfeMunicipio = new List<NfeMunicipio>();
                lstNfeMunicipio = (await ConsisteMunicipioIBGE(cliente.Cidade, cliente.Uf, listaErros)).ToList();

            }

            //Não iremos comparar o endereço, pois o úsuario poderá alterar o cadastro do cliente.

            //await VerificarEndereco(cliente, listaErros);

            //return listaErros;
        }

        private List<string> ValidarRefBancaria(List<RefBancariaDtoCliente> lstRefBancaria, List<string> lstErros)
        {
            for (int i = 0; i < lstRefBancaria.Count; i++)
            {
                if (!string.IsNullOrEmpty(lstRefBancaria[i].Banco) &&
                    !string.IsNullOrEmpty(lstRefBancaria[i].Agencia) &&
                    !string.IsNullOrEmpty(lstRefBancaria[i].ContaBanco))
                {
                    if (string.IsNullOrEmpty(lstRefBancaria[i].Banco))
                        lstErros.Add("Ref Bancária (" + lstRefBancaria[i].OrdemBanco.ToString() + "): informe o banco.");
                    if (string.IsNullOrEmpty(lstRefBancaria[i].Agencia))
                        lstErros.Add("Ref Bancária (" + lstRefBancaria[i].OrdemBanco.ToString() + "): informe o agência.");
                    if (string.IsNullOrEmpty(lstRefBancaria[i].ContaBanco))
                        lstErros.Add("Ref Bancária (" + lstRefBancaria[i].OrdemBanco.ToString() + "): informe o número da conta.");
                }
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
        private Task<string> GerarIdCliente(InfraBanco.ContextoBdGravacao dbgravacao, string id_nsu)
        {
            return UtilsGlobais.Util.GerarNsu(dbgravacao, id_nsu);
        }

        private async Task<string> CadastrarDadosClienteDto(InfraBanco.ContextoBdGravacao dbgravacao, DadosClienteCadastroDto clienteDto, string apelido, string log)
        {
            string retorno = "";
            List<string> lstRetorno = new List<string>();
            string id_cliente = await GerarIdCliente(dbgravacao, Constantes.Constantes.NSU_CADASTRO_CLIENTES);

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
                    Usuario_Cadastro = apelido.ToUpper(),
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
                    Tel_Res = clienteDto.TelefoneResidencial?.Trim(),
                    Ddd_Com = clienteDto.DddComercial,
                    Tel_Com = clienteDto.TelComercial?.Trim(),
                    Ramal_Com = clienteDto.Ramal,
                    Contato = clienteDto.Contato?.ToUpper(),
                    Ddd_Com_2 = clienteDto.DddComercial2,
                    Tel_Com_2 = clienteDto.TelComercial2?.Trim(),
                    Ramal_Com_2 = clienteDto.Ramal2,
                    Ddd_Cel = clienteDto.DddCelular,
                    Tel_Cel = clienteDto.Celular,
                    Dt_Nasc = clienteDto.Nascimento,
                    Filiacao = clienteDto.Observacao_Filiacao?.ToUpper(),
                    Obs_crediticias = "",
                    Midia = "",
                    Email = clienteDto.Email,
                    Email_Xml = clienteDto.EmailXml,
                    Dt_Ult_Atualizacao = DateTime.Now,
                    Usuario_Ult_Atualizacao = apelido.ToUpper()
                };

                //Busca os nomes reais das colunas da tabela SQL
                Util.Util.MontaLog(tCliente, log, campos_a_omitir);

                dbgravacao.Add(tCliente);
                await dbgravacao.SaveChangesAsync();
                retorno = tCliente.Id;
            }

            return retorno;
        }

        private async Task AtualizarRefBancaria(InfraBanco.ContextoBdGravacao dbgravacao, string id_cliente, string loja,
            List<RefBancariaDtoCliente> lstRefBancaria, string apelido)
        {
            int qtdeRef = 1;
            string campos_a_omitir_ref_bancaria = "id_cliente|ordem|excluido_status|dt_cadastro|usuario_cadastro";

            //afazer: verificar se é uma inclusão de nova ref

            //buscamos as referencias para poder comparar os dados
            var lstRefBase = from c in dbgravacao.TclienteRefBancarias
                             where c.Id_Cliente == id_cliente
                             select c;
            //montamos a lista para comparar
            List<TclienteRefBancaria> lstRefBancariaBase = new List<TclienteRefBancaria>();
            foreach (TclienteRefBancaria t in await lstRefBase.ToListAsync())
            {
                lstRefBancariaBase.Add(t);
            }

            foreach (RefBancariaDtoCliente r in lstRefBancaria)
            {
                r.OrdemBanco = 1;

                if (!string.IsNullOrEmpty(r.Agencia) && !string.IsNullOrEmpty(r.Banco) && !string.IsNullOrEmpty(r.ContaBanco))
                {

                    if (lstRefBancariaBase.Count > 0)
                    {

                        foreach (TclienteRefBancaria t in lstRefBancariaBase)
                        {
                            if (t.Ordem == r.OrdemBanco)
                            {
                                string ddd = Util.Util.MontarDDD(r.TelefoneBanco);
                                string tel = Util.Util.MontarTelefone(r.TelefoneBanco).Replace("-", "").Trim();

                                if (t.Agencia != r.Agencia ||
                                    t.Banco != r.Banco ||
                                    t.Conta != r.ContaBanco ||
                                    t.Contato != r.ContatoBanco ||
                                    t.Ddd != ddd ||
                                    t.Telefone != tel)
                                {
                                    //alterar status = 1 = excluido para o que esta alterando
                                    t.Excluido_Status = 1;
                                    //deletar o que foi alterado
                                    dbgravacao.Remove(t);

                                    //passar os novos valores para tcliente
                                    TclienteRefBancaria novaRef = new TclienteRefBancaria();
                                    novaRef.Id_Cliente = id_cliente;
                                    novaRef.Banco = r.Banco;
                                    novaRef.Agencia = r.Agencia;
                                    novaRef.Conta = r.ContaBanco;
                                    novaRef.Ddd = Util.Util.MontarDDD(r.TelefoneBanco).Trim();
                                    novaRef.Telefone = Util.Util.MontarTelefone(r.TelefoneBanco).Replace("-", "").Trim();
                                    novaRef.Contato = r.ContatoBanco;
                                    novaRef.Ordem = t.Ordem;//ordem que ja existia
                                    novaRef.Usuario_Cadastro = t.Usuario_Cadastro;//usuario que fez o cadastro
                                    novaRef.Dt_Cadastro = DateTime.Now;
                                    novaRef.Excluido_Status = 0;

                                    dbgravacao.Add(novaRef);

                                    //funcionando a montagem de log de inclusão
                                    string log = Util.Util.MontaLogInclusao(novaRef, campos_a_omitir_ref_bancaria);
                                    log = "Ref Bancária incluída: " + log;
                                    string logexclusao = Util.Util.MontaLogExclusao(t, campos_a_omitir_ref_bancaria);
                                    log = log + "Ref Bancária excluída: " + logexclusao;

                                    //dbgravacao.Remove(t);

                                    bool salvouLog = Util.Util.GravaLog(dbgravacao, apelido, loja, "",
                                        id_cliente, Constantes.Constantes.OP_LOG_CLIENTE_ALTERACAO, log, contextoProvider);

                                    await dbgravacao.SaveChangesAsync();
                                }
                            }
                        }
                    }
                    else
                    {
                        //passar os novos valores para tcliente
                        TclienteRefBancaria novaRef = new TclienteRefBancaria();
                        novaRef.Id_Cliente = id_cliente;
                        novaRef.Banco = r.Banco;
                        novaRef.Agencia = r.Agencia;
                        novaRef.Conta = r.ContaBanco;
                        novaRef.Ddd = Util.Util.MontarDDD(r.TelefoneBanco).Trim();
                        novaRef.Telefone = Util.Util.MontarTelefone(r.TelefoneBanco).Replace("-", "").Trim();
                        novaRef.Contato = r.ContatoBanco;
                        novaRef.Ordem = (short)qtdeRef;
                        novaRef.Usuario_Cadastro = apelido.ToUpper();
                        novaRef.Dt_Cadastro = DateTime.Now;
                        novaRef.Excluido_Status = 0;

                        dbgravacao.Add(novaRef);

                        string log = Util.Util.MontaLogInclusao(novaRef, campos_a_omitir_ref_bancaria);
                        log = "Ref Bancária incluída: " + log;
                        bool salvouLog = Util.Util.GravaLog(dbgravacao, apelido, loja, "",
                            id_cliente, Constantes.Constantes.OP_LOG_CLIENTE_INCLUSAO, log, contextoProvider);

                        await dbgravacao.SaveChangesAsync();
                    }

                    qtdeRef++;
                }
            }

            await dbgravacao.SaveChangesAsync();
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
                    Conta = r.ContaBanco,
                    Dt_Cadastro = DateTime.Now,
                    Usuario_Cadastro = apelido,
                    Ordem = (short)(qtdeRef),
                    Ddd = r.DddBanco,
                    Telefone = r.TelefoneBanco,
                    Contato = r.ContatoBanco,
                    Excluido_Status = 0
                };
                dbgravacao.Add(cliRefBancaria);
                qtdeRef++;

                //Busca os nomes reais das colunas da tabela SQL
                log = Util.Util.MontaLog(cliRefBancaria, log, campos_a_omitir_ref_bancaria);
            }

            await dbgravacao.SaveChangesAsync();
            return log;
        }

        private async Task AtualizarRefComercial(InfraBanco.ContextoBdGravacao dbgravacao, string id_cliente, string loja,
            List<RefComercialDtoCliente> lstRefComercial, string apelido)
        {
            int qtdeRef = 1;

            string campos_a_omitir_ref_comercial = "id_cliente|ordem|excluido_status|dt_cadastro|usuario_cadastro";

            //buscar os dados na base para comparar
            var lstRefComercialTask = from c in dbgravacao.TclienteRefComercials
                                      where c.Id_Cliente == id_cliente
                                      select c;

            List<TclienteRefComercial> lstRefComercialBase = new List<TclienteRefComercial>();
            foreach (var c in await lstRefComercialTask.ToListAsync())
            {
                lstRefComercialBase.Add(c);
            }

            foreach (RefComercialDtoCliente r in lstRefComercial)
            {
                r.Ordem = qtdeRef;

                if (!string.IsNullOrEmpty(r.Nome_Empresa) && !string.IsNullOrEmpty(r.Contato))
                {
                    if (lstRefComercialBase.Count > 0)
                    {
                        foreach (TclienteRefComercial t in lstRefComercialBase)
                        {
                            if (t.Ordem == r.Ordem)
                            {
                                string ddd = Util.Util.MontarDDD(r.Telefone);
                                string tel = Util.Util.MontarTelefone(r.Telefone).Replace("-", "").Trim();
                                //verificar se teve alteração em algum campo
                                if (r.Nome_Empresa != t.Nome_Empresa ||
                                    r.Contato != t.Contato ||
                                    r.Ddd != ddd ||
                                    r.Telefone != tel)
                                {
                                    //alterar o status do que esta sendo verificado
                                    t.Excluido_Status = 1;
                                    //remover o que teve alteração
                                    dbgravacao.Remove(t);

                                    //criar uma nova instancia comercial
                                    TclienteRefComercial novaRef = new TclienteRefComercial();
                                    novaRef.Id_Cliente = id_cliente;
                                    novaRef.Nome_Empresa = r.Nome_Empresa;
                                    novaRef.Contato = r.Contato;
                                    novaRef.Ddd = ddd;
                                    novaRef.Telefone = tel;
                                    novaRef.Dt_Cadastro = DateTime.Now;
                                    novaRef.Usuario_Cadastro = t.Usuario_Cadastro;
                                    novaRef.Ordem = t.Ordem;
                                    novaRef.Excluido_Status = 0;
                                    //add na base
                                    dbgravacao.Add(novaRef);

                                    //montar o log de inclusão 
                                    string log = Util.Util.MontaLogInclusao(novaRef, campos_a_omitir_ref_comercial);
                                    log = "Ref Comercial incluída: " + log;
                                    //montar o log de exclusão
                                    string logexclusao = Util.Util.MontaLogExclusao(t, campos_a_omitir_ref_comercial);
                                    log = log + "Ref Comercial excluída: " + logexclusao;
                                    //gravar log
                                    bool salvouLog = Util.Util.GravaLog(dbgravacao, apelido, loja, "",
                                        id_cliente, Constantes.Constantes.OP_LOG_CLIENTE_ALTERACAO, log, contextoProvider);
                                    //fazer o savechanges
                                    await dbgravacao.SaveChangesAsync();
                                }
                            }
                        }


                    }
                    else
                    {
                        TclienteRefComercial novaRef = new TclienteRefComercial();
                        novaRef.Id_Cliente = id_cliente;
                        novaRef.Nome_Empresa = r.Nome_Empresa;
                        novaRef.Contato = r.Contato;
                        novaRef.Ddd = Util.Util.MontarDDD(r.Telefone);
                        novaRef.Telefone = Util.Util.MontarTelefone(r.Telefone).Replace("-", "").Trim();
                        novaRef.Ordem = (short)qtdeRef;
                        novaRef.Dt_Cadastro = DateTime.Now;
                        novaRef.Usuario_Cadastro = apelido.ToUpper();
                        novaRef.Excluido_Status = 0;

                        dbgravacao.Add(novaRef);

                        string log = Util.Util.MontaLogInclusao(novaRef, campos_a_omitir_ref_comercial);
                        log = "Ref Bancária incluída: " + log;
                        bool salvouLog = Util.Util.GravaLog(dbgravacao, apelido, loja, "",
                            id_cliente, Constantes.Constantes.OP_LOG_CLIENTE_INCLUSAO, log, contextoProvider);

                        await dbgravacao.SaveChangesAsync();
                    }
                }
                qtdeRef++;
            }

            await dbgravacao.SaveChangesAsync();
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
                log = Util.Util.MontaLog(c, log, campos_a_omitir_ref_comercial);
            }

            await dbgravacao.SaveChangesAsync();
            return log;
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
                    retorno = "Preencha a IE (Inscrição Estadual) com um número válido!!" +
                            "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE.";
                else
                    retorno = ie;
            }
            else
            {
                retorno = ie;
            }

            blnResultado = isInscricaoEstadualOkCom(ie, uf);
            if (!blnResultado)
            {
                listaErros.Add("Preencha a IE (Inscrição Estadual) com um número válido!!" +
                            "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE.");
            }

            return retorno;
        }

        private bool isInscricaoEstadualOkCom(string ie, string uf)
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

        public async Task<IEnumerable<NfeMunicipio>> BuscarSiglaUf(string uf, string municipio)
        {
            //verificar se passo a lista de erros
            string retorno = "";
            List<NfeMunicipio> lstNfeMunicipio = new List<NfeMunicipio>();

            var db = contextoNFeProvider.GetContextoLeitura();

            var nfeUFTask = from c in db.NfeUfs
                            where c.SiglaUF == uf.ToUpper()
                            select c;


            NfeUf nfeUf = await nfeUFTask.FirstOrDefaultAsync();

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

        //private async Task VerificarEndereco(DadosClienteCadastroDto cliente, List<string> listaErros)
        //{
        //    CepBll.CepBll cep = new CepBll.CepBll(contextoCepProvider);
        //    List<CepDto> cepDto = new List<CepDto>();
        //    string cepSoDigito = cliente.Cep.Replace(".", "").Replace("-", "");
        //    cepDto = (await cep.BuscarPorCep(cepSoDigito)).ToList();
        //    foreach (var c in cepDto)
        //    {
        //        if (c.Cep != cepSoDigito)
        //            listaErros.Add("Número do Cep diferente!");
        //        if (c.Endereco != cliente.Endereco ||
        //            c.Bairro != cliente.Bairro ||
        //            c.Cidade != cliente.Cidade ||
        //            c.Uf != cliente.Uf)
        //            listaErros.Add("Os dados informados estão divergindo da base de dados!");
        //    }
        //}

        public async Task<IEnumerable<string>> BuscarListaPedidosBonshop(string cpf_cnpj)
        {
            var db = contextoProvider.GetContextoLeitura();

            List<string> lstRetorno = new List<string>();

            var lstRetornoTask = from c in (from c in db.Tpedidos.Include(x => x.Tcliente)
                                            where c.Tcliente.Cnpj_Cpf == cpf_cnpj &&
                                                  c.St_Entrega == Constantes.Constantes.ST_ENTREGA_ENTREGUE
                                            select c).ToList()
                                 orderby new { c.Pedido } descending
                                 select c.Pedido;
            if (lstRetornoTask != null)
            {
                lstRetorno = (lstRetornoTask).ToList();
            }

            return await Task.FromResult(lstRetorno);

        }

    }
}
