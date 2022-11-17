using Cep;
using Cliente.Dados;
using FormaPagamento;
using FormaPagamento.Dados;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prepedido.Dados;
using Prepedido.Dados.DetalhesPrepedido;
using Produto;
using Produto.RegrasCrtlEstoque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilsGlobais;

namespace Prepedido.Bll
{
    public class PrepedidoBll
    {
        private readonly ILogger<PrepedidoBll> _logger;
        private readonly ContextoBdProvider contextoProvider;
        private readonly Cliente.ClienteBll clienteBll;
        private readonly ValidacoesPrepedidoBll validacoesPrepedidoBll;
        private readonly CepBll cepBll;
        private readonly ValidacoesFormaPagtoBll validacoesFormaPagtoBll;
        private readonly MontarLogPrepedidoBll montarLogPrepedidoBll;
        private readonly IBancoNFeMunicipio bancoNFeMunicipio;
        private readonly FormaPagtoBll formaPagtoBll;
        private readonly PedidoVisualizacao.PedidoVisualizacaoBll pedidoVisualizacaoBll;

        public PrepedidoBll(
            ILogger<PrepedidoBll> logger,
            ContextoBdProvider contextoProvider,
            Cliente.ClienteBll clienteBll,
            ValidacoesPrepedidoBll validacoesPrepedidoBll,
            CepBll cepBll,
            ValidacoesFormaPagtoBll validacoesFormaPagtoBll,
            Prepedido.Bll.MontarLogPrepedidoBll montarLogPrepedidoBll,
            IBancoNFeMunicipio bancoNFeMunicipio,
            FormaPagtoBll formaPagtoBll,
            PedidoVisualizacao.PedidoVisualizacaoBll pedidoVisualizacaoBll
            )
        {
            this._logger = logger;
            this.contextoProvider = contextoProvider;
            this.clienteBll = clienteBll;
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
            this.cepBll = cepBll;
            this.validacoesFormaPagtoBll = validacoesFormaPagtoBll;
            this.montarLogPrepedidoBll = montarLogPrepedidoBll;
            this.bancoNFeMunicipio = bancoNFeMunicipio;
            this.formaPagtoBll = formaPagtoBll;
            this.pedidoVisualizacaoBll = pedidoVisualizacaoBll;
        }

        public async Task<IEnumerable<string>> ListarNumerosPrepedidosCombo(string orcamentista)
        {
            //toda vez precisamos de uma nova conexao para os casos em que houver transacao
            var db = contextoProvider.GetContextoLeitura();
            var lista = from r in db.Torcamento
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

            var lista = (from c in db.Torcamento
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

        public async Task<IEnumerable<PrepedidosCadastradosPrepedidoDados>> ListarPrePedidos(string apelido, TipoBuscaPrepedido tipoBusca,
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
        public async Task<IEnumerable<PrepedidosCadastradosPrepedidoDados>> ListarPrePedidosFiltroEstrito(string apelido, TipoBuscaPrepedido tipoBusca,
                string clienteBusca, string numeroPrePedido, DateTime? dataInicial, DateTime? dataFinal)
        {

            var db = contextoProvider.GetContextoLeitura();

            var lst = db.Torcamento.
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

            //lst.OrderByDescending(r => r.Data_hora);
            List<PrepedidosCadastradosPrepedidoDados> lstdados = new List<PrepedidosCadastradosPrepedidoDados>();
            //COLOCAR O STATUS DO PEDIDO PARA PREPEDIDOS QUE VIRARAM PEDIDOS
            if (tipoBusca != TipoBuscaPrepedido.Excluidos)
            {

                lstdados = lst.Select(r => new PrepedidosCadastradosPrepedidoDados
                {

                    Status = r.St_Orc_Virou_Pedido == 1 ? "Pedido em andamento" : "Pedido em processamento",
                    DataPrePedido = r.Data,
                    NumeroPrepedido = r.Orcamento,
                    NomeCliente = r.Endereco_nome,
                    ValoTotal = r.Permite_RA_Status == 1 ? r.Vl_Total_NF : r.Vl_Total
                }).OrderByDescending(r => r.NumeroPrepedido).ToList();
            }
            if (tipoBusca == TipoBuscaPrepedido.Excluidos)
            {
                lstdados = lst.Select(r => new PrepedidosCadastradosPrepedidoDados
                {
                    Status = "Excluído",
                    DataPrePedido = r.Data,
                    NumeroPrepedido = r.Orcamento,
                    NomeCliente = r.Endereco_nome,
                    ValoTotal = r.Permite_RA_Status == 1 && r.Permite_RA_Status != 0 ? r.Vl_Total_NF : r.Vl_Total
                }).OrderByDescending(r => r.NumeroPrepedido).ToList();
            }

            return await Task.FromResult(lstdados);
        }

        public async Task<bool> RemoverPrePedido(string numeroPrePedido, string apelido)
        {
            this._logger.LogInformation($"Começo do processo para Remover Pré-Pedido.");

            if (string.IsNullOrEmpty(apelido))
            {
                this._logger.LogInformation($"Não foi possivel remover Pré-Pedido. Campos apelido não foi preenchido.");
                return await Task.FromResult(false);
            }

            if (string.IsNullOrEmpty(numeroPrePedido))
            {
                this._logger.LogInformation($"Não foi possivel remover Pré-Pedido. Campos numeroPrePedido não foi preenchido.");
                return await Task.FromResult(false);
            }

            this._logger.LogInformation($"Remover Pré-Pedido: {numeroPrePedido} com usuário: {apelido}.");

            try
            {
                this._logger.LogInformation($"Abrindo contexto para consulta e update para remover Pré-Pedido: {numeroPrePedido} com usuário: {apelido}.");
                using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTO))
                {
                    var prepedido = await dbgravacao.Torcamento
                        .Where(c => c.Orcamento == numeroPrePedido
                            && (c.St_Orcamento == "" || c.St_Orcamento == null)
                            && c.St_Orc_Virou_Pedido == 0)
                        .FirstOrDefaultAsync();

                    if (prepedido != null)
                    {
                        prepedido.St_Orcamento = "CAN";
                        prepedido.Cancelado_Data = DateTime.Now;
                        prepedido.Cancelado_Usuario = apelido;
                        await dbgravacao.SaveChangesAsync();
                        dbgravacao.transacao.Commit();

                        this._logger.LogInformation($"Pré-Pedido: {numeroPrePedido} com usuário: {apelido}, foi cancelado com sucesso.");
                        return await Task.FromResult(true);
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogInformation($"Ocorreu um problema para cancelar Pré-Pedido: {numeroPrePedido} com usuário: {apelido}, foi cancelado com sucesso. Exception: {ex.Message}");
                throw new Exception(ex.Message);
            }

            this._logger.LogInformation($"Pré-Pedido: {numeroPrePedido} com usuário: {apelido}, não foi cancelado.");
            return await Task.FromResult(false);
        }

        public async Task<PrePedidoDados> BuscarPrePedido(string apelido, string numPrePedido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var prepedido = from c in db.Torcamento
                            where c.Orcamento == numPrePedido
                            select c;


            Torcamento pp = prepedido.FirstOrDefault();

            Tloja t = await (from c in db.Tloja
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
                var pedido = from c in db.Tpedido
                             where c.Orcamento == numPrePedido
                             select c.Pedido;

                PrePedidoDados prepedidoPedido = new PrePedidoDados
                {
                    St_Orc_Virou_Pedido = true,
                    NumeroPedido = pedido.Select(r => r.ToString()).FirstOrDefault()
                };
                return prepedidoPedido;
            }

            Cliente.Dados.DadosClienteCadastroDados cadastroCliente = new Cliente.Dados.DadosClienteCadastroDados();
            if (pp.St_memorizacao_completa_enderecos == 0)
            {
                cadastroCliente = await ObterDadosCliente(t.Loja, pp.Orcamentista, pp.Vendedor, pp.Id_Cliente);
            }
            else
            {
                //vamos preencher os dados do cliente com o prepedido
                cadastroCliente = await ObterDadosClientePrepedido(pp, t.Loja);
            }
            var enderecoEntregaTask = ObterEnderecoEntrega(pp);
            var lstProdutoTask = await ObterProdutos(pp);

            var vltotalRa = lstProdutoTask.Select(r => r.VlTotalRA).Sum();
            var totalDestePedidoComRa = lstProdutoTask.Select(r => r.TotalItemRA).Sum();
            var totalDestePedido = lstProdutoTask.Select(r => r.VlTotalItem).Sum();


            PrePedidoDados prepedidoDados = new PrePedidoDados
            {
                CorHeader = corHeader,
                TextoHeader = textoHeader,
                CanceladoData = canceladoData,
                NumeroPrePedido = pp.Orcamento,
                DataHoraPedido = Convert.ToString(pp.Data?.ToString("dd/MM/yyyy")),
                Hora_Prepedido = Util.FormataHora(pp.Hora),
                DadosCliente = cadastroCliente,
                EnderecoEntrega = await enderecoEntregaTask,
                ListaProdutos = lstProdutoTask.ToList(),
                TotalFamiliaParcelaRA = vltotalRa,
                PermiteRAStatus = pp.Permite_RA_Status,
                CorTotalFamiliaRA = vltotalRa > 0 ? "green" : "red",
                PercRT = pp.Perc_RT,
                Vl_total_NF = totalDestePedidoComRa,
                Vl_total = totalDestePedido,
                DetalhesPrepedido = ObterDetalhesPrePedido(pp, apelido),
                FormaPagto = ObterFormaPagto(pp).ToList(),
                FormaPagtoCriacao = await ObterFormaPagtoPrePedido(pp)
            };

            return await Task.FromResult(prepedidoDados);
        }

        public async Task<decimal> ObtemPercentualVlPedidoRA()
        {
            var db = contextoProvider.GetContextoLeitura();

            string percentual = await (from c in db.Tcontrole
                                       where c.Id_Nsu == Constantes.ID_PARAM_PercVlPedidoLimiteRA
                                       select c.Nsu).FirstOrDefaultAsync();

            decimal retorno = decimal.Parse(percentual);

            return retorno;
        }

        private async Task<string> ObterDescricaoFormaPagto(short av_forma_pagto)
        {
            var db = contextoProvider.GetContextoLeitura();

            var tipoTask = from c in db.TformaPagto
                           where c.Id == av_forma_pagto
                           select c.Descricao;

            string tipo = await tipoTask.FirstOrDefaultAsync();
            return tipo;
        }

        public async Task<FormaPagtoCriacaoDados> ObterFormaPagtoPrePedido(Torcamento torcamento)
        {
            FormaPagtoCriacaoDados pagto = new FormaPagtoCriacaoDados();

            //descrição d meio de pagto
            pagto.Descricao_meio_pagto = await ObterDescricaoFormaPagto(torcamento.Av_Forma_Pagto);
            pagto.Tipo_parcelamento = torcamento.Tipo_Parcelamento;
            pagto.Qtde_Parcelas_Para_Exibicao = (int)torcamento.Qtde_Parcelas;

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

        private DetalhesPrepedidoDados ObterDetalhesPrePedido(Torcamento torcamento, string apelido)
        {
            //TODO: Retornar usuario torcamento.Etg_Imediata_Usuario
            //var usuario = torcamento.Etg_Imediata_Usuario // tipo usuario[1] idusario 50116 

            var detail = new DetalhesPrepedidoDados
            {
                Observacoes = torcamento.Obs_1,
                NumeroNF = torcamento.Obs_2,

                PrevisaoEntregaTexto = torcamento.St_Etg_Imediata == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO ?
                torcamento.PrevisaoEntregaData?.ToString("dd/MM/yyyy") + " (" + Texto.iniciaisEmMaiusculas(torcamento.Etg_Imediata_Usuario) +
                " - " + torcamento.PrevisaoEntregaDtHrUltAtualiz?.ToString("dd/MM/yyyy HH:mm") + ")" : null,

                EntregaImediata = torcamento.St_Etg_Imediata == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO ?
                "NÃO (" + Texto.iniciaisEmMaiusculas(torcamento.Etg_Imediata_Usuario) + " - " + torcamento.Etg_Imediata_Data?.ToString("dd/MM/yyyy HH:mm") + ")" :
                "SIM (" + Texto.iniciaisEmMaiusculas(torcamento.Etg_Imediata_Usuario) + " - " + torcamento.Etg_Imediata_Data?.ToString("dd/MM/yyyy HH:mm") + ")",

                BemDeUso_Consumo = torcamento.StBemUsoConsumo,
                InstaladorInstala = torcamento.InstaladorInstalaStatus,
                GarantiaIndicadorTexto = Convert.ToString(torcamento.GarantiaIndicadorStatus) ==
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
                    lista.Add(String.Format("Prestações: " + torcamento.Pce_Prestacao_Qtde + " x " + " {0:c2}" +
                            " (" + Util.OpcaoFormaPagto(torcamento.Pce_Forma_Pagto_Prestacao) +
                            ") vencendo a cada " +
                            torcamento.Pce_Prestacao_Periodo + " dias", torcamento.Pce_Prestacao_Valor));
                    //if (torcamento.Pce_Forma_Pagto_Prestacao != 5 && torcamento.Pce_Forma_Pagto_Prestacao != 7)
                    //{
                    //    lista.Add(String.Format("Prestações: " + torcamento.Pce_Prestacao_Qtde + " x " + " {0:c2}" +
                    //        " (" + Util.OpcaoFormaPagto(torcamento.Pce_Forma_Pagto_Prestacao) +
                    //        ") vencendo a cada " +
                    //        torcamento.Pce_Prestacao_Periodo + " dias", torcamento.Pce_Prestacao_Valor));
                    //}
                    //else
                    //{
                    //    lista.Add(String.Format("Prestações: " + torcamento.Pce_Prestacao_Qtde + " x " + " {0:c2}" +
                    //        " (" + Util.OpcaoFormaPagto(torcamento.Pce_Forma_Pagto_Prestacao) + ")",
                    //        torcamento.Pce_Prestacao_Valor));
                    //}
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
        private async Task<IEnumerable<PrepedidoProdutoPrepedidoDados>> ObterProdutos(Torcamento orc)
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtos = from c in db.TorcamentoItem
                           where c.Orcamento == orc.Orcamento
                           orderby c.Sequencia
                           select c;

            List<PrepedidoProdutoPrepedidoDados> listaProduto = new List<PrepedidoProdutoPrepedidoDados>();

            foreach (var p in produtos)
            {
                PrepedidoProdutoPrepedidoDados produtoPrepedido = new PrepedidoProdutoPrepedidoDados
                {
                    Fabricante = p.Fabricante,
                    Produto = p.Produto,
                    Descricao = p.Descricao_Html,
                    Obs = p.Obs,
                    Qtde = p.Qtde,
                    Permite_Ra_Status = orc.Permite_RA_Status,
                    BlnTemRa = p.Preco_NF != p.Preco_Venda ? true : false,
                    Preco_NF = p.Preco_NF ?? 0,
                    CustoFinancFornecPrecoListaBase = p.CustoFinancFornecPrecoListaBase,//essa variavel não pode ter o valor alterado
                    Preco_Lista = (decimal)p.Preco_Lista,//essa variavel é o valor base para calcular 
                    Desc_Dado = p.Desc_Dado ?? 0,
                    Preco_Venda = p.Preco_Venda,
                    VlTotalRA = (decimal)(p.Qtde * (p.Preco_NF - p.Preco_Venda)),
                    Comissao = orc.Perc_RT,
                    TotalItemRA = (p.Qtde ?? 0) * (p.Preco_NF ?? 0),
                    TotalItem = (p.Qtde ?? 0) * (p.Preco_Venda),
                    VlTotalItem = (p.Qtde ?? 0) * (p.Preco_Venda)

                };

                listaProduto.Add(produtoPrepedido);
            }

            return await Task.FromResult(listaProduto);
        }

        private async Task<Cliente.Dados.DadosClienteCadastroDados> ObterDadosClientePrepedido(Torcamento orcamento, string loja)
        {
            var dadosCliente = from c in contextoProvider.GetContextoLeitura().Tcliente
                               where c.Id == orcamento.Id_Cliente
                               select c;
            var cli = await dadosCliente.FirstOrDefaultAsync();

            Cliente.Dados.DadosClienteCadastroDados cadastroCliente = new Cliente.Dados.DadosClienteCadastroDados
            {
                Loja = await pedidoVisualizacaoBll.ObterRazaoSocial_Nome_Loja(loja),
                Indicador_Orcamentista = orcamento.Orcamentista,
                Vendedor = orcamento.Vendedor,
                Id = cli.Id,
                Cnpj_Cpf = Util.FormatCpf_Cnpj_Ie(orcamento.Endereco_cnpj_cpf),
                Rg = orcamento.Endereco_rg,
                Ie = Util.FormatCpf_Cnpj_Ie(orcamento.Endereco_ie),
                Contribuinte_Icms_Status = orcamento.Endereco_contribuinte_icms_status,
                Tipo = cli.Tipo,
                Observacao_Filiacao = cli.Filiacao,
                Nome = orcamento.Endereco_nome,
                ProdutorRural = orcamento.Endereco_produtor_rural_status,
                DddResidencial = orcamento.Endereco_ddd_res,
                TelefoneResidencial = orcamento.Endereco_tel_res,
                DddComercial = orcamento.Endereco_ddd_com,
                TelComercial = orcamento.Endereco_tel_com,
                Ramal = orcamento.Endereco_ramal_com,
                DddCelular = orcamento.Endereco_ddd_cel,
                Celular = orcamento.Endereco_tel_cel,
                TelComercial2 = orcamento.Endereco_tel_com_2,
                DddComercial2 = orcamento.Endereco_ddd_com_2,
                Ramal2 = orcamento.Endereco_ramal_com_2,
                Email = orcamento.Endereco_email,
                EmailXml = orcamento.Endereco_email_xml,
                Endereco = orcamento.Endereco_logradouro,
                Complemento = orcamento.Endereco_complemento,
                Numero = orcamento.Endereco_numero,
                Bairro = orcamento.Endereco_bairro,
                Cidade = orcamento.Endereco_cidade,
                Uf = orcamento.Endereco_uf,
                Cep = orcamento.Endereco_cep,
                Contato = orcamento.Endereco_contato
            };

            return cadastroCliente;
        }

        private async Task<Cliente.Dados.DadosClienteCadastroDados> ObterDadosCliente(string loja, string indicador_orcamentista, string vendedor, string idCliente)
        {
            //afazer: criar a condição para preencher os dados do cliente que estão salvos no t_ORCAMENTO ou no t_CLIENTE

            var dadosCliente = from c in contextoProvider.GetContextoLeitura().Tcliente
                               where c.Id == idCliente
                               select c;
            var cli = await dadosCliente.FirstOrDefaultAsync();
            Cliente.Dados.DadosClienteCadastroDados cadastroCliente = new Cliente.Dados.DadosClienteCadastroDados
            {
                Loja = await pedidoVisualizacaoBll.ObterRazaoSocial_Nome_Loja(loja),
                Indicador_Orcamentista = indicador_orcamentista,
                Vendedor = vendedor,
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

        private async Task<Cliente.Dados.EnderecoEntregaClienteCadastroDados> ObterEnderecoEntrega(Torcamento p)
        {
            Cliente.Dados.EnderecoEntregaClienteCadastroDados enderecoEntrega = new Cliente.Dados.EnderecoEntregaClienteCadastroDados();
            enderecoEntrega.OutroEndereco = Convert.ToBoolean(p.St_End_Entrega);

            //afazer: criar método para pegar todos os dados de endereço com os campos novos

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
                if (p.EndEtg_Cod_Justificativa.Length == 1 && p.EndEtg_Cod_Justificativa != "0")
                    p.EndEtg_Cod_Justificativa = "00" + p.EndEtg_Cod_Justificativa;
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
            }

            return enderecoEntrega;
        }

        public async Task<short> Obter_Permite_RA_Status(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var raStatus = (from c in db.TorcamentistaEindicador
                            where c.Apelido == apelido
                            select c.Permite_RA_Status).FirstOrDefaultAsync();

            return await raStatus;
        }

        public async Task<IEnumerable<string>> CadastrarPrepedido(PrePedidoDados prePedido, string apelido,
            decimal limiteArredondamento, bool verificarPrepedidoRepetido, Constantes.CodSistemaResponsavel sistemaResponsavelCadastro,
            int limite_de_itens, ContextoBdGravacao dbGravacao)
        {
            List<string> lstErros = new List<string>();

            string loja = "";
            if (!string.IsNullOrEmpty(apelido))
            {
                TorcamentistaEindicador tOrcamentista = await BuscarTorcamentista(apelido);
                if (tOrcamentista == null)
                {
                    lstErros.Add("O Orçamentista não existe!");
                    return lstErros;
                }

                //complementar os dados Cadastrais do cliente
                prePedido.DadosCliente.Indicador_Orcamentista = tOrcamentista.Apelido.ToUpper();
                prePedido.DadosCliente.Loja = tOrcamentista.Loja;
                prePedido.DadosCliente.Vendedor = tOrcamentista.Vendedor?.ToUpper();
                prePedido.EnderecoCadastroClientePrepedido.Vendedor = tOrcamentista.Vendedor?.ToUpper();

                if (string.IsNullOrEmpty(tOrcamentista.Vendedor))
                    lstErros.Add("NÃO HÁ NENHUM VENDEDOR DEFINIDO PARA ATENDÊ-LO");

                //validar o Orcamentista
                if (tOrcamentista.Apelido != apelido)
                    lstErros.Add("Falha ao recuperar os dados cadastrais!");

                loja = tOrcamentista.Loja;
            }


            if (string.IsNullOrEmpty(prePedido.DadosCliente.Id))
            {
                lstErros.Add("Id do cliente não informado.");
            }

            //setamos a loja aqui para o caso de não ter parceiro
            loja = !string.IsNullOrEmpty(apelido) ? prePedido.DadosCliente.Loja : loja;

            //verificamos se tem ID para saber que o cliente existe
            Tcliente cliente = (await clienteBll.BuscarTclienteComTransacao(prePedido.EnderecoCadastroClientePrepedido.Endereco_cnpj_cpf,
                dbGravacao));

            if (cliente != null)
            {
                prePedido.DadosCliente.Id = cliente.Id;
            }

            if (cliente.Tipo == Constantes.ID_PF)
            {
                if (prePedido.EnderecoCadastroClientePrepedido.Endereco_tipo_pessoa != Constantes.ID_PF)
                {
                    lstErros.Add("Se cliente é tipo PF, o tipo da pessoa de endereço cadastral deve ser PF.");
                }
            }
            if (cliente.Tipo == Constantes.ID_PJ)
            {
                if (prePedido.EnderecoCadastroClientePrepedido.Endereco_tipo_pessoa != Constantes.ID_PJ)
                {
                    lstErros.Add("Se cliente é tipo PJ, o tipo da pessoa de endereço cadastral deve ser PJ.");
                }
            }

            //antes de validar vamos passar o EnderecoCadastral para dadoscliente
            prePedido.DadosCliente =
                DadosClienteCadastroDados.DadosClienteCadastroDadosDeEnderecoCadastralClientePrepedidoDados(
                    prePedido.EnderecoCadastroClientePrepedido, apelido?.ToUpper(), prePedido.DadosCliente.Loja, prePedido.DadosCliente.Id,
                    prePedido.DadosCliente.UsuarioCadastro);

            //vamos validar os dados do cliente
            await Cliente.ValidacoesClienteBll.ValidarDadosCliente(prePedido.DadosCliente, false,
                null, null,
                lstErros, contextoProvider, cepBll, bancoNFeMunicipio, null,
                prePedido.DadosCliente.Tipo == Constantes.ID_PF ? true : false, sistemaResponsavelCadastro, false, cliente);

            //verificar como esta sendo salvo
            if (!ValidacoesPrepedidoBll.ValidarDetalhesPrepedido(prePedido.DetalhesPrepedido, lstErros))
            {
                return lstErros;
            }

            if (prePedido.ListaProdutos.Count > limite_de_itens)
            {
                lstErros.Add($"É permitido apenas {limite_de_itens} itens.");
                return lstErros;
            }
            if (!await Util.LojaHabilitadaProdutosECommerce(prePedido.DadosCliente.Loja, contextoProvider))
            {
                lstErros.Add($"Loja não habilitada para e-commerce: {prePedido.DadosCliente.Loja}");
                return lstErros;
            }

            //Validar endereço de entraga
            await validacoesPrepedidoBll.ValidarEnderecoEntrega(prePedido.EnderecoEntrega, lstErros,
                prePedido.DadosCliente.Indicador_Orcamentista, prePedido.DadosCliente.Tipo, true, prePedido.DadosCliente.Loja,
                sistemaResponsavelCadastro);
            if (lstErros.Any())
                return lstErros;

            //busca a sigla do tipo de pagamento pelo código enviado
            string c_custoFinancFornecTipoParcelamento = ObterSiglaFormaPagto(prePedido.FormaPagtoCriacao);

            //precisa incluir uma validação de forma de pagamento com base no orçamentista enviado
            FormaPagtoDados formasPagto = await formaPagtoBll.ObterFormaPagto(apelido ?? prePedido.DadosCliente.Vendedor, prePedido.DadosCliente.Tipo, sistemaResponsavelCadastro);
            validacoesFormaPagtoBll.ValidarFormaPagto(prePedido.FormaPagtoCriacao, lstErros, limiteArredondamento,
                0.1M, c_custoFinancFornecTipoParcelamento, formasPagto, prePedido.PermiteRAStatus,
                prePedido.Vl_total_NF, prePedido.Vl_total);
            if (lstErros.Any())
                return lstErros;

            //Esta sendo verificado qual o tipo de pagamento que esta sendo feito e retornando a quantidade de parcelas
            int c_custoFinancFornecQtdeParcelas = ObterCustoFinancFornecQtdeParcelasDeFormaPagto(prePedido.FormaPagtoCriacao);

            float perc_limite_RA_sem_desagio = await Util.VerificarSemDesagioRA(contextoProvider);

            decimal percVlPedidoRA = await ObtemPercentualVlPedidoRA();

            //Vamos conforntar os valores de cada item, total do prepedido e o percentual máximo de RA
            //afazer: verificar se a lista de produtos contém itens
            if (prePedido.ListaProdutos.Count <= 0)
                lstErros.Add("Não há itens na lista de produtos!");

            if (sistemaResponsavelCadastro != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO)
            {
                await validacoesPrepedidoBll.MontarProdutosParaComparacao(prePedido,
                c_custoFinancFornecTipoParcelamento, c_custoFinancFornecQtdeParcelas,
                prePedido.DadosCliente.Loja, lstErros, percVlPedidoRA, limiteArredondamento,
                ValidacoesPrepedidoBll.AmbienteValidacao.PrepedidoValidacao);
            }

            Util.ValidarTipoCustoFinanceiroFornecedor(lstErros, c_custoFinancFornecTipoParcelamento, c_custoFinancFornecQtdeParcelas);
            if (lstErros.Count > 0)
                return lstErros;

            //Calculamos os produtos com o coeficiente e retornamos uma lista de coeficientes dos fabricantes
            List<TpercentualCustoFinanceiroFornecedor> lstPercentualCustoFinanFornec =
            (await BuscarCoeficientePercentualCustoFinanFornec(prePedido,
                (short)c_custoFinancFornecQtdeParcelas, c_custoFinancFornecTipoParcelamento, lstErros)).ToList();

            Tparametro parametroRegra = await Util.BuscarRegistroParametro(Constantes.ID_PARAMETRO_Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal,
                contextoProvider);
            //esse metodo tb tras a sigla da pessoa
            string tipoPessoa = UtilsProduto.MultiCdRegraDeterminaPessoa(prePedido.DadosCliente.Tipo,
                (Constantes.ContribuinteICMS)prePedido.DadosCliente.Contribuinte_Icms_Status,
                (Constantes.ProdutorRural)prePedido.DadosCliente.ProdutorRural);
            string descricao = Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo);

            //List<RegrasBll> regraCrtlEstoque = new List<RegrasBll>();
            List<RegrasBll> regraCrtlEstoque = (await ObtemCtrlEstoqueProdutoRegra(contextoProvider, prePedido, lstErros)).ToList();
            if (lstErros.Count != 0) return lstErros;
            await UtilsProduto.ObterCtrlEstoqueProdutoRegra_Teste(lstErros, regraCrtlEstoque, prePedido.DadosCliente.Uf, tipoPessoa, contextoProvider);
            if (lstErros.Count != 0) return lstErros;

            Produto.ProdutoGeralBll.VerificarRegrasAssociadasAosProdutos(regraCrtlEstoque, lstErros, prePedido.DadosCliente.Uf, prePedido.DadosCliente.Tipo, 0);
            //obtendo qtde disponivel
            await UtilsProduto.VerificarEstoque(regraCrtlEstoque, contextoProvider);

            ObterDisponibilidadeEstoque(regraCrtlEstoque, prePedido, parametroRegra, lstErros);

            VerificarEstoqueInsuficiente(regraCrtlEstoque, prePedido, parametroRegra);

            //realiza a análise da quantidade de pedidos necessária(auto-split)
            VerificarQtdePedidosAutoSplit(regraCrtlEstoque, lstErros, prePedido);

            //contagem de empresas que serão usadas no auto-split, ou seja, a quantidade de pedidos que será cadastrada, 
            //já que cada pedido se refere ao estoque de uma empresa
            List<int> lst_empresa_selecionada = ContagemEmpresasUsadasAutoSplit(regraCrtlEstoque, prePedido);

            //há algum produto descontinuado?
            await ExisteProdutoDescontinuado(prePedido, lstErros);

            if (lstErros.Count <= 0)
            {
                //verifica se o prepedio já foi gravado
                if (verificarPrepedidoRepetido)
                {
                    var prepedidoJaCadastradoNumero = await new PrepedidoRepetidoBll(dbGravacao).PrepedidoJaCadastradoCriterioSiteColors(prePedido);
                    if (!String.IsNullOrEmpty(prepedidoJaCadastradoNumero))
                    {
                        lstErros.Add($"Esta solicitação já foi gravada com o número {prepedidoJaCadastradoNumero}");
                        return lstErros;
                    }
                }

                //Se orcamento existir, fazer o delete das informações
                if (!string.IsNullOrEmpty(prePedido.NumeroPrePedido))
                {
                    await DeletarOrcamentoExiste(dbGravacao, prePedido, apelido);
                }

                if (string.IsNullOrEmpty(prePedido.NumeroPrePedido))
                {
                    //gerar o numero de orçamento
                    await GerarNumeroOrcamento(dbGravacao, prePedido);
                }

                if (string.IsNullOrEmpty(prePedido.NumeroPrePedido))
                    lstErros.Add("FALHA NA OPERAÇÃO COM O BANCO DE DADOS AO TENTAR GERAR NSU.");

                //Cadastrar dados do Orcamento e endereço de entrega 
                string log = await EfetivarCadastroPrepedido(dbGravacao,
                    prePedido, loja, c_custoFinancFornecTipoParcelamento,
                    sistemaResponsavelCadastro, perc_limite_RA_sem_desagio);
                //Cadastrar orcamento itens
                List<TorcamentoItem> lstOrcamentoItem = (await MontaListaOrcamentoItem(prePedido,
                    lstPercentualCustoFinanFornec, dbGravacao, sistemaResponsavelCadastro)).ToList();

                //vamos passar o coeficiente que foi criado na linha 596 e passar como param para cadastrar nos itens
                //await ComplementarInfosOrcamentoItem(dbgravacao, lstOrcamentoItem,
                //    prePedido.DadosCliente.Loja);

                log = await CadastrarOrctoItens(dbGravacao, lstOrcamentoItem, log);

                bool gravouLog = Util.GravaLog(dbGravacao, apelido, prePedido.DadosCliente.Loja, prePedido.NumeroPrePedido,
                    prePedido.DadosCliente.Id, Constantes.OP_LOG_ORCAMENTO_NOVO, log);

                lstErros.Add(prePedido.NumeroPrePedido);
                //using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTO))
                //{
                //dbGravacao.transacao.Commit();
                //}
            }

            return lstErros;
        }

        public string ObterSiglaFormaPagto(FormaPagtoCriacaoDados formaPagto)
        {
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

        public async Task DeletarOrcamentoExiste(ContextoBdGravacao dbgravacao, PrePedidoDados prePedido, string apelido)
        {
            var orcamentoTask = from c in dbgravacao.Torcamento.Include(r => r.TorcamentoItem)
                                where c.Orcamento == prePedido.NumeroPrePedido &&
                                      c.Orcamentista == apelido
                                select c;

            Torcamento orcamento = orcamentoTask.FirstOrDefault();

            dbgravacao.Remove(orcamento);
            await dbgravacao.SaveChangesAsync();
        }

        //todo: apagar esta rotina
        public async Task DeletarOrcamentoExisteComTransacao(PrePedidoDados prePedido, string apelido)
        {
            using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_ORCAMENTO))
            {
                await DeletarOrcamentoExiste(dbgravacao, prePedido, apelido);
                dbgravacao.transacao.Commit();
            }
        }

        private async Task<string> EfetivarCadastroPrepedido(ContextoBdGravacao dbgravacao, PrePedidoDados prepedido,
            string loja, string siglaPagto, InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavelCadastro,
            float perc_limite_RA_sem_desagio = 0)
        {
            //vamos buscar a midia do cliente para cadastrar no orçamento
            string midia = await (from c in dbgravacao.Tcliente
                                  where c.Cnpj_Cpf == prepedido.DadosCliente.Cnpj_Cpf
                                  select c.Midia).FirstOrDefaultAsync();

            Torcamento torcamento = new Torcamento();
            //Varificar se tem tem os dados no prepedido => loja, apelido, vendedor e permiteRaStatus(se não tiver parceiro = false)
            torcamento.Orcamento = prepedido.NumeroPrePedido;
            torcamento.Loja = prepedido.DadosCliente.Loja;
            torcamento.Data = DateTime.Now.Date;
            torcamento.Hora = Util.HoraParaBanco(DateTime.Now);
            torcamento.Id_Cliente = prepedido.DadosCliente.Id;
            torcamento.Orcamentista = prepedido.DadosCliente.Indicador_Orcamentista;
            torcamento.Midia = midia == null ? midia = "" : midia;
            torcamento.Comissao_Loja_Indicou = 0;
            torcamento.Servicos = "";
            torcamento.Venda_Externa = 0;//Obs:NÃO ACHEI ESSE CAMPOS SENDO SALVO NOS ARQUIVOS DE ORCAMENTO
            torcamento.Vendedor = prepedido.DadosCliente.Vendedor;
            torcamento.St_Orcamento = "";
            torcamento.St_Fechamento = "";
            torcamento.St_Orc_Virou_Pedido = 0;
            torcamento.CustoFinancFornecQtdeParcelas = (short)ObterCustoFinancFornecQtdeParcelasDeFormaPagto(prepedido.FormaPagtoCriacao);
            torcamento.Vl_Total = Calcular_Vl_Total(prepedido);
            torcamento.Vl_Total_NF = CalcularVl_Total_NF(prepedido);
            torcamento.Vl_Total_RA = prepedido.PermiteRAStatus == 1 ? CalcularVl_Total_NF(prepedido) - Calcular_Vl_Total(prepedido) : 0M;
            torcamento.Perc_RT = prepedido.PercRT;
            torcamento.Perc_Desagio_RA_Liquida = perc_limite_RA_sem_desagio;
            torcamento.Permite_RA_Status = prepedido.PermiteRAStatus;
            torcamento.St_End_Entrega = prepedido.EnderecoEntrega.OutroEndereco == true ? (short)1 : (short)0;
            torcamento.CustoFinancFornecTipoParcelamento = siglaPagto;//sigla pagto
            torcamento.Sistema_responsavel_cadastro = (int)sistemaResponsavelCadastro;
            torcamento.Sistema_responsavel_atualizacao = (int)sistemaResponsavelCadastro;
            torcamento.Perc_max_comissao_padrao = prepedido.DadosCliente.Perc_max_comissao_padrao;
            torcamento.Perc_max_comissao_e_desconto_padrao = prepedido.DadosCliente.Perc_max_comissao_e_desconto_padrao;
            torcamento.IdOrcamentoCotacao = prepedido.DadosCliente.IdOrcamentoCotacao;
            torcamento.IdIndicadorVendedor = prepedido.DadosCliente.IdIndicadorVendedor;
            torcamento.UsuarioCadastroIdTipoUsuarioContexto = prepedido.UsuarioCadastroIdTipoUsuarioContexto;//Id do contexto do usuário logado de acordo com a tabela t_CFG_TIPO_USUARIO_CONTEXTO.Id Se aprovação pelo cliente em rota pública preencher com 4
            torcamento.UsuarioCadastroId = prepedido.UsuarioCadastroId;//Id do registro do usuário logado (t_ORCAMENTISTA_E_INDICADOR_VENDEDOR.Id, t_ORCAMENTISTA_E_INDICADOR.Id ou t_USUARIO.Id) Se aprovação pelo cliente em rota pública deve ser nulo
            torcamento.Usuario_cadastro = prepedido.Usuario_cadastro;//preencher com "[N] 999999", onde  N = UsuarioCadastroIdTipoUsuarioContexto  999999 = UsuarioCadastroId
            

            //inclui os campos de endereço cadastral no Torccamento
            IncluirDadosClienteParaTorcamento(prepedido, torcamento);

            //vamos criar método que inseri os dados de forma de pagto para o Torcamento
            IncluirFormaPagtoParaTorcamento(prepedido, torcamento);

            //vamos incluir os campos de detalhesPrepedido para Torcamento
            IncluirDetalhesPrepedidoParaTorcamento(prepedido, torcamento, prepedido.DadosCliente.UsuarioCadastro.ToUpper());

            if (prepedido.EnderecoEntrega == null)
            {
                prepedido.EnderecoEntrega = new EnderecoEntregaClienteCadastroDados();
            }
            //vamos incluir os campos de endereço de entrega
            IncluirEnderecoEntregaParaTorcamento(prepedido, torcamento);

            //vamos alterar o modo de criar o log e montar apenas os campos que devem ser salvos
            string campos_a_inserir = montarLogPrepedidoBll.MontarCamposAInserirPrepedido(torcamento, prepedido);

            string log = "";
            log = Util.MontaLogInserir(torcamento, log, campos_a_inserir, true);

            dbgravacao.Add(torcamento);
            await dbgravacao.SaveChangesAsync();

            return log;
        }

        private void IncluirDadosClienteParaTorcamento(PrePedidoDados prePedido, Torcamento torcamento)
        {
            //aqui vamos passar os 
            if (torcamento != null)
            {
                torcamento.St_memorizacao_completa_enderecos = 1;
                torcamento.Endereco_logradouro = prePedido.DadosCliente.Endereco;
                torcamento.Endereco_bairro = prePedido.DadosCliente.Bairro;
                torcamento.Endereco_cidade = prePedido.DadosCliente.Cidade;
                torcamento.Endereco_uf = prePedido.DadosCliente.Uf;
                torcamento.Endereco_cep = prePedido.DadosCliente.Cep;
                torcamento.Endereco_numero = prePedido.DadosCliente.Numero;
                torcamento.Endereco_complemento = !string.IsNullOrEmpty(prePedido.DadosCliente.Complemento) ?
                    prePedido.DadosCliente.Complemento : "";
                torcamento.Endereco_email = !string.IsNullOrEmpty(prePedido.DadosCliente.Email) ?
                    prePedido.DadosCliente.Email : "";
                torcamento.Endereco_email_xml = !string.IsNullOrEmpty(prePedido.DadosCliente.EmailXml) ?
                    prePedido.DadosCliente.EmailXml : "";
                torcamento.Endereco_nome = prePedido.DadosCliente.Nome;
                torcamento.Endereco_ddd_res = !string.IsNullOrEmpty(prePedido.DadosCliente.DddResidencial) ?
                    prePedido.DadosCliente.DddResidencial : "";
                torcamento.Endereco_tel_res = !string.IsNullOrEmpty(prePedido.DadosCliente.TelefoneResidencial) ?
                    prePedido.DadosCliente.TelefoneResidencial : "";
                torcamento.Endereco_ddd_com = !string.IsNullOrEmpty(prePedido.DadosCliente.DddComercial) ?
                    prePedido.DadosCliente.DddComercial : "";
                torcamento.Endereco_tel_com = !string.IsNullOrEmpty(prePedido.DadosCliente.TelComercial) ?
                    prePedido.DadosCliente.TelComercial : "";
                torcamento.Endereco_ramal_com = !string.IsNullOrEmpty(prePedido.DadosCliente.Ramal) ?
                    prePedido.DadosCliente.Ramal : "";
                torcamento.Endereco_ddd_cel = !string.IsNullOrEmpty(prePedido.DadosCliente.DddCelular) ?
                    prePedido.DadosCliente.DddCelular : "";
                torcamento.Endereco_tel_cel = !string.IsNullOrEmpty(prePedido.DadosCliente.Celular) ?
                    prePedido.DadosCliente.Celular : "";
                torcamento.Endereco_ddd_com_2 = !string.IsNullOrEmpty(prePedido.DadosCliente.DddComercial2) ?
                    prePedido.DadosCliente.DddComercial2 : "";
                torcamento.Endereco_tel_com_2 = !string.IsNullOrEmpty(prePedido.DadosCliente.TelComercial2) ?
                    prePedido.DadosCliente.TelComercial2 : "";
                torcamento.Endereco_ramal_com_2 = !string.IsNullOrEmpty(prePedido.DadosCliente.Ramal2) ?
                    prePedido.DadosCliente.Ramal2 : "";
                torcamento.Endereco_tipo_pessoa = prePedido.DadosCliente.Tipo;
                torcamento.Endereco_cnpj_cpf = prePedido.DadosCliente.Cnpj_Cpf;
                torcamento.Endereco_contribuinte_icms_status = prePedido.DadosCliente.Contribuinte_Icms_Status;
                torcamento.Endereco_produtor_rural_status = prePedido.DadosCliente.ProdutorRural;
                torcamento.Endereco_ie = !string.IsNullOrEmpty(prePedido.DadosCliente.Ie) ?
                    prePedido.DadosCliente.Ie : "";
                torcamento.Endereco_rg = !string.IsNullOrEmpty(prePedido.DadosCliente.Rg) ?
                    prePedido.DadosCliente.Rg : "";
                torcamento.Endereco_contato = !string.IsNullOrEmpty(prePedido.DadosCliente.Contato) ?
                    prePedido.DadosCliente.Contato : "";
            }
        }

        private void IncluirFormaPagtoParaTorcamento(PrePedidoDados prepedido, Torcamento torcamento)
        {
            if (torcamento != null)
            {
                torcamento.Qtde_Parcelas = ObterQtdeParcelasDeFormaPagto(prepedido.FormaPagtoCriacao);
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
                    //if (prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "5" &&
                    //    prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "7")
                    torcamento.Pce_Prestacao_Periodo = (short)prepedido.FormaPagtoCriacao.C_pce_prestacao_periodo;
                    torcamento.Qtde_Parcelas = ObterQtdeParcelasDeFormaPagto(prepedido.FormaPagtoCriacao);
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
                    torcamento.Qtde_Parcelas = ObterQtdeParcelasDeFormaPagto(prepedido.FormaPagtoCriacao);
                }

                //Verificando campos NULL para compatibilidade, pois os campos aceitam NULL mas, não é salvo dessa forma
                torcamento.Pu_Valor = torcamento.Pu_Valor.HasValue ? torcamento.Pu_Valor : 0.0M;
                torcamento.Pc_Valor_Parcela = torcamento.Pc_Valor_Parcela.HasValue ? torcamento.Pc_Valor_Parcela : 0.0M;
                torcamento.Pce_Entrada_Valor = torcamento.Pce_Entrada_Valor.HasValue ? torcamento.Pce_Entrada_Valor : 0.0M;
                torcamento.Pce_Prestacao_Valor = torcamento.Pce_Prestacao_Valor.HasValue ? torcamento.Pce_Prestacao_Valor : 0.0M;
                torcamento.Pse_Prim_Prest_Valor = torcamento.Pse_Prim_Prest_Valor.HasValue ? torcamento.Pse_Prim_Prest_Valor : 0.0M;
            }
        }

        private void IncluirDetalhesPrepedidoParaTorcamento(PrePedidoDados prepedido, Torcamento torcamento, string orcamentista)
        {

            torcamento.Obs_1 = prepedido.DetalhesPrepedido.Observacoes == null ?
                "" : prepedido.DetalhesPrepedido.Observacoes;
            torcamento.Obs_2 = prepedido.DetalhesPrepedido.NumeroNF == null ?
                "" : prepedido.DetalhesPrepedido.NumeroNF;
            torcamento.StBemUsoConsumo = prepedido.DetalhesPrepedido.BemDeUso_Consumo !=
                (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO ?
                (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_SIM : (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO;

            torcamento.InstaladorInstalaStatus = prepedido.DetalhesPrepedido.InstaladorInstala ==
                (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_SIM ?
                (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_SIM :
                (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO;

            torcamento.InstaladorInstalaIdTipoUsuarioContexto = prepedido.DetalhesPrepedido.InstaladorInstalaIdTipoUsuarioContexto;//mesmo campo da T_ORCAMENTO_COTACAO ou o prepedido tb pode passar esse valor
            torcamento.InstaladorInstalaIdUsuarioUltAtualiz = prepedido.DetalhesPrepedido.InstaladorInstalaIdUsuarioUltAtualiz;//mesmo campo da T_ORCAMENTO_COTACAO
            torcamento.InstaladorInstalaUsuarioUltAtualiz = prepedido.DetalhesPrepedido.InstaladorInstalaUsuarioUltAtualiz;//preencher com '[N] 999999' onde  N = InstaladorInstalaIdTipoUsuarioContexto  999999 = InstaladorInstalaIdUsuarioUltAtualiz Se tipo usuário = 4 preencher com 'Cliente'
            torcamento.InstaladorInstalaDtHrUltAtualiz = prepedido.DetalhesPrepedido.InstaladorInstalaDtHrUltAtualiz;//mesmo campo da T_ORCAMENTO_COTACAO

            torcamento.PrevisaoEntregaData = prepedido.DetalhesPrepedido.PrevisaoEntrega;
            torcamento.PrevisaoEntregaUsuarioUltAtualiz = prepedido.DetalhesPrepedido.PrevisaoEntregaUsuarioUltAtualiz;
            torcamento.PrevisaoEntregaDtHrUltAtualiz = prepedido.DetalhesPrepedido.PrevisaoEntregaDtHrUltAtualiz;
            torcamento.PrevisaoEntregaIdTipoUsuarioContexto = prepedido.DetalhesPrepedido.PrevisaoEntregaIdTipoUsuarioContexto;
            torcamento.PrevisaoEntregaIdUsuarioUltAtualiz = prepedido.DetalhesPrepedido.PrevisaoEntregaIdUsuarioUltAtualiz;
            
            torcamento.St_Etg_Imediata = short.Parse(prepedido.DetalhesPrepedido.EntregaImediata) == (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO ?
                   (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO : (short)Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM;
            torcamento.Etg_Imediata_Usuario = prepedido.EnderecoEntrega.Etg_Imediata_Usuario;
            torcamento.EtgImediataIdTipoUsuarioContexto = prepedido.EnderecoEntrega.EtgImediataIdTipoUsuarioContexto;
            torcamento.EtgImediataIdUsuarioUltAtualiz = prepedido.EnderecoEntrega.EtgImediataIdUsuarioUltAtualiz;
            torcamento.Etg_Imediata_Data = prepedido.EnderecoEntrega.Etg_imediata_data;

            torcamento.GarantiaIndicadorStatus = prepedido.DetalhesPrepedido.GarantiaIndicador;// == null ? byte.Parse(Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO) : byte.Parse(Constantes.COD_GARANTIA_INDICADOR_STATUS__SIM);
            torcamento.GarantiaIndicadorIdTipoUsuarioContexto = prepedido.DetalhesPrepedido.GarantiaIndicadorIdTipoUsuarioContexto;
            torcamento.GarantiaIndicadorIdUsuarioUltAtualiz = prepedido.DetalhesPrepedido.GarantiaIndicadorIdUsuarioUltAtualiz;
            torcamento.GarantiaIndicadorUsuarioUltAtualiz = prepedido.DetalhesPrepedido.GarantiaIndicadorUsuarioUltAtualiz;
            torcamento.GarantiaIndicadorDtHrUltAtualiz = prepedido.DetalhesPrepedido.GarantiaIndicadorDtHrUltAtualiz;
        }

        private void IncluirEnderecoEntregaParaTorcamento(PrePedidoDados prepedido, Torcamento torcamento)
        {
            if (torcamento != null)
            {
                if (prepedido.EnderecoEntrega.EndEtg_cod_justificativa != null)
                {
                    torcamento.EndEtg_Endereco = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_endereco) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_endereco;
                    torcamento.EndEtg_Endereco_Numero = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_endereco_numero) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_endereco_numero;
                    torcamento.EndEtg_Endereco_Complemento = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_endereco_complemento) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_endereco_complemento;
                    torcamento.EndEtg_Bairro = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_bairro) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_bairro;
                    torcamento.EndEtg_Cidade = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_cidade) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_cidade;
                    torcamento.EndEtg_UF = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_uf) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_uf;
                    torcamento.EndEtg_CEP = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_cep) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_cep.Replace("-", "");
                    torcamento.EndEtg_Cod_Justificativa = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_cod_justificativa) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_cod_justificativa;
                    torcamento.EndEtg_email = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_email) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_email;
                    torcamento.EndEtg_email_xml = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_email_xml) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_email_xml;
                    torcamento.EndEtg_nome = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_nome) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_nome;
                    torcamento.EndEtg_ddd_res = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_ddd_res) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_ddd_res;
                    torcamento.EndEtg_tel_res = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_tel_res) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_tel_res;
                    torcamento.EndEtg_ddd_com = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_ddd_com) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_ddd_com;
                    torcamento.EndEtg_tel_com = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_tel_com) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_tel_com;
                    torcamento.EndEtg_ramal_com = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_ramal_com) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_ramal_com;
                    torcamento.EndEtg_ddd_cel = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_ddd_cel) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_ddd_cel;
                    torcamento.EndEtg_tel_cel = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_tel_cel) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_tel_cel;
                    torcamento.EndEtg_ddd_com_2 = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_ddd_com_2) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_ddd_com_2;
                    torcamento.EndEtg_tel_com_2 = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_tel_com_2) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_tel_com_2;
                    torcamento.EndEtg_ramal_com_2 = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_ramal_com_2) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_ramal_com_2;
                    torcamento.EndEtg_tipo_pessoa = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_tipo_pessoa) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_tipo_pessoa;
                    torcamento.EndEtg_cnpj_cpf = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_cnpj_cpf) ?
                        "" : Util.SoDigitosCpf_Cnpj(prepedido.EnderecoEntrega.EndEtg_cnpj_cpf);
                    torcamento.EndEtg_contribuinte_icms_status = prepedido.EnderecoEntrega.EndEtg_contribuinte_icms_status;
                    torcamento.EndEtg_produtor_rural_status = prepedido.EnderecoEntrega.EndEtg_produtor_rural_status;
                    torcamento.EndEtg_ie = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_ie) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_ie;
                    torcamento.EndEtg_rg = string.IsNullOrEmpty(prepedido.EnderecoEntrega.EndEtg_rg) ?
                        "" : prepedido.EnderecoEntrega.EndEtg_rg;
                    
                }
            }
        }

        private async Task<string> CadastrarOrctoItens(ContextoBdGravacao dbgravacao,
            List<TorcamentoItem> lstOrcItens, string log)
        {
            foreach (var i in lstOrcItens)
            {
                dbgravacao.Add(i);
            }
            await dbgravacao.SaveChangesAsync();

            log = montarLogPrepedidoBll.MontarCamposAInserirItensPrepedido(lstOrcItens, log);
            return log;
        }

        public static int ObterCustoFinancFornecQtdeParcelasDeFormaPagto(FormaPagtoCriacaoDados formaPagto)
        {
            int custoFinancFornecQtdeParcelas = 0;

            if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
                custoFinancFornecQtdeParcelas = 0;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                custoFinancFornecQtdeParcelas = 1;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                custoFinancFornecQtdeParcelas = formaPagto.C_pc_qtde ?? -1;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                custoFinancFornecQtdeParcelas = formaPagto.C_pc_maquineta_qtde ?? -1;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                custoFinancFornecQtdeParcelas = formaPagto.C_pce_prestacao_qtde ?? -1;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                custoFinancFornecQtdeParcelas = (formaPagto.C_pse_demais_prest_qtde ?? -1) + 1;//conforme linha 199 pág OrcamentoNovoConfirma.asp

            return custoFinancFornecQtdeParcelas;
        }

        public static short? ObterQtdeParcelasDeFormaPagto(FormaPagtoCriacaoDados formaPagto)
        {
            short? qtdeParcelas = 0;

            if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
                qtdeParcelas = 1;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                qtdeParcelas = 1;
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                qtdeParcelas = (short)(formaPagto.C_pc_qtde ?? -1);
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                qtdeParcelas = (short)(formaPagto.C_pc_maquineta_qtde ?? -1);
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                qtdeParcelas = (short)((formaPagto.C_pce_prestacao_qtde ?? -1) + 1);
            else if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                qtdeParcelas = (short)((formaPagto.C_pse_demais_prest_qtde ?? -1) + 1);//conforme linha 199 pág OrcamentoNovoConfirma.asp

            return qtdeParcelas;
        }

        private decimal Calcular_Vl_Total(PrePedidoDados prepedido)
        {
            decimal vl_total = 0M;

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.Produto))
                {
                    vl_total += (decimal)(p.Qtde * p.Preco_Venda);
                }
            }

            return Math.Round(vl_total, 2);
        }

        private decimal CalcularVl_Total_NF(PrePedidoDados prepedido)
        {
            decimal vl_total_NF = 0M;

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.Produto))
                {
                    vl_total_NF += (decimal)(p.Qtde * p.Preco_NF);
                }
            }

            return Math.Round(vl_total_NF, 2);
        }

        private async Task ExisteProdutoDescontinuado(PrePedidoDados prepedido, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.Produto) && !string.IsNullOrEmpty(p.Fabricante))
                {
                    var produtoTask = (from c in db.Tproduto
                                       where c.Produto == p.Produto && c.Fabricante == p.Fabricante
                                       select c.Descontinuado).FirstOrDefaultAsync();
                    var produto = await produtoTask;

                    if (produto != null && produto.ToUpper() == "S")
                    {
                        if (p.Qtde > p.Qtde_estoque_total_disponivel)
                            lstErros.Add("Produto (" + p.Fabricante + ")" + p.Produto +
                                " consta como 'descontinuado' e não há mais saldo suficiente no estoque para atender à quantidade solicitada.");
                    }
                }
            }
        }

        private List<int> ContagemEmpresasUsadasAutoSplit(List<RegrasBll> lstRegras, PrePedidoDados prepedido)
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
        private void VerificarQtdePedidosAutoSplit(List<RegrasBll> lstRegras, List<string> lstErros, PrePedidoDados prepedido)
        {
            int qtde_a_alocar = 0;

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.Produto))
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
                                        if (regra.Fabricante == p.Fabricante && regra.Produto == p.Produto)
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
                            if (regra.Produto == p.Produto && regra.Fabricante == p.Fabricante)
                            {
                                foreach (var re in regra.TwmsCdXUfXPessoaXCd)
                                {
                                    if (regra.Fabricante == p.Fabricante &&
                                       regra.Produto == p.Produto &&
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
                            p.Fabricante + ")" + p.Produto + " que não puderam ser alocados na lista de produtos sem presença no estoque de nenhum CD");
                    }
                }
            }
        }

        //se estoque for insuficiente, retorna true
        private bool VerificarEstoqueInsuficiente(List<RegrasBll> lstRegras, PrePedidoDados prepedido, Tparametro parametro)
        {
            bool retorno = false;
            int qtde_estoque_total_disponivel = 0;
            int qtde_estoque_total_disponivel_global = 0;

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.Produto) && !string.IsNullOrEmpty(p.Fabricante))
                {
                    foreach (var regra in lstRegras)
                    {
                        if (!string.IsNullOrEmpty(regra.Produto) && !string.IsNullOrEmpty(regra.Fabricante))
                        {
                            foreach (var r in regra.TwmsCdXUfXPessoaXCd)
                            {
                                if (r.Id_nfe_emitente > 0)
                                {
                                    if (r.St_inativo == 0)
                                    {
                                        if (regra.Fabricante == p.Fabricante && regra.Produto == p.Produto)
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
        private void ObterDisponibilidadeEstoque(List<RegrasBll> lstRegras, PrePedidoDados prepedido, Tparametro parametroRegra, List<string> lstErros)
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
                                    if (regra.Fabricante == p.Fabricante && regra.Produto == p.Produto)
                                    {
                                        re.Estoque_Fabricante = p.Fabricante;
                                        re.Estoque_Produto = p.Produto;
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

        public async Task<IEnumerable<TpercentualCustoFinanceiroFornecedor>> BuscarCoeficientePercentualCustoFinanFornec(PrePedidoDados prePedido, short qtdeParcelas, string siglaPagto, List<string> lstErros)
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
                        await (from c in db.TpercentualCustoFinanceiroFornecedor
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
                                i.Preco_Lista = (decimal)percCustoTask.Coeficiente * (decimal)i.CustoFinancFornecPrecoListaBase;
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

        public static async Task<List<RegrasBll>> ObtemCtrlEstoqueProdutoRegra(ContextoBdProvider contextoProvider,
            PrePedidoDados prePedido, List<string> lstErros)
        {
            /*
            #Para cada produto no pedido:
            #	SELECT id_wms_regra_cd FROM t_PRODUTO_X_WMS_REGRA_CD WHERE fabricante = FABRICANTE AND produto = PRODUTO
            #		se t_PRODUTO_X_WMS_REGRA_CD.id_wms_regra_cd = 0, erro
            #		se nenhum registro, erro
            #		se mais de um registro, erro (não corre pq possui chave única no banco)
            #	SELECT * FROM t_WMS_REGRA_CD WHERE id = id_wms_regra_cd
            #		se nenhum registro, erro
            #		se mais de um registro, erro (não está no ASP)
            #	SELECT * FROM t_WMS_REGRA_CD_X_UF WHERE (id_wms_regra_cd = id_wms_regra_cd) AND (uf = UF)
            #		se nenhum registro, erro
            #		se mais de um registro, erro (não está no ASP)
            #	SELECT * FROM t_WMS_REGRA_CD_X_UF_X_PESSOA WHERE (id_wms_regra_cd_x_uf = t_WMS_REGRA_CD_X_UF.id) AND (tipo_pessoa = tipo_pessoa)
            #		se nenhum registro, erro
            #		se mais de um registro, erro (não está no ASP)
            #		se t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente = 0, erro
            #	SELECT * FROM t_NFe_EMITENTE WHERE (id = t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente)
            #		se nenhum registro, tudo bem --- confirmar com Hamilton
            #		se t_NFe_EMITENTE.st_ativo <> 1, erro
            #	SELECT * FROM t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD WHERE (id_wms_regra_cd_x_uf_x_pessoa = t_WMS_REGRA_CD_X_UF_X_PESSOA.id) ORDER BY ordem_prioridade
            #		se nenhum registro, erro
            #		para cada registro:
            #			SELECT * FROM t_NFe_EMITENTE WHERE (id = t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.id_nfe_emitente)
            #				se nenhum registro, tudo bem --- confirmar com Hamilton
            #				se t_NFe_EMITENTE.st_ativo <> 1 então considerar t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.st_inativo = 1
            #			Localizar t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.id_nfe_emitente = t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente
            #				se t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.st_inativo = 1, erro
            #				se não localizar, tudo bem
            */

            List<RegrasBll> lstRegrasCrtlEstoque = new List<RegrasBll>();

            var dbreal = contextoProvider.GetContextoLeitura();

            //monta um cache de dados para não repetir s acessos
            var produtosDistintos = prePedido.ListaProdutos.Select(r => r.Produto).Distinct();
            var dbTprodutoXwmsRegraCds = await (from c in dbreal.TprodutoXwmsRegraCd
                                                where produtosDistintos.Contains(c.Produto)
                                                select new { c.Produto, c.Fabricante, c.Id_wms_regra_cd }).ToListAsync();
            var regraId_wms_regra_cdDistinct = dbTprodutoXwmsRegraCds.Select(r => r.Id_wms_regra_cd).Distinct();
            var dbTwmsRegraCds = await (from c in dbreal.TwmsRegraCd
                                        where regraId_wms_regra_cdDistinct.Contains(c.Id)
                                        select new { c.Id, c.Apelido, c.Descricao, c.St_inativo }).ToListAsync();
            var dbTwmsRegraCdXUfs = await (from c in dbreal.TwmsRegraCdXUf
                                           where regraId_wms_regra_cdDistinct.Contains(c.Id_wms_regra_cd) &&
                                                 c.Uf == prePedido.DadosCliente.Uf
                                           select new { c.Id, c.Id_wms_regra_cd, c.Uf, c.St_inativo }).ToListAsync();

            //esta tabela é pequena
            var dbTnfEmitentes = await (from c in dbreal.TnfEmitente select new { c.Id, c.St_Ativo }).ToListAsync(); ;

            //para melhorar a velocidade, poderíamos terminar de montar o cache

            //buscar a sigla tipo pessoa
            var tipo_pessoa = UtilsProduto.MultiCdRegraDeterminaPessoa(prePedido.DadosCliente.Tipo,
                (Constantes.ContribuinteICMS)prePedido.DadosCliente.Contribuinte_Icms_Status,
                (Constantes.ProdutorRural)prePedido.DadosCliente.ProdutorRural);
            if (string.IsNullOrEmpty(tipo_pessoa))
            {
                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf +
                    "': não foi possível determinar o tipo de pessoa (tipo_cliente=" +
                    prePedido.DadosCliente.Tipo + ", contribuinte_icms_status=" + prePedido.DadosCliente.Contribuinte_Icms_Status +
                    ", produtor_rural_status=" + prePedido.DadosCliente.ProdutorRural + ")");
                return lstRegrasCrtlEstoque;
            }

            foreach (var item in prePedido.ListaProdutos)
            {
                var regraProdutoTask = from c in dbTprodutoXwmsRegraCds
                                       where c.Fabricante == item.Fabricante &&
                                             c.Produto == item.Produto
                                       select c;

                var regraLista = regraProdutoTask.ToList();

                if (regraLista.Count() != 1)
                {
                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                        Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': produto (" + item.Fabricante + ")" +
                        item.Produto + " não possui regra associada");
                }
                else
                {
                    var regra = regraLista.First();
                    if (regra.Id_wms_regra_cd == 0)
                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                            Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': produto (" + item.Fabricante + ")" +
                            item.Produto + " não está associado a nenhuma regra");
                    else
                    {
                        var wmsRegraCdTask = from c in dbTwmsRegraCds
                                             where c.Id == regra.Id_wms_regra_cd
                                             select c;

                        var wmsRegraCdLista = wmsRegraCdTask.ToList();
                        if (wmsRegraCdLista.Count() != 1)
                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                item.Produto + " não foi localizada no banco de dados (Id=" + regra.Id_wms_regra_cd + ")");
                        else
                        {
                            var wmsRegraCd = wmsRegraCdLista.First();
                            RegrasBll itemRegra = new RegrasBll();
                            itemRegra.Fabricante = item.Fabricante;
                            itemRegra.Produto = item.Produto;
                            itemRegra.St_Regra_ok = true;

                            itemRegra.TwmsRegraCd = new Produto.RegrasCrtlEstoque.t_WMS_REGRA_CD
                            {
                                Id = wmsRegraCd.Id,
                                Apelido = wmsRegraCd.Apelido,
                                Descricao = wmsRegraCd.Descricao,
                                St_inativo = wmsRegraCd.St_inativo
                            };

                            var wmsRegraCdXUfTask = from c in dbTwmsRegraCdXUfs
                                                    where c.Id_wms_regra_cd == regra.Id_wms_regra_cd &&
                                                          c.Uf == prePedido.DadosCliente.Uf
                                                    select c;
                            var wmsRegraCdXUfLista = wmsRegraCdXUfTask.ToList();

                            if (wmsRegraCdXUfLista.Count() != 1)
                            {
                                itemRegra.St_Regra_ok = false;
                                lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                    Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                    item.Produto + " não está cadastrada para a UF '" + prePedido.DadosCliente.Uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
                            }
                            else
                            {
                                var wmsRegraCdXUf = wmsRegraCdXUfLista.First();
                                itemRegra.TwmsRegraCdXUf = new Produto.RegrasCrtlEstoque.t_WMS_REGRA_CD_X_UF
                                {
                                    Id = wmsRegraCdXUf.Id,
                                    Id_wms_regra_cd = wmsRegraCdXUf.Id_wms_regra_cd,
                                    Uf = wmsRegraCdXUf.Uf,
                                    St_inativo = wmsRegraCdXUf.St_inativo
                                };

                                var wmsRegraCdXUfXPessoaTask = from c in dbreal.TwmsRegraCdXUfPessoa
                                                               where c.Id_wms_regra_cd_x_uf == itemRegra.TwmsRegraCdXUf.Id &&
                                                                     c.Tipo_pessoa == tipo_pessoa
                                                               select c;

                                var wmsRegraCdXUfXPessoaLista = await wmsRegraCdXUfXPessoaTask.ToListAsync();

                                if (wmsRegraCdXUfXPessoaLista.Count() != 1)
                                {
                                    itemRegra.St_Regra_ok = false;
                                    lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                        Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                        item.Produto + " não está cadastrada para a UF '" + prePedido.DadosCliente.Uf + "' (Id=" + regra.Id_wms_regra_cd + ")");
                                }
                                else
                                {
                                    var wmsRegraCdXUfXPessoa = wmsRegraCdXUfXPessoaLista.First();
                                    itemRegra.TwmsRegraCdXUfXPessoa = new Produto.RegrasCrtlEstoque.t_WMS_REGRA_CD_X_UF_X_PESSOA
                                    {
                                        Id = wmsRegraCdXUfXPessoa.Id,
                                        Id_wms_regra_cd_x_uf = wmsRegraCdXUfXPessoa.Id_wms_regra_cd_x_uf,
                                        Tipo_pessoa = wmsRegraCdXUfXPessoa.Tipo_pessoa,
                                        St_inativo = wmsRegraCdXUfXPessoa.St_inativo,
                                        Spe_id_nfe_emitente = wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                    };

                                    if (wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente == 0)
                                    {
                                        itemRegra.St_Regra_ok = false;
                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                            Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                            item.Produto + " não especifica nenhum CD para aguardar produtos sem presença no estoque (Id=" + regra.Id_wms_regra_cd + ")");
                                    }
                                    else
                                    {
                                        var nfEmitenteTask = from c in dbTnfEmitentes
                                                             where c.Id == wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente
                                                             select c;
                                        var nfEmitente = nfEmitenteTask.FirstOrDefault();

                                        if (nfEmitente != null)
                                        {
                                            if (nfEmitente.St_Ativo != 1)
                                            {
                                                itemRegra.St_Regra_ok = false;
                                                lstErros.Add("Falha na regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                                    Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante +
                                                    ")" + item.Produto + " especifica um CD para aguardar produtos sem presença no estoque que não está habilitado " +
                                                    "(Id=" + regra.Id_wms_regra_cd + ")");
                                            }
                                        }
                                        var wmsRegraCdXUfXPessoaXcdTask = from c in dbreal.TwmsRegraCdXUfXPessoaXCd
                                                                          where c.Id_wms_regra_cd_x_uf_x_pessoa == wmsRegraCdXUfXPessoa.Id
                                                                          orderby c.Ordem_prioridade
                                                                          select c;
                                        var wmsRegraCdXUfXPessoaXcd = await wmsRegraCdXUfXPessoaXcdTask.ToListAsync();

                                        if (wmsRegraCdXUfXPessoaXcd.Count == 0)
                                        {
                                            itemRegra.St_Regra_ok = false;
                                            lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                                Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" + item.Fabricante + ")" +
                                                item.Produto + " não especifica nenhum CD para consumo do estoque (Id=" + regra.Id_wms_regra_cd + ")");
                                        }
                                        else
                                        {
                                            itemRegra.TwmsCdXUfXPessoaXCd = new List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD>();
                                            foreach (var i in wmsRegraCdXUfXPessoaXcd)
                                            {
                                                Produto.RegrasCrtlEstoque.t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD item_cd_uf_pess_cd = new Produto.RegrasCrtlEstoque.t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
                                                {
                                                    Id = i.Id,
                                                    Id_wms_regra_cd_x_uf_x_pessoa = i.Id_wms_regra_cd_x_uf_x_pessoa,
                                                    Id_nfe_emitente = i.Id_nfe_emitente,
                                                    Ordem_prioridade = i.Ordem_prioridade,
                                                    St_inativo = i.St_inativo
                                                };

                                                var nfeCadastroPrincipalTask = from c in dbTnfEmitentes
                                                                               where c.Id == item_cd_uf_pess_cd.Id_nfe_emitente
                                                                               select c;

                                                var nfeCadastroPrincipal = nfeCadastroPrincipalTask.FirstOrDefault();

                                                if (nfeCadastroPrincipal != null)
                                                {
                                                    if (nfeCadastroPrincipal.St_Ativo != 1)
                                                        item_cd_uf_pess_cd.St_inativo = 1;
                                                }

                                                itemRegra.TwmsCdXUfXPessoaXCd.Add(item_cd_uf_pess_cd);
                                            }

                                            //'VERIFICA SE O CD SELECIONADO PARA ALOCAR OS PRODUTOS PENDENTES ESTÁ HABILITADO NA LISTA DOS CD'S P/ PRODUTOS DISPONÍVEIS
                                            //'OBSERVAÇÃO: ESTA CHECAGEM É FEITA POR UMA QUESTÃO DE COERÊNCIA, POIS SE OCORRER A SITUAÇÃO EM QUE UM CD SELECIONADO P/
                                            //'PRODUTOS PENDENTES NÃO ESTEJA HABILITADO P/ OS PRODUTOS DISPONÍVEIS, O SISTEMA IRIA PROCESSAR DA SEGUINTE FORMA:
                                            //'	1) AO ANALISAR A DISPONIBILIDADE DOS PRODUTOS NOS CD'S, SE O CD 'X' ESTIVER DESATIVADO ELE SERÁ IGNORADO.
                                            //'	2) SE AO FINAL DA ALOCAÇÃO DOS PRODUTOS DISPONÍVEIS RESTAR UMA QUANTIDADE PENDENTE P/ SER ALOCADA COMO
                                            //'		PRODUTO SEM PRESENÇA NO ESTOQUE E, CASO O CD SELECIONADO P/ TAL SEJA O CD 'X', O SISTEMA IRÁ ALOCAR A
                                            //'		QUANTIDADE REMANESCENTE P/ O CD 'X', SENDO QUE SE ESSE CD POSSUIR DISPONIBILIDADE NO ESTOQUE, OS PRODUTOS SERÃO
                                            //'		CONSUMIDOS NORMALMENTE AO INVÉS DE FICAREM COMO PENDENTES.

                                            foreach (var i in itemRegra.TwmsCdXUfXPessoaXCd)
                                            {

                                                if (i.Id_nfe_emitente == itemRegra.TwmsRegraCdXUfXPessoa.Spe_id_nfe_emitente)
                                                {
                                                    if (i.St_inativo == 1)
                                                    {
                                                        itemRegra.St_Regra_ok = false;
                                                        lstErros.Add("Falha na leitura da regra de consumo do estoque para a UF '" + prePedido.DadosCliente.Uf + "' e '" +
                                                            Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo) + "': regra associada ao produto (" +
                                                            item.Fabricante + ")" + item.Produto + " especifica o CD '" + Util.ObterApelidoEmpresaNfeEmitentes(wmsRegraCdXUfXPessoa.Spe_id_nfe_emitente, contextoProvider) +
                                                            "' para alocação de produtos sem presença no estoque, sendo que este CD está desativado para " +
                                                            "processar produtos disponíveis (Id=" + regra.Id_wms_regra_cd + ")");
                                                    }
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

        private async Task ComplementarInfosOrcamentoItem(ContextoBdGravacao dbgravacao,
            List<TorcamentoItem> lstOrcamentoItem, string loja)
        {
            int indiceItem = 0;

            TorcamentoItem orcItem = new TorcamentoItem();

            foreach (TorcamentoItem item in lstOrcamentoItem)
            {
                var prodLista = from c in dbgravacao.TprodutoLoja.Include(x => x.Tproduto).Include(x => x.Tproduto.Tfabricante)
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
                    item.Qtde_Spe = 0; //essa quantidade não esta sendo alterada
                    item.Abaixo_Min_Status = 0; //sempre recebe 0 aqui
                    item.Abaixo_Min_Autorizacao = "";//sempre esta vazio no modulo orcamento
                    item.Abaixo_Min_Autorizador = "";//sempre esta vazio no modulo orcamento
                    item.Abaixo_Min_Superv_Autorizador = "";//sempre esta vazio no modulo orcamento
                    item.Sequencia = (short?)RenumeraComBase1(indiceItem);
                    item.Subgrupo = !string.IsNullOrEmpty(prod.Tproduto.Subgrupo) ? prod.Tproduto.Subgrupo : "";

                    indiceItem++;
                }
            }
        }

        /*
         * param o indice_item, 1, e o indice do item
         */
        private int RenumeraComBase1(int indiceItem)
        {
            int retorno;
            retorno = indiceItem + 1;

            return retorno;
        }

        private async Task<IEnumerable<TorcamentoItem>> MontaListaOrcamentoItem(PrePedidoDados prepedido,
            List<TpercentualCustoFinanceiroFornecedor> lstPercentualCustoFinanFornec, ContextoBdGravacao dbgravacao,
            Constantes.CodSistemaResponsavel sistemaResponsavelCadastro)
        {
            List<TorcamentoItem> lstOrcamentoItem = new List<TorcamentoItem>();

            if (prepedido.FormaPagtoCriacao.Rb_forma_pagto != Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                lstOrcamentoItem = MontaListaOrcamentoItemComCoeficiente(prepedido, lstPercentualCustoFinanFornec, sistemaResponsavelCadastro);
            }
            else
            {
                lstOrcamentoItem = MontaListaOrcamentoItemSemCoeficiente(prepedido);
            }

            //incluir a montagem dos outros campos
            await ComplementarInfosOrcamentoItem(dbgravacao, lstOrcamentoItem, prepedido.DadosCliente.Loja);

            return lstOrcamentoItem;
        }

        private List<TorcamentoItem> MontaListaOrcamentoItemSemCoeficiente(PrePedidoDados prepedido)
        {
            List<TorcamentoItem> lstOrcamentoItem = new List<TorcamentoItem>();

            foreach (PrepedidoProdutoPrepedidoDados p in prepedido.ListaProdutos)
            {
                TorcamentoItem item = new TorcamentoItem
                {
                    Orcamento = prepedido.NumeroPrePedido,
                    Produto = p.Produto,
                    Fabricante = UtilsGlobais.Util.Normaliza_Codigo(p.Fabricante, Constantes.TAM_MIN_FABRICANTE),
                    Qtde = p.Qtde,
                    Preco_Venda = Math.Round(p.Preco_Venda, 2),
                    Preco_NF = prepedido.PermiteRAStatus == 1 ? Math.Round((decimal)p.Preco_NF, 2) : Math.Round(p.Preco_Venda, 2),
                    Obs = p.Obs == null ? "" : p.Obs,
                    Desc_Dado = p.Desc_Dado,
                    Preco_Lista = Math.Round(p.Preco_Lista, 2),
                    CustoFinancFornecCoeficiente = 1,
                    StatusDescontoSuperior = p.StatusDescontoSuperior,
                    IdUsuarioDescontoSuperior = p.IdUsuarioDescontoSuperior,
                    DataHoraDescontoSuperior = p.DataHoraDescontoSuperior
                };
                lstOrcamentoItem.Add(item);
            }

            return lstOrcamentoItem;
        }

        private List<TorcamentoItem> MontaListaOrcamentoItemComCoeficiente(PrePedidoDados prepedido,
            List<TpercentualCustoFinanceiroFornecedor> lstPercentualCustoFinanFornec, Constantes.CodSistemaResponsavel sistemaResponsavelCadastro)
        {
            List<TorcamentoItem> lstOrcamentoItem = new List<TorcamentoItem>();
            TpercentualCustoFinanceiroFornecedor coeficiente = null;
            foreach (PrepedidoProdutoPrepedidoDados p in prepedido.ListaProdutos)
            {
                if (sistemaResponsavelCadastro != Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ORCAMENTO_COTACAO)
                {
                    coeficiente = lstPercentualCustoFinanFornec.Where(x => x.Fabricante == p.Fabricante).FirstOrDefault();
                }
                TorcamentoItem item = new TorcamentoItem
                {
                    Orcamento = prepedido.NumeroPrePedido,
                    Produto = p.Produto,
                    Fabricante = UtilsGlobais.Util.Normaliza_Codigo(p.Fabricante, Constantes.TAM_MIN_FABRICANTE),
                    Qtde = p.Qtde,
                    Preco_Venda = Math.Round(p.Preco_Venda, 2),
                    Preco_NF = prepedido.PermiteRAStatus == 1 ? Math.Round((decimal)p.Preco_NF, 2) : Math.Round(p.Preco_Venda, 2),
                    Obs = p.Obs == null ? "" : p.Obs,
                    Desc_Dado = p.Desc_Dado,
                    Preco_Lista = Math.Round(p.Preco_Lista, 2),
                    CustoFinancFornecCoeficiente = coeficiente != null ? coeficiente.Coeficiente : p.CustoFinancFornecCoeficiente,
                    StatusDescontoSuperior = p.StatusDescontoSuperior,
                    IdUsuarioDescontoSuperior = p.IdUsuarioDescontoSuperior,
                    DataHoraDescontoSuperior = p.DataHoraDescontoSuperior
                };
                lstOrcamentoItem.Add(item);
            }

            return lstOrcamentoItem;
        }

        public async Task<bool> ValidarOrcamentistaIndicador(string apelido)
        {
            bool retorno = false;

            var db = contextoProvider.GetContextoLeitura();

            var orcamentistaTask = await (from c in db.TorcamentistaEindicador
                                          where c.Apelido == apelido
                                          select c.Apelido).FirstOrDefaultAsync();
            if (orcamentistaTask == apelido)
                return retorno = true;

            return retorno;
        }

        public static string DecodificaCustoFinanFornecQtdeParcelas(string tipoParcelamento, short custoFFQtdeParcelas)
        {
            string retorno = "";

            if (tipoParcelamento == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
                retorno = "0+" + custoFFQtdeParcelas;
            else if (tipoParcelamento == Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA)
                retorno = "1+" + custoFFQtdeParcelas;

            return retorno;
        }

        public async Task<TorcamentistaEindicador> BuscarTorcamentista(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var tOrcamentistaTask = from c in db.TorcamentistaEindicador
                                    where c.Apelido == apelido
                                    select c;
            TorcamentistaEindicador tOrcamentista = await tOrcamentistaTask.FirstOrDefaultAsync();

            return tOrcamentista;
        }

        public async Task<bool> TorcamentistaExiste(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();
            return await ((from c in db.TorcamentistaEindicador
                           where c.Apelido == apelido
                           select c).AnyAsync());
        }

        public async Task GerarNumeroOrcamento(ContextoBdGravacao dbgravacao, PrePedidoDados prepedido)
        {
            string sufixoIdOrcamento = Constantes.SUFIXO_ID_ORCAMENTO;

            var nsuTask = await Nsu.GerarNsu(dbgravacao, Constantes.NSU_ORCAMENTO);
            string nsu = nsuTask.ToString();

            int ndescarte = nsu.Length - Constantes.TAM_MIN_NUM_ORCAMENTO;
            string sdescarte = nsu.Substring(0, ndescarte);

            if (sdescarte.Length == ndescarte)
            {
                prepedido.NumeroPrePedido = nsu.Substring(ndescarte) + sufixoIdOrcamento;
            }
        }

        public async Task<BuscarStatusPrepedidoRetornoDados> BuscarStatusPrepedido(string orcamento)
        {
            BuscarStatusPrepedidoRetornoDados ret = new BuscarStatusPrepedidoRetornoDados();

            //vamos buscar o prepedido para verificar se ele virou pedido
            var db = contextoProvider.GetContextoLeitura();

            var orcamentotask = await (from c in db.Torcamento
                                       where c.Orcamento == orcamento
                                       select c).FirstOrDefaultAsync();

            if (orcamentotask != null)
            {
                ret.St_orc_virou_pedido = Convert.ToBoolean(orcamentotask.St_Orc_Virou_Pedido);

                //virou
                if ((bool)ret.St_orc_virou_pedido)
                {
                    Tpedido pedido_pai = (from c in db.Tpedido
                                          where c.Orcamento == orcamentotask.Orcamento
                                          select c).FirstOrDefault();

                    List<Tpedido> lstPedido = await (from c in db.Tpedido
                                                     where c.Pedido.Contains(pedido_pai.Pedido)
                                                     select c).ToListAsync();

                    if (lstPedido.Count > 0)
                    {
                        ret.Pedidos = new List<StatusPedidoPrepedidoDados>();
                        lstPedido.ForEach(x =>
                        {
                            ret.Pedidos.Add(new StatusPedidoPrepedidoDados
                            {
                                Pedido = x.Pedido,
                                St_Entrega = x.St_Entrega
                            });
                        });
                    }
                }
            }

            return ret;
        }

        public async Task<List<InformacoesStatusPrepedidoRetornoDados>> ListarStatusPrepedido(DateTime? dataLimiteInferior, List<string> lstPrepedido,
            int sistema_responsavel)
        {
            var db = contextoProvider.GetContextoLeitura();

            List<InformacoesStatusPrepedidoRetornoDados> lstInfosStatusPrepedido = new List<InformacoesStatusPrepedidoRetornoDados>();

            var lstTorcamentoQuery = from c in db.Torcamento
                                     where c.Sistema_responsavel_cadastro == sistema_responsavel
                                     select c;
            if (lstPrepedido != null && lstPrepedido.Count > 0)
            {
                lstTorcamentoQuery = from c in lstTorcamentoQuery
                                     where lstPrepedido.Contains(c.Orcamento)
                                     select c;
            }

            if (dataLimiteInferior != null)
            {
                lstTorcamentoQuery = from c in lstTorcamentoQuery
                                     where c.Data >= dataLimiteInferior.Value
                                     select c;
            }

            var lstTorcamento = await lstTorcamentoQuery.ToListAsync();

            //vamos buscar todos que viraram pedido
            //aqui ja estamos buscando todos que contém o orçamento
            List<string> lstViraramPedido = (from d in lstTorcamento
                                             where d.St_Orc_Virou_Pedido == 1
                                             select d.Orcamento).ToList();

            List<Tpedido> lstPedidosPai = await (from c in db.Tpedido
                                                 where lstViraramPedido.Contains(c.Orcamento)
                                                 select c).ToListAsync();


            lstInfosStatusPrepedido = MontarInformacoesStatusPrepedidoRetornoDados(lstTorcamento, lstPedidosPai);
            return lstInfosStatusPrepedido;
        }


        private List<InformacoesStatusPrepedidoRetornoDados> MontarInformacoesStatusPrepedidoRetornoDados(List<Torcamento> lstTorcamento,
            List<Tpedido> lstPedidos)
        {
            List<InformacoesStatusPrepedidoRetornoDados> lstRetPedido = new List<InformacoesStatusPrepedidoRetornoDados>();
            foreach (var orcamento in lstTorcamento)
            {
#pragma warning disable IDE0017 // Simplify object initialization
                InformacoesStatusPrepedidoRetornoDados info = new InformacoesStatusPrepedidoRetornoDados();
#pragma warning restore IDE0017 // Simplify object initialization
                info.Orcamento = orcamento.Orcamento;
                info.Data = orcamento.Data;
                info.St_orcamento = orcamento.St_Orcamento;
                info.St_virou_pedido = Convert.ToBoolean(orcamento.St_Orc_Virou_Pedido);

                if (orcamento.St_Orc_Virou_Pedido == 1)
                {
                    info.LstInformacoesPedido = new List<InformacoesPedidoRetornoDados>();

                    Tpedido pedidoPai = lstPedidos
                        .Where(x => x.Orcamento == orcamento.Orcamento)
                        .Select(c => c).FirstOrDefault();

                    if (pedidoPai != null)
                        info.LstInformacoesPedido.Add(new InformacoesPedidoRetornoDados
                        {
                            Pedido = pedidoPai.Pedido,
                            St_entrega = pedidoPai.St_Entrega,
                            DescricaoStatusEntrega = pedidoVisualizacaoBll.FormataSatusPedido(pedidoPai.St_Entrega),
                            Entregue_data = pedidoPai.Entregue_Data,
                            Cancelado_data = pedidoPai.Cancelado_Data,
                            PedidoRecebidoStatus = pedidoPai.PedidoRecebidoStatus,
                            PedidoRecebidoData = pedidoPai.PedidoRecebidoData,
                            Analise_credito = pedidoPai.Analise_Credito,
                            DescricaoAnaliseCredito = pedidoVisualizacaoBll.DescricaoAnaliseCreditoCadastroPedido(Convert.ToString(pedidoPai.Analise_Credito), false, pedidoPai.Pedido, pedidoPai.Orcamentista),
                            Analise_credito_data = pedidoPai.Analise_credito_Data,
                            St_pagto = pedidoPai.St_Pagto,
                            DescricaoStatusPagto = pedidoVisualizacaoBll.StatusPagto(pedidoPai.St_Pagto)
                        });

                    //vamos buscar os filhotes desse pai
                    List<Tpedido> lstfilhotes = lstPedidos
                        .Where(x => x.Pedido.Contains(pedidoPai.Pedido + "-"))
                        .Select(c => c).ToList();

                    if (lstfilhotes != null)
                    {
                        if (lstfilhotes.Count > 0)
                        {
                            foreach (var filho in lstfilhotes)
                            {
                                info.LstInformacoesPedido.Add(new InformacoesPedidoRetornoDados
                                {
                                    Pedido = filho.Pedido,
                                    St_entrega = filho.St_Entrega,
                                    DescricaoStatusEntrega = pedidoVisualizacaoBll.FormataSatusPedido(filho.St_Entrega),
                                    Entregue_data = filho.Entregue_Data,
                                    Cancelado_data = filho.Cancelado_Data,
                                    PedidoRecebidoStatus = filho.PedidoRecebidoStatus,
                                    PedidoRecebidoData = filho.PedidoRecebidoData,
                                    Analise_credito = pedidoPai.Analise_Credito,
                                    DescricaoAnaliseCredito = pedidoVisualizacaoBll.DescricaoAnaliseCreditoCadastroPedido(Convert.ToString(pedidoPai.Analise_Credito), false, filho.Pedido, filho.Orcamentista),
                                    Analise_credito_data = pedidoPai.Analise_credito_Data,
                                    St_pagto = pedidoPai.St_Pagto,
                                    DescricaoStatusPagto = pedidoVisualizacaoBll.StatusPagto(pedidoPai.St_Pagto)
                                });
                            }
                        }
                    }
                }

                lstRetPedido.Add(info);
            }

            return lstRetPedido;
        }

    }
}
