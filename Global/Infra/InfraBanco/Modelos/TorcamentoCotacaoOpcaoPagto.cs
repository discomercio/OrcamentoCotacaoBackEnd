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
        [Column("Id")]
        public int Id { get; set; }

        [Column("IdOrcamentoCotacaoOpcao")]
        public int IdOrcamentoCotacaoOpcao { get; set; }

        [Column("Aprovado")]
        public bool Aprovado { get; set; }

        [Column("IdTipoUsuarioContextoAprovado")]
        public short? IdTipoUsuarioContextoAprovado { get; set; }

        [Column("IdUsuarioAprovado")]
        public int? IdUsuarioAprovado { get; set; }

        [Column("DataAprovado")]
        public DateTime? DataAprovado { get; set; }

        [Column("DataHoraAprovado")]
        public DateTime? DataHoraAprovado { get; set; }

        [Column("Observacao")]
        public string Observacao { get; set; }

        [Column("tipo_parcelamento")]
        public int Tipo_parcelamento { get; set; }

        [Column("av_forma_pagto")]
        public int Av_forma_pagto { get; set; }

        [Column("pc_qtde_parcelas")]
        public int Pc_qtde_parcelas { get; set; }

        [Column("pc_valor_parcela")]
        public decimal Pc_valor_parcela { get; set; }

        [Column("pc_maquineta_qtde_parcelas")]
        public int Pc_maquineta_qtde_parcelas { get; set; }

        [Column("pc_maquineta_valor_parcela")]
        public decimal Pc_maquineta_valor_parcela { get; set; }

        [Column("pce_forma_pagto_entrada")]
        public int Pce_forma_pagto_entrada { get; set; }

        [Column("pce_forma_pagto_prestacao")]
        public int Pce_forma_pagto_prestacao { get; set; }

        [Column("pce_entrada_valor")]
        public decimal Pce_entrada_valor { get; set; }

        [Column("pce_prestacao_qtde")]
        public int Pce_prestacao_qtde { get; set; }

        [Column("pce_prestacao_valor")]
        public decimal Pce_prestacao_valor { get; set; }

        [Column("pce_prestacao_periodo")]
        public int Pce_prestacao_periodo { get; set; }

        [Column("pse_forma_pagto_prim_prest")]
        public int Pse_forma_pagto_prim_prest { get; set; }

        [Column("pse_forma_pagto_demais_prest")]
        public int Pse_forma_pagto_demais_prest { get; set; }

        [Column("pse_prim_prest_valor")]
        public decimal Pse_prim_prest_valor { get; set; }

        [Column("pse_prim_prest_apos")]
        public int Pse_prim_prest_apos { get; set; }

        [Column("pse_demais_prest_qtde")]
        public int Pse_demais_prest_qtde { get; set; }

        [Column("pse_demais_prest_valor")]
        public decimal Pse_demais_prest_valor { get; set; }

        [Column("pse_demais_prest_periodo")]
        public int Pse_demais_prest_periodo { get; set; }

        [Column("pu_forma_pagto")]
        public int Pu_forma_pagto { get; set; }

        [Column("pu_valor")]
        public decimal Pu_valor { get; set; }

        [Column("pu_vencto_apos")]
        public int Pu_vencto_apos { get; set; }

        [Column("Habilitado")]
        public bool Habilitado { get; set; }

        public List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> TorcamentoCotacaoOpcaoItemAtomicoCustoFin { get; set; }
        public TcfgTipoUsuarioContexto TcfgTipoUsuarioContexto { get; set; }
    }
}
