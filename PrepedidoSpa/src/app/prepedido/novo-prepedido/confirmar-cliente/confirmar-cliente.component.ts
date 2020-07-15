import { Component, OnInit, ElementRef, ViewChild, AfterContentInit, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { BuscarClienteService } from 'src/app/servicos/cliente/buscar-cliente.service';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { Location } from '@angular/common';
import { MatDialog } from '@angular/material';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';
import { ClienteCadastroUtils } from 'src/app/utils/ClienteCadastroUtils';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { NovoPrepedidoDadosService } from '../novo-prepedido-dados.service';
import { ConfirmarEnderecoComponent } from '../confirmar-endereco/confirmar-endereco.component';
import { Constantes } from 'src/app/dto/Constantes';
import { ValidacoesClienteUtils } from 'src/app/utils/validacoesClienteUtils';
import { EnderecoCadastralClientePrepedidoDto } from 'src/app/dto/Prepedido/EnderecoCadastralClientePrepedidoDto';
import { ClienteCorpoComponent } from 'src/app/cliente/cliente-corpo/cliente-corpo.component';
import { FormatarTelefone } from 'src/app/utils/formatarTelefone';

@Component({
  selector: 'app-confirmar-cliente',
  templateUrl: './confirmar-cliente.component.html',
  styleUrls: ['./confirmar-cliente.component.scss']
})
export class ConfirmarClienteComponent extends TelaDesktopBaseComponent implements OnInit {

  //dados
  dadosClienteCadastroDto = new DadosClienteCadastroDto();
  clienteCadastroDto = new ClienteCadastroDto();
  enderecoEntregaDtoClienteCadastro = new EnderecoEntregaDtoClienteCadastro();
  public endCadastralClientePrepedidoDto = new EnderecoCadastralClientePrepedidoDto();

  constructor(private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    telaDesktopService: TelaDesktopService,
    private readonly location: Location,
    public readonly dialog: MatDialog,
    private readonly alertaService: AlertaService,
    private readonly novoPrepedidoDadosService: NovoPrepedidoDadosService,
    private readonly buscarClienteService: BuscarClienteService) {
    super(telaDesktopService);
  }

  ngOnInit() {
    this.dadosClienteCadastroDto = null;
    if (this.router.getCurrentNavigation()) {
      let clienteCadastroDto: ClienteCadastroDto = (this.router.getCurrentNavigation().extras.state) as ClienteCadastroDto;
      if (clienteCadastroDto && clienteCadastroDto.DadosCliente) {
        //estramente, precisamos fazer por timeout
        //é que, se for simplesmente setado, ele não "percebe" que foi carregado

        setTimeout(() => {
          this.dadosClienteCadastroDto = clienteCadastroDto.DadosCliente;

          this.verificarCriarNovoPrepedido();
        }, 0);
        return;
      }
    }
    //se chegar como null é pq foi salvo como link; não temos dados para mostrar
    if (!this.dadosClienteCadastroDto) {

      //voltamos para a tela anterior: router.navigate(["/novo-prepedido"]);

      //ou melhor, fazemos a busca de novo!
      const clienteBusca = this.activatedRoute.snapshot.params.cpfCnpj;

      this.buscarClienteService.buscar(clienteBusca).toPromise()
        .then((r) => {
          if (r === null) {

            //erro, voltamos para a tela anterior
            this.router.navigate(["/novo-prepedido"]);
            return;
          }
          //cliente já existe
          //quando para nesse ponto, já fomos direcionado para a tela "home"

          this.dadosClienteCadastroDto = r.DadosCliente;
          this.clienteCadastroDto = r;
          this.verificarCriarNovoPrepedido();
          this.salvarAtivoInicializar();
        }).catch((r) => {
          //erro, voltamos para a tela anterior
          this.router.navigate(["/novo-prepedido"]);
        });
    }
    else {
      //inicializamos
      this.salvarAtivoInicializar();
    }
    //inicializar as fases
    this.fase1 = true;
    //gabriel
    // this.fase2 = true;
    // this.fase1e2juntas = true;
    this.fase2 = false;
    this.fase1e2juntas = false;
    if (this.telaDesktop) {
      this.fase1 = true;
      this.fase2 = true;
      this.fase1e2juntas = true;
    }

    //para pegar o enter
    document.getElementById("idcontinuar").focus();
  }

  verificarCriarNovoPrepedido() {
    if (!!this.novoPrepedidoDadosService.prePedidoDto) {
      let existente = this.novoPrepedidoDadosService.prePedidoDto.DadosCliente.Id;
      if (existente == this.dadosClienteCadastroDto.Id) {
        //nao criamos! usamos o que já está no serviço
        this.dadosClienteCadastroDto = this.novoPrepedidoDadosService.prePedidoDto.DadosCliente;
        this.enderecoEntregaDtoClienteCadastro = this.novoPrepedidoDadosService.prePedidoDto.EnderecoEntrega;
        this.endCadastralClientePrepedidoDto = this.novoPrepedidoDadosService.prePedidoDto.EnderecoCadastroClientePrepedido;
        if (this.telaDesktop) {
          this.confirmarEndereco.atualizarDadosEnderecoTela(this.enderecoEntregaDtoClienteCadastro);
          //afazer chamar atualizarDadosEnderecoCadastral
          
          this.clienteCorpo.atualizarDadosEnderecoCadastralClienteTela(this.endCadastralClientePrepedidoDto);
        }
        return;
      }
    }

    ///vamos criar um novo
    this.novoPrepedidoDadosService.criarNovo(this.dadosClienteCadastroDto, this.enderecoEntregaDtoClienteCadastro,
      this.endCadastralClientePrepedidoDto);
    //quando a tela é para celular o "this.confirmarEndereco" esta "undefined" e já da problema
    //comentei para teste
    if (this.telaDesktop) {
      this.confirmarEndereco.atualizarDadosEnderecoTela(this.enderecoEntregaDtoClienteCadastro);
      //vamos atualizar o end cadastral
    }
  }

  //#region salvar alterações no IE e Contribuinte_Icms_Status
  //variáveis apra controlar salvarAtivo
  salvarAtivoInicializar() {
    this.dadosClienteCadastroDtoIe = this.dadosClienteCadastroDto.Ie;
    this.dadosClienteCadastroDtoProdutorRural = this.dadosClienteCadastroDto.ProdutorRural;
    this.dadosClienteCadastroDtoContribuinte_Icms_Status = this.dadosClienteCadastroDto.Contribuinte_Icms_Status;
  }
  private dadosClienteCadastroDtoIe: string;
  private dadosClienteCadastroDtoProdutorRural: number;
  private dadosClienteCadastroDtoContribuinte_Icms_Status: number;
  public constantes = new Constantes();
  salvarAtivo(): boolean {
    //diz se o botão de salvar está ligado
    if (!this.dadosClienteCadastroDto) {
      return false;
    }
    //se estiver com NULL é pq ainda não pegou os valores
    if (this.dadosClienteCadastroDtoIe == null) {
      return false;
    }
    if (this.dadosClienteCadastroDtoIe !== this.dadosClienteCadastroDto.Ie) {
      return true;
    }
    //vamos verificar se o cliente selecionou Produtor Rural, pois ele é obrigado a informar sim ou não
    /*
      Peguei um caso que o cliente não continha essa informação, e estava sendo permitido seguir na criação do prepedido,
      pois nesse momento estamos apenas verificando se o cliente teve alteração no cadastro dele, sendo assim, 
      iremos verificar se Produtor Rural é igual a "0"      
     */
    if (this.dadosClienteCadastroDtoProdutorRural !== this.dadosClienteCadastroDto.ProdutorRural ||
      this.dadosClienteCadastroDtoProdutorRural == 0) {
      return true;
    }
    if (this.dadosClienteCadastroDtoContribuinte_Icms_Status !== this.dadosClienteCadastroDto.Contribuinte_Icms_Status) {
      return true;
    }

    //vamos fazer essa validação aqui para garantir que o cliente esta 
    //sendo validado todas as vezes, mesmo que não tenha alterações
    // if ((this.dadosClienteCadastroDto.ProdutorRural === this.constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) &&
    //     (this.dadosClienteCadastroDto.Contribuinte_Icms_Status !== this.constantes.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM)) {
    //     return true;
    //   }

    return false;
  }

  //vamos salvar as alterações
  salvar(continuar: boolean): void {
    //as validações  
    let mensagem = new ClienteCadastroUtils().validarInscricaoestadualIcms(this.dadosClienteCadastroDto);
    if (mensagem && mensagem.trim() !== "") {
      this.mostrarMensagem(mensagem);
      return;
    }

    //estamos removendo os dados antes de salvar
    this.dadosClienteCadastroDto = new ClienteCadastroUtils().validarProdutorRural(this.dadosClienteCadastroDto);
    //tudo validado!
    this.buscarClienteService.atualizarCliente(this.dadosClienteCadastroDto).subscribe(
      {
        next: (r) => {
          //retorna uma lista de strings com erros
          if (r.length == 0) {
            this.salvarAtivoInicializar();
            if (continuar) {
              //salvamento automático? então já clicamos no continuar
              this.continuarEfetivo();
              return;
            }

            this.mostrarMensagem(`Dados salvos com sucesso.`);
            return;
          }

          //vamos mostrar os erros
          this.mostrarMensagem(`Ocorreu um erro ao salvar os dados. Mensagens de erro: ` + r.join(", "));
        },
        error: (r) => {
          this.alertaService.mostrarErroInternet(r);
        }
      }
    );
  }

  mostrarMensagem(msg: string): void {
    this.alertaService.mostrarMensagem(msg);
  }

  //#endregion

  //#region fase
  /*
  temos 2 fases: uma que confirma o cliente e a segunda que confirma o endereço de entrega
  na especificação original, é uma tela só no desktop e duas telas no celular
  talvez no desktop também fique em duas
  aqui controlamos a transição entre as telas
  */
  fase1 = true;
  fase2 = false;
  fase1e2juntas = false;
  //#endregion

  voltar() {
    //voltamos apra a fase anterior
    //fazer uma variavel para receber um valor para saber para onde voltar
    if (this.fase1e2juntas) {
      this.location.back();
      return;
    }
    if (this.fase1) {
      this.location.back();
      return;
    }

    //vltamos para a fase 1
    this.fase1 = true;
    this.fase2 = false;
    //    setTimeout(()=>this.ngOnInit(),1000);
  }
  //precisa do static: false porque está dentro de um ngif
  @ViewChild("confirmarEndereco", { static: false }) confirmarEndereco: ConfirmarEnderecoComponent;

  //esta como undefined
  @ViewChild("clienteCorpo", { static: false }) clienteCorpo: ClienteCorpoComponent;


  continuar(): void {
    //primeiro, vamos ver o CEP que está dentro do cliente
    //somente se o confirmarEndereco estiver atribuído. Se não estiver, é porque não estamos na tela em que precisamos testar ele
    if (this.confirmarEndereco && !this.confirmarEndereco.podeAvancar()) {
      this.alertaService.mostrarMensagem("Aguarde o carregamento do endereço antes de continuar.");
      return;
    }
    
    //afazer: não consigo pegar os dados do cep que foi alterado em dados cadastrais
    this.clienteCorpo.prepararAvancarEnderecoCadastralClientePrepedidoDto();

    //avisamos para o corpo do cliente que vamos avançar
    if (this.confirmarEndereco) {
      this.confirmarEndereco.prepararAvancar();
    }
    //salvamos automaticamente
    if (this.salvarAtivo()) {
      this.salvar(true);
      return;
    }

    this.continuarEfetivo();
  }
  continuarEfetivo(): void {
    //se estamos na fase 2, cotninua
    //caso contrário, volta para a fase 1

    if (this.fase2 || this.fase1e2juntas) {
      //vamos validar o endereço
      let validacoes: string[] = new Array();

      if (!this.endCadastralClientePrepedidoDto.Endereco_tipo_pessoa) {
        this.alertaService.mostrarMensagem("É necessário preencher os dados cadastrais!");
        return;
      }
      else {
        this.endCadastralClientePrepedidoDto = this.clienteCorpo.converterTelefones(this.endCadastralClientePrepedidoDto);
        validacoes = ValidacoesClienteUtils.validarEnderecoCadastralClientePrepedidoDto(this.endCadastralClientePrepedidoDto);

        if (this.enderecoEntregaDtoClienteCadastro.OutroEndereco) {
          this.enderecoEntregaDtoClienteCadastro = this.confirmarEndereco.converterTelefones(this.enderecoEntregaDtoClienteCadastro);
          validacoes = ValidacoesClienteUtils.validarEnderecoEntregaDtoClienteCadastro(this.enderecoEntregaDtoClienteCadastro,
            this.endCadastralClientePrepedidoDto.Endereco_tipo_pessoa);
        }
      }

      if (validacoes.length > 0) {
        this.alertaService.mostrarMensagem("Campos inválidos. Preencha os campos marcados como obrigatórios. \nLista de erros: \n" + validacoes.join("\n"));
        return;
      }
      //salvar no serviço
      //afazer: incluir a passagem de EnderecoCadastralClientePrepedidoDto para salavar no serviço
      this.novoPrepedidoDadosService.setarDTosParciais(this.dadosClienteCadastroDto, this.enderecoEntregaDtoClienteCadastro,
        this.endCadastralClientePrepedidoDto);

      //continuamos
      this.router.navigate(['novo-prepedido/itens']);
      return;
    }
    this.fase2 = true;
    this.fase1 = false;
  }  
}

