using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Especificacao.Ambiente.PrepedidoApi
{
    class InjecaoDependencias
    {
        public static void ConfigurarDependencias(IServiceCollection services)
        {
            //bll
            services.AddTransient<global::PrepedidoBusiness.Bll.PrepedidoApiBll, global::PrepedidoBusiness.Bll.PrepedidoApiBll>();
            services.AddTransient<Prepedido.PrepedidoBll, Prepedido.PrepedidoBll>();
            services.AddTransient<global::PrepedidoBusiness.Bll.ClientePrepedidoBll, global::PrepedidoBusiness.Bll.ClientePrepedidoBll>();
            services.AddTransient<Cliente.ClienteBll, Cliente.ClienteBll>();
            services.AddTransient<Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll, Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll>();
            services.AddTransient<global::PrepedidoBusiness.Bll.PedidoPrepedidoApiBll, global::PrepedidoBusiness.Bll.PedidoPrepedidoApiBll>();
            services.AddTransient<global::PrepedidoBusiness.Bll.AcessoBll, global::PrepedidoBusiness.Bll.AcessoBll>();
            services.AddTransient<Produto.ProdutoGeralBll, Produto.ProdutoGeralBll>();
            services.AddTransient<global::PrepedidoBusiness.Bll.ProdutoPrepedidoBll, global::PrepedidoBusiness.Bll.ProdutoPrepedidoBll>();
            services.AddTransient<Cep.CepBll, Cep.CepBll>();
            services.AddTransient<global::PrepedidoBusiness.Bll.CepPrepedidoBll, global::PrepedidoBusiness.Bll.CepPrepedidoBll>();
            services.AddTransient<Prepedido.FormaPagto.FormaPagtoBll, Prepedido.FormaPagto.FormaPagtoBll>();
            services.AddTransient<global::PrepedidoBusiness.Bll.FormaPagtoPrepedidoBll, global::PrepedidoBusiness.Bll.FormaPagtoPrepedidoBll>();
            services.AddTransient<Prepedido.FormaPagto.ValidacoesFormaPagtoBll, Prepedido.FormaPagto.ValidacoesFormaPagtoBll>();
            services.AddTransient<Produto.CoeficienteBll, Produto.CoeficienteBll>();
            services.AddTransient<global::PrepedidoBusiness.Bll.CoeficientePrepedidoBll, global::PrepedidoBusiness.Bll.CoeficientePrepedidoBll>();
            services.AddTransient<Prepedido.ValidacoesPrepedidoBll, Prepedido.ValidacoesPrepedidoBll>();
            services.AddTransient<Prepedido.MontarLogPrepedidoBll, Prepedido.MontarLogPrepedidoBll>();

            services.AddTransient<Cep.IBancoNFeMunicipio, Testes.Utils.BancoTestes.TestesBancoNFeMunicipio>();


        }

    }
}
