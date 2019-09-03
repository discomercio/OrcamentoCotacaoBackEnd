import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule, MatInputModule, MatButtonModule, MatCardModule, MatRadioModule, MatAutocompleteModule, MatCheckbox, MatCheckboxModule, MatTableModule, MatSnackBarModule, MatSelectModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { NovoPrepedidoComponent } from './novo-prepedido.component';
import { SelecionarClienteComponent } from './selecionar-cliente/selecionar-cliente.component';
import { ConfirmarClienteComponent } from './confirmar-cliente/confirmar-cliente.component';
import { NovoPrepedidoRoutingModule } from './novo-prepedido-routing.module';
import { CadastrarClienteComponent } from './cadastrar-cliente/cadastrar-cliente.component';
import { ClienteCorpoModule } from 'src/app/cliente/cliente-corpo/cliente-corpo.module';


@NgModule({
  declarations: [ NovoPrepedidoComponent, SelecionarClienteComponent, ConfirmarClienteComponent, CadastrarClienteComponent],
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
    ReactiveFormsModule,
    NovoPrepedidoRoutingModule
  ],
  exports: []
})
export class NovoPrepedidoModule { }
