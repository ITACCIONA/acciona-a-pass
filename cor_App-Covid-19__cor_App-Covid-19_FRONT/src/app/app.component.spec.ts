import { HttpClientModule } from '@angular/common/http';
import { async, TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { MsalModule, MsalService, MSAL_CONFIG, MSAL_CONFIG_ANGULAR } from '@azure/msal-angular';
import { ConfigTestingModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { AppComponent } from './app.component';
import { UiModule } from './ui/ui.module';

function MSALConfigFactory() {
  return {};
}

function MSALConfigAngularFactory() {
  return {};
}

describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        BrowserAnimationsModule,
        RouterTestingModule,
        ConfigTestingModule,
        TranslateModule,
        I18NextModule,
        MsalModule,
        UiModule,
        HttpClientModule,
        MatDialogModule
      ],
      providers: [
        MsalService,
        { provide: MSAL_CONFIG, useFactory: MSALConfigFactory },
        { provide: MSAL_CONFIG_ANGULAR, useFactory: MSALConfigAngularFactory }
      ],
      declarations: [AppComponent]
    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });
});
