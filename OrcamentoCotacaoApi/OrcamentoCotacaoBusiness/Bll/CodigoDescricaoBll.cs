using InfraBanco.Modelos.Filtros;
using InfraBanco.Modelos;
using OrcamentoCotacaoBusiness.Models.Request.CodigoDescricao;
using OrcamentoCotacaoBusiness.Models.Response.CodigoDescricao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class CodigoDescricaoBll
    {
        private readonly CodigoDescricao.CodigoDescricaoBll codigoDescricaoBll;

        public CodigoDescricaoBll(CodigoDescricao.CodigoDescricaoBll codigoDescricaoBll)
        {
            this.codigoDescricaoBll = codigoDescricaoBll;
        }

        public ListaCodigoDescricaoResponse StatusPorFiltro(CodigoDescricaoRequest request)
        {
            try
            {
                var response = new ListaCodigoDescricaoResponse();
                response.ListaCodigoDescricao = new List<CodigoDescricaoResponse>();

                var retorno = codigoDescricaoBll.PorFiltro(new TcodigoDescricaoFiltro() { Grupo = request.Grupo, Codigo = request.Codigo });
                if (retorno == null)
                {
                    response.Mensagem = "Nenhum código encontrado!";
                    return response;
                }

                foreach (var item in retorno)
                {
                    var ret = new CodigoDescricaoResponse();
                    ret.Codigo = item.Codigo;
                    ret.Descricao = item.Descricao;
                    ret.Ordenacao = item.Ordenacao;
                    ret.St_Inativo = item.St_Inativo;
                    response.ListaCodigoDescricao.Add(ret);
                }

                response.Sucesso = true;
                return response;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
