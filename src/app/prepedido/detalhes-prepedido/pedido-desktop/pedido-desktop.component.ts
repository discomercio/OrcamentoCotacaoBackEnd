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
import { ClienteCadastroUtils } from 'src/app/dto/ClienteCadastroUtils/ClienteCadastroUtils';

@Component({
  selector: 'app-pedido-desktop',
  templateUrl: './pedido-desktop.component.html',
  styleUrls: ['./pedido-desktop.component.scss']
})
export class PedidoDesktopComponent implements OnInit {

  @Input() pedido: PedidoDto = null;

  constructor(private readonly router: Router,
    public readonly impressaoService: ImpressaoService,
    private readonly location: Location) { }
  ngOnInit() {
  }

  //cosntantes
  constantes = new Constantes();

  //para formatar as coisas
  dataFormatarTela = DataUtils.formatarTela;
  dataFormatarTelaHora = DataUtils.formatarTelaHora;
  moedaUtils: MoedaUtils = new MoedaUtils();
  clienteCadastroUtils = new ClienteCadastroUtils();

  //parar imprimir (qeur dizer, para ir apra a versão de impressão)
  imprimir(): void {
    //window.alert("Afazer: versão para impressão somente com o pedido");
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
    const p = this.pedido ? this.pedido.DadosCliente : null;
    if (!p)
      return "";
    return "Afazer: endereço de entrega";
  }

  //total do pedido
  totalDestePedido(): number {
    if (!this.pedido || !this.pedido.ListaProdutos)
      return 0;

    let ret = 0;
    for (let i = 0; i < this.pedido.ListaProdutos.length; i++) {
      ret += this.pedido.ListaProdutos[i].VlTotal;
    }
    return ret;
  }

  //status da entrega imediata
  entregaImediataInicio(): string {
    if (!this.pedido || !this.pedido.DetalhesNF)
      return "";
    if (this.pedido.DetalhesNF.EntregaImediata == this.constantes.COD_ETG_IMEDIATA_NAO)
      return "NÃO";
    if (this.pedido.DetalhesNF.EntregaImediata == this.constantes.COD_ETG_IMEDIATA_SIM)
      return "SIM";

    return "";
  }
  entregaImediata(): string {
    let ret = this.entregaImediataInicio();

    if (ret == "")
      return ret;

    return ret + " afazer: etg_imediata_data";
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

  clicarCliente(): void {
    if (this.pedido && this.pedido.DadosCliente && this.pedido.DadosCliente.Cnpj_Cpf)
      this.router.navigate(["cliente", this.pedido.DadosCliente.Cnpj_Cpf]);
    else
      window.alert("Erro: cliente sem CPF/CNPJ");
  }

  //controle da impressão
  imprimirOcorrenciasAlterado() {
    this.impressaoService.imprimirOcorrenciasSet(!this.impressaoService.imprimirOcorrencias());
  }
  imprimirBlocoNotasAlterado() {
    this.impressaoService.imprimirBlocoNotasSet(!this.impressaoService.imprimirBlocoNotas());
  }

  voltar() {
    this.location.back();
  }

}

