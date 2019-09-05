import { Component, OnInit, Input, OnChanges, SimpleChanges, SimpleChange } from '@angular/core';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { Constantes } from 'src/app/dto/Constantes';
import { StringUtils } from 'src/app/utils/stringUtils';
import { Title } from '@angular/platform-browser';
import { CpfCnpjUtils } from 'src/app/utils/cpfCnpjUtils';
import { DataUtils } from 'src/app/utils/dataUtils';
import { ClienteCadastroUtils } from 'src/app/dto/ClienteCadastroUtils/ClienteCadastroUtils';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';

@Component({
  selector: 'app-cliente-corpo',
  templateUrl: './cliente-corpo.component.html',
  styleUrls: ['./cliente-corpo.component.scss']
})
export class ClienteCorpoComponent implements OnInit, OnChanges {
  constructor(private readonly titleService: Title) { }

  ngOnInit() {
    this.initEhPf();
    this.definirTitle();
  }

  //o dado sendo editado
  @Input() dadosClienteCadastroDto = new DadosClienteCadastroDto();
  //é o mesmo dado, mas passamos separadamente para ficar mais fácil de construir esta tela
  @Input() clienteCadastroDto = new ClienteCadastroDto();

  @Input() cadastrando = false;

  //se pode editar os campos de ICMS, produtor rural e inscrição estadual
  @Input() editarIcms = true;


  public constantes: Constantes = new Constantes();

  public DataUtilsFormatarTela = DataUtils.formatarTela;
  public clienteCadastroUtils = new ClienteCadastroUtils();

  //se estamos confirmando, o lable das caixa de texto sempre fica para cima
  //se estamos cadastrando, o comportamento padrão! aparece na caixa quando está vazia e sem o foco
  public floatLabel(): string {
    if (!this.cadastrando) {
      return "always";
    }
    return "auto";
  }


  //se estamos cadastrando PF ou PJ
  //usamos uma variável fixa pq não queremos mexer nos campos quando o usuário editar o campo de CPF/CNPJ
  ehPf() {
    return this.calculadoEhPf;
  }
  private initEhPf() {
    //testamos pelo tamanho do CPF/CNPJ
    if(!this.dadosClienteCadastroDto){
      return;
    }
    this.calculadoEhPf = false;
    if (this.dadosClienteCadastroDto.Cnpj_Cpf && StringUtils.retorna_so_digitos(this.dadosClienteCadastroDto.Cnpj_Cpf).length == 11) {
      this.calculadoEhPf = true;
    }
  }
  private calculadoEhPf = false;
  ngOnChanges(changes: SimpleChanges) {
    //precisamos disso porque ele inicializa com null
    this.initEhPf();
  }

  definirTitle() {
    if (this.ehPf() && this.cadastrando) {
      this.titleService.setTitle("Cadastro - pessoa física");
    }
    if (this.ehPf() && !this.cadastrando) {
      this.titleService.setTitle("Consulta - pessoa física");
    }
    if (!this.ehPf() && this.cadastrando) {
      this.titleService.setTitle("Cadastro - pessoa jurídica");
    }
    if (!this.ehPf() && !this.cadastrando) {
      this.titleService.setTitle("Consulta - pessoa jurídica");
    }
  }

  //dados para exibir
  cnpj_cpf_formatado(): string {
    if (!this.dadosClienteCadastroDto || !this.dadosClienteCadastroDto.Cnpj_Cpf) {
      return "";
    }
    return CpfCnpjUtils.cnpj_cpf_formata(this.dadosClienteCadastroDto.Cnpj_Cpf);
  }


}
