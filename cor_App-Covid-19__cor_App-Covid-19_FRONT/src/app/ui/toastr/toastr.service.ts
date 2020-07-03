import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { I18NextPipe } from 'angular-i18next';

@Injectable({
  providedIn: 'root'
})
export class ToastrService {
  private duration = 2000;

  constructor(private snackBar: MatSnackBar, private i18n: I18NextPipe) {}

  success(msg: string) {
    this.snackBar.open(this.i18n.transform(msg), null, { duration: this.duration, panelClass: 'bg-success' });
  }

  error(msg: string) {
    this.snackBar.open(this.i18n.transform(msg), null, { duration: this.duration, panelClass: 'bg-error' });
  }
}
