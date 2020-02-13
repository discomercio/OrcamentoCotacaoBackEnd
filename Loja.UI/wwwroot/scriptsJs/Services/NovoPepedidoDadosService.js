define(["require", "exports", "../DtosTs/DtoPedido/DtoPedido", "../DtosTs/DtoPedido/DtoDetalhesPedido", "../DtosTs/DtoPedido/DtoFormaPagtoCriacao"], function (require, exports, DtoPedido_1, DtoDetalhesPedido_1, DtoFormaPagtoCriacao_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var NovoPedidoDadosService = /** @class */ (function () {
        function NovoPedidoDadosService() {
            //esta classe mantém o PrePedidoDto sendo criado
            //gaurdamos em um serviço para manter os dados
            this.dtoPedido = null;
        }
        NovoPedidoDadosService.prototype.setar = function (dtoPedido) {
            this.dtoPedido = dtoPedido;
        };
        //somente setar dados do cliente
        NovoPedidoDadosService.prototype.setarDTosParciais = function (clienteCadastroDto, enderecoEntregaDtoClienteCadastro) {
            var p = this.dtoPedido;
            p.DadosCliente = clienteCadastroDto;
            p.EnderecoEntrega = enderecoEntregaDtoClienteCadastro;
        };
        NovoPedidoDadosService.prototype.criarNovo = function (clienteCadastroDto, enderecoEntregaDtoClienteCadastro) {
            this.dtoPedido = new DtoPedido_1.DtoPedido();
            var p = this.dtoPedido;
            //temos que criar os objetos...
            p.NumeroPrePedido = "";
            p.DataHoraPedido = "";
            p.DadosCliente = clienteCadastroDto;
            p.EnderecoEntrega = enderecoEntregaDtoClienteCadastro;
            p.ListaProdutos = new Array();
            p.TotalFamiliaParcelaRA = 0;
            p.PermiteRAStatus = 0;
            p.OpcaoPossuiRA = "";
            p.CorTotalFamiliaRA = "";
            p.PercRT = null;
            p.ValorTotalDestePedidoComRA = null;
            p.VlTotalDestePedido = null;
            p.DetalhesPrepedido = new DtoDetalhesPedido_1.DtoDetalhesPedido();
            p.FormaPagto = new Array();
            p.St_Orc_Virou_Pedido = false;
            p.NumeroPedido = "";
            p.FormaPagtoCriacao = new DtoFormaPagtoCriacao_1.DtoFormaPagtoCriacao();
        };
        NovoPedidoDadosService.prototype.totalPedido = function () {
            return this.dtoPedido.ListaProdutos.reduce(function (sum, current) { return sum + current.TotalItem; }, 0);
        };
        return NovoPedidoDadosService;
    }());
    exports.NovoPedidoDadosService = NovoPedidoDadosService;
});
//# sourceMappingURL=/scriptsJs/Services/NovoPepedidoDadosService.js.map