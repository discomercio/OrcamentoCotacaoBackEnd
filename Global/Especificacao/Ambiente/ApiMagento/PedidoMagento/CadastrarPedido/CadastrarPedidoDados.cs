using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
{
    static class CadastrarPedidoDados
    {
        public static MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto PedidoBase()
        {
            var ret = LerJson();
            ret.EnderecoEntrega = null;
            ret.OutroEndereco = false;
            return ret;
        }
        public static MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto PedidoBaseComEnderecoDeEntrega()
        {
            var ret = LerJson();
            return ret;
        }

        private static MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto LerJson()
        {
            var nomeArquivo = "Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoDados.json";
            using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(nomeArquivo);
            if (stream == null)
            {
                Assert.Equal("", nomeArquivo + $"StackTrace: '{Environment.StackTrace}'");
                throw new NullReferenceException(nomeArquivo);
            }
            using StreamReader reader = new StreamReader(stream);
            var texto = reader.ReadToEnd();
            var json = JsonConvert.DeserializeObject<MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto>(texto);
            return json;
        }

    }
}
