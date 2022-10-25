using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TProdutoCatalogoPropriedade : IModel
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

        public TcfgDataType TcfgDataType { get; set; }

        public TcfgTipoPermissaoEdicaoCadastro TcfgTipoPermissaoEdicaoCadastro { get; set; }

        public TcfgTipoPropriedadeProdutoCatalogo TcfgTipoPropriedadeProdutoCatalogo { get; set; }

        public List<TProdutoCatalogoPropriedadeOpcao> TprodutoCatalogoPropriedadeOpcaos{ get; set; }

        public List<TprodutoCatalogoItem> TprodutoCatalogoItems { get; set; }
    }
}
