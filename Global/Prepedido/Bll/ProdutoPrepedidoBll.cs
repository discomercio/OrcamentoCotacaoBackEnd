using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using Produto;
using System.Linq;
using System.Threading.Tasks;

namespace PrepedidoBusiness.Bll
{
    public class ProdutoPrepedidoBll
    {
        private readonly ProdutoGeralBll produtoGeralBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public ProdutoPrepedidoBll(
            Produto.ProdutoGeralBll produtoGeralBll, 
            InfraBanco.ContextoBdProvider contextoProvider
            )
        {
            this.produtoGeralBll = produtoGeralBll;
            this.contextoProvider = contextoProvider;
        }

        public async Task<PrepedidoBusiness.Dto.Produto.ProdutoComboDto> ListaProdutosComboApiArclube(string loja, string id_cliente)
        {
            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = (from c in db.Tclientes
                               where (c.Id == id_cliente || id_cliente == null)
                               select new
                               {
                                   tipo_cliente = c.Tipo,
                                   contribuite_icms_status = c.Contribuinte_Icms_Status,
                                   produtor_rural_status = c.Produtor_Rural_Status,
                                   uf = c.Uf
                               }).FirstOrDefaultAsync();

            var cliente = await clienteTask;

            if (cliente == null)
                return null;

            var aux = await produtoGeralBll.ListaProdutosComboDados(loja, cliente.uf, cliente.tipo_cliente, 
                (Constantes.ContribuinteICMS)cliente.contribuite_icms_status, 
                (Constantes.ProdutorRural)cliente.produtor_rural_status);

            return PrepedidoBusiness.Dto.Produto.ProdutoComboDto.ProdutoComboDto_De_ProdutoComboDados(aux);
        }
    }
}
