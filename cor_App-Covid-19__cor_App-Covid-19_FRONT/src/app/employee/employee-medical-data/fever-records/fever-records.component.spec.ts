import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ConfigTestingModule, PipesModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from '../../../ui/ui.module';
import { FeverRecordsComponent } from './fever-records.component';

describe('FeverRecordsComponent', () => {
  let component: FeverRecordsComponent;
  let fixture: ComponentFixture<FeverRecordsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FeverRecordsComponent],
      imports: [
        UiModule,
        TranslateModule,
        I18NextModule,
        MatDatepickerModule,
        PipesModule,
        MatIconModule,
        MatCardModule,
        MatListModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        HttpClientModule,
        ConfigTestingModule,
        MatSnackBarModule
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FeverRecordsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
