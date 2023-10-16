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
                    PrevisaoEntrega = DateTime.TryParse(item.PrevisaoEntrega, out var data) ? data.AddHours(9) : null,
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
                    DataCadastro = DateTime.TryParse(item.Criacao, out var datacria) ? datacria.AddHours(9) : null,
                    Validade = DateTime.TryParse(item.Expiracao, out var exp) ? exp.AddHours(9) : null,
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

            var filtro = _mapper.Map<Relatorios.Filtros.DadosOrcamentosFiltro>(request);
            var listaDadosOrcamento = relatoriosBll.RelatorioDadosOrcamento(filtro);
            response.ListaDadosOrcamento = new List<DadosOrcamentoResponse>();

            foreach (var item in listaDadosOrcamento)
            {
                var dadoOrcamento = new DadosOrcamentoResponse()
                {
                    Loja = item.Loja,
                    Orcamento = item.Orcamento,
                    PrePedido = item.Prepedido,
                    Status = item.Status,
                    Pedido = item.Pedido,
                    Vendedor = item.Vendedor,
                    Indicador = item.Parceiro,
                    IndicadorVendedor = item.VendedorParceiro,
                    IdCliente = item.IdCliente,
                    UsuarioCadastro = item.UsuarioCadastro,
                    UF = item.UF,
                    TipoCliente = item.TipoCliente,
                    ContribuinteIcms = item.ContribuinteIcms,
                    QtdeMsgPendente = item.QtdeMsgPendente,
                    EntregaImediata = item.EntregaImediata,
                    PrevisaoEntrega = DateTime.TryParse(item.PrevisaoEntrega, out var data) ? data.AddHours(9) : null,
                    InstaladorInstala = item.InstaladorInstala,
                    ComissaoOpcao1 = item.ComissaoOpcao1,
                    DescMedioAVistaOpcao1 = !string.IsNullOrEmpty(item.DescMedioAVistaOpcao1) ? Math.Round(Decimal.Parse(item.DescMedioAVistaOpcao1), 2, MidpointRounding.AwayFromZero) : null,
                    DescMedioAPrazoOpcao1 = !string.IsNullOrEmpty(item.DescMedioAPrazoOpcao1) ? Math.Round(Decimal.Parse(item.DescMedioAPrazoOpcao1), 2, MidpointRounding.AwayFromZero) : null,
                    FormaPagtoAVistaOpcao1 = item.FormaPagtoAVistaOpcao1,
                    ValorFormaPagtoAVistaOpcao1 = !string.IsNullOrEmpty(item.ValorFormaPagtoAVistaOpcao1) ? Math.Round(decimal.Parse(item.ValorFormaPagtoAVistaOpcao1), 2, MidpointRounding.AwayFromZero): null,
                    StatusDescSuperiorAVistaOpcao1 = item.StatusDescSuperiorAVistaOpcao1,
                    FormaPagtoAPrazoOpcao1 = item.FormaPagtoAPrazoOpcao1,
                    ValorFormaPagtoAPrazoOpcao1 = !string.IsNullOrEmpty(item.ValorFormaPagtoAPrazoOpcao1) ? Math.Round(Decimal.Parse(item.ValorFormaPagtoAPrazoOpcao1), 2, MidpointRounding.AwayFromZero) : null,
                    QtdeParcelasFormaPagtoAPrazoOpcao1=item.QtdeParcelasFormaPagtoAPrazoOpcao1,
                    StatusDescSuperiorAPrazoOpcao1 = item.StatusDescSuperiorAPrazoOpcao1,
                    ComissaoOpcao2 = item.ComissaoOpcao2,
                    DescMedioAVistaOpcao2 = !string.IsNullOrEmpty(item.DescMedioAVistaOpcao2) ? Math.Round(Decimal.Parse(item.DescMedioAVistaOpcao2), 2, MidpointRounding.AwayFromZero) : null,
                    DescMedioAPrazoOpcao2 = !string.IsNullOrEmpty(item.DescMedioAPrazoOpcao2) ? Math.Round(Decimal.Parse(item.DescMedioAPrazoOpcao2), 2, MidpointRounding.AwayFromZero) : null,
                    FormaPagtoAVistaOpcao2 = item.FormaPagtoAVistaOpcao2,
                    ValorFormaPagtoAVistaOpcao2 = !string.IsNullOrEmpty(item.ValorFormaPagtoAVistaOpcao2) ? Math.Round(decimal.Parse(item.ValorFormaPagtoAVistaOpcao2), 2, MidpointRounding.AwayFromZero) : null,
                    StatusDescSuperiorAVistaOpcao2 = item.StatusDescSuperiorAVistaOpcao2,
                    FormaPagtoAPrazoOpcao2 = item.FormaPagtoAPrazoOpcao2,
                    ValorFormaPagtoAPrazoOpcao2 = !string.IsNullOrEmpty(item.ValorFormaPagtoAPrazoOpcao2) ? Math.Round(Decimal.Parse(item.ValorFormaPagtoAPrazoOpcao2), 2, MidpointRounding.AwayFromZero) : null,
                    QtdeParcelasFormaPagtoAPrazoOpcao2 = item.QtdeParcelasFormaPagtoAPrazoOpcao2,
                    StatusDescSuperiorAPrazoOpcao2 = item.StatusDescSuperiorAPrazoOpcao2,
                    ComissaoOpcao3 = item.ComissaoOpcao3,
                    DescMedioAVistaOpcao3 = !string.IsNullOrEmpty(item.DescMedioAVistaOpcao3) ? Math.Round(Decimal.Parse(item.DescMedioAVistaOpcao3), 2, MidpointRounding.AwayFromZero) : null,
                    DescMedioAPrazoOpcao3 = !string.IsNullOrEmpty(item.DescMedioAPrazoOpcao3) ? Math.Round(Decimal.Parse(item.DescMedioAPrazoOpcao3), 2, MidpointRounding.AwayFromZero) : null,
                    FormaPagtoAVistaOpcao3 = item.FormaPagtoAVistaOpcao3,
                    ValorFormaPagtoAVistaOpcao3 = !string.IsNullOrEmpty(item.ValorFormaPagtoAVistaOpcao3) ? Math.Round(decimal.Parse(item.ValorFormaPagtoAVistaOpcao3), 2, MidpointRounding.AwayFromZero) : null,
                    StatusDescSuperiorAVistaOpcao3 = item.StatusDescSuperiorAVistaOpcao3,
                    FormaPagtoAPrazoOpcao3 = item.FormaPagtoAPrazoOpcao3,
                    ValorFormaPagtoAPrazoOpcao3 = !string.IsNullOrEmpty(item.ValorFormaPagtoAPrazoOpcao3) ? Math.Round(Decimal.Parse(item.ValorFormaPagtoAPrazoOpcao3), 2, MidpointRounding.AwayFromZero) : null,
                    QtdeParcelasFormaPagtoAPrazoOpcao3 = item.QtdeParcelasFormaPagtoAPrazoOpcao3,
                    StatusDescSuperiorAPrazoOpcao3 = item.StatusDescSuperiorAPrazoOpcao3,
                    OpcaoAprovada = item.OpcaoAprovada,
                    ComissaoOpcaoAprovada = item.ComissaoOpcaoAprovada,
                    DescMedioOpcaoAprovada = !string.IsNullOrEmpty(item.DescMedioOpcaoAprovada) ? Math.Round(decimal.Parse(item.DescMedioOpcaoAprovada), 2, MidpointRounding.AwayFromZero) : null,
                    FormaPagtoOpcaoAprovada = item.FormaPagtoOpcaoAprovada,
                    ValorFormaPagtoOpcaoAprovada = !string.IsNullOrEmpty(item.ValorFormaPagtoOpcaoAprovada)? Math.Round(decimal.Parse(item.ValorFormaPagtoOpcaoAprovada), 2, MidpointRounding.AwayFromZero): null,
                    QtdeParcelasFormaOpcaoAprovada= item.QtdeParcelasFormaOpcaoAprovada,
                    StatusDescSuperiorOpcaoAprovada = item.StatusDescSuperiorOpcaoAprovada,
                    DataCadastro = DateTime.TryParse(item.Criacao, out var datacria) ? datacria.AddHours(9) : null,
                    Validade = DateTime.TryParse(item.Expiracao, out var exp) ? exp.AddHours(9) : null,
                };
                response.ListaDadosOrcamento.Add(dadoOrcamento);
            }

            response.Sucesso = true;
            return response;
        }
    }
}
