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
                cpfCnpjFormat.Add(Util.FormatCpf_Cnpj(cpf));
            }

            return cpfCnpjFormat;
        }

        public async Task<PedidoDto> BuscarPedido(string apelido, string numPedido)
        {
            var db = contextoProvider.GetContexto();

            //parateste
            //numPedido = "128591N";
            //apelido = "PEDREIRA";

            var pedido = from c in db.Tpedidos
                         where c.Pedido == numPedido && c.Orcamentista == apelido
                         select c;

            Tpedido p = new Tpedido();
            p = pedido.FirstOrDefault();
            if (p == null)
                return null;

            var dadosCliente = from c in db.Tclientes
                               where c.Id == p.Id_Cliente
                               select new DadosClienteCadastroDto
                               {
                                   Loja = p.Loja,
                                   Indicador = p.Indicador,
                                   Vendedor = p.Vendedor,
                                   Id = c.Id,
                                   Cnpj_Cpf = c.Cnpj_Cpf,
                                   Rg = c.Rg,
                                   Ie = c.Ie,
                                   Tipo = c.Tipo,
                                   Nascimento = c.Dt_Nasc,
                                   Sexo = c.Sexo,
                                   Nome = c.Nome,
                                   ProdutorRural = c.Produtor_Rural_Status,
                                   DddResidencial = c.Ddd_Res,
                                   TelefoneResidencial = c.Tel_Res,
                                   DddComercial = c.Ddd_Com,
                                   Ramal = c.Ramal_Com,
                                   DddCelular = c.Ddd_Cel,
                                   Obs = c.Obs_crediticias,
                                   Email = c.Email,
                                   Endereco = c.Endereco,
                                   Numero = c.Endereco_Numero,
                                   Bairro = c.Bairro,
                                   Cidade = c.Cidade,
                                   Uf = c.Uf,
                                   Cep = c.Cep
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

            string analiseCredito = Convert.ToString(p.Analise_Credito);

            switch (analiseCredito)
            {
                case Constantes.COD_AN_CREDITO_ST_INICIAL:
                    analiseCredito = "";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE:
                    analiseCredito = "Pendente";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_VENDAS:
                    analiseCredito = "Pendente Vendas";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_ENDERECO:
                    analiseCredito = "Pendente Endereço";
                    break;
                case Constantes.COD_AN_CREDITO_OK:
                    analiseCredito = "Crédito OK";
                    break;
                case Constantes.COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO:
                    analiseCredito = "Crédito OK (aguardando depósito)";
                    break;
                case Constantes.COD_AN_CREDITO_OK_DEPOSITO_AGUARDANDO_DESBLOQUEIO:
                    analiseCredito = "Crédito OK (depósito aguardando desbloqueio)";
                    break;
                case Constantes.COD_AN_CREDITO_NAO_ANALISADO:
                    analiseCredito = "";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_CARTAO:
                    analiseCredito = "Pendente Cartão de Crédito";
                    break;
            }


            if (analiseCredito != "")
            {
                analiseCredito += p.Analise_credito_Data;
            }

            //verifica o status da entrega
            DateTime? dataEntrega = new DateTime();
            if (p.St_Entrega == Constantes.ST_ENTREGA_A_ENTREGAR || p.St_Entrega == Constantes.ST_ENTREGA_SEPARAR)
                dataEntrega = p.A_Entregar_Data_Marcada;

            var perdas = BuscarPerdas(numPedido);

            decimal TotalPerda = (decimal)perdas.Result.Select(r => r.Valor).Sum();

            var transportador = from c in db.Ttransportadoras
                                where c.Id == p.Transportadora_Id
                                select c.Nome;
            string TranspNome = await transportador.Select(r => r.ToString()).FirstOrDefaultAsync();

            DetalhesFormaPagamentos detalhesFormaPagto = new DetalhesFormaPagamentos
            {
                FormaPagto = p.Forma_Pagto,
                InfosAnaliseCredito = p.Forma_Pagto,
                StatusPagto = p.St_Pagto,
                VlTotalFamilia = p.Vl_Total_Familia,
                VlPago = p.Vl_Total_Familia,
                VlPerdas = TotalPerda,
                SaldoAPagar = saldo_a_pagar,
                AnaliseCredito = analiseCredito,
                DataColeta = dataEntrega,
                Transportadora = TranspNome,
                VlFrete = p.Frete_Valor,
                //BlocoNotas = "",  retirar pois precisa de DTO para eles
                //Ocorrencias="",
                VlDevolucao = 0
            };

            PedidoDto DtoPedido = new PedidoDto
            {
                NumeroPedido = numPedido,
                DataHoraPedido = p.Data_Hora,
                StatusHoraPedido = p.St_Entrega + p.Entregue_Data,
                DadosCliente = dadosCliente.FirstOrDefault(),
                ListaProdutos = produtosItens.ToList(),
                DetalhesNF = detalhesNf,
                DetalhesFormaPagto = detalhesFormaPagto,
                ListaProdutoDevolvido = await BuscarProdutosDevolvidos(numPedido),
                ListaPerdas = await BuscarPerdas(numPedido),
                BlocoNotas = await BuscarPedidoBlocoNotas(numPedido)
            };

            return await Task.FromResult(DtoPedido);
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
