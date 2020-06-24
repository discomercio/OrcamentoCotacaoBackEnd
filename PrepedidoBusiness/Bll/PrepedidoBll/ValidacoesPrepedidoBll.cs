using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Dto.Produto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrepedidoBusiness.Bll.PrepedidoBll
{
    public class ValidacoesPrepedidoBll
    {
        public readonly CoeficienteBll coeficienteBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;


        public ValidacoesPrepedidoBll(CoeficienteBll coeficienteBll, InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.coeficienteBll = coeficienteBll;
            this.contextoProvider = contextoProvider;
        }

        //vamos validar os produtos que foram enviados
        public async Task MontarProdutosParaComparacao(List<PrepedidoProdutoDtoPrepedido> lstProdutos,
                    string siglaFormaPagto, int qtdeParcelas, string loja, List<string> lstErros)
        {
            List<PrepedidoProdutoDtoPrepedido> lstProdutosParaComparacao = new List<PrepedidoProdutoDtoPrepedido>();
            List<List<CoeficienteDto>> lstCoeficienteDtoArclube = new List<List<CoeficienteDto>>();
            List<CoeficienteDto> coefDtoArclube = new List<CoeficienteDto>();

            List<string> lstFornec = new List<string>();
            lstFornec = lstProdutos.Select(x => x.Fabricante).Distinct().ToList();

            //buscar coeficiente 
            List<CoeficienteDto> lstCoeficiente = (await BuscarListaCoeficientesFornecedores(lstFornec, qtdeParcelas, siglaFormaPagto)).ToList();
            List<PrepedidoProdutoDtoPrepedido> lstProdutosCompare = (await BuscarProdutos(lstProdutos, loja)).ToList();

            lstCoeficiente.ForEach(x =>
            {
                lstProdutosCompare.ForEach(y =>
                {
                    if (x.Fabricante == y.Fabricante)
                    {
                        //vamos calcular o preco_lista com o coeficiente
                        y.VlLista = Math.Round(((decimal)y.Preco * (decimal)x.Coeficiente), 2);
                        y.VlUnitario = Math.Round(y.VlLista * (decimal)(1 - y.Desconto / 100), 2);
                    }
                });
            });

            ConfrontarProdutos(lstProdutos, lstProdutosCompare, lstErros);
        }

        private async Task<IEnumerable<CoeficienteDto>> BuscarListaCoeficientesFornecedores(List<string> lstFornecedores, int qtdeParcelas, string siglaFP)
        {
            List<CoeficienteDto> lstcoefDto = new List<CoeficienteDto>();

            var lstcoeficientesTask = coeficienteBll.BuscarListaCoeficientesFornecedores(lstFornecedores);
            if (lstcoeficientesTask != null)
            {
                foreach (var i in await lstcoeficientesTask)
                {
                    lstcoefDto = new List<CoeficienteDto>();
                    foreach (var y in i)
                    {
                        if (y.TipoParcela == siglaFP && y.QtdeParcelas == qtdeParcelas)
                        {
                            lstcoefDto.Add(new CoeficienteDto()
                            {
                                Fabricante = y.Fabricante,
                                TipoParcela = y.TipoParcela,
                                QtdeParcelas = y.QtdeParcelas,
                                Coeficiente = y.Coeficiente
                            });
                        }
                    }
                }
            }

            return lstcoefDto;
        }

        private async Task<IEnumerable<PrepedidoProdutoDtoPrepedido>> BuscarProdutos(List<PrepedidoProdutoDtoPrepedido> lstProdutos, string loja)
        {
            var db = contextoProvider.GetContextoLeitura();
            List<PrepedidoProdutoDtoPrepedido> lsProdutosCompare = new List<PrepedidoProdutoDtoPrepedido>();

            lstProdutos.ForEach(async x =>
            {
                PrepedidoProdutoDtoPrepedido produto = await (from c in db.TprodutoLojas
                                                              where c.Produto == x.NumProduto &&
                                                                    c.Fabricante == x.Fabricante &&
                                                                    c.Vendavel == "S" &&
                                                                    c.Loja == loja
                                                              select new PrepedidoProdutoDtoPrepedido
                                                              {
                                                                  Fabricante = c.Fabricante,
                                                                  NumProduto = c.Produto,
                                                                  Preco = c.Preco_Lista,
                                                                  Desconto = x.Desconto
                                                              }).FirstOrDefaultAsync();

                if (produto != null)
                {
                    lsProdutosCompare.Add(produto);
                }
            });

            return await Task.FromResult(lsProdutosCompare);
        }

        private void ConfrontarProdutos(List<PrepedidoProdutoDtoPrepedido> lstProdutos,
            List<PrepedidoProdutoDtoPrepedido> lstProdutosCompare, List<string> lstErros)
        {
            decimal diffVlLista = 0;
            decimal diffVlUnitario = 0;
            decimal limite = 0.01M;

            lstProdutos.ForEach(x =>
            {
                lstProdutosCompare.ForEach(y =>
               {
                   if (x.NumProduto == y.NumProduto && x.Fabricante == y.Fabricante)
                   {
                       diffVlLista = Math.Abs(x.VlLista - y.VlLista);
                       diffVlUnitario = Math.Abs((x.VlUnitario - y.VlUnitario));

                        //afazer: pegar o valor do appsettings
                        if (diffVlLista < limite && diffVlUnitario < limite)
                       {
                           lstErros.Add("O valor do Produto (cód.)" + x.NumProduto + " está divergindo!");
                       }
                   }
               });
            });
        }
    }
}
