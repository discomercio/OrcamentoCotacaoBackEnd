using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using InfraBanco.Modelos;
using PrepedidoBusiness.Utils;
using PrepedidoBusiness.Dto.Prepedido;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Dto.ClienteCadastro;
using InfraBanco.Constantes;
using PrepedidoBusiness.Bll.Regras;
using InfraBanco;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using PrepedidoBusiness.Bll.ProdutoBll;
using PrepedidoBusiness.Bll.ClienteBll;

namespace PrepedidoBusiness.Bll
{
    public class PrepedidoBll
    {
        private readonly ContextoBdProvider contextoProvider;
        private readonly ClienteBll.ClienteBll clienteBll;

        public PrepedidoBll(ContextoBdProvider contextoProvider, ClienteBll.ClienteBll clienteBll)
        {
            this.contextoProvider = contextoProvider;
            this.clienteBll = clienteBll;
        }

        public async Task<IEnumerable<string>> ListarNumerosPrepedidosCombo(string orcamentista)
        {
            //toda vez precisamos de uma nova conexao para os casos em que houver transacao
            var db = contextoProvider.GetContextoLeitura();
            var lista = from r in db.Torcamentos
                        where r.Orcamentista == orcamentista &&
                              r.St_Orcamento != "CAN"
                              && r.Data >= Util.LimiteDataBuscas()
                        orderby r.Orcamento
                        select r.Orcamento;
            var res = lista.AsEnumerable();
            return await Task.FromResult(res);
        }

        public async Task<IEnumerable<string>> ListarCpfCnpjPrepedidosCombo(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var lista = (from c in db.Torcamentos
                         where c.Orcamentista == apelido &&
                               c.St_Orcamento != "CAN" &&
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

        public enum TipoBuscaPrepedido
        {
            Todos = 0, NaoViraramPedido = 1, SomenteViraramPedido = 2, Excluidos = 3
        }
        public async Task<IEnumerable<PrepedidosCadastradosDtoPrepedido>> ListarPrePedidos(string apelido, TipoBuscaPrepedido tipoBusca,
            string clienteBusca, string numeroPrePedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            if (dataInicial < Util.LimiteDataBuscas())
            {
                dataInicial = Util.LimiteDataBuscas();
            }

            //usamos a mesma lógica de PedidoBll.ListarPedidos:
            /*
             * se fizeram a busca por algum CPF ou CNPJ ou pedido e não achamos nada, removemos o filtro de datas
             * para não aparecer para o usuário que não tem nenhum registro
             * */
            var ret = await ListarPrePedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPrePedido, dataInicial, dataFinal);
            //se tiver algum registro, retorna imediatamente
            if (ret.Any())
                return ret;

            if (String.IsNullOrEmpty(clienteBusca) && String.IsNullOrEmpty(numeroPrePedido))
                return ret;

            //busca sem datas
            ret = await ListarPrePedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPrePedido, dataInicial, null);
            if (ret.Any())
                return ret;

            //ainda não achamos nada? então faz a busca sem filtrar por tipo
            ret = await ListarPrePedidosFiltroEstrito(apelido, TipoBuscaPrepedido.Todos, clienteBusca, numeroPrePedido, dataInicial, null);
            return ret;

        }

        //a busca sem malabarismos para econtrar algum registro
        public async Task<IEnumerable<PrepedidosCadastradosDtoPrepedido>> ListarPrePedidosFiltroEstrito(string apelido, TipoBuscaPrepedido tipoBusca,
                string clienteBusca, string numeroPrePedido, DateTime? dataInicial, DateTime? dataFinal)
        {

            var db = contextoProvider.GetContextoLeitura();

            var lst = db.Torcamentos.
                Where(r => r.Orcamentista == apelido);

            //filtro conforme o tipo do prepedido
            switch (tipoBusca)
            {
                case TipoBuscaPrepedido.Todos:
                    lst = lst.Where(r => r.St_Orcamento != "CAN");
                    break;
                case TipoBuscaPrepedido.NaoViraramPedido:
                    lst = lst.Where(r => (r.St_Orc_Virou_Pedido == 0) && (r.St_Orcamento != "CAN"));
                    break;
                case TipoBuscaPrepedido.SomenteViraramPedido:
                    lst = lst.Where(c => c.St_Orc_Virou_Pedido == 1);
                    break;
                case TipoBuscaPrepedido.Excluidos:
                    lst = lst.Where(c => c.St_Orcamento == "CAN");
                    break;
            }
            if (!string.IsNullOrEmpty(clienteBusca))
                lst = lst.Where(r => r.Tcliente.Cnpj_Cpf.Contains(clienteBusca));
            if (!string.IsNullOrEmpty(numeroPrePedido))
                lst = lst.Where(r => r.Orcamento.Contains(numeroPrePedido));
            if (dataInicial.HasValue)
                lst = lst.Where(r => r.Data >= dataInicial.Value);
            if (dataFinal.HasValue)
                lst = lst.Where(r => r.Data <= dataFinal.Value);


            List<PrepedidosCadastradosDtoPrepedido> lstdto = new List<PrepedidosCadastradosDtoPrepedido>();
            //COLOCAR O STATUS DO PEDIDO PARA PREPEDIDOS QUE VIRARAM PEDIDOS
            if (tipoBusca != TipoBuscaPrepedido.Excluidos)
            {
                lstdto = lst.Select(r => new PrepedidosCadastradosDtoPrepedido
                {

                    Status = r.St_Orc_Virou_Pedido == 1 ? "Pré-Pedido - Com Pedido" : "Pré-Pedido - Sem Pedido",
                    DataPrePedido = r.Data,
                    NumeroPrepedido = r.Orcamento,
                    NomeCliente = r.Tcliente.Nome,
                    ValoTotal = r.Permite_RA_Status == 1 ? r.Vl_Total_NF : r.Vl_Total
                }).OrderByDescending(r => r.DataPrePedido).ToList();
            }
            if (tipoBusca == TipoBuscaPrepedido.Excluidos)
            {
                lstdto = lst.Select(r => new PrepedidosCadastradosDtoPrepedido
                {
                    Status = "Excluído",
                    DataPrePedido = r.Data,
                    NumeroPrepedido = r.Orcamento,
                    NomeCliente = r.Tcliente.Nome,
                    ValoTotal = r.Permite_RA_Status == 1 && r.Permite_RA_Status != 0 ? r.Vl_Total_NF : r.Vl_Total
                }).OrderByDescending(r => r.DataPrePedido).ToList();
            }

            return await Task.FromResult(lstdto);
        }

        public async Task<bool> RemoverPrePedido(string numeroPrePedido, string apelido)
        {
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                var prepedido = from c in dbgravacao.Torcamentos
                                where c.Orcamentista == apelido &&
                                      c.Orcamento == numeroPrePedido &&
                                      (c.St_Orcamento == "" || c.St_Orcamento == null) &&
                                      c.St_Orc_Virou_Pedido == 0
                                select c;
                Torcamento prePedido = await prepedido.FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(prePedido.ToString()))
                {
                    prePedido.St_Orcamento = "CAN";
                    prePedido.Cancelado_Data = DateTime.Now;
                    prePedido.Cancelado_Usuario = apelido;
                    await dbgravacao.SaveChangesAsync();
                    dbgravacao.transacao.Commit();
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }

        public async Task<PrePedidoDto> BuscarPrePedido(string apelido, string numPrePedido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var prepedido = from c in db.Torcamentos
                            where c.Orcamentista == apelido && c.Orcamento == numPrePedido
                            select c;


            Torcamento pp = prepedido.FirstOrDefault();

            Tloja t = await (from c in db.Tlojas
                             where c.Loja == pp.Loja
                             select c).FirstOrDefaultAsync();

            if (pp == null)
                return null;

            string corHeader = "black";
            string textoHeader = "";
            string canceladoData = "";

            if (pp.St_Orcamento == Constantes.ST_ORCAMENTO_CANCELADO)
            {
                corHeader = "red";
                textoHeader = "CANCELADO";
                canceladoData = pp.Cancelado_Data?.ToString("dd/MM/yyyy");
            }

            if (pp.St_Orc_Virou_Pedido == 1)
            {
                var pedido = from c in db.Tpedidos
                             where c.Orcamento == numPrePedido
                             select c.Pedido;

                PrePedidoDto prepedidoPedido = new PrePedidoDto
                {
                    St_Orc_Virou_Pedido = true,
                    NumeroPedido = pedido.Select(r => r.ToString()).FirstOrDefault()
                };
                return prepedidoPedido;
            }

            var cadastroClienteTask = ObterDadosCliente(t.Razao_Social, pp.Orcamentista, pp.Vendedor, pp.Id_Cliente);
            var enderecoEntregaTask = ObterEnderecoEntrega(pp);
            var lstProdutoTask = await ObterProdutos(pp);

            var vltotalRa = lstProdutoTask.Select(r => r.VlTotalRA).Sum();
            var totalDestePedidoComRa = lstProdutoTask.Select(r => r.TotalItemRA).Sum();
            var totalDestePedido = lstProdutoTask.Select(r => r.TotalItem).Sum();


            PrePedidoDto prepedidoDto = new PrePedidoDto
            {
                CorHeader = corHeader,
                TextoHeader = textoHeader,
                CanceladoData = canceladoData,
                NumeroPrePedido = pp.Orcamento,
                DataHoraPedido = Convert.ToString(pp.Data?.ToString("dd/MM/yyyy")),
                Hora_Prepedido = Util.FormataHora(pp.Hora),
                DadosCliente = await cadastroClienteTask,
                EnderecoEntrega = await enderecoEntregaTask,
                ListaProdutos = lstProdutoTask.ToList(),
                TotalFamiliaParcelaRA = vltotalRa,
                PermiteRAStatus = pp.Permite_RA_Status,
                CorTotalFamiliaRA = vltotalRa > 0 ? "green" : "red",
                PercRT = pp.Perc_RT,
                ValorTotalDestePedidoComRA = totalDestePedidoComRa,
                VlTotalDestePedido = totalDestePedido,
                DetalhesPrepedido = ObterDetalhesPrePedido(pp, apelido),
                FormaPagto = ObterFormaPagto(pp).ToList(),
                FormaPagtoCriacao = await ObterFormaPagtoPrePedido(pp)
            };

            return await Task.FromResult(prepedidoDto);
        }

        public async Task<decimal> ObtemPercentualVlPedidoRA()
        {
            var db = contextoProvider.GetContextoLeitura();

            string percentual = await (from c in db.Tcontroles
                                       where c.Id_Nsu == Constantes.ID_PARAM_PercVlPedidoLimiteRA
                                       select c.Nsu).FirstOrDefaultAsync();

            decimal retorno = decimal.Parse(percentual);

            return retorno;
        }

        private async Task<string> ObterDescricaoFormaPagto(short av_forma_pagto)
        {
            var db = contextoProvider.GetContextoLeitura();

            var tipoTask = from c in db.TformaPagtos
                           where c.Id == av_forma_pagto
                           select c.Descricao;

            string tipo = await tipoTask.FirstOrDefaultAsync();
            return tipo;
        }

        public async Task<FormaPagtoCriacaoDto> ObterFormaPagtoPrePedido(Torcamento torcamento)
        {
            FormaPagtoCriacaoDto pagto = new FormaPagtoCriacaoDto();

            //descrição d meio de pagto
            pagto.Descricao_meio_pagto = await ObterDescricaoFormaPagto(torcamento.Av_Forma_Pagto);
            pagto.Tipo_parcelamento = torcamento.Tipo_Parcelamento;
            pagto.Qtde_Parcelas = (int)torcamento.Qtde_Parcelas;

            if (torcamento.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_A_VISTA))
            {
                pagto.Op_av_forma_pagto = torcamento.Av_Forma_Pagto.ToString();

            }
            else if (torcamento.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_PARCELA_UNICA))
            {
                pagto.Rb_forma_pagto = torcamento.Pu_Forma_Pagto.ToString();
                pagto.C_pu_valor = torcamento.Pu_Valor;
                pagto.C_pu_vencto_apos = torcamento.Pu_Vencto_Apos;
            }
            else if (torcamento.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO))
            {
                pagto.C_pc_qtde = torcamento.Pc_Qtde_Parcelas;
                pagto.C_pc_valor = torcamento.Pc_Valor_Parcela;
                pagto.C_pc_qtde = torcamento.Qtde_Parcelas;
            }
            else if (torcamento.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA))
            {
                pagto.C_pc_maquineta_valor = torcamento.Pc_Maquineta_Valor_Parcela;
                pagto.C_pc_maquineta_qtde = torcamento.Pc_Maquineta_Qtde_Parcelas;
                pagto.C_pc_maquineta_qtde = torcamento.Qtde_Parcelas;
            }
            else if (torcamento.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA))
            {
                pagto.Op_pce_entrada_forma_pagto = torcamento.Pce_Forma_Pagto_Entrada.ToString();
                pagto.Op_pce_prestacao_forma_pagto = torcamento.Pce_Forma_Pagto_Prestacao.ToString();
                pagto.C_pce_entrada_valor = torcamento.Pce_Entrada_Valor;
                pagto.C_pce_prestacao_qtde = torcamento.Pce_Prestacao_Qtde;
                pagto.C_pce_prestacao_valor = torcamento.Pce_Prestacao_Valor;
                pagto.C_pce_prestacao_periodo = torcamento.Pce_Prestacao_Periodo;
            }
            else if (torcamento.Tipo_Parcelamento == short.Parse(Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA))
            {
                pagto.Op_pse_prim_prest_forma_pagto = torcamento.Pse_Forma_Pagto_Prim_Prest.ToString();
                pagto.Op_pse_demais_prest_forma_pagto = torcamento.Pse_Forma_Pagto_Demais_Prest.ToString();
                pagto.C_pse_prim_prest_valor = torcamento.Pse_Prim_Prest_Valor;
                pagto.C_pse_prim_prest_apos = torcamento.Pse_Prim_Prest_Apos;
                pagto.C_pse_demais_prest_qtde = torcamento.Pse_Demais_Prest_Qtde;
                pagto.C_pse_demais_prest_valor = torcamento.Pse_Demais_Prest_Valor;
                pagto.C_pse_demais_prest_periodo = torcamento.Pse_Demais_Prest_Periodo;
                pagto.C_pse_demais_prest_qtde = torcamento.Qtde_Parcelas;
            }
            return pagto;
        }

        private DetalhesDtoPrepedido ObterDetalhesPrePedido(Torcamento torcamento, string apelido)
        {
            DetalhesDtoPrepedido detail = new DetalhesDtoPrepedido
            {
                Observacoes = torcamento.Obs_1,
                NumeroNF = torcamento.Obs_2,
                EntregaImediata = torcamento.St_Etg_Imediata ==
                (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO ?
                "NÃO (" + torcamento.Etg_Imediata_Usuario +
                " em " + torcamento.Etg_Imediata_Data?.ToString("dd/MM/yyyy HH:mm") + ")" :
                "SIM (" + torcamento.Etg_Imediata_Usuario +
                " em " + torcamento.Etg_Imediata_Data?.ToString("dd/MM/yyyy HH:mm") + ")",
                BemDeUso_Consumo = torcamento.StBemUsoConsumo == (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO ?
                "NÃO" : "SIM",
                InstaladorInstala = torcamento.InstaladorInstalaStatus ==
                (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO ?
                "NÃO" : "SIM",
                GarantiaIndicador = Convert.ToString(torcamento.GarantiaIndicadorStatus) ==
                Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO ?
                "NÃO" : "SIM",
                DescricaoFormaPagamento = torcamento.Forma_Pagamento
            };

            return detail;
        }

        private IEnumerable<string> ObterFormaPagto(Torcamento torcamento)
        {
            List<string> lista = new List<string>();

            switch (Convert.ToString(torcamento.Tipo_Parcelamento))
            {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    lista.Add("À Vista (" + Util.OpcaoFormaPagto(torcamento.Av_Forma_Pagto) + ")");
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    lista.Add(String.Format("Parcela Única: " + " {0:c2} (" +
                        Util.OpcaoFormaPagto(torcamento.Pu_Forma_Pagto) +
                        ") vencendo após " + torcamento.Pu_Vencto_Apos, torcamento.Pu_Valor) + " dias");
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    lista.Add(String.Format("Parcelado no Cartão (internet) em " + torcamento.Pc_Qtde_Parcelas + " x " +
                        " {0:c2}", torcamento.Pc_Valor_Parcela));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    lista.Add(String.Format("Parcelado no Cartão (maquineta) em " + torcamento.Pc_Maquineta_Qtde_Parcelas +
                        " x {0:c2}", torcamento.Pc_Maquineta_Valor_Parcela));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    lista.Add(String.Format("Entrada: " + "{0:c2} (" +
                        Util.OpcaoFormaPagto(torcamento.Pce_Forma_Pagto_Entrada) + ")", torcamento.Pce_Entrada_Valor));
                    if (torcamento.Pce_Forma_Pagto_Prestacao != 5 && torcamento.Pce_Forma_Pagto_Prestacao != 7)
                    {
                        lista.Add(String.Format("Prestações: " + torcamento.Pce_Prestacao_Qtde + " x " + " {0:c2}" +
                            " (" + Util.OpcaoFormaPagto(torcamento.Pce_Forma_Pagto_Prestacao) +
                            ") vencendo a cada " +
                            torcamento.Pce_Prestacao_Periodo + " dias", torcamento.Pce_Prestacao_Valor));
                    }
                    else
                    {
                        lista.Add(String.Format("Prestações: " + torcamento.Pce_Prestacao_Qtde + " x " + " {0:c2}" +
                            " (" + Util.OpcaoFormaPagto(torcamento.Pce_Forma_Pagto_Prestacao) + ")",
                            torcamento.Pce_Prestacao_Valor));
                    }
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    lista.Add(String.Format("1ª Prestação: " + " {0:c2} (" +
                        Util.OpcaoFormaPagto(torcamento.Pse_Forma_Pagto_Prim_Prest) +
                        ") vencendo após " + torcamento.Pse_Prim_Prest_Apos + " dias", torcamento.Pse_Prim_Prest_Valor));
                    lista.Add(String.Format("Prestações: " + torcamento.Pse_Demais_Prest_Qtde + " x " +
                        " {0:c2} (" + Util.OpcaoFormaPagto(torcamento.Pse_Forma_Pagto_Demais_Prest) +
                        ") vencendo a cada " + torcamento.Pse_Demais_Prest_Periodo + " dias", 
                        torcamento.Pse_Demais_Prest_Valor));
                    break;
            }

            return lista;
        }

        //Obtem produtos do orçamento
        private async Task<IEnumerable<PrepedidoProdutoDtoPrepedido>> ObterProdutos(Torcamento orc)
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtos = from c in db.TorcamentoItems
                           where c.Orcamento == orc.Orcamento
                           orderby c.Sequencia
                           select c;

            List<PrepedidoProdutoDtoPrepedido> listaProduto = new List<PrepedidoProdutoDtoPrepedido>();

            foreach (var p in produtos)
            {
                PrepedidoProdutoDtoPrepedido produtoPrepedido = new PrepedidoProdutoDtoPrepedido
                {
                    Fabricante = p.Fabricante,
                    NumProduto = p.Produto,
                    Descricao = p.Descricao_Html,
                    Obs = p.Obs,
                    Qtde = p.Qtde,
                    Permite_Ra_Status = orc.Permite_RA_Status,
                    BlnTemRa = p.Preco_NF != p.Preco_Venda ? true : false,
                    //estamos alterando os valores de "Preco" para "p.Preco_Lista" e
                    //"VlLista" para "p.Preco_NF"
                    Preco = p.Preco_NF,//essa variavel não pode ter o valor alterado
                    VlLista = (decimal)p.Preco_Lista,//essa variavel é o valor base para calcular 
                    Desconto = p.Desc_Dado,
                    VlUnitario = p.Preco_Venda,
                    VlTotalRA = (decimal)(p.Qtde * (p.Preco_NF - p.Preco_Venda)),
                    Comissao = orc.Perc_RT,
                    TotalItemRA = p.Qtde * p.Preco_NF,
                    TotalItem = p.Qtde * p.Preco_Venda,
                    VlTotalItem = p.Qtde * p.Preco_Venda

                };

                listaProduto.Add(produtoPrepedido);
            }

            return await Task.FromResult(listaProduto);
        }

        private async Task<DadosClienteCadastroDto> ObterDadosCliente(string loja, string indicador_orcamentista, string vendedor, string idCliente)
        {
            var dadosCliente = from c in contextoProvider.GetContextoLeitura().Tclientes
                               where c.Id == idCliente
                               select c;
            var cli = await dadosCliente.FirstOrDefaultAsync();
            DadosClienteCadastroDto cadastroCliente = new DadosClienteCadastroDto
            {
                Loja = loja,
                Indicador_Orcamentista = indicador_orcamentista,
                Vendedor = vendedor,
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
        }

        private async Task<EnderecoEntregaDtoClienteCadastro> ObterEnderecoEntrega(Torcamento p)
        {
            EnderecoEntregaDtoClienteCadastro enderecoEntrega = new EnderecoEntregaDtoClienteCadastro();
            enderecoEntrega.OutroEndereco = Convert.ToBoolean(p.St_End_Entrega);

            if (p.St_End_Entrega == 1)
            {
                enderecoEntrega.EndEtg_endereco = p.EndEtg_Endereco;
                enderecoEntrega.EndEtg_endereco_numero = p.EndEtg_Endereco_Numero;
                enderecoEntrega.EndEtg_endereco_complemento = p.EndEtg_Endereco_Complemento;
                enderecoEntrega.EndEtg_bairro = p.EndEtg_Bairro;
                enderecoEntrega.EndEtg_cidade = p.EndEtg_Cidade;
                enderecoEntrega.EndEtg_uf = p.EndEtg_UF;
                enderecoEntrega.EndEtg_cep = p.EndEtg_CEP;
                enderecoEntrega.EndEtg_cod_justificativa = p.EndEtg_Cod_Justificativa;
                enderecoEntrega.EndEtg_descricao_justificativa = await Util.ObterDescricao_Cod(
                    Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA, p.EndEtg_Cod_Justificativa, contextoProvider);
            }


            return enderecoEntrega;
        }


        public async Task<short> Obter_Permite_RA_Status(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var raStatus = (from c in db.TorcamentistaEindicadors
                            where c.Apelido == apelido
                            select c.Permite_RA_Status).FirstOrDefaultAsync();


            return await raStatus;
        }



        public async Task<string> PrepedidoJaCadastrado(PrePedidoDto prePedido)
        {
            /*
             * critério:
             * - se já existe algum prepedido do mesmo cliente, criado com a mesma data e há menos de 10 minutos
             * - com a mesma loja e o mesmo orcamentista
             * - onde todos os produtos sejam iguais (fabricante, produto, qtde e preco_venda)
             * - a ordem dos produtos no pedido não precisa ser igual (no sistema em ASP exigia a mesma ordem para considerar igual)
             * - consequencia: se a forma de pagamento for diferente, o valor será diferente e o prepedido será considerado diferente
             * */
            /*

            '	VERIFICA SE ESTE ORÇAMENTO JÁ FOI GRAVADO!
                dim orcamento_a, vjg
                s = "SELECT t_ORCAMENTO.orcamento, fabricante, produto, qtde, preco_venda FROM t_ORCAMENTO INNER JOIN t_ORCAMENTO_ITEM ON (t_ORCAMENTO.orcamento=t_ORCAMENTO_ITEM.orcamento)" & _
                    " WHERE (id_cliente='" & cliente_selecionado & "') AND (data=" & bd_formata_data(Date) & ")" & _
                    " AND (loja='" & loja & "') AND (orcamentista='" & usuario & "')" & _
                    " AND (hora>='" & formata_hora_hhnnss(Now-converte_min_to_dec(10))& "')" & _
                    " ORDER BY t_ORCAMENTO_ITEM.orcamento, sequencia"
                    */

            var banco = contextoProvider.GetContextoLeitura();
            var hora = Util.HoraParaBanco(DateTime.Now.AddMinutes(-10)); //no máxio há 10 minutos
            var prepedidosExistentes = await (from prepedidoBanco in banco.Torcamentos
                                              join item in banco.TorcamentoItems on prepedidoBanco.Orcamento equals item.Orcamento
                                              where prepedidoBanco.Id_Cliente == prePedido.DadosCliente.Id
                                                 && (prepedidoBanco.Data.HasValue && prepedidoBanco.Data.Value.Date == DateTime.Now.Date)
                                                 && hora.CompareTo(prepedidoBanco.Hora) <= 0
                                                 && prepedidoBanco.Loja == prePedido.DadosCliente.Loja
                                                 && prepedidoBanco.Orcamentista == prePedido.DadosCliente.Indicador_Orcamentista
                                              select new { prepedidoBanco.Orcamento, item.Fabricante, item.Produto, item.Qtde, item.Preco_Venda, item.Sequencia }).ToListAsync();

            //agora já está na memória, as operações são rápidas
            var orcamentosExistentes = (from prepedido in prepedidosExistentes select prepedido.Orcamento).Distinct();

            foreach (var orcamentoExistente in orcamentosExistentes)
            {
                var itensDestePrepedido = (from item in prepedidosExistentes
                                           where item.Orcamento == orcamentoExistente
                                           orderby item.Fabricante, item.Produto, item.Qtde
                                           select item).ToList();
                //todos estes itens devem ser iguais aos do prepedido sendo criado para que o prepedido já exista
                var itensParaCriar = (from item in prePedido.ListaProdutos
                                      orderby item.Fabricante, item.NumProduto, item.Qtde
                                      select new { item.Fabricante, Produto = item.NumProduto, item.Qtde, Preco_Venda = Math.Round(item.VlUnitario, 2) }).ToList();

                if (itensDestePrepedido.Count() == itensParaCriar.Count() && itensDestePrepedido.Count() > 0)
                {
                    bool algumDiferente = false;
                    for (int i = 0; i < itensDestePrepedido.Count(); i++)
                    {
                        var um = itensDestePrepedido[i];
                        var dois = itensParaCriar[i];
                        //nao comparamos a sequencia
                        if (!(um.Fabricante == dois.Fabricante && um.Produto == dois.Produto && um.Qtde == dois.Qtde && um.Preco_Venda == dois.Preco_Venda))
                        {
                            algumDiferente = true;
                            break;
                        }
                    }

                    if (!algumDiferente)
                        return itensDestePrepedido[0].Orcamento;
                }

            }

            return null;
        }


        public async Task<IEnumerable<string>> CadastrarPrepedido(PrePedidoDto prePedido, string apelido)
        {
            List<string> lstErros = new List<string>();

            TorcamentistaEindicador tOrcamentista = await BuscarTorcamentista(apelido);

            //complementar os dados Cadastrais do cliente
            prePedido.DadosCliente.Indicador_Orcamentista = tOrcamentista.Apelido.ToUpper();
            prePedido.DadosCliente.Loja = tOrcamentista.Loja;
            prePedido.DadosCliente.Vendedor = tOrcamentista.Vendedor.ToUpper();

            if (string.IsNullOrEmpty(tOrcamentista.Vendedor))
                lstErros.Add("NÃO HÁ NENHUM VENDEDOR DEFINIDO PARA ATENDÊ-LO");

            //validar o Orcamentista
            if (tOrcamentista.Apelido != apelido)
                lstErros.Add("Falha ao recuperar os dados cadastrais!");

            //verifica se o prepedio já foi gravado
            var prepedidoJaCadastradoNumero = await PrepedidoJaCadastrado(prePedido);
            if (!String.IsNullOrEmpty(prepedidoJaCadastradoNumero))
            {
                lstErros.Add($"Este pré-pedido já foi gravado com o número {prepedidoJaCadastradoNumero}");
                return lstErros;
            }

            //verificar como esta sendo salvo
            if (prePedido.DetalhesPrepedido.EntregaImediata == "1")//Não
            {
                if (prePedido.DetalhesPrepedido.EntregaImediataData <= DateTime.Now)
                {
                    lstErros.Add("Favor informar a data de 'Entrega Imediata' posterior a data atual!");
                    return lstErros;
                }
            }

            if (prePedido.ListaProdutos.Count > 12)
            {
                lstErros.Add("É permitido apenas 12 itens por Pré-Pedido!");
                return lstErros;
            }


            //afazer: Verificar se teve alteração no cadastro do cliente para ser incluído
            //nos campos de Torcamento


            if (await Util.LojaHabilitadaProdutosECommerce(prePedido.DadosCliente.Loja, contextoProvider))
            {
                //Fazer a validação dos dados do cliente, pois agora será permitido realizar alterações em todo o cadastro
                //do cliente para realizar um novo Prepedido, mas esses dados de cadastro que poderão ser alterados, não serão 
                //alterados na tabela de cliente. Apenas será incluído para salvar o Prepedido.
                // Esses dados serão incluídos no novo Dto que foi criado "EnderecoCadastralClientePrepedidoDto", então
                //precisamos fazer uma nova rotina que irá fazer a validação de dados que foram inseridos no Dto, seguindo a 
                //validação existente no ASP

                //Afazer: incluir a validação dos novo campos de endereço de entrega
                //Validar endereço de entraga
                if (await ValidarEndecoEntrega(prePedido.EnderecoEntrega, lstErros))
                {
                    if (ValidarFormaPagto(prePedido, lstErros))
                    {

                        //Esta sendo verificado qual o tipo de pagamento que esta sendo feito e retornando a quantidade de parcelas
                        int c_custoFinancFornecQtdeParcelas = ObterQtdeParcelasFormaPagto(prePedido);

                        //varificar o numero para saber o tipo de pagamento
                        string c_custoFinancFornecTipoParcelamento = ObterSiglaFormaPagto(prePedido);


                        if (Util.ValidarTipoCustoFinanceiroFornecedor(lstErros, c_custoFinancFornecTipoParcelamento, c_custoFinancFornecQtdeParcelas))
                        {
                            //Calculamos os produtos com o coeficiente e retornamos uma lista de coeficientes dos fabricantes
                            List<TpercentualCustoFinanceiroFornecedor> lstPercentualCustoFinanFornec =
                            (await BuscarCoeficientePercentualCustoFinanFornec(prePedido,
                                (short)c_custoFinancFornecQtdeParcelas, c_custoFinancFornecTipoParcelamento, lstErros)).ToList();

                            Tparametro parametroRegra = await Util.BuscarRegistroParametro(Constantes.ID_PARAMETRO_Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal,
                                contextoProvider);
                            //esse metodo tb tras a sigla da pessoa
                            string tipoPessoa = Util.MultiCdRegraDeterminaPessoa(prePedido.DadosCliente.Tipo, prePedido.DadosCliente.Contribuinte_Icms_Status,
                                prePedido.DadosCliente.ProdutorRural);
                            string descricao = Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo);

                            //List<RegrasBll> regraCrtlEstoque = new List<RegrasBll>();
                            List<RegrasBll> regraCrtlEstoque = (await ObterCtrlEstoqueProdutoRegra(prePedido, lstErros)).ToList();
                            await Util.ObterCtrlEstoqueProdutoRegra_Teste(lstErros, regraCrtlEstoque, prePedido.DadosCliente.Uf, tipoPessoa, contextoProvider);


                            ProdutoGeralBll.VerificarRegrasAssociadasAosProdutos(regraCrtlEstoque, lstErros, prePedido.DadosCliente);
                            //obtendo qtde disponivel
                            await Util.VerificarEstoque(regraCrtlEstoque, contextoProvider);

                            ObterDisponibilidadeEstoque(regraCrtlEstoque, prePedido, parametroRegra, lstErros);

                            VerificarEstoqueInsuficiente(regraCrtlEstoque, prePedido, parametroRegra);

                            //realiza a análise da quantidade de pedidos necessária(auto-split)
                            VerificarQtdePedidosAutoSplit(regraCrtlEstoque, lstErros, prePedido);

                            //contagem de empresas que serão usadas no auto-split, ou seja, a quantidade de pedidos que será cadastrada, 
                            //já que cada pedido se refere ao estoque de uma empresa
                            List<int> lst_empresa_selecionada = ContagemEmpresasUsadasAutoSplit(regraCrtlEstoque, prePedido);

                            //há algum produto descontinuado?
                            await ExisteProdutoDescontinuado(prePedido, lstErros);

                            float perc_limite_RA_sem_desagio = await Util.VerificarSemDesagioRA(contextoProvider);

                            if (lstErros.Count <= 0)
                            {
                                using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
                                {

                                    //Se orcamento existir, fazer o delete das informações
                                    if (!string.IsNullOrEmpty(prePedido.NumeroPrePedido))
                                    {
                                        await DeletarOrcamentoExiste(dbgravacao, prePedido, apelido);
                                    }

                                    if (string.IsNullOrEmpty(prePedido.NumeroPrePedido))
                                    {
                                        //gerar o numero de orçamento
                                        await GerarNumeroOrcamento(dbgravacao, prePedido);
                                    }

                                    if (string.IsNullOrEmpty(prePedido.NumeroPrePedido))
                                        lstErros.Add("FALHA NA OPERAÇÃO COM O BANCO DE DADOS AO TENTAR GERAR NSU.");

                                    //Cadastrar dados do Orcamento e endereço de entrega 
                                    string log = await EfetivarCadastroPrepedido(dbgravacao,
                                        prePedido, tOrcamentista, c_custoFinancFornecTipoParcelamento, perc_limite_RA_sem_desagio);
                                    //Cadastrar orcamento itens
                                    List<TorcamentoItem> lstOrcamentoItem = MontaListaOrcamentoItem(prePedido);

                                    //vamos passar o coeficiente que foi criado na linha 596 e passar como param para cadastrar nos itens
                                    await ComplementarInfosOrcamentoItem(dbgravacao, lstOrcamentoItem,
                                        prePedido.DadosCliente.Loja, lstPercentualCustoFinanFornec);

                                    log = await CadastrarOrctoItens(dbgravacao, lstOrcamentoItem, log);

                                    bool gravouLog = Util.GravaLog(dbgravacao, apelido, prePedido.DadosCliente.Loja, prePedido.NumeroPrePedido,
                                        prePedido.DadosCliente.Id, Constantes.OP_LOG_ORCAMENTO_NOVO, log);

                                    dbgravacao.transacao.Commit();
                                    lstErros.Add(prePedido.NumeroPrePedido);
                                }
                            }
                        }
                    }
                }

            }
            return lstErros;
        }

        private string ObterSiglaFormaPagto(PrePedidoDto prePedido)
        {
            FormaPagtoCriacaoDto formaPagto = prePedido.FormaPagtoCriacao;
            string retorno = "";

            if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
                retorno = Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                retorno = Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                retorno = Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                retorno = Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                retorno = Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                retorno = Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;

            return retorno;
        }

        public async Task DeletarOrcamentoExiste(ContextoBdGravacao dbgravacao, PrePedidoDto prePedido, string apelido)
        {
            var orcamentoTask = from c in dbgravacao.Torcamentos.Include(r => r.TorcamentoItem)
                                where c.Orcamento == prePedido.NumeroPrePedido &&
                                      c.Orcamentista == apelido
                                select c;

            Torcamento orcamento = orcamentoTask.FirstOrDefault();

            dbgravacao.Remove(orcamento);
            await dbgravacao.SaveChangesAsync();
        }

        public async Task DeletarOrcamentoExisteComTransacao(PrePedidoDto prePedido, string apelido)
        {
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
            {
                await DeletarOrcamentoExiste(dbgravacao, prePedido, apelido);
                dbgravacao.transacao.Commit();
            }
        }


        private async Task<string> EfetivarCadastroPrepedido(ContextoBdGravacao dbgravacao, PrePedidoDto prepedido,
            TorcamentistaEindicador orcamentista, string siglaPagto, float perc_limite_RA_sem_desagio = 0)
        {
            //vamos buscar a midia do cliente para cadastrar no orçamento
            string midia = await (from c in dbgravacao.Tclientes
                                  where c.Cnpj_Cpf == prepedido.DadosCliente.Cnpj_Cpf
                                  select c.Midia).FirstOrDefaultAsync();

            Torcamento torcamento = new Torcamento();

            torcamento.Orcamento = prepedido.NumeroPrePedido;
            torcamento.Loja = orcamentista.Loja;
            torcamento.Data = DateTime.Now.Date;
            torcamento.Hora = Util.HoraParaBanco(DateTime.Now);
            torcamento.Id_Cliente = prepedido.DadosCliente.Id;
            torcamento.Orcamentista = orcamentista.Apelido;
            torcamento.Midia = midia == null ? midia = "" : midia;
            torcamento.Comissao_Loja_Indicou = 0;
            torcamento.Servicos = "";
            torcamento.Venda_Externa = 0;//Obs:NÃO ACHEI ESSE CAMPOS SENDO SALVO NOS ARQUIVOS DE ORCAMENTO
            //Vl_Servicos verificar como esse campo é inserido t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAG TorcamentistaEIndicadorRestricao
            torcamento.Vendedor = orcamentista.Vendedor;
            torcamento.Obs_1 = prepedido.DetalhesPrepedido.Observacoes == null ?
                "" : prepedido.DetalhesPrepedido.Observacoes;
            torcamento.Obs_2 = prepedido.DetalhesPrepedido.NumeroNF == null ?
                "" : prepedido.DetalhesPrepedido.NumeroNF;
            torcamento.Qtde_Parcelas = (short?)prepedido.FormaPagtoCriacao.Qtde_Parcelas;
            torcamento.Forma_Pagamento = prepedido.FormaPagtoCriacao.C_forma_pagto == null ?
                "" : prepedido.FormaPagtoCriacao.C_forma_pagto;
            torcamento.Tipo_Parcelamento = short.Parse(prepedido.FormaPagtoCriacao.Rb_forma_pagto);

            if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                torcamento.Av_Forma_Pagto = short.Parse(prepedido.FormaPagtoCriacao.Op_av_forma_pagto);
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                torcamento.Pu_Forma_Pagto = short.Parse(prepedido.FormaPagtoCriacao.Op_pu_forma_pagto);
                torcamento.Pu_Valor = prepedido.FormaPagtoCriacao.C_pu_valor;
                torcamento.Pu_Vencto_Apos = (short)prepedido.FormaPagtoCriacao.C_pu_vencto_apos;
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
            {
                torcamento.Pc_Qtde_Parcelas = (short)prepedido.FormaPagtoCriacao.C_pc_qtde;
                torcamento.Pc_Valor_Parcela = prepedido.FormaPagtoCriacao.C_pc_valor;
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
            {
                torcamento.Pc_Maquineta_Valor_Parcela = (decimal)prepedido.FormaPagtoCriacao.C_pc_maquineta_valor;
                torcamento.Pc_Maquineta_Qtde_Parcelas = (short)prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde;
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                torcamento.Pce_Forma_Pagto_Entrada = short.Parse(prepedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto);
                torcamento.Pce_Forma_Pagto_Prestacao = short.Parse(prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto);
                torcamento.Pce_Entrada_Valor = prepedido.FormaPagtoCriacao.C_pce_entrada_valor;
                torcamento.Pce_Prestacao_Qtde = (short)prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde;
                torcamento.Pce_Prestacao_Valor = prepedido.FormaPagtoCriacao.C_pce_prestacao_valor;
                if (prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "5" &&
                    prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "7")
                    torcamento.Pce_Prestacao_Periodo = (short)prepedido.FormaPagtoCriacao.C_pce_prestacao_periodo;
                torcamento.Qtde_Parcelas = (short?)(prepedido.FormaPagtoCriacao.Qtde_Parcelas);
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                torcamento.Pse_Forma_Pagto_Prim_Prest = short.Parse(prepedido.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto);
                torcamento.Pse_Forma_Pagto_Demais_Prest = short.Parse(prepedido.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto);
                torcamento.Pse_Prim_Prest_Valor = prepedido.FormaPagtoCriacao.C_pse_prim_prest_valor;
                torcamento.Pse_Prim_Prest_Apos = (short)prepedido.FormaPagtoCriacao.C_pse_prim_prest_apos;
                torcamento.Pse_Demais_Prest_Qtde = (short)prepedido.FormaPagtoCriacao.C_pse_demais_prest_qtde;
                torcamento.Pse_Demais_Prest_Valor = (decimal)prepedido.FormaPagtoCriacao.C_pse_demais_prest_valor;
                torcamento.Pse_Demais_Prest_Periodo = (short)prepedido.FormaPagtoCriacao.C_pse_demais_prest_periodo;
                torcamento.Qtde_Parcelas = (short)(prepedido.FormaPagtoCriacao.Qtde_Parcelas + 1);
            }
            torcamento.St_Orcamento = "";
            torcamento.St_Fechamento = "";
            torcamento.St_Orc_Virou_Pedido = 0;
            torcamento.CustoFinancFornecTipoParcelamento = prepedido.FormaPagtoCriacao.Rb_forma_pagto;
            torcamento.CustoFinancFornecQtdeParcelas = (short)ObterQtdeParcelasFormaPagto(prepedido);
            torcamento.Vl_Total = Calcular_Vl_Total(prepedido);
            torcamento.Vl_Total_NF = CalcularVl_Total_NF(prepedido);
            torcamento.Vl_Total_RA = CalcularVl_Total_NF(prepedido) - Calcular_Vl_Total(prepedido);
            torcamento.Perc_RT = 0;

            torcamento.StBemUsoConsumo = prepedido.DetalhesPrepedido.BemDeUso_Consumo !=
                Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO.ToString() ?
                (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_SIM : (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO;

            torcamento.InstaladorInstalaStatus = prepedido.DetalhesPrepedido.InstaladorInstala ==
                Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_SIM.ToString() ?
                (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_SIM :
                (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO;

            torcamento.InstaladorInstalaUsuarioUltAtualiz = orcamentista.Apelido;
            torcamento.InstaladorInstalaDtHrUltAtualiz = DateTime.Now;
            torcamento.Perc_Desagio_RA_Liquida = perc_limite_RA_sem_desagio;
            torcamento.Permite_RA_Status = orcamentista.Permite_RA_Status;

            torcamento.St_End_Entrega = prepedido.EnderecoEntrega.OutroEndereco == true ? (short)1 : (short)0;
            if (prepedido.EnderecoEntrega.OutroEndereco)
            {
                torcamento.EndEtg_Endereco = prepedido.EnderecoEntrega.EndEtg_endereco;
                torcamento.EndEtg_Endereco_Numero = prepedido.EnderecoEntrega.EndEtg_endereco_numero;
                torcamento.EndEtg_Endereco_Complemento = prepedido.EnderecoEntrega.EndEtg_endereco_complemento;
                torcamento.EndEtg_Bairro = prepedido.EnderecoEntrega.EndEtg_bairro;
                torcamento.EndEtg_Cidade = prepedido.EnderecoEntrega.EndEtg_cidade;
                torcamento.EndEtg_UF = prepedido.EnderecoEntrega.EndEtg_uf;
                torcamento.EndEtg_CEP = prepedido.EnderecoEntrega.EndEtg_cep.Replace("-", "");
                torcamento.EndEtg_Cod_Justificativa = prepedido.EnderecoEntrega.EndEtg_cod_justificativa;
            }

            //voltei esse código, pois não esta claro que será feito alteração para a linha 956 deste arquivo
            //torcamento.St_Etg_Imediata = prepedido.DetalhesPrepedido.EntregaImediata != Constantes.COD_ETG_IMEDIATA_NAO ? 
            //    short.Parse(Constantes.COD_ETG_IMEDIATA_SIM) : short.Parse(Constantes.COD_ETG_IMEDIATA_NAO);

            //if (torcamento.St_Etg_Imediata == short.Parse(Constantes.COD_ETG_IMEDIATA_SIM))
            //{
            //    torcamento.Etg_Imediata_Data = DateTime.Now;
            //    torcamento.Etg_Imediata_Usuario = orcamentista.Apelido;
            //}

            //não apagar: esperando o cliente informar se alteramos a forma que iremos salvar os campos de entrega imediata
            //no caso de não ser entrega imediata
            //NECESSÁRIO VERIFICAR A REGRA PARA ESSE CAMPO
            if (prepedido.DetalhesPrepedido.EntregaImediata == Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO.ToString())
            {
                //verificar se a data esta correta
                torcamento.St_Etg_Imediata = (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO;
                //montar a data
                //estou montando a data, pois comparando com a data que esta sendo salvo na base 
                //preciso montar a data com no formato "yyyy-MM-dd hh:mm:ss.ms"
                //a data que vem da tela esta com o horário zerado
                if (!prepedido.DetalhesPrepedido.EntregaImediataData.HasValue)
                {

                }
                else
                {
                    int dd = prepedido.DetalhesPrepedido.EntregaImediataData.Value.Day;
                    int MM = prepedido.DetalhesPrepedido.EntregaImediataData.Value.Month;
                    int yyyy = prepedido.DetalhesPrepedido.EntregaImediataData.Value.Year;
                    int hh = DateTime.Now.Hour;
                    int mm = DateTime.Now.Minute;
                    int ss = DateTime.Now.Second;
                    int ms = DateTime.Now.Millisecond;

                    torcamento.Etg_Imediata_Data = new DateTime(yyyy, MM, dd, hh, mm, ss, ms);
                    torcamento.Etg_Imediata_Usuario = orcamentista.Apelido;

                    //novos campos:Vamos esperar o Hamilton dar ok para inclusão desses novos campos
                    torcamento.PrevisaoEntregaData = new DateTime(yyyy, MM, dd, hh, mm, ss, ms);
                    torcamento.PrevisaoEntregaUsuarioUltAtualiz = orcamentista.Apelido;
                    torcamento.PrevisaoEntregaDtHrUltAtualiz = new DateTime(yyyy, MM, dd, hh, mm, ss, ms);
                }
            }
            else
            {
                torcamento.St_Etg_Imediata = (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM;
                torcamento.Etg_Imediata_Data = DateTime.Now;
                torcamento.Etg_Imediata_Usuario = orcamentista.Apelido;
            }

            torcamento.CustoFinancFornecTipoParcelamento = siglaPagto;//sigla pagto
            torcamento.GarantiaIndicadorStatus = prepedido.DetalhesPrepedido.GarantiaIndicador == null ? byte.Parse(Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO) : byte.Parse(Constantes.COD_GARANTIA_INDICADOR_STATUS__SIM);
            torcamento.GarantiaIndicadorUsuarioUltAtualiz = orcamentista.Apelido;
            torcamento.GarantiaInidicadorDtHrUltAtualiz = DateTime.Now;


            //Verificando campos NULL para compatibilidade, pois os campos aceitam NULL mas, não é salvo dessa forma
            torcamento.Pu_Valor = torcamento.Pu_Valor.HasValue ? torcamento.Pu_Valor : 0.0M;
            torcamento.Pc_Valor_Parcela = torcamento.Pc_Valor_Parcela.HasValue ? torcamento.Pc_Valor_Parcela : 0.0M;
            torcamento.Pce_Entrada_Valor = torcamento.Pce_Entrada_Valor.HasValue ? torcamento.Pce_Entrada_Valor : 0.0M;
            torcamento.Pce_Prestacao_Valor = torcamento.Pce_Prestacao_Valor.HasValue ? torcamento.Pce_Prestacao_Valor : 0.0M;
            torcamento.Pse_Prim_Prest_Valor = torcamento.Pse_Prim_Prest_Valor.HasValue ? torcamento.Pse_Prim_Prest_Valor : 0.0M;

            //Inserindo os novos campos de sistema responsavel
            torcamento.Sistema_responsavel_cadastro = Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;
            torcamento.Sistema_responsavel_atualizacao = Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS;


            //vamos alterar o modo de criar o log e montar apenas os campos que devem ser salvos
            //montamos os valores a serem inseridos
            string campos_a_inserir = MontarCamposAInserirValorTotal(torcamento.Vl_Total.ToString());
            //montamos alguns detalhes do prepedido
            campos_a_inserir += MontarCamposAInserirDetalhes(torcamento);
            //montamos a forma de pagto selecionada
            campos_a_inserir += MontarCamposAInserirFormaPagto(prepedido.FormaPagtoCriacao);
            //montamos os custos
            campos_a_inserir += "custoFinancFornecTipoParcelamento|custoFinancFornecQtdeParcelas|";
            //montamos os campos de endereço de entrega se existir
            campos_a_inserir += prepedido.EnderecoEntrega.OutroEndereco ?
                MontaCamposAInserirEnderecoEntrega(prepedido.EnderecoEntrega) : "Endereço entrega=mesmo do cadastro|";

            campos_a_inserir += "InstaladorInstalaStatus|GarantiaIndicadorStatus|perc_desagio_RA_liquida";

            string log = "";
            log = Util.MontaLogInserir(torcamento, log, campos_a_inserir, true);

            dbgravacao.Add(torcamento);
            await dbgravacao.SaveChangesAsync();

            return log;
        }

        private async Task<string> CadastrarOrctoItens(ContextoBdGravacao dbgravacao, List<TorcamentoItem> lstOrcItens, string log)
        {
            string logItem = "";
            foreach (var i in lstOrcItens)
            {
                dbgravacao.Add(i);

                //vamos montar os campos a inserir
                string campos_a_inserir = "";
                campos_a_inserir = MontarCamposAInserirItens(i);

                logItem += "\n";

                logItem = MontaLogInserirItens(i, campos_a_inserir, logItem);

            }
            await dbgravacao.SaveChangesAsync();
            log += logItem;
            return log;
        }

        private string MontaLogInserirItens(TorcamentoItem item, string campos_a_inserir, string log)
        {
            //montamos o produto ex: 1x001001(001);
            log += item.Qtde + "x" + item.Produto;
            if (item.Fabricante != "" && item.Fabricante != null)
            {
                log += "(" + item.Fabricante + ");";
            }

            PropertyInfo[] property = item.GetType().GetProperties();

            foreach (var c in property)
            {

                //pegando o real nome da coluna 
                ColumnAttribute column = (ColumnAttribute)Attribute.GetCustomAttribute(c, typeof(ColumnAttribute));
                if (column != null)
                {
                    string coluna = column.Name;
                    if (campos_a_inserir.Contains("preco_NF"))
                    {

                    }
                    if (campos_a_inserir.Contains(coluna))
                    {

                        //pegando o valor coluna
                        var value = (c.GetValue(item, null));
                        log = log + coluna + "=" + value + "; ";
                    }
                }
            }

            return log;
        }

        private string MontarCamposAInserirItens(TorcamentoItem item)
        {
            string campos_a_inserir = "";

            campos_a_inserir = "preco_lista|desc_dado|preco_venda|preco_NF|obs|custoFinancFornecCoeficiente|custoFinancFornecPrecoListaBase|";

            if (item.Qtde_Spe > 0)
            {
                campos_a_inserir += "spe|";
            }
            if (item.Abaixo_Min_Status != 0 && item.Abaixo_Min_Status != null)
            {
                campos_a_inserir += "abaixo_min_status|abaixo_min_autorizacao|abaixo_min_autorizador|abaixo_min_superv_autorizador";
            }

            return campos_a_inserir;
        }

        private string MontaCamposAInserirEnderecoEntrega(EnderecoEntregaDtoClienteCadastro end)
        {
            string campos_a_inserir = "";

            if (end.OutroEndereco)
            {
                campos_a_inserir = "|EndEtg_endereco|EndEtg_bairro|EndEtg_cidade|EndEtg_uf|EndEtg_cep|" +
                    "EndEtg_endereco_numero|EndEtg_endereco_complemento|EndEtg_cod_justificativa|";
            }


            return campos_a_inserir;
        }

        private string MontarCamposAInserirDetalhes(Torcamento orcamento)
        {
            string campos_a_inserir = "";

            if (orcamento.Forma_Pagamento != "" && orcamento.Forma_Pagamento != null)
            {
                campos_a_inserir += "forma_pagto|";
            }
            if (orcamento.Servicos != "" && orcamento.Servicos != null)
            {
                campos_a_inserir += "servicos|";
            }
            if (orcamento.Vl_Servicos.ToString() != "" && orcamento.Vl_Servicos != 0)
            {
                campos_a_inserir += "vl_servicos|";
            }
            if (orcamento.St_Etg_Imediata.ToString() != "")
            {
                campos_a_inserir += "st_etg_imediata|";
            }
            if (orcamento.StBemUsoConsumo.ToString() != "")
            {
                campos_a_inserir += "StBemUsoConsumo|";
            }
            if (orcamento.Obs_1 != "" && orcamento.Obs_1 != null)
            {
                campos_a_inserir += "obs_1|";
            }
            if (orcamento.Obs_2 != "" && orcamento.Obs_2 != null)
            {
                campos_a_inserir += "obs_2|";
            }

            return campos_a_inserir;
        }

        private string MontarCamposAInserirValorTotal(string vlTotal)
        {
            string campos_a_inserir = "";
            //estamos recendo esse param de entrada, pois esse campos vl total não tem
            campos_a_inserir = "vl total=" + vlTotal + "|vl_total_NF|vl_total_RA|qtde_parcelas|perc_RT|midia|";

            return campos_a_inserir;
        }

        private string MontarCamposAInserirFormaPagto(FormaPagtoCriacaoDto forma_pagto_criacao)
        {
            string campos_a_inserir = "";
            campos_a_inserir += "tipo_parcelamento|";

            if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
                campos_a_inserir = "av_forma_pagto|";
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                campos_a_inserir += "pu_forma_pagto|pu_valor|pu_vencto_apos|";
            }
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                campos_a_inserir += "pc_qtde_parcelas|pc_valor_parcela|";
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                campos_a_inserir += "pc_maquineta_qtde_parcelas|pc_maquineta_valor_parcela|";
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                campos_a_inserir += "pce_forma_pagto_entrada|pce_forma_pagto_prestacao|pce_entrada_valor|pce_prestacao_qtde|" +
                    "pce_prestacao_valor|pce_prestacao_periodo|";
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                campos_a_inserir += "pse_forma_pagto_prim_prest|pse_forma_pagto_demais_prest|pse_prim_prest_valor|pse_prim_prest_apos|" +
                    "pse_demais_prest_qtde|pse_demais_prest_valor|pse_demais_prest_periodo|";


            return campos_a_inserir;
        }

        private int ObterQtdeParcelasFormaPagto(PrePedidoDto prepedido)
        {
            FormaPagtoCriacaoDto formaPagto = prepedido.FormaPagtoCriacao;
            int qtdeParcelas = 0;

            if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
                qtdeParcelas = 0;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                qtdeParcelas = 1;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                qtdeParcelas = (int)formaPagto.C_pc_qtde;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                qtdeParcelas = (int)formaPagto.C_pc_maquineta_qtde;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                qtdeParcelas = (int)formaPagto.C_pce_prestacao_qtde;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                qtdeParcelas = (int)formaPagto.C_pse_demais_prest_qtde++;//conforme linha 199 pág OrcamentoNovoConfirma.asp

            return qtdeParcelas;
        }

        private bool ValidarFormaPagto(PrePedidoDto prepedido, List<string> lstErros)
        {
            bool retorno = false;

            decimal vlTotalFormaPagto = 0M;

            if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_av_forma_pagto))
                    lstErros.Add("Indique a forma de pagamento (à vista).");
                if (!CalculaItens(prepedido, out vlTotalFormaPagto))
                    lstErros.Add("Há divergência entre o valor total do pré-pedido (" + Constantes.SIMBOLO_MONETARIO + " " +
                        prepedido.VlTotalDestePedido + ") e o valor total descrito através da forma de pagamento (" + Constantes.SIMBOLO_MONETARIO + " " +
                        Math.Abs((decimal)prepedido.VlTotalDestePedido - vlTotalFormaPagto) + ")!");
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_pu_forma_pagto))
                    lstErros.Add("Indique a forma de pagamento da parcela única.");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pu_valor.ToString()))
                    lstErros.Add("Indique o valor da parcela única.");
                else if (prepedido.FormaPagtoCriacao.C_pu_valor <= 0)
                    lstErros.Add("Valor da parcela única é inválido.");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pu_vencto_apos.ToString()))
                    lstErros.Add("Indique o intervalo de vencimento da parcela única.");
                else if (prepedido.FormaPagtoCriacao.C_pu_vencto_apos <= 0)
                    lstErros.Add("Intervalo de vencimento da parcela única é inválido.");
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
            {
                if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_qtde.ToString()))
                    lstErros.Add("Indique a quantidade de parcelas (parcelado no cartão [internet]).");
                else if (prepedido.FormaPagtoCriacao.C_pc_qtde < 1)
                    lstErros.Add("Quantidade de parcelas inválida (parcelado no cartão [internet]).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_valor.ToString()))
                    lstErros.Add("Indique o valor da parcela (parcelado no cartão [internet]).");
                else if (prepedido.FormaPagtoCriacao.C_pc_valor <= 0)
                    lstErros.Add("Valor de parcela inválido (parcelado no cartão [internet]).");
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
            {
                if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde.ToString()))
                    lstErros.Add("Indique a quantidade de parcelas (parcelado no cartão [maquineta]).");
                else if (prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde < 1)
                    lstErros.Add("Quantidade de parcelas inválida (parcelado no cartão [maquineta]).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pc_maquineta_valor.ToString()))
                    lstErros.Add("Indique o valor da parcela (parcelado no cartão [maquineta]).");
                else if (prepedido.FormaPagtoCriacao.C_pc_maquineta_valor <= 0)
                    lstErros.Add("Valor de parcela inválido (parcelado no cartão [maquineta]).");
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto.ToString()))
                    lstErros.Add("Indique a forma de pagamento da entrada (parcelado com entrada).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pce_entrada_valor.ToString()))
                    lstErros.Add("Indique o valor da entrada (parcelado com entrada).");
                else if (prepedido.FormaPagtoCriacao.C_pce_entrada_valor <= 0)
                    lstErros.Add("Valor da entrada inválido (parcelado com entrada).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto))
                    lstErros.Add("Indique a forma de pagamento das prestações (parcelado com entrada).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde.ToString()))
                    lstErros.Add("Indique a quantidade de prestações (parcelado com entrada).");
                else if (prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde <= 0)
                    lstErros.Add("Quantidade de prestações inválida (parcelado com entrada).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pce_prestacao_valor.ToString()))
                    lstErros.Add("Indique o valor da prestação (parcelado com entrada).");
                else if (prepedido.FormaPagtoCriacao.C_pce_prestacao_valor <= 0)
                    lstErros.Add("Valor de prestação inválido (parcelado com entrada).");
                else if (prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "7" &&
                    prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "5")
                {
                    if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pce_prestacao_periodo.ToString()))
                        lstErros.Add("Indique o intervalo de vencimento entre as parcelas (parcelado com entrada).");
                }
                else if (prepedido.FormaPagtoCriacao.C_pce_prestacao_periodo <= 0)
                    lstErros.Add("Intervalo de vencimento inválido (parcelado com entrada).");
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto))
                    lstErros.Add("Indique a forma de pagamento da 1ª prestação (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pse_prim_prest_valor.ToString()))
                    lstErros.Add("Indique o valor da 1ª prestação (parcelado sem entrada).");
                else if (prepedido.FormaPagtoCriacao.C_pse_prim_prest_valor <= 0)
                    lstErros.Add("Valor da 1ª prestação inválido (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pse_prim_prest_apos.ToString()))
                    lstErros.Add("Indique o intervalo de vencimento da 1ª parcela (parcelado sem entrada).");
                else if (prepedido.FormaPagtoCriacao.C_pse_prim_prest_apos <= 0)
                    lstErros.Add("Intervalo de vencimento da 1ª parcela é inválido (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto))
                    lstErros.Add("Indique a forma de pagamento das demais prestações (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pse_demais_prest_qtde.ToString()))
                    lstErros.Add("Indique a quantidade das demais prestações (parcelado sem entrada).");
                else if (prepedido.FormaPagtoCriacao.C_pse_demais_prest_qtde <= 0)
                    lstErros.Add("Quantidade de prestações inválida (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pse_demais_prest_valor.ToString()))
                    lstErros.Add("Indique o valor das demais prestações (parcelado sem entrada).");
                else if (prepedido.FormaPagtoCriacao.C_pse_demais_prest_valor <= 0)
                    lstErros.Add("Valor de prestação inválido (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pse_demais_prest_periodo.ToString()))
                    lstErros.Add("Indique o intervalo de vencimento entre as parcelas (parcelado sem entrada).");
                else if (prepedido.FormaPagtoCriacao.C_pse_demais_prest_periodo < 0)
                    lstErros.Add("Intervalo de vencimento inválido (parcelado sem entrada).");
            }
            else
            {
                lstErros.Add("É obrigatório especificar a forma de pagamento");
            }

            if (lstErros.Count == 0)
                retorno = true;

            return retorno;
        }


        private bool CalculaItens(PrePedidoDto prePedido, out decimal vlTotalFormaPagto)
        {
            bool retorno = true;
            decimal vl_total_NF = 0;
            decimal vl_total = 0;


            foreach (var p in prePedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    vl_total += (decimal)(p.Qtde * p.VlUnitario);
                    vl_total_NF += (decimal)(p.Qtde * p.Preco);
                }
            }
            vlTotalFormaPagto = vl_total_NF;
            if (Math.Abs(vlTotalFormaPagto - vl_total_NF) > 0.1M)
                retorno = false;

            return retorno;
        }

        private decimal Calcular_Vl_Total(PrePedidoDto prepedido)
        {
            decimal vl_total = 0M;

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    vl_total += (decimal)(p.Qtde * p.VlUnitario);
                }
            }

            return Math.Round(vl_total, 2);
        }

        private decimal CalcularVl_Total_NF(PrePedidoDto prepedido)
        {
            decimal vl_total_NF = 0M;

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    vl_total_NF += (decimal)(p.Qtde * p.Preco_Lista);
                }
            }

            return Math.Round(vl_total_NF, 2);
        }

        private async Task ExisteProdutoDescontinuado(PrePedidoDto prepedido, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto) && !string.IsNullOrEmpty(p.Fabricante))
                {
                    var produtoTask = (from c in db.Tprodutos
                                       where c.Produto == p.NumProduto && c.Fabricante == p.Fabricante
                                       select c.Descontinuado).FirstOrDefaultAsync();
                    var produto = await produtoTask;

                    if (produto.ToUpper() == "S")
                    {
                        if (p.Qtde > p.Qtde_estoque_total_disponivel)
                            lstErros.Add("Produto (" + p.Fabricante + ")" + p.NumProduto +
                                " consta como 'descontinuado' e não há mais saldo suficiente no estoque para atender à quantidade solicitada.");
                    }
                }
            }
        }

        private List<int> ContagemEmpresasUsadasAutoSplit(List<RegrasBll> lstRegras, PrePedidoDto prepedido)
        {
            int qtde_empresa_selecionada = 0;
            List<int> lista_empresa_selecionada = new List<int>();

            foreach (var regra in lstRegras)
            {
                if (!string.IsNullOrEmpty(regra.Produto))
                {
                    foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                    {
                        if (r.Id_nfe_emitente > 0)
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

            return lista_empresa_selecionada;
        }

        //Metodo que verifica a qtde solicitada e estoque
        private void VerificarQtdePedidosAutoSplit(List<RegrasBll> lstRegras, List<string> lstErros, PrePedidoDto prepedido)
        {
            int qtde_a_alocar = 0;

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    qtde_a_alocar = (int)p.Qtde;

                    foreach (var regra in lstRegras)
                    {
                        if (qtde_a_alocar == 0)
                            break;
                        if (!string.IsNullOrEmpty(regra.Produto) && !string.IsNullOrEmpty(regra.Fabricante))
                        {
                            foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (qtde_a_alocar == 0)
                                    break;
                                if (re.Id_nfe_emitente > 0)
                                {
                                    if (re.St_inativo == 0)
                                    {
                                        if (regra.Fabricante == p.Fabricante && regra.Produto == p.NumProduto)
                                        {
                                            if (re.Estoque_Qtde >= qtde_a_alocar)
                                            {
                                                re.Estoque_Qtde_Solicitado = (short)qtde_a_alocar;
                                                qtde_a_alocar = 0;
                                            }
                                            else if (re.Estoque_Qtde > 0)
                                            {
                                                re.Estoque_Qtde_Solicitado = re.Estoque_Qtde;
                                                qtde_a_alocar = qtde_a_alocar - re.Estoque_Qtde;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (qtde_a_alocar > 0)
                    {
                        foreach (var regra in lstRegras)
                        {
                            if (qtde_a_alocar == 0)
                                break;
                            if (regra.Produto == p.NumProduto && regra.Fabricante == p.Fabricante)
                            {
                                foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                                {
                                    if (regra.Fabricante == p.Fabricante &&
                                       regra.Produto == p.NumProduto &&
                                       re.Id_nfe_emitente > 0 &&
                                       re.Id_nfe_emitente == regra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                    {
                                        re.Estoque_Qtde_Solicitado += (short)qtde_a_alocar;
                                        qtde_a_alocar = 0;
                                    }
                                }
                            }
                        }
                    }
                    if (qtde_a_alocar > 0)
                    {
                        lstErros.Add("Falha ao processar a alocação de produtos no estoque: restaram " + qtde_a_alocar + " unidades do produto (" +
                            p.Fabricante + ")" + p.NumProduto + " que não puderam ser alocados na lista de produtos sem presença no estoque de nenhum CD");
                    }
                }
            }
        }

        //se estoque for insuficiente, retorna true
        private bool VerificarEstoqueInsuficiente(List<RegrasBll> lstRegras, PrePedidoDto prepedido, Tparametro parametro)
        {
            bool retorno = false;
            int qtde_estoque_total_disponivel = 0;
            int qtde_estoque_total_disponivel_global = 0;

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    foreach (var regra in lstRegras)
                    {
                        if (!string.IsNullOrEmpty(regra.Produto))
                        {
                            foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (r.Id_nfe_emitente > 0)
                                {
                                    if (r.St_inativo == 0)
                                    {
                                        if (regra.Fabricante == p.Fabricante && regra.Produto == p.NumProduto)
                                        {
                                            qtde_estoque_total_disponivel += r.Estoque_Qtde;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (parametro.Campo_inteiro == 1)
                    {
                        if (qtde_estoque_total_disponivel_global == 0)
                        {
                            p.Qtde_estoque_total_disponivel = 0;

                        }
                    }
                    else
                    {
                        p.Qtde_estoque_total_disponivel = (short?)qtde_estoque_total_disponivel;
                    }
                    if (p.Qtde > p.Qtde_estoque_total_disponivel)
                        retorno = true;
                }
            }
            return retorno;

        }

        //qtde_estoque_total_global_disponivel é uma variavel que esta global no deles
        private void ObterDisponibilidadeEstoque(List<RegrasBll> lstRegras, PrePedidoDto prepedido, Tparametro parametroRegra, List<string> lstErros)
        {
            int id_nfe_emitente_selecao_manual = 0;

            foreach (var regra in lstRegras)
            {
                if (!string.IsNullOrEmpty(regra.Produto))
                {
                    foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                    {
                        if (re.Id_nfe_emitente > 0 && id_nfe_emitente_selecao_manual == 0)
                        {
                            if (re.St_inativo == 0)
                            {
                                foreach (var p in prepedido.ListaProdutos)
                                {
                                    if (regra.Fabricante == p.Fabricante && regra.Produto == p.NumProduto)
                                    {
                                        re.Estoque_Fabricante = p.Fabricante;
                                        re.Estoque_Produto = p.NumProduto;
                                        re.Estoque_Descricao = p.Descricao;
                                        re.Estoque_DescricaoHtml = p.Descricao;
                                        re.Estoque_Qtde_Solicitado = p.Qtde;
                                        re.Estoque_Qtde = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task<bool> ValidarEndecoEntrega(EnderecoEntregaDtoClienteCadastro endEtg, List<string> lstErros)
        {
            bool retorno = true;

            if (endEtg.OutroEndereco)
            {
                if(string.IsNullOrEmpty(endEtg.EndEtg_cod_justificativa))
                {
                    lstErros.Add("SELECIONE A JUSTIFICATIVA DO ENDEREÇO DE ENTREGA!");
                    retorno = false;
                }
                
                if (string.IsNullOrEmpty(endEtg.EndEtg_endereco))
                {
                    lstErros.Add("PREENCHA O ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }
                if (endEtg.EndEtg_endereco.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
                {
                    lstErros.Add("ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " + endEtg.EndEtg_endereco.Length +
                        " CARACTERES<br>TAMANHO MÁXIMO: " + Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");
                    retorno = false;
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_endereco_numero))
                {
                    lstErros.Add("PREENCHA O NÚMERO DO ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_bairro))
                {
                    lstErros.Add("PREENCHA O BAIRRO DO ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_cidade))
                {
                    lstErros.Add("PREENCHA A CIDADE DO ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_uf) || !Util.VerificaUf(endEtg.EndEtg_uf))
                {
                    lstErros.Add("UF INVÁLIDA NO ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }
                if (!Util.VerificaCep(endEtg.EndEtg_cep))
                {
                    lstErros.Add("CEP INVÁLIDO NO ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }

                
                List<NfeMunicipio> lstNfeMunicipio = (await ValidacoesClienteBll.ConsisteMunicipioIBGE(
                    endEtg.EndEtg_cidade, endEtg.EndEtg_uf, lstErros, contextoProvider)).ToList();
            }

            //fazer a consistência de cep pelo consiste que é feito no consiste
            //string uf = VerificarInscricaoEstadualValida(dadosCliente.Ie, dadosCliente.Uf, lstErros);
            //List<NfeMunicipio> lstNfeMunicipio = new List<NfeMunicipio>();
            //lstNfeMunicipio = (await ConsisteMunicipioIBGE(dadosCliente.Cidade, dadosCliente.Uf, lstErros)).ToList();

            return retorno;
        }

        public async Task<IEnumerable<TpercentualCustoFinanceiroFornecedor>> BuscarCoeficientePercentualCustoFinanFornec(PrePedidoDto prePedido, short qtdeParcelas, string siglaPagto, List<string> lstErros)
        {
            List<TpercentualCustoFinanceiroFornecedor> lstPercentualCustoFinanFornec =
                new List<TpercentualCustoFinanceiroFornecedor>();

            var db = contextoProvider.GetContextoLeitura();

            if (siglaPagto != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
            {
                var lstFabricantes = prePedido.ListaProdutos.Select(x => x.Fabricante).Distinct();

                foreach (var fabr in lstFabricantes)
                {
                    TpercentualCustoFinanceiroFornecedor percCustoTask =
                        await (from c in db.TpercentualCustoFinanceiroFornecedors
                               where c.Fabricante == fabr &&
                                     c.Tipo_Parcelamento == siglaPagto &&
                                     c.Qtde_Parcelas == qtdeParcelas
                               select c).FirstOrDefaultAsync();

                    lstPercentualCustoFinanFornec.Add(percCustoTask);

                    foreach (var i in prePedido.ListaProdutos)
                    {
                        if (fabr == i.Fabricante)
                        {
                            if (percCustoTask != null)
                            {
                                i.VlLista = (decimal)percCustoTask.Coeficiente * (decimal)i.Preco;
                            }
                            else
                            {
                                lstErros.Add("Opção de parcelamento não disponível para fornecedor " + i.Fabricante + ": " +
                                    DecodificaCustoFinanFornecQtdeParcelas(prePedido.FormaPagtoCriacao.C_forma_pagto, qtdeParcelas) + " parcela(s)");
                            }
                        }
                    }
                }
            }

            return lstPercentualCustoFinanFornec;
        }

        private async Task<IEnumerable<RegrasBll>> ObterCtrlEstoqueProdutoRegra(PrePedidoDto prePedido, List<string> lstErros)
        {
            List<RegrasBll> lstRegrasCrtlEstoque = new List<RegrasBll>();

            var db = contextoProvider.GetContextoLeitura();

            foreach (var item in prePedido.ListaProdutos)
            {
                var regraProdutoTask = from c in db.TprodutoXwmsRegraCds
                                       where c.Fabricante == item.Fabricante &&
                                             c.Produto == item.NumProduto
                                       select c;

                var regra = await regraProdutoTask.FirstOrDefaultAsync();

                if (regra == null)
                {
                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                        Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': produto (" + item.Fabricante + ")" +
                        item.NumProduto + " não possui regra associada");
                }
                else
                {
                    if (regra.Id_wms_regra_cd == 0)
                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                            Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': produto (" + item.Fabricante + ")" +
                            item.NumProduto + " não está associado a nenhuma regra");
                    else
                    {
                        var wmsRegraTask = from c in db.TwmsRegraCds
                                           where c.Id == regra.Id_wms_regra_cd
                                           select c;

                        var wmsRegra = await wmsRegraTask.FirstOrDefaultAsync();
                        if (wmsRegra == null)
                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                item.NumProduto + " não foi localizada no banco de dados (Id=" + regra.Id_wms_regra_cd + ")");
                        else
                        {
                            RegrasBll itemRegra = new RegrasBll();
                            itemRegra.Fabricante = item.Fabricante;
                            itemRegra.Produto = item.NumProduto;

                            itemRegra.TwmsRegraCd = new Regras.t_WMS_REGRA_CD
                            {
                                Id = wmsRegra.Id,
                                Apelido = wmsRegra.Apelido,
                                Descricao = wmsRegra.Descricao,
                                St_inativo = wmsRegra.St_inativo
                            };

                            var wmsRegraCdXUfTask = from c in db.TwmsRegraCdXUfs
                                                    where c.Id_wms_regra_cd == itemRegra.TwmsRegraCd.Id &&
                                                          c.Uf == prePedido.DadosCliente.Uf
                                                    select c;
                            var wmsRegraCdXUf = await wmsRegraCdXUfTask.FirstOrDefaultAsync();

                            if (wmsRegraCdXUf == null)
                            {
                                itemRegra.St_Regra = false;
                                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                    Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                    item.NumProduto + " não está cadastrada para a UF '" + prePedido.DadosCliente.Uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
                            }
                            else
                            {
                                itemRegra.TwmsRegraCdXUf = new Regras.t_WMS_REGRA_CD_X_UF
                                {
                                    Id = wmsRegraCdXUf.Id,
                                    Id_wms_regra_cd = wmsRegraCdXUf.Id_wms_regra_cd,
                                    Uf = wmsRegraCdXUf.Uf,
                                    St_inativo = wmsRegraCdXUf.St_inativo
                                };

                                //buscar a sigla tipo pessoa
                                var tipo_pessoa = Util.MultiCdRegraDeterminaPessoa(prePedido.DadosCliente.Tipo,
                                    prePedido.DadosCliente.Contribuinte_Icms_Status, prePedido.DadosCliente.ProdutorRural);

                                var wmsRegraCdXUfXPessoaTask = from c in db.TwmsRegraCdXUfPessoas
                                                               where c.Id_wms_regra_cd_x_uf == itemRegra.TwmsRegraCdXUf.Id &&
                                                                     c.Tipo_pessoa == tipo_pessoa
                                                               select c;

                                var wmsRegraCdXUfXPessoa = await wmsRegraCdXUfXPessoaTask.FirstOrDefaultAsync();

                                if (wmsRegraCdXUfXPessoa == null)
                                {
                                    itemRegra.St_Regra = false;
                                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                        Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                        item.NumProduto + " não está cadastrada para a UF '" + prePedido.DadosCliente.Uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
                                }
                                else
                                {
                                    itemRegra.TwmsRegraCdXUfXPessoa = new Regras.t_WMS_REGRA_CD_X_UF_X_PESSOA
                                    {
                                        Id = wmsRegraCdXUfXPessoa.Id,
                                        Id_wms_regra_cd_x_uf = wmsRegraCdXUfXPessoa.Id_wms_regra_cd_x_uf,
                                        Tipo_pessoa = wmsRegraCdXUfXPessoa.Tipo_pessoa,
                                        St_inativo = wmsRegraCdXUfXPessoa.St_inativo,
                                        Spe_id_nfe_emitente = wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                    };

                                    if (wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                                    {
                                        itemRegra.St_Regra = false;
                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                            Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                            item.NumProduto + " não especifica nenhum CD para aguardar produtos sem presença no estoque (Id=" + regra.Id_wms_regra_cd + ")");
                                    }
                                    else
                                    {
                                        var nfEmitenteTask = from c in db.TnfEmitentes
                                                             where c.Id == wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                                             select c;
                                        var nfEmitente = await nfEmitenteTask.FirstOrDefaultAsync();

                                        if (nfEmitente != null)
                                        {
                                            if (nfEmitente.St_Ativo != 1)
                                            {
                                                itemRegra.St_Regra = false;
                                                lstErros.Add("Falha na regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                                    Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante +
                                                    ")" + item.NumProduto + " especifica um CD para aguardar produtos sem presença no estoque que não está habilitado " +
                                                    "(Id=" + regra.Id_wms_regra_cd + ")");
                                            }
                                        }
                                        var wmsRegraCdXUfXPessoaXcdTask = from c in db.TwmsRegraCdXUfXPessoaXCds
                                                                          where c.Id_wms_regra_cd_x_uf_x_pessoa == wmsRegraCdXUfXPessoa.Id
                                                                          orderby c.Ordem_prioridade
                                                                          select c;
                                        var wmsRegraCdXUfXPessoaXcd = await wmsRegraCdXUfXPessoaXcdTask.ToListAsync();

                                        if (wmsRegraCdXUfXPessoaXcd.Count == 0)
                                        {
                                            itemRegra.St_Regra = false;
                                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                                Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                                item.NumProduto + " não especifica nenhum CD para consumo do estoque (Id=" + regra.Id_wms_regra_cd + ")");
                                        }
                                        else
                                        {
                                            itemRegra.TwmsCdXUfXPessoaXCd = new List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD>();
                                            foreach (var i in wmsRegraCdXUfXPessoaXcd)
                                            {
                                                Regras.t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new Regras.t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
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

                                                itemRegra.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
                                            }
                                            foreach (var i in itemRegra.TwmsCdXUfXPessoaXCd)
                                            {

                                                if (i.Id_nfe_emitente == itemRegra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                                {
                                                    if (i.St_inativo == 1)
                                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                                            Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" +
                                                            item.Fabricante + ")" + item.NumProduto + " especifica o CD '" + Util.ObterApelidoEmpresaNfeEmitentes(wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente, contextoProvider) +
                                                            "' para alocação de produtos sem presença no estoque, sendo que este CD está desativado para " +
                                                            "processar produtos disponíveis (Id=" + regra.Id_wms_regra_cd + ")");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            lstRegrasCrtlEstoque.Add(itemRegra);
                        }
                    }
                }

            }

            return lstRegrasCrtlEstoque;
        }

        private async Task ComplementarInfosOrcamentoItem(ContextoBdGravacao dbgravacao, List<TorcamentoItem> lstOrcamentoItem,
            string loja, List<TpercentualCustoFinanceiroFornecedor> lstPercentualCustoFinanFornec)
        {
            int indiceItem = 0;

            TorcamentoItem orcItem = new TorcamentoItem();
            foreach (var percCustoFinanFornec in lstPercentualCustoFinanFornec)
            {
                foreach (TorcamentoItem item in lstOrcamentoItem)
                {
                    if (percCustoFinanFornec.Fabricante == item.Fabricante)
                    {
                        var prodLista = from c in dbgravacao.TprodutoLojas.Include(x => x.Tproduto).Include(x => x.Tproduto.Tfabricante)
                                        where c.Tproduto.Tfabricante.Fabricante == item.Fabricante &&
                                              c.Loja == loja &&
                                              c.Tproduto.Produto == item.Produto
                                        select c;

                        var prod = await prodLista.FirstOrDefaultAsync();

                        if (prod != null)
                        {
                            //montagem das informações do produto                    
                            item.Margem = prod.Margem;
                            item.Desc_Max = prod.Desc_Max;
                            item.Comissao = prod.Comissao;
                            item.Preco_Fabricante = prod.Tproduto.Preco_Fabricante;
                            item.Vl_Custo2 = prod.Tproduto.Vl_Custo2;
                            item.Descricao = prod.Tproduto.Descricao;
                            item.Descricao_Html = prod.Tproduto.Descricao_Html;
                            item.Ean = prod.Tproduto.Ean;
                            item.Grupo = prod.Tproduto.Grupo;
                            item.Peso = prod.Tproduto.Peso;
                            item.Qtde_Volumes = prod.Tproduto.Qtde_Volumes;
                            item.Markup_Fabricante = prod.Tproduto.Tfabricante.Markup;
                            item.Cubagem = prod.Tproduto.Cubagem;
                            item.Ncm = prod.Tproduto.Ncm;
                            item.Cst = prod.Tproduto.Cst;
                            item.Descontinuado = prod.Tproduto.Descontinuado;
                            item.CustoFinancFornecPrecoListaBase = Math.Round((decimal)prod.Preco_Lista, 2);
                            item.CustoFinancFornecCoeficiente = percCustoFinanFornec.Coeficiente;
                            item.Qtde_Spe = 0; //essa quantidade não esta sendo alterada
                            item.Abaixo_Min_Status = 0; //sempre recebe 0 aqui
                            item.Abaixo_Min_Autorizacao = "";//sempre esta vazio no modulo orcamento
                            item.Abaixo_Min_Autorizador = "";//sempre esta vazio no modulo orcamento
                            item.Abaixo_Min_Superv_Autorizador = "";//sempre esta vazio no modulo orcamento
                            item.Sequencia = (short?)RenumeraComBase1(indiceItem);
                            item.Subgrupo = prod.Tproduto.Subgrupo;

                            indiceItem++;
                        }
                    }
                }
            }

        }

        //afazer: criar metodo passando o valor de indice do produto
        /*
         * param o indice_item, 1, e o indice do item
         */
        private int RenumeraComBase1(int indiceItem)
        {
            int retorno;
            retorno = indiceItem + 1;

            return retorno;
        }

        private List<TorcamentoItem> MontaListaOrcamentoItem(PrePedidoDto prepedido)
        {
            List<TorcamentoItem> lstOrcamentoItem = new List<TorcamentoItem>();

            foreach (PrepedidoProdutoDtoPrepedido p in prepedido.ListaProdutos)
            {
                TorcamentoItem item = new TorcamentoItem
                {
                    Orcamento = prepedido.NumeroPrePedido,
                    Produto = p.NumProduto,
                    Fabricante = Utils.Util.Normaliza_Codigo(p.Fabricante, Constantes.TAM_MIN_FABRICANTE),
                    Qtde = p.Qtde,
                    Preco_Venda = Math.Round(p.VlUnitario, 2),
                    Preco_NF = prepedido.PermiteRAStatus == 1 ? Math.Round((decimal)p.Preco_Lista, 2) : Math.Round(p.VlUnitario, 2),
                    Obs = p.Obs == null ? "" : p.Obs,
                    Desc_Dado = p.Desconto,
                    Preco_Lista = Math.Round(p.VlLista, 2)
                };
                lstOrcamentoItem.Add(item);
            }

            return lstOrcamentoItem;
        }

        public async Task<bool> ValidarOrcamentistaIndicador(string apelido)
        {
            bool retorno = false;

            var db = contextoProvider.GetContextoLeitura();

            var orcamentistaTask = await (from c in db.TorcamentistaEindicadors
                                          where c.Apelido == apelido
                                          select c.Apelido).FirstOrDefaultAsync();
            if (orcamentistaTask == apelido)
                return retorno = true;

            return retorno;
        }

        private string DecodificaCustoFinanFornecQtdeParcelas(string tipoParcelamento, short custoFFQtdeParcelas)
        {
            string retorno = "";

            if (tipoParcelamento == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
                retorno = "0+" + custoFFQtdeParcelas;
            else if (tipoParcelamento == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA)
                retorno = "1+" + custoFFQtdeParcelas;

            return retorno;
        }

        private async Task<TorcamentistaEindicador> BuscarTorcamentista(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var tOrcamentistaTask = from c in db.TorcamentistaEindicadors
                                    where c.Apelido == apelido
                                    select c;
            TorcamentistaEindicador tOrcamentista = await tOrcamentistaTask.FirstOrDefaultAsync();

            return tOrcamentista;
        }

        public async Task GerarNumeroOrcamento(ContextoBdGravacao dbgravacao, PrePedidoDto prepedido)
        {
            string sufixoIdOrcamento = Constantes.SUFIXO_ID_ORCAMENTO;

            var nsuTask = await Util.GerarNsu(dbgravacao, Constantes.NSU_ORCAMENTO, contextoProvider);
            string nsu = nsuTask.ToString();

            int ndescarte = nsu.Length - Constantes.TAM_MIN_NUM_ORCAMENTO;
            string sdescarte = nsu.Substring(0, ndescarte);

            if (sdescarte.Length == ndescarte)
            {
                prepedido.NumeroPrePedido = nsu.Substring(ndescarte) + sufixoIdOrcamento;
            }
        }
    }
}
