using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava80
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0054:Use compound assignment", Justification = "Estilo de código")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "Estilo de código")]
    class Grava80 : PassoBaseGravacao
    {
        public Grava80(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao, Execucao.Gravacao gravacao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao, gravacao)
        {
        }

        public async Task ExecutarAsync()
        {
            //Passo80: VERIFICA SE O ENDEREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE (POSSÍVEL FRAUDE)
            //	Passo80 / FluxoVerificacaoEndereco.feature
            var controle = new VerificarSeEnderecoJaFoiUsadoControle
            {
                saveChangesPendente = false,
                blnAnalisarEndereco = false
            };


            /*
			 * 
			 * loja/PedidoNovoConrima.asp 
			 * de
			 * 1) VERIFICA SE O ENDEREÇO USADO É O DO PARCEIRO
			 * até
			 * s = "UPDATE t_PEDIDO SET analise_endereco_tratar_status = 1 WHERE (pedido = '" & id_pedido & "')"
			 * */


            await VerificarSeEnderecoJaFoiUsado(Pedido.Ambiente.ComIndicador, Execucao.TabelasBanco.Indicador, Retorno.ListaErros,
                Pedido.EnderecoCadastralCliente.Endereco_logradouro, Pedido.EnderecoCadastralCliente.Endereco_numero, Pedido.EnderecoCadastralCliente.Endereco_cep,
                controle, ContextoBdGravacao, Gravacao.Tpedido_pai.Pedido, Gravacao.Tpedido_pai.Id_Cliente,
                Pedido.EnderecoCadastralCliente.Endereco_complemento, Pedido.EnderecoCadastralCliente.Endereco_bairro, Pedido.EnderecoCadastralCliente.Endereco_cidade, Pedido.EnderecoCadastralCliente.Endereco_uf,
                Pedido.Ambiente.Usuario);

            //'	ENDEREÇO DE ENTREGA (SE HOUVER)
            if (Pedido.EnderecoEntrega.OutroEndereco)
            {
                await VerificarSeEnderecoJaFoiUsado(Pedido.Ambiente.ComIndicador, Execucao.TabelasBanco.Indicador, Retorno.ListaErros,
                    Pedido.EnderecoEntrega.EndEtg_endereco, Pedido.EnderecoEntrega.EndEtg_endereco_numero, Pedido.EnderecoEntrega.EndEtg_cep,
                    controle, ContextoBdGravacao, Gravacao.Tpedido_pai.Pedido, Gravacao.Tpedido_pai.Id_Cliente,
                    Pedido.EnderecoEntrega.EndEtg_endereco_complemento, Pedido.EnderecoEntrega.EndEtg_bairro, Pedido.EnderecoEntrega.EndEtg_cidade, Pedido.EnderecoEntrega.EndEtg_uf,
                    Pedido.Ambiente.Usuario);
            }

            if (controle.blnAnalisarEndereco)
            {
                Gravacao.Tpedido_pai.Analise_Endereco_Tratar_Status = 1;
                ContextoBdGravacao.Update(Gravacao.Tpedido_pai);
                controle.saveChangesPendente = true;
            }

            if (controle.saveChangesPendente)
                await ContextoBdGravacao.SaveChangesAsync();

        }

        private class VerificarSeEnderecoJaFoiUsadoControle
        {
            public bool blnAnalisarEndereco;
            public bool saveChangesPendente;
        }

        private static async Task VerificarSeEnderecoJaFoiUsado(bool PedidoAmbienteComIndicador,
                                                                TorcamentistaEindicador? ExecucaoTabelasBancoIndicador,
                                                                List<string> ListaErros,
                                                                string? Verificar_Endereco_logradouro,
                                                                string? Verificar_Endereco_numero,
                                                                string? Verificar_Endereco_cep,
                                                                VerificarSeEnderecoJaFoiUsadoControle controle,
                                                                ContextoBdGravacao ContextoBdGravacao,
                                                                string Tpedido_pai_Pedido, string Tpedido_pai_Id_Cliente,
                                                                string? Verificar_Endereco_complemento,
                                                                string? Verificar_Endereco_bairro,
                                                                string? Verificar_Endereco_cidade,
                                                                string? Verificar_Endereco_uf,
                                                                string? Pedido_Ambiente_Usuario)
        {
            var blnAnEnderecoCadClienteOuoEndEntregaUsaEndParceiro = false;
            //'	VERIFICA SE O ENDEREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE (POSSÍVEL FRAUDE)
            //'	ENDEREÇO DO CADASTRO
            if (PedidoAmbienteComIndicador)
            {
                //'	1) VERIFICA SE O ENDEREÇO USADO É O DO PARCEIRO
                var indicador = ExecucaoTabelasBancoIndicador;
                if (indicador == null)
                {
                    ListaErros.Add("Erro: Pedido.Ambiente.ComIndicador e Execucao.TabelasBanco.Indicador == null!");
                }
                else
                {
                    //if isEnderecoIgual(EndCob_endereco, EndCob_endereco_numero, EndCob_cep, r_orcamentista_e_indicador.endereco, r_orcamentista_e_indicador.endereco_numero, r_orcamentista_e_indicador.cep) then
                    if (CompararEndereco.IsEnderecoIgual(
                            Verificar_Endereco_logradouro,
                            Verificar_Endereco_numero,
                            Verificar_Endereco_cep,
                            indicador.Endereco,
                            indicador.Endereco_Numero,
                            indicador.Cep))
                    {
                        blnAnEnderecoCadClienteOuoEndEntregaUsaEndParceiro = true;
                        controle.blnAnalisarEndereco = true;

                        int intNsuPai = await UtilsGlobais.Nsu.Fin_gera_nsu(InfraBanco.Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO, ContextoBdGravacao);
                        {
                            var rs = new InfraBanco.Modelos.TpedidoAnaliseEndereco();
                            rs.Id = intNsuPai;
                            rs.Pedido = Tpedido_pai_Pedido;
                            rs.Id_cliente = Tpedido_pai_Id_Cliente;
                            rs.Tipo_endereco = Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE;
                            rs.Endereco_logradouro = UtilsGlobais.Texto.leftStr(Verificar_Endereco_logradouro, Constantes.MAX_TAMANHO_CAMPO_ENDERECO);
                            rs.Endereco_numero = UtilsGlobais.Texto.leftStr(Verificar_Endereco_numero, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_NUMERO);
                            rs.Endereco_complemento = UtilsGlobais.Texto.leftStr(Verificar_Endereco_complemento, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_COMPLEMENTO);
                            rs.Endereco_bairro = UtilsGlobais.Texto.leftStr(Verificar_Endereco_bairro, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_BAIRRO);
                            rs.Endereco_cidade = UtilsGlobais.Texto.leftStr(Verificar_Endereco_cidade, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_CIDADE);
                            rs.Endereco_uf = Verificar_Endereco_uf;
                            rs.Endereco_cep = Verificar_Endereco_cep;
                            rs.Usuario_cadastro = Pedido_Ambiente_Usuario;
                            rs.Dt_cadastro = DateTime.Now.Date;
                            rs.Dt_hr_cadastro = DateTime.Now;
                            ContextoBdGravacao.TpedidoAnaliseEnderecos.Add(rs);
                            controle.saveChangesPendente = true;
                        }

                        int intNsu = await UtilsGlobais.Nsu.Fin_gera_nsu(InfraBanco.Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO, ContextoBdGravacao);

                        {
                            var rs = new InfraBanco.Modelos.TpedidoAnaliseEnderecoConfrontacao();
                            rs.Id = intNsu;
                            rs.Id_pedido_analise_endereco = intNsuPai;
                            rs.Pedido = "";
                            rs.Id_cliente = "";
                            rs.Tipo_endereco = Constantes.COD_PEDIDO_AN_ENDERECO__END_PARCEIRO;
                            rs.Endereco_logradouro = UtilsGlobais.Texto.leftStr(indicador.Endereco, Constantes.MAX_TAMANHO_CAMPO_ENDERECO);
                            rs.Endereco_numero = UtilsGlobais.Texto.leftStr(indicador.Endereco_Numero, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_NUMERO);
                            rs.Endereco_complemento = UtilsGlobais.Texto.leftStr(indicador.Endereco_Complemento, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_COMPLEMENTO);
                            rs.Endereco_bairro = UtilsGlobais.Texto.leftStr(indicador.Bairro, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_BAIRRO);
                            rs.Endereco_cidade = UtilsGlobais.Texto.leftStr(indicador.Cidade, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_CIDADE);
                            rs.Endereco_uf = indicador.Uf;
                            rs.Endereco_cep = indicador.Cep;
                            ContextoBdGravacao.TpedidoAnaliseConfrontacaos.Add(rs);
                            controle.saveChangesPendente = true;
                        }
                    }
                }
            }
            //'	2)VERIFICA PEDIDOS DE OUTROS CLIENTES
            if (!blnAnEnderecoCadClienteOuoEndEntregaUsaEndParceiro)
            {
                //COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE_MEMORIZADO
                var listaPedidos_cad_cliente_memorizado = from pedidoLido in ContextoBdGravacao.Tpedidos
                                                          where pedidoLido.Id_Cliente != Tpedido_pai_Id_Cliente
                                                          && pedidoLido.Endereco_cep == UtilsGlobais.Util.Cep_SoDigito(Verificar_Endereco_cep)
                                                          select new Cl_ANALISE_ENDERECO_CONFRONTACAO()
                                                          {
                                                              Pedido = pedidoLido.Pedido,
                                                              Id_cliente = pedidoLido.Id_Cliente,
                                                              Tipo_endereco = Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE_MEMORIZADO,
                                                              Endereco_logradouro = pedidoLido.Endereco_logradouro,
                                                              Endereco_numero = pedidoLido.Endereco_numero,
                                                              Endereco_complemento = pedidoLido.Endereco_complemento,
                                                              Endereco_bairro = pedidoLido.Endereco_bairro,
                                                              Endereco_cidade = pedidoLido.Endereco_cidade,
                                                              Endereco_uf = pedidoLido.Endereco_uf,
                                                              Endereco_cep = pedidoLido.Endereco_cep,
                                                              Data_hora = pedidoLido.Data_Hora
                                                          };

                //COD_PEDIDO_AN_ENDERECO__END_ENTREGA
                var listaPedidos_end_entrega = from pedidoLido in ContextoBdGravacao.Tpedidos
                                               where pedidoLido.Id_Cliente != Tpedido_pai_Id_Cliente
                                               && pedidoLido.EndEtg_Cep == UtilsGlobais.Util.Cep_SoDigito(Verificar_Endereco_cep)
                                               && pedidoLido.St_End_Entrega == 1
                                               select new Cl_ANALISE_ENDERECO_CONFRONTACAO()
                                               {
                                                   Pedido = pedidoLido.Pedido,
                                                   Id_cliente = pedidoLido.Id_Cliente,
                                                   Tipo_endereco = Constantes.COD_PEDIDO_AN_ENDERECO__END_ENTREGA,
                                                   Endereco_logradouro = pedidoLido.EndEtg_Endereco,
                                                   Endereco_numero = pedidoLido.EndEtg_Endereco_Numero,
                                                   Endereco_complemento = pedidoLido.EndEtg_Endereco_Complemento,
                                                   Endereco_bairro = pedidoLido.EndEtg_Bairro,
                                                   Endereco_cidade = pedidoLido.EndEtg_Cidade,
                                                   Endereco_uf = pedidoLido.EndEtg_UF,
                                                   Endereco_cep = pedidoLido.EndEtg_Cep,
                                                   Data_hora = pedidoLido.Data_Hora
                                               };

                var registrosLidos = await listaPedidos_cad_cliente_memorizado.ToListAsync();
                registrosLidos.AddRange(await listaPedidos_end_entrega.ToListAsync());
                registrosLidos = (from v in registrosLidos orderby v.Data_hora descending select v).Distinct().ToList();

                int intQtdeTotalPedidosAnEndereco = 0;
                var vAnEndConfrontacao = new List<Cl_ANALISE_ENDERECO_CONFRONTACAO>();

                foreach (var registroLido in registrosLidos)
                {
                    if (CompararEndereco.IsEnderecoIgual(
                        Verificar_Endereco_logradouro,
                        Verificar_Endereco_numero,
                        Verificar_Endereco_cep,
                        registroLido.Endereco_logradouro,
                        registroLido.Endereco_numero,
                        registroLido.Endereco_cep))
                    {
                        vAnEndConfrontacao.Add(registroLido);
                        intQtdeTotalPedidosAnEndereco = intQtdeTotalPedidosAnEndereco + 1;
                        if (intQtdeTotalPedidosAnEndereco >= Constantes.MAX_AN_ENDERECO_QTDE_PEDIDOS_CADASTRAMENTO)
                            break;
                    }
                }


                var blnGravouRegPai = false;
                int intNsuPai = 0;
                foreach (var vAnEndConfrontacao_i in vAnEndConfrontacao)
                {
                    if (!string.IsNullOrWhiteSpace(vAnEndConfrontacao_i.Pedido))
                    {
                        controle.blnAnalisarEndereco = true;
                        //'	JÁ GRAVOU O REGISTRO PAI?
                        if (!blnGravouRegPai)
                        {
                            blnGravouRegPai = true;
                            intNsuPai = await UtilsGlobais.Nsu.Fin_gera_nsu(InfraBanco.Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO, ContextoBdGravacao);
                            {
                                var rs = new InfraBanco.Modelos.TpedidoAnaliseEndereco();
                                rs.Id = intNsuPai;
                                rs.Pedido = Tpedido_pai_Pedido;
                                rs.Id_cliente = Tpedido_pai_Id_Cliente;
                                rs.Tipo_endereco = Constantes.COD_PEDIDO_AN_ENDERECO__CAD_CLIENTE;
                                rs.Endereco_logradouro = UtilsGlobais.Texto.leftStr(Verificar_Endereco_logradouro, Constantes.MAX_TAMANHO_CAMPO_ENDERECO);
                                rs.Endereco_numero = UtilsGlobais.Texto.leftStr(Verificar_Endereco_numero, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_NUMERO);
                                rs.Endereco_complemento = UtilsGlobais.Texto.leftStr(Verificar_Endereco_complemento, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_COMPLEMENTO);
                                rs.Endereco_bairro = UtilsGlobais.Texto.leftStr(Verificar_Endereco_bairro, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_BAIRRO);
                                rs.Endereco_cidade = UtilsGlobais.Texto.leftStr(Verificar_Endereco_cidade, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_CIDADE);
                                rs.Endereco_uf = Verificar_Endereco_uf;
                                rs.Endereco_cep = Verificar_Endereco_cep;
                                rs.Usuario_cadastro = Pedido_Ambiente_Usuario;
                                rs.Dt_cadastro = DateTime.Now.Date;
                                rs.Dt_hr_cadastro = DateTime.Now;
                                ContextoBdGravacao.TpedidoAnaliseEnderecos.Add(rs);
                                controle.saveChangesPendente = true;
                            }
                        }

                        int intNsu = await UtilsGlobais.Nsu.Fin_gera_nsu(InfraBanco.Constantes.Constantes.T_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO, ContextoBdGravacao);

                        {
                            var rs = new InfraBanco.Modelos.TpedidoAnaliseEnderecoConfrontacao();
                            rs.Id = intNsu;
                            rs.Id_pedido_analise_endereco = intNsuPai;
                            rs.Pedido = vAnEndConfrontacao_i.Pedido;
                            rs.Id_cliente = vAnEndConfrontacao_i.Id_cliente;
                            rs.Tipo_endereco = vAnEndConfrontacao_i.Tipo_endereco;
                            rs.Endereco_logradouro = UtilsGlobais.Texto.leftStr(vAnEndConfrontacao_i.Endereco_logradouro, Constantes.MAX_TAMANHO_CAMPO_ENDERECO);
                            rs.Endereco_numero = UtilsGlobais.Texto.leftStr(vAnEndConfrontacao_i.Endereco_numero, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_NUMERO);
                            rs.Endereco_complemento = UtilsGlobais.Texto.leftStr(vAnEndConfrontacao_i.Endereco_complemento, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_COMPLEMENTO);
                            rs.Endereco_bairro = UtilsGlobais.Texto.leftStr(vAnEndConfrontacao_i.Endereco_bairro, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_BAIRRO);
                            rs.Endereco_cidade = UtilsGlobais.Texto.leftStr(vAnEndConfrontacao_i.Endereco_cidade, Constantes.MAX_TAMANHO_CAMPO_ENDERECO_CIDADE);
                            rs.Endereco_uf = vAnEndConfrontacao_i.Endereco_uf;
                            rs.Endereco_cep = vAnEndConfrontacao_i.Endereco_cep;
                            ContextoBdGravacao.TpedidoAnaliseConfrontacaos.Add(rs);
                            controle.saveChangesPendente = true;
                        }
                    }
                }
            }
        }
    }
}
