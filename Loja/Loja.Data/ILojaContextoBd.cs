using Loja.Modelo;
using Loja.Modelos;
using System.Linq;

namespace Loja.Data
{
    public interface ILojaContextoBd
    {
        IQueryable<TalertaProduto> TalertaProdutos { get; }
        IQueryable<Tbanco> Tbancos { get; }
        IQueryable<TclienteRefBancaria> TclienteRefBancarias { get; }
        IQueryable<TclienteRefComercial> TclienteRefComercials { get; }
        IQueryable<Tcliente> Tclientes { get; }
        IQueryable<TcodigoDescricao> TcodigoDescricaos { get; }
        IQueryable<Tcontrole> Tcontroles { get; }
        IQueryable<Tdesconto> Tdescontos { get; }
        IQueryable<TecProdutoCompostoItem> TecProdutoCompostoItems { get; }
        IQueryable<TecProdutoComposto> TecProdutoCompostos { get; }
        IQueryable<TestoqueItem> TestoqueItems { get; }
        IQueryable<TestoqueLog> TestoqueLogs { get; }
        IQueryable<TestoqueMovimento> TestoqueMovimentos { get; }
        IQueryable<Testoque> Testoques { get; }
        IQueryable<Tfabricante> Tfabricantes { get; }
        IQueryable<TfinControle> TfinControles { get; }
        IQueryable<TformaPagto> TformaPagtos { get; }
        IQueryable<Tlog> Tlogs { get; }
        IQueryable<Tloja> Tlojas { get; }
        IQueryable<TnfEmitente> TnfEmitentes { get; }
        IQueryable<Toperacao> Toperacaos { get; }
        IQueryable<TorcamentistaEIndicadorRestricaoFormaPagto> torcamentistaEIndicadorRestricaoFormaPagtos { get; }
        IQueryable<TorcamentistaEindicador> TorcamentistaEindicadors { get; }
        IQueryable<TorcamentoItem> TorcamentoItems { get; }
        IQueryable<Torcamento> Torcamentos { get; }
        IQueryable<Tparametro> Tparametros { get; }
        IQueryable<TpedidoAnaliseEndereco> TpedidoAnaliseEnderecos { get; }
        IQueryable<TpedidoBlocosNotas> TpedidoBlocosNotas { get; }
        IQueryable<TpedidoItemDevolvidoBlocoNotas> TpedidoItemDevolvidoBlocoNotas { get; }
        IQueryable<TpedidoItemDevolvido> TpedidoItemDevolvidos { get; }
        IQueryable<TpedidoItem> TpedidoItems { get; }
        IQueryable<TpedidoOcorrenciaMensagem> TpedidoOcorrenciaMensagems { get; }
        IQueryable<TpedidoOcorrencia> TpedidoOcorrencias { get; }
        IQueryable<TpedidoPagamento> TpedidoPagamentos { get; }
        IQueryable<TpedidoPerda> TpedidoPerdas { get; }
        IQueryable<Tpedido> Tpedidos { get; }
        IQueryable<TpercentualCustoFinanceiroFornecedor> TpercentualCustoFinanceiroFornecedors { get; }
        IQueryable<TperfilItem> TperfilItems { get; }
        IQueryable<Tperfil> Tperfils { get; }
        IQueryable<TperfilUsuario> TperfiUsuarios { get; }
        IQueryable<TprazoPagtoVisanet> TprazoPagtoVisanets { get; }
        IQueryable<TprodutoLoja> TprodutoLojas { get; }
        IQueryable<Tproduto> Tprodutos { get; }
        IQueryable<TprodutoXAlerta> TprodutoXAlertas { get; }
        IQueryable<TprodutoXwmsRegraCd> TprodutoXwmsRegraCds { get; }
        IQueryable<TsessaoHistorico> TsessaoHistoricos { get; }
        IQueryable<TtransportadoraCep> TtransportadoraCeps { get; }
        IQueryable<Ttransportadora> Ttransportadoras { get; }
        IQueryable<Tusuario> Tusuarios { get; }
        IQueryable<TusuarioXLoja> TusuarioXLojas { get; }
        IQueryable<TwmsRegraCd> TwmsRegraCds { get; }
        IQueryable<TwmsRegraCdXUfPessoa> TwmsRegraCdXUfPessoas { get; }
        IQueryable<TwmsRegraCdXUf> TwmsRegraCdXUfs { get; }
        IQueryable<TwmsRegraCdXUfXPessoaXCd> TwmsRegraCdXUfXPessoaXCds { get; }
    }
}