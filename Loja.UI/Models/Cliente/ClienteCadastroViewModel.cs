using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loja.Bll.Dto.ClienteDto;
using Loja.Bll.Dto.CepDto;
using Loja.UI.Models.Cep;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Loja.UI.Models.Cliente
{
    public class ClienteCadastroViewModel
    {
        public DadosClienteCadastroDto? DadosCliente { get; set; }
        public List<RefBancariaDtoCliente>? RefBancaria { get; set; }
        public List<RefComercialDtoCliente>? RefComercial { get; set; }
        public SelectList? LstContribuinte { get; set; }
        public SelectList? LstIndicadores { get; set; }
        public SelectList? LstProdutoRural { get; set; }
        public EnderecoEntregaDtoClienteCadastro? EndEntrega { get; set; }
        public List<EnderecoEntregaJustificativaDto>? EndJustificativa { get; set; }
        public CepViewModel? Cep { get; set; }
        public Boolean PermiteEdicao { get; set; }
        public SelectList? LstComboBanco { get; set; }
    }
}
