using InfraBanco.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UtilsGlobais.RequestResponse;

namespace Produto.Dados
{
    public class ProdutoCatalogoPropriedadeDados: RequestBase
    {
        public int id { get; set; }
        public short IdCfgTipoPropriedade { get; set; }
        public short IdCfgTipoPermissaoEdicaoCadastro { get; set; }
        public short IdCfgDataType { get; set; }
        public string descricao { get; set; }
        public bool oculto { get; set; }
        public int ordem { get; set; }
        public DateTime dt_cadastro { get; set; }
        public string usuario_cadastro { get; set; }
        public string loja { get; set; }

        [JsonProperty("produtoCatalogoPropriedadeOpcao")]
        public List<ProdutoCatalogoPropriedadeOpcoesDados> produtoCatalogoPropriedadeOpcoesDados { get; set; }
    }
}
        
        

    

    

    
