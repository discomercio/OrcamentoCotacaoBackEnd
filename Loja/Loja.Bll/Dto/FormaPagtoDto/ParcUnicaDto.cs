﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.FormaPagtoDto
{
    public class ParcUnicaDto
    {
        public short Id { get; set; }

        public string Descricao { get; set; }
        public int? Ordenacao { get; set; }
    }
}
