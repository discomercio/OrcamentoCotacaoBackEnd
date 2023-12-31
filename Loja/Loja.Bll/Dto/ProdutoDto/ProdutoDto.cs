﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.ProdutoDto
{
    public class ProdutoDto
    {
        public string Fabricante { get; set; }
        public string Fabricante_Nome { get; set; }
        public string Produto { get; set; }
        public string Descricao_html { get; set; }
        public decimal? Preco_lista { get; set; }
        public int Estoque { get; set; }
        public string Alertas { get; set; }
        public short? Qtde_Max_Venda { get; set; }
        public short? QtdeSolicitada { get; set; }
        public List<int> Lst_empresa_selecionada { get; set; }
    }
}
