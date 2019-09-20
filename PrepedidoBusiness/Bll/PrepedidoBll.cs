using PrepedidoBusiness;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using InfraBanco.Modelos;
using PrepedidoBusiness.Utils;
using PrepedidoBusiness.Dtos.Prepedido;
using PrepedidoBusiness.Dtos.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Dtos.ClienteCadastro;
using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Bll.Regras;
using System.Transactions;

namespace PrepedidoBusiness.Bll
{
    public class PrepedidoBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public PrepedidoBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<string>> ListarNumerosPrepedidosCombo(string orcamentista)
        {
            //toda vez precisamos de uma nova conexao para os casos em que houver transacao
            var db = contextoProvider.GetContextoLeitura();
            var lista = from r in db.Torcamentos
                        where r.Orcamentista == orcamentista &&
                              r.St_Orcamento != "CAN"
                        orderby r.Orcamento
                        select r.Orcamento;
            var res = lista.AsEnumerable();
            return await Task.FromResult(res);
        }

        public async Task<IEnumerable<string>> ListarCpfCnpjPrepedidosCombo(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var lista = (from c in db.Torcamentos.Include(r => r.Tcliente)
                         where c.Orcamentista == apelido &&
                               c.St_Orcamento != "CAN"
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
            Todos = 0, NaoViraramPedido = 1, SomenteViraramPedido = 2
        }
        public async Task<IEnumerable<PrepedidosCadastradosDtoPrepedido>> ListarPrePedidos(string apelido, TipoBuscaPrepedido tipoBusca,
            string clienteBusca, string numeroPrePedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            //apelido = "MARISARJ";
            //usamos a mesma lógica de PedidoBll.ListarPedidos:
            /*
             * se fizeram a busca por algum CPF ou CNPJ ou pedido e não achamos nada, removemos o filtro de datas
             * para não aparcer para o usuário que não tem nenhum registro
             * */
            var ret = await ListarPrePedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPrePedido, dataInicial, dataFinal);
            //se tiver algum registro, retorna imediatamente
            if (ret.Count() > 0)
                return ret;

            if (String.IsNullOrEmpty(clienteBusca) && String.IsNullOrEmpty(numeroPrePedido))
                return ret;

            //busca sem datas
            ret = await ListarPrePedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPrePedido, null, null);
            if (ret.Count() > 0)
                return ret;

            //ainda não achamos nada? então faz a busca sem filtrar por tipo
            ret = await ListarPrePedidosFiltroEstrito(apelido, TipoBuscaPrepedido.Todos, clienteBusca, numeroPrePedido, null, null);
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
            }
            if (!string.IsNullOrEmpty(clienteBusca))
                lst = lst.Where(r => r.Tcliente.Cnpj_Cpf.Contains(clienteBusca));
            if (!string.IsNullOrEmpty(numeroPrePedido))
                lst = lst.Where(r => r.Orcamento.Contains(numeroPrePedido));
            if (dataInicial.HasValue)
                lst = lst.Where(r => r.Data >= dataInicial.Value);
            if (dataFinal.HasValue)
                lst = lst.Where(r => r.Data <= dataFinal.Value);

            //COLOCAR O STATUS DO PEDIDO PARA PREPEDIDOS QUE VIRARAM PEDIDOS
            var lstfinal = lst.Select(r => new PrepedidosCadastradosDtoPrepedido
            {
                Status = r.St_Orc_Virou_Pedido == 1 ? "Pré-Pedido - Com Pedido" : "Pré-Pedido - Sem Pedido",
                DataPrePedido = r.Data_Hora,
                NumeroPrepedido = r.Orcamento,
                NomeCliente = r.Tcliente.Nome,
                ValoTotal = r.Vl_Total
            }).OrderByDescending(r => r.DataPrePedido);

            var res = lstfinal.AsEnumerable();
            return await Task.FromResult(res);
        }

        public async Task<bool> RemoverPrePedido(string numeroPrePedido, string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            Torcamento prePedido = db.Torcamentos.
                Where(
                        r => r.Orcamentista == apelido &&
                        r.Orcamento == numeroPrePedido &&
                        (r.St_Orcamento == "" || r.St_Orcamento == null) &&
                        r.St_Orc_Virou_Pedido == 0
                      ).SingleOrDefault();

            if (!string.IsNullOrEmpty(prePedido.ToString()))
            {
                prePedido.St_Orcamento = "CAN";
                prePedido.Cancelado_Data = DateTime.Now;
                prePedido.Cancelado_Usuario = apelido;
                await db.SaveChangesAsync();
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<PrePedidoDto> BuscarPrePedido(string apelido, string numPrePedido)
        {
            //parateste
            //apelido = "MARISARJ";
            //numPrePedido = "214289Z";

            var db = contextoProvider.GetContextoLeitura();

            var prepedido = from c in db.Torcamentos
                            where c.Orcamentista == apelido && c.Orcamento == numPrePedido
                            select c;


            Torcamento pp = prepedido.FirstOrDefault();

            if (pp == null)
                return null;

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

            var cadastroClienteTask = ObterDadosCliente(pp.Loja, pp.Orcamentista, pp.Vendedor, pp.Id_Cliente);
            var enderecoEntregaTask = ObterEnderecoEntrega(pp);
            var lstProdutoTask = await ObterProdutos(pp);

            var vltotalRa = lstProdutoTask.Select(r => r.VlTotalRA).Sum();
            var totalDestePedidoComRa = lstProdutoTask.Select(r => r.TotalItemRA).Sum();
            var totalDestePedido = lstProdutoTask.Select(r => r.TotalItem).Sum();

            PrePedidoDto prepedidoDto = new PrePedidoDto
            {
                NumeroPrePedido = pp.Orcamento,
                DataHoraPedido = Convert.ToString(pp.Data + pp.Hora),
                DadosCliente = await cadastroClienteTask,
                EnderecoEntrega = await enderecoEntregaTask,
                ListaProdutos = lstProdutoTask.ToList(),
                TotalFamiliaParcelaRA = vltotalRa,
                PermiteRAStatus = pp.Permite_RA_Status,
                CorTotalFamiliaRA = vltotalRa > 0 ? "green" : "red",
                PercRT = pp.Perc_RT,
                ValorTotalDestePedidoComRA = totalDestePedidoComRa,
                VlTotalDestePedido = totalDestePedido,
                DetalhesPrepedido = ObterDetalhesPrePedido(pp),
                FormaPagto = ObterFormaPagto(pp).ToList()
            };

            return await Task.FromResult(prepedidoDto);
        }

        private DetalhesDtoPrepedido ObterDetalhesPrePedido(Torcamento torcamento)
        {
            DetalhesDtoPrepedido detail = new DetalhesDtoPrepedido
            {
                Observacoes = torcamento.Obs_1,
                NumeroNF = torcamento.Obs_2,
                EntregaImediata = Convert.ToString(torcamento.St_Etg_Imediata) == Constantes.COD_ETG_IMEDIATA_NAO ?
                "NÃO" : "SIM " + torcamento.Etg_Imediata_Usuario +
                " em " + torcamento.Etg_Imediata_Data?.ToString("dd/MM/yyyy"),
                BemDeUso_Consumo = Convert.ToString(torcamento.StBemUsoConsumo) == Constantes.COD_ST_BEM_USO_CONSUMO_NAO ?
                "NÃO" : "SIM",
                InstaladorInstala = Convert.ToString(torcamento.InstaladorInstalaStatus) == Constantes.COD_INSTALADOR_INSTALA_NAO ?
                "NÃO" : "SIM",
                GarantiaIndicador = Convert.ToString(torcamento.GarantiaIndicadorStatus) ==
                Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO ?
                "NÃO" : "SIM"
            };

            return detail;
        }

        private IEnumerable<string> ObterFormaPagto(Torcamento torcamento)
        {
            List<string> lista = new List<string>();

            switch (Convert.ToString(torcamento.Tipo_Parcelamento))
            {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    lista.Add("À vista (" + Util.OpcaoFormaPagto(Convert.ToString(torcamento.Av_Forma_Pagto)) + ")");
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    lista.Add(String.Format("Parcela Única: " + Constantes.SIMBOLO_MONETARIO + " {0:c2} (" +
                        Util.OpcaoFormaPagto(Convert.ToString(torcamento.Pu_Forma_Pagto)) + ") vencendo após " + torcamento.Pu_Vencto_Apos, torcamento.Pu_Valor));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    lista.Add(String.Format("Parcelado no Cartão (internet) em " + torcamento.Pc_Qtde_Parcelas + " X " +
                        Constantes.SIMBOLO_MONETARIO + " {0:c2}", torcamento.Pc_Valor_Parcela));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    lista.Add(String.Format("Parcelado no Cartão (maquineta) em " + torcamento.Pc_Maquineta_Qtde_Parcelas + " X " +
                        Constantes.SIMBOLO_MONETARIO + " {0:c2}", torcamento.Pc_Maquineta_Valor_Parcela));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    lista.Add(String.Format("Entrada " + Constantes.SIMBOLO_MONETARIO + "{0:c2} (" +
                        Util.OpcaoFormaPagto(Convert.ToString(torcamento.Pce_Forma_Pagto_Entrada)) + ")", torcamento.Pce_Entrada_Valor));
                    lista.Add(String.Format("Prestações: " + torcamento.Pce_Prestacao_Qtde + " X " + Constantes.SIMBOLO_MONETARIO + " {0:c2}" +
                        " (" + Util.OpcaoFormaPagto(Convert.ToString(torcamento.Pce_Forma_Pagto_Prestacao)) + ") vencendo a cada " +
                        torcamento.Pce_Prestacao_Periodo + " dias", torcamento.Pce_Prestacao_Valor));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    lista.Add(String.Format("1ª Prestação: " + Constantes.SIMBOLO_MONETARIO + " {0:c2} (" +
                        Util.OpcaoFormaPagto(Convert.ToString(torcamento.Pse_Forma_Pagto_Prim_Prest)) + ") vencendo após " + torcamento.Pse_Prim_Prest_Apos +
                        " dias", torcamento.Pse_Prim_Prest_Valor));
                    lista.Add(String.Format("Demais Prestações: " + torcamento.Pse_Demais_Prest_Qtde + " X " + Constantes.SIMBOLO_MONETARIO + " {0:c2}" +
                        " (" + Util.OpcaoFormaPagto(Convert.ToString(torcamento.Pse_Forma_Pagto_Demais_Prest)) + ") vencendo a cada " +
                        torcamento.Pse_Demais_Prest_Periodo + " dias", torcamento.Pse_Demais_Prest_Valor));
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
                    Preco = p.Preco_NF,
                    VlLista = (decimal)p.Preco_Lista,
                    Desconto = p.Desc_Dado,
                    VlUnitario = p.Preco_Venda,
                    VlTotalRA = (decimal)(p.Qtde * (p.Preco_NF - p.Preco_Venda)),
                    Comissao = orc.Perc_RT,
                    TotalItemRA = p.Qtde * p.Preco_NF,
                    TotalItem = p.Qtde * p.Preco_Venda
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
                Rg = Util.FormatCpf_Cnpj_Ie(cli.Rg),
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
                TelComercial2 = cli.Tel_Com_2,
                DddComercial2 = cli.Ddd_Com_2,
                Ramal2 = cli.Ramal_Com_2,
                Email = cli.Email,
                EmailXml = cli.Email_Xml,
                Endereco = cli.Endereco,
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

            if (p.St_End_Entrega == 1)
            {
                enderecoEntrega.EndEtg_endereco = p.EndEtg_Endereco;
                enderecoEntrega.EndEtg_endereco_numero = p.EndEtg_Endereco_Numero;
                enderecoEntrega.EndEtg_endereco_complemento = p.EndEtg_Endereco_Complemento;
                enderecoEntrega.EndEtg_bairro = p.EndEtg_Bairro;
                enderecoEntrega.EndEtg_cidade = p.EndEtg_Cidade;
                enderecoEntrega.EndEtg_uf = p.EndEtg_UF;
                enderecoEntrega.EndEtg_cep = p.EndEtg_CEP;
                enderecoEntrega.EndEtg_descricao_justificativa = await ObterDescricao_Cod(Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA, p.EndEtg_Cod_Justificativa);
            }
            else
                return null;

            return enderecoEntrega;
        }

        //Esse metodo esta em Util.cs
        //afazer: alterar a chamada desse método para Util.cs
        private async Task<string> ObterDescricao_Cod(string grupo, string cod)
        {
            var db = contextoProvider.GetContextoLeitura();

            var desc = from c in db.TcodigoDescricaos
                       where c.Grupo == grupo && c.Codigo == cod
                       select c.Descricao;

            string result = await desc.FirstOrDefaultAsync();

            if (result == null || result == "")
                return "Código não cadastrado (" + cod + ")";

            return result;
        }

        public async Task<short> Obter_Permite_RA_Status(string apelido)
        {
            //paraTeste
            //apelido = "PEDREIRA";

            var db = contextoProvider.GetContextoLeitura();

            var raStatus = (from c in db.TorcamentistaEindicadors
                            where c.Apelido == apelido
                            select c.Permite_RA_Status).FirstOrDefaultAsync();


            return await raStatus;
        }

        public async Task<IEnumerable<string>> CadastrarPrepedido(PrePedidoDto prePedido, string apelido)
        {
            apelido = "MARISARJ";

            List<string> lstErros = new List<string>();

            TorcamentistaEindicador tOrcamentista = await BuscarTorcamentista(apelido);

            if (string.IsNullOrEmpty(tOrcamentista.Vendedor))
                lstErros.Add("NÃO HÁ NENHUM VENDEDOR DEFINIDO PARA ATENDÊ-LO");

            //validar o Orcamentista
            if (tOrcamentista.Apelido != apelido)
                lstErros.Add("Falha ao recuperar os dados cadastrais!!");

            if (Util.LojaHabilitadaProdutosECommerce(prePedido.DadosCliente.Loja))
            {
                //monta os produtos
                //var lstProd =  MontaProdutos(prePedido.ListaProdutos, lstErros, prePedido.DadosCliente.Loja);
                //descrição do tipo de parcelamento
                //afazer: necessário analisar qual o tipo de forma de pagamento que esta vindo 
                string desc_tipo_parcelamento = DescricaoCustoFornecTipoParcelamento(prePedido.FormaPagtoCriacao.Rb_forma_pagto);

                //Validar endereço de entraga
                if (ValidarEndecoEntrega(prePedido.EnderecoEntrega, lstErros))
                {
                    if (Util.ValidarTipoCustoFinanceiroFornecedor(lstErros, prePedido.FormaPagtoCriacao.Rb_forma_pagto))
                    {
                        //obter a quantidade de parcelas
                        //Esta sendo verificado qual o tipo de pagamento que esta sendo feito e retornando a quantidade de parcelas
                        int c_custoFinancFornecQtdeParcelas = ObterQtdeParcelasFormaPagto(prePedido);
                        //Analizar o funcionamento
                        float coeficiente = await BuscarCoeficientePercentualCustoFinanFornec(prePedido, (short)c_custoFinancFornecQtdeParcelas, lstErros);

                        Tparametro parametroRegra = await BuscarRegistroParametro(Constantes.ID_PARAMETRO_Flag_Orcamento_ConsisteDisponibilidadeEstoqueGlobal);
                        string tipoPessoa = Util.MultiCdRegraDeterminaPessoa(prePedido.DadosCliente.Tipo, prePedido.DadosCliente.Contribuinte_Icms_Status,
                            prePedido.DadosCliente.ProdutorRural);
                        string descricao = Util.DescricaoMultiCDRegraTipoPessoa(prePedido.DadosCliente.Tipo);

                        List<RegrasBll> regraCrtlEstoque = (await ObterCtrlEstoqueProdutoRegra(prePedido, lstErros)).ToList();
                        ProdutoBll.VerificarRegrasAssociadasAosProdutos(regraCrtlEstoque, lstErros, prePedido.DadosCliente);
                        ObterDisponibilidadeEstoque(regraCrtlEstoque, prePedido, parametroRegra, lstErros);
                        VerificarEstoqueInsuficiente(regraCrtlEstoque, prePedido, parametroRegra);
                        //REALIZA A ANÁLISE DA QUANTIDADE DE PEDIDOS NECESSÁRIA(AUTO-SPLIT)
                        VerificarQtdePedidosAutoSplit(regraCrtlEstoque, lstErros, prePedido);
                        //CONTAGEM DE EMPRESAS QUE SERÃO USADAS NO AUTO-SPLIT, OU SEJA, A QUANTIDADE DE PEDIDOS QUE SERÁ CADASTRADA, 
                        //JÁ QUE CADA PEDIDO SE REFERE AO ESTOQUE DE UMA EMPRESA
                        List<int> lst_empresa_selecionada = ContagemEmpresasUsadasAutoSplit(regraCrtlEstoque, prePedido);//verificar quando é usado essa lista
                        //HÁ ALGUM PRODUTO DESCONTINUADO?
                        ExisteProdutoDescontinuado(prePedido, lstErros);

                        //afazer:Validar todos os campos que estão sendo validados
                        //validar detalhesPrepedidos

                        if (lstErros.Count <= 0)
                        {
                            using (TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                            {
                                var db = contextoProvider.GetContextoGravacao();
                                //gerar o numero de orçamento
                                GerarNumeroOrcamento(prePedido);
                                if (string.IsNullOrEmpty(prePedido.NumeroPrePedido))
                                    lstErros.Add("FALHA NA OPERAÇÃO COM O BANCO DE DADOS AO TENTAR GERAR NSU.");
                                //Cadastrar dados do Orcamento e endereço de entrega 
                                string log = await EfetivarCadastroPrepedido(prePedido, tOrcamentista);
                                //Cadastrar orcamento itens
                                List<TorcamentoItem> lstOrcamentoItem = MontaListaOrcamentoItem(prePedido.ListaProdutos, prePedido.NumeroPrePedido);
                                (await ComplementarInfosOrcamentoItem(lstOrcamentoItem, prePedido.DadosCliente.Loja)).ToList();
                                log = await CadastrarOrctoItens(lstOrcamentoItem, log);


                            }
                        }
                    }
                }
            }
            return lstErros;
        }

        #region ExisteMensagensAlertaProdutos
        //ESSE METODO SERÁ USADO NO COMBO DE PRODUTOS PARA SELECIONAR
        //CASO EXISTA ALERTA, ELA SERÁ USADA
        //private async void ExisteMensagensAlertaProdutos(PrePedidoDto prepedido, List<string> lstErros)
        //{
        //    var db = contextoProvider.GetContextoLeitura();

        //    foreach(var p in prepedido.ListaProdutos)
        //    {
        //        var produtoTask = from c in db.TprodutoXAlertas.Include(r => r.TalertaProduto).Include(r => r.Tproduto)
        //                          where c.Fabricante == p.Fabricante &&
        //                                c.Produto == p.NumProduto &&
        //                                c.TalertaProduto.Ativo == "S"
        //                          orderby c.Dt_Cadastro, c.Id_Alerta
        //                          select c;
        //        var produto = await produtoTask.FirstOrDefaultAsync();

        //        if(!string.IsNullOrEmpty(produto.TalertaProduto.Mensagem))
        //        {
        //            lstErros.Add()
        //        }
        //    }
        //}
        #endregion

        private async Task<string> EfetivarCadastroPrepedido(PrePedidoDto prepedido, TorcamentistaEindicador orcamentista)
        {
            Torcamento torcamento = new Torcamento();

            torcamento.Orcamento = prepedido.NumeroPrePedido;
            torcamento.Loja = orcamentista.Loja;
            torcamento.Data = DateTime.Now;
            torcamento.Hora = DateTime.Now.Hour.ToString().Replace(":", "");
            torcamento.Id_Cliente = prepedido.DadosCliente.Id;
            torcamento.Midia = "";
            torcamento.Servicos = "";
            //Vl_Servicos verificar como esse campo é inserido
            torcamento.Vendedor = orcamentista.Vendedor;
            torcamento.Obs_1 = prepedido.DetalhesPrepedido.Observacoes;
            torcamento.Obs_2 = prepedido.DetalhesPrepedido.NumeroNF;
            torcamento.Qtde_Parcelas = (short?)ObterQtdeParcelasFormaPagto(prepedido);
            torcamento.Forma_Pagamento = prepedido.DetalhesPrepedido.DescricaoFormaPagamento;

            if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                torcamento.Av_Forma_Pagto = short.Parse(prepedido.FormaPagtoCriacao.Op_av_forma_pagto);
                torcamento.Qtde_Parcelas = 1;
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                torcamento.Pu_Forma_Pagto = short.Parse(prepedido.FormaPagtoCriacao.Op_av_forma_pagto);
                torcamento.Pu_Valor = prepedido.FormaPagtoCriacao.C_pu_valor;
                torcamento.Pu_Vencto_Apos = (short)prepedido.FormaPagtoCriacao.C_pu_vencto_apos;
                torcamento.Qtde_Parcelas = 1;
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
            {
                torcamento.Pc_Qtde_Parcelas = (short)prepedido.FormaPagtoCriacao.C_pc_qtde;
                torcamento.Pc_Valor_Parcela = prepedido.FormaPagtoCriacao.C_pc_valor;
                torcamento.Qtde_Parcelas = (short)prepedido.FormaPagtoCriacao.C_pc_qtde;
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
            {
                torcamento.Pc_Maquineta_Valor_Parcela = (decimal)prepedido.FormaPagtoCriacao.C_pc_maquineta_valor;
                torcamento.Pc_Maquineta_Qtde_Parcelas = (short)prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde;
                torcamento.Qtde_Parcelas = (short)prepedido.FormaPagtoCriacao.C_pc_maquineta_qtde;
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                torcamento.Pce_Forma_Pagto_Entrada = short.Parse(prepedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto);
                torcamento.Pce_Forma_Pagto_Prestacao = short.Parse(prepedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto);
                torcamento.Pce_Entrada_Valor = prepedido.FormaPagtoCriacao.C_pce_entrada_valor;
                torcamento.Pce_Prestacao_Qtde = (short)prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde;
                torcamento.Pce_Prestacao_Valor = prepedido.FormaPagtoCriacao.C_pce_prestacao_valor;
                torcamento.Pce_Prestacao_Periodo = (short)prepedido.FormaPagtoCriacao.C_pce_prestacao_periodo;
                torcamento.Qtde_Parcelas = (short?)(prepedido.FormaPagtoCriacao.C_pce_prestacao_qtde + 1);

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
                torcamento.Qtde_Parcelas = (short)(prepedido.FormaPagtoCriacao.C_pse_demais_prest_qtde + 1);
            }
            torcamento.St_Orcamento = "";
            torcamento.St_Fechamento = "";
            torcamento.CustoFinancFornecParcelamento = prepedido.FormaPagtoCriacao.Rb_forma_pagto;//verificar o valor de entrada; esperado apenas 2 caracteres
            torcamento.CustoFinancFornecQtdeParcelas = (short)ObterQtdeParcelasFormaPagto(prepedido);
            torcamento.Vl_Total = Calcular_Vl_Total(prepedido);
            torcamento.Vl_Total_NF = CalcularVl_Total_NF(prepedido);
            torcamento.Vl_Total_RA = CalcularVl_Total_NF(prepedido) - Calcular_Vl_Total(prepedido);
            torcamento.Perc_RT = 0;//não é passado valor para esse campo rever
            torcamento.InstaladorInstalaStatus = prepedido.DetalhesPrepedido.InstaladorInstala != "" ? short.Parse(Constantes.COD_INSTALADOR_INSTALA_SIM) : short.Parse(Constantes.COD_INSTALADOR_INSTALA_NAO);
            torcamento.InstaladorInstalaUsuarioUltAtualiz = orcamentista.Apelido;
            torcamento.InstaladorInstalaDtHrUltAtualiz = DateTime.Now;
            torcamento.Perc_Desagio_RA_Liquida = Constantes.PERC_DESAGIO_RA_LIQUIDA;
            torcamento.Permite_RA_Status = orcamentista.Permite_RA_Status;
            torcamento.St_End_Entrega = 1;
            torcamento.EndEtg_Endereco = prepedido.EnderecoEntrega.EndEtg_endereco;
            torcamento.EndEtg_Endereco_Numero = prepedido.EnderecoEntrega.EndEtg_endereco_numero;
            torcamento.EndEtg_Endereco_Complemento = prepedido.EnderecoEntrega.EndEtg_endereco_complemento;
            torcamento.EndEtg_Bairro = prepedido.EnderecoEntrega.EndEtg_bairro;
            torcamento.EndEtg_Cidade = prepedido.EnderecoEntrega.EndEtg_cidade;
            torcamento.EndEtg_UF = prepedido.EnderecoEntrega.EndEtg_uf;
            torcamento.EndEtg_CEP = prepedido.EnderecoEntrega.EndEtg_cep;
            torcamento.EndEtg_Cod_Justificativa = prepedido.EnderecoEntrega.EndEtg_descricao_justificativa;

            //Montar uma rotina para pegar os campos a omitir antes de montar o log
            string campos_a_omitir = MontarCamposAOmitirFormaPagtoCriacao(prepedido.FormaPagtoCriacao);

            var db = contextoProvider.GetContextoGravacao();
            string log = "";
            log = Util.MontaLog(torcamento, log, campos_a_omitir);

            db.Add(torcamento);
            await db.SaveChangesAsync();

            return log;
        }

        private async Task<string> CadastrarOrctoItens(List<TorcamentoItem> lstOrcItens, string log)
        {
            var db = contextoProvider.GetContextoGravacao();

            foreach (var i in lstOrcItens)
            {
                db.Add(i);
                log = Util.MontaLog(i, log, "");
            }
            await db.SaveChangesAsync();

            return log;
        }

        private string MontarCamposAOmitirFormaPagtoCriacao(FormaPagtoCriacaoDto forma_pagto_criacao)
        {
            string campos_a_omitir = "";

            if (forma_pagto_criacao.Rb_forma_pagto != Constantes.COD_FORMA_PAGTO_A_VISTA)
                campos_a_omitir = "av_forma_pagto|";
            else if (forma_pagto_criacao.Rb_forma_pagto != Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                if (!campos_a_omitir.Contains("av_forma_pagto"))
                    campos_a_omitir += "av_forma_pagto|";
                campos_a_omitir += "pu_forma_pagto|pu_valor|pu_vencto_apos|";
            }
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                campos_a_omitir += "pc_qtde_parcelas|pc_valor_parcela|";
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                campos_a_omitir += "pc_maquineta_qtde_parcelas|pc_maquineta_valor_parcela|";
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                campos_a_omitir += "pce_forma_pagto_entrada|pce_forma_pagto_prestacao|pce_entrada_valor|pce_prestacao_qtde|" +
                    "pce_prestacao_valor|pce_prestacao_periodo|";
            else if (forma_pagto_criacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                campos_a_omitir += "pse_forma_pagto_prim_prest|pse_forma_pagto_demais_prest|pse_prim_prest_valor|pse_prim_prest_apos|" +
                    "pse_demais_prest_qtde|pse_demais_prest_valor|pse_demais_prest_periodo";

            return campos_a_omitir;
        }

        private int ObterQtdeParcelasFormaPagto(PrePedidoDto prepedido)
        {
            FormaPagtoCriacaoDto formaPagto = prepedido.FormaPagtoCriacao;
            int qtdeParcelas = 0;

            if (formaPagto.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_A_VISTA)
                qtdeParcelas = 1;
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

        //No asp. do cliente isso, cada condição da forma de pagto retorna a variavel vlTotalFormaPagto que será usada em outro momento
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
                        Math.Abs((decimal)prepedido.VlTotalDestePedido - vlTotalFormaPagto) + ")!!");
            }
            else if (prepedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.Op_av_forma_pagto))
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
                else if (string.IsNullOrEmpty(prepedido.FormaPagtoCriacao.C_pce_prestacao_periodo.ToString()))
                    lstErros.Add("Indique o intervalo de vencimento entre as parcelas (parcelado com entrada).");
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

            return retorno;
        }

        //afazer:Verificar se realmente existe necessidade de refazer o calculo
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

            return vl_total;
        }

        private decimal CalcularVl_Total_NF(PrePedidoDto prepedido)
        {
            decimal vl_total_NF = 0M;

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    vl_total_NF += (decimal)(p.Qtde * p.Preco);
                }
            }

            return vl_total_NF;
        }

        private async void ExisteProdutoDescontinuado(PrePedidoDto prepedido, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();

            foreach (var p in prepedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    var produtoTask = (from c in db.Tprodutos
                                       where c.Produto == p.NumProduto
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
                        if (!string.IsNullOrEmpty(regra.Produto))
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
                            if (regra.Produto == p.NumProduto)
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
            //Verificar se necessita retornar algo
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
        private async void ObterDisponibilidadeEstoque(List<RegrasBll> lstRegras, PrePedidoDto prepedido, Tparametro parametroRegra, List<string> lstErros)
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

                        if (!await EstoqueVerificaDisponibilidadeIntegralV2(re))
                        {
                            lstErros.Add("Falha ao tentar consultar disponibilidade no estoque do produto(" +
                                regra.Fabricante + ")" + regra.Produto);
                        }
                    }
                }
            }
        }

        private bool ValidarEndecoEntrega(EnderecoEntregaDtoClienteCadastro endEtg, List<string> lstErros)
        {
            bool retorno = false;

            if (string.IsNullOrEmpty(endEtg.EndEtg_endereco))
                lstErros.Add("PREENCHA O ENDEREÇO DE ENTREGA.");
            if (endEtg.EndEtg_endereco.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
                lstErros.Add("ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " + endEtg.EndEtg_endereco.Length +
                    " CARACTERES<br>TAMANHO MÁXIMO: " + Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");
            if (string.IsNullOrEmpty(endEtg.EndEtg_endereco_numero))
                lstErros.Add("PREENCHA O NÚMERO DO ENDEREÇO DE ENTREGA.");
            if (string.IsNullOrEmpty(endEtg.EndEtg_bairro))
                lstErros.Add("PREENCHA O BAIRRO DO ENDEREÇO DE ENTREGA.");
            if (string.IsNullOrEmpty(endEtg.EndEtg_cidade))
                lstErros.Add("PREENCHA A CIDADE DO ENDEREÇO DE ENTREGA.");
            if (string.IsNullOrEmpty(endEtg.EndEtg_uf) || !Util.VerificaUf(endEtg.EndEtg_uf))
                lstErros.Add("UF INVÁLIDA NO ENDEREÇO DE ENTREGA.");
            if (!Util.VerificaCep(endEtg.EndEtg_cep))
                lstErros.Add("CEP INVÁLIDO NO ENDEREÇO DE ENTREGA.");

            return retorno;
        }

        private async Task<bool> EstoqueVerificaDisponibilidadeIntegralV2(t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD regra)
        {
            bool retorno = false;

            var db = contextoProvider.GetContextoLeitura();

            if (regra.Id_nfe_emitente != 0)
            {
                var somaEstoqueTask = from c in db.Testoques.Include(r => r.TestoqueItem)
                                      where c.Id_nfe_emitente == regra.Id_nfe_emitente &&
                                            c.TestoqueItem.Fabricante == regra.Estoque_Fabricante &&
                                            c.TestoqueItem.Produto == regra.Estoque_Produto &&
                                            c.TestoqueItem.Qtde_utilizada > 0 &&
                                            c.TestoqueItem.Qtde_utilizada.HasValue
                                      select (int)c.TestoqueItem.Qtde_utilizada;

                var somaEstoque = somaEstoqueTask.Select(r => r).SumAsync();

                var somaEstoqueGlobalTask = from c in db.Testoques.Include(r => r.TestoqueItem)
                                            where c.Fabricante == regra.Estoque_Fabricante &&
                                                  c.TestoqueItem.Produto == regra.Estoque_Produto &&
                                                  ((c.TestoqueItem.Qtde_utilizada > 0) &&
                                                   (c.Id_nfe_emitente == regra.Id_nfe_emitente) ||
                                                   ((from d in db.TnfEmitentes
                                                     where d.St_Habilitado_Ctrl_Estoque == 1 && d.St_Ativo == 1
                                                     select d.Id).Contains(c.Id_nfe_emitente)))
                                            select c.TestoqueItem.Qtde - c.TestoqueItem.Qtde_utilizada;

                var somaEstoqueGlobal = await somaEstoqueGlobalTask.Select(r => r).SumAsync();


                regra.Estoque_Qtde = (short)(await somaEstoque);
                retorno = true;
            }

            return retorno;
        }

        //afazer: Verificar o funcionamento
        public async Task<float> BuscarCoeficientePercentualCustoFinanFornec(PrePedidoDto prePedido, short qtdeParcelas, List<string> lstErros)
        {
            float coeficiente = 0;

            var db = contextoProvider.GetContextoLeitura();

            foreach (var i in prePedido.ListaProdutos)
            {
                var percCustoTask = from c in db.TpercentualCustoFinanceiroFornecedors
                                    where c.Fabricante == i.Fabricante &&
                                          c.Tipo_Parcelamento == prePedido.FormaPagtoCriacao.C_forma_pagto &&
                                          c.Qtde_Parcelas == qtdeParcelas
                                    select c;

                var percCusto = await percCustoTask.FirstOrDefaultAsync();

                if (percCusto != null)
                {
                    coeficiente = percCusto.Coeficiente;
                    i.VlLista = (decimal)coeficiente * i.VlLista;
                }
                else
                {
                    lstErros.Add("Opção de parcelamento não disponível para fornecedor " + i.Fabricante + ": " +
                        DecodificaCustoFinanFornecQtdeParcelas(prePedido.FormaPagtoCriacao.C_forma_pagto, qtdeParcelas) + " parcela(s)");
                }

            }

            return coeficiente;
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

                            //fazer montagem dos dados apartir daqui
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

                                var wmsRegraCdXUfXPessoaTask = from c in db.TwmsRegraCdXUfPessoas
                                                               where c.Id_wms_regra_cd_x_uf == itemRegra.TwmsRegraCdXUf.Id &&
                                                                     c.Tipo_pessoa == prePedido.DadosCliente.Tipo
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

        

        private async Task<IEnumerable<TorcamentoItem>> ComplementarInfosOrcamentoItem(List<TorcamentoItem> lstOrcamentoItem, string loja)
        {
            List<TorcamentoItem> lstRetorno = new List<TorcamentoItem>();

            var db = contextoProvider.GetContextoLeitura();

            foreach (TorcamentoItem item in lstOrcamentoItem)
            {

                var prodLista = from c in db.Tprodutos.Include(r => r.TprodutoLoja).Include(r => r.Tfabricante)
                                where c.Fabricante == item.Fabricante &&
                                      c.Produto == item.Produto &&
                                      c.TprodutoLoja.Loja == loja
                                select c;

                var prod = await prodLista.FirstOrDefaultAsync();

                if (prod != null)
                {
                    //montagem das informações do produto
                    item.Preco_Lista = prod.TprodutoLoja.Preco_Lista;
                    item.Margem = prod.TprodutoLoja.Margem;
                    item.Desc_Max = prod.TprodutoLoja.Desc_Max;
                    item.Comissao = prod.TprodutoLoja.Comissao;
                    item.Preco_Fabricante = prod.Preco_Fabricante;
                    item.Vl_Custo2 = prod.Vl_Custo2;
                    item.Descricao = prod.Descricao;
                    item.Descricao_Html = prod.Descricao_Html;
                    item.Ean = prod.Ean;
                    item.Grupo = prod.Grupo;
                    item.Peso = prod.Peso;
                    item.Qtde_Volumes = prod.Qtde_Volumes;
                    item.Markup_Fabricante = prod.Tfabricante.Markup;
                    item.Cubagem = prod.Cubagem;
                    item.Ncm = prod.Ncm;
                    item.Cst = prod.Cst;
                    item.Descontinuado = prod.Descontinuado;
                    item.CustoFinancFornecPrecoListaBase = (decimal)prod.TprodutoLoja.Preco_Lista;
                }
            }

            return lstRetorno;
        }

        private List<TorcamentoItem> MontaListaOrcamentoItem(List<PrepedidoProdutoDtoPrepedido> lstProdutos, string id_orcamento)
        {
            List<TorcamentoItem> lstOrcamentoItem = new List<TorcamentoItem>();

            foreach (PrepedidoProdutoDtoPrepedido p in lstProdutos)
            {
                TorcamentoItem item = new TorcamentoItem
                {
                    Orcamento = id_orcamento,
                    Produto = p.NumProduto,
                    Fabricante = Utils.Util.Normaliza_Codigo(p.Fabricante, Constantes.TAM_MIN_FABRICANTE),
                    Qtde = p.Qtde,
                    Preco_Venda = p.VlUnitario,
                    Preco_NF = p.Permite_Ra_Status == 1 ? p.Preco : p.VlUnitario,
                    Obs = p.Obs
                };
                lstOrcamentoItem.Add(item);
            }

            return lstOrcamentoItem;
        }


        private async Task<bool> ValidarOrcamentistaIndicador(string apelido)
        {
            bool retorno = false;

            var db = contextoProvider.GetContextoLeitura();

            var orcamentistaTask = await (from c in db.TorcamentistaEindicadors
                                          where c.Apelido == apelido
                                          select c.Apelido).FirstOrDefaultAsync();
            if (orcamentistaTask == apelido)
                retorno = true;

            return retorno;
        }

        private async Task<bool> ValidarSeOrcamentoJaExiste(PrePedidoDto prePedido)
        {
            bool retorno = true;

            var db = contextoProvider.GetContextoLeitura();

            var orcamentoTask = from c in db.Torcamentos.Include(r => r.TorcamentoItem)
                                where c.Id_Cliente == prePedido.DadosCliente.Id &&
                                      c.Data == DateTime.Now.Date &&
                                      c.Loja == prePedido.DadosCliente.Loja &&
                                      c.Orcamentista == prePedido.DadosCliente.Indicador_Orcamentista &&
                                      c.Hora == Convert.ToString(DateTime.Now.Hour +
                                                                 DateTime.Now.Minute +
                                                                 DateTime.Now.Second)
                                orderby c.TorcamentoItem.Orcamento, c.TorcamentoItem.Sequencia
                                select c.Orcamento;

            var qtdeRegistro = await orcamentoTask.ToListAsync();

            if (qtdeRegistro.Count == 0)
                retorno = false;

            return retorno;
        }

        private List<string> ValidarTipoCustoFinanceiroFornecedor(List<string> lstErros, string custoFinanceiroTipoParcelato)
        {
            if (custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA &&
                custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
                custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
                lstErros.Add("A forma de pagamento não foi informada (à vista, com entrada, sem entrada).");

            if (custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
                custoFinanceiroTipoParcelato != Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
            {
                if (int.Parse(custoFinanceiroTipoParcelato) <= 0)
                    lstErros.Add("Não foi informada a quantidade de parcelas para a forma de pagamento selecionada " +
                        "(" + DescricaoCustoFornecTipoParcelamento(custoFinanceiroTipoParcelato) + ")");
            }


            return lstErros;
        }

        private string DescricaoCustoFornecTipoParcelamento(string custoFinanceiro)
        {
            string retorno = "";

            switch (custoFinanceiro)
            {
                case Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA:
                    retorno = "Com Entrada";
                    break;
                case Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA:
                    retorno = "Sem Entrada";
                    break;
                case Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA:
                    retorno = "À Vista";
                    break;
            }

            return retorno;
        }

        //Busca produtos e verifica se loja esta cadastrada
        private async Task<IEnumerable<string>> VerificaCadaProdutoSelecionado(List<TorcamentoItem> lstOrcamentoItem,
            string loja)
        {
            List<string> lstErros = new List<string>();

            var db = contextoProvider.GetContextoLeitura();

            foreach (TorcamentoItem item in lstOrcamentoItem)
            {

                var prodLista = from c in db.Tprodutos.Include(r => r.TprodutoLoja).Include(r => r.Tfabricante)
                                where c.Fabricante == item.Fabricante &&
                                      c.Produto == item.Produto &&
                                      c.TprodutoLoja.Loja == loja
                                select c.Produto;

                var prod = await prodLista.FirstOrDefaultAsync();

                if (prod == null)
                    lstErros.Add("Produto " + item.Produto + " do fabricante " + item.Fabricante +
                        " NÃO está cadastrado para a loja " + loja);
            }
            return lstErros;
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

        private async Task<Tparametro> BuscarRegistroParametro(string id)
        {
            var db = contextoProvider.GetContextoLeitura();

            var parametroTask = from c in db.Tparametros
                                where c.Id == id
                                select c;

            var parametro = await parametroTask.FirstOrDefaultAsync();

            return parametro;

        }

        //private string MultiCdRegraDeterminaPessoa(string tipoCliente, byte contribuinteIcmsStatus, byte produtoRuralStatus)
        //{
        //    string tipoPessoa = "";

        //    if (tipoCliente == Constantes.ID_PF)
        //    {
        //        if (produtoRuralStatus == byte.Parse(Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM))
        //            tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL;
        //        else if (produtoRuralStatus == byte.Parse(Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO))
        //            tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA;
        //    }
        //    else if (tipoCliente == Constantes.ID_PJ)
        //    {
        //        if (contribuinteIcmsStatus == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM))
        //            tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE;
        //        else if (contribuinteIcmsStatus == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO))
        //            tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE;
        //        else if (contribuinteIcmsStatus == byte.Parse(Constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO))
        //            tipoPessoa = Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO;
        //    }

        //    return tipoPessoa;
        //}

        //private string Util.DescricaoMultiCDRegraTipoPessoa(string codTipoPessoa)
        //{
        //    string retorno = "";

        //    codTipoPessoa = codTipoPessoa.ToUpper();

        //    switch (codTipoPessoa)
        //    {
        //        case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA:
        //            retorno = "Pessoa Física";
        //            break;
        //        case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL:
        //            retorno = "Produtor Rural";
        //            break;
        //        case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE:
        //            retorno = "PJ Contribuinte";
        //            break;
        //        case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE:
        //            retorno = "PJ Não Contribuinte";
        //            break;
        //        case Constantes.COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO:
        //            retorno = "PJ Isento";
        //            break;
        //    }

        //    return retorno;
        //}

        private async Task<TorcamentistaEindicador> BuscarTorcamentista(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var tOrcamentistaTask = from c in db.TorcamentistaEindicadors
                                    where c.Apelido == apelido
                                    select c;
            TorcamentistaEindicador tOrcamentista = await tOrcamentistaTask.FirstOrDefaultAsync();

            return tOrcamentista;
        }        

        private string DescobrirQualTipoFormaPagamento(FormaPagtoCriacaoDto formaPagto)
        {
            string retorno = "nada";



            return retorno;
        }

        public void GerarNumeroOrcamento(PrePedidoDto prepedido)
        {
            string sufixoIdOrcamento = Constantes.SUFIXO_ID_ORCAMENTO;

            var nsuTask = Util.GerarNsu(Constantes.NSU_ORCAMENTO, contextoProvider);
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
