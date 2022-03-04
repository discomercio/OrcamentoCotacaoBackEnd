using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Produto.Dados
{
    public class ProdutoDados
    {

        [MaxLength(4)]
        public string Fabricante { get; set; }

        [MaxLength(30)]
        public string Fabricante_Nome { get; set; }

        [MaxLength(8)]
        public string Produto { get; set; }

        [MaxLength(4000)]
        public string Descricao_html { get; set; }
        
        [MaxLength(120)]
        public string Descricao { get; set; }

        public decimal? Preco_lista { get; set; }

        public int Estoque { get; set; }

        [MaxLength(2000)]
        public string Alertas { get; set; }
        public short? Qtde_Max_Venda { get; set; }

        public float? Desc_Max { get; set; }


        public CoeficienteDados CoeficienteRelativo { get; set; }

        public decimal GetPrecoListaComCoeficiente()
        {
            if (CoeficienteRelativo == null)
            {
                return (decimal)Preco_lista;
            }

            return Convert.ToDecimal(CoeficienteRelativo.Coeficiente) * Preco_lista.GetValueOrDefault();
        }
    }
}
        
        

    

    

    
