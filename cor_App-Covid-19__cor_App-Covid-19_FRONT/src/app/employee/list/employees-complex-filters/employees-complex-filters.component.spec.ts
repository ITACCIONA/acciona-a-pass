import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDividerModule } from '@angular/material/divider';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ConfigModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { EmployeesComplexFiltersComponent } from './employees-complex-filters.component';

describe('EmployeesComplexFiltersComponent', () => {
  let component: EmployeesComplexFiltersComponent;
  let fixture: ComponentFixture<EmployeesComplexFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EmployeesComplexFiltersComponent],
      imports: [
        BrowserAnimationsModule,
        TranslateModule,
        I18NextModule,
        MatDatepickerModule,
        MatCheckboxModule,
        MatListModule,
        MatInputModule,
        ReactiveFormsModule,
        MatDividerModule,
        MatNativeDateModule,
        MatSelectModule,
        HttpClientModule,
        ConfigModule
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeesComplexFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
