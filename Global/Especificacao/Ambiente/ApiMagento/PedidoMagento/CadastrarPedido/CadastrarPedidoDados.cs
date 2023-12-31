﻿using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
{
    public static class CadastrarPedidoDados
    {
        public static MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto PedidoBase()
        {
            var ret = PedidoBaseComEnderecoDeEntrega();
            /*
            "Obrigatório informar um endereço de entrega na API Magento para cliente PF."
            Autlamente, a API Magento somente aceita pedidos PF
            então o default é retornar COM endereço de entrega
            */
            return ret;
        }

        //temos que gear um novo Pedido_bs_x_ac em cada pedido
        private static volatile int Pedido_magento = 123456789;
        private static readonly object _lockObject_Pedido_magento = new object();

        public static MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto PedidoBaseComEnderecoDeEntrega()
        {
            var ret = Testes.Utils.LerJson.LerArquivoEmbutido<MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto>(
                "Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoDados.json");
            lock (_lockObject_Pedido_magento)
            {
                ret.InfCriacaoPedido.Pedido_magento = Pedido_magento.ToString("d");
                Pedido_magento++;
            }
            return ret;
        }

        public static MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto PedidoBaseClientePF()
        {
            var ret = PedidoBaseComEnderecoDeEntrega();
            return ret;
        }
        public static MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto PedidoBaseClientePJ()
        {
            var ret = PedidoBaseComEnderecoDeEntrega();
            ret.EnderecoCadastralCliente.Endereco_tipo_pessoa = "PJ";
            ret.EnderecoCadastralCliente.Endereco_cnpj_cpf = "76297703000195";
            return ret;
        }
        public static MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto PedidoBaseClientePjComEnderecoDeEntrega()
        {
            return PedidoBaseClientePJ();
        }


        public static MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto PedidoBase_para_banco_ARCLUBE_DIS20201204()
        {
            var ret = Testes.Utils.LerJson.LerArquivoEmbutido<MagentoBusiness.MagentoDto.PedidoMagentoDto.PedidoMagentoDto>(
                "Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoDados_para_banco_ARCLUBE_DIS20201204.json");
            return ret;
        }

    }
}
