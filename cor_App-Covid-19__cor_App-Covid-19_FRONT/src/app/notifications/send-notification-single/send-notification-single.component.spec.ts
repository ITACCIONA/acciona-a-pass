import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ConfigTestingModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from '../../ui/ui.module';
import { SendNotificationSingleComponent } from './send-notification-single.component';

describe('SendNotificationSingleComponent', () => {
  let component: SendNotificationSingleComponent;
  let fixture: ComponentFixture<SendNotificationSingleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SendNotificationSingleComponent],
      imports: [
        TranslateModule,
        I18NextModule,
        MatDialogModule,
        UiModule,
        ReactiveFormsModule,
        MatInputModule,
        MatSnackBarModule,
        HttpClientModule,
        ConfigTestingModule,
        BrowserAnimationsModule,
        MatIconModule
      ],
      providers: [{ provide: MAT_DIALOG_DATA, useValue: {} }]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SendNotificationSingleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
