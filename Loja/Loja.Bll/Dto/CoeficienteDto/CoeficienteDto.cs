﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.CoeficienteDto
{
    public class CoeficienteDto
    {
        public string Fabricante { get; set; }
        public string TipoParcela { get; set; }
        public short QtdeParcelas { get; set; }
        public float Coeficiente { get; set; }
    }
}
