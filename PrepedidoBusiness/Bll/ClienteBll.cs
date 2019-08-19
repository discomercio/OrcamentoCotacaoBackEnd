using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PrepedidoBusiness;
using System.Linq;
using PrepedidoBusiness.Dtos.ClienteCadastro;

namespace PrepedidoBusiness.Bll
{
    public class ClienteBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public ClienteBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        

        public async Task<DadosClienteCadastroDto> BuscarCliente(string cpf_cnpj)
        {
            var db = contextoProvider.GetContexto();

            var dadosCliente = db.Tclientes.Where(r => r.Cnpj_Cpf == cpf_cnpj)
                .FirstOrDefault();
            
            DadosClienteCadastroDto dados = new DadosClienteCadastroDto()
            {
                
                Id = dadosCliente.Id,
                Cnpj_Cpf = dadosCliente.Cnpj_Cpf,
                Rg = dadosCliente.Rg,
                Nascimento = dadosCliente.Dt_Nasc,
                Sexo = dadosCliente.Sexo,
                Nome = dadosCliente.Nome,
                ProdutorRural = dadosCliente.Produtor_Rural_Status,
                DddResidencial = dadosCliente.Ddd_Res,
                TelefoneResidencial = dadosCliente.Tel_Res,
                DddComercial = dadosCliente.Ddd_Com,
                TelComercial = dadosCliente.Tel_Com,
                Ramal = dadosCliente.Ramal_Com,
                DddCelular = dadosCliente.Ddd_Cel,
                Celular = dadosCliente.Tel_Cel,
                Obs = dadosCliente.Obs_crediticias,
                Email = dadosCliente.Email,
                Endereco = dadosCliente.Endereco,
                Numero = dadosCliente.Endereco_Numero,
                Complemento = dadosCliente.Endereco_Complemento,
                Bairro = dadosCliente.Bairro,
                Cidade = dadosCliente.Cidade,
                Uf = dadosCliente.Uf,
                Cep = dadosCliente.Cep
            }; 

            return await Task.FromResult(dados);

        }

    }
}
