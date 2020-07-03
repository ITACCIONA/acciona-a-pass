import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ConfigTestingModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from '../../ui/ui.module';
import { GeneralFormComponent } from '../general-form/general-form.component';
import { SendNotificationGeneralComponent } from './send-notification-general.component';

describe('SendNotificationGeneralComponent', () => {
  let component: SendNotificationGeneralComponent;
  let fixture: ComponentFixture<SendNotificationGeneralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SendNotificationGeneralComponent, GeneralFormComponent],
      imports: [
        UiModule,
        TranslateModule,
        I18NextModule,
        MatSelectModule,
        MatIconModule,
        ReactiveFormsModule,
        MatButtonToggleModule,
        HttpClientModule,
        ConfigTestingModule,
        MatDialogModule,
        MatSnackBarModule,
        MatFormFieldModule,
        MatInputModule,
        BrowserAnimationsModule,
        MatSlideToggleModule,
        MatAutocompleteModule
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SendNotificationGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
