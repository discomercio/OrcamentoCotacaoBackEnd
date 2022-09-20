using FormaPagamento;
using Microsoft.Extensions.DependencyInjection;

namespace Especificacao.Ambiente.PrepedidoApi
{
    class InjecaoDependencias
    {
        public static void ConfigurarDependencias(IServiceCollection services)
        {
            //bll
            services.AddTransient<global::Prepedido.Bll.PrepedidoApiBll, global::Prepedido.Bll.PrepedidoApiBll>();
            services.AddTransient<global::Prepedido.Bll.PrepedidoBll, global::Prepedido.Bll.PrepedidoBll>();
            services.AddTransient<global::Prepedido.Bll.ClientePrepedidoBll, global::Prepedido.Bll.ClientePrepedidoBll>();
            services.AddTransient<Cliente.ClienteBll, Cliente.ClienteBll>();
            services.AddTransient<global::Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll, global::Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll>();
            services.AddTransient<global::Prepedido.Bll.AcessoBll, global::Prepedido.Bll.AcessoBll>();
            services.AddTransient<Produto.ProdutoGeralBll, Produto.ProdutoGeralBll>();
            services.AddTransient<global::Prepedido.Bll.ProdutoPrepedidoBll, global::Prepedido.Bll.ProdutoPrepedidoBll>();
            services.AddTransient<Cep.CepBll, Cep.CepBll>();
            services.AddTransient<global::Prepedido.Bll.CepPrepedidoBll, global::Prepedido.Bll.CepPrepedidoBll>();
            services.AddTransient<FormaPagtoBll, FormaPagtoBll>();
            services.AddTransient<global::Prepedido.Bll.FormaPagtoPrepedidoBll, global::Prepedido.Bll.FormaPagtoPrepedidoBll>();
            services.AddTransient<ValidacoesFormaPagtoBll, ValidacoesFormaPagtoBll>();
            services.AddTransient<Produto.CoeficienteBll, Produto.CoeficienteBll>();
            services.AddTransient<global::Prepedido.Bll.CoeficientePrepedidoBll, global::Prepedido.Bll.CoeficientePrepedidoBll>();
            services.AddTransient<global::Prepedido.Bll.ValidacoesPrepedidoBll, global::Prepedido.Bll.ValidacoesPrepedidoBll>();
            services.AddTransient<global::Prepedido.Bll.MontarLogPrepedidoBll, global::Prepedido.Bll.MontarLogPrepedidoBll>();

            services.AddTransient<Cep.IBancoNFeMunicipio, Testes.Utils.BancoTestes.TestesBancoNFeMunicipio>();
        }
    }
}
