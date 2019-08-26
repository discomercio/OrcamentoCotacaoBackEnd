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
                lista = lista.Where(r => r.Tcliente.Nome.Contains(clienteBusca));
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

        public async Task<PedidoDto> BuscarCliente(string apelido, string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var pedido = from c in db.Tpedidos
                         where c.Pedido == numPedido && c.Orcamentista == apelido
                         select c;

            Tpedido p = new Tpedido();
            p = pedido.FirstOrDefault();

            var dadosCliente = from c in db.Tclientes
                               where c.Id == p.Id_Cliente
                               select new DadosClienteCadastroDto
                               {
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

            var produtosItens = from c in db.TpedidoItems
                                where c.Pedido == numPedido
                                select new PedidoProdutosDtoPedido
                                {
                                    Fabricante = c.Fabricante,
                                    NumProduto = c.Produto,
                                    Descricao = c.Descricao,
                                    Qtde = c.Qtde,
                                    Faltando = c.Qtde,//Fazer um metodo para produtos que estão faltando
                                    VlLista = c.Preco_Lista,
                                    Desconto = c.Desc_Dado,
                                    VlVenda = c.Qtde * c.Preco_Venda,
                                    VlTotal = c.Qtde * c.Preco_Venda,
                                    Comissao = c.Comissao
                                };


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


            //buscar o valor Total de devoluções
            var vlTotDevolucao = from c in db.TpedidoItemDevolvidos
                                 where c.Pedido.StartsWith(numPedido)
                                 select c.Qtde * c.Preco_NF;

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

            
            if(analiseCredito != "")
            {
                analiseCredito += p.Analise_credito_Data;
            }

            //verifica o status da entrega
            DateTime? dataEntrega = new DateTime();
            if (p.St_Entrega == Constantes.ST_ENTREGA_A_ENTREGAR || p.St_Entrega == Constantes.ST_ENTREGA_SEPARAR)
                dataEntrega = p.A_Entregar_Data_Marcada;

            DetalhesFormaPagamentos detalhesFormaPagto = new DetalhesFormaPagamentos
            {
                FormaPagto = p.Forma_Pagto,
                InfosAnaliseCredito = p.Forma_Pagto,
                StatusPagto = p.St_Pagto,
                VlTotalFamilia = p.Vl_Total_Familia,
                VlPago = p.Vl_Total_Familia,
                VlPerdas = vlTotDevolucao.Single(),//verificar se dara certo
                SaldoAPagar = p.Vl_Total_Familia - vlTotDevolucao.Single(),
                AnaliseCredito = analiseCredito,
                DataColeta = dataEntrega
                //terminar de montar o dto
            };

            PedidoDto DtoPedido = new PedidoDto
            {
                DadosCliente = dadosCliente.FirstOrDefault(),
                ListaProdutos = produtosItens.ToList(),
                DetalhesNF = detalhesNf,
                DetalhesFormaPagto = detalhesFormaPagto
            };

            return await Task.FromResult(DtoPedido);
        }
    }
}
