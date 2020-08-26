using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoBusiness.Dto.Cep
{
    public class UFeMunicipiosDados
    {
        [MaxLength(2)]
        public string Codigo { get; set; }

        [MaxLength(2)]
        public string SiglaUF { get; set; }

        [MaxLength(50)]
        public string Descricao { get; set; }

        public List<MunicipioDados> ListaMunicipio { get; set; }
    }
}
