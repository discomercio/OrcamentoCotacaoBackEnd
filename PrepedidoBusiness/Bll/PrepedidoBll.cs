using PrepedidoBusiness;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using System.Linq;
using InfraBanco.Modelos;
using PrepedidoBusiness.Utils;
using PrepedidoBusiness.Dtos.Prepedido;
using PrepedidoBusiness.Dtos.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Dtos.ClienteCadastro;
using InfraBanco.Constantes;

namespace PrepedidoBusiness.Bll
{
    public class PrepedidoBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public PrepedidoBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<string>> ListarNumerosPrepedidosCombo(string orcamentista)
        {
            //toda vez precisamos de uma nova conexao para os casos em que houver transacao
            var db = contextoProvider.GetContexto();
            var lista = from r in db.Torcamentos
                        where r.Orcamentista == orcamentista &&
                              r.St_Orcamento != "CAN"
                        orderby r.Orcamento
                        select r.Orcamento;
            var res = lista.AsEnumerable();
            return await Task.FromResult(res);
        }

        public async Task<IEnumerable<string>> ListarCpfCnpjPrepedidosCombo(string apelido)
        {
            var db = contextoProvider.GetContexto();

            var lista = (from c in db.Torcamentos.Include(r => r.Tcliente)
                         where c.Orcamentista == apelido &&
                               c.St_Orcamento != "CAN"
                         orderby c.Tcliente.Cnpj_Cpf
                         select c.Tcliente.Cnpj_Cpf).Distinct();

            var ret = await lista.Distinct().ToListAsync();
            List<string> cpfCnpjFormat = new List<string>();

            foreach (string cpf in ret)
            {
                cpfCnpjFormat.Add(Util.FormatCpf_Cnpj_Ie(cpf));
            }

            return cpfCnpjFormat;
        }

        public enum TipoBuscaPrepedido
        {
            Todos = 0, NaoViraramPedido = 1, SomenteViraramPedido = 2
        }
        public async Task<IEnumerable<PrepedidosCadastradosDtoPrepedido>> ListarPrePedidos(string apelido, TipoBuscaPrepedido tipoBusca,
            string clienteBusca, string numeroPrePedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            //usamos a mesma lógica de PedidoBll.ListarPedidos:
            /*
             * se fizeram a busca por algum CPF ou CNPJ ou pedido e não achamos nada, removemos o filtro de datas
             * para não aparcer para o usuário que não tem nenhum registro
             * */
            var ret = await ListarPrePedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPrePedido, dataInicial, dataFinal);
            //se tiver algum registro, retorna imediatamente
            if (ret.Count() > 0)
                return ret;

            if (String.IsNullOrEmpty(clienteBusca) && String.IsNullOrEmpty(numeroPrePedido))
                return ret;

            //busca sem datas
            ret = await ListarPrePedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPrePedido, null, null);
            if (ret.Count() > 0)
                return ret;

            //ainda não achamos nada? então faz a busca sem filtrar por tipo
            ret = await ListarPrePedidosFiltroEstrito(apelido, TipoBuscaPrepedido.Todos, clienteBusca, numeroPrePedido, null, null);
            return ret;

        }

        //a busca sem malabarismos para econtrar algum registro
        public async Task<IEnumerable<PrepedidosCadastradosDtoPrepedido>> ListarPrePedidosFiltroEstrito(string apelido, TipoBuscaPrepedido tipoBusca,
                string clienteBusca, string numeroPrePedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            var db = contextoProvider.GetContexto();

            var lst = db.Torcamentos.
                Where(r => r.Orcamentista == apelido);

            //filtro conforme o tipo do prepedido
            switch (tipoBusca)
            {
                case TipoBuscaPrepedido.Todos:
                    lst = lst.Where(r => r.St_Orcamento != "CAN");
                    break;
                case TipoBuscaPrepedido.NaoViraramPedido:
                    lst = lst.Where(r => (r.St_Orc_Virou_Pedido == 0) && (r.St_Orcamento != "CAN"));
                    break;
                case TipoBuscaPrepedido.SomenteViraramPedido:
                    lst = lst.Where(c => c.St_Orc_Virou_Pedido == 1);
                    break;
            }
            if (!string.IsNullOrEmpty(clienteBusca))
                lst = lst.Where(r => r.Tcliente.Cnpj_Cpf.Contains(clienteBusca));
            if (!string.IsNullOrEmpty(numeroPrePedido))
                lst = lst.Where(r => r.Orcamento.Contains(numeroPrePedido));
            if (dataInicial.HasValue)
                lst = lst.Where(r => r.Data >= dataInicial.Value);
            if (dataFinal.HasValue)
                lst = lst.Where(r => r.Data <= dataFinal.Value);

            //COLOCAR O STATUS DO PEDIDO PARA PREPEDIDOS QUE VIRARAM PEDIDOS
            var lstfinal = lst.Select(r => new PrepedidosCadastradosDtoPrepedido
            {
                Status = r.St_Orc_Virou_Pedido == 1 ? "Pré-Pedido - Com Pedido" : "Pré-Pedido - Sem Pedido",
                DataPrePedido = r.Data_Hora,
                NumeroPrepedido = r.Orcamento,
                NomeCliente = r.Tcliente.Nome,
                ValoTotal = r.Vl_Total
            }).OrderByDescending(r => r.DataPrePedido);

            var res = lstfinal.AsEnumerable();
            return await Task.FromResult(res);
        }

        public async Task<bool> RemoverPrePedido(string numeroPrePedido, string apelido)
        {
            var db = contextoProvider.GetContexto();

            Torcamento prePedido = db.Torcamentos.
                Where(
                        r => r.Orcamentista == apelido &&
                        r.Orcamento == numeroPrePedido &&
                        (r.St_Orcamento == "" || r.St_Orcamento == null) &&
                        r.St_Orc_Virou_Pedido == 0
                      ).SingleOrDefault();

            if (!string.IsNullOrEmpty(prePedido.ToString()))
            {
                prePedido.St_Orcamento = "CAN";
                prePedido.Cancelado_Data = DateTime.Now;
                prePedido.Cancelado_Usuario = apelido;
                await db.SaveChangesAsync();
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<PrePedidoDto> BuscarPrePedido(string apelido, string numPrePedido)
        {
            var db = contextoProvider.GetContexto();

            var prepedido = from c in db.Torcamentos
                            where c.Orcamentista == apelido && c.Orcamento == numPrePedido
                            select c;

            Torcamento pp = prepedido.FirstOrDefault();

            if (pp == null)
                return null;

            var cadastroClienteTask = ObterDadosCliente(pp.Loja, pp.Orcamentista, pp.Vendedor, pp.Id_Cliente);
            //ClienteBll cl = new ClienteBll(contextoProvider);
            //var cadastroClienteTask = cl.BuscarCliente(pp.)

            PrePedidoDto prepedidoDto = new PrePedidoDto();
            return await Task.FromResult(prepedidoDto);
        }

        private async Task<DadosClienteCadastroDto> ObterDadosCliente(string loja, string indicador_orcamentista, string vendedor, string idCliente)
        {
            var dadosCliente = from c in contextoProvider.GetContexto().Tclientes
                               where c.Id == idCliente
                               select c;
            var cli = await dadosCliente.FirstOrDefaultAsync();
            DadosClienteCadastroDto cadastroCliente = new DadosClienteCadastroDto
            {
                Loja = loja,
                Indicador_Orcamentista = indicador_orcamentista,
                Vendedor = vendedor,
                Id = cli.Id,
                Cnpj_Cpf = Util.FormatCpf_Cnpj_Ie(cli.Cnpj_Cpf),
                Rg = Util.FormatCpf_Cnpj_Ie(cli.Rg),
                Ie = Util.FormatCpf_Cnpj_Ie(cli.Ie),
                Tipo = cli.Tipo,
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
                TelComercial2 = cli.Tel_Com_2,
                DddComercial2 = cli.Ddd_Com_2,
                Ramal2 = cli.Ramal_Com_2,
                Obs = cli.Obs_crediticias,
                Email = cli.Email,
                Endereco = cli.Endereco,
                Numero = cli.Endereco_Numero,
                Bairro = cli.Bairro,
                Cidade = cli.Cidade,
                Uf = cli.Uf,
                Cep = cli.Cep
            };
            return cadastroCliente;
        }

        private async Task<EnderecoEntregaDtoClienteCadastro> ObterEnderecoEntrega(Torcamento p)
        {
            EnderecoEntregaDtoClienteCadastro enderecoEntrega = new EnderecoEntregaDtoClienteCadastro
            {
                EndEtg_endereco = p.EndEtg_Endereco,
                EndEtg_endereco_numero = p.EndEtg_Endereco_Numero,
                EndEtg_endereco_complemento = p.EndEtg_Endereco_Complemento,
                EndEtg_bairro = p.EndEtg_Bairro,
                EndEtg_cidade = p.EndEtg_Cidade,
                EndEtg_uf = p.EndEtg_UF,
                EndEtg_cep = p.EndEtg_CEP,
                EndEtg_cod_justificativa = await ObterDescricao_Cod(Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA, p.EndEtg_Cod_Justificativa)
            };

            return enderecoEntrega;
        }

        private async Task<string> ObterDescricao_Cod(string grupo, string cod)
        {
            var db = contextoProvider.GetContexto();

            var desc = from c in db.TcodigoDescricaos
                       where c.Grupo == grupo && c.Codigo == cod
                       select c.Descricao;

            string result = await desc.FirstOrDefaultAsync();

            if (result == null || result == "")
                return "Código não cadastrado (" + cod + ")";

            return result;
        }

    }
}
