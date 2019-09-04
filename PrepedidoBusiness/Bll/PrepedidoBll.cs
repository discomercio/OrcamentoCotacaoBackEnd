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
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;

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
            //parateste
            apelido = "PEDREIRA";
            numPrePedido = "214289Z";

            var db = contextoProvider.GetContexto();

            var prepedido = from c in db.Torcamentos
                            where c.Orcamentista == apelido && c.Orcamento == numPrePedido
                            select c;


            Torcamento pp = prepedido.FirstOrDefault();

            if (pp == null)
                return null;

            if (pp.St_Orc_Virou_Pedido == 1)
            {
                var pedido = from c in db.Tpedidos
                             where c.Orcamento == numPrePedido
                             select c.Pedido;

                PrePedidoDto prepedidoPedido = new PrePedidoDto
                {
                    St_Orc_Virou_Pedido = true,
                    NumeroPedido = pedido.Select(r => r.ToString()).FirstOrDefault()
                };
                return prepedidoPedido;
            }
            
            var cadastroClienteTask = ObterDadosCliente(pp.Loja, pp.Orcamentista, pp.Vendedor, pp.Id_Cliente);
            var enderecoEntregaTask = ObterEnderecoEntrega(pp);
            var lstProdutoTask = await ObterProdutos(pp);

            var vltotalRa = lstProdutoTask.Select(r => r.VlTotalRA).Sum();
            var totalDestePedidoComRa = lstProdutoTask.Select(r => r.TotalItemRA).Sum();
            var totalDestePedido = lstProdutoTask.Select(r => r.TotalItem).Sum();

            PrePedidoDto prepedidoDto = new PrePedidoDto
            {
                NumeroPrePedido = pp.Orcamento,
                DataHoraPedido = Convert.ToString(pp.Data + pp.Hora),
                DadosCliente = await cadastroClienteTask,
                EnderecoEntrega = await enderecoEntregaTask,
                ListaProdutos = lstProdutoTask.ToList(),
                TotalFamiliaParcelaRA = vltotalRa,
                PermiteRAStatus = pp.Permite_RA_Status,
                CorTotalFamiliaRA = vltotalRa > 0 ? "green" : "red",
                PercRT = pp.Perc_RT,
                ValorTotalDestePedidoComRA = totalDestePedidoComRa,
                VlTotalDestePedido = totalDestePedido,
                DetalhesPrepedido = ObterDetalhesPrePedido(pp),
                FormaPagto = ObterFormaPagto(pp).ToList()
            };

            return await Task.FromResult(prepedidoDto);
        }

        private DetalhesDtoPrepedido ObterDetalhesPrePedido(Torcamento torcamento)
        {
            DetalhesDtoPrepedido detail = new DetalhesDtoPrepedido
            {
                Observacoes = torcamento.Obs_1,
                NumeroNF = torcamento.Obs_2,
                EntregaImediata = Convert.ToString(torcamento.St_Etg_Imediata) == Constantes.COD_ETG_IMEDIATA_NAO ?
                "NÃO" : "SIM " + torcamento.Etg_Imediata_Usuario +
                " em " + torcamento.Etg_Imediata_Data?.ToString("dd/MM/yyyy"),
                BemDeUso_Consumo = Convert.ToString(torcamento.StBemUsoConsumo) == Constantes.COD_ST_BEM_USO_CONSUMO_NAO ?
                "NÃO" : "SIM",
                InstaladorInstala = Convert.ToString(torcamento.InstaladorInstalaStatus) == Constantes.COD_INSTALADOR_INSTALA_NAO ?
                "NÃO" : "SIM",
                GarantiaIndicador = Convert.ToString(torcamento.GarantiaIndicadorStatus) ==
                Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO ?
                "NÃO" : "SIM"
            };

            return detail;
        }

        private IEnumerable<string> ObterFormaPagto(Torcamento torcamento)
        {
            List<string> lista = new List<string>();

            switch (Convert.ToString(torcamento.Tipo_Parcelamento))
            {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    lista.Add("À vista (" + Util.OpcaoFormaPagto(Convert.ToString(torcamento.Av_Forma_Pagto)) + ")");
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    lista.Add(String.Format("Parcela Única: " + Constantes.SIMBOLO_MONETARIO + " {0:c2} (" +
                        Util.OpcaoFormaPagto(Convert.ToString(torcamento.Pu_Forma_Pagto)) + ") vencendo após " + torcamento.Pu_Vencto_Apos, torcamento.Pu_Valor));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    lista.Add(String.Format("Parcelado no Cartão (internet) em " + torcamento.Pc_Qtde_Parcelas + " X " +
                        Constantes.SIMBOLO_MONETARIO + " {0:c2}" , torcamento.Pc_Valor_Parcela));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    lista.Add(String.Format("Parcelado no Cartão (maquineta) em " + torcamento.Pc_Maquineta_Qtde_Parcelas + " X " +
                        Constantes.SIMBOLO_MONETARIO + " {0:c2}" , torcamento.Pc_Maquineta_Valor_Parcela));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    lista.Add(String.Format("Entrada " + Constantes.SIMBOLO_MONETARIO + "{0:c2} (" +
                        Util.OpcaoFormaPagto(Convert.ToString(torcamento.Pce_Forma_Pagto_Entrada)) + ")", torcamento.Pce_Entrada_Valor));
                    lista.Add(String.Format("Prestações: " + torcamento.Pce_Prestacao_Qtde + " X " + Constantes.SIMBOLO_MONETARIO + " {0:c2}" +
                        " (" + Util.OpcaoFormaPagto(Convert.ToString(torcamento.Pce_Forma_Pagto_Prestacao)) + ") vencendo a cada " +
                        torcamento.Pce_Prestacao_Periodo + " dias", torcamento.Pce_Prestacao_Valor));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    lista.Add(String.Format("1ª Prestação: " + Constantes.SIMBOLO_MONETARIO + " {0:c2} (" +
                        Util.OpcaoFormaPagto(Convert.ToString(torcamento.Pse_Forma_Pagto_Prim_Prest)) + ") vencendo após " + torcamento.Pse_Prim_Prest_Apos + 
                        " dias", torcamento.Pse_Prim_Prest_Valor));
                    lista.Add(String.Format("Demais Prestações: " + torcamento.Pse_Demais_Prest_Qtde + " X " + Constantes.SIMBOLO_MONETARIO + " {0:c2}" +
                        " (" + Util.OpcaoFormaPagto(Convert.ToString(torcamento.Pse_Forma_Pagto_Demais_Prest)) + ") vencendo a cada " +
                        torcamento.Pse_Demais_Prest_Periodo + " dias", torcamento.Pse_Demais_Prest_Valor));
                    break;
            }

            return lista;
        }

        private async Task<IEnumerable<PrepedidoProdutoDtoPrepedido>> ObterProdutos(Torcamento orc)
        {
            var db = contextoProvider.GetContexto();

            var produtos = from c in db.TorcamentoItems
                           where c.Orcamento == orc.Orcamento
                           orderby c.Sequencia
                           select c;

            List<PrepedidoProdutoDtoPrepedido> listaProduto = new List<PrepedidoProdutoDtoPrepedido>();

            foreach (var p in produtos)
            {
                PrepedidoProdutoDtoPrepedido produtoPrepedido = new PrepedidoProdutoDtoPrepedido
                {
                    Fabricante = p.Fabricante,
                    NumProduto = p.Produto,
                    Descricao = p.Descricao_Html,
                    Obs = p.Obs,
                    Qtde = p.Qtde,
                    Permite_Ra_Status = orc.Permite_RA_Status,
                    BlnTemRa = p.Preco_NF != p.Preco_Venda ? true : false,
                    Preco = p.Preco_NF,
                    VlLista = p.Preco_Lista,
                    Desconto = p.Desc_Dado,
                    VlUnitario = p.Preco_Venda,
                    VlTotalRA = (decimal)(p.Qtde * (p.Preco_NF - p.Preco_Venda)),
                    Comissao = orc.Perc_RT,
                    TotalItemRA = p.Qtde * p.Preco_NF,
                    TotalItem = p.Qtde * p.Preco_Venda
                };

                listaProduto.Add(produtoPrepedido);
            }

            return await Task.FromResult(listaProduto);
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
                TelComercial2 = cli.Tel_Com_2,
                DddComercial2 = cli.Ddd_Com_2,
                Ramal2 = cli.Ramal_Com_2,
                Email = cli.Email,
                EmailXml = cli.Email_Xml,
                Endereco = cli.Endereco,
                Numero = cli.Endereco_Numero,
                Bairro = cli.Bairro,
                Cidade = cli.Cidade,
                Uf = cli.Uf,
                Cep = cli.Cep,
                Contato = cli.Contato
            };
            return cadastroCliente;
        }

        private async Task<EnderecoEntregaDtoClienteCadastro> ObterEnderecoEntrega(Torcamento p)
        {
            EnderecoEntregaDtoClienteCadastro enderecoEntrega = new EnderecoEntregaDtoClienteCadastro();

            if (p.St_End_Entrega == 1)
            {
                enderecoEntrega.EndEtg_endereco = p.EndEtg_Endereco;
                enderecoEntrega.EndEtg_endereco_numero = p.EndEtg_Endereco_Numero;
                enderecoEntrega.EndEtg_endereco_complemento = p.EndEtg_Endereco_Complemento;
                enderecoEntrega.EndEtg_bairro = p.EndEtg_Bairro;
                enderecoEntrega.EndEtg_cidade = p.EndEtg_Cidade;
                enderecoEntrega.EndEtg_uf = p.EndEtg_UF;
                enderecoEntrega.EndEtg_cep = p.EndEtg_CEP;
                enderecoEntrega.EndEtg_cod_justificativa = await ObterDescricao_Cod(Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA, p.EndEtg_Cod_Justificativa);
            }
            else
                return null;

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
