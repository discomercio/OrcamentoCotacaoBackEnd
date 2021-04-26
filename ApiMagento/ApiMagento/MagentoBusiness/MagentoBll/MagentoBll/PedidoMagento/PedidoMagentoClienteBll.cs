using MagentoBusiness.MagentoDto.PedidoMagentoDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pedido.Dados;
using MagentoBusiness.MagentoDto.ClienteMagentoDto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InfraBanco.Constantes;
using Prepedido;
using MagentoBusiness.UtilsMagento;
using MagentoBusiness.MagentoDto;
using MagentoBusiness.MagentoDto.MarketplaceDto;
using InfraBanco.Modelos;

#nullable enable

namespace MagentoBusiness.MagentoBll.MagentoBll
{
    public class PedidoMagentoClienteBll
    {
        private readonly Cliente.ClienteBll clienteBll;
        private readonly ConfiguracaoApiMagento configuracaoApiMagento;

        public PedidoMagentoClienteBll(Cliente.ClienteBll clienteBll,
            ConfiguracaoApiMagento configuracaoApiMagento)
        {
            this.clienteBll = clienteBll;
            this.configuracaoApiMagento = configuracaoApiMagento;
        }

        internal async Task<Tcliente?> CadastrarClienteSeNaoExistir(PedidoMagentoDto pedidoMagento, List<string> listaErros,
            PedidoMagentoBll.Indicador_vendedor_loja indicador_Vendedor_Loja, string usuario_cadastro)
        {
            var idCliente = await clienteBll.BuscarTcliente(pedidoMagento.Cnpj_Cpf).FirstOrDefaultAsync();
            if (idCliente != null)
                return idCliente;

            Cliente.Dados.ClienteCadastroDados clienteCadastro = new Cliente.Dados.ClienteCadastroDados
            {
                DadosCliente =
                EnderecoCadastralClienteMagentoDto.DadosClienteDeEnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente,
                    indicador_Vendedor_Loja.loja, indicador_Vendedor_Loja.vendedor,
                    configuracaoApiMagento.DadosIndicador.Indicador, ""),
                RefBancaria = new List<Cliente.Dados.Referencias.RefBancariaClienteDados>(),
                RefComercial = new List<Cliente.Dados.Referencias.RefComercialClienteDados>()
            };

            List<string> lstRet = (await clienteBll.CadastrarCliente(clienteCadastro,
                indicador_Vendedor_Loja.indicador,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO,
                usuario_cadastro)).ToList();

            //tem erro?
            listaErros.AddRange(lstRet);
            idCliente = await clienteBll.BuscarTcliente(pedidoMagento.Cnpj_Cpf).FirstOrDefaultAsync();
            return idCliente;
        }

        internal void LimitarPedidosMagentoPJ(PedidoMagentoDto pedidoMagento, List<string> lstErros)
        {
            /*
             * definido em 201021 
             
            magento: problema no cadastro de PJ, vai puxar do estoque errado se for contribuinte de ICMS.
            Hoje não usa, mas é importante ter o recurso.
            O problema é: se a gente presumir o ICMS da PJ, vamos criar o pedido pegando do estoque errado.
            Hamilton vai conversar com Karina para saber como funciona. Mas é um BELO problema.

            Boa tarde
            @Edu  conversei com a @Karina e ficou decidido que neste primeiro momento a integração com o Magento 
            não irá tratar os pedidos de clientes PJ. Esses pedidos continuarão sendo cadastrados através do 
            processo semi-automático. Então creio que seria melhor fazer normalmente a validação do campo de 
            contribuinte ICMS para rejeitar os pedidos que vierem sem essa informação p/ garantir a consistência 
            dos dados caso seja enviado um pedido de cliente PJ.

            Conversei com o time e pegando alguns pontos que eles comentaram é melhor seguir com semi-automático mesmo e no futuro se surgir alguma ideia ou solução a gente adapta. 

            Resumo: API do Magento para PJ não aceita nenhum pedido, tods serão feitos no semi-automático
            */
            if (pedidoMagento.EnderecoCadastralCliente.Endereco_tipo_pessoa != Constantes.ID_PF)
                lstErros.Add("A API Magento somente aceita pedidos para PF (EnderecoCadastralCliente.Endereco_tipo_pessoa).");

        }

        internal void MoverEnderecoEntregaParaEnderecoCadastral(PedidoMagentoDto pedidoMagento, List<string> listaErros)
        {
            //somente para PF, garantindo mesmo
            LimitarPedidosMagentoPJ(pedidoMagento, listaErros);

            //endereço de entrega exigido
            if (!pedidoMagento.OutroEndereco || pedidoMagento.EnderecoEntrega == null)
                listaErros.Add("Obrigatório informar um endereço de entrega na API Magento para cliente PF.");
            if (listaErros.Count > 0)
                return;

            pedidoMagento.Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(pedidoMagento.Cnpj_Cpf);
            //linha abaixo, só para o compilaodr não reclamar. Já garantimos que não é null mais acima.
            pedidoMagento.EnderecoEntrega ??= new EnderecoEntregaClienteMagentoDto();
            pedidoMagento.EnderecoEntrega.EndEtg_cnpj_cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(pedidoMagento.EnderecoEntrega.EndEtg_cnpj_cpf);

            //exigimos que o CPF/CNPJ esteja igual nos dois blocos de informação
            if (pedidoMagento.Cnpj_Cpf != pedidoMagento.EnderecoEntrega.EndEtg_cnpj_cpf)
            {
                listaErros.Add("Cnpj_Cpf está diferente de EnderecoEntrega.EndEtg_cnpj_cpf.");
            }

            //vamos mover os campos
            pedidoMagento.EnderecoCadastralCliente = EnderecoCadastralClienteMagentoDto.EnderecoCadastralClienteMagentoDto_De_EnderecoEntregaClienteMagentoDto(
                pedidoMagento.EnderecoEntrega,
                pedidoMagento.EnderecoCadastralCliente);

            pedidoMagento.OutroEndereco = false;
            pedidoMagento.EnderecoEntrega = null;
        }

    }
}
