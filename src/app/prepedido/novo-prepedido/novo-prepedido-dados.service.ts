import { Injectable } from '@angular/core';
import { PrePedidoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrePedidoDto';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { DetalhesDtoPrepedido } from 'src/app/dto/Prepedido/DetalhesPrepedido/DetalhesDtoPrepedido';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';
import { FormaPagtoCriacaoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/FormaPagtoCriacaoDto';
import { MoedaUtils } from 'src/app/utils/moedaUtils';
import { EnderecoCadastralClientePrepedidoDto } from 'src/app/dto/Prepedido/EnderecoCadastralClientePrepedidoDto';

@Injectable({
  providedIn: 'root'
})

export class NovoPrepedidoDadosService {

  //esta classe mantém o PrePedidoDto sendo criado
  //gaurdamos em um serviço para manter os dados

  public prePedidoDto: PrePedidoDto = null;
  constructor() { }

  public setar(prePedidoDto: PrePedidoDto) {
    this.prePedidoDto = prePedidoDto;
  }

  //somente setar dados do cliente
  public setarDTosParciais(clienteCadastroDto: DadosClienteCadastroDto,
    enderecoEntregaDtoClienteCadastro: EnderecoEntregaDtoClienteCadastro, 
    endCadastralClientePrepedidoDto: EnderecoCadastralClientePrepedidoDto) {
    let p = this.prePedidoDto;
    p.DadosCliente = clienteCadastroDto;
    p.EnderecoEntrega = enderecoEntregaDtoClienteCadastro;
    p.EnderecoCadastroClientePrepedido = endCadastralClientePrepedidoDto;
  }

  public criarNovo(clienteCadastroDto: DadosClienteCadastroDto,
    enderecoEntregaDtoClienteCadastro: EnderecoEntregaDtoClienteCadastro, 
    endCadastralClientePrepedidoDto : EnderecoCadastralClientePrepedidoDto) {
    this.prePedidoDto = new PrePedidoDto();
    let p = this.prePedidoDto;
    //temos que criar os objetos...
    p.NumeroPrePedido = "";
    p.DataHoraPedido = "";
    p.DadosCliente = clienteCadastroDto;
    p.EnderecoCadastroClientePrepedido = endCadastralClientePrepedidoDto;
    p.EnderecoEntrega = enderecoEntregaDtoClienteCadastro;
    p.ListaProdutos = new Array();
    p.TotalFamiliaParcelaRA = 0;
    p.PermiteRAStatus = 0;
    p.OpcaoPossuiRA = "";
    p.CorTotalFamiliaRA = "";
    p.PercRT = null;
    p.ValorTotalDestePedidoComRA = null;
    p.VlTotalDestePedido = null;
    p.DetalhesPrepedido = new DetalhesDtoPrepedido();
    p.FormaPagto = new Array();
    p.St_Orc_Virou_Pedido = false;
    p.NumeroPedido = "";
    p.FormaPagtoCriacao = new FormaPagtoCriacaoDto();
  }


  public moedaUtils : MoedaUtils = new MoedaUtils();
  public totalPedido(): number {
    return this.prePedidoDto.VlTotalDestePedido = this.moedaUtils.formatarDecimal(
      this.prePedidoDto.ListaProdutos.reduce((sum, current) => sum + this.moedaUtils.formatarDecimal(current.TotalItem), 0));

  }

  public totalPedidoRA(): number {
    //afazer: calcular o total de Preco_Lista para somar apenas o total como é feito no total do pedido
    return this.prePedidoDto.ValorTotalDestePedidoComRA = this.moedaUtils.formatarDecimal(
      this.prePedidoDto.ListaProdutos.reduce((sum, current) => sum + this.moedaUtils.formatarDecimal(current.TotalItemRA), 0));
  }
}

