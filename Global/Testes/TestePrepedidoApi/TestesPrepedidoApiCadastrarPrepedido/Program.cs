using Newtonsoft.Json;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TestesPrepedidoApiCadastrarPrepedido
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(30, 30);
            var configuracaoTestes = ConfiguracaoTestes.LerConfiguracao();
            CriarPrePedidos(configuracaoTestes);
            EsperarQualquerTeclaParaSair();
        }

        private static void CriarPrePedidos(ConfiguracaoTestes configuracaoTestes)
        {
            CarregarHeader(configuracaoTestes);
            //fazer login
            FazerLogin(configuracaoTestes);

            for (var execucao = 0; execucao < configuracaoTestes.NumeroExecucoes; execucao++)
            {
                var dto = CriarPrepPedidoBase();

                List<string> res = EnviarRequisicao(configuracaoTestes, dto);
                if (res.Count == 0)
                {
                    Console.WriteLine($"Execucao {execucao}: pedido criado {res}");
                }
                else
                {
                    Console.WriteLine($"Execucao {execucao}: erros {String.Join(" - ", res)}");
                }
            }
        }

        private static string token;
        private static readonly HttpClient client = new HttpClient();
        private static void FazerLogin(ConfiguracaoTestes configuracaoTestes)
        {
            StringContent contentString = PrepararLogin(configuracaoTestes);

            var response = client.PostAsync(configuracaoTestes.UrlPrepedidoApiFazerLogin, contentString).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            var res = JsonConvert.DeserializeObject<string>(responseString);
            token = res;
        }

        private static StringContent PrepararLogin(ConfiguracaoTestes configuracaoTestes)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jsonContent = JsonConvert.SerializeObject(new
            {
                apelido = configuracaoTestes.apelido,
                senha = configuracaoTestes.senha
            });

            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            contentString.Headers.Add("X-API-Version", xApiVersion);
            return contentString;
        }

        private static string xApiVersion = "DEBUG";
        
        private static void CarregarHeader(ConfiguracaoTestes configuracaoTestes)
        {
            StringContent contentString = PrepararLogin(configuracaoTestes);

            var response = client.PostAsync(configuracaoTestes.UrlPrepedidoApiFazerLogin, contentString).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            //verificar se responseString tem o x-api-version: 
            if(responseString.IndexOf("X-API-Version: ") > -1)
            {
                string textoCompleto = responseString.Substring(responseString.IndexOf("X-API-Version: "));
                string[] textoQuebrado = textoCompleto.Split("X-API-Version: ");
                xApiVersion = textoQuebrado.Where(c => !string.IsNullOrEmpty(c)).Select(c => c).FirstOrDefault();
            }
        }

        private static PrePedidoDto LerJson()
        {
            string texto;
            using (var stream = new StreamReader("../../../PrepedidoDto.json", Encoding.GetEncoding(28591)))
            {
                texto = stream.ReadToEnd();
            }

            var json = JsonConvert.DeserializeObject<PrePedidoDto>(texto);
            return json;

        }

        private static PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido.PrePedidoDto CriarPrepPedidoBase()
        {
            return LerJson();
        }

        private static List<string> EnviarRequisicao(ConfiguracaoTestes configuracaoTestes, PrePedidoDto dto)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonContent = JsonConvert.SerializeObject(dto);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //para testar no appdev precisa alterar de DEBUG para a data 
            contentString.Headers.Add("X-API-Version", xApiVersion);
            var response = client.PostAsync(configuracaoTestes.UrlPrePedidoApiCadastrarPrePedido, contentString).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<List<string>>(responseString);
            return res;
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
