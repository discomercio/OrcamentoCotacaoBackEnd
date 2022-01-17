using Produto.Dados;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Produto
{
    public class CoeficienteDto
    {
        public string Fabricante { get; set; }
        public string TipoParcela { get; set; }
        public short QtdeParcelas { get; set; }
        public float Coeficiente { get; set; }

        public static CoeficienteDto CoeficienteDto_De_CoeficienteDados(CoeficienteDados origem)
        {
            if (origem == null) return null;
            return new CoeficienteDto()
            {
                Fabricante = origem.Fabricante,
                TipoParcela = origem.TipoParcela,
                QtdeParcelas = origem.QtdeParcelas,
                Coeficiente = origem.Coeficiente
            };
        }

        internal static List<CoeficienteDto> CoeficienteDtoLista_De_CoeficienteDados(IEnumerable<CoeficienteDados> origem)
        {
            if (origem == null) return null;
            var ret = new List<CoeficienteDto>();
            if (origem != null)
                foreach (var p in origem)
                    ret.Add(CoeficienteDto_De_CoeficienteDados(p));
            return ret;
        }
        internal static List<List<CoeficienteDto>> CoeficienteDtoListaLista_De_CoeficienteDados(IEnumerable<IEnumerable<CoeficienteDados>> origem)
        {
            if (origem == null) return null;
            var ret = new List<List<CoeficienteDto>>();
            if (origem != null)
                foreach (var p in origem)
                    ret.Add(CoeficienteDtoLista_De_CoeficienteDados(p));
            return ret;
        }


    }
}
