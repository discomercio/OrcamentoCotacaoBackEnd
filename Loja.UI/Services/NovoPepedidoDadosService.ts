import { PedidoDto } from "../DtosTs/PedidoDto/PedidoDto";
import { EnderecoEntregaClienteCadastroDto } from "../DtosTs/ClienteDto/EnderecoEntregaClienteCadastroDto";
import { FormaPagtoCriacaoDto } from "../DtosTs/PedidoDto/FormaPagtoCriacaoDto";


export class NovoPedidoDadosService {

    //esta classe mantém o PrePedidoDto sendo criado
    //gaurdamos em um serviço para manter os dados

    public pedidoDto: PedidoDto = null;
    constructor() { }

    public setar(pedidoDto: PedidoDto) {
        this.pedidoDto = pedidoDto;
    }

    //somente setar dados do cliente
    public setarDTosParciais(clienteCadastroDto: DadosClienteCadastroDto,
        enderecoEntregaDtoClienteCadastro: EnderecoEntregaClienteCadastroDto) {
        let p = this.pedidoDto;
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
        this.pedidoDto = new PedidoDto();
        let p = this.pedidoDto;
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
        p.FormaPagtoCriacao = new FormaPagtoCriacaoDto();

    }

    


}