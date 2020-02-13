import { DtoPedido } from "../DtosTs/DtoPedido/DtoPedido";
import { DtoEnderecoEntregaClienteCadastro } from "../DtosTs/DtoCliente/DtoEnderecoEntregaClienteCadastro";
import { DtoDetalhesPedido } from "../DtosTs/DtoPedido/DtoDetalhesPedido";
import { DtoFormaPagtoCriacao } from "../DtosTs/DtoPedido/DtoFormaPagtoCriacao";

export class NovoPedidoDadosService {

    //esta classe mantém o PrePedidoDto sendo criado
    //gaurdamos em um serviço para manter os dados

    public dtoPedido: DtoPedido = null;
    constructor() { }

    public setar(dtoPedido: DtoPedido) {
        this.dtoPedido = dtoPedido;
    }

    //somente setar dados do cliente
    public setarDTosParciais(clienteCadastroDto: DtoDadosClienteCadastro,
        enderecoEntregaDtoClienteCadastro: DtoEnderecoEntregaClienteCadastro) {
        let p = this.dtoPedido;
        p.DadosCliente = clienteCadastroDto;
        p.EnderecoEntrega = enderecoEntregaDtoClienteCadastro;
    }

    //public criarNovo(clienteCadastroDto: DtoDadosClienteCadastro,
    //    enderecoEntregaDtoClienteCadastro: DtoEnderecoEntregaClienteCadastro) {
    //    this.dtoPedido = new DtoPedido();
    //    let p = this.dtoPedido;
    //    //temos que criar os objetos...
    //    p.NumeroPrePedido = "";
    //    p.DataHoraPedido = "";
    //    p.DadosCliente = clienteCadastroDto;
    //    p.EnderecoEntrega = enderecoEntregaDtoClienteCadastro;
    //    p.ListaProdutos = new Array();
    //    p.TotalFamiliaParcelaRA = 0;
    //    p.PermiteRAStatus = 0;
    //    p.OpcaoPossuiRA = "";
    //    p.CorTotalFamiliaRA = "";
    //    p.PercRT = null;
    //    p.ValorTotalDestePedidoComRA = null;
    //    p.VlTotalDestePedido = null;
    //    p.DetalhesPrepedido = new DtoDetalhesPedido();
    //    p.FormaPagto = new Array();
    //    p.St_Orc_Virou_Pedido = false;
    //    p.NumeroPedido = "";
    //    p.FormaPagtoCriacao = new DtoFormaPagtoCriacao();

    //}

    public criarNovo() {
        this.dtoPedido = new DtoPedido();
        let p = this.dtoPedido;
        //temos que criar os objetos...
        p.NumeroPrePedido = "";
        p.DataHoraPedido = "";
        p.ListaProdutos = new Array();
        p.TotalFamiliaParcelaRA = 0;
        p.PermiteRAStatus = 0;
        p.OpcaoPossuiRA = "";
        p.CorTotalFamiliaRA = "";
        p.PercRT = null;
        p.ValorTotalDestePedidoComRA = null;
        p.VlTotalDestePedido = null;
        //p.DetalhesPrepedido = new DtoDetalhesPedido();
        p.FormaPagto = new Array();
        p.St_Orc_Virou_Pedido = false;
        p.NumeroPedido = "";
        p.FormaPagtoCriacao = new DtoFormaPagtoCriacao();

    }

    


}