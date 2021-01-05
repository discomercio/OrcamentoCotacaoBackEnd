using Loja.Bll.Dto.CepDto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.UI.Models.Cep
{
    public class CepViewModel
    {
        public List<CepDto> ListaCep { get; set; }
        public string ClienteTipo { get; set; }
        public SelectList LstProdutoRural { get; set; }
        public SelectList LstContribuinte { get; set; }
    }
}
