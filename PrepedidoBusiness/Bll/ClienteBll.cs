using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PrepedidoBusiness;
using System.Linq;
using PrepedidoBusiness.Dtos.ClienteCadastro;
using PrepedidoBusiness.Dto.ClienteCadastro.Referencias;
using Microsoft.EntityFrameworkCore;
using PrepedidoBusiness.Dto.ClienteCadastro;

namespace PrepedidoBusiness.Bll
{
    public class ClienteBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public ClienteBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<List<string>> AtualizarClienteParcial(string apelido, DadosClienteCadastroDto dadosClienteCadastroDto)
        {
            /*
             * somente os seguintes campos serão atualizados:
             * produtor rural
             * inscrição estadual
             * tipo de contibuinte ICMS
             * */

            
            //afazer: rfazer a rotina
            //afazer: validar IE conforme estadol
            //afazer: deve ter um log com o apelido do orcamentista
            //para teste
            var ret = new List<string>();
            //ret.Add("Algum erro 1.");
            //ret.Add("Algum erro 2.");
            return ret;
        }


        public async Task<ClienteCadastroDto> BuscarCliente(string cpf_cnpj)
        {
            var db = contextoProvider.GetContexto();

            var dadosCliente = db.Tclientes.Where(r => r.Cnpj_Cpf == cpf_cnpj)
                .FirstOrDefault();
            if (dadosCliente == null)
                return null;

            //afazer: Montar os 4 dto's para retornar para tela de cliente
            var dadosClienteTask = ObterDadosClienteCadastro(dadosCliente);
            var refBancariaTask = ObterReferenciaBancaria(dadosCliente);
            var refComercialTask = ObterReferenciaComenrcial(dadosCliente);

            ClienteCadastroDto cliente = new ClienteCadastroDto
            {
                DadosCliente = await dadosClienteTask,
                RefBancaria = await refBancariaTask,
                RefComercial = await refComercialTask
            };

            return await Task.FromResult(cliente);
        }

        public async Task<IEnumerable<ListaBancoDto>> ListaBancosCombo()
        {
            var db = contextoProvider.GetContexto();

            var bancos = from c in db.Tbancos
                         orderby c.Codigo
                         select new ListaBancoDto
                         {
                             Codigo = c.Codigo,
                             Descricao = c.Descricao
                         };

            return bancos;
        }

        public async Task<DadosClienteCadastroDto> ObterDadosClienteCadastro(Tcliente cli)
        {
            DadosClienteCadastroDto dados = new DadosClienteCadastroDto
            {
                Id = cli.Id,
                Cnpj_Cpf = cli.Cnpj_Cpf,
                Rg = cli.Rg,
                Ie = cli.Ie,
                Contribuinte_Icms_Status = cli.Contribuinte_Icms_Status,
                Tipo = cli.Tipo,
                Observacao_Filiacao = cli.Filiacao,
                Nascimento = cli.Dt_Nasc,
                Sexo = cli.Sexo,
                Nome = cli.Nome,
                ProdutorRural = cli.Produtor_Rural_Status,
                DddResidencial = cli.Ddd_Res,
                TelefoneResidencial = cli.Tel_Res,
                DddComercial = cli.Ddd_Com,
                TelComercial = cli.Tel_Com,
                Ramal = cli.Ramal_Com,
                DddCelular = cli.Ddd_Cel,
                Celular = cli.Tel_Cel,
                Obs = cli.Obs_crediticias,
                Email = cli.Email,
                EmailXml = cli.Email_Xml,
                Endereco = cli.Endereco,
                Numero = cli.Endereco_Numero,
                Complemento = cli.Endereco_Complemento,
                Bairro = cli.Bairro,
                Cidade = cli.Cidade,
                Uf = cli.Uf,
                Cep = cli.Cep,
                Contato = cli.Contato
            };

            return dados;
        }

        private async Task<RefBancariaDtoCliente> ObterReferenciaBancaria(Tcliente cli)
        {
            var db = contextoProvider.GetContexto();

            var rBanco = from c in db.TclienteRefBancarias
                         where c.Id_Cliente == cli.Id
                         orderby c.Ordem
                         select c;

            RefBancariaDtoCliente refBanco = new RefBancariaDtoCliente
            {
                Banco = await rBanco.Select(r => r.Banco).FirstOrDefaultAsync(),
                Agencia = await rBanco.Select(r => r.Agencia).FirstOrDefaultAsync(),
                Conta = await rBanco.Select(r => r.Conta).FirstOrDefaultAsync(),
                Contato = await rBanco.Select(r => r.Contato).FirstOrDefaultAsync(),
                Ddd = await rBanco.Select(r => r.Ddd).FirstOrDefaultAsync(),
                Telefone = await rBanco.Select(r => r.Telefone).FirstOrDefaultAsync()
            };

            return refBanco;
        }

        private async Task<RefComercialDtoCliente> ObterReferenciaComenrcial(Tcliente cli)
        {
            var db = contextoProvider.GetContexto();

            var rComercial = from c in db.TclienteRefComercials
                             where c.Id_Cliente == cli.Id
                             orderby c.Ordem
                             select c;

            RefComercialDtoCliente rCom = new RefComercialDtoCliente
            {
                Nome_Empresa = await rComercial.Select(r => r.Nome_Empresa).FirstOrDefaultAsync(),
                Contato = await rComercial.Select(r => r.Contato).FirstOrDefaultAsync(),
                Ddd = await rComercial.Select(r => r.Ddd).FirstOrDefaultAsync(),
                Telefone = await rComercial.Select(r => r.Telefone).FirstOrDefaultAsync()
            };

            return rCom;
        }


    }
}
