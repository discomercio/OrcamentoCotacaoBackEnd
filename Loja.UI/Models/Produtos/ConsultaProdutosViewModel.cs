using Loja.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loja.Bll.Dto.ProdutoDto;

namespace Loja.UI.Models.Produtos
{
    public class ConsultaProdutosViewModel
    {
        public string Fabricante { get; set; }

        public List<ConsultaProdutosDto> LstProdutos { get; set; }
    }
}
