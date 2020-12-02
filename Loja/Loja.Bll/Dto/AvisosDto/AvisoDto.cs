using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.AvisosDto
{
    public class AvisoDto
    {
        public string Id { get; set; }
        public string Mensagem { get; set; }
        public string Usuario { get; set; }
        public string Destinatario { get; set; }
        public DateTime Dt_ult_atualizacao { get; set; }


        public static List<AvisoDto> AvisoDto_De_AvisoDados(List<Avisos.Dados.AvisoDados> lstAvisoDados)
        {
            if (lstAvisoDados == null) return null;

            List<AvisoDto> lstDto = new List<AvisoDto>();

            foreach (var i in lstAvisoDados)
            {
                AvisoDto dto = new AvisoDto
                {
                    Id = i.Id,
                    Usuario = i.Usuario,
                    Mensagem = i.Mensagem,
                    Destinatario = i.Destinatario,
                    Dt_ult_atualizacao = i.Dt_ult_atualizacao
                };

                lstDto.Add(dto);
            }

            return lstDto;
        }
    }
}
