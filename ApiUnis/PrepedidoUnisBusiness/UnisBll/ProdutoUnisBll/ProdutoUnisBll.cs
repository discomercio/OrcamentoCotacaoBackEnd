using InfraBanco.Constantes;
using Microsoft.EntityFrameworkCore;
using PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto;
using Produto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.ProdutoUnisBll
{
    public class ProdutoUnisBll
    {
        private readonly ProdutoGeralBll produtoBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public ProdutoUnisBll(ProdutoGeralBll produtoBll, InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.produtoBll = produtoBll;
            this.contextoProvider = contextoProvider;
        }
        public async Task<ProdutoComboUnisDto> ListaProdutosCombo(string loja, string cpf_cnpj)
        {
            if (loja == null)
                return null;
            if (cpf_cnpj == null)
                return null;

            var db = contextoProvider.GetContextoLeitura();
            //Buscar dados do cliente
            var clienteTask = (from c in db.Tcliente
                               where c.Cnpj_Cpf == cpf_cnpj 
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

            var ret = await produtoBll.ListaProdutosComboDados(loja, cliente.uf, cliente.tipo_cliente,
                (Constantes.ContribuinteICMS)cliente.contribuite_icms_status, 
                (Constantes.ProdutorRural)cliente.produtor_rural_status);
            if (ret == null)
                return null;

            ProdutoComboUnisDto produtoComboUnisDto = ProdutoComboUnisDto.ProdutoComboUnisDtoDeProdutoComboDados(ret);
            return produtoComboUnisDto;
        }

        public async Task<List<ProdutoUnisDto>> ListarProdutos(string loja)
        {
            var ret = await produtoBll.BuscarTodosProdutos(loja);
            if (ret == null)
                return null;

            //ProdutoComboUnisDto produtoComboUnisDto = ProdutoComboUnisDto.ProdutoComboUnisDtoDeProdutoComboDados(ret);
            return ((List<Produto.Dados.ProdutoDados>)ret).Select(x => new ProdutoUnisDto() { Produto = x.Produto  }).ToList();
        }
    }
}
