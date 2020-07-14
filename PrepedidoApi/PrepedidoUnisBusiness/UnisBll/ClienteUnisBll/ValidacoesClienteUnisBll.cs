using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using InfraBanco;
using Microsoft.EntityFrameworkCore;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using InfraBanco.Constantes;
using PrepedidoBusiness.Utils;
using InfraBanco.Modelos;

namespace PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll
{
    public class ValidacoesClienteUnisBll
    {
        public static async Task<TorcamentistaEindicador> ValidarBuscarOrcamentista(string apelido, ContextoBdProvider contexto)
        {
            var db = contexto.GetContextoLeitura();

            TorcamentistaEindicador orcamentista = await (from c in db.TorcamentistaEindicadors
                                                          where c.Apelido == apelido
                                                          select c).FirstOrDefaultAsync();

            return orcamentista;
        }

    }
}
