using InfraBanco.Constantes;
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
            Validar_c_indicador_etc();
            Validar_NFe_xPed();
        }

        private void Validar_NFe_xPed()
        {
            /*
             * não aceitamos: | page down e seta à direita. 
             * Ou seja, só recusamos o |
             * 
                        function filtra_nome_identificador() {
                        var letra;
                        letra=String.fromCharCode(window.event.keyCode);
                        if ((letra=="|")||(window.event.keyCode==34)||(window.event.keyCode==39)) window.event.keyCode=0;
                        }
            */
            if ((Pedido.Extra.Nfe_XPed ?? "").Contains("|"))
                Retorno.ListaErros.Add("Campo Nfe_XPed não pode conter o caractere |");

        }
        private void Validar_c_indicador_etc()
        {
            //passo50 c_indicador c_perc_RT rb_RA garantia_indicador somente se for indicacao

            /*
            #loja/PedidoNovoConfirma.asp
            #if rb_indicacao = "S" then
            #	c_indicador = Trim(Request.Form("c_indicador"))
            #	c_perc_RT = Trim(Request.Form("c_perc_RT"))
            #	rb_RA = Trim(Request.Form("rb_RA"))
            #	rb_garantia_indicador = Trim(Request.Form("rb_garantia_indicador"))
            #else
            #	c_indicador = ""
            #	c_perc_RT = ""
            #	rb_RA = ""
            #	rb_garantia_indicador = COD_GARANTIA_INDICADOR_STATUS__NAO
            #	end if
            */

            //alteração em relação ao ASP: damos erro se os dados forem inconsistentes
            if (!Pedido.Ambiente.ComIndicador)
            {
                if (!string.IsNullOrWhiteSpace(Pedido.Ambiente.Indicador))
                {
                    Retorno.ListaErros.Add("O campo Indicador deve estar vazio se o campo ComIndicador for falso.");
                }
                if(Pedido.Valor.PedidoPossuiRa())
                {
                    Retorno.ListaErros.Add("O pedido não pode ter RA se o campo ComIndicador for falso.");
                }

                Pedido.DetalhesPedido.GarantiaIndicador = Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO;
            }
        }
        private void Validar_garantia_indicador()
        {
            //# alert('Preencha o campo "Garantia Indicador"');
            if (string.IsNullOrWhiteSpace(Pedido.DetalhesPedido.GarantiaIndicador))
            {
                Retorno.ListaErros.Add("Preencha o campo \"Garantia Indicador\"");
                return;
            }

            if (Pedido.DetalhesPedido.GarantiaIndicador != Constantes.COD_GARANTIA_INDICADOR_STATUS__SIM
                && Pedido.DetalhesPedido.GarantiaIndicador != Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO)
            {
                Retorno.ListaErros.Add("Preencha o campo \"Garantia Indicador\" com 0 ou 1");
            }
        }
        private void Validar_bem_uso_consumo()
        {
            //# alert('Informe se é "Bem de Uso/Consumo"');
            if (Pedido.DetalhesPedido.BemDeUso_Consumo != (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_NAO
                && Pedido.DetalhesPedido.BemDeUso_Consumo != (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_SIM)
            {
                Retorno.ListaErros.Add("Informe se é \"Bem de Uso / Consumo\" com 0 ou 1");
            }
        }
        private void Validar_instalador_instala()
        {
            //# alert('Preencha o campo "Instalador Instala"');
            if (Pedido.DetalhesPedido.InstaladorInstala != (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO
                && Pedido.DetalhesPedido.InstaladorInstala != (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_SIM
                && Pedido.DetalhesPedido.InstaladorInstala != (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO_DEFINIDO)
            {
                Retorno.ListaErros.Add("Preencha o campo \"Instalador Instala\" com 0 ou 1 ou 2");
            }
        }
        private void ValidarTamanhos()
        {
            //#s = "" + f.c_obs1.value;
            //#if (s.length > MAX_TAM_OBS1) {
            //#	alert('Conteúdo de "Observações " excede em ' + (s.length-MAX_TAM_OBS1) + ' caracteres o tamanho máximo de ' + MAX_TAM_OBS1 + '!!');
            if (Pedido.DetalhesPedido.Observacoes?.Length > Constantes.MAX_TAM_OBS1)
                Retorno.ListaErros.Add("Conteúdo de \"Observações\" excede em " +
                    (Pedido.DetalhesPedido.Observacoes.Length - Constantes.MAX_TAM_OBS1) +
                    " caracteres o tamanho máximo de " + Constantes.MAX_TAM_OBS1 + "!");


            //#s = "" + f.c_nf_texto.value;
            //#if (s.length > MAX_TAM_NF_TEXTO) {
            //#    alert('Conteúdo de "Constar na NF" excede em ' + (s.length-MAX_TAM_NF_TEXTO) + ' caracteres o tamanho máximo de ' + MAX_TAM_NF_TEXTO + '!!');
            if (!String.IsNullOrEmpty(Pedido.Extra.Nfe_Texto_Constar))
                if (Pedido.Extra.Nfe_Texto_Constar?.Length > Constantes.MAX_TAM_NF_TEXTO)
                    Retorno.ListaErros.Add("Conteúdo de \"Constar na NF\" excede em " +
                        (Pedido.Extra.Nfe_Texto_Constar?.Length - Constantes.MAX_TAM_NF_TEXTO) +
                        " caracteres o tamanho máximo de " + Constantes.MAX_TAM_NF_TEXTO + "!");

            //#s = "" + f.c_forma_pagto.value;
            //#if (s.length > MAX_TAM_FORMA_PAGTO) {
            //#	alert('Conteúdo de "Forma de Pagamento" excede em ' + (s.length-MAX_TAM_FORMA_PAGTO) + ' caracteres o tamanho máximo de ' + MAX_TAM_FORMA_PAGTO + '!!');
            if (!String.IsNullOrEmpty(Pedido.FormaPagtoCriacao.C_forma_pagto))
                if (Pedido.FormaPagtoCriacao.C_forma_pagto?.Length > Constantes.MAX_TAM_FORMA_PAGTO)
                    Retorno.ListaErros.Add("Conteúdo de \"Forma de Pagamento\" excede em " +
                        (Pedido.FormaPagtoCriacao.C_forma_pagto?.Length - Constantes.MAX_TAM_FORMA_PAGTO) +
                        " caracteres o tamanho máximo de " + Constantes.MAX_TAM_FORMA_PAGTO + "!");
        }

        private void EntregaImediata()
        {
            //valida somente EntregaImediataData e EntregaImediata
            Prepedido.Bll.ValidacoesPrepedidoBll.ValidarDetalhesPrepedido(Pedido.DetalhesPedido, Retorno.ListaErros);
        }
        private void ValidarRA()
        {
            if (string.IsNullOrEmpty(Pedido.Ambiente.Indicador) && Pedido.Valor.PedidoPossuiRa())
                Retorno.ListaErros.Add("Necessário indicador para usar RA");

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
