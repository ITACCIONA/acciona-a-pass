import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ConfigTestingModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from 'src/app/ui/ui.module';
import { PassportWarningComponent } from '../employee-medical-data/passport-warning/passport-warning.component';
import { PassportComponent } from '../employee-medical-data/passport/passport.component';
import { PrlComponent } from './prl.component';

describe('PrlComponent', () => {
  let component: PrlComponent;
  let fixture: ComponentFixture<PrlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        MatCardModule,
        I18NextModule.forRoot(),
        MatIconModule,
        UiModule,
        MatDatepickerModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatDividerModule,
        HttpClientModule,
        ConfigTestingModule,
        MatSnackBarModule
      ],
      declarations: [PrlComponent, PassportWarningComponent, PassportComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
