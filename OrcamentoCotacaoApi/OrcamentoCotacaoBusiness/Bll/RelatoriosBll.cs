using AutoMapper;
using InfraBanco.Modelos.Filtros;
using OrcamentoCotacao;
using OrcamentoCotacaoBusiness.Models.Request.Relatorios;
using OrcamentoCotacaoBusiness.Models.Response.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class RelatoriosBll
    {
        private readonly Relatorios.RelatoriosBll relatoriosBll;
        private readonly OrcamentoCotacaoBll orcamentoCotacaoBll;
        private readonly IMapper _mapper;
        public RelatoriosBll(Relatorios.RelatoriosBll _relatoriosBll, OrcamentoCotacaoBll _orcamentoCotacaoBll,
            IMapper mapper)
        {
            relatoriosBll = _relatoriosBll;
            orcamentoCotacaoBll = _orcamentoCotacaoBll;
            _mapper = mapper;
        }

        public RelatorioItensOrcamentosResponse RelatorioItensOrcamento(ItensOrcamentoRequest request)
        {
            var response = new RelatorioItensOrcamentosResponse();
            response.Sucesso = false;
            //validar data de criação e fim
            var config = orcamentoCotacaoBll.BuscarConfigValidade(request.LojaLogada);
            if (config == null)
            {
                response.Mensagem = "Falha ao buscar configurações do orçamento.";
                return response;
            }

            if (!request.DtInicio.HasValue)
            {
                response.Mensagem = "O campo 'Início da criação' é obrigatório!";
                return response;
            }
            if (!request.DtFim.HasValue)
            {
                response.Mensagem = "O campo 'Fim da criação' é obrigatório!";
                return response;
            }
            var diferenca = (request.DtFim.Value.Date - request.DtInicio.Value.Date).TotalDays;
            if (diferenca > config.MaxPeriodoConsulta_RelatorioGerencial)
            {
                response.Mensagem = $"A diferença entre as datas de \"Início da criação\" e \"Fim da criação\" ultrapassa o período de {config.MaxPeriodoConsulta_RelatorioGerencial} de dias!";
                return response;
            }

            var filtro = _mapper.Map<Relatorios.Filtros.ItensOrcamentosFiltro>(request);
            var listaItensOrcamento = relatoriosBll.RelatorioItensOrcamento(filtro);
            response.ListaItensOrcamento = new List<ItensOrcamentoResponse>();

            foreach (var item in listaItensOrcamento)
            {
                var itemOrcamento = new ItensOrcamentoResponse()
                {
                    Loja = item.Loja,
                    Orcamento = item.Orcamento,
                    PrePedido = item.Prepedido,
                    Pedido = item.Pedido,
                    Status = item.Status,
                    Vendedor = item.Vendedor,
                    Indicador = item.Parceiro,
                    IndicadorVendedor = item.VendedorParceiro,
                    UsuarioCadastro = item.UsuarioCadastro,
                    IdCliente = item.IdCliente,
                    UF = item.UF,
                    TipoCliente = item.TipoCliente,
                    ContribuinteIcms = item.ContribuinteIcms,
                    EntregaImediata = item.EntregaImediata,
                    PrevisaoEntrega = DateTime.TryParse(item.PrevisaoEntrega, out var data) ? data : null,
                    InstaladorInstala = item.InstaladorInstala,
                    NumOpcaoOrcamento = item.NumOpcaoOrcamento,
                    FormaPagtoAVista = item.FormaPagtoAVista,
                    FormaPagtoAPrazo = item.FormaPagtoAPrazo,
                    QtdeParcelasFormaPagtoAPrazo = item.QtdeParcelasFormaPagtoAPrazo,
                    OpcaoAprovada = item.OpcaoAprovada,
                    FormaPagtoOpcaoAprovada = item.FormaPagtoOpcaoAprovada,
                    Fabricante = item.Fabricante,
                    Produto = item.Produto,
                    Qtde = item.Qtde,
                    DescricaoProduto = item.Descricao,
                    Categoria = item.Categoria,
                    PrecoListaUnitAVista = !string.IsNullOrEmpty(item.PrecoListaUnitAVista) ? Math.Round(Decimal.Parse(item.PrecoListaUnitAVista), 2, MidpointRounding.AwayFromZero) : null,
                    PrecoListaUnitAPrazo = !string.IsNullOrEmpty(item.PrecoListaUnitAPrazo) ? Math.Round(Decimal.Parse(item.PrecoListaUnitAPrazo), 2, MidpointRounding.AwayFromZero) : null,
                    PrecoNFUnitAVista = !string.IsNullOrEmpty(item.PrecoNFUnitAVista) ? Math.Round(Decimal.Parse(item.PrecoNFUnitAVista), 2, MidpointRounding.AwayFromZero) : null,
                    PrecoNFUnitAPrazo = !string.IsNullOrEmpty(item.PrecoNFUnitAPrazo) ? Math.Round(Decimal.Parse(item.PrecoNFUnitAPrazo), 2, MidpointRounding.AwayFromZero) : null,
                    DescontoAVista = !string.IsNullOrEmpty(item.DescontoAVista) ? Math.Round(Decimal.Parse(item.DescontoAVista), 2, MidpointRounding.AwayFromZero) : null,
                    DescontoAPrazo = !string.IsNullOrEmpty(item.DescontoAPrazo) ? Math.Round(Decimal.Parse(item.DescontoAPrazo), 2, MidpointRounding.AwayFromZero) : null,
                    DescSuperiorAVista = !string.IsNullOrEmpty(item.DescSuperiorAVista) ? Math.Round(Decimal.Parse(item.DescSuperiorAVista), 2, MidpointRounding.AwayFromZero) : null,
                    DescSuperiorAPrazo = !string.IsNullOrEmpty(item.DescSuperiorAPrazo) ? Math.Round(Decimal.Parse(item.DescSuperiorAPrazo), 2, MidpointRounding.AwayFromZero) : null,
                    Comissao = !string.IsNullOrEmpty(item.Comissao) ? Math.Round(Decimal.Parse(item.Comissao), 1, MidpointRounding.AwayFromZero) : null,
                    DataCadastro = DateTime.TryParse(item.Criacao, out var datacria) ? datacria : null,
                    Validade = DateTime.TryParse(item.Expiracao, out var exp) ? exp : null,
                };
                response.ListaItensOrcamento.Add(itemOrcamento);
            }

            response.Sucesso = true;
            return response;
        }

        public RelatorioDadosOrcamentosResponse RelatorioDadosOrcamento(DadosOrcamentoRequest request)
        {
            var response = new RelatorioDadosOrcamentosResponse();
            response.Sucesso = false;
            
            var config = orcamentoCotacaoBll.BuscarConfigValidade(request.LojaLogada);
            if (config == null)
            {
                response.Mensagem = "Falha ao buscar configurações do orçamento.";
                return response;
            }

            if (!request.DtInicio.HasValue)
            {
                response.Mensagem = "O campo 'Início da criação' é obrigatório!";
                return response;
            }
            if (!request.DtFim.HasValue)
            {
                response.Mensagem = "O campo 'Fim da criação' é obrigatório!";
                return response;
            }
            var diferenca = (request.DtFim.Value.Date - request.DtInicio.Value.Date).TotalDays;
            if (diferenca > config.MaxPeriodoConsulta_RelatorioGerencial)
            {
                response.Mensagem = $"A diferença entre as datas de \"Início da criação\" e \"Fim da criação\" ultrapassa o período de {config.MaxPeriodoConsulta_RelatorioGerencial} de dias!";
                return response;
            }

            var filtro = _mapper.Map<Relatorios.Filtros.DadosOrcamentosFiltro>(request);
            filtro.ParamLimiteQtdeMaxOpcao = config.LimiteQtdeMaxOpcaoOrcamento;
            var listaDadosOrcamento = relatoriosBll.RelatorioDadosOrcamento(filtro);
            response.ListaDadosOrcamento = new List<DadosOrcamentoResponse>();

            foreach (var item in listaDadosOrcamento)
            {
                var dadoOrcamento = new DadosOrcamentoResponse();

                dadoOrcamento.Loja = item.Loja;
                dadoOrcamento.Orcamento = item.Orcamento;
                dadoOrcamento.PrePedido = item.Prepedido;
                dadoOrcamento.Status = item.Status;
                dadoOrcamento.Pedido = item.Pedido;
                dadoOrcamento.Vendedor = item.Vendedor;
                dadoOrcamento.Indicador = item.Parceiro;
                dadoOrcamento.IndicadorVendedor = item.VendedorParceiro;
                dadoOrcamento.IdCliente = item.IdCliente;
                dadoOrcamento.UsuarioCadastro = item.UsuarioCadastro;
                dadoOrcamento.UF = item.UF;
                dadoOrcamento.TipoCliente = item.TipoCliente;
                dadoOrcamento.ContribuinteIcms = item.ContribuinteIcms;
                dadoOrcamento.QtdeMsgPendente = item.QtdeMsgPendente;
                dadoOrcamento.EntregaImediata = item.EntregaImediata;
                dadoOrcamento.PrevisaoEntrega = DateTime.TryParse(item.PrevisaoEntrega, out var data) ? data : null;
                dadoOrcamento.InstaladorInstala = item.InstaladorInstala;
                dadoOrcamento.ListaOpcoes = new List<OpcaoDadosOrcamento>();
                foreach(var opcao in item.ListaOpcoes)
                {
                    OpcaoDadosOrcamento op = new OpcaoDadosOrcamento();
                    op.ComissaoOpcao = opcao.ComissaoOpcao;
                    op.DescMedioAVistaOpcao = !string.IsNullOrEmpty(opcao.DescMedioAVistaOpcao) ? Math.Round(Decimal.Parse(opcao.DescMedioAVistaOpcao), 2, MidpointRounding.AwayFromZero) : null;
                    op.DescMedioAPrazoOpcao = !string.IsNullOrEmpty(opcao.DescMedioAPrazoOpcao) ? Math.Round(Decimal.Parse(opcao.DescMedioAPrazoOpcao), 2, MidpointRounding.AwayFromZero) : null;
                    op.FormaPagtoAVistaOpcao = opcao.FormaPagtoAVistaOpcao;
                    op.ValorFormaPagtoAVistaOpcao = !string.IsNullOrEmpty(opcao.ValorFormaPagtoAVistaOpcao) ? Math.Round(decimal.Parse(opcao.ValorFormaPagtoAVistaOpcao), 2, MidpointRounding.AwayFromZero) : null;
                    op.StatusDescSuperiorAVistaOpcao = opcao.StatusDescSuperiorAVistaOpcao;
                    op.FormaPagtoAPrazoOpcao = opcao.FormaPagtoAPrazoOpcao;
                    op.ValorFormaPagtoAPrazoOpcao = !string.IsNullOrEmpty(opcao.ValorFormaPagtoAPrazoOpcao) ? Math.Round(Decimal.Parse(opcao.ValorFormaPagtoAPrazoOpcao), 2, MidpointRounding.AwayFromZero) : null;
                    op.QtdeParcelasFormaPagtoAPrazoOpcao = opcao.QtdeParcelasFormaPagtoAPrazoOpcao;
                    op.StatusDescSuperiorAPrazoOpcao = opcao.StatusDescSuperiorAPrazoOpcao;
                    dadoOrcamento.ListaOpcoes.Add(op);
                }
                dadoOrcamento.OpcaoAprovada = item.OpcaoAprovada;
                dadoOrcamento.ComissaoOpcaoAprovada = item.ComissaoOpcaoAprovada;
                dadoOrcamento.DescMedioOpcaoAprovada = !string.IsNullOrEmpty(item.DescMedioOpcaoAprovada) ? Math.Round(decimal.Parse(item.DescMedioOpcaoAprovada), 2, MidpointRounding.AwayFromZero) : null;
                dadoOrcamento.FormaPagtoOpcaoAprovada = item.FormaPagtoOpcaoAprovada;
                dadoOrcamento.ValorFormaPagtoOpcaoAprovada = !string.IsNullOrEmpty(item.ValorFormaPagtoOpcaoAprovada) ? Math.Round(decimal.Parse(item.ValorFormaPagtoOpcaoAprovada), 2, MidpointRounding.AwayFromZero) : null;
                dadoOrcamento.QtdeParcelasFormaOpcaoAprovada = item.QtdeParcelasFormaOpcaoAprovada;
                dadoOrcamento.StatusDescSuperiorOpcaoAprovada = item.StatusDescSuperiorOpcaoAprovada;
                dadoOrcamento.DataCadastro = DateTime.TryParse(item.Criacao, out var datacria) ? datacria : null;
                dadoOrcamento.Validade = DateTime.TryParse(item.Expiracao, out var exp) ? exp : null;
                
                response.ListaDadosOrcamento.Add(dadoOrcamento);
            }

            response.Sucesso = true;
            return response;
        }
    }
}
