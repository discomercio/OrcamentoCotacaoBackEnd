import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClienteCorpoComponent } from './cliente-corpo.component';
import { MatButtonModule, MatIconModule, MatInputModule, MatAutocompleteModule, MatCardModule, MatRadioModule, MatCheckboxModule, MatTableModule, MatSnackBarModule, MatSelectModule, MatExpansionModule } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { TextMaskModule } from 'angular2-text-mask';


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
    MatExpansionModule,
    TextMaskModule,
    ReactiveFormsModule,
    MatSelectModule
  ],
  exports: [ClienteCorpoComponent]
})
export class ClienteCorpoModule { }
