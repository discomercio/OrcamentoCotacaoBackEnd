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
        [Column("id")]
        public int id { get; set; }

        [Column("IdCfgTipoPropriedade")]
        public short IdCfgTipoPropriedade { get; set; }

        [Column("IdCfgTipoPermissaoEdicaoCadastro")]
        public short IdCfgTipoPermissaoEdicaoCadastro { get; set; }

        [Column("IdCfgDataType")]
        public short IdCfgDataType { get; set; }

        [Column("descricao")]
        public string descricao { get; set; }

        [Column("oculto")]
        public bool oculto { get; set; }

        [Column("ordem")]
        public int ordem { get; set; }

        [Column("dt_cadastro")]
        public DateTime dt_cadastro { get; set; }

        [Column("usuario_cadastro")]
        public string usuario_cadastro { get; set; }

        public TcfgDataType TcfgDataType { get; set; }

        public TcfgTipoPermissaoEdicaoCadastro TcfgTipoPermissaoEdicaoCadastro { get; set; }

        public TcfgTipoPropriedadeProdutoCatalogo TcfgTipoPropriedadeProdutoCatalogo { get; set; }

        public List<TProdutoCatalogoPropriedadeOpcao> TprodutoCatalogoPropriedadeOpcaos{ get; set; }

        public List<TprodutoCatalogoItem> TprodutoCatalogoItems { get; set; }
    }
}
