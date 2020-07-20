import { Component, OnInit, Input, ChangeDetectorRef } from '@angular/core';
import { PedidoDto } from 'src/app/dto/pedido/detalhesPedido/PedidoDto2';
import { DataUtils } from 'src/app/utils/dataUtils';
import { MoedaUtils } from 'src/app/utils/moedaUtils';
import { Constantes } from 'src/app/dto/Constantes';
import { Router, ActivatedRoute } from '@angular/router';
import { FormatarEndereco } from 'src/app/utils/formatarEndereco';
import { FormatarTelefone } from 'src/app/utils/formatarTelefone';
import { Location } from '@angular/common';
import { ImpressaoService } from 'src/app/utils/impressao.service';
import { StringUtils } from 'src/app/utils/stringUtils';
import { PedidoProdutosDtoPedido } from 'src/app/dto/pedido/detalhesPedido/PedidoProdutosDtoPedido';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { PedidoBuscarService } from 'src/app/servicos/pedido/pedido-buscar.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { MatDialog } from '@angular/material';
import { AutenticacaoService } from 'src/app/servicos/autenticacao/autenticacao.service';
import { HttpParams } from '@angular/common/http';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { ClienteCadastroUtils } from 'src/app/utils/ClienteCadastroUtils';

@Component({
  selector: 'app-pedido-desktop',
  templateUrl: './pedido-desktop.component.html',
  styleUrls: ['./pedido-desktop.component.scss']
})
export class PedidoDesktopComponent extends TelaDesktopBaseComponent implements OnInit {

  @Input() pedido: PedidoDto = null;

  constructor(private readonly activatedRoute: ActivatedRoute,
    public readonly pedidoBuscarService: PedidoBuscarService,
    private readonly router: Router,
    public readonly impressaoService: ImpressaoService,
    telaDesktopService: TelaDesktopService,
    private readonly location: Location,
    public readonly dialog: MatDialog,
    private readonly autenticacaoService: AutenticacaoService) {
    super(telaDesktopService);
  }
  ngOnInit() {
    this.router.routeReuseStrategy.shouldReuseRoute = function () {
      return false;
    };      
    
    this.montarEnderecoEntrega(this.pedido.EnderecoEntrega);
  }

  formatarEndereco: FormatarEndereco = new FormatarEndereco();

  //cosntantes
  constantes = new Constantes();
  stringUtils = StringUtils;

  //para formatar as coisas
  dataFormatarTela = DataUtils.formatarTela;
  dataFormatarTelaHora = DataUtils.formatarTelaHora;
  dataformatarTelaDataeHora = DataUtils.formatarTelaDataeHora;
  dataformatarTelaHoraSemSegundos = DataUtils.formatarTelaHoraSemSegundos;
  dataformatarTelaHoraComSegundos = DataUtils.formatarTelaHoraComSegundos;
  moedaUtils: MoedaUtils = new MoedaUtils();
  clienteCadastroUtils = new ClienteCadastroUtils();

  //parar imprimir (quer dizer, para ir para a versão de impressão)
  imprimir(): void {
    //versão para impressão somente com o pedido

    //deixar a rota comentada, pois pode ser que no futuro seja utilizado
    //this.router.navigate(['/pedido/imprimir', this.pedido.NumeroPedido]);

    this.impressaoService.forcarImpressao = true;
    setTimeout(() => {
      window.print();
      this.impressaoService.forcarImpressao = false;
    }
      , 1);

    //       return false;

  }

  public enderecoEntregaFormatado: string;
  public qtdeLinhaEndereco: number;
  montarEnderecoEntrega(enderecoEntregaDto: EnderecoEntregaDtoClienteCadastro) {
    if (enderecoEntregaDto.OutroEndereco) {
      let retorno: string = "";
      let sEndereco: string;
      let split: string[];
      //vamos formatar conforme é feito no asp
      sEndereco = this.formatarEndereco.formata_endereco(enderecoEntregaDto.EndEtg_endereco,
        enderecoEntregaDto.EndEtg_endereco_numero, enderecoEntregaDto.EndEtg_endereco_complemento,
        enderecoEntregaDto.EndEtg_bairro, enderecoEntregaDto.EndEtg_cidade, enderecoEntregaDto.EndEtg_uf,
        enderecoEntregaDto.EndEtg_cep);

      //vamos verificar se esta ativo a memorização de endereço completa
      //se a memorização não estiver ativa ou o registro foi criado no formato antigo, paramos por aqui

      if (enderecoEntregaDto.St_memorizacao_completa_enderecos == 0) {
        this.enderecoEntregaFormatado = sEndereco + "\n" + enderecoEntregaDto.EndEtg_descricao_justificativa;
        return;
      }
      else {
        if (this.pedido.DadosCliente.Tipo == this.constantes.ID_PF) {
          this.enderecoEntregaFormatado = sEndereco + "\n" + enderecoEntregaDto.EndEtg_descricao_justificativa;
          return;
        }
      }
      
      //memorização ativa, colocamos os campos adicionais
      if (enderecoEntregaDto.EndEtg_tipo_pessoa == this.constantes.ID_PF) {
        this.enderecoEntregaFormatado = this.formatarEndereco.montarEnderecoEntregaPF(this.pedido.EnderecoEntrega, sEndereco);

        split = this.enderecoEntregaFormatado.split('\n');
        this.qtdeLinhaEndereco = split.length;
        return;
      }
      //se chegar aqui é PJ
      this.enderecoEntregaFormatado = this.formatarEndereco.montarEnderecoEntregaPJ(this.pedido.EnderecoEntrega, sEndereco);
      split = this.enderecoEntregaFormatado.split('\n');
      this.qtdeLinhaEndereco = split.length;
    }
  }

  //para dizer se é PF ou PJ
  ehPf(): boolean {
    if (this.pedido && this.pedido.DadosCliente && this.pedido.DadosCliente.Tipo)
      return this.pedido.DadosCliente.Tipo == this.constantes.ID_PF;
    //sem dados! qualquer opção serve...  
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

