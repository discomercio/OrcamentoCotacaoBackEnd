import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { PrePedidoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrePedidoDto';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { DetalhesDtoPrepedido } from 'src/app/dto/Prepedido/DetalhesPrepedido/DetalhesDtoPrepedido';
import { NovoPrepedidoDadosService } from '../novo-prepedido-dados.service';
import { CpfCnpjUtils } from 'src/app/utils/cpfCnpjUtils';
import { Constantes } from 'src/app/dto/Constantes';
import { Router } from '@angular/router';
import { PrepedidoProdutoDtoPrepedido } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrepedidoProdutoDtoPrepedido';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';

@Component({
  selector: 'app-itens',
  templateUrl: './itens.component.html',
  styleUrls: [
    './itens.component.scss',
    '../../../estilos/tabelaresponsiva.scss'
  ]
})
export class ItensComponent extends TelaDesktopBaseComponent implements OnInit {

  //#region dados
  //dados sendo criados
  criando = true;
  prePedidoDto: PrePedidoDto;
  //#endregion

  constructor(
    private readonly location: Location,
    private readonly router: Router,
    private readonly novoPrepedidoDadosService: NovoPrepedidoDadosService,
    telaDesktopService: TelaDesktopService
  ) {
    super(telaDesktopService);
  }

  ngOnInit() {

    //afazer: temporario
    window.alert("Afazer: tela ainda em desenvolvimento");
    this.router.navigate(["/novo-prepedido"]);

    this.prePedidoDto = this.novoPrepedidoDadosService.prePedidoDto;
    //se veio diretamente para esta tela, e não tem nada no serviço, não podemos cotninuar
    //então voltamos para o começo do processo!
    if (!this.prePedidoDto) {
      this.router.navigate(["/novo-prepedido"]);
    }

    this.criando = !this.prePedidoDto.NumeroPrePedido;
  }

  //#region formatação de dados para a tela
  cpfCnpj() {
    let ret = "CPF: ";
    if (this.prePedidoDto.DadosCliente.Tipo == new Constantes().ID_PJ) {
      ret = "CNPJ: ";
    }
    //fica melhor sem nada na frente:
    ret = "";
    return ret + CpfCnpjUtils.cnpj_cpf_formata(this.prePedidoDto.DadosCliente.Cnpj_Cpf);
  }

  //#endregion


  //#region navegação
  voltar() {
    this.location.back();
  }
  continuar() {
    window.alert("Afazer: salvar o pedido");
  }
  adicionarProduto() {
    let novo = new PrepedidoProdutoDtoPrepedido();
    this.prePedidoDto.ListaProdutos.push(novo);
  }
  //#endregion

}
