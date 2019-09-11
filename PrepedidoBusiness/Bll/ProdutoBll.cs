using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.Produto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InfraBanco.Modelos;
using System.Linq;

namespace PrepedidoBusiness.Bll
{
    public class ProdutoBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public ProdutoBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        private bool LojaHabilitadaProdutosECommerce(string loja)
        {
            bool retorno = false;

            if (loja == Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
                retorno = true;
            if (loja == Constantes.NUMERO_LOJA_BONSHOP)
                retorno = true;
            if (IsLojaVrf(loja))
                retorno = true;
            if (loja == Constantes.NUMERO_LOJA_MARCELO_ARTVEN)
                retorno = true;
            if (loja == Constantes.NUMERO_LOJA_BONSHOP_LAB)
                retorno = true;

            return retorno;
        }

        private bool IsLojaVrf(string loja)
        {
            bool retorno = false;

            if (loja == Constantes.NUMERO_LOJA_VRF ||
                loja == Constantes.NUMERO_LOJA_VRF2 ||
                loja == Constantes.NUMERO_LOJA_VRF3 ||
                loja == Constantes.NUMERO_LOJA_VRF4)
                retorno = true;

            return retorno;
        }

        public async Task<IEnumerable<ProdutoDto>> BuscarProduto(string codProduto, string loja, string apelido)
        {
            //paraTeste
            //apelido = "MARISARJ";
            //codProduto = "003000";
            //loja = "202";

            bool lojaHabilitada = false;
            decimal vlProdCompostoPrecoListaLoja = 0;

            List<ProdutoDto> lstProduto = new List<ProdutoDto>();

            var db = contextoProvider.GetContextoLeitura();

            if (string.IsNullOrEmpty(codProduto))
                return null;
            else if (string.IsNullOrEmpty(loja))
                return null;

            if (LojaHabilitadaProdutosECommerce(loja))
            {
                var prodCompostoTask = from c in db.TecProdutoCompostos
                                       where c.Produto_Composto == codProduto
                                       select c;

                string parada = "";

                var t = prodCompostoTask.FirstOrDefault();

                if (t != null)
                {
                    
                }
            }


            return lstProduto;
        }
    }
}
