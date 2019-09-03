using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using PrepedidoBusiness.Dtos.Pedido;
using PrepedidoBusiness.Utils;
using PrepedidoBusiness.Dtos.ClienteCadastro;
using InfraBanco.Modelos;
using PrepedidoBusiness.Dto.Pedido.DetalhesPedido;
using Microsoft.EntityFrameworkCore.Internal;
using InfraBanco.Constantes;
using System.Collections;

namespace PrepedidoBusiness.Bll
{
    public class PedidoBll
    {
        private readonly InfraBanco.ContextoProvider contextoProvider;

        public PedidoBll(InfraBanco.ContextoProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }

        public async Task<IEnumerable<string>> ListarNumerosPedidoCombo(string apelido)
        {
            var db = contextoProvider.GetContexto();

            var lista = from p in db.Tpedidos
                        where p.Orcamentista == apelido
                        orderby p.Pedido
                        select p.Pedido;

            var res = lista.AsEnumerable();
            return await Task.FromResult(res);
        }

        public enum TipoBuscaPedido
        {
            Todos = 0, PedidosEncerrados = 1, PedidosEmAndamento = 2
        }


        public async Task<IEnumerable<PedidoDtoPedido>> ListarPedidos(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            //fazemos a busca
            var ret = await ListarPedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPedido, dataInicial, dataFinal);

            //se tiver algum registro, retorna imediatamente
            if (ret.Count() > 0)
                return ret;

            /*
             * se fizeram a busca por algum CPF ou CNPJ ou pedido e não achamos nada, removemos o filtro de datas
             * para não aparcer para o usuário que não tem nenhum registro
             * */
            if (String.IsNullOrEmpty(clienteBusca) && String.IsNullOrEmpty(numeroPedido))
                return ret;

            //busca sem datas
            ret = await ListarPedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPedido, null, null);
            if (ret.Count() > 0)
                return ret;

            //ainda não achamos nada? então faz a busca sem filtrar por tipo
            ret = await ListarPedidosFiltroEstrito(apelido, TipoBuscaPedido.Todos, clienteBusca, numeroPedido, null, null);
            return ret;
        }

        //a busca sem malabarismos para econtrar algum registro
        private async Task<IEnumerable<PedidoDtoPedido>> ListarPedidosFiltroEstrito(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            var db = contextoProvider.GetContexto();

            var lista = db.Tpedidos.Include(r => r.Tcliente).
                Where(r => r.Indicador == apelido);

            switch (tipoBusca)
            {
                case TipoBuscaPedido.Todos:
                    //SE TIVER QUE INCLUIR OS PEDIDO CANCELADOS É SÓ DESCOMENTAR A LINHA ABAIXO
                    //lista = lista.Where(r => r.St_Entrega != "CAN");
                    break;
                case TipoBuscaPedido.PedidosEncerrados:
                    lista = lista.Where(r => r.St_Entrega == "ETG" || r.St_Entrega == "CAN");
                    break;
                case TipoBuscaPedido.PedidosEmAndamento:
                    lista = lista.Where(r => r.St_Entrega != "ETG" && r.St_Entrega != "CAN");
                    break;
            }

            if (!string.IsNullOrEmpty(clienteBusca))
                lista = lista.Where(r => r.Tcliente.Cnpj_Cpf.Contains(clienteBusca));
            if (!string.IsNullOrEmpty(numeroPedido))
                lista = lista.Where(r => r.Pedido.Contains(numeroPedido));
            if (dataInicial.HasValue)
                lista = lista.Where(r => r.Data >= dataInicial.Value);
            if (dataFinal.HasValue)
                lista = lista.Where(r => r.Data <= dataFinal.Value);

            var listaFinal = lista.Select(r => new PedidoDtoPedido
            {
                NomeCliente = r.Tcliente.Nome_Iniciais_Em_Maiusculas,
                NumeroPedido = r.Pedido,
                DataPedido = r.Data,
                Status = r.St_Entrega,
                ValorTotal = r.Vl_Total_NF
            }).OrderByDescending(r => r.DataPedido);


            //colocar as mensagens de status
            var listaComStatus = await listaFinal.ToListAsync();
            foreach (var pedido in listaComStatus)
            {
                if (pedido.Status == "ESP")
                    pedido.Status = "Em espera";
                if (pedido.Status == "SPL")
                    pedido.Status = "Split possível";
                if (pedido.Status == "SEP")
                    pedido.Status = "Separar";
                if (pedido.Status == "AET")
                    pedido.Status = "A entregar";
                if (pedido.Status == "ETG")
                    pedido.Status = "Entrega";
                if (pedido.Status == "CAN")
                    pedido.Status = "Cancelado";
            }
            return await Task.FromResult(listaComStatus);
        }

        public async Task<IEnumerable<string>> ListarCpfCnpjPedidosCombo(string apelido)
        {
            var db = contextoProvider.GetContexto();

            var lista = (from c in db.Tpedidos.Include(r => r.Tcliente)
                         where c.Orcamentista == apelido
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
                Obs = cli.Obs_crediticias,
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

        private async Task<EnderecoEntregaDtoClienteCadastro> ObterEnderecoEntrega(Tpedido p)
        {
            EnderecoEntregaDtoClienteCadastro enderecoEntrega = new EnderecoEntregaDtoClienteCadastro
            {
                EndEtg_endereco = p.Endereco_Logradouro,
                EndEtg_endereco_numero = p.EndEtg_Endereco_Numero,
                EndEtg_endereco_complemento = p.EndEtg_Endereco_Complemento,
                EndEtg_bairro = p.EndEtg_Bairro,
                EndEtg_cidade = p.EndEtg_Cidade,
                EndEtg_uf = p.EndEtg_UF,
                EndEtg_cep = p.EndEtg_Cep,
                EndEtg_cod_justificativa = await ObterDescricao_Cod(Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA, p.EndEtg_Cod_Justificativa)
            };

            return enderecoEntrega;
        }

        private async Task<IEnumerable<PedidoProdutosDtoPedido>> ObterProdutos(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var produtosItens = from c in db.TpedidoItems
                                where c.Pedido == numPedido
                                select c;

            List<PedidoProdutosDtoPedido> lstProduto = new List<PedidoProdutosDtoPedido>();

            foreach (var c in produtosItens)
            {
                PedidoProdutosDtoPedido produto = new PedidoProdutosDtoPedido
                {
                    Fabricante = c.Fabricante,
                    NumProduto = c.Produto,
                    Descricao = c.Descricao,
                    Qtde = c.Qtde,
                    Faltando = (short)await VerificarEstoque(numPedido, c.Fabricante, c.Produto),
                    Preco = c.Preco_NF,
                    VlLista = c.Preco_Lista,
                    Desconto = c.Desc_Dado,
                    VlUnitario = c.Preco_Venda,
                    VlTotalItem = c.Qtde * c.Preco_Venda,
                    VlTotalItemComRA = c.Qtde * c.Preco_NF,
                    VlVenda = c.Preco_Venda,
                    VlTotal = c.Qtde * c.Preco_Venda,
                    Comissao = c.Comissao
                };

                lstProduto.Add(produto);
            }

            return lstProduto;
        }


        public async Task<PedidoDto> BuscarPedido(string apelido, string numPedido)
        {
            var db = contextoProvider.GetContexto();

            //parateste
            //numPedido = "094947N-A";
            //apelido = "PEDREIRA";

            var pedido = from c in db.Tpedidos
                         where c.Pedido == numPedido && c.Orcamentista == apelido
                         select c;
            Tpedido p = pedido.FirstOrDefault();
            if (p == null)
                return null;

            var cadastroClienteTask = ObterDadosCliente(p.Loja, p.Indicador, p.Vendedor, p.Id_Cliente);
            var enderecoEntregaTask = ObterEnderecoEntrega(p);
            var lstProdutoTask = ObterProdutos(numPedido);

            var vlTotalDestePedidoComRATask = lstProdutoTask.Result.Select(r => r.VlTotalItemComRA).Sum();
            var vlTotalDestePedidoTask = lstProdutoTask.Result.Select(r => r.VlTotalItem).Sum();

            //buscar valor total de devoluções NF
            var vlDevNf = from c in db.TpedidoItemDevolvidos
                          where c.Pedido.StartsWith(numPedido)
                          select c.Qtde * c.Preco_NF;
            var vl_TotalFamiliaDevolucaoPrecoNFTask = vlDevNf.Select(r => r.Value).SumAsync();

            var vlFamiliaParcelaRATask = CalculaTotalFamiliaRA(numPedido);

            string garantiaIndicadorStatus = Convert.ToString(p.GarantiaIndicadorStatus);
            if (garantiaIndicadorStatus == Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO)
                garantiaIndicadorStatus = "NÃO";
            if (garantiaIndicadorStatus == Constantes.COD_GARANTIA_INDICADOR_STATUS__SIM)
                garantiaIndicadorStatus = "SIM";

            DetalhesNFPedidoDtoPedido detalhesNf = new DetalhesNFPedidoDtoPedido
            {
                Observacoes = p.Obs_1,
                ConstaNaNF = p.Nfe_Texto_Constar,
                XPed = p.Nfe_XPed,
                NumeroNF = p.Obs_2,
                NFSimples = p.Obs_3,
                EntregaImediata = p.St_Etg_Imediata,
                StBemUsoConsumo = p.StBemUsoConsumo,
                InstaladorInstala = p.InstaladorInstalaStatus,
                GarantiaIndicadorStatus = garantiaIndicadorStatus
            };

            //verifica o status da entrega
            DateTime? dataEntrega = new DateTime();
            if (p.St_Entrega == Constantes.ST_ENTREGA_A_ENTREGAR || p.St_Entrega == Constantes.ST_ENTREGA_SEPARAR)
                dataEntrega = p.A_Entregar_Data_Marcada;

            var perdas = BuscarPerdas(numPedido);

            var transportadora = from c in db.Ttransportadoras
                                 where c.Id == p.Transportadora_Id
                                 select c.Nome;
            var TranspNomeTask = transportadora.Select(r => r.ToString()).FirstOrDefaultAsync();

            var lstFormaPgtoTask = ObterFormaPagto(numPedido, apelido);

            var analiseCreditoTask = ObterAnaliseCredito(Convert.ToString(p.Analise_Credito), numPedido, apelido);
            string corAnalise = CorAnaliseCredito(Convert.ToString(p.Analise_Credito));

            string corStatusPagto = CorSatusPagto(p.St_Pagto);

            var saldo_a_pagarTask = CalculaSaldoAPagar(numPedido, await vl_TotalFamiliaDevolucaoPrecoNFTask);

            var TotalPerda = (await perdas).Select(r => r.Valor).Sum();
            var lstOcorrenciaTask = ObterOcorrencias(numPedido);
            var lstBlocNotasDevolucaoTask = BuscarPedidoBlocoNotasDevolucao(numPedido);

            var saldo_a_pagar = await saldo_a_pagarTask;
            if (p.St_Entrega == Constantes.ST_PAGTO_PAGO && saldo_a_pagar > 0)
                saldo_a_pagar = 0;

            DetalhesFormaPagamentos detalhesFormaPagto = new DetalhesFormaPagamentos
            {
                FormaPagto = (await lstFormaPgtoTask).ToList(),
                InfosAnaliseCredito = p.Forma_Pagto,
                StatusPagto = p.St_Pagto,
                CorStatusPagto = corStatusPagto,
                VlTotalFamilia = p.Vl_Total_Familia,
                VlPago = p.Vl_Total_Familia,
                VlDevolucao = await vl_TotalFamiliaDevolucaoPrecoNFTask,
                VlPerdas = TotalPerda,
                SaldoAPagar = saldo_a_pagar,
                AnaliseCredito = await analiseCreditoTask,
                CorAnalise = corAnalise,
                DataColeta = dataEntrega,
                Transportadora = await TranspNomeTask,
                VlFrete = p.Frete_Valor
            };

            PedidoDto DtoPedido = new PedidoDto
            {
                NumeroPedido = numPedido,
                DataHoraPedido = p.Data,
                StatusHoraPedido = await MontarDtoStatuPedido(p),
                DadosCliente = await cadastroClienteTask,
                ListaProdutos = (await ObterProdutos(numPedido)).ToList(),
                TotalFamiliaParcelaRA = await vlFamiliaParcelaRATask,
                PermiteRAStatus = p.Permite_RA_Status,
                OpcaoPossuiRA = p.Opcao_Possui_RA,
                PercRT = p.Perc_RT,
                ValorTotalDestePedidoComRA = vlTotalDestePedidoComRATask,
                VlTotalDestePedido = vlTotalDestePedidoTask,
                DetalhesNF = detalhesNf,
                DetalhesFormaPagto = detalhesFormaPagto,
                ListaProdutoDevolvido = await BuscarProdutosDevolvidos(numPedido),
                ListaPerdas = await BuscarPerdas(numPedido),
                BlocoNotas = await BuscarPedidoBlocoNotas(numPedido),
                EnderecoEntrega = await enderecoEntregaTask,
                ListaOcorrencia = (await lstOcorrenciaTask).ToList(),
                ListaBlocoNotasDevolucao = (await lstBlocNotasDevolucaoTask).ToList()
            };

            return await Task.FromResult(DtoPedido);
        }

        private async Task<StatusPedidoDtoPedido> MontarDtoStatuPedido(Tpedido p)
        {
            StatusPedidoDtoPedido status = new StatusPedidoDtoPedido();

            if (!String.IsNullOrEmpty(p.Pedido_Bs_X_Marketplace))
            {
                status.Status = await ObterDescricao_Cod("PedidoECommerce_Origem", p.Marketplace_codigo_origem) + ":" + p.Pedido_Bs_X_Marketplace;
                status.CorEntrega = CorStatusEntrega(p.St_Entrega);
            }
            else if (!String.IsNullOrEmpty(p.Pedido_Bs_X_Ac))
            {
                status.CorEntrega = "purple";
                status.Pedido_Bs_X_Ac = p.Pedido_Bs_X_Ac;
            }
            else
            {
                status.Status = FormataSatusPedido(p.St_Entrega);
                status.CorEntrega = CorStatusEntrega(p.St_Entrega);
            }

            status.St_Entrega = FormataSatusPedido(p.St_Entrega);            
            status.Entregue_Data = p.Entregue_Data?.ToString("dd/MM/yyyy");
            status.Cancelado_Data = p.Cancelado_Data?.ToString("dd/MM/yyyy");
            status.Pedido_Data = p.Data?.ToString("dd/MM/yyyy");
            status.Recebida_Data = p.PedidoRecebidoData?.ToString("dd/MM/yyyy");


            return await Task.FromResult(status);
        }

        private string CorStatusEntrega(string st_entrega)
        {
            string cor = "black";

            switch(st_entrega)
            {
                case Constantes.ST_ENTREGA_ESPERAR:
                    cor = "deeppink";
                    break;
                case Constantes.ST_ENTREGA_SPLIT_POSSIVEL:
                    cor = "darkorange";
                    break;
                case Constantes.ST_ENTREGA_SEPARAR:
                    cor = "maroon";
                    break;
                case Constantes.ST_ENTREGA_A_ENTREGAR:
                    cor = "blue";
                    break;
                case Constantes.ST_ENTREGA_ENTREGUE:
                    cor = "green";
                    break;
                case Constantes.ST_ENTREGA_CANCELADO:
                    cor = "red";
                    break;
            }

            return cor;
        }

        private string FormataSatusPedido(string status)
        {
            string retorno = "";

            switch (status)
            {
                case "ESP":
                    retorno = "Em espera";
                    break;
                case "SPL":
                    retorno = "Split possível";
                    break;
                case "SEP":
                    retorno = "Separar";
                    break;
                case "AET":
                    retorno = "A entregar";
                    break;
                case "ETG":
                    retorno = "Entrega";
                    break;
                case "CAN":
                    retorno = "Cancelado";
                    break;
            }

            return retorno;
        }

        private string CorSatusPagto(string statusPagto)
        {
            string retorno = "";

            switch (statusPagto)
            {
                case Constantes.ST_PAGTO_PAGO:
                    retorno = "green";
                    break;
                case Constantes.ST_PAGTO_NAO_PAGO:
                    retorno = "red";
                    break;
                case Constantes.ST_PAGTO_PARCIAL:
                    retorno = "deeppink";
                    break;
            }

            return retorno;
        }

        private string CorAnaliseCredito(string codigo)
        {
            if (codigo == null)
                return "";

            string retorno = "";

            switch (codigo)
            {
                case Constantes.COD_AN_CREDITO_PENDENTE:
                    retorno = "red";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_VENDAS:
                    retorno = "red";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_ENDERECO:
                    retorno = "red";
                    break;
                case Constantes.COD_AN_CREDITO_OK:
                    retorno = "green";
                    break;
                case Constantes.COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO:
                    retorno = "darkorange";
                    break;
                case Constantes.COD_AN_CREDITO_OK_DEPOSITO_AGUARDANDO_DESBLOQUEIO:
                    retorno = "darkorange";
                    break;
            }

            return retorno;
        }

        private async Task<IEnumerable<OcorrenciasDtoPedido>> ObterOcorrencias(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var id = await (from c in db.TpedidoOcorrencias
                            where c.Pedido == numPedido
                            select c.Id).FirstOrDefaultAsync();

            //nenhuma ocorrencia para esse pedido
            if (id == 0)
                return new List<OcorrenciasDtoPedido>();


            var msg = (await ObterMensagemOcorrencia(id)).ToList();

            var ocorrencia = from d in db.TpedidoOcorrencias
                             where d.Pedido == numPedido
                             select d;
            List<OcorrenciasDtoPedido> lista = new List<OcorrenciasDtoPedido>();
            OcorrenciasDtoPedido ocorre = new OcorrenciasDtoPedido();

            foreach (var i in ocorrencia)
            {
                ocorre.Usuario = i.Usuario_Cadastro;
                ocorre.Dt_Hr_Cadastro = i.Dt_Hr_Cadastro;
                if (i.Finalizado_Status != 0)
                    ocorre.Situacao = "Finalizado";
                else
                {
                    ocorre.Situacao = (from c in db.TpedidoOcorrenciaMensagems
                                       where c.Id_Ocorrencia == i.Id &&
                                             c.Fluxo_Mensagem == Constantes.COD_FLUXO_MENSAGEM_OCORRENCIAS_EM_PEDIDOS__CENTRAL_PARA_LOJA
                                       select c).Count() > 0 ? "Em Andamento" : "Aberta";

                }
                if (i.Tel_1 != "")
                    ocorre.Contato = i.Contato + "(" + i.Ddd_1 + ") " + i.Tel_1;
                if (i.Tel_2 != "")
                    ocorre.Contato = i.Contato + "(" + i.Ddd_2 + ") " + i.Tel_2;
                ocorre.Texto_Ocorrencia = i.Texto_Ocorrencia;
                ocorre.mensagemDtoOcorrenciaPedidos = msg;
                ocorre.Finalizado_Usuario = i.Finalizado_Usuario;
                ocorre.Finalizado_Data_Hora = i.Finalizado_Data_Hora;
                ocorre.Tipo_Ocorrencia = await ObterDescricao_Cod(Constantes.GRUPO_T_CODIGO_DESCRICAO__OCORRENCIAS_EM_PEDIDOS__TIPO_OCORRENCIA, i.Tipo_Ocorrencia);
                ocorre.Texto_Finalizacao = i.Texto_Finalizacao;
                lista.Add(ocorre);
            }

            return await Task.FromResult(lista);
        }

        private async Task<IEnumerable<MensagemDtoOcorrenciaPedido>> ObterMensagemOcorrencia(int idOcorrencia)
        {
            var db = contextoProvider.GetContexto();

            var msg = from c in db.TpedidoOcorrenciaMensagems
                      where c.Id_Ocorrencia == idOcorrencia
                      select new MensagemDtoOcorrenciaPedido
                      {
                          Dt_Hr_Cadastro = c.Dt_Hr_Cadastro,
                          Usuario = c.Usuario_Cadastro,
                          Loja = c.Loja,
                          Texto_Mensagem = c.Texto_Mensagem
                      };

            return await Task.FromResult(msg);
        }

        private async Task<string> ObterAnaliseCredito(string codigo, string numPedido, string apelido)
        {
            string retorno = "";

            switch (codigo)
            {
                case Constantes.COD_AN_CREDITO_ST_INICIAL:
                    retorno = "";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE:
                    retorno = "Pendente";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_VENDAS:
                    retorno = "Pendente Vendas";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_ENDERECO:
                    retorno = "Pendente Endereço";
                    break;
                case Constantes.COD_AN_CREDITO_OK:
                    retorno = "Crédito OK";
                    break;
                case Constantes.COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO:
                    retorno = "Crédito OK (aguardando depósito)";
                    break;
                case Constantes.COD_AN_CREDITO_OK_DEPOSITO_AGUARDANDO_DESBLOQUEIO:
                    retorno = "Crédito OK (depósito aguardando desbloqueio)";
                    break;
                case Constantes.COD_AN_CREDITO_NAO_ANALISADO:
                    retorno = "";
                    break;
                case Constantes.COD_AN_CREDITO_PENDENTE_CARTAO:
                    retorno = "Pendente Cartão de Crédito";
                    break;
            }

            if (retorno != "")
            {
                var db = contextoProvider.GetContexto();

                var ret = from c in db.Tpedidos
                          where c.Pedido == numPedido && c.Orcamentista == apelido
                          select new { analise_credito_data = c.Analise_credito_Data.ToString(), analise_credito_usuario = c.Analise_Credito_Usuario.ToString() };

                if (await ret.Select(r => r.analise_credito_data).FirstOrDefaultAsync() != "")
                {
                    string credito = await ret.Select(r => r.analise_credito_data).FirstOrDefaultAsync();
                    if (await ret.Select(r => r.analise_credito_usuario).FirstOrDefaultAsync() != "")
                    {
                        credito += " - " + await ret.Select(r => r.analise_credito_usuario).FirstOrDefaultAsync();
                        retorno = retorno + "(" + credito + ")";
                    }
                }
            }

            return await Task.FromResult(retorno);
        }

        private async Task<decimal> CalculaSaldoAPagar(string numPedido, decimal vlDevNf)
        {
            var db = contextoProvider.GetContexto();

            //buscar o valor total pago
            var vlFamiliaP = from c in db.TpedidoPagamentos
                             where c.Pedido.StartsWith(numPedido)
                             select c;
            var vl_TotalFamiliaPagoTask = vlFamiliaP.Select(r => r.Valor).SumAsync();

            //buscar valor total NF
            var vlNf = from c in db.TpedidoItems.Include(r => r.Tpedido)
                       where c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO && c.Tpedido.Pedido.StartsWith(numPedido)
                       select c.Qtde * c.Preco_NF;
            var vl_TotalFamiliaPrecoNFTask = vlNf.Select(r => r.Value).SumAsync();

            decimal result = await vl_TotalFamiliaPrecoNFTask - await vl_TotalFamiliaPagoTask - vlDevNf;

            return await Task.FromResult(result);
        }

        //Retorna o valor da Familia RA
        private async Task<decimal> CalculaTotalFamiliaRA(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var vlTotalVendaPorItem = from c in db.TpedidoItems.Include(r => r.Tpedido)
                                      where c.Tpedido.St_Entrega != Constantes.ST_ENTREGA_CANCELADO && c.Tpedido.Pedido.StartsWith(numPedido)
                                      select new { venda = c.Qtde * c.Preco_Venda, nf = c.Qtde * c.Preco_NF };

            var vlTotalVenda = await vlTotalVendaPorItem.Select(r => r.venda).SumAsync();
            var vlTotalNf = await vlTotalVendaPorItem.Select(r => r.nf).SumAsync();

            decimal result = vlTotalVenda.Value - vlTotalNf.Value;

            return await Task.FromResult(result);
        }

        private async Task<IEnumerable<ProdutoDevolvidoDtoPedido>> BuscarProdutosDevolvidos(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var lista = from c in db.TpedidoItemDevolvidos
                        where c.Pedido.StartsWith(numPedido)
                        select new ProdutoDevolvidoDtoPedido
                        {
                            Data = c.Devolucao_Data,
                            Hora = c.Devolucao_Hora,
                            Qtde = c.Qtde,
                            CodProduto = c.Produto,
                            DescricaoProduto = c.Descricao,
                            Motivo = c.Motivo,
                            NumeroNF = c.NFe_Numero_NF
                        };

            return await Task.FromResult(lista);
        }

        private async Task<IEnumerable<PedidoPerdasDtoPedido>> BuscarPerdas(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var lista = from c in db.TpedidoPerdas
                        where c.Pedido == numPedido
                        select new PedidoPerdasDtoPedido
                        {
                            Data = c.Data,
                            Hora = c.Hora,
                            Valor = c.Valor,
                            Obs = c.Obs
                        };

            return await Task.FromResult(lista);
        }

        private async Task<int> VerificarEstoque(string numPedido, string fabricante, string produto)
        {
            var db = contextoProvider.GetContexto();

            var prod = from c in db.TestoqueMovimentos
                       where c.Anulado_Status == 0 &&
                             c.Pedido == numPedido &&
                             c.Fabricante == fabricante &&
                             c.Produto == produto &&
                             c.Estoque == Constantes.ID_ESTOQUE_VENDIDO &&
                             c.Qtde.HasValue
                       select new { qtde = (int)c.Qtde };

            int qtde = await prod.Select(r => r.qtde).SumAsync();

            return await Task.FromResult(qtde);
        }

        private async Task<BlocoNotasDtoPedido> BuscarPedidoBlocoNotas(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var bl = from c in db.TpedidoBlocosNotas
                     where c.Pedido == numPedido &&
                           c.Nivel_Acesso == Constantes.COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__PUBLICO &&
                           c.Anulado_Status == 0
                     select c;

            BlocoNotasDtoPedido bloco = new BlocoNotasDtoPedido
            {
                Dt_Hora_Cadastro = await bl.Select(r => r.Dt_Hr_Cadastro).FirstOrDefaultAsync(),
                Usuario = await bl.Select(r => r.Usuario).FirstOrDefaultAsync(),
                Loja = await bl.Select(r => r.Loja).FirstOrDefaultAsync(),
                Mensagem = await bl.Select(r => r.Mensagem).FirstOrDefaultAsync()
            };

            return await Task.FromResult(bloco);

        }

        private async Task<IEnumerable<BlocoNotasDevolucaoMercadoriasDtoPedido>> BuscarPedidoBlocoNotasDevolucao(string numPedido)
        {
            var db = contextoProvider.GetContexto();

            var blDevolucao = from c in db.TpedidoItemDevolvidoBlocoNotas.Include(r => r.TpedidoItemDevolvido)
                              where c.TpedidoItemDevolvido.Pedido == numPedido && c.Anulado_Status == 0
                              orderby c.Dt_Hr_Cadastro, c.Id
                              select new BlocoNotasDevolucaoMercadoriasDtoPedido
                              {
                                  Dt_Hr_Cadastro = c.Dt_Hr_Cadastro,
                                  Usuario = c.Usuario,
                                  Loja = c.Loja,
                                  Mensagem = c.Mensagem
                              };

            if (blDevolucao.Count() == 0)
                return new List<BlocoNotasDevolucaoMercadoriasDtoPedido>();

            List<BlocoNotasDevolucaoMercadoriasDtoPedido> lista = new List<BlocoNotasDevolucaoMercadoriasDtoPedido>();

            foreach (var b in blDevolucao)
            {
                lista.Add(new BlocoNotasDevolucaoMercadoriasDtoPedido
                {
                    Dt_Hr_Cadastro = b.Dt_Hr_Cadastro,
                    Usuario = b.Usuario,
                    Loja = b.Loja,
                    Mensagem = b.Mensagem
                });
            }

            return await Task.FromResult(lista);
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

        private async Task<IEnumerable<string>> ObterFormaPagto(string numPedido, string orcamentista)
        {
            var db = contextoProvider.GetContexto();

            var p = from c in db.Tpedidos
                    where c.Pedido == numPedido && c.Orcamentista == orcamentista
                    select c;

            Tpedido pedido = p.FirstOrDefault();
            List<string> lista = new List<string>();
            string parcelamento = Convert.ToString(pedido.Tipo_Parcelamento);

            switch (parcelamento)
            {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    lista.Add("À Vista (" + Util.OpcaoFormaPagto(parcelamento) + ")");
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    lista.Add("Parcela Única: " + Constantes.SIMBOLO_MONETARIO + " " +
                        Util.OpcaoFormaPagto(Convert.ToString(pedido.Pu_Forma_Pagto)) + " vencendo após " + pedido.Pu_Vencto_Apos + " dias");
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    lista.Add("Parcelado no Cartão (internet) em " + pedido.Pc_Qtde_Parcelas + " X " +
                        Constantes.SIMBOLO_MONETARIO + " " + pedido.Pc_Valor_Parcela);
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    lista.Add("Parcelado no Cartão (maquineta) em " + pedido.Pc_Maquineta_Qtde_Parcelas + " X " + pedido.Pc_Maquineta_Valor_Parcela);
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    lista.Add("Entrada: " + Constantes.SIMBOLO_MONETARIO + " " +
                        pedido.Pce_Entrada_Valor + " (" + Util.OpcaoFormaPagto(Convert.ToString(pedido.Pce_Forma_Pagto_Entrada)) + ")");
                    lista.Add("Prestações: " + pedido.Pce_Prestacao_Qtde + " X " + Constantes.SIMBOLO_MONETARIO + " " + pedido.Pce_Prestacao_Valor +
                        " (" + Util.OpcaoFormaPagto(Convert.ToString(pedido.Pce_Forma_Pagto_Prestacao)) + ") vencendo a cada " +
                        pedido.Pce_Prestacao_Periodo + " dias");
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    lista.Add("1ª Prestação: " + Constantes.SIMBOLO_MONETARIO + " " + pedido.Pse_Prim_Prest_Valor + " (" +
                        Util.OpcaoFormaPagto(Convert.ToString(pedido.Pse_Forma_Pagto_Prim_Prest)) + ") vencendo após " + pedido.Pse_Prim_Prest_Apos + " dias");
                    lista.Add("Demais Prestações: " + pedido.Pse_Demais_Prest_Qtde + " X " + Constantes.SIMBOLO_MONETARIO + " " + pedido.Pse_Demais_Prest_Valor +
                        " (" + Util.OpcaoFormaPagto(Convert.ToString(pedido.Pse_Forma_Pagto_Demais_Prest)) + ") vencendo a cada " +
                        pedido.Pse_Demais_Prest_Periodo + " dias");
                    break;
            }

            return await Task.FromResult(lista.ToList());
        }

        //private async Task<string> OpcaoFormaPagto(string codigo)
        //{
        //    string retorno = "";

        //    switch (codigo)
        //    {
        //        case Constantes.ID_FORMA_PAGTO_DINHEIRO:
        //            retorno = "Dinheiro";
        //            break;
        //        case Constantes.ID_FORMA_PAGTO_DEPOSITO:
        //            retorno = "Depósito";
        //            break;
        //        case Constantes.ID_FORMA_PAGTO_CHEQUE:
        //            retorno = "Cheque";
        //            break;
        //        case Constantes.ID_FORMA_PAGTO_BOLETO:
        //            retorno = "Boleto";
        //            break;
        //        case Constantes.ID_FORMA_PAGTO_CARTAO:
        //            retorno = "Cartão (internet)";
        //            break;
        //        case Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA:
        //            retorno = "Cartão (maquineta)";
        //            break;
        //        case Constantes.ID_FORMA_PAGTO_BOLETO_AV:
        //            retorno = "Boleto AV";
        //            break;
        //    };

        //    return await Task.FromResult(retorno);
        //}


        //public async Task<decimal> CalculaTotalDevolucoes_Venda_E_NF(string numPedido)
        //{
        //    var db = contextoProvider.GetContexto();

        //    var itemDevTotal = from c in db.TpedidoItemDevolvidos
        //                       where c.Pedido.StartsWith(numPedido)
        //                       select new { venda = c.Qtde * c.Preco_Venda, nf = c.Qtde * c.Preco_NF };

        //    var vlTotDevVenda = await itemDevTotal.Select(r => r.venda).SumAsync();
        //    var vlTotDevNf = await itemDevTotal.Select(r => r.nf).SumAsync();

        //    decimal result = vlTotDevVenda.Value - vlTotDevNf.Value;

        //    return await Task.FromResult(result);
        //}

    }
}
