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

  //parar imprimir (qeur dizer, para ir apra a versão de impressão)
  imprimir(): void {
    //window.alert("Afazer: versão para impressão somente com o pedido");
    this.router.navigate(['/pedido/imprimir', this.pedido.NumeroPedido]);
  }
  voltar() {
    this.location.back();
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
    return new FormatarEndereco().formata_endereco(p.Endereco, p.Numero, p.Complemento, p.Bairro, p.Cidade, p.Uf, p.Cep);
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
s = ""
	with r_cliente
		if Trim(.tel_res) <> "" then
			s = telefone_formata(Trim(.tel_res))
			s_aux=Trim(.ddd_res)
			if s_aux<>"" then s = "(" & s_aux & ") " & s
			end if
		end with
	
	s2 = ""
	with r_cliente
		if Trim(.tel_com) <> "" then
			s2 = telefone_formata(Trim(.tel_com))
			s_aux = Trim(.ddd_com)
			if s_aux<>"" then s2 = "(" & s_aux & ") " & s2
			s_aux = Trim(.ramal_com)
			if s_aux<>"" then s2 = s2 & "  (R. " & s_aux & ")"
			end if
		end with
	with r_cliente
		if Trim(.tel_cel) <> "" then
			s3 = telefone_formata(Trim(.tel_cel))
			s_aux = Trim(.ddd_cel)
			if s_aux<>"" then s3 = "(" & s_aux & ") " & s3
			end if
		end with
	with r_cliente
		if Trim(.tel_com_2) <> "" then
			s4 = telefone_formata(Trim(.tel_com_2))
			s_aux = Trim(.ddd_com_2)
			if s_aux<>"" then s4 = "(" & s_aux & ") " & s4
			s_aux = Trim(.ramal_com_2)
			if s_aux<>"" then s4 = s4 & "  (R. " & s_aux & ")"
			end if
    end with

e são usados desta forma:

<% if r_cliente.tipo = ID_PF then %>
	<td class="MD" width="33%" align="left"><p class="Rf">TELEFONE RESIDENCIAL</p><p class="C"><%=s%>&nbsp;</p></td>
	<td class="MD" width="33%" align="left"><p class="Rf">TELEFONE COMERCIAL</p><p class="C"><%=s2%>&nbsp;</p></td>
		<td align="left"><p class="Rf">CELULAR</p><p class="C"><%=s3%>&nbsp;</p></td>

<% else %>
	<td class="MD" width="50%" align="left"><p class="Rf">TELEFONE</p><p class="C"><%=s2%>&nbsp;</p></td>
	<td width="50%" align="left"><p class="Rf">TELEFONE</p><p class="C"><%=s4%>&nbsp;</p></td>

<% end if %>


    */
  telefone1(): string {
    const p = this.pedido ? this.pedido.DadosCliente : null;
    if (!p)
      return "";
    let s = "";

    //pessoa física
    if (this.ehPf()) {
      if (!p.TelefoneResidencial)
        return "";
      if (p.TelefoneResidencial.trim() == "")
        return "";
      s = FormatarTelefone.telefone_formata(p.TelefoneResidencial);
      let s_aux = p.DddResidencial.trim();
      if (s_aux != "")
        s = "(" + s_aux + ") " + s;
      return s;
    }

    //pessoa jurídica
    let s2 = "";
    if (!p.TelComercial)
      return "";
    if (p.TelComercial.trim() == "")
      return "";

    s2 = FormatarTelefone.telefone_formata(p.TelComercial);
    let s_aux = p.DddComercial.trim();
    if (s_aux != "")
      s2 = "(" + s_aux + ") " + s2;
    s_aux = p.Ramal.trim();
    if (s_aux != "")
      s2 = s2 + "  (R. " + s_aux + ")";
    return s2;
  }
  telefone2(): string {
    const p = this.pedido ? this.pedido.DadosCliente : null;
    if (!p)
      return "";
    let s = "";

    //pessoa física
    if (this.ehPf()) {
      let s2 = "";
      if (!p.TelComercial)
        return "";
      if (p.TelComercial.trim() == "")
        return "";

      s2 = FormatarTelefone.telefone_formata(p.TelComercial);
      let s_aux = p.DddComercial.trim();
      if (s_aux != "")
        s2 = "(" + s_aux + ") " + s2;
      s_aux = p.Ramal.trim();
      if (s_aux != "")
        s2 = s2 + "  (R. " + s_aux + ")";
      return s2;
    }

    /*
    afazer: preicsamos do 
    tel_com_2
		if (p.TelComercial.tel_com_2) <> "" then
			s4 = telefone_formata(Trim(.tel_com_2))
			s_aux = Trim(.ddd_com_2)
			if s_aux<>"" then s4 = "(" & s_aux & ") " & s4
			s_aux = Trim(.ramal_com_2)
			if s_aux<>"" then s4 = s4 & "  (R. " & s_aux & ")"
			end if
    end with
*/
    return "afazer: tel_com_2";
  }
  telefoneCelular(): string {
    const p = this.pedido ? this.pedido.DadosCliente : null;
    if (!p)
      return "";

    let s2 = "";
    if (!p.Celular)
      return "";
    if (p.Celular.trim() == "")
      return "";

    s2 = FormatarTelefone.telefone_formata(p.Celular);
    let s_aux = p.DddCelular.trim();
    if (s_aux != "")
      s2 = "(" + s_aux + ") " + s2;
    return s2;
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

}

