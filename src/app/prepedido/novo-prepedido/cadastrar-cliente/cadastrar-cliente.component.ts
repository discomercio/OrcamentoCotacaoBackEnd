import { Component, OnInit, Input } from '@angular/core';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { ActivatedRoute, Router } from '@angular/router';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';
import { StringUtils } from 'src/app/utils/stringUtils';
import { Location, registerLocaleData } from '@angular/common';
import { ClienteCadastroUtils } from 'src/app/dto/AngularClienteCadastroUtils/ClienteCadastroUtils';
import { ValidacoesUtils } from 'src/app/utils/validacoesUtils';
import { CpfCnpjUtils } from 'src/app/utils/cpfCnpjUtils';
import { RefBancariaDtoCliente } from 'src/app/dto/ClienteCadastro/Referencias/RefBancariaDtoCliente';
import { RefComercialDtoCliente } from 'src/app/dto/ClienteCadastro/Referencias/RefComercialDtoCliente';
import { FormatarTelefone } from 'src/app/utils/formatarTelefone';
import { FormatarEndereco } from 'src/app/utils/formatarEndereco';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { BuscarClienteService } from 'src/app/servicos/cliente/buscar-cliente.service';
import { Constantes } from 'src/app/dto/Constantes';

@Component({
  selector: 'app-cadastrar-cliente',
  templateUrl: './cadastrar-cliente.component.html',
  styleUrls: ['./cadastrar-cliente.component.scss']
})
export class CadastrarClienteComponent extends TelaDesktopBaseComponent implements OnInit {

  //o dado sendo editado
  dadosClienteCadastroDto = new DadosClienteCadastroDto();
  clienteCadastroDto = new ClienteCadastroDto();

  //cosntrutor
  constructor(private readonly activatedRoute: ActivatedRoute,
    private readonly buscarClienteService: BuscarClienteService,
    private readonly router: Router,
    private readonly location: Location,
    private readonly alertaService: AlertaService,
    telaDesktopService: TelaDesktopService) {
    super(telaDesktopService);
  }


  //carregamos os dados que passaram pelo Router
  //ou somente o número do CPF/CNPJ
  ngOnInit() {
    //lemos o único dado que é fixo
    const cpfCnpj = this.activatedRoute.snapshot.params.cpfCnpj;
    this.dadosClienteCadastroDto.Cnpj_Cpf = cpfCnpj;
    this.clienteCadastroDto.DadosCliente = this.dadosClienteCadastroDto;

    //inicializar como vazio
    this.dadosClienteCadastroDto.Nome = "";
    this.dadosClienteCadastroDto.Contato = "";
  }


  ehPf() {
    if (this.dadosClienteCadastroDto.Cnpj_Cpf && StringUtils.retorna_so_digitos(this.dadosClienteCadastroDto.Cnpj_Cpf).length == 11) {
      return true;
    }
    return false;
  }
  voltar() {
    this.location.back();
  }

  continuar() {
    //validações
    let validacoes: string[] = new Array();

    let constantes = new Constantes();
    if (this.ehPf()) {
      this.dadosClienteCadastroDto.Tipo = constantes.ID_PF;
    }
    else {
      this.dadosClienteCadastroDto.Tipo = constantes.ID_PJ;
    }

    this.converterTelefones();

    /*
    campos obrigatórios para PF:
    CPF
    sexo
    produtor rural
    ENDEREÇO
    nro
    BAIRRO
    CIDADE
    UF
    CEP
    algum telefone

    campos obrigatórios para PJ:
    CNPJ
    CONTRIBUINTE ICMS
    RAZÃO SOCIAL
    TELEFONE
    NOME DA PESSOA PARA CONTATO NA EMPRESA
    ENDEREÇO
    nro
    BAIRRO
    CIDADE
    UF
    CEP

    */

    //um MONTE de validações....
    validacoes = validacoes.concat(this.validarGeral());

    //validações específicas para PF e PJ
    if (this.ehPf()) {
      validacoes = validacoes.concat(this.validarGeralPf());
    }
    else {
      validacoes = validacoes.concat(this.validarGeralPj());
    }

    //endereço
    validacoes = validacoes.concat(this.validarEndereco());

    //inscricao estadual
    let mensagem = new ClienteCadastroUtils().validarInscricaoestadualIcms(this.dadosClienteCadastroDto);
    if (mensagem && mensagem.trim() !== "") {
      validacoes.push(mensagem);
    }


    //validar telefone
    validacoes = validacoes.concat(this.validarTelefones());

    //validar referências bancárias
    //não exigimos um número de referências, mas as que foram criadas devem estar preenchidas
    for (let i = 0; i < this.clienteCadastroDto.RefBancaria.length; i++) {
      let este = this.clienteCadastroDto.RefBancaria[i];
      validacoes = validacoes.concat(this.validarRefBancaria(este));
    }

    //validar referências comerciais
    //não exigimos um número de referências, mas as que foram criadas devem estar preenchidas
    for (let i = 0; i < this.clienteCadastroDto.RefComercial.length; i++) {
      let este = this.clienteCadastroDto.RefComercial[i];
      validacoes = validacoes.concat(this.validarRefComerial(este));
    }

    //mostrar as mensagens
    if (validacoes.length > 0) {
      this.desconverterTelefones();
      this.alertaService.mostrarMensagem("Campos inválidos. Preencha os campos marcados como obrigatórios. \nLista de erros: \n" + validacoes.join("\n"));
      return;
    }

    //salvar e ir para a tela de confirmação de cliente
    this.carregando = true;
    this.buscarClienteService.cadastrarCliente(this.clienteCadastroDto).toPromise()
      .then((r) => {
        this.carregando = false;
        if (r === null) {
          this.desconverterTelefones();
          this.alertaService.mostrarErroInternet();
          return;
        }
        //se tem algum erro, mostra os erros
        if (r.length > 0) {
          this.desconverterTelefones();
          this.alertaService.mostrarMensagem("Erros ao salvar. \nLista de erros: \n" + r.join("\n"));
          return;
        }
        //agora podemos continuar
        this.router.navigate(['/novo-prepedido/confirmar-cliente', StringUtils.retorna_so_digitos(this.clienteCadastroDto.DadosCliente.Cnpj_Cpf)], { state: r })
      }).catch((r) => {
        //deu erro na busca
        //ou não achou nada...
        this.desconverterTelefones();
        this.carregando = false;
        this.alertaService.mostrarErroInternet();
      });
  }
  carregando = false;

  converterTelefones() {
    /*
converter telefone do formato da edição (separar os DDDs)
TelefoneResidencial
Celular
TelComercial
TelComercial2
*/

    {
      let s = FormatarTelefone.SepararTelefone(this.dadosClienteCadastroDto.TelefoneResidencial);
      this.dadosClienteCadastroDto.TelefoneResidencial = s.Telefone;
      this.dadosClienteCadastroDto.DddResidencial = s.Ddd;

      s = FormatarTelefone.SepararTelefone(this.dadosClienteCadastroDto.Celular);
      this.dadosClienteCadastroDto.Celular = s.Telefone;
      this.dadosClienteCadastroDto.DddCelular = s.Ddd;

      s = FormatarTelefone.SepararTelefone(this.dadosClienteCadastroDto.TelComercial);
      this.dadosClienteCadastroDto.TelComercial = s.Telefone;
      this.dadosClienteCadastroDto.DddComercial = s.Ddd;

      s = FormatarTelefone.SepararTelefone(this.dadosClienteCadastroDto.TelComercial2);
      this.dadosClienteCadastroDto.TelComercial2 = s.Telefone;
      this.dadosClienteCadastroDto.DddComercial2 = s.Ddd;
    }

    //converter referências bancárias
    for (let i = 0; i < this.clienteCadastroDto.RefBancaria.length; i++) {
      let este = this.clienteCadastroDto.RefBancaria[i];
      let s = FormatarTelefone.SepararTelefone(este.Telefone);
      este.Telefone = s.Telefone;
      este.Ddd = s.Ddd;
    }

    //converter referências comerciais
    for (let i = 0; i < this.clienteCadastroDto.RefComercial.length; i++) {
      let este = this.clienteCadastroDto.RefComercial[i];
      let s = FormatarTelefone.SepararTelefone(este.Telefone);
      este.Telefone = s.Telefone;
      este.Ddd = s.Ddd;
    }


  }
  desconverterTelefones() {
    {
      this.dadosClienteCadastroDto.TelefoneResidencial = this.dadosClienteCadastroDto.DddResidencial + this.dadosClienteCadastroDto.TelefoneResidencial;

      this.dadosClienteCadastroDto.Celular = this.dadosClienteCadastroDto.DddCelular + this.dadosClienteCadastroDto.Celular;

      this.dadosClienteCadastroDto.TelComercial = this.dadosClienteCadastroDto.DddComercial + this.dadosClienteCadastroDto.TelComercial;

      this.dadosClienteCadastroDto.TelComercial2 = this.dadosClienteCadastroDto.DddComercial2 + this.dadosClienteCadastroDto.TelComercial2;
    }

    //converter referências bancárias
    for (let i = 0; i < this.clienteCadastroDto.RefBancaria.length; i++) {
      let este = this.clienteCadastroDto.RefBancaria[i];
      este.Telefone = este.Ddd + este.Telefone;
    }

    //converter referências comerciais
    for (let i = 0; i < this.clienteCadastroDto.RefComercial.length; i++) {
      let este = this.clienteCadastroDto.RefComercial[i];
      este.Telefone = este.Ddd + este.Telefone;
    }

  }

  validarGeral(): string[] {
    let ret: string[] = new Array();

    if ((this.dadosClienteCadastroDto.Cnpj_Cpf.trim() === "") || (!CpfCnpjUtils.cnpj_cpf_ok(this.dadosClienteCadastroDto.Cnpj_Cpf))) {
      ret.push('CNPJ/CPF inválido!!');
    }

    if ((this.dadosClienteCadastroDto.Email !== "") && (!ValidacoesUtils.email_ok(this.dadosClienteCadastroDto.Email))) {
      ret.push('E-mail inválido!!');
    }

    if ((this.dadosClienteCadastroDto.EmailXml !== "") && (!ValidacoesUtils.email_ok(this.dadosClienteCadastroDto.EmailXml))) {
      ret.push('E-mail (XML) inválido!!');
    }
    return ret;
  }

  validarGeralPj(): string[] {
    let ret: string[] = new Array();

    let s = this.dadosClienteCadastroDto.Contato.trim();
    if (s === "") {
      ret.push('Informe o nome da pessoa para contato!!');
    }

    if (this.dadosClienteCadastroDto.Nome.trim() == "") {
      ret.push('Preencha o nome!!');
    }

    return ret;
  }

  validarGeralPf(): string[] {
    let ret: string[] = new Array();

    let s = this.dadosClienteCadastroDto.Sexo;
    if ((s == "") || (!ValidacoesUtils.sexo_ok(s))) {
      ret.push('Indique qual o sexo!!');
    }
    //nao validamos a data dessa forma, ela já é uma data no formulário: if (!isDate(f.dt_nasc)) {
    //e ela é opcional, então não validamos nada!
    return ret;
  }

  validarRefBancaria(ref: RefBancariaDtoCliente): string[] {
    let ret: string[] = new Array();

    if (ref.Banco.trim() === "") {
      ret.push('Informe o banco no cadastro de Referência Bancária!!');
    }
    if (ref.Agencia.trim() === "") {
      ret.push('Informe a agência no cadastro de Referência Bancária!!');
    }
    if (ref.Conta.trim() === "") {
      ret.push('Informe o número da conta no cadastro de Referência Bancária!!');
    }
    return ret;
  }

  validarRefComerial(ref: RefComercialDtoCliente): string[] {
    let ret: string[] = new Array();

    if (ref.Nome_Empresa.trim() == "") {
      ret.push('Informe o nome da empresa no cadastro de Referência Comercial!!');
    }

    return ret;
  }

  validarTelefones(): string[] {
    let ret: string[] = new Array();

    if (this.ehPf()) {
      if (!FormatarTelefone.ddd_ok(this.dadosClienteCadastroDto.DddResidencial)) {
        ret.push('DDD residencial inválido!!');
      }
      if (!FormatarTelefone.telefone_ok(this.dadosClienteCadastroDto.TelefoneResidencial)) {
        ret.push('Telefone residencial inválido!!');
      }
      if ((this.dadosClienteCadastroDto.DddResidencial.trim() !== "") || (this.dadosClienteCadastroDto.TelefoneResidencial.trim() != "")) {
        if (this.dadosClienteCadastroDto.DddResidencial.trim() === "") {
          ret.push('Preencha o DDD residencial!!');
        }
        if (this.dadosClienteCadastroDto.TelefoneResidencial.trim() === "") {
          ret.push('Preencha o telefone residencial!!');
        }
      }
    }

    if (!FormatarTelefone.ddd_ok(this.dadosClienteCadastroDto.DddComercial)) {
      ret.push('DDD comercial inválido!!');
    }

    if (!FormatarTelefone.telefone_ok(this.dadosClienteCadastroDto.TelComercial)) {
      ret.push('Telefone comercial inválido!!');
    }

    if ((this.dadosClienteCadastroDto.DddComercial.trim() !== "") || (this.dadosClienteCadastroDto.TelComercial.trim() !== "")) {
      if (this.dadosClienteCadastroDto.DddComercial.trim() === "") {
        ret.push('Preencha o DDD comercial!!');
      }
      if (this.dadosClienteCadastroDto.TelComercial.trim() === "") {
        ret.push('Preencha o telefone comercial!!');
      }
    }

    if (this.ehPf()) {
      if ((this.dadosClienteCadastroDto.TelefoneResidencial.trim() === "")
        && (this.dadosClienteCadastroDto.TelComercial.trim() === "")
        && (this.dadosClienteCadastroDto.Celular.trim() === "")) {
        ret.push('Preencha pelo menos um telefone!!');
      }
    }
    else {

      //PJ, telefone 1 é obrigatório
      if (this.dadosClienteCadastroDto.TelComercial.trim() === "") {
        ret.push('Preencha o telefone comercial!!');
      }


      if (this.dadosClienteCadastroDto.TelComercial2.trim() !== "") {
        if (this.dadosClienteCadastroDto.DddComercial2.trim() === "") {
          ret.push('Preencha o DDD comercial 2!!');
        }
        if (this.dadosClienteCadastroDto.TelComercial2.trim() === "") {
          ret.push('Preencha o telefone comercial 2!!');
        }
      }

    }
    if (this.ehPf()) {
      if (!FormatarTelefone.ddd_ok(this.dadosClienteCadastroDto.DddCelular)) {
        ret.push('DDD celular inválido!!');
      }
      if (!FormatarTelefone.telefone_ok(this.dadosClienteCadastroDto.Celular)) {
        ret.push('Telefone celular inválido!!');
      }
      if ((this.dadosClienteCadastroDto.DddCelular.trim() === "") && (this.dadosClienteCadastroDto.Celular.trim() !== "")) {
        ret.push('Preencha o DDD do celular.');
      }
      if ((this.dadosClienteCadastroDto.Celular.trim() === "") && (this.dadosClienteCadastroDto.DddCelular.trim() !== "")) {
        ret.push('Preencha o número do celular.');
      }
    }
    if (!this.ehPf()) {
      if (!FormatarTelefone.ddd_ok(this.dadosClienteCadastroDto.DddComercial2)) {
        ret.push('DDD comercial 2 inválido!!');
      }
      if (!FormatarTelefone.telefone_ok(this.dadosClienteCadastroDto.TelComercial2)) {
        ret.push('Telefone comercial 2 inválido!!');
      }
      if ((this.dadosClienteCadastroDto.DddComercial2.trim() === "") && (this.dadosClienteCadastroDto.TelComercial2.trim() !== "")) {
        ret.push('Preencha o DDD do telefone comercial 2.');
      }
      if ((this.dadosClienteCadastroDto.TelComercial2.trim() === "") && (this.dadosClienteCadastroDto.DddComercial2.trim() != "")) {
        ret.push('Preencha o telefone comercial 2.');
      }

    }

    return ret;
  }

  validarEndereco(): string[] {
    let ret: string[] = new Array();

    if (this.dadosClienteCadastroDto.Endereco.trim() === "") {
      ret.push('Preencha o endereço!!');
    }

    if (this.dadosClienteCadastroDto.Numero.trim() === "") {
      ret.push('Preencha o número do endereço!!');
    }

    if (this.dadosClienteCadastroDto.Bairro.trim() === "") {
      ret.push('Preencha o bairro!!');
    }

    if (this.dadosClienteCadastroDto.Cidade.trim() === "") {
      ret.push('Preencha a cidade!!');
    }

    let s = this.dadosClienteCadastroDto.Uf.trim();
    if ((s === "") || (!ValidacoesUtils.uf_ok(s))) {
      ret.push('UF inválida!!');
    }

    if (this.dadosClienteCadastroDto.Cep.toString().trim() === "") {
      ret.push('Informe o CEP!!');
    }

    if (!new FormatarEndereco().cep_ok(this.dadosClienteCadastroDto.Cep.toString())) {
      ret.push('CEP inválido!!');
    }

    return ret;
  }
}



