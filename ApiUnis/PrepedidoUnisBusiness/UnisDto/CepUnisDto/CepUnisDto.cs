﻿using Cep.Dados;
using System.ComponentModel.DataAnnotations;

namespace PrepedidoUnisBusiness.UnisDto.CepUnisDto
{
    public class CepUnisDto
    {
        [MaxLength(8)]
        public string Cep { get; set; }

        [MaxLength(2)]
        public string Uf { get; set; }

        [MaxLength(60)]
        public string Cidade { get; set; }

        [MaxLength(72)]
        public string Bairro { get; set; }

        [MaxLength(80)]
        public string Endereco { get; set; }

        [MaxLength(100)]
        public string LogradouroComplemento { get; set; }

        public static CepUnisDto CepUnisDtoDeCepDados(CepDados cepDto)
        {
            CepUnisDto cepUnis = new CepUnisDto()
            {
                Cep = cepDto.Cep,
                Uf = cepDto.Uf,
                Cidade = cepDto.Cidade,
                Bairro = cepDto.Bairro,
                Endereco = cepDto.Endereco,
                LogradouroComplemento = cepDto.LogradouroComplemento
            };

            return cepUnis;
        }
    }
}
