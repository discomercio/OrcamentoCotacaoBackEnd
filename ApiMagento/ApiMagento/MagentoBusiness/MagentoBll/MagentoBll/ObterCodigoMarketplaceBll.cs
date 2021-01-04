using InfraBanco;
using MagentoBusiness.MagentoDto;
using MagentoBusiness.MagentoDto.MarketplaceDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoBusiness.MagentoBll.MagentoBll
{
    public class ObterCodigoMarketplaceBll
    {
        private readonly ContextoBdProvider contextoProvider;

        public ObterCodigoMarketplaceBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<MarketplaceResultadoDto> ObterCodigoMarketplace()
        {
            MarketplaceResultadoDto resultado = new MarketplaceResultadoDto
            {
                ListaMarketplace = new List<MarketplaceMagentoDto>(),
                ListaErros = new List<string>()
            };


            List<InfraBanco.Modelos.TcodigoDescricao> listarCodigo = await (UtilsGlobais.Util.ListarCodigoMarketPlace(contextoProvider).ToListAsync());

            if (listarCodigo == null)
            {
                resultado.ListaErros.Add("Falha ao buscar lista Marketplace.");
                return resultado;
            }


            foreach (var x in listarCodigo)
            {
                resultado.ListaMarketplace.Add(new MarketplaceMagentoDto()
                {
                    Grupo = x.Grupo,
                    Codigo = x.Codigo,
                    Descricao = x.Descricao,
                    Descricao_parametro = x.Descricao_parametro,
                    Parametro_1_campo_flag = x.Parametro_1_campo_flag,
                    Parametro_2_campo_flag = x.Parametro_2_campo_flag,
                    Parametro_2_campo_texto = x.Parametro_2_campo_texto,
                    Parametro_3_campo_flag = x.Parametro_3_campo_flag,
                    Parametro_3_campo_texto = x.Parametro_3_campo_texto,
                    Parametro_4_campo_flag = x.Parametro_4_campo_flag,
                    Parametro_5_campo_flag = x.Parametro_5_campo_flag,
                    Parametro_campo_texto = x.Parametro_campo_texto
                });
            };

            return resultado;
        }
    }
}
