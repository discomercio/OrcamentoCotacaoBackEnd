using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using PrepedidoBusiness.Dtos.Pedido;
using PrepedidoBusiness.Utils;
using PrepedidoBusiness.Dtos.ClienteCadastro;
using InfraBanco.Modelos;
using PrepedidoBusiness.Dto.Pedido.DetalhesPedido;
using Microsoft.EntityFrameworkCore.Internal;
using InfraBanco.Constantes;
using System.Collections;

namespace PrepedidoBusiness.Bll
{
    public class PedidoBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public PedidoBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<string>> ListarNumerosPedidoCombo(string apelido)
        {
            var db = contextoProvider.GetContexto();

            var lista = from p in db.Tpedidos
                        where p.Orcamentista == apelido
                        orderby p.Pedido
                        select p.Pedido;

            var res = lista.AsEnumerable();
            return await Task.FromResult(res);
        }

        public enum TipoBuscaPedido
        {
            Todos = 0, PedidosEncerrados = 1, PedidosEmAndamento = 2
        }


        public async Task<IEnumerable<PedidoDtoPedido>> ListarPedidos(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            //fazemos a busca
            var ret = await ListarPedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPedido, dataInicial, dataFinal);

            //se tiver algum registro, retorna imediatamente
            if (ret.Count() > 0)
                return ret;

            /*
             * se fizeram a busca por algum CPF ou CNPJ ou pedido e não achamos nada, removemos o filtro de datas
             * para não aparcer para o usuário que não tem nenhum registro
             * */
            if (String.IsNullOrEmpty(clienteBusca) && String.IsNullOrEmpty(numeroPedido))
                return ret;

            //busca sem datas
            ret = await ListarPedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPedido, null, null);
            if (ret.Count() > 0)
                return ret;

            //ainda não achamos nada? então faz a busca sem filtrar por tipo
            ret = await ListarPedidosFiltroEstrito(apelido, TipoBuscaPedido.Todos, clienteBusca, numeroPedido, null, null);
            return ret;
        }

        //a busca sem malabarismos para econtrar algum registro
        private async Task<IEnumerable<PedidoDtoPedido>> ListarPedidosFiltroEstrito(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            var db = contextoProvider.GetContexto();

            var lista = db.Tpedidos.Include(r => r.Tcliente).
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

            var listaFinal = lista.Select(r => new PedidoDtoPedido
            {
                NomeCliente = r.Tcliente.Nome_Iniciais_Em_Maiusculas,
                NumeroPedido = r.Pedido,
                DataPedido = r.Data,
                Status = r.St_Entrega,
                ValorTotal = r.Vl_Total_NF
            }).OrderByDescending(r => r.DataPedido);


            //colocar as mensagens de status
            var listaComStatus = await listaFinal.ToListAsync();
            foreach (var pedido in listaComStatus)
            {
                if (pedido.Status == "ESP")
                    pedido.Status = "Em espera";
                if (pedido.Status == "SPL")
                    pedido.Status = "Split possível";
                if (pedido.Status == "SEP")
                    pedido.Status = "Separar";
                if (pedido.Status == "AET")
                    pedido.Status = "A entregar";
                if (pedido.Status == "ETG")
                    pedido.Status = "Entrega";
                if (pedido.Status == "CAN")
                    pedido.Status = "Cancelado";
            }
            return await Task.FromResult(listaComStatus);
        }

        public async Task<IEnumerable<string>> ListarCpfCnpjPedidosCombo(string apelido)
        {
            var db = contextoProvider.GetContexto();

            var lista = (from c in db.Tpedidos.Include(r => r.Tcliente)
                         where c.Orcamentista == apelido
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

        public async Task<PedidoDto> BuscarPedido(string apelido, string numPedido)
        {
            var db = contextoProvider.GetContexto();

            //parateste
            //numPedido = "000678N";
            //apelido = "SOLUTION";

            var pedido = from c in db.Tpedidos
                         where c.Pedido == numPedido && c.Orcamentista == apelido
                         select c;
            Tpedido p = pedido.FirstOrDefault();
            if (p == null)
                return null;

            var dadosCliente = from c in db.Tclientes
                               where c.Id == p.Id_Cliente
                               select c;
            Tcliente cli = dadosCliente.FirstOrDefault();
            DadosClienteCadastroDto cadastroCliente = new DadosClienteCadastroDto
            {
                Loja = p.Loja,
                Indicador = p.Indicador,
                Vendedor = p.Vendedor,
                Id = cli.Id,
                Cnpj_Cpf = Util.FormatCpf_Cnpj_Ie(cli.Cnpj_Cpf),
                Rg = Util.FormatCpf_Cnpj_Ie(cli.Rg),
                Ie = Util.FormatCpf_Cnpj_Ie(cli.Ie),
                Tipo = cli.Tipo,
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
                Obs = cli.Obs_crediticias,
                Email = cli.Email,
                Endereco = cli.Endereco,
                Numero = cli.Endereco_Numero,
                Bairro = cli.Bairro,
                Cidade = cli.Cidade,
                Uf = cli.Uf,
                Cep = cli.Cep
            };

            EnderecoEntregaDtoClienteCadastro enderecoEntrega = new EnderecoEntregaDtoClienteCadastro
            {
                EndEtg_endereco = p.Endereco_Logradouro,
                EndEtg_endereco_numero = p.EndEtg_Endereco_Numero,
                EndEtg_endereco_complemento = p.EndEtg_Endereco_Complemento,
                EndEtg_bairro = p.EndEtg_Bairro,
                EndEtg_cidade = p.EndEtg_Cidade,
                EndEtg_uf = p.EndEtg_UF,
                EndEtg_cep = p.EndEtg_Cep,
                EndEtg_cod_justificativa = await ObterDescricao_Cod(Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA, p.EndEtg_Cod_Justificativa)
            };

            short? faltante = (short)await VerificarEstoque(numPedido);

            var produtosItens = from c in db.TpedidoItems
                                where c.Pedido == numPedido
                                select new PedidoProdutosDtoPedido
                                {
                                    Fabricante = c.Fabricante,
                                    NumProduto = c.Produto,
                                    Descricao = c.Descricao,
                                    Qtde = c.Qtde,
                                    Faltando = faltante,
                                    Preco = c.Preco_NF,
                                    VlLista = c.Preco_Lista,
                                    Desconto = c.Desc_Dado,
                                    VlVenda = c.Qtde * c.Preco_Venda,
                                    VlTotal = c.Qtde * c.Preco_Venda,
                                    Comissao = c.Comissao
                                };

            decimal saldo_a_pagar = await CalculaSaldoAPagar(numPedido);
            decimal vlFamiliaParcelaRA = await CalculaTotalFamiliaRA(numPedido);

            if (p.St_Entrega == Constantes.ST_PAGTO_PAGO && saldo_a_pagar > 0)
                saldo_a_pagar = 0;

            DetalhesNFPedidoDtoPedido detalhesNf = new DetalhesNFPedidoDtoPedido
            {
                Observacoes = p.Obs_1,
                ConstaNaNF = p.Nfe_Texto_Constar,
                XPed = p.Nfe_XPed,
                NumeroNF = p.Obs_2,
                NFSimples = p.Obs_3,
                EntregaImediata = p.St_Etg_Imediata,
                StBemUsoConsumo = p.StBemUsoConsumo,
                InstaladorInstala = p.InstaladorInstalaStatus
            };

            //verifica o status da entrega
            DateTime? dataEntrega = new DateTime();
            if (p.St_Entrega == Constantes.ST_ENTREGA_A_ENTREGAR || p.St_Entrega == Constantes.ST_ENTREGA_SEPARAR)
                dataEntrega = p.A_Entregar_Data_Marcada;

            var perdas = BuscarPerdas(numPedido);

            decimal TotalPerda = (decimal)perdas.Result.Select(r => r.Valor).Sum();

            var transportadora = from c in db.Ttransportadoras
                                 where c.Id == p.Transportadora_Id
                                 select c.Nome;
            string TranspNome = await transportadora.Select(r => r.ToString()).FirstOrDefaultAsync();

            //buscar valor total de devoluções NF
            var vlDevNf = from c in db.TpedidoItemDevolvidos
                          where c.Pedido.StartsWith(numPedido)
                          select c.Qtde * c.Preco_NF;
            decimal vl_TotalFamiliaDevolucaoPrecoNF = await vlDevNf.Select(r => r.Value).SumAsync();

            IEnumerable<string> lstFormaPgto = await ObterFormaPagto(numPedido, apelido);

            DetalhesFormaPagamentos detalhesFormaPagto = new DetalhesFormaPagamentos
            {
                FormaPagto = lstFormaPgto.ToList(),
                InfosAnaliseCredito = p.Forma_Pagto,
                StatusPagto = p.St_Pagto,
                VlTotalFamilia = p.Vl_Total_Familia,
                VlPago = p.Vl_Total_Familia,
                VlDevolucao = vl_TotalFamiliaDevolucaoPrecoNF,
                VlPerdas = TotalPerda,
                SaldoAPagar = saldo_a_pagar,
                AnaliseCredito = await ObterAnaliseCredito(Convert.ToString(p.Analise_Credito), numPedido, apelido),
                DataColeta = dataEntrega,
                Transportadora = TranspNome,
                VlFrete = p.Frete_Valor
            };

            var lstOcorrencia = await ObterOcorrencias(numPedido);
            List<OcorrenciasDtoPedido> listaOcorrencia = lstOcorrencia.ToList();

            PedidoDto DtoPedido = new PedidoDto
            {
                NumeroPedido = numPedido,
                DataHoraPedido = p.Data_Hora,
                StatusHoraPedido = p.St_Entrega + p.Entregue_Data,
                DadosCliente = cadastroCliente,
                ListaProdutos = produtosItens.ToList(),
                DetalhesNF = detalhesNf,
                DetalhesFormaPagto = detalhesFormaPagto,
                ListaProdutoDevolvido = await BuscarProdutosDevolvidos(numPedido),
                ListaPerdas = await BuscarPerdas(numPedido),
                BlocoNotas = await BuscarPedidoBlocoNotas(numPedido),
                EnderecoEntrega = enderecoEntrega,
                ListaOcorrencia = listaOcorrencia
            };

            return await Task.FromResult(DtoPedido);
        }

        public async Task<IEnumerable<OcorrenciasDtoPedido>> ObterOcorrencias(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var id = await(from c in db.TpedidoOcorrencias
                     where c.Pedido == numPedido
                     select c.Id).FirstOrDefaultAsync();

            //nenhuma ocorrencia para esse pedido
            if (id == 0)
                return new List<OcorrenciasDtoPedido>();


            var msg = (await ObterMensagemOcorrencia(id)).ToList();

            var ocorrencia = from d in db.TpedidoOcorrencias
                             where d.Pedido == numPedido
                             select new OcorrenciasDtoPedido
                             {
                                 Usuario = d.Usuario_Cadastro,
                                 Dt_Hr_Cadastro = d.Dt_Hr_Cadastro,
                                 Situacao = (d.Finalizado_Status != 0 ? "Finalizado" : (from c in db.TpedidoOcorrenciaMensagems
                                                                                        where c.Id_Ocorrencia == d.Id &&
                                                                                              c.Fluxo_Mensagem == Constantes.COD_FLUXO_MENSAGEM_OCORRENCIAS_EM_PEDIDOS__CENTRAL_PARA_LOJA
                                                                                        select c).Count() > 0 ? "Em Andamento" : "Aberta"),
                                 Contato = d.Contato + d.Tel_1 != "" ? "(" + d.Ddd_1 + ") " + d.Tel_1 : d.Tel_2 != "" ? "(" + d.Ddd_2 + ") " + d.Tel_2 : "",
                                 Texto_Ocorrencia = d.Texto_Ocorrencia,
                                 mensagemDtoOcorrenciaPedidos = msg,
                                 Finalizado_Usuario = d.Finalizado_Usuario,
                                 Finalizado_Data_Hora = d.Finalizado_Data_Hora,
                                 Tipo_Ocorrencia = ObterDescricao_Cod(Constantes.GRUPO_T_CODIGO_DESCRICAO__OCORRENCIAS_EM_PEDIDOS__TIPO_OCORRENCIA, d.Tipo_Ocorrencia).ToString(),
                                 Texto_Finalizacao = d.Texto_Finalizacao
                             };

            return await Task.FromResult(ocorrencia);
        }

        public async Task<IEnumerable<MensagemDtoOcorrenciaPedido>> ObterMensagemOcorrencia(int idOcorrencia)
        {
            var db = contextoProvider.GetContexto();

            var msg = from c in db.TpedidoOcorrenciaMensagems
                      where c.Id_Ocorrencia == idOcorrencia
                      select new MensagemDtoOcorrenciaPedido
                      {
                          Dt_Hr_Cadastro = c.Dt_Hr_Cadastro,
                          Usuario = c.Usuario_Cadastro,
                          Loja = c.Loja,
                          Texto_Mensagem = c.Texto_Mensagem
                      };

            return await Task.FromResult(msg);
        }

        public async Task<string> ObterAnaliseCredito(string codigo, string numPedido, string apelido)
        {
            string retorno = "";

            switch (codigo)
            {
                case Constantes.COD_AN_CREDITO_ST_INICIAL:
                    retorno = "";
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
                    retorno = "";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_CARTAO:
                    retorno = "Pendente Cartão de Crédito";
                    break;
            }

            if (retorno != "")
            {
                var db = contextoProvider.GetContexto();

                var ret = from c in db.Tpedidos
                          where c.Pedido == numPedido && c.Orcamentista == apelido
                          select new { analise_credito_data = c.Analise_credito_Data.ToString(), analise_credito_usuario = c.Analise_Credito_Usuario.ToString() };

                if (await ret.Select(r => r.analise_credito_data).FirstOrDefaultAsync() != "")
                {
                    string credito = await ret.Select(r => r.analise_credito_data).FirstOrDefaultAsync();
                    if (await ret.Select(r => r.analise_credito_usuario).FirstOrDefaultAsync() != "")
                    {
                        credito += " - " + await ret.Select(r => r.analise_credito_usuario).FirstOrDefaultAsync();
                        retorno = retorno + "(" + credito + ")";
                    }
                }
            }

            return await Task.FromResult(retorno);
        }

        public async Task<decimal> CalculaSaldoAPagar(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            //buscar o valor total pago
            var vlFamiliaP = from c in db.TpedidoPagamentos
                             where c.Pedido.StartsWith(numPedido)
                             select c;
            decimal vl_TotalFamiliaPago = await vlFamiliaP.Select(r => r.Valor).SumAsync();

            //buscar valor total NF
            var vlNf = from c in db.TpedidoItems.Include(r => r.Tpedido)
                       where c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO && c.Tpedido.Pedido.StartsWith(numPedido)
                       select c.Qtde * c.Preco_NF;
            decimal vl_TotalFamiliaPrecoNF = await vlNf.Select(r => r.Value).SumAsync();

            //buscar valor total de devoluções NF
            var vlDevNf = from c in db.TpedidoItemDevolvidos
                          where c.Pedido.StartsWith(numPedido)
                          select c.Qtde * c.Preco_NF;
            decimal vl_TotalFamiliaDevolucaoPrecoNF = await vlDevNf.Select(r => r.Value).SumAsync();

            decimal result = vl_TotalFamiliaPrecoNF - vl_TotalFamiliaPago - vl_TotalFamiliaDevolucaoPrecoNF;

            return await Task.FromResult(result);
        }

        public async Task<decimal> CalculaTotalFamiliaRA(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var vlTotalVendaPorItem = from c in db.TpedidoItems.Include(r => r.Tpedido)
                                      where c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO && c.Tpedido.Pedido.StartsWith(numPedido)
                                      select new { venda = c.Qtde * c.Preco_Venda, nf = c.Qtde * c.Preco_NF };

            var vlTotalVenda = await vlTotalVendaPorItem.Select(r => r.venda).SumAsync();
            var vlTotalNf = await vlTotalVendaPorItem.Select(r => r.nf).SumAsync();

            decimal result = vlTotalVenda.Value - vlTotalNf.Value;

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<ProdutoDevolvidoDtoPedido>> BuscarProdutosDevolvidos(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var lista = from c in db.TpedidoItemDevolvidos
                        where c.Pedido.StartsWith(numPedido)
                        select new ProdutoDevolvidoDtoPedido
                        {
                            Data = c.Devolucao_Data,
                            Hora = c.Devolucao_Hora,
                            Qtde = c.Qtde,
                            CodProduto = c.Produto,
                            DescricaoProduto = c.Descricao,
                            Motivo = c.Motivo,
                            NumeroNF = c.NFe_Numero_NF
                        };

            return await Task.FromResult(lista);
        }

        public async Task<IEnumerable<PedidoPerdasDtoPedido>> BuscarPerdas(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var lista = from c in db.TpedidoPerdas
                        where c.Pedido == numPedido
                        select new PedidoPerdasDtoPedido
                        {
                            Data = c.Data,
                            Hora = c.Hora,
                            Valor = c.Valor,
                            Obs = c.Obs
                        };

            return await Task.FromResult(lista);
        }

        public async Task<int> VerificarEstoque(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var fabricanteProduto = from c in db.TpedidoItems
                                    where c.Pedido == numPedido
                                    select new { fabricante = c.Fabricante, produto = c.Produto };
            string fabricante = await fabricanteProduto.Select(r => r.fabricante).FirstOrDefaultAsync();
            string produto = await fabricanteProduto.Select(r => r.produto).FirstOrDefaultAsync();

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
        }

        public async Task<BlocoNotasDtoPedido> BuscarPedidoBlocoNotas(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var bl = from c in db.TpedidoBlocosNotas
                     where c.Pedido == numPedido &&
                           c.Nivel_Acesso == Constantes.COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__PUBLICO &&
                           c.Anulado_Status == 0
                     select c;

            BlocoNotasDtoPedido bloco = new BlocoNotasDtoPedido
            {
                Dt_Hora_Cadastro = await bl.Select(r => r.Dt_Hr_Cadastro).FirstOrDefaultAsync(),
                Usuario = await bl.Select(r => r.Usuario).FirstOrDefaultAsync(),
                Loja = await bl.Select(r => r.Loja).FirstOrDefaultAsync(),
                Mensagem = await bl.Select(r => r.Mensagem).FirstOrDefaultAsync()
            };

            return await Task.FromResult(bloco);

        }

        public async Task<string> ObterDescricao_Cod(string grupo, string cod)
        {
            var db = contextoProvider.GetContexto();

            var desc = from c in db.TcodigoDescricaos
                       where c.Grupo == grupo && c.Codigo == cod
                       select c.Descricao;

            string result = await desc.FirstOrDefaultAsync();

            if (result == null || result == "")
                return await Task.FromResult("Código não cadastrado (" + cod + ")");

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<string>> ObterFormaPagto(string numPedido, string orcamentista)
        {
            var db = contextoProvider.GetContexto();

            var p = from c in db.Tpedidos
                    where c.Pedido == numPedido && c.Orcamentista == orcamentista
                    select c;

            Tpedido pedido = p.FirstOrDefault();
            List<string> lista = new List<string>();
            string parcelamento = Convert.ToString(pedido.Tipo_Parcelamento);

            switch (parcelamento)
            {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    lista.Add("À Vista (" + await OpcaoFormaPagto(parcelamento) + ")");
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    lista.Add("Parcela Única: " + Constantes.SIMBOLO_MONETARIO + " " +
                        await OpcaoFormaPagto(Convert.ToString(pedido.Pu_Forma_Pagto)) + " vencendo após " + pedido.Pu_Vencto_Apos + " dias");
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    lista.Add("Parcelado no Cartão (internet) em " + pedido.Pc_Qtde_Parcelas + " X " +
                        Constantes.SIMBOLO_MONETARIO + " " + pedido.Pc_Valor_Parcela);
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    lista.Add("Parcelado no Cartão (maquineta) em " + pedido.Pc_Maquineta_Qtde_Parcelas + " X " + pedido.Pc_Maquineta_Valor_Parcela);
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    lista.Add("Entrada: " + Constantes.SIMBOLO_MONETARIO + " " +
                        pedido.Pce_Entrada_Valor + " (" + await OpcaoFormaPagto(Convert.ToString(pedido.Pce_Forma_Pagto_Entrada)) + ")");
                    lista.Add("Prestações: " + pedido.Pce_Prestacao_Qtde + " X " + Constantes.SIMBOLO_MONETARIO + " " + pedido.Pce_Prestacao_Valor +
                        " (" + await OpcaoFormaPagto(Convert.ToString(pedido.Pce_Forma_Pagto_Prestacao)) + ") vencendo a cada " +
                        pedido.Pce_Prestacao_Periodo + " dias");
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    lista.Add("1ª Prestação: " + Constantes.SIMBOLO_MONETARIO + " " + pedido.Pse_Prim_Prest_Valor + " (" +
                        await OpcaoFormaPagto(Convert.ToString(pedido.Pse_Forma_Pagto_Prim_Prest)) + ") vencendo após " + pedido.Pse_Prim_Prest_Apos + " dias");
                    lista.Add("Demais Prestações: " + pedido.Pse_Demais_Prest_Qtde + " X " + Constantes.SIMBOLO_MONETARIO + " " + pedido.Pse_Demais_Prest_Valor +
                        " (" + await OpcaoFormaPagto(Convert.ToString(pedido.Pse_Forma_Pagto_Demais_Prest)) + ") vencendo a cada " +
                        pedido.Pse_Demais_Prest_Periodo + " dias");
                    break;
            }

            return await Task.FromResult(lista.ToList());
        }

        public async Task<string> OpcaoFormaPagto(string codigo)
        {
            string retorno = "";

            switch (codigo)
            {
                case Constantes.ID_FORMA_PAGTO_DINHEIRO:
                    retorno = "Dinheiro";
                    break;
                case Constantes.ID_FORMA_PAGTO_DEPOSITO:
                    retorno = "Depósito";
                    break;
                case Constantes.ID_FORMA_PAGTO_CHEQUE:
                    retorno = "Cheque";
                    break;
                case Constantes.ID_FORMA_PAGTO_BOLETO:
                    retorno = "Boleto";
                    break;
                case Constantes.ID_FORMA_PAGTO_CARTAO:
                    retorno = "Cartão (internet)";
                    break;
                case Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA:
                    retorno = "Cartão (maquineta)";
                    break;
                case Constantes.ID_FORMA_PAGTO_BOLETO_AV:
                    retorno = "Boleto AV";
                    break;
            };

            return await Task.FromResult(retorno);
        }


        //public async Task<decimal> CalculaTotalDevolucoes_Venda_E_NF(string numPedido)
        //{
        //    var db = contextoProvider.GetContexto();

        //    var itemDevTotal = from c in db.TpedidoItemDevolvidos
        //                       where c.Pedido.StartsWith(numPedido)
        //                       select new { venda = c.Qtde * c.Preco_Venda, nf = c.Qtde * c.Preco_NF };

        //    var vlTotDevVenda = await itemDevTotal.Select(r => r.venda).SumAsync();
        //    var vlTotDevNf = await itemDevTotal.Select(r => r.nf).SumAsync();

        //    decimal result = vlTotDevVenda.Value - vlTotDevNf.Value;

        //    return await Task.FromResult(result);
        //}

    }
}
