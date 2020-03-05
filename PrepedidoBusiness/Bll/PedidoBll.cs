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
        private readonly InfraBanco.ContextoBdProvider contextoProvider;

        public PedidoBll(InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.contextoProvider = contextoProvider;
        }


        public async Task<IEnumerable<string>> ListarNumerosPedidoCombo(string apelido)
        {

            var db = contextoProvider.GetContextoLeitura();

            var lista = from p in db.Tpedidos
                        where p.Orcamentista == apelido &&
                              p.Data >= Util.LimiteDataBuscas()
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
            if (dataInicial < Util.LimiteDataBuscas())
            {
                dataInicial = Util.LimiteDataBuscas();
            }
            //fazemos a busca
            var ret = await ListarPedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPedido, dataInicial, dataFinal);

            //se tiver algum registro, retorna imediatamente
            if (ret.Any())
                return ret;

            /*
             * se fizeram a busca por algum CPF ou CNPJ ou pedido e não achamos nada, removemos o filtro de datas
             * para não aparcer para o usuário que não tem nenhum registro
             * */
            if (String.IsNullOrEmpty(clienteBusca) && String.IsNullOrEmpty(numeroPedido))
                return ret;

            //busca sem data final
            ret = await ListarPedidosFiltroEstrito(apelido, tipoBusca, clienteBusca, numeroPedido, dataInicial, null);
            if (ret.Any())
                return ret;

            //ainda não achamos nada? então faz a busca sem filtrar por tipo
            ret = await ListarPedidosFiltroEstrito(apelido, TipoBuscaPedido.Todos, clienteBusca, numeroPedido, dataInicial, null);
            return ret;
        }

        //a busca sem malabarismos para econtrar algum registro
        private async Task<IEnumerable<PedidoDtoPedido>> ListarPedidosFiltroEstrito(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            var db = contextoProvider.GetContextoLeitura();

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
                    pedido.Status = "Separar Mercadoria";
                if (pedido.Status == "AET")
                    pedido.Status = "A entregar";
                if (pedido.Status == "ETG")
                    pedido.Status = "Entregue";
                if (pedido.Status == "CAN")
                    pedido.Status = "Cancelado";
            }
            return await Task.FromResult(listaComStatus);
        }

        public async Task<IEnumerable<string>> ListarCpfCnpjPedidosCombo(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var lista = (from c in db.Tpedidos.Include(r => r.Tcliente)
                         where c.Orcamentista == apelido &&
                               c.Data >= Util.LimiteDataBuscas()
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
            var dadosCliente = from c in contextoProvider.GetContextoLeitura().Tclientes
                               where c.Id == idCliente
                               select c;
            var cli = await dadosCliente.FirstOrDefaultAsync();
            DadosClienteCadastroDto cadastroCliente = new DadosClienteCadastroDto
            {
                Loja = await ObterRazaoSocialLoja(loja),
                Indicador_Orcamentista = indicador_orcamentista,
                Vendedor = vendedor,
                Id = cli.Id,
                Cnpj_Cpf = Util.FormatCpf_Cnpj_Ie(cli.Cnpj_Cpf),
                Rg = cli.Rg,
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
                Celular = cli.Tel_Cel,
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

        private async Task<string> ObterRazaoSocialLoja(string loja)
        {
            var db = contextoProvider.GetContextoLeitura();

            var ret = from c in db.Tlojas
                      where c.Loja == loja
                      select c.Razao_Social;

            string retorno = await ret.SingleOrDefaultAsync();

            return retorno;
        }

        private async Task<EnderecoEntregaDtoClienteCadastro> ObterEnderecoEntrega(Tpedido p)
        {
            EnderecoEntregaDtoClienteCadastro enderecoEntrega = new EnderecoEntregaDtoClienteCadastro
            {
                EndEtg_endereco = p.EndEtg_Endereco,
                EndEtg_endereco_numero = p.EndEtg_Endereco_Numero,
                EndEtg_endereco_complemento = p.EndEtg_Endereco_Complemento,
                EndEtg_bairro = p.EndEtg_Bairro,
                EndEtg_cidade = p.EndEtg_Cidade,
                EndEtg_uf = p.EndEtg_UF,
                EndEtg_cep = p.EndEtg_Cep
            };

            //obtemos a descricao somente se o codigo existir
            enderecoEntrega.EndEtg_descricao_justificativa = "";
            if (!String.IsNullOrEmpty(p.EndEtg_Cod_Justificativa))
                enderecoEntrega.EndEtg_descricao_justificativa = await Util.ObterDescricao_Cod(Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
                    p.EndEtg_Cod_Justificativa, contextoProvider);

            return await Task.FromResult(enderecoEntrega);
        }

        private async Task<IEnumerable<PedidoProdutosDtoPedido>> ObterProdutos(string numPedido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var produtosItens = from c in db.TpedidoItems
                                where c.Pedido == numPedido
                                select c;

            List<PedidoProdutosDtoPedido> lstProduto = new List<PedidoProdutosDtoPedido>();

            int qtde_sem_presenca = 0;
            int qtde_vendido = 0;
            short faltante = 0;

            foreach (var c in produtosItens)
            {
                qtde_sem_presenca = await VerificarEstoqueSemPresenca(numPedido, c.Fabricante, c.Produto);
                qtde_vendido = await VerificarEstoqueVendido(numPedido, c.Fabricante, c.Produto);

                if (qtde_sem_presenca != 0)
                    faltante = (short)qtde_sem_presenca;

                PedidoProdutosDtoPedido produto = new PedidoProdutosDtoPedido
                {
                    Fabricante = c.Fabricante,
                    NumProduto = c.Produto,
                    Descricao = c.Descricao_Html,
                    Qtde = c.Qtde,
                    Faltando = faltante,
                    CorFaltante = ObterCorFaltante((int)c.Qtde, qtde_vendido, qtde_sem_presenca),
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
            var db = contextoProvider.GetContextoLeitura();

            //parateste
            //numPedido = "094947N-A";
            //apelido = "PEDREIRA";

            var pedido = from c in db.Tpedidos
                         where c.Pedido == numPedido
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
                EntregaImediata = ObterEntregaImediata(p),
                StBemUsoConsumo = p.StBemUsoConsumo,
                InstaladorInstala = p.InstaladorInstalaStatus,
                GarantiaIndicadorStatus = garantiaIndicadorStatus
            };

            //verifica o status da entrega
            DateTime? dataEntrega = new DateTime();
            if (p.St_Entrega == Constantes.ST_ENTREGA_A_ENTREGAR || p.St_Entrega == Constantes.ST_ENTREGA_SEPARAR)
                dataEntrega = p.A_Entregar_Data_Marcada;

            var perdas = BuscarPerdas(numPedido);
            var TranspNomeTask = ObterNomeTransportadora(p.Transportadora_Id);
            var lstFormaPgtoTask = ObterFormaPagto(p);
            var analiseCreditoTask = ObterAnaliseCredito(Convert.ToString(p.Analise_Credito), numPedido, apelido);
            string corAnalise = CorAnaliseCredito(Convert.ToString(p.Analise_Credito));
            string corStatusPagto = CorSatusPagto(p.St_Pagto);
            var saldo_a_pagarTask = CalculaSaldoAPagar(numPedido, await vl_TotalFamiliaDevolucaoPrecoNFTask);
            var TotalPerda = (await perdas).Select(r => r.Valor).Sum();
            var lstOcorrenciaTask = ObterOcorrencias(numPedido);
            var lstBlocNotasDevolucaoTask = BuscarPedidoBlocoNotasDevolucao(numPedido);
            var vlPagoTask = CalcularValorPago(numPedido);

            var saldo_a_pagar = await saldo_a_pagarTask;
            if (p.St_Entrega == Constantes.ST_PAGTO_PAGO && saldo_a_pagar > 0)
                saldo_a_pagar = 0;

            //afazer: calcular o VL Pago = o valor que já foi pago (entrada ou pagamento parcial)
            /* O valor de VL Pago é um select na base
             * precisamos calcular o saldo a pagar que é o calculo de 
             * saldo a pagar = vl_TotalFamiliaPrecoNF - vl_TotalFamiliaPago - vl_TotalFamiliaDevolucaoPrecoNF
             */

            DetalhesFormaPagamentos detalhesFormaPagto = new DetalhesFormaPagamentos
            {
                FormaPagto = (await lstFormaPgtoTask).ToList(),
                InfosAnaliseCredito = p.Forma_Pagto,
                StatusPagto = StatusPagto(p.St_Pagto).ToUpper(),
                CorStatusPagto = corStatusPagto,
                VlTotalFamilia = p.Vl_Total_Familia,
                VlPago = await vlPagoTask,
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
                ListaProdutoDevolvido = (await BuscarProdutosDevolvidos(numPedido)).ToList(),
                ListaPerdas = (await BuscarPerdas(numPedido)).ToList(),
                ListaBlocoNotas = (await BuscarPedidoBlocoNotas(numPedido)).ToList(),
                EnderecoEntrega = await enderecoEntregaTask,
                ListaOcorrencia = (await lstOcorrenciaTask).ToList(),
                ListaBlocoNotasDevolucao = (await lstBlocNotasDevolucaoTask).ToList()
            };

            return await Task.FromResult(DtoPedido);
        }

        private async Task<decimal> CalcularValorPago(string numPedido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var vlFamiliaP = from c in db.TpedidoPagamentos
                             where c.Pedido.StartsWith(numPedido)
                             select c;
            var vl_TotalFamiliaPagoTask = vlFamiliaP.Select(r => r.Valor).SumAsync();

            return await vl_TotalFamiliaPagoTask;
        }

        private string ObterEntregaImediata(Tpedido p)
        {
            string retorno = "";
            string dataFormatada = "";
            //varificar a variavel st_etg_imediata para saber se é sim ou não pela constante 
            //caso não atenda nenhuma das condições o retorno fica como vazio.
            if (p.St_Etg_Imediata == short.Parse(Constantes.COD_ETG_IMEDIATA_NAO))
                retorno = "NÃO";
            else if (p.St_Etg_Imediata == short.Parse(Constantes.COD_ETG_IMEDIATA_SIM))
                retorno = "SIM";
            //formatar a data da variavel etg_imediata_data
            if (retorno != "")
                dataFormatada = p.Etg_Imediata_Data?.ToString("dd/MM/yyyy HH:mm");
            //verificar se o retorno acima esta vazio
            if (!string.IsNullOrEmpty(dataFormatada))
                retorno += " (" + IniciaisEmMaisculas(p.Etg_Imediata_Usuario) + " - " + dataFormatada + ")";


            return retorno;
        }

        private string IniciaisEmMaisculas(string texto)
        {
            string retorno = "";
            string palavras_minusculas = "|A|AS|AO|AOS|À|ÀS|E|O|OS|UM|UNS|UMA|UMAS" +
                "|DA|DAS|DE|DO|DOS|EM|NA|NAS|NO|NOS|COM|SEM|POR|PELO|PELA|PARA|PRA|P/|S/|C/|TEM|OU|E/OU|ATE|ATÉ|QUE|SE|QUAL|";
            string palavras_maiusculas = "|II|III|IV|VI|VII|VIII|IX|XI|XII|XIII|XIV" +
                "|XV|XVI|XVII|XVIII|XIX|XX|XXI|XXII|XXIII|S/A|S/C|AC|AL|AM|AP|BA|CE|DF|ES|GO" +
                "|MA|MG|MS|MT|PA|PB|PE|PI|PR|RJ|RN|RO|RR|RS|SC|SE|SP|TO|ME|EPP|";

            string letra = "";
            string palavra = "";
            string frase = "";
            string s = "";
            bool blnAltera = false;

            string char34 = Convert.ToString((char)34);

            for (int i = 0; i < texto.Length; i++)
            {
                letra = texto.Substring(i, 1);
                palavra += letra;

                if ((letra == " ") || (i == texto.Length - 1) || (letra == "(") || (letra == ")") || (letra == "[") || (letra == "]")
                    || (letra == "'") || (letra == char34) || (letra == "-"))
                {
                    s = "|" + palavra.ToUpper().Trim() + "|";
                    if (palavras_minusculas.IndexOf(s) != 0 && frase != "")
                    {
                        //SE FOR FINAL DA FRASE, DEIXA INALTERADO(EX: BLOCO A)
                        if (i < texto.Length)
                            palavra = palavra.ToLower();
                    }
                    else if (palavras_maiusculas.IndexOf(s) >= 0)
                        palavra = palavra.ToUpper();
                    else
                    {
                        //ANALISA SE CONVERTE O TEXTO OU NÃO
                        blnAltera = true;
                        if (TemDigito(palavra))
                        {
                            //ENDEREÇOS CUJO Nº DA RESIDÊNCIA SÃO SEPARADOS POR VÍRGULA, SEM NENHUM ESPAÇO EM BRANCO
                            //CASO CONTRÁRIO, CONSIDERA QUE É ALGUM TIPO DE CÓDIGO
                            if (palavra.IndexOf(",") != 0)
                                blnAltera = false;
                        }
                        if (palavra.IndexOf(".") >= 0)
                        {
                            if (palavra.IndexOf(palavra, palavra.IndexOf(".") + 1, StringComparison.OrdinalIgnoreCase.CompareTo(".")) != 0)
                                blnAltera = false;
                        }
                        if (palavra.IndexOf("/") != 0)
                        {
                            if (palavra.Length <= 4)
                                blnAltera = false;
                        }
                        //verifica se tem vogal
                        if (!TemVogal(palavra))
                            blnAltera = false;

                        if (blnAltera)
                            palavra = palavra.Substring(0, 1).ToUpper() + palavra.Substring(1, palavra.Length - 1).ToLower();//verificar
                    }
                    frase = frase + palavra;
                    palavra = "";
                }
            }
            return retorno = frase;
        }




        private bool TemVogal(string texto)
        {
            bool retorno = false;
            string letra = "";

            for (int i = 0; i < texto.Length; i++)
            {
                letra = texto.Substring(i, 1).ToUpper();
                if (letra == "A" || letra == "E" || letra == "I" || letra == "O" || letra == "U")
                    retorno = true;
            }

            return retorno;
        }

        private bool TemDigito(string texto)
        {
            int ehNumero;
            bool retorno = false;

            for (int i = 0; i < texto.Length; i++)
            {
                if (int.TryParse(texto.Substring(i, 1), out ehNumero))
                    retorno = true;
            }

            return retorno;
        }

        private async Task<string> ObterNomeTransportadora(string idTransportadora)
        {
            var db = contextoProvider.GetContextoLeitura();

            var transportadora = from c in db.Ttransportadoras
                                 where c.Id == idTransportadora
                                 select c.Nome;
            var retorno = await transportadora.Select(r => r.ToString()).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(retorno))
                retorno = idTransportadora + " (" + retorno + ")";

            return retorno;
        }

        private async Task<StatusPedidoDtoPedido> MontarDtoStatuPedido(Tpedido p)
        {
            var db = contextoProvider.GetContextoLeitura();
            var countTask = from c in db.TpedidoItemDevolvidos
                            where c.Pedido == p.Pedido
                            select c;
            int countItemDevolvido = await countTask.CountAsync();

            StatusPedidoDtoPedido status = new StatusPedidoDtoPedido();

            if (!String.IsNullOrEmpty(p.Pedido_Bs_X_Marketplace))
            {
                //verificar
                status.Descricao_Pedido_Bs_X_Marketplace = Util.ObterDescricao_Cod("PedidoECommerce_Origem", p.Marketplace_codigo_origem, contextoProvider) + ":" + p.Pedido_Bs_X_Marketplace;
                //status.Cor_Pedido_Bs_X_Marketplace = CorStatusEntrega(p.St_Entrega);
            }
            if (!String.IsNullOrEmpty(p.Pedido_Bs_X_Ac))
            {
                status.Cor_Pedido_Bs_X_Ac = "purple";
                status.Pedido_Bs_X_Ac = p.Pedido_Bs_X_Ac;
            }

            status.Status = FormataSatusPedido(p.St_Entrega);
            status.CorEntrega = CorStatusEntrega(p.St_Entrega, countItemDevolvido);//verificar a saida 
            status.St_Entrega = FormataSatusPedido(p.St_Entrega);
            status.Entregue_Data = p.Entregue_Data?.ToString("dd/MM/yyyy");
            status.Cancelado_Data = p.Cancelado_Data?.ToString("dd/MM/yyyy");
            status.Pedido_Data = p.Data?.ToString("dd/MM/yyyy") + " " + Formata_hhmmss_para_hh_minuto(p.Hora);
            status.Recebida_Data = p.PedidoRecebidoData?.ToString("dd/MM/yyyy");


            return await Task.FromResult(status);
        }

        private string Formata_hhmmss_para_hh_minuto(string hora)
        {
            string hh = "";
            string mm = "";
            string ss = "";
            string retorno = "";

            if (!string.IsNullOrEmpty(hora))
            {
                hh = hora.Substring(0, 2);
                mm = hora.Substring(2, 2);
                //ss = hora.Substring(4, 2);

                //retorno = hh + ":" + mm + ":" + ss;
                retorno = hh + ":" + mm;
            }

            return retorno;

        }

        private string CorStatusEntrega(string st_entrega, int countItemDevolvido)
        {
            string cor = "black";

            switch (st_entrega)
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
                    //fazer a verificação de itens devolvidos aqui
                    if (countItemDevolvido > 0)
                        cor = "red";
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
                case Constantes.ST_ENTREGA_ESPERAR:
                    retorno = "Esperar Mercadoria";
                    break;
                case Constantes.ST_ENTREGA_SPLIT_POSSIVEL:
                    retorno = "Split Possível";
                    break;
                case Constantes.ST_ENTREGA_SEPARAR:
                    retorno = "Separar Mercadoria";
                    break;
                case Constantes.ST_ENTREGA_A_ENTREGAR:
                    retorno = "A Entregar";
                    break;
                case Constantes.ST_ENTREGA_ENTREGUE:
                    retorno = "Entregue";
                    break;
                case Constantes.ST_ENTREGA_CANCELADO:
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
            var db = contextoProvider.GetContextoLeitura();

            var id = await (from c in db.TpedidoOcorrencias
                            where c.Pedido == numPedido
                            select c.Id).FirstOrDefaultAsync();

            //nenhuma ocorrencia para esse pedido
            if (id == 0)
                return new List<OcorrenciasDtoPedido>();


            var msg = (await ObterMensagemOcorrencia(id)).ToList();



            var ocorrencia = await (from d in db.TpedidoOcorrencias
                                    where d.Pedido == numPedido
                                    select d).ToListAsync();
            List<OcorrenciasDtoPedido> lista = new List<OcorrenciasDtoPedido>();
            

            foreach (var i in ocorrencia)
            {
                OcorrenciasDtoPedido ocorre = new OcorrenciasDtoPedido();
                ocorre.Usuario = i.Usuario_Cadastro;
                ocorre.Dt_Hr_Cadastro = i.Dt_Hr_Cadastro;
                if (i.Finalizado_Status != 0)
                    ocorre.Situacao = "FINALIZADO";
                else
                {
                    ocorre.Situacao = (from c in db.TpedidoOcorrenciaMensagems
                                       where c.Id_Ocorrencia == i.Id &&
                                             c.Fluxo_Mensagem == Constantes.COD_FLUXO_MENSAGEM_OCORRENCIAS_EM_PEDIDOS__CENTRAL_PARA_LOJA
                                       select c).Count() > 0 ? "EM ANDAMENTO" : "ABERTA";

                }
                ocorre.Contato = i.Contato ?? "";
                if (i.Tel_1 != "")
                    ocorre.Contato += "&nbsp;&nbsp; (" + i.Ddd_1 + ") " + Util.FormatarTelefones(i.Tel_1);
                if (i.Tel_2 != "")
                    ocorre.Contato += "   &nbsp;&nbsp;(" + i.Ddd_2 + ") " + Util.FormatarTelefones(i.Tel_2);
                ocorre.Texto_Ocorrencia = i.Texto_Ocorrencia;
                ocorre.mensagemDtoOcorrenciaPedidos = msg;
                ocorre.Finalizado_Usuario = i.Finalizado_Usuario;
                ocorre.Finalizado_Data_Hora = i.Finalizado_Data_Hora;
                ocorre.Tipo_Ocorrencia = await Util.ObterDescricao_Cod(Constantes.GRUPO_T_CODIGO_DESCRICAO__OCORRENCIAS_EM_PEDIDOS__TIPO_OCORRENCIA, i.Tipo_Ocorrencia, contextoProvider);
                ocorre.Texto_Finalizacao = i.Texto_Finalizacao;
                lista.Add(ocorre);
            }

            return await Task.FromResult(lista);
        }

        private async Task<IEnumerable<MensagemDtoOcorrenciaPedido>> ObterMensagemOcorrencia(int idOcorrencia)
        {
            var db = contextoProvider.GetContextoLeitura();

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
                var db = contextoProvider.GetContextoLeitura();

                var ret = from c in db.Tpedidos
                          where c.Pedido == numPedido && c.Orcamentista == apelido
                          select new { analise_credito_data = c.Analise_credito_Data, analise_credito_usuario = c.Analise_Credito_Usuario };

                var registro = ret.FirstOrDefault();
                if (registro != null)
                {
                    if (registro.analise_credito_data.HasValue)
                    {
                        //string credito = registro.analise_credito_data;
                        if (!string.IsNullOrEmpty(registro.analise_credito_usuario))
                        {
                            string maiuscula = (Char.ToUpper(registro.analise_credito_usuario[0]) +
                                registro.analise_credito_usuario.Substring(1).ToLower());

                            retorno = retorno + " (" + registro.analise_credito_data?.ToString("dd/MM/yyyy HH:mm") + " - "
                                + maiuscula + ")";
                        }
                    }
                }
            }

            return await Task.FromResult(retorno);
        }

        private async Task<decimal> CalculaSaldoAPagar(string numPedido, decimal vlDevNf)
        {
            var db = contextoProvider.GetContextoLeitura();

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
            var db = contextoProvider.GetContextoLeitura();

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
            var db = contextoProvider.GetContextoLeitura();

            var lista = from c in db.TpedidoItemDevolvidos
                        where c.Pedido.StartsWith(numPedido)
                        select new ProdutoDevolvidoDtoPedido
                        {
                            Data = c.Devolucao_Data,
                            Hora = c.Devolucao_Hora.Substring(0, 2) + ":" + c.Devolucao_Hora.Substring(2, 2),
                            Qtde = c.Qtde,
                            CodProduto = c.Produto,
                            DescricaoProduto = c.Descricao_Html,
                            Motivo = c.Motivo,
                            NumeroNF = c.NFe_Numero_NF
                        };

            return await Task.FromResult(lista);
        }

        private async Task<IEnumerable<PedidoPerdasDtoPedido>> BuscarPerdas(string numPedido)
        {
            var db = contextoProvider.GetContextoLeitura();

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

        private async Task<int> VerificarEstoqueVendido(string numPedido, string fabricante, string produto)
        {
            var db = contextoProvider.GetContextoLeitura();

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

        private async Task<int> VerificarEstoqueSemPresenca(string numPedido, string fabricante, string produto)
        {
            var db = contextoProvider.GetContextoLeitura();

            var prod = from c in db.TestoqueMovimentos
                       where c.Anulado_Status == 0 &&
                             c.Pedido == numPedido &&
                             c.Fabricante == fabricante &&
                             c.Produto == produto &&
                             c.Estoque == Constantes.ID_ESTOQUE_SEM_PRESENCA &&
                             c.Qtde.HasValue
                       select new { qtde = (int)c.Qtde };

            int qtde = await prod.Select(r => r.qtde).SumAsync();

            return await Task.FromResult(qtde);
        }

        private string ObterCorFaltante(int qtde, int qtde_estoque_vendido, int qtde_estoque_sem_presenca)
        {
            string retorno = "";

            if (qtde <= 0 || qtde != (qtde_estoque_vendido + qtde_estoque_sem_presenca))
                retorno = "black";
            if (qtde_estoque_vendido != 0 && qtde_estoque_sem_presenca != 0)
                retorno = "darkorange";
            else if (qtde_estoque_sem_presenca == 0)
                retorno = "black";
            else if (qtde_estoque_vendido == 0)
                retorno = "red";

            return retorno;
        }

        private async Task<IEnumerable<BlocoNotasDtoPedido>> BuscarPedidoBlocoNotas(string numPedido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var bl = from c in db.TpedidoBlocosNotas
                     where c.Pedido == numPedido &&
                           c.Nivel_Acesso == Constantes.COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__PUBLICO &&
                           c.Anulado_Status == 0
                     select c;

            List<BlocoNotasDtoPedido> lstBlocoNotas = new List<BlocoNotasDtoPedido>();

            foreach (var i in bl)
            {
                BlocoNotasDtoPedido bloco = new BlocoNotasDtoPedido
                {
                    Dt_Hora_Cadastro = i.Dt_Hr_Cadastro,
                    Usuario = i.Usuario,
                    Loja = i.Loja,
                    Mensagem = i.Mensagem
                };
                lstBlocoNotas.Add(bloco);
            }

            return await Task.FromResult(lstBlocoNotas);

        }

        private async Task<IEnumerable<BlocoNotasDevolucaoMercadoriasDtoPedido>> BuscarPedidoBlocoNotasDevolucao(string numPedido)
        {
            var db = contextoProvider.GetContextoLeitura();

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

        private async Task<IEnumerable<string>> ObterFormaPagto(Tpedido ped)
        {
            var db = contextoProvider.GetContextoLeitura();

            var p = from c in db.Tpedidos
                    where c.Pedido == ped.Pedido && c.Indicador == ped.Indicador
                    select c;

            Tpedido pedido = p.FirstOrDefault();
            List<string> lista = new List<string>();
            string parcelamento = Convert.ToString(pedido.Tipo_Parcelamento);

            switch (parcelamento)
            {
                case Constantes.COD_FORMA_PAGTO_A_VISTA:
                    lista.Add("À Vista (" + Util.OpcaoFormaPagto(pedido.Av_Forma_Pagto.ToString()) + ")");
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    lista.Add(String.Format("Parcela Única: " + " {0:c2} (" +
                        Util.OpcaoFormaPagto(Convert.ToString(pedido.Pu_Forma_Pagto)) +
                        ") vencendo após " + pedido.Pu_Vencto_Apos + " dias"));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    lista.Add(String.Format("Parcelado no Cartão (internet) em " + pedido.Pc_Qtde_Parcelas + " x " +
                        " {0:c2}", pedido.Pc_Valor_Parcela));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    lista.Add(String.Format("Parcelado no Cartão (maquineta) em " + pedido.Pc_Maquineta_Qtde_Parcelas +
                        " x {0:c2}" + pedido.Pc_Maquineta_Valor_Parcela));
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    lista.Add(String.Format("Entrada " + "{0:c2} (" +
                        Util.OpcaoFormaPagto(Convert.ToString(pedido.Pce_Forma_Pagto_Entrada)) + ")", pedido.Pce_Entrada_Valor));
                    if (pedido.Pce_Forma_Pagto_Prestacao != 5 && pedido.Pce_Forma_Pagto_Prestacao != 7)
                    {
                        lista.Add(String.Format("Prestações: " + pedido.Pce_Prestacao_Qtde + " x " + " {0:c2}" +
                            " (" + Util.OpcaoFormaPagto(Convert.ToString(pedido.Pce_Forma_Pagto_Prestacao)) +
                            ") vencendo a cada " +
                            pedido.Pce_Prestacao_Periodo + " dias", pedido.Pce_Prestacao_Valor));
                    }
                    else
                    {
                        lista.Add(String.Format("Prestações: " + pedido.Pce_Prestacao_Qtde + " x " + " {0:c2}" +
                            " (" + Util.OpcaoFormaPagto(Convert.ToString(pedido.Pce_Forma_Pagto_Prestacao)) + ")", pedido.Pce_Prestacao_Valor));
                    }
                    break;
                case Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    lista.Add(String.Format("1ª Prestação: " + " {0:c2} (" +
                        Util.OpcaoFormaPagto(Convert.ToString(pedido.Pse_Forma_Pagto_Prim_Prest)) +
                        ") vencendo após " + pedido.Pse_Prim_Prest_Apos + " dias", pedido.Pse_Prim_Prest_Valor));
                    lista.Add(String.Format("Prestações: " + pedido.Pse_Demais_Prest_Qtde + " x " +
                        " {0:c2} (" + Util.OpcaoFormaPagto(Convert.ToString(pedido.Pse_Forma_Pagto_Demais_Prest)) +
                        ") vencendo a cada " + pedido.Pse_Demais_Prest_Periodo + " dias", pedido.Pse_Demais_Prest_Valor));
                    break;
            }

            return await Task.FromResult(lista.ToList());
        }

        private string StatusPagto(string status)
        {
            string retorno = "";

            switch (status)
            {
                case Constantes.ST_PAGTO_PAGO:
                    retorno = "Pago";
                    break;
                case Constantes.ST_PAGTO_NAO_PAGO:
                    retorno = "Não-Pago";
                    break;
                case Constantes.ST_PAGTO_PARCIAL:
                    retorno = "Pago Parcial";
                    break;
            };
            return retorno;
        }
    }
}
