﻿using Cliente.Dados;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.EntityFrameworkCore;
using Orcamento.Dto;
using Prepedido.Dto;
using Prepedido.PedidoVisualizacao.Dados;
using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilsGlobais;

namespace Prepedido.PedidoVisualizacao
{
    public class PedidoVisualizacaoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public PedidoVisualizacaoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }


        public async Task<IEnumerable<string>> ListarNumerosPedidoCombo(string apelido)
        {

            using (var db = contextoProvider.GetContextoLeitura())
            {

                var lista = from p in db.Tpedido
                            where p.Orcamentista == apelido &&
                                  p.Data >= Util.LimiteDataBuscas()
                            orderby p.Pedido
                            select p.Pedido;

                var res = lista.AsEnumerable();
                return await Task.FromResult(res);
            }
        }

        public enum TipoBuscaPedido
        {
            Todos = 0, PedidosEncerrados = 1, PedidosEmAndamento = 2
        }

        public async Task<IEnumerable<PedidosPedidoDados>> ListarPedidos(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            if (dataInicial < Util.LimiteDataBuscas())
            {
                dataInicial = Util.LimiteDataBuscas();
            }
            //fazemos a busca
            var ret = await ListarPedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPedido, dataInicial, dataFinal);

            //se tiver algum registro, retorna imediatamente
            if (ret.Any())
                return ret;

            /*
             * se fizeram a busca por algum CPF ou CNPJ ou pedido e não achamos nada, removemos o filtro de datas
             * para não aparcer para o usuário que não tem nenhum registro
             * */
            if (String.IsNullOrEmpty(clienteBusca) && String.IsNullOrEmpty(numeroPedido))
                return ret;

            //busca sem data final
            ret = await ListarPedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPedido, dataInicial, null);
            if (ret.Any())
                return ret;

            //ainda não achamos nada? então faz a busca sem filtrar por tipo
            ret = await ListarPedidosFiltroEstrito(apelido, TipoBuscaPedido.Todos, clienteBusca, numeroPedido, dataInicial, null);
            return ret;
        }

        public PedidoConsultaDto ListarPedidosPorFiltro(TorcamentoFiltro filtro)
        {
            try
            {
                using (var db = contextoProvider.GetContextoLeitura())
                {
                    var paraterQuery = (from p in db.TcfgParametro
                                        join unp in db.TcfgUnidadeNegocioParametro on p.Id equals unp.IdCfgParametro
                                        where p.Id == 20
                                        select unp.Valor).FirstOrDefault();

                    if (filtro.DtInicio.HasValue && filtro.DtFim.HasValue)
                    {
                        if (filtro.DtFim.Value.Date < filtro.DtInicio.Value.Date)
                        {
                            return new PedidoConsultaDto()
                            {
                                Sucesso = false,
                                Mensagem = "Data 'Início da criação' não deve ser menor que data 'Fim da criação'!",
                                OrcamentoCotacaoLista = new List<OrcamentoCotacaoListaDto>(),
                                QtdeRegistros = 0
                            };
                        }

                        TimeSpan difference = filtro.DtFim.Value.Date - filtro.DtInicio.Value.Date;

                        if (difference.Days > Convert.ToInt32(paraterQuery))
                        {
                            return new PedidoConsultaDto()
                            {
                                Sucesso = false,
                                Mensagem = $"Número máximo de dias para o intervalo da consulta deve ser menor ou igual {paraterQuery}",
                                OrcamentoCotacaoLista = new List<OrcamentoCotacaoListaDto>(),
                                QtdeRegistros = 0
                            };
                        }
                    }

                    var query = from c in db.Tpedido

                                join v in db.Tusuario on c.Vendedor equals v.Usuario into vendedorTemp
                                from vendedor in vendedorTemp.DefaultIfEmpty()

                                join vp in db.TorcamentistaEindicadorVendedor on c.IdIndicadorVendedor equals vp.Id into gj
                                from loj in gj.DefaultIfEmpty()

                                where c.St_Entrega != "CAN" //CANCELADOS

                                select new OrcamentoCotacaoListaDto
                                {
                                    //NumPedidoOrdenacao = Convert.ToInt32(c.Pedido.Replace("N", "")),
                                    NumeroOrcamento = c.IdOrcamentoCotacao.HasValue ? c.IdOrcamentoCotacao.Value.ToString() : "-",
                                    NumPedido = c.Pedido,
                                    Cliente_Obra = c.Endereco_nome,
                                    Loja = c.Loja,
                                    IdVendedor = vendedor.Id,
                                    Vendedor = c.Vendedor,
                                    Parceiro = !String.IsNullOrEmpty(c.Indicador) ? c.Indicador : "-",
                                    VendedorParceiro = loj.Nome,
                                    Valor = db.TpedidoItem.Where(x => x.Pedido == c.Pedido).Select(x => new TpedidoItem() { Preco_Venda = x.Preco_Venda, Qtde = x.Qtde }).Sum(x => x.Preco_Venda * x.Qtde.Value).ToString(),
                                    Orcamentista = c.Orcamentista,
                                    IdStatus = 0,
                                    St_Entrega = c.St_Entrega,
                                    Status = (from s in db.TcodigoDescricao where s.Codigo == c.St_Entrega select s).FirstOrDefault().Descricao,
                                    VistoEm = "",
                                    IdIndicadorVendedor = c.IdIndicadorVendedor,
                                    Mensagem = "",
                                    DtCadastro = c.Data,
                                    DtExpiracao = null,
                                    DtInicio = filtro.DtInicio,
                                    DtFim = filtro.DtFim
                                };

                    #region Where

                    if (!string.IsNullOrEmpty(filtro.IdBaseBusca))
                    {
                        query = query.Where(x => Convert.ToInt32(x.NumPedido.Substring(0, 6)) <= Convert.ToInt32(filtro.IdBaseBusca.Substring(0, 6)));
                    }
                    if (filtro.Status != null && filtro.Status.Length > 0)
                    {
                        query = query.Where(f => filtro.Status.Contains(f.St_Entrega));
                    }

                    if (!string.IsNullOrWhiteSpace(filtro.Nome_numero))
                    {
                        int aux = 0;

                        if (int.TryParse(filtro.Nome_numero, out aux))
                        {
                            query = query.Where(f => f.NumeroOrcamento.Contains(aux.ToString()));
                        }
                        else
                        {
                            query = query.Where(f =>
                            f.Cliente_Obra.Contains(filtro.Nome_numero)
                            || f.NumPedido.Contains(filtro.Nome_numero));
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(filtro.Loja))
                    {
                        query = query.Where(f => f.Loja == filtro.Loja);
                    }

                    if (filtro.Vendedores != null && filtro.Vendedores.Length > 0)
                    {
                        query = query.Where(f => filtro.Vendedores.Contains(f.IdVendedor.ToString()));
                    }

                    if (!string.IsNullOrWhiteSpace(filtro.Vendedor))
                    {
                        query = query.Where(f => f.Vendedor == filtro.Vendedor);
                    }

                    if (filtro.Parceiros != null && filtro.Parceiros.Length > 0)
                    {
                        query = query.Where(f => filtro.Parceiros.Contains(f.Parceiro));
                    }

                    if (!string.IsNullOrEmpty(filtro.Parceiro))
                    {
                        query = query.Where(x => x.Parceiro == filtro.Parceiro);
                    }

                    if (filtro.VendedorParceiros != null && filtro.VendedorParceiros.Count() > 0)
                    {
                        query = query.Where(x => filtro.VendedorParceiros.Contains(x.IdIndicadorVendedor.ToString()));
                    }

                    if (!string.IsNullOrEmpty(filtro.VendedorParceiro))
                    {
                        query = query.Where(x => x.VendedorParceiro == filtro.VendedorParceiro);
                    }

                    if (filtro.DtInicio.HasValue)
                    {
                        query = query.Where(f => f.DtCadastro.Value.Date >= filtro.DtInicio.Value.Date);
                    }

                    if (filtro.DtFim.HasValue)
                    {
                        query = query.Where(f => f.DtCadastro.Value.Date <= filtro.DtFim.Value.Date);
                    }

                    #endregion

                    #region Ordenação

                    if (!string.IsNullOrWhiteSpace(filtro.NomeColunaOrdenacao))
                    {
                        switch (filtro.NomeColunaOrdenacao.ToUpper())
                        {
                            case "NUMPEDIDO":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.NumPedido);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.NumPedido);
                                }
                                break;
                            case "NUMEROORCAMENTO":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.NumeroOrcamento);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.NumeroOrcamento);
                                }
                                break;
                            case "CLIENTE_OBRA":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.Cliente_Obra);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.Cliente_Obra);
                                }
                                break;

                            case "VENDEDOR":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.Vendedor);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.Vendedor);
                                }
                                break;

                            case "PARCEIRO":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.Parceiro);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.Parceiro);
                                }
                                break;

                            case "VENDEDORPARCEIRO":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.VendedorParceiro);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.VendedorParceiro);
                                }
                                break;

                            case "STATUS":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.Status);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.Status);
                                }
                                break;

                            case "DTCADASTRO":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => o.DtCadastro);
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => o.DtCadastro);
                                }
                                break;
                            case "VALOR":
                                if (filtro.OrdenacaoAscendente)
                                {
                                    query = query.OrderBy(o => Convert.ToDecimal(o.Valor));
                                }
                                else
                                {
                                    query = query.OrderByDescending(o => Convert.ToDecimal(o.Valor));
                                }
                                break;
                        }
                    }
                    else
                    {
                        query = query.OrderByDescending(o => o.DtCadastro);
                    }

                    #endregion

                    #region Paginação

                    int qtdeRegistros = query.Count();

                    if (filtro.Exportar)
                    {
                        filtro.QtdeItensPagina = qtdeRegistros;
                    }

                    query = query
                            .Skip((filtro.Pagina) * filtro.QtdeItensPagina)
                            .Take(filtro.QtdeItensPagina);

                    #endregion

                    var result = query.ToList();

                    return new PedidoConsultaDto()
                    {
                        Sucesso = true,
                        OrcamentoCotacaoLista = result,
                        QtdeRegistros = qtdeRegistros
                    };
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //a busca sem malabarismos para econtrar algum registro
        private async Task<IEnumerable<PedidosPedidoDados>> ListarPedidosFiltroEstrito(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {

                //inclui um filtro para não trazer os filhos na listagem do pedido
                var lista = db.Tpedido.
                    Where(r => r.Indicador == apelido);

                switch (tipoBusca)
                {
                    case TipoBuscaPedido.Todos:
                        //SE TIVER QUE INCLUIR OS PEDIDO CANCELADOS É SÓ DESCOMENTAR A LINHA ABAIXO
                        //lista = lista.Where(r => r.St_Entrega != "CAN");
                        break;
                    case TipoBuscaPedido.PedidosEncerrados:
                        lista = lista.Where(r => r.St_Entrega == "ETG" || r.St_Entrega == "CAN");
                        break;
                    case TipoBuscaPedido.PedidosEmAndamento:
                        lista = lista.Where(r => r.St_Entrega != "ETG" && r.St_Entrega != "CAN");
                        break;
                }

                if (!string.IsNullOrEmpty(clienteBusca))
                    lista = lista.Where(r => r.Tcliente.Cnpj_Cpf.Contains(clienteBusca));
                if (!string.IsNullOrEmpty(numeroPedido))
                    lista = lista.Where(r => r.Pedido.Contains(numeroPedido));
                if (dataInicial.HasValue)
                    lista = lista.Where(r => r.Data >= dataInicial.Value);
                if (dataFinal.HasValue)
                    lista = lista.Where(r => r.Data <= dataFinal.Value);

                List<PedidosPedidoDados> lst_pedido = new List<PedidosPedidoDados>();

                //precisa calcular os itens de cada produto referente ao pedido, 
                //para trazer o valor total do pedido e não o total da familia
                foreach (var i in await lista.ToListAsync())
                {
                    var itens = from c in db.TpedidoItem
                                where c.Pedido == i.Pedido
                                select c;

                    if (itens != null)
                    {
                        string nome = await (from c in itens
                                             where c.Pedido == i.Pedido
                                             select c.Tpedido.Endereco_nome).FirstOrDefaultAsync();

                        lst_pedido.Add(new PedidosPedidoDados
                        {
                            NomeCliente = nome,
                            NumeroPedido = i.Pedido,
                            DataPedido = i.Data,
                            Status = i.St_Entrega,
                            ValorTotal = await itens.SumAsync(x => x.Preco_Venda * x.Qtde)
                        });
                    }
                }


                //colocar as mensagens de status
                var listaComStatus = lst_pedido;
                foreach (var pedido in listaComStatus)
                {
                    if (pedido.Status == "ESP")
                        pedido.Status = "Em espera";
                    if (pedido.Status == "SPL")
                        pedido.Status = "Split Possível";
                    if (pedido.Status == "SEP")
                        pedido.Status = "Separar Mercadoria";
                    if (pedido.Status == "AET")
                        pedido.Status = "A entregar";
                    if (pedido.Status == "ETG")
                        pedido.Status = "Entregue";
                    if (pedido.Status == "CAN")
                        pedido.Status = "Cancelado";
                }
                return await Task.FromResult(listaComStatus.OrderByDescending(x => x.NumeroPedido));
            }
        }

        public async Task<IEnumerable<string>> ListarCpfCnpjPedidosCombo(string apelido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {

                var lista = (from c in db.Tpedido
                             where c.Orcamentista == apelido &&
                                   c.Data >= Util.LimiteDataBuscas()
                             orderby c.Tcliente.Cnpj_Cpf
                             select c.Tcliente.Cnpj_Cpf).Distinct();

                var ret = await lista.Distinct().ToListAsync();
                List<string> cpfCnpjFormat = new List<string>();

                foreach (string cpf in ret)
                {
                    cpfCnpjFormat.Add(Util.FormatCpf_Cnpj_Ie(cpf));
                }

                return cpfCnpjFormat;
            }
        }

        private async Task<DadosClienteCadastroDados> ObterDadosCliente(Tpedido pedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var dadosCliente = from c in db.Tcliente
                                   where c.Id == pedido.Id_Cliente
                                   select c;
                var cli = await dadosCliente.FirstOrDefaultAsync();
                DadosClienteCadastroDados cadastroCliente = new DadosClienteCadastroDados
                {
                    Loja = await ObterRazaoSocial_Nome_Loja(pedido.Loja),
                    Indicador_Orcamentista = pedido.Orcamentista,
                    Vendedor = pedido.Vendedor,
                    Id = cli.Id,
                    Cnpj_Cpf = Util.FormatCpf_Cnpj_Ie(cli.Cnpj_Cpf),
                    Rg = cli.Rg,
                    Ie = Util.FormatCpf_Cnpj_Ie(cli.Ie),
                    Contribuinte_Icms_Status = cli.Contribuinte_Icms_Status,
                    Tipo = cli.Tipo,
                    Observacao_Filiacao = cli.Filiacao,
                    Nome = cli.Nome,
                    ProdutorRural = cli.Produtor_Rural_Status,
                    DddResidencial = cli.Ddd_Res,
                    TelefoneResidencial = cli.Tel_Res,
                    DddComercial = cli.Ddd_Com,
                    TelComercial = cli.Tel_Com,
                    Ramal = cli.Ramal_Com,
                    DddCelular = cli.Ddd_Cel,
                    Celular = cli.Tel_Cel,
                    TelComercial2 = cli.Tel_Com_2,
                    DddComercial2 = cli.Ddd_Com_2,
                    Ramal2 = cli.Ramal_Com_2,
                    Email = cli.Email,
                    EmailXml = cli.Email_Xml,
                    Endereco = cli.Endereco,
                    Complemento = cli.Endereco_Complemento,
                    Numero = cli.Endereco_Numero,
                    Bairro = cli.Bairro,
                    Cidade = cli.Cidade,
                    Uf = cli.Uf,
                    Cep = cli.Cep,
                    Contato = cli.Contato
                };
                return cadastroCliente;
            }
        }

        private async Task<DadosClienteCadastroDados> ObterDadosClientePedido(Tpedido pedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var dadosCliente = from c in db.Tcliente
                                   where c.Id == pedido.Id_Cliente
                                   select c;
                var cli = await dadosCliente.FirstOrDefaultAsync();
                DadosClienteCadastroDados cadastroCliente = new DadosClienteCadastroDados
                {
                    Loja = await ObterRazaoSocial_Nome_Loja(pedido.Loja),
                    Indicador_Orcamentista = pedido.Indicador,
                    Vendedor = pedido.Vendedor,
                    Id = cli.Id,
                    Cnpj_Cpf = Util.FormatCpf_Cnpj_Ie(pedido.Endereco_cnpj_cpf),
                    Rg = pedido.Endereco_rg,
                    Ie = Util.FormatCpf_Cnpj_Ie(pedido.Endereco_ie),
                    Contribuinte_Icms_Status = pedido.Endereco_contribuinte_icms_status,
                    Tipo = cli.Tipo,
                    Observacao_Filiacao = cli.Filiacao,
                    Nome = pedido.Endereco_nome,
                    ProdutorRural = pedido.Endereco_produtor_rural_status,
                    DddResidencial = pedido.Endereco_ddd_res,
                    TelefoneResidencial = pedido.Endereco_tel_res,
                    DddComercial = pedido.Endereco_ddd_com,
                    TelComercial = pedido.Endereco_tel_com,
                    Ramal = pedido.Endereco_ramal_com,
                    DddCelular = pedido.Endereco_ddd_cel,
                    Celular = pedido.Endereco_tel_cel,
                    TelComercial2 = pedido.Endereco_tel_com_2,
                    DddComercial2 = pedido.Endereco_ddd_com_2,
                    Ramal2 = pedido.Endereco_ramal_com_2,
                    Email = pedido.Endereco_email,
                    EmailXml = pedido.Endereco_email_xml,
                    Endereco = pedido.Endereco_logradouro,
                    Complemento = pedido.Endereco_complemento,
                    Numero = pedido.Endereco_numero,
                    Bairro = pedido.Endereco_bairro,
                    Cidade = pedido.Endereco_cidade,
                    Uf = pedido.Endereco_uf,
                    Cep = pedido.Endereco_cep,
                    Contato = pedido.Endereco_contato
                };
                return cadastroCliente;
            }
        }

        public async Task<string> ObterRazaoSocial_Nome_Loja(string loja)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {

                string retorno = "";

                Tloja lojaTask = await (from c in db.Tloja
                                        where c.Loja == loja
                                        select c).SingleOrDefaultAsync();

                if (lojaTask != null)
                {
                    if (!string.IsNullOrEmpty(lojaTask.Razao_Social))
                        retorno = lojaTask.Razao_Social;
                    else
                        retorno = lojaTask.Nome;
                }

                return retorno;
            }
        }

        private async Task<EnderecoEntregaClienteCadastroDados> ObterEnderecoEntrega(Tpedido p)
        {
            EnderecoEntregaClienteCadastroDados enderecoEntrega = new EnderecoEntregaClienteCadastroDados();

            enderecoEntrega.OutroEndereco = !string.IsNullOrEmpty(p.EndEtg_Cod_Justificativa) ? true : false;
            enderecoEntrega.EndEtg_endereco = p.EndEtg_Endereco;
            enderecoEntrega.EndEtg_endereco_numero = p.EndEtg_Endereco_Numero;
            enderecoEntrega.EndEtg_endereco_complemento = p.EndEtg_Endereco_Complemento;
            enderecoEntrega.EndEtg_bairro = p.EndEtg_Bairro;
            enderecoEntrega.EndEtg_cidade = p.EndEtg_Cidade;
            enderecoEntrega.EndEtg_uf = p.EndEtg_UF;
            enderecoEntrega.EndEtg_cep = p.EndEtg_Cep;
            enderecoEntrega.EndEtg_cod_justificativa = p.EndEtg_Cod_Justificativa;
            enderecoEntrega.EndEtg_descricao_justificativa = await Util.ObterDescricao_Cod(
                Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA, p.EndEtg_Cod_Justificativa, contextoProvider);
            enderecoEntrega.EndEtg_email = p.EndEtg_email;
            enderecoEntrega.EndEtg_email_xml = p.EndEtg_email_xml;
            enderecoEntrega.EndEtg_nome = p.EndEtg_nome;
            enderecoEntrega.EndEtg_ddd_res = p.EndEtg_ddd_res;
            enderecoEntrega.EndEtg_tel_res = p.EndEtg_tel_res;
            enderecoEntrega.EndEtg_ddd_com = p.EndEtg_ddd_com;
            enderecoEntrega.EndEtg_tel_com = p.EndEtg_tel_com;
            enderecoEntrega.EndEtg_ramal_com = p.EndEtg_ramal_com;
            enderecoEntrega.EndEtg_ddd_cel = p.EndEtg_ddd_cel;
            enderecoEntrega.EndEtg_tel_cel = p.EndEtg_tel_cel;
            enderecoEntrega.EndEtg_ddd_com_2 = p.EndEtg_ddd_com_2;
            enderecoEntrega.EndEtg_tel_com_2 = p.EndEtg_tel_com_2;
            enderecoEntrega.EndEtg_ramal_com_2 = p.EndEtg_ramal_com_2;
            enderecoEntrega.EndEtg_tipo_pessoa = p.EndEtg_tipo_pessoa;
            enderecoEntrega.EndEtg_cnpj_cpf = p.EndEtg_cnpj_cpf;
            enderecoEntrega.EndEtg_contribuinte_icms_status = p.EndEtg_contribuinte_icms_status;
            enderecoEntrega.EndEtg_produtor_rural_status = p.EndEtg_produtor_rural_status;
            enderecoEntrega.EndEtg_ie = p.EndEtg_ie;
            enderecoEntrega.EndEtg_rg = p.EndEtg_rg;
            enderecoEntrega.St_memorizacao_completa_enderecos = p.St_memorizacao_completa_enderecos == 1;

            return enderecoEntrega;
        }

        private async Task<IEnumerable<PedidoProdutosPedidoDados>> ObterProdutos(string numPedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {

                List<TpedidoItem> produtosItens = await (from c in db.TpedidoItem
                                                         where c.Pedido == numPedido
                                                         orderby c.Sequencia
                                                         select c).ToListAsync();

                List<PedidoProdutosPedidoDados> lstProduto = new List<PedidoProdutosPedidoDados>();

                int qtde_sem_presenca = 0;
                int qtde_vendido = 0;
                short faltante = 0;

                foreach (var c in produtosItens)
                {
                    qtde_sem_presenca = await VerificarEstoqueSemPresenca(numPedido, c.Fabricante, c.Produto);
                    qtde_vendido = await VerificarEstoqueVendido(numPedido, c.Fabricante, c.Produto);

                    if (qtde_sem_presenca != 0)
                        faltante = (short)qtde_sem_presenca;

                    PedidoProdutosPedidoDados produto = new PedidoProdutosPedidoDados
                    {
                        Fabricante = c.Fabricante,
                        Produto = c.Produto,
                        Descricao = c.Descricao_Html,
                        Qtde = c.Qtde,
                        Faltando = faltante,
                        CorFaltante = ObterCorFaltante((int)c.Qtde, qtde_vendido, qtde_sem_presenca),
                        Preco_NF = c.Preco_NF,
                        Preco_Lista = c.Preco_Lista,
                        Desc_Dado = c.Desc_Dado,
                        VlTotalItem = c.Qtde * c.Preco_Venda,
                        VlTotalItemComRA = c.Qtde * c.Preco_NF,
                        Preco_Venda = c.Preco_Venda,
                        Comissao = c.Comissao
                    };

                    lstProduto.Add(produto);
                }

                return lstProduto;
            }
        }

        public async Task<PedidoDados> BuscarPedido(string apelido, string numPedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {

                //vamos verificar se o pedido é o pai 057581N
                var pedido_pai = "";

                if (numPedido.Contains('-'))
                {
                    string[] split = numPedido.Split('-');
                    pedido_pai = split[0];
                }
                else
                {
                    pedido_pai = numPedido;
                }

                var pedido = from c in db.Tpedido
                             where c.Pedido.Contains(pedido_pai)
                             select c;

                //aqui vamos montar o tpedido pai para montar alguns detalhes que contém somente nele
                Tpedido tPedidoPai = await pedido.Select(x => x).Where(x => x.Pedido == pedido_pai).FirstOrDefaultAsync();

                //aqui vamos montar o pedido conforme o número do pedido que veio na entrada
                Tpedido p = pedido
                    .Where(x => x.Pedido == numPedido)
                    .FirstOrDefault();

                if (p == null)
                    return null;

                DadosClienteCadastroDados dadosCliente = new DadosClienteCadastroDados();
                if (p.St_memorizacao_completa_enderecos == 0)
                {
                    dadosCliente = await ObterDadosCliente(p);
                }
                else
                {
                    dadosCliente = await ObterDadosClientePedido(p);
                }

                var enderecoEntregaTask = await ObterEnderecoEntrega(p);
                var lstProdutoTask = await ObterProdutos(numPedido);

                var vlTotalDestePedidoComRATask = lstProdutoTask.Select(r => r.VlTotalItemComRA).Sum();
                var vlTotalDestePedidoTask = lstProdutoTask.Select(r => r.VlTotalItem).Sum();

                //buscar valor total de devoluções NF
                var vlDevNf = from c in db.TpedidoItemDevolvido
                              where c.Pedido.StartsWith(pedido_pai)
                              select c.Qtde * c.Preco_NF;
                var vl_TotalFamiliaDevolucaoPrecoNFTask = vlDevNf.Select(r => r.Value).SumAsync();

                var vlFamiliaParcelaRATask = await CalculaTotalFamiliaRA(pedido_pai);

                string garantiaIndicadorStatus = Convert.ToString(p.GarantiaIndicadorStatus);
                if (garantiaIndicadorStatus == Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO)
                    garantiaIndicadorStatus = "NÃO";
                if (garantiaIndicadorStatus == Constantes.COD_GARANTIA_INDICADOR_STATUS__SIM)
                    garantiaIndicadorStatus = "SIM";

                DetalhesNFPedidoPedidoDados detalhesNf = new DetalhesNFPedidoPedidoDados();
                detalhesNf.Observacoes = p.Obs_1;
                detalhesNf.ConstaNaNF = p.Nfe_Texto_Constar;
                detalhesNf.XPed = p.Nfe_XPed;
                detalhesNf.NumeroNF = tPedidoPai.Obs_2;//consta somente no pai
                detalhesNf.NFSimples = tPedidoPai.Obs_3;

                string nomeUsuario = null;
                if (tPedidoPai.PrevisaoEntregaIdTipoUsuarioContexto == (short)Constantes.TipoUsuarioContexto.UsuarioInterno)
                {
                    nomeUsuario = (from c in db.Tusuario
                                   where c.Id == tPedidoPai.PrevisaoEntregaIdUsuarioUltAtualiz
                                   select c.Usuario).FirstOrDefault();
                }
                if (tPedidoPai.PrevisaoEntregaIdTipoUsuarioContexto == (short)Constantes.TipoUsuarioContexto.Parceiro)
                {
                    nomeUsuario = (from c in db.TorcamentistaEindicador
                                   where c.IdIndicador == tPedidoPai.PrevisaoEntregaIdUsuarioUltAtualiz
                                   select c.Apelido).FirstOrDefault();
                }
                if (tPedidoPai.PrevisaoEntregaIdTipoUsuarioContexto == (short)Constantes.TipoUsuarioContexto.VendedorParceiro)
                {
                    //buscar na t_orcamentista_indicador_vendedor nome amigável
                    var nomeCompleto = (from c in db.TorcamentistaEindicadorVendedor
                                        where c.Id == tPedidoPai.PrevisaoEntregaIdUsuarioUltAtualiz
                                        select c.Nome).FirstOrDefault();
                    nomeUsuario = $"[VP] {nomeCompleto.Split(" ")[0]}";
                }
                if (string.IsNullOrEmpty(nomeUsuario))
                {
                    nomeUsuario = tPedidoPai.PrevisaoEntregaUsuarioUltAtualiz;
                }

                detalhesNf.PrevisaoEntrega = tPedidoPai.St_Etg_Imediata == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO ?
                    tPedidoPai.PrevisaoEntregaData?.ToString("dd/MM/yyyy") + " ("
                    + Texto.iniciaisEmMaiusculas(nomeUsuario) +
                    " - " + tPedidoPai.PrevisaoEntregaData?.ToString("dd/MM/yyyy") + " "
                    + tPedidoPai.PrevisaoEntregaDtHrUltAtualiz?.ToString("HH:mm") + ")" : null;

                detalhesNf.EntregaImediata = ObterEntregaImediata(p);
                detalhesNf.StBemUsoConsumo = p.StBemUsoConsumo;
                detalhesNf.InstaladorInstala = tPedidoPai.InstaladorInstalaStatus;
                detalhesNf.GarantiaIndicadorStatus = garantiaIndicadorStatus;//esse campo não esta mais sendo utilizado

                //verifica o status da entrega
                DateTime? dataEntrega = new DateTime();
                if (tPedidoPai.St_Entrega == Constantes.ST_ENTREGA_A_ENTREGAR || tPedidoPai.St_Entrega == Constantes.ST_ENTREGA_SEPARAR)
                    dataEntrega = tPedidoPai.A_Entregar_Data_Marcada;

                var perdas = await BuscarPerdas(numPedido);
                var TranspNomeTask = await ObterNomeTransportadora(p.Transportadora_Id);
                var lstFormaPgtoTask = await ObterFormaPagto(tPedidoPai);
                var analiseCreditoTask = DescricaoAnaliseCreditoCadastroPedido(Convert.ToString(tPedidoPai.Analise_Credito), true, numPedido, apelido);
                string corAnalise = CorAnaliseCredito(Convert.ToString(tPedidoPai.Analise_Credito));
                string corStatusPagto = CorSatusPagto(tPedidoPai.St_Pagto);
                var saldo_a_pagarTask = await CalculaSaldoAPagar(pedido_pai, await vl_TotalFamiliaDevolucaoPrecoNFTask);
                var TotalPerda = perdas.Select(r => r.Valor).Sum();
                var lstOcorrenciaTask = await ObterOcorrencias(pedido_pai);
                var lstBlocNotasDevolucaoTask = await BuscarPedidoBlocoNotasDevolucao(pedido_pai);
                var vlPagoTask = await CalcularValorPago(pedido_pai);
                var vlTotalFamiliaPrecoNf = await CalcularVltotalFamiliPrecoNF(pedido_pai);

                var saldo_a_pagar = saldo_a_pagarTask;
                if (tPedidoPai.St_Entrega == Constantes.ST_PAGTO_PAGO && saldo_a_pagar > 0)
                    saldo_a_pagar = 0;

                DetalhesFormaPagamentosDados detalhesFormaPagto = new DetalhesFormaPagamentosDados();

                detalhesFormaPagto.FormaPagto = lstFormaPgtoTask.ToList();
                detalhesFormaPagto.InfosAnaliseCredito = p.Forma_Pagto;
                detalhesFormaPagto.StatusPagto = StatusPagto(tPedidoPai.St_Pagto).ToUpper();
                detalhesFormaPagto.CorStatusPagto = corStatusPagto;
                detalhesFormaPagto.VlTotalFamilia = vlTotalFamiliaPrecoNf;
                detalhesFormaPagto.VlPago = vlPagoTask;
                detalhesFormaPagto.VlDevolucao = await vl_TotalFamiliaDevolucaoPrecoNFTask;
                detalhesFormaPagto.VlPerdas = TotalPerda;
                detalhesFormaPagto.SaldoAPagar = saldo_a_pagar;
                detalhesFormaPagto.AnaliseCredito = analiseCreditoTask;
                detalhesFormaPagto.CorAnalise = corAnalise;
                detalhesFormaPagto.DataColeta = dataEntrega;
                detalhesFormaPagto.Transportadora = TranspNomeTask;
                detalhesFormaPagto.VlFrete = p.Frete_Valor;

                //vou montar uma lista fake para verificar como mostrar uma qtde de numeros de pedido por linha
                List<List<string>> lstPorLinha = new List<List<string>>();
                List<string> lstNumPedDabase = await pedido.Select(r => r.Pedido).ToListAsync();

                List<string> lstLinha = new List<string>();

                int aux = lstNumPedDabase.Count();
                for (var i = 0; i <= lstNumPedDabase.Count(); i++)
                {
                    if (i < lstNumPedDabase.Count)
                        lstLinha.Add(lstNumPedDabase[i]);

                    if (lstLinha.Count == 8)
                    {
                        lstPorLinha.Add(lstLinha);
                        lstLinha = new List<string>();
                    }
                    if (i == aux)
                    {
                        lstPorLinha.Add(lstLinha);
                    }
                }

                return new PedidoDados
                {
                    NumeroPedido = numPedido,
                    Lista_NumeroPedidoFilhote = lstPorLinha,
                    DataHoraPedido = p.Data,
                    StatusHoraPedido = await MontarDtoStatuPedido(p),
                    DadosCliente = dadosCliente,
                    ListaProdutos = (await ObterProdutos(numPedido)).ToList(),
                    TotalFamiliaParcelaRA = vlFamiliaParcelaRATask,
                    PermiteRAStatus = p.Permite_RA_Status,
                    OpcaoPossuiRA = p.Opcao_Possui_RA == "S",
                    PercRT = p.Perc_RT,
                    ValorTotalDestePedidoComRA = vlTotalDestePedidoComRATask,
                    VlTotalDestePedido = vlTotalDestePedidoTask,
                    DetalhesNF = detalhesNf,
                    DetalhesFormaPagto = detalhesFormaPagto,
                    ListaProdutoDevolvido = (await BuscarProdutosDevolvidos(numPedido)).ToList(),
                    ListaPerdas = (await BuscarPerdas(numPedido)).ToList(),
                    ListaBlocoNotas = (await BuscarPedidoBlocoNotas(numPedido)).ToList(),
                    EnderecoEntrega = enderecoEntregaTask,
                    ListaOcorrencia = lstOcorrenciaTask.ToList(),
                    ListaBlocoNotasDevolucao = lstBlocNotasDevolucaoTask.ToList()
                };
            }
        }

        private async Task<decimal> CalcularVltotalFamiliPrecoNF(string numPedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var familiaTask = from c in db.TpedidoItem
                                  where c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO &&
                                        c.Tpedido.Pedido.Contains(numPedido)
                                  select new
                                  {
                                      total = c.Qtde * c.Preco_Venda,
                                      totalNf = c.Qtde * c.Preco_NF
                                  };

                decimal total = (decimal)(await familiaTask.SumAsync(x => x.total));
                decimal totalNf = (decimal)(await familiaTask.SumAsync(x => x.totalNf));

                return totalNf;
            }
        }

        private async Task<decimal> CalcularValorPago(string numPedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {

                var vlFamiliaP = from c in db.TpedidoPagamento
                                 where c.Pedido.StartsWith(numPedido)
                                 select c;
                var vl_TotalFamiliaPagoTask = vlFamiliaP.Select(r => r.Valor).SumAsync();

                return await vl_TotalFamiliaPagoTask;
            }
        }

        private string ObterEntregaImediata(Tpedido p)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                string nomeUsuario = null;
                if (p.EtgImediataIdTipoUsuarioContexto == (short)Constantes.TipoUsuarioContexto.UsuarioInterno)
                {
                    nomeUsuario = (from c in db.Tusuario
                                   where c.Id == p.EtgImediataIdUsuarioUltAtualiz
                                   select c.Usuario).FirstOrDefault();
                }
                if (p.EtgImediataIdTipoUsuarioContexto == (short)Constantes.TipoUsuarioContexto.Parceiro)
                {
                    nomeUsuario = (from c in db.TorcamentistaEindicador
                                   where c.IdIndicador == p.EtgImediataIdUsuarioUltAtualiz
                                   select c.Apelido).FirstOrDefault();
                }
                if (p.EtgImediataIdTipoUsuarioContexto == (short)Constantes.TipoUsuarioContexto.VendedorParceiro)
                {
                    //buscar na t_orcamentista_indicador_vendedor nome amigável
                    var nomeCompleto = (from c in db.TorcamentistaEindicadorVendedor
                                        where c.Id == p.EtgImediataIdUsuarioUltAtualiz
                                        select c.Nome).FirstOrDefault();
                    nomeUsuario = $"[VP] {nomeCompleto.Split(" ")[0]}"; //paliativo pois não temos um nome curto para esse usuário
                }
                if (string.IsNullOrEmpty(nomeUsuario))
                {
                    nomeUsuario = p.Etg_Imediata_Usuario;
                }
                string retorno = "";
                string dataFormatada = "";
                //varificar a variavel st_etg_imediata para saber se é sim ou não pela constante 
                //caso não atenda nenhuma das condições o retorno fica como vazio.
                if (p.St_Etg_Imediata == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO)
                    retorno = "NÃO";
                else if (p.St_Etg_Imediata == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM)
                    retorno = "SIM";
                //formatar a data da variavel etg_imediata_data
                if (retorno != "")
                    dataFormatada = p.Etg_Imediata_Data?.ToString("dd/MM/yyyy HH:mm");
                //verificar se o retorno acima esta vazio
                if (!string.IsNullOrEmpty(dataFormatada))
                    //retorno += " (" + IniciaisEmMaisculas(p.Etg_Imediata_Usuario) + " - " + dataFormatada + ")";
                    retorno += " (" + Texto.iniciaisEmMaiusculas(nomeUsuario) + " - " + dataFormatada + ")";


                return retorno;
            }

        }

        private async Task<string> ObterNomeTransportadora(string idTransportadora)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var transportadora = from c in db.Ttransportadora
                                     where c.Id == idTransportadora
                                     select c.Nome;

                var retorno = await transportadora.Select(r => r.ToString()).FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(retorno))
                    retorno = idTransportadora + " (" + retorno + ")";

                return retorno;
            }
        }

        private async Task<StatusPedidoPedidoDados> MontarDtoStatuPedido(Tpedido p)
        {
            /*
             * Buscar o pedido para:
             * verificar se o st_entrega ==  ST_ENTREGA_ENTREGUE
             * se pedido.PedidoRecebidoStatus == COD_ST_PEDIDO_RECEBIDO_SIM => se o status é entregue monta como esta sendo feito
             * se o status é não entregue iremos mostrar apenas a data do pedido entre "(data do pedido)"
             * 
             */

            using (var db = contextoProvider.GetContextoLeitura())
            {
                var countTask = from c in db.TpedidoItemDevolvido
                                where c.Pedido == p.Pedido
                                select c;
                int countItemDevolvido = await countTask.CountAsync();

                StatusPedidoPedidoDados status = new StatusPedidoPedidoDados();

                if (!String.IsNullOrEmpty(p.Pedido_Bs_X_Marketplace))
                {
                    status.Descricao_Pedido_Bs_X_Marketplace = Util.ObterDescricao_Cod("PedidoECommerce_Origem", p.Marketplace_codigo_origem, contextoProvider) + ":" + p.Pedido_Bs_X_Marketplace;
                }
                if (!String.IsNullOrEmpty(p.Pedido_Bs_X_Ac))
                {
                    status.Cor_Pedido_Bs_X_Ac = "purple";
                    status.Pedido_Bs_X_Ac = p.Pedido_Bs_X_Ac;
                }

                status.Status = FormataSatusPedido(p.St_Entrega);
                status.CorEntrega = CorStatusEntrega(p.St_Entrega, countItemDevolvido);//verificar a saida 
                status.St_Entrega = FormataSatusPedido(p.St_Entrega);
                status.Entregue_Data = p.Entregue_Data?.ToString("dd/MM/yyyy");
                status.Cancelado_Data = p.Cancelado_Data?.ToString("dd/MM/yyyy");
                status.Pedido_Data = p.Data?.ToString("dd/MM/yyyy");
                status.Pedido_Hora = Formata_hhmmss_para_hh_minuto(p.Hora);
                status.Recebida_Data = p.PedidoRecebidoData?.ToString("dd/MM/yyyy");
                status.PedidoRecebidoStatus = p.PedidoRecebidoStatus.ToString();

                return await Task.FromResult(status);
            }
        }

        private string Formata_hhmmss_para_hh_minuto(string hora)
        {
            string hh = "";
            string mm = "";
            string retorno = "";

            if (!string.IsNullOrEmpty(hora))
            {
                hh = hora.Substring(0, 2);
                mm = hora.Substring(2, 2);

                retorno = hh + ":" + mm;
            }

            return retorno;
        }

        private static string Formata_hhmmss_para_hh_minuto_ss(string hora)
        {
            string hh = "";
            string mm = "";
            string ss = "";
            string retorno = "";

            if (!string.IsNullOrEmpty(hora))
            {
                hh = hora.Substring(0, 2);
                mm = hora.Substring(2, 2);

                retorno = hh + ":" + mm + ":" + ss;
            }

            return retorno;
        }

        private string CorStatusEntrega(string st_entrega, int countItemDevolvido)
        {
            string cor = "black";

            switch (st_entrega)
            {
                case Constantes.ST_ENTREGA_ESPERAR:
                    cor = "deeppink";
                    break;
                case Constantes.ST_ENTREGA_SPLIT_POSSIVEL:
                    cor = "darkorange";
                    break;
                case Constantes.ST_ENTREGA_SEPARAR:
                    cor = "maroon";
                    break;
                case Constantes.ST_ENTREGA_A_ENTREGAR:
                    cor = "blue";
                    break;
                case Constantes.ST_ENTREGA_ENTREGUE:
                    cor = "green";
                    if (countItemDevolvido > 0)
                        cor = "red";
                    break;
                case Constantes.ST_ENTREGA_CANCELADO:
                    cor = "red";
                    break;
            }

            return cor;
        }

        public string FormataSatusPedido(string status)
        {
            string retorno = "";

            switch (status)
            {
                case Constantes.ST_ENTREGA_ESPERAR:
                    retorno = "Esperar Mercadoria";
                    break;
                case Constantes.ST_ENTREGA_SPLIT_POSSIVEL:
                    retorno = "Split Possível";
                    break;
                case Constantes.ST_ENTREGA_SEPARAR:
                    retorno = "Separar Mercadoria";
                    break;
                case Constantes.ST_ENTREGA_A_ENTREGAR:
                    retorno = "A Entregar";
                    break;
                case Constantes.ST_ENTREGA_ENTREGUE:
                    retorno = "Entregue";
                    break;
                case Constantes.ST_ENTREGA_CANCELADO:
                    retorno = "Cancelado";
                    break;
            }

            return retorno;
        }

        private string CorSatusPagto(string statusPagto)
        {
            string retorno = "";

            switch (statusPagto)
            {
                case Constantes.ST_PAGTO_PAGO:
                    retorno = "green";
                    break;
                case Constantes.ST_PAGTO_NAO_PAGO:
                    retorno = "red";
                    break;
                case Constantes.ST_PAGTO_PARCIAL:
                    retorno = "deeppink";
                    break;
            }

            return retorno;
        }

        private string CorAnaliseCredito(string codigo)
        {
            if (codigo == null)
                return "";

            string retorno = "";

            switch (codigo)
            {
                case Constantes.COD_AN_CREDITO_PENDENTE:
                    retorno = "red";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_VENDAS:
                    retorno = "red";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_ENDERECO:
                    retorno = "red";
                    break;
                case Constantes.COD_AN_CREDITO_OK:
                    retorno = "green";
                    break;
                case Constantes.COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO:
                    retorno = "darkorange";
                    break;
                case Constantes.COD_AN_CREDITO_OK_DEPOSITO_AGUARDANDO_DESBLOQUEIO:
                    retorno = "darkorange";
                    break;
                case Constantes.COD_AN_CREDITO_OK_AGUARDANDO_PAGTO_BOLETO_AV:
                    retorno = "darkorange";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_PAGTO_ANTECIPADO_BOLETO:
                    retorno = "blue";
                    break;
            }

            return retorno;
        }

        private async Task<IEnumerable<OcorrenciasPedidoDados>> ObterOcorrencias(string numPedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var leftJoin = (from a in db.TcodigoDescricao.Where(x => x.Grupo ==
                                Constantes.GRUPO_T_CODIGO_DESCRICAO__OCORRENCIAS_EM_PEDIDOS__MOTIVO_ABERTURA)
                                join b in db.TpedidoOcorrencia
                                     on a.Codigo equals b.Cod_Motivo_Abertura into juncao
                                from j in juncao.Where(x => x.Pedido == numPedido).ToList()
                                select new
                                {
                                    tcodigo_descricao = a,
                                    juncao = j
                                }).ToList();

                List<OcorrenciasPedidoDados> lista = new List<OcorrenciasPedidoDados>();

                leftJoin.ForEach(async x =>
                {
                    //objeto para ser adicionado na lista de retorno
                    OcorrenciasPedidoDados ocorre = new OcorrenciasPedidoDados();
                    ocorre.Usuario = x.juncao.Usuario_Cadastro;
                    ocorre.Dt_Hr_Cadastro = x.juncao.Dt_Hr_Cadastro;
                    if (x.juncao.Finalizado_Status != 0)
                        ocorre.Situacao = "FINALIZADA";
                    else
                    {
                        ocorre.Situacao = (from c in db.TpedidoOcorrenciaMensagem
                                           where c.Id_Ocorrencia == x.juncao.Id &&
                                                 c.Fluxo_Mensagem == Constantes.COD_FLUXO_MENSAGEM_OCORRENCIAS_EM_PEDIDOS__CENTRAL_PARA_LOJA
                                           select c).Count() > 0 ? "EM ANDAMENTO" : "ABERTA";

                    }

                    ocorre.Contato = x.juncao.Contato ?? "";
                    if (x.juncao.Tel_1 != "")
                        ocorre.Contato += "&nbsp;&nbsp; (" + x.juncao.Ddd_1 + ") " + Util.FormatarTelefones(x.juncao.Tel_1);
                    if (x.juncao.Tel_2 != "")
                        ocorre.Contato += "   &nbsp;&nbsp;(" + x.juncao.Ddd_2 + ") " + Util.FormatarTelefones(x.juncao.Tel_2);

                    ocorre.Texto_Ocorrencia = x.juncao.Texto_Ocorrencia;
                    ocorre.mensagemDtoOcorrenciaPedidos = (await ObterMensagemOcorrencia(x.juncao.Id)).ToList();
                    ocorre.Finalizado_Usuario = x.juncao.Finalizado_Usuario;
                    ocorre.Finalizado_Data_Hora = x.juncao.Finalizado_Data_Hora;
                    ocorre.Tipo_Ocorrencia = await Util.ObterDescricao_Cod(
                        Constantes.GRUPO_T_CODIGO_DESCRICAO__OCORRENCIAS_EM_PEDIDOS__TIPO_OCORRENCIA,
                        x.juncao.Tipo_Ocorrencia, contextoProvider);
                    ocorre.Texto_Finalizacao = x.juncao.Texto_Finalizacao;
                    lista.Add(ocorre);
                });

                return await Task.FromResult(lista);
            }
        }

        private async Task<IEnumerable<MensagemOcorrenciaPedidoDados>> ObterMensagemOcorrencia(int idOcorrencia)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var msg = from c in db.TpedidoOcorrenciaMensagem
                          where c.Id_Ocorrencia == idOcorrencia
                          select new MensagemOcorrenciaPedidoDados
                          {
                              Dt_Hr_Cadastro = c.Dt_Hr_Cadastro,
                              Usuario = c.Usuario_Cadastro,
                              Loja = c.Loja,
                              Texto_Mensagem = c.Texto_Mensagem
                          };

                return await Task.FromResult(msg);
            }
        }

        public string DescricaoAnaliseCreditoCadastroPedido(string codigo, bool visualizacao, string numero_pedido, string apelido)
        {
            string retorno = "";
            switch (codigo)
            {
                case Constantes.COD_AN_CREDITO_ST_INICIAL:
                    retorno = "Aguardando Análise Inicial";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE:
                    retorno = "Pendente";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_VENDAS:
                    retorno = "Pendente Vendas";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_ENDERECO:
                    retorno = "Pendente Endereço";
                    break;
                case Constantes.COD_AN_CREDITO_OK:
                    retorno = "Crédito OK";
                    break;
                case Constantes.COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO:
                    retorno = "Crédito OK (aguardando depósito)";
                    break;
                case Constantes.COD_AN_CREDITO_OK_DEPOSITO_AGUARDANDO_DESBLOQUEIO:
                    retorno = "Crédito OK (depósito aguardando desbloqueio)";
                    break;
                case Constantes.COD_AN_CREDITO_NAO_ANALISADO:
                    retorno = "Pedido Sem Análise de Crédito";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_CARTAO:
                    retorno = "Pendente Cartão de Crédito";
                    break;
                case Constantes.COD_AN_CREDITO_OK_AGUARDANDO_PAGTO_BOLETO_AV:
                    retorno = "Crédito OK (aguardando pagto boleto AV)";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_PAGTO_ANTECIPADO_BOLETO:
                    retorno = "Pendente - Pagto Antecipado Boleto";
                    break;
            }

            if (visualizacao)
            {
                using (var db = contextoProvider.GetContextoLeitura())
                {
                    var ret = from c in db.Tpedido
                              where c.Pedido == numero_pedido && c.Indicador == apelido
                              select new { analise_credito_data = c.Analise_credito_Data, analise_credito_usuario = c.Analise_Credito_Usuario };

                    var registro = ret.FirstOrDefault();
                    if (registro != null)
                    {
                        if (registro.analise_credito_data.HasValue)
                        {
                            if (!string.IsNullOrEmpty(registro.analise_credito_usuario))
                            {
                                string maiuscula = (Char.ToUpper(registro.analise_credito_usuario[0]) +
                                    registro.analise_credito_usuario.Substring(1).ToLower());

                                retorno = retorno + " (" + registro.analise_credito_data?.ToString("dd/MM/yyyy HH:mm") + " - "
                                    + maiuscula + ")";
                            }
                        }
                    }
                }
            }
            return retorno;
        }


        private async Task<decimal> CalculaSaldoAPagar(string numPedido, decimal vlDevNf)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                //buscar o valor total pago
                var vlFamiliaP = from c in db.TpedidoPagamento
                                 where c.Pedido.StartsWith(numPedido)
                                 select c;
                var vl_TotalFamiliaPagoTask = await vlFamiliaP.Select(r => r.Valor).SumAsync();

                //buscar valor total NF
                var vlNf = from c in db.TpedidoItem
                           where c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO && c.Tpedido.Pedido.StartsWith(numPedido)
                           select c.Qtde * c.Preco_NF;
                var vl_TotalFamiliaPrecoNFTask = await vlNf.Select(r => r.Value).SumAsync();

                decimal result = vl_TotalFamiliaPrecoNFTask - vl_TotalFamiliaPagoTask - vlDevNf;

                return await Task.FromResult(result);
            }
        }

        //Retorna o valor da Familia RA
        private async Task<decimal> CalculaTotalFamiliaRA(string numPedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var vlTotalVendaPorItem = from c in db.TpedidoItem
                                          where c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO && c.Tpedido.Pedido.StartsWith(numPedido)
                                          select new { venda = c.Qtde * c.Preco_Venda, nf = c.Qtde * c.Preco_NF };

                var vlTotalVenda = await vlTotalVendaPorItem.Select(r => r.venda).SumAsync();
                var vlTotalNf = await vlTotalVendaPorItem.Select(r => r.nf).SumAsync();

                decimal result = vlTotalNf.Value - vlTotalVenda.Value;

                return await Task.FromResult(result);
            }
        }

        private async Task<IEnumerable<ProdutoDevolvidoPedidoDados>> BuscarProdutosDevolvidos(string numPedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await
                    (from c in db.TpedidoItemDevolvido
                     where c.Pedido.StartsWith(numPedido)
                     select new ProdutoDevolvidoPedidoDados
                     {
                         Data = c.Devolucao_Data,
                         Hora = c.Devolucao_Hora.Substring(0, 2) + ":" + c.Devolucao_Hora.Substring(2, 2),
                         Qtde = c.Qtde,
                         CodProduto = c.Produto,
                         DescricaoProduto = c.Descricao_Html,
                         Motivo = c.Motivo,
                         NumeroNF = c.NFe_Numero_NF
                     }).ToListAsync();
            }
        }

        private async Task<IEnumerable<PedidoPerdasPedidoDados>> BuscarPerdas(string numPedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                return await (from c in db.TpedidoPerda
                              where c.Pedido == numPedido
                              select new PedidoPerdasPedidoDados
                              {
                                  Data = c.Data,
                                  Hora = Formata_hhmmss_para_hh_minuto_ss(c.Hora),
                                  Valor = c.Valor,
                                  Obs = c.Obs
                              }).ToListAsync();
            }
        }

        private async Task<int> VerificarEstoqueVendido(string numPedido, string fabricante, string produto)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var prod = from c in db.TestoqueMovimento
                           where c.Anulado_Status == 0 &&
                                 c.Pedido == numPedido &&
                                 c.Fabricante == fabricante &&
                                 c.Produto == produto &&
                                 c.Estoque == Constantes.ID_ESTOQUE_VENDIDO &&
                                 c.Qtde.HasValue
                           select new { qtde = (int)c.Qtde };

                int qtde = await prod.Select(r => r.qtde).SumAsync();

                return await Task.FromResult(qtde);
            }
        }

        private async Task<int> VerificarEstoqueSemPresenca(string numPedido, string fabricante, string produto)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var prod = from c in db.TestoqueMovimento
                           where c.Anulado_Status == 0 &&
                                 c.Pedido == numPedido &&
                                 c.Fabricante == fabricante &&
                                 c.Produto == produto &&
                                 c.Estoque == Constantes.ID_ESTOQUE_SEM_PRESENCA &&
                                 c.Qtde.HasValue
                           select new { qtde = (int)c.Qtde };

                int qtde = await prod.Select(r => r.qtde).SumAsync();

                return await Task.FromResult(qtde);
            }
        }

        private string ObterCorFaltante(int qtde, int qtde_estoque_vendido, int qtde_estoque_sem_presenca)
        {
            string retorno = "";

            if (qtde <= 0 || qtde != (qtde_estoque_vendido + qtde_estoque_sem_presenca))
                retorno = "black";
            if (qtde_estoque_vendido != 0 && qtde_estoque_sem_presenca != 0)
                retorno = "darkorange";
            else if (qtde_estoque_sem_presenca == 0)
                retorno = "black";
            else if (qtde_estoque_vendido == 0)
                retorno = "red";

            return retorno;
        }

        private async Task<IEnumerable<BlocoNotasPedidoDados>> BuscarPedidoBlocoNotas(string numPedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var bl = from c in db.TpedidoBlocosNotas
                         where c.Pedido == numPedido &&
                               c.Nivel_Acesso == Constantes.COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__PUBLICO &&
                               c.Anulado_Status == 0
                         select c;

                List<BlocoNotasPedidoDados> lstBlocoNotas = new List<BlocoNotasPedidoDados>();

                foreach (var i in bl)
                {
                    BlocoNotasPedidoDados bloco = new BlocoNotasPedidoDados
                    {
                        Dt_Hora_Cadastro = i.Dt_Hr_Cadastro,
                        Usuario = i.Usuario,
                        Loja = i.Loja,
                        Mensagem = i.Mensagem
                    };
                    lstBlocoNotas.Add(bloco);
                }

                return await Task.FromResult(lstBlocoNotas);
            }
        }

        private async Task<IEnumerable<BlocoNotasDevolucaoMercadoriasPedidoDados>> BuscarPedidoBlocoNotasDevolucao(string numPedido)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var blDevolucao = from c in db.TpedidoItemDevolvidoBlocoNotas
                                  where c.TpedidoItemDevolvido.Pedido == numPedido && c.Anulado_Status == 0
                                  orderby c.Dt_Hr_Cadastro, c.Id
                                  select new BlocoNotasDevolucaoMercadoriasPedidoDados
                                  {
                                      Dt_Hr_Cadastro = c.Dt_Hr_Cadastro,
                                      Usuario = c.Usuario,
                                      Loja = c.Loja,
                                      Mensagem = c.Mensagem
                                  };

                if (blDevolucao.Count() == 0)
                    return new List<BlocoNotasDevolucaoMercadoriasPedidoDados>();

                List<BlocoNotasDevolucaoMercadoriasPedidoDados> lista = new List<BlocoNotasDevolucaoMercadoriasPedidoDados>();

                foreach (var b in blDevolucao)
                {
                    lista.Add(new BlocoNotasDevolucaoMercadoriasPedidoDados
                    {
                        Dt_Hr_Cadastro = b.Dt_Hr_Cadastro,
                        Usuario = b.Usuario,
                        Loja = b.Loja,
                        Mensagem = b.Mensagem
                    });
                }

                return await Task.FromResult(lista);
            }
        }

        public async Task<IEnumerable<string>> ObterFormaPagto(Tpedido ped)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var p = from c in db.Tpedido
                        where c.Pedido == ped.Pedido && c.Indicador == ped.Indicador
                        select c;

                Tpedido pedido = p.FirstOrDefault();
                List<string> lista = new List<string>();
                string parcelamento = Convert.ToString(pedido.Tipo_Parcelamento);

                switch (parcelamento)
                {
                    case Constantes.COD_FORMA_PAGTO_A_VISTA:
                        lista.Add("À Vista (" + Util.OpcaoFormaPagto(pedido.Av_Forma_Pagto) + ")");
                        break;
                    case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                        lista.Add(String.Format("Parcela Única: " + "{0:c2} (" +
                            Util.OpcaoFormaPagto(pedido.Pu_Forma_Pagto) +
                            ") vencendo após " + pedido.Pu_Vencto_Apos, pedido.Pu_Valor) + " dias");
                        break;
                    case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                        lista.Add(String.Format("Parcelado no Cartão (internet) em " + pedido.Pc_Qtde_Parcelas + " x " +
                            " {0:c2}", pedido.Pc_Valor_Parcela));
                        break;
                    case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                        lista.Add(String.Format("Parcelado no Cartão (maquineta) em " + pedido.Pc_Maquineta_Qtde_Parcelas +
                            " x {0:c2}", pedido.Pc_Maquineta_Valor_Parcela));
                        break;
                    case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                        lista.Add(String.Format("Entrada: " + "{0:c2} (" +
                            Util.OpcaoFormaPagto(pedido.Pce_Forma_Pagto_Entrada) + ")", pedido.Pce_Entrada_Valor));
                        if (pedido.Pce_Forma_Pagto_Prestacao != 5 && pedido.Pce_Forma_Pagto_Prestacao != 7)
                        {
                            lista.Add(String.Format("Prestações: " + pedido.Pce_Prestacao_Qtde + " x " + " {0:c2}" +
                                " (" + Util.OpcaoFormaPagto(pedido.Pce_Forma_Pagto_Prestacao) +
                                ") vencendo a cada " +
                                pedido.Pce_Prestacao_Periodo + " dias", pedido.Pce_Prestacao_Valor));
                        }
                        else
                        {
                            lista.Add(String.Format("Prestações: " + pedido.Pce_Prestacao_Qtde + " x " + " {0:c2}" +
                                " (" + Util.OpcaoFormaPagto(pedido.Pce_Forma_Pagto_Prestacao) + ")", pedido.Pce_Prestacao_Valor));
                        }
                        break;
                    case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                        lista.Add(String.Format("1ª Prestação: " + " {0:c2} (" +
                            Util.OpcaoFormaPagto(pedido.Pse_Forma_Pagto_Prim_Prest) +
                            ") vencendo após " + pedido.Pse_Prim_Prest_Apos + " dias", pedido.Pse_Prim_Prest_Valor));
                        lista.Add(String.Format("Prestações: " + pedido.Pse_Demais_Prest_Qtde + " x " +
                            " {0:c2} (" + Util.OpcaoFormaPagto(pedido.Pse_Forma_Pagto_Demais_Prest) +
                            ") vencendo a cada " + pedido.Pse_Demais_Prest_Periodo + " dias", pedido.Pse_Demais_Prest_Valor));
                        break;
                }

                return await Task.FromResult(lista.ToList());
            }
        }

        public string StatusPagto(string status)
        {
            string retorno = "";

            switch (status)
            {
                case Constantes.ST_PAGTO_PAGO:
                    retorno = "Pago";
                    break;
                case Constantes.ST_PAGTO_NAO_PAGO:
                    retorno = "Não-Pago";
                    break;
                case Constantes.ST_PAGTO_PARCIAL:
                    retorno = "Pago Parcial";
                    break;
            };
            return retorno;
        }

        public bool BuscarPedidosParaAnular(string pedidoBase)
        {
            using (var db = contextoProvider.GetContextoLeitura())
            {
                var saida = from c in db.Tpedido
                            join d in db.TpedidoItemDevolvido on c.Pedido equals d.Pedido into dev
                            from devolvido in dev.DefaultIfEmpty()
                            join e in db.TpedidoPerda on c.Pedido equals e.Pedido into per
                            from perda in per.DefaultIfEmpty()
                            where c.PedidoBase == pedidoBase
                            select new
                            {
                                pedido = c.Pedido,
                                stEntrega = c.St_Entrega,
                                comissaoPaga = c.ComissaoPaga,
                                devolvidoComissaoDescontada = devolvido == null ? 0 : devolvido.ComissaoDescontada,
                                perdaComissaoDescontada = perda == null ? 0 : perda.ComissaoDescontada
                            };

                if (saida != null)
                {
                    var can = saida.Where(x => x.stEntrega != Constantes.ST_ENTREGA_CANCELADO).FirstOrDefault();
                    if (can == null) return false;

                    var comissaoPaga = saida.Where(x => x.comissaoPaga == 1).FirstOrDefault();
                    if (comissaoPaga != null) return false;

                    var devolComissaoPaga = saida.Where(x => x.devolvidoComissaoDescontada == 1).FirstOrDefault();
                    if (devolComissaoPaga != null) return false;

                    var perdaComissaoPaga = saida.Where(x => x.perdaComissaoDescontada == 1).FirstOrDefault();
                    if (perdaComissaoPaga != null) return false;
                }
            }

            return true;
        }

        public void AtualizarAnulacaoOrcamentoCotacaoPedido(string pedidoBase, ContextoBdGravacao dbGravacao)
        {
            var pedidos = from c in dbGravacao.Tpedido
                          where c.PedidoBase == pedidoBase
                          select c;

            if (pedidos == null) return;

            foreach (var pedido in pedidos)
            {
                pedido.IdOrcamentoCotacao = null;
                pedido.Orcamento = null;
                pedido.Orcamentista = "";
                pedido.IdIndicadorVendedor = null;
                pedido.St_Orc_Virou_Pedido = 0;

                //dbGravacao.Update(pedido);
            }

            dbGravacao.SaveChanges();

            return;
        }
    }
}