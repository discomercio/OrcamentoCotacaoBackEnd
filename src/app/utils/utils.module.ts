import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ErroModule } from './erro/erro.module';
import { UtilsRoutingModule } from './utils-routing.module';
import { MatDialog, MatDialogModule, MatButtonModule } from '@angular/material';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';


@NgModule({
  declarations: [ConfirmationDialogComponent],
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    UtilsRoutingModule,
    ErroModule
  ],
  entryComponents: [
    ConfirmationDialogComponent
  ]
})
export class UtilsModule { }
