﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoBusiness.Dto.Cep
{
    public class UFeMunicipiosDto
    {
        [MaxLength(2)]
        public string Codigo { get; set; }

        [MaxLength(2)]
        public string SiglaUF { get; set; }

        [MaxLength(50)]
        public string Descricao { get; set; }

        public List<MunicipioDto> ListaMunicipio { get; set; }
    }
}
