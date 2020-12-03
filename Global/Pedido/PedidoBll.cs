using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore.Internal;
using InfraBanco.Constantes;
using Produto.RegrasCrtlEstoque;
using UtilsGlobais;
using Pedido.Dados;
using Cliente.Dados;
using Pedido.Dados.DetalhesPedido;
using Produto;
using InfraBanco;

#nullable enable

namespace Pedido
{
    public class PedidoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public PedidoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }


        public async Task<IEnumerable<string>> ListarNumerosPedidoCombo(string apelido)
        // MÉTODOS NOVOS SENDO MOVIDO
        public async Task<PercentualMaxDescEComissao> ObterPercentualMaxDescEComissao(string loja)
        {

            var db = contextoProvider.GetContextoLeitura();

            var lista = from p in db.Tpedidos
                        where p.Orcamentista == apelido &&
                              p.Data >= Util.LimiteDataBuscas()
                        orderby p.Pedido
                        select p.Pedido;
            var ret = from c in db.Tlojas
                      where c.Loja == loja
                      select new PercentualMaxDescEComissao
                      {
                          PercMaxComissao = c.Perc_Max_Comissao,
                          PercMaxComissaoEDesc = c.Perc_Max_Comissao_E_Desconto,
                          PercMaxComissaoEDescNivel2 = c.Perc_Max_Comissao_E_Desconto_Nivel2,
                          PercMaxComissaoEDescPJ = c.Perc_Max_Comissao_E_Desconto_Pj,
                          PercMaxComissaoEDescNivel2PJ = c.Perc_Max_Comissao_E_Desconto_Nivel2_Pj
                      };

            var res = lista.AsEnumerable();
            return await Task.FromResult(res);
            return await ret.FirstOrDefaultAsync();
        }

        public enum TipoBuscaPedido
        public void ValidarPercentualRT(float percComissao, float percentualMax, List<string> lstErros)
        {
            Todos = 0, PedidosEncerrados = 1, PedidosEmAndamento = 2
            if (percComissao < 0 || percComissao > 100)
            {
                lstErros.Add("Percentual de comissão inválido.");
            }


        public async Task<IEnumerable<PedidosPedidoDados>> ListarPedidos(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
            if (percComissao > percentualMax)
            {
            if (dataInicial < Util.LimiteDataBuscas())
            {
                dataInicial = Util.LimiteDataBuscas();
                lstErros.Add("O percentual de comissão excede o máximo permitido.");
            }
            //fazemos a busca
            var ret = await ListarPedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPedido, dataInicial, dataFinal);
        }

            //se tiver algum registro, retorna imediatamente
            if (ret.Any())
                return ret;
        public async Task VerificarSePedidoExite(List<Cl_ITEM_PEDIDO_NOVO> v_item, PedidoCriacaoDados pedido,
            string usuario, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();

            /*
             * se fizeram a busca por algum CPF ou CNPJ ou pedido e não achamos nada, removemos o filtro de datas
             * para não aparcer para o usuário que não tem nenhum registro
             * */
            if (String.IsNullOrEmpty(clienteBusca) && String.IsNullOrEmpty(numeroPedido))
                return ret;
            //verificar se o pedido existe
            string hora_atual = UtilsGlobais.Util.TransformaHora_Minutos();

            //busca sem data final
            ret = await ListarPedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPedido, dataInicial, null);
            if (ret.Any())
                return ret;
            var lstProdTask = await (from c in db.TpedidoItems
                                     where c.Tpedido.Id_Cliente == pedido.DadosCliente.Id &&
                                           c.Tpedido.Data == DateTime.Now.Date &&
                                           c.Tpedido.Loja == pedido.DadosCliente.Loja &&
                                           c.Tpedido.Vendedor == usuario &&
                                           c.Tpedido.Data >= DateTime.Now.Date &&
                                           c.Tpedido.Hora.CompareTo(hora_atual) <= 0 &&
                                           c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO
                                     orderby c.Pedido, c.Sequencia
                                     select new
                                     {
                                         c.Pedido,
                                         c.Produto,
                                         c.Fabricante,
                                         Qtde = c.Qtde ?? 0,
                                         c.Preco_Venda
                                     }).ToListAsync();

            //ainda não achamos nada? então faz a busca sem filtrar por tipo
            ret = await ListarPedidosFiltroEstrito(apelido, TipoBuscaPedido.Todos, clienteBusca, numeroPedido, dataInicial, null);
            return ret;
            foreach (var x in lstProdTask)
            {
                foreach (var y in v_item)
                {
                    if (x.Produto == y.Produto &&
                        x.Fabricante == y.Fabricante &&
                        x.Qtde == y.Qtde &&
                        x.Preco_Venda == y.Preco_Venda)
                    {
                        lstErros.Add("Este pedido já foi gravado com o número " + x.Pedido);
                        return;
                    }
                };
            };
        }

        //a busca sem malabarismos para econtrar algum registro
        private async Task<IEnumerable<PedidosPedidoDados>> ListarPedidosFiltroEstrito(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
        public float VerificarPagtoPreferencial(Tparametro tParametro, PedidoCriacaoDados pedido,
            float percDescComissaoUtilizar, PercentualMaxDescEComissao percentualMax, decimal vl_total)
        {
            var db = contextoProvider.GetContextoLeitura();

            //inclui um filtro para não trazer os filhos na listagem do pedido
            var lista = db.Tpedidos.
                Where(r => r.Indicador == apelido);

            switch (tipoBusca)
            List<string> lstOpcoesPagtoPrefericiais = new List<string>();
            if (!string.IsNullOrEmpty(tParametro.Id))
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
                //a verificação é feita na linha 380 ate 388
                lstOpcoesPagtoPrefericiais = tParametro.Campo_texto.Split(',').ToList();
            }

            if (!string.IsNullOrEmpty(clienteBusca))
                lista = lista.Where(r => r.Tcliente.Cnpj_Cpf.Contains(clienteBusca));
            if (!string.IsNullOrEmpty(numeroPedido))
                lista = lista.Where(r => r.Pedido.Contains(numeroPedido));
            if (dataInicial.HasValue)
                lista = lista.Where(r => r.Data >= dataInicial.Value);
            if (dataFinal.HasValue)
                lista = lista.Where(r => r.Data <= dataFinal.Value);
            string s_pg = "";
            decimal? vlNivel1 = 0;
            decimal? vlNivel2 = 0;

            List<PedidosPedidoDados> lst_pedido = new List<PedidosPedidoDados>();

            //precisa calcular os itens de cada produto referente ao pedido, 
            //para trazer o valor total do pedido e não o total da familia
            foreach (var i in await lista.ToListAsync())
            //identifica e verifica se é pagto preferencial e calcula  637 ate 712
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
                s_pg = pedido.FormaPagtoCriacao.Op_av_forma_pagto;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                s_pg = pedido.FormaPagtoCriacao.Op_pu_forma_pagto;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                s_pg = Constantes.ID_FORMA_PAGTO_CARTAO;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                s_pg = Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA;
            if (!string.IsNullOrEmpty(s_pg))
            {
                var itens = from c in db.TpedidoItems
                            where c.Pedido == i.Pedido
                            select c;

                if (itens != null)
                if (lstOpcoesPagtoPrefericiais.Count > 0)
                {
                    string nome = await (from c in itens
                                         where c.Pedido == i.Pedido
                                         select c.Tpedido.Tcliente.Nome_Iniciais_Em_Maiusculas).FirstOrDefaultAsync();

                    lst_pedido.Add(new PedidosPedidoDados
                    foreach (var op in lstOpcoesPagtoPrefericiais)
                    {
                        NomeCliente = nome,
                        NumeroPedido = i.Pedido,
                        DataPedido = i.Data,
                        Status = i.St_Entrega,
                        ValorTotal = await itens.SumAsync(x => x.Preco_Venda * x.Qtde)
                    });
                        if (s_pg == op)
                        {
                            if (pedido.DadosCliente.Tipo == Constantes.ID_PJ)
                                percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                            else
                                percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;
                        }
                    }
                }
            }

            bool pgtoPreferencial = false;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                s_pg = pedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto;

            //colocar as mensagens de status
            var listaComStatus = lst_pedido;
            foreach (var pedido in listaComStatus)
                if (!string.IsNullOrEmpty(s_pg))
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
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
                    {
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
            return await Task.FromResult(listaComStatus.OrderByDescending(x => x.NumeroPedido));
                    }
                }
                //verificamos a entrada
                if (pgtoPreferencial)
                    vlNivel2 = pedido.FormaPagtoCriacao.C_pce_entrada_valor;
                else
                    vlNivel1 = pedido.FormaPagtoCriacao.C_pce_entrada_valor;

        public async Task<IEnumerable<string>> ListarCpfCnpjPedidosCombo(string apelido)
                //Identifica e contabiliza o valor das parcelas
                pgtoPreferencial = false;
                s_pg = pedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto;
                if (!string.IsNullOrEmpty(s_pg))
                {
            var db = contextoProvider.GetContextoLeitura();
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
                    {
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
                    }
                }

            var lista = (from c in db.Tpedidos
                         where c.Orcamentista == apelido &&
                               c.Data >= Util.LimiteDataBuscas()
                         orderby c.Tcliente.Cnpj_Cpf
                         select c.Tcliente.Cnpj_Cpf).Distinct();
                if (pgtoPreferencial)
                    vlNivel2 = vlNivel2 +
                        (pedido.FormaPagtoCriacao.C_pce_prestacao_qtde * pedido.FormaPagtoCriacao.C_pce_prestacao_valor);
                else
                    vlNivel1 = vlNivel1 +
                        (pedido.FormaPagtoCriacao.C_pce_prestacao_qtde * pedido.FormaPagtoCriacao.C_pce_prestacao_valor);

            var ret = await lista.Distinct().ToListAsync();
            List<string> cpfCnpjFormat = new List<string>();

            foreach (string cpf in ret)
                if (vlNivel2 > (vl_total / 2))
                {
                cpfCnpjFormat.Add(Util.FormatCpf_Cnpj_Ie(cpf));
                    if (pedido.DadosCliente.Tipo == Constantes.ID_PJ)
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                    else
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;
                }

            return cpfCnpjFormat;
            }
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                s_pg = pedido.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto;

        private async Task<DadosClienteCadastroDados> ObterDadosCliente(Tpedido pedido)
                if (!string.IsNullOrEmpty(s_pg))
                {
            var dadosCliente = from c in contextoProvider.GetContextoLeitura().Tclientes
                               where c.Id == pedido.Id_Cliente
                               select c;
            var cli = await dadosCliente.FirstOrDefaultAsync();
            DadosClienteCadastroDados cadastroCliente = new DadosClienteCadastroDados
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
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
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
                    }
                }
                //verificamos a entrada
                if (pgtoPreferencial)
                    vlNivel2 = pedido.FormaPagtoCriacao.C_pse_prim_prest_valor;
                else
                    vlNivel1 = pedido.FormaPagtoCriacao.C_pse_prim_prest_valor;

        private async Task<DadosClienteCadastroDados> ObterDadosClientePedido(Tpedido pedido)
                //Identifica e contabiliza o valor das parcelas
                pgtoPreferencial = false;
                s_pg = pedido.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto;
                if (!string.IsNullOrEmpty(s_pg))
                {
            var dadosCliente = from c in contextoProvider.GetContextoLeitura().Tclientes
                               where c.Id == pedido.Id_Cliente
                               select c;
            var cli = await dadosCliente.FirstOrDefaultAsync();
            DadosClienteCadastroDados cadastroCliente = new DadosClienteCadastroDados
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
                    {
                Loja = await ObterRazaoSocial_Nome_Loja(pedido.Loja),
                Indicador_Orcamentista = pedido.Orcamentista,
                Vendedor = pedido.Vendedor,
                Id = cli.Id,
                Cnpj_Cpf = Util.FormatCpf_Cnpj_Ie(pedido.Endereco_cnpj_cpf),
                Rg = pedido.Endereco_rg,
                Ie = Util.FormatCpf_Cnpj_Ie(pedido.Endereco_ie),
                Contribuinte_Icms_Status = pedido.Endereco_contribuinte_icms_status,
                Tipo = cli.Tipo,
                Observacao_Filiacao = cli.Filiacao,
                Nascimento = cli.Dt_Nasc,
                Sexo = cli.Sexo,
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
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
                    }
                }

        private async Task<string> ObterRazaoSocial_Nome_Loja(string loja)
        {
            var db = contextoProvider.GetContextoLeitura();
                if (pgtoPreferencial)
                    vlNivel2 = vlNivel2 +
                        (pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde * pedido.FormaPagtoCriacao.C_pse_demais_prest_valor);
                else
                    vlNivel1 = vlNivel1 +
                        (pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde * pedido.FormaPagtoCriacao.C_pse_demais_prest_valor);

            string retorno = "";

            Tloja lojaTask = await (from c in db.Tlojas
                                    where c.Loja == loja
                                    select c).SingleOrDefaultAsync();

            if (lojaTask != null)
                if (vlNivel2 > (vl_total / 2))
                {
                if (!string.IsNullOrEmpty(lojaTask.Razao_Social))
                    retorno = lojaTask.Razao_Social;
                    if (pedido.DadosCliente.Tipo == Constantes.ID_PJ)
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                    else
                    retorno = lojaTask.Nome;
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;
                }

            return retorno;
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
            enderecoEntrega.St_memorizacao_completa_enderecos = p.St_memorizacao_completa_enderecos;

            return enderecoEntrega;
            return percDescComissaoUtilizar;
        }

        private async Task<IEnumerable<PedidoProdutosPedidoDados>> ObterProdutos(string numPedido)
        public async Task VerificarDescontoArredondado(string loja, List<Cl_ITEM_PEDIDO_NOVO> v_item,
            List<string> lstErros, string c_custoFinancFornecTipoParcelamento, short c_custoFinancFornecQtdeParcelas,
            string id_cliente, float percDescComissaoUtilizar, List<string> vdesconto)
        {
            var db = contextoProvider.GetContextoLeitura();

            List<TpedidoItem> produtosItens = await (from c in db.TpedidoItems
                                                     where c.Pedido == numPedido
                                                     select c).ToListAsync();
            float coeficiente = 0;
            float? desc_dado_arredondado = 0;

            List<PedidoProdutosPedidoDados> lstProduto = new List<PedidoProdutosPedidoDados>();

            int qtde_sem_presenca = 0;
            int qtde_vendido = 0;
            short faltante = 0;
            //aqui estão verificando o v_item e não pedido
            //vamos vericar cada produto da lista
            foreach (var item in v_item)
            {
                var produtoLojaTask = (from c in db.TprodutoLojas.Include(x => x.Tproduto).Include(x => x.Tfabricante)
                                       where c.Tproduto.Fabricante == item.Fabricante &&
                                             c.Tproduto.Produto == item.Produto &&
                                             c.Loja == loja
                                       select c).FirstOrDefaultAsync();

            foreach (var c in produtosItens)
                if (produtoLojaTask == null)
                    lstErros.Add("Produto " + item.Produto + " do fabricante " + item.Fabricante + "NÃO está " +
                        "cadastrado para a loja " + loja);
                else
                {
                qtde_sem_presenca = await VerificarEstoqueSemPresenca(numPedido, c.Fabricante, c.Produto);
                qtde_vendido = await VerificarEstoqueVendido(numPedido, c.Fabricante, c.Produto);
                    TprodutoLoja produtoLoja = await produtoLojaTask;
                    item.Preco_lista = produtoLoja.Preco_Lista ?? 0;
                    item.Margem = produtoLoja.Margem ?? 0;
                    item.Desc_max = produtoLoja.Desc_Max ?? 0;
                    item.Comissao = produtoLoja.Comissao ?? 0;
                    item.Preco_fabricante = produtoLoja.Tproduto.Preco_Fabricante ?? 0;
                    item.Vl_custo2 = produtoLoja.Tproduto.Vl_Custo2;
                    item.Descricao = produtoLoja.Tproduto.Descricao;
                    item.Descricao_html = produtoLoja.Tproduto.Descricao_Html;
                    item.Ean = produtoLoja.Tproduto.Ean;
                    item.Grupo = produtoLoja.Tproduto.Grupo;
                    item.Peso = produtoLoja.Tproduto.Peso;
                    item.Qtde_volumes = produtoLoja.Tproduto.Qtde_Volumes ?? 0;
                    item.Markup_fabricante = produtoLoja.Tfabricante.Markup;
                    item.Cubagem = produtoLoja.Tproduto.Cubagem;
                    item.Ncm = produtoLoja.Tproduto.Ncm;
                    item.Cst = produtoLoja.Tproduto.Cst;
                    item.Descontinuado = produtoLoja.Tproduto.Descontinuado;

                if (qtde_sem_presenca != 0)
                    faltante = (short)qtde_sem_presenca;

                PedidoProdutosPedidoDados produto = new PedidoProdutosPedidoDados
                    if (c_custoFinancFornecTipoParcelamento ==
                            Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
                        coeficiente = 1;
                    else
                    {
                    Fabricante = c.Fabricante,
                    NumProduto = c.Produto,
                    Descricao = c.Descricao_Html,
                    Qtde = c.Qtde,
                    Faltando = faltante,
                    CorFaltante = ObterCorFaltante((int)c.Qtde, qtde_vendido, qtde_sem_presenca),
                    Preco = c.Preco_NF,
                    VlLista = c.Preco_Lista,
                    Desconto = c.Desc_Dado,
                    VlUnitario = c.Preco_Venda,
                    VlTotalItem = c.Qtde * c.Preco_Venda,
                    VlTotalItemComRA = c.Qtde * c.Preco_NF,
                    VlVenda = c.Preco_Venda,
                    VlTotal = c.Qtde * c.Preco_Venda,
                    Comissao = c.Comissao
                };
                        var coeficienteTask = (from c in db.TpercentualCustoFinanceiroFornecedors
                                               where c.Fabricante == item.Fabricante &&
                                                     c.Tipo_Parcelamento == c_custoFinancFornecTipoParcelamento &&
                                                     c.Qtde_Parcelas == c_custoFinancFornecQtdeParcelas
                                               select c).FirstOrDefaultAsync();
                        if (await coeficienteTask == null)
                            lstErros.Add("Opção de parcelamento não disponível para fornecedor " + item.Fabricante +
                                ": " + DecodificaCustoFinanFornecQtdeParcelas(c_custoFinancFornecTipoParcelamento,
                                c_custoFinancFornecQtdeParcelas) + " parcela(s)");
                        else
                        {
                            coeficiente = (await coeficienteTask).Coeficiente;
                            //voltamos a atribuir ao tpedidoItem
                            item.Preco_lista = Math.Round((decimal)coeficiente * item.Preco_lista, 2);
                        }

                lstProduto.Add(produto);

                    }

            return lstProduto;
                    item.CustoFinancFornecCoeficiente = coeficiente;

                    if (item.Preco_lista == 0)
                    {
                        item.Desc_Dado = 0;
                        desc_dado_arredondado = 0;
                    }
                    else
                    {
                        item.Desc_Dado = (float)(100 *
                            (item.Preco_lista - item.Preco_Venda) / item.Preco_lista);
                        desc_dado_arredondado = item.Desc_Dado;
                    }

        public async Task<PedidoDados> BuscarPedido(string apelido, string numPedido)
                    if (desc_dado_arredondado > percDescComissaoUtilizar)
                    {
            var db = contextoProvider.GetContextoLeitura();
                        var tDescontoTask = from c in db.Tdescontos
                                            where c.Usado_status == 0 &&
                                                  c.Id_cliente == id_cliente &&
                                                  c.Fabricante == item.Fabricante &&
                                                  c.Produto == item.Produto &&
                                                  c.Loja == loja &&
                                                  c.Data >= DateTime.Now.AddMinutes(-30)
                                            orderby c.Data descending
                                            select c;

            //vamos verificar se o pedido é o pai 057581N
            var pedido_pai = "";
                        Tdesconto tdesconto = await tDescontoTask.FirstOrDefaultAsync();

            if (numPedido.Contains('-'))
                        if (tdesconto == null)
                        {
                string[] split = numPedido.Split('-');
                pedido_pai = split[0];
                            lstErros.Add("Produto " + item.Produto + " do fabricante " + item.Fabricante +
                                ": desconto de " + item.Desc_Dado + "% excede o máximo permitido.");
                        }
                        else
                        {
                pedido_pai = numPedido;
                            tdesconto = await tDescontoTask.FirstOrDefaultAsync();
                            if ((decimal)item.Desc_Dado >= tdesconto.Desc_max)
                                lstErros.Add("Produto " + item.Produto + " do fabricante " + item.Fabricante +
                                    ": desconto de " + item.Desc_Dado + " % excede o máximo autorizado.");
                            else
                            {
                                item.Abaixo_min_status = 1;
                                item.Abaixo_min_autorizacao = tdesconto.Id;
                                item.Abaixo_min_autorizador = tdesconto.Autorizador;
                                item.Abaixo_min_superv_autorizador = tdesconto.Supervisor_autorizador;

                                //essa variavel aparentemente apenas sinaliza 
                                //se existe uma senha de autorização para desconto superior
                                if (vdesconto.Count > 0)
                                {
                                    vdesconto.Add(tdesconto.Id);
                                }
                            }
                        }
                    }
                }
            }
        }

            var pedido = from c in db.Tpedidos
                         where c.Pedido.Contains(pedido_pai)
                         select c;
        private string DecodificaCustoFinanFornecQtdeParcelas(string tipoParcelamento, short custoFFQtdeParcelas)
        {
            string retorno = "";

            //aqui vamos montar o tpedido pai para montar alguns detalhes que contém somente nele
            Tpedido tPedidoPai = await pedido.Select(x => x).Where(x => x.Pedido == pedido_pai).FirstOrDefaultAsync();
            if (tipoParcelamento == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
                retorno = "0+" + custoFFQtdeParcelas;
            else if (tipoParcelamento == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA)
                retorno = "1+" + custoFFQtdeParcelas;

            //aqui vamos montar o pedido conforme o número do pedido que veio na entrada
            Tpedido p = await pedido.Select(x => x).Where(x => x.Pedido == numPedido).FirstOrDefaultAsync();
            return retorno;
        }

            if (p == null)
                return null;
            DadosClienteCadastroDados dadosCliente = new DadosClienteCadastroDados();
            if (p.St_memorizacao_completa_enderecos == 0)
        public async Task<float> BuscarCoeficientePercentualCustoFinanFornec(PedidoCriacaoDados pedido, short qtdeParcelas, string siglaPagto, List<string> lstErros)
        {
                dadosCliente = await ObterDadosCliente(p);
            float coeficiente = 0;

            var db = contextoProvider.GetContextoLeitura();

            if (siglaPagto == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
                coeficiente = 1;
            else
            {
                foreach (var i in pedido.ListaProdutos)
                {
                    var percCustoTask = from c in db.TpercentualCustoFinanceiroFornecedors
                                        where c.Fabricante == i.Fabricante &&
                                              c.Tipo_Parcelamento == siglaPagto &&
                                              c.Qtde_Parcelas == qtdeParcelas
                                        select c;

                    var percCusto = await percCustoTask.FirstOrDefaultAsync();

                    if (percCusto != null)
                    {
                        coeficiente = percCusto.Coeficiente;
                        i.Preco_Lista = (decimal)coeficiente * (i.CustoFinancFornecPrecoListaBase);
                    }
                    else
                    {
                dadosCliente = await ObterDadosClientePedido(p);
                        lstErros.Add("Opção de parcelamento não disponível para fornecedor " + i.Fabricante + ": " +
                            DecodificaCustoFinanFornecQtdeParcelas(pedido.FormaPagtoCriacao.C_forma_pagto, qtdeParcelas) + " parcela(s)");
                    }

            var enderecoEntregaTask = ObterEnderecoEntrega(p);
            var lstProdutoTask = ObterProdutos(numPedido);
                }
            }

            var vlTotalDestePedidoComRATask = lstProdutoTask.Result.Select(r => r.VlTotalItemComRA).Sum();
            var vlTotalDestePedidoTask = lstProdutoTask.Result.Select(r => r.VlTotalItem).Sum();
            return coeficiente;
        }

            //buscar valor total de devoluções NF
            var vlDevNf = from c in db.TpedidoItemDevolvidos
                          where c.Pedido.StartsWith(pedido_pai)
                          select c.Qtde * c.Preco_NF;
            var vl_TotalFamiliaDevolucaoPrecoNFTask = vlDevNf.Select(r => r.Value).SumAsync();
        public async Task<ProdutoValidadoComEstoqueDados> VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado(PedidoProdutoPedidoDados produto,
            string cpf_cnpj, int id_nfe_emitente_selecao_manual)
        {
            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = from c in db.Tclientes
                              where c.Cnpj_Cpf == cpf_cnpj
                              select c;

            var vlFamiliaParcelaRATask = CalculaTotalFamiliaRA(pedido_pai);
            Tcliente cliente = await clienteTask.FirstOrDefaultAsync();

            string garantiaIndicadorStatus = Convert.ToString(p.GarantiaIndicadorStatus);
            if (garantiaIndicadorStatus == Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO)
                garantiaIndicadorStatus = "NÃO";
            if (garantiaIndicadorStatus == Constantes.COD_GARANTIA_INDICADOR_STATUS__SIM)
                garantiaIndicadorStatus = "SIM";
            var prodValidadoEstoqueListaErros = new List<string>();

            DetalhesNFPedidoPedidoDados detalhesNf = new DetalhesNFPedidoPedidoDados();
            detalhesNf.Observacoes = p.Obs_1;
            detalhesNf.ConstaNaNF = p.Nfe_Texto_Constar;
            detalhesNf.XPed = p.Nfe_XPed;
            detalhesNf.NumeroNF = tPedidoPai.Obs_2;//consta somente no pai
            detalhesNf.NFSimples = tPedidoPai.Obs_3;
            //obtém  a sigla para regra
            string cliente_regra = Produto.UtilsProduto.MultiCdRegraDeterminaPessoa(cliente.Tipo, cliente.Contribuinte_Icms_Status,
                cliente.Produtor_Rural_Status);

            detalhesNf.PrevisaoEntrega = tPedidoPai.St_Etg_Imediata == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO ?
                tPedidoPai.PrevisaoEntregaData?.ToString("dd/MM/yyyy") + " ("
                + Texto.iniciaisEmMaiusculas(tPedidoPai.PrevisaoEntregaUsuarioUltAtualiz) +
                " - " + tPedidoPai.PrevisaoEntregaData?.ToString("dd/MM/yyyy") + " "
                + tPedidoPai.PrevisaoEntregaDtHrUltAtualiz?.ToString("HH:mm") + ")" : null;
            //buscar o produto
            //PedidoProdutosDtoPedido produto = new PedidoProdutosDtoPedido();

            detalhesNf.EntregaImediata = ObterEntregaImediata(p);
            detalhesNf.StBemUsoConsumo = p.StBemUsoConsumo;
            detalhesNf.InstaladorInstala = tPedidoPai.InstaladorInstalaStatus;
            detalhesNf.GarantiaIndicadorStatus = garantiaIndicadorStatus;//esse campo não esta mais sendo utilizado
            List<RegrasBll> regraCrtlEstoque = (await ObterCtrlEstoqueProdutoRegraParaUMProduto(produto, cliente,
                prodValidadoEstoqueListaErros)).ToList();

            //verifica o status da entrega
            DateTime? dataEntrega = new DateTime();
            if (tPedidoPai.St_Entrega == Constantes.ST_ENTREGA_A_ENTREGAR || tPedidoPai.St_Entrega == Constantes.ST_ENTREGA_SEPARAR)
                dataEntrega = tPedidoPai.A_Entregar_Data_Marcada;
            //afazer: verificar se há necessidade de continuar com esse método, pois acima faz a mesma coisa com validação
            await UtilsProduto.ObterCtrlEstoqueProdutoRegra_Teste(prodValidadoEstoqueListaErros, regraCrtlEstoque, cliente.Uf, cliente_regra, contextoProvider);

            var perdas = BuscarPerdas(numPedido);
            var TranspNomeTask = ObterNomeTransportadora(p.Transportadora_Id);
            var lstFormaPgtoTask = ObterFormaPagto(tPedidoPai);
            var analiseCreditoTask = ObterAnaliseCredito(Convert.ToString(tPedidoPai.Analise_Credito), pedido_pai, apelido);
            string corAnalise = CorAnaliseCredito(Convert.ToString(tPedidoPai.Analise_Credito));
            string corStatusPagto = CorSatusPagto(tPedidoPai.St_Pagto);
            var saldo_a_pagarTask = CalculaSaldoAPagar(pedido_pai, await vl_TotalFamiliaDevolucaoPrecoNFTask);
            var TotalPerda = (await perdas).Select(r => r.Valor).Sum();
            var lstOcorrenciaTask = ObterOcorrencias(pedido_pai);
            var lstBlocNotasDevolucaoTask = BuscarPedidoBlocoNotasDevolucao(pedido_pai);
            var vlPagoTask = CalcularValorPago(pedido_pai);
            var vlTotalFamiliaPrecoNf = await CalcularVltotalFamiliPrecoNF(pedido_pai);
            VerificarRegrasAssociadasParaUMProduto(regraCrtlEstoque, prodValidadoEstoqueListaErros, cliente, id_nfe_emitente_selecao_manual);

            var saldo_a_pagar = await saldo_a_pagarTask;
            if (tPedidoPai.St_Entrega == Constantes.ST_PAGTO_PAGO && saldo_a_pagar > 0)
                saldo_a_pagar = 0;
            if (id_nfe_emitente_selecao_manual != 0)
                await VerificarCDHabilitadoTodasRegras(regraCrtlEstoque, id_nfe_emitente_selecao_manual, prodValidadoEstoqueListaErros);

            DetalhesFormaPagamentosDados detalhesFormaPagto = new DetalhesFormaPagamentosDados();
            await ObterDisponibilidadeEstoque(regraCrtlEstoque, produto, prodValidadoEstoqueListaErros, id_nfe_emitente_selecao_manual);

            detalhesFormaPagto.FormaPagto = (await lstFormaPgtoTask).ToList();
            detalhesFormaPagto.InfosAnaliseCredito = p.Forma_Pagto;
            detalhesFormaPagto.StatusPagto = StatusPagto(tPedidoPai.St_Pagto).ToUpper();
            detalhesFormaPagto.CorStatusPagto = corStatusPagto;
            detalhesFormaPagto.VlTotalFamilia = vlTotalFamiliaPrecoNf;
            detalhesFormaPagto.VlPago = await vlPagoTask;
            detalhesFormaPagto.VlDevolucao = await vl_TotalFamiliaDevolucaoPrecoNFTask;
            detalhesFormaPagto.VlPerdas = TotalPerda;
            detalhesFormaPagto.SaldoAPagar = saldo_a_pagar;
            detalhesFormaPagto.AnaliseCredito = await analiseCreditoTask;
            detalhesFormaPagto.CorAnalise = corAnalise;
            detalhesFormaPagto.DataColeta = dataEntrega;
            detalhesFormaPagto.Transportadora = await TranspNomeTask;
            detalhesFormaPagto.VlFrete = p.Frete_Valor;
            //meto responsavel por atribuir a qtde de estoque ao produto
            //await Util.Util.VerificarEstoque(regraCrtlEstoque, produto, id_nfe_emitente_selecao_manual, contextoProvider);

            //vou montar uma lista fake para verificar como mostrar uma qtde de numeros de pedido por linha
            List<List<string>> lstPorLinha = new List<List<string>>();
            List<string> lstNumPedDabase = await pedido.Select(r => r.Pedido).ToListAsync();
            bool estoqueInsuficiente = VerificarEstoqueInsuficienteUMProduto(regraCrtlEstoque, produto,
                id_nfe_emitente_selecao_manual, prodValidadoEstoqueListaErros);

            List<string> lstLinha = new List<string>();
            VerificarQtdePedidosAutoSplit(regraCrtlEstoque, prodValidadoEstoqueListaErros, produto, id_nfe_emitente_selecao_manual);

            int aux = lstNumPedDabase.Count();
            for (var i = 0; i <= lstNumPedDabase.Count(); i++)
            {
                if (i < lstNumPedDabase.Count)
                    lstLinha.Add(lstNumPedDabase[i]);
            List<int> lst_empresa_selecionada = ContagemEmpresasUsadasAutoSplit(regraCrtlEstoque, id_nfe_emitente_selecao_manual);

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
            await ExisteProdutoDescontinuado(produto, prodValidadoEstoqueListaErros);

            PedidoDados DtoPedido = new PedidoDados
            {
                NumeroPedido = numPedido,
                Lista_NumeroPedidoFilhote = lstPorLinha,
                DataHoraPedido = p.Data,
                StatusHoraPedido = await MontarDtoStatuPedido(p),
                DadosCliente = dadosCliente,
                ListaProdutos = (await ObterProdutos(numPedido)).ToList(),
                TotalFamiliaParcelaRA = await vlFamiliaParcelaRATask,
                PermiteRAStatus = p.Permite_RA_Status,
                OpcaoPossuiRA = p.Opcao_Possui_RA,
                PercRT = p.Perc_RT,
                ValorTotalDestePedidoComRA = vlTotalDestePedidoComRATask,
                VlTotalDestePedido = vlTotalDestePedidoTask,
                DetalhesNF = detalhesNf,
                DetalhesFormaPagto = detalhesFormaPagto,
                ListaProdutoDevolvido = (await BuscarProdutosDevolvidos(numPedido)).ToList(),
                ListaPerdas = (await BuscarPerdas(numPedido)).ToList(),
                ListaBlocoNotas = (await BuscarPedidoBlocoNotas(numPedido)).ToList(),
                EnderecoEntrega = (await enderecoEntregaTask),
                ListaOcorrencia = (await lstOcorrenciaTask).ToList(),
                ListaBlocoNotasDevolucao = (await lstBlocNotasDevolucaoTask).ToList()
            };
            var prodValidadoEstoqueProduto = new ProdutoPedidoDados();
            prodValidadoEstoqueProduto.Produto = produto.Produto;
            prodValidadoEstoqueProduto.Fabricante = produto.Fabricante;
            prodValidadoEstoqueProduto.Estoque = produto.Qtde_estoque_total_disponivel ?? 0;
            prodValidadoEstoqueProduto.QtdeSolicitada = produto.Qtde;
            prodValidadoEstoqueProduto.Preco_lista = produto.Preco_Lista;
            prodValidadoEstoqueProduto.Descricao_html = produto.Descricao;
            prodValidadoEstoqueProduto.Lst_empresa_selecionada = lst_empresa_selecionada;
            ProdutoValidadoComEstoqueDados prodValidadoEstoque = new ProdutoValidadoComEstoqueDados(prodValidadoEstoqueProduto,
                prodValidadoEstoqueListaErros);

            return await Task.FromResult(DtoPedido);
            return prodValidadoEstoque;
        }

        private async Task<decimal> CalcularVltotalFamiliPrecoNF(string numPedido)
        //todo: afazer: tentar unificar com Prepedido.PrepedidoBll.ObterCtrlEstoqueProdutoRegra
        public async Task<IEnumerable<RegrasBll>> ObterCtrlEstoqueProdutoRegraParaUMProduto(PedidoProdutoPedidoDados produto,
            Tcliente tcliente, List<string> lstErros)
        {
            List<RegrasBll> lstRegrasCrtlEstoque = new List<RegrasBll>();

            var db = contextoProvider.GetContextoLeitura();
            var familiaTask = from c in db.TpedidoItems
                              where c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO &&
                                    c.Tpedido.Pedido.Contains(numPedido)
                              select new
                              {
                                  total = c.Qtde * c.Preco_Venda,
                                  totalNf = c.Qtde * c.Preco_NF
                              };

            decimal total = (decimal)(await familiaTask.SumAsync(x => x.total));
            decimal totalNf = (decimal)(await familiaTask.SumAsync(x => x.totalNf));
            //vamos verificar passando todos os produtos simples da lista de produto que irá para ser selecionado

            return totalNf;
        }
            var regraProdutoTask = from c in db.TprodutoXwmsRegraCds
                                   where c.Fabricante == produto.Fabricante &&
                                         c.Produto == produto.Produto
                                   select c;

        private async Task<decimal> CalcularValorPago(string numPedido)
            var regra = await regraProdutoTask.FirstOrDefaultAsync();

            if (regra == null)
            {
            var db = contextoProvider.GetContextoLeitura();

            var vlFamiliaP = from c in db.TpedidoPagamentos
                             where c.Pedido.StartsWith(numPedido)
                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                    Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': produto (" + produto.Fabricante + ")" +
                    produto.Produto + " não possui regra associada");
            }
            else
            {
                if (regra.Id_wms_regra_cd == 0)
                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                        Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': produto (" + produto.Fabricante + ")" +
                        produto.Produto + " não está associado a nenhuma regra");
                else
                {
                    var wmsRegraTask = from c in db.TwmsRegraCds
                                       where c.Id == regra.Id_wms_regra_cd
                                       select c;
            var vl_TotalFamiliaPagoTask = vlFamiliaP.Select(r => r.Valor).SumAsync();

            return await vl_TotalFamiliaPagoTask;
        }
                    var wmsRegra = await wmsRegraTask.FirstOrDefaultAsync();
                    if (wmsRegra == null)
                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                            Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': regra associada ao produto (" +
                            produto.Fabricante + ")" +
                            produto.Produto + " não foi localizada no banco de dados (Id=" + regra.Id_wms_regra_cd + ")");
                    else
                    {
                        RegrasBll itemRegra = new RegrasBll();
                        itemRegra.Fabricante = produto.Fabricante;
                        itemRegra.Produto = produto.Produto;

        private string ObterEntregaImediata(Tpedido p)
                        itemRegra.TwmsRegraCd = new t_WMS_REGRA_CD
                        {
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
                retorno += " (" + IniciaisEmMaisculas(p.Etg_Imediata_Usuario) + " - " + dataFormatada + ")";
                            Id = wmsRegra.Id,
                            Apelido = wmsRegra.Apelido,
                            Descricao = wmsRegra.Descricao,
                            St_inativo = wmsRegra.St_inativo
                        };

                        var wmsRegraCdXUfTask = from c in db.TwmsRegraCdXUfs
                                                where c.Id_wms_regra_cd == itemRegra.TwmsRegraCd.Id &&
                                                      c.Uf == tcliente.Uf
                                                select c;
                        var wmsRegraCdXUf = await wmsRegraCdXUfTask.FirstOrDefaultAsync();

            return retorno;
                        if (wmsRegraCdXUf == null)
                        {
                            itemRegra.St_Regra = false;
                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + tcliente.Uf + "' e '" +
                                Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) + "': regra associada ao produto (" +
                                produto.Fabricante + ")" +
                                produto.Produto + " não está cadastrada para a UF '" + tcliente.Uf + "' (Id=" +
                                regra.Id_wms_regra_cd + ")");
                        }

        private string IniciaisEmMaisculas(string text)
                        else
                        {
            string retorno = "";
            string palavras_minusculas = "|A|AS|AO|AOS|À|ÀS|E|O|OS|UM|UNS|UMA|UMAS" +
                "|DA|DAS|DE|DO|DOS|EM|NA|NAS|NO|NOS|COM|SEM|POR|PELO|PELA|PARA|PRA|P/|S/|C/|TEM|OU|E/OU|ATE|ATÉ|QUE|SE|QUAL|";
            string palavras_maiusculas = "|II|III|IV|VI|VII|VIII|IX|XI|XII|XIII|XIV" +
                "|XV|XVI|XVII|XVIII|XIX|XX|XXI|XXII|XXIII|S/A|S/C|AC|AL|AM|AP|BA|CE|DF|ES|GO" +
                "|MA|MG|MS|MT|PA|PB|PE|PI|PR|RJ|RN|RO|RR|RS|SC|SE|SP|TO|ME|EPP|";
                            itemRegra.TwmsRegraCdXUf = new t_WMS_REGRA_CD_X_UF
                            {
                                Id = wmsRegraCdXUf.Id,
                                Id_wms_regra_cd = wmsRegraCdXUf.Id_wms_regra_cd,
                                Uf = wmsRegraCdXUf.Uf,
                                St_inativo = wmsRegraCdXUf.St_inativo
                            };

            string letra;
            string palavra = "";
            string frase = "";
            string s;
            bool blnAltera;
                            //buscar a sigla tipo pessoa
                            var tipo_pessoa = UtilsProduto.MultiCdRegraDeterminaPessoa(tcliente.Tipo,
                                tcliente.Contribuinte_Icms_Status, tcliente.Produtor_Rural_Status);

            string[] teste = text.Split(' ');
                            var wmsRegraCdXUfXPessoaTask = from c in db.TwmsRegraCdXUfPessoas
                                                           where c.Id_wms_regra_cd_x_uf == itemRegra.TwmsRegraCdXUf.Id &&
                                                                 c.Tipo_pessoa == tipo_pessoa
                                                           select c;

            string char34 = Convert.ToString((char)34);
                            var wmsRegraCdXUfXPessoa = await wmsRegraCdXUfXPessoaTask.FirstOrDefaultAsync();

            foreach (string t in teste)
                            if (wmsRegraCdXUfXPessoa == null)
                            {
                string texto = t;
                for (int i = 0; i < texto.Length; i++)
                                itemRegra.St_Regra = false;
                                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                    tcliente.Uf + "' e '" +
                                    Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                    "': regra associada ao produto (" + produto.Fabricante + ")" +
                                    produto.Produto + " não está cadastrada para a UF '" + tcliente.Uf +
                                    "' (Id=" + regra.Id_wms_regra_cd + ")");
                            }
                            else
                            {
                    letra = texto.Substring(i, 1);
                    palavra += letra;
                                itemRegra.TwmsRegraCdXUfXPessoa = new t_WMS_REGRA_CD_X_UF_X_PESSOA
                                {
                                    Id = wmsRegraCdXUfXPessoa.Id,
                                    Id_wms_regra_cd_x_uf = wmsRegraCdXUfXPessoa.Id_wms_regra_cd_x_uf,
                                    Tipo_pessoa = wmsRegraCdXUfXPessoa.Tipo_pessoa,
                                    St_inativo = wmsRegraCdXUfXPessoa.St_inativo,
                                    Spe_id_nfe_emitente = wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                };

                    if ((letra == " ") || (i == texto.Length - 1) || (letra == "(") || (letra == ")") || (letra == "[") || (letra == "]")
                        || (letra == "'") || (letra == char34) || (letra == "-"))
                                if (wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                                {
                        s = "|" + palavra.ToUpper().Trim() + "|";
                        if (palavras_minusculas.IndexOf(s) != 0 && frase != "")
                        {
                            //SE FOR FINAL DA FRASE, DEIXA INALTERADO(EX: BLOCO A)
                            if (i < texto.Length && texto.Length < 1)
                                palavra = palavra.ToLower();
                                    itemRegra.St_Regra = false;
                                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                        tcliente.Uf + "' e '" +
                                        Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                        "': regra associada ao produto (" + produto.Fabricante + ")" +
                                        produto.Produto +
                                        " não especifica nenhum CD para aguardar produtos sem presença no estoque (Id="
                                        + regra.Id_wms_regra_cd + ")");
                                }
                        else if (palavras_maiusculas.IndexOf(s) >= 0)
                            palavra = palavra.ToUpper();
                                else
                                {
                            //ANALISA SE CONVERTE O TEXTO OU NÃO
                            blnAltera = true;
                            if (TemDigito(palavra))
                                    var nfEmitenteTask = from c in db.TnfEmitentes
                                                         where c.Id == wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                                         select c;
                                    var nfEmitente = await nfEmitenteTask.FirstOrDefaultAsync();

                                    if (nfEmitente != null)
                                    {
                                //ENDEREÇOS CUJO Nº DA RESIDÊNCIA SÃO SEPARADOS POR VÍRGULA, SEM NENHUM ESPAÇO EM BRANCO
                                //CASO CONTRÁRIO, CONSIDERA QUE É ALGUM TIPO DE CÓDIGO
                                if (palavra.IndexOf(",") != 0)
                                    blnAltera = false;
                                        if (nfEmitente.St_Ativo != 1)
                                        {
                                            itemRegra.St_Regra = false;
                                            lstErros.Add("Falha na regra de consumo do estoque para a UF '" +
                                                tcliente.Uf + "' e '" +
                                                Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                                "': regra associada ao produto (" + produto.Fabricante +
                                                ")" + produto.Produto +
                                                " especifica um CD para aguardar produtos sem presença no estoque que não está habilitado " +
                                                "(Id=" + regra.Id_wms_regra_cd + ")");
                                        }
                            if (palavra.IndexOf(".") >= 0)
                                    }
                                    var wmsRegraCdXUfXPessoaXcdTask = from c in db.TwmsRegraCdXUfXPessoaXCds
                                                                      where c.Id_wms_regra_cd_x_uf_x_pessoa == wmsRegraCdXUfXPessoa.Id
                                                                      orderby c.Ordem_prioridade
                                                                      select c;
                                    var wmsRegraCdXUfXPessoaXcd = await wmsRegraCdXUfXPessoaXcdTask.ToListAsync();

                                    if (wmsRegraCdXUfXPessoaXcd.Count == 0)
                                    {
                                if (palavra.IndexOf(palavra, palavra.IndexOf(".") + 1, StringComparison.OrdinalIgnoreCase.CompareTo(".")) != 0)
                                    blnAltera = false;
                                        itemRegra.St_Regra = false;
                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" +
                                            tcliente.Uf + "' e '" +
                                            Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                            "': regra associada ao produto (" + produto.Fabricante + ")" +
                                            produto.Produto +
                                            " não especifica nenhum CD para consumo do estoque (Id=" +
                                            regra.Id_wms_regra_cd + ")");
                                    }
                            if (palavra.IndexOf("/") != 0)
                                    else
                                    {
                                if (palavra.Length <= 4)
                                    blnAltera = false;
                                        itemRegra.TwmsCdXUfXPessoaXCd = new List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD>();
                                        foreach (var i in wmsRegraCdXUfXPessoaXcd)
                                        {
                                            t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
                                            {
                                                Id = i.Id,
                                                Id_wms_regra_cd_x_uf_x_pessoa = i.Id_wms_regra_cd_x_uf_x_pessoa,
                                                Id_nfe_emitente = i.Id_nfe_emitente,
                                                Ordem_prioridade = i.Ordem_prioridade,
                                                St_inativo = i.St_inativo
                                            };

                                            var nfeCadastroPrincipalTask = from c in db.TnfEmitentes
                                                                           where c.Id == item_cd_uf_pess_cd.Id_nfe_emitente
                                                                           select c;

                                            var nfeCadastroPrincipal = await nfeCadastroPrincipalTask.FirstOrDefaultAsync();

                                            if (nfeCadastroPrincipal != null)
                                            {
                                                if (nfeCadastroPrincipal.St_Ativo != 1)
                                                    item_cd_uf_pess_cd.St_inativo = 1;
                                            }
                            //verifica se tem vogal
                            if (!TemVogal(palavra))
                                blnAltera = false;

                            if (blnAltera)
                                palavra = palavra.Substring(0, 1).ToUpper() + palavra.Substring(1, palavra.Length - 1).ToLower();
                                            itemRegra.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
                                        }
                        if (retorno.Length > 0)
                                        foreach (var i in itemRegra.TwmsCdXUfXPessoaXCd)
                                        {
                            frase = frase + " " + palavra;

                        }
                        else
                                            if (i.Id_nfe_emitente == itemRegra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                            {
                            frase = frase + palavra;
                                                if (i.St_inativo == 1)
                                                    lstErros.Add(
                                                        "Falha na leitura da regra de consumo do estoque para a UF '" +
                                                        tcliente.Uf + "' e '" +
                                                        Util.DescricaoMultiCDRegraTipoPessoa(tcliente.Tipo) +
                                                        "': regra associada ao produto (" +
                                                        produto.Fabricante + ")" + produto.Produto +
                                                        " especifica o CD '" + Util.ObterApelidoEmpresaNfeEmitentes(wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente,
                                                        contextoProvider) +
                                                        "' para alocação de produtos sem presença no estoque, sendo que este CD está desativado para " +
                                                        "processar produtos disponíveis (Id=" + regra.Id_wms_regra_cd + ")");
                                            }
                        palavra = "";
                        retorno += frase;
                        frase = "";
                                        }
                                    }
                                }

            return retorno;
                            }
                        }
                        lstRegrasCrtlEstoque.Add(itemRegra);
                    }
                }
            }
            return lstRegrasCrtlEstoque;
        }

        public static void VerificarRegrasAssociadasParaUMProduto(List<RegrasBll> lst, List<string> lstErros,
            Tcliente cliente, int id_nfe_emitente_selecao_manual)
        {

            /*id_nfe_emitente_selecao_manual = 0;*///esse é a seleção do checkebox 


        private bool TemVogal(string texto)
            foreach (var i in lst)
            {
            bool retorno = false;
            string letra = "";

            for (int i = 0; i < texto.Length; i++)
                if (!string.IsNullOrEmpty(i.Produto))
                {
                letra = texto.Substring(i, 1).ToUpper();
                if (letra == "A" || letra == "E" || letra == "I" || letra == "O" || letra == "U")
                    retorno = true;
                    if (i.TwmsRegraCd.Id == 0)
                    {
                        lstErros.Add("Produto (" + i.Fabricante + ")" + i.Produto +
                            " não possui regra de consumo do estoque associada");
                    }

            return retorno;
                    else if (i.St_Regra == false)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                            "' associada ao produto (" + i.Fabricante + ")" + i.Produto + " está desativada");
                    }

        private bool TemDigito(string texto)
                    else if (i.TwmsRegraCdXUf.St_inativo == 1)
                    {
            int ehNumero;
            bool retorno = false;

            for (int i = 0; i < texto.Length; i++)
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                            "' associada ao produto (" + i.Fabricante + ")" + i.Produto + " está bloqueada para a UF '" +
                            cliente.Uf + "'");
                    }
                    else if (i.TwmsRegraCdXUfXPessoa.St_inativo == 1)
                    {
                if (int.TryParse(texto.Substring(i, 1), out ehNumero))
                    retorno = true;
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido + "' associada ao produto (" +
                            i.Fabricante + ")" + i.Produto + " está bloqueada para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }

            return retorno;
                    else if (i.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                    {
                        lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                            "' associada ao produto (" + i.Fabricante + ")" + i.Produto +
                            " não especifica nenhum CD para aguardar produtos sem presença no estoque para clientes '" +
                            cliente.Tipo + "' da UF '" + cliente.Uf + "'");
                    }
                    else
                    {
                        int qtdeCdAtivo = 0;
                        foreach (var t in i.TwmsCdXUfXPessoaXCd)
                        {
                            if (t.Id_nfe_emitente > 0)
                            {
                                if (t.St_inativo == 0)
                                {
                                    qtdeCdAtivo = qtdeCdAtivo + 1;
                                }
                            }
                        }

        private async Task<string> ObterNomeTransportadora(string idTransportadora)
                        if (qtdeCdAtivo == 0 && id_nfe_emitente_selecao_manual == 0)
                        {
            var db = contextoProvider.GetContextoLeitura();
                            lstErros.Add("Regra de consumo do estoque '" + i.TwmsRegraCd.Apelido +
                                "' associada ao produto (" + i.Fabricante + ")" + i.Produto +
                                " não especifica nenhum CD ativo para clientes '" + cliente.Tipo +
                                "' da UF '" + cliente.Uf + "'");

            var transportadora = from c in db.Ttransportadoras
                                 where c.Id == idTransportadora
                                 select c.Nome;
            var retorno = await transportadora.Select(r => r.ToString()).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(retorno))
                retorno = idTransportadora + " (" + retorno + ")";

            return retorno;
                        }
                    }
                }
            }
        }

        private async Task<StatusPedidoPedidoDados> MontarDtoStatuPedido(Tpedido p)
        public async Task VerificarCDHabilitadoTodasRegras(List<RegrasBll> lstRegras,
            int id_nfe_emitente_selecao_manual, List<string> lstErros)
        {
            /*
             * Buscar o pedido para:
             * verificar se o st_entrega ==  ST_ENTREGA_ENTREGUE
             * se pedido.PedidoRecebidoStatus == COD_ST_PEDIDO_RECEBIDO_SIM => se o status é entregue monta como esta sendo feito
             * se o status é não entregue iremos mostrar apenas a data do pedido entre "(data do pedido)"
             * 
             */
            //id_nfe_emitente_selecao_manual = 0;//esse é a seleção do checkebox 
            bool desativado = false;
            bool achou = false;
            List<string> lstErrosAux = new List<string>();

            var db = contextoProvider.GetContextoLeitura();
            var countTask = from c in db.TpedidoItemDevolvidos
                            where c.Pedido == p.Pedido
                            select c;
            int countItemDevolvido = await countTask.CountAsync();

            StatusPedidoPedidoDados status = new StatusPedidoPedidoDados();

            if (!String.IsNullOrEmpty(p.Pedido_Bs_X_Marketplace))
            foreach (var i in lstRegras)
            {
                status.Descricao_Pedido_Bs_X_Marketplace = Util.ObterDescricao_Cod("PedidoECommerce_Origem", p.Marketplace_codigo_origem, contextoProvider) + ":" + p.Pedido_Bs_X_Marketplace;
            }
            if (!String.IsNullOrEmpty(p.Pedido_Bs_X_Ac))
                achou = false;
                desativado = false;
                if (i.Produto != "")


                {
                status.Cor_Pedido_Bs_X_Ac = "purple";
                status.Pedido_Bs_X_Ac = p.Pedido_Bs_X_Ac;
                    foreach (var t in i.TwmsCdXUfXPessoaXCd)
                    {
                        if (t.Id_nfe_emitente == id_nfe_emitente_selecao_manual)
                        {
                            achou = true;
                            if (t.St_inativo == 1)
                            {
                                desativado = true;
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

        private string Formata_hhmmss_para_hh_minuto(string hora)
                    }
                }
                if (!achou)
                {
            string hh = "";
            string mm = "";
            string retorno = "";

            if (!string.IsNullOrEmpty(hora))
                    lstErrosAux.Add("Produto (" + i.Fabricante + ")" + i.Produto + ": regra '"
                        + i.TwmsRegraCd.Apelido + "' (Id=" + i.TwmsRegraCd.Id + ") não permite o CD '" +
                        await UtilsGlobais.Util.ObterApelidoEmpresaNfeEmitentes(id_nfe_emitente_selecao_manual, contextoProvider));
                }
                else if (desativado)
                {
                hh = hora.Substring(0, 2);
                mm = hora.Substring(2, 2);

                retorno = hh + ":" + mm;
                    lstErrosAux.Add("Regra '" + i.TwmsRegraCd.Apelido + "'(Id = " + i.TwmsRegraCd.Id + ") define o CD '" +
                        await Util.ObterApelidoEmpresaNfeEmitentes(id_nfe_emitente_selecao_manual, contextoProvider) +
                        "' como 'desativado'");
                }

            return retorno;
            }

        private string Formata_hhmmss_para_hh_minuto_ss(string hora)
            if (lstErrosAux.Count > 0)
            {
            string hh = "";
            string mm = "";
            string ss = "";
            string retorno = "";

            if (!string.IsNullOrEmpty(hora))
                //não iremos utilizar essa msg, mas deixaremos aqui caso necessite
                //string erro = "O CD selecionado manualmente não pode ser usado devido aos seguintes motivos:";
                foreach (var e in lstErrosAux)
                {
                hh = hora.Substring(0, 2);
                mm = hora.Substring(2, 2);

                retorno = hh + ":" + mm + ":" + ss;
                    lstErros.Add(e);
                }

            return retorno;
            }
        }

        private string CorStatusEntrega(string st_entrega, int countItemDevolvido)
        public async Task ObterDisponibilidadeEstoque(List<RegrasBll> lstRegrasCrtlEstoque, PedidoProdutoPedidoDados produto,
            List<string> lstErros, int id_nfe_emitente_selecao_manual)
        {
            string cor = "black";
            //int id_nfe_emitente_selecao_manual = 0;

            switch (st_entrega)
            foreach (var r in lstRegrasCrtlEstoque)
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
                if (!string.IsNullOrEmpty(r.Produto))
                {
                    if (r.TwmsRegraCd != null)
                    {
                        foreach (var p in r.TwmsCdXUfXPessoaXCd)
                        {
                            if (p.Id_nfe_emitente > 0 &&
                                (id_nfe_emitente_selecao_manual == 0 || p.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                            {
                                if (p.St_inativo == 0 || id_nfe_emitente_selecao_manual != 0)
                                {
                                    if (r.Fabricante == produto.Fabricante && r.Produto == produto.Produto)
                                    {
                                        p.Estoque_Fabricante = produto.Fabricante;
                                        p.Estoque_Produto = produto.Produto;
                                        p.Estoque_DescricaoHtml = produto.Descricao;
                                        p.Estoque_Qtde_Solicitado = produto.Qtde;//essa variavel não deve ser utilizada, a qtde só sera solicitada 
                                        //quando o usuario inserir a qtde 
                                        p.Estoque_Qtde = 0;
                                        if (!await EstoqueVerificaDisponibilidadeIntegralV2(p, produto))
                                        {
                                            lstErros.Add("Falha ao tentar consultar disponibilidade no estoque do produto (" +
                                                r.Fabricante + ")" + r.Produto);
                                        }

            return cor;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private string FormataSatusPedido(string status)
        public async Task<bool> EstoqueVerificaDisponibilidadeIntegralV2(t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD regra,
            PedidoProdutoPedidoDados produto)
        {
            string retorno = "";

            switch (status)
            bool retorno = false;
            if (regra.Estoque_Qtde_Solicitado > 0 && regra.Estoque_Produto != "")
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
                var retornaRegra = await BuscarListaQtdeEstoque(regra);
                produto.Qtde_estoque_total_disponivel = retornaRegra.Estoque_Qtde_Estoque_Global;
                retorno = true;
            }

            return retorno;
        }

        private string CorSatusPagto(string statusPagto)
        private async Task<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD> BuscarListaQtdeEstoque(t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD regra)
        {
            string retorno = "";
            var db = contextoProvider.GetContextoLeitura();
            int qtde = 0;
            int qtdeUtilizada = 0;
            int saldo = 0;

            switch (statusPagto)
            if (regra.Estoque_Qtde_Solicitado > 0 && !string.IsNullOrEmpty(regra.Estoque_Produto))
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
                var estoqueCDTask = (from c in db.TestoqueItems.Include(r => r.Testoque)
                                     where c.Testoque.Id_nfe_emitente == regra.Id_nfe_emitente &&
                                           c.Fabricante == regra.Estoque_Fabricante &&
                                           c.Produto == regra.Estoque_Produto &&
                                           (c.Qtde - c.Qtde_utilizada) > 0
                                     select new
                                     {
                                         qtde = c.Qtde ?? 0,
                                         qtdeUtilizada = c.Qtde_utilizada ?? 0
                                     });
                if (estoqueCDTask != null)
                {
                    qtde = await estoqueCDTask.SumAsync(x => x.qtde);
                    qtdeUtilizada = await estoqueCDTask.SumAsync(x => x.qtdeUtilizada);
                    saldo = qtde - qtdeUtilizada;
                    regra.Estoque_Qtde = (short)(qtde - qtdeUtilizada);

            return retorno;
        }

        private string CorAnaliseCredito(string codigo)
                    var estoqueGlobalTask = (from c in db.TestoqueItems.Include(r => r.Testoque)
                                             where c.Fabricante == regra.Estoque_Fabricante &&
                                                   c.Produto == regra.Estoque_Produto &&
                                                   (c.Qtde - c.Qtde_utilizada) > 0 &&
                                                   (c.Testoque.Id_nfe_emitente == regra.Id_nfe_emitente ||
                                                    db.TnfEmitentes.Where(r => r.St_Habilitado_Ctrl_Estoque == 1 && r.St_Ativo == 1)
                                                    .Select(r => r.Id).Contains(c.Testoque.Id_nfe_emitente))
                                             select new
                                             {
            if (codigo == null)
                return "";
                                                 qtde = c.Qtde ?? 0,
                                                 qtdeUtilizada = c.Qtde_utilizada ?? 0
                                             });
                    qtde = await estoqueGlobalTask.SumAsync(x => x.qtde);
                    qtdeUtilizada = await estoqueGlobalTask.SumAsync(x => x.qtdeUtilizada);
                    saldo = qtde - qtdeUtilizada;
                    regra.Estoque_Qtde_Estoque_Global = (short)(qtde - qtdeUtilizada);
                }

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
            }

            return retorno;
            return regra;
        }

        private async Task<IEnumerable<OcorrenciasPedidoDados>> ObterOcorrencias(string numPedido)
        private bool VerificarEstoqueInsuficienteUMProduto(List<RegrasBll> lstRegras, PedidoProdutoPedidoDados produto,
            int id_nfe_emitente_selecao_manual, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();
            bool retorno = false;
            int qtde_estoque_total_disponivel = 0;

            var leftJoin = (from a in db.TcodigoDescricaos.Where(x => x.Grupo ==
                                Constantes.GRUPO_T_CODIGO_DESCRICAO__OCORRENCIAS_EM_PEDIDOS__MOTIVO_ABERTURA)
                            join b in db.TpedidoOcorrencias
                                 on a.Codigo equals b.Cod_Motivo_Abertura into juncao
                            from j in juncao.Where(x => x.Pedido == numPedido).ToList()
                            select new
            if (!string.IsNullOrEmpty(produto.Produto))
            {
                                tcodigo_descricao = a,
                                juncao = j
                            }).ToList();
                foreach (var regra in lstRegras)
                {
                    if (!string.IsNullOrEmpty(regra.Produto))
                    {
                        foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                        {
                            if (r.Id_nfe_emitente > 0 &&
                                (id_nfe_emitente_selecao_manual == 0 || r.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                            {
                                if (r.St_inativo == 0 || id_nfe_emitente_selecao_manual != 0)
                                {
                                    if (regra.Fabricante == produto.Fabricante && regra.Produto == produto.Produto)
                                    {
                                        qtde_estoque_total_disponivel += (int)r.Estoque_Qtde;
                                    }
                                }
                            }
                        }
                    }
                }

            List<OcorrenciasPedidoDados> lista = new List<OcorrenciasPedidoDados>();

            leftJoin.ForEach(async x =>
                if (qtde_estoque_total_disponivel == 0)
                {
                //objeto para ser adicionado na lista de retorno
                OcorrenciasPedidoDados ocorre = new OcorrenciasPedidoDados();
                ocorre.Usuario = x.juncao.Usuario_Cadastro;
                ocorre.Dt_Hr_Cadastro = x.juncao.Dt_Hr_Cadastro;
                if (x.juncao.Finalizado_Status != 0)
                    ocorre.Situacao = "FINALIZADA";
                    produto.Qtde_estoque_total_disponivel = 0;
                    lstErros.Add("PRODUTO SEM PRESENÇA NO ESTOQUE");
                    retorno = true;
                }
                else
                {
                    ocorre.Situacao = (from c in db.TpedidoOcorrenciaMensagems
                                       where c.Id_Ocorrencia == x.juncao.Id &&
                                             c.Fluxo_Mensagem == Constantes.COD_FLUXO_MENSAGEM_OCORRENCIAS_EM_PEDIDOS__CENTRAL_PARA_LOJA
                                       select c).Count() > 0 ? "EM ANDAMENTO" : "ABERTA";

                    produto.Qtde_estoque_total_disponivel = (short?)qtde_estoque_total_disponivel;
                }
            }

                ocorre.Contato = x.juncao.Contato ?? "";
                if (x.juncao.Tel_1 != "")
                    ocorre.Contato += "&nbsp;&nbsp; (" + x.juncao.Ddd_1 + ") " + Util.FormatarTelefones(x.juncao.Tel_1);
                if (x.juncao.Tel_2 != "")
                    ocorre.Contato += "   &nbsp;&nbsp;(" + x.juncao.Ddd_2 + ") " + Util.FormatarTelefones(x.juncao.Tel_2);
            return retorno;

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

        private async Task<IEnumerable<MensagemOcorrenciaPedidoDados>> ObterMensagemOcorrencia(int idOcorrencia)
        public List<RegrasBll> VerificarQtdePedidosAutoSplit(List<RegrasBll> lstRegras, List<string> lstErros,
            PedidoProdutoPedidoDados produto, int id_nfe_emitente_selecao_manual)
        {
            var db = contextoProvider.GetContextoLeitura();
            int qtde_a_alocar = 0;

            var msg = from c in db.TpedidoOcorrenciaMensagems
                      where c.Id_Ocorrencia == idOcorrencia
                      select new MensagemOcorrenciaPedidoDados
            List<RegrasBll> lstRegras_apoio = lstRegras;
            lstRegras = new List<RegrasBll>();

            if (!string.IsNullOrEmpty(produto.Produto))
            {
                          Dt_Hr_Cadastro = c.Dt_Hr_Cadastro,
                          Usuario = c.Usuario_Cadastro,
                          Loja = c.Loja,
                          Texto_Mensagem = c.Texto_Mensagem
                      };
                qtde_a_alocar = (int)produto.Qtde;

            return await Task.FromResult(msg);
                foreach (var regra in lstRegras_apoio)
                {
                    if (qtde_a_alocar == 0)
                        break;
                    if (!string.IsNullOrEmpty(regra.Produto))
                    {
                        foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                        {
                            if (qtde_a_alocar == 0)
                                break;
                            if (re.Id_nfe_emitente > 0 &&
                                (id_nfe_emitente_selecao_manual == 0 || re.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                            {
                                if (re.St_inativo == 0 || id_nfe_emitente_selecao_manual != 0)
                                {
                                    if (regra.Fabricante == produto.Fabricante && regra.Produto == produto.Produto)
                                    {
                                        if (re.Estoque_Qtde >= qtde_a_alocar)
                                        {
                                            re.Estoque_Qtde_Solicitado = (short)qtde_a_alocar;
                                            qtde_a_alocar = 0;
                                        }

        private async Task<string> ObterAnaliseCredito(string codigo, string numPedido, string apelido)
                                        else if (re.Estoque_Qtde > 0)
                                        {
            string retorno = "";
                                            re.Estoque_Qtde_Solicitado = re.Estoque_Qtde;
                                            qtde_a_alocar = qtde_a_alocar - re.Estoque_Qtde;
                                            lstRegras.Add(regra);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    lstRegras.Add(regra);
                }

            switch (codigo)
                if (qtde_a_alocar > 0)
                {
                case Constantes.COD_AN_CREDITO_ST_INICIAL:
                    retorno = "";
                    foreach (var regra in lstRegras_apoio)
                    {
                        if (qtde_a_alocar == 0)
                            break;
                case Constantes.COD_AN_CREDITO_PENDENTE:
                    retorno = "Pendente";
                        if (!string.IsNullOrEmpty(regra.Produto))
                        {
                            foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (qtde_a_alocar == 0)
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
                    retorno = "";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_CARTAO:
                    retorno = "Pendente Cartão de Crédito";
                    break;
                                if (id_nfe_emitente_selecao_manual == 0)
                                {
                                    //seleção automática
                                    if (regra.Fabricante == produto.Fabricante && regra.Produto == produto.Produto &&
                                        re.Id_nfe_emitente > 0 &&
                                        re.Id_nfe_emitente == regra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                    {
                                        re.Estoque_Qtde_Solicitado ??= 0;
                                        re.Estoque_Qtde_Solicitado = (short)(re.Estoque_Qtde_Solicitado + qtde_a_alocar);
                                        qtde_a_alocar = 0;
                                    }
                                }
                                else
                                {
                                    //seleção manual
                                    if (regra.Fabricante == produto.Fabricante &&
                                       regra.Produto == produto.Produto &&
                                       re.Id_nfe_emitente > 0 &&
                                       re.Id_nfe_emitente == regra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                    {
                                        re.Estoque_Qtde_Solicitado ??= 0;
                                        re.Estoque_Qtde_Solicitado = (short)(re.Estoque_Qtde_Solicitado + qtde_a_alocar);
                                        qtde_a_alocar = 0;//verificar esse valor
                                    }
                                }
                            }
                        }

            if (retorno != "")
                        lstRegras.Add(regra);
                    }
                }
                if (qtde_a_alocar > 0)
                {
                var db = contextoProvider.GetContextoLeitura();
                    lstErros.Add("Falha ao processar a alocação de produtos no estoque: restaram " +
                        qtde_a_alocar + " unidades do produto (" +
                        produto.Fabricante + ")" + produto.Produto +
                        " que não puderam ser alocados na lista de produtos sem presença no estoque de nenhum CD");
                }
            }

                var ret = from c in db.Tpedidos
                          where c.Pedido == numPedido && c.Orcamentista == apelido
                          select new { analise_credito_data = c.Analise_credito_Data, analise_credito_usuario = c.Analise_Credito_Usuario };
            return lstRegras;
        }

                var registro = ret.FirstOrDefault();
                if (registro != null)
        private List<int> ContagemEmpresasUsadasAutoSplit(List<RegrasBll> lstRegras, int id_nfe_emitente_selecao_manual)
        {
                    if (registro.analise_credito_data.HasValue)
            int qtde_empresa_selecionada = 0;
            List<int> lista_empresa_selecionada = new List<int>();

            foreach (var regra in lstRegras)
            {
                        if (!string.IsNullOrEmpty(registro.analise_credito_usuario))
                if (!string.IsNullOrEmpty(regra.Produto))
                {
                            string maiuscula = (Char.ToUpper(registro.analise_credito_usuario[0]) +
                                registro.analise_credito_usuario.Substring(1).ToLower());

                            retorno = retorno + " (" + registro.analise_credito_data?.ToString("dd/MM/yyyy HH:mm") + " - "
                                + maiuscula + ")";
                    foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                    {
                        if (r.Id_nfe_emitente > 0 &&
                            (id_nfe_emitente_selecao_manual == 0 || r.Id_nfe_emitente == id_nfe_emitente_selecao_manual))
                        {
                            if (r.Estoque_Qtde_Solicitado > 0)
                            {
                                qtde_empresa_selecionada++;
                                lista_empresa_selecionada.Add(r.Id_nfe_emitente);
                            }
                        }
                    }
                }
            }

            return await Task.FromResult(retorno);
            return lista_empresa_selecionada;
        }

        private async Task<decimal> CalculaSaldoAPagar(string numPedido, decimal vlDevNf)
        private async Task ExisteProdutoDescontinuado(PedidoProdutoPedidoDados produto, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();

            //buscar o valor total pago
            var vlFamiliaP = from c in db.TpedidoPagamentos
                             where c.Pedido.StartsWith(numPedido)
                             select c;
            var vl_TotalFamiliaPagoTask = vlFamiliaP.Select(r => r.Valor).SumAsync();

            //buscar valor total NF
            var vlNf = from c in db.TpedidoItems
                       where c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO && c.Tpedido.Pedido.StartsWith(numPedido)
                       select c.Qtde * c.Preco_NF;
            var vl_TotalFamiliaPrecoNFTask = vlNf.Select(r => r.Value).SumAsync();
            if (!string.IsNullOrEmpty(produto.Produto))
            {
                var produtoTask = (from c in db.Tprodutos
                                   where c.Produto == produto.Produto
                                   select c.Descontinuado).FirstOrDefaultAsync();
                var p = await produtoTask;

            decimal result = await vl_TotalFamiliaPrecoNFTask - await vl_TotalFamiliaPagoTask - vlDevNf;

            return await Task.FromResult(result);
                if (p.ToUpper() == "S")
                {
                    if (produto.Qtde > produto.Qtde_estoque_total_disponivel)
                        lstErros.Add("Produto (" + produto.Fabricante + ")" + produto.Produto +
                            " consta como 'descontinuado' e não há mais saldo suficiente " +
                            "no estoque para atender à quantidade solicitada.");
                }
            }
        }

        //Retorna o valor da Familia RA
        private async Task<decimal> CalculaTotalFamiliaRA(string numPedido)
        public class VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado_TesteRetorno
        {
            public List<RegrasBll> regrasBlls = new List<RegrasBll>();
            public List<string> prodValidadoEstoqueListaErros = new List<string>();
        }
        public async Task<VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado_TesteRetorno> VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado_Teste(PedidoProdutoPedidoDados produto,
            string cpf_cnpj, int id_nfe_emitente_selecao_manual)
        {
            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = from c in db.Tclientes
                              where c.Cnpj_Cpf == cpf_cnpj
                              select c;

            var vlTotalVendaPorItem = from c in db.TpedidoItems
                                      where c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO && c.Tpedido.Pedido.StartsWith(numPedido)
                                      select new { venda = c.Qtde * c.Preco_Venda, nf = c.Qtde * c.Preco_NF };
            Tcliente cliente = await clienteTask.FirstOrDefaultAsync();

            var vlTotalVenda = await vlTotalVendaPorItem.Select(r => r.venda).SumAsync();
            var vlTotalNf = await vlTotalVendaPorItem.Select(r => r.nf).SumAsync();
            var retorno = new VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado_TesteRetorno();

            decimal result = vlTotalNf.Value - vlTotalVenda.Value;
            //obtém  a sigla para regra
            string cliente_regra = UtilsProduto.MultiCdRegraDeterminaPessoa(cliente.Tipo, cliente.Contribuinte_Icms_Status,
                cliente.Produtor_Rural_Status);

            return await Task.FromResult(result);
        }
            //buscar o produto
            //PedidoProdutosDtoPedido produto = new PedidoProdutosDtoPedido();

        private async Task<IEnumerable<ProdutoDevolvidoPedidoDados>> BuscarProdutosDevolvidos(string numPedido)
        {
            var db = contextoProvider.GetContextoLeitura();
            List<RegrasBll> regraCrtlEstoque = (await ObterCtrlEstoqueProdutoRegraParaUMProduto(produto, cliente,
                retorno.prodValidadoEstoqueListaErros)).ToList();

            var lista = from c in db.TpedidoItemDevolvidos
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
                        };
            await UtilsProduto.ObterCtrlEstoqueProdutoRegra_Teste(retorno.prodValidadoEstoqueListaErros, regraCrtlEstoque, cliente.Uf,
                cliente_regra, contextoProvider);

            return await Task.FromResult(lista);
            VerificarRegrasAssociadasParaUMProduto(regraCrtlEstoque, retorno.prodValidadoEstoqueListaErros, cliente,
                id_nfe_emitente_selecao_manual);

            if (id_nfe_emitente_selecao_manual != 0)
                await VerificarCDHabilitadoTodasRegras(regraCrtlEstoque, id_nfe_emitente_selecao_manual,
                    retorno.prodValidadoEstoqueListaErros);

            await ObterDisponibilidadeEstoque(regraCrtlEstoque, produto, retorno.prodValidadoEstoqueListaErros,
                id_nfe_emitente_selecao_manual);

            //meto responsavel por atribuir a qtde de estoque ao produto
            //await Util.Util.VerificarEstoque(regraCrtlEstoque, produto, id_nfe_emitente_selecao_manual, contextoProvider);

            bool estoqueInsuficiente = VerificarEstoqueInsuficienteUMProduto(regraCrtlEstoque, produto,
                id_nfe_emitente_selecao_manual, retorno.prodValidadoEstoqueListaErros);

            regraCrtlEstoque = VerificarQtdePedidosAutoSplit(regraCrtlEstoque, retorno.prodValidadoEstoqueListaErros, produto, id_nfe_emitente_selecao_manual);

            List<int> lst_empresa_selecionada = ContagemEmpresasUsadasAutoSplit(regraCrtlEstoque, id_nfe_emitente_selecao_manual);

            await ExisteProdutoDescontinuado(produto, retorno.prodValidadoEstoqueListaErros);

            retorno.regrasBlls = regraCrtlEstoque;

            return retorno;
        }

        private async Task<IEnumerable<PedidoPerdasPedidoDados>> BuscarPerdas(string numPedido)

        public async Task<string> LeParametroControle(string id)
        {
            var db = contextoProvider.GetContextoLeitura();

            var lista = from c in db.TpedidoPerdas
                        where c.Pedido == numPedido
                        select new PedidoPerdasPedidoDados
                        {
                            Data = c.Data,
                            Hora = Formata_hhmmss_para_hh_minuto_ss(c.Hora),
                            Valor = c.Valor,
                            Obs = c.Obs
                        };
            string controle = await (from c in db.Tcontroles
                                     where c.Id_Nsu == id
                                     select c.Nsu).FirstOrDefaultAsync();

            return await Task.FromResult(lista);

            return controle;
        }

        private async Task<int> VerificarEstoqueVendido(string numPedido, string fabricante, string produto)
        public async Task<TtransportadoraCep> ObterTransportadoraPeloCep(string cep)
        {
            cep = cep.Replace("-", "").Trim();

            int cepteste = int.Parse(cep);
            cep = cepteste.ToString();
            var db = contextoProvider.GetContextoLeitura();

            var prod = from c in db.TestoqueMovimentos
                       where c.Anulado_Status == 0 &&
                             c.Pedido == numPedido &&
                             c.Fabricante == fabricante &&
                             c.Produto == produto &&
                             c.Estoque == Constantes.ID_ESTOQUE_VENDIDO &&
                             c.Qtde.HasValue
                       select new { qtde = (int)c.Qtde };

            int qtde = await prod.Select(r => r.qtde).SumAsync();

            return await Task.FromResult(qtde);
            TtransportadoraCep transportadoraCep = await (from c in db.TtransportadoraCeps
                                                          where (c.Tipo_range == 1 && c.Cep_unico == cep) ||
                                                                (
                                                                    c.Tipo_range == 2 &&
                                                                     (
                                                                         c.Cep_faixa_inicial.CompareTo(cep) <= 0 &&
                                                                         c.Cep_faixa_final.CompareTo(cep) >= 0
                                                                      )
                                                                )
                                                          select c).FirstOrDefaultAsync();

            return transportadoraCep;
        }

        private async Task<int> VerificarEstoqueSemPresenca(string numPedido, string fabricante, string produto)
        public async Task<int> Fin_gera_nsu(string id_nsu, List<string> lstErros, ContextoBdGravacao dbgravacao)
        {
            var db = contextoProvider.GetContextoLeitura();
            int intRetorno = 0;
            //int intRecordsAffected = 0;
            //int intQtdeTentativas, intNsuUltimo, intNsuNovo;
            //bool blnSucesso = true;
            int nsu = 0;

            var prod = from c in db.TestoqueMovimentos
                       where c.Anulado_Status == 0 &&
                             c.Pedido == numPedido &&
                             c.Fabricante == fabricante &&
                             c.Produto == produto &&
                             c.Estoque == Constantes.ID_ESTOQUE_SEM_PRESENCA &&
                             c.Qtde.HasValue
                       select new { qtde = (int)c.Qtde };
            //conta a qtde de id
            var qtdeIdFin = from c in dbgravacao.TfinControles
                            where c.Id == id_nsu
                            select c.Id;

            int qtde = await prod.Select(r => r.qtde).SumAsync();

            return await Task.FromResult(qtde);
            if (qtdeIdFin != null)
            {
                intRetorno = await qtdeIdFin.CountAsync();
            }

        private string ObterCorFaltante(int qtde, int qtde_estoque_vendido, int qtde_estoque_sem_presenca)
            //não está cadastrado, então cadastra agora 
            if (intRetorno == 0)
            {
            string retorno = "";
                //criamos um novo para salvar
                TfinControle tfinControle = new TfinControle();

            if (qtde <= 0 || qtde != (qtde_estoque_vendido + qtde_estoque_sem_presenca))
                retorno = "black";
            if (qtde_estoque_vendido != 0 && qtde_estoque_sem_presenca != 0)
                retorno = "darkorange";
            else if (qtde_estoque_sem_presenca == 0)
                retorno = "black";
            else if (qtde_estoque_vendido == 0)
                retorno = "red";
                tfinControle.Id = id_nsu;
                tfinControle.Nsu = 0;
                tfinControle.Dt_hr_ult_atualizacao = DateTime.Now;

            return retorno;
                dbgravacao.Add(tfinControle);

            }

        private async Task<IEnumerable<BlocoNotasPedidoDados>> BuscarPedidoBlocoNotas(string numPedido)
        {
            var db = contextoProvider.GetContextoLeitura();
            //laço de tentativas para gerar o nsu(devido a acesso concorrente)

            var bl = from c in db.TpedidoBlocosNotas
                     where c.Pedido == numPedido &&
                           c.Nivel_Acesso == Constantes.COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__PUBLICO &&
                           c.Anulado_Status == 0
                     select c;

            List<BlocoNotasPedidoDados> lstBlocoNotas = new List<BlocoNotasPedidoDados>();
            //obtém o último nsu usado
            var tfincontroleEditando = await (from c in dbgravacao.TfinControles
                                              where c.Id == id_nsu
                                              select c).FirstOrDefaultAsync();

            foreach (var i in bl)

            if (tfincontroleEditando == null)
            {
                BlocoNotasPedidoDados bloco = new BlocoNotasPedidoDados
                {
                    Dt_Hora_Cadastro = i.Dt_Hr_Cadastro,
                    Usuario = i.Usuario,
                    Loja = i.Loja,
                    Mensagem = i.Mensagem
                };
                lstBlocoNotas.Add(bloco);
                lstErros.Add("Falha ao localizar o registro para geração de NSU (" + id_nsu + ")!");
                return nsu;
            }

            return await Task.FromResult(lstBlocoNotas);

            tfincontroleEditando.Id = id_nsu;
            tfincontroleEditando.Nsu++;
            tfincontroleEditando.Dt_hr_ult_atualizacao = DateTime.Now;
            //tenta atualizar o banco de dados
            dbgravacao.Update(tfincontroleEditando);

            await dbgravacao.SaveChangesAsync();

            return tfincontroleEditando.Nsu;
        }

        private async Task<IEnumerable<BlocoNotasDevolucaoMercadoriasPedidoDados>> BuscarPedidoBlocoNotasDevolucao(string numPedido)
        public async Task<bool> Grava_log_estoque_v2(string strUsuario, short id_nfe_emitente, string strFabricante,
            string strProduto, short intQtdeSolicitada, short intQtdeAtendida, string strOperacao,
            string strCodEstoqueOrigem, string strCodEstoqueDestino, string strLojaEstoqueOrigem,
            string strLojaEstoqueDestino, string strPedidoEstoqueOrigem, string strPedidoEstoqueDestino,
            string strDocumento, string strComplemento, string strIdOrdemServico, ContextoBdGravacao contexto)
        {
            var db = contextoProvider.GetContextoLeitura();

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
            TestoqueLog testoqueLog = new TestoqueLog();

            if (blDevolucao.Count() == 0)
                return new List<BlocoNotasDevolucaoMercadoriasPedidoDados>();
            testoqueLog.data = DateTime.Now.Date;
            testoqueLog.Data_hora = DateTime.Now;
            testoqueLog.Usuario = strUsuario;
            testoqueLog.Id_nfe_emitente = id_nfe_emitente;
            testoqueLog.Fabricante = strFabricante;
            testoqueLog.Produto = strProduto;
            testoqueLog.Qtde_solicitada = intQtdeSolicitada;
            testoqueLog.Qtde_atendida = intQtdeAtendida;
            testoqueLog.Operacao = strOperacao;
            testoqueLog.Cod_estoque_origem = strCodEstoqueOrigem;
            testoqueLog.Cod_estoque_destino = strCodEstoqueDestino;
            testoqueLog.Loja_estoque_origem = strLojaEstoqueOrigem;
            testoqueLog.Loja_estoque_destino = strLojaEstoqueDestino;
            testoqueLog.Pedido_estoque_origem = strPedidoEstoqueOrigem;
            testoqueLog.Pedido_estoque_destino = strPedidoEstoqueDestino;
            testoqueLog.Documento = strDocumento;
            testoqueLog.Complemento = strComplemento.Length > 80 ? strComplemento.Substring(0, 80) : strComplemento;
            testoqueLog.Id_ordem_servico = strIdOrdemServico;

            List<BlocoNotasDevolucaoMercadoriasPedidoDados> lista = new List<BlocoNotasDevolucaoMercadoriasPedidoDados>();
            contexto.Add(testoqueLog);
            await contexto.SaveChangesAsync();

            foreach (var b in blDevolucao)

            return true;
        }

        public void ConsisteProdutosValorZerados(List<PedidoProdutoPedidoDados> lstProdutos, List<string> lstErros,
            bool comIndicacao, short PermiteRaStatus)
        {
                lista.Add(new BlocoNotasDevolucaoMercadoriasPedidoDados
            foreach (var x in lstProdutos)
            {
                    Dt_Hr_Cadastro = b.Dt_Hr_Cadastro,
                    Usuario = b.Usuario,
                    Loja = b.Loja,
                    Mensagem = b.Mensagem
                });
                if (x.Preco_Venda <= 0)
                    lstErros.Add("Produto '" + x.Produto + "' está com valor de venda zerado!");
                else if (comIndicacao && PermiteRaStatus == 1 && x.Preco_NF <= 0)
                    lstErros.Add("Produto '" + x.Produto + "' está com preço zerado!");
            };
        }

            return await Task.FromResult(lista);
        }

        private async Task<IEnumerable<string>> ObterFormaPagto(Tpedido ped)
        public async Task ValidarProdutosComFormaPagto(PedidoCriacaoDados pedidoCriacao, string siglaCustoFinancFornec,
            int qtdeParcCustoFinancFornec, List<string> lstErros)
        {
            if (pedidoCriacao.FormaPagtoCriacao.Rb_forma_pagto != Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                var db = contextoProvider.GetContextoLeitura();

            var p = from c in db.Tpedidos
                    where c.Pedido == ped.Pedido && c.Indicador == ped.Indicador
                    select c;
                foreach (var prod in pedidoCriacao.ListaProdutos)
                {
                    TpercentualCustoFinanceiroFornecedor custoFinancFornec = await (from c in db.TpercentualCustoFinanceiroFornecedors
                                                                                    where c.Fabricante == prod.Fabricante &&
                                                                                        c.Tipo_Parcelamento == siglaCustoFinancFornec &&
                                                                                        c.Qtde_Parcelas == qtdeParcCustoFinancFornec
                                                                                    select c).FirstOrDefaultAsync();

            Tpedido pedido = p.FirstOrDefault();
            List<string> lista = new List<string>();
            string parcelamento = Convert.ToString(pedido.Tipo_Parcelamento);
                    if (custoFinancFornec == null)
                        lstErros.Add("Opção de parcelamento não disponível para fornecedor " + prod.Fabricante + ": " +
                            DecodificaCustoFinanFornecQtdeParcelas(siglaCustoFinancFornec, (short)qtdeParcCustoFinancFornec) + " parcela(s).");

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
                    TprodutoLoja prodLoja = await (from c in db.TprodutoLojas.Include(x => x.Tproduto)
                                                   where c.Tproduto.Produto == prod.Produto &&
                                                   c.Tproduto.Fabricante == prod.Fabricante &&
                                                   c.Loja == pedidoCriacao.LojaUsuario
                                                   select c).FirstOrDefaultAsync();

        private string StatusPagto(string status)
        {
            string retorno = "";
                    if (prodLoja == null)
                        lstErros.Add("Produto " + prod.Produto + " não localizado para a loja " + pedidoCriacao.LojaUsuario + ".");

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

            }
        }
    }
}
