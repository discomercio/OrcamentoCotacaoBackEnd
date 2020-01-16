import { Injectable } from '@angular/core';
import { PrePedidoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrePedidoDto';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { DetalhesDtoPrepedido } from 'src/app/dto/Prepedido/DetalhesPrepedido/DetalhesDtoPrepedido';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';
import { FormaPagtoCriacaoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/FormaPagtoCriacaoDto';

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
    enderecoEntregaDtoClienteCadastro: EnderecoEntregaDtoClienteCadastro) {
    let p = this.prePedidoDto;
    p.DadosCliente = clienteCadastroDto;
    p.EnderecoEntrega = enderecoEntregaDtoClienteCadastro;
  }

  public criarNovo(clienteCadastroDto: DadosClienteCadastroDto,
    enderecoEntregaDtoClienteCadastro: EnderecoEntregaDtoClienteCadastro) {
    this.prePedidoDto = new PrePedidoDto();
    let p = this.prePedidoDto;
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
    p.DetalhesPrepedido = new DetalhesDtoPrepedido();
    p.FormaPagto = new Array();
    p.St_Orc_Virou_Pedido = false;
    p.NumeroPedido = "";
    p.FormaPagtoCriacao = new FormaPagtoCriacaoDto();

  }


}

