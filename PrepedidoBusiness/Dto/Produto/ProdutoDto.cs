using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Produto
{
    public class ProdutoDto
    {
        //esta lista depende, para fazer o cálculo do estoque, do DadosClienteCadastroDto.Tipo do cliente (PF ou PJ) e UF do cliente
        //afazer: confirmar com hamilton, está usando UF do endereço e não do endereço de entrega
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public string Descricao_html { get; set; }
        public decimal? Preco_lista { get; set; }
        public int Estoque { get; set; }
        public string Alertas { get; set; }
    }
}
        
        

    

    

    
