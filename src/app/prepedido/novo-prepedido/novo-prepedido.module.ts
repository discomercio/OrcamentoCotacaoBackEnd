import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule, MatInputModule, MatButtonModule, MatCardModule, MatRadioModule, MatAutocompleteModule, MatCheckbox, MatCheckboxModule, MatTableModule, MatSnackBarModule, MatSelectModule, MatStepperModule, MatDialogModule, MatPaginatorModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { NovoPrepedidoComponent } from './novo-prepedido.component';
import { SelecionarClienteComponent } from './selecionar-cliente/selecionar-cliente.component';
import { ConfirmarClienteComponent } from './confirmar-cliente/confirmar-cliente.component';
import { NovoPrepedidoRoutingModule } from './novo-prepedido-routing.module';
import { CadastrarClienteComponent } from './cadastrar-cliente/cadastrar-cliente.component';
import { ClienteCorpoModule } from 'src/app/cliente/cliente-corpo/cliente-corpo.module';
import { ConfirmarEnderecoComponent } from './confirmar-endereco/confirmar-endereco.component';
import { ItensComponent } from './itens/itens.component';
import { TextMaskModule } from 'angular2-text-mask';
import { SelecProdDialogComponent } from './selec-prod-dialog/selec-prod-dialog.component';
import { DadosPagtoComponent } from './dados-pagto/dados-pagto.component';
import { ConfirmarPrepedidoComponent } from './confirmar-prepedido/confirmar-prepedido.component';


@NgModule({
  declarations: [NovoPrepedidoComponent,
    SelecionarClienteComponent, ConfirmarClienteComponent, CadastrarClienteComponent,
    ConfirmarEnderecoComponent, ItensComponent,
    SelecProdDialogComponent,
    DadosPagtoComponent,
    ConfirmarPrepedidoComponent],
  imports: [
    CommonModule,
    ClienteCorpoModule,
    FormsModule,
    MatButtonModule,
    MatInputModule,
    MatAutocompleteModule,
    MatIconModule,
    FlexLayoutModule,
    MatCardModule,
    MatRadioModule,
    MatCheckboxModule,
    MatTableModule,
    MatSnackBarModule,
    MatSelectModule,
    MatStepperModule,
    MatDialogModule,
    MatPaginatorModule,
    ReactiveFormsModule,
    TextMaskModule,
    NovoPrepedidoRoutingModule
  ],
  exports: [],
  entryComponents: [
    SelecProdDialogComponent
  ]
})
export class NovoPrepedidoModule { }
