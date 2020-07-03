import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { I18NextModule } from 'angular-i18next';
import { I18N_PROVIDERS } from './config';

@NgModule({
  declarations: [],
  imports: [CommonModule, I18NextModule.forRoot()],
  providers: [I18N_PROVIDERS],
  exports: []
})
export class TranslateModule {}
