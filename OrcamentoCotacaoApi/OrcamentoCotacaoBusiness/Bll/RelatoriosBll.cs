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
            var diferenca = (request.DtFim.Value - request.DtInicio.Value).TotalDays;
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
                    Prepedido = item.Prepedido,
                    Pedido = item.Pedido,
                    Status = item.Status,
                    Vendedor = item.Vendedor,
                    Parceiro = item.Parceiro,
                    VendedorParceiro = item.VendedorParceiro,
                    UsuarioCadastro = item.UsuarioCadastro,
                    IdCliente = item.IdCliente,
                    UF = item.UF,
                    TipoCliente = item.TipoCliente,
                    ContribuinteIcms = item.ContribuinteIcms,
                    EntregaImediata = item.EntregaImediata,
                    PrevisaoEntrega = item.PrevisaoEntrega,
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
                    Descricao = item.Descricao,
                    Categoria = item.Categoria,
                    PrecoListaUnitAVista = item.PrecoListaUnitAVista,
                    PrecoListaUnitAPrazo = item.PrecoListaUnitAPrazo,
                    PrecoNFUnitAVista = item.PrecoNFUnitAVista,
                    PrecoNFUnitAPrazo = item.PrecoNFUnitAPrazo,
                    DescontoAVista = item.DescontoAVista,
                    DescontoAPrazo = item.DescontoAPrazo,
                    DescSuperiorAVista = item.DescSuperiorAVista,
                    DescSuperiorAPrazo = item.DescSuperiorAPrazo,
                    Comissao = item.Comissao,
                    Criacao = item.Criacao,
                    Expiracao = item.Expiracao
                };
                response.ListaItensOrcamento.Add(itemOrcamento);
            }

            response.Sucesso = true;
            return response;
        }
    }
}
