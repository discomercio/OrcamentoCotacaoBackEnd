using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Dto.Produto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrepedidoBusiness.Dto.ClienteCadastro;
using InfraBanco.Constantes;
using PrepedidoBusiness.Utils;
using InfraBanco.Modelos;
using PrepedidoBusiness.Bll.ClienteBll;
using PrepedidoBusiness.Dto.Cep;

namespace PrepedidoBusiness.Bll.PrepedidoBll
{
    public class ValidacoesPrepedidoBll
    {
        private readonly CoeficienteBll coeficienteBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly ClienteBll.ClienteBll clienteBll;
        private readonly CepBll cepBll;


        public ValidacoesPrepedidoBll(CoeficienteBll coeficienteBll, InfraBanco.ContextoBdProvider contextoProvider,
            ClienteBll.ClienteBll clienteBll, CepBll cepBll)
        {
            this.coeficienteBll = coeficienteBll;
            this.contextoProvider = contextoProvider;
            this.clienteBll = clienteBll;
            this.cepBll = cepBll;
        }

        //vamos validar os produtos que foram enviados
        public async Task MontarProdutosParaComparacao(PrePedidoDto prepedido,
                    string siglaFormaPagto, int qtdeParcelas, string loja, List<string> lstErros, float perc_limite_RA)
        {
            List<PrepedidoProdutoDtoPrepedido> lstProdutosParaComparacao = new List<PrepedidoProdutoDtoPrepedido>();
            List<List<CoeficienteDto>> lstCoeficienteDtoArclube = new List<List<CoeficienteDto>>();
            List<CoeficienteDto> coefDtoArclube = new List<CoeficienteDto>();

            List<string> lstFornec = new List<string>();
            lstFornec = prepedido.ListaProdutos.Select(x => x.Fabricante).Distinct().ToList();

            //buscar coeficiente 
            List<CoeficienteDto> lstCoeficiente =
                (await BuscarListaCoeficientesFornecedores(lstFornec, qtdeParcelas, siglaFormaPagto)).ToList();

            List<PrepedidoProdutoDtoPrepedido> lstProdutosCompare =
                (await BuscarProdutos(prepedido.ListaProdutos, loja)).ToList();

            lstCoeficiente.ForEach(x =>
            {
                lstProdutosCompare.ForEach(y =>
                {
                    if (x.Fabricante == y.Fabricante)
                    {
                        //vamos calcular o preco_lista com o coeficiente
                        y.VlLista = Math.Round(((decimal)y.Preco * (decimal)x.Coeficiente), 2);
                        y.VlUnitario = Math.Round(y.VlLista * (decimal)(1 - y.Desconto / 100), 2);
                        y.TotalItem = Math.Round((decimal)(y.VlUnitario * y.Qtde), 2);
                        y.TotalItemRA = Math.Round((decimal)(y.VlLista * y.Qtde), 2);
                    }
                });
            });

            ConfrontarProdutos(prepedido, lstProdutosCompare, lstErros);
            ConfrontarTotaisEPercentualMaxRA(prepedido, lstProdutosCompare, lstErros, perc_limite_RA);
        }

        private async Task<IEnumerable<CoeficienteDto>> BuscarListaCoeficientesFornecedores(List<string> lstFornecedores, int qtdeParcelas, string siglaFP)
        {
            List<CoeficienteDto> lstcoefDto = new List<CoeficienteDto>();

            var lstcoeficientesTask = coeficienteBll.BuscarListaCoeficientesFornecedores(lstFornecedores);
            if (lstcoeficientesTask != null)
            {
                foreach (var i in await lstcoeficientesTask)
                {
                    lstcoefDto = new List<CoeficienteDto>();
                    foreach (var y in i)
                    {
                        if (y.TipoParcela == siglaFP && y.QtdeParcelas == qtdeParcelas)
                        {
                            lstcoefDto.Add(new CoeficienteDto()
                            {
                                Fabricante = y.Fabricante,
                                TipoParcela = y.TipoParcela,
                                QtdeParcelas = y.QtdeParcelas,
                                Coeficiente = y.Coeficiente
                            });
                        }
                    }
                }
            }

            return lstcoefDto;
        }

        private async Task<IEnumerable<PrepedidoProdutoDtoPrepedido>> BuscarProdutos(List<PrepedidoProdutoDtoPrepedido> lstProdutos, string loja)
        {
            var db = contextoProvider.GetContextoLeitura();
            List<PrepedidoProdutoDtoPrepedido> lsProdutosCompare = new List<PrepedidoProdutoDtoPrepedido>();

            foreach(var x in lstProdutos)
            {
                PrepedidoProdutoDtoPrepedido produto = await (from c in db.TprodutoLojas
                                                              where c.Produto == x.NumProduto &&
                                                                    c.Fabricante == x.Fabricante &&
                                                                    c.Vendavel == "S" &&
                                                                    c.Loja == loja
                                                              select new PrepedidoProdutoDtoPrepedido
                                                              {
                                                                  Fabricante = c.Fabricante,
                                                                  NumProduto = c.Produto,
                                                                  Preco = c.Preco_Lista,
                                                                  Desconto = x.Desconto,
                                                                  Qtde = x.Qtde
                                                              }).FirstOrDefaultAsync();

                if (produto != null)
                {
                    lsProdutosCompare.Add(produto);
                }
            }

            return lsProdutosCompare;
        }

        private void ConfrontarProdutos(PrePedidoDto prepedido,
            List<PrepedidoProdutoDtoPrepedido> lstProdutosCompare, List<string> lstErros)
        {
            decimal diffVlLista = 0;
            decimal diffVlUnitario = 0;
            decimal limite = 0.01M;
            decimal diffTotalItem = 0;
            decimal diffTotalItemRa = 0;

            prepedido.ListaProdutos.ForEach(x =>
            {
                lstProdutosCompare.ForEach(y =>
               {
                   if (x.NumProduto == y.NumProduto && x.Fabricante == y.Fabricante)
                   {
                       diffVlLista = Math.Abs(x.VlLista - y.VlLista);
                       diffVlUnitario = Math.Abs((x.VlUnitario - y.VlUnitario));

                       //afazer: pegar o valor do appsettings
                       if (diffVlLista > limite || diffVlUnitario > limite)
                       {
                           lstErros.Add("O valor do Produto (cód.)" + x.NumProduto + " está divergindo!");
                       }
                       else
                       {
                           diffTotalItem = Math.Abs((decimal)(x.TotalItem - y.TotalItem));
                           diffTotalItemRa = Math.Abs((decimal)(x.TotalItemRA - y.TotalItemRA));

                           if (diffTotalItem > limite || diffTotalItemRa > limite)
                               lstErros.Add("O valor total do Produto (cód.)" + x.NumProduto +
                                   " está divergindo!");
                       }
                   }
               });
            });




            //vamos verificar se o percentual de RA é maior que o máximo permitido

        }

        private void ConfrontarTotaisEPercentualMaxRA(PrePedidoDto prepedido,
            List<PrepedidoProdutoDtoPrepedido> lstProdutosCompare, List<string> lstErros, float perc_limite_RA)
        {
            //PAREI VAIDANDO OS VALORES DE PRODUTOS E TOTAIS
            decimal totalCompare = 0;
            decimal totalRaCompare = 0;
            decimal limite = 0.01M;

            lstProdutosCompare.ForEach(x =>
            {
                totalCompare += Math.Round((decimal)(x.TotalItem), 2);
                totalRaCompare += Math.Round((decimal)(x.TotalItemRA), 2);
            });

            if (Math.Abs((decimal)(totalCompare - prepedido.VlTotalDestePedido)) > limite)
                lstErros.Add("Os valores totais estão divergindo!");

            if(prepedido.PermiteRAStatus == 1)
            {
                if (Math.Abs((decimal)(totalRaCompare - prepedido.ValorTotalDestePedidoComRA)) > limite)
                    lstErros.Add("Os valores totais de RA estão divergindo!");

                //vamos verificar o valor de RA
                decimal ra = totalRaCompare - totalCompare;
                decimal perc = Math.Round((decimal)(perc_limite_RA / 100), 2);
                decimal percentual = Math.Round(perc * (decimal)prepedido.VlTotalDestePedido, 2);

                if (ra > percentual)
                    lstErros.Add("O valor total de RA excede o limite permitido!");
            }
        }

        public bool CalculaItens(PrePedidoDto prePedido, out decimal vlTotalFormaPagto)
        {
            bool retorno = true;
            decimal vl_total_NF = 0;
            decimal vl_total = 0;


            foreach (var p in prePedido.ListaProdutos)
            {
                if (!string.IsNullOrEmpty(p.NumProduto))
                {
                    vl_total += (decimal)(p.Qtde * p.VlUnitario);
                    vl_total_NF += (decimal)(p.Qtde * p.Preco);
                }
            }
            vlTotalFormaPagto = vl_total_NF;
            if (Math.Abs(vlTotalFormaPagto - vl_total_NF) > 0.1M)
                retorno = false;

            return retorno;
        }

        public async Task<bool> ValidarEnderecoEntrega(PrePedidoDto prepedido, List<string> lstErros)
        {
            bool retorno = true;

            await ValidarDadosEnderecoEntrega(prepedido, lstErros);

            await ValidarDadosPessoaEnderecoEntrega(prepedido, lstErros);

            if (lstErros.Count != 0)
                retorno = false;

            return retorno;
        }
        private async Task ValidarDadosEnderecoEntrega(PrePedidoDto prePedido, List<string> lstErros)
        {

            if (prePedido.EnderecoEntrega.OutroEndereco)
            {
                if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_cod_justificativa))
                    lstErros.Add("SELECIONE A JUSTIFICATIVA DO ENDEREÇO DE ENTREGA!");

                if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_endereco))
                    lstErros.Add("PREENCHA O ENDEREÇO DE ENTREGA.");

                if (prePedido.EnderecoEntrega.EndEtg_endereco.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO)
                    lstErros.Add("ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " + prePedido.EnderecoEntrega.EndEtg_endereco.Length +
                        " CARACTERES<br>TAMANHO MÁXIMO: " + Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES");

                if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_endereco_numero))
                    lstErros.Add("PREENCHA O NÚMERO DO ENDEREÇO DE ENTREGA.");

                if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_bairro))
                    lstErros.Add("PREENCHA O BAIRRO DO ENDEREÇO DE ENTREGA.");

                if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_cidade))
                    lstErros.Add("PREENCHA A CIDADE DO ENDEREÇO DE ENTREGA.");

                if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_uf) || !Util.VerificaUf(prePedido.EnderecoEntrega.EndEtg_uf))
                    lstErros.Add("UF INVÁLIDA NO ENDEREÇO DE ENTREGA.");

                if (!Util.VerificaCep(prePedido.EnderecoEntrega.EndEtg_cep))
                    lstErros.Add("CEP INVÁLIDO NO ENDEREÇO DE ENTREGA.");

                if(lstErros.Count == 0)
                {
                    List<NfeMunicipio> lstNfeMunicipio = (await ValidacoesClienteBll.ConsisteMunicipioIBGE(
                    prePedido.EnderecoEntrega.EndEtg_cidade, prePedido.EnderecoEntrega.EndEtg_uf, lstErros, contextoProvider)).ToList();

                    //vamos comparar endereço
                    string cepSoDigito = prePedido.EnderecoEntrega.EndEtg_cep.Replace(".", "").Replace("-", "");
                    List<CepDto> lstCepDto = (await cepBll.BuscarPorCep(cepSoDigito)).ToList();

                    CepDto cep = new CepDto()
                    {
                        Cep = prePedido.EnderecoEntrega.EndEtg_cep,
                        Endereco = prePedido.EnderecoEntrega.EndEtg_endereco,
                        Bairro = prePedido.EnderecoEntrega.EndEtg_bairro,
                        Cidade = prePedido.EnderecoEntrega.EndEtg_cidade,
                        Uf = prePedido.EnderecoEntrega.EndEtg_uf
                    };
                    ValidacoesClienteBll.VerificarEndereco(cep, lstCepDto, lstErros);
                }
                
            }
        }

        public bool ValidarDetalhesPrepedido(DetalhesDtoPrepedido detalhesPrepedido, List<string> lstErros)
        {
            bool retorno = true;

            if (detalhesPrepedido.EntregaImediata ==
                Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO.ToString())//Não
            {
                if (detalhesPrepedido.EntregaImediataData <= DateTime.Now)
                {
                    lstErros.Add("Favor informar a data de 'Entrega Imediata' posterior a data atual!");
                    retorno = false;
                }
            }

            return retorno;
        }

        private async Task ValidarDadosPessoaEnderecoEntrega(PrePedidoDto prePedido, List<string> lstErros)
        {
            var cliente = await clienteBll.BuscarCliente(prePedido.DadosCliente.Cnpj_Cpf,
                prePedido.DadosCliente.Indicador_Orcamentista);

            //incluir a flag blnUsarMemorizacaoCompletaEnderecos
            if (Util.ValidaCNPJ(cliente.DadosCliente.Cnpj_Cpf))
            {
                if (prePedido.EnderecoEntrega.EndEtg_tipo_pessoa != Constantes.ID_PJ &&
                    prePedido.EnderecoEntrega.EndEtg_tipo_pessoa != Constantes.ID_PF)
                {
                    lstErros.Add("Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!");
                }
                else if (string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_nome))
                {
                    lstErros.Add("Preencha o nome/razão social no endereço de entrega!");
                }
                else
                {
                    if (prePedido.EnderecoEntrega.EndEtg_tipo_pessoa == Constantes.ID_PJ)
                    {
                        ValidarDadosPessoaEnderecoEntrega_PJ(prePedido.EnderecoEntrega, lstErros);
                        ValidarDadosPessoaEnderecoEntrega_PJ_Tel(prePedido.EnderecoEntrega, lstErros);
                    }
                    if (prePedido.EnderecoEntrega.EndEtg_tipo_pessoa == Constantes.ID_PF)
                    {
                        ValidarDadosPessoaEnderecoEntrega_PF(prePedido.EnderecoEntrega, lstErros);
                        ValidarDadosPessoaEnderecoEntrega_PF_Tel(prePedido.EnderecoEntrega, lstErros);
                    }
                }

                if (lstErros.Count == 0)
                {
                    if (!string.IsNullOrEmpty(prePedido.EnderecoEntrega.EndEtg_ie))
                    {
                        ValidacoesClienteBll.VerificarInscricaoEstadualValida(
                            prePedido.EnderecoEntrega.EndEtg_ie, prePedido.EnderecoEntrega.EndEtg_uf, lstErros);
                    }
                }

            }
        }

        private void ValidarDadosPessoaEnderecoEntrega_PJ(EnderecoEntregaDtoClienteCadastro endEtg, List<string> lstErros)
        {
            if (endEtg.EndEtg_tipo_pessoa == Constantes.ID_PJ)
            {
                if (string.IsNullOrEmpty(endEtg.EndEtg_cnpj_cpf) ||
                    !Util.ValidaCNPJ(endEtg.EndEtg_cnpj_cpf))
                    lstErros.Add("Endereço de entrega: CNPJ inválido!");

                if (string.IsNullOrEmpty(endEtg.EndEtg_contribuinte_icms_status.ToString()))
                    lstErros.Add("Endereço de entrega: selecione o tipo de contribuinte de ICMS!");

                if (endEtg.EndEtg_contribuinte_icms_status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                    string.IsNullOrEmpty(endEtg.EndEtg_ie))
                    lstErros.Add("Endereço de entrega: se o cliente é contribuinte do ICMS a inscrição " +
                        "estadual deve ser preenchida!");

                if (endEtg.EndEtg_contribuinte_icms_status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                    endEtg.EndEtg_ie.IndexOf("ISEN") > -1)
                    lstErros.Add("Endereço de entrega: se cliente é não contribuinte do ICMS, " +
                        "não pode ter o valor ISENTO no campo de Inscrição Estadual!");

                if (endEtg.EndEtg_contribuinte_icms_status ==
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                    endEtg.EndEtg_ie.IndexOf("ISEN") > -1)
                    lstErros.Add("Endereço de entrega: se cliente é contribuinte do ICMS, " +
                        "não pode ter o valor ISENTO no campo de Inscrição Estadual!");

                if (!string.IsNullOrEmpty(endEtg.EndEtg_ie))
                    ValidacoesClienteBll.VerificarInscricaoEstadualValida(endEtg.EndEtg_ie,
                        endEtg.EndEtg_uf, lstErros);
            }
        }

        private void ValidarDadosPessoaEnderecoEntrega_PJ_Tel(EnderecoEntregaDtoClienteCadastro endEtd, List<string> lstErros)
        {
            if (!string.IsNullOrEmpty(endEtd.EndEtg_tel_com))
            {
                if (Util.Telefone_SoDigito(endEtd.EndEtg_tel_com).Length < 6)
                    lstErros.Add("Endereço de entrega: telefone comercial inválido!");

                if (!string.IsNullOrEmpty(endEtd.EndEtg_ddd_com))
                {
                    if (endEtd.EndEtg_ddd_com.Length != 2)
                        lstErros.Add("Endereço de entrega: ddd do telefone comercial inválido!");
                }
            }

            if (!string.IsNullOrEmpty(endEtd.EndEtg_tel_com_2))
            {
                if (Util.Telefone_SoDigito(endEtd.EndEtg_tel_com_2).Length < 6)
                    lstErros.Add("Endereço de entrega: telefone comercial 2 inválido!");

                if (!string.IsNullOrEmpty(endEtd.EndEtg_ddd_com_2))
                {
                    if (endEtd.EndEtg_ddd_com_2.Length != 2)
                        lstErros.Add("Endereço de entrega: ddd do telefone comercial 2 inválido!");
                }
            }

        }

        private void ValidarDadosPessoaEnderecoEntrega_PF(EnderecoEntregaDtoClienteCadastro endEtg, List<string> lstErros)
        {
            if (endEtg.EndEtg_tipo_pessoa == Constantes.ID_PF)
            {
                if (string.IsNullOrEmpty(endEtg.EndEtg_cnpj_cpf) ||
                    !Util.ValidaCPF(endEtg.EndEtg_cnpj_cpf))
                    lstErros.Add("Endereço de entrega: CPF inválido!");

                if (string.IsNullOrEmpty(endEtg.EndEtg_produtor_rural_status.ToString()))
                    lstErros.Add("Endereço de entrega: informe se o cliente é produtor rural ou não!");

                if (endEtg.EndEtg_produtor_rural_status !=
                    (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO && 
                    endEtg.EndEtg_tipo_pessoa == Constantes.ID_PF)
                {
                    if (endEtg.EndEtg_contribuinte_icms_status !=
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)
                        lstErros.Add("Endereço de entrega: para ser cadastrado como Produtor Rural, " +
                            "é necessário ser contribuinte do ICMS e possuir nº de IE!");

                    if (string.IsNullOrEmpty(endEtg.EndEtg_contribuinte_icms_status.ToString()))
                        lstErros.Add("Endereço de entrega: informe se o cliente é contribuinte do ICMS, " +
                            "não contribuinte ou isento!");

                    if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        string.IsNullOrEmpty(endEtg.EndEtg_ie))
                        lstErros.Add("Endereço de entrega: se o cliente é contribuinte do ICMS a " +
                            "inscrição estadual deve ser preenchida!");

                    if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO &&
                        endEtg.EndEtg_ie.IndexOf("ISEN") > -1)
                        lstErros.Add("Endereço de entrega: se cliente é não contribuinte do ICMS, " +
                            "não pode ter o valor ISENTO no campo de Inscrição Estadual!");

                    if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        endEtg.EndEtg_ie.IndexOf("ISEN") > -1)
                        lstErros.Add("Endereço de entrega: se cliente é contribuinte do ICMS, " +
                            "não pode ter o valor ISENTO no campo de Inscrição Estadual!");

                    if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO &&
                        !string.IsNullOrEmpty(endEtg.EndEtg_ie))
                        lstErros.Add("Endereço de entrega: se o Contribuinte ICMS é isento, " +
                            "o campo IE deve ser vazio!");

                    if (endEtg.EndEtg_contribuinte_icms_status ==
                        (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM &&
                        endEtg.EndEtg_produtor_rural_status != (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM)
                        lstErros.Add("Endereço de entrega: se cliente é contribuinte do ICMS, ele dever ser Produtor Rural!");

                }
            }
        }

        private void ValidarDadosPessoaEnderecoEntrega_PF_Tel(EnderecoEntregaDtoClienteCadastro endEtd, List<string> lstErros)
        {
            if (!string.IsNullOrEmpty(endEtd.EndEtg_tel_res))
            {
                if (Util.Telefone_SoDigito(endEtd.EndEtg_tel_res).Length < 6)
                    lstErros.Add("Endereço de entrega: telefone residencial inválido.");

                if (endEtd.EndEtg_ddd_res.Length != 2)
                    lstErros.Add("ddd residencial inválido.");
            }

            if (!string.IsNullOrEmpty(endEtd.EndEtg_ddd_cel))
            {
                if (Util.Telefone_SoDigito(endEtd.EndEtg_tel_cel).Length < 6)
                    lstErros.Add("Endereço de entrega: telefone celular inválido.");

                if (endEtd.EndEtg_ddd_cel.Length != 2)
                    lstErros.Add("ddd do celular inválido.");
            }
        }
    }
}
