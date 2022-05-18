using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    public class TorcamentoCotacaoOpcaoPagto : IModel
    {
        public int Id { get; set; }
        public int IdOrcamentoCotacaoOpcao { get; set; }
        public bool Aprovado { get; set; }
        public short? IdTipoUsuarioContextoAprovado { get; set; }
        public int IdUsuarioAprovado { get; set; }
        public DateTime? DataAprovado { get; set; }
        public DateTime? DataHoraAprovado { get; set; }
        public string Observacao { get; set; }
        public int Tipo_parcelamento { get; set; }
        public int Av_forma_pagto { get; set; }
        public int Pc_qtde_parcelas { get; set; }
        public decimal Pc_valor_parcela { get; set; }
        public int Pc_maquineta_qtde_parcelas { get; set; }
        public decimal Pc_maquineta_valor_parcela { get; set; }
        public int Pce_forma_pagto_entrada { get; set; }
        public int Pce_forma_pagto_prestacao { get; set; }
        public decimal Pce_entrada_valor { get; set; }
        public int Pce_prestacao_qtde { get; set; }
        public decimal Pce_prestacao_valor { get; set; }
        public int Pce_prestacao_periodo { get; set; }
        public int Pse_forma_pagto_prim_prest { get; set; }
        public int Pse_forma_pagto_demais_prest { get; set; }
        public decimal Pse_prim_prest_valor { get; set; }
        public int Pse_prim_prest_apos { get; set; }
        public int Pse_demais_prest_qtde { get; set; }
        public decimal Pse_demais_prest_valor { get; set; }
        public int Pse_demais_prest_periodo { get; set; }
        public int Pu_forma_pagto { get; set; }
        public decimal Pu_valor { get; set; }
        public int Pu_vencto_apos { get; set; }      
        
        public TcfgTipoUsuarioContexto TcfgTipoUsuarioContexto { get; set; }
    }
}
