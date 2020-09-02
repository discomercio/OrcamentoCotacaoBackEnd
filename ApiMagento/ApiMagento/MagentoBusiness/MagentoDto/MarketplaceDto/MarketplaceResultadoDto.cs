using System;
using System.Collections.Generic;
using System.Text;

namespace MagentoBusiness.MagentoDto.MarketplaceDto
{
    public class MarketplaceResultadoDto
    {
        public List<MarketplaceMagentoDto> ListaMarketplace { get; set; }
        public List<string> ListaErros { get; set; }
    }
}
