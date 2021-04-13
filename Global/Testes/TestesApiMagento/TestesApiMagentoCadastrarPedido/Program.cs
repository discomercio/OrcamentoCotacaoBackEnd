using MagentoBusiness.MagentoDto.PedidoMagentoDto;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TestesApiMagentoCadastrarPedido
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuracaoTestes = ConfiguracaoTestes.LerConfiguracao();
            CriarPedidos(configuracaoTestes);
            EsperarQualquerTeclaParaSair();
        }

        private static readonly HttpClient client = new HttpClient();
        private static void CriarPedidos(ConfiguracaoTestes configuracaoTestes)
        {
            for (var execucao = 0; execucao < configuracaoTestes.NumeroExecucoes; execucao++)
            {
                var dto = CriarPedidoBase();

                PedidoResultadoMagentoDto res = EnviarRequisicao(configuracaoTestes, dto);
                if (res.ListaErros.Count == 0)
                {
                    Console.WriteLine($"Execucao {execucao}: pedido criado {res.IdPedidoCadastrado}");
                }
                else
                {
                    Console.WriteLine($"Execucao {execucao}: erros {String.Join(" - ", res.ListaErros)}");
                }
            }
        }

        private static PedidoResultadoMagentoDto EnviarRequisicao(ConfiguracaoTestes configuracaoTestes, PedidoMagentoDto dto)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var jsonContent = JsonConvert.SerializeObject(dto);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = client.PostAsync(configuracaoTestes.UrlApiMagentoCadastrarPedido, contentString).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoResultadoMagentoDto>(responseString);
            return res;
        }

        private static MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto CriarPedidoBase()
        {
            var dto = Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoDados.PedidoBase_para_banco_ARCLUBE_DIS20201204();
            //precismaos alterar o CPF e o número do pedido magento
            var novoCpf = Especificacao.Testes.Utils.CpfCnpj.GerarCpf();
            dto.Cnpj_Cpf = novoCpf;
            dto.EnderecoCadastralCliente.Endereco_cnpj_cpf = novoCpf;
            if (dto.EnderecoEntrega != null)
                dto.EnderecoEntrega.EndEtg_cnpj_cpf = novoCpf;

            //agora o número magento
            var numeroMagento = (new Random().Next(100000001, 199999999)).ToString();
            //pra dar erro
            //numeroMagento = (new Random().Next(00000001, 99999999)).ToString();
            dto.InfCriacaoPedido.Pedido_bs_x_ac = numeroMagento;
            dto.InfCriacaoPedido.Pedido_bs_x_marketplace = numeroMagento;
            return dto;
        }

        private static void EsperarQualquerTeclaParaSair()
        {
            Console.WriteLine("Aperte qualquer tecla para sair");
            while (Console.KeyAvailable)
                Console.ReadKey();
            Console.ReadKey();
        }
    }
}
