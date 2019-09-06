import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { EnderecoEntregaDtoClienteCadastro } from 'src/app/dto/ClienteCadastro/EnderecoEntregaDTOClienteCadastro';
import { ClienteCadastroUtils } from 'src/app/dto/AngularClienteCadastroUtils/ClienteCadastroUtils';
import { BuscarClienteService, JustificativaEndEntregaComboDto } from 'src/app/servicos/cliente/buscar-cliente.service';
import { EnderecoEntregaJustificativaDto } from 'src/app/dto/ClienteCadastro/EnderecoEntregaJustificativaDto';

@Component({
  selector: 'app-confirmar-endereco',
  templateUrl: './confirmar-endereco.component.html',
  styleUrls: [
    './confirmar-endereco.component.scss',
    '../../../estilos/endereco.scss'
  ]
})
export class ConfirmarEnderecoComponent implements OnInit {

  constructor(private readonly buscarClienteService: BuscarClienteService) { }

  buscarClienteServiceJustificativaEndEntregaComboTemporario: EnderecoEntregaJustificativaDto[];
  ngOnInit() {
    this.enderecoDiferente = false;
    this.buscarClienteServiceJustificativaEndEntregaComboTemporario = this.buscarClienteService.JustificativaEndEntregaComboTemporario();
  }

  //dados
  @Input() dadosClienteCadastroDto = new DadosClienteCadastroDto();
  @Input() mostrarEndereco = false; //no celular, como é uma tela separada,temos que mostrar o endereço já cadastrado
  @Input() enderecoEntregaDtoClienteCadastro = new EnderecoEntregaDtoClienteCadastro();
  @Output() enderecoDiferenteChange = new EventEmitter<boolean>();


  //utilitários
  public clienteCadastroUtils = new ClienteCadastroUtils();
  enderecoDiferente = false;

  atualizarEnderecoDiferente() {
    //ah, nao fazemos nada, está tudo por estilo....
    this.enderecoDiferenteChange.emit(this.enderecoDiferente);
  }

}
