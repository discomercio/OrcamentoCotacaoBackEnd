import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ErroModule } from './erro/erro.module';
import { UtilsRoutingModule } from './utils-routing.module';
import { MatDialog, MatDialogModule, MatButtonModule } from '@angular/material';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { AlertDialogComponent } from './alert-dialog/alert-dialog.component';


@NgModule({
  declarations: [ConfirmationDialogComponent, AlertDialogComponent],
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    UtilsRoutingModule,
    ErroModule
  ],
  entryComponents: [
    ConfirmationDialogComponent, AlertDialogComponent
  ]
})
export class UtilsModule { }
