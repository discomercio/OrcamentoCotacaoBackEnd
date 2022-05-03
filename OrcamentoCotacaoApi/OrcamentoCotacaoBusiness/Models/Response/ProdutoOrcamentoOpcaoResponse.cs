using AutoMapper;
using InfraBanco.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ProdutoOrcamentoOpcaoResponseViewModel: Request.ProdutoRequestViewModel
    {
        public int Id { get; set; }
        public int IdItemUnificado { get; set; }
        public int IdOpcaoPagto { get; set; }


        
    }

}
