using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoBusiness.Dto.Cep
{
    public class MunicipioDados
    {
        [MaxLength(7)]
        public string Codigo { get; set; }

        [MaxLength(150)]
        public string Descricao { get; set; }

        [MaxLength(150)]
        public string DescricaoSemAcento { get; set; }
    }
}
