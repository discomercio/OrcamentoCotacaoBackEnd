using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsGlobais
{
    public static class Valores
    {
        public static decimal Arredondar2casas(decimal valor)
        {
            return Decimal.Round(valor, 2);
        }
    }
}
