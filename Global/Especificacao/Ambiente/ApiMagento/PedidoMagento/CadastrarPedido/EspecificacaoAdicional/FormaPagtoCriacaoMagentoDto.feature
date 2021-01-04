@ignore
Feature: FormaPagtoCriacaoMagentoDto
#validar os campos:
#        /// Tipo_Parcelamento:
#        ///     COD_FORMA_PAGTO_A_VISTA = "1",
#        ///     COD_FORMA_PAGTO_PARCELADO_CARTAO = "2",
#        ///     COD_FORMA_PAGTO_PARCELA_UNICA = "5",
#        /// <hr />
#        /// </summary>
#        [Required]
#        public short Tipo_Parcelamento { get; set; }//Tipo da forma de pagto
#
#        public decimal? C_pu_valor { get; set; }
#
#        public int? C_pc_qtde { get; set; }
#        public decimal? C_pc_valor { get; set; }

@mytag
Scenario: Tipo_Parcelamento
	Given Fazer esta validação
