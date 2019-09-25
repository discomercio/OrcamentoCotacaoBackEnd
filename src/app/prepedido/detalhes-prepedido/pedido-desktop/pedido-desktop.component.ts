import { Component, OnInit, Input } from '@angular/core';
import { PedidoDto } from 'src/app/dto/pedido/detalhesPedido/PedidoDto';
import { DataUtils } from 'src/app/utils/dataUtils';
import { MoedaUtils } from 'src/app/utils/moedaUtils';
import { Constantes } from 'src/app/dto/Constantes';
import { Router } from '@angular/router';
import { FormatarEndereco } from 'src/app/utils/formatarEndereco';
import { FormatarTelefone } from 'src/app/utils/formatarTelefone';
import { Location } from '@angular/common';
import { ImpressaoService } from 'src/app/utils/impressao.service';
import { ClienteCadastroUtils } from 'src/app/dto/AngularClienteCadastroUtils/ClienteCadastroUtils';
import { StringUtils } from 'src/app/utils/stringUtils';
import { PedidoProdutosDtoPedido } from 'src/app/dto/pedido/detalhesPedido/PedidoProdutosDtoPedido';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';

@Component({
  selector: 'app-pedido-desktop',
  templateUrl: './pedido-desktop.component.html',
  styleUrls: ['./pedido-desktop.component.scss']
})
export class PedidoDesktopComponent extends TelaDesktopBaseComponent implements OnInit {

  @Input() pedido: PedidoDto = null;

  constructor(private readonly router: Router,
    public readonly impressaoService: ImpressaoService,
    telaDesktopService: TelaDesktopService,
    private readonly location: Location) {
    super(telaDesktopService);
  }
  ngOnInit() {
  }

  //cosntantes
  constantes = new Constantes();
  stringUtils = StringUtils;


  //para formatar as coisas
  dataFormatarTela = DataUtils.formatarTela;
  dataFormatarTelaHora = DataUtils.formatarTelaHora;
  dataformatarTelaDataeHora = DataUtils.formatarTelaDataeHora;
  dataformatarTelaHoraSemSegundos = DataUtils.formatarTelaHoraSemSegundos;
  moedaUtils: MoedaUtils = new MoedaUtils();
  clienteCadastroUtils = new ClienteCadastroUtils();

  //parar imprimir (qeur dizer, para ir apra a versão de impressão)
  imprimir(): void {
    //versão para impressão somente com o pedido
    this.router.navigate(['/pedido/imprimir', this.pedido.NumeroPedido]);
  }

  //para dizer se é PF ou PJ
  ehPf(): boolean {
    if (this.pedido && this.pedido.DadosCliente && this.pedido.DadosCliente.Tipo)
      return this.pedido.DadosCliente.Tipo == this.constantes.ID_PF;
    //sem dados! qualqer opção serve...  
    return true;
  }

  formata_endereco(): string {
    const p = this.pedido ? this.pedido.DadosCliente : null;
    if (!p)
      return "Sem endereço";
    return this.clienteCadastroUtils.formata_endereco(p);
  }

  formata_endereco_entrega(): string {
    const p = this.pedido ? this.pedido.EnderecoEntrega : null;
    if (!p) {
      return "";
    }

    return new FormatarEndereco().formata_endereco(p.EndEtg_endereco, p.EndEtg_endereco_numero, p.EndEtg_endereco_complemento,
      p.EndEtg_bairro, p.EndEtg_cidade, p.EndEtg_uf, p.EndEtg_cep);
  }

  //status da entrega imediata
  entregaImediata(): string {
    if (!this.pedido || !this.pedido.DetalhesNF)
      return "";
    return this.pedido.DetalhesNF.EntregaImediata;
  }

  /*
  //para pegar o telefone
  //copiamos a lógica do ASP
  */
  telefone1(): string {
    const p = this.pedido ? this.pedido.DadosCliente : null;
    if (!p)
      return "";
    return this.clienteCadastroUtils.telefone1(p);
  }
  telefone2(): string {
    const p = this.pedido ? this.pedido.DadosCliente : null;
    if (!p)
      return "";
    return this.clienteCadastroUtils.telefone2(p);
  }
  telefoneCelular(): string {
    const p = this.pedido ? this.pedido.DadosCliente : null;
    if (!p)
      return "";
    return this.clienteCadastroUtils.telefoneCelular(p);
  }

  public corDaLinhaPedido(linha: PedidoProdutosDtoPedido): string {
    return linha.CorFaltante;
  }

  //#region  impressão
  //controle da impressão
  imprimirOcorrenciasAlterado() {
    this.impressaoService.imprimirOcorrenciasSet(!this.impressaoService.imprimirOcorrencias());
  }
  imprimirBlocoNotasAlterado() {
    this.impressaoService.imprimirBlocoNotasSet(!this.impressaoService.imprimirBlocoNotas());
  }
  imprimirNotasDevAlterado() {
    this.impressaoService.imprimirNotasDevSet(!this.impressaoService.imprimirNotasDev());
  }

  voltar() {
    this.location.back();
  }
  //#endregion

}

