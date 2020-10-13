using System;
using System.Collections.Generic;
using System.Text;

namespace MagentoBusiness.MagentoDto.MarketplaceDto
{
    public class MarketplaceResultadoDto
    {
        public List<MarketplaceMagentoDto> ListaMarketplace { get; set; } = new List<MarketplaceMagentoDto>();
        public List<string> ListaErros { get; set; } = new List<string>();
    }
}
