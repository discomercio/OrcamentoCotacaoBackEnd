using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo05
{
    class Passo05 : PassoBase
    {
        public Passo05(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
            : base(pedido, retorno, pedidoCriacao)

        {
        }

        public void Executar()
        {
            //05 - Passo05: ajustando dados (garantindo cpf/cnpj e telefones somente com dígitos, etc)
            var pedido = Pedido;
            pedido.EnderecoCadastralCliente.Endereco_cnpj_cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(pedido.EnderecoCadastralCliente.Endereco_cnpj_cpf);
            pedido.EnderecoCadastralCliente.Endereco_cep = UtilsGlobais.Util.Cep_SoDigito(pedido.EnderecoCadastralCliente.Endereco_cep);
            pedido.EnderecoCadastralCliente.Endereco_tel_res = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoCadastralCliente.Endereco_tel_res);
            pedido.EnderecoCadastralCliente.Endereco_tel_cel = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoCadastralCliente.Endereco_tel_cel);
            pedido.EnderecoCadastralCliente.Endereco_tel_com = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoCadastralCliente.Endereco_tel_com);
            pedido.EnderecoCadastralCliente.Endereco_tel_com_2 = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoCadastralCliente.Endereco_tel_com_2);
            if (pedido.EnderecoEntrega.OutroEndereco)
            {
                pedido.EnderecoEntrega.EndEtg_cnpj_cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(pedido.EnderecoEntrega.EndEtg_cnpj_cpf);
                pedido.EnderecoEntrega.EndEtg_cep = UtilsGlobais.Util.Cep_SoDigito(pedido.EnderecoEntrega.EndEtg_cep);
                pedido.EnderecoEntrega.EndEtg_tel_res = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoEntrega.EndEtg_tel_res);
                pedido.EnderecoEntrega.EndEtg_tel_cel = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoEntrega.EndEtg_tel_cel);
                pedido.EnderecoEntrega.EndEtg_tel_com = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoEntrega.EndEtg_tel_com);
                pedido.EnderecoEntrega.EndEtg_tel_com_2 = UtilsGlobais.Util.Telefone_SoDigito(pedido.EnderecoEntrega.EndEtg_tel_com_2);
            }
        }
    }
}
