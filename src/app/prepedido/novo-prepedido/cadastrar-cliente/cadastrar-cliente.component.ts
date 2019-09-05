import { Component, OnInit, Input } from '@angular/core';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { ActivatedRoute } from '@angular/router';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';
import { ClienteCadastroDto } from 'src/app/dto/ClienteCadastro/ClienteCadastroDto';

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
  }

}



