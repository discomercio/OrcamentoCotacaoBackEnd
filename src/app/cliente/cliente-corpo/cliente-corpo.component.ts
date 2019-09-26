import { Component, OnInit, Input, OnChanges, SimpleChanges, SimpleChange } from '@angular/core';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { Constantes } from 'src/app/dto/Constantes';
import { StringUtils } from 'src/app/utils/stringUtils';
import { Title } from '@angular/platform-browser';
import { CpfCnpjUtils } from 'src/app/utils/cpfCnpjUtils';
import { DataUtils } from 'src/app/utils/dataUtils';
import { ClienteCadastroUtils } from 'src/app/dto/AngularClienteCadastroUtils/ClienteCadastroUtils';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';
import { FormatarTelefone } from 'src/app/utils/formatarTelefone';
import { RefBancariaDtoCliente } from 'src/app/dto/ClienteCadastro/Referencias/RefBancariaDtoCliente';
import { ListaBancoDto } from 'src/app/dto/ClienteCadastro/ListaBancoDto';
import { BuscarClienteService } from 'src/app/servicos/cliente/buscar-cliente.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { RefComercialDtoCliente } from 'src/app/dto/ClienteCadastro/Referencias/RefComercialDtoCliente';

@Component({
  selector: 'app-cliente-corpo',
  templateUrl: './cliente-corpo.component.html',
  styleUrls: ['./cliente-corpo.component.scss', '../../estilos/endereco.scss', '../../estilos/telefone.scss']
})
export class ClienteCorpoComponent implements OnInit, OnChanges {
  constructor(private readonly titleService: Title,
    private readonly buscarClienteService: BuscarClienteService,
    private readonly alertaService: AlertaService) {
  }

  @Input() mostrarEndereco = true; //ao confrimar o cliente para um pre-pedido, não queremos mostrar o endereço aqui
  
  ngOnInit() {

    this.criarElementos();
    this.initEhPf();
    this.definirTitle();
    if (this.cadastrando) {
      this.buscarClienteService.listaBancosCombo().toPromise()
        .then((r) => {
          if (r === null) {
            //erro
            this.alertaService.mostrarErroInternet();
            return;
          }
          //cliente já existe
          this.listaBancosCombo = r;
        }).catch((r) => {
          //erro
          this.alertaService.mostrarErroInternet();
        });

    }
  }

  //o dado sendo editado
  @Input() dadosClienteCadastroDto = new DadosClienteCadastroDto();
  //é o mesmo dado, mas passamos separadamente para ficar mais fácil de construir esta tela
  @Input() clienteCadastroDto = new ClienteCadastroDto();

  @Input() cadastrando = false;

  //se pode editar os campos de ICMS, produtor rural e inscrição estadual
  @Input() editarIcms = true;


  criarElementos() {
    //cria os elementos vazios
    //este não podemos porque já tem o Cnpj_Cpf ao criar: this.dadosClienteCadastroDto = new DadosClienteCadastroDto();
    ClienteCadastroUtils.inicializarDadosClienteCadastroDto(this.dadosClienteCadastroDto);

    //isto não pode ser criado aqui dentro! //this.clienteCadastroDto = new ClienteCadastroDto();
    this.clienteCadastroDto.DadosCliente = this.dadosClienteCadastroDto;
    this.clienteCadastroDto.RefBancaria = new Array();
    this.clienteCadastroDto.RefComercial = new Array();
  }

  public constantes: Constantes = new Constantes();

  public DataUtilsFormatarTela = DataUtils.formatarTela;
  public clienteCadastroUtils = new ClienteCadastroUtils();
  public telefone_ddd_formata = FormatarTelefone.telefone_ddd_formata;
  //lista de bancos
  public listaBancosCombo: ListaBancoDto[];

  //para editar os telefones
  //editamos em outros campos porque usamos uma máscara
  //usando o angular2-text-mask
  public mascaraTelefone = FormatarTelefone.mascaraTelefone;


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
    if (!this.dadosClienteCadastroDto) {
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
    //nao usamos o title pq deveriamos setar em todas as páginas!
    /*
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
    */
  }

  //dados para exibir
  cnpj_cpf_formatado(): string {
    if (!this.dadosClienteCadastroDto || !this.dadosClienteCadastroDto.Cnpj_Cpf) {
      return "";
    }
    return CpfCnpjUtils.cnpj_cpf_formata(this.dadosClienteCadastroDto.Cnpj_Cpf);
  }



  //#region referencia coemrial e bancária
  adicionarRefBancaria() {
    let novo = new RefBancariaDtoCliente();
    novo.Banco = "";
    novo.BancoDescricao = "";
    novo.Agencia = "";
    novo.Conta = "";
    novo.Ddd = "";
    novo.Telefone = "";
    novo.Contato = "";

    this.clienteCadastroDto.RefBancaria.push(novo);
  }
  removerRefBancaria(indice: number) {
    this.clienteCadastroDto.RefBancaria.splice(indice, 1);
  }
  adicionarRefComercial() {
    let novo = new RefComercialDtoCliente();
    novo.Nome_Empresa = "";
    novo.Contato = "";
    novo.Ddd = "";
    novo.Telefone = "";

    this.clienteCadastroDto.RefComercial.push(novo);
  }
  removerRefComercial(indice: number) {
    this.clienteCadastroDto.RefComercial.splice(indice, 1);
  }
  //#endregion
}
