import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ConfigTestingModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from '../../../ui/ui.module';
import { FeverModalComponent } from './fever-modal.component';

describe('FeverModalComponent', () => {
  let component: FeverModalComponent;
  let fixture: ComponentFixture<FeverModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FeverModalComponent],
      imports: [
        UiModule,
        MatIconModule,
        TranslateModule,
        I18NextModule,
        MatDatepickerModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        ConfigTestingModule,
        HttpClientModule,
        MatSnackBarModule,
        MatNativeDateModule,
        MatInputModule,
        BrowserAnimationsModule
      ],
      providers: [{ provide: MAT_DIALOG_DATA, useValue: {} }]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FeverModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
