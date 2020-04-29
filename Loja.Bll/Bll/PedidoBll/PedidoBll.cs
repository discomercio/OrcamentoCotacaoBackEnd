using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Loja.Data;
using Loja.Modelos;
using System.Linq;
using Loja.Bll.Dto.PedidoDto;
using Microsoft.EntityFrameworkCore;
using Loja.Bll.Dto.ClienteDto;
using Loja.Bll.Dto.PedidoDto.DetalhesPedido;
using Loja.Bll.ClienteBll;
using Loja.Bll.Dto.PrepedidoDto.DetalhesPrepedido;
using Loja.Bll.Dto.ProdutoDto;
using Loja.Modelo;
using Loja.Bll.RegrasCtrlEstoque;
using System.Text.RegularExpressions;
using Loja.Bll.Dto.LojaDto;
using Loja.Bll.Dto.IndicadorDto;
using Loja.Bll.Dto.FormaPagtoDto;
using Loja.Bll.Bll.PedidoBll.EfetivaPedido;

namespace Loja.Bll.PedidoBll
{
    public class PedidoBll
    {
        private readonly ContextoBdProvider contextoProvider;
        private readonly ContextoCepProvider contextoCepProvider;
        private readonly ContextoNFeProvider contextoNFeProvider;
        private readonly Loja.Bll.ProdutoBll.ProdutoBll produtoBll;
        private readonly Loja.Bll.ClienteBll.ClienteBll clienteBll;
        //private readonly Loja.Bll.Bll.PedidoBll.EfetivaPedido.EfetivaPedido efetivarPedido;

        public PedidoBll(ContextoBdProvider contextoProvider, ContextoCepProvider contextoCepProvider,
            ContextoNFeProvider contextoNFeProvider, ProdutoBll.ProdutoBll produtoBll, ClienteBll.ClienteBll clienteBll)
        {
            this.contextoProvider = contextoProvider;
            this.contextoCepProvider = contextoCepProvider;
            this.contextoNFeProvider = contextoNFeProvider;
            this.produtoBll = produtoBll;
            this.clienteBll = clienteBll;
            //this.efetivarPedido = efetivarPedido;
        }


        public async Task<IEnumerable<string>> ListarNumerosPedidoCombo(string apelido)
        {

            var db = contextoProvider.GetContextoLeitura();

            var lista = from p in db.Tpedidos
                        where p.Orcamentista == apelido &&
                              p.Data >= Util.Util.LimiteDataBuscas()
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
            if (dataInicial < Util.Util.LimiteDataBuscas())
            {
                dataInicial = Util.Util.LimiteDataBuscas();
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
                    pedido.Status = "Entrega";
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
                               c.Data >= Util.Util.LimiteDataBuscas()
                         orderby c.Tcliente.Cnpj_Cpf
                         select c.Tcliente.Cnpj_Cpf).Distinct();

            var ret = await lista.Distinct().ToListAsync();
            List<string> cpfCnpjFormat = new List<string>();

            foreach (string cpf in ret)
            {
                cpfCnpjFormat.Add(Util.Util.FormatCpf_Cnpj_Ie(cpf));
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
                Cnpj_Cpf = Util.Util.FormatCpf_Cnpj_Ie(cli.Cnpj_Cpf),
                Rg = Util.Util.FormatCpf_Cnpj_Ie(cli.Rg),
                Ie = Util.Util.FormatCpf_Cnpj_Ie(cli.Ie),
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
                EndEtg_endereco = p.Endereco_Logradouro,
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
                enderecoEntrega.EndEtg_descricao_justificativa = await Util.Util.ObterDescricao_Cod(Constantes.Constantes.GRUPO_T_CODIGO_DESCRICAO__ENDETG_JUSTIFICATIVA,
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
                    Preco = c.Preco_NF,//esse tem que ser o valor base 
                    VlLista = c.Preco_Lista,//esse é o valor base * coeficiente
                    Desconto = c.Desc_Dado,
                    VlUnitario = c.Preco_Venda,
                    VlTotalItem = c.Qtde * c.Preco_Venda,
                    VlTotalItemComRA = c.Qtde * c.Preco_Lista,
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
            if (garantiaIndicadorStatus == Constantes.Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO)
                garantiaIndicadorStatus = "NÃO";
            if (garantiaIndicadorStatus == Constantes.Constantes.COD_GARANTIA_INDICADOR_STATUS__SIM)
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
            if (p.St_Entrega == Constantes.Constantes.ST_ENTREGA_A_ENTREGAR || p.St_Entrega == Constantes.Constantes.ST_ENTREGA_SEPARAR)
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

            var saldo_a_pagar = await saldo_a_pagarTask;
            if (p.St_Entrega == Constantes.Constantes.ST_PAGTO_PAGO && saldo_a_pagar > 0)
                saldo_a_pagar = 0;

            DetalhesFormaPagamentos detalhesFormaPagto = new DetalhesFormaPagamentos
            {
                FormaPagto = (await lstFormaPgtoTask).ToList(),
                InfosAnaliseCredito = p.Forma_Pagto,
                StatusPagto = StatusPagto(p.St_Pagto).ToUpper(),
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
                ListaProdutoDevolvido = (await BuscarProdutosDevolvidos(numPedido)).ToList(),
                ListaPerdas = (await BuscarPerdas(numPedido)).ToList(),
                ListaBlocoNotas = (await BuscarPedidoBlocoNotas(numPedido)).ToList(),
                EnderecoEntrega = await enderecoEntregaTask,
                ListaOcorrencia = (await lstOcorrenciaTask).ToList(),
                ListaBlocoNotasDevolucao = (await lstBlocNotasDevolucaoTask).ToList()
            };

            return await Task.FromResult(DtoPedido);
        }

        private string ObterEntregaImediata(Tpedido p)
        {
            string retorno = "";
            string dataFormatada = "";
            //varificar a variavel st_etg_imediata para saber se é sim ou não pela constante 
            //caso não atenda nenhuma das condições o retorno fica como vazio.
            if (p.St_Etg_Imediata == short.Parse(Constantes.Constantes.COD_ETG_IMEDIATA_NAO))
                retorno = "NÃO";
            else if (p.St_Etg_Imediata == short.Parse(Constantes.Constantes.COD_ETG_IMEDIATA_SIM))
                retorno = "SIM";
            //formatar a data da variavel etg_imediata_data
            if (retorno != "")
                dataFormatada = p.Etg_Imediata_Data?.ToString();
            //verificar se o retorno acima esta vazio
            if (dataFormatada != "")
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

            return retorno;
        }

        private async Task<StatusPedidoDtoPedido> MontarDtoStatuPedido(Tpedido p)
        {
            StatusPedidoDtoPedido status = new StatusPedidoDtoPedido();

            if (!String.IsNullOrEmpty(p.Pedido_Bs_X_Marketplace))
            {
                //verificar
                status.Descricao_Pedido_Bs_X_Marketplace = Util.Util.ObterDescricao_Cod("PedidoECommerce_Origem", p.Marketplace_codigo_origem, contextoProvider) + ":" + p.Pedido_Bs_X_Marketplace;
                //status.Cor_Pedido_Bs_X_Marketplace = CorStatusEntrega(p.St_Entrega);
            }
            if (!String.IsNullOrEmpty(p.Pedido_Bs_X_Ac))
            {
                status.Cor_Pedido_Bs_X_Ac = "purple";
                status.Pedido_Bs_X_Ac = p.Pedido_Bs_X_Ac;
            }

            status.Status = FormataSatusPedido(p.St_Entrega);
            status.CorEntrega = CorStatusEntrega(p.St_Entrega);//verificar a saida 
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
                ss = hora.Substring(4, 2);

                retorno = hh + ":" + mm + ":" + ss;
            }

            return retorno;

        }

        private string CorStatusEntrega(string st_entrega)
        {
            string cor = "black";

            switch (st_entrega)
            {
                case Constantes.Constantes.ST_ENTREGA_ESPERAR:
                    cor = "deeppink";
                    break;
                case Constantes.Constantes.ST_ENTREGA_SPLIT_POSSIVEL:
                    cor = "darkorange";
                    break;
                case Constantes.Constantes.ST_ENTREGA_SEPARAR:
                    cor = "maroon";
                    break;
                case Constantes.Constantes.ST_ENTREGA_A_ENTREGAR:
                    cor = "blue";
                    break;
                case Constantes.Constantes.ST_ENTREGA_ENTREGUE:
                    cor = "green";
                    break;
                case Constantes.Constantes.ST_ENTREGA_CANCELADO:
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
                case Constantes.Constantes.ST_ENTREGA_ESPERAR:
                    retorno = "Esperar Mercadoria";
                    break;
                case Constantes.Constantes.ST_ENTREGA_SPLIT_POSSIVEL:
                    retorno = "Split Possível";
                    break;
                case Constantes.Constantes.ST_ENTREGA_SEPARAR:
                    retorno = "Separar Mercadoria";
                    break;
                case Constantes.Constantes.ST_ENTREGA_A_ENTREGAR:
                    retorno = "A Entregar";
                    break;
                case Constantes.Constantes.ST_ENTREGA_ENTREGUE:
                    retorno = "Entregue";
                    break;
                case Constantes.Constantes.ST_ENTREGA_CANCELADO:
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
                case Constantes.Constantes.ST_PAGTO_PAGO:
                    retorno = "green";
                    break;
                case Constantes.Constantes.ST_PAGTO_NAO_PAGO:
                    retorno = "red";
                    break;
                case Constantes.Constantes.ST_PAGTO_PARCIAL:
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
                case Constantes.Constantes.COD_AN_CREDITO_PENDENTE:
                    retorno = "red";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_PENDENTE_VENDAS:
                    retorno = "red";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_PENDENTE_ENDERECO:
                    retorno = "red";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_OK:
                    retorno = "green";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO:
                    retorno = "darkorange";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_OK_DEPOSITO_AGUARDANDO_DESBLOQUEIO:
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
                                             c.Fluxo_Mensagem == Constantes.Constantes.COD_FLUXO_MENSAGEM_OCORRENCIAS_EM_PEDIDOS__CENTRAL_PARA_LOJA
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
                ocorre.Tipo_Ocorrencia = await Util.Util.ObterDescricao_Cod(Constantes.Constantes.GRUPO_T_CODIGO_DESCRICAO__OCORRENCIAS_EM_PEDIDOS__TIPO_OCORRENCIA, i.Tipo_Ocorrencia, contextoProvider);
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

        public async Task<string> ObterAnaliseCredito(string codigo, string numPedido, string apelido)
        {
            string retorno = "";

            switch (codigo)
            {
                case Constantes.Constantes.COD_AN_CREDITO_ST_INICIAL:
                    retorno = "";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_PENDENTE:
                    retorno = "Pendente";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_PENDENTE_VENDAS:
                    retorno = "Pendente Vendas";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_PENDENTE_ENDERECO:
                    retorno = "Pendente Endereço";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_OK:
                    retorno = "Crédito OK";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO:
                    retorno = "Crédito OK (aguardando depósito)";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_OK_DEPOSITO_AGUARDANDO_DESBLOQUEIO:
                    retorno = "Crédito OK (depósito aguardando desbloqueio)";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_NAO_ANALISADO:
                    retorno = "";
                    break;
                case Constantes.Constantes.COD_AN_CREDITO_PENDENTE_CARTAO:
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
                            retorno = retorno + "(" + registro.analise_credito_data + " - "
                                + registro.analise_credito_usuario + ")";
                        }
                    }
                }
            }

            return await Task.FromResult(retorno);
        }

        private async Task<decimal> CalculaSaldoAPagar(string numPedido, decimal vlDevNf)
        {
            //buscar o valor total pago
            var vlFamiliaP = from c in contextoProvider.GetContextoLeitura().TpedidoPagamentos
                             where c.Pedido.StartsWith(numPedido)
                             select c;
            var vl_TotalFamiliaPagoTask = vlFamiliaP.Select(r => r.Valor).SumAsync();

            //buscar valor total NF
            var vlNf = from c in contextoProvider.GetContextoLeitura().TpedidoItems.Include(r => r.Tpedido)
                       where c.Tpedido.St_Entrega != Constantes.Constantes.ST_ENTREGA_CANCELADO && c.Tpedido.Pedido.StartsWith(numPedido)
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
                                      where c.Tpedido.St_Entrega != Constantes.Constantes.ST_ENTREGA_CANCELADO && c.Tpedido.Pedido.StartsWith(numPedido)
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
                             c.Estoque == Constantes.Constantes.ID_ESTOQUE_VENDIDO &&
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
                             c.Estoque == Constantes.Constantes.ID_ESTOQUE_SEM_PRESENCA &&
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
                           c.Nivel_Acesso == Constantes.Constantes.COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__PUBLICO &&
                           c.Anulado_Status == 0
                     select c;

            List<BlocoNotasDtoPedido> lstBlocoNotas = new List<BlocoNotasDtoPedido>();

            foreach (var i in bl)
            {
                BlocoNotasDtoPedido bloco = new BlocoNotasDtoPedido
                {
                    Dt_Hora_Cadastro = i.Dt_Cadastro,
                    Usuario = i.Usuario,
                    Loja = i.Loja,
                    Mensagem = i.Mensagem
                };
                lstBlocoNotas.Add(bloco);
            }

            //BlocoNotasDtoPedido bloco = new BlocoNotasDtoPedido
            //{
            //    Dt_Hora_Cadastro = await bl.Select(r => r.Dt_Hr_Cadastro).FirstOrDefaultAsync(),
            //    Usuario = await bl.Select(r => r.Usuario).FirstOrDefaultAsync(),
            //    Loja = await bl.Select(r => r.Loja).FirstOrDefaultAsync(),
            //    Mensagem = await bl.Select(r => r.Mensagem).FirstOrDefaultAsync()
            //};

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
                case Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA:
                    lista.Add("À Vista (" + Util.Util.OpcaoFormaPagto(parcelamento) + ")");
                    break;
                case Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA:
                    lista.Add("Parcela Única: " + Constantes.Constantes.SIMBOLO_MONETARIO + " " +
                        Util.Util.OpcaoFormaPagto(Convert.ToString(pedido.Pu_Forma_Pagto)) + " vencendo após " + pedido.Pu_Vencto_Apos + " dias");
                    break;
                case Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO:
                    lista.Add("Parcelado no Cartão (internet) em " + pedido.Pc_Qtde_Parcelas + " X " +
                        Constantes.Constantes.SIMBOLO_MONETARIO + " " + pedido.Pc_Valor_Parcela);
                    break;
                case Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA:
                    lista.Add("Parcelado no Cartão (maquineta) em " + pedido.Pc_Maquineta_Qtde_Parcelas + " X " + pedido.Pc_Maquineta_Valor_Parcela);
                    break;
                case Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA:
                    string valor = pedido.Pce_Entrada_Valor?.ToString("#0.00");
                    lista.Add("Entrada: " + Constantes.Constantes.SIMBOLO_MONETARIO + " " +
                        valor + " (" + Util.Util.OpcaoFormaPagto(Convert.ToString(pedido.Pce_Forma_Pagto_Entrada)) + ")");
                    lista.Add("Prestações: " + pedido.Pce_Prestacao_Qtde + " X " + Constantes.Constantes.SIMBOLO_MONETARIO + " " + pedido.Pce_Prestacao_Valor +
                        " (" + Util.Util.OpcaoFormaPagto(Convert.ToString(pedido.Pce_Forma_Pagto_Prestacao)) + ") vencendo a cada " +
                        pedido.Pce_Prestacao_Periodo + " dias");
                    break;
                case Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA:
                    lista.Add("1ª Prestação: " + Constantes.Constantes.SIMBOLO_MONETARIO + " " + pedido.Pse_Prim_Prest_Valor + " (" +
                        Util.Util.OpcaoFormaPagto(Convert.ToString(pedido.Pse_Forma_Pagto_Prim_Prest)) + ") vencendo após " + pedido.Pse_Prim_Prest_Apos + " dias");
                    lista.Add("Demais Prestações: " + pedido.Pse_Demais_Prest_Qtde + " X " + Constantes.Constantes.SIMBOLO_MONETARIO + " " + pedido.Pse_Demais_Prest_Valor +
                        " (" + Util.Util.OpcaoFormaPagto(Convert.ToString(pedido.Pse_Forma_Pagto_Demais_Prest)) + ") vencendo a cada " +
                        pedido.Pse_Demais_Prest_Periodo + " dias");
                    break;
            }

            return await Task.FromResult(lista.ToList());
        }

        private string StatusPagto(string status)
        {
            string retorno = "";

            switch (status)
            {
                case Constantes.Constantes.ST_PAGTO_PAGO:
                    retorno = "Pago";
                    break;
                case Constantes.Constantes.ST_PAGTO_NAO_PAGO:
                    retorno = "Não-Pago";
                    break;
                case Constantes.Constantes.ST_PAGTO_PARCIAL:
                    retorno = "Pago Parcial";
                    break;
            };
            return retorno;
        }

        public async Task<short> Obter_Permite_RA_Status(string apelido)
        {
            var db = contextoProvider.GetContextoLeitura();

            var raStatus = (from c in db.TorcamentistaEindicadors
                            where c.Apelido == apelido
                            select c.Permite_RA_Status).FirstOrDefaultAsync();


            return await raStatus;
        }

        public async Task<PercentualMaxDescEComissao> ObterPercentualMaxDescEComissao(string loja)
        {
            var db = contextoProvider.GetContextoLeitura();

            var ret = from c in db.Tlojas
                      where c.Loja == loja
                      select new PercentualMaxDescEComissao
                      {
                          PercMaxComissao = c.Perc_Max_Comissao,
                          PercMaxComissaoEDesc = c.Perc_Max_Comissao_E_Desconto,
                          PercMaxComissaoEDescNivel2 = c.Perc_Max_Comissao_E_Desconto_Nivel2,
                          PercMaxComissaoEDescPJ = c.Perc_Max_Comissao_E_Desconto_Pj,
                          PercMaxComissaoEDescNivel2PJ = c.Perc_Max_Comissao_E_Desconto_Nivel2_Pj
                      };

            return await ret.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<string>> ValidarIndicador_SelecaoCD(string loja_atual, string idCliente, string usuario_atual,
            string lstOperacoesPermitidas, string cpf_cnpj, int comIndicacao, int cdAutomatico, int cdManual,
            int cdSelecionado, float percComissao, int comRA, string indicador)
        {
            List<string> lstErros = new List<string>();

            //vamos validar os dados
            if (comIndicacao == 1)
            {
                TorcamentistaEindicador torcamentista = new TorcamentistaEindicador();
                if (!string.IsNullOrEmpty(indicador) && comIndicacao == 1)
                {

                    //vamos validar o indicador
                    if (!string.IsNullOrEmpty(indicador))
                    {
                        List<IndicadorDto> lstIndicadores = (await BuscarOrcamentistaEIndicadorListaCompleta(usuario_atual,
                            lstOperacoesPermitidas, loja_atual)).ToList();

                        IndicadorDto indicadorDto = (from c in lstIndicadores
                                                     where c.Apelido == indicador
                                                     select new IndicadorDto
                                                     {
                                                         Apelido = c.Apelido,
                                                         PermiteRA = c.PermiteRA,
                                                         RazaoSocial = c.RazaoSocial
                                                     }).FirstOrDefault();

                        //vamos verificar os dados do indicador
                        if (indicadorDto != null)
                        {
                            if (indicadorDto.Apelido != indicador)
                            {
                                lstErros.Add("O nome do indicador está errado");
                            }

                            torcamentista = await ValidaIndicadorOrcamentista(indicador, lstErros);

                            if (comRA == 1)
                            {
                                if (indicadorDto.PermiteRA == 0)
                                {
                                    lstErros.Add("O Indicador selecionado não permite RA!");
                                }
                            }

                        }

                        if (percComissao > 0)
                        {
                            PercentualMaximoDto percentualMax = (await BuscarPercMaxPorLoja(loja_atual));

                            if (percentualMax != null)
                            {
                                ValidarPercentualRT(percComissao, percentualMax.PercMaxComissao, lstErros);
                            }
                        }
                        //vamos verificar o CD
                        if (cdManual == 1)
                        {
                            //verificamos se tem cd selecionado
                            if (cdSelecionado > 0)
                            {
                                var lstSelecaoCd = (await produtoBll.WmsApelidoEmpresaNfeEmitenteMontaItensSelect(null)).ToList();
                            }
                        }

                    }
                }
            }
            return lstErros;
        }

        public async Task<IEnumerable<string>> PreparaParaCadastrarPedido(string loja, string id_cliente,
            string usuario_atual, string listaOpercoesPermitidas, string cpf_cnpj, PedidoDto pedido)
        {
            List<string> lstErros = new List<string>();

            var db = contextoProvider.GetContextoLeitura();


            //buscar DadosClienteCadastro
            var dadosCliente = db.Tclientes.Where(r => r.Cnpj_Cpf == cpf_cnpj)
                .FirstOrDefault();


            pedido.DadosCliente = clienteBll.ObterDadosClienteCadastro(dadosCliente, loja);


            var percentualMaxTask = ObterPercentualMaxDescEComissao(loja);

            var tparametro = Util.Util.BuscarRegistroParametro(
                Constantes.Constantes.ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto, 
                contextoProvider.GetContextoLeitura());

            //obtem o percPercVlPedidoLimiteRA
            var nsuTask = Util.Util.LeParametroControle(Constantes.Constantes.ID_PARAM_PercVlPedidoLimiteRA, 
                contextoProvider);

            //busca o vendedor externo
            //ao se logar essa session é criada
            //busca de t_usuario
            short? vendedor_externo = (from c in db.Tusuarios
                                       where usuario_atual == c.Usuario
                                       select c.Vendedor_Externo).FirstOrDefault();

            //busca a loja que indicou
            Tloja tloja = (from c in db.Tlojas
                           where c.Loja == loja
                           select c).FirstOrDefault();

            ////le o orçamentista
            ////aqui nós temos qu validar o indicador que foi selecionado 
            //TorcamentistaEindicador torcamentista = new TorcamentistaEindicador();
            //if (!string.IsNullOrEmpty(indicador) && comIndicacao == 1)
            //{
            //    torcamentista = await ValidaIndicadorOrcamentista(indicador, comRA, semRA, lstErros);
            //}
            //if (!string.IsNullOrEmpty(percComissao.ToString()))
            //{
            //    PercentualMaxDescEComissao percentualMax = await percentualMaxTask;
            //    ValidarPercentualRT(percComissao, percentualMax.PercMaxComissao, lstErros);
            //}
            //fazer a verificação de cada um dos produtos selecionados linha 343 ate 427
            List<TprodutoLoja> lstProdutoLoja = (await VerificarProdutosSelecionados(pedido.ListaProdutos, lstErros, loja)).ToList();


            if (ValidarFormaPagto(pedido, lstErros))
            {
                //fazer a verificação do custoFinanFornecTipoParcelamento linha 429 ate 470
                int c_custoFinancFornecQtdeParcelas = ObterQtdeParcelasFormaPagto(pedido);
                string siglaPagto = ObterSiglaFormaPagto(pedido);
                if (Util.Util.ValidarTipoCustoFinanceiroFornecedor(lstErros, siglaPagto, c_custoFinancFornecQtdeParcelas))
                {
                    float coeficiente = await BuscarCoeficientePercentualCustoFinanFornec(pedido,
                        (short)c_custoFinancFornecQtdeParcelas, siglaPagto, lstErros);

                    string tipoPessoa = Util.Util.MultiCdRegraDeterminaPessoa(pedido.DadosCliente.Tipo,
                        pedido.DadosCliente.Contribuinte_Icms_Status, pedido.DadosCliente.ProdutorRural);

                    string descricao = Util.Util.DescricaoMultiCDRegraTipoPessoa(pedido.DadosCliente.Tipo);

                    //fazer a verificação de consumo do estoque linha 489 ate 700
                    //verificar o tipo de seleção do cd
                    //podemos verificar um produto por vez
                    int id_nfe_emitente_selecao_manual;
                    if (pedido.CDManual == 1)
                    {
                        id_nfe_emitente_selecao_manual = pedido.CDSelecionado;
                        if (id_nfe_emitente_selecao_manual == 0)
                        {
                            lstErros.Add("O CD selecionado manualmente é inválido.");
                        }
                    }
                    else
                    {
                        id_nfe_emitente_selecao_manual = 0;
                    }


                    //fazer a verificação quantidade de pedidos que serão cadastrados (auto-split) linha 704 ate 822
                    /*inicializa o campo 'qtde_solicitada', 
                     * pois ele irá controlar a quantidade a ser alocada no estoque de cada empresa
                     */

                    //prepara os produtos
                    List<ProdutoDto> lstProdutosDtoSelecionados = new List<ProdutoDto>();
                    lstProdutosDtoSelecionados = (await BuscarProdutosDtoSelecionados(pedido.ListaProdutos, loja)).ToList();

                    //vamos alterar para verificar a regra de cada produto, utilizando da regra existente ao selecionar um produto na tela
                    //fazer um foreach nos produtos e verificar se concordou em compras produtos mesmo sem presença de estoque
                    foreach (var produto in pedido.ListaProdutos)
                    {
                        ProdutoValidadoComEstoqueDto produtoValidado =
                            await produtoBll.VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado(produto,
                            cpf_cnpj, id_nfe_emitente_selecao_manual);

                        if (produtoValidado.ListaErros.Count > 0)
                        {
                            foreach (var erro in produtoValidado.ListaErros)
                            {
                                if (erro.Contains("PRODUTO SEM PRESENÇA"))
                                {
                                    pedido.OpcaoVendaSemEstoque = true;
                                }
                            }
                        }
                    }

                    //fazer a verificação se tem RA
                    float strPercLimiteRASemDesagio = 0;
                    float strPercDesagio = 0;

                    //if (comRA == 1)
                    //{
                    //    strPercLimiteRASemDesagio = await Util.Util.VerificarSemDesagioRA(contextoProvider);
                    //    strPercDesagio = torcamentista.Perc_Desagio_RA ?? 0;

                    //}
                    //fazer a verificação se tem alerta do produto linha 851 ate 889
                    //já foi feito a verificação de alerta de produtos na rotina abaixo
                    //produtoBll.VerificarRegrasDisponibilidadeEstoqueProdutosSelecionados


                    //podemos montar o dto com as informações existente e armazenar na Session
                    //DadosClienteCadastroDto
                    //FormaPagtoCriacao
                    //List<PedidoProdutosPedidoDto>                    
                    //opção de venda sem estoque
                    //strPercLimiteRASemDesagio
                    //strPercDesagio

                    //DetalhesNFPedidoDtoPedido = esse é o último a ser armazenado
                }
            }

            //vamos verificar as msg que estão retornando
            if (lstErros.Count > 0)
            {
                foreach (var erro in lstErros)
                {
                    if (erro == "PRESENÇA SEM ESTOQUE")
                    {
                        //tem msg de presença
                        if (pedido.OpcaoVendaSemEstoque)
                        {
                            lstErros.Remove(erro);
                        }
                    }
                }

            }

            return lstErros;
        }


        public class CadastrarPedidoRetorno
        {
            public string NumeroPedidoCriado;
            public IEnumerable<string> ListaErros;
        }

        public async Task<CadastrarPedidoRetorno> CadastrarPedido(PedidoDto pedido, string loja, string cpf_cnpj,
            string usuario, int id_nfe_emitente_selecao_manual, bool vendedor_externo, EfetivaPedidoBll efetivaPedido)
        {
            List<string> lstErros = new List<string>();
            var db = contextoProvider.GetContextoLeitura();

            //verifica se o cd é manual ou não
            if (pedido.CDManual == (short)1)
            {
                //pegaria o numero do cd
                TnfEmitente nfEmitente = await (from c in db.TnfEmitentes
                                                where c.Id == pedido.CDSelecionado
                                                select c).FirstOrDefaultAsync();
            }

            //verifica se tem indicador            
            var percentualMaxTask = ObterPercentualMaxDescEComissao(loja);
            PercentualMaxDescEComissao percentualMax = await percentualMaxTask;
            if (pedido.ComIndicador == 1)
            {
                //talvez incluir o garantia indicador no dto PedidoDto
                //pois aqui podemos atribuir o percentual RT
                if (!string.IsNullOrEmpty(pedido.PercRT.ToString()))
                {
                    ValidarPercentualRT((float)pedido.PercRT, percentualMax.PercMaxComissao, lstErros);
                }
            }

            string NumeroPedidoCriado = "";

            //valida forma de pagto já foi analisado
            if (ValidarFormaPagto(pedido, lstErros))
            {
                //Custo finanfornec já foi analisado
                short c_custoFinancFornecQtdeParcelas = (Int16)ObterQtdeParcelasFormaPagto(pedido);
                string c_custoFinancFornecTipoParcelamento = ObterSiglaFormaPagto(pedido);
                bool validouCustoFinanFornec = Util.Util.ValidarTipoCustoFinanceiroFornecedor(lstErros,
                    c_custoFinancFornecTipoParcelamento, c_custoFinancFornecQtdeParcelas);

                bool validouEndEntrega = false;
                //end entrega já foi aramzenado, mas vamos validar 
                if (pedido.EnderecoEntrega != null && pedido.EnderecoEntrega.OutroEndereco)
                {
                    validouEndEntrega = ValidarEndecoEntrega(pedido.EnderecoEntrega, lstErros);
                }
                else
                {
                    validouEndEntrega = true;
                }

                //variavel inicializada que será utilizada mais a frente
                List<string> vdesconto = new List<string>();

                //opcão de venda sem estoque ja foi armazenado


                //detalhes de observações 332 ate 348
                //aqui esta recebendo os dados apenas

                //vendedor externo 350 ate 357
                //aqui esta verificando se existe a session de vendedor externo
                /* se existe a session
                 *      var loja_indicou = loja; e var venda_externa = 1;
                 */

                //busca dados do cliente
                var dadosCliente = db.Tclientes.Where(r => r.Cnpj_Cpf == cpf_cnpj).FirstOrDefault();
                DadosClienteCadastroDto clienteCadastro = clienteBll.ObterDadosClienteCadastro(dadosCliente, loja);
                //aqui iremos receber a midia, indicador_original, tipo_cliente, cep

                //pega percmaxcomissão
                //percentualMax; já foi carregado na linha 1304

                //validação do percRT para saber é maior que o max de comissão já foi realizado caso exista indicador
                //linha 1305 ate 1313

                //le orçamentista : passar indicador
                TorcamentistaEindicador orcamentistaIndicador = await Util.Util.BuscarOrcamentistaEIndicador(
                        pedido.NomeIndicador, contextoProvider.GetContextoLeitura());

                pedido.PermiteRAStatus = orcamentistaIndicador != null ?
                    orcamentistaIndicador.Permite_RA_Status : (short)0;

                //verificar limite mensal de compras do indicador ou RA liquido
                //calcular limite mensal de compras do indicador
                //vl_limite_mensal_disponivel = vl_limite_mensal - vl_limite_mensal_consumido
                float perc_desagio_RA = 0;
                float perc_limite_RA_sem_desagio = 0;
                decimal vl_limite_mensal = 0;
                decimal vl_limite_mensal_consumido = 0;
                decimal vl_limite_mensal_disponivel = 0;
                if (pedido.ComIndicador == 1)
                {
                    //perc_desagio_RA
                    perc_desagio_RA = await Util.Util.ObterPercentualDesagioRAIndicador(pedido.NomeIndicador, contextoProvider);
                    perc_limite_RA_sem_desagio = await Util.Util.VerificarSemDesagioRA(contextoProvider);
                    vl_limite_mensal = await Util.Util.ObterLimiteMensalComprasDoIndicador(pedido.NomeIndicador, contextoProvider);
                    vl_limite_mensal_consumido = await Util.Util.CalcularLimiteMensalConsumidoDoIndicador(pedido.NomeIndicador, DateTime.Now, contextoProvider);
                    vl_limite_mensal_disponivel = vl_limite_mensal - vl_limite_mensal_consumido;
                }

                List<ProdutoValidadoComEstoqueDto> lst_produtoValidado = new List<ProdutoValidadoComEstoqueDto>();


                //verifica se o pedido já foi gravado 463 ate 509
                //Não iremos executar essa verificação, iremos bloquear o click do botão ao clicar uma vez nele

                //verifica o custofiananfornectipo validamos na linha 1321
                // a variavel é "bool validouCustoFinanFornec"

                //calcula valor total do pedido
                decimal vl_total = 0;
                decimal vl_total_NF = 0;
                decimal vl_total_RA = 0;

                if (pedido.ListaProdutos.Count > 0)
                {
                    vl_total = Calcular_Vl_Total(pedido);
                    vl_total_NF = CalcularVl_Total_NF(pedido);
                }

                vl_total_RA = vl_total_NF - vl_total;

                //Percentual de comissão e desconto já foi analisado rever?
                float percDescComissaoUtilizar = 0;
                if (pedido.DadosCliente.Tipo == "PJ")
                {
                    percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                }
                else
                {
                    percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;
                }

                //vamos verificar a forma de pagamento 
                //busca relação de pagto preferenciais (que fazem uso o percentual de comissão+desconto nível 2)
                Tparametro tParametro = await Util.Util.BuscarRegistroParametro(Constantes.Constantes.
                    ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto, contextoProvider.GetContextoLeitura());

                percDescComissaoUtilizar = VerificarPagtoPreferencial(tParametro, pedido, percDescComissaoUtilizar,
                    percentualMax, vl_total);

                //precisa passar os valores para todos os campos da lista de produtos


                //busca produtos , busca percentual custo finananceiro, calcula desconto 716 ate 824
                //desc_dado_arredondado
                //estamos retornando uma lista TpedidoItem com descontos verificados e aplicados
                //aqui esta faltando trazer Tpedido e Tproduto
                List<TpedidoItem> lstPedidoItem = (await VerificarDescontoArredondado(loja, pedido, lstErros,
                    c_custoFinancFornecTipoParcelamento, c_custoFinancFornecQtdeParcelas,
                    percDescComissaoUtilizar, vdesconto)).ToList();

                List<cl_ITEM_PEDIDO_NOVO> v_item = new List<cl_ITEM_PEDIDO_NOVO>();
                //Faz a verificação de regra de cada produto
                //recupera os produtos que concordou mesmo sem estoque 
                List<PedidoProdutosDtoPedido> lstProdutosConcordouSemEstoque = new List<PedidoProdutosDtoPedido>();
                //faz a lógica, regras para consumo do estoque 931 ate 1297                

                //vou buscar a lista de coeficiente para calcular o valor de custoFinacFornec...
                float coeficiente = await BuscarCoeficientePercentualCustoFinanFornec(pedido, c_custoFinancFornecQtdeParcelas,
                    c_custoFinancFornecTipoParcelamento, lstErros);

                short sequencia = 0;
                foreach (var produto in pedido.ListaProdutos)
                {
                    //COMPARAR SE É EXATAMENTE A MESMA REGRA
                    //altera: Qtde_estoque_total_disponivel
                    ProdutoValidadoComEstoqueDto produto_validado_item = new ProdutoValidadoComEstoqueDto();
                    produto_validado_item = await produtoBll.VerificarRegrasDisponibilidadeEstoqueProdutoSelecionado(produto,
                    cpf_cnpj, id_nfe_emitente_selecao_manual);

                    if (produto_validado_item.ListaErros.Count > 0)
                    {
                        foreach (var erro in produto_validado_item.ListaErros)
                        {
                            if (erro.Contains("PRODUTO SEM PRESENÇA"))
                            {
                                lstProdutosConcordouSemEstoque.Add(new PedidoProdutosDtoPedido
                                {
                                    NumProduto = produto_validado_item.Produto.Produto,
                                    Fabricante = produto_validado_item.Produto.Fabricante,
                                    Qtde = produto_validado_item.Produto.QtdeSolicitada,
                                    Qtde_estoque_total_disponivel = (short)produto_validado_item.Produto.Estoque
                                });
                                pedido.OpcaoVendaSemEstoque = true;
                            }
                            //verificar sobre o cd indisponivel
                        }
                    }

                    lst_produtoValidado.Add(produto_validado_item);

                    //verificar aqui

                    foreach (var item in lstPedidoItem)
                    {
                        if (item.Produto == produto.NumProduto && item.Fabricante == produto.Fabricante)
                        {
                            sequencia++;
                            //afazer: arrumar os campos que precisam ser corrigidos 
                            //foi incluido o Tproduto
                            cl_ITEM_PEDIDO_NOVO novoItem = new cl_ITEM_PEDIDO_NOVO();
                            novoItem.produto = produto.NumProduto;
                            novoItem.Fabricante = Util.Util.Normaliza_Codigo(produto.Fabricante, Constantes.Constantes.TAM_MIN_FABRICANTE);
                            novoItem.Qtde = (short)produto.Qtde;
                            novoItem.Preco_Venda = produto.VlUnitario;
                            novoItem.Preco_NF = pedido.PermiteRAStatus == 1 &&
                                           pedido.OpcaoPossuiRA == "S" ? (decimal)produto.Preco_Lista : (decimal)produto.VlUnitario;
                            novoItem.qtde_estoque_total_disponivel = 0;
                            novoItem.Qtde_estoque_vendido = 0;
                            novoItem.Qtde_estoque_sem_presenca = 0;
                            novoItem.Preco_lista = item.Preco_Lista;
                            novoItem.Margem = (float)item.Margem;
                            novoItem.Desc_max = (float)item.Desc_Max;
                            novoItem.Comissao = (float)item.Comissao;
                            novoItem.Preco_fabricante = (decimal)item.Tproduto.Preco_Fabricante;
                            novoItem.Vl_custo2 = item.Tproduto.Vl_Custo2;
                            novoItem.Descricao = item.Tproduto.Descricao;
                            novoItem.Descricao_html = item.Tproduto.Descricao_Html;
                            novoItem.Ean = item.Tproduto.Ean;
                            novoItem.Grupo = item.Tproduto.Grupo;
                            novoItem.Peso = (float)item.Tproduto.Peso;
                            novoItem.Qtde_volumes = (short)item.Tproduto.Qtde_Volumes;
                            novoItem.Markup_fabricante = (float)item.Markup_Fabricante;
                            novoItem.cubagem = item.Cubagem;
                            novoItem.Ncm = item.Tproduto.Ncm;
                            novoItem.Cst = item.Tproduto.Cst;
                            novoItem.Descontinuado = item.Tproduto.Descontinuado;
                            novoItem.CustoFinancFornecPrecoListaBase = item.CustoFinancFornecPrecoListaBase;
                            novoItem.custoFinancFornecCoeficiente = item.CustoFinancFornecCoeficiente;
                            novoItem.Desc_Dado = (float)item.Desc_Dado;
                            novoItem.Abaixo_min_status = item.Abaixo_Min_Status == null ? (short)0 : (short)item.Abaixo_Min_Status;
                            novoItem.Abaixo_min_autorizador = item.Abaixo_Min_Autorizador == null ? "" : item.Abaixo_Min_Autorizador;
                            novoItem.abaixo_min_autorizacao = item.Abaixo_Min_Autorizacao == null ? "" : item.Abaixo_Min_Autorizacao;
                            novoItem.Abaixo_min_superv_autorizador = item.Abaixo_Min_Superv_Autorizador == null ? "" :
                                item.Abaixo_Min_Superv_Autorizador;
                            novoItem.Sequencia = sequencia;

                            v_item.Add(novoItem);

                        }
                    }
                }

                //busca valor de limite para aprovação automática da analise de credito 1300 ate 1307
                string vl_aprov_auto_analise_credito = await Util.Util.LeParametroControle(
                    Constantes.Constantes.ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO, contextoProvider);

                //variavel para verificar se existe lojaIndicação_comissao
                var lojaIndicacao_Comissao = new { loja = "", comissaoIndicacao = (float?)0 };

                //busca percentual de comissão
                if (orcamentistaIndicador != null)
                {
                    lojaIndicacao_Comissao = await (from c in db.Tlojas
                                                    where c.Loja == orcamentistaIndicador.Loja
                                                    select new
                                                    {
                                                        loja = c.Loja,
                                                        comissaoIndicacao = c.Comissao_Indicacao
                                                    }).FirstOrDefaultAsync();

                    //valida indicador
                    //valida garantia indicador, entrega imediata, bem de uso comum  - TELA observações
                    if (lojaIndicacao_Comissao == null || lojaIndicacao_Comissao.comissaoIndicacao == null)
                    {
                        lstErros.Add("Loja " + orcamentistaIndicador.Loja + " não está cadastrada.");
                    }


                }

                if (pedido.ComIndicador == 1)
                {
                    if (string.IsNullOrEmpty(pedido.NomeIndicador))
                    {
                        lstErros.Add("Informe quem é o indicador.");
                    }
                    #region verificação de garantiaIndicador
                    //verifica garantia indicador, mas não utilizamos 
                    //Precisamos verificar?
                    //else if(Garantia == 1)
                    //{

                    //    //aqui "c_ped_bonshop" é o PEDIDO BONSHOP 
                    //    //1729 ate 1766 pag PedidoNovo.asp "var = pedBonshop"
                    //}
                    #endregion
                }

                if (pedido.EnderecoEntrega.OutroEndereco != true && pedido.EnderecoEntrega.OutroEndereco != false)
                {
                    lstErros.Add("É necessário selecionar uma opção para o campo 'Entrega Imediata'.");
                }
                if (pedido.DetalhesNF.StBemUsoConsumo != 1 && pedido.DetalhesNF.StBemUsoConsumo != 0)
                {
                    lstErros.Add("É necessário informar se é 'Bem de Uso/Consumo'.");
                }
                if (pedido.DetalhesNF.InstaladorInstala == short.Parse(Constantes.Constantes.COD_INSTALADOR_INSTALA_NAO_DEFINIDO))
                {
                    lstErros.Add("É necessário preencher o campo 'Instalador Instala'.");
                }

                if (pedido.EnderecoEntrega.OutroEndereco == true)
                {
                    if (string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_cep))
                    {
                        lstErros.Add("Informe o CEP do endereço de entrega.");
                    }
                }

                //consistência do valor total da forma de pagamento
                ValidarFormaPagto(pedido, lstErros);


                //tratamento para cadastramento de pedidos do site magento da bonshop

                //cadastra o pedido 1715 ate 2016
                if (lstErros.Count == 0)
                {
                    //vamos efetivar o cadastro do pedido
                    //vamos abrir uma nova transaction do contexto que esta sendo utilizado para Using

                    using (var dbgravacao = contextoProvider.GetContextoGravacaoParaUsing())
                    {
                        //pegar a qtde_spe;
                        short qtde_spe = BuscarQtdeProdutosASeparar(lstProdutosConcordouSemEstoque, pedido.ListaProdutos, lstErros);

                        int qtdeErros = lstErros.Count;

                        //salvando: Pedido, itens, movimentação de estoque, forma de pagto, 
                        NumeroPedidoCriado = await efetivaPedido.EfetivarCadastroPedido(pedido, lst_produtoValidado,
                            (float)lojaIndicacao_Comissao.comissaoIndicacao, dadosCliente, usuario,
                            perc_limite_RA_sem_desagio, perc_desagio_RA, pedido.PedBonshop, dbgravacao, lstErros, v_item,
                            qtde_spe, vdesconto, vendedor_externo);
                        bool efetivou = !string.IsNullOrWhiteSpace(NumeroPedidoCriado);
                        if (efetivou)
                        {
                            //vamos gravar o Log aqui
                            //monta o log 2691 ate 2881
                            //grava log 
                            //commit
                            await dbgravacao.SaveChangesAsync();
                            await dbgravacao.transacao.CommitAsync();
                        }



                    }
                }
            }
            var cadastrarPedidoRetorno = new CadastrarPedidoRetorno();
            cadastrarPedidoRetorno.ListaErros = lstErros;
            cadastrarPedidoRetorno.NumeroPedidoCriado = NumeroPedidoCriado;
            return cadastrarPedidoRetorno;
        }

        public async Task<decimal> ObtemPercentualVlPedidoRA()
        {
            decimal percentualRA = decimal.Parse(await Util.Util.LeParametroControle(
                Constantes.Constantes.ID_PARAM_PercDesagioRAIndicadorParaCadastroFeitoNaLoja, contextoProvider));

            var db = contextoProvider.GetContextoLeitura();

            string percentual = await (from c in db.Tcontroles
                                       where c.Id_Nsu == Constantes.Constantes.ID_PARAM_PercVlPedidoLimiteRA
                                       select c.Nsu).FirstOrDefaultAsync();

            decimal retorno = decimal.Parse(percentual);

            return retorno;
        }

        public async Task<PercentualMaximoDto> BuscarPercMaxPorLoja(string loja)
        {


            var db = contextoProvider.GetContextoLeitura();

            PercentualMaximoDto retorno = await (from c in db.Tlojas
                                                 where c.Loja == loja
                                                 select new PercentualMaximoDto
                                                 {
                                                     PercMaxComissao = c.Perc_Max_Comissao,
                                                     PercMaxComissaoEDesconto = c.Perc_Max_Comissao_E_Desconto,
                                                     PercMaxComissaoEDescontoPJ = c.Perc_Max_Comissao_E_Desconto_Pj,
                                                     PercMaxComissaoEDescontoNivel2 = c.Perc_Max_Comissao_E_Desconto_Nivel2,
                                                     PercMaxComissaoEDescontoNivel2PJ = c.Perc_Max_Comissao_E_Desconto_Nivel2_Pj
                                                 }).FirstOrDefaultAsync();

            return retorno;
        }


        //retorna qtde a separar
        public short BuscarQtdeProdutosASeparar(List<PedidoProdutosDtoPedido> lstProdutosConcordouSemEstoque,
            List<PedidoProdutosDtoPedido> lstProdutosSelecionados, List<string> lstErros)
        {
            short qtde_spe = 0;
            foreach (var p in lstProdutosSelecionados)
            {
                if (p.Qtde > p.Qtde_estoque_total_disponivel)
                {
                    if (lstProdutosConcordouSemEstoque.Count > 0)
                    {
                        foreach (var produto in lstProdutosConcordouSemEstoque)
                        {
                            if (p.Fabricante == produto.Fabricante && p.NumProduto == produto.NumProduto)
                            {
                                qtde_spe = (short)produto.Qtde_estoque_total_disponivel;
                                break;
                            }
                        }
                        if (qtde_spe != p.Qtde_estoque_total_disponivel)
                        {
                            lstErros.Add("Produto " + p.NumProduto + " do fabricante " + p.Fabricante +
                                ": disponibilidade do estoque foi alterada.");
                        }
                    }
                }
            }

            return qtde_spe;
        }

        //lojaIndicacao_Comissao.comissaoIndicacao = comissaoIndicacao
        //retorna o NUmeroPedidoCriado; se null, deu erro
        //        public async Task<string> EfetivarCadastro(PedidoDto pedido, List<ProdutoValidadoComEstoqueDto> lst_produtoValidado,
        //            float comissaoIndicacao, Tcliente cliente, string usuario_atual, float perc_limite_RA_sem_desagio,
        //            float perc_desagio_RA, string c_ped_bonshop, ContextoBdGravacao dbGravacao, List<string> lstErros,
        //            List<cl_ITEM_PEDIDO_NOVO> v_item, short qtde_spe, List<string> vdesconto, bool vendedor_externo)
        //        {

        //            //vEmpresaAutoSplit preciso dessa lista 
        //            List<string> lista_empresa_selecionada = new List<string>();
        //            List<string> vLogAutoSplit = new List<string>();
        //            int indicePedido = 0;

        //            string retorno = null;
        ////            bool blnAnEnderecoEndEntregaUsaEndParceiro = false;
        ////            bool blnAnalisarEndereco = false;
        ////            bool blnAnEnderecoCadClienteUsaEndParceiro = false;
        ////            bool blnGravouRegPai = false;
        ////            bool blnGravouTransportadora = false;

        ////            string idPedidoBase = "";
        ////            string idPedidoBase_temporario = "";

        ////            bool idPedidoBaseAdd = false;

        ////            indicePedido = 1;//para controlar se é pedido filhote

        ////            //Criando PedidoNovo
        ////            Tpedido pedidonovo = new Tpedido();
        ////            pedidonovo.Loja = pedido.DadosCliente.Loja;
        ////            pedidonovo.Data = DateTime.Now.Date;
        ////            pedidonovo.Hora = DateTime.Now.Hour.ToString().PadLeft(2, '0') +
        ////                DateTime.Now.Month.ToString().PadLeft(2, '0') +
        ////                DateTime.Now.Minute.ToString().PadLeft(2, '0');

        ////            if (indicePedido == 1)
        ////            {
        ////                //pedido base
        ////                #region Pedido sendo Cadastrado
        ////                //Fomra de pagto
        ////                if (produtoValidado.Produto.Lst_empresa_selecionada.Count > 1)
        ////                {
        ////                    pedidonovo.St_Auto_Split = 1;
        ////                }
        ////                if (pedidonovo.St_Pagto != Constantes.Constantes.ST_PAGTO_NAO_PAGO)
        ////                {
        ////                    pedidonovo.Dt_St_Pagto = DateTime.Now.Date;
        ////                    pedidonovo.Dt_Hr_St_Pagto = DateTime.Now;
        ////                    pedidonovo.Usuario_St_Pagto = usuario_atual;
        ////                }

        ////                pedidonovo.St_Pagto = Constantes.Constantes.ST_PAGTO_NAO_PAGO;
        ////                pedidonovo.St_Recebido = pedidonovo.St_Recebido != "" ? pedidonovo.St_Recebido : "";
        ////                pedidonovo.Obs_1 = pedido.DetalhesNF.Observacoes;
        ////                pedidonovo.Obs_2 = pedido.DetalhesNF.NumeroNF;

        ////                /*
        ////                 * alterar a forma como é feito a passagem do campos da forma de pagamento:
        ////                 * pce_entrada_valor /pce_prestacao_valor / pse_prim_prest_valor / pse_demais_prest_valor / pu_valor
        ////                 * 
        ////                 * Quando não utilizados, esta sendo salvo como null
        ////                 * precisamos salvar como 0.00
        ////                 */

        ////                //parcela unica
        ////                pedidonovo.Pu_Valor = pedido.FormaPagtoCriacao.C_pu_valor == null ? 0M : pedido.FormaPagtoCriacao.C_pu_valor; ;
        ////                //pagto com entrada
        ////                pedidonovo.Pce_Entrada_Valor = pedido.FormaPagtoCriacao.C_pce_entrada_valor == null ?
        ////                    0M : pedido.FormaPagtoCriacao.C_pce_entrada_valor;
        ////                pedidonovo.Pce_Prestacao_Valor = pedido.FormaPagtoCriacao.C_pce_prestacao_valor == null ?
        ////                    0M : pedido.FormaPagtoCriacao.C_pce_prestacao_valor;
        ////                //parcela sem entrada
        ////                pedidonovo.Pse_Prim_Prest_Valor = pedido.FormaPagtoCriacao.C_pse_prim_prest_valor == null ?
        ////                    0M : pedido.FormaPagtoCriacao.C_pse_prim_prest_valor;
        ////                pedidonovo.Pse_Demais_Prest_Valor = pedido.FormaPagtoCriacao.C_pse_demais_prest_valor == null ?
        ////                    0M : pedido.FormaPagtoCriacao.C_pse_demais_prest_valor;


        ////                pedidonovo.Tipo_Parcelamento = short.Parse(pedido.FormaPagtoCriacao.Rb_forma_pagto);
        ////                if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA)
        ////                {
        ////                    pedidonovo.Av_Forma_Pagto = short.Parse(pedido.FormaPagtoCriacao.Op_av_forma_pagto);
        ////                    pedidonovo.CustoFinancFornecQtdeParcelas = 0;
        ////                    pedidonovo.Qtde_Parcelas = 1;
        ////                }

        ////                if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
        ////                {
        ////                    pedidonovo.Pu_Forma_Pagto = short.Parse(pedido.FormaPagtoCriacao.Op_pu_forma_pagto);
        ////                    pedidonovo.Pu_Valor = pedido.FormaPagtoCriacao.C_pu_valor;
        ////                    pedidonovo.Pu_Vencto_Apos = (short)pedido.FormaPagtoCriacao.C_pu_vencto_apos;
        ////                    pedidonovo.CustoFinancFornecQtdeParcelas = 1;
        ////                    pedidonovo.Qtde_Parcelas = 1;
        ////                }

        ////                if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
        ////                {
        ////                    pedidonovo.Qtde_Parcelas = (short)pedido.FormaPagtoCriacao.C_pc_qtde;
        ////                    pedidonovo.Pc_Qtde_Parcelas = (short)pedido.FormaPagtoCriacao.C_pc_qtde;
        ////                    pedidonovo.Pc_Valor_Parcela = pedido.FormaPagtoCriacao.C_pc_valor;
        ////                    pedidonovo.CustoFinancFornecQtdeParcelas = (short)pedido.FormaPagtoCriacao.C_pc_qtde;
        ////                }
        ////                else
        ////                {
        ////                    pedidonovo.Pc_Valor_Parcela = 0M;
        ////                }

        ////                if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
        ////                {
        ////                    pedidonovo.Pc_Maquineta_Valor_Parcela = (decimal)pedido.FormaPagtoCriacao.C_pc_maquineta_valor;
        ////                    pedidonovo.Pc_Maquineta_Qtde_Parcelas = (short)pedido.FormaPagtoCriacao.C_pc_maquineta_qtde;
        ////                    pedidonovo.Qtde_Parcelas = (short?)pedido.FormaPagtoCriacao.C_pc_maquineta_qtde;
        ////                    pedidonovo.CustoFinancFornecQtdeParcelas = (short)pedido.FormaPagtoCriacao.C_pc_maquineta_qtde;
        ////                }
        ////                if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
        ////                {
        ////                    pedidonovo.Pce_Forma_Pagto_Entrada = short.Parse(pedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto);
        ////                    pedidonovo.Pce_Forma_Pagto_Prestacao = short.Parse(pedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto);
        ////                    pedidonovo.Pce_Entrada_Valor = pedido.FormaPagtoCriacao.C_pce_entrada_valor;
        ////                    pedidonovo.Pce_Prestacao_Qtde = (short)pedido.FormaPagtoCriacao.C_pce_prestacao_qtde;
        ////                    pedidonovo.Pce_Prestacao_Valor = pedido.FormaPagtoCriacao.C_pce_prestacao_valor;
        ////                    if (pedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "5" &&
        ////                        pedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto != "7")
        ////                        pedidonovo.Pce_Prestacao_Periodo = (short)pedido.FormaPagtoCriacao.C_pce_prestacao_periodo;
        ////                    pedidonovo.Qtde_Parcelas = (short?)(pedido.FormaPagtoCriacao.Qtde_Parcelas);
        ////                    pedidonovo.CustoFinancFornecQtdeParcelas = (short)pedido.FormaPagtoCriacao.C_pce_prestacao_qtde;
        ////                }
        ////                if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
        ////                {
        ////                    pedidonovo.Pse_Forma_Pagto_Prim_Prest = short.Parse(pedido.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto);
        ////                    pedidonovo.Pse_Forma_Pagto_Demais_Prest = short.Parse(pedido.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto);
        ////                    pedidonovo.Pse_Prim_Prest_Valor = pedido.FormaPagtoCriacao.C_pse_prim_prest_valor;
        ////                    pedidonovo.Pse_Prim_Prest_Apos = (short)pedido.FormaPagtoCriacao.C_pse_prim_prest_apos;
        ////                    pedidonovo.Pse_Demais_Prest_Qtde = (short)pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde;
        ////                    pedidonovo.Pse_Demais_Prest_Valor = (decimal)pedido.FormaPagtoCriacao.C_pse_demais_prest_valor;
        ////                    pedidonovo.Pse_Demais_Prest_Periodo = (short)pedido.FormaPagtoCriacao.C_pse_demais_prest_periodo;
        ////                    pedidonovo.Qtde_Parcelas = (short)(pedido.FormaPagtoCriacao.Qtde_Parcelas + 1);
        ////                    pedidonovo.CustoFinancFornecQtdeParcelas = (short)(pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde + 1);
        ////                }

        ////                pedidonovo.Forma_Pagto = pedido.FormaPagtoCriacao.C_forma_pagto;

        ////                pedidonovo.Vl_Total_Familia = (decimal)pedido.VlTotalDestePedido;

        ////                //condições para pedido origem blnPedidoECommerceOrigemMarketplaceCreditoOkAutomatico 

        ////                //afazer: fazer as validações de analise de credito aqui PedidoNovoConfirma linha 1833
        ////                //preciso da variavel "vl_aprov_auto_analise_credito" 
        ////                decimal vl_aprov_auto_analise_credito = decimal.Parse(await ObtemValorLimiteAprovacaoAutomaticaAnaliseDeCredito());

        ////                if (Calcular_Vl_Total(pedido) <= vl_aprov_auto_analise_credito)
        ////                {
        ////                    pedidonovo.Analise_Credito = short.Parse(Constantes.Constantes.COD_AN_CREDITO_OK);
        ////                    pedidonovo.Analise_credito_Data = DateTime.Now;
        ////                    pedidonovo.Analise_Credito_Usuario = "AUTOMÁTICO";
        ////                }
        ////                else if (pedido.DadosCliente.Loja == Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE &&
        ////                    pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA &&
        ////                    pedido.FormaPagtoCriacao.Op_av_forma_pagto == Constantes.Constantes.ID_FORMA_PAGTO_DINHEIRO)
        ////                {
        ////                    pedidonovo.Analise_Credito = short.Parse(Constantes.Constantes.COD_AN_CREDITO_PENDENTE_VENDAS);
        ////                    pedidonovo.Analise_credito_Data = DateTime.Now;
        ////                    pedidonovo.Analise_Credito_Usuario = "AUTOMÁTICO";
        ////                }
        ////                else if (pedido.DadosCliente.Loja == Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE &&
        ////                    pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA &&
        ////                    pedido.FormaPagtoCriacao.Op_av_forma_pagto == Constantes.Constantes.ID_FORMA_PAGTO_BOLETO_AV)
        ////                {
        ////                    pedidonovo.Analise_Credito = short.Parse(Constantes.Constantes.COD_AN_CREDITO_PENDENTE_VENDAS);
        ////                    pedidonovo.Analise_Credito_Pendente_Vendas_Motivo = "006";//aguardando emissão do boleto
        ////                    pedidonovo.Analise_credito_Data = DateTime.Now;
        ////                    pedidonovo.Analise_Credito_Usuario = "AUTOMÁTICO";
        ////                }
        ////                else if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA &&
        ////                    (pedido.FormaPagtoCriacao.Op_av_forma_pagto == Constantes.Constantes.ID_FORMA_PAGTO_DEPOSITO ||
        ////                     pedido.FormaPagtoCriacao.Op_av_forma_pagto == Constantes.Constantes.ID_FORMA_PAGTO_BOLETO_AV))
        ////                {
        ////                    pedidonovo.Analise_Credito = short.Parse(Constantes.Constantes.COD_AN_CREDITO_OK_AGUARDANDO_DEPOSITO);
        ////                    pedidonovo.Analise_credito_Data = DateTime.Now;
        ////                    pedidonovo.Analise_Credito_Usuario = "AUTOMÁTICO";
        ////                }
        ////                else if (pedido.DadosCliente.Loja == Constantes.Constantes.NUMERO_LOJA_TRANSFERENCIA ||
        ////                    pedido.DadosCliente.Loja == Constantes.Constantes.NUMERO_LOJA_KITS)
        ////                {
        ////                    pedidonovo.Analise_Credito = short.Parse(Constantes.Constantes.COD_AN_CREDITO_OK);
        ////                    pedidonovo.Analise_credito_Data = DateTime.Now;
        ////                    pedidonovo.Analise_Credito_Usuario = "AUTOMÁTICO";
        ////                }
        ////                else if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
        ////                {
        ////                    pedidonovo.Analise_Credito = short.Parse(Constantes.Constantes.COD_AN_CREDITO_PENDENTE_VENDAS);
        ////                    pedidonovo.Analise_credito_Data = DateTime.Now;
        ////                    pedidonovo.Analise_Credito_Usuario = "AUTOMÁTICO";
        ////                }

        ////                pedidonovo.Forma_Pagto = "";//c_forma_pagto = Informações sobre analise de crédito em PedidoNovoConsiste 3188

        ////                //Valores
        ////                pedidonovo.CustoFinancFornecTipoParcelamento = ObterSiglaFormaPagto(pedido);
        ////                //afazer: arrumar aqui, pois avista não deve ter valor de 1 e sim de 0
        ////                //pedidonovo.CustoFinancFornecQtdeParcelas = (short)ObterQtdeParcelasFormaPagto(pedido);

        ////                pedidonovo.Vl_Total_NF = CalcularVl_Total_NF(pedido);
        ////                //gabriel aqui
        ////                if (pedido.PermiteRAStatus == 1)
        ////                    pedidonovo.Vl_Total_RA = CalcularVl_Total_NF(pedido) - Calcular_Vl_Total(pedido);
        ////                else
        ////                    pedidonovo.Vl_Total_RA = 0M;

        ////                //decimal teste = Convert.ToDecimal(Constantes.Constantes.PERC_DESAGIO_RA_LIQUIDA);

        ////                //decimal? valorparasubtrair = (
        ////                //    Convert.ToDecimal(Constantes.Constantes.PERC_DESAGIO_RA_LIQUIDA) / 100) * pedidonovo.Vl_Total_RA;

        ////                //decimal? valorRaLiquido = pedidonovo.Vl_Total_RA - valorparasubtrair;

        ////                //pedidonovo.Vl_Total_RA_Liquido = valorRaLiquido != 0 ? Math.Round((decimal)valorRaLiquido, 2): 0;
        ////                //((Constantes.Constantes.PERC_DESAGIO_RA_LIQUIDA / 100) * pedidonovo.Vl_Total_RA);

        ////                pedidonovo.Perc_RT = pedido.PercRT;//receber o percRT novamente pois foi alterado??
        ////                pedidonovo.Perc_Desagio_RA = perc_desagio_RA;
        ////                pedidonovo.Perc_Limite_RA_Sem_Desagio = perc_limite_RA_sem_desagio;

        ////                //se não for pedido filhote vamos salvar alguns campos como vazio
        ////                pedidonovo.Split_Status = 0;
        ////                //pedidonovo.Split_Hora = "";
        ////                pedidonovo.St_Orc_Virou_Pedido = 0;//afazer: isso é salvo quando transformamos Prepedido em Pedido
        ////                pedidonovo.A_Entregar_Status = 0;
        ////                pedidonovo.Vl_Frete = 0M;

        ////            }
        ////            else
        ////            {
        ////                //pedido filhote
        ////                pedidonovo.St_Auto_Split = 1;
        ////                pedidonovo.Split_Status = 1;
        ////                pedidonovo.Split_Data = DateTime.Now.Date;
        ////                pedidonovo.Split_Hora = DateTime.Now.Hour.ToString().PadLeft(2, '0') +
        ////                    DateTime.Now.Month.ToString().PadLeft(2, '0') +
        ////                    DateTime.Now.Minute.ToString().PadLeft(2, '0');
        ////                pedidonovo.Split_Usuario = Constantes.Constantes.ID_USUARIO_SISTEMA;
        ////                pedidonovo.St_Pagto = "";
        ////                pedidonovo.St_Recebido = "";
        ////                pedidonovo.Obs_1 = "";
        ////                pedidonovo.Obs_2 = "";
        ////                pedidonovo.Qtde_Parcelas = 0;
        ////                pedidonovo.Forma_Pagto = "";
        ////            }

        ////            //campos armazenados tanto no pedido - pai quanto no pedido - filhote
        ////            pedidonovo.Id_Cliente = pedido.DadosCliente.Id;
        ////            pedidonovo.Midia = cliente.Midia;
        ////            pedidonovo.Servicos = "";

        ////            //verifica a operação de origem = OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO & blnMagentoPedidoComIndicador
        ////            bool blnMagentoPedidoComIndicador = false;
        ////            //pedidonovo.Vendedor = id do vendedor externo
        ////            //senão for

        ////            //detalhes do pedido
        ////            pedidonovo.Vendedor = usuario_atual;
        ////            pedidonovo.Usuario_Cadastro = usuario_atual;
        ////            pedidonovo.St_Entrega = "";
        ////            pedidonovo.Pedido_Bs_X_At = c_ped_bonshop;
        ////            if (pedido.DetalhesNF.EntregaImediata != "")
        ////            {
        ////                pedidonovo.St_Etg_Imediata = short.Parse(pedido.DetalhesNF.EntregaImediata);
        ////                pedidonovo.Etg_Imediata_Data = DateTime.Now;
        ////                pedidonovo.Etg_Imediata_Usuario = usuario_atual;
        ////            }

        ////            pedidonovo.StBemUsoConsumo = pedido.DetalhesNF.StBemUsoConsumo;
        ////            pedidonovo.InstaladorInstalaStatus = pedido.DetalhesNF.InstaladorInstala;
        ////            pedidonovo.InstaladorInstalaUsuarioUltAtualiz = usuario_atual;
        ////            pedidonovo.InstaladorInstalaDtHrUltAtualiz = DateTime.Now;


        ////            //referente ao magento
        ////            pedidonovo.Pedido_Bs_X_Ac = "";  //s_pedido_ac id do pedido magento
        ////            pedidonovo.Pedido_Bs_X_Marketplace = ""; //s_numero_mktplace
        ////            pedidonovo.Marketplace_codigo_origem = ""; //s_origem_pedido

        ////            //Nota Fiscal
        ////            //constar na nota fiscal a variavel é "c_nf_texto" 
        ////            pedidonovo.Nfe_Texto_Constar = "";
        ////            //verificar, pois no PedidoNovoConsiste é possivel inserir o tetxto o 
        ////            //número de "xPed" a variavel é "c_num_pedido_compra" 
        ////            pedidonovo.Nfe_XPed = "";

        ////            //afazer:loja que indicou só se for vendedor externo
        ////            //preciso da loja que indicou  

        ////            pedidonovo.Loja_Indicou = vendedor_externo == true ? pedido.DadosCliente.Loja : "";
        ////            pedidonovo.Comissao_Loja_Indicou = 0;//comissao_loja_indicou

        ////            pedidonovo.Venda_Externa = vendedor_externo == true ? (short)1 : (short)0;//venda_externa vem da session

        ////            pedidonovo.Indicador = !string.IsNullOrWhiteSpace(pedido.NomeIndicador) ? pedido.NomeIndicador : "";

        ////            //quero ver o pq nao esta sendo salvo corretamente
        ////            pedidonovo.GarantiaIndicadorStatus = pedido.DetalhesNF.GarantiaIndicadorStatus != "0" &&
        ////               pedido.DetalhesNF.GarantiaIndicadorStatus != null ?
        ////               byte.Parse(Constantes.Constantes.COD_GARANTIA_INDICADOR_STATUS__SIM) :
        ////                 byte.Parse(Constantes.Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO);

        ////            pedidonovo.GarantiaIndicadorUsuarioUltAtualiz = usuario_atual;
        ////            pedidonovo.GarantiaIndicadorDtHrUltAtualiz = DateTime.Now;

        ////            pedidonovo.Obs_1 = string.IsNullOrWhiteSpace(pedido.DetalhesNF.Observacoes) ? "" : pedido.DetalhesNF.Observacoes;
        ////            pedidonovo.Obs_2 = string.IsNullOrWhiteSpace(pedido.DetalhesNF.ConstaNaNF) ? "" : pedido.DetalhesNF.ConstaNaNF;

        ////            //Endereço de entrega
        ////            if (pedido.EnderecoEntrega.OutroEndereco == true)
        ////            {
        ////                pedidonovo.EndEtg_Endereco = pedido.EnderecoEntrega.EndEtg_endereco;
        ////                pedidonovo.EndEtg_Endereco_Numero = pedido.EnderecoEntrega.EndEtg_endereco_numero;
        ////                pedidonovo.EndEtg_Endereco_Complemento = pedido.EnderecoEntrega.EndEtg_endereco_complemento;
        ////                pedidonovo.EndEtg_Bairro = pedido.EnderecoEntrega.EndEtg_bairro;
        ////                pedidonovo.EndEtg_Cidade = pedido.EnderecoEntrega.EndEtg_cidade;
        ////                pedidonovo.EndEtg_UF = pedido.EnderecoEntrega.EndEtg_uf;
        ////                pedidonovo.EndEtg_Cep = pedido.EnderecoEntrega.EndEtg_cep.Replace("-", "");
        ////                pedidonovo.EndEtg_Cod_Justificativa = pedido.EnderecoEntrega.EndEtg_cod_justificativa;

        ////                //blnUsarMemorizacaoCompletaEnderecos
        ////                //vamos fazer no util "Util.Util.IsActivatedFlagPedidoUsarMemorizacaoCompletaEnderecos(contextoProvider)"
        ////                //esses campos não fazer parte do Tpedido
        ////                #region memoriza campos de endereço entrega
        ////                //if (await Util.Util.IsActivatedFlagPedidoUsarMemorizacaoCompletaEnderecos(contextoProvider))
        ////                //{
        ////                //    pedidonovo.EndEtg_email = EndEtg_email;
        ////                //    pedidonovo.EndEtg_email_xml = EndEtg_email_xml;
        ////                //    pedidonovo.EndEtg_nome = EndEtg_nome;
        ////                //    pedidonovo.EndEtg_ddd_res = EndEtg_ddd_res;
        ////                //    pedidonovo.EndEtg_tel_res = EndEtg_tel_res;
        ////                //    pedidonovo.EndEtg_ddd_com = EndEtg_ddd_com;
        ////                //    pedidonovo.EndEtg_tel_com = EndEtg_tel_com;
        ////                //    pedidonovo.EndEtg_ramal_com = EndEtg_ramal_com;
        ////                //    pedidonovo.EndEtg_ddd_cel = EndEtg_ddd_cel;
        ////                //    pedidonovo.EndEtg_tel_cel = EndEtg_tel_cel;
        ////                //    pedidonovo.EndEtg_ddd_com_2 = EndEtg_ddd_com_2;
        ////                //    pedidonovo.EndEtg_tel_com_2 = EndEtg_tel_com_2;
        ////                //    pedidonovo.EndEtg_ramal_com_2 = EndEtg_ramal_com_2;
        ////                //    pedidonovo.EndEtg_tipo_pessoa = EndEtg_tipo_pessoa;
        ////                //    pedidonovo.EndEtg_cnpj_cpf = retorna_so_digitos(EndEtg_cnpj_cpf);
        ////                //    pedidonovo.EndEtg_contribuinte_icms_status = converte_numero(EndEtg_contribuinte_icms_status);
        ////                //    pedidonovo.EndEtg_produtor_rural_status = converte_numero(EndEtg_produtor_rural_status);
        ////                //    pedidonovo.EndEtg_ie = EndEtg_ie;
        ////                //    pedidonovo.EndEtg_rg = EndEtg_rg;
        ////                //}
        ////                #endregion
        ////            }

        ////            //01 / 02 / 2018: os pedidos do Arclube usam o RA para incluir o valor do frete e, 
        ////            //portanto, não devem ter deságio do RA
        ////            //afazer: arrumar isso pedidoNovoConfirma linha 1978
        ////            //getParametroPercDesagioRALiquida esta em BDD.asp linha 6148


        ////            string opercao_origem = "";
        ////            if (pedidonovo.Loja != Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE)
        ////            {
        ////                var db = contextoProvider.GetContextoLeitura();
        ////                Tparametro tparametro = await Util.Util.BuscarRegistroParametro(Constantes.Constantes.ID_PARAMETRO_PERC_DESAGIO_RA_LIQUIDA,
        ////                    contextoProvider);
        ////                pedidonovo.Perc_Desagio_RA_Liquida = tparametro.Campo_real;
        ////            }
        ////            //para pedido magento que não esta feito
        ////            #region referenteao magento
        ////            //if(opercao_origem == Constantes.Constantes.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO && !blnMagentoPedidoComIndicador)
        ////            //{
        ////            //    decimal percCommissionValue = 0;
        ////            //    decimal percCommissionDiscount = 0;
        ////            //    decimal vlMagentoShippingAmount = 0;
        ////            //    pedidonovo.Magento_Installer_Comission_Value = percCommissionValue;
        ////            //    pedidonovo.Magento_Installer_Comission_Discount = percCommissionDiscount;
        ////            //    pedidonovo.Magento_Shipping_Amount = vlMagentoShippingAmount;
        ////            //}
        ////            #endregion

        ////            //RA
        ////            pedidonovo.Permite_RA_Status = pedido.PermiteRAStatus;
        ////            pedidonovo.Opcao_Possui_RA = pedido.PermiteRAStatus == 1 ? "S" : "-";

        ////            //Endereço cliente
        ////            pedidonovo.Endereco_Memorizado_Status = 1;
        ////            pedidonovo.Endereco_Logradouro = cliente.Endereco;
        ////            pedidonovo.Endereco_Bairro = cliente.Bairro;
        ////            pedidonovo.Endereco_Cidade = cliente.Cidade;
        ////            pedidonovo.Endereco_Uf = cliente.Uf;
        ////            pedidonovo.Endereco_Cep = cliente.Cep;
        ////            pedidonovo.Endereco_Numero = cliente.Endereco_Numero;
        ////            pedidonovo.Endereco_Complemento = cliente.Endereco_Complemento == null ? "" :
        ////                cliente.Endereco_Complemento;

        ////            //não existe esses campo em Tpedido
        ////            //bool blnUsarMemorizacaoCompletaEnderecos = true;
        ////            #region blnUsarMemorizacaoCompletaEnderecos
        ////            //if (blnUsarMemorizacaoCompletaEnderecos)
        ////            //{
        ////            //    //pedidonovo.st_memorizacao_completa_enderecos = 1;
        ////            //    pedidonovo.endereco_email = cliente.Email;
        ////            //    pedidonovo.endereco_email_xml = cliente.Email_Xml;
        ////            //    pedidonovo.endereco_nome = cliente.Nome;
        ////            //    pedidonovo.endereco_ddd_res = cliente.Ddd_Res;
        ////            //    pedidonovo.endereco_tel_res = cliente.Tel_Res;
        ////            //    pedidonovo.endereco_ddd_com = cliente.Ddd_Com;
        ////            //    pedidonovo.endereco_tel_com = cliente.Tel_Com;
        ////            //    pedidonovo.endereco_ramal_com = cliente.Ramal_Com;
        ////            //    pedidonovo.endereco_ddd_cel = cliente.Ddd_Cel;
        ////            //    pedidonovo.endereco_tel_cel = cliente.Tel_Cel;
        ////            //    pedidonovo.endereco_ddd_com_2 = cliente.Ddd_Com_2;
        ////            //    pedidonovo.endereco_tel_com_2 = cliente.Tel_Com_2;
        ////            //    pedidonovo.endereco_ramal_com_2 = cliente.Ramal_Com_2;
        ////            //    pedidonovo.endereco_tipo_pessoa = cliente.Tipo;
        ////            //    pedidonovo.endereco_cnpj_cpf = cliente.Cnpj_Cpf;
        ////            //    pedidonovo.endereco_contribuinte_icms_status = cliente.Contribuinte_Icms_Status;
        ////            //    pedidonovo.endereco_produtor_rural_status = cliente.Produtor_Rural_Status;
        ////            //    pedidonovo.endereco_ie = cliente.Ie;
        ////            //    pedidonovo.endereco_rg = cliente.Rg;
        ////            //}
        ////            #endregion


        ////            //referente ao magento
        ////            string s_pedido_ac = "";
        ////            if (opercao_origem == Constantes.Constantes.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO ||
        ////                (pedidonovo.Loja) == Constantes.Constantes.NUMERO_LOJA_ECOMMERCE_AR_CLUBE && s_pedido_ac != "")
        ////            {
        ////                pedidonovo.Plataforma_Origem_Pedido = Constantes.Constantes.COD_PLATAFORMA_ORIGEM_PEDIDO__MAGENTO;
        ////            }
        ////            else
        ////            {
        ////                pedidonovo.Plataforma_Origem_Pedido = Constantes.Constantes.COD_PLATAFORMA_ORIGEM_PEDIDO__ERP;
        ////            }

        ////            pedidonovo.Sistema_responsavel_atualizacao = Constantes.Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ERP;
        ////            pedidonovo.Sistema_responsavel_cadastro = Constantes.Constantes.COD_SISTEMA_RESPONSAVEL_CADASTRO__ERP;

        ////            //Emitente NF
        ////            pedidonovo.Id_Nfe_Emitente = (short)item;

        ////            //Numero Pedido Temporario
        ////            //vamos gerar o numero de pedido temporario
        ////            pedidonovo.Pedido = await GerarNumeroPedidoTemporario(lstErros, dbGravacao);

        ////            /*
        ////             * Não posso inserir nesse momento, pois geramos uma chave temporária 
        ////             * que contém mais de 9 caracteres
        ////            */
        ////            //salvar Pedido na base
        ////            dbGravacao.Add(pedidonovo);
        ////            await dbGravacao.SaveChangesAsync();
        ////            #endregion


        ////            int indice_item = 0;

        ////            decimal vl_total_RA_liquido = -1;
        ////            decimal vl_total_RA = 0;

        ////            //vou buscar todas as variaveis abaixo
        ////            //qtde_spe, qtde_estoque_vendido_aux, qtde_estoque_sem_presenca_aux, 
        ////            short total_estoque_vendido = 0;
        ////            short total_estoque_sem_presenca = 0;
        ////            string s_log_item_autosplit = "";

        ////            string s_log_cliente_indicador = "";
        ////            /*lstPedidoItem*/
        ////            //com os itens verificados e preenchidos
        ////            //vamos preparar os tpedidoItem salvar os produto
        ////#pragma warning disable CS0219 // Variable is assigned but its value is never used
        ////            short qtde_estoque_vendido_aux = 0;
        ////            short qtde_estoque_sem_presenca_aux = 0;
        ////#pragma warning restore CS0219 // Variable is assigned but its value is never used
        ////            //montar um objeto com esses 2 parametros
        ////            short[] qtde_estoque_aux = new short[2] { qtde_estoque_vendido_aux = 0, qtde_estoque_sem_presenca_aux = 0 };

        ////            int intQtdeTotalPedidosAnEndereco = 0;

        ////            //esse objeto esta sendo instaciado aqui, para podermos salvar o 
        ////            //pedido com o Id definitivo e no final da rotina
        ////            Tpedido pedidonovoTrocaId = new Tpedido();
        ////            //Verificando os itens e salvando itens, 
        ////            #region Itens do pedido
        ////            foreach (var produtoValidado in lst_produtoValidado)
        ////            {


        ////                foreach (var produto in pedido.ListaProdutos)
        ////                {
        ////                    //aqui vai ser utilizado o cl_ITEM_PEDIDO_NOVO
        ////                    //lista validada mesmo sem presença de estoque
        ////                    foreach (var v in v_item)
        ////                    {
        ////                        if (produto.Fabricante == v.Fabricante && produto.NumProduto == v.produto)
        ////                        {
        ////                            //Lista de regras
        ////                            List<RegrasBll> lstRegras = (await Util.Util.Buscar_IdCDselecionado(produto, cliente, pedido.CDSelecionado,
        ////                                produtoValidado, dbGravacao)).ToList();

        ////                            int sequencia_item = 0;

        ////                            foreach (var r in lstRegras)
        ////                            {
        ////                                if (!string.IsNullOrEmpty(r.Produto))
        ////                                {
        ////                                    foreach (var rCDUfPessoaCD in r.TwmsCdXUfXPessoaXCd)
        ////                                    {
        ////                                        //Cd selecionado
        ////                                        if (rCDUfPessoaCD.Id_nfe_emitente == item)
        ////                                        {
        ////                                            indice_item = -1;
        ////                                            int indiceAuxProduto = 0;
        ////                                            foreach (var p2 in pedido.ListaProdutos)
        ////                                            {
        ////                                                if (p2.Fabricante == rCDUfPessoaCD.Estoque_Fabricante &&
        ////                                                    p2.NumProduto == rCDUfPessoaCD.Estoque_Produto)
        ////                                                {
        ////                                                    indice_item = indiceAuxProduto;
        ////                                                }
        ////                                                indiceAuxProduto++;
        ////                                            }

        ////                                            if (indice_item > -1)
        ////                                            {
        ////                                                sequencia_item++;

        ////                                                v.Pedido = pedidonovo.Pedido;
        ////                                                //inicio teste dbGravacao2
        ////                                                //vamos abrir outra transação aqui


        ////                                                //será necessário atribuir os valores para TpedidoItem
        ////                                                TpedidoItem tpedidoItem = await MontarTpedidoItemParaCadastrar(v);

        ////                                                tpedidoItem.Pedido = pedidonovo.Pedido;

        ////                                                //Salvando Item sem o ID do pedido
        ////                                                dbGravacao.Add(tpedidoItem);
        ////                                                await dbGravacao.SaveChangesAsync();

        ////                                                //vamos fazer a movimentação de estoque
        ////                                                if (rCDUfPessoaCD.Estoque_Qtde_Solicitado > rCDUfPessoaCD.Estoque_Qtde)
        ////                                                {
        ////                                                    qtde_spe = (short)(rCDUfPessoaCD.Estoque_Qtde_Solicitado - rCDUfPessoaCD.Estoque_Qtde);
        ////                                                }
        ////                                                else
        ////                                                {
        ////                                                    qtde_spe = 0;
        ////                                                }

        ////                                                //valida estoque
        ////                                                var validouEstoque = await EstoqueProdutoSaidaV2(
        ////                                                    usuario_atual, pedidonovo.Pedido, (short)item, v.Fabricante,
        ////                                                    v.produto, (short)rCDUfPessoaCD.Estoque_Qtde_Solicitado,
        ////                                                    qtde_spe, qtde_estoque_aux, lstErros, dbGravacao);

        ////                                                if (!validouEstoque)
        ////                                                {
        ////                                                    lstErros.Add(
        ////                                                        Constantes.Constantes.ERR_FALHA_OPERACAO_MOVIMENTO_ESTOQUE);
        ////                                                    return null;
        ////                                                }
        ////                                                //altera lista que concordou mesmo sem presença de estoque
        ////                                                v.Qtde_estoque_vendido = (short)(v.Qtde_estoque_vendido + qtde_estoque_aux[0]);
        ////                                                v.Qtde_estoque_sem_presenca = (short)(v.Qtde_estoque_sem_presenca + qtde_estoque_aux[1]);

        ////                                                total_estoque_vendido += qtde_estoque_aux[0];
        ////                                                total_estoque_sem_presenca += qtde_estoque_aux[1];

        ////                                                if (!string.IsNullOrEmpty(s_log_item_autosplit))
        ////                                                {
        ////                                                    s_log_item_autosplit = s_log_item_autosplit + " ";
        ////                                                }

        ////                                                s_log_item_autosplit = s_log_item_autosplit + "(" + v.Fabricante + ")" +
        ////                                                    v.produto + ":" + " Qtde Solicitada = " +
        ////                                                    rCDUfPessoaCD.Estoque_Qtde_Solicitado + "," +
        ////                                                    " Qtde Sem Presença Autorizada = " + qtde_spe.ToString() + "," +
        ////                                                    " Qtde Estoque Vendido = " + qtde_estoque_aux[0].ToString() + "," +
        ////                                                    " Qtde Sem Presença = " + qtde_estoque_aux[1].ToString();


        ////                                            }
        ////                                        }
        ////                                    }
        ////                                }
        ////                            }//fim da regra
        ////                            if (indicePedido == 1)
        ////                            {
        ////                                //antes de gerar o numero do pedido definitivo, estamos armazenando 
        ////                                //o numero de pedido temporario que foi utilizado para salvar os itens e 
        ////                                idPedidoBase_temporario = pedidonovo.Pedido;
        ////                                // gerar num_pedido
        ////                                if (idPedidoBase == "")
        ////                                    idPedidoBase = await GerarNumeroPedido(lstErros, dbGravacao);

        ////                                if (string.IsNullOrEmpty(idPedidoBase))
        ////                                {
        ////                                    lstErros.Add(Constantes.Constantes.ERR_FALHA_OPERACAO_GERAR_NSU);
        ////                                    return null;
        ////                                }
        ////                            }
        ////                            else
        ////                            {
        ////                                //Gera Pedido filhote
        ////                                idPedidoBase = idPedidoBase + Constantes.Constantes.COD_SEPARADOR_FILHOTE +
        ////                                    Gera_letra_pedido_filhote(indicePedido - 1);
        ////                            }

        ////                            //Log
        ////                            vLogAutoSplit.Add(idPedidoBase + " (" + await Util.Util.ObterApelidoEmpresaNfeEmitentes(
        ////                                item, contextoProvider.GetContextoLeitura()) + ") " + s_log_item_autosplit);


        ////                            //buscando o item para criar um novo com o Id_Pedido definitivo
        ////                            List<TpedidoItem> tpedidoItemset = await (from c in dbGravacao.TpedidoItems
        ////                                                                      where c.Pedido == idPedidoBase_temporario
        ////                                                                      select c).ToListAsync();
        ////                            //TpedidoItem tpedidoItemset = new TpedidoItem();
        ////                            //tpedidoItemset.Pedido = idPedidoBase;
        ////                            //tpedidoItemset.Fabricante = v.Fabricante;
        ////                            //Alterando pedido Item Cadastrado
        ////                            foreach (var itemset in tpedidoItemset)
        ////                            {
        ////                                dbGravacao.Remove(itemset);
        ////                            }

        ////                            //buscando o item para criar um novo com o Id_Pedido definitivo
        ////                            TestoqueMovimento testoqueMovto = await (from c in dbGravacao.TestoqueMovimentos
        ////                                                                     where c.Pedido == idPedidoBase_temporario
        ////                                                                     select c).FirstOrDefaultAsync();
        ////                            //testoqueMovto.Pedido = idPedidoBase;
        ////                            //Alterando estoque movimento Cadastrado
        ////                            dbGravacao.Remove(testoqueMovto);

        ////                            //buscando o item para criar um novo com o Id_Pedido definitivo
        ////                            TestoqueLog testoqueLog = await (from c in dbGravacao.TestoqueLogs
        ////                                                             where c.Pedido_estoque_origem == idPedidoBase_temporario
        ////                                                             select c).FirstOrDefaultAsync();
        ////                            if (testoqueLog != null)
        ////                            {
        ////                                //testoqueLog.Pedido_estoque_origem = idPedidoBase;
        ////                                //alterando estoque log
        ////                                dbGravacao.Remove(testoqueLog);
        ////                            }

        ////                            //buscando o item para criar um novo com o Id_Pedido definitivo
        ////                            TestoqueLog testoqueLog2 = await (from c in dbGravacao.TestoqueLogs
        ////                                                              where c.Pedido_estoque_destino == idPedidoBase_temporario
        ////                                                              select c).FirstOrDefaultAsync();
        ////                            if (testoqueLog2 != null)
        ////                            {
        ////                                //testoqueLog2.Pedido_estoque_destino = idPedidoBase;
        ////                                //alterando estoque log
        ////                                dbGravacao.Remove(testoqueLog2);
        ////                            }

        ////                            //await dbGravacao.SaveChangesAsync();

        ////                            //para alterar o valor da chave primária, precisamos excluir o existente e inserir novamente
        ////                            //excluimos o pedidonovo com o Id temporario
        ////                            dbGravacao.Remove(pedidonovo);
        ////                            await dbGravacao.SaveChangesAsync();

        ////                            //iremos passar os dados dos registros que foram removidos
        ////                            //esse objeto esta sendo instanciado fora do foreach, pois iremos salvar no final da rotina                            
        ////                            pedidonovoTrocaId = pedidonovo;
        ////                            pedidonovoTrocaId.Pedido = idPedidoBase;

        ////                            if (!idPedidoBaseAdd)
        ////                            {
        ////                                //inserimos o pedidonovo com o Id definitivo
        ////                                dbGravacao.Add(pedidonovoTrocaId);
        ////                                await dbGravacao.SaveChangesAsync();

        ////                                idPedidoBaseAdd = true;
        ////                            }


        ////                            List<TpedidoItem> tpedidoItemTrocaId = new List<TpedidoItem>();
        ////                            tpedidoItemTrocaId = tpedidoItemset;
        ////                            foreach (var i in tpedidoItemset)
        ////                            {
        ////                                i.Pedido = idPedidoBase;
        ////                                dbGravacao.Add(i);
        ////                            }

        ////                            TestoqueMovimento testoqueMovtoTrocaId = testoqueMovto; ;
        ////                            testoqueMovtoTrocaId.Pedido = idPedidoBase;
        ////                            dbGravacao.Add(testoqueMovtoTrocaId);

        ////                            if (testoqueLog != null)
        ////                            {
        ////                                TestoqueLog testoqueLogTrocaId = testoqueLog; ;
        ////                                testoqueLogTrocaId.Pedido_estoque_origem = idPedidoBase;
        ////                                dbGravacao.Add(testoqueLogTrocaId);
        ////                            }

        ////                            if (testoqueLog2 != null)
        ////                            {
        ////                                TestoqueLog testoqueLog2TrocaId = testoqueLog2;
        ////                                testoqueLog2TrocaId.Pedido_estoque_destino = idPedidoBase;
        ////                                dbGravacao.Add(testoqueLog2TrocaId);
        ////                            }
        ////                            //salvando alterações
        ////                            await dbGravacao.SaveChangesAsync();




        ////                            //Indicador
        ////                            if (indicePedido == 1)
        ////                            {
        ////                                //indicador: se este pedido é com indicador e o cliente ainda 
        ////                                //não tem um indicador no cadastro, então cadastra este.
        ////                                if (pedido.ComIndicador == 1)
        ////                                {
        ////                                    if (!string.IsNullOrEmpty(pedidonovo.Indicador))
        ////                                    {
        ////                                        pedidonovoTrocaId.Indicador = pedido.NomeIndicador;

        ////                                        //alterando indicado do pedido cadastrado
        ////                                        dbGravacao.Update(pedidonovoTrocaId);
        ////                                        await dbGravacao.SaveChangesAsync();

        ////                                        s_log_cliente_indicador = "Cadastrado o indicador '" + pedido.NomeIndicador +
        ////                                            "' no cliente id=" + pedido.DadosCliente.Id;
        ////                                    }
        ////                                }
        ////                            }

        ////                            //status de entrega
        ////                            string status_entrega = "";
        ////                            if (total_estoque_vendido == 0)
        ////                                status_entrega = Constantes.Constantes.ST_ENTREGA_ESPERAR;
        ////                            else if (total_estoque_sem_presenca == 0)
        ////                                status_entrega = Constantes.Constantes.ST_ENTREGA_SEPARAR;
        ////                            else
        ////                                status_entrega = Constantes.Constantes.ST_ENTREGA_SPLIT_POSSIVEL;

        ////                            pedidonovoTrocaId.St_Entrega = status_entrega;

        ////                            //alterando status do pedido cadastrado
        ////                            dbGravacao.Update(pedidonovoTrocaId);
        ////                            await dbGravacao.SaveChangesAsync();
        ////                            if (pedido.PermiteRAStatus == 1)
        ////                            {
        ////                                //calcula total ra liquido bd 
        ////                                vl_total_RA_liquido = await CalculaTotalRALiquidoBD(idPedidoBase, dbGravacao, lstErros);
        ////                                if (vl_total_RA_liquido == -1)
        ////                                {
        ////                                    return null;
        ////                                }

        ////                                //RA
        ////                                if (indicePedido == 1)
        ////                                {
        ////                                    pedidonovoTrocaId.Vl_Total_RA_Liquido = Math.Round(vl_total_RA_liquido, 2);
        ////                                    pedidonovoTrocaId.Qtde_Parcelas_Desagio_RA = 0;

        ////                                    if (pedidonovo.Vl_Total_RA != 0)
        ////                                        pedidonovoTrocaId.St_Tem_Desagio_RA = 1;
        ////                                    else
        ////                                        pedidonovoTrocaId.St_Tem_Desagio_RA = 0;

        ////                                    //alterando RA e desagio pedido cadastrado
        ////                                    dbGravacao.Update(pedidonovoTrocaId);
        ////                                    await dbGravacao.SaveChangesAsync();
        ////                                }
        ////                            }

        ////                            //Desconto
        ////                            if (indicePedido == 1)
        ////                            {
        ////                                //senhas de autorização para desconto superior
        ////                                foreach (var d in vdesconto)
        ////                                {
        ////                                    if (!string.IsNullOrEmpty(d))
        ////                                    {
        ////                                        Tdesconto tdesconto = await (from c in dbGravacao.Tdescontos
        ////                                                                     where c.Usado_status == 0 &&
        ////                                                                           c.Cancelado_status == 0 &&
        ////                                                                           c.Id == d
        ////                                                                     select c).FirstOrDefaultAsync();

        ////                                        if (tdesconto == null)
        ////                                        {
        ////                                            lstErros.Add("Senha de autorização para desconto superior não encontrado.");
        ////                                            return null;
        ////                                        }
        ////                                        else
        ////                                        {
        ////                                            tdesconto.Usado_status = 1;
        ////                                            tdesconto.Usado_data = DateTime.Now;
        ////                                            if (opercao_origem == Constantes.Constantes.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO &&
        ////                                                blnMagentoPedidoComIndicador)
        ////                                            {
        ////                                                tdesconto.Vendedor = usuario_atual;
        ////                                            }

        ////                                            tdesconto.Usado_usuario = usuario_atual;

        ////                                            //alterando tabela de desconto
        ////                                            dbGravacao.Update(tdesconto);
        ////                                            await dbGravacao.SaveChangesAsync();
        ////                                        }
        ////                                    }
        ////                                }
        ////                            }

        ////                            int intNsuPai = 0;
        ////                            int intNsu = 0;
        ////                            //VERIFICA SE O ENDEREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE(POSSÍVEL FRAUDE)
        ////                            //ENDEREÇO DO CADASTRO linha 2264
        ////                            if (indicePedido == 1)
        ////                            {
        ////                                if (lstErros.Count == 0)
        ////                                {
        ////                                    intQtdeTotalPedidosAnEndereco = 0;
        ////                                    //1) verifica se o endereço usado é o do parceiro
        ////                                    if (pedido.ComIndicador == 1)
        ////                                    {
        ////                                        if (!string.IsNullOrEmpty(pedidonovoTrocaId.Indicador))
        ////                                        {
        ////                                            //buscar orçamentista para comparar
        ////                                            TorcamentistaEindicador torcamentista = await (from c in dbGravacao.TorcamentistaEindicadors
        ////                                                                                           where c.Apelido == pedidonovo.Indicador
        ////                                                                                           select c).FirstOrDefaultAsync();
        ////                                            //verificar se o endereço é igual
        ////                                            //retorna bool
        ////                                            //CompararEndereco do cadastro do cliente com o orçamentista aqui
        ////                                            if (CompararEnderecoParceiro(pedidonovoTrocaId.Endereco_Logradouro,
        ////                                                int.Parse(pedidonovoTrocaId.Endereco_Numero),
        ////                                                int.Parse(pedidonovo.Endereco_Cep.Replace("-", "")),
        ////                                                torcamentista.Endereco,
        ////                                                int.Parse(torcamentista.Endereco_Numero),
        ////                                                int.Parse(torcamentista.Cep.Replace("-", ""))))
        ////                                            {
        ////                                                blnAnEnderecoCadClienteUsaEndParceiro = true;
        ////                                                blnAnalisarEndereco = true;

        ////                                                //gerar fin_gera_nsu
        ////                                                intNsuPai = await Fin_gera_nsu(Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO,
        ////                                                    lstErros, dbGravacao);

        ////                                                if (intNsuPai == 0)
        ////                                                {
        ////                                                    lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO(" + lstErros.Last() + ")");
        ////                                                }
        ////                                                else
        ////                                                {
        ////                                                    TpedidoAnaliseEndereco tpedidoAnaliseEnd = new TpedidoAnaliseEndereco();
        ////                                                    tpedidoAnaliseEnd.Id = intNsuPai;
        ////                                                    tpedidoAnaliseEnd.Pedido = idPedidoBase;
        ////                                                    tpedidoAnaliseEnd.Id_cliente = pedidonovoTrocaId.Id_Cliente;
        ////                                                    tpedidoAnaliseEnd.Tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE;
        ////                                                    tpedidoAnaliseEnd.Endereco_logradouro = pedidonovoTrocaId.Endereco_Logradouro;
        ////                                                    tpedidoAnaliseEnd.Endereco_bairro = pedidonovoTrocaId.Endereco_Bairro;
        ////                                                    tpedidoAnaliseEnd.Endereco_cidade = pedidonovoTrocaId.Endereco_Cidade;
        ////                                                    tpedidoAnaliseEnd.Endereco_uf = pedidonovoTrocaId.Endereco_Uf;
        ////                                                    tpedidoAnaliseEnd.Endereco_cep = pedidonovoTrocaId.Endereco_Cep;
        ////                                                    tpedidoAnaliseEnd.Endereco_numero = pedidonovoTrocaId.Endereco_Numero;
        ////                                                    tpedidoAnaliseEnd.Endereco_complemento = pedidonovoTrocaId.Endereco_Complemento;
        ////                                                    tpedidoAnaliseEnd.Usuario_cadastro = pedidonovoTrocaId.Usuario_Cadastro;

        ////                                                    dbGravacao.Add(tpedidoAnaliseEnd);
        ////                                                    await dbGravacao.SaveChangesAsync();
        ////                                                }
        ////                                            }
        ////                                        }
        ////                                    }
        ////                                }
        ////                                if (lstErros.Count == 0)
        ////                                {
        ////                                    //afazer: arrumar o selectabaixo para 
        ////                                    //2)verifica pedidos de outros clientes
        ////                                    if (!blnAnEnderecoCadClienteUsaEndParceiro)
        ////                                    {
        ////                                        List<cl_ANALISE_ENDERECO_CONFRONTACAO> vAnEndConfrontacao = new List<cl_ANALISE_ENDERECO_CONFRONTACAO>();

        ////                                        var tpedidoCli_St_0Task = from c in dbGravacao.Tpedidos.Include(x => x.Tcliente)
        ////                                                                  where c.Endereco_Memorizado_Status == 0 &&
        ////                                                                        c.Tcliente.Id != cliente.Id &&
        ////                                                                        c.Tcliente.Cep == cliente.Cep.Replace("-", "").Trim()
        ////                                                                  select new
        ////                                                                  {
        ////                                                                      t = c,
        ////                                                                      tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE
        ////                                                                  };


        ////                                        var tpedido_St_1Task = from c in dbGravacao.Tpedidos
        ////                                                               where c.Endereco_Memorizado_Status == 1 &&
        ////                                                                     c.Id_Cliente != cliente.Id &&
        ////                                                                     c.Endereco_Cep == cliente.Cep
        ////                                                               select new
        ////                                                               {
        ////                                                                   t = c,
        ////                                                                   tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE_MEMORIZADO
        ////                                                               };

        ////                                        var tpedido_St_Entrega_1Task = from c in dbGravacao.Tpedidos
        ////                                                                       where c.St_End_Entrega == 1 &&
        ////                                                                             c.Id_Cliente != cliente.Id &&
        ////                                                                             c.EndEtg_Cep == pedidonovo.EndEtg_Cep
        ////                                                                       select new
        ////                                                                       {
        ////                                                                           t = c,
        ////                                                                           tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__END_ENTREGA
        ////                                                                       };


        ////                                        var tpedidoUnion1 = tpedidoCli_St_0Task.ToList();
        ////                                        var tpedidoUnion2 = tpedido_St_1Task.ToList();
        ////                                        var tpedidoUnion3 = tpedido_St_Entrega_1Task.ToList();

        ////                                        var unionAll = (tpedidoUnion1
        ////                                            .Union(tpedidoUnion2)
        ////                                            .Union(tpedidoUnion3)
        ////                                            .Distinct().OrderByDescending(x => x.t.Data_Hora)).ToList();

        ////                                        foreach (var allPed in unionAll)
        ////                                        {
        ////                                            if (CompararEnderecoParceiro(cliente.Endereco,
        ////                                                int.Parse(cliente.Endereco_Numero), int.Parse(cliente.Cep),
        ////                                                allPed.t.Endereco_Logradouro, int.Parse(allPed.t.Endereco_Numero),
        ////                                                int.Parse(allPed.t.Endereco_Cep)))
        ////                                            {
        ////                                                //if (vAnEndConfrontacao.Count != 0)
        ////                                                //{
        ////                                                vAnEndConfrontacao.Add(new cl_ANALISE_ENDERECO_CONFRONTACAO
        ////                                                {
        ////                                                    Pedido = allPed.t.Pedido,
        ////                                                    Id_cliente = allPed.t.Id_Cliente,
        ////                                                    Tipo_endereco = allPed.tipo_endereco,
        ////                                                    Endereco_logradouro = allPed.t.Endereco_Logradouro,
        ////                                                    Endereco_bairro = allPed.t.Endereco_Bairro,
        ////                                                    Endereco_cidade = allPed.t.Endereco_Cidade,
        ////                                                    Endereco_uf = allPed.t.Endereco_Uf,
        ////                                                    Endereco_cep = allPed.t.Endereco_Cep,
        ////                                                    Endereco_numero = allPed.t.Endereco_Numero,
        ////                                                    Endereco_complemento = allPed.t.Endereco_Complemento
        ////                                                });

        ////                                                intQtdeTotalPedidosAnEndereco++;
        ////                                                if (intQtdeTotalPedidosAnEndereco >=
        ////                                                    Constantes.Constantes.MAX_AN_ENDERECO_QTDE_PEDIDOS_CADASTRAMENTO)
        ////                                                {
        ////                                                    break;
        ////                                                }
        ////                                                //}
        ////                                            }
        ////                                        }

        ////                                        blnGravouRegPai = false;

        ////                                        foreach (var i in vAnEndConfrontacao)
        ////                                        {
        ////                                            if (!string.IsNullOrEmpty(i.Pedido))
        ////                                            {
        ////                                                blnAnalisarEndereco = true;
        ////                                                //já gravou o registro pai ?
        ////                                                if (!blnGravouRegPai)
        ////                                                {
        ////                                                    blnGravouRegPai = true;
        ////                                                    intNsuPai = await Fin_gera_nsu(Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO,
        ////                                                        lstErros, dbGravacao);

        ////                                                    if (intNsuPai == 0)
        ////                                                    {
        ////                                                        lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO (" + lstErros.Last() + ")");
        ////                                                        return null;
        ////                                                    }
        ////                                                    else
        ////                                                    {
        ////                                                        TpedidoAnaliseEndereco tpedidoEndAnalise = new TpedidoAnaliseEndereco
        ////                                                        {

        ////                                                            Id = intNsuPai,
        ////                                                            Pedido = pedidonovo.Pedido,
        ////                                                            Id_cliente = cliente.Id,
        ////                                                            Tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE,
        ////                                                            Endereco_logradouro = cliente.Endereco,
        ////                                                            Endereco_bairro = cliente.Bairro,
        ////                                                            Endereco_cidade = cliente.Cidade,
        ////                                                            Endereco_cep = cliente.Cep,
        ////                                                            Endereco_uf = cliente.Uf,
        ////                                                            Endereco_numero = cliente.Endereco_Numero,
        ////                                                            Endereco_complemento = cliente.Endereco_Complemento,
        ////                                                            Dt_cadastro = DateTime.Now.Date,
        ////                                                            Dt_hr_cadastro = DateTime.Now,
        ////                                                            Usuario_cadastro = usuario_atual.ToUpper()
        ////                                                        };

        ////                                                        dbGravacao.Add(tpedidoEndAnalise);
        ////                                                        await dbGravacao.SaveChangesAsync();
        ////                                                    }
        ////                                                }

        ////                                                intNsu = await Fin_gera_nsu(Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO,
        ////                                                    lstErros, dbGravacao);

        ////                                                if (intNsu == 0)
        ////                                                {
        ////                                                    lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO (" + lstErros.Last() + ")");
        ////                                                    return null;
        ////                                                }
        ////                                                else
        ////                                                {
        ////                                                    TpedidoAnaliseEnderecoConfrontacao tpedidoAnaliseConfrontacao = new TpedidoAnaliseEnderecoConfrontacao
        ////                                                    {
        ////                                                        Id = intNsu,
        ////                                                        Id_pedido_analise_endereco = intNsuPai,
        ////                                                        Pedido = i.Pedido,
        ////                                                        Id_cliente = i.Id_cliente,
        ////                                                        Tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE,
        ////                                                        Endereco_logradouro = i.Endereco_logradouro,
        ////                                                        Endereco_bairro = i.Endereco_bairro,
        ////                                                        Endereco_cidade = i.Endereco_cidade,
        ////                                                        Endereco_cep = i.Endereco_cep,
        ////                                                        Endereco_uf = i.Endereco_uf,
        ////                                                        Endereco_numero = i.Endereco_numero,
        ////                                                        Endereco_complemento = i.Endereco_complemento
        ////                                                    };

        ////                                                    dbGravacao.Add(tpedidoAnaliseConfrontacao);
        ////                                                    await dbGravacao.SaveChangesAsync();
        ////                                                }
        ////                                            }
        ////                                        }
        ////                                    }

        ////                                    if (lstErros.Count == 0)
        ////                                    {
        ////                                        //endereço de entrega(se houver)
        ////                                        if (pedido.EnderecoEntrega.OutroEndereco)
        ////                                        {
        ////                                            //1) verifica se o endereço usado é o do parceiro
        ////                                            if (!string.IsNullOrEmpty(pedidonovoTrocaId.Indicador))
        ////                                            {
        ////                                                //vamos buscar orçamentista para comparar
        ////                                                //buscar orçamentista para comparar
        ////                                                TorcamentistaEindicador torcamentista = await (from c in dbGravacao.TorcamentistaEindicadors
        ////                                                                                               where c.Apelido == pedidonovoTrocaId.Indicador
        ////                                                                                               select c).FirstOrDefaultAsync();
        ////                                                //verificar se o endereço é igual
        ////                                                //retorna bool
        ////                                                if (CompararEnderecoParceiro(pedidonovo.EndEtg_Endereco, int.Parse(pedidonovoTrocaId.EndEtg_Endereco_Numero),
        ////                                                    int.Parse(pedidonovoTrocaId.EndEtg_Cep.Replace("-", "")), torcamentista.Endereco,
        ////                                                    int.Parse(torcamentista.Endereco_Numero),
        ////                                                    int.Parse(torcamentista.Cep.Replace("-", ""))))
        ////                                                {

        ////                                                    blnAnEnderecoEndEntregaUsaEndParceiro = true;
        ////                                                    blnAnalisarEndereco = true;

        ////                                                    intNsuPai = await Fin_gera_nsu(Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO,
        ////                                                                lstErros, dbGravacao);

        ////                                                    if (intNsuPai == 0)
        ////                                                    {
        ////                                                        lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO (" + lstErros.Last() + ")");
        ////                                                        return null;
        ////                                                    }
        ////                                                    else
        ////                                                    {
        ////                                                        intNsu = await Fin_gera_nsu(Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO,
        ////                                                                lstErros, dbGravacao);

        ////                                                        TpedidoAnaliseEndereco tpedidoAnaliseEnd = new TpedidoAnaliseEndereco();

        ////                                                        tpedidoAnaliseEnd.Id = intNsuPai;
        ////                                                        tpedidoAnaliseEnd.Pedido = idPedidoBase;
        ////                                                        tpedidoAnaliseEnd.Id_cliente = pedidonovoTrocaId.Id_Cliente;
        ////                                                        tpedidoAnaliseEnd.Tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__END_ENTREGA;
        ////                                                        tpedidoAnaliseEnd.Endereco_logradouro = pedidonovoTrocaId.EndEtg_Endereco;
        ////                                                        tpedidoAnaliseEnd.Endereco_bairro = pedidonovoTrocaId.EndEtg_Bairro;
        ////                                                        tpedidoAnaliseEnd.Endereco_cidade = pedidonovoTrocaId.EndEtg_Cidade;
        ////                                                        tpedidoAnaliseEnd.Endereco_uf = pedidonovoTrocaId.EndEtg_UF;
        ////                                                        tpedidoAnaliseEnd.Endereco_cep = pedidonovoTrocaId.EndEtg_Cep;
        ////                                                        tpedidoAnaliseEnd.Endereco_numero = pedidonovoTrocaId.EndEtg_Endereco_Numero;
        ////                                                        tpedidoAnaliseEnd.Endereco_complemento = pedidonovoTrocaId.EndEtg_Endereco_Complemento;
        ////                                                        tpedidoAnaliseEnd.Usuario_cadastro = usuario_atual;

        ////                                                        dbGravacao.Add(tpedidoAnaliseEnd);
        ////                                                        //await dbGravacao.SaveChangesAsync();
        ////                                                    }

        ////                                                    if (lstErros.Count == 0)
        ////                                                    {
        ////                                                        intNsu = await Fin_gera_nsu(Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO,
        ////                                                                lstErros, dbGravacao);

        ////                                                        if (intNsuPai == 0)
        ////                                                        {
        ////                                                            lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO (" + lstErros.Last() + ")");
        ////                                                            return null;
        ////                                                        }
        ////                                                        else
        ////                                                        {
        ////                                                            TpedidoAnaliseEnderecoConfrontacao tpedidoAnaliseConfrontacao = new TpedidoAnaliseEnderecoConfrontacao
        ////                                                            {
        ////                                                                Id = intNsu,
        ////                                                                Id_pedido_analise_endereco = intNsuPai,
        ////                                                                Pedido = "",
        ////                                                                Id_cliente = "",
        ////                                                                Tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__END_PARCEIRO,
        ////                                                                Endereco_logradouro = torcamentista.Endereco,
        ////                                                                Endereco_bairro = torcamentista.Bairro,
        ////                                                                Endereco_cidade = torcamentista.Cidade,
        ////                                                                Endereco_cep = torcamentista.Cep,
        ////                                                                Endereco_uf = torcamentista.Uf,
        ////                                                                Endereco_numero = torcamentista.Endereco_Numero,
        ////                                                                Endereco_complemento = torcamentista.Endereco_Complemento
        ////                                                            };

        ////                                                            dbGravacao.Add(tpedidoAnaliseConfrontacao);
        ////                                                            //await dbGravacao.SaveChangesAsync();
        ////                                                        }
        ////                                                    }
        ////                                                }
        ////                                            }

        ////                                            //2)verifica pedidos de outros clientes
        ////                                            if (lstErros.Count == 0)
        ////                                            {
        ////                                                if (!blnAnEnderecoEndEntregaUsaEndParceiro)
        ////                                                {
        ////                                                    //vAnEndConfrontacao
        ////                                                    List<cl_ANALISE_ENDERECO_CONFRONTACAO> vAnEndConfrontacao = new List<cl_ANALISE_ENDERECO_CONFRONTACAO>();
        ////                                                    intQtdeTotalPedidosAnEndereco = 0;

        ////                                                    var tpedidoCli_St_0Task = from c in dbGravacao.Tpedidos.Include(x => x.Tcliente)
        ////                                                                              where c.Endereco_Memorizado_Status == 0 &&
        ////                                                                                    c.Tcliente.Id == cliente.Id &&
        ////                                                                                    c.Tcliente.Cep == cliente.Cep.Replace("-", "").Trim()
        ////                                                                              select new
        ////                                                                              {
        ////                                                                                  t = c,
        ////                                                                                  tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE
        ////                                                                              };
        ////                                                    var tpedido_St_1Task = from c in dbGravacao.Tpedidos
        ////                                                                           where c.Endereco_Memorizado_Status == 1 &&
        ////                                                                                 c.Id_Cliente == cliente.Id &&
        ////                                                                                 c.EndEtg_Cep == cliente.Cep
        ////                                                                           select new
        ////                                                                           {
        ////                                                                               t = c,
        ////                                                                               tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE_MEMORIZADO
        ////                                                                           };
        ////                                                    var tpedido_St_Entrega_1Task = from c in dbGravacao.Tpedidos
        ////                                                                                   where c.St_End_Entrega == 1 &&
        ////                                                                                         c.Id_Cliente == cliente.Id &&
        ////                                                                                         c.EndEtg_Cep == pedidonovoTrocaId.EndEtg_Cep
        ////                                                                                   select new
        ////                                                                                   {
        ////                                                                                       t = c,
        ////                                                                                       tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__END_ENTREGA
        ////                                                                                   };

        ////                                                    var tpedidoUnion1 = tpedidoCli_St_0Task.ToList();
        ////                                                    var tpedidoUnion2 = tpedido_St_1Task.ToList();
        ////                                                    var tpedidoUnion3 = tpedido_St_Entrega_1Task.ToList();

        ////                                                    var unionAll = (tpedidoUnion1
        ////                                                        .Union(tpedidoUnion2)
        ////                                                        .Union(tpedidoUnion3)
        ////                                                        .Distinct().OrderByDescending(x => x.t.Data_Hora)).ToList();

        ////                                                    foreach (var allPed in unionAll)
        ////                                                    {
        ////                                                        if (CompararEnderecoParceiro(pedido.EnderecoEntrega.EndEtg_endereco,
        ////                                                            int.Parse(pedido.EnderecoEntrega.EndEtg_endereco_numero),
        ////                                                            int.Parse(pedido.EnderecoEntrega.EndEtg_cep),
        ////                                                            allPed.t.Endereco_Logradouro, int.Parse(allPed.t.Endereco_Numero),
        ////                                                            int.Parse(allPed.t.Endereco_Cep)))
        ////                                                        {
        ////                                                            if (vAnEndConfrontacao.Count != 0)
        ////                                                            {
        ////                                                                vAnEndConfrontacao.Add(new cl_ANALISE_ENDERECO_CONFRONTACAO
        ////                                                                {
        ////                                                                    Pedido = allPed.t.Pedido,
        ////                                                                    Id_cliente = allPed.t.Id_Cliente,
        ////                                                                    Tipo_endereco = allPed.tipo_endereco,
        ////                                                                    Endereco_logradouro = allPed.t.Endereco_Logradouro,
        ////                                                                    Endereco_bairro = allPed.t.Endereco_Bairro,
        ////                                                                    Endereco_cidade = allPed.t.Endereco_Cidade,
        ////                                                                    Endereco_uf = allPed.t.Endereco_Uf,
        ////                                                                    Endereco_cep = allPed.t.Endereco_Cep,
        ////                                                                    Endereco_numero = allPed.t.Endereco_Numero,
        ////                                                                    Endereco_complemento = allPed.t.Endereco_Complemento
        ////                                                                });

        ////                                                                intQtdeTotalPedidosAnEndereco++;
        ////                                                                if (intQtdeTotalPedidosAnEndereco >=
        ////                                                                    Constantes.Constantes.MAX_AN_ENDERECO_QTDE_PEDIDOS_CADASTRAMENTO)
        ////                                                                {
        ////                                                                    break;
        ////                                                                }
        ////                                                            }
        ////                                                        }
        ////                                                    }

        ////                                                    blnGravouRegPai = false;

        ////                                                    foreach (var i in vAnEndConfrontacao)
        ////                                                    {
        ////                                                        if (!string.IsNullOrEmpty(i.Pedido))
        ////                                                        {
        ////                                                            blnAnalisarEndereco = true;
        ////                                                            //já gravou o registro pai ?
        ////                                                            if (!blnGravouRegPai)
        ////                                                            {
        ////                                                                blnGravouRegPai = true;
        ////                                                                intNsuPai = await Fin_gera_nsu(Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO,
        ////                                                                    lstErros, dbGravacao);

        ////                                                                if (intNsuPai == 0)
        ////                                                                {
        ////                                                                    lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO (" + lstErros.Last() + ")");
        ////                                                                    return null;
        ////                                                                }
        ////                                                                else
        ////                                                                {
        ////                                                                    TpedidoAnaliseEndereco tpedidoEndAnalise = new TpedidoAnaliseEndereco
        ////                                                                    {

        ////                                                                        Id = intNsuPai,
        ////                                                                        Pedido = pedidonovo.Pedido,
        ////                                                                        Id_cliente = cliente.Id,
        ////                                                                        Tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE,
        ////                                                                        Endereco_logradouro = cliente.Endereco,
        ////                                                                        Endereco_bairro = cliente.Bairro,
        ////                                                                        Endereco_cidade = cliente.Cidade,
        ////                                                                        Endereco_cep = cliente.Cep,
        ////                                                                        Endereco_uf = cliente.Uf,
        ////                                                                        Endereco_numero = cliente.Endereco_Numero,
        ////                                                                        Endereco_complemento = cliente.Endereco_Complemento
        ////                                                                    };

        ////                                                                    dbGravacao.Add(tpedidoEndAnalise);
        ////                                                                    //await dbGravacao.SaveChangesAsync();
        ////                                                                }
        ////                                                            }

        ////                                                            intNsu = await Fin_gera_nsu(Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO,
        ////                                                                lstErros, dbGravacao);

        ////                                                            if (intNsu == 0)
        ////                                                            {
        ////                                                                lstErros.Add("FALHA AO GERAR NSU PARA O NOVO REGISTRO (" + lstErros.Last() + ")");
        ////                                                                return null;
        ////                                                            }
        ////                                                            else
        ////                                                            {
        ////                                                                TpedidoAnaliseEnderecoConfrontacao tpedidoAnaliseConfrontacao = new TpedidoAnaliseEnderecoConfrontacao
        ////                                                                {
        ////                                                                    Id = intNsu,
        ////                                                                    Id_pedido_analise_endereco = intNsuPai,
        ////                                                                    Pedido = i.Pedido,
        ////                                                                    Id_cliente = i.Id_cliente,
        ////                                                                    Tipo_endereco = Constantes.Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE,
        ////                                                                    Endereco_logradouro = i.Endereco_logradouro,
        ////                                                                    Endereco_bairro = i.Endereco_bairro,
        ////                                                                    Endereco_cidade = i.Endereco_cidade,
        ////                                                                    Endereco_cep = i.Endereco_cep,
        ////                                                                    Endereco_uf = i.Endereco_uf,
        ////                                                                    Endereco_numero = i.Endereco_numero,
        ////                                                                    Endereco_complemento = i.Endereco_complemento
        ////                                                                };

        ////                                                                dbGravacao.Add(tpedidoAnaliseConfrontacao);
        ////                                                                //await dbGravacao.SaveChangesAsync();
        ////                                                            }
        ////                                                        }
        ////                                                    }

        ////                                                }
        ////                                            }
        ////                                        }
        ////                                    }
        ////                                }
        ////                                if (lstErros.Count == 0)
        ////                                {
        ////                                    if (blnAnalisarEndereco)
        ////                                    {
        ////                                        //analise_endereco_tratar_status
        ////                                        //na comparação de pedidos arclube - 119664N / meu - 119663N, no meu foi salvo como 0
        ////                                        //sendo assim, não entrou nesse bloco
        ////                                        pedidonovoTrocaId.Analise_Endereco_Tratar_Status = 1;
        ////                                        dbGravacao.Update(pedidonovoTrocaId);
        ////                                        await dbGravacao.SaveChangesAsync();
        ////                                    }
        ////                                }
        ////                            }//fim infice pedido
        ////                        }//fim if produto=v
        ////                        if (lstErros.Count != 0)
        ////                        {
        ////                            break;
        ////                        }
        ////                    }//Fim item qu condordou mesmo sem presença de estoque
        ////                }

        ////                //Verifica se add transportadora
        ////                if (produtoValidado.ListaErros.Count == 0)
        ////                {
        ////                    if (!blnGravouTransportadora)
        ////                    {
        ////                        blnGravouTransportadora = true;

        ////                        if (pedido.EnderecoEntrega.OutroEndereco == true)
        ////                        {
        ////                            if (!string.IsNullOrEmpty(pedido.EnderecoEntrega.EndEtg_cep))
        ////                            {
        ////                                //aqui vai fazer a busca da transportadora pelo cep de endereço de entrega selecionado
        ////                                TtransportadoraCep transportadoraCep = await Util.Util.ObterTransportadoraPeloCep(
        ////                                    pedido.EnderecoEntrega.EndEtg_cep, dbGravacao);
        ////                                //os campos abaixo receberão os valores da busca
        ////                                if (!string.IsNullOrEmpty(transportadoraCep.Id.ToString()))
        ////                                {
        ////                                    pedidonovo.Transportadora_Id = transportadoraCep.Transportadora_id.ToString();
        ////                                    pedidonovo.Transportadora_Data = DateTime.Now;
        ////                                    pedidonovo.Transportadora_Usuario = usuario_atual;
        ////                                    pedidonovo.Transportadora_Selecao_Auto_Status =
        ////                                        Constantes.Constantes.TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_S;
        ////                                    pedidonovo.Transportadora_Selecao_Auto_Cep = pedido.EnderecoEntrega.EndEtg_cep;
        ////                                    pedidonovo.Transportadora_Selecao_Auto_Transportadora = transportadoraCep.Transportadora_id.ToString(); ;
        ////                                    pedidonovo.Transportadora_Selecao_Auto_Tipo_Endereco =
        ////                                        Constantes.Constantes.TRANSPORTADORA_SELECAO_AUTO_TIPO_ENDERECO_ENTREGA;
        ////                                    pedidonovo.Transportadora_Selecao_Auto_Data_Hora = DateTime.Now;
        ////                                }//arrumar ate aqui
        ////                            }
        ////                        }
        ////                        else
        ////                        {
        ////                            //aqui vai pelo cep do cliente mesmo, pois ele solicitou entragar no mesmo endereço do cadastro
        ////                            TtransportadoraCep transportadoraCep = await Util.Util.ObterTransportadoraPeloCep(
        ////                                    pedido.DadosCliente.Cep, dbGravacao);
        ////                            //os campos abaixo receberão os valores da busca
        ////                            if (!string.IsNullOrEmpty(transportadoraCep.Id.ToString()))
        ////                            {
        ////                                pedidonovo.Transportadora_Id = transportadoraCep.Transportadora_id.ToString();
        ////                                pedidonovo.Transportadora_Data = DateTime.Now;
        ////                                pedidonovo.Transportadora_Usuario = usuario_atual;
        ////                                pedidonovo.Transportadora_Selecao_Auto_Status =
        ////                                    Constantes.Constantes.TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_S;
        ////                                pedidonovo.Transportadora_Selecao_Auto_Cep = pedido.DadosCliente.Cep;
        ////                                pedidonovo.Transportadora_Selecao_Auto_Transportadora = transportadoraCep.Transportadora_id.ToString(); ;
        ////                                pedidonovo.Transportadora_Selecao_Auto_Tipo_Endereco =
        ////                                    Constantes.Constantes.TRANSPORTADORA_SELECAO_AUTO_TIPO_ENDERECO_CLIENTE;
        ////                                pedidonovo.Transportadora_Selecao_Auto_Data_Hora = DateTime.Now;
        ////                            }
        ////                        }
        ////                    }
        ////                }

        ////            }
        ////            #endregion
        ////            //vamos salvar os campos novos
        ////            pedidonovoTrocaId.Sistema_responsavel_cadastro = pedidonovo.Sistema_responsavel_cadastro;
        ////            pedidonovoTrocaId.Sistema_responsavel_atualizacao = pedidonovo.Sistema_responsavel_atualizacao;


        ////            //salvar Pedido na base
        ////            dbGravacao.Update(pedidonovoTrocaId);
        ////            retorno = pedidonovoTrocaId.Pedido;



        ////            //salvando todas alterações
        ////            await dbGravacao.SaveChangesAsync();

        //            return retorno;

        //        }

        public async Task<int> Fin_gera_nsu(string id_nsu, List<string> lstErros, ContextoBdGravacao dbgravacao)
        {
            int intRetorno = 0;
            int intRecordsAffected = 0;
            int intQtdeTentativas, intNsuUltimo, intNsuNovo;
            bool blnSucesso = true;
            int nsu = 0;



            //conta a qtde de id
            var qtdeIdFin = from c in dbgravacao.TfinControles
                            where c.Id == id_nsu
                            select c.Id;


            if (qtdeIdFin != null)
            {
                intRetorno = await qtdeIdFin.CountAsync();
            }

            //não está cadastrado, então cadastra agora 
            if (intRetorno == 0)
            {
                //criamos um novo para salvar
                TfinControle tfinControle = new TfinControle();

                tfinControle.Id = id_nsu;
                tfinControle.Nsu = 0;
                tfinControle.Dt_hr_ult_atualizacao = DateTime.Now;

                dbgravacao.Add(tfinControle);

            }

            //laço de tentativas para gerar o nsu(devido a acesso concorrente)


            //obtém o último nsu usado
            var tfincontroleEditando = await (from c in dbgravacao.TfinControles
                                              where c.Id == id_nsu
                                              select c).FirstOrDefaultAsync();


            if (tfincontroleEditando == null)
            {
                lstErros.Add("Falha ao localizar o registro para geração de NSU (" + id_nsu + ")!");
                return nsu;
            }


            tfincontroleEditando.Id = id_nsu;
            tfincontroleEditando.Nsu++;
            tfincontroleEditando.Dt_hr_ult_atualizacao = DateTime.Now;
            //tenta atualizar o banco de dados
            dbgravacao.Update(tfincontroleEditando);

            await dbgravacao.SaveChangesAsync();

            return tfincontroleEditando.Nsu;
        }

        //precisa de orçamentista com endereço, endereco do cliente, 
        public bool CompararEnderecoParceiro(string end_logradouro_1, int end_numero_1, int end_cep_1,
            string end_logradouro_2, int end_numero_2, int end_cep_2)
        {
            bool retorno = false;

            const string PREFIXOS = "|R|RUA|AV|AVEN|AVENIDA|TV|TRAV|TRAVESSA|AL|ALAM|ALAMEDA|PC|PRACA|PQ|PARQUE|EST|ESTR|ESTRADA|CJ|CONJ|CONJUNTO|";
            string[] v1, v2;
            string s, s1, s2;
            bool blnFlag, blnNumeroIgual;
            string[] v_end_numero_1, v_end_numero_2;
            int n_end_numero_1, n_end_numero_2;


            string pedido_end_logradouro_1 = Util.Util.RemoverAcentos(end_logradouro_1);
            int pedido_end_numero_1 = int.Parse(end_numero_1.ToString().Replace("-", ""));
            int pedido_end_cep_1 = int.Parse(end_cep_1.ToString().Replace("-", ""));

            string orcamentista_end_logradouro_2 = Util.Util.RemoverAcentos(end_logradouro_2);
            int orcamentista_end_numero_2 = int.Parse(end_numero_2.ToString().Replace("-", ""));
            int orcamentista_end_cep_2 = int.Parse(end_cep_2.ToString().Replace("-", ""));

            if (pedido_end_cep_1 != orcamentista_end_cep_2)
                return retorno;

            blnNumeroIgual = false;

            if (pedido_end_numero_1 == orcamentista_end_numero_2)
                blnNumeroIgual = true;

            if (!blnNumeroIgual)
            {
                v_end_numero_1 = pedido_end_numero_1.ToString().Split("/");
                n_end_numero_1 = 0;

                foreach (var v in v_end_numero_1)
                {
                    if (!string.IsNullOrEmpty(v))
                        n_end_numero_1++;
                }

                v_end_numero_2 = orcamentista_end_numero_2.ToString().Split("/");
                n_end_numero_2 = 0;

                foreach (var v in v_end_numero_2)
                {
                    if (!string.IsNullOrEmpty(v))
                        n_end_numero_2++;
                }

                if (n_end_numero_1 == 1 && n_end_numero_2 == 1)
                {
                    if (pedido_end_numero_1 != orcamentista_end_numero_2)
                        return retorno;
                }
                else
                {
                    foreach (var vend1 in v_end_numero_1)
                    {
                        if (!string.IsNullOrEmpty(vend1))
                        {
                            foreach (var vend2 in v_end_numero_2)
                            {
                                if (!string.IsNullOrEmpty(vend2))
                                {
                                    if (vend1.Trim() == vend2.Trim())
                                    {
                                        blnNumeroIgual = true;
                                        break;
                                    }
                                }
                            }
                            if (blnNumeroIgual)
                                break;
                        }
                    }
                }
            }

            if (!blnNumeroIgual)
                return retorno;

            pedido_end_logradouro_1 = Regex.Replace(pedido_end_logradouro_1, "[^0-9a-zA-Z]+", "");
            orcamentista_end_logradouro_2 = Regex.Replace(orcamentista_end_logradouro_2, "[^0-9a-zA-Z]+", "");

            v1 = pedido_end_logradouro_1.Split(" ");
            v2 = orcamentista_end_logradouro_2.Split(" ");

            s1 = "";

            foreach (var vend1 in v1)
            {
                blnFlag = false;

                s = vend1.Trim();

                if (!string.IsNullOrEmpty(s))
                {
                    if (string.IsNullOrEmpty(s1))
                    {
                        if (PREFIXOS.IndexOf("|" + s + "|") != -1)
                            blnFlag = true;
                    }
                    else
                        blnFlag = false;

                    if (blnFlag)
                    {
                        if (!string.IsNullOrEmpty(s1))
                            s1 += " ";

                        s1 += " ";
                    }

                }
            }

            s2 = "";

            foreach (var vend2 in v2)
            {
                blnFlag = false;

                s = vend2.Trim();

                if (!string.IsNullOrEmpty(s))
                {
                    if (string.IsNullOrEmpty(s2))
                    {
                        if (PREFIXOS.IndexOf("|" + s + "|") != -1)
                            blnFlag = true;
                    }
                    else
                        blnFlag = false;

                    if (blnFlag)
                    {
                        if (!string.IsNullOrEmpty(s2))
                            s2 += " ";

                        s2 += " ";
                    }

                }
            }

            if (s1 != s2)
                return retorno;

            return retorno = true;

        }

        public async Task<decimal> CalculaTotalRALiquidoBD(string id_pedido, ContextoBdGravacao dbGravacao, List<string> lstErros)
        {
            float percentual_desagio_RA_liquido = 0;
            decimal vl_total = 0;
            decimal vl_total_RA_liquido = 0;

            id_pedido = id_pedido.Trim();
            id_pedido = Normaliza_num_pedido(id_pedido);

            //busca pedido
            Tpedido tpedido = await (from c in dbGravacao.Tpedidos
                                     where c.Pedido == id_pedido
                                     select c).FirstOrDefaultAsync();

            if (tpedido == null)
            {
                lstErros.Add("Pedido-base " + id_pedido + " não foi encontrado.");
            }

            percentual_desagio_RA_liquido = tpedido.Perc_Desagio_RA_Liquida;

            //obtém os valores totais de nf, ra e venda
            var vlTotalTask = from c in dbGravacao.TpedidoItems.Include(x => x.Tpedido)
                              where c.Tpedido.St_Entrega != Constantes.Constantes.ST_ENTREGA_CANCELADO &&
                                    c.Tpedido.Pedido.Contains(id_pedido)
                              select new
                              {
                                  vlTotalRA = c.Qtde * (c.Preco_Lista - c.Preco_Venda)
                              };
            if (vlTotalTask != null)
                vl_total = (decimal)vlTotalTask.Sum(x => x.vlTotalRA);

            //afazer = vl_total é a soma de preco_lista
            vl_total_RA_liquido = (vl_total - ((decimal)percentual_desagio_RA_liquido / 100) * vl_total);

            return vl_total_RA_liquido;
        }

        public string Normaliza_num_pedido(string id_pedido)
        {
            string s_num = "";
            string s_ano = "";
            string s_filhote = "";
            string c = "";

            int letra_numerica;

            string retorno = "";

            if (string.IsNullOrEmpty(id_pedido))
                return retorno;

            for (int i = 0; i <= id_pedido.Length; i++)
            {
                if (int.TryParse(id_pedido.Substring(i, 1), out letra_numerica))
                {
                    s_num += letra_numerica;
                }
                else
                {
                    break;
                }
            }

            if (string.IsNullOrEmpty(s_num))
            {
                return retorno;
            }

            letra_numerica = 0;
            int eNumero;
            for (int i = 0; i < id_pedido.Length; i++)
            {
                c = id_pedido.Substring(i, 1);

                if (!int.TryParse(c, out eNumero))
                {
                    if (string.IsNullOrEmpty(s_ano))
                        s_ano = c;
                    else if (string.IsNullOrEmpty(s_filhote))
                        s_filhote = c;
                }
            }

            if (string.IsNullOrEmpty(s_ano))
                return retorno;

            s_num = Util.Util.Normaliza_Codigo(s_num, Constantes.Constantes.TAM_MIN_NUM_PEDIDO);

            retorno = s_num + s_ano;

            if (!string.IsNullOrEmpty(s_filhote))
                retorno += Constantes.Constantes.COD_SEPARADOR_FILHOTE + s_filhote;

            return retorno;
        }

        public string Gera_letra_pedido_filhote(int indice_pedido)
        {
            string s_letra = "";
            string gera_letra_pedido_filhote = "";
            if (indice_pedido <= 0)
            {
                return "";

            }

            char letra = 'A';

            s_letra = (((int)letra - 1) + indice_pedido).ToString();

            gera_letra_pedido_filhote = s_letra;

            return gera_letra_pedido_filhote;
        }

        public async Task<TpedidoItem> MontarTpedidoItemParaCadastrar(cl_ITEM_PEDIDO_NOVO v_item)
        {
            TpedidoItem tpedidoItem = new TpedidoItem();

            tpedidoItem.Pedido = v_item.Pedido;
            tpedidoItem.Fabricante = v_item.Fabricante;
            tpedidoItem.Produto = v_item.produto;
            tpedidoItem.Qtde = v_item.Qtde;
            tpedidoItem.Desc_Dado = v_item.Desc_Dado;
            tpedidoItem.Preco_Venda = v_item.Preco_Venda;
            tpedidoItem.Preco_NF = v_item.Preco_NF;
            tpedidoItem.Preco_Fabricante = v_item.Preco_fabricante;
            tpedidoItem.Vl_Custo2 = v_item.Vl_custo2;
            tpedidoItem.Preco_Lista = v_item.Preco_lista;
            tpedidoItem.Margem = v_item.Margem;
            tpedidoItem.Desc_Max = v_item.Desc_max;
            tpedidoItem.Comissao = v_item.Comissao;
            tpedidoItem.Descricao = v_item.Descricao;
            tpedidoItem.Descricao_Html = v_item.Descricao_html;
            tpedidoItem.Ean = v_item.Ean;
            tpedidoItem.Grupo = v_item.Grupo;
            tpedidoItem.Peso = v_item.Peso;
            tpedidoItem.Qtde_Volumes = v_item.Qtde_volumes;
            tpedidoItem.Abaixo_Min_Status = v_item.Abaixo_min_status;
            tpedidoItem.Abaixo_Min_Autorizacao = v_item.abaixo_min_autorizacao;
            tpedidoItem.Abaixo_Min_Autorizador = v_item.Abaixo_min_autorizador;
            tpedidoItem.Abaixo_Min_Superv_Autorizador = v_item.Abaixo_min_superv_autorizador;
            tpedidoItem.Sequencia = v_item.Sequencia;
            tpedidoItem.Markup_Fabricante = v_item.Markup_fabricante;
            tpedidoItem.CustoFinancFornecCoeficiente = v_item.custoFinancFornecCoeficiente;
            tpedidoItem.CustoFinancFornecPrecoListaBase = v_item.CustoFinancFornecPrecoListaBase;
            tpedidoItem.Cubagem = v_item.cubagem;
            tpedidoItem.Ncm = v_item.Ncm;
            tpedidoItem.Cst = v_item.Cst;
            tpedidoItem.Descontinuado = v_item.Descontinuado;


            return await Task.FromResult(tpedidoItem);
        }

        //Estamos gerando o id_estoque, id_estoque_movimento, gravando e gravando o Log
        public async Task<bool> EstoqueProdutoSaidaV2(string id_usuario, string id_pedido, short id_nfe_emitente,
            string id_fabricante, string id_produto, int qtde_a_sair, int qtde_autorizada_sem_presenca,
            short[] qtde_estoque_aux, List<string> lstErros, ContextoBdGravacao contexto)
        {
            //essas variveis tem que retornar
            int qtde_disponivel = 0;
            //qtde_estoque_vendido = 0;
            //qtde_estoque_sem_presenca = 0;

            if (qtde_a_sair <= 0 || string.IsNullOrEmpty(id_produto) || string.IsNullOrEmpty(id_pedido))
            {
                return true;
            }

            //afazer: verificar se é por causa do db abaixo, testar passando o contexto de gravação

            var lotesTask = await (from c in contexto.TestoqueItems.Include(x => x.Testoque)
                                   where c.Testoque.Id_nfe_emitente == id_nfe_emitente &&
                                         c.Fabricante == id_fabricante &&
                                         c.Produto == id_produto &&
                                         (c.Qtde - c.Qtde_utilizada) > 0
                                   select new
                                   {
                                       id_estoque = c.Id_estoque,
                                       saldo = (c.Qtde - c.Qtde_utilizada)
                                   }).ToListAsync();

            //armazena as entradas no estoque candidatas à saída de produtos
            List<string> v_estoque = new List<string>();

            foreach (var lote in lotesTask)
            {
                v_estoque.Add(lote.id_estoque);
                qtde_disponivel += (int)lote.saldo;
            }

            //NÃO HÁ PRODUTOS SUFICIENTES NO ESTOQUE!!
            if ((qtde_a_sair - qtde_autorizada_sem_presenca) > qtde_disponivel)
            {
                lstErros.Add("Produto " + id_produto + " do fabricante " + id_fabricante + ": faltam " +
                    ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_disponivel) + " unidades no estoque (" +
                    Util.Util.ObterApelidoEmpresaNfeEmitentes(id_nfe_emitente, contextoProvider.GetContextoLeitura()) +
                    ") para poder atender ao pedido.");
                return false;
            }

            bool retorno = true;
            //realiza a saída do estoque!!
            int qtde_movimentada = 0;
            int qtde_movto = 0;
            int qtde_aux = 0;
            int qtde_utilizada_aux = 0;

            foreach (var v in v_estoque)
            {
                if (!string.IsNullOrEmpty(v))
                {
                    //a quantidade necessária já foi retirada do estoque!!
                    if (qtde_movimentada >= qtde_a_sair)
                    {
                        break;
                    }

                    TestoqueItem testoqueItem = await (from c in contexto.TestoqueItems
                                                       where c.Id_estoque == v &&
                                                             c.Fabricante == id_fabricante &&
                                                             c.Produto == id_produto
                                                       select c).FirstOrDefaultAsync();

                    qtde_aux = (short)testoqueItem.Qtde;
                    qtde_utilizada_aux = (short)testoqueItem.Qtde_utilizada;
                    qtde_estoque_aux[0] = (short)qtde_aux;

                    if ((qtde_a_sair - qtde_movimentada) > (qtde_aux - qtde_utilizada_aux))
                    {
                        //quantidade de produtos deste item de estoque é insuficiente p/ atender o pedido
                        qtde_movto = qtde_aux - qtde_utilizada_aux;
                    }
                    else
                    {
                        //quantidade de produtos deste item sozinho é suficiente p / atender o pedido
                        qtde_movto = qtde_a_sair - qtde_movimentada;
                        qtde_estoque_aux[0] = (short)qtde_movto;
                    }

                    testoqueItem.Qtde_utilizada = (short?)(qtde_utilizada_aux + qtde_movto);
                    testoqueItem.Data_ult_movimento = DateTime.Now.Date;


                    contexto.Update(testoqueItem);
                    await contexto.SaveChangesAsync();

                    //contabiliza quantidade movimentada
                    qtde_movimentada = qtde_movimentada + qtde_movto;

                    //registra o movimento de saída no estoque
                    string id_estoqueNovo = await GeraIdEstoque(contexto, lstErros);

                    if (string.IsNullOrEmpty(id_estoqueNovo))
                    {
                        lstErros.Add("Falha ao tentar gerar um número identificador para o registro de movimento no estoque. " + lstErros.Last() + "");
                        return retorno = false;
                    }

                    TestoqueMovimento testoqueMovimento = new TestoqueMovimento();

                    testoqueMovimento.Id_Movimento = id_estoqueNovo;
                    testoqueMovimento.Data = DateTime.Now.Date;
                    testoqueMovimento.Hora = DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                        DateTime.Now.Month.ToString().PadLeft(2, '0') +
                        DateTime.Now.Minute.ToString().PadLeft(2, '0');
                    testoqueMovimento.Usuario = id_usuario;
                    testoqueMovimento.Id_Estoque = v;
                    testoqueMovimento.Fabricante = id_fabricante;
                    testoqueMovimento.Produto = id_produto;
                    testoqueMovimento.Qtde = (short)qtde_movto;
                    testoqueMovimento.Operacao = Constantes.Constantes.OP_ESTOQUE_VENDA;
                    testoqueMovimento.Estoque = Constantes.Constantes.ID_ESTOQUE_VENDIDO;
                    testoqueMovimento.Pedido = id_pedido;
                    testoqueMovimento.Kit = 0;

                    contexto.Add(testoqueMovimento);
                    await contexto.SaveChangesAsync();

                    //t_estoque: atualiza data do último movimento
                    Testoque testoque = await (from c in contexto.Testoques
                                               where c.Id_estoque == v
                                               select c).FirstOrDefaultAsync();

                    testoque.Data_ult_movimento = DateTime.Now.Date;

                    contexto.Update(testoque);
                    await contexto.SaveChangesAsync();

                    //já conseguiu alocar tudo
                    if (qtde_movimentada >= qtde_a_sair)
                    {
                        retorno = true;
                    }
                }
            }

            //não conseguiu movimentar a quantidade suficiente
            if (qtde_movimentada < (qtde_a_sair - qtde_autorizada_sem_presenca))
            {
                lstErros.Add("Produto " + id_produto + " do fabricante " + id_fabricante + ": faltam " +
                    ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_movimentada) +
                    " unidades no estoque para poder atender ao pedido.");
                return retorno = false;
            }

            //Estamos gerando o id_estoque_movimento
            //registra a venda sem presença no estoque
            if (qtde_movimentada < qtde_a_sair)
            {
                //REGISTRA O MOVIMENTO DE SAÍDA NO ESTOQUE
                var id_estoque_movto = await GeraIdEstoqueMovto(lstErros, contexto);

                string id_movto = id_estoque_movto;

                if (string.IsNullOrEmpty(id_movto))
                {
                    lstErros.Add("Falha ao tentar gerar um número identificador para o registro de movimento no estoque.");
                    return retorno = false;
                }

                qtde_estoque_aux[1] = (short)(qtde_a_sair - qtde_movimentada);
                //qtde_estoque_sem_presenca = qtde_a_sair - qtde_movimentada;


                TestoqueMovimento testoqueMovimento = new TestoqueMovimento();
                testoqueMovimento.Id_Movimento = id_movto;
                testoqueMovimento.Data = DateTime.Now.Date;
                testoqueMovimento.Hora = DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                    DateTime.Now.Month.ToString().PadLeft(2, '0') +
                    DateTime.Now.Minute.ToString().PadLeft(2, '0');
                testoqueMovimento.Usuario = id_usuario;
                testoqueMovimento.Id_Estoque = "";// está sem presença no estoque
                testoqueMovimento.Fabricante = id_fabricante;
                testoqueMovimento.Produto = id_produto;
                testoqueMovimento.Qtde = qtde_estoque_aux[1];
                testoqueMovimento.Operacao = Constantes.Constantes.OP_ESTOQUE_VENDA;
                testoqueMovimento.Estoque = Constantes.Constantes.ID_ESTOQUE_SEM_PRESENCA;
                testoqueMovimento.Pedido = id_pedido;
                testoqueMovimento.Kit = 0;

                contexto.Add(testoqueMovimento);
                await contexto.SaveChangesAsync();
            }

            qtde_estoque_aux[0] = (short)qtde_movimentada;
            //short qtde_estoque_vendido = (short)qtde_movimentada;


            //log de movimentação do estoque
            if (!await Util.Util.Grava_log_estoque_v2(id_usuario, id_nfe_emitente, id_fabricante, id_produto,
                (short)qtde_a_sair, qtde_estoque_aux[0], Constantes.Constantes.OP_ESTOQUE_LOG_VENDA,
                Constantes.Constantes.ID_ESTOQUE_VENDA, Constantes.Constantes.ID_ESTOQUE_VENDIDO,
                "", "", "", id_pedido, "", "", "", contexto))
            {
                lstErros.Add("FALHA AO GRAVAR O LOG DA MOVIMENTAÇÃO NO ESTOQUE");
                return retorno = false;
            }
            if (qtde_estoque_aux[1] > 0)
            {
                if (!await Util.Util.Grava_log_estoque_v2(id_usuario, id_nfe_emitente, id_fabricante, id_produto,
                qtde_estoque_aux[1], qtde_estoque_aux[1],
                Constantes.Constantes.OP_ESTOQUE_LOG_VENDA_SEM_PRESENCA, "",
                Constantes.Constantes.ID_ESTOQUE_SEM_PRESENCA, "", "", "", id_pedido, "", "", "", contexto))
                {
                    lstErros.Add("FALHA AO GRAVAR O LOG DA MOVIMENTAÇÃO NO ESTOQUE");
                    return retorno = false;
                }
            }
            return retorno;
        }


        public async Task<string> GeraIdEstoqueMovto(List<string> lstErros, ContextoBdGravacao contexto)
        {
            string retorno = "";
            retorno = await Util.Util.GerarNsu(contexto, Constantes.Constantes.NSU_ID_ESTOQUE_MOVTO);

            return retorno;
        }

        public async Task<string> GeraIdEstoque(ContextoBdGravacao contexto, List<string> lstErros)
        {
            string retorno = "";


            retorno = await Util.Util.GerarNsu(contexto, Constantes.Constantes.NSU_ID_ESTOQUE_MOVTO);

            return retorno;
        }

        public async Task<IEnumerable<IndicadorDto>> BuscarOrcamentistaEIndicadorListaCompleta(string usuarioSistema,
           string lstOperacoesPermitidas, string loja)
        {

            //vamos trazer a lista de indicadores de um DTO com tudo que precisaremos na tela
            List<TorcamentistaEindicador> lst = (await Util.Util.BuscarOrcamentistaEIndicadorListaCompleta(
                contextoProvider, usuarioSistema, lstOperacoesPermitidas, loja)).ToList();

            List<IndicadorDto> lstIndicadorDto = new List<IndicadorDto>();

            foreach (var i in lst)
            {
                lstIndicadorDto.Add(new IndicadorDto
                {
                    Apelido = i.Apelido,
                    RazaoSocial = i.Razao_Social_Nome,
                    PermiteRA = i.Permite_RA_Status
                });
            }

            return lstIndicadorDto;
        }

        public async Task<string> GerarNumeroPedido(List<string> lstErros, ContextoBdGravacao contextoBdGravacao)
        {
            string numPedido = "";
            string s_num = "";
            string s_letra_ano = "";
            int n_descarte = 0;
            string s_descarte = "";
            //passar o db ContextoBdGravacao
            var dbgravacao = contextoBdGravacao;

            s_num = await Util.Util.GerarNsu(contextoBdGravacao, Constantes.Constantes.NSU_PEDIDO);

            if (!string.IsNullOrEmpty(s_num))
            {
                n_descarte = s_num.Length - Constantes.Constantes.TAM_MIN_NUM_PEDIDO;
                s_descarte = s_num.Substring(0, n_descarte);
                string teste = new String('0', n_descarte);

                if (s_descarte != teste)
                    return numPedido;

                s_num = s_num.Substring(s_num.Length - n_descarte);

                //obtém a letra para o sufixo do pedido de acordo c / o ano da geração do 
                //nsu(importante: fazer a leitura somente após gerar o nsu, pois a letra pode ter 
                //sido alterada devido à mudança de ano!!)
                var ret = from c in dbgravacao.Tcontroles
                          where c.Id_Nsu == Constantes.Constantes.NSU_PEDIDO
                          select c;

                var controle = await ret.FirstOrDefaultAsync();

                if (controle == null)
                    lstErros.Add("Não existe registro na tabela de controle com o id = '" +
                        Constantes.Constantes.NSU_PEDIDO_TEMPORARIO);

                s_letra_ano = controle.Ano_Letra_Seq;

                numPedido = s_num + s_letra_ano;

            }

            return numPedido;
        }

        public async Task<string> GerarNumeroPedidoTemporario(List<string> lstErros, ContextoBdGravacao contextoBdGravacao)
        {
            string numPedido = "";
            string s_num = "";
            string s_letra_ano = "";
            int n_descarte = 0;
            string s_descarte = "";
            //passar o db ContextoBdGravacao
            var dbgravacao = contextoBdGravacao;

            s_num = await Util.Util.GerarNsu(contextoBdGravacao, Constantes.Constantes.NSU_PEDIDO_TEMPORARIO);

            if (!string.IsNullOrEmpty(s_num))
            {
                n_descarte = s_num.Length - Constantes.Constantes.TAM_MIN_NUM_PEDIDO;
                s_descarte = s_num.Substring(0, n_descarte);
                string teste = new String('0', n_descarte);
                if (s_descarte != teste)
                    return numPedido;

                s_num = s_num.Substring(s_num.Length - n_descarte);

                //obtém a letra para o sufixo do pedido de acordo c / o ano da geração do 
                //nsu(importante: fazer a leitura somente após gerar o nsu, pois a letra pode ter 
                //sido alterada devido à mudança de ano!!)
                var ret = from c in dbgravacao.Tcontroles
                          where c.Id_Nsu == Constantes.Constantes.NSU_PEDIDO_TEMPORARIO
                          select c;

                var controle = await ret.FirstOrDefaultAsync();

                if (controle == null)
                    lstErros.Add("Não existe registro na tabela de controle com o id = '" +
                        Constantes.Constantes.NSU_PEDIDO_TEMPORARIO);

                s_letra_ano = controle.Ano_Letra_Seq;

                numPedido = "T" + s_num + s_letra_ano;

            }

            return numPedido;
        }

        public async Task<string> ObtemValorLimiteAprovacaoAutomaticaAnaliseDeCredito()
        {
            var db = contextoProvider.GetContextoLeitura();

            string nsu = await (from c in db.Tcontroles
                                where c.Id_Nsu == Constantes.Constantes.ID_PARAM_CAD_VL_APROV_AUTO_ANALISE_CREDITO
                                select c.Nsu).FirstOrDefaultAsync();



            return nsu;
        }

        private async Task<IEnumerable<TpedidoItem>> VerificarDescontoArredondado(string loja, PedidoDto pedido, List<string> lstErros,
            string c_custoFinancFornecTipoParcelamento, short c_custoFinancFornecQtdeParcelas,
            float percDescComissaoUtilizar, List<string> vdesconto)
        {
            var db = contextoProvider.GetContextoLeitura();

            float coeficiente = 0;
            float? desc_dado_arredondado = 0;
            List<TpedidoItem> lstPedidoItem = new List<TpedidoItem>();

            //aqui estão verificando o v_item e não pedido
            //vamos vericar cada produto da lista
            foreach (var produto in pedido.ListaProdutos)
            {
                var produtoLojaTask = (from c in db.TprodutoLojas.Include(x => x.Tproduto).Include(x => x.Tfabricante)
                                       where c.Tproduto.Fabricante == produto.Fabricante &&
                                             c.Tproduto.Produto == produto.NumProduto &&
                                             c.Loja == loja
                                       select c).FirstOrDefaultAsync();

                if (produtoLojaTask == null)
                    lstErros.Add("Produto " + produto.NumProduto + " do fabricante " + produto.Fabricante + "NÃO está " +
                        "cadastrado para a loja " + loja);
                else
                {
                    TprodutoLoja produtoLoja = await produtoLojaTask;
                    //vamos atribuir para um TpedidoItem que será uma lista de produtos selecionados
                    TpedidoItem tpedidoItem = new TpedidoItem();
                    {
                        tpedidoItem.Preco_Venda = (decimal)produto.VlUnitario;
                        tpedidoItem.Produto = produtoLoja.Tproduto.Produto;
                        tpedidoItem.Fabricante = produtoLoja.Tproduto.Fabricante;
                        tpedidoItem.Preco_Lista = (decimal)produtoLoja.Preco_Lista;
                        tpedidoItem.Margem = produtoLoja.Margem;
                        tpedidoItem.Desc_Max = produtoLoja.Desc_Max;
                        tpedidoItem.Comissao = produtoLoja.Comissao;
                        tpedidoItem.Preco_Fabricante = produtoLoja.Tproduto.Preco_Fabricante;
                        tpedidoItem.Vl_Custo2 = produtoLoja.Tproduto.Vl_Custo2;
                        tpedidoItem.Descricao = produtoLoja.Tproduto.Descricao;
                        tpedidoItem.Descricao_Html = produtoLoja.Tproduto.Descricao_Html;
                        tpedidoItem.Ean = produtoLoja.Tproduto.Ean;
                        tpedidoItem.Grupo = produtoLoja.Tproduto.Grupo;
                        tpedidoItem.Peso = produtoLoja.Tproduto.Peso;
                        tpedidoItem.Qtde_Volumes = produtoLoja.Tproduto.Qtde_Volumes;
                        tpedidoItem.Markup_Fabricante = produtoLoja.Tfabricante.Markup;
                        tpedidoItem.Cubagem = produtoLoja.Tproduto.Cubagem;
                        tpedidoItem.Ncm = produtoLoja.Tproduto.Ncm;
                        tpedidoItem.Cst = produtoLoja.Tproduto.Cst;
                        tpedidoItem.Descontinuado = produtoLoja.Tproduto.Descontinuado;
                        tpedidoItem.CustoFinancFornecPrecoListaBase = (decimal)produtoLoja.Preco_Lista;
                        tpedidoItem.Tproduto = produtoLoja.Tproduto;


                        //Tproduto para ser verificado depois dessa rotina

                        if (c_custoFinancFornecTipoParcelamento ==
                            Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
                            coeficiente = 1;
                        else
                        {
                            var coeficienteTask = (from c in db.TpercentualCustoFinanceiroFornecedors
                                                   where c.Fabricante == produto.Fabricante &&
                                                         c.Tipo_Parcelamento == c_custoFinancFornecTipoParcelamento &&
                                                         c.Qtde_Parcelas == c_custoFinancFornecQtdeParcelas
                                                   select c).FirstOrDefaultAsync();
                            if (await coeficienteTask == null)
                                lstErros.Add("Opção de parcelamento não disponível para fornecedor " + produto.Fabricante +
                                    ": " + DecodificaCustoFinanFornecQtdeParcelas(c_custoFinancFornecTipoParcelamento,
                                    c_custoFinancFornecQtdeParcelas) + " parcela(s)");
                            else
                            {
                                coeficiente = (await coeficienteTask).Coeficiente;
                                //voltamos a atribuir ao tpedidoItem
                                tpedidoItem.Preco_Lista = Math.Round((decimal)coeficiente * tpedidoItem.Preco_Lista, 2);
                            }
                        }

                        tpedidoItem.CustoFinancFornecCoeficiente = coeficiente;

                        if (tpedidoItem.Preco_Lista == 0)
                        {
                            tpedidoItem.Desc_Dado = 0;
                            desc_dado_arredondado = 0;
                        }
                        else
                        {
                            tpedidoItem.Desc_Dado = (float)(100 *
                                (tpedidoItem.Preco_Lista - tpedidoItem.Preco_Venda) / tpedidoItem.Preco_Lista);
                            desc_dado_arredondado = tpedidoItem.Desc_Dado;
                        }

                        if (desc_dado_arredondado > percDescComissaoUtilizar)
                        {
                            var tDescontoTask = from c in db.Tdescontos
                                                where c.Usado_status == 0 &&
                                                      c.Id_cliente == pedido.DadosCliente.Id &&
                                                      c.Fabricante == tpedidoItem.Fabricante &&
                                                      c.Produto == tpedidoItem.Produto &&
                                                      c.Loja == loja &&
                                                      c.Data >= DateTime.Now.AddMinutes(-30)
                                                orderby c.Data descending
                                                select c;

                            Tdesconto tdesconto = await tDescontoTask.FirstOrDefaultAsync();

                            if (tdesconto == null)
                            {
                                lstErros.Add("Produto " + produto.NumProduto + " do fabricante " + produto.Fabricante +
                                    ": desconto de " + tpedidoItem.Desc_Dado + "% excede o máximo permitido.");
                            }
                            else
                            {
                                tdesconto = await tDescontoTask.FirstOrDefaultAsync();
                                if ((decimal)tpedidoItem.Desc_Dado >= tdesconto.Desc_max)
                                    lstErros.Add("Produto " + produto.NumProduto + " do fabricante " + produto.Fabricante +
                                        ": desconto de " + tpedidoItem.Desc_Dado + " % excede o máximo autorizado.");
                                else
                                {
                                    tpedidoItem.Abaixo_Min_Status = 1;
                                    tpedidoItem.Abaixo_Min_Autorizacao = tdesconto.Id;
                                    tpedidoItem.Abaixo_Min_Autorizador = tdesconto.Autorizador;
                                    tpedidoItem.Abaixo_Min_Superv_Autorizador = tdesconto.Supervisor_autorizador;

                                    //essa variavel aparentemente apenas sinaliza 
                                    //se existe uma senha de autorização para desconto superior
                                    if (vdesconto.Count > 0)
                                    {
                                        vdesconto.Add(tdesconto.Id);
                                    }
                                }
                            }
                        }
                    }

                    lstPedidoItem.Add(tpedidoItem);

                }
            }
            return lstPedidoItem;
        }


        private float VerificarPagtoPreferencial(Tparametro tParametro, PedidoDto pedido,
            float percDescComissaoUtilizar, PercentualMaxDescEComissao percentualMax, decimal vl_total)
        {
            List<string> lstOpcoesPagtoPrefericiais = new List<string>();
            if (!string.IsNullOrEmpty(tParametro.Id))
            {
                //a verificação é feita na linha 380 ate 388
                lstOpcoesPagtoPrefericiais = tParametro.Campo_texto.Split(',').ToList();
            }

            string s_pg = "";
            decimal? vlNivel1 = 0;
            decimal? vlNivel2 = 0;

            //identifica e verifica se é pagto preferencial e calcula  637 ate 712
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA)
                s_pg = pedido.FormaPagtoCriacao.Op_av_forma_pagto;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                s_pg = pedido.FormaPagtoCriacao.Op_pu_forma_pagto;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                s_pg = Constantes.Constantes.ID_FORMA_PAGTO_CARTAO;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                s_pg = Constantes.Constantes.ID_FORMA_PAGTO_CARTAO_MAQUINETA;
            if (!string.IsNullOrEmpty(s_pg))
            {
                if (lstOpcoesPagtoPrefericiais.Count > 0)
                {
                    foreach (var op in lstOpcoesPagtoPrefericiais)
                    {
                        if (s_pg == op)
                        {
                            if (pedido.DadosCliente.Tipo == Constantes.Constantes.ID_PJ)
                                percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                            else
                                percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;
                        }
                    }
                }
            }

            bool pgtoPreferencial = false;
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                s_pg = pedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto;

                if (!string.IsNullOrEmpty(s_pg))
                {
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
                    {
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
                    }
                }
                //verificamos a entrada
                if (pgtoPreferencial)
                    vlNivel2 = pedido.FormaPagtoCriacao.C_pce_entrada_valor;
                else
                    vlNivel1 = pedido.FormaPagtoCriacao.C_pce_entrada_valor;

                //Identifica e contabiliza o valor das parcelas
                pgtoPreferencial = false;
                s_pg = pedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto;
                if (!string.IsNullOrEmpty(s_pg))
                {
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
                    {
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
                    }
                }

                if (pgtoPreferencial)
                    vlNivel2 = vlNivel2 +
                        (pedido.FormaPagtoCriacao.C_pce_prestacao_qtde * pedido.FormaPagtoCriacao.C_pce_prestacao_valor);
                else
                    vlNivel1 = vlNivel1 +
                        (pedido.FormaPagtoCriacao.C_pce_prestacao_qtde * pedido.FormaPagtoCriacao.C_pce_prestacao_valor);

                if (vlNivel2 > (vl_total / 2))
                {
                    if (pedido.DadosCliente.Tipo == Constantes.Constantes.ID_PJ)
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                    else
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;
                }
            }
            if (pedido.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                s_pg = pedido.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto;

                if (!string.IsNullOrEmpty(s_pg))
                {
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
                    {
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
                    }
                }
                //verificamos a entrada
                if (pgtoPreferencial)
                    vlNivel2 = pedido.FormaPagtoCriacao.C_pse_prim_prest_valor;
                else
                    vlNivel1 = pedido.FormaPagtoCriacao.C_pse_prim_prest_valor;

                //Identifica e contabiliza o valor das parcelas
                pgtoPreferencial = false;
                s_pg = pedido.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto;
                if (!string.IsNullOrEmpty(s_pg))
                {
                    if (lstOpcoesPagtoPrefericiais.Count > 0)
                    {
                        foreach (var op in lstOpcoesPagtoPrefericiais)
                        {
                            if (s_pg == op)
                                pgtoPreferencial = true;
                        }
                    }
                }

                if (pgtoPreferencial)
                    vlNivel2 = vlNivel2 +
                        (pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde * pedido.FormaPagtoCriacao.C_pse_demais_prest_valor);
                else
                    vlNivel1 = vlNivel1 +
                        (pedido.FormaPagtoCriacao.C_pse_demais_prest_qtde * pedido.FormaPagtoCriacao.C_pse_demais_prest_valor);

                if (vlNivel2 > (vl_total / 2))
                {
                    if (pedido.DadosCliente.Tipo == Constantes.Constantes.ID_PJ)
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDescPJ;
                    else
                        percDescComissaoUtilizar = percentualMax.PercMaxComissaoEDesc;
                }
            }
            return percDescComissaoUtilizar;
        }
        private bool ValidarFormaPagto(PedidoDto pedidoDto, List<string> lstErros)
        {
            bool retorno = false;

            decimal vlTotalFormaPagto = 0M;

            if (pedidoDto.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA)
            {
                if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.Op_av_forma_pagto))
                    lstErros.Add("Indique a forma de pagamento (à vista).");
                if (!CalculaItens(pedidoDto, out vlTotalFormaPagto))
                    lstErros.Add("Há divergência entre o valor total do pedido (" +
                        Constantes.Constantes.SIMBOLO_MONETARIO + " " +
                        pedidoDto.VlTotalDestePedido + ") e o valor total descrito através da forma de pagamento (" +
                        Constantes.Constantes.SIMBOLO_MONETARIO + " " +
                        Math.Abs((decimal)pedidoDto.VlTotalDestePedido - vlTotalFormaPagto) + ")!!");
            }
            else if (pedidoDto.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
            {
                if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.Op_pu_forma_pagto))
                    lstErros.Add("Indique a forma de pagamento da parcela única.");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pu_valor.ToString()))
                    lstErros.Add("Indique o valor da parcela única.");
                else if (pedidoDto.FormaPagtoCriacao.C_pu_valor <= 0)
                    lstErros.Add("Valor da parcela única é inválido.");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pu_vencto_apos.ToString()))
                    lstErros.Add("Indique o intervalo de vencimento da parcela única.");
                else if (pedidoDto.FormaPagtoCriacao.C_pu_vencto_apos <= 0)
                    lstErros.Add("Intervalo de vencimento da parcela única é inválido.");
            }
            else if (pedidoDto.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
            {
                if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pc_qtde.ToString()))
                    lstErros.Add("Indique a quantidade de parcelas (parcelado no cartão [internet]).");
                else if (pedidoDto.FormaPagtoCriacao.C_pc_qtde < 1)
                    lstErros.Add("Quantidade de parcelas inválida (parcelado no cartão [internet]).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pc_valor.ToString()))
                    lstErros.Add("Indique o valor da parcela (parcelado no cartão [internet]).");
                else if (pedidoDto.FormaPagtoCriacao.C_pc_valor <= 0)
                    lstErros.Add("Valor de parcela inválido (parcelado no cartão [internet]).");
            }
            else if (pedidoDto.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
            {
                if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pc_maquineta_qtde.ToString()))
                    lstErros.Add("Indique a quantidade de parcelas (parcelado no cartão [maquineta]).");
                else if (pedidoDto.FormaPagtoCriacao.C_pc_maquineta_qtde < 1)
                    lstErros.Add("Quantidade de parcelas inválida (parcelado no cartão [maquineta]).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pc_maquineta_valor.ToString()))
                    lstErros.Add("Indique o valor da parcela (parcelado no cartão [maquineta]).");
                else if (pedidoDto.FormaPagtoCriacao.C_pc_maquineta_valor <= 0)
                    lstErros.Add("Valor de parcela inválido (parcelado no cartão [maquineta]).");
            }
            else if (pedidoDto.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
            {
                if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.Op_pce_entrada_forma_pagto.ToString()))
                    lstErros.Add("Indique a forma de pagamento da entrada (parcelado com entrada).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pce_entrada_valor.ToString()))
                    lstErros.Add("Indique o valor da entrada (parcelado com entrada).");
                else if (pedidoDto.FormaPagtoCriacao.C_pce_entrada_valor <= 0)
                    lstErros.Add("Valor da entrada inválido (parcelado com entrada).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto))
                    lstErros.Add("Indique a forma de pagamento das prestações (parcelado com entrada).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pce_prestacao_qtde.ToString()))
                    lstErros.Add("Indique a quantidade de prestações (parcelado com entrada).");
                else if (pedidoDto.FormaPagtoCriacao.C_pce_prestacao_qtde <= 0)
                    lstErros.Add("Quantidade de prestações inválida (parcelado com entrada).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pce_prestacao_valor.ToString()))
                    lstErros.Add("Indique o valor da prestação (parcelado com entrada).");
                else if (pedidoDto.FormaPagtoCriacao.C_pce_prestacao_valor <= 0)
                    lstErros.Add("Valor de prestação inválido (parcelado com entrada).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pce_prestacao_periodo.ToString()))
                    lstErros.Add("Indique o intervalo de vencimento entre as parcelas (parcelado com entrada).");
                else if (pedidoDto.FormaPagtoCriacao.C_pce_prestacao_periodo <= 0)
                    lstErros.Add("Intervalo de vencimento inválido (parcelado com entrada).");
            }
            else if (pedidoDto.FormaPagtoCriacao.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
            {
                if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.Op_pse_prim_prest_forma_pagto))
                    lstErros.Add("Indique a forma de pagamento da 1ª prestação (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pse_prim_prest_valor.ToString()))
                    lstErros.Add("Indique o valor da 1ª prestação (parcelado sem entrada).");
                else if (pedidoDto.FormaPagtoCriacao.C_pse_prim_prest_valor <= 0)
                    lstErros.Add("Valor da 1ª prestação inválido (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pse_prim_prest_apos.ToString()))
                    lstErros.Add("Indique o intervalo de vencimento da 1ª parcela (parcelado sem entrada).");
                else if (pedidoDto.FormaPagtoCriacao.C_pse_prim_prest_apos <= 0)
                    lstErros.Add("Intervalo de vencimento da 1ª parcela é inválido (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.Op_pse_demais_prest_forma_pagto))
                    lstErros.Add("Indique a forma de pagamento das demais prestações (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_qtde.ToString()))
                    lstErros.Add("Indique a quantidade das demais prestações (parcelado sem entrada).");
                else if (pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_qtde <= 0)
                    lstErros.Add("Quantidade de prestações inválida (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_valor.ToString()))
                    lstErros.Add("Indique o valor das demais prestações (parcelado sem entrada).");
                else if (pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_valor <= 0)
                    lstErros.Add("Valor de prestação inválido (parcelado sem entrada).");
                else if (string.IsNullOrEmpty(pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_periodo.ToString()))
                    lstErros.Add("Indique o intervalo de vencimento entre as parcelas (parcelado sem entrada).");
                else if (pedidoDto.FormaPagtoCriacao.C_pse_demais_prest_periodo < 0)
                    lstErros.Add("Intervalo de vencimento inválido (parcelado sem entrada).");
            }
            else
            {
                lstErros.Add("É obrigatório especificar a forma de pagamento");
            }

            if (lstErros.Count == 0)
                retorno = true;

            return retorno;
        }

        public decimal Calcular_Vl_Total(PedidoDto pedido)
        {
            decimal vl_total = 0M;

            foreach (var p in pedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    vl_total += (decimal)(p.Qtde * p.VlUnitario);
                }
            }

            return vl_total;
        }

        public decimal CalcularVl_Total_NF(PedidoDto pedido)
        {
            decimal vl_total_NF = 0M;

            foreach (var p in pedido.ListaProdutos)
            {
                //afazer: corrigir esse calculo, pois sempre teremos que calcular o total de NF
                //
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    if (pedido.PermiteRAStatus == 1)
                        vl_total_NF += (decimal)(p.Qtde * p.Preco_Lista);
                    else
                        vl_total_NF += (decimal)(p.Qtde * p.VlUnitario);
                }
            }

            return vl_total_NF;
        }

        private bool CalculaItens(PedidoDto pedidoDto, out decimal vlTotalFormaPagto)
        {
            bool retorno = true;
            decimal vl_total_NF = 0;
            decimal vl_total = 0;


            foreach (var p in pedidoDto.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    vl_total += (decimal)(p.Qtde * p.VlUnitario);
                    vl_total_NF += (decimal)(p.Qtde * p.Preco);//aqui vai o
                }
            }
            vlTotalFormaPagto = vl_total_NF;
            if (Math.Abs(vlTotalFormaPagto - vl_total_NF) > 0.1M)
                retorno = false;

            return retorno;
        }

        public async Task<IEnumerable<ProdutoDto>> BuscarProdutosDtoSelecionados(List<PedidoProdutosDtoPedido> lstProdutosPedido,
            string loja)
        {
            List<ProdutoDto> lstProdutoDto = (await produtoBll.BuscarTodosProdutos(loja)).ToList();
            //vamos filtrar os produtos que foram selecionados para verificar as regras
            List<ProdutoDto> lstProdutosDtoSelecionados = new List<ProdutoDto>();
            foreach (var p in lstProdutosPedido)
            {
                var lstProdutosDtoSelecionadosTask = (from c in lstProdutoDto
                                                      where c.Produto == p.NumProduto &&
                                                            c.Fabricante == p.Fabricante
                                                      select new ProdutoDto
                                                      {
                                                          Fabricante = c.Fabricante,
                                                          Fabricante_Nome = c.Fabricante_Nome,
                                                          Produto = c.Produto,
                                                          Descricao_html = c.Descricao_html,
                                                          Alertas = c.Alertas,
                                                          Estoque = c.Estoque,
                                                          Preco_lista = c.Preco_lista,
                                                          Qtde_Max_Venda = c.Qtde_Max_Venda
                                                          //QtdeSolicitada = p.Qtde

                                                      }).FirstOrDefault();

                lstProdutosDtoSelecionados.Add(lstProdutosDtoSelecionadosTask);
            }

            return lstProdutosDtoSelecionados;
        }

        public async Task<TorcamentistaEindicador> ValidaIndicadorOrcamentista(string indicador, List<string> lstErros)
        {
            TorcamentistaEindicador torcamentista = await Util.Util.BuscarOrcamentistaEIndicador(indicador, 
                contextoProvider.GetContextoLeitura());

            if (torcamentista == null)
            {
                lstErros.Add("Informe quem é o indicador.");
            }

            return torcamentista;
        }

        public void ValidarPercentualRT(float percComissao, float percentualMax, List<string> lstErros)
        {
            if (percComissao < 0 || percComissao > 100)
            {
                lstErros.Add("Percentual de comissão inválido.");
            }
            if (percComissao > percentualMax)
            {
                lstErros.Add("O percentual de comissão excede o máximo permitido.");
            }
        }

        public async Task<IEnumerable<TprodutoLoja>> VerificarProdutosSelecionados(List<PedidoProdutosDtoPedido> lstProdutos, List<string> lstErros, string loja)
        {
            List<TprodutoLoja> lstProdutoLoja = new List<TprodutoLoja>();

            foreach (var prod in lstProdutos)
            {
                if (prod.Qtde <= 0)
                {
                    lstErros.Add("Produto" + prod.NumProduto + " do fabricante " + prod.Fabricante + ": quantidade " +
                        prod.Qtde + " é inválida.");
                }

                var db = contextoProvider.GetContextoLeitura();

                var prodTask = from c in db.TprodutoLojas.Include(x => x.Tproduto)
                               where c.Tproduto.Fabricante == prod.Fabricante &&
                                     c.Tproduto.Produto == prod.NumProduto &&
                                     c.Loja == loja
                               select c;

                if (await prodTask.FirstOrDefaultAsync() == null)
                {
                    lstErros.Add("Produto " + prod.NumProduto + " do fabricante " + prod.Fabricante +
                        " NÃO está cadastrado.");
                }
                else
                {
                    TprodutoLoja tprodutoLoja = await prodTask.FirstOrDefaultAsync();

                    if (tprodutoLoja.Vendavel != "S")
                    {
                        lstErros.Add("Produto " + prod.NumProduto + " do fabricante " + prod.Fabricante +
                        " NÃO está disponível para venda.");
                    }
                    else if (prod.Qtde > tprodutoLoja.Qtde_Max_Venda)
                    {
                        lstErros.Add("Produto " + prod.NumProduto + " do fabricante " + prod.Fabricante +
                        " : quantidade " + prod.Qtde + " excede o máximo permitido.");
                    }
                    else
                    {
                        lstProdutoLoja.Add(tprodutoLoja);
                    }
                }
            }
            return lstProdutoLoja;
        }

        private int ObterQtdeParcelasFormaPagto(PedidoDto pedido)
        {
            FormaPagtoCriacaoDto formaPagto = pedido.FormaPagtoCriacao;
            int qtdeParcelas = 1;

            if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA)
                qtdeParcelas = 1;
            else if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                qtdeParcelas = 1;
            else if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                qtdeParcelas = (int)formaPagto.C_pc_qtde;
            else if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                qtdeParcelas = (int)formaPagto.C_pc_maquineta_qtde;
            else if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                qtdeParcelas = (int)formaPagto.C_pce_prestacao_qtde;
            else if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                qtdeParcelas = (int)formaPagto.C_pse_demais_prest_qtde++;

            return qtdeParcelas;
        }

        public string ObterSiglaFormaPagto(PedidoDto pedido)
        {
            FormaPagtoCriacaoDto formaPagto = pedido.FormaPagtoCriacao;
            string retorno = "";

            if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA)
                retorno = Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA;
            else if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                retorno = Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                retorno = Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                retorno = Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                retorno = Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                retorno = Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;

            return retorno;
        }

        public async Task<float> BuscarCoeficientePercentualCustoFinanFornec(PedidoDto pedido, short qtdeParcelas, string siglaPagto, List<string> lstErros)
        {
            float coeficiente = 0;

            var db = contextoProvider.GetContextoLeitura();

            if (siglaPagto == Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA)
                coeficiente = 1;
            else
            {
                foreach (var i in pedido.ListaProdutos)
                {
                    var percCustoTask = from c in db.TpercentualCustoFinanceiroFornecedors
                                        where c.Fabricante == i.Fabricante &&
                                              c.Tipo_Parcelamento == siglaPagto &&
                                              c.Qtde_Parcelas == qtdeParcelas
                                        select c;

                    var percCusto = await percCustoTask.FirstOrDefaultAsync();

                    if (percCusto != null)
                    {
                        coeficiente = percCusto.Coeficiente;
                        i.VlLista = (decimal)coeficiente * (decimal)i.Preco;
                    }
                    else
                    {
                        lstErros.Add("Opção de parcelamento não disponível para fornecedor " + i.Fabricante + ": " +
                            DecodificaCustoFinanFornecQtdeParcelas(pedido.FormaPagtoCriacao.C_forma_pagto, qtdeParcelas) + " parcela(s)");
                    }

                }
            }

            return coeficiente;
        }
        private string DecodificaCustoFinanFornecQtdeParcelas(string tipoParcelamento, short custoFFQtdeParcelas)
        {
            string retorno = "";

            if (tipoParcelamento == Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA)
                retorno = "0+" + custoFFQtdeParcelas;
            else if (tipoParcelamento == Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA)
                retorno = "1+" + custoFFQtdeParcelas;

            return retorno;
        }

        private bool ValidarEndecoEntrega(EnderecoEntregaDtoClienteCadastro endEtg, List<string> lstErros)
        {
            bool retorno = true;

            if (endEtg.OutroEndereco)
            {
                if (string.IsNullOrEmpty(endEtg.EndEtg_endereco))
                {
                    lstErros.Add("PREENCHA O ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }
                if (endEtg.EndEtg_endereco.Length > Constantes.Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
                {
                    lstErros.Add("ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: "
                        + endEtg.EndEtg_endereco.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                        Constantes.Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");
                    retorno = false;
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_endereco_numero))
                {
                    lstErros.Add("PREENCHA O NÚMERO DO ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_bairro))
                {
                    lstErros.Add("PREENCHA O BAIRRO DO ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_cidade))
                {
                    lstErros.Add("PREENCHA A CIDADE DO ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }
                if (string.IsNullOrEmpty(endEtg.EndEtg_uf) || !Util.Util.VerificaUf(endEtg.EndEtg_uf))
                {
                    lstErros.Add("UF INVÁLIDA NO ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }
                if (!Util.Util.VerificaCep(endEtg.EndEtg_cep))
                {
                    lstErros.Add("CEP INVÁLIDO NO ENDEREÇO DE ENTREGA.");
                    retorno = false;
                }
            }

            return retorno;
        }

        public async Task<MeioPagtoPreferenciais> BuscarMeiosPagtoPreferenciais()
        {
            var tParametro = await Util.Util.BuscarRegistroParametro(Constantes.Constantes.
                    ID_PARAMETRO_PercMaxComissaoEDesconto_Nivel2_MeiosPagto, contextoProvider.GetContextoLeitura());

            MeioPagtoPreferenciais meioPagtoPreferenciais = new MeioPagtoPreferenciais();

            meioPagtoPreferenciais.Id = tParametro.Id;
            meioPagtoPreferenciais.Campo_inteiro = tParametro.Campo_inteiro;
            meioPagtoPreferenciais.Campo_monetario = tParametro.Campo_monetario;
            meioPagtoPreferenciais.Campo_real = tParametro.Campo_real;
            meioPagtoPreferenciais.Campo_texto = tParametro.Campo_texto;
            meioPagtoPreferenciais.Dt_hr_ult_atualizacao = tParametro.Dt_hr_ult_atualizacao;
            meioPagtoPreferenciais.Usuario_ult_atualizacao = tParametro.Usuario_ult_atualizacao;

            return meioPagtoPreferenciais;
        }

        public async Task<IEnumerable<ObjetoSenhaDesconto>> BuscarSenhaDesconto(string cliente_id, string loja)
        {
            List<Tdesconto> lst_tdesconto = (await Util.Util.BuscarListaIndicadoresLoja(cliente_id, loja, contextoProvider)).ToList();

            ObjetoSenhaDesconto objSenhaDesc = new ObjetoSenhaDesconto();

            List<ObjetoSenhaDesconto> lstObjetoSenhaDescontos = new List<ObjetoSenhaDesconto>();

            foreach (var i in lst_tdesconto)
            {
                lstObjetoSenhaDescontos.Add(new ObjetoSenhaDesconto
                {
                    Id = i.Id,
                    Fabricante = i.Fabricante,
                    Produto = i.Produto,
                    Desc_Max = i.Desc_max,
                    Data = i.Data,
                    IdCliente = i.Id_cliente,
                    Cpf_Cnpj = i.Cnpj_cpf,
                    Loja = i.Loja,
                    Autorizador = i.Autorizador,
                    supervisor_autorizador = i.Supervisor_autorizador
                });
            }

            return lstObjetoSenhaDescontos;

        }
    }
}
