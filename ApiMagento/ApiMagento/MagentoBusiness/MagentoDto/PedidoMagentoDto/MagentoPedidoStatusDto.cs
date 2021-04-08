using System.ComponentModel.DataAnnotations;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class MagentoPedidoStatusDto
    {
        /// <summary>
        /// Status: 
        /// <br />
        /// 1 = aprovação pendente  (pedido cadastrado e pagamento não confirmado)
        /// <br />
        /// 2 = aprovado (pagamento confirmado)
        /// <br />
        /// 3 = rejeitado (pedido cancelado)
        /// <br />
        /// <br />
        /// Estado inicial: 1 ou 2.
        /// <br />
        /// Transições possíveis: de 1 para 2, de 1 para 3. Qualquer outra transição resulta em erro.
        /// <hr />
        /// </summary>
        [Required]
        public int Status { get; set; }
    }
}
