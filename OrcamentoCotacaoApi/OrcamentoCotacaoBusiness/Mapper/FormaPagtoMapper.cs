using AutoMapper;
using OrcamentoCotacaoBusiness.Models.Request.Orcamento;
using OrcamentoCotacaoBusiness.Models.Response;

namespace OrcamentoCotacaoBusiness.Mapper
{
    public class FormaPagtoMapper : Profile
    {
        public FormaPagtoMapper()
        {
            CreateMap<FormaPagtoCriacaoResponseViewModel, CadastroOrcamentoOpcaoFormaPagtoRequest>();
            CreateMap<AtualizarOrcamentoOpcaoFormaPagtoRequest, CadastroOrcamentoOpcaoFormaPagtoRequest>();
        }
    }
}
