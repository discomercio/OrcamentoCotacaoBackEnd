import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { PrePedidoDto } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrePedidoDto';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { DetalhesDtoPrepedido } from 'src/app/dto/Prepedido/DetalhesPrepedido/DetalhesDtoPrepedido';
import { NovoPrepedidoDadosService } from '../novo-prepedido-dados.service';
import { CpfCnpjUtils } from 'src/app/utils/cpfCnpjUtils';
import { Constantes } from 'src/app/dto/Constantes';
import { Router, ActivatedRoute } from '@angular/router';
import { PrepedidoProdutoDtoPrepedido } from 'src/app/dto/Prepedido/DetalhesPrepedido/PrepedidoProdutoDtoPrepedido';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { PrepedidoBuscarService } from 'src/app/servicos/prepedido/prepedido-buscar.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';

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
    private readonly activatedRoute: ActivatedRoute,
    private readonly location: Location,
    private readonly router: Router,
    private readonly novoPrepedidoDadosService: NovoPrepedidoDadosService,
    public readonly prepedidoBuscarService: PrepedidoBuscarService,
    public readonly alertaService: AlertaService,
    telaDesktopService: TelaDesktopService
  ) {
    super(telaDesktopService);
  }

  ngOnInit() {

    //se tem um parâmetro no link, colocamos ele no serviço
    let numeroPrepedido = this.activatedRoute.snapshot.params.numeroPrepedido;
    if (!!numeroPrepedido) {
      //se for igual ao que está no serviço, podemos usar o que está no serviço
      this.prePedidoDto = this.novoPrepedidoDadosService.prePedidoDto;
      if (!this.prePedidoDto || this.prePedidoDto.NumeroPrePedido.toString() !== numeroPrepedido.toString()) {
        //ou não tem nada no serviço ou está diferente
        this.prepedidoBuscarService.buscar(numeroPrepedido).subscribe({
          next: (r) => {
            if (r == null) {
              this.alertaService.mostrarErroInternet();
              this.router.navigate(["/novo-prepedido"]);
              return;
            }

            //detalhes do prepedido
            this.prePedidoDto = r;
            this.novoPrepedidoDadosService.setar(r);
            this.criando = !this.prePedidoDto.NumeroPrePedido;
          },
          error: (r) => this.alertaService.mostrarErroInternet()
        });
        return;
      }
    }

    this.prePedidoDto = this.novoPrepedidoDadosService.prePedidoDto;
    //se veio diretamente para esta tela, e não tem nada no serviço, não podemos continuar
    //então voltamos para o começo do processo!
    if (!this.prePedidoDto) {
      this.router.navigate(["/novo-prepedido"]);
      return;
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

  permite_RA_Status = false;
  increverPermite_RA_Status() {
    this.prepedidoBuscarService.Obter_Permite_RA_Status().subscribe({
      next: (r) => {
        if (r != 0) {
          this.permite_RA_Status = true;
        }
      },
      error: (r) => this.alertaService.mostrarErroInternet()
    });
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

  removerLinha(i: PrepedidoProdutoDtoPrepedido) {
    this.prePedidoDto.ListaProdutos = this.prePedidoDto.ListaProdutos.filter(function (value, index, arr) {
      return value !== i;
    });
  }
  //#endregion

}
