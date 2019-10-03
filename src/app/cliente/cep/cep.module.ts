import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CepComponent } from './cep/cep.component';
import { CepDialogComponent } from './cep-dialog/cep-dialog.component';
import { MatFormFieldModule, MatInputModule, MatIconModule, MatButtonModule, MatDialogModule } from '@angular/material';
import { FormsModule } from '@angular/forms';
import { TextMaskModule } from 'angular2-text-mask';



@NgModule({
  declarations: [CepComponent, CepDialogComponent],
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    FormsModule,
    TextMaskModule,
    CommonModule
  ],
  exports: [CepComponent],
  entryComponents: [
    CepDialogComponent
  ]
})
export class CepModule { }
