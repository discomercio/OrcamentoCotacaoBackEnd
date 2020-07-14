using System;

#nullable enable
namespace Loja.Bll.Dto.PrepedidoDto
{
    public class ResumoPrepedidoDto
    {
        public ResumoPrepedidoDto(string lojId, string orcamentista, string prepedido, string cliente, DateTime? data, decimal vlTotal)
        {
            LojaId = lojId ?? throw new ArgumentNullException(nameof(lojId));
            Orcamentista = orcamentista ?? throw new ArgumentNullException(nameof(orcamentista));
            Prepedido = prepedido ?? throw new ArgumentNullException(nameof(prepedido));
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
            Data = data;
            VlTotal = vlTotal;
        }

        public string LojaId { get; }
        public string Orcamentista { get; }
        public string Prepedido { get; }
        public string Cliente { get; }
        public DateTime? Data { get; }
        public decimal VlTotal { get; }
    }
}

