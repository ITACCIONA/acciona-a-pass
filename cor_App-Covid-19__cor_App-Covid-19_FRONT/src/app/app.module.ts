import { registerLocaleData } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import localeEs from '@angular/common/locales/es';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MsalInterceptor, MsalModule, MsalService, MSAL_CONFIG, MSAL_CONFIG_ANGULAR } from '@azure/msal-angular';
import { ConfigModule, ConfigService, I18NextInterceptor, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MSALConfigAngularFactory, MSALConfigFactory } from './auth';
import { NotificationsModule } from './notifications/notifications.module';
import { UiModule } from './ui/ui.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MsalModule,
    TranslateModule,
    I18NextModule,
    ConfigModule,
    UiModule,
    NotificationsModule,
    MatButtonModule
  ],
  providers: [
    MsalService,
    { provide: HTTP_INTERCEPTORS, useClass: I18NextInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: MsalInterceptor, multi: true },
    { provide: MSAL_CONFIG, useFactory: MSALConfigFactory, deps: [ConfigService] },
    { provide: MSAL_CONFIG_ANGULAR, useFactory: MSALConfigAngularFactory, deps: [ConfigService] }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor() {
    registerLocaleData(localeEs);
  }
}
