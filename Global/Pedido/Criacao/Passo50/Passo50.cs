using InfraBanco.Modelos;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo50
{
    class Passo50 : PassoBase
    {
        public Passo50(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
            : base(pedido, retorno, pedidoCriacao)

        {
        }

        public void Executar()
        {
            EntregaImediata();
            ValidarRA();
            ValidarTamanhos();
            Validar_bem_uso_consumo();
            Validar_instalador_instala();
            Validar_garantia_indicador();

            //todo: passo50 tamanhos
            //c_indicador c_perc_RT rb_RA garantia_indicador somente se for indicacao
        }

        private void Validar_garantia_indicador()
        {
            //todo: passo50 tamanhos
            //# alert('Preencha o campo "Garantia Indicador"');
        }
        private void Validar_bem_uso_consumo()
        {
            //todo: passo50 tamanhos
            //#if (!blnFlag) {
            //# alert('Informe se é "Bem de Uso/Consumo"');
        }
        private void Validar_instalador_instala()
        {
            //todo: passo50 tamanhos
            //# alert('Preencha o campo "Instalador Instala"');
        }
        private void ValidarTamanhos()
        {
            //todo: passo50 tamanhos
            //#s = "" + f.c_obs1.value;
            //#if (s.length > MAX_TAM_OBS1) {
            //#	alert('Conteúdo de "Observações " excede em ' + (s.length-MAX_TAM_OBS1) + ' caracteres o tamanho máximo de ' + MAX_TAM_OBS1 + '!!');

            //todo: passo50 tamanhos
            //#s = "" + f.c_nf_texto.value;
            //#if (s.length > MAX_TAM_NF_TEXTO) {
            //#    alert('Conteúdo de "Constar na NF" excede em ' + (s.length-MAX_TAM_NF_TEXTO) + ' caracteres o tamanho máximo de ' + MAX_TAM_NF_TEXTO + '!!');


            //todo: passo50 tamanhos
            //#s = "" + f.c_forma_pagto.value;
            //#if (s.length > MAX_TAM_FORMA_PAGTO) {
            //#	alert('Conteúdo de "Forma de Pagamento" excede em ' + (s.length-MAX_TAM_FORMA_PAGTO) + ' caracteres o tamanho máximo de ' + MAX_TAM_FORMA_PAGTO + '!!');
        }

        private void EntregaImediata()
        {
            //valida somente EntregaImediataData e EntregaImediata
            Prepedido.ValidacoesPrepedidoBll.ValidarDetalhesPrepedido(Pedido.DetalhesPedido, Retorno.ListaErros);
        }
        private void ValidarRA()
        {
            if (string.IsNullOrEmpty(Pedido.Ambiente.Indicador) && Pedido.Valor.PedidoPossuiRa())
                Retorno.ListaErros.Add("Necessário indicador para usar RA");

            if (Pedido.Valor.PedidoPossuiRa() && !Pedido.Valor.PermiteRAStatus)
                Retorno.ListaErros.Add("Pedido está usando RA mas está inconsistente com PermiteRAStatus.");

            //o resto só validamos se tiver RA
            if (!Pedido.Valor.PedidoPossuiRa())
                return;


            //repetido, né? melhor...
            if (String.IsNullOrEmpty(Pedido.Ambiente.Indicador))
            {
                Retorno.ListaErros.Add("Necessário indicador para usar RA");
                return;
            }

            if (Criacao.Execucao.TabelasBanco.Indicador == null)
            {
                Retorno.ListaErros.Add($"Falha ao recuperar os dados do indicador! Indicador: {Pedido.Ambiente.Indicador}");
                return;
            }
            bool Indicador_Permite_RA_Status;
            Indicador_Permite_RA_Status = Criacao.Execucao.TabelasBanco.Indicador.Permite_RA_Status == 1;


            if (Pedido.Valor.PedidoPossuiRa() && !Indicador_Permite_RA_Status)
                Retorno.ListaErros.Add("Indicador não tem permissão para usar RA");
        }
    }
}
