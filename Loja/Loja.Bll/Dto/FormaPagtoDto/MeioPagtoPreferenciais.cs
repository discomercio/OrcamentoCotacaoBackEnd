using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.FormaPagtoDto
{
    public class MeioPagtoPreferenciais
    {
        public string Id { get; set; }
        public int Campo_inteiro { get; set; }
        //public decimal Campo_monetario { get; set; }
        //public float Campo_real { get; set; }
        public string Campo_data { get; set; }
        public string Campo_texto { get; set; }
        //public DateTime Dt_hr_ult_atualizacao { get; set; }
        //public string Usuario_ult_atualizacao { get; set; }
    }
}
