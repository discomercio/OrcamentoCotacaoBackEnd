import { Component, OnInit, Input } from '@angular/core';
import { DadosClienteCadastroDto } from 'src/app/dto/ClienteCadastro/DadosClienteCadastroDto';
import { runInThisContext } from 'vm';
import { StringUtils } from 'src/app/utils/stringUtils';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { CpfCnpjUtils } from 'src/app/utils/cpfCnpjUtils';
import { Constantes } from 'src/app/dto/Constantes';
import { TelaDesktopBaseComponent } from 'src/app/servicos/telaDesktop/telaDesktopBaseComponent';
import { supportsPassiveEventListeners } from '@angular/cdk/platform';
import { TelaDesktopService } from 'src/app/servicos/telaDesktop/telaDesktop.service';

@Component({
  selector: 'app-cadastrar-cliente',
  templateUrl: './cadastrar-cliente.component.html',
  styleUrls: ['./cadastrar-cliente.component.scss']
})
export class CadastrarClienteComponent extends TelaDesktopBaseComponent implements OnInit {

  //o dado sendo editado
  dadosClienteCadastroDto = new DadosClienteCadastroDto();

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
  }

}



