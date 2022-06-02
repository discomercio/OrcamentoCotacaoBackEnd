using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedido.Criacao.Execucao
{
    public class Transportadora
    {
        private Transportadora(
            string? transportadora_Id,
            bool transportadora_Selecao_Auto,
            string? transportadora_Selecao_Auto_Cep,
            byte transportadora_Selecao_Auto_Tipo_Endereco)
        {
            Transportadora_Id = transportadora_Id;
            Transportadora_Selecao_Auto = transportadora_Selecao_Auto;
            Transportadora_Selecao_Auto_Cep = transportadora_Selecao_Auto_Cep;
            Transportadora_Selecao_Auto_Tipo_Endereco = transportadora_Selecao_Auto_Tipo_Endereco;
        }

        public string? Transportadora_Id { get; private set; }
        public bool Transportadora_Selecao_Auto { get; private set; }
        public byte Transportadora_Selecao_Auto_status()
        {
            if (Transportadora_Selecao_Auto)
                return Constantes.TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_S;
            return Constantes.TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_N;
        }

        public string? Transportadora_Selecao_Auto_Cep { get; private set; }
        public byte Transportadora_Selecao_Auto_Tipo_Endereco { get; private set; }

        public static async Task<Transportadora> CriarInstancia(PedidoCriacaoDados pedido, ContextoBdProvider contextoBdProvider)
        {
            //'	OBTENÇÃO DE TRANSPORTADORA QUE ATENDA AO CEP INFORMADO, SE HOUVER
            /*
		if rb_end_entrega = "S" then
			if EndEtg_cep <> "" then
				sTranspSelAutoTransportadoraId = obtem_transportadora_pelo_cep(retorna_so_digitos(EndEtg_cep))
				if sTranspSelAutoTransportadoraId <> "" then
					sTranspSelAutoCep = retorna_so_digitos(EndEtg_cep)
					iTranspSelAutoTipoEndereco = TRANSPORTADORA_SELECAO_AUTO_TIPO_ENDERECO_ENTREGA
					iTranspSelAutoStatus = TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_S
					end if
				end if
		else
			if EndCob_cep <> "" then
				sTranspSelAutoTransportadoraId = obtem_transportadora_pelo_cep(retorna_so_digitos(EndCob_cep))
				if sTranspSelAutoTransportadoraId <> "" then
					sTranspSelAutoCep = retorna_so_digitos(EndCob_cep)
					iTranspSelAutoTipoEndereco = TRANSPORTADORA_SELECAO_AUTO_TIPO_ENDERECO_CLIENTE
					iTranspSelAutoStatus = TRANSPORTADORA_SELECAO_AUTO_STATUS_FLAG_S
					end if
				end if
			end if
*/
            string? sTranspSelAutoTransportadoraId;
            bool iTranspSelAutoStatus = false;
            string? sTranspSelAutoCep = null;
            byte iTranspSelAutoTipoEndereco = 0;

            sTranspSelAutoTransportadoraId = "";
            if (pedido.EnderecoEntrega.OutroEndereco)
            {
                if (!string.IsNullOrWhiteSpace(pedido.EnderecoEntrega.EndEtg_cep))
                {
                    sTranspSelAutoTransportadoraId = await Obtem_transportadora_pelo_cep(UtilsGlobais.Util.Cep_SoDigito(pedido.EnderecoEntrega.EndEtg_cep), contextoBdProvider);
                    if (!string.IsNullOrWhiteSpace(sTranspSelAutoTransportadoraId))
                    {
                        sTranspSelAutoCep = UtilsGlobais.Util.Cep_SoDigito(pedido.EnderecoEntrega.EndEtg_cep);
                        iTranspSelAutoTipoEndereco = Constantes.TRANSPORTADORA_SELECAO_AUTO_TIPO_ENDERECO_ENTREGA;
                        iTranspSelAutoStatus = true;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(pedido.EnderecoCadastralCliente.Endereco_cep))
                {
                    sTranspSelAutoTransportadoraId = await Obtem_transportadora_pelo_cep(UtilsGlobais.Util.Cep_SoDigito(pedido.EnderecoCadastralCliente.Endereco_cep), contextoBdProvider);
                    if (!string.IsNullOrWhiteSpace(sTranspSelAutoTransportadoraId))
                    {
                        sTranspSelAutoCep = UtilsGlobais.Util.Cep_SoDigito(pedido.EnderecoCadastralCliente.Endereco_cep);
                        iTranspSelAutoTipoEndereco = Constantes.TRANSPORTADORA_SELECAO_AUTO_TIPO_ENDERECO_CLIENTE;
                        iTranspSelAutoStatus = true;
                    }
                }
            }


            var ret = new Transportadora(
                transportadora_Id: sTranspSelAutoTransportadoraId,
                transportadora_Selecao_Auto: iTranspSelAutoStatus,
                transportadora_Selecao_Auto_Cep: sTranspSelAutoCep,
                transportadora_Selecao_Auto_Tipo_Endereco: iTranspSelAutoTipoEndereco);
            return ret;
        }

        public static async Task<string> Obtem_transportadora_pelo_cep(string cep, ContextoBdProvider contextoBdProvider)
        {
            /*
            ' _________________________________________________
            ' OBTÉM TRANSPORTADORA PELO CEP
            '
            function obtem_transportadora_pelo_cep(Byval cep)
                strCep = retorna_so_digitos(cep)
                strSql = "SELECT " & _
                            " transportadora_id" & _
                        " FROM t_TRANSPORTADORA_CEP " & _
                        " WHERE"
                strSql = strSql & _
                    " (" & _
                        " ((tipo_range = 1) AND (cep_unico = '" & strCep & "'))" & _
                        " OR" & _
                        " ((tipo_range = 2) AND ('" & strCep & "' BETWEEN cep_faixa_inicial AND cep_faixa_final))" & _
                    ") "
                obtem_transportadora_pelo_cep = Trim("" & r("transportadora_id"))
            end function
            */

            var db = contextoBdProvider.GetContextoLeitura();
            var transportadoraCep = await (from c in db.TtransportadoraCep
                                           where (c.Tipo_range == 1 && c.Cep_unico == cep) ||
                                                 (
                                                     c.Tipo_range == 2 &&
                                                      (
                                                          c.Cep_faixa_inicial.CompareTo(cep) <= 0 &&
                                                          c.Cep_faixa_final.CompareTo(cep) >= 0
                                                       )
                                                 )
                                           select c.Transportadora_id).FirstOrDefaultAsync();

            return transportadoraCep;
        }
    }
}
