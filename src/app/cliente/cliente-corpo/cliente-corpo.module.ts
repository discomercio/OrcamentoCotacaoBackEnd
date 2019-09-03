import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClienteCorpoComponent } from './cliente-corpo.component';
import { MatButtonModule, MatIconModule, MatInputModule, MatAutocompleteModule, MatCardModule, MatRadioModule, MatCheckboxModule, MatTableModule, MatSnackBarModule, MatSelectModule } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';


@NgModule({
  declarations: [ClienteCorpoComponent],
  imports: [
    CommonModule,
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
    ReactiveFormsModule,
    MatSelectModule
  ],
  exports: [ClienteCorpoComponent]
})
export class ClienteCorpoModule { }
