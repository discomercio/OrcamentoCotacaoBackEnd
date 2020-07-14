import { Component, OnInit, Input } from '@angular/core';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PrepedidoBuscarService } from 'src/app/servicos/prepedido/prepedido-buscar.service';
import { DetalhesPrepedidoComponent } from '../detalhes-prepedido.component';
import { MoedaUtils } from 'src/app/utils/moedaUtils';
import { Constantes } from 'src/app/dto/Constantes';
import { StringUtils } from 'src/app/utils/stringUtils';
import { ImpressaoService } from 'src/app/utils/impressao.service';
import { FormatarEndereco } from 'src/app/utils/formatarEndereco';
import { ClienteCadastroUtils } from 'src/app/utils/ClienteCadastroUtils';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';

@Component({
  selector: 'app-prepedido-desktop',
  templateUrl: './prepedido-desktop.component.html',
  styleUrls: ['./prepedido-desktop.component.scss']
})
export class PrepedidoDesktopComponent extends TelaDesktopBaseComponent implements OnInit {
  formatarEndereco: FormatarEndereco = new FormatarEndereco();

  constructor(
    telaDesktopService: TelaDesktopService,
    private readonly activatedRoute: ActivatedRoute,
    private readonly router: Router,
    public readonly prepedidoBuscarService: PrepedidoBuscarService,
    public readonly detalhesPrepedido: DetalhesPrepedidoComponent,
    public readonly impressaoService: ImpressaoService
  ) { super(telaDesktopService) }

  moedaUtils = new MoedaUtils();
  public prepedidoDto = this.detalhesPrepedido.prepedido;
  clienteCadastroUtils = new ClienteCadastroUtils();
  constantes = new Constantes();
  stringUtils = StringUtils;

  ngOnInit() {
    this.montarEnderecoEntrega(this.prepedidoDto.EnderecoEntrega);
    console.log(this.enderecoEntregaFormatado);
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
        if (this.prepedidoDto.DadosCliente.Tipo == this.constantes.ID_PF) {
          this.enderecoEntregaFormatado = sEndereco + "\n" + enderecoEntregaDto.EndEtg_descricao_justificativa;
          return;
        }
      }
      
      //memorização ativa, colocamos os campos adicionais
      if (enderecoEntregaDto.EndEtg_tipo_pessoa == this.constantes.ID_PF) {
        this.enderecoEntregaFormatado = this.formatarEndereco.montarEnderecoEntregaPF(this.prepedidoDto.EnderecoEntrega, sEndereco);

        split = this.enderecoEntregaFormatado.split('\n');
        this.qtdeLinhaEndereco = split.length;
        return;
      }
      //se chegar aqui é PJ
      this.enderecoEntregaFormatado = this.formatarEndereco.montarEnderecoEntregaPJ(this.prepedidoDto.EnderecoEntrega, sEndereco);
      split = this.enderecoEntregaFormatado.split('\n');
      this.qtdeLinhaEndereco = split.length;
    }


  }

  //para dizer se é PF ou PJ
  ehPf(): boolean {
    if (this.prepedidoDto && this.prepedidoDto.DadosCliente && this.prepedidoDto.DadosCliente.Tipo)
      return this.prepedidoDto.DadosCliente.Tipo == this.constantes.ID_PF;
    //sem dados! qualquer opção serve...  
    return true;
  }

  imprimir(): void {
    //versão para impressão somente com o pedido
    // this.router.navigate(['/prepedido/imprimir', this.prepedidoDto.NumeroPrePedido]);

    this.impressaoService.forcarImpressao = true;
    setTimeout(() => {
      window.print();
      this.impressaoService.forcarImpressao = false;
    }
      , 1);
  }

  formata_endereco(): string {
    const p = this.prepedidoDto ? this.prepedidoDto.DadosCliente : null;
    if (!p)
      return "Sem endereço";
    return this.clienteCadastroUtils.formata_endereco(p);
  }

  telefone1(): string {
    const p = this.prepedidoDto ? this.prepedidoDto.DadosCliente : null;
    if (!p)
      return "";
    return this.clienteCadastroUtils.telefone1(p);
  }
  telefone2(): string {
    const p = this.prepedidoDto ? this.prepedidoDto.DadosCliente : null;
    if (!p)
      return "";
    return this.clienteCadastroUtils.telefone2(p);
  }
  telefoneCelular(): string {
    const p = this.prepedidoDto ? this.prepedidoDto.DadosCliente : null;
    if (!p)
      return "";
    return this.clienteCadastroUtils.telefoneCelular(p);
  }

  //status da entrega imediata
  entregaImediata(): string {
    if (!this.prepedidoDto || !this.prepedidoDto.DetalhesPrepedido)
      return "";
    return this.prepedidoDto.DetalhesPrepedido.EntregaImediata;
  }

  editar() {
    this.router.navigate(['/novo-prepedido/confirmar-cliente',
      this.stringUtils.retorna_so_digitos(this.prepedidoDto.DadosCliente.Cnpj_Cpf)]);
  }

  verificaValor() {
    if (this.prepedidoDto.TotalFamiliaParcelaRA >= 0)
      return true
    else
      return false;
  }
}
